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

using System;

namespace Plexdata.LogWriter.Abstraction
{
    /// <summary>
    /// A type used to build logger settings from external configuration.
    /// </summary>
    /// <remarks>
    /// This interface represents a type used to build logger settings from external configuration. 
    /// It is actually meant as replacement of interface <c>IConfigurationBuilder</c>, which could 
    /// not be used because of some version conflicts.
    /// </remarks>
    /// <example>
    /// The code snippet below demonstrates how to instantiate and use the logger settings builder.
    /// <code language="C#">
    /// ILoggerSettingsBuilder builder = new LoggerSettingsBuilder();
    /// builder.SetFilename("logger-settings.json");
    /// ILoggerSettingsSection settings = builder.Build();
    /// </code>
    /// </example>
    /// <seealso cref="ILoggerSettingsSection"/>
    public interface ILoggerSettingsBuilder
    {
        /// <summary>
        /// The filename used to build the configuration.
        /// </summary>
        /// <remarks>
        /// This property represents the filename that will be used to build the configuration.
        /// </remarks>
        /// <value>
        /// The fully qualified name of the used configuration file.
        /// </value>
        String FileName { get; }

        /// <summary>
        /// The type of the configuration file.
        /// </summary>
        /// <remarks>
        /// The file type is determined either by the extension of a provided filename or it is 
        /// manually set by the caller.
        /// </remarks>
        /// <value>
        /// The type of the used configuration file.
        /// </value>
        String FileType { get; }

        /// <summary>
        /// Sets the filename to be used to load the logger settings from.
        /// </summary>
        /// <remarks>
        /// This method sets the filename to be used to load the logger settings from. The file 
        /// type is determined by the extension of provided filename. Supported types are JSON 
        /// and XML file formats. Note that the configuration file is expected within current 
        /// directory if no file path is applied.
        /// </remarks>
        /// <param name="filename">
        /// The name of the used file.
        /// </param>
        /// <seealso cref="SetFilename(String, String)"/>
        void SetFilename(String filename);

        /// <summary>
        /// Sets the filename and type to be used to load the logger settings from.
        /// </summary>
        /// <remarks>
        /// This method sets the filename and type to be used to load the logger settings from. 
        /// The type is determined from parameter <paramref name="filetype"/>. Allowed are the 
        /// types JSON and XML. Any other type will be ignored. Furthermore, nothing does happen 
        /// if either the filename is empty, or the file does not exist, or if the file type  is 
        /// not supported. Note that the configuration file is expected within current directory 
        /// if no file path is applied.
        /// </remarks>
        /// <param name="filename">
        /// The name of the used file.
        /// </param>
        /// <param name="filetype">
        /// The type of the used file.
        /// </param>
        void SetFilename(String filename, String filetype);

        /// <summary>
        /// Builds the logger settings configuration instance.
        /// </summary>
        /// <remarks>
        /// This method builds the logger settings configuration instance by processing the 
        /// provided file. This method never returns null or throws any exception. An empty 
        /// instance is returned instead.
        /// </remarks>
        /// <returns>
        /// An instance of type <see cref="ILoggerSettingsSection"/> that includes the applied 
        /// logger settings.
        /// </returns>
        ILoggerSettingsSection Build();
    }
}
