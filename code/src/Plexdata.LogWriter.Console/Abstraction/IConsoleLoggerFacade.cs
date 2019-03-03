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

using Plexdata.LogWriter.Definitions.Console;
using System;

namespace Plexdata.LogWriter.Abstraction
{
    /// <summary>
    /// This interface represents all actions that are possible 
    /// with the implementation of the native console.
    /// </summary>
    /// <remarks>
    /// The interface is required as an abstraction between the 
    /// console logger itself and the native console implementation. 
    /// This interface might be re-implemented if a different access 
    /// to the native console becomes necessary.
    /// </remarks>
    public interface IConsoleLoggerFacade
    {
        /// <summary>
        /// Gets the number of references actually attached to the console facade.
        /// </summary>
        /// <remarks>
        /// Generally speaking, a console window can only be attached once per process. 
        /// For this reason a reference counting is necessary.
        /// </remarks>
        /// <value>
        /// The number of currently attached logger references.
        /// </value>
        Int32 References { get; }

        /// <summary>
        /// Determines whether the facade is attached to a console window.
        /// </summary>
        /// <remarks>
        /// This property represents some kind of management information and might 
        /// not really be interesting during runtime.
        /// </remarks>
        /// <value>
        /// True if the facade is attached and false otherwise.
        /// </value>
        Boolean IsAttached { get; }

        /// <summary>
        /// Determines whether the facade must detached the console window.
        /// </summary>
        /// <remarks>
        /// Detaching a console window must be done for example when the calling process 
        /// does not already own a console window.
        /// </remarks>
        /// <value>
        /// True if the facade must detached the console window and false if not.
        /// </value>
        Boolean MustDetach { get; }

        /// <summary>
        /// Gets and sets whether the console facade should use colored messages.
        /// </summary>
        /// <remarks>
        /// Message coloring might be helpful to be able to distinguish different message 
        /// types such as errors and warnings.
        /// </remarks>
        /// <value>
        /// True if colored message should be used and false if not.
        /// </value>
        Boolean UseColors { get; set; }

        /// <summary>
        /// Gets and sets the foreground color to be used.
        /// </summary>
        /// <remarks>
        /// The foreground color just represents the color of the message text.
        /// </remarks>
        /// <value>
        /// The console color to be used as foreground.
        /// </value>
        ConsoleColor Foreground { get; set; }

        /// <summary>
        /// Gets and sets the background color to be used.
        /// </summary>
        /// <remarks>
        /// The background color represents the color behind the message text.
        /// </remarks>
        /// <value>
        /// The console color to be used as background.
        /// </value>
        ConsoleColor Background { get; set; }

        /// <summary>
        /// Gets and sets the title text of the console window.
        /// </summary>
        /// <remarks>
        /// Changing the console window title might be useful especially for processes 
        /// that do not already own a console window.
        /// </remarks>
        /// <value>
        /// The text of the console window title.
        /// </value>
        String WindowTitle { get; set; }

        /// <summary>
        /// Enables or disables the quick-edit mode of the console window.
        /// </summary>
        /// <remarks>
        /// The quick-edit mode represents a special feature of the Windows console 
        /// and allows to mark and copy text.
        /// </remarks>
        /// <value>
        /// True to enable the quick-edit mode and false to disable it.
        /// </value>
        Boolean QuickEdit { get; set; }

        /// <summary>
        /// Gets and sets the dimension of the console window.
        /// </summary>
        /// <remarks>
        /// The dimension (number of rows and columns) represents another special feature 
        /// of a Windows console and defines the size of underlying output buffer.
        /// </remarks>
        /// <value>
        /// The number of rows and columns of the console buffer.
        /// </value>
        Dimension BufferSize { get; set; }

        /// <summary>
        /// This method safely attaches the console window.
        /// </summary>
        /// <remarks>
        /// Safely attaching actually means that a console window is created only if 
        /// necessary. For example a console application already owns a console window. 
        /// In such a case nothing is gonna happen.
        /// </remarks>
        void Attach();

        /// <summary>
        /// This method safely detaches the console window.
        /// </summary>
        /// <remarks>
        /// Safely detaching means that the console window is destroyed only if it is 
        /// required. For example a console application already owns a console window. 
        /// Therefore, the facade cannot destroy this window because otherwise the process 
        /// would be terminated as well. In such a case nothing is gonna happen.
        /// </remarks>
        void Detach();

        /// <summary>
        /// This method writes provided message out to the console window.
        /// </summary>
        /// <remarks>
        /// This method represents the major function of this facade because it is going 
        /// to perform the real work.
        /// </remarks>
        /// <param name="message">
        /// The message text to be written.
        /// </param>
        void Write(String message);

        /// <summary>
        /// This method flushes the output stream.
        /// </summary>
        /// <remarks>
        /// Flushing represents a standard stream behaviour which causes an update of 
        /// the actual output.
        /// </remarks>
        void Flush();
    }
}
