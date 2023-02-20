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

namespace Plexdata.LogWriter.Internals.Writers
{
    /// <summary>
    /// A class allowing data transmission via network.
    /// </summary>
    /// <remarks>
    /// This class represents the implementation of a 
    /// TCP network writer.
    /// </remarks>
    internal class TcpNetworkWriter : ITcpNetworkWriter
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
        /// An instance of the TCP client socket.
        /// </summary>
        /// <remarks>
        /// This field holds an instance of the TCP client socket created through the factory.
        /// </remarks>
        private readonly ITcpClientSocket client;

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
        public TcpNetworkWriter(INetworkInternalFactory factory, INetworkLoggerSettings settings)
            : base()
        {
            this.factory = factory ?? throw new ArgumentNullException(nameof(factory));
            this.settings = settings ?? throw new ArgumentNullException(nameof(settings));

            this.client = this.factory.Create<ITcpClientSocket>(this.settings);
        }

        #endregion

        #region Public properties

        /// <inheritdoc />
        public Boolean IsDisposed { get; private set; }

        #endregion

        #region Public methods

        /// <summary>
        /// Writes provided message using underlying client connection.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This method writes provided message using underlying client 
        /// connection.
        /// </para>
        /// <para>
        /// Nothing happens either if this instance is already disposed, or 
        /// if the message is invalid, or if open the underlying client has 
        /// failed for any reason.
        /// </para>
        /// <para>
        /// Furthermore, a zero termination is appended to the message, but only if 
        /// <see cref="INetworkLoggerSettings.Termination"/> is enabled and the message 
        /// does not yet end with a zero termination.
        /// </para>
        /// <para>
        /// Finally note, the client connection is closed automatically if the message 
        /// end with a zero termination.
        /// </para>
        /// </remarks>
        /// <param name="message">
        /// The message to write.
        /// </param>
        /// <seealso cref="INetworkLoggerSettings.Termination"/>
        /// <seealso cref="INetworkLoggerSettings.Encoding"/>
        /// <seealso cref="ITcpClientSocket.Open()"/>
        /// <seealso cref="ITcpClientSocket.Send(Byte[])"/>
        /// <seealso cref="ITcpClientSocket.Close()"/>
        public void Write(String message)
        {
            if (this.IsDisposed)
            {
                return;
            }

            if (String.IsNullOrWhiteSpace(message))
            {
                return;
            }

            if (!this.client.Open())
            {
                return;
            }

            if (this.settings.Termination && !message.EndsWith("\0"))
            {
                message += "\0";
            }

            Byte[] payload = this.settings.Encoding.GetBytes(message);

            Int32 totalBytes = this.client.Send(payload);

            Debug.WriteLine($"{nameof(ITcpNetworkWriter)}::{nameof(this.Write)}(): Payload length: {payload.Length}; Total bytes sent: {totalBytes}.");

            // GELF documentation: "Each message needs to be delimited with
            // a null byte (\0) when sent in the same TCP connection."
            // According to above, close any connection as soon as a message
            // has a zero termination no matter if Termination is on or off.
            if (payload[payload.Length - 1] != 0)
            {
                this.client.Close();
            }

            return;
        }

        /// <summary>
        /// This method performs the object disposal.
        /// </summary>
        /// <remarks>
        /// The method represents the implementation of interface 
        /// <see cref="IDisposable"/>.
        /// </remarks>
        /// <seealso cref="Dispose(Boolean)"/>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion

        #region Protected methods

        /// <summary>
        /// This method actually does the object disposal.
        /// </summary>
        /// <remarks>
        /// This method detaches from the event handler and 
        /// marks this object as disposed.
        /// </remarks>
        /// <param name="disposing">
        /// True to dispose all managed resources and false 
        /// to dispose only unmanaged resources.
        /// </param>
        protected virtual void Dispose(Boolean disposing)
        {
            if (this.IsDisposed) { return; }

            if (disposing)
            {
                this.client.SafeDispose();
            }

            this.IsDisposed = true;
        }

        #endregion
    }
}
