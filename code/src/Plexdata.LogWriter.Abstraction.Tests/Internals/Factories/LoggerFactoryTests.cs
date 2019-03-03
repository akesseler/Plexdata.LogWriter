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
using Plexdata.LogWriter.Internals.Events;
using Plexdata.LogWriter.Internals.Factories;
using Plexdata.LogWriter.Internals.Formatters;
using Plexdata.LogWriter.Settings;
using System;
using System.Diagnostics.CodeAnalysis;

namespace Plexdata.LogWriter.Abstraction.Tests.Internals.Factories
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    [TestOf(nameof(LoggerFactory))]
    public class LoggerFactoryTests
    {
        #region Prologue

        private ILoggerFactory instance = null;
        private ILoggerSettings settings = null;

        [SetUp]
        public void Setup()
        {
            this.instance = new LoggerFactory();
            this.settings = new TestSettings();
        }

        #endregion

        #region CreateLogEvent

        [Test]
        public void LoggerFactory_CreateLogEvent_InstanceOfLogEventIsCreated()
        {
            Assert.That(this.instance.CreateLogEvent(LogLevel.Disabled, DateTime.Now, "context", "scope", "message", null, null), Is.InstanceOf<LogEvent>());
        }

        #endregion

        #region CreateLogEventFormatter

        [Test]
        public void LoggerFactory_CreateLogEventFormatterSettingsAreNull_ThrowsArgumentNullException()
        {
            Assert.That(() => this.instance.CreateLogEventFormatter(null), Throws.ArgumentNullException);
        }

        [Test]
        public void LoggerFactory_CreateLogEventFormatterForDefaultType_FormatterTypeIsDefault()
        {
            this.settings.LogType = LogType.Default;
            Assert.That(this.instance.CreateLogEventFormatter(this.settings), Is.InstanceOf(typeof(RawFormatter)));
        }

        [Test]
        [TestCase(LogType.Raw, typeof(RawFormatter))]
        [TestCase(LogType.Csv, typeof(CsvFormatter))]
        [TestCase(LogType.Json, typeof(JsonFormatter))]
        public void LoggerFactory_CreateLogEventFormatterForSpecialType_FormatterTypeAsExpected(LogType type, Type expected)
        {
            this.settings.LogType = type;
            Assert.That(this.instance.CreateLogEventFormatter(this.settings), Is.InstanceOf(expected));
        }

        #endregion

        #region Private test class implementations

        private class TestSettings : LoggerSettings
        {
            public TestSettings()
                : base()
            {
            }
        }

        #endregion
    }
}
