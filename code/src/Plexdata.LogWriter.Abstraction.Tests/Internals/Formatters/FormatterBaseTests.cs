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

using Moq;
using NUnit.Framework;
using Plexdata.LogWriter.Definitions;
using Plexdata.LogWriter.Internals.Formatters;
using Plexdata.LogWriter.Settings;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Plexdata.LogWriter.Abstraction.Tests.Internals.Formatters
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    [TestOf(nameof(FormatterBase))]
    public class FormatterBaseTests
    {
        #region Prologue

        private Mock<ILoggerSettings> settings = null;
        private TestFormatter instance = null;

        [SetUp]
        public void Setup()
        {
            this.settings = new Mock<ILoggerSettings>();
            this.instance = new TestFormatter(this.settings.Object);
        }

        #endregion

        #region Construction

        [Test]
        public void FormatterBase_SettingsAreNull_ThrowsArgumentNullException()
        {
            Assert.That(() => new TestFormatter(null), Throws.ArgumentNullException);
        }

        #endregion

        #region GetKey

        [TestCase("12345678-90ab-cdef-1234-567890abcdef", "12345678-90AB-CDEF-1234-567890ABCDEF")]
        [TestCase("12345678-90AB-CDEF-1234-567890ABCDEF", "12345678-90AB-CDEF-1234-567890ABCDEF")]
        public void GetKey_WithValidKey_ResultIsExpected(String actual, String expected)
        {
            Assert.That(this.instance.TestGetKey(Guid.Parse(actual)), Is.EqualTo(expected));
        }

        #endregion

        #region GetLevel

        [TestCase(LogLevel.Disabled)]
        [TestCase((LogLevel)42)]
        public void GetLevel_LogLevelIsNotSupported_ThrowsNotSupportedException(LogLevel level)
        {
            Assert.That(() => this.instance.TestGetLevel(level), Throws.InstanceOf<NotSupportedException>());
        }

        [TestCase("1", LogLevel.Trace, "TRACE")]
        [TestCase("2", LogLevel.Debug, "DEBUG")]
        [TestCase("3", LogLevel.Verbose, "VERBOSE")]
        [TestCase("4", LogLevel.Message, "MESSAGE")]
        [TestCase("5", LogLevel.Warning, "WARNING")]
        [TestCase("6", LogLevel.Error, "ERROR")]
        [TestCase("7", LogLevel.Fatal, "FATAL")]
        [TestCase("8", LogLevel.Critical, "CRITICAL")]
        [TestCase("9", LogLevel.Default, "MESSAGE")]
        public void GetLevel_LogLevelIsSupported_ResultIsExpected(String label, LogLevel level, String expected)
        {
            Assert.That(this.instance.TestGetLevel(level), Is.EqualTo(expected));
        }

        #endregion

        #region GetTime

        [TestCase("2019-10-29T17:05:42.6789", LogTime.Utc, DateTimeKind.Utc, null)]
        [TestCase("2019-10-29T17:05:42.6789", LogTime.Utc, DateTimeKind.Utc, "")]
        [TestCase("2019-10-29T17:05:42.6789", LogTime.Utc, DateTimeKind.Utc, " ")]
        [TestCase("2019-10-29T17:05:42.6789", LogTime.Utc, DateTimeKind.Utc, "\t")]
        [TestCase("2019-10-29T17:05:42.6789", LogTime.Local, DateTimeKind.Utc, null)]
        [TestCase("2019-10-29T17:05:42.6789", LogTime.Local, DateTimeKind.Utc, "")]
        [TestCase("2019-10-29T17:05:42.6789", LogTime.Local, DateTimeKind.Utc, " ")]
        [TestCase("2019-10-29T17:05:42.6789", LogTime.Local, DateTimeKind.Utc, "\t")]
        [TestCase("2019-10-29T17:05:42.6789", LogTime.Utc, DateTimeKind.Local, null)]
        [TestCase("2019-10-29T17:05:42.6789", LogTime.Utc, DateTimeKind.Local, "")]
        [TestCase("2019-10-29T17:05:42.6789", LogTime.Utc, DateTimeKind.Local, " ")]
        [TestCase("2019-10-29T17:05:42.6789", LogTime.Utc, DateTimeKind.Local, "\t")]
        [TestCase("2019-10-29T17:05:42.6789", LogTime.Local, DateTimeKind.Local, null)]
        [TestCase("2019-10-29T17:05:42.6789", LogTime.Local, DateTimeKind.Local, "")]
        [TestCase("2019-10-29T17:05:42.6789", LogTime.Local, DateTimeKind.Local, " ")]
        [TestCase("2019-10-29T17:05:42.6789", LogTime.Local, DateTimeKind.Local, "\t")]
        public void GetTime_TimeFormatIsInvalid_ResultIsDefaultTimeFormat(String value, LogTime type, DateTimeKind kind, String format)
        {
            DateTime expected = DateTime.SpecifyKind(DateTime.Parse(value), (type == LogTime.Utc ? DateTimeKind.Utc : DateTimeKind.Local));
            if (expected.Kind == DateTimeKind.Local) { expected = expected.Add(TimeZoneInfo.Local.GetUtcOffset(expected)); }

            this.settings.SetupGet(x => x.LogTime).Returns(type);
            this.settings.SetupGet(x => x.TimeFormat).Returns(format);

            DateTime actual = DateTime.SpecifyKind(DateTime.Parse(value), kind);
            if (actual.Kind == DateTimeKind.Local) { actual = actual.Add(TimeZoneInfo.Local.GetUtcOffset(actual)); }

            Assert.That(this.instance.TestGetTime(actual), Is.EqualTo(expected.ToString(LoggerSettings.DefaultTimeFormat)));
        }

        [TestCase("2019-10-29T17:05:42.6789", LogTime.Utc, DateTimeKind.Utc, "s")]
        [TestCase("2019-10-29T17:05:42.6789", LogTime.Local, DateTimeKind.Utc, "s")]
        [TestCase("2019-10-29T17:05:42.6789", LogTime.Utc, DateTimeKind.Local, "s")]
        [TestCase("2019-10-29T17:05:42.6789", LogTime.Local, DateTimeKind.Local, "s")]
        [TestCase("2019-10-29T17:05:42.6789", LogTime.Utc, DateTimeKind.Utc, "Wow d-M-yy H.mm.ss")]
        [TestCase("2019-10-29T17:05:42.6789", LogTime.Local, DateTimeKind.Utc, "Wow d-M-yy H.mm.ss")]
        [TestCase("2019-10-29T17:05:42.6789", LogTime.Utc, DateTimeKind.Local, "Wow d-M-yy H.mm.ss")]
        [TestCase("2019-10-29T17:05:42.6789", LogTime.Local, DateTimeKind.Local, "Wow d-M-yy H.mm.ss")]
        public void GetTime_TimeFormatIsValid_ResultIsCustomTimeFormat(String value, LogTime type, DateTimeKind kind, String format)
        {
            DateTime expected = DateTime.SpecifyKind(DateTime.Parse(value), (type == LogTime.Utc ? DateTimeKind.Utc : DateTimeKind.Local));
            if (expected.Kind == DateTimeKind.Local) { expected = expected.Add(TimeZoneInfo.Local.GetUtcOffset(expected)); }

            this.settings.SetupGet(x => x.LogTime).Returns(type);
            this.settings.SetupGet(x => x.TimeFormat).Returns(format);

            DateTime actual = DateTime.SpecifyKind(DateTime.Parse(value), kind);
            if (actual.Kind == DateTimeKind.Local) { actual = actual.Add(TimeZoneInfo.Local.GetUtcOffset(actual)); }

            Assert.That(this.instance.TestGetTime(actual), Is.EqualTo(expected.ToString(format)));
        }

        #endregion

        #region GetContext

        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        public void GetContext_ContextIsInvalid_ResultIsDefaultValue(String context)
        {
            Assert.That(this.instance.TestGetContext(context), Is.EqualTo("null"));
        }

        [TestCase(null, "my default result")]
        [TestCase("", "my default result")]
        [TestCase(" ", "my default result")]
        [TestCase("\r\n", "my default result")]
        [TestCase(" \r\n", "my default result")]
        [TestCase(" \r\n ", "my default result")]
        public void GetContext_ContextIsInvalid_ResultIsStandardValue(String context, String standard)
        {
            Assert.That(this.instance.TestGetContext(context, standard), Is.EqualTo(standard));
        }

        [TestCase("my result", "my result")]
        [TestCase(" my result ", "my result")]
        [TestCase("my result\r\n", "my result")]
        [TestCase(" my result \r\n", "my result")]
        [TestCase(" my result \r\n ", "my result")]
        public void GetContext_ContextIsValid_ResultIsExpected(String context, String expected)
        {
            Assert.That(this.instance.TestGetContext(context), Is.EqualTo(expected));
        }

        #endregion

        #region GetScope

        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        public void GetScope_ScopeIsInvalid_ResultIsDefaultValue(String scope)
        {
            Assert.That(this.instance.TestGetScope(scope), Is.EqualTo("null"));
        }

        [TestCase(null, "my default result")]
        [TestCase("", "my default result")]
        [TestCase(" ", "my default result")]
        [TestCase("\r\n", "my default result")]
        [TestCase(" \r\n", "my default result")]
        [TestCase(" \r\n ", "my default result")]
        public void GetScope_ScopeIsInvalid_ResultIsStandardValue(String scope, String standard)
        {
            Assert.That(this.instance.TestGetScope(scope, standard), Is.EqualTo(standard));
        }

        [TestCase("my result", "my result")]
        [TestCase(" my result ", "my result")]
        [TestCase("my result\r\n", "my result")]
        [TestCase(" my result \r\n", "my result")]
        [TestCase(" my result \r\n ", "my result")]
        public void GetScope_ScopeIsValid_ResultIsExpected(String scope, String expected)
        {
            Assert.That(this.instance.TestGetScope(scope), Is.EqualTo(expected));
        }

        #endregion

        #region GetMessage

        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        public void GetMessage_MessageIsInvalid_ResultIsDefaultValue(String message)
        {
            Assert.That(this.instance.TestGetMessage(message), Is.EqualTo("null"));
        }

        [TestCase(null, "my default result")]
        [TestCase("", "my default result")]
        [TestCase(" ", "my default result")]
        [TestCase("\r\n", "my default result")]
        [TestCase(" \r\n", "my default result")]
        [TestCase(" \r\n ", "my default result")]
        public void GetMessage_MessageIsInvalid_ResultIsStandardValue(String message, String standard)
        {
            Assert.That(this.instance.TestGetMessage(message, standard), Is.EqualTo(standard));
        }

        [TestCase("my result", "my result")]
        [TestCase(" my result ", "my result")]
        [TestCase("my result\r\n", "my result")]
        [TestCase(" my result \r\n", "my result")]
        [TestCase(" my result \r\n ", "my result")]
        public void GetMessage_MessageIsValid_ResultIsExpected(String message, String expected)
        {
            Assert.That(this.instance.TestGetMessage(message), Is.EqualTo(expected));
        }

        #endregion

        #region GetException

        [Test]
        public void GetException_ExceptionIsInvalid_ResultIsDefaultValue()
        {
            Assert.That(this.instance.TestGetException(null), Is.EqualTo("null"));
        }

        [Test]
        public void GetException_ExceptionIsInvalid_ResultIsStandardValue()
        {
            String standard = "my default result";

            Assert.That(this.instance.TestGetException(null, standard), Is.EqualTo(standard));
        }

        [Test]
        public void GetException_ExceptionIsValid_ResultIsExpected()
        {
            String actual = "Well an exception.\r\n   Stack trace simulation with C:\\path\\folder\\file.name:Line 42";
            String expected = "System.Exception: Well an exception.\r\n   Stack trace simulation with C:\\path\\folder\\file.name:Line 42";

            Assert.That(this.instance.TestGetException(new Exception(actual)), Is.EqualTo(expected));
        }

        #endregion

        #region TrimEnd

        [Test]
        public void TrimEnd_BuilderIsNull_DoesNotThrow()
        {
            Assert.That(() => this.instance.TestTrimEnd(null, '#'), Throws.Nothing);
        }

        [TestCase("", '#', 0)]
        [TestCase("#", '#', 0)]
        [TestCase("my builder value", '#', 16)]
        [TestCase("my builder value#", '#', 16)]
        [TestCase("my builder value# ", '#', 18)]
        public void TrimEnd_BuilderWithStringValue_ResultLengthAsExpected(String value, Char item, Int32 expected)
        {
            StringBuilder builder = new StringBuilder(value);

            this.instance.TestTrimEnd(builder, item);

            Assert.That(builder.Length, Is.EqualTo(expected));
        }

        #endregion

        #region Private test class implementations

        private class TestFormatter : FormatterBase
        {
            public TestFormatter(ILoggerSettings settings)
                : base(settings)
            {
            }

            protected override String ToLabel(String label)
            {
                throw new NotImplementedException();
            }

            protected override String ToOutput(String value, Char split)
            {
                throw new NotImplementedException();
            }

            protected override String ToOutput(String label, String value, Char split)
            {
                throw new NotImplementedException();
            }

            protected override String ToValue(String value, Char split)
            {
                throw new NotImplementedException();
            }

            public String TestGetKey(Guid key)
            {
                return base.GetKey(key);
            }

            public String TestGetLevel(LogLevel level)
            {
                return base.GetLevel(level);
            }

            public String TestGetTime(DateTime time)
            {
                return base.GetTime(time);
            }

            public String TestGetContext(String context)
            {
                return base.GetContext(context);
            }

            public String TestGetContext(String context, String standard)
            {
                return base.GetContext(context, standard);
            }

            public String TestGetScope(String scope)
            {
                return base.GetScope(scope);
            }

            public String TestGetScope(String scope, String standard)
            {
                return base.GetScope(scope, standard);
            }

            public String TestGetMessage(String message)
            {
                return base.GetMessage(message);
            }

            public String TestGetMessage(String message, String standard)
            {
                return base.GetMessage(message, standard);
            }

            public String TestGetException(Exception exception)
            {
                return base.GetException(exception);
            }

            public String TestGetException(Exception exception, String standard)
            {
                return base.GetException(exception, standard);
            }

            public void TestTrimEnd(StringBuilder builder, Char value)
            {
                base.TrimEnd(builder, value);
            }
        }

        #endregion
    }
}
