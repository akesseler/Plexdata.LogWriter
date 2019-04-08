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
        #region EnsureFullFilePathOrThrow

        [Test]
        [TestCase(null)]
        [TestCase("")]
        [TestCase("  ")]
        public void EnsureFullFilePathOrThrow_NameIsInvalid_ThrowsArgumentOutOfRangeException(String filename)
        {
            Assert.That(() => filename.EnsureFullFilePathOrThrow(), Throws.InstanceOf<ArgumentOutOfRangeException>());
        }

        [Test]
        [TestCase("test-file-name.xyz")]
        public void EnsureFullFilePathOrThrow_PathIsEmpty_ThrowsArgumentOutOfRangeException(String filename)
        {
            Assert.That(() => filename.EnsureFullFilePathOrThrow(), Throws.InstanceOf<ArgumentOutOfRangeException>());
        }

        [Test]
        [TestCase("/")]
        [TestCase("\\")]
        [TestCase("/\\")]
        [TestCase("\\/")]
        [TestCase("/test-file-name.xyz")]
        [TestCase("\\test-file-name.xyz")]
        [TestCase("/\\test-file-name.xyz")]
        [TestCase("\\/test-file-name.xyz")]
        public void EnsureFullFilePathOrThrow_PathIsInvalid_ThrowsArgumentOutOfRangeException(String filename)
        {
            Assert.That(() => filename.EnsureFullFilePathOrThrow(), Throws.InstanceOf<ArgumentOutOfRangeException>());
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
        [TestCase("/path-should-not-exist/")]
        [TestCase("c:/path-should-not-exist/")]
        [TestCase("\\path-should-not-exist\\")]
        [TestCase("c:\\path-should-not-exist\\")]
        public void EnsureFullFilePathOrThrow_FileNameIsInvalid_ThrowsArgumentOutOfRangeException(String filename)
        {
            Assert.That(() => filename.EnsureFullFilePathOrThrow(), Throws.InstanceOf<ArgumentOutOfRangeException>());
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
        [TestCase("/path-should-not-exist/file-may-not-exist")]
        [TestCase("c:/path-should-not-exist/file-may-not-exist")]
        [TestCase("\\path-should-not-exist\\file-may-not-exist")]
        [TestCase("c:\\path-should-not-exist\\file-may-not-exist")]
        public void EnsureFullFilePathOrThrow_PathDoesNotExist_ThrowsDirectoryNotFoundException(String filename)
        {
            Assert.That(() => filename.EnsureFullFilePathOrThrow(), Throws.InstanceOf<DirectoryNotFoundException>());
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
