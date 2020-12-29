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
using Plexdata.LogWriter.Internals.Settings;
using System;
using System.IO;

namespace Plexdata.LogWriter.Settings
{
    /// <summary>
    /// The default implementation that builds the logger settings from external configuration.
    /// </summary>
    /// <remarks>
    /// Mainly, this implementation serves as a simply replacement of type <c>IConfigurationBuilder</c> 
    /// and has been created to solve conflicts with different versions of that interface.
    /// </remarks>
    public class LoggerSettingsBuilder : ILoggerSettingsBuilder
    {
        #region Private enumerations

        /// <summary>
        /// An internal enumeration to manage all supported file types.
        /// </summary>
        /// <remarks>
        /// The file types are only for internal usage.
        /// </remarks>
        private enum FileTypes
        {
            /// <summary>
            /// File type is unknown or unsupported.
            /// </summary>
            Unknown,

            /// <summary>
            /// File is of type JSON. 
            /// </summary>
            Json,

            /// <summary>
            /// File is of type XML. 
            /// </summary>
            Xml
        }

        #endregion

        #region Private fields

        /// <summary>
        /// This field represents the used filename.
        /// </summary>
        /// <remarks>
        /// This field always contains a fully qualified filename.
        /// </remarks>
        private String filename = String.Empty;

        /// <summary>
        /// This field represents the used filetype.
        /// </summary>
        /// <remarks>
        /// This type is determined either by the extension of a file 
        /// or manually defined by the caller.
        /// </remarks>
        private FileTypes filetype = FileTypes.Unknown;

        #endregion

        #region Construction

        /// <summary>
        /// The default class constructor.
        /// </summary>
        /// <remarks>
        /// This default constructor just initializes all properties with its default 
        /// values.
        /// </remarks>
        /// <seealso cref="LoggerSettingsBuilder(String, String)"/>
        public LoggerSettingsBuilder()
            : this(null, null)
        {
        }

        /// <summary>
        /// The extended class constructor.
        /// </summary>
        /// <remarks>
        /// This constructor initialize the class with provided filename. The type is 
        /// determined by the extension of the filename.
        /// </remarks>
        /// <param name="filename">
        /// The name of the used file.
        /// </param>
        /// <seealso cref="LoggerSettingsBuilder(String, String)"/>
        public LoggerSettingsBuilder(String filename)
            : this(filename, null)
        {
        }

        /// <summary>
        /// The fully parameterized class constructor.
        /// </summary>
        /// <remarks>
        /// This constructor initialize the class with provided filename. The type is 
        /// determined from provided parameter.
        /// </remarks>
        /// <param name="filename">
        /// The name of the used file.
        /// </param>
        /// <param name="filetype">
        /// The type of the used file.
        /// </param>
        public LoggerSettingsBuilder(String filename, String filetype)
            : base()
        {
            this.SetFilename(filename, filetype);
        }

        #endregion

        #region Public properties

        /// <inheritdoc />
        public String FileName
        {
            get
            {
                if (String.IsNullOrEmpty(this.filename))
                {
                    return String.Empty;
                }

                return this.filename;
            }
        }

        /// <inheritdoc />
        public String FileType
        {
            get
            {
                return this.filetype.ToString();
            }
        }

        #endregion

        #region Public methods

        /// <inheritdoc />
        public void SetFilename(String filename)
        {
            this.SetFilename(filename, null);
        }

        /// <inheritdoc />
        public void SetFilename(String filename, String filetype)
        {
            if (String.IsNullOrWhiteSpace(filename))
            {
                return;
            }

            if (String.IsNullOrWhiteSpace(Path.GetDirectoryName(filename)))
            {
                filename = Path.Combine(Directory.GetCurrentDirectory(), filename);
            }

            if (!File.Exists(filename))
            {
                return;
            }

            if (String.IsNullOrWhiteSpace(filetype))
            {
                filetype = Path.GetExtension(filename).TrimStart('.');
            }

            if (this.IsEqual(filetype, FileTypes.Json.ToString()))
            {
                this.filetype = FileTypes.Json;
            }

            if (this.IsEqual(filetype, FileTypes.Xml.ToString()))
            {
                this.filetype = FileTypes.Xml;
            }

            if (this.filetype == FileTypes.Unknown)
            {
                return;
            }

            this.filename = filename;
        }

        /// <inheritdoc />
        public ILoggerSettingsSection Build()
        {
            try
            {
                if (!String.IsNullOrWhiteSpace(this.filename))
                {
                    using (Stream stream = new FileStream(this.filename, FileMode.Open, FileAccess.Read, FileShare.Read))
                    {
                        switch (this.filetype)
                        {
                            case FileTypes.Json:
                                return new LoggerSettingsJson(stream);
                            case FileTypes.Xml:
                                return new LoggerSettingsXml(stream);
                        }
                    }
                }
            }
            catch { }

            return new LoggerSettingsEmpty();
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Determines the equality of two string.
        /// </summary>
        /// <remarks>
        /// This method determines the equality of two string by comparing them without 
        /// case sensitivity.
        /// </remarks>
        /// <param name="x">
        /// The left string to be compared.
        /// </param>
        /// <param name="y">
        /// The right string to be compared.
        /// </param>
        /// <returns>
        /// True, if both strings are considered as equal and false otherwise.
        /// </returns>
        private Boolean IsEqual(String x, String y)
        {
            return String.Compare(x, y, StringComparison.OrdinalIgnoreCase) == 0;
        }

        #endregion
    }
}
