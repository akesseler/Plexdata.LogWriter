/*
 * MIT License
 * 
 * Copyright (c) 2023 plexdata.de
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in all
 * copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
 * SOFTWARE.
 */

using NUnit.Framework;
using Plexdata.LogWriter.Definitions;
using Plexdata.LogWriter.Internals.Extensions;
using Plexdata.Utilities.Testing;
using System;
using System.Diagnostics.CodeAnalysis;

namespace Plexdata.LogWriter.Abstraction.Tests.Internals.Extensions
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    [Category(TestType.UnitTest)]
    [TestOf(nameof(LogLevelExtension))]
    public class LogLevelExtensionTests
    {
        [TestCase(-1)]
        [TestCase(42)]
        [TestCase(LogLevel.Disabled)]
        public void ToUnixSeverityLevel_LogLevelIsInvalid_ThrowsArgumentOutOfRangeException(LogLevel level)
        {
            Assert.That(() => level.ToUnixSeverityLevel(), Throws.InstanceOf<ArgumentOutOfRangeException>());
        }

        [TestCase(LogLevel.Trace, 7)]
        [TestCase(LogLevel.Debug, 7)]
        [TestCase(LogLevel.Verbose, 6)]
        [TestCase(LogLevel.Message, 5)]
        [TestCase(LogLevel.Warning, 4)]
        [TestCase(LogLevel.Error, 3)]
        [TestCase(LogLevel.Fatal, 2)]
        [TestCase(LogLevel.Critical, 1)]
        [TestCase(LogLevel.Disaster, 0)]
        public void ToUnixSeverityLevel_LogLevelAsProvided_ResultAsExpected(LogLevel level, Int32 expected)
        {
            Assert.That(level.ToUnixSeverityLevel(), Is.EqualTo(expected));
        }
    }
}
