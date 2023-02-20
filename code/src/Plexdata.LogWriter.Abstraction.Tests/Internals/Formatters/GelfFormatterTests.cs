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

using Moq;
using NUnit.Framework;
using Plexdata.LogWriter.Definitions;
using Plexdata.LogWriter.Internals.Facades;
using Plexdata.LogWriter.Internals.Formatters;
using Plexdata.Utilities.Testing;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Text;

namespace Plexdata.LogWriter.Abstraction.Tests.Internals.Formatters
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    [Category(TestType.UnitTest)]
    [TestOf(nameof(GelfFormatter))]
    public class GelfFormatterTests
    {
        #region Prologue

        private readonly StringBuilder builder = new StringBuilder();

        private Mock<ILogEvent> value = null;
        private Mock<ILoggerSettings> settings = null;
        private Mock<IResolverFacade> resolver = null;

        private GelfFormatter instance = null;
        private Guid testKey;
        private LogLevel testLogLevel;
        private DateTime testTime;
        private String testContext;
        private String testScope;
        private String testTemplate;
        private String testMessage;
        private Exception testException;
        private (String, Object)[] testDetails;

        private Boolean testShowKey;
        private Boolean testShowTime;

        [SetUp]
        public void Setup()
        {
            this.testKey = Guid.Parse("12345678-90AB-CDEF-1234-567890ABCDEF");
            this.testLogLevel = LogLevel.Default;
            this.testTime = DateTime.SpecifyKind(DateTime.Parse("2019-10-29T17:05:42.6789"), DateTimeKind.Utc);
            this.testContext = null;
            this.testScope = null;
            this.testTemplate = null;
            this.testMessage = "test-message-value";
            this.testException = null;
            this.testDetails = null;

            this.value = new Mock<ILogEvent>();
            this.value.SetupGet(x => x.Key).Returns(() => this.testKey);
            this.value.SetupGet(x => x.Level).Returns(() => this.testLogLevel);
            this.value.SetupGet(x => x.Time).Returns(() => this.testTime);
            this.value.SetupGet(x => x.Context).Returns(() => this.testContext);
            this.value.SetupGet(x => x.Scope).Returns(() => this.testScope);
            this.value.SetupGet(x => x.Template).Returns(() => this.testTemplate);
            this.value.SetupGet(x => x.Message).Returns(() => this.testMessage);
            this.value.SetupGet(x => x.Exception).Returns(() => this.testException);
            this.value.SetupGet(x => x.Details).Returns(() => this.testDetails);

            this.testShowKey = false;
            this.testShowTime = true;

            this.settings = new Mock<ILoggerSettings>();
            this.settings.SetupGet(x => x.LogType).Returns(LogType.Gelf);
            this.settings.SetupGet(x => x.ShowKey).Returns(() => this.testShowKey);
            this.settings.SetupGet(x => x.ShowTime).Returns(() => this.testShowTime);
            this.settings.SetupGet(x => x.Culture).Returns(new CultureInfo("en-US"));

            this.resolver = new Mock<IResolverFacade>();
            this.resolver.Setup(x => x.GetLocalHostName()).Returns("test-host-name");
            this.resolver.Setup(x => x.GetNewLine()).Returns("\r\n");

            this.instance = new GelfFormatter(this.settings.Object, this.resolver.Object);
        }

        #endregion

        #region Construction

        [TestCase(LogType.Csv)]
        [TestCase(LogType.Json)]
        [TestCase(LogType.Raw)]
        [TestCase(LogType.Xml)]
        public void GelfFormatter_WrongLogType_ThrowsNotSupportedException(LogType logType)
        {
            this.settings.SetupGet(x => x.LogType).Returns(logType);

            Assert.That(() => new GelfFormatter(this.settings.Object, this.resolver.Object), Throws.InstanceOf<NotSupportedException>());
        }

        [Test]
        public void GelfFormatter_IResolverFacadeIsNull_ThrowsArgumentNullException()
        {
            Assert.That(() => new GelfFormatter(this.settings.Object, null), Throws.ArgumentNullException);
        }

        #endregion

        #region Format argument validation

        [Test]
        public void Format_BuilderIsNull_ThrowsArgumentNullException()
        {
            Assert.That(() => this.instance.Format(null, this.value.Object), Throws.ArgumentNullException);
        }

        [Test]
        public void Format_ValueIsNull_ThrowsArgumentNullException()
        {
            Assert.That(() => this.instance.Format(new StringBuilder(), null), Throws.ArgumentNullException);
        }

        #endregion

        #region Overrides behaviour

        [TestCase(null, "\"null\"")]
        [TestCase("", "\"\"")]
        [TestCase("  ", "\"\"")]
        [TestCase("label", "\"label\"")]
        [TestCase("  label  ", "\"label\"")]
        public void ToLabel_LabelAsProvided_ResultAsExpected(String label, String expected)
        {
            TestGelfFormatter instance = new TestGelfFormatter(this.settings.Object, this.resolver.Object);

            Assert.That(instance.TestToLabel(label), Is.EqualTo(expected));
        }

        [Test]
        public void ToValue_ValueAndSplitValid_ThrowsNotSupportedException()
        {
            TestGelfFormatter instance = new TestGelfFormatter(this.settings.Object, this.resolver.Object);

            Assert.That(() => instance.TestToValue("value", '#'), Throws.InstanceOf<NotSupportedException>());
        }

        [Test]
        public void ToOutput_ValueAndSplitValid_ThrowsNotSupportedException()
        {
            TestGelfFormatter instance = new TestGelfFormatter(this.settings.Object, this.resolver.Object);

            Assert.That(() => instance.TestToOutput("value", '#'), Throws.InstanceOf<NotSupportedException>());
        }

        [Test]
        public void ToOutput_LabelAndValueAndSplitValid_ThrowsNotSupportedException()
        {
            TestGelfFormatter instance = new TestGelfFormatter(this.settings.Object, this.resolver.Object);

            Assert.That(() => instance.TestToOutput("label", "value", '#'), Throws.InstanceOf<NotSupportedException>());
        }

        #endregion

        #region Minimal message creation

        [TestCase(LogLevel.Trace, false, "{\"version\":\"1.1\",\"host\":\"test-host-name\",\"short_message\":\"test-message-value\",\"level\":7}")]
        [TestCase(LogLevel.Trace, true, "{\"version\":\"1.1\",\"host\":\"test-host-name\",\"short_message\":\"test-message-value\",\"timestamp\":1572368742.678,\"level\":7}")]
        [TestCase(LogLevel.Debug, false, "{\"version\":\"1.1\",\"host\":\"test-host-name\",\"short_message\":\"test-message-value\",\"level\":7}")]
        [TestCase(LogLevel.Debug, true, "{\"version\":\"1.1\",\"host\":\"test-host-name\",\"short_message\":\"test-message-value\",\"timestamp\":1572368742.678,\"level\":7}")]
        [TestCase(LogLevel.Verbose, false, "{\"version\":\"1.1\",\"host\":\"test-host-name\",\"short_message\":\"test-message-value\",\"level\":6}")]
        [TestCase(LogLevel.Verbose, true, "{\"version\":\"1.1\",\"host\":\"test-host-name\",\"short_message\":\"test-message-value\",\"timestamp\":1572368742.678,\"level\":6}")]
        [TestCase(LogLevel.Message, false, "{\"version\":\"1.1\",\"host\":\"test-host-name\",\"short_message\":\"test-message-value\",\"level\":5}")]
        [TestCase(LogLevel.Message, true, "{\"version\":\"1.1\",\"host\":\"test-host-name\",\"short_message\":\"test-message-value\",\"timestamp\":1572368742.678,\"level\":5}")]
        [TestCase(LogLevel.Warning, false, "{\"version\":\"1.1\",\"host\":\"test-host-name\",\"short_message\":\"test-message-value\",\"level\":4}")]
        [TestCase(LogLevel.Warning, true, "{\"version\":\"1.1\",\"host\":\"test-host-name\",\"short_message\":\"test-message-value\",\"timestamp\":1572368742.678,\"level\":4}")]
        [TestCase(LogLevel.Error, false, "{\"version\":\"1.1\",\"host\":\"test-host-name\",\"short_message\":\"test-message-value\",\"level\":3}")]
        [TestCase(LogLevel.Error, true, "{\"version\":\"1.1\",\"host\":\"test-host-name\",\"short_message\":\"test-message-value\",\"timestamp\":1572368742.678,\"level\":3}")]
        [TestCase(LogLevel.Fatal, false, "{\"version\":\"1.1\",\"host\":\"test-host-name\",\"short_message\":\"test-message-value\",\"level\":2}")]
        [TestCase(LogLevel.Fatal, true, "{\"version\":\"1.1\",\"host\":\"test-host-name\",\"short_message\":\"test-message-value\",\"timestamp\":1572368742.678,\"level\":2}")]
        [TestCase(LogLevel.Critical, false, "{\"version\":\"1.1\",\"host\":\"test-host-name\",\"short_message\":\"test-message-value\",\"level\":1}")]
        [TestCase(LogLevel.Critical, true, "{\"version\":\"1.1\",\"host\":\"test-host-name\",\"short_message\":\"test-message-value\",\"timestamp\":1572368742.678,\"level\":1}")]
        [TestCase(LogLevel.Disaster, false, "{\"version\":\"1.1\",\"host\":\"test-host-name\",\"short_message\":\"test-message-value\",\"level\":0}")]
        [TestCase(LogLevel.Disaster, true, "{\"version\":\"1.1\",\"host\":\"test-host-name\",\"short_message\":\"test-message-value\",\"timestamp\":1572368742.678,\"level\":0}")]
        public void Format_MinimalMessageCreationWithoutTemplate_ResultAsExpected(LogLevel logLevel, Boolean showTime, String expected)
        {
            this.testLogLevel = logLevel;
            this.testShowTime = showTime;

            this.instance.Format(this.builder, this.value.Object);

            String actual = this.builder.ToString();

            Assert.That(actual, Is.EqualTo(expected));
        }

        [TestCase(LogLevel.Trace, false, "{\"version\":\"1.1\",\"host\":\"test-host-name\",\"short_message\":\"test-template-value\",\"full_message\":\"test-message-value\",\"level\":7}")]
        [TestCase(LogLevel.Trace, true, "{\"version\":\"1.1\",\"host\":\"test-host-name\",\"short_message\":\"test-template-value\",\"full_message\":\"test-message-value\",\"timestamp\":1572368742.678,\"level\":7}")]
        [TestCase(LogLevel.Debug, false, "{\"version\":\"1.1\",\"host\":\"test-host-name\",\"short_message\":\"test-template-value\",\"full_message\":\"test-message-value\",\"level\":7}")]
        [TestCase(LogLevel.Debug, true, "{\"version\":\"1.1\",\"host\":\"test-host-name\",\"short_message\":\"test-template-value\",\"full_message\":\"test-message-value\",\"timestamp\":1572368742.678,\"level\":7}")]
        [TestCase(LogLevel.Verbose, false, "{\"version\":\"1.1\",\"host\":\"test-host-name\",\"short_message\":\"test-template-value\",\"full_message\":\"test-message-value\",\"level\":6}")]
        [TestCase(LogLevel.Verbose, true, "{\"version\":\"1.1\",\"host\":\"test-host-name\",\"short_message\":\"test-template-value\",\"full_message\":\"test-message-value\",\"timestamp\":1572368742.678,\"level\":6}")]
        [TestCase(LogLevel.Message, false, "{\"version\":\"1.1\",\"host\":\"test-host-name\",\"short_message\":\"test-template-value\",\"full_message\":\"test-message-value\",\"level\":5}")]
        [TestCase(LogLevel.Message, true, "{\"version\":\"1.1\",\"host\":\"test-host-name\",\"short_message\":\"test-template-value\",\"full_message\":\"test-message-value\",\"timestamp\":1572368742.678,\"level\":5}")]
        [TestCase(LogLevel.Warning, false, "{\"version\":\"1.1\",\"host\":\"test-host-name\",\"short_message\":\"test-template-value\",\"full_message\":\"test-message-value\",\"level\":4}")]
        [TestCase(LogLevel.Warning, true, "{\"version\":\"1.1\",\"host\":\"test-host-name\",\"short_message\":\"test-template-value\",\"full_message\":\"test-message-value\",\"timestamp\":1572368742.678,\"level\":4}")]
        [TestCase(LogLevel.Error, false, "{\"version\":\"1.1\",\"host\":\"test-host-name\",\"short_message\":\"test-template-value\",\"full_message\":\"test-message-value\",\"level\":3}")]
        [TestCase(LogLevel.Error, true, "{\"version\":\"1.1\",\"host\":\"test-host-name\",\"short_message\":\"test-template-value\",\"full_message\":\"test-message-value\",\"timestamp\":1572368742.678,\"level\":3}")]
        [TestCase(LogLevel.Fatal, false, "{\"version\":\"1.1\",\"host\":\"test-host-name\",\"short_message\":\"test-template-value\",\"full_message\":\"test-message-value\",\"level\":2}")]
        [TestCase(LogLevel.Fatal, true, "{\"version\":\"1.1\",\"host\":\"test-host-name\",\"short_message\":\"test-template-value\",\"full_message\":\"test-message-value\",\"timestamp\":1572368742.678,\"level\":2}")]
        [TestCase(LogLevel.Critical, false, "{\"version\":\"1.1\",\"host\":\"test-host-name\",\"short_message\":\"test-template-value\",\"full_message\":\"test-message-value\",\"level\":1}")]
        [TestCase(LogLevel.Critical, true, "{\"version\":\"1.1\",\"host\":\"test-host-name\",\"short_message\":\"test-template-value\",\"full_message\":\"test-message-value\",\"timestamp\":1572368742.678,\"level\":1}")]
        [TestCase(LogLevel.Disaster, false, "{\"version\":\"1.1\",\"host\":\"test-host-name\",\"short_message\":\"test-template-value\",\"full_message\":\"test-message-value\",\"level\":0}")]
        [TestCase(LogLevel.Disaster, true, "{\"version\":\"1.1\",\"host\":\"test-host-name\",\"short_message\":\"test-template-value\",\"full_message\":\"test-message-value\",\"timestamp\":1572368742.678,\"level\":0}")]
        public void Format_MinimalMessageCreationWithTemplate_ResultAsExpected(LogLevel logLevel, Boolean showTime, String expected)
        {
            this.testTemplate = "test-template-value";
            this.testLogLevel = logLevel;
            this.testShowTime = showTime;

            this.instance.Format(this.builder, this.value.Object);

            String actual = this.builder.ToString();

            Assert.That(actual, Is.EqualTo(expected));
        }

        #endregion

        #region Format tests

        [Test]
        public void Format_MessageWithKey_ResultContainsKeyAsExpected()
        {
            this.testShowKey = true;

            this.instance.Format(this.builder, this.value.Object);

            String actual = this.builder.ToString();

            Assert.That(actual, Contains.Substring("\"_Key\":\"12345678-90AB-CDEF-1234-567890ABCDEF\""));
        }

        [Test]
        public void Format_MessageWithContext_ResultContainsContextAsExpected()
        {
            this.testContext = "test-context";

            this.instance.Format(this.builder, this.value.Object);

            String actual = this.builder.ToString();

            Assert.That(actual, Contains.Substring("\"_Context\":\"test-context\""));
        }

        [Test]
        public void Format_MessageWithScope_ResultContainsScopeAsExpected()
        {
            this.testScope = "test-scope";

            this.instance.Format(this.builder, this.value.Object);

            String actual = this.builder.ToString();

            Assert.That(actual, Contains.Substring("\"_Scope\":\"test-scope\""));
        }

        [Test]
        public void Format_MessageWithException_ResultContainsExceptionAsExpected()
        {
            this.testException = this.GetException();

            this.instance.Format(this.builder, this.value.Object);

            String actual = this.builder.ToString();

            Assert.That(actual, Contains.Substring("\"_Exception\":\"string-of-test-dummy-exception-stack-trace\\\\ntest-dummy-exception-stack-trace\\\\nstack-trace-line-1\\\\nstack-trace-line-2\\\\n\""));
        }

        [Test]
        public void Format_MessageWithDetails_ResultContainsDetailsAsExpected()
        {
            this.testDetails = new (String, Object)[]
            {
                ("detail-label-1", "detail-value-1"),
                ("detail-label-2", true),
                ("detail-label-3", 42),
                ("detail-label-4", this.testTime)
            };

            this.instance.Format(this.builder, this.value.Object);

            String actual = this.builder.ToString();

            Assert.That(actual, Contains.Substring("\"_detail-label-1\":\"detail-value-1\""));
            Assert.That(actual, Contains.Substring("\"_detail-label-2\":\"true\""));
            Assert.That(actual, Contains.Substring("\"_detail-label-3\":42"));
            Assert.That(actual, Contains.Substring("\"_detail-label-4\":\"10/29/2019 5:05:42 PM\""));
        }

        [Test]
        public void Format_MessageDetailsWithInvalidLabels_ResultContainsDetailsAsExpected()
        {
            this.testDetails = new (String, Object)[]
            {
                ("id", "value-1"),
                ("Id", "value-2"),
                ("ID", "value-3"),
                ("_underscore", "value-4"),
                ("field.with.dot", "value-5"),
                ("field#with#hash", "value-6"),
                ("field@with@at", "value-7"),
                ("field with\tspace", "value-8"),
            };

            this.instance.Format(this.builder, this.value.Object);

            String actual = this.builder.ToString();

            Assert.That(actual, Contains.Substring("\"_id_field\":\"value-1\""));
            Assert.That(actual, Contains.Substring("\"_Id_field\":\"value-2\""));
            Assert.That(actual, Contains.Substring("\"_ID_field\":\"value-3\""));
            Assert.That(actual, Contains.Substring("\"__underscore\":\"value-4\""));
            Assert.That(actual, Contains.Substring("\"_field.with.dot\":\"value-5\""));
            Assert.That(actual, Contains.Substring("\"_field_with_hash\":\"value-6\""));
            Assert.That(actual, Contains.Substring("\"_field_with_at\":\"value-7\""));
            Assert.That(actual, Contains.Substring("\"_field_with_space\":\"value-8\""));
        }

        #endregion

        #region Private helper methods

        private class TestGelfFormatter : GelfFormatter
        {
            public TestGelfFormatter(ILoggerSettings settings, IResolverFacade resolver) : base(settings, resolver) { }

            public String TestToLabel(String label) { return base.ToLabel(label); }

            public String TestToValue(String value, Char split) { return base.ToValue(value, split); }

            public String TestToOutput(String value, Char split) { return base.ToOutput(value, split); }

            public String TestToOutput(String label, String value, Char split) { return base.ToOutput(label, value, split); }
        }

        private Exception GetException()
        {
            return new TestDummyException("test-exception", this.resolver.Object);
        }

        private class TestDummyException : Exception
        {
            private IResolverFacade resolver;

            public TestDummyException(String message, IResolverFacade resolver)
                : base(message)
            {
                this.resolver = resolver ?? throw new ArgumentNullException(nameof(resolver));
            }

            public override String StackTrace
            {
                get
                {
                    return $"test-dummy-exception-stack-trace{this.resolver.GetNewLine()}stack-trace-line-1{this.resolver.GetNewLine()}stack-trace-line-2{this.resolver.GetNewLine()}";
                }
            }

            public override String ToString()
            {
                return $"string-of-test-dummy-exception-stack-trace{this.resolver.GetNewLine()}{this.StackTrace}";
            }
        }

        #endregion
    }
}
