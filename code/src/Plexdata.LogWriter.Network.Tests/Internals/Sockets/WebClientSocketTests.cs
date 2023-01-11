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
using Plexdata.LogWriter.Internals.Sockets;
using Plexdata.Utilities.Testing;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Threading;

namespace Plexdata.LogWriter.Network.Tests.Internals.Sockets
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    [Category(TestType.UnitTest)]
    [TestOf(nameof(WebClientSocket))]
    internal class WebClientSocketTests
    {
        #region Prologue

        private Mock<INetworkInternalFactory> factory;
        private Mock<INetworkLoggerSettings> settings;
        private Mock<IClientFacade> client;

        private String host;
        private Int32 port;
        private Uri target;
        private String method;
        private String content;
        private Int32 timeout;

        [SetUp]
        public void Setup()
        {
            this.factory = new Mock<INetworkInternalFactory>();
            this.settings = new Mock<INetworkLoggerSettings>();
            this.client = new Mock<IClientFacade>();

            this.host = "http://localhost/route/service";
            this.port = 42666;
            this.target = new Uri($"http://localhost:{this.port}/route/service");
            this.method = "POST";
            this.content = "application/json";
            this.timeout = 1000;

            this.settings.SetupGet(x => x.Host).Returns(() => this.host);
            this.settings.SetupGet(x => x.Port).Returns(() => this.port);
            this.settings.SetupGet(x => x.Timeout).Returns(() => this.timeout);

            this.factory.Setup(x => x.Create<IClientFacade>(this.target, this.method, this.content, this.timeout)).Returns(this.client.Object);
        }

        #endregion

        #region Construction

        [TestCase(typeof(INetworkInternalFactory))]
        [TestCase(typeof(INetworkLoggerSettings))]
        public void WebClientSocket_DependencyIsNull_ThrowsArgumentNullException(Type dependency)
        {
            this.factory = dependency == typeof(INetworkInternalFactory) ? null : this.factory;
            this.settings = dependency == typeof(INetworkLoggerSettings) ? null : this.settings;

            Assert.That(() => this.CreateInstance(), Throws.ArgumentNullException);
        }

        #endregion

        #region Connect

        [Test]
        public void Connect_INetworkInternalFactory_CreateWasCalledAsExpected()
        {
            this.CreateInstance().Connect();

            this.factory.Verify(x => x.Create<IClientFacade>(this.target, this.method, this.content, this.timeout), Times.Once());
        }

        [Test]
        public void Connect_CallOnce_ResultIsTrue()
        {
            Assert.That(this.CreateInstance().Connect(), Is.True);
        }

        [Test]
        public void Connect_CallTwice_ResultIsTrueAndDependenciesWereCalledOnce()
        {
            IWebClientSocket instance = this.CreateInstance();

            Assert.That(instance.Connect(), Is.True);
            Assert.That(instance.Connect(), Is.True);

            this.factory.Verify(x => x.Create<IClientFacade>(this.target, this.method, this.content, this.timeout), Times.Once());
        }

        [Test]
        public void Connect_INetworkInternalFactoryCreateThrowsException_ResultIsFalse()
        {
            this.factory.Setup(x => x.Create<IClientFacade>(It.IsAny<Uri>(), It.IsAny<String>(), It.IsAny<String>(), It.IsAny<Int32>())).Throws<Exception>();

            Assert.That(this.CreateInstance().Connect(), Is.False);
        }

        [Test]
        public void Connect_INetworkInternalFactoryCreateThrowsException_IClientFacadeDisposeShouldNeverBeCalled()
        {
            this.factory.Setup(x => x.Create<IClientFacade>(It.IsAny<Uri>(), It.IsAny<String>(), It.IsAny<String>(), It.IsAny<Int32>())).Throws<Exception>();

            this.CreateInstance().Connect();

            this.client.Verify(x => x.Dispose(), Times.Never());
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        public void Connect_HostIsInvalid_ResultIsFalse(String host)
        {
            this.host = host;

            Assert.That(this.CreateInstance().Connect(), Is.False);
        }

        [TestCase(IPEndPoint.MinPort - 1)]
        [TestCase(IPEndPoint.MaxPort + 1)]
        public void Connect_PortIsInvalid_ResultIsFalse(Int32 port)
        {
            this.port = port;

            Assert.That(this.CreateInstance().Connect(), Is.False);
        }

        [Test]
        public void Connect_PortIsZeroAndHostContainsNoPort_CreateWasCalledAsExpected()
        {
            this.host = "http://localhost/route/service";
            this.port = 0;
            this.target = new Uri("http://localhost:80/route/service");

            this.CreateInstance().Connect();

            this.factory.Verify(x => x.Create<IClientFacade>(this.target, this.method, this.content, this.timeout), Times.Once());
        }

        #endregion

        #region Send

        [Test]
        public void Send_NotConnected_ResultIsZero()
        {
            Byte[] payload = new Byte[10];

            IWebClientSocket instance = this.CreateInstance();

            Int32 actual = instance.Send(payload);

            Assert.That(actual, Is.Zero);
        }

        [Test]
        public void Send_PayloadIsNull_ResultIsZero()
        {
            Byte[] payload = null;

            IWebClientSocket instance = this.CreateInstance();

            instance.Connect();

            Int32 actual = instance.Send(payload);

            Assert.That(actual, Is.Zero);
        }

        [Test]
        public void Send_PayloadIsEmpty_ResultIsZero()
        {
            Byte[] payload = new Byte[0];

            IWebClientSocket instance = this.CreateInstance();

            instance.Connect();

            Int32 actual = instance.Send(payload);

            Assert.That(actual, Is.Zero);
        }

        [Test]
        public void Send_IClientFacadeSendThrowsException_ResultIsZero()
        {
            Byte[] payload = new Byte[10];

            this.client.Setup(x => x.Send(It.IsAny<Byte[]>())).Throws<Exception>();

            IWebClientSocket instance = this.CreateInstance();

            instance.Connect();

            Int32 actual = instance.Send(payload);

            Assert.That(actual, Is.Zero);
        }

        [Test]
        public void Send_ValidInput_IClientFacadeSendWasCalledOnce()
        {
            Byte[] payload = new Byte[10];

            IWebClientSocket instance = this.CreateInstance();

            instance.Connect();

            instance.Send(payload);

            this.client.Verify(x => x.Send(payload), Times.Once());
        }

        #endregion

        #region Dispose

        [Test]
        public void Dispose_NotConnected_IClientFacadeDisposeWasNeverCalled()
        {
            this.CreateInstance().Dispose();

            this.client.Verify(x => x.Dispose(), Times.Never());
        }

        [Test]
        public void Dispose_ConnectedAndDisposedOnce_IClientFacadeDisposeWasCalledOnce()
        {
            IWebClientSocket instance = this.CreateInstance();

            instance.Connect();
            instance.Dispose();

            this.client.Verify(x => x.Dispose(), Times.Once());
        }

        [Test]
        public void Dispose_ConnectedAndDisposedTwice_IClientFacadeDisposeWasCalledOnce()
        {
            IWebClientSocket instance = this.CreateInstance();

            instance.Connect();
            instance.Dispose();
            instance.Dispose();

            this.client.Verify(x => x.Dispose(), Times.Once());
        }

        #endregion

        #region Private helpers

        private IWebClientSocket CreateInstance()
        {
            return new WebClientSocket(
                this.factory?.Object,
                this.settings?.Object
            );
        }

        #endregion
    }
}
