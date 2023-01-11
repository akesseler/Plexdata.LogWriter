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
using System;
using System.Net;
using System.Net.Sockets;

namespace Plexdata.LogWriter.Internals.Abstraction
{
    /// <summary>
    /// An interface to allow physical data transmission.
    /// </summary>
    /// <remarks>
    /// This interface represents the abstraction of a 
    /// facade of a TCP socket client.
    /// </remarks>
    internal interface ITcpClientSocket : IDisposable
    {
        /// <summary>
        /// Opens the socket.
        /// </summary>
        /// <remarks>
        /// This method opens the socket using current settings, but only if needed.
        /// </remarks>
        /// <returns>
        /// True if open operation was successful and false otherwise.
        /// </returns>
        /// <seealso cref="INetworkInternalFactory.Create{TInterface}(Object[])"/>
        /// <seealso cref="IResolverFacade.GetRemoteEndPoint(String, AddressFamily, Int32)"/>
        /// <seealso cref="IResolverFacade.GetAddressFamily(Address)"/>
        /// <seealso cref="ISocketFacade.Connect(IPEndPoint)"/>
        Boolean Open();

        /// <summary>
        /// Sends the payload via connected socket.
        /// </summary>
        /// <remarks>
        /// This method sends the payload via connected socket.
        /// </remarks>
        /// <param name="payload">
        /// The payload to send.
        /// </param>
        /// <returns>
        /// The number of bytes sent. Zero is returned if either the socket is not yet 
        /// connected, or the payload is null or empty, or an exception was thrown.
        /// </returns>
        Int32 Send(Byte[] payload);

        /// <summary>
        /// Closes the socket.
        /// </summary>
        /// <remarks>
        /// This method closes the socket.
        /// </remarks>
        void Close();
    }
}
