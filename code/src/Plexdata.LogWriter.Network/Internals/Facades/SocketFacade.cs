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

using Plexdata.LogWriter.Internals.Abstraction;
using Plexdata.LogWriter.Internals.Extensions;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Net.Sockets;

namespace Plexdata.LogWriter.Internals.Facades
{
    /// <summary>
    /// A class to allow physical data transmission.
    /// </summary>
    /// <remarks>
    /// This class represents the implementation of a 
    /// facade of a UDP or TCP client.
    /// </remarks>
    [ExcludeFromCodeCoverage]
    internal class SocketFacade : ISocketFacade
    {
        #region Private fields

        /// <summary>
        /// The native socket.
        /// </summary>
        /// <remarks>
        /// This field holds the used native socket.
        /// </remarks>
        private Socket socket = null;

        #endregion

        #region Construction

        /// <summary>
        /// The class constructor.
        /// </summary>
        /// <remarks>
        /// This constructor initializes its dependencies.
        /// </remarks>
        /// <param name="family">
        /// The address family to be used.
        /// </param>
        /// <param name="type">
        /// The socket type to be used.
        /// </param>
        /// <param name="protocol">
        /// The protocol type to be used.
        /// </param>
        public SocketFacade(AddressFamily family, SocketType type, ProtocolType protocol)
            : base()
        {
            this.socket = new Socket(family, type, protocol);
        }

        #endregion

        #region Public methods

        /// <inheritdoc />
        public void Connect(IPEndPoint target)
        {
            this.socket.Connect(target);
        }

        /// <inheritdoc />
        public Int32 Send(Byte[] payload)
        {
            return this.socket.Send(payload);
        }

        /// <inheritdoc />
        public void Close()
        {
            this.socket.Close();
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
