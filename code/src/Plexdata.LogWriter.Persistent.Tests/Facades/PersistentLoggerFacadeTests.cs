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
using Plexdata.LogWriter.Facades;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text;

namespace Plexdata.LogWriter.Persistent.Tests.Facades
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    [Category("IntegrationTest")]
    [TestOf(nameof(PersistentLoggerFacade))]
    public class PersistentLoggerFacadeTests
    {
        #region Prologue

        private String filename = null;
        private PersistentLoggerFacade instance = null;

        [SetUp]
        public void Setup()
        {
            this.filename = Path.GetTempFileName();
            this.instance = new PersistentLoggerFacade();
        }

        [TearDown]
        public void Cleanup()
        {
            this.Sweep(this.filename);
            this.filename = null;
        }

        #endregion

        #region Invalid Parameters

        [Test]
        [TestCase(null)]
        [TestCase("")]
        [TestCase("  ")]
        public void Write_FilenameIsInvalid_ThrowsArgumentOutOfRangeException(String filename)
        {
            Assert.That(() => this.instance.Write(filename, Encoding.ASCII, "message"), Throws.InstanceOf<ArgumentOutOfRangeException>());
        }

        [Test]
        public void Write_EncodingIsNull_ThrowsArgumentNullException()
        {
            Assert.That(() => this.instance.Write(this.filename, null, "message"), Throws.ArgumentNullException);
        }

        [Test]
        public void Write_MessagesAreNull_ThrowsArgumentNullException()
        {
            Assert.That(() => this.instance.Write(this.filename, Encoding.ASCII, (String[])null), Throws.ArgumentNullException);
        }

        [Test]
        public void Write_AllMessagesAreInvalid_NothingIsWritten()
        {
            List<String> messages = new List<String> { null, "", "   ", "\r\n" };

            Int64 expected = (new FileInfo(this.filename)).Length;

            this.instance.Write(this.filename, Encoding.ASCII, messages);

            Int64 actual = (new FileInfo(this.filename)).Length;

            Assert.That(actual, Is.EqualTo(expected));
        }

        #endregion

        #region Valid Parameters

        [Test]
        public void Write_SomeMessagesAreInvalid_ExpectedLengthWasWritten()
        {
            List<String> messages = new List<String> { "message", null, "  message  ", "", "   ", "\r\nmessage", "message\r\n", "message  \r\n  " };

            Int64 written = Encoding.ASCII.GetBytes("message\r\nmessage\r\nmessage\r\nmessage\r\nmessage\r\n").Length;

            Int64 expected = (new FileInfo(this.filename)).Length + written;

            this.instance.Write(this.filename, Encoding.ASCII, messages);

            Int64 actual = (new FileInfo(this.filename)).Length;

            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        public void Write_AllMessagesAreValid_ExpectedLengthWasWritten()
        {
            List<String> messages = new List<String> { "message", "  message  ", "\r\nmessage", "message\r\n", "message  \r\n  ", " \r\n   \r\n    message  \r\n  \r\n  " };

            Int64 written = Encoding.ASCII.GetBytes("message\r\nmessage\r\nmessage\r\nmessage\r\nmessage\r\nmessage\r\n").Length;

            Int64 expected = (new FileInfo(this.filename)).Length + written;

            this.instance.Write(this.filename, Encoding.ASCII, messages);

            Int64 actual = (new FileInfo(this.filename)).Length;

            Assert.That(actual, Is.EqualTo(expected));
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
