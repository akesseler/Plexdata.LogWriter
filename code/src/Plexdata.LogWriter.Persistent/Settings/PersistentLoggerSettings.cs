/*
 * MIT License
 * 
 * Copyright (c) 2019 plexdata.de
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
using Plexdata.LogWriter.Internals.Extensions;
using System;
using System.IO;
using System.Text;

namespace Plexdata.LogWriter.Settings
{
    /// <summary>
    /// The class to provide settings for all of the persistent logging writers.
    /// </summary>
    /// <remarks>
    /// This class allows a configuration of classes derived from persistent logger 
    /// interfaces. But keep in mind, the availability of particular setting items 
    /// depends of which of the available persistent loggers is actually used.
    /// </remarks>
    /// <example>
    /// See below for a fully qualified and executable example of how to use persistent 
    /// logger.
    /// <code language="C#">
    /// using Plexdata.LogWriter.Abstraction;
    /// using Plexdata.LogWriter.Definitions;
    /// using Plexdata.LogWriter.Extensions;
    /// using Plexdata.LogWriter.Logging;
    /// using Plexdata.LogWriter.Settings;
    /// using System;
    /// using System.Text;
    /// 
    /// namespace Plexdata.LogWriter.Examples
    /// {
    ///     class Program
    ///     {
    ///         static void Main(String[] args)
    ///         {
    ///             IPersistentLoggerSettings settings = new PersistentLoggerSettings()
    ///             {
    ///                 Filename = @"c:\temp\logging\test.log",
    ///                 LogLevel = LogLevel.Trace,
    ///                 IsQueuing = false,
    ///                 IsRolling = true,
    ///                 Threshold = 3,
    ///                 Encoding = Encoding.ASCII,
    ///             };
    /// 
    ///             IPersistentLogger logger = new PersistentLogger(settings);
    /// 
    ///             logger.Debug("This is a Debug logging entry.");
    ///             logger.Trace("This is a Trace logging entry.");
    ///             logger.Verbose("This is a Verbose logging entry.");
    ///             logger.Message("This is a Message logging entry.");
    ///             logger.Warning("This is a Warning logging entry.");
    ///             logger.Error("This is a Error logging entry.");
    ///             logger.Fatal("This is a Fatal logging entry.");
    ///             logger.Critical("This is a Critical logging entry.");
    /// 
    ///             Console.Write("Hit any key to finish... ");
    ///             Console.ReadKey();
    ///         }
    ///     }
    /// }
    /// </code>
    /// </example>
    public class PersistentLoggerSettings : LoggerSettings, IPersistentLoggerSettings
    {
        #region Private fields

        /// <summary>
        /// This field holds the filename to be used.
        /// </summary>
        /// <remarks>
        /// The value of <see cref="PersistentLoggerSettings.DefaultFilename"/> 
        /// is used as initial value.
        /// </remarks>
        private String filename;

        /// <summary>
        /// This field holds the chosen rolling mode.
        /// </summary>
        /// <remarks>
        /// The value of <see cref="PersistentLoggerSettings.DefaultRolling"/> 
        /// is used as initial value.
        /// </remarks>
        private Boolean rolling;

        /// <summary>
        /// This field holds the chosen queuing mode.
        /// </summary>
        /// <remarks>
        /// The value of <see cref="PersistentLoggerSettings.DefaultQueuing"/> 
        /// is used as initial value.
        /// </remarks>
        private Boolean queuing;

        /// <summary>
        /// This field holds the chosen threshold.
        /// </summary>
        /// <remarks>
        /// The value of <see cref="PersistentLoggerSettings.DefaultThreshold"/> 
        /// is used as initial value.
        /// </remarks>
        private Int32 threshold;

        /// <summary>
        /// This field holds the chosen encoding.
        /// </summary>
        /// <remarks>
        /// The value of <see cref="PersistentLoggerSettings.DefaultEncoding"/> 
        /// is used as initial value.
        /// </remarks>
        private Encoding encoding;

        #endregion

        #region Private constants

        /// <summary>
        /// The fully qualified default logging filename.
        /// </summary>
        /// <remarks>
        /// The name of default logging file is set to <c>plexdata.log</c> and the 
        /// path points to the current user's temporary folder.
        /// </remarks>
        private static readonly String DefaultFilename = Path.Combine(Path.GetTempPath(), "plexdata.log");

        /// <summary>
        /// The default rolling value.
        /// </summary>
        /// <remarks>
        /// The default rolling value is set to <c>false</c>, which means rolling 
        /// is initially disabled.
        /// </remarks>
        private static readonly Boolean DefaultRolling = false;

        /// <summary>
        /// The default queuing value.
        /// </summary>
        /// <remarks>
        /// The default queuing value is set to <c>false</c>, which means queuing 
        /// is initially disabled.
        /// </remarks>
        private static readonly Boolean DefaultQueuing = false;

        /// <summary>
        /// The default threshold value.
        /// </summary>
        /// <remarks>
        /// The default threshold value is set to <c>-1</c>, which means the threshold 
        /// is initially not used.
        /// </remarks>
        private static readonly Int32 DefaultThreshold = -1;

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
        static PersistentLoggerSettings() { }

        /// <summary>
        /// The standard class constructor.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This constructor just initializes all properties with its default values.
        /// </para>
        /// <para>
        /// The name of default logging file is set to <c>plexdata.log</c> and the path 
        /// points to the current user's temporary folder.
        /// </para>
        /// </remarks>
        /// <exception cref="Exception">
        /// This constructor may throw several exceptions. For more information about these exceptions 
        /// please see <see cref="SettingsValidationExtension.EnsureFullPathAndWriteAccessOrThrow(String)"/>.
        /// </exception>
        /// <seealso cref="SettingsValidationExtension.EnsureFullPathAndWriteAccessOrThrow(String)"/>
        public PersistentLoggerSettings()
            : base()
        {
            this.Filename = PersistentLoggerSettings.DefaultFilename;
            this.IsRolling = PersistentLoggerSettings.DefaultRolling;
            this.IsQueuing = PersistentLoggerSettings.DefaultQueuing;
            this.Threshold = PersistentLoggerSettings.DefaultThreshold;
            this.Encoding = PersistentLoggerSettings.DefaultEncoding;
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
        /// <seealso cref="LoggerSettings.LoadSettings(ILoggerSettingsSection)"/>
        public PersistentLoggerSettings(ILoggerSettingsSection configuration)
            : this()
        {
            this.LoadSettings(configuration);
        }

        #endregion

        #region Public properties

        /// <inheritdoc />
        /// <exception cref="Exception">
        /// This property may throw several exceptions. For more information about these exceptions 
        /// please see <see cref="SettingsValidationExtension.EnsureFullPathAndWriteAccessOrThrow(String)"/>.
        /// </exception>
        /// <seealso cref="SettingsValidationExtension.EnsureFullPathAndWriteAccessOrThrow(String)"/>
        public String Filename
        {
            get
            {
                return this.filename;
            }
            set
            {
                if (this.filename != value)
                {
                    this.filename = value.EnsureFullPathAndWriteAccessOrThrow();
                    base.RaisePropertyChanged(nameof(this.Filename));
                }
            }
        }

        /// <inheritdoc />
        public Boolean IsRolling
        {
            get
            {
                return this.rolling;
            }
            set
            {
                if (this.rolling != value)
                {
                    this.rolling = value;
                    base.RaisePropertyChanged(nameof(this.IsRolling));
                }
            }
        }

        /// <inheritdoc />
        public Boolean IsQueuing
        {
            get
            {
                return this.queuing;
            }
            set
            {
                if (this.queuing != value)
                {
                    this.queuing = value;
                    base.RaisePropertyChanged(nameof(this.IsQueuing));
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

            this.Filename = base.GetValue(section[nameof(this.Filename)], PersistentLoggerSettings.DefaultFilename);
            this.IsRolling = base.GetValue(section[nameof(this.IsRolling)], PersistentLoggerSettings.DefaultRolling);
            this.IsQueuing = base.GetValue(section[nameof(this.IsQueuing)], PersistentLoggerSettings.DefaultQueuing);
            this.Threshold = base.GetValue(section[nameof(this.Threshold)], PersistentLoggerSettings.DefaultThreshold);
            this.Encoding = base.GetValue(section[nameof(this.Encoding)], PersistentLoggerSettings.DefaultEncoding);
        }

        #endregion
    }
}
