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
using System;
using System.Text;

namespace Plexdata.LogWriter.Settings
{
    /// <summary>
    /// The class to provide settings for all of the network logging writers.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This class allows a configuration of classes derived from network logger 
    /// interfaces. But keep in mind, the availability of particular setting items 
    /// depends of which of the available network loggers is actually used.
    /// </para>
    /// <para>
    /// Attention, logging type GELF is used as the default logging type.
    /// </para>
    /// </remarks>
    public class NetworkLoggerSettings : LoggerSettings, INetworkLoggerSettings
    {
        #region Private constants

        /// <summary>
        /// The default host name.
        /// </summary>
        /// <remarks>
        /// The default host name is set to <c>localhost</c>.
        /// </remarks>
        private static readonly String DefaultHost = "localhost";

        /// <summary>
        /// The default port number.
        /// </summary>
        /// <remarks>
        /// The default port number is set to <c>12201</c>. This default 
        /// value has been taken from Graylog GELF documentation page.
        /// </remarks>
        private static readonly Int32 DefaultPort = 12201;

        /// <summary>
        /// The default address type.
        /// </summary>
        /// <remarks>
        /// The default address type is set to <see cref="Address.Default"/>.
        /// </remarks>
        private static readonly Address DefaultAddress = Address.Default;

        /// <summary>
        /// The default protocol type.
        /// </summary>
        /// <remarks>
        /// The default protocol type is set to <see cref="Protocol.Default"/>.
        /// </remarks>
        private static readonly Protocol DefaultProtocol = Protocol.Default;

        /// <summary>
        /// The default encoding.
        /// </summary>
        /// <remarks>
        /// The default encoding value is set to <c>UTF-8</c>.
        /// </remarks>
        private static readonly Encoding DefaultEncoding = Encoding.UTF8;

        /// <summary>
        /// The default compression value.
        /// </summary>
        /// <remarks>
        /// The default compression value is set to <c>true</c>.
        /// </remarks>
        private static readonly Boolean DefaultCompressed = true;

        /// <summary>
        /// The default threshold value.
        /// </summary>
        /// <remarks>
        /// The default threshold value is set to <c>512</c>.
        /// </remarks>
        private static readonly Int32 DefaultThreshold = 512;

        /// <summary>
        /// The default maximum value.
        /// </summary>
        /// <remarks>
        /// The default maximum value is set to <c>8192</c>. This default 
        /// value has been taken from Graylog GELF documentation page.
        /// </remarks>
        private static readonly Int32 DefaultMaximum = 8192;

        /// <summary>
        /// The default termination value.
        /// </summary>
        /// <remarks>
        /// The default termination value is set to <c>false</c>.
        /// </remarks>
        private static readonly Boolean DefaultTermination = false;

        /// <summary>
        /// The default timeout value.
        /// </summary>
        /// <remarks>
        /// The default timeout value is set to 100000 milliseconds (100 seconds) 
        /// and has been taken from official Microsoft documentation.
        /// </remarks>
        private static readonly Int32 DefaultTimeout = 100000;

        #endregion

        #region Private fields

        /// <summary>
        /// This field holds the host name to be used.
        /// </summary>
        /// <remarks>
        /// The value of <see cref="NetworkLoggerSettings.DefaultHost"/> 
        /// is used as initial value.
        /// </remarks>
        private String host;

        /// <summary>
        /// This field holds the port number to be used.
        /// </summary>
        /// <remarks>
        /// The value of <see cref="NetworkLoggerSettings.DefaultPort"/> 
        /// is used as initial value.
        /// </remarks>
        private Int32 port;

        /// <summary>
        /// This field holds the address type to be used.
        /// </summary>
        /// <remarks>
        /// The value of <see cref="NetworkLoggerSettings.DefaultAddress"/> 
        /// is used as initial value.
        /// </remarks>
        private Address address;

        /// <summary>
        /// This field holds the protocol type to be used.
        /// </summary>
        /// <remarks>
        /// The value of <see cref="NetworkLoggerSettings.DefaultProtocol"/> 
        /// is used as initial value.
        /// </remarks>
        private Protocol protocol;

