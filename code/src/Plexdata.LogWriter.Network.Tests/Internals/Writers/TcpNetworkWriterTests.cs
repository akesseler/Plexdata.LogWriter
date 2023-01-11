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
using Plexdata.LogWriter.Internals.Abstraction;
using Plexdata.LogWriter.Internals.Writers;
using Plexdata.Utilities.Testing;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Plexdata.LogWriter.Network.Tests.Internals.Writers
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    [Category(TestType.UnitTest)]
    [TestOf(nameof(TcpNetworkWriter))]
    internal class TcpNetworkWriterTests
    {
        private Mock<INetworkInternalFactory> factory;
        private Mock<INetworkLoggerSettings> settings;
        private Mock<ITcpClientSocket> client;

        private Encoding encoding;
        private Boolean termination;
        private Boolean opened;

        [SetUp]
        public void Setup()
        {
            this.factory = new Mock<INetworkInternalFactory>();
            this.settings = new Mock<INetworkLoggerSettings>();
            this.client = new Mock<ITcpClientSocket>();

            this.encoding = Encoding.UTF8;
            this.settings.SetupGet(x => x.Encoding).Returns(() => this.encoding);

            this.termination = false;
            this.settings.SetupGet(x => x.Termination).Returns(() => this.termination);

            this.factory.Setup(x => x.Create<ITcpClientSocket>(this.settings.Object)).Returns(this.client.Object);

            this.opened = true;
            this.client.Setup(x => x.Open()).Returns(() => this.opened);
        }

        #region Construction Tests

        [TestCase(typeof(INetworkInternalFactory))]
        [TestCase(typeof(INetworkLoggerSettings))]
        public void TcpNetworkWriter_DependencyIsNull_ThrowsArgumentNullException(Type dependency)
        {
            this.factory = dependency == typeof(INetworkInternalFactory) ? null : this.factory;
            this.settings = dependency == typeof(INetworkLoggerSettings) ? null : this.settings;

            Assert.That(() => this.CreateInstance(), Throws.ArgumentNullException);
        }

        [Test]
        public void TcpNetworkWriter_DependencyIsValid_NetworkInternalFactoryCreateWasCalledWithWithExpectedTypes()
        {
            this.CreateInstance();

            this.factory.Verify(x => x.Create<ITcpClientSocket>(this.settings.Object), Times.Once());
        }

        #endregion

        #region IsDisposed Tests

        [Test]
        public void IsDisposed_CreateAndDispose_PropertyTogglesAsExpected()
        {
            ITcpNetworkWriter instance = this.CreateInstance();

            Assert.That(instance.IsDisposed, Is.False);

            instance.Dispose();

            Assert.That(instance.IsDisposed, Is.True);

            instance.Dispose();

            Assert.That(instance.IsDisposed, Is.True);
        }

        #endregion

        #region Write Tests

        [Test]
        public void Write_IsDisposedIsTrue_ClientOpenWasNeverCalled()
        {
            ITcpNetworkWriter instance = this.CreateInstance();

            instance.Dispose();

            instance.Write("message");

            this.client.Verify(x => x.Open(), Times.Never());
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        public void Write_MessageIsInvalid_ClientOpenWasNeverCalled(String message)
        {
            this.CreateInstance().Write(message);

            this.client.Verify(x => x.Open(), Times.Never());
        }

        [Test]
        public void Write_MessageIsValid_ClientOpenWasCalledOnce()
        {
            this.CreateInstance().Write("message");

            this.client.Verify(x => x.Open(), Times.Once());
        }

        [Test]
        public void Write_ITcpClientSocketOpenReturnsFalse_ClientSendWasNeverCalled()
        {
            this.opened = false;

            this.CreateInstance().Write("message");

            this.client.Verify(x => x.Send(It.IsAny<Byte[]>()), Times.Never());
        }

        [TestCase("message", "message")]
        [TestCase("message\0", "message\0")]
        public void Write_TerminationIsFalse_ClientSendWasCalledAsExpected(String message, String expected)
        {
            String actual = null;

            this.client
                .Setup(x => x.Send(It.IsAny<Byte[]>()))
                .Callback((Byte[] p) => { actual = this.encoding.GetString(p); });

            this.CreateInstance().Write(message);

            Assert.That(actual, Is.EqualTo(expected));
        }

        [TestCase("message", "message\0")]
        [TestCase("message\0", "message\0")]
        public void Write_TerminationIsTrue_ClientSendWasCalledAsExpected(String message, String expected)
        {
            String actual = null;

            this.termination = true;

            this.client
                .Setup(x => x.Send(It.IsAny<Byte[]>()))
                .Callback((Byte[] p) => { actual = this.encoding.GetString(p); });

            this.CreateInstance().Write(message);

            Assert.That(actual, Is.EqualTo(expected));
        }

        [TestCase(true, "message", 0)]
        [TestCase(true, "message\0", 0)]
        [TestCase(false, "message", 1)]
        [TestCase(false, "message\0", 0)]
        public void Write_TerminationAndMessageAsProvided_ClientCloseWasCalledAsExpected(Boolean termination, String message, Int32 expected)
        {
            this.termination = termination;

            this.CreateInstance().Write(message);

            this.client.Verify(x => x.Close(), Times.Exactly(expected));
        }

        #endregion

        #region Dispose Tests

        [Test]
        public void Dispose_MultipleDisposings_ClientDisposeWasCalledOnce()
        {
            ITcpNetworkWriter instance = this.CreateInstance();

            instance.Dispose();
            instance.Dispose();

            this.client.Verify(x => x.Dispose(), Times.Once());
        }

        #endregion

        private ITcpNetworkWriter CreateInstance()
        {
            return new TcpNetworkWriter(
                this.factory?.Object,
                this.settings?.Object
            );
        }
    }
}
