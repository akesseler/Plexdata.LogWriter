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

using Plexdata.LogWriter.Definitions;
using System;
using System.Collections.Generic;

namespace Plexdata.LogWriter.Abstraction
{
    /// <summary>
    /// This interface represents the scope of settings used together 
    /// with the console logger.
    /// </summary>
    /// <remarks>
    /// The console logger settings extend the basic logger settings 
    /// by additional information that are only used in conjunction 
    /// with the specific console logger.
    /// </remarks>
    public interface IConsoleLoggerSettings : ILoggerSettings
    {
        /// <summary>
        /// Enables or disables the coloring of messages.
        /// </summary>
        /// <remarks>
        /// Text coloring is a special feature of a console window and may 
        /// help to distinguish different message type from each other.
        /// </remarks>
        /// <value>
        /// True if message coloring is enabled and false if it is disabled.
        /// </value>
        Boolean UseColors { get; set; }

        /// <summary>
        /// Gets and sets the title of the console window.
        /// </summary>
        /// <remarks>
        /// Modifying the title of the console window might be useful to 
        /// distinguish a console logging window from other console windows.
        /// </remarks>
        /// <value>
        /// The text to be used as console window title.
        /// </value>
        String WindowTitle { get; set; }

        /// <summary>
        /// Enables or disables the quick-edit mode of a console window.
        /// </summary>
        /// <remarks>
        /// The quick-edit mode represents a special feature of the Windows 
        /// console and allows to mark and copy text.
        /// </remarks>
        /// <value>
        /// True to enable the quick-edit mode and false to disable it.
        /// </value>
        Boolean QuickEdit { get; set; }

        /// <summary>
        /// Gets and sets the dimension of the console window.
        /// </summary>
        /// <remarks>
        /// The dimension (number of rows and columns) represents another 
        /// special feature of a Windows console and defines the size of 
        /// underlying output buffer.
        /// </remarks>
        /// <value>
        /// The number of rows and columns of the console buffer.
        /// </value>
        Dimension BufferSize { get; set; }

        /// <summary>
        /// Gets and sets the list of colors to be used for different message types.
        /// </summary>
        /// <remarks>
        /// The coloring list is actually a dictionary with the logging level as 
        /// key. Each logging level is assigned to a particular color setup for 
        /// the foreground and the background.
        /// </remarks>
        /// <value>
        /// The assignment list of message specific foreground and background colors.
        /// </value>
        IDictionary<LogLevel, Coloring> Coloring { get; }
    }
}
