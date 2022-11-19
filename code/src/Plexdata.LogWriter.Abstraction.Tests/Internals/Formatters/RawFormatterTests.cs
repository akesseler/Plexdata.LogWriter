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
    [TestOf(nameof(RawFormatter))]
    public class RawFormatterTests
    {
        #region Prologue

        private readonly StringBuilder builder = new StringBuilder(); // => Implicit test of clearing the builder.

        private Mock<ILogEvent> value = null;
        private Mock<ILoggerSettings> settings = null;
        private RawFormatter instance = null;

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
            this.settings.SetupGet(x => x.LogType).Returns(LogType.Raw);
            this.settings.SetupGet(x => x.LogTime).Returns(LogTime.Utc);
            this.settings.SetupGet(x => x.PartSplit).Returns('@');
            this.settings.SetupGet(x => x.ShowTime).Returns(true);
            this.settings.SetupGet(x => x.TimeFormat).Returns("yyyyMMddHHmmss");
            this.settings.SetupGet(x => x.FullName).Returns(false);
            this.settings.SetupGet(x => x.Culture).Returns(new CultureInfo("en-US"));

            this.instance = new RawFormatter(this.settings.Object);
        }

        #endregion

        #region Construction

        [TestCase(LogType.Csv)]
        [TestCase(LogType.Json)]
        [TestCase(LogType.Xml)]
        public void RawFormatter_WrongLogType_ThrowsNotSupportedException(LogType logType)
        {
            this.settings.SetupGet(x => x.LogType).Returns(logType);

            Assert.That(() => new RawFormatter(this.settings.Object), Throws.InstanceOf<NotSupportedException>());
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

        [TestCase(false, "MESSAGE@{0}")]
        [TestCase(true, "20191029170542@MESSAGE@{0}")]
        public void Format_ShowTimeAsDefined_ResultAsExpected(Boolean showTime, String expected)
        {
            String message = "my message";

            this.value.SetupGet(x => x.Message).Returns(message);
            this.settings.SetupGet(x => x.ShowTime).Returns(showTime);

            this.instance.Format(this.builder, this.value.Object);

            String actual = this.builder.ToString();

            Assert.That(actual, Is.EqualTo(String.Format(expected, message)));
        }

        [TestCase(LogLevel.Trace, "20191029170542@TRACE@{0}")]
        [TestCase(LogLevel.Debug, "20191029170542@DEBUG@{0}")]
        [TestCase(LogLevel.Verbose, "20191029170542@VERBOSE@{0}")]
        [TestCase(LogLevel.Message, "20191029170542@MESSAGE@{0}")]
        [TestCase(LogLevel.Warning, "20191029170542@WARNING@{0}")]
        [TestCase(LogLevel.Error, "20191029170542@ERROR@{0}")]
        [TestCase(LogLevel.Fatal, "20191029170542@FATAL@{0}")]
        [TestCase(LogLevel.Critical, "20191029170542@CRITICAL@{0}")]
        [TestCase(LogLevel.Disaster, "20191029170542@DISASTER@{0}")]
        public void Format_LogLevelAsDefined_ResultAsExpected(LogLevel logLevel, String expected)
        {
            String message = "my message";

            this.value.SetupGet(x => x.Message).Returns(message);
            this.value.SetupGet(x => x.Level).Returns(logLevel);

            this.instance.Format(this.builder, this.value.Object);

            String actual = this.builder.ToString();

            Assert.That(actual, Is.EqualTo(String.Format(expected, message)));
        }

        [TestCase(null, "20191029170542@MESSAGE@{0}")]
        [TestCase("", "20191029170542@MESSAGE@{0}")]
        [TestCase(" ", "20191029170542@MESSAGE@{0}")]
        [TestCase("my context", "20191029170542@MESSAGE@my context@{0}")]
        public void Format_ContextAsDefined_ResultAsExpected(String context, String expected)
        {
            String message = "my message";

            this.value.SetupGet(x => x.Message).Returns(message);
            this.value.SetupGet(x => x.Context).Returns(context);

            this.instance.Format(this.builder, this.value.Object);

            String actual = this.builder.ToString();

            Assert.That(actual, Is.EqualTo(String.Format(expected, message)));
        }

        [TestCase(null, "20191029170542@MESSAGE@{0}")]
        [TestCase("", "20191029170542@MESSAGE@{0}")]
        [TestCase(" ", "20191029170542@MESSAGE@{0}")]
        [TestCase("my scope", "20191029170542@MESSAGE@my scope@{0}")]
        public void Format_ScopeAsDefined_ResultAsExpected(String scope, String expected)
        {
            String message = "my message";

            this.value.SetupGet(x => x.Message).Returns(message);
            this.value.SetupGet(x => x.Scope).Returns(scope);

            this.instance.Format(this.builder, this.value.Object);

            String actual = this.builder.ToString();

            Assert.That(actual, Is.EqualTo(String.Format(expected, message)));
        }

        [TestCase(null, "20191029170542@MESSAGE@empty")]
        [TestCase("", "20191029170542@MESSAGE@empty")]
        [TestCase(" ", "20191029170542@MESSAGE@empty")]
        [TestCase("my message", "20191029170542@MESSAGE@my message")]
        public void Format_MessageAsDefined_ResultAsExpected(String message, String expected)
        {
            this.value.SetupGet(x => x.Message).Returns(message);

            this.instance.Format(this.builder, this.value.Object);

            String actual = this.builder.ToString();

            Assert.That(actual, Is.EqualTo(expected));
        }

        [TestCase(-1, 'a', "20191029170542@MESSAGE@{0}")]
        [TestCase(0, 'a', "20191029170542@MESSAGE@{0}")]
        [TestCase(1, 'a', "20191029170542@MESSAGE@{0}@[Label0=Value0]")]
        [TestCase(2, 'a', "20191029170542@MESSAGE@{0}@[Label0=Value0@Label1=Value1]")]
        [TestCase(-1, '@', "20191029170542@MESSAGE@{0}")]
        [TestCase(0, '@', "20191029170542@MESSAGE@{0}")]
        [TestCase(1, '@', "20191029170542@MESSAGE@{0}@[Label0=V@lue0]")]
        [TestCase(2, '@', "20191029170542@MESSAGE@{0}@[Label0=V@lue0@Label1=V@lue1]")]
        public void Format_ValuesAsDefined_ResultAsExpected(Int32 count, Char take, String expected)
        {
            String message = "my message";

            this.value.SetupGet(x => x.Message).Returns(message);
            this.value.SetupGet(x => x.Details).Returns(this.GetDetails(count, take));

            this.instance.Format(this.builder, this.value.Object);

            String actual = this.builder.ToString();

            Assert.That(actual, Is.EqualTo(String.Format(expected, message)));
        }

        [TestCase(false, "20191029170542@MESSAGE@{0}")]
        [TestCase(true, "20191029170542@MESSAGE@{0}\r\nSystem.Exception: exception")]
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
            String expected = "20191029170542@MESSAGE@my context@my scope@my message@[Label0=Value0@Label1=Value1]\r\nSystem.Exception: exception";

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

        [TestCase("de-DE", ':', "20191029170542:MESSAGE:{0}:[Boolean=true:Double=1234567,89:Decimal=1234567,89:DateTime=29.10.2019 17:05:42:Object=System.Object]")]
        [TestCase("de-DE", ',', "20191029170542,MESSAGE,{0},[Boolean=true,Double=1234567,89,Decimal=1234567,89,DateTime=29.10.2019 17:05:42,Object=System.Object]")]
        [TestCase("de-DE", ';', "20191029170542;MESSAGE;{0};[Boolean=true;Double=1234567,89;Decimal=1234567,89;DateTime=29.10.2019 17:05:42;Object=System.Object]")]
        [TestCase("en-US", ',', "20191029170542,MESSAGE,{0},[Boolean=true,Double=1234567.89,Decimal=1234567.89,DateTime=10/29/2019 5:05:42 PM,Object=System.Object]")]
        [TestCase("en-US", ';', "20191029170542;MESSAGE;{0};[Boolean=true;Double=1234567.89;Decimal=1234567.89;DateTime=10/29/2019 5:05:42 PM;Object=System.Object]")]
        [TestCase("en-US", ':', "20191029170542:MESSAGE:{0}:[Boolean=true:Double=1234567.89:Decimal=1234567.89:DateTime=10/29/2019 5:05:42 PM:Object=System.Object]")]
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
