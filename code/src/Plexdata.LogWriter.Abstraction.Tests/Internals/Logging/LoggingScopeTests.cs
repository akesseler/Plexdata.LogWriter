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
using Plexdata.LogWriter.Internals.Logging;
using Plexdata.Utilities.Testing;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace Plexdata.LogWriter.Abstraction.Tests.Internals.Logging
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    [Category(TestType.UnitTest)]
    [TestOf(nameof(LoggingScope))]
    public class LoggingScopeTests
    {
        [Test]
        public void LoggingScope_DefaultConstruction_ThrowsNothing()
        {
            Assert.That(() => new LoggingScope(), Throws.Nothing);
        }

        [Test]
        public void LoggingScope_ExtendedConstructionValueIsNull_ThrowsNothing()
        {
            Assert.That(() => new LoggingScope(null), Throws.Nothing);
        }

        [Test]
        public void LoggingScope_ExtendedConstructionPropertiesCheck_PropertiesAsExpected()
        {
            LoggingScope instance = new LoggingScope("test-string");

            Assert.That(instance.Value, Is.EqualTo("test-string"));
            Assert.That(instance.IsDisposed, Is.False);
        }

        [Test]
        public void ToDisplay_ValueIsNull_ResultIsNull()
        {
            LoggingScope instance = new LoggingScope(null);

            Assert.That(instance.ToDisplay(), Is.Null);
        }

        [Test]
        public void ToDisplay_ValueIsString_ResultAsExpected()
        {
            LoggingScope instance = new LoggingScope("test-string");

            Assert.That(instance.ToDisplay(), Is.EqualTo("test-string"));
        }

        [Test]
        public void ToDisplay_ValueIsGuid_ResultAsExpected()
        {
            LoggingScope instance = new LoggingScope(Guid.Parse("11223344-5566-7788-9900-AABBCCDDEEFF"));

            Assert.That(instance.ToDisplay(), Is.EqualTo("11223344-5566-7788-9900-aabbccddeeff"));
        }

        [Test]
        public void ToDisplay_ValueIsMemberInfo_ResultAsExpected()
        {
            MemberInfo expected = MethodBase.GetCurrentMethod();

            LoggingScope instance = new LoggingScope(expected);

            Assert.That(instance.ToDisplay(), Is.EqualTo(expected.Name));
        }

        [Test]
        public void ToDisplay_ValueIsOther_ResultAsExpected()
        {
            LoggingScope instance = new LoggingScope(42);

            Assert.That(instance.ToDisplay(), Is.EqualTo("42"));
        }

        [Test]
        public void Dispose_SingleCall_PropertiesAsExpected()
        {
            LoggingScope instance = new LoggingScope("test-string");

            instance.Dispose();

            Assert.That(instance.Value, Is.Null);
            Assert.That(instance.IsDisposed, Is.True);
        }

        [Test]
        public void Dispose_SingleCall_EventCalledOnce()
        {
            Int32 execution = 0;

            LoggingScope instance = new LoggingScope("test-string");

            instance.Disposing += delegate (Object sender, EventArgs args) { execution++; };

            instance.Dispose();

            Assert.That(execution, Is.EqualTo(1));
        }

        [Test]
        public void Dispose_MultiCalls_PropertiesAsExpected()
        {
            LoggingScope instance = new LoggingScope("test-string");

            instance.Dispose();
            instance.Dispose();
            instance.Dispose();

            Assert.That(instance.Value, Is.Null);
            Assert.That(instance.IsDisposed, Is.True);
        }

        [Test]
        public void Dispose_MultiCall_EventCalledOnce()
        {
            Int32 execution = 0;

            LoggingScope instance = new LoggingScope("test-string");

            instance.Disposing += delegate (Object sender, EventArgs args) { execution++; };

            instance.Dispose();
            instance.Dispose();
            instance.Dispose();

            Assert.That(execution, Is.EqualTo(1));
        }

        [Test]
        public void Dispose_DisposingEventThrows_ThrowsNothing()
        {
            LoggingScope instance = new LoggingScope("test-string");

            instance.Disposing += delegate (Object sender, EventArgs args) { throw new Exception(); };

            Assert.That(() => instance.Dispose(), Throws.Nothing);
        }
    }
}
