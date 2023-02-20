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
using Plexdata.LogWriter.Definitions;
using Plexdata.LogWriter.Internals.Abstraction;
using Plexdata.LogWriter.Internals.Extensions;
using Plexdata.LogWriter.Internals.Factories;
using System;
using System.Diagnostics;

namespace Plexdata.LogWriter.Facades
{
    /// <summary>
    /// This class represents the default implementation of interface 
    /// <see cref="INetworkLoggerFacade"/>.
    /// </summary>
    /// <remarks>
    /// Major task of this default implementation is to handle writing 
    /// of messages into its assigned network targets.
    /// </remarks>
    public class NetworkLoggerFacade : INetworkLoggerFacade
    {
        #region Private fields

        /// <summary>
        /// The internal factory to create appropriated network writers.
        /// </summary>
        /// <remarks>
        /// This factory allows to create network writers depending on 
        /// current settings.
        /// </remarks>
        private readonly INetworkInternalFactory factory;

        /// <summary>
        /// The internal network writer currently used.
        /// </summary>
        /// <remarks>
        /// Each used network writer depends on current settings.
        /// </remarks>
        /// <seealso cref="ApplySettings(INetworkLoggerSettings)"/>
        /// <seealso cref="CreateWriter(INetworkLoggerSettings)"/>
        private INetworkWriter writer;

        #endregion

        #region Construction

        /// <summary>
        /// The standard constructor.
        /// </summary>
        /// <remarks>
        /// This constructor initialize a new instance of this class using 
        /// a default instance of <see cref="INetworkInternalFactory"/>.
        /// </remarks>
        /// <seealso cref="NetworkLoggerFacade(INetworkInternalFactory)"/>
        public NetworkLoggerFacade()
            : this(new NetworkInternalFactory())
        {
        }

        /// <summary>
        /// The extended constructor.
        /// </summary>
        /// <remarks>
        /// This constructor is intentionally made as <em>internal</em> and 
        /// is only used for testing.
        /// </remarks>
        /// <param name="factory">
        /// An instance used network internal factory.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// This exception is thrown if parameter <paramref name="factory"/> is 
        /// <c>null</c>, which should indeed not happen except during testing.
        /// </exception>
        /// <seealso cref="NetworkLoggerFacade()"/>
        internal NetworkLoggerFacade(INetworkInternalFactory factory)
            : base()
        {
            this.factory = factory ?? throw new ArgumentNullException(nameof(factory));
        }

        /// <summary>
        /// The class destructor.
        /// </summary>
        /// <remarks>
        /// The destructor performs an object disposal.
        /// </remarks>
        ~NetworkLoggerFacade()
        {
            this.Dispose(false);
        }

        #endregion

        #region Public properties

        /// <summary>
        /// Determines whether this instance has been disposed.
        /// </summary>
        /// <remarks>
        /// After calling method <see cref="IDisposable.Dispose()"/> the 
        /// object is no longer functional. This property can be queried 
        /// to determine if this instance is disposed already.
        /// </remarks>
        /// <value>
        /// True if the object has been disposed and false otherwise.
        /// </value>
        public Boolean IsDisposed { get; private set; } = false;

        #endregion

        #region Public methods

        /// <inheritdoc />
        public void ApplySettings(INetworkLoggerSettings settings)
        {
            if (this.IsDisposed) { return; }

            this.writer.SafeDispose();
            this.writer = this.CreateWriter(settings);
        }

        /// <inheritdoc />
        public void Write(String message)
        {
            if (this.IsDisposed) { return; }

            try
            {
                this.writer.Write(message);
            }
            catch (Exception exception)
            {
                Debug.WriteLine(exception);
            }
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
                this.writer.SafeDispose();
                this.writer = null;
            }

            this.IsDisposed = true;
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Creates a new network writer according to current settings.
        /// </summary>
        /// <remarks>
        /// The network writer to create is determined by currenlty used 
        /// protocol. See <see cref="INetworkLoggerSettings.Protocol"/>.
        /// </remarks>
        /// <param name="settings">
        /// The settings used to determine which network writer has to 
        /// be created.
        /// </param>
        /// <returns>
        /// An instance of <see cref="INetworkWriter"/>.
        /// </returns>
        /// <exception cref="NotSupportedException">
        /// This exception is thrown in case of current protocol value is 
        /// neither <see cref="Protocol.Udp"/>, nor <see cref="Protocol.Tcp"/>, 
        /// nor <see cref="Protocol.Web"/>.
        /// </exception>
        /// <seealso cref="IUdpNetworkWriter"/>
        /// <seealso cref="ITcpNetworkWriter"/>
        /// <seealso cref="IWebNetworkWriter"/>
        private INetworkWriter CreateWriter(INetworkLoggerSettings settings)
        {
            switch (settings.Protocol)
            {
                case Protocol.Udp:
                    return this.factory.Create<IUdpNetworkWriter>(settings);
                case Protocol.Tcp:
                    return this.factory.Create<ITcpNetworkWriter>(settings);
                case Protocol.Web:
                    return this.factory.Create<IWebNetworkWriter>(settings);
                default:
                    throw new NotSupportedException();
            }
        }

        #endregion
    }
}
