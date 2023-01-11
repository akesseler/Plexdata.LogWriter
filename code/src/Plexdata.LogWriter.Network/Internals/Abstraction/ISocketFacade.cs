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

using System;
using System.Net;

namespace Plexdata.LogWriter.Internals.Abstraction
{
    /// <summary>
    /// An interface to allow physical data transmission.
    /// </summary>
    /// <remarks>
    /// This interface represents the abstraction of a 
    /// facade of a UDP or TCP client.
    /// </remarks>
    internal interface ISocketFacade : IDisposable
    {
        /// <summary>
        /// Connects to an instance of <see cref="IPEndPoint"/>.
        /// </summary>
        /// <remarks>
        /// This method just calls <see cref="System.Net.Sockets.Socket.Connect(EndPoint)"/>.
        /// </remarks>
        /// <param name="target">
        /// An endpoint that represents the remote device.
        /// </param>
        /// <exception cref="Exception">
        /// Any exception thrown by <see cref="System.Net.Sockets.Socket.Connect(EndPoint)"/>.
        /// </exception>
        void Connect(IPEndPoint target);

        /// <summary>
        /// Sends data to a connected remote socket.
        /// </summary>
        /// <remarks>
        /// This method just calls <see cref="System.Net.Sockets.Socket.Send(Byte[])"/>.
        /// </remarks>
        /// <param name="payload">
        /// The payload to send.
        /// </param>
        /// <returns>
        /// The number of bytes sent to the remote socket.
        /// </returns>
        /// <exception cref="Exception">
        /// Any exception thrown by <see cref="System.Net.Sockets.Socket.Send(Byte[])"/>.
        /// </exception>
        Int32 Send(Byte[] payload);

        /// <summary>
        /// Closes the remote socket connection and releases all associated resources.
        /// </summary>
        /// <remarks>
        /// This method just calls <see cref="System.Net.Sockets.Socket.Close()"/>.
        /// </remarks>
        void Close();
    }
}
