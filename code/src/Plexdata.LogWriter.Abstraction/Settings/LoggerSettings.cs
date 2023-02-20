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
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Plexdata.LogWriter.Settings
{
    /// <summary>
    /// The abstract base class of all provided logger settings implementations.
    /// </summary>
    /// <remarks>
    /// The abstract logger settings base class provides information that are essential 
    /// for each of the supported <i>Plexdata Logging Writers</i>.
    /// </remarks>
    public abstract class LoggerSettings : ILoggerSettings
    {
        #region Private fields

        /// <summary>
        /// This field represents current logging level.
        /// </summary>
        /// <remarks>
        /// The value of this field is managed in the implementation 
        /// of property <see cref="LoggerSettings.LogLevel"/>.
        /// </remarks>
        private LogLevel logLevel;

        /// <summary>
        /// This field represents current logging type.
        /// </summary>
        /// <remarks>
        /// The value of this field is managed in the implementation 
        /// of property <see cref="LoggerSettings.LogType"/>.
        /// </remarks>
        private LogType logType;

        /// <summary>
        /// This field represents current logging time.
        /// </summary>
        /// <remarks>
        /// The value of this field is managed in the implementation 
        /// of property <see cref="LoggerSettings.LogTime"/>.
        /// </remarks>
        private LogTime logTime;

        /// <summary>
        /// This field represents current state of key visibility.
        /// </summary>
        /// <remarks>
        /// The value of this field is managed in the implementation 
        /// of property <see cref="LoggerSettings.ShowKey"/>.
        /// </remarks>
        private Boolean showKey;

        /// <summary>
        /// This field represents current state of time stamp visibility.
        /// </summary>
        /// <remarks>
        /// The value of this field is managed in the implementation 
        /// of property <see cref="LoggerSettings.ShowTime"/>.
        /// </remarks>
        private Boolean showTime;

        /// <summary>
        /// This field represents current time stamp format.
        /// </summary>
        /// <remarks>
        /// The value of this field is managed in the implementation 
        /// of property <see cref="LoggerSettings.TimeFormat"/>.
        /// </remarks>
        private String timeFormat;

        /// <summary>
        /// This field represents current part split character.
        /// </summary>
        /// <remarks>
        /// The value of this field is managed in the implementation 
        /// of property <see cref="LoggerSettings.PartSplit"/>.
        /// </remarks>
        private Char partSplit;

        /// <summary>
        /// This field represents current state of full name usage.
        /// </summary>
        /// <remarks>
        /// The value of this field is managed in the implementation 
        /// of property <see cref="LoggerSettings.FullName"/>.
        /// </remarks>
        private Boolean fullName;

        /// <summary>
        /// This field represents currently assigned culture.
        /// </summary>
        /// <remarks>
        /// The value of this field is managed in the implementation 
        /// of property <see cref="LoggerSettings.Culture"/>.
        /// </remarks>
        private CultureInfo culture;

        #endregion

        #region Private constants

        /// <summary>
        /// The default value of showing keys.
        /// </summary>
        /// <remarks>
        /// The default value of showing keys is set to <c>true</c>, 
        /// which means showing keys is initially enabled.
        /// </remarks>
        private static readonly Boolean DefaultShowKey = true;

        /// <summary>
        /// The default value of showing timestamps.
        /// </summary>
        /// <remarks>
        /// The default value of showing timestamps is set to <c>true</c>, 
        /// which means showing timestamps is initially enabled.
        /// </remarks>
        private static readonly Boolean DefaultShowTime = true;

        /// <summary>
        /// The default part split value.
        /// </summary>
        /// <remarks>
        /// The default part split value is set to <c>semicolon</c>, which 
        /// means each part is initially delimited by <c>;</c>.
        /// </remarks>
        private static readonly Char DefaultPartSplit = ';';

        /// <summary>
        /// The default full name usage value.
        /// </summary>
        /// <remarks>
        /// The default full name usage value is set to <c>true</c>, which 
        /// means full name usage is initially enabled.
        /// </remarks>
        private static readonly Boolean DefaultFullName = true;

        /// <summary>
        /// The default culture.
        /// </summary>
        /// <remarks>
        /// The default culture value is set to <c>en-US</c>, which means US 
        /// culture is initially used.
        /// </remarks>
        private static readonly CultureInfo DefaultCulture = new CultureInfo("en-US");

        #endregion

        #region Public fields

        /// <summary>
        /// The field defines the default time stamp format.
        /// </summary>
        /// <remarks>
        /// The default time stamp format is always used if either a different 
        /// time stamp format is not defined or if a provided time stamp format 
        /// is recognized as invalid.
        /// </remarks>
        public static readonly String DefaultTimeFormat = "yyyy-MM-dd HH:mm:ss.ffff";

        #endregion

        #region Construction

        /// <summary>
        /// The static constructor.
        /// </summary>
        /// <remarks>
        /// The static constructor does nothing.
        /// </remarks>
        static LoggerSettings() { }

        /// <summary>
        /// The default class constructor.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This default constructor just initializes all properties with its default 
        /// values.
        /// </para>
        /// <para>
        /// Intentionally, this constructor can only be called by derived classes.
        /// </para>
        /// </remarks>
        protected LoggerSettings()
            : base()
        {
            this.LogLevel = LogLevel.Default;
            this.LogType = LogType.Default;
            this.LogTime = LogTime.Default;
            this.ShowKey = LoggerSettings.DefaultShowKey;
            this.ShowTime = LoggerSettings.DefaultShowTime;
            this.TimeFormat = LoggerSettings.DefaultTimeFormat;
            this.PartSplit = LoggerSettings.DefaultPartSplit;
            this.FullName = LoggerSettings.DefaultFullName;
            this.Culture = LoggerSettings.DefaultCulture;
        }

        #endregion

        #region Events

        /// <inheritdoc />
        /// <remarks>
        /// This event occurs as soon as one of the properties has changed. 
        /// But be aware, derived classes may handle this in different ways.
        /// </remarks>
        /// <seealso cref="RaisePropertyChanged(String)"/>
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Public properties

        /// <inheritdoc />
        public LogLevel LogLevel
        {
            get
            {
                return this.logLevel;
            }
            set
            {
                if (this.logLevel != value)
                {
                    this.logLevel = value;
                    this.RaisePropertyChanged(nameof(this.LogLevel));
                }
            }
        }

        /// <inheritdoc />
        public LogType LogType
        {
            get
            {
                return this.logType;
            }
            set
            {
                if (this.logType != value)
                {
                    this.logType = value;
                    this.RaisePropertyChanged(nameof(this.LogType));
                }
            }
        }

        /// <inheritdoc />
        public LogTime LogTime
        {
            get
            {
                return this.logTime;
            }
            set
            {
                if (this.logTime != value)
                {
                    this.logTime = value;
                    this.RaisePropertyChanged(nameof(this.LogTime));
                }
            }
        }

        /// <inheritdoc />
        public Boolean ShowKey
        {
            get
            {
                return this.showKey;
            }
            set
            {
                if (this.showKey != value)
                {
                    this.showKey = value;
                    this.RaisePropertyChanged(nameof(this.ShowKey));
                }
            }
        }

        /// <inheritdoc />
        public Boolean ShowTime
        {
            get
            {
                return this.showTime;
            }
            set
            {
                if (this.showTime != value)
                {
                    this.showTime = value;
                    this.RaisePropertyChanged(nameof(this.ShowTime));
                }
            }
        }

        /// <inheritdoc />
        public String TimeFormat
        {
            get
            {
                return this.timeFormat;
            }
            set
            {
                if (this.timeFormat != value)
                {
                    this.timeFormat = value;
                    this.RaisePropertyChanged(nameof(this.TimeFormat));
                }
            }
        }

        /// <inheritdoc />
        public Char PartSplit
        {
            get
            {
                return this.partSplit;
            }
            set
            {
                if (this.partSplit != value)
                {
                    this.partSplit = value;
                    this.RaisePropertyChanged(nameof(this.PartSplit));
                }
            }
        }

        /// <inheritdoc />
        public Boolean FullName
        {
            get
            {
                return this.fullName;
            }
            set
            {
                if (this.fullName != value)
                {
                    this.fullName = value;
                    this.RaisePropertyChanged(nameof(this.FullName));
                }
            }
        }

        /// <inheritdoc />
        public CultureInfo Culture
        {
            get
            {
                return this.culture;
            }
            set
            {
                if (this.culture != value)
                {
                    this.culture = value;
                    this.RaisePropertyChanged(nameof(this.Culture));
                }
            }
        }

        #endregion

        #region Protected properties

        /// <summary>
        /// Gets the path of the settings section within a configuration file.
        /// </summary>
        /// <remarks>
        /// This property returns the namespace as path of the settings section 
        /// within a configuration file. Each part of this namespace is separated 
        /// by a colon instead of a dot. This in turn makes the settings path 
        /// looking like <c>plexdata:logwriter:settings</c>.
        /// </remarks>
        /// <value>
        /// The settings path to be used inside a configuration file.
        /// </value>
        protected static String SettingsPath
        {
            get
            {
                return typeof(LoggerSettings).Namespace.Replace(".", ":");
            }
        }

        #endregion

        #region Protected methods

        /// <summary>
        /// The method loads all settings from provided <paramref name="configuration"/> 
        /// and initializes all properties accordingly.
        /// </summary>
        /// <remarks>
        /// This method should be overwritten but called by derived classes to ensure 
        /// that all settings of sub-classes are loaded as well.
        /// </remarks>
        /// <param name="configuration">
        /// An instance of <see cref="Plexdata.LogWriter.Abstraction.ILoggerSettingsSection"/> 
        /// that represents the settings to be applied.
        /// </param>
        /// <seealso cref="LoggerSettings.GetValue{TType}(String, TType)"/>
        protected virtual void LoadSettings(ILoggerSettingsSection configuration)
        {
            if (configuration == null) { return; }

            ILoggerSettingsSection section = configuration.GetSection(LoggerSettings.SettingsPath);

            this.LogLevel = this.GetValue(section[nameof(this.LogLevel)], LogLevel.Default);
            this.LogType = this.GetValue(section[nameof(this.LogType)], LogType.Default);
            this.LogTime = this.GetValue(section[nameof(this.LogTime)], LogTime.Default);
            this.ShowKey = this.GetValue(section[nameof(this.ShowKey)], LoggerSettings.DefaultShowKey);
            this.ShowTime = this.GetValue(section[nameof(this.ShowTime)], LoggerSettings.DefaultShowTime);
            this.TimeFormat = this.GetValue(section[nameof(this.TimeFormat)], LoggerSettings.DefaultTimeFormat);
            this.PartSplit = this.GetValue(section[nameof(this.PartSplit)], LoggerSettings.DefaultPartSplit);
            this.FullName = this.GetValue(section[nameof(this.FullName)], LoggerSettings.DefaultFullName);
            this.Culture = this.GetValue(section[nameof(this.Culture)], LoggerSettings.DefaultCulture);
        }

        /// <summary>
        /// Gets the type-safe value of provided <paramref name="value"/>.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This method returns the type-safe value of provided <paramref name="value"/> 
        /// or its <paramref name="standard"/> value in case of conversion has failed.
        /// </para>
        /// <para>
        /// Please note, whitespaces are not supported for character types at the moment.
        /// </para>
        /// </remarks>
        /// <typeparam name="TType">
        /// The type to convert the value into.
        /// </typeparam>
        /// <param name="value">
        /// The value to be converted.
        /// </param>
        /// <param name="standard">
        /// The fallback value if conversion fails.
        /// </param>
        /// <returns>
        /// The type-safe conversion result.
        /// </returns>
        protected TType GetValue<TType>(String value, TType standard)
        {
            // BUG: Whitespaces cannot be applied to character types!
            if (String.IsNullOrWhiteSpace(value))
            {
                return standard;
            }

            if (typeof(TType).IsEnum)
            {
                foreach (Object current in Enum.GetValues(typeof(TType)))
                {
                    if (String.Compare(value, current.ToString(), StringComparison.OrdinalIgnoreCase) == 0)
                    {
                        return (TType)current;
                    }
                }

                return standard;
            }

            if (typeof(TType) == typeof(Boolean))
            {
                if (String.Compare(value, Boolean.TrueString, StringComparison.OrdinalIgnoreCase) == 0)
                {
                    return (TType)(Object)true;
                }

                if (String.Compare(value, Boolean.FalseString, StringComparison.OrdinalIgnoreCase) == 0)
                {
                    return (TType)(Object)false;
                }

                return standard;
            }

            if (typeof(TType) == typeof(String))
            {
                return (TType)(Object)value;
            }

            if (typeof(TType) == typeof(Char))
            {
                return (TType)(Object)value[0];
            }

            if (typeof(TType) == typeof(Int32))
            {
                if (Int32.TryParse(value, out Int32 helper))
                {
                    return (TType)(Object)helper;
                }

                return standard;
            }

            if (typeof(TType) == typeof(CultureInfo))
            {
                try
                {
                    if (!this.IsValidCultureName(value))
                    {
                        return standard;
                    }

                    return (TType)(Object)CultureInfo.GetCultureInfo(value);
                }
                catch
                {
                    return standard;
                }
            }

            if (typeof(TType) == typeof(Encoding))
            {
                try
                {
                    return (TType)(Object)Encoding.GetEncoding(value);
                }
                catch
                {
                    return standard;
                }
            }

            return standard;
        }

        /// <summary>
        /// Gets the values from <paramref name="section"/> for provided <paramref name="key"/>.
        /// </summary>
        /// <remarks>
        /// This method tries to read the values from <paramref name="section"/> for provided 
        /// <paramref name="key"/> and returns them.
        /// </remarks>
        /// <param name="section">
        /// An instance of <see cref="Plexdata.LogWriter.Abstraction.ILoggerSettingsSection"/> 
        /// that represents the settings get a list of section values for.
        /// </param>
        /// <param name="key">
        /// The key to get a list of section values for.
        /// </param>
        /// <param name="standard">
        /// The fallback value if conversion fails.
        /// </param>
        /// <returns>
        /// The list of section values or <paramref name="standard"/> if getting section values 
        /// fails or result was <em>empty</em>.
        /// </returns>
        protected IEnumerable<String> GetSectionValues(ILoggerSettingsSection section, String key, IEnumerable<String> standard)
        {
            if (section == null)
            {
                return standard;
            }

            if (String.IsNullOrWhiteSpace(key))
            {
                return standard;
            }

            IEnumerable<String> result = section.GetValues(key);

            if (result.Any())
            {
                return result;
            }

            return standard;
        }

        /// <summary>
        /// Informs listeners about property changes.
        /// </summary>
        /// <remarks>
        /// This method raises the <see cref="PropertyChanged"/> event to inform 
        /// listeners about changes of a particular property.
        /// </remarks>
        /// <param name="property">
        /// The name of the property that has been changed.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// This exception is thrown as soon as provided <paramref name="property"/> 
        /// is <c>null</c>, <em>empty</em> or consists only of <em>whitespaces</em>. 
        /// </exception>
        protected void RaisePropertyChanged(String property)
        {
            if (String.IsNullOrWhiteSpace(property))
            {
                throw new ArgumentOutOfRangeException(nameof(property));
            }

            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Checks if value is a valid culture name.
        /// </summary>
        /// <remarks>
        /// This method determines whether provided <paramref name="value"/> 
        /// represents a valid culture name.
        /// </remarks>
        /// <param name="value">
        /// The culture name to be validated.
        /// </param>
        /// <returns>
        /// True if value represents a valid culture name and false otherwise.
        /// </returns>
        private Boolean IsValidCultureName(String value)
        {
            return CultureInfo
                .GetCultures(CultureTypes.AllCultures)
                .Any(culture => String.Equals(culture.Name, value, StringComparison.InvariantCultureIgnoreCase));
        }

        #endregion
    }
}
