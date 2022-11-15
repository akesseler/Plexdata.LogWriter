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
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;

namespace Plexdata.LogWriter.Internals.Settings
{
    /// <summary>
    /// The class that represents a XML configuration file section.
    /// </summary>
    /// <remarks>
    /// Instances of this class are used together with configuration sections of a XML file.
    /// </remarks>
    internal class LoggerSettingsXml : LoggerSettingsBase, ILoggerSettingsSection
    {
        #region Construction

        /// <summary>
        /// The stream-based class constructor.
        /// </summary>
        /// <remarks>
        /// This constructor applies its <see cref="LoggerSettingsBase.Parent"/> 
        /// property by loading all data from provided stream.
        /// </remarks>
        /// <param name="stream">
        /// The stream to load the configuration from.
        /// </param>
        /// <seealso cref="Load(Stream)"/>
        public LoggerSettingsXml(Stream stream)
            : base()
        {
            base.Parent = this.Load(stream);
        }

        /// <summary>
        /// The element-based class constructor.
        /// </summary>
        /// <remarks>
        /// This constructor just applies provided <paramref name="parent"/> to its 
        /// <see cref="LoggerSettingsBase.Parent"/> property. The applied parent might 
        /// be <c>null</c>.
        /// </remarks>
        /// <param name="parent">
        /// The parent element to be assigned.
        /// </param>
        public LoggerSettingsXml(XElement parent)
            : base()
        {
            base.Parent = parent;
        }

        #endregion

        #region Public properties

        /// <summary>
        /// Gets a particular settings value.
        /// </summary>
        /// <remarks>
        /// This property tries to find a particular settings value for a provided key. An 
        /// empty string is returned in case of no value could be found.
        /// </remarks>
        /// <param name="key">
        /// The key to get a value for.
        /// </param>
        /// <value>
        /// The value assigned to provided key or an empty string in any case of an error.
        /// </value>
        /// <seealso cref="LoggerSettingsBase.FindValue(String)"/>
        public String this[String key]
        {
            get
            {
                return base.FindValue(key);
            }
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Gets the subsection of provided section key.
        /// </summary>
        /// <remarks>
        /// This method tries to find the subsection for a particular section key. The section 
        /// key may contain multiple section parts. But each part must be separated by a colon.
        /// </remarks>
        /// <param name="key">
        /// The key to get a configuration section for.
        /// </param>
        /// <returns>
        /// An instance of <see cref="ILoggerSettingsSection"/> that represents a configuration 
        /// subsection. This method never returns <c>null</c>.
        /// </returns>
        /// <seealso cref="LoggerSettingsBase.FindSection(String)"/>
        public ILoggerSettingsSection GetSection(String key)
        {
            return new LoggerSettingsXml(base.FindSection(key));
        }

        /// <summary>
        /// Gets the values of provided section key.
        /// </summary>
        /// <remarks>
        /// This method tries to find all values for a particular section key. The section 
        /// key may contain multiple section parts. But each part must be separated by a colon.
        /// </remarks>
        /// <param name="key">
        /// The key to get a list of section values for.
        /// </param>
        /// <returns>
        /// The list of section values. This method never returns <c>null</c>.
        /// </returns>
        /// <seealso cref="LoggerSettingsBase.FindSection(String)"/>
        public IEnumerable<String> GetValues(String key)
        {
            try
            {
                XElement section = base.FindSection(key);

                if (section == null)
                {
                    return Enumerable.Empty<String>();
                }

                // NOTE: This piece of code might never throw any exception.
                IEnumerable<String> result = section.Elements().Select(x => x.Value);

                // NOTE: This piece of code might never be hit.
                if (result == null)
                {
                    return Enumerable.Empty<String>();
                }

                return result;
            }
            catch
            {
                return Enumerable.Empty<String>();
            }
        }

        #endregion

        #region Protected methods

        /// <summary>
        /// Loads the configuration from provided stream.
        /// </summary>
        /// <remarks>
        /// Task of this method is to load the configuration from provided stream. 
        /// This method interprets the stream content as a XML formatted configuration.
        /// The method does never throw any exception.
        /// </remarks>
        /// <param name="stream">
        /// The stream to load the configuration from.
        /// </param>
        /// <returns>
        /// The root element of the loaded configuration or <c>null</c> in case of 
        /// an error.
        /// </returns>
        protected override XElement Load(Stream stream)
        {
            if (stream == null)
            {
                return null;
            }

            try
            {
                // NOTE: This piece of code might never throw any exception.
                using (XmlReader reader = XmlReader.Create(stream, new XmlReaderSettings()))
                {
                    return base.Load(reader);
                }
            }
            catch
            {
                return null;
            }
        }

        #endregion
    }
}
