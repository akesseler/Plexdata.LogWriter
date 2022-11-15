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

using Plexdata.LogWriter.Definitions;
using System;
using System.ComponentModel;
using System.Globalization;

namespace Plexdata.LogWriter.Abstraction
{
    /// <summary>
    /// The general interface of all provided logger settings implementations.
    /// </summary>
    /// <remarks>
    /// The logger settings interface provides information that are essential 
    /// for each of the supported <i>Plexdata Logging Writers</i>.
    /// </remarks>
    public interface ILoggerSettings : INotifyPropertyChanged
    {
        /// <summary>
        /// Gets and sets the logging level to be used.
        /// </summary>
        /// <remarks>
        /// The logging level is used to tag messages with a (hopefully) unique 
        /// identifier. This makes is easier to distinguish for example logging 
        /// information from real problems.
        /// </remarks>
        /// <value>
        /// One of the values of enumeration <see cref="Plexdata.LogWriter.Definitions.LogLevel"/>
        /// </value>
        LogLevel LogLevel { get; set; }

        /// <summary>
        /// Gets and sets the output format to be used.
        /// </summary>
        /// <remarks>
        /// The logging type defines how the output is formatted. Possible formats are 
        /// <see cref="LogType.Raw"/>, <see cref="LogType.Csv"/> or <see cref="LogType.Json"/>.
        /// </remarks>
        /// <value>
        /// One of the possible format types of the <see cref="Plexdata.LogWriter.Definitions.LogType"/> 
        /// enumeration.
        /// </value>
        LogType LogType { get; set; }

        /// <summary>
        /// Gets and sets the type of time to be used.
        /// </summary>
        /// <remarks>
        /// The logging time is used to determine the proper date time value.
        /// </remarks>
        /// <value>
        /// One of the values of enumeration <see cref="Plexdata.LogWriter.Definitions.LogTime"/>
        /// </value>
        LogTime LogTime { get; set; }

        /// <summary>
        /// Determines whether the message timestamp is shown or not.
        /// </summary>
        /// <remarks>
        /// Showing the timestamp of a particular message is very useful. But sometimes it is more 
        /// useful to hide this timestamp. For this purpose this property can be used.
        /// </remarks>
        /// <value>
        /// True, the message timestamp is visible and false the timestamp is hidden.
        /// </value>
        Boolean ShowTime { get; set; }

        /// <summary>
        /// Gets and sets the time format to be used.
        /// </summary>
        /// <remarks>
        /// The time format string is used to define how a timestamp has to look. As default 
        /// value an adaptation of the standard ISO format is used.
        /// </remarks>
        /// <value>
        /// Any date and time format that is valid for method <see cref="DateTime.ToString(String)"/>.
        /// </value>
        String TimeFormat { get; set; }

        /// <summary>
        /// Gets and sets the character to separate each of the message parts.
        /// </summary>
        /// <remarks>
        /// The message part separator is used to be able to distinguish each part of a log-message 
        /// from other log-message parts. The value of this property might be changed to use a separator 
        /// different from the default separator.
        /// </remarks>
        /// <value>
        /// The character to separate the message parts.
        /// </value>
        Char PartSplit { get; set; }

        /// <summary>
        /// Determines whether the full name is used in messages or not.
        /// </summary>
        /// <remarks>
        /// The usage of full name applies especially to type of the context as well as the scope 
        /// type. With this property it becomes possible to show the full name (including fully 
        /// qualified namespace) respectively to show the short name in all messages. But keep in 
        /// mind, a distinction between full name and short name is only possible as long as the 
        /// provided type does it support as well.
        /// </remarks>
        /// <value>
        /// True, if the full name should be used in messages and false if not.
        /// </value>
        Boolean FullName { get; set; }

        /// <summary>
        /// Gets and sets the culture information to be used.
        /// </summary>
        /// <remarks>
        /// The culture information is used for example to convert values into its 
        /// string representation.
        /// </remarks>
        /// <value>
        /// The used culture information instance.
        /// </value>
        CultureInfo Culture { get; set; }
    }
}
