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
using Plexdata.LogWriter.Abstraction;
using Plexdata.LogWriter.Definitions;
using Plexdata.LogWriter.Internals.Extensions;
using Plexdata.Utilities.Testing;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Threading;

namespace Plexdata.LogWriter.Persistent.Tests.Internals.Extensions
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    [Category(TestType.IntegrationTest)]
    [TestOf(nameof(SettingsPoliciesExtension))]
    public class SettingsPoliciesExtensionTests
    {
        #region Prologue

        private const String BaseName = "filename";
        private const String BaseExt = ".ext";

        private String filepath = String.Empty;
        private String fullname = String.Empty;

        [SetUp]
        public void Setup()
        {
            this.filepath = Path.Combine(Path.GetTempPath(), "SettingsPoliciesExtensionTests");
            this.fullname = Path.Combine(this.filepath, $"{SettingsPoliciesExtensionTests.BaseName}{SettingsPoliciesExtensionTests.BaseExt}");

            if (!Directory.Exists(this.filepath))
            {
                Directory.CreateDirectory(this.filepath);
            }
        }

        [TearDown]
        public void Cleanup()
        {
            if (Directory.Exists(this.filepath))
            {
                Directory.Delete(this.filepath, true);
            }
        }

        #endregion

        #region General tests

        [Test]
        public void SettingsPoliciesExtension_ValidateDefaultThreshold_DefaultThresholdIsFiveMegabytes()
        {
            Assert.That(SettingsPoliciesExtension.DefaultThreshold, Is.EqualTo(5 * 1024 * 1024));
        }

        [Test]
        public void GetCurrentFilename_SettingsInstanceIsNull_ThrowsArgumentNullException()
        {
            IPersistentLoggerSettings settings = null;
            Assert.That(() => settings.GetCurrentFilename(out Boolean dispose), Throws.ArgumentNullException);
        }

        [Test]
        public void GetCurrentFilename_SettingsInstanceIsValid_IsRollingWasCalledOnce()
        {
            Mock<IPersistentLoggerSettings> settings = this.GetSettings(false, -1, LogTime.Utc, this.fullname);

            settings.Object.GetCurrentFilename(out Boolean dispose);

            settings.VerifyGet(x => x.IsRolling, Times.Once);
        }

        #endregion

        #region Default filename tests

        [TestCase(0)]
        [TestCase(-1)]
        public void GetCurrentFilename_RollingOffThresholdZero_ResultIsSameFilename(Int32 threshold)
        {
            Mock<IPersistentLoggerSettings> settings = this.GetSettings(false, threshold, LogTime.Utc, this.fullname);

            Assert.That(settings.Object.GetCurrentFilename(out Boolean dispose), Is.EqualTo(this.fullname));
        }

        [TestCase(LogTime.Utc)]
        [TestCase(LogTime.Local)]
        public void GetCurrentFilename_RollingOffThresholdNotZeroNoFileExist_ResultIsNewFilename(LogTime kind)
        {
            Mock<IPersistentLoggerSettings> settings = this.GetSettings(false, 1, kind, this.fullname);

            String expected = this.GetNextThresholdFilename(kind);

            // Hits part [info==null] of GetStandardFilename().
            String actual = settings.Object.GetCurrentFilename(out Boolean dispose);

            Assert.That(actual, Is.GreaterThanOrEqualTo(expected));
        }

        [TestCase(LogTime.Utc, 1024)]
        [TestCase(LogTime.Local, 1024)]
        [TestCase(LogTime.Utc, 1025)]
        [TestCase(LogTime.Local, 1025)]
        public void GetCurrentFilename_RollingOffThresholdNotZeroFileExistsAndIsFull_ResultIsNewFilename(LogTime kind, Int32 size)
        {
            Mock<IPersistentLoggerSettings> settings = this.GetSettings(false, 1, kind, this.fullname);

            String expected = this.GetNextThresholdFilename(kind);

            this.CreateTestFiles(1024, expected);

            // Hits part [info.Length>=settings...] of GetStandardFilename().
            String actual = settings.Object.GetCurrentFilename(out Boolean dispose);

            Assert.That(actual, Is.GreaterThanOrEqualTo(expected));
        }

        [TestCase(LogTime.Utc, 0)]
        [TestCase(LogTime.Local, 0)]
        [TestCase(LogTime.Utc, 100)]
        [TestCase(LogTime.Local, 100)]
        [TestCase(LogTime.Utc, 1023)]
        [TestCase(LogTime.Local, 1023)]
        public void GetCurrentFilename_RollingOffThresholdNotZeroFileExistsAndIsNotFull_ResultIsOldFilename(LogTime kind, Int32 size)
        {
            Mock<IPersistentLoggerSettings> settings = this.GetSettings(false, 1, kind, this.fullname);

            String expected = this.GetNextThresholdFilename(kind);

            this.CreateTestFiles(size, expected);

            // Hits ELSE part at [info==null] of GetStandardFilename().
            String actual = settings.Object.GetCurrentFilename(out Boolean dispose);

            Assert.That(actual, Is.EqualTo(expected));
        }

        [TestCase(LogTime.Utc, 100)]
        [TestCase(LogTime.Local, 100)]
        public void GetCurrentFilename_RollingOffThresholdNotZeroMultiFileExistAndNotFull_ResultIsOldFilename(LogTime kind, Int32 size)
        {
            Mock<IPersistentLoggerSettings> settings = this.GetSettings(false, 1, kind, this.fullname);

            DateTime date = DateTime.Now.AddHours(-1);
            String prefix = "Hello";
            String suffix = "World";

            List<String> filenames = new List<String>();
            filenames.Add(this.GetSomeThresholdFilename(kind, date, String.Empty, String.Empty));
            filenames.Add(this.GetSomeThresholdFilename(kind, date, prefix, String.Empty));
            filenames.Add(this.GetSomeThresholdFilename(kind, date, String.Empty, suffix));
            filenames.Add(this.GetSomeThresholdFilename(kind, date, prefix, suffix));

            for (Int32 index = 0; index < 5; index++)
            {
                filenames.Add(this.GetSomeThresholdFilename(kind, date.AddMinutes(-index), String.Empty, String.Empty));
                filenames.Add(this.GetSomeThresholdFilename(kind, date.AddMinutes(-index), prefix, String.Empty));
                filenames.Add(this.GetSomeThresholdFilename(kind, date.AddMinutes(-index), String.Empty, suffix));
                filenames.Add(this.GetSomeThresholdFilename(kind, date.AddMinutes(-index), prefix, suffix));
            }

            String expected = this.GetSomeThresholdFilename(kind, date, String.Empty, String.Empty);

            this.CreateTestFiles(size, filenames.Distinct().ToArray());

            // Hits ELSE part at [info==null] of GetStandardFilename().
            String actual = settings.Object.GetCurrentFilename(out Boolean dispose);

            Assert.That(actual, Is.EqualTo(expected));
        }

        #endregion

        #region Rolling filename tests

        [Test]
        public void GetCurrentFilename_RollingOnNoFileExist_ResultIsFilenameOne()
        {
            Mock<IPersistentLoggerSettings> settings = this.GetSettings(true, 1, LogTime.Utc, this.fullname);

            String fileOne = Path.Combine(this.filepath, $"{SettingsPoliciesExtensionTests.BaseName}_one{SettingsPoliciesExtensionTests.BaseExt}");
            String fileTwo = Path.Combine(this.filepath, $"{SettingsPoliciesExtensionTests.BaseName}_two{SettingsPoliciesExtensionTests.BaseExt}");

            Assert.That(settings.Object.GetCurrentFilename(out Boolean dispose), Is.EqualTo(fileOne));
            Assert.That(dispose, Is.False);
        }

        [Test]
        public void GetCurrentFilename_RollingOnFileOneExistsFileIsLess_ResultIsFilenameOne()
        {
            Mock<IPersistentLoggerSettings> settings = this.GetSettings(true, 1, LogTime.Utc, this.fullname);

            String fileOne = Path.Combine(this.filepath, $"{SettingsPoliciesExtensionTests.BaseName}_one{SettingsPoliciesExtensionTests.BaseExt}");
            String fileTwo = Path.Combine(this.filepath, $"{SettingsPoliciesExtensionTests.BaseName}_two{SettingsPoliciesExtensionTests.BaseExt}");

            this.CreateTestFiles(100, fileOne);

            Assert.That(settings.Object.GetCurrentFilename(out Boolean dispose), Is.EqualTo(fileOne));
            Assert.That(dispose, Is.False);
        }

        [Test]
        public void GetCurrentFilename_RollingOnFileOneExistsFileIsFull_ResultIsFilenameTwo()
        {
            Mock<IPersistentLoggerSettings> settings = this.GetSettings(true, 1, LogTime.Utc, this.fullname);

            String fileOne = Path.Combine(this.filepath, $"{SettingsPoliciesExtensionTests.BaseName}_one{SettingsPoliciesExtensionTests.BaseExt}");
            String fileTwo = Path.Combine(this.filepath, $"{SettingsPoliciesExtensionTests.BaseName}_two{SettingsPoliciesExtensionTests.BaseExt}");

            this.CreateTestFiles(1024, fileOne);

            Assert.That(settings.Object.GetCurrentFilename(out Boolean dispose), Is.EqualTo(fileTwo));
            Assert.That(dispose, Is.False);
        }

        [Test]
        public void GetCurrentFilename_RollingOnFileTwoExistsFileIsLess_ResultIsFilenameTwo()
        {
            Mock<IPersistentLoggerSettings> settings = this.GetSettings(true, 1, LogTime.Utc, this.fullname);

            String fileOne = Path.Combine(this.filepath, $"{SettingsPoliciesExtensionTests.BaseName}_one{SettingsPoliciesExtensionTests.BaseExt}");
            String fileTwo = Path.Combine(this.filepath, $"{SettingsPoliciesExtensionTests.BaseName}_two{SettingsPoliciesExtensionTests.BaseExt}");

            this.CreateTestFiles(100, fileTwo);

            Assert.That(settings.Object.GetCurrentFilename(out Boolean dispose), Is.EqualTo(fileTwo));
            Assert.That(dispose, Is.False);
        }

        [Test]
        public void GetCurrentFilename_RollingOnFileTwoExistsFileIsFull_ResultIsFilenameOne()
        {
            Mock<IPersistentLoggerSettings> settings = this.GetSettings(true, 1, LogTime.Utc, this.fullname);

            String fileOne = Path.Combine(this.filepath, $"{SettingsPoliciesExtensionTests.BaseName}_one{SettingsPoliciesExtensionTests.BaseExt}");
            String fileTwo = Path.Combine(this.filepath, $"{SettingsPoliciesExtensionTests.BaseName}_two{SettingsPoliciesExtensionTests.BaseExt}");

            this.CreateTestFiles(1024, fileTwo);

            Assert.That(settings.Object.GetCurrentFilename(out Boolean dispose), Is.EqualTo(fileOne));
            Assert.That(dispose, Is.False);
        }

        [Test]
        public void GetCurrentFilename_RollingOnBothFilesExistFileOneIsLessFileTwoIsFull_ResultIsFilenameOne()
        {
            Mock<IPersistentLoggerSettings> settings = this.GetSettings(true, 1, LogTime.Utc, this.fullname);

            String fileOne = Path.Combine(this.filepath, $"{SettingsPoliciesExtensionTests.BaseName}_one{SettingsPoliciesExtensionTests.BaseExt}");
            String fileTwo = Path.Combine(this.filepath, $"{SettingsPoliciesExtensionTests.BaseName}_two{SettingsPoliciesExtensionTests.BaseExt}");

            this.CreateTestFiles(100, fileOne);
            this.CreateTestFiles(1024, fileTwo);

            Assert.That(settings.Object.GetCurrentFilename(out Boolean dispose), Is.EqualTo(fileOne));
            Assert.That(dispose, Is.False);
        }

        [Test]
        public void GetCurrentFilename_RollingOnBothFilesExistFileOneIsFullFileTwoIsLess_ResultIsFilenameTwo()
        {
            Mock<IPersistentLoggerSettings> settings = this.GetSettings(true, 1, LogTime.Utc, this.fullname);

            String fileOne = Path.Combine(this.filepath, $"{SettingsPoliciesExtensionTests.BaseName}_one{SettingsPoliciesExtensionTests.BaseExt}");
            String fileTwo = Path.Combine(this.filepath, $"{SettingsPoliciesExtensionTests.BaseName}_two{SettingsPoliciesExtensionTests.BaseExt}");

            this.CreateTestFiles(1024, fileOne);
            this.CreateTestFiles(100, fileTwo);

            Assert.That(settings.Object.GetCurrentFilename(out Boolean dispose), Is.EqualTo(fileTwo));
            Assert.That(dispose, Is.False);
        }

        [Test]
        public void GetCurrentFilename_RollingOnBothFilesExistBothAreFullFileOneIsOlder_ResultIsFilenameOne()
        {
            Mock<IPersistentLoggerSettings> settings = this.GetSettings(true, 1, LogTime.Utc, this.fullname);

            String fileOne = Path.Combine(this.filepath, $"{SettingsPoliciesExtensionTests.BaseName}_one{SettingsPoliciesExtensionTests.BaseExt}");
            String fileTwo = Path.Combine(this.filepath, $"{SettingsPoliciesExtensionTests.BaseName}_two{SettingsPoliciesExtensionTests.BaseExt}");

            this.CreateTestFiles(1024, fileOne);
            Thread.Sleep(2000);
            this.CreateTestFiles(1024, fileTwo);

            Assert.That(settings.Object.GetCurrentFilename(out Boolean dispose), Is.EqualTo(fileOne));
            Assert.That(dispose, Is.True);
        }

        [Test]
        public void GetCurrentFilename_RollingOnBothFilesExistBothAreFullFileTwoIsOlder_ResultIsFilenameTwo()
        {
            Mock<IPersistentLoggerSettings> settings = this.GetSettings(true, 1, LogTime.Utc, this.fullname);

            String fileOne = Path.Combine(this.filepath, $"{SettingsPoliciesExtensionTests.BaseName}_one{SettingsPoliciesExtensionTests.BaseExt}");
            String fileTwo = Path.Combine(this.filepath, $"{SettingsPoliciesExtensionTests.BaseName}_two{SettingsPoliciesExtensionTests.BaseExt}");

            this.CreateTestFiles(1024, fileTwo);
            Thread.Sleep(2000);
            this.CreateTestFiles(1024, fileOne);

            Assert.That(settings.Object.GetCurrentFilename(out Boolean dispose), Is.EqualTo(fileTwo));
            Assert.That(dispose, Is.True);
        }

        #endregion

        #region Private settings helpers

        private void CreateTestFiles(Int32 length, params String[] filenames)
        {
            if (filenames == null) { return; }

            foreach (String filename in filenames)
            {
                File.AppendAllText(filename, String.Empty.PadLeft(length, '#'));
            }
        }

        private String GetNextThresholdFilename(LogTime kind)
        {
            return this.GetSomeThresholdFilename(kind, DateTime.Now, String.Empty, String.Empty);
        }

        private String GetSomeThresholdFilename(LogTime kind, DateTime date, String prefix, String suffix)
        {
            if (kind == LogTime.Utc)
            {
                date = date.ToUniversalTime();
            }
            else
            {
                date = date.ToLocalTime();
            }

            return Path.Combine(this.filepath, $"{SettingsPoliciesExtensionTests.BaseName}_{prefix}{date.ToString("yyyyMMddHHmmss")}{suffix}{SettingsPoliciesExtensionTests.BaseExt}");
        }

        private Mock<IPersistentLoggerSettings> GetSettings(Boolean rolling, Int32 threshold, LogTime kind, String filename)
        {
            Mock<IPersistentLoggerSettings> settings = new Mock<IPersistentLoggerSettings>();

            settings.SetupGet(x => x.IsRolling).Returns(rolling);
            settings.SetupGet(x => x.Threshold).Returns(threshold);
            settings.SetupGet(x => x.Filename).Returns(filename);
            settings.SetupGet(x => x.LogTime).Returns(kind);

            return settings;
        }

        #endregion
    }
}
