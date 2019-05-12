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

using Moq;
using NUnit.Framework;
using Plexdata.LogWriter.Abstraction;
using Plexdata.LogWriter.Settings;
using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text;

namespace Plexdata.LogWriter.Persistent.Tests.Settings
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    [Category("IntegrationTest")]
    [TestOf(nameof(PersistentLoggerSettings))]
    public class PersistentLoggerSettingsTests
    {
        #region Prologue

        private Mock<ILoggerSettingsSection> section;
        private Mock<ILoggerSettingsSection> configuration;
        private String testingFilename = null;
        private String defaultFilename = null;

        [SetUp]
        public void Setup()
        {
            this.testingFilename = Path.GetTempFileName();
            this.defaultFilename = Path.Combine(Path.GetTempPath(), "plexdata.log");

            this.section = new Mock<ILoggerSettingsSection>();
            this.configuration = new Mock<ILoggerSettingsSection>();

            this.configuration
                .Setup(x => x.GetSection(It.IsAny<String>()))
                .Returns(this.section.Object);
        }

        [TearDown]
        public void Cleanup()
        {
            this.Sweep(this.testingFilename);
            this.testingFilename = null;

            this.Sweep(this.defaultFilename);
            this.defaultFilename = null;
        }

        #endregion

        #region Construction

        [Test]
        public void PersistentLoggerSettings_ValidateDefaultSettings_DefaultSettingsAsExpected()
        {
            PersistentLoggerSettings instance = new PersistentLoggerSettings();

            Assert.That(instance.Filename, Is.EqualTo(this.defaultFilename));
            Assert.That(instance.IsRolling, Is.False);
            Assert.That(instance.IsQueuing, Is.False);
            Assert.That(instance.Threshold, Is.EqualTo(-1));
            Assert.That(instance.Encoding, Is.EqualTo(Encoding.UTF8));
        }

        [Test]
        public void PersistentLoggerSettings_ValidateDefaultSettingsForInvalidConfiguration_DefaultSettingsAsExpected()
        {
            PersistentLoggerSettings instance = new PersistentLoggerSettings(null);

            Assert.That(instance.Filename, Is.EqualTo(this.defaultFilename));
            Assert.That(instance.IsRolling, Is.False);
            Assert.That(instance.IsQueuing, Is.False);
            Assert.That(instance.Threshold, Is.EqualTo(-1));
            Assert.That(instance.Encoding, Is.EqualTo(Encoding.UTF8));
        }

        [Test]
        public void PersistentLoggerSettings_ConfigurationValid_GetSectionCalledAsExpected()
        {
            PersistentLoggerSettings instance = new PersistentLoggerSettings(this.configuration.Object);

            this.configuration.Verify(x => x.GetSection("Plexdata:LogWriter:Settings"), Times.Exactly(2));
        }

        [Test]
        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        public void PersistentLoggerSettings_ConfigurationValidButFilenameEmpty_GetSectionValueForFilenameIsDefault(String value)
        {
            this.section.SetupGet(x => x["Filename"]).Returns(value);

            PersistentLoggerSettings instance = new PersistentLoggerSettings(this.configuration.Object);

            Assert.That(instance.Filename, Is.EqualTo(this.defaultFilename));
        }

        [Test]
        public void PersistentLoggerSettings_ConfigurationValidAndFilenameValid_GetSectionValueForFilenameAsExpected()
        {
            this.section.SetupGet(x => x["Filename"]).Returns(this.testingFilename);

            PersistentLoggerSettings instance = new PersistentLoggerSettings(this.configuration.Object);

            Assert.That(instance.Filename, Is.EqualTo(this.testingFilename));
        }

        [Test]
        [TestCase(null, false)]
        [TestCase("", false)]
        [TestCase(" ", false)]
        [TestCase("invalid", false)]
        [TestCase("true", true)]
        [TestCase("TRUE", true)]
        [TestCase("True", true)]
        [TestCase("false", false)]
        [TestCase("FALSE", false)]
        [TestCase("False", false)]
        public void PersistentLoggerSettings_ConfigurationValid_GetSectionValueForIsRollingAsExpected(String value, Object expected)
        {
            this.section.SetupGet(x => x["IsRolling"]).Returns(value);

            PersistentLoggerSettings instance = new PersistentLoggerSettings(this.configuration.Object);

            Assert.That(instance.IsRolling, Is.EqualTo(expected));
        }

        [Test]
        [TestCase(null, false)]
        [TestCase("", false)]
        [TestCase(" ", false)]
        [TestCase("invalid", false)]
        [TestCase("true", true)]
        [TestCase("TRUE", true)]
        [TestCase("True", true)]
        [TestCase("false", false)]
        [TestCase("FALSE", false)]
        [TestCase("False", false)]
        public void PersistentLoggerSettings_ConfigurationValid_GetSectionValueForIsQueuingAsExpected(String value, Object expected)
        {
            this.section.SetupGet(x => x["IsQueuing"]).Returns(value);

            PersistentLoggerSettings instance = new PersistentLoggerSettings(this.configuration.Object);

            Assert.That(instance.IsQueuing, Is.EqualTo(expected));
        }

        [Test]
        [TestCase(null, -1)]
        [TestCase("", -1)]
        [TestCase(" ", -1)]
        [TestCase("invalid", -1)]
        [TestCase("-1", -1)]
        [TestCase("0", 0)]
        [TestCase("42", 42)]
        [TestCase("42.0", -1)]
        public void PersistentLoggerSettings_ConfigurationValid_GetSectionValueForThresholdAsExpected(String value, Object expected)
        {
            this.section.SetupGet(x => x["Threshold"]).Returns(value);

            PersistentLoggerSettings instance = new PersistentLoggerSettings(this.configuration.Object);

            Assert.That(instance.Threshold, Is.EqualTo(expected));
        }

        [Test]
        [TestCaseSource(nameof(EncodingTestItemList))]
        public void PersistentLoggerSettings_ConfigurationValid_GetSectionValueForEncodingAsExpected(Object current   /*String value, Object expected*/)
        {
            EncodingTestItem nominee = (EncodingTestItem)current;

            this.section.SetupGet(x => x["Encoding"]).Returns(nominee.Value);

            PersistentLoggerSettings instance = new PersistentLoggerSettings(this.configuration.Object);

            Assert.That(instance.Encoding.BodyName, Is.EqualTo(nominee.Result.BodyName));
        }

        #endregion

        #region Filename

        [Test]
        public void Filename_ValueIsNull_EnsureFullPathAndWriteAccessOrThrowDoesThrowArgumentOutOfRangeException()
        {
            PersistentLoggerSettings instance = new PersistentLoggerSettings();

            Assert.That(() => instance.Filename = null, Throws.InstanceOf<ArgumentOutOfRangeException>());
        }

        [Test]
        public void Filename_ValueChanged_PropertyChangedFired()
        {
            Boolean fired = false;
            PersistentLoggerSettings instance = new PersistentLoggerSettings();

            instance.Filename = this.testingFilename;
            instance.PropertyChanged += (sender, args) => { fired = true; };
            instance.Filename = Path.GetTempFileName();

            Assert.That(fired, Is.True);

            this.Sweep(instance.Filename);
        }

        [Test]
        public void Filename_ValueUnchanged_PropertyChangedNotFired()
        {
            Boolean fired = false;
            PersistentLoggerSettings instance = new PersistentLoggerSettings();

            instance.Filename = this.testingFilename;
            instance.PropertyChanged += (sender, args) => { fired = true; };
            instance.Filename = this.testingFilename;

            Assert.That(fired, Is.False);
        }

        #endregion

        #region IsRolling

        [Test]
        public void IsRolling_ValueChanged_PropertyChangedFired()
        {
            Boolean fired = false;
            PersistentLoggerSettings instance = new PersistentLoggerSettings();

            instance.Filename = this.testingFilename;
            instance.IsRolling = false;
            instance.PropertyChanged += (sender, args) => { fired = true; };
            instance.IsRolling = true;

            Assert.That(fired, Is.True);
        }

        [Test]
        public void IsRolling_ValueUnchanged_PropertyChangedNotFired()
        {
            Boolean fired = false;
            PersistentLoggerSettings instance = new PersistentLoggerSettings();

            instance.Filename = this.testingFilename;
            instance.IsRolling = true;
            instance.PropertyChanged += (sender, args) => { fired = true; };
            instance.IsRolling = true;

            Assert.That(fired, Is.False);
        }

        #endregion

        #region IsQueuing

        [Test]
        public void IsQueuing_ValueChanged_PropertyChangedFired()
        {
            Boolean fired = false;
            PersistentLoggerSettings instance = new PersistentLoggerSettings();

            instance.Filename = this.testingFilename;
            instance.IsQueuing = false;
            instance.PropertyChanged += (sender, args) => { fired = true; };
            instance.IsQueuing = true;

            Assert.That(fired, Is.True);
        }

        [Test]
        public void IsQueuing_ValueUnchanged_PropertyChangedNotFired()
        {
            Boolean fired = false;
            PersistentLoggerSettings instance = new PersistentLoggerSettings();

            instance.Filename = this.testingFilename;
            instance.IsQueuing = true;
            instance.PropertyChanged += (sender, args) => { fired = true; };
            instance.IsQueuing = true;

            Assert.That(fired, Is.False);
        }

        #endregion

        #region Threshold

        [Test]
        public void Threshold_ValueChanged_PropertyChangedFired()
        {
            Boolean fired = false;
            PersistentLoggerSettings instance = new PersistentLoggerSettings();

            instance.Filename = this.testingFilename;
            instance.Threshold = 42;
            instance.PropertyChanged += (sender, args) => { fired = true; };
            instance.Threshold = 23;

            Assert.That(fired, Is.True);
        }

        [Test]
        public void Threshold_ValueUnchanged_PropertyChangedNotFired()
        {
            Boolean fired = false;
            PersistentLoggerSettings instance = new PersistentLoggerSettings();

            instance.Filename = this.testingFilename;
            instance.Threshold = 42;
            instance.PropertyChanged += (sender, args) => { fired = true; };
            instance.Threshold = 42;

            Assert.That(fired, Is.False);
        }

        #endregion

        #region Encoding

        [Test]
        public void Encoding_ValueChanged_PropertyChangedFired()
        {
            Boolean fired = false;
            PersistentLoggerSettings instance = new PersistentLoggerSettings();

            instance.Filename = this.testingFilename;
            instance.Encoding = Encoding.UTF7;
            instance.PropertyChanged += (sender, args) => { fired = true; };
            instance.Encoding = Encoding.UTF32;

            Assert.That(fired, Is.True);
        }

        [Test]
        public void Encoding_ValueUnchanged_PropertyChangedNotFired()
        {
            Boolean fired = false;
            PersistentLoggerSettings instance = new PersistentLoggerSettings();

            instance.Filename = this.testingFilename;
            instance.Encoding = Encoding.ASCII;
            instance.PropertyChanged += (sender, args) => { fired = true; };
            instance.Encoding = Encoding.ASCII;

            Assert.That(fired, Is.False);
        }

        #endregion

        #region Private methods

        private void Sweep(String filename)
        {
            if (File.Exists(filename))
            {
                File.Delete(filename);
            }
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