        /// <summary>
        /// This field holds the encoding to be used.
        /// </summary>
        /// <remarks>
        /// The value of <see cref="NetworkLoggerSettings.DefaultEncoding"/> 
        /// is used as initial value.
        /// </remarks>
        private Encoding encoding;

        /// <summary>
        /// This field holds the compressed value to be used.
        /// </summary>
        /// <remarks>
        /// The value of <see cref="NetworkLoggerSettings.DefaultCompressed"/> 
        /// is used as initial value.
        /// </remarks>
        private Boolean compressed;

        /// <summary>
        /// This field holds the threshold value to be used.
        /// </summary>
        /// <remarks>
        /// The value of <see cref="NetworkLoggerSettings.DefaultThreshold"/> 
        /// is used as initial value.
        /// </remarks>
        private Int32 threshold;

        /// <summary>
        /// This field holds the maximum value to be used.
        /// </summary>
        /// <remarks>
        /// The value of <see cref="NetworkLoggerSettings.DefaultMaximum"/> 
        /// is used as initial value.
        /// </remarks>
        private Int32 maximum;

        /// <summary>
        /// This field holds the termination value to be used.
        /// </summary>
        /// <remarks>
        /// The value of <see cref="NetworkLoggerSettings.DefaultTermination"/> 
        /// is used as initial value.
        /// </remarks>
        private Boolean termination;

        /// <summary>
        /// This field holds the timeout value to be used.
        /// </summary>
        /// <remarks>
        /// The value of <see cref="NetworkLoggerSettings.DefaultTimeout"/> 
        /// is used as initial value.
        /// </remarks>
        private Int32 timeout;

        #endregion

        #region Construction

        /// <summary>
        /// The static class constructor.
        /// </summary>
        /// <remarks>
        /// This constructor initialize a new instance of this class.
        /// </remarks>
        static NetworkLoggerSettings() { }

        /// <summary>
        /// The standard class constructor.
        /// </summary>
        /// <remarks>
        /// This constructor just initializes all properties with its default values.
        /// </remarks>
        public NetworkLoggerSettings()
            : base()
        {
            this.Host = NetworkLoggerSettings.DefaultHost;
            this.Port = NetworkLoggerSettings.DefaultPort;
            this.Address = NetworkLoggerSettings.DefaultAddress;
            this.Protocol = NetworkLoggerSettings.DefaultProtocol;
            this.Encoding = NetworkLoggerSettings.DefaultEncoding;
            this.Compressed = NetworkLoggerSettings.DefaultCompressed;
            this.Threshold = NetworkLoggerSettings.DefaultThreshold;
            this.Maximum = NetworkLoggerSettings.DefaultMaximum;
            this.Termination = NetworkLoggerSettings.DefaultTermination;
            this.Timeout = NetworkLoggerSettings.DefaultTimeout;
        }

        /// <summary>
        /// The extended constructor that initializes all properties from provided 
        /// <paramref name="configuration"/> instance.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Usually, it is no necessary to call this constructor manually. This is 
        /// because of, this constructor is actually reserved for the mechanism of 
        /// dependency injection.
        /// </para>
        /// <para>
        /// Please note, the default values are taken if one or more properties are 
        /// not included in the configuration.
        /// </para>
        /// </remarks>
        /// <param name="configuration">
        /// The configuration to read all property values from.
        /// </param>
        public NetworkLoggerSettings(ILoggerSettingsSection configuration)
            : this()
        {
            this.LoadSettings(configuration);
        }

        #endregion

        #region Public Properties

        /// <inheritdoc />
        public String Host
        {
            get
            {
                return this.host;
            }
            set
            {
                if (this.host != value)
                {
                    this.host = value;
                    base.RaisePropertyChanged(nameof(this.Host));
                }
            }
        }

        /// <inheritdoc />
        public Int32 Port
        {
            get
            {
                return this.port;
            }
            set
            {
                if (this.port != value)
                {
                    this.port = value;
                    base.RaisePropertyChanged(nameof(this.Port));
                }
            }
        }

        /// <inheritdoc />
        public Address Address
        {
            get
            {
                return this.address;
            }
            set
            {
                if (this.address != value)
                {
                    this.address = value;
                    base.RaisePropertyChanged(nameof(this.Address));
                }
            }
        }

