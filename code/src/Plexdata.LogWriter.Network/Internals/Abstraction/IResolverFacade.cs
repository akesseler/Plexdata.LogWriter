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

using Plexdata.LogWriter.Definitions;
using System;
using System.Net;
using System.Net.Sockets;

namespace Plexdata.LogWriter.Internals.Abstraction
{
    /// <summary>
    /// An interface allowing to resolve specific network information.
    /// </summary>
    /// <remarks>
    /// This interface represents the abstraction of a facade to obtain 
    /// specific network information.
    /// </remarks>
    internal interface IResolverFacade
    {
        /// <summary>
        /// Tries to determine remote host name.
        /// </summary>
        /// <remarks>
        /// This method tries to determine remote host name using class 
        /// <see cref="Dns"/>.
        /// </remarks>
        /// <param name="host">
        /// The host name or IP address to resolve.
        /// </param>
        /// <returns>
        /// A string containing the primary host name for the server.
        /// </returns>
        /// <exception cref="Exception">
        /// Any exception thrown by <see cref="Dns.GetHostEntry(String)"/>.
        /// </exception>
        /// <seealso cref="Dns.GetHostEntry(String)"/>
        /// <seealso cref="IPHostEntry.HostName"/>
        String GetRemoteHostName(String host);

        /// <summary>
        /// Just maps provided <see cref="Address"/> into its <see cref="AddressFamily"/> 
        /// representation.
        /// </summary>
        /// <remarks>
        /// This method does nothing else but to map provided <see cref="Address"/> into 
        /// its <see cref="AddressFamily"/> representation.
        /// </remarks>
        /// <param name="address">
        /// The address type to map.
        /// </param>
        /// <returns>
        /// The mapped address type.
        /// </returns>
        /// <exception cref="NotSupportedException">
        /// This exception is thrown if provided address type is not supported.
        /// </exception>
        AddressFamily GetAddressFamily(Address address);

        /// <summary>
        /// Tries to determine the remote IP address.
        /// </summary>
        /// <remarks>
        /// This method tries to determine the remote IP address.
        /// </remarks>
        /// <param name="host">
        /// The host name or IP address of the remote system.
        /// </param>
        /// <param name="family">
        /// The address family to determine an IP address for.
        /// </param>
        /// <returns>
        /// An instance of <see cref="IPAddress"/> representing the remote system.
        /// </returns>
        /// <exception cref="Exception">
        /// Any exception thrown by <see cref="Dns.GetHostEntry(String)"/> or by 
        /// <see cref="Dns.GetHostAddresses(String)"/>.
        /// </exception>
        /// <seealso cref="GetRemoteHostName(String)"/>
        /// <seealso cref="Dns.GetHostAddresses(String)"/>
        IPAddress GetRemoteAddresses(String host, AddressFamily family);

        /// <summary>
        /// Tries to create an instance of <see cref="IPEndPoint"/>.
        /// </summary>
        /// <remarks>
        /// This method just tries to create an instance of <see cref="IPEndPoint"/>.
        /// </remarks>
        /// <param name="address">
        /// An instance of <see cref="IPAddress"/> representing the remote system.
        /// </param>
        /// <param name="port">
        /// The port to transmit data through.
        /// </param>
        /// <returns>
        /// An instance of <see cref="IPEndPoint"/> representing the remote endpoint.
        /// </returns>
        /// <exception cref="Exception">
        /// Any exception thrown by <see cref="IPEndPoint(IPAddress, Int32)"/>.
        /// </exception>
        IPEndPoint GetRemoteEndPoint(IPAddress address, Int32 port);

        /// <summary>
        /// Tries to create an instance of <see cref="IPEndPoint"/>.
        /// </summary>
        /// <remarks>
        /// This method does actually the same as method <see cref="GetRemoteEndPoint(IPAddress, Int32)"/> 
        /// does but from parameter host instead.
        /// </remarks>
        /// <param name="host">
        /// The host name or IP address of the remote system.
        /// </param>
        /// <param name="family">
        /// The address family to determine an IP address for.
        /// </param>
        /// <param name="port">
        /// The port to transmit data through.
        /// </param>
        /// <returns>
        /// An instance of <see cref="IPEndPoint"/> representing the remote endpoint.
        /// </returns>
        /// <exception cref="Exception">
        /// Any exception thrown by <see cref="Dns.GetHostEntry(String)"/> 
        /// or by <see cref="Dns.GetHostAddresses(String)"/> 
        /// or by <see cref="GetRemoteEndPoint(IPAddress, Int32)"/>.
        /// </exception>
        /// <seealso cref="GetRemoteEndPoint(IPAddress, Int32)"/>
        /// <seealso cref="GetRemoteAddresses(String, AddressFamily)"/>
        IPEndPoint GetRemoteEndPoint(String host, AddressFamily family, Int32 port);
    }
}
