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
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace Plexdata.LogWriter.Internals.Settings
{
    /// <summary>
    /// The class of any empty logger settings.
    /// </summary>
    /// <remarks>
    /// Instances of this class are used for example if no logger settings section 
    /// could be found for a particular section key.
    /// </remarks>
    internal class LoggerSettingsEmpty : LoggerSettingsBase, ILoggerSettingsSection
    {
        #region Construction

        /// <summary>
        /// The default class constructor.
        /// </summary>
        /// <remarks>
        /// This default constructor just initializes all properties with its default 
        /// values.
        /// </remarks>
        public LoggerSettingsEmpty()
            : base()
        {
        }

        #endregion

        #region Public properties

        /// <summary>
        /// Gets a particular settings value.
        /// </summary>
        /// <remarks>
        /// This property always returns an empty string.
        /// </remarks>
        /// <param name="key">
        /// The key to get a value for.
        /// </param>
        /// <value>
        /// An empty string is always returned.
        /// </value>
        public String this[String key]
        {
            get
            {
                return String.Empty;
            }
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Gets the subsection of provided section key.
        /// </summary>
        /// <remarks>
        /// This method does nothing and always returns itself.
        /// </remarks>
        /// <param name="key">
        /// The key to get a configuration section for.
        /// </param>
        /// <returns>
        /// A self reference is always returned.
        /// </returns>
        public ILoggerSettingsSection GetSection(String key)
        {
            return this;
        }

        /// <summary>
        /// Gets the values of provided section key.
        /// </summary>
        /// <remarks>
        /// This method does nothing and always returns an empty string list.
        /// </remarks>
        /// <param name="key">
        /// The key to get a list of section values for.
        /// </param>
        /// <returns>
        /// An empty string list is always returned.
        /// </returns>
        public IEnumerable<String> GetValues(String key)
        {
            return Enumerable.Empty<String>();
        }

        #endregion

        #region  Protected methods

        /// <summary>
        /// Loads the configuration from provided stream.
        /// </summary>
        /// <remarks>
        /// This method does nothing.
        /// </remarks>
        /// <param name="stream">
        /// The stream to load the configuration from.
        /// </param>
        /// <returns>
        /// This method always returns <c>null</c>.
        /// </returns>
        protected override XElement Load(Stream stream)
        {
            return null;
        }

        #endregion
    }
}
