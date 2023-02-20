/*
 * MIT License
 * 
 * Copyright (c) 2023 plexdata.de
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
using System.Linq;
using System.Text;

namespace Plexdata.LogWriter.Network.Tests.Internals.Writers
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    [Category(TestType.UnitTest)]
    [TestOf(nameof(WebNetworkWriter))]
    internal class WebNetworkWriterTests
    {
        private Mock<INetworkInternalFactory> factory;
        private Mock<INetworkLoggerSettings> settings;
        private Mock<IWebClientSocket> client;

        private Encoding encoding;
        private Boolean connected;

        [SetUp]
        public void Setup()
        {
            this.factory = new Mock<INetworkInternalFactory>();
            this.settings = new Mock<INetworkLoggerSettings>();
            this.client = new Mock<IWebClientSocket>();

            this.encoding = Encoding.UTF8;
            this.settings.SetupGet(x => x.Encoding).Returns(() => this.encoding);

            this.factory.Setup(x => x.Create<IWebClientSocket>(this.settings.Object)).Returns(this.client.Object);

            this.connected = true;
            this.client.Setup(x => x.Connect()).Returns(() => this.connected);
        }

        #region Construction Tests

        [TestCase(typeof(INetworkInternalFactory))]
        [TestCase(typeof(INetworkLoggerSettings))]
        public void WebNetworkWriter_DependencyIsNull_ThrowsArgumentNullException(Type dependency)
        {
            this.factory = dependency == typeof(INetworkInternalFactory) ? null : this.factory;
            this.settings = dependency == typeof(INetworkLoggerSettings) ? null : this.settings;

            Assert.That(() => this.CreateInstance(), Throws.ArgumentNullException);
        }

        [Test]
        public void WebNetworkWriter_DependencyIsValid_NetworkInternalFactoryCreateWasCalledWithWithExpectedTypes()
        {
            this.CreateInstance();

            this.factory.Verify(x => x.Create<IWebClientSocket>(this.settings.Object), Times.Once());
        }

        #endregion

        #region IsDisposed Tests

        [Test]
        public void IsDisposed_CreateAndDispose_PropertyTogglesAsExpected()
        {
            IWebNetworkWriter instance = this.CreateInstance();

            Assert.That(instance.IsDisposed, Is.False);

            instance.Dispose();

            Assert.That(instance.IsDisposed, Is.True);

            instance.Dispose();

            Assert.That(instance.IsDisposed, Is.True);
        }

        #endregion

        #region Write Tests

        [Test]
        public void Write_IsDisposedIsTrue_ClientConnectWasNeverCalled()
        {
            IWebNetworkWriter instance = this.CreateInstance();

            instance.Dispose();

            instance.Write("message");

            this.client.Verify(x => x.Connect(), Times.Never());
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        public void Write_MessageIsInvalid_ClientConnectWasNeverCalled(String message)
        {
            this.CreateInstance().Write(message);

            this.client.Verify(x => x.Connect(), Times.Never());
        }

        [Test]
        public void Write_MessageIsValid_ClientConnectWasCalledOnce()
        {
            this.CreateInstance().Write("message");

            this.client.Verify(x => x.Connect(), Times.Once());
        }

        [Test]
        public void Write_IWebClientSocketConnectReturnsFalse_ClientSendWasNeverCalled()
        {
            this.connected = false;

            this.CreateInstance().Write("message");

            this.client.Verify(x => x.Send(It.IsAny<Byte[]>()), Times.Never());
        }

        [Test]
        public void Write_PayloadConversion_ClientSendWasCalledAsExpected()
        {
            String message = "message";
            Byte[] payload = this.encoding.GetBytes(message);
            Byte[] actual = null;

            this.client
                .Setup(x => x.Send(It.IsAny<Byte[]>()))
                .Callback((Byte[] p) => { actual = p; });

            this.CreateInstance().Write(message);

            Assert.That(actual.SequenceEqual(payload), Is.True);
        }

        #endregion

        #region Dispose Tests

        [Test]
        public void Dispose_MultipleDisposings_ClientDisposeWasCalledOnce()
        {
            IWebNetworkWriter instance = this.CreateInstance();

            instance.Dispose();
            instance.Dispose();

            this.client.Verify(x => x.Dispose(), Times.Once());
        }

        #endregion

        private IWebNetworkWriter CreateInstance()
        {
            return new WebNetworkWriter(
                this.factory?.Object,
                this.settings?.Object
            );
        }
    }
}
