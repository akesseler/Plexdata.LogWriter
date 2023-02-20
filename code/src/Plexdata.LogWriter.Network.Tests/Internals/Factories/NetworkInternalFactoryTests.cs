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
using Plexdata.LogWriter.Internals.Facades;
using Plexdata.LogWriter.Internals.Factories;
using Plexdata.LogWriter.Internals.Helpers;
using Plexdata.LogWriter.Internals.Sockets;
using Plexdata.LogWriter.Internals.Writers;
using Plexdata.Utilities.Testing;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Net.Sockets;
using System.Runtime.Serialization;

namespace Plexdata.LogWriter.Network.Tests.Internals.Factories
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    [Category(TestType.IntegrationTest)]
    [TestOf(nameof(NetworkInternalFactory))]
    internal class NetworkInternalFactoryTests
    {
        private Mock<INetworkLoggerSettings> settings;

        [SetUp]
        public void Setup()
        {
            this.settings = new Mock<INetworkLoggerSettings>();
        }

        [Test]
        public void Create_WithOptionsIGZipFacade_ResultIsInstanceOfGZipFacade()
        {
            IGZipFacade actual = this.CreateInstance().Create<IGZipFacade>();
            Assert.That(actual, Is.InstanceOf<GZipFacade>());
        }

        [Test]
        public void Create_WithOptionsIRandomFacade_ResultIsInstanceOfRandomFacade()
        {
            IRandomFacade actual = this.CreateInstance().Create<IRandomFacade>();
            Assert.That(actual, Is.InstanceOf<RandomFacade>());
        }

        [Test]
        public void Create_WithOptionsIResolverFacade_ResultIsInstanceOfResolverFacade()
        {
            IResolverFacade actual = this.CreateInstance().Create<IResolverFacade>();
            Assert.That(actual, Is.InstanceOf<ResolverFacade>());
        }

        [Test]
        public void Create_WithOptionsISocketFacade_ResultIsInstanceOfSocketFacade()
        {
            using (ISocketFacade actual = this.CreateInstance().Create<ISocketFacade>(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp))
            {
                Assert.That(actual, Is.InstanceOf<SocketFacade>());
            }
        }

        [Test]
        public void Create_WithOptionsIClientFacade_ResultIsInstanceOfClientFacade()
        {
            using (IClientFacade actual = this.CreateInstance().Create<IClientFacade>(new Uri("http://localhost"), "POST", "application/json", 1234))
            {
                Assert.That(actual, Is.InstanceOf<ClientFacade>());
            }
        }

        [Test]
        public void Create_WithOptionsISerializable_ThrowsNotSupportedException()
        {
            Assert.That(() => this.CreateInstance().Create<ISerializable>(), Throws.InstanceOf<NotSupportedException>());
        }

        [Test]
        public void Create_WithSettingsIUdpChunkHelper_ResultIsInstanceOfUdpChunkHelper()
        {
            IUdpChunkHelper actual = this.CreateInstance().Create<IUdpChunkHelper>(this.settings.Object);
            Assert.That(actual, Is.InstanceOf<UdpChunkHelper>());
        }

        [Test]
        public void Create_WithSettingsIUdpNetworkWriter_ResultIsInstanceOfUdpNetworkWriter()
        {
            using (IUdpNetworkWriter actual = this.CreateInstance().Create<IUdpNetworkWriter>(this.settings.Object))
            {
                Assert.That(actual, Is.InstanceOf<UdpNetworkWriter>());
            }
        }

        [Test]
        public void Create_WithSettingsIUdpClientSocket_ResultIsInstanceOfUdpClientSocket()
        {
            using (IUdpClientSocket actual = this.CreateInstance().Create<IUdpClientSocket>(this.settings.Object))
            {
                Assert.That(actual, Is.InstanceOf<UdpClientSocket>());
            }
        }

        [Test]
        public void Create_WithSettingsITcpNetworkWriter_ResultIsInstanceOfTcpNetworkWriter()
        {
            using (ITcpNetworkWriter actual = this.CreateInstance().Create<ITcpNetworkWriter>(this.settings.Object))
            {
                Assert.That(actual, Is.InstanceOf<TcpNetworkWriter>());
            }
        }

        [Test]
        public void Create_WithSettingsITcpClientSocket_ResultIsInstanceOfTcpClientSocket()
        {
            using (ITcpClientSocket actual = this.CreateInstance().Create<ITcpClientSocket>(this.settings.Object))
            {
                Assert.That(actual, Is.InstanceOf<TcpClientSocket>());
            }
        }

        [Test]
        public void Create_WithSettingsIWebNetworkWriter_ResultIsInstanceOfWebNetworkWriter()
        {
            using (IWebNetworkWriter actual = this.CreateInstance().Create<IWebNetworkWriter>(this.settings.Object))
            {
                Assert.That(actual, Is.InstanceOf<WebNetworkWriter>());
            }
        }

        [Test]
        public void Create_WithSettingsIWebClientSocket_ResultIsInstanceOfWebClientSocket()
        {
            using (IWebClientSocket actual = this.CreateInstance().Create<IWebClientSocket>(this.settings.Object))
            {
                Assert.That(actual, Is.InstanceOf<WebClientSocket>());
            }
        }

        [Test]
        public void Create_WithSettingsISerializable_ThrowsNotSupportedException()
        {
            Assert.That(() => this.CreateInstance().Create<ISerializable>(this.settings.Object), Throws.InstanceOf<NotSupportedException>());
        }

        private INetworkInternalFactory CreateInstance()
        {
            return new NetworkInternalFactory();
        }
    }
}