        /// <inheritdoc />
        public Protocol Protocol
        {
            get
            {
                return this.protocol;
            }
            set
            {
                if (this.protocol != value)
                {
                    this.protocol = value;
                    base.RaisePropertyChanged(nameof(this.Protocol));
                }
            }
        }

        /// <inheritdoc />
        public Encoding Encoding
        {
            get
            {
                return this.encoding;
            }
            set
            {
                if (value == null)
                {
                    value = Encoding.UTF8;
                }

                if (this.encoding != value)
                {
                    this.encoding = value;
                    base.RaisePropertyChanged(nameof(this.Encoding));
                }
            }
        }

        /// <inheritdoc />
        public Boolean Compressed
        {
            get
            {
                return this.compressed;
            }
            set
            {
                if (this.compressed != value)
                {
                    this.compressed = value;
                    base.RaisePropertyChanged(nameof(this.Compressed));
                }
            }
        }

        /// <inheritdoc />
        public Int32 Threshold
        {
            get
            {
                return this.threshold;
            }
            set
            {
                if (this.threshold != value)
                {
                    this.threshold = value;
                    base.RaisePropertyChanged(nameof(this.Threshold));
                }
            }
        }

        /// <inheritdoc />
        public Int32 Maximum
        {
            get
            {
                return this.maximum;
            }
            set
            {
                if (this.maximum != value)
                {
                    this.maximum = value;
                    base.RaisePropertyChanged(nameof(this.Maximum));
                }
            }
        }

        /// <inheritdoc />
        public Boolean Termination
        {
            get
            {
                return this.termination;
            }
            set
            {
                if (this.termination != value)
                {
                    this.termination = value;
                    base.RaisePropertyChanged(nameof(this.Termination));
                }
            }
        }

        /// <inheritdoc />
        public Int32 Timeout
        {
            get
            {
                return this.timeout;
            }
            set
            {
                if (this.timeout != value)
                {
                    this.timeout = value;
                    base.RaisePropertyChanged(nameof(this.Timeout));
                }
            }
        }

        #endregion

        #region Protected Methods

        /// <summary>
        /// The method loads all settings from provided <paramref name="configuration"/> 
        /// and initializes all properties accordingly.
        /// </summary>
        /// <remarks>
        /// This method has been overwritten and calls the inherited base class method 
        /// to ensure all settings are loaded accordingly.
        /// </remarks>
        /// <param name="configuration">
        /// An instance of <see cref="Plexdata.LogWriter.Abstraction.ILoggerSettingsSection"/> 
        /// that represents the settings to be applied.
        /// </param>
        /// <seealso cref="LoggerSettings.LoadSettings(ILoggerSettingsSection)"/>
        protected override void LoadSettings(ILoggerSettingsSection configuration)
        {
            if (configuration == null) { return; }

            base.LoadSettings(configuration);

            ILoggerSettingsSection section = configuration.GetSection(LoggerSettings.SettingsPath);

            base.LogType = base.GetValue(section[nameof(this.LogType)], LogType.Gelf);

            this.Host = base.GetValue(section[nameof(this.Host)], NetworkLoggerSettings.DefaultHost);
            this.Port = base.GetValue(section[nameof(this.Port)], NetworkLoggerSettings.DefaultPort);
            this.Address = base.GetValue(section[nameof(this.Address)], NetworkLoggerSettings.DefaultAddress);
            this.Protocol = base.GetValue(section[nameof(this.Protocol)], NetworkLoggerSettings.DefaultProtocol);
            this.Encoding = base.GetValue(section[nameof(this.Encoding)], NetworkLoggerSettings.DefaultEncoding);
            this.Compressed = base.GetValue(section[nameof(this.Compressed)], NetworkLoggerSettings.DefaultCompressed);
            this.Threshold = base.GetValue(section[nameof(this.Threshold)], NetworkLoggerSettings.DefaultThreshold);
            this.Maximum = base.GetValue(section[nameof(this.Maximum)], NetworkLoggerSettings.DefaultMaximum);
            this.Termination = base.GetValue(section[nameof(this.Termination)], NetworkLoggerSettings.DefaultTermination);
            this.Timeout = base.GetValue(section[nameof(this.Timeout)], NetworkLoggerSettings.DefaultTimeout);
        }

        #endregion
    }
}
