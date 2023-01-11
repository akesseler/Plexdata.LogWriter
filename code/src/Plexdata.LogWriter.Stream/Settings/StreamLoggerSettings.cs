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
using System.IO;
using System.Text;

namespace Plexdata.LogWriter.Settings
{
    /// <summary>
    /// The class to provide settings for all of the stream logging writers.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This class allows a configuration of classes derived from stream logger 
    /// interfaces. But keep in mind, the availability of particular setting items 
    /// depends of which of the available stream loggers is actually used.
    /// </para>
    /// <para>
    /// Attention, logging type JSON is used as the default logging type.
    /// </para>
    /// </remarks>
    public class StreamLoggerSettings : LoggerSettings, IStreamLoggerSettings
    {
        #region Private fields

        /// <summary>
        /// This field holds the used stream.
        /// </summary>
        /// <remarks>
        /// The value of <c>null</c> is used as initial value.
        /// </remarks>
        private Stream stream;

        /// <summary>
        /// This field holds the chosen encoding.
        /// </summary>
        /// <remarks>
        /// The value of <see cref="StreamLoggerSettings.DefaultEncoding"/> 
        /// is used as initial value.
        /// </remarks>
        private Encoding encoding;

        #endregion

        #region Private constants

        /// <summary>
        /// The default encoding value.
        /// </summary>
        /// <remarks>
        /// The default encoding value is set to <c>UTF-8</c>.
        /// </remarks>
        private static readonly Encoding DefaultEncoding = Encoding.UTF8;

        #endregion

        #region Construction

        /// <summary>
        /// The static class constructor.
        /// </summary>
        /// <remarks>
        /// This constructor does actually nothing.
        /// </remarks>
        static StreamLoggerSettings() { }

        /// <summary>
        /// The standard class constructor.
        /// </summary>
        /// <remarks>
        /// This constructor just initializes all properties with its default 
        /// values.
        /// </remarks>
        public StreamLoggerSettings()
            : base()
        {
            base.LogType = LogType.Json;

            this.Stream = null;
            this.Encoding = StreamLoggerSettings.DefaultEncoding;
        }

        /// <summary>
        /// The extended constructor that initializes all properties with its 
        /// default values, but takes the <paramref name="stream"/> instance.
        /// </summary>
        /// <remarks>
        /// This constructor initializes all properties with its default values, 
        /// except property <see cref="StreamLoggerSettings.Stream"/>. The provided 
        /// <paramref name="stream"/> instance is used to initialize this property 
        /// instead.
        /// </remarks>
        /// <param name="stream">
        /// The <see cref="System.IO.Stream"/> derived instance to be used.
        /// </param>
        public StreamLoggerSettings(Stream stream)
            : this()
        {
            this.Stream = stream;
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
        /// <para>
        /// But pay attention on property Stream, because of this property cannot be 
        /// read from any configuration! This property must be set manually instead.
        /// </para>
        /// </remarks>
        /// <param name="configuration">
        /// The configuration to read all property values from.
        /// </param>
        /// <seealso cref="LoggerSettings.LoadSettings(ILoggerSettingsSection)"/>
        public StreamLoggerSettings(ILoggerSettingsSection configuration)
            : this()
        {
            this.LoadSettings(configuration);
        }

        /// <summary>
        /// The extended constructor that initializes all properties from provided 
        /// <paramref name="configuration"/> instance. Property <see cref="StreamLoggerSettings.Stream"/> 
        /// is initialized from parameter <paramref name="stream"/>.
        /// </summary>
        /// <remarks>
        /// This constructor works pretty much the same way as constructor 
        /// <see cref="StreamLoggerSettings.StreamLoggerSettings(ILoggerSettingsSection)"/> 
        /// does, except property <see cref="StreamLoggerSettings.Stream"/> 
        /// is initialized from parameter <paramref name="stream"/>.
        /// </remarks>
        /// <param name="configuration">
        /// The configuration to read all property values from.
        /// </param>
        /// <param name="stream">
        /// The <see cref="System.IO.Stream"/> derived instance to be used.
        /// </param>
        public StreamLoggerSettings(ILoggerSettingsSection configuration, Stream stream)
            : this(configuration)
        {
            this.Stream = stream;
        }

        #endregion

        #region Public properties

        /// <inheritdoc />
        public Stream Stream
        {
            get
            {
                return this.stream;
            }
            set
            {
                if (this.stream != value)
                {
                    this.stream = value;
                    base.RaisePropertyChanged(nameof(this.Stream));
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
                if (this.encoding != value)
                {
                    this.encoding = value;
                    base.RaisePropertyChanged(nameof(this.Encoding));
                }
            }
        }

        #endregion

        #region Protected methods

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

            base.LogType = base.GetValue(section[nameof(this.LogType)], LogType.Json);

            this.Stream = null;
            this.Encoding = base.GetValue(section[nameof(this.Encoding)], StreamLoggerSettings.DefaultEncoding);
        }

        #endregion
    }
}

