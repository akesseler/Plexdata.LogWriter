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

using System;
using System.Collections.Generic;

namespace Plexdata.LogWriter.Abstraction
{
    /// <summary>
    /// A type representing a section of the logger settings configuration.
    /// </summary>
    /// <remarks>
    /// This interface representing a section of the logger settings configuration. It is 
    /// actually meant as replacement of interface <c>IConfiguration</c>, which could not 
    /// be used because of some version conflicts.
    /// </remarks>
    /// <example>
    /// The code snippet below demonstrates how to obtain and use logger settings section 
    /// as well as how to query a value.
    /// <code language="C#">
    /// ILoggerSettingsBuilder builder = new LoggerSettingsBuilder();
    /// builder.SetFilename("logger-settings.json");
    /// ILoggerSettingsSection settings = builder.Build();
    /// ILoggerSettingsSection section = settings.GetSection("plexdata:logwriter:settings");
    /// String value = section["loglevel"];
    /// </code>
    /// </example>
    /// <seealso cref="ILoggerSettingsBuilder"/>
    public interface ILoggerSettingsSection
    {
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
        String this[String key] { get; }

        /// <summary>
        /// The path of this configuration section.
        /// </summary>
        /// <remarks>
        /// The section path represents the hierarchical position inside the underlying 
        /// configuration.
        /// </remarks>
        /// <value>
        /// The section path or an empty string.
        /// </value>
        String Path { get; }

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
        ILoggerSettingsSection GetSection(String key);

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
        IEnumerable<String> GetValues(String key);
    }
}
