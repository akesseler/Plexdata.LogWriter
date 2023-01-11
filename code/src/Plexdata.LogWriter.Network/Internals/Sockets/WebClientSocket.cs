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
using Plexdata.LogWriter.Internals.Extensions;
using System;
using System.Diagnostics;
using System.Linq;
using System.Net;

namespace Plexdata.LogWriter.Internals.Sockets
{
    /// <summary>
    /// A class to allow physical data transmission.
    /// </summary>
    /// <remarks>
    /// This class represents the implementation of a 
    /// facade of a WEB/HTTP socket client.
    /// </remarks>
    internal class WebClientSocket : IWebClientSocket
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
        /// An instance of the client facade.
        /// </summary>
        /// <remarks>
        /// This field holds an instance of the client created through the factory.
        /// </remarks>
        private IClientFacade client = null;

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
        public WebClientSocket(INetworkInternalFactory factory, INetworkLoggerSettings settings)
            : base()
        {
            this.factory = factory ?? throw new ArgumentNullException(nameof(factory));
            this.settings = settings ?? throw new ArgumentNullException(nameof(settings));
        }

        #endregion

        #region Public methods

        /// <inheritdoc />
        public Boolean Connect()
        {
            if (this.client != null)
            {
                return true;
            }

            try
            {
                // Method "POST" in conjunction with "application/json" is used
                // for GELF requests, but might be used also for other requests.
                this.client = this.factory.Create<IClientFacade>(
                    this.BuildRemoteUri(this.settings.Host, this.settings.Port),
                    "POST", "application/json", this.settings.Timeout);

                return true;
            }
            catch (Exception exception)
            {
                this.client.SafeDispose();
                this.client = null;

                Debug.WriteLine(exception);
                return false;
            }
        }

        /// <inheritdoc />
        public Int32 Send(Byte[] payload)
        {
            if (this.client == null)
            {
                return 0;
            }

            if (!payload?.Any() ?? true)
            {
                return 0;
            }

            try
            {
                return this.client.Send(payload);
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
            this.client.SafeDispose();
            this.client = null;
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Builds the URL of the remote host.
        /// </summary>
        /// <remarks>
        /// This method builds the URL of the remote host.
        /// </remarks>
        /// <param name="host">
        /// The fully qualified URL (including protocol and path) of the 
        /// remote host.
        /// </param>
        /// <param name="port">
        /// This URL may include the port. The port is taken from parameter 
        /// <paramref name="port"/> if no port number is included.
        /// </param>
        /// <returns>
        /// The fully qualified URL (including port) of the remote host.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// This exception is thrown if parameter <paramref name="host"/> is <c>null</c>, empty 
        /// or whitespace, or if parameter <paramref name="port"/> is out of range [0...65535].
        /// </exception>
        private Uri BuildRemoteUri(String host, Int32 port)
        {
            if (String.IsNullOrWhiteSpace(host))
            {
                throw new ArgumentOutOfRangeException(nameof(host), $"Parameter \"{nameof(host)}\" cannot be null or whitespace.");
            }

            if (port < IPEndPoint.MinPort || port > IPEndPoint.MaxPort)
            {
                throw new ArgumentOutOfRangeException(nameof(port), $"Parameter \"{nameof(port)}\" must be in range of [{IPEndPoint.MinPort}...{IPEndPoint.MaxPort}].");
            }

            UriBuilder builder = new UriBuilder(host);

            if (port != IPEndPoint.MinPort && port != builder.Port)
            {
                builder.Port = port;
            }

            return builder.Uri;
        }

        #endregion
    }
}
