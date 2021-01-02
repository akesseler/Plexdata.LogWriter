/*
 * MIT License
 * 
 * Copyright (c) 2021 plexdata.de
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
using Plexdata.LogWriter.Logging;
using System;
using System.Diagnostics.CodeAnalysis;

namespace Plexdata.LogWriter.Abstraction.Tests.Logging
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    [TestOf(nameof(EmptyLogger<Object>))]
    public class EmptyLoggerContextTests
    {
        #region IsDisabled

        [Test]
        public void IsDisabled_EmptyLoggerMustBeDisabled_ResultIsExpected()
        {
            Assert.That((new EmptyLogger<Object>()).IsDisabled, Is.True);
        }

        #endregion

        #region IsEnabled

        [TestCase(LogLevel.Disabled)]
        [TestCase(LogLevel.Trace)]
        [TestCase(LogLevel.Debug)]
        [TestCase(LogLevel.Verbose)]
        [TestCase(LogLevel.Message)]
        [TestCase(LogLevel.Warning)]
        [TestCase(LogLevel.Error)]
        [TestCase(LogLevel.Fatal)]
        [TestCase(LogLevel.Critical)]
        [TestCase(LogLevel.Default)]
        public void IsEnabled_EmptyLoggerMustBeDisabled_ResultIsExpected(LogLevel level)
        {
            Assert.That((new EmptyLogger<Object>()).IsEnabled(level), Is.False);
        }

        #endregion
    }
}
