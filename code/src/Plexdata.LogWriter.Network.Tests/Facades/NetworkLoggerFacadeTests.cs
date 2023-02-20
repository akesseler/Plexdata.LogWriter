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
using Plexdata.LogWriter.Definitions;
using Plexdata.LogWriter.Facades;
using Plexdata.LogWriter.Internals.Abstraction;
using Plexdata.Utilities.Testing;
using System;
using System.Diagnostics.CodeAnalysis;

namespace Plexdata.LogWriter.Network.Tests.Facades
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    [Category(TestType.UnitTest)]
    [TestOf(nameof(NetworkLoggerFacade))]
    internal class NetworkLoggerFacadeTests
    {
        #region Prologue

        private Mock<INetworkLoggerSettings> settings;
        private Mock<INetworkInternalFactory> factory;
        private Mock<IUdpNetworkWriter> udpWriter;
        private Mock<ITcpNetworkWriter> tcpWriter;
        private Mock<IWebNetworkWriter> webWriter;
        private Protocol protocol;

        [SetUp]
        public void SetUp()
        {
            this.settings = new Mock<INetworkLoggerSettings>();
            this.factory = new Mock<INetworkInternalFactory>();
            this.udpWriter = new Mock<IUdpNetworkWriter>();
            this.tcpWriter = new Mock<ITcpNetworkWriter>();
            this.webWriter = new Mock<IWebNetworkWriter>();

            this.factory.Setup(x => x.Create<IUdpNetworkWriter>(It.IsAny<INetworkLoggerSettings>())).Returns(this.udpWriter.Object);
            this.factory.Setup(x => x.Create<ITcpNetworkWriter>(It.IsAny<INetworkLoggerSettings>())).Returns(this.tcpWriter.Object);
            this.factory.Setup(x => x.Create<IWebNetworkWriter>(It.IsAny<INetworkLoggerSettings>())).Returns(this.webWriter.Object);

            this.protocol = Protocol.Unknown;
            this.settings.SetupGet(x => x.Protocol).Returns(() => this.protocol);
        }

        #endregion

        #region Construction

        [Test]
        public void NetworkLoggerFacade_StandardConstructor_ThrowsNothing()
        {
            Assert.That(() => new NetworkLoggerFacade(), Throws.Nothing);
        }

        [Test]
        public void NetworkLoggerFacade_INetworkInternalFactoryIsNull_ThrowsArgumentNullException()
        {
            this.factory = null;

            Assert.That(() => this.CreateInstance(), Throws.ArgumentNullException);
        }

        #endregion

        #region IsDisposed

        [Test]
        public void IsDisposed_DisposeInstance_PropertySetAsExpected()
        {
            INetworkLoggerFacade instance = this.CreateInstance();

            Assert.That(instance.IsDisposed, Is.False);

            instance.Dispose();

            Assert.That(instance.IsDisposed, Is.True);

            instance.Dispose();

            Assert.That(instance.IsDisposed, Is.True);
        }

        #endregion

        #region ApplySettings

        [Test]
        public void ApplySettings_ProtocolUnknown_ThrowsNotSupportedException()
        {
            this.protocol = Protocol.Unknown;

            Assert.That(() => this.CreateInstance().ApplySettings(this.settings.Object), Throws.InstanceOf<NotSupportedException>());
        }

        [TestCase(Protocol.Udp, 1, 0, 0)]
        [TestCase(Protocol.Tcp, 0, 1, 0)]
        [TestCase(Protocol.Web, 0, 0, 1)]
        public void ApplySettings_ProtocolAsProvided_FactoryCalledAsExpected(Protocol protocol, Int32 udpCalls, Int32 tcpCalls, Int32 webCalls)
        {
            this.protocol = protocol;

            this.CreateInstance().ApplySettings(this.settings.Object);

            this.factory.Verify(x => x.Create<IUdpNetworkWriter>(this.settings.Object), Times.Exactly(udpCalls));
            this.factory.Verify(x => x.Create<ITcpNetworkWriter>(this.settings.Object), Times.Exactly(tcpCalls));
            this.factory.Verify(x => x.Create<IWebNetworkWriter>(this.settings.Object), Times.Exactly(webCalls));
        }

        [Test]
        public void ApplySettings_SettingsAppliedTwice_OldWriterDisposedAsExpected()
        {
            this.protocol = Protocol.Udp;

            Mock<IUdpNetworkWriter> oldWriter = new Mock<IUdpNetworkWriter>();
            Mock<IUdpNetworkWriter> newWriter = new Mock<IUdpNetworkWriter>();

            INetworkLoggerFacade instance = this.CreateInstance();

            this.factory.Setup(x => x.Create<IUdpNetworkWriter>(this.settings.Object)).Returns(oldWriter.Object);

            instance.ApplySettings(this.settings.Object);

            this.factory.Setup(x => x.Create<IUdpNetworkWriter>(this.settings.Object)).Returns(newWriter.Object);

            instance.ApplySettings(this.settings.Object);

            oldWriter.Verify(x => x.Dispose(), Times.Once());
        }

        [Test]
        public void ApplySettings_SettingsAppliedTwiceButDisposed_WriterDisposedOnlyOnce()
        {
            this.protocol = Protocol.Udp;

            INetworkLoggerFacade instance = this.CreateInstance();

            instance.ApplySettings(this.settings.Object);

            instance.Dispose();

            this.udpWriter.Verify(x => x.Dispose(), Times.Once());

            instance.ApplySettings(this.settings.Object);

            this.udpWriter.Verify(x => x.Dispose(), Times.Once());
        }

        #endregion

        #region Write

        [Test]
        public void Write_InstanceNotDisposed_INetworkWriterWriteCalledOnce()
        {
            this.protocol = Protocol.Udp;

            INetworkLoggerFacade instance = this.CreateInstance();

            instance.ApplySettings(this.settings.Object);

            instance.Write("message");

            this.udpWriter.Verify(x => x.Write("message"), Times.Once());
        }

        [Test]
        public void Write_InstanceDisposed_INetworkWriterWriteNeverCalled()
        {
            this.protocol = Protocol.Udp;

            INetworkLoggerFacade instance = this.CreateInstance();

            instance.ApplySettings(this.settings.Object);

            instance.Dispose();

            instance.Write("message");

            this.udpWriter.Verify(x => x.Write("message"), Times.Never());
        }

        [Test]
        public void Write_INetworkWriterWriteThrowsException_INetworkLoggerFacadeWriteThrowsNothing()
        {
            this.protocol = Protocol.Udp;

            this.udpWriter.Setup(x => x.Write(It.IsAny<String>())).Throws<Exception>();

            INetworkLoggerFacade instance = this.CreateInstance();

            instance.ApplySettings(this.settings.Object);

            Assert.That(() => instance.Write("message"), Throws.Nothing);
        }

        #endregion

        #region Dispose

        [Test]
        public void Dispose_InstanceDisposedTwice_INetworkWriterDisposedOnlyOnce()
        {
            this.protocol = Protocol.Udp;

            INetworkLoggerFacade instance = this.CreateInstance();

            instance.ApplySettings(this.settings.Object);

            instance.Dispose();
            instance.Dispose();

            this.udpWriter.Verify(x => x.Dispose(), Times.Once());
        }

        #endregion

        #region Private helpers

        private INetworkLoggerFacade CreateInstance()
        {
            return new NetworkLoggerFacade(
                this.factory?.Object
            );
        }

        #endregion
    }
}
