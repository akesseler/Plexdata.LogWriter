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
using System.Diagnostics.CodeAnalysis;

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

        #region Private test class implementations

        private class LoggerSettingsDummyClass : LoggerSettings
        {
            public LoggerSettingsDummyClass() : base() { }
        }

        #endregion
    }
}
