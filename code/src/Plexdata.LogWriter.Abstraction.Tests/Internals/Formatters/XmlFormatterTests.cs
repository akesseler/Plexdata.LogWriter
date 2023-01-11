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

using Moq;
using NUnit.Framework;
using Plexdata.LogWriter.Definitions;
using Plexdata.LogWriter.Internals.Formatters;
using Plexdata.Utilities.Testing;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Text;

namespace Plexdata.LogWriter.Abstraction.Tests.Internals.Formatters
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    [Category(TestType.UnitTest)]
    [TestOf(nameof(XmlFormatter))]
    public class XmlFormatterTests
    {
        #region Prologue

        private readonly StringBuilder builder = new StringBuilder(); // => Implicit test of clearing the builder.

        private Mock<ILogEvent> value = null;
        private Mock<ILoggerSettings> settings = null;
        private XmlFormatter instance = null;

        [SetUp]
        public void Setup()
        {
            this.value = new Mock<ILogEvent>();
            this.value.SetupGet(x => x.Key).Returns(Guid.Parse("12345678-90AB-CDEF-1234-567890ABCDEF"));
            this.value.SetupGet(x => x.Level).Returns(LogLevel.Default);
            this.value.SetupGet(x => x.Time).Returns(DateTime.SpecifyKind(DateTime.Parse("2019-10-29T17:05:42.6789"), DateTimeKind.Utc));
            this.value.SetupGet(x => x.Context).Returns((String)null);
            this.value.SetupGet(x => x.Scope).Returns((String)null);
            this.value.SetupGet(x => x.Message).Returns((String)null);
            this.value.SetupGet(x => x.Exception).Returns((Exception)null);
            this.value.SetupGet(x => x.Details).Returns((ValueTuple<String, Object>[])null);

            this.settings = new Mock<ILoggerSettings>();

            this.settings.SetupGet(x => x.LogLevel).Returns(LogLevel.Default);
            this.settings.SetupGet(x => x.LogType).Returns(LogType.Xml);
            this.settings.SetupGet(x => x.LogTime).Returns(LogTime.Utc);
            this.settings.SetupGet(x => x.PartSplit).Returns('@');
            this.settings.SetupGet(x => x.ShowKey).Returns(true);
            this.settings.SetupGet(x => x.ShowTime).Returns(true);
            this.settings.SetupGet(x => x.TimeFormat).Returns("yyyyMMddHHmmss");
            this.settings.SetupGet(x => x.FullName).Returns(false);
            this.settings.SetupGet(x => x.Culture).Returns(new CultureInfo("en-US"));

            this.instance = new XmlFormatter(this.settings.Object);
        }

        #endregion

        #region Construction

        [TestCase(LogType.Csv)]
        [TestCase(LogType.Json)]
        [TestCase(LogType.Raw)]
        [TestCase(LogType.Gelf)]
        public void XmlFormatter_WrongLogType_ThrowsNotSupportedException(LogType logType)
        {
            this.settings.SetupGet(x => x.LogType).Returns(logType);

            Assert.That(() => new XmlFormatter(this.settings.Object), Throws.InstanceOf<NotSupportedException>());
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

        #region Format tests

        [TestCase(false, "<?xml version=\"1.0\" encoding=\"UTF-8\"?><logging><notification><key /><time>20191029170542</time><level>MESSAGE</level><context /><scope /><message>my message</message><details /><exception /></notification></logging>")]
        [TestCase(true, "<?xml version=\"1.0\" encoding=\"UTF-8\"?><logging><notification><key>12345678-90AB-CDEF-1234-567890ABCDEF</key><time>20191029170542</time><level>MESSAGE</level><context /><scope /><message>my message</message><details /><exception /></notification></logging>")]
        public void Format_ShowKeyAsDefined_ResultAsExpected(Boolean showKey, String expected)
        {
            String message = "my message";

            this.value.SetupGet(x => x.Message).Returns(message);
            this.settings.SetupGet(x => x.ShowKey).Returns(showKey);

            this.instance.Format(this.builder, this.value.Object);

            String actual = this.builder.ToString();

            Assert.That(actual, Is.EqualTo(String.Format(expected, message)));
        }

        [TestCase(false, "<?xml version=\"1.0\" encoding=\"UTF-8\"?><logging><notification><key>12345678-90AB-CDEF-1234-567890ABCDEF</key><time /><level>MESSAGE</level><context /><scope /><message>my message</message><details /><exception /></notification></logging>")]
        [TestCase(true, "<?xml version=\"1.0\" encoding=\"UTF-8\"?><logging><notification><key>12345678-90AB-CDEF-1234-567890ABCDEF</key><time>20191029170542</time><level>MESSAGE</level><context /><scope /><message>my message</message><details /><exception /></notification></logging>")]
        public void Format_ShowTimeAsDefined_ResultAsExpected(Boolean showTime, String expected)
        {
            String message = "my message";

            this.value.SetupGet(x => x.Message).Returns(message);
            this.settings.SetupGet(x => x.ShowTime).Returns(showTime);

            this.instance.Format(this.builder, this.value.Object);

            String actual = this.builder.ToString();

            Assert.That(actual, Is.EqualTo(String.Format(expected, message)));
        }

        [TestCase(LogLevel.Trace, "<?xml version=\"1.0\" encoding=\"UTF-8\"?><logging><notification><key>12345678-90AB-CDEF-1234-567890ABCDEF</key><time>20191029170542</time><level>TRACE</level><context /><scope /><message>my message</message><details /><exception /></notification></logging>")]
        [TestCase(LogLevel.Debug, "<?xml version=\"1.0\" encoding=\"UTF-8\"?><logging><notification><key>12345678-90AB-CDEF-1234-567890ABCDEF</key><time>20191029170542</time><level>DEBUG</level><context /><scope /><message>my message</message><details /><exception /></notification></logging>")]
        [TestCase(LogLevel.Verbose, "<?xml version=\"1.0\" encoding=\"UTF-8\"?><logging><notification><key>12345678-90AB-CDEF-1234-567890ABCDEF</key><time>20191029170542</time><level>VERBOSE</level><context /><scope /><message>my message</message><details /><exception /></notification></logging>")]
        [TestCase(LogLevel.Message, "<?xml version=\"1.0\" encoding=\"UTF-8\"?><logging><notification><key>12345678-90AB-CDEF-1234-567890ABCDEF</key><time>20191029170542</time><level>MESSAGE</level><context /><scope /><message>my message</message><details /><exception /></notification></logging>")]
        [TestCase(LogLevel.Warning, "<?xml version=\"1.0\" encoding=\"UTF-8\"?><logging><notification><key>12345678-90AB-CDEF-1234-567890ABCDEF</key><time>20191029170542</time><level>WARNING</level><context /><scope /><message>my message</message><details /><exception /></notification></logging>")]
        [TestCase(LogLevel.Error, "<?xml version=\"1.0\" encoding=\"UTF-8\"?><logging><notification><key>12345678-90AB-CDEF-1234-567890ABCDEF</key><time>20191029170542</time><level>ERROR</level><context /><scope /><message>my message</message><details /><exception /></notification></logging>")]
        [TestCase(LogLevel.Fatal, "<?xml version=\"1.0\" encoding=\"UTF-8\"?><logging><notification><key>12345678-90AB-CDEF-1234-567890ABCDEF</key><time>20191029170542</time><level>FATAL</level><context /><scope /><message>my message</message><details /><exception /></notification></logging>")]
        [TestCase(LogLevel.Critical, "<?xml version=\"1.0\" encoding=\"UTF-8\"?><logging><notification><key>12345678-90AB-CDEF-1234-567890ABCDEF</key><time>20191029170542</time><level>CRITICAL</level><context /><scope /><message>my message</message><details /><exception /></notification></logging>")]
        [TestCase(LogLevel.Disaster, "<?xml version=\"1.0\" encoding=\"UTF-8\"?><logging><notification><key>12345678-90AB-CDEF-1234-567890ABCDEF</key><time>20191029170542</time><level>DISASTER</level><context /><scope /><message>my message</message><details /><exception /></notification></logging>")]
        public void Format_LogLevelAsDefined_ResultAsExpected(LogLevel logLevel, String expected)
        {
            String message = "my message";

            this.value.SetupGet(x => x.Message).Returns(message);
            this.value.SetupGet(x => x.Level).Returns(logLevel);

            this.instance.Format(this.builder, this.value.Object);

            String actual = this.builder.ToString();

            Assert.That(actual, Is.EqualTo(String.Format(expected, message)));
        }

        [TestCase(null, "<?xml version=\"1.0\" encoding=\"UTF-8\"?><logging><notification><key>12345678-90AB-CDEF-1234-567890ABCDEF</key><time>20191029170542</time><level>MESSAGE</level><context /><scope /><message>my message</message><details /><exception /></notification></logging>")]
        [TestCase("", "<?xml version=\"1.0\" encoding=\"UTF-8\"?><logging><notification><key>12345678-90AB-CDEF-1234-567890ABCDEF</key><time>20191029170542</time><level>MESSAGE</level><context /><scope /><message>my message</message><details /><exception /></notification></logging>")]
        [TestCase(" ", "<?xml version=\"1.0\" encoding=\"UTF-8\"?><logging><notification><key>12345678-90AB-CDEF-1234-567890ABCDEF</key><time>20191029170542</time><level>MESSAGE</level><context /><scope /><message>my message</message><details /><exception /></notification></logging>")]
        [TestCase("my context", "<?xml version=\"1.0\" encoding=\"UTF-8\"?><logging><notification><key>12345678-90AB-CDEF-1234-567890ABCDEF</key><time>20191029170542</time><level>MESSAGE</level><context>my context</context><scope /><message>my message</message><details /><exception /></notification></logging>")]
        public void Format_ContextAsDefined_ResultAsExpected(String context, String expected)
        {
            String message = "my message";

            this.value.SetupGet(x => x.Message).Returns(message);
            this.value.SetupGet(x => x.Context).Returns(context);

            this.instance.Format(this.builder, this.value.Object);

            String actual = this.builder.ToString();

            Assert.That(actual, Is.EqualTo(String.Format(expected, message)));
        }

        [TestCase(null, "<?xml version=\"1.0\" encoding=\"UTF-8\"?><logging><notification><key>12345678-90AB-CDEF-1234-567890ABCDEF</key><time>20191029170542</time><level>MESSAGE</level><context /><scope /><message>my message</message><details /><exception /></notification></logging>")]
        [TestCase("", "<?xml version=\"1.0\" encoding=\"UTF-8\"?><logging><notification><key>12345678-90AB-CDEF-1234-567890ABCDEF</key><time>20191029170542</time><level>MESSAGE</level><context /><scope /><message>my message</message><details /><exception /></notification></logging>")]
        [TestCase(" ", "<?xml version=\"1.0\" encoding=\"UTF-8\"?><logging><notification><key>12345678-90AB-CDEF-1234-567890ABCDEF</key><time>20191029170542</time><level>MESSAGE</level><context /><scope /><message>my message</message><details /><exception /></notification></logging>")]
        [TestCase("my scope", "<?xml version=\"1.0\" encoding=\"UTF-8\"?><logging><notification><key>12345678-90AB-CDEF-1234-567890ABCDEF</key><time>20191029170542</time><level>MESSAGE</level><context /><scope>my scope</scope><message>my message</message><details /><exception /></notification></logging>")]
        public void Format_ScopeAsDefined_ResultAsExpected(String scope, String expected)
        {
            String message = "my message";

            this.value.SetupGet(x => x.Message).Returns(message);
            this.value.SetupGet(x => x.Scope).Returns(scope);

            this.instance.Format(this.builder, this.value.Object);

            String actual = this.builder.ToString();

            Assert.That(actual, Is.EqualTo(String.Format(expected, message)));
        }

        [TestCase(null, "<?xml version=\"1.0\" encoding=\"UTF-8\"?><logging><notification><key>12345678-90AB-CDEF-1234-567890ABCDEF</key><time>20191029170542</time><level>MESSAGE</level><context /><scope /><message /><details /><exception /></notification></logging>")]
        [TestCase("", "<?xml version=\"1.0\" encoding=\"UTF-8\"?><logging><notification><key>12345678-90AB-CDEF-1234-567890ABCDEF</key><time>20191029170542</time><level>MESSAGE</level><context /><scope /><message /><details /><exception /></notification></logging>")]
        [TestCase(" ", "<?xml version=\"1.0\" encoding=\"UTF-8\"?><logging><notification><key>12345678-90AB-CDEF-1234-567890ABCDEF</key><time>20191029170542</time><level>MESSAGE</level><context /><scope /><message /><details /><exception /></notification></logging>")]
        [TestCase("my message", "<?xml version=\"1.0\" encoding=\"UTF-8\"?><logging><notification><key>12345678-90AB-CDEF-1234-567890ABCDEF</key><time>20191029170542</time><level>MESSAGE</level><context /><scope /><message>my message</message><details /><exception /></notification></logging>")]
        public void Format_MessageAsDefined_ResultAsExpected(String message, String expected)
        {
            this.value.SetupGet(x => x.Message).Returns(message);

            this.instance.Format(this.builder, this.value.Object);

            String actual = this.builder.ToString();

            Assert.That(actual, Is.EqualTo(expected));
        }

        [TestCase(-1, 'a', "<?xml version=\"1.0\" encoding=\"UTF-8\"?><logging><notification><key>12345678-90AB-CDEF-1234-567890ABCDEF</key><time>20191029170542</time><level>MESSAGE</level><context /><scope /><message>{0}</message><details /><exception /></notification></logging>")]
        [TestCase(0, 'a', "<?xml version=\"1.0\" encoding=\"UTF-8\"?><logging><notification><key>12345678-90AB-CDEF-1234-567890ABCDEF</key><time>20191029170542</time><level>MESSAGE</level><context /><scope /><message>{0}</message><details /><exception /></notification></logging>")]
        [TestCase(1, 'a', "<?xml version=\"1.0\" encoding=\"UTF-8\"?><logging><notification><key>12345678-90AB-CDEF-1234-567890ABCDEF</key><time>20191029170542</time><level>MESSAGE</level><context /><scope /><message>{0}</message><details><label0>Value0</label0></details><exception /></notification></logging>")]
        [TestCase(2, 'a', "<?xml version=\"1.0\" encoding=\"UTF-8\"?><logging><notification><key>12345678-90AB-CDEF-1234-567890ABCDEF</key><time>20191029170542</time><level>MESSAGE</level><context /><scope /><message>{0}</message><details><label0>Value0</label0><label1>Value1</label1></details><exception /></notification></logging>")]
        [TestCase(-1, '@', "<?xml version=\"1.0\" encoding=\"UTF-8\"?><logging><notification><key>12345678-90AB-CDEF-1234-567890ABCDEF</key><time>20191029170542</time><level>MESSAGE</level><context /><scope /><message>{0}</message><details /><exception /></notification></logging>")]
        [TestCase(0, '@', "<?xml version=\"1.0\" encoding=\"UTF-8\"?><logging><notification><key>12345678-90AB-CDEF-1234-567890ABCDEF</key><time>20191029170542</time><level>MESSAGE</level><context /><scope /><message>{0}</message><details /><exception /></notification></logging>")]
        [TestCase(1, '@', "<?xml version=\"1.0\" encoding=\"UTF-8\"?><logging><notification><key>12345678-90AB-CDEF-1234-567890ABCDEF</key><time>20191029170542</time><level>MESSAGE</level><context /><scope /><message>{0}</message><details><label0>V@lue0</label0></details><exception /></notification></logging>")]
        [TestCase(2, '@', "<?xml version=\"1.0\" encoding=\"UTF-8\"?><logging><notification><key>12345678-90AB-CDEF-1234-567890ABCDEF</key><time>20191029170542</time><level>MESSAGE</level><context /><scope /><message>{0}</message><details><label0>V@lue0</label0><label1>V@lue1</label1></details><exception /></notification></logging>")]
        public void Format_ValuesAsDefined_ResultAsExpected(Int32 count, Char take, String expected)
        {
            String message = "my message";

            this.value.SetupGet(x => x.Message).Returns(message);
            this.value.SetupGet(x => x.Details).Returns(this.GetDetails(count, take));

            this.instance.Format(this.builder, this.value.Object);

            String actual = this.builder.ToString();

            Assert.That(actual, Is.EqualTo(String.Format(expected, message)));
        }

        [TestCase(false, "<?xml version=\"1.0\" encoding=\"UTF-8\"?><logging><notification><key>12345678-90AB-CDEF-1234-567890ABCDEF</key><time>20191029170542</time><level>MESSAGE</level><context /><scope /><message>{0}</message><details /><exception /></notification></logging>")]
        [TestCase(true, "<?xml version=\"1.0\" encoding=\"UTF-8\"?><logging><notification><key>12345678-90AB-CDEF-1234-567890ABCDEF</key><time>20191029170542</time><level>MESSAGE</level><context /><scope /><message>{0}</message><details /><exception>System.Exception: exception</exception></notification></logging>")]
        public void Format_ExceptionAsDefined_ResultAsExpected(Boolean exception, String expected)
        {
            String message = "my message";

            this.value.SetupGet(x => x.Message).Returns(message);
            this.value.SetupGet(x => x.Exception).Returns(exception ? new Exception("exception") : null);

            this.instance.Format(this.builder, this.value.Object);

            String actual = this.builder.ToString();

            Assert.That(actual, Is.EqualTo(String.Format(expected, message)));
        }

        [Test]
        public void Format_OutputAsDefined_ResultAsExpected()
        {
            String expected = "<?xml version=\"1.0\" encoding=\"UTF-8\"?><logging><notification><key>12345678-90AB-CDEF-1234-567890ABCDEF</key><time>20191029170542</time><level>MESSAGE</level><context>my context</context><scope>my scope</scope><message>my message</message><details><label0>Value0</label0><label1>Value1</label1></details><exception>System.Exception: exception</exception></notification></logging>";

            this.settings.SetupGet(x => x.ShowTime).Returns(true);
            this.value.SetupGet(x => x.Context).Returns("my context");
            this.value.SetupGet(x => x.Scope).Returns("my scope");
            this.value.SetupGet(x => x.Message).Returns("my message");
            this.value.SetupGet(x => x.Exception).Returns(new Exception("exception"));
            this.value.SetupGet(x => x.Details).Returns(this.GetDetails(2, 'a'));

            this.instance.Format(this.builder, this.value.Object);

            String actual = this.builder.ToString();

            Assert.That(actual, Is.EqualTo(expected));
        }

        [TestCase("de-DE", ':', "<?xml version=\"1.0\" encoding=\"UTF-8\"?><logging><notification><key>12345678-90AB-CDEF-1234-567890ABCDEF</key><time>20191029170542</time><level>MESSAGE</level><context /><scope /><message>{0}</message><details><boolean>true</boolean><double>1234567,89</double><decimal>1234567,89</decimal><datetime>29.10.2019 17:05:42</datetime><object>System.Object</object></details><exception /></notification></logging>")]
        [TestCase("de-DE", ',', "<?xml version=\"1.0\" encoding=\"UTF-8\"?><logging><notification><key>12345678-90AB-CDEF-1234-567890ABCDEF</key><time>20191029170542</time><level>MESSAGE</level><context /><scope /><message>{0}</message><details><boolean>true</boolean><double>1234567,89</double><decimal>1234567,89</decimal><datetime>29.10.2019 17:05:42</datetime><object>System.Object</object></details><exception /></notification></logging>")]
        [TestCase("de-DE", ';', "<?xml version=\"1.0\" encoding=\"UTF-8\"?><logging><notification><key>12345678-90AB-CDEF-1234-567890ABCDEF</key><time>20191029170542</time><level>MESSAGE</level><context /><scope /><message>{0}</message><details><boolean>true</boolean><double>1234567,89</double><decimal>1234567,89</decimal><datetime>29.10.2019 17:05:42</datetime><object>System.Object</object></details><exception /></notification></logging>")]
        [TestCase("en-US", ',', "<?xml version=\"1.0\" encoding=\"UTF-8\"?><logging><notification><key>12345678-90AB-CDEF-1234-567890ABCDEF</key><time>20191029170542</time><level>MESSAGE</level><context /><scope /><message>{0}</message><details><boolean>true</boolean><double>1234567.89</double><decimal>1234567.89</decimal><datetime>10/29/2019 5:05:42 PM</datetime><object>System.Object</object></details><exception /></notification></logging>")]
        [TestCase("en-US", ';', "<?xml version=\"1.0\" encoding=\"UTF-8\"?><logging><notification><key>12345678-90AB-CDEF-1234-567890ABCDEF</key><time>20191029170542</time><level>MESSAGE</level><context /><scope /><message>{0}</message><details><boolean>true</boolean><double>1234567.89</double><decimal>1234567.89</decimal><datetime>10/29/2019 5:05:42 PM</datetime><object>System.Object</object></details><exception /></notification></logging>")]
        [TestCase("en-US", ':', "<?xml version=\"1.0\" encoding=\"UTF-8\"?><logging><notification><key>12345678-90AB-CDEF-1234-567890ABCDEF</key><time>20191029170542</time><level>MESSAGE</level><context /><scope /><message>{0}</message><details><boolean>true</boolean><double>1234567.89</double><decimal>1234567.89</decimal><datetime>10/29/2019 5:05:42 PM</datetime><object>System.Object</object></details><exception /></notification></logging>")]
        public void Format_CulturesAsDefined_ResultAsExpected(String culture, Char part, String expected)
        {
            String message = "my message";

            (String Label, Object Value)[] details = new (String Label, Object Value)[] {
                ( Label: "Boolean",  Value: true                                       ),
                ( Label: "Double",   Value: 1234567.89                                 ),
                ( Label: "Decimal",  Value: 1234567.89m                                ),
                ( Label: "DateTime", Value: DateTime.Parse("2019-10-29T17:05:42.6789") ),
                ( Label: "Object",   Value: new Object()                               )
            };

            this.settings.SetupGet(x => x.PartSplit).Returns(part);
            this.settings.SetupGet(x => x.Culture).Returns(new CultureInfo(culture));
            this.value.SetupGet(x => x.Message).Returns(message);
            this.value.SetupGet(x => x.Details).Returns(details);

            this.instance.Format(this.builder, this.value.Object);

            String actual = this.builder.ToString();

            Assert.That(actual, Is.EqualTo(String.Format(expected, message)));
        }

        [Test]
        public void Format_CheckCharacterEscaping_ResultIsExpectedCharacterEscaping()
        {
            String message = "< \" & ' >";
            String expected = "<?xml version=\"1.0\" encoding=\"UTF-8\"?><logging><notification><key>12345678-90AB-CDEF-1234-567890ABCDEF</key><time>20191029170542</time><level>MESSAGE</level><context /><scope /><message>&lt; &quot; &amp; &apos; &gt;</message><details /><exception /></notification></logging>";

            this.value.SetupGet(x => x.Message).Returns(message);

            this.instance.Format(this.builder, this.value.Object);

            String actual = this.builder.ToString();

            Assert.That(actual, Is.EqualTo(expected));
        }

        #endregion

        #region Private helper methods

        private (String, Object)[] GetDetails(Int32 count, Char replace)
        {
            List<(String Label, Object Value)> values = null;

            if (count != -1)
            {
                values = new List<(String Label, Object Value)>();

                for (Int32 index = 0; index < count; index++)
                {
                    values.Add((Label: $"Label{index}", Value: $"V{replace}lue{index}"));
                }

                return values.ToArray();
            }

            return null;
        }

        #endregion
    }
}
