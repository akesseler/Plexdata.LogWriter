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

using NUnit.Framework;
using Plexdata.LogWriter.Features;
using Plexdata.Utilities.Testing;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Plexdata.LogWriter.Stream.Tests.Features
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    [Category(TestType.UnitTest)]
    [TestOf(nameof(LoggerStream))]
    public class LoggerStreamTests
    {
        #region Construction

        [Test]
        public void LoggerStream_DefaultConstruction_PropertiesWithDefaultValues()
        {
            LoggerStream instance = new LoggerStream();
            Assert.That(instance.Encoding, Is.EqualTo(Encoding.UTF8));
        }

        [Test]
        public void LoggerStream_ExtendedConstruction_PropertiesWithProvidedValues()
        {
            LoggerStream instance = new LoggerStream(Encoding.ASCII);
            Assert.That(instance.Encoding, Is.EqualTo(Encoding.ASCII));
        }

        [Test]
        public void LoggerStream_DefaultConstruction_OverwrittenPropertiesAsExpected()
        {
            LoggerStream instance = new LoggerStream();
            Assert.That(instance.CanRead, Is.False);
            Assert.That(instance.CanSeek, Is.False);
            Assert.That(instance.CanWrite, Is.True);
            Assert.That(instance.Length, Is.Zero);
            Assert.That(instance.Position, Is.Zero);
        }

        #endregion

        #region Properties

        [TestCase(null, "utf-8")]
        [TestCase("utf-8", "utf-8")]
        [TestCase("us-ascii", "us-ascii")]
        public void Encoding_CallSetterWithValue_GetterReturnsExpectedValue(String actual, String expected)
        {
            LoggerStream instance = new LoggerStream();
            instance.Encoding = actual == null ? (Encoding)null : Encoding.GetEncoding(actual);
            Assert.That(instance.Encoding.BodyName, Is.EqualTo(expected));
        }

        [Test]
        public void Position_CallSetterWithValue_PositionRemainsZero([Values(0L, -42, 42)] Int64 actual)
        {
            LoggerStream instance = new LoggerStream();
            instance.Position = actual;
            Assert.That(instance.Position, Is.Zero);
        }

        #endregion

        #region Methods

        [Test]
        public void Flush_MessageListIsEmpty_EventIsNotFiredMessageListIsEmpty()
        {
            Boolean fired = false;
            LoggerStream instance = new LoggerStream();

            instance.ProcessStreamData += (sender, args) => { fired = true; };
            instance.Flush();

            Assert.That(fired, Is.False);
            Assert.That(this.GetMessagesFieldValue(instance), Is.Empty);
        }

        [Test]
        public void Flush_ContainsVariousItems_EventIsFiredOnceMessageListIsEmpty([Values(1, 3, 5)] Int32 count)
        {
            Int32 fired = 0;
            LoggerStream instance = new LoggerStream();

            this.SetMessagesFieldValue(instance, this.GetInitialMessageList(count));

            instance.ProcessStreamData += (sender, args) => { fired++; };
            instance.Flush();

            Assert.That(fired, Is.EqualTo(1));
            Assert.That(this.GetMessagesFieldValue(instance), Is.Empty);
        }

        [Test]
        public void Flush_EventHandlerThrowsException_NoExceptionIsEscalated([Values(1, 3, 5)] Int32 count)
        {
            LoggerStream instance = new LoggerStream();

            this.SetMessagesFieldValue(instance, this.GetInitialMessageList(count));

            instance.ProcessStreamData += (sender, args) => { throw new NotSupportedException(); };

            Assert.That(() => instance.Flush(), Throws.Nothing);
            Assert.That(this.GetMessagesFieldValue(instance), Is.Empty);
        }

        [Test]
        public void Read_CheckNotSupported_ThrowsNotSupportedException()
        {
            LoggerStream instance = new LoggerStream();
            Assert.That(() => instance.Read(null, 0, 0), Throws.InstanceOf<NotSupportedException>());
        }

        [Test]
        public void Seek_CheckNotSupported_ThrowsNotSupportedException()
        {
            LoggerStream instance = new LoggerStream();
            Assert.That(() => instance.Seek(0, 0), Throws.InstanceOf<NotSupportedException>());
        }

        [Test]
        public void SetLength_CheckNothingChanges_LengthRemainsZero([Values(0L, -42, 42)] Int64 actual)
        {
            LoggerStream instance = new LoggerStream();
            instance.SetLength(actual);
            Assert.That(instance.Length, Is.Zero);
        }

        [Test]
        public void Write_LengthIsInvalid_NoExceptionIsEscalated()
        {
            LoggerStream instance = new LoggerStream();

            Byte[] buffer = new Byte[] { 0x05, 0x23, 0x42 };

            Assert.That(() => instance.Write(buffer, 0, 5), Throws.Nothing);
            Assert.That(this.GetMessagesFieldValue(instance), Is.Empty);
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase("  ")]
        [TestCase("\r\n")]
        [TestCase("  \r\n  ")]
        [TestCase("\r\n\r\n")]
        [TestCase("  \r\n  \r\n  ")]
        public void Write_BufferIsInvalid_NothingIsChanged(String messages)
        {
            LoggerStream instance = new LoggerStream();

            Byte[] buffer = messages == null ? null : instance.Encoding.GetBytes(messages);
            instance.Write(buffer, 0, buffer == null ? 0 : buffer.Length);

            Assert.That(this.GetMessagesFieldValue(instance), Is.Empty);
        }

        [Test]
        public void Write_BufferWithVariousItems_MessageListAsExpected([Values(1, 3, 5)] Int32 count)
        {
            LoggerStream instance = new LoggerStream();

            List<String> messages = this.GetInitialMessageList(count);

            Byte[] buffer = instance.Encoding.GetBytes(String.Join(LoggerStream.ETB, messages));

            instance.Write(buffer, 0, buffer.Length);

            // Each string in this list is compared!
            Assert.That(this.GetMessagesFieldValue(instance), Is.EqualTo(messages));
        }

        #endregion

        #region Private helpers

        private List<String> GetInitialMessageList(Int32 count)
        {
            List<String> result = new List<String>();

            for (Int32 index = 1; index <= count; index++)
            {
                // Need to test escaped CRLFs and other escaped control characters!
                result.Add(String.Format("\"message\":\"message-{0}\",\"level\":\"level-{0}\",\"escaped\":\"  \\r\\n \\\\ \\t   \"", index));
            }

            return result;
        }

        private void SetMessagesFieldValue(LoggerStream instance, List<String> messages)
        {
            FieldInfo info = typeof(LoggerStream).GetField("messages", BindingFlags.NonPublic | BindingFlags.Instance);
            info.SetValue(instance, messages);
        }

        private List<String> GetMessagesFieldValue(LoggerStream instance)
        {
            FieldInfo info = typeof(LoggerStream).GetField("messages", BindingFlags.NonPublic | BindingFlags.Instance);
            return (info.GetValue(instance) as IEnumerable<String>).ToList();
        }

        #endregion
    }
}
