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
using Plexdata.LogWriter.Abstraction;
using Plexdata.LogWriter.Definitions;
using Plexdata.LogWriter.Settings;
using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text;

namespace Plexdata.LogWriter.Stream.Tests.Settings
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    [TestOf(nameof(StreamLoggerSettings))]
    public class StreamLoggerSettingsTests
    {
        #region Prologue

        private Mock<ILoggerSettingsSection> section;
        private Mock<ILoggerSettingsSection> configuration;
        private MemoryStream stream1;
        private MemoryStream stream2;

        [SetUp]
        public void Setup()
        {
            this.section = new Mock<ILoggerSettingsSection>();
            this.configuration = new Mock<ILoggerSettingsSection>();

            this.configuration
                .Setup(x => x.GetSection(It.IsAny<String>()))
                .Returns(this.section.Object);

            this.stream1 = new MemoryStream();
            this.stream2 = new MemoryStream();
        }

        [TearDown]
        public void Cleanup()
        {
            this.stream1.Dispose();
            this.stream1 = null;

            this.stream2.Dispose();
            this.stream2 = null;
        }

        #endregion

        #region Construction

        [Test]
        public void StreamLoggerSettings_ValidateDefaultSettings_DefaultSettingsAsExpected()
        {
            StreamLoggerSettings instance = new StreamLoggerSettings();

            Assert.That(instance.LogType, Is.EqualTo(LogType.Json));
            Assert.That(instance.Stream, Is.Null);
            Assert.That(instance.Encoding, Is.EqualTo(Encoding.UTF8));
        }

        [Test]
        public void StreamLoggerSettings_ValidateDefaultSettingsWithStream_DefaultSettingsAsExpected()
        {
            StreamLoggerSettings instance = new StreamLoggerSettings(this.stream1);

            Assert.That(instance.LogType, Is.EqualTo(LogType.Json));
            Assert.That(instance.Stream, Is.EqualTo(this.stream1));
            Assert.That(instance.Encoding, Is.EqualTo(Encoding.UTF8));
        }

        [Test]
        public void StreamLoggerSettings_ValidateDefaultSettingsForInvalidConfiguration_DefaultSettingsAsExpected()
        {
            StreamLoggerSettings instance = new StreamLoggerSettings((ILoggerSettingsSection)null);

            Assert.That(instance.LogType, Is.EqualTo(LogType.Json));
            Assert.That(instance.Stream, Is.Null);
            Assert.That(instance.Encoding, Is.EqualTo(Encoding.UTF8));
        }

        [Test]
        public void StreamLoggerSettings_ValidateDefaultSettingsForInvalidConfigurationWithStream_DefaultSettingsAsExpected()
        {
            StreamLoggerSettings instance = new StreamLoggerSettings((ILoggerSettingsSection)null, this.stream1);

            Assert.That(instance.LogType, Is.EqualTo(LogType.Json));
            Assert.That(instance.Stream, Is.EqualTo(this.stream1));
            Assert.That(instance.Encoding, Is.EqualTo(Encoding.UTF8));
        }

        [Test]
        public void StreamLoggerSettings_ConfigurationValid_GetSectionCalledAsExpected()
        {
            StreamLoggerSettings instance = new StreamLoggerSettings(this.configuration.Object);

            this.configuration.Verify(x => x.GetSection("Plexdata:LogWriter:Settings"), Times.Exactly(2));
        }

        [Test]
        public void StreamLoggerSettings_ConfigurationValid_GetSectionValueForLogTypeIsDefault()
        {
            this.section.SetupGet(x => x["LogType"]).Returns((String)null);

            StreamLoggerSettings instance = new StreamLoggerSettings(this.configuration.Object);

            Assert.That(instance.LogType, Is.EqualTo(LogType.Json));
        }

        [TestCaseSource(nameof(EncodingTestItemList))]
        public void StreamLoggerSettings_ConfigurationValid_GetSectionValueForEncodingAsExpected(Object current)
        {
            EncodingTestItem nominee = (EncodingTestItem)current;

            this.section.SetupGet(x => x["Encoding"]).Returns(nominee.Value);

            StreamLoggerSettings instance = new StreamLoggerSettings(this.configuration.Object);

            Assert.That(instance.Encoding.BodyName, Is.EqualTo(nominee.Result.BodyName));
        }

        #endregion

        #region Stream

        [Test]
        public void Stream_ValueChanged_PropertyChangedFired()
        {
            Boolean fired = false;
            StreamLoggerSettings instance = new StreamLoggerSettings();

            instance.Stream = this.stream1;
            instance.PropertyChanged += (sender, args) => { fired = true; };
            instance.Stream = this.stream2;

            Assert.That(fired, Is.True);
        }

        [Test]
        public void Stream_ValueUnchanged_PropertyChangedNotFired()
        {
            Boolean fired = false;
            StreamLoggerSettings instance = new StreamLoggerSettings();

            instance.Stream = this.stream1;
            instance.PropertyChanged += (sender, args) => { fired = true; };
            instance.Stream = this.stream1;

            Assert.That(fired, Is.False);
        }

        #endregion

        #region Encoding

        [Test]
        public void Encoding_ValueChanged_PropertyChangedFired()
        {
            Boolean fired = false;
            StreamLoggerSettings instance = new StreamLoggerSettings();

            instance.Encoding = Encoding.UTF7;
            instance.PropertyChanged += (sender, args) => { fired = true; };
            instance.Encoding = Encoding.UTF32;

            Assert.That(fired, Is.True);
        }

        [Test]
        public void Encoding_ValueUnchanged_PropertyChangedNotFired()
        {
            Boolean fired = false;
            StreamLoggerSettings instance = new StreamLoggerSettings();

            instance.Encoding = Encoding.ASCII;
            instance.PropertyChanged += (sender, args) => { fired = true; };
            instance.Encoding = Encoding.ASCII;

            Assert.That(fired, Is.False);
        }

        #endregion

        #region Other privates

        private class EncodingTestItem
        {
            public EncodingTestItem(String value, Encoding result)
            {
                this.Value = value;
                this.Result = result;
            }

            public String Value { get; private set; }

            public Encoding Result { get; private set; }

            public override String ToString()
            {
                return $"Value=\"{(this.Value ?? "<null>")}\", Result=\"{this.Result.BodyName}\"";
            }
        }

        public static Object[] EncodingTestItemList = new Object[] {
            new EncodingTestItem(null,          Encoding.UTF8),
            new EncodingTestItem("",            Encoding.UTF8),
            new EncodingTestItem(" ",           Encoding.UTF8),
            new EncodingTestItem("invalid",     Encoding.UTF8),
            new EncodingTestItem("utf-7",       Encoding.UTF7),
            new EncodingTestItem("utf-16be",    Encoding.BigEndianUnicode),
            new EncodingTestItem("utf-16",      Encoding.Unicode),
            new EncodingTestItem("iso-8859-1",  Encoding.Default),
            new EncodingTestItem("iso-8859-15", Encoding.GetEncoding("iso-8859-15")),
            new EncodingTestItem("us-ascii",    Encoding.ASCII),
            new EncodingTestItem("utf-8",       Encoding.UTF8),
            new EncodingTestItem("utf-32",      Encoding.UTF32),
            new EncodingTestItem("UTF-7",       Encoding.UTF7),
            new EncodingTestItem("UTF-16BE",    Encoding.BigEndianUnicode),
            new EncodingTestItem("UTF-16",      Encoding.Unicode),
            new EncodingTestItem("ISO-8859-1",  Encoding.Default),
            new EncodingTestItem("US-ASCII",    Encoding.ASCII),
            new EncodingTestItem("UTF-8",       Encoding.UTF8),
            new EncodingTestItem("UTF-32",      Encoding.UTF32),
            new EncodingTestItem("ISO-8859-15", Encoding.GetEncoding("iso-8859-15")),
        };

        #endregion
    }
}
