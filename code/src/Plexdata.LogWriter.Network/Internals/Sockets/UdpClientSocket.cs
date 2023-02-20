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

using Plexdata.LogWriter.Abstraction;
using Plexdata.LogWriter.Internals.Abstraction;
using Plexdata.LogWriter.Internals.Extensions;
using System;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;

namespace Plexdata.LogWriter.Internals.Sockets
{
    /// <summary>
    /// A class to allow physical data transmission.
    /// </summary>
    /// <remarks>
    /// This class represents the implementation of a 
    /// facade of a UDP socket client.
    /// </remarks>
    internal class UdpClientSocket : IUdpClientSocket
    {
        #region Private fields

        /// <summary>
        /// The internal factory to create required dependencies.
        /// </summary>
        /// <remarks>
        /// This field holds the internal factory to be able to create needed dependencies.
        /// </remarks>
        private readonly INetworkInternalFactory factory;

        /// <summary>
        /// An instance of the settings to use.
        /// </summary>
        /// <remarks>
        /// This field holds an instance of the used settings.
        /// </remarks>
        private readonly INetworkLoggerSettings settings;

        /// <summary>
        /// An instance of the resolver facade.
        /// </summary>
        /// <remarks>
        /// This field holds an instance of the resolver created through the factory.
        /// </remarks>
        private readonly IResolverFacade resolver;

        /// <summary>
        /// An instance of the socket facade.
        /// </summary>
        /// <remarks>
        /// This field holds an instance of the socket created through the factory.
        /// </remarks>
        private ISocketFacade socket = null;

        #endregion

        #region Construction

        /// <summary>
        /// The only class constructor.
        /// </summary>
        /// <remarks>
        /// This constructor initializes all its fields and properties.
        /// </remarks>
        /// <param name="factory">
        /// An instance of the factory to use.
        /// </param>
        /// <param name="settings">
        /// An instance of the settings to use.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// This exception is thrown if one of the parameters is <c>null</c>.
        /// </exception>
        public UdpClientSocket(INetworkInternalFactory factory, INetworkLoggerSettings settings)
            : base()
        {
            this.factory = factory ?? throw new ArgumentNullException(nameof(factory));
            this.settings = settings ?? throw new ArgumentNullException(nameof(settings));

            this.resolver = this.factory.Create<IResolverFacade>();
        }

        #endregion

        #region Public methods

        /// <inheritdoc />
        public Boolean Connect()
        {
            if (this.socket != null)
            {
                return true;
            }

            try
            {
                AddressFamily family = this.resolver.GetAddressFamily(this.settings.Address);

                this.socket = this.factory.Create<ISocketFacade>(family, SocketType.Dgram, ProtocolType.Udp);

                IPEndPoint target = this.resolver.GetRemoteEndPoint(this.settings.Host, family, this.settings.Port);

                this.socket.Connect(target);

                return true;
            }
            catch (Exception exception)
            {
                this.socket.SafeDispose();
                this.socket = null;

                Debug.WriteLine(exception);
                return false;
            }
        }

        /// <inheritdoc />
        public Int32 Send(Byte[] payload)
        {
            if (this.socket == null)
            {
                return 0;
            }

            if (!payload?.Any() ?? true)
            {
                return 0;
            }

            try
            {
                return this.socket.Send(payload);
            }
            catch (Exception exception)
            {
                Debug.WriteLine(exception);
                return 0;
            }
        }

        /// <summary>
        /// Frees all disposable instance resources.
        /// </summary>
        /// <remarks>
        /// This method frees all disposable instance resources.
        /// </remarks>
        public void Dispose()
        {
            this.socket.SafeDispose();
            this.socket = null;
        }

        #endregion
    }
}
