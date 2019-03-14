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
using Plexdata.LogWriter.Definitions;
using System;
using System.ComponentModel;
using System.Globalization;

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
        // TODO: Review and/or complete documentation.

        #region Private fields

        private LogLevel logLevel;
        private LogType logType;
        private LogTime logTime;
        private Boolean showTime;
        private String timeFormat;
        private Char partSplit;
        private Boolean fullName;
        private CultureInfo culture;

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
            this.logLevel = LogLevel.Default;
            this.logType = LogType.Default;
            this.logTime = LogTime.Default;
            this.showTime = true;
            this.timeFormat = LoggerSettings.DefaultTimeFormat;
            this.partSplit = ';';
            this.fullName = true;
            this.culture = new CultureInfo("en-US");
        }

        #endregion

        #region Events

        /// <inheritdoc />
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

        #region Protected methods

        protected void RaisePropertyChanged(String property)
        {
            if (String.IsNullOrWhiteSpace(property))
            {
                throw new ArgumentOutOfRangeException(nameof(property));
            }

            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }

        #endregion
    }
}
