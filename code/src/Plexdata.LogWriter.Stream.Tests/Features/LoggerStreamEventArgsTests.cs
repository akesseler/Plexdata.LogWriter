/*
 * MIT License
 * 
 * Copyright (c) 2022 plexdata.de
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
using Plexdata.LogWriter.Features;
using Plexdata.Utilities.Testing;
using System;
using System.Diagnostics.CodeAnalysis;

namespace Plexdata.LogWriter.Stream.Tests.Features
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    [Category(TestType.UnitTest)]
    [TestOf(nameof(LoggerStreamEventArgs))]
    public class LoggerStreamEventArgsTests
    {
        [Test]
        public void LoggerStreamEventArgs_MessageListIsNull_ThrowsArgumentNullException()
        {
            Assert.That(() => new LoggerStreamEventArgs(null), Throws.ArgumentNullException);
        }

        [Test]
        public void LoggerStreamEventArgs_MessageListIsEmpty_ThrowsArgumentOutOfRangeException()
        {
            Assert.That(() => new LoggerStreamEventArgs(new String[0]), Throws.InstanceOf<ArgumentOutOfRangeException>());
        }

        [Test]
        public void LoggerStreamEventArgs_MessageListIsValid_MessagesAppliedAsExpected()
        {
            String[] messages = new String[] { "message-1", "message-2", "message-3", "message-4" };

            LoggerStreamEventArgs instance = new LoggerStreamEventArgs(messages);

            Assert.That(String.Join(String.Empty, instance.Messages), Is.EqualTo(String.Join(String.Empty, messages)));
        }
    }
}
