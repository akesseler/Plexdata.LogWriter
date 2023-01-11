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
using Plexdata.LogWriter.Definitions;
using Plexdata.LogWriter.Internals.Events;
using Plexdata.Utilities.Testing;
using System;
using System.Diagnostics.CodeAnalysis;

namespace Plexdata.LogWriter.Abstraction.Tests.Internals.Events
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    [Category(TestType.UnitTest)]
    [TestOf(nameof(LogEvent))]
    public class LogEventTests
    {
        #region LogEvent

        [Test]
        public void LogEvent_NoKey_DoesNotThrow()
        {
            Assert.That(() => new LogEvent(LogLevel.Default, DateTime.Now, null, null, null, null, null), Throws.Nothing);
        }

        [Test]
        public void LogEvent_EmptyKey_DoesNotThrow()
        {
            Assert.That(() => new LogEvent(Guid.Empty, LogLevel.Default, DateTime.Now, null, null, null, null, null), Throws.Nothing);
        }

        [Test]
        public void LogEvent_InvalidLevel_DoesNotThrow()
        {
            Assert.That(() => new LogEvent(Guid.Empty, (LogLevel)999, DateTime.Now, null, null, null, null, null), Throws.Nothing);
        }

        [Test]
        public void LogEvent_InvalidLevel_LevelIsDefault()
        {
            LogEvent instance = new LogEvent(Guid.Empty, (LogLevel)999, DateTime.Now, null, null, null, null, null);

            Assert.That(instance.Level, Is.EqualTo(LogLevel.Default));
        }

        [Test]
        public void LogEvent_NoMessageNoException_InstanceIsInvalid()
        {
            LogEvent instance = new LogEvent(Guid.Empty, LogLevel.Default, DateTime.Now, null, null, null, null, null);

            Assert.That(instance.IsValid, Is.False);
        }

        [Test]
        public void LogEvent_NoMessageNoException_TemplateAndMessageAsExpected()
        {
            LogEvent instance = new LogEvent(Guid.Empty, LogLevel.Default, DateTime.Now, null, null, null, null, null);

            Assert.That(instance.Template, Is.Empty);
            Assert.That(instance.Message, Is.Empty);
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase("  ")]
        public void LogEvent_NoMessageButException_TemplateAndMessageAsExpected(String message)
        {
            Exception exception = new Exception();

            LogEvent instance = new LogEvent(Guid.Empty, LogLevel.Default, DateTime.Now, null, null, message, exception, null);

            Assert.That(instance.Template, Is.Empty);
            Assert.That(instance.Message, Is.EqualTo(exception.Message));
        }

        [Test]
        public void LogEvent_MessageAndException_TemplateAndMessageAsExpected()
        {
            Exception exception = new Exception();

            LogEvent instance = new LogEvent(Guid.Empty, LogLevel.Default, DateTime.Now, null, null, "message", exception, null);

            Assert.That(instance.Template, Is.Empty);
            Assert.That(instance.Message, Is.EqualTo("message"));
        }

        [Test]
        public void LogEvent_MessageButNoException_TemplateAndMessageAsExpected()
        {
            LogEvent instance = new LogEvent(Guid.Empty, LogLevel.Default, DateTime.Now, null, null, "message", null, null);

            Assert.That(instance.Template, Is.Empty);
            Assert.That(instance.Message, Is.EqualTo("message"));
        }

        [TestCase(null, "")]
        [TestCase("", "")]
        [TestCase("  ", "")]
        [TestCase("context", "context")]
        [TestCase("  context  ", "context")]
        public void LogEvent_ProbeContext_ContextAsExpected(String context, String expected)
        {
            LogEvent instance = new LogEvent(Guid.Empty, LogLevel.Default, DateTime.Now, context, null, null, null, null);

            Assert.That(instance.Context, Is.EqualTo(expected));
        }

        [TestCase(null, "")]
        [TestCase("", "")]
        [TestCase("  ", "")]
        [TestCase("scope", "scope")]
        [TestCase("  scope  ", "scope")]
        public void LogEvent_ProbeScope_ScopeAsExpected(String scope, String expected)
        {
            LogEvent instance = new LogEvent(Guid.Empty, LogLevel.Default, DateTime.Now, null, scope, null, null, null);

            Assert.That(instance.Scope, Is.EqualTo(expected));
        }

        #endregion
    }
}
