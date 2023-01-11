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

using Plexdata.LogWriter.Abstraction;
using Plexdata.LogWriter.Internals.Abstraction;
using Plexdata.LogWriter.Internals.Facades;
using Plexdata.LogWriter.Internals.Helpers;
using Plexdata.LogWriter.Internals.Sockets;
using Plexdata.LogWriter.Internals.Writers;
using System;
using System.Net.Sockets;

namespace Plexdata.LogWriter.Internals.Factories
{
    /// <summary>
    /// A class to create internal dependencies.
    /// </summary>
    /// <remarks>
    /// This class represents the implementation of a 
    /// factory to create internal dependencies.
    /// </remarks>
    internal class NetworkInternalFactory : INetworkInternalFactory
    {
        #region Construction

        /// <summary>
        /// The standard constructor.
        /// </summary>
        /// <remarks>
        /// This constructor initialize a new instance of this class.
        /// </remarks>
        public NetworkInternalFactory()
            : base()
        {
        }

        #endregion

        #region Public methods

        /// <inheritdoc/>
        public TInterface Create<TInterface>(params Object[] options)
        {
            if (typeof(TInterface) == typeof(IGZipFacade))
            {
                return (TInterface)(IGZipFacade)new GZipFacade();
            }

            if (typeof(TInterface) == typeof(IRandomFacade))
            {
                return (TInterface)(IRandomFacade)new RandomFacade();
            }

            if (typeof(TInterface) == typeof(IResolverFacade))
            {
                return (TInterface)(IResolverFacade)new ResolverFacade();
            }

            if (typeof(TInterface) == typeof(ISocketFacade))
            {
                return (TInterface)(ISocketFacade)new SocketFacade((AddressFamily)options[0], (SocketType)options[1], (ProtocolType)options[2]);
            }

            if (typeof(TInterface) == typeof(IClientFacade))
            {
                return (TInterface)(IClientFacade)new ClientFacade((Uri)options[0], (String)options[1], (String)options[2], (Int32)options[3]);
            }

            throw new NotSupportedException($"Type of interface '{typeof(TInterface)}' is not supported by this factory.");
        }

        /// <inheritdoc/>
        public TInterface Create<TInterface>(INetworkLoggerSettings settings)
        {
            if (typeof(TInterface) == typeof(IUdpChunkHelper))
            {
                return (TInterface)(IUdpChunkHelper)new UdpChunkHelper(this, settings);
            }

            if (typeof(TInterface) == typeof(IUdpNetworkWriter))
            {
                return (TInterface)(IUdpNetworkWriter)new UdpNetworkWriter(this, settings);
            }

            if (typeof(TInterface) == typeof(IUdpClientSocket))
            {
                return (TInterface)(IUdpClientSocket)new UdpClientSocket(this, settings);
            }

            if (typeof(TInterface) == typeof(ITcpNetworkWriter))
            {
                return (TInterface)(ITcpNetworkWriter)new TcpNetworkWriter(this, settings);
            }

            if (typeof(TInterface) == typeof(ITcpClientSocket))
            {
                return (TInterface)(ITcpClientSocket)new TcpClientSocket(this, settings);
            }

            if (typeof(TInterface) == typeof(IWebNetworkWriter))
            {
                return (TInterface)(IWebNetworkWriter)new WebNetworkWriter(this, settings);
            }

            if (typeof(TInterface) == typeof(IWebClientSocket))
            {
                return (TInterface)(IWebClientSocket)new WebClientSocket(this, settings);
            }

            throw new NotSupportedException($"Type of interface '{typeof(TInterface)}' is not supported by this factory.");
        }

        #endregion
    }
}
