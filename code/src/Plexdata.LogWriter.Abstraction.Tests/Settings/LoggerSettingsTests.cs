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
using Plexdata.LogWriter.Settings;
using Plexdata.Utilities.Testing;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Plexdata.LogWriter.Abstraction.Tests.Settings
{
    #region Public test enum

    public enum TestEnum
    {
        TestEnumValue1 = 0,
        TestEnumValue2 = 1
    }

    #endregion

    [TestFixture]
    [ExcludeFromCodeCoverage]
    [Category(TestType.UnitTest)]
    [TestOf(nameof(LoggerSettings))]
    public class LoggerSettingsTests
    {
        #region Prologue

        private Mock<ILoggerSettingsSection> section;
        private Mock<ILoggerSettingsSection> configuration;

        [SetUp]
        public void Setup()
        {
            this.section = new Mock<ILoggerSettingsSection>();
            this.configuration = new Mock<ILoggerSettingsSection>();

            this.configuration
                .Setup(x => x.GetSection(It.IsAny<String>()))
                .Returns(this.section.Object);
        }

        #endregion

        #region LoggerSettings

        [Test]
        public void LoggerSettings_ValidateDefaultTimeFormat_DefaultTimeFormatAsExpected()
        {
            Assert.That(LoggerSettings.DefaultTimeFormat, Is.EqualTo("yyyy-MM-dd HH:mm:ss.ffff"));
        }

        [Test]
        public void LoggerSettings_ValidateDefaultSettings_DefaultSettingsAsExpected()
        {
            ILoggerSettings instance = new LoggerSettingsDummyClass();

            Assert.That(instance.LogLevel, Is.EqualTo(LogLevel.Default));
            Assert.That(instance.LogType, Is.EqualTo(LogType.Default));
            Assert.That(instance.LogTime, Is.EqualTo(LogTime.Default));
            Assert.That(instance.ShowKey, Is.EqualTo(true));
            Assert.That(instance.ShowTime, Is.EqualTo(true));
            Assert.That(instance.TimeFormat, Is.EqualTo(LoggerSettings.DefaultTimeFormat));
            Assert.That(instance.PartSplit, Is.EqualTo(';'));
            Assert.That(instance.FullName, Is.EqualTo(true));
            Assert.That(instance.Culture, Is.Not.Null);
            Assert.That(instance.Culture.Name, Is.EqualTo("en-US"));
        }

        #endregion

        #region LogLevel

        [Test]
        public void LogLevel_ValueChanged_PropertyChangedFired()
        {
            Boolean fired = false;
            ILoggerSettings instance = new LoggerSettingsDummyClass();

            instance.LogLevel = LogLevel.Trace;
            instance.PropertyChanged += (sender, args) => { fired = true; };
            instance.LogLevel = LogLevel.Critical;

            Assert.That(fired, Is.True);
        }

        [Test]
        public void LogLevel_ValueUnchanged_PropertyChangedNotFired()
        {
            Boolean fired = false;
            ILoggerSettings instance = new LoggerSettingsDummyClass();

            instance.LogLevel = LogLevel.Critical;
            instance.PropertyChanged += (sender, args) => { fired = true; };
            instance.LogLevel = LogLevel.Critical;

            Assert.That(fired, Is.False);
        }

        #endregion

        #region LogType

        [Test]
        public void LogType_ValueChanged_PropertyChangedFired()
        {
            Boolean fired = false;
            ILoggerSettings instance = new LoggerSettingsDummyClass();

            instance.LogType = LogType.Csv;
            instance.PropertyChanged += (sender, args) => { fired = true; };
            instance.LogType = LogType.Raw;

            Assert.That(fired, Is.True);
        }

        [Test]
        public void LogType_ValueUnchanged_PropertyChangedNotFired()
        {
            Boolean fired = false;
            ILoggerSettings instance = new LoggerSettingsDummyClass();

            instance.LogType = LogType.Json;
            instance.PropertyChanged += (sender, args) => { fired = true; };
            instance.LogType = LogType.Json;

            Assert.That(fired, Is.False);
        }

        #endregion

        #region LogTime

        [Test]
        public void LogTime_ValueChanged_PropertyChangedFired()
        {
            Boolean fired = false;
            ILoggerSettings instance = new LoggerSettingsDummyClass();

            instance.LogTime = LogTime.Local;
            instance.PropertyChanged += (sender, args) => { fired = true; };
            instance.LogTime = LogTime.Utc;

            Assert.That(fired, Is.True);
        }

        [Test]
        public void LogTime_ValueUnchanged_PropertyChangedNotFired()
        {
            Boolean fired = false;
            ILoggerSettings instance = new LoggerSettingsDummyClass();

            instance.LogTime = LogTime.Default;
            instance.PropertyChanged += (sender, args) => { fired = true; };
            instance.LogTime = LogTime.Default;

            Assert.That(fired, Is.False);
        }

        #endregion

        #region ShowKey

        [Test]
        public void ShowKey_ValueChanged_PropertyChangedFired()
        {
            Boolean fired = false;
            ILoggerSettings instance = new LoggerSettingsDummyClass();

            instance.ShowKey = false;
            instance.PropertyChanged += (sender, args) => { fired = true; };
            instance.ShowKey = true;

            Assert.That(fired, Is.True);
        }

        [Test]
        public void ShowKey_ValueUnchanged_PropertyChangedNotFired()
        {
            Boolean fired = false;
            ILoggerSettings instance = new LoggerSettingsDummyClass();

            instance.ShowKey = true;
            instance.PropertyChanged += (sender, args) => { fired = true; };
            instance.ShowKey = true;

            Assert.That(fired, Is.False);
        }

        #endregion

        #region ShowTime

        [Test]
        public void ShowTime_ValueChanged_PropertyChangedFired()
        {
            Boolean fired = false;
            ILoggerSettings instance = new LoggerSettingsDummyClass();

            instance.ShowTime = false;
            instance.PropertyChanged += (sender, args) => { fired = true; };
            instance.ShowTime = true;

            Assert.That(fired, Is.True);
        }

        [Test]
        public void ShowTime_ValueUnchanged_PropertyChangedNotFired()
        {
            Boolean fired = false;
            ILoggerSettings instance = new LoggerSettingsDummyClass();

            instance.ShowTime = true;
            instance.PropertyChanged += (sender, args) => { fired = true; };
            instance.ShowTime = true;

            Assert.That(fired, Is.False);
        }

        #endregion

        #region TimeFormat

        [Test]
        public void TimeFormat_ValueChanged_PropertyChangedFired()
        {
            Boolean fired = false;
            ILoggerSettings instance = new LoggerSettingsDummyClass();

            instance.TimeFormat = "old time format";
            instance.PropertyChanged += (sender, args) => { fired = true; };
            instance.TimeFormat = "new time format";

            Assert.That(fired, Is.True);
        }

        [Test]
        public void TimeFormat_ValueUnchanged_PropertyChangedNotFired()
        {
            Boolean fired = false;
            ILoggerSettings instance = new LoggerSettingsDummyClass();

            instance.TimeFormat = "same time format";
            instance.PropertyChanged += (sender, args) => { fired = true; };
            instance.TimeFormat = "same time format";

            Assert.That(fired, Is.False);
        }

        #endregion

        #region PartSplit

        [Test]
        public void PartSplit_ValueChanged_PropertyChangedFired()
        {
            Boolean fired = false;
            ILoggerSettings instance = new LoggerSettingsDummyClass();

            instance.PartSplit = '#';
            instance.PropertyChanged += (sender, args) => { fired = true; };
            instance.PartSplit = '@';

            Assert.That(fired, Is.True);
        }

        [Test]
        public void PartSplit_ValueUnchanged_PropertyChangedNotFired()
        {
            Boolean fired = false;
            ILoggerSettings instance = new LoggerSettingsDummyClass();

            instance.PartSplit = '|';
            instance.PropertyChanged += (sender, args) => { fired = true; };
            instance.PartSplit = '|';

            Assert.That(fired, Is.False);
        }

        #endregion

        #region FullName

        [Test]
        public void FullName_ValueChanged_PropertyChangedFired()
        {
            Boolean fired = false;
            ILoggerSettings instance = new LoggerSettingsDummyClass();

            instance.FullName = false;
            instance.PropertyChanged += (sender, args) => { fired = true; };
            instance.FullName = true;

            Assert.That(fired, Is.True);
        }

        [Test]
        public void FullName_ValueUnchanged_PropertyChangedNotFired()
        {
            Boolean fired = false;
            ILoggerSettings instance = new LoggerSettingsDummyClass();

            instance.FullName = true;
            instance.PropertyChanged += (sender, args) => { fired = true; };
            instance.FullName = true;

            Assert.That(fired, Is.False);
        }

        #endregion

        #region Culture

        [Test]
        public void Culture_ValueChanged_PropertyChangedFired()
        {
            Boolean fired = false;
            ILoggerSettings instance = new LoggerSettingsDummyClass();

            instance.Culture = CultureInfo.GetCultureInfo("en-US");
            instance.PropertyChanged += (sender, args) => { fired = true; };
            instance.Culture = CultureInfo.GetCultureInfo("de-DE");

            Assert.That(fired, Is.True);
        }

        [Test]
        public void Culture_ValueUnchanged_PropertyChangedNotFired()
        {
            Boolean fired = false;
            ILoggerSettings instance = new LoggerSettingsDummyClass();

            instance.Culture = CultureInfo.GetCultureInfo("en-US");
            instance.PropertyChanged += (sender, args) => { fired = true; };
            instance.Culture = CultureInfo.GetCultureInfo("en-US");

            Assert.That(fired, Is.False);
        }

        #endregion

        #region PropertyChanged

        [TestCase(null)]
        [TestCase("")]
        [TestCase("  ")]
        public void PropertyChanged_InvalidPropertyName_ThrowsArgumentOutOfRangeException(String property)
        {
            LoggerSettingsDummyClass instance = new LoggerSettingsDummyClass();

            Assert.That(() => instance.TestRaisePropertyChanged(property), Throws.InstanceOf<ArgumentOutOfRangeException>());
        }

        #endregion

        #region LoadSettings

        [Test]
        public void LoadSettings_ConfigurationIsNull_NothingChanges()
        {
            Int32 fired = 0;
            LoggerSettingsDummyClass instance = new LoggerSettingsDummyClass();

            instance.PropertyChanged += (sender, args) => { fired++; };

            instance.LoadSettingsTest(null);

            Assert.That(fired, Is.Zero);
        }

        [Test]
        public void LoadSettings_ConfigurationValid_GetSectionCalledOnceWithSettingsPath()
        {
            LoggerSettingsDummyClass instance = new LoggerSettingsDummyClass();

            instance.LoadSettingsTest(this.configuration.Object);

            this.configuration.Verify(x => x.GetSection("Plexdata:LogWriter:Settings"), Times.Once);
        }

        [TestCase(null, LogLevel.Default)]
        [TestCase("", LogLevel.Default)]
        [TestCase(" ", LogLevel.Default)]
        [TestCase("invalid", LogLevel.Default)]
        [TestCase("trace", LogLevel.Trace)]
        [TestCase("TRACE", LogLevel.Trace)]
        [TestCase("Trace", LogLevel.Trace)]
        [TestCase("debug", LogLevel.Debug)]
        [TestCase("DEBUG", LogLevel.Debug)]
        [TestCase("Debug", LogLevel.Debug)]
        [TestCase("verbose", LogLevel.Verbose)]
        [TestCase("VERBOSE", LogLevel.Verbose)]
        [TestCase("Verbose", LogLevel.Verbose)]
        [TestCase("message", LogLevel.Message)]
        [TestCase("MESSAGE", LogLevel.Message)]
        [TestCase("Message", LogLevel.Message)]
        [TestCase("warning", LogLevel.Warning)]
        [TestCase("WARNING", LogLevel.Warning)]
        [TestCase("Warning", LogLevel.Warning)]
        [TestCase("error", LogLevel.Error)]
        [TestCase("ERROR", LogLevel.Error)]
        [TestCase("Error", LogLevel.Error)]
        [TestCase("fatal", LogLevel.Fatal)]
        [TestCase("FATAL", LogLevel.Fatal)]
        [TestCase("Fatal", LogLevel.Fatal)]
        [TestCase("critical", LogLevel.Critical)]
        [TestCase("CRITICAL", LogLevel.Critical)]
        [TestCase("Critical", LogLevel.Critical)]
        [TestCase("disaster", LogLevel.Disaster)]
        [TestCase("DISASTER", LogLevel.Disaster)]
        [TestCase("Disaster", LogLevel.Disaster)]
        public void LoadSettings_ConfigurationValid_GetSectionValueForLogLevelAsExpected(String value, Object expected)
        {
            LoggerSettingsDummyClass instance = new LoggerSettingsDummyClass();

            this.section.SetupGet(x => x["LogLevel"]).Returns(value);

            instance.LoadSettingsTest(this.configuration.Object);

            Assert.That(instance.LogLevel, Is.EqualTo(expected));
        }

        [TestCase(null, LogType.Default)]
        [TestCase("", LogType.Default)]
        [TestCase(" ", LogType.Default)]
        [TestCase("invalid", LogType.Default)]
        [TestCase("csv", LogType.Csv)]
        [TestCase("CSV", LogType.Csv)]
        [TestCase("Csv", LogType.Csv)]
        [TestCase("raw", LogType.Raw)]
        [TestCase("RAW", LogType.Raw)]
        [TestCase("Raw", LogType.Raw)]
        [TestCase("json", LogType.Json)]
        [TestCase("JSON", LogType.Json)]
        [TestCase("Json", LogType.Json)]
        public void LoadSettings_ConfigurationValid_GetSectionValueForLogTypeAsExpected(String value, Object expected)
        {
            LoggerSettingsDummyClass instance = new LoggerSettingsDummyClass();

            this.section.SetupGet(x => x["LogType"]).Returns(value);

            instance.LoadSettingsTest(this.configuration.Object);

            Assert.That(instance.LogType, Is.EqualTo(expected));
        }

        [TestCase(null, LogTime.Default)]
        [TestCase("", LogTime.Default)]
        [TestCase(" ", LogTime.Default)]
        [TestCase("invalid", LogTime.Default)]
        [TestCase("utc", LogTime.Utc)]
        [TestCase("UTC", LogTime.Utc)]
        [TestCase("Utc", LogTime.Utc)]
        [TestCase("local", LogTime.Local)]
        [TestCase("LOCAL", LogTime.Local)]
        [TestCase("Local", LogTime.Local)]
        public void LoadSettings_ConfigurationValid_GetSectionValueForLogTimeAsExpected(String value, Object expected)
        {
            LoggerSettingsDummyClass instance = new LoggerSettingsDummyClass();

            this.section.SetupGet(x => x["LogTime"]).Returns(value);

            instance.LoadSettingsTest(this.configuration.Object);

            Assert.That(instance.LogTime, Is.EqualTo(expected));
        }

        [TestCase(null, true)]
        [TestCase("", true)]
        [TestCase(" ", true)]
        [TestCase("invalid", true)]
        [TestCase("true", true)]
        [TestCase("TRUE", true)]
        [TestCase("True", true)]
        [TestCase("false", false)]
        [TestCase("FALSE", false)]
        [TestCase("False", false)]
        public void LoadSettings_ConfigurationValid_GetSectionValueForShowKeyAsExpected(String value, Object expected)
        {
            LoggerSettingsDummyClass instance = new LoggerSettingsDummyClass();

            this.section.SetupGet(x => x["ShowKey"]).Returns(value);

            instance.LoadSettingsTest(this.configuration.Object);

            Assert.That(instance.ShowKey, Is.EqualTo(expected));
        }

        [TestCase(null, true)]
        [TestCase("", true)]
        [TestCase(" ", true)]
        [TestCase("invalid", true)]
        [TestCase("true", true)]
        [TestCase("TRUE", true)]
        [TestCase("True", true)]
        [TestCase("false", false)]
        [TestCase("FALSE", false)]
        [TestCase("False", false)]
        public void LoadSettings_ConfigurationValid_GetSectionValueForShowTimeAsExpected(String value, Object expected)
        {
            LoggerSettingsDummyClass instance = new LoggerSettingsDummyClass();

            this.section.SetupGet(x => x["ShowTime"]).Returns(value);

            instance.LoadSettingsTest(this.configuration.Object);

            Assert.That(instance.ShowTime, Is.EqualTo(expected));
        }

        [TestCase(null, "yyyy-MM-dd HH:mm:ss.ffff")]
        [TestCase("", "yyyy-MM-dd HH:mm:ss.ffff")]
        [TestCase(" ", "yyyy-MM-dd HH:mm:ss.ffff")]
        [TestCase("time-format", "time-format")]
        [TestCase("TIME-FORMAT", "TIME-FORMAT")]
        [TestCase("Time-Format", "Time-Format")]
        public void LoadSettings_ConfigurationValid_GetSectionValueForTimeFormatAsExpected(String value, Object expected)
        {
            LoggerSettingsDummyClass instance = new LoggerSettingsDummyClass();

            this.section.SetupGet(x => x["TimeFormat"]).Returns(value);

            instance.LoadSettingsTest(this.configuration.Object);

            Assert.That(instance.TimeFormat, Is.EqualTo(expected));
        }

        [TestCase(null, ';')]
        [TestCase("", ';')]
        [TestCase(" ", ';')]
        [TestCase("abc", 'a')]
        [TestCase("ABC", 'A')]
        [TestCase("Abc", 'A')]
        public void LoadSettings_ConfigurationValid_GetSectionValueForPartSplitAsExpected(String value, Object expected)
        {
            LoggerSettingsDummyClass instance = new LoggerSettingsDummyClass();

            this.section.SetupGet(x => x["PartSplit"]).Returns(value);

            instance.LoadSettingsTest(this.configuration.Object);

            Assert.That(instance.PartSplit, Is.EqualTo(expected));
        }

        [TestCase(null, true)]
        [TestCase("", true)]
        [TestCase(" ", true)]
        [TestCase("invalid", true)]
        [TestCase("true", true)]
        [TestCase("TRUE", true)]
        [TestCase("True", true)]
        [TestCase("false", false)]
        [TestCase("FALSE", false)]
        [TestCase("False", false)]
        public void LoadSettings_ConfigurationValid_GetSectionValueForFullNameAsExpected(String value, Object expected)
        {
            LoggerSettingsDummyClass instance = new LoggerSettingsDummyClass();

            this.section.SetupGet(x => x["FullName"]).Returns(value);

            instance.LoadSettingsTest(this.configuration.Object);

            Assert.That(instance.FullName, Is.EqualTo(expected));
        }

        [TestCase(null, "en-US")]
        [TestCase("", "en-US")]
        [TestCase(" ", "en-US")]
        [TestCase("invalid", "en-US")]
        [TestCase("de", "de")]
        [TestCase("de-de", "de-DE")]
        [TestCase("DE-DE", "de-DE")]
        public void LoadSettings_ConfigurationValid_GetSectionValueForCultureAsExpected(String value, Object expected)
        {
            LoggerSettingsDummyClass instance = new LoggerSettingsDummyClass();

            this.section.SetupGet(x => x["Culture"]).Returns(value);

            instance.LoadSettingsTest(this.configuration.Object);

            Assert.That(instance.Culture.Name, Is.EqualTo(expected));
        }

        #endregion

        #region GetValue

        [TestCase(null, "standard")]
        [TestCase("", "standard")]
        [TestCase(" ", "standard")]
        public void GetValue_ValueIsInvalid_ResultIsStandard(String value, String standard)
        {
            LoggerSettingsDummyClass instance = new LoggerSettingsDummyClass();

            Assert.That(instance.TestGetValue<String>(value, standard), Is.EqualTo(standard));
        }

        [TestCase("TestEnumValue1", TestEnum.TestEnumValue1, TestEnum.TestEnumValue1)]
        [TestCase("testenumvalue1", TestEnum.TestEnumValue1, TestEnum.TestEnumValue1)]
        [TestCase("TestEnumValue2", TestEnum.TestEnumValue1, TestEnum.TestEnumValue2)]
        [TestCase("testenumvalue2", TestEnum.TestEnumValue1, TestEnum.TestEnumValue2)]
        [TestCase("TestEnumInvalid", TestEnum.TestEnumValue1, TestEnum.TestEnumValue1)]
        [TestCase("testenuminvalid", TestEnum.TestEnumValue1, TestEnum.TestEnumValue1)]
        public void GetValue_ValueIsEnum_ResultAsExpected(String value, TestEnum standard, TestEnum expected)
        {
            LoggerSettingsDummyClass instance = new LoggerSettingsDummyClass();

            Assert.That(instance.TestGetValue<TestEnum>(value, standard), Is.EqualTo(expected));
        }

        [TestCase("False", true, false)]
        [TestCase("FALSE", true, false)]
        [TestCase("false", true, false)]
        [TestCase("True", false, true)]
        [TestCase("TRUE", false, true)]
        [TestCase("true", false, true)]
        [TestCase("Invalid", false, false)]
        [TestCase("INVALID", false, false)]
        [TestCase("invalid", false, false)]
        public void GetValue_ValueIsBoolean_ResultAsExpected(String value, Boolean standard, Boolean expected)
        {
            LoggerSettingsDummyClass instance = new LoggerSettingsDummyClass();

            Assert.That(instance.TestGetValue<Boolean>(value, standard), Is.EqualTo(expected));
        }

        [TestCase("string", "other", "string")]
        public void GetValue_ValueIsString_ResultAsExpected(String value, String standard, String expected)
        {
            LoggerSettingsDummyClass instance = new LoggerSettingsDummyClass();

            Assert.That(instance.TestGetValue<String>(value, standard), Is.EqualTo(expected));
        }

        [TestCase("string", 'o', 's')]
        public void GetValue_ValueIsChar_ResultAsExpected(String value, Char standard, Char expected)
        {
            LoggerSettingsDummyClass instance = new LoggerSettingsDummyClass();

            Assert.That(instance.TestGetValue<Char>(value, standard), Is.EqualTo(expected));
        }

        [TestCase("42", -1, 42)]
        [TestCase("xx", -1, -1)]
        public void GetValue_ValueIsInt32_ResultAsExpected(String value, Int32 standard, Int32 expected)
        {
            LoggerSettingsDummyClass instance = new LoggerSettingsDummyClass();

            Assert.That(instance.TestGetValue<Int32>(value, standard), Is.EqualTo(expected));
        }

        [TestCase("en-US", "de-DE", "en-US")]
        [TestCase("de-DE", "en-US", "de-DE")]
        [TestCase("xx-YY", "en-US", "en-US")]
        public void GetValue_ValueIsCultureInfo_ResultAsExpected(String value, String standard, String expected)
        {
            LoggerSettingsDummyClass instance = new LoggerSettingsDummyClass();

            Assert.That(instance.TestGetValue<CultureInfo>(value, CultureInfo.GetCultureInfo(standard)), Is.EqualTo(CultureInfo.GetCultureInfo(expected)));
        }

        [TestCase("invalid", "utf-8", "utf-8")]
        [TestCase("utf-7", "utf-8", "utf-7")]
        [TestCase("utf-8", "utf-8", "utf-8")]
        public void GetValue_ValueIsEncoding_ResultAsExpected(String value, String standard, String expected)
        {
            LoggerSettingsDummyClass instance = new LoggerSettingsDummyClass();

            Assert.That(instance.TestGetValue<Encoding>(value, Encoding.GetEncoding(standard)), Is.EqualTo(Encoding.GetEncoding(expected)));
        }

        [Test]
        public void GetValue_ValueIsOther_ResultIsStandard()
        {
            LoggerSettingsDummyClass instance = new LoggerSettingsDummyClass();

            String value = "4711";
            Int64 standard = 42;
            Int64 expected = 42;

            Assert.That(instance.TestGetValue<Int64>(value, standard), Is.EqualTo(expected));
        }

        #endregion

        #region GetSectionValues

        [Test]
        public void GetSectionValues_SectionIsNull_ResultIsStandard()
        {
            List<String> standard = new List<String>() { "1", "2", "3" };
            List<String> expected = new List<String>() { "1", "2", "3" };

            LoggerSettingsDummyClass instance = new LoggerSettingsDummyClass();

            Assert.That(instance.TestGetSectionValues(null, "key", standard).SequenceEqual(expected), Is.True);
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase("  ")]
        public void GetSectionValues_KeyInvalid_ResultIsStandard(String key)
        {
            List<String> standard = new List<String>() { "1", "2", "3" };
            List<String> expected = new List<String>() { "1", "2", "3" };

            LoggerSettingsDummyClass instance = new LoggerSettingsDummyClass();

            Assert.That(instance.TestGetSectionValues(this.section.Object, key, standard).SequenceEqual(expected), Is.True);
        }

        [Test]
        public void GetSectionValues_SectionGetValuesReturnsEmpty_ResultIsStandard()
        {
            List<String> standard = new List<String>() { "1", "2", "3" };
            List<String> expected = new List<String>() { "1", "2", "3" };

            this.section.Setup(x => x.GetValues(It.IsAny<String>())).Returns(Enumerable.Empty<String>());

            LoggerSettingsDummyClass instance = new LoggerSettingsDummyClass();

            Assert.That(instance.TestGetSectionValues(this.section.Object, "key", standard).SequenceEqual(expected), Is.True);
        }

        [Test]
        public void GetSectionValues_SectionGetValuesReturnsValues_ResultAsExpected()
        {
            List<String> standard = new List<String>() { "1", "2", "3" };
            List<String> expected = new List<String>() { "3", "2", "1" };
            List<String> result = new List<String>() { "3", "2", "1" };

            this.section.Setup(x => x.GetValues(It.IsAny<String>())).Returns(result);

            LoggerSettingsDummyClass instance = new LoggerSettingsDummyClass();

            Assert.That(instance.TestGetSectionValues(this.section.Object, "key", standard).SequenceEqual(expected), Is.True);
        }

        #endregion

        #region Private test class implementations

        private class LoggerSettingsDummyClass : LoggerSettings
        {
            public LoggerSettingsDummyClass() : base() { }

            public void TestRaisePropertyChanged(String property)
            {
                base.RaisePropertyChanged(property);
            }

            public void LoadSettingsTest(ILoggerSettingsSection configuration)
            {
                base.LoadSettings(configuration);
            }

            public TType TestGetValue<TType>(String value, TType standard)
            {
                return base.GetValue<TType>(value, standard);
            }

            public IEnumerable<String> TestGetSectionValues(ILoggerSettingsSection section, String key, IEnumerable<String> standard)
            {
                return base.GetSectionValues(section, key, standard);
            }
        }

        #endregion
    }
}
