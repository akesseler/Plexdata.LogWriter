/*
 * MIT License
 * 
 * Copyright (c) 2021 plexdata.de
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

namespace Plexdata.LogWriter.Stream.Tests.Facades
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    [TestOf(nameof(StreamLoggerFacade))]
    public class StreamLoggerFacadeTests
    {
        #region Prologue

        private System.IO.Stream stream;
        private StreamLoggerFacade instance = null;

        [SetUp]
        public void Setup()
        {
            this.stream = new MemoryStream();
            this.instance = new StreamLoggerFacade();
        }

        [TearDown]
        public void Cleanup()
        {
            this.instance = null;
            this.stream.Dispose();
            this.stream = null;
        }

        #endregion

        #region Invalid Parameters

        [Test]
        public void Write_StreamIsNull_ThrowsArgumentNullException()
        {
            Assert.That(() => this.instance.Write(null, Encoding.ASCII, "message"), Throws.ArgumentNullException);
        }

        [Test]
        public void Write_EncodingIsNull_ThrowsArgumentNullException()
        {
            Assert.That(() => this.instance.Write(this.stream, null, "message"), Throws.ArgumentNullException);
        }

        [Test]
        public void Write_MessagesAreNull_ThrowsArgumentNullException()
        {
            Assert.That(() => this.instance.Write(this.stream, Encoding.ASCII, (String[])null), Throws.ArgumentNullException);
        }

        [Test]
        public void Write_AllMessagesAreInvalid_NothingIsWritten()
        {
            List<String> messages = new List<String> { null, "", "   ", "\r\n" };

            Int64 expected = this.stream.Length;

            Assert.That(this.instance.Write(this.stream, Encoding.ASCII, messages), Is.False);

            Int64 actual = this.stream.Length;

            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        public void Write_StreamIsReadOnly_NothingIsWritten()
        {
            using (MemoryStream stream = new MemoryStream(new Byte[] { }, false))
            {
                Int64 expected = stream.Length;

                StreamLoggerFacade instance = new StreamLoggerFacade();

                Assert.That(instance.Write(stream, Encoding.ASCII, "message"), Is.False);

                Int64 actual = stream.Length;

                Assert.That(actual, Is.EqualTo(expected));
            }
        }

        [Test]
        public void Write_StreamIsDisposed_NothingIsWritten()
        {
            // NOTE: Unfortunately, this test does not hit the method's catch part!

            MemoryStream stream = new MemoryStream();

            stream.Dispose();

            StreamLoggerFacade instance = new StreamLoggerFacade();

            Assert.That(instance.Write(stream, Encoding.ASCII, "message"), Is.False);
        }

        #endregion

        #region Valid Parameters

        [Test]
        public void Write_SomeMessagesAreInvalid_ExpectedLengthWasWritten()
        {
            List<String> messages = new List<String> { "message", null, "  message  ", "", "   ", "\r\nmessage", "message\r\n", "message  \r\n  " };

            Int64 written = this.GetBuffer(Encoding.ASCII, "message", 5).Length;

            Int64 expected = this.stream.Length + written;

            Assert.That(this.instance.Write(this.stream, Encoding.ASCII, messages), Is.True);

            Int64 actual = this.stream.Length;

            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        public void Write_AllMessagesAreValid_ExpectedLengthWasWritten()
        {
            List<String> messages = new List<String> { "message", "  message  ", "\r\nmessage", "message\r\n", "message  \r\n  ", " \r\n   \r\n    message  \r\n  \r\n  " };

            Int64 written = this.GetBuffer(Encoding.ASCII, "message", 6).Length;

            Int64 expected = this.stream.Length + written;

            this.instance.Write(this.stream, Encoding.ASCII, messages);

            Int64 actual = this.stream.Length;

            Assert.That(actual, Is.EqualTo(expected));
        }

        #endregion

        #region Private helpers

        private Byte[] GetBuffer(Encoding encoding, String message, Int32 count)
        {
            StringBuilder builder = new StringBuilder((message.Length + StreamLoggerFacade.ETB.Length) * count);

            for (Int32 index = 0; index < count; index++)
            {
                builder.Append($"{message}{StreamLoggerFacade.ETB}");
            }

            return encoding.GetBytes(builder.ToString());
        }

        #endregion
    }
}
