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

        private String filename = null;

        [SetUp]
        public void Setup()
        {
            this.filename = Path.GetTempFileName();
        }

        [TearDown]
        public void Cleanup()
        {
            this.Sweep(this.filename);
            this.filename = null;
        }

        #endregion

        #region Construction

        [Test]
        public void PersistentLoggerSettings_ValidateDefaultSettings_DefaultSettingsAsExpected()
        {
            PersistentLoggerSettings instance = new PersistentLoggerSettings();

            Assert.That(instance.Filename, Is.EqualTo(Path.Combine(Path.GetTempPath(), "plexdata.log")));
            Assert.That(instance.IsRolling, Is.False);
            Assert.That(instance.IsQueuing, Is.False);
            Assert.That(instance.Threshold, Is.EqualTo(-1));
            Assert.That(instance.Encoding, Is.EqualTo(Encoding.UTF8));
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

            instance.Filename = this.filename;
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

            instance.Filename = this.filename;
            instance.PropertyChanged += (sender, args) => { fired = true; };
            instance.Filename = this.filename;

            Assert.That(fired, Is.False);
        }

        #endregion

        #region IsRolling

        [Test]
        public void IsRolling_ValueChanged_PropertyChangedFired()
        {
            Boolean fired = false;
            PersistentLoggerSettings instance = new PersistentLoggerSettings();

            instance.Filename = this.filename;
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

            instance.Filename = this.filename;
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

            instance.Filename = this.filename;
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

            instance.Filename = this.filename;
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

            instance.Filename = this.filename;
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

            instance.Filename = this.filename;
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

            instance.Filename = this.filename;
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

            instance.Filename = this.filename;
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
    }
}
