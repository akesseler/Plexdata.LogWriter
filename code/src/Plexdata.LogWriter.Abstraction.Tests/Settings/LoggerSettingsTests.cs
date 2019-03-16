/*
 * MIT License
 * 
 * Copyright (c) 2019 plexdata.de
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
using Plexdata.LogWriter.Settings;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace Plexdata.LogWriter.Abstraction.Tests.Settings
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    [TestOf(nameof(LoggerSettings))]
    public class LoggerSettingsTests
    {
        #region LoggerSettings

        [Test]
        public void LoggerSettings_ValidateDefaultTimeFormat_DefaultTimeFormatAsExpected()
        {
            Assert.That(LoggerSettings.DefaultTimeFormat, Is.EqualTo("yyyy-MM-dd HH:mm:ss.ffff"));
        }

        [Test]
        public void LoggerSettings_ValidateDefaultSettings_DefaultSettingsAsExpected()
        {
            LoggerSettingsDummyClass instance = new LoggerSettingsDummyClass();

            Assert.That(instance.LogLevel, Is.EqualTo(LogLevel.Default));
            Assert.That(instance.LogType, Is.EqualTo(LogType.Default));
            Assert.That(instance.LogTime, Is.EqualTo(LogTime.Default));
            Assert.That(instance.ShowTime, Is.EqualTo(true));
            Assert.That(instance.TimeFormat, Is.EqualTo(LoggerSettings.DefaultTimeFormat));
            Assert.That(instance.PartSplit, Is.EqualTo(';'));
            Assert.That(instance.FullName, Is.EqualTo(true));
            Assert.That(instance.Culture.Name, Is.EqualTo("en-US"));
        }

        #endregion

        #region LogLevel

        [Test]
        public void LogLevel_ValueChanged_PropertyChangedFired()
        {
            Boolean fired = false;
            LoggerSettingsDummyClass instance = new LoggerSettingsDummyClass();

            instance.LogLevel = LogLevel.Trace;
            instance.PropertyChanged += (sender, args) => { fired = true; };
            instance.LogLevel = LogLevel.Critical;

            Assert.That(fired, Is.True);
        }

        [Test]
        public void LogLevel_ValueUnchanged_PropertyChangedNotFired()
        {
            Boolean fired = false;
            LoggerSettingsDummyClass instance = new LoggerSettingsDummyClass();

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
            LoggerSettingsDummyClass instance = new LoggerSettingsDummyClass();

            instance.LogType = LogType.Csv;
            instance.PropertyChanged += (sender, args) => { fired = true; };
            instance.LogType = LogType.Raw;

            Assert.That(fired, Is.True);
        }

        [Test]
        public void LogType_ValueUnchanged_PropertyChangedNotFired()
        {
            Boolean fired = false;
            LoggerSettingsDummyClass instance = new LoggerSettingsDummyClass();

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
            LoggerSettingsDummyClass instance = new LoggerSettingsDummyClass();

            instance.LogTime = LogTime.Local;
            instance.PropertyChanged += (sender, args) => { fired = true; };
            instance.LogTime = LogTime.Utc;

            Assert.That(fired, Is.True);
        }

        [Test]
        public void LogTime_ValueUnchanged_PropertyChangedNotFired()
        {
            Boolean fired = false;
            LoggerSettingsDummyClass instance = new LoggerSettingsDummyClass();

            instance.LogTime = LogTime.Default;
            instance.PropertyChanged += (sender, args) => { fired = true; };
            instance.LogTime = LogTime.Default;

            Assert.That(fired, Is.False);
        }

        #endregion

        #region ShowTime

        [Test]
        public void ShowTime_ValueChanged_PropertyChangedFired()
        {
            Boolean fired = false;
            LoggerSettingsDummyClass instance = new LoggerSettingsDummyClass();

            instance.ShowTime = false;
            instance.PropertyChanged += (sender, args) => { fired = true; };
            instance.ShowTime = true;

            Assert.That(fired, Is.True);
        }

        [Test]
        public void ShowTime_ValueUnchanged_PropertyChangedNotFired()
        {
            Boolean fired = false;
            LoggerSettingsDummyClass instance = new LoggerSettingsDummyClass();

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
            LoggerSettingsDummyClass instance = new LoggerSettingsDummyClass();

            instance.TimeFormat = "old time format";
            instance.PropertyChanged += (sender, args) => { fired = true; };
            instance.TimeFormat = "new time format";

            Assert.That(fired, Is.True);
        }

        [Test]
        public void TimeFormat_ValueUnchanged_PropertyChangedNotFired()
        {
            Boolean fired = false;
            LoggerSettingsDummyClass instance = new LoggerSettingsDummyClass();

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
            LoggerSettingsDummyClass instance = new LoggerSettingsDummyClass();

            instance.PartSplit = '#';
            instance.PropertyChanged += (sender, args) => { fired = true; };
            instance.PartSplit = '@';

            Assert.That(fired, Is.True);
        }

        [Test]
        public void PartSplit_ValueUnchanged_PropertyChangedNotFired()
        {
            Boolean fired = false;
            LoggerSettingsDummyClass instance = new LoggerSettingsDummyClass();

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
            LoggerSettingsDummyClass instance = new LoggerSettingsDummyClass();

            instance.FullName = false;
            instance.PropertyChanged += (sender, args) => { fired = true; };
            instance.FullName = true;

            Assert.That(fired, Is.True);
        }

        [Test]
        public void FullName_ValueUnchanged_PropertyChangedNotFired()
        {
            Boolean fired = false;
            LoggerSettingsDummyClass instance = new LoggerSettingsDummyClass();

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
            LoggerSettingsDummyClass instance = new LoggerSettingsDummyClass();

            instance.Culture = CultureInfo.GetCultureInfo("en-US");
            instance.PropertyChanged += (sender, args) => { fired = true; };
            instance.Culture = CultureInfo.GetCultureInfo("de-DE");

            Assert.That(fired, Is.True);
        }

        [Test]
        public void Culture_ValueUnchanged_PropertyChangedNotFired()
        {
            Boolean fired = false;
            LoggerSettingsDummyClass instance = new LoggerSettingsDummyClass();

            instance.Culture = CultureInfo.GetCultureInfo("en-US");
            instance.PropertyChanged += (sender, args) => { fired = true; };
            instance.Culture = CultureInfo.GetCultureInfo("en-US");

            Assert.That(fired, Is.False);
        }

        #endregion

        #region PropertyChanged

        [Test]
        [TestCase(null)]
        [TestCase("")]
        [TestCase("  ")]
        public void PropertyChanged_InvalidPropertyName_ThrowsArgumentOutOfRangeException(String property)
        {
            LoggerSettingsDummyClass instance = new LoggerSettingsDummyClass();

            Assert.That(() => instance.TestRaisePropertyChanged(property), Throws.InstanceOf<ArgumentOutOfRangeException>());
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
        }

        #endregion
    }
}
