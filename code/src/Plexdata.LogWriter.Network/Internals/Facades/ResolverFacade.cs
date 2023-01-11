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

using Plexdata.LogWriter.Definitions;
using Plexdata.LogWriter.Internals.Abstraction;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net;
using System.Net.Sockets;

namespace Plexdata.LogWriter.Internals.Facades
{
    /// <summary>
    /// A class allowing to resolve specific network information.
    /// </summary>
    /// <remarks>
    /// This class represents the implementation of a facade to obtain 
    /// specific network information.
    /// </remarks>
    [ExcludeFromCodeCoverage]
    internal class ResolverFacade : IResolverFacade
    {
        #region Construction

        /// <summary>
        /// The standard class constructor.
        /// </summary>
        /// <remarks>
        /// This constructor just initializes all properties with its default values.
        /// </remarks>
        public ResolverFacade()
            : base()
        {
        }

        #endregion

        #region Public methods

        /// <inheritdoc />
        public String GetRemoteHostName(String host)
        {
            return Dns.GetHostEntry(host).HostName;
        }

        /// <inheritdoc />
        public AddressFamily GetAddressFamily(Address address)
        {
            switch (address)
            {
                case Address.IPv4:
                    return AddressFamily.InterNetwork;
                case Address.IPv6:
                    return AddressFamily.InterNetworkV6;
                default:
                    throw new NotSupportedException($"Value '{address}' of parameter '{nameof(address)}' is not supported.");
            }
        }

        /// <inheritdoc />
        public IPAddress GetRemoteAddresses(String host, AddressFamily family)
        {
            return Dns.GetHostAddresses(this.GetRemoteHostName(host)).FirstOrDefault(x => x.AddressFamily == family);
        }

        /// <inheritdoc />
        public IPEndPoint GetRemoteEndPoint(IPAddress address, Int32 port)
        {
            return new IPEndPoint(address, port);
        }

        /// <inheritdoc />
        public IPEndPoint GetRemoteEndPoint(String host, AddressFamily family, Int32 port)
        {
            return new IPEndPoint(this.GetRemoteAddresses(host, family), port);
        }

        #endregion
    }
}
