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
using Plexdata.LogWriter.Internals.Settings;
using Plexdata.LogWriter.Settings;
using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace Plexdata.LogWriter.Abstraction.Tests.Settings
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    [Category("IntegrationTest")]
    [TestOf(nameof(LoggerSettingsBuilder))]
    public class LoggerSettingsBuilderTests
    {
        #region SetFilename

        [Test]
        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        [TestCase("file-does-not-exists.json")]
        [TestCase("z:\\path-does-not-exists\\file-does-not-exists.json")]
        public void SetFilename_InvalidArguments_PropertiesUseDefaultValues(String filename)
        {
            LoggerSettingsBuilder instance = new LoggerSettingsBuilder();

            instance.SetFilename(filename);

            Assert.That(instance.FileName, Is.Empty);
            Assert.That(instance.FileType, Is.EqualTo("Unknown"));
        }

        [Test]
        public void SetFilename_FormatFromExtensionUnsupported_PropertiesUseDefaultValues()
        {
            String filename = Path.Combine(Path.GetTempPath(), "some-file-name.txt");

            try
            {
                using (StreamWriter writer = File.CreateText(filename)) { }

                LoggerSettingsBuilder instance = new LoggerSettingsBuilder();

                instance.SetFilename(filename);

                Assert.That(instance.FileName, Is.Empty);
                Assert.That(instance.FileType, Is.EqualTo("Unknown"));
            }
            finally
            {
                if (File.Exists(filename))
                {
                    File.Delete(filename);
                }
            }
        }

        [Test]
        [TestCase("some-file-name.xml", "Xml")]
        [TestCase("some-file-name.json", "Json")]
        public void SetFilename_FormatFromExtensionSupported_PropertiesUseAppliedValues(String filename, String filetype)
        {
            String fullname = Path.Combine(Path.GetTempPath(), filename);

            try
            {
                using (StreamWriter writer = File.CreateText(fullname)) { }

                LoggerSettingsBuilder instance = new LoggerSettingsBuilder();

                instance.SetFilename(fullname);

                Assert.That(instance.FileName, Is.EqualTo(fullname));
                Assert.That(instance.FileType, Is.EqualTo(filetype));
            }
            finally
            {
                if (File.Exists(fullname))
                {
                    File.Delete(fullname);
                }
            }
        }

        [Test]
        public void SetFilename_FormatManualAppliedUnsupported_PropertiesUseDefaultValues()
        {
            String filename = Path.Combine(Path.GetTempPath(), "some-file-name.json");

            try
            {
                using (StreamWriter writer = File.CreateText(filename)) { }

                LoggerSettingsBuilder instance = new LoggerSettingsBuilder();

                instance.SetFilename(filename, "txt");

                Assert.That(instance.FileName, Is.Empty);
                Assert.That(instance.FileType, Is.EqualTo("Unknown"));
            }
            finally
            {
                if (File.Exists(filename))
                {
                    File.Delete(filename);
                }
            }
        }

        [Test]
        [TestCase("some-file-name.xyz", "Xml")]
        [TestCase("some-file-name.xyz", "Json")]
        public void SetFilename_FormatManualAppliedSupported_PropertiesUseAppliedValues(String filename, String filetype)
        {
            String fullname = Path.Combine(Path.GetTempPath(), filename);

            try
            {
                using (StreamWriter writer = File.CreateText(fullname)) { }

                LoggerSettingsBuilder instance = new LoggerSettingsBuilder();

                instance.SetFilename(fullname, filetype);

                Assert.That(instance.FileName, Is.EqualTo(fullname));
                Assert.That(instance.FileType, Is.EqualTo(filetype));
            }
            finally
            {
                if (File.Exists(fullname))
                {
                    File.Delete(fullname);
                }
            }
        }

        #endregion

        #region Build

        [Test]
        public void Build_FileNameInvalid_ResultIsLoggerSettingsEmpty()
        {
            LoggerSettingsBuilder instance = new LoggerSettingsBuilder();

            Assert.That(instance.Build(), Is.InstanceOf<LoggerSettingsEmpty>());
        }

        [Test]
        public void Build_FileTypeInvalid_ResultIsLoggerSettingsEmpty()
        {
            LoggerSettingsBuilder instance = new LoggerSettingsBuilder("some-file-name.txt");

            Assert.That(instance.Build(), Is.InstanceOf<LoggerSettingsEmpty>());
        }

        [Test]
        [TestCase("xml", typeof(LoggerSettingsXml))]
        [TestCase("json", typeof(LoggerSettingsJson))]
        public void Build_FileNameAndTypeValid_ResultIsLoggerSettingsTypeAsExpected(String filetype, Type expected)
        {
            String fullname = Path.Combine(Path.GetTempPath(), "some-file-name.txt");

            try
            {
                using (StreamWriter writer = File.CreateText(fullname)) { }

                LoggerSettingsBuilder instance = new LoggerSettingsBuilder(fullname, filetype);

                Assert.That(instance.Build(), Is.InstanceOf(expected));
            }
            finally
            {
                if (File.Exists(fullname))
                {
                    File.Delete(fullname);
                }
            }
        }

        [Test]
        public void Build_FileNameAndTypeValidButLocked_ResultIsLoggerSettingsEmpty()
        {
            String fullname = Path.Combine(Path.GetTempPath(), "some-file-name.txt");

            try
            {
                using (StreamWriter writer = File.CreateText(fullname))
                {
                    writer.WriteLine("access file for write");

                    LoggerSettingsBuilder instance = new LoggerSettingsBuilder(fullname, "json");

                    Assert.That(instance.Build(), Is.InstanceOf<LoggerSettingsEmpty>());
                }
            }
            finally
            {
                if (File.Exists(fullname))
                {
                    File.Delete(fullname);
                }
            }
        }

        #endregion
    }
}
