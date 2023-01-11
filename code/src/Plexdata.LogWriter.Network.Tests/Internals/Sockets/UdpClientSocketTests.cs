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
using Plexdata.LogWriter.Definitions;
using Plexdata.LogWriter.Internals.Abstraction;
using Plexdata.LogWriter.Internals.Sockets;
using Plexdata.Utilities.Testing;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Net.Sockets;

namespace Plexdata.LogWriter.Network.Tests.Internals.Sockets
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    [Category(TestType.UnitTest)]
    [TestOf(nameof(UdpClientSocket))]
    internal class UdpClientSocketTests
    {
        #region Prologue

        private Mock<INetworkInternalFactory> factory;
        private Mock<INetworkLoggerSettings> settings;
        private Mock<IResolverFacade> resolver;
        private Mock<ISocketFacade> socket;

        private IPEndPoint target;
        private Address address;
        private AddressFamily family;
        private String host;
        private Int32 port;

        [SetUp]
        public void Setup()
        {
            this.factory = new Mock<INetworkInternalFactory>();
            this.settings = new Mock<INetworkLoggerSettings>();

            this.resolver = new Mock<IResolverFacade>();
            this.socket = new Mock<ISocketFacade>();

            this.address = Address.IPv6;
            this.family = AddressFamily.InterNetworkV6;
            this.host = "localhost";
            this.port = 42666;
            this.target = new IPEndPoint(IPAddress.IPv6Loopback, this.port);

            this.settings.SetupGet(x => x.Address).Returns(this.address);
            this.settings.SetupGet(x => x.Host).Returns(this.host);
            this.settings.SetupGet(x => x.Port).Returns(this.port);

            this.factory.Setup(x => x.Create<IResolverFacade>()).Returns(this.resolver.Object);
            this.factory.Setup(x => x.Create<ISocketFacade>(this.family, SocketType.Dgram, ProtocolType.Udp)).Returns(this.socket.Object);

            this.resolver.Setup(x => x.GetAddressFamily(this.address)).Returns(this.family);
            this.resolver.Setup(x => x.GetRemoteEndPoint(this.host, this.family, this.port)).Returns(this.target);
        }

        #endregion

        #region Construction

        [TestCase(typeof(INetworkInternalFactory))]
        [TestCase(typeof(INetworkLoggerSettings))]
        public void UdpClientSocket_DependencyIsNull_ThrowsArgumentNullException(Type dependency)
        {
            this.factory = dependency == typeof(INetworkInternalFactory) ? null : this.factory;
            this.settings = dependency == typeof(INetworkLoggerSettings) ? null : this.settings;

            Assert.That(() => this.CreateInstance(), Throws.ArgumentNullException);
        }

        [Test]
        public void UdpClientSocket_DependencyIsValid_NetworkInternalFactoryCreateWasCalledWithWithExpectedTypes()
        {
            this.CreateInstance();

            this.factory.Verify(x => x.Create<IResolverFacade>(), Times.Once());
        }

        #endregion

        #region Connect

        [Test]
        public void Connect_IResolverFacade_GetAddressFamilyWasCalledAsExpected()
        {
            this.CreateInstance().Connect();

            this.resolver.Verify(x => x.GetAddressFamily(this.address), Times.Once());
        }

        [Test]
        public void Connect_INetworkInternalFactory_CreateWasCalledAsExpected()
        {
            this.CreateInstance().Connect();

            this.factory.Verify(x => x.Create<ISocketFacade>(this.family, SocketType.Dgram, ProtocolType.Udp), Times.Once());
        }

        [Test]
        public void Connect_IResolverFacade_GetRemoteEndPointWasCalledAsExpected()
        {
            this.CreateInstance().Connect();

            this.resolver.Verify(x => x.GetRemoteEndPoint(this.host, this.family, this.port), Times.Once());
        }

        [Test]
        public void Connect_ISocketFacade_ConnectWasCalledAsExpected()
        {
            this.CreateInstance().Connect();

            this.socket.Verify(x => x.Connect(this.target), Times.Once());
        }

        [Test]
        public void Connect_CallOnce_ResultIsTrue()
        {
            Assert.That(this.CreateInstance().Connect(), Is.True);
        }

        [Test]
        public void Connect_CallTwice_ResultIsTrueAndDependenciesWereCalledOnce()
        {
            IUdpClientSocket instance = this.CreateInstance();

            Assert.That(instance.Connect(), Is.True);
            Assert.That(instance.Connect(), Is.True);

            this.resolver.Verify(x => x.GetAddressFamily(It.IsAny<Address>()), Times.Once());
            this.factory.Verify(x => x.Create<It.IsAnyType>(It.IsAny<AddressFamily>(), It.IsAny<SocketType>(), It.IsAny<ProtocolType>()), Times.Once());
            this.resolver.Verify(x => x.GetRemoteEndPoint(It.IsAny<String>(), It.IsAny<AddressFamily>(), It.IsAny<Int32>()), Times.Once());
            this.socket.Verify(x => x.Connect(It.IsAny<IPEndPoint>()), Times.Once());
        }

        [Test]
        public void Connect_IResolverFacadeGetAddressFamilyThrowsException_ResultIsFalse()
        {
            this.resolver.Setup(x => x.GetAddressFamily(It.IsAny<Address>())).Throws<Exception>();

            Assert.That(this.CreateInstance().Connect(), Is.False);
        }

        [Test]
        public void Connect_IResolverFacadeGetRemoteEndPointThrowsException_ISocketFacadeDisposeWasCalledOnce()
        {
            this.resolver.Setup(x => x.GetRemoteEndPoint(It.IsAny<String>(), It.IsAny<AddressFamily>(), It.IsAny<Int32>())).Throws<Exception>();

            this.CreateInstance().Connect();

            this.socket.Verify(x => x.Dispose(), Times.Once());
        }

        #endregion

        #region Send

        [Test]
        public void Send_NotConnected_ResultIsZero()
        {
            Byte[] payload = new Byte[10];

            IUdpClientSocket instance = this.CreateInstance();

            Int32 actual = instance.Send(payload);

            Assert.That(actual, Is.Zero);
        }

        [Test]
        public void Send_PayloadIsNull_ResultIsZero()
        {
            Byte[] payload = null;

            IUdpClientSocket instance = this.CreateInstance();

            instance.Connect();

            Int32 actual = instance.Send(payload);

            Assert.That(actual, Is.Zero);
        }

        [Test]
        public void Send_PayloadIsEmpty_ResultIsZero()
        {
            Byte[] payload = new Byte[0];

            IUdpClientSocket instance = this.CreateInstance();

            instance.Connect();

            Int32 actual = instance.Send(payload);

            Assert.That(actual, Is.Zero);
        }

        [Test]
        public void Send_ISocketFacadeSendThrowsException_ResultIsZero()
        {
            Byte[] payload = new Byte[10];

            this.socket.Setup(x => x.Send(It.IsAny<Byte[]>())).Throws<Exception>();

            IUdpClientSocket instance = this.CreateInstance();

            instance.Connect();

            Int32 actual = instance.Send(payload);

            Assert.That(actual, Is.Zero);
        }

        [Test]
        public void Send_ValidInput_ISocketFacadeSendWasCalledOnce()
        {
            Byte[] payload = new Byte[10];

            IUdpClientSocket instance = this.CreateInstance();

            instance.Connect();

            instance.Send(payload);

            this.socket.Verify(x => x.Send(payload), Times.Once());
        }

        #endregion

        #region Dispose

        [Test]
        public void Dispose_NotConnected_ISocketFacadeDisposeWasNeverCalled()
        {
            this.CreateInstance().Dispose();

            this.socket.Verify(x => x.Dispose(), Times.Never());
        }

        [Test]
        public void Dispose_ConnectedAndDisposedOnce_ISocketFacadeDisposeWasCalledOnce()
        {
            IUdpClientSocket instance = this.CreateInstance();

            instance.Connect();
            instance.Dispose();

            this.socket.Verify(x => x.Dispose(), Times.Once());
        }

        [Test]
        public void Dispose_ConnectedAndDisposedTwice_ISocketFacadeDisposeWasCalledOnce()
        {
            IUdpClientSocket instance = this.CreateInstance();

            instance.Connect();
            instance.Dispose();
            instance.Dispose();

            this.socket.Verify(x => x.Dispose(), Times.Once());
        }

        #endregion

        #region Private helpers

        private IUdpClientSocket CreateInstance()
        {
            return new UdpClientSocket(
                this.factory?.Object,
                this.settings?.Object
            );
        }

        #endregion
    }
}
