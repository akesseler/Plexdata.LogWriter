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
        /// <summary>
        /// The field defines the default time stamp format.
        /// </summary>
        /// <remarks>
        /// The default time stamp format is always used if either a different 
        /// time stamp format is not defined or if a provided time stamp format 
        /// is recognized as invalid.
        /// </remarks>
        public static readonly String DefaultTimeFormat = "yyyy-MM-dd HH:mm:ss.ffff";

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
            this.ShowTime = true;
            this.TimeFormat = LoggerSettings.DefaultTimeFormat;
            this.PartSplit = ';';
            this.FullName = true;
            this.Culture = new CultureInfo("en-US");
        }

        /// <inheritdoc />
        public LogLevel LogLevel { get; set; }

        /// <inheritdoc />
        public LogType LogType { get; set; }

        /// <inheritdoc />
        public LogTime LogTime { get; set; }

        /// <inheritdoc />
        public Boolean ShowTime { get; set; }

        /// <inheritdoc />
        public String TimeFormat { get; set; }

        /// <inheritdoc />
        public Char PartSplit { get; set; }

        /// <inheritdoc />
        public Boolean FullName { get; set; }

        /// <inheritdoc />
        public CultureInfo Culture { get; set; }
    }
}
