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
using Plexdata.LogWriter.Internals.Extensions;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace Plexdata.LogWriter.Persistent.Tests.Internals.Extensions
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    [Category("IntegrationTest")]
    [TestOf(nameof(SettingsValidationExtension))]
    public class SettingsValidationExtensionTests
    {
        #region Prologue

        private String defaultDirectory = null;
        private List<String> cleanupFolders = null;

        [SetUp]
        public void Setup()
        {
            this.defaultDirectory = Path.Combine(Path.GetTempPath(), "logging");
            this.cleanupFolders = new List<String>();
            this.cleanupFolders.Add(this.defaultDirectory);
        }

        [TearDown]
        public void Cleanup()
        {
            foreach (String cleanupFolder in this.cleanupFolders)
            {
                try
                {
                    if (Directory.Exists(cleanupFolder))
                    {
                        System.Diagnostics.Debug.WriteLine($"Remove folder {cleanupFolder}.");
                        Directory.Delete(cleanupFolder, true);
                    }
                }
                catch (Exception exception)
                {
                    System.Diagnostics.Debug.WriteLine($"Delete folder {cleanupFolder} crashed with exception {exception.Message}");
                    System.Diagnostics.Debug.WriteLine(exception);
                }
            }

            this.cleanupFolders.Clear();
        }

        #endregion

        #region EnsureFullFilePathOrThrow

        [Test]
        [TestCase(null)]
        [TestCase("")]
        [TestCase("  ")]
        [TestCase("/")]
        [TestCase("\\")]
        [TestCase("/\\")]
        [TestCase("\\/")]
        [TestCase("/path-should-not-exist/")]
        [TestCase("c:/path-should-not-exist/")]
        [TestCase("\\path-should-not-exist\\")]
        [TestCase("c:\\path-should-not-exist\\")]
        public void EnsureFullFilePathOrThrow_FileNameIsInvalid_ThrowsArgumentOutOfRangeException(String filename)
        {
            Assert.That(() => filename.EnsureFullFilePathOrThrow(), Throws.InstanceOf<ArgumentOutOfRangeException>());
        }

        [Test]
        [TestCase("test-file-name.xyz")]
        [TestCase("/test-file-name.xyz")]
        [TestCase("\\test-file-name.xyz")]
        [TestCase("/\\test-file-name.xyz")]
        [TestCase("\\/test-file-name.xyz")]
        public void EnsureFullFilePathOrThrow_PathIsEmpty_PathIsDefaultDirectory(String filename)
        {
            String expected = Path.Combine(this.defaultDirectory, filename.Replace("\\", "").Replace("/", ""));
            String actual = filename.EnsureFullFilePathOrThrow();

            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        [TestCase("*/")]
        [TestCase("?/")]
        [TestCase("|/")]
        [TestCase("*\\")]
        [TestCase("?\\")]
        [TestCase("|\\")]
        [TestCase("*/\\")]
        [TestCase("?/\\")]
        [TestCase("|/\\")]
        [TestCase("*\\/")]
        [TestCase("?\\/")]
        [TestCase("|\\/")]
        [TestCase("*/test-file-name.xyz")]
        [TestCase("?/test-file-name.xyz")]
        [TestCase("|/test-file-name.xyz")]
        [TestCase("*\\test-file-name.xyz")]
        [TestCase("?\\test-file-name.xyz")]
        [TestCase("|\\test-file-name.xyz")]
        [TestCase("*/\\test-file-name.xyz")]
        [TestCase("?/\\test-file-name.xyz")]
        [TestCase("|/\\test-file-name.xyz")]
        [TestCase("*\\/test-file-name.xyz")]
        [TestCase("?\\/test-file-name.xyz")]
        [TestCase("|\\/test-file-name.xyz")]
        [TestCase("*\\/test-file-name.xyz")]
        [TestCase("?\\/test-file-name.xyz")]
        [TestCase("|\\/test-file-name.xyz")]
        public void EnsureFullFilePathOrThrow_PathNameContainsInvalidCharacters_ThrowsArgumentException(String filename)
        {
            Assert.That(() => filename.EnsureFullFilePathOrThrow(), Throws.InstanceOf<ArgumentException>());
        }

        [Test]
        [TestCase("/path-should-not-exist/test*file-name.xyz")]
        [TestCase("/path-should-not-exist/test?file-name.xyz")]
        [TestCase("/path-should-not-exist/test|file-name.xyz")]
        [TestCase("c:/path-should-not-exist/test*file-name.xyz")]
        [TestCase("c:/path-should-not-exist/test?file-name.xyz")]
        [TestCase("c:/path-should-not-exist/test|file-name.xyz")]
        [TestCase("\\path-should-not-exist\\test*file-name.xyz")]
        [TestCase("\\path-should-not-exist\\test?file-name.xyz")]
        [TestCase("\\path-should-not-exist\\test|file-name.xyz")]
        [TestCase("c:\\path-should-not-exist\\test*file-name.xyz")]
        [TestCase("c:\\path-should-not-exist\\test?file-name.xyz")]
        [TestCase("c:\\path-should-not-exist\\test|file-name.xyz")]
        public void EnsureFullFilePathOrThrow_FileNameContainsInvalidCharacters_ThrowsArgumentException(String filename)
        {
            Assert.That(() => filename.EnsureFullFilePathOrThrow(), Throws.InstanceOf<ArgumentException>());
        }

        [Test]
        [TestCase("/path-should-not-exist/file-may-not-exist", "C:\\path-should-not-exist\\file-may-not-exist")]
        [TestCase("c:/path-should-not-exist/file-may-not-exist", "c:\\path-should-not-exist\\file-may-not-exist")]
        [TestCase("\\path-should-not-exist\\file-may-not-exist", "C:\\path-should-not-exist\\file-may-not-exist")]
        [TestCase("c:\\path-should-not-exist\\file-may-not-exist", "c:\\path-should-not-exist\\file-may-not-exist")]
        public void EnsureFullFilePathOrThrow_PathDoesNotExist_PathIsCreatedAsExpected(String filename, String expected)
        {
            this.cleanupFolders.Add(Path.GetDirectoryName(expected));
            String actual = filename.EnsureFullFilePathOrThrow();

            Assert.That(actual, Is.EqualTo(expected));
            Assert.That(Directory.Exists(Path.GetDirectoryName(expected)), Is.True);
        }

        [Test]
        public void EnsureFullFilePathOrThrow_FileNameIsDirectory_ThrowsArgumentException()
        {
            String filename = Path.GetTempPath().TrimEnd(Path.DirectorySeparatorChar).TrimEnd(Path.AltDirectorySeparatorChar);
            Assert.That(() => filename.EnsureFullFilePathOrThrow(), Throws.InstanceOf<ArgumentException>());
        }

        [Test]
        [TestCase("/Users/{0}/Documents/test-file-name.xyz", "C:\\Users\\{0}\\Documents\\test-file-name.xyz")]
        [TestCase("C:/Users/{0}/Documents/test-file-name.xyz", "C:\\Users\\{0}\\Documents\\test-file-name.xyz")]
        [TestCase("\\Users\\{0}\\Documents\\test-file-name.xyz", "C:\\Users\\{0}\\Documents\\test-file-name.xyz")]
        [TestCase("C:\\Users\\{0}\\Documents\\test-file-name.xyz", "C:\\Users\\{0}\\Documents\\test-file-name.xyz")]
        public void EnsureFullFilePathOrThrow_PartialPathConversion_ResultIsFullPath(String filename, String expected)
        {
            filename = String.Format(filename, Environment.UserName);
            expected = String.Format(expected, Environment.UserName);
            Assert.That(filename.EnsureFullFilePathOrThrow(), Is.EqualTo(expected));
        }

        [Test]
        [TestCase("{0}test-file-name.xyz")]
        [TestCase("  {0}test-file-name.xyz")]
        [TestCase("{0}test-file-name.xyz  ")]
        [TestCase("  {0}test-file-name.xyz  ")]
        public void EnsureFullFilePathOrThrow_FilePathAndNameValid_ResultFullFilePath(String format)
        {
            String filename = String.Format(format, Path.GetTempPath());
            Assert.That(filename.EnsureFullFilePathOrThrow(), Is.EqualTo(filename.Trim()));
        }

        [Test]
        [TestCase("%tmp%")]
        [TestCase("%TMP%")]
        [TestCase("%temp%")]
        [TestCase("%TEMP%")]
        [TestCase("%localappdata%\\temp")]
        [TestCase("%LOCALAPPDATA%\\Temp")]
        [TestCase("%homedrive%%homepath%\\appdata\\local\\temp")]
        [TestCase("%HOMEDRIVE%%HOMEPATH%\\AppData\\Local\\Temp")]
        [TestCase("%homedrive%\\%homepath%\\appdata\\local\\temp")]
        [TestCase("%HOMEDRIVE%\\%HOMEPATH%\\AppData\\Local\\Temp")]
        public void EnsureFullFilePathOrThrow_FilePathWithEnvironmentVariable_ResultFullFilePath(String path)
        {
            String filename = Path.Combine(path, "test-file-name.xyz");
            String expected = Path.Combine(Path.GetTempPath(), "test-file-name.xyz").ToLower();

            Assert.That(filename.EnsureFullFilePathOrThrow().ToLower(), Is.EqualTo(expected));
        }

        #endregion

        #region EnsureFullPathAndWriteAccessOrThrow

        [Test]
        public void EnsureFullPathAndWriteAccessOrThrow_FileIsNew_FileDoesNotExist()
        {
            String filename = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "test-file-name.xyz");

            if (File.Exists(filename)) { File.Delete(filename); }

            filename.EnsureFullPathAndWriteAccessOrThrow();

            Assert.That(File.Exists(filename), Is.False);
        }

        [Test]
        public void EnsureFullPathAndWriteAccessOrThrow_FileIsOld_FileStillExists()
        {
            String filename = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "test-file-name.xyz");

            if (!File.Exists(filename)) { using (Stream stream = File.Create(filename)) { } }

            filename.EnsureFullPathAndWriteAccessOrThrow();

            Assert.That(File.Exists(filename), Is.True);

            if (File.Exists(filename)) { File.Delete(filename); }
        }

        #endregion
    }
}
