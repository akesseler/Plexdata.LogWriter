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
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Text;

namespace Plexdata.LogWriter.Internals.Native.Windows
{
    /// <summary>
    /// This class provides native access to the console window.
    /// </summary>
    /// <remarks>
    /// Please be aware, this class cannot be used on platforms other than Windows!
    /// This is because of this class uses the Win32-API to accomplish its tasks.
    /// </remarks>
    [ExcludeFromCodeCoverage]
    internal class SystemConsoleWrapper
    {
        #region Private fields

        /// <summary>
        /// This field holds the singleton instance of this class.
        /// </summary>
        /// <remarks>
        /// The instance of class <see cref="SystemConsoleWrapper"/> that represents the used 
        /// singleton. This instance is created automatically during the very first call 
        /// to property <see cref="Instance"/>.
        /// </remarks>
        private static SystemConsoleWrapper instance = null;

        /// <summary>
        /// This field holds the native file handle to the output stream.
        /// </summary>
        /// <remarks>
        /// Usually, an output into a console window takes place by using the standard output 
        /// stream. But for this class using this standard output stream is not applicable. 
        /// Therefore, an independent writing mechanism is used through this handle.
        /// </remarks>
        private IntPtr hOutput = IntPtr.Zero;

        /// <summary>
        /// This field holds the file name of the standard input stream.
        /// </summary>
        /// <remarks>
        /// The file name of the standard input stream is required to query information 
        /// from the console window.
        /// </remarks>
        private readonly String inputName = "CONIN$";

        /// <summary>
        /// This field holds the file name of the standard output stream.
        /// </summary>
        /// <remarks>
        /// The file name of the standard output stream is used to create the file handle 
        /// which is used the write data into the console window.
        /// </remarks>
        /// <seealso cref="hOutput"/>
        private readonly String outputName = "CONOUT$";

        /// <summary>
        /// This field holds the instance of the internal synchronization object.
        /// </summary>
        /// <remarks>
        /// Writing data into a console window should be thread-safely locked. For this purpose 
        /// this object is used.
        /// </remarks>
        /// <seealso cref="Attach"/>
        /// <seealso cref="Detach"/>
        /// <seealso cref="Write(String)"/>
        private readonly Object interlock = new Object();

        #endregion

        #region Construction

        /// <summary>
        /// The private default constructor just initializes almost all properties with its 
        /// default values.
        /// </summary>
        /// <remarks>
        /// This constructor is intentionally made private to  prevent this class from being 
        /// instantiated more than once.
        /// </remarks>
        private SystemConsoleWrapper()
        {
            this.References = 0;

            this.IsAttached = false;
            this.MustDetach = false;

            this.UseColors = false;
            this.Foreground = ConsoleColor.Gray;
            this.Background = ConsoleColor.Black;

            this.Attach();
        }

        /// <summary>
        /// The static class constructor.
        /// </summary>
        /// <remarks>
        /// This constructor does actually do nothing.
        /// </remarks>
        static SystemConsoleWrapper() { }

        /// <summary>
        /// The class destructor.
        /// </summary>
        /// <remarks>
        /// The class destructor actually forces a detach from an attached console window.
        /// </remarks>
        ~SystemConsoleWrapper()
        {
            this.References = 0;
            this.Detach();
        }

        #endregion

        #region Public properties

        /// <summary>
        /// Gets the singleton instance of this class.
        /// </summary>
        /// <remarks>
        /// Using a singleton instance is necessary to ensure that only one console window exists 
        /// and it remains open over this whole live time.
        /// </remarks>
        /// <value>
        /// An instance of this class that can be used from everywhere.
        /// </value>
        public static SystemConsoleWrapper Instance
        {
            get
            {
                if (SystemConsoleWrapper.instance == null)
                {
                    SystemConsoleWrapper.instance = new SystemConsoleWrapper();
                }

                return SystemConsoleWrapper.instance;
            }
        }

        /// <summary>
        /// Gets current number of referenced instances.
        /// </summary>
        /// <remarks>
        /// The number of references is incremented during an attach operation and is decremented 
        /// when detaching takes place.
        /// </remarks>
        /// <value>
        /// The number of instances that are currently attached.
        /// </value>
        public Int32 References { get; private set; }

        /// <summary>
        /// Gets current attach state of the console window.
        /// </summary>
        /// <remarks>
        /// This property will not switch that often from true to false and vice versa. This is 
        /// because of that only one console window can be attached per process.
        /// </remarks>
        /// <value>
        /// True if a console window is actually attached and false if not.
        /// </value>
        /// <seealso cref="Attach()"/>
        /// <seealso cref="Detach()"/>
        public Boolean IsAttached { get; private set; }

        /// <summary>
        /// Gets the state if the console window must be detached.
        /// </summary>
        /// <remarks>
        /// This property is actually an internal management value and might be therefore not really 
        /// relevant for an external caller. Just to point out, this property will never become true 
        /// as long as the current process is already a console application.
        /// </remarks>
        /// <value>
        /// True if a console window must be detached and false if not.
        /// </value>
        public Boolean MustDetach { get; private set; }

        /// <summary>
        /// Determines if message coloring should be used or not.
        /// </summary>
        /// <remarks>
        /// Message coloring allows the use of predefined colors for each written message 
        /// independently.
        /// </remarks>
        /// <value>
        /// True to enable the usage of message coloring and false to disable it.
        /// </value>
        /// <seealso cref="Foreground"/>
        /// <seealso cref="Background"/>
        public Boolean UseColors { get; set; }

        /// <summary>
        /// Gets or sets the text color to be used.
        /// </summary>
        /// <remarks>
        /// Changing current foreground color affects the whole message text to be written.
        /// </remarks>
        /// <value>
        /// One of the predefined console colors.
        /// </value>
        /// <seealso cref="UseColors"/>
        /// <seealso cref="Background"/>
        public ConsoleColor Foreground { get; set; }

        /// <summary>
        /// Gets or sets the background color to be used.
        /// </summary>
        /// <remarks>
        /// Changing current background color affects the whole message text to be written.
        /// written.
        /// </remarks>
        /// <value>
        /// One of the predefined console colors.
        /// </value>
        /// <seealso cref="UseColors"/>
        /// <seealso cref="Foreground"/>
        public ConsoleColor Background { get; set; }

        /// <summary>
        /// Gets and sets the title of the console window.
        /// </summary>
        /// <remarks>
        /// Changing current console window title may cause conflicts with other instances 
        /// attached to this class.
        /// </remarks>
        /// <value>
        /// A string representing current console window title.
        /// </value>
        /// <seealso cref="GetWindowTitle()"/>
        /// <seealso cref="SetWindowTitle(String)"/>
        public String WindowTitle
        {
            get
            {
                return this.GetWindowTitle();
            }
            set
            {
                this.SetWindowTitle(value);
            }
        }

        /// <summary>
        /// Enables or disables the Quick Edit Mode of current console window.
        /// </summary>
        /// <remarks>
        /// The Quick Edit Mode represents a feature of the console screen buffer that allows 
        /// to mark a portion of available output and copy it into the clipboard. But be aware 
        /// enabling or disabling the Quick Edit Mode will effect the whole console window.
        /// </remarks>
        /// <value>
        /// True to enable the Quick Edit Mode and false to disable it.
        /// </value>
        /// <seealso cref="GetQuickEditMode()"/>
        /// <seealso cref="SetQuickEditMode(Boolean)"/>
        public Boolean QuickEdit
        {
            get
            {
                return this.GetQuickEditMode();
            }
            set
            {
                this.SetQuickEditMode(value);
            }
        }

        /// <summary>
        /// Gets or sets the number of lines and columns of the underlying console screen buffer.
        /// </summary>
        /// <remarks>
        /// As already mentioned, the console screen buffer size represents the number of columns 
        /// as well as the number of lines. But be aware, nothing will happen if provided size is 
        /// empty or if provided size is less than current console window size.
        /// </remarks>
        /// <value>
        /// The number of lines and columns of the underlying console screen buffer.
        /// </value>
        /// <seealso cref="GetScreenBufferSize()"/>
        /// <seealso cref="SetScreenBufferSize(Dimension)"/>
        public Dimension BufferSize
        {
            get
            {
                return this.GetScreenBufferSize();
            }
            set
            {
                this.SetScreenBufferSize(value);
            }
        }

        #endregion

        #region Public methods

        /// <summary>
        /// This method safely tries to attach a console window.
        /// </summary>
        /// <remarks>
        /// The behavior of attaching a console window is a little bit tricky. This is because of 
        /// the different uses cases that are supported by this method. The first question to answer 
        /// is, does the current process already own a console window. If the answer is yes, then 
        /// nothing else is necessary. But if the answer is no, then another question must be answered.
        /// This question is, does the current process require an additional console window. If the 
        /// answer on this question is yes, then it must be check whether another caller in the same 
        /// process has already attached the console window. If yes, then nothing else is needed. But 
        /// if not, then a new console window must be created. To be able to track all these different 
        /// states a check of various properties is required. All properties affected by this check are 
        /// listed below.
        /// </remarks>
        /// <seealso cref="Increment"/>
        /// <seealso cref="References"/>
        /// <seealso cref="IsAttached"/>
        /// <seealso cref="MustDetach"/>
        public void Attach()
        {
            lock (this.interlock)
            {
                this.Increment();

                if (this.IsAttached) { return; }

                if (GetConsoleWindow() == IntPtr.Zero)
                {
                    if (!AllocConsole()) { /* TODO: Error handling. */ return; }

                    this.MustDetach = true;
                }

                if (this.hOutput != IntPtr.Zero && this.hOutput != INVALID_HANDLE_VALUE)
                {
                    CloseHandle(this.hOutput);
                    this.hOutput = IntPtr.Zero;
                }

                // This is the way of how to force an output into the console window 
                // instead of writing the stuff into the output window of Visual Studio.
                this.hOutput = CreateFile(this.outputName, GENERIC_READ | GENERIC_WRITE, FILE_SHARE_WRITE, IntPtr.Zero, OPEN_EXISTING, 0, IntPtr.Zero);

                this.IsAttached = this.hOutput != IntPtr.Zero && this.hOutput != INVALID_HANDLE_VALUE;
            }
        }

        /// <summary>
        /// This method safely tries to detach a console window.
        /// </summary>
        /// <remarks>
        /// Detaching a console window is a little bit tricky as well. But keeping it simple, here 
        /// just a short explanation. Nothing is to do if current process is a console application.
        /// Otherwise, if current process is not a console application and this call represents the 
        /// last detach then close the previously attached console window.
        /// </remarks>
        /// <seealso cref="Decrement"/>
        /// <seealso cref="References"/>
        /// <seealso cref="IsAttached"/>
        /// <seealso cref="MustDetach"/>
        public void Detach()
        {
            lock (this.interlock)
            {
                this.Decrement();

                if (this.References == 0)
                {
                    if (this.hOutput != IntPtr.Zero && this.hOutput != INVALID_HANDLE_VALUE)
                    {
                        CloseHandle(this.hOutput);
                        this.hOutput = IntPtr.Zero;
                    }

                    if (this.MustDetach)
                    {
                        if (!FreeConsole()) { /* TODO: Error handling. */ }
                    }

                    this.MustDetach = false;
                    this.IsAttached = false;
                }
            }
        }

        /// <summary>
        /// This method writes provided message into the attached console window.
        /// </summary>
        /// <remarks>
        /// Each message is written by applying current styling. Furthermore, a new line is 
        /// appended automatically but only if provided message does not have a line break 
        /// appended already.
        /// </remarks>
        /// <param name="message">
        /// The message to be written.
        /// </param>
        public void Write(String message)
        {
            if (!this.IsAttached) { return; }

            if (this.hOutput == IntPtr.Zero) { return; }

            if (this.hOutput == INVALID_HANDLE_VALUE) { return; }

            if (String.IsNullOrWhiteSpace(message)) { return; }

            lock (this.interlock)
            {
                if (message.EndsWith(Environment.NewLine))
                {
                    message = message.Remove(message.Length - Environment.NewLine.Length, Environment.NewLine.Length);
                }

                this.WriteStyled(message);

                // Writing the "End of Line" independently fixes a bug in conjunction 
                // with coloring. This coloring bug occurs when the written message 
                // reaches the end of the console screen buffer.
                this.WriteNative(Environment.NewLine);
            }
        }

        public void Flush()
        {
            // According to documentation of function FlushFileBuffers():
            // The function fails if hFile is a handle to the console output. That is 
            // because the console output is not buffered. The function returns FALSE, 
            // and GetLastError returns ERROR_INVALID_HANDLE.
            // See: https://docs.microsoft.com/en-us/windows/desktop/api/fileapi/nf-fileapi-flushfilebuffers
        }

        #endregion

        #region Private methods

        /// <summary>
        /// This method increments the internal reference counter.
        /// </summary>
        /// <remarks>
        /// For sure, using a reference counter is pretty old-school. But on the other hand, it 
        /// is required to track the number of attaches to be able to determine the last detach.
        /// </remarks>
        /// <seealso cref="References"/>
        /// <seealso cref="Decrement()"/>
        private void Increment()
        {
            this.References++;
        }

        /// <summary>
        /// This method decrements the internal reference counter.
        /// </summary>
        /// <remarks>
        /// Decrementation takes place as long as current reference counter is greater than zero. 
        /// Otherwise decrementation is stopped.
        /// </remarks>
        /// <seealso cref="References"/>
        /// <seealso cref="Increment()"/>
        private void Decrement()
        {
            this.References--;

            if (this.References < 0)
            {
                this.References = 0;
            }
        }

        /// <summary>
        /// Gets the bitwise combination of current foreground and background color.
        /// </summary>
        /// <remarks>
        /// This method gets the bitwise combination of current colors for the foreground as well 
        /// as for the background.
        /// </remarks>
        /// <returns>
        /// The flags for console foreground and background color.
        /// </returns>
        /// <seealso cref="Foreground"/>
        /// <seealso cref="Background"/>
        /// <seealso cref="GetForegroundColorFlags(ConsoleColor)"/>
        /// <seealso cref="GetBackgroundColorFlags(ConsoleColor)"/>
        private UInt16 GetConsoleColorFlags()
        {
            return unchecked((UInt16)(this.GetBackgroundColorFlags(this.Background) | this.GetForegroundColorFlags(this.Foreground)));
        }

        /// <summary>
        /// Gets the bitwise combination of requested foreground color.
        /// </summary>
        /// <remarks>
        /// This method gets the bitwise combination of current foreground color by mapping 
        /// provided value into its Win32-API representation.
        /// </remarks>
        /// <param name="color">
        /// The predefined color value to be converted.
        /// </param>
        /// <returns>
        /// The flags for console foreground color.
        /// </returns>
        /// <seealso cref="GetBackgroundColorFlags(ConsoleColor)"/>
        private UInt16 GetForegroundColorFlags(ConsoleColor color)
        {
            switch (color)
            {
                case ConsoleColor.Black:
                    return 0;
                case ConsoleColor.DarkBlue:
                    return FOREGROUND_BLUE;
                case ConsoleColor.DarkGreen:
                    return FOREGROUND_GREEN;
                case ConsoleColor.DarkCyan:
                    return FOREGROUND_GREEN | FOREGROUND_BLUE;
                case ConsoleColor.DarkRed:
                    return FOREGROUND_RED;
                case ConsoleColor.DarkMagenta:
                    return FOREGROUND_RED | FOREGROUND_BLUE;
                case ConsoleColor.DarkYellow:
                    return FOREGROUND_RED | FOREGROUND_GREEN;
                case ConsoleColor.DarkGray:
                    return FOREGROUND_INTENSITY;
                case ConsoleColor.Gray:
                    return FOREGROUND_RED | FOREGROUND_GREEN | FOREGROUND_BLUE;
                case ConsoleColor.Blue:
                    return FOREGROUND_INTENSITY | FOREGROUND_BLUE;
                case ConsoleColor.Green:
                    return FOREGROUND_INTENSITY | FOREGROUND_GREEN;
                case ConsoleColor.Cyan:
                    return FOREGROUND_INTENSITY | FOREGROUND_GREEN | FOREGROUND_BLUE;
                case ConsoleColor.Red:
                    return FOREGROUND_INTENSITY | FOREGROUND_RED;
                case ConsoleColor.Magenta:
                    return FOREGROUND_INTENSITY | FOREGROUND_RED | FOREGROUND_BLUE;
                case ConsoleColor.Yellow:
                    return FOREGROUND_INTENSITY | FOREGROUND_RED | FOREGROUND_GREEN;
                case ConsoleColor.White:
                    return FOREGROUND_INTENSITY | FOREGROUND_RED | FOREGROUND_GREEN | FOREGROUND_BLUE;
                default:
                    return this.GetForegroundColorFlags(ConsoleColor.Gray);
            }
        }

        /// <summary>
        /// Gets the bitwise combination of requested background color.
        /// </summary>
        /// <remarks>
        /// This method gets the bitwise combination of current background color by mapping 
        /// provided value into its Win32-API representation.
        /// </remarks>
        /// <param name="color">
        /// The predefined color value to be converted.
        /// </param>
        /// <returns>
        /// The flags for console background color.
        /// </returns>
        /// <seealso cref="GetForegroundColorFlags(ConsoleColor)"/>
        private UInt16 GetBackgroundColorFlags(ConsoleColor color)
        {
            switch (color)
            {
                case ConsoleColor.Black:
                    return 0;
                case ConsoleColor.DarkBlue:
                    return BACKGROUND_BLUE;
                case ConsoleColor.DarkGreen:
                    return BACKGROUND_GREEN;
                case ConsoleColor.DarkCyan:
                    return BACKGROUND_GREEN | BACKGROUND_BLUE;
                case ConsoleColor.DarkRed:
                    return BACKGROUND_RED;
                case ConsoleColor.DarkMagenta:
                    return BACKGROUND_RED | BACKGROUND_BLUE;
                case ConsoleColor.DarkYellow:
                    return BACKGROUND_RED | BACKGROUND_GREEN;
                case ConsoleColor.DarkGray:
                    return BACKGROUND_INTENSITY;
                case ConsoleColor.Gray:
                    return BACKGROUND_RED | BACKGROUND_GREEN | BACKGROUND_BLUE;
                case ConsoleColor.Blue:
                    return BACKGROUND_INTENSITY | BACKGROUND_BLUE;
                case ConsoleColor.Green:
                    return BACKGROUND_INTENSITY | BACKGROUND_GREEN;
                case ConsoleColor.Cyan:
                    return BACKGROUND_INTENSITY | BACKGROUND_GREEN | BACKGROUND_BLUE;
                case ConsoleColor.Red:
                    return BACKGROUND_INTENSITY | BACKGROUND_RED;
                case ConsoleColor.Magenta:
                    return BACKGROUND_INTENSITY | BACKGROUND_RED | BACKGROUND_BLUE;
                case ConsoleColor.Yellow:
                    return BACKGROUND_INTENSITY | BACKGROUND_RED | BACKGROUND_GREEN;
                case ConsoleColor.White:
                    return BACKGROUND_INTENSITY | BACKGROUND_RED | BACKGROUND_GREEN | BACKGROUND_BLUE;
                default:
                    return this.GetBackgroundColorFlags(ConsoleColor.Black);
            }
        }

        /// <summary>
        /// This method changes current console colors into requested colors.
        /// </summary>
        /// <remarks>
        /// Changing the current console colors takes place by modifying the color bits inside the 
        /// attributes of current screen buffer information.
        /// </remarks>
        /// <param name="hConsole">
        /// The Win32-Handle of the affected console screen buffer.
        /// </param>
        /// <param name="color">
        /// The bitset of foreground and background color to be used.
        /// </param>
        /// <returns>
        /// The previously used bitset of attributes of the affected console screen buffer or (-1) 
        /// if color usage is disabled or in case of an error.
        /// </returns>
        /// <seealso cref="RestoreConsoleAttributes(IntPtr, UInt16)"/>
        private UInt16 SetConsoleColor(IntPtr hConsole, UInt16 color)
        {
            if (!this.UseColors)
            {
                return INVALID_RESULT_VALUE;
            }

            if (!GetConsoleScreenBufferInfo(hConsole, out CONSOLE_SCREEN_BUFFER_INFO bufferInfo))
            {
                // TODO: Error handling. 
                return INVALID_RESULT_VALUE;
            }

            UInt16 result = bufferInfo.wAttributes;

            UInt16 attributes = (UInt16)(result & ~(FOREGROUND_MASK | BACKGROUND_MASK));

            if (!SetConsoleTextAttribute(hConsole, unchecked((UInt16)(attributes | color))))
            {
                // TODO: Error handling. 
                return INVALID_RESULT_VALUE;
            }

            return result;
        }

        /// <summary>
        /// This method changes all of the console screen buffer attributes.
        /// </summary>
        /// <remarks>
        /// Changing the current console screen buffer attributes takes only place if using colors 
        /// is enabled and provided attributes are not (-1).
        /// </remarks>
        /// <param name="hConsole">
        /// The Win32-Handle of the affected console screen buffer.
        /// </param>
        /// <param name="attributes">
        /// The bitset of attributes to be applied.
        /// </param>
        /// <seealso cref="SetConsoleColor(IntPtr, UInt16)"/>
        private void RestoreConsoleAttributes(IntPtr hConsole, UInt16 attributes)
        {
            if (attributes != INVALID_RESULT_VALUE)
            {
                SetConsoleTextAttribute(hConsole, attributes);
            }
        }

        /// <summary>
        /// Writes provided message out to console screen buffer by using currently applied styles.
        /// </summary>
        /// <remarks>
        /// Currently applied styles means at the moment that only coloring is used.
        /// </remarks>
        /// <param name="message">
        /// The message to be written.
        /// </param>
        /// <seealso cref="Write(String)"/>
        /// <seealso cref="WriteNative(String)"/>
        /// <seealso cref="SetConsoleColor(IntPtr, UInt16)"/>
        /// <seealso cref="RestoreConsoleAttributes(IntPtr, UInt16)"/>
        private void WriteStyled(String message)
        {
            UInt16 attributes = this.SetConsoleColor(this.hOutput, this.GetConsoleColorFlags());

            this.WriteNative(message);

            this.RestoreConsoleAttributes(this.hOutput, attributes);
        }

        /// <summary>
        /// Writes provided message out to console screen buffer.
        /// </summary>
        /// <remarks>
        /// The method puts provided message as it is into the console screen buffer.
        /// </remarks>
        /// <param name="message">
        /// The message to be written.
        /// </param>
        private void WriteNative(String message)
        {
            if (!WriteConsole(this.hOutput, message, (UInt32)message.Length, out UInt32 written, IntPtr.Zero))
            {
                // TODO: Error handling.
            }
        }

        /// <summary>
        /// The method gets the title of current console window.
        /// </summary>
        /// <remarks>
        /// Determine current console window title takes place by calling the corresponding Win32 
        /// native function.
        /// </remarks>
        /// <returns>
        /// The title of the console window or an empty string if no title is applied.
        /// </returns>
        /// <seealso cref="WindowTitle"/>
        /// <seealso cref="SetWindowTitle(String)"/>
        private String GetWindowTitle()
        {
            StringBuilder builder = new StringBuilder(MAX_PATH);

            Int32 length = (Int32)GetConsoleTitle(builder, (UInt32)builder.Capacity);

            if (length <= 0)
            {
                // TODO: Error handling. 
                return String.Empty;
            }

            return builder.ToString();
        }

        /// <summary>
        /// The method sets the title of current console window.
        /// </summary>
        /// <remarks>
        /// Changing current console window title takes place by calling the corresponding Win32 
        /// native function, but only if provided value is neither null nor empty nor contains only 
        /// whitespace.
        /// </remarks>
        /// <param name="value">
        /// The console window title to be used.
        /// </param>
        /// <seealso cref="WindowTitle"/>
        /// <seealso cref="GetWindowTitle()"/>
        private void SetWindowTitle(String value)
        {
            if (!String.IsNullOrWhiteSpace(value))
            {
                if (!SetConsoleTitle(value))
                {
                    // TODO: Error handling.
                }
            }
        }

        /// <summary>
        /// This method gets currently applied Quick Edit Mode.
        /// </summary>
        /// <remarks>
        /// The Quick Edit Mode represents a feature of the console screen buffer that allows to mark 
        /// a portion of available output and copy it into the clipboard.
        /// </remarks>
        /// <returns>
        /// True if the Quick Edit Mode is currently enabled and false if not.
        /// </returns>
        /// <seealso cref="QuickEdit"/>
        /// <seealso cref="SetQuickEditMode(Boolean)"/>
        private Boolean GetQuickEditMode()
        {
            Boolean result = false;

            IntPtr hInput = CreateFile(this.inputName, GENERIC_READ | GENERIC_WRITE, FILE_SHARE_READ, IntPtr.Zero, OPEN_EXISTING, 0, IntPtr.Zero);

            if (hInput != IntPtr.Zero && hInput != INVALID_HANDLE_VALUE)
            {
                if (GetConsoleMode(hInput, out UInt32 mode))
                {
                    result = (mode & (ENABLE_QUICK_EDIT_MODE | ENABLE_EXTENDED_FLAGS)) == (ENABLE_QUICK_EDIT_MODE | ENABLE_EXTENDED_FLAGS);
                }

                CloseHandle(hInput);
            }

            return result;
        }

        /// <summary>
        /// This method sets currently applied Quick Edit Mode.
        /// </summary>
        /// <remarks>
        /// Be aware enabling or disabling the Quick Edit Mode will effect the whole console window.
        /// </remarks>
        /// <param name="value">
        /// True to enable the Quick Edit Mode and false to disable it.
        /// </param>
        /// <seealso cref="QuickEdit"/>
        /// <seealso cref="GetQuickEditMode()"/>
        private void SetQuickEditMode(Boolean value)
        {
            IntPtr hInput = CreateFile(this.inputName, GENERIC_READ | GENERIC_WRITE, FILE_SHARE_READ, IntPtr.Zero, OPEN_EXISTING, 0, IntPtr.Zero);

            if (hInput != IntPtr.Zero && hInput != INVALID_HANDLE_VALUE)
            {
                if (GetConsoleMode(hInput, out UInt32 mode))
                {
                    mode |= ((value ? ENABLE_QUICK_EDIT_MODE : 0) | ENABLE_EXTENDED_FLAGS);

                    if (!SetConsoleMode(hInput, mode))
                    {
                        // TODO: Error handling.
                    }
                }

                CloseHandle(hInput);
            }
        }

        /// <summary>
        /// This method gets currently applied screen buffer size.
        /// </summary>
        /// <returns>
        /// The number of lines and columns of the console screen buffer.
        /// </returns>
        /// <remarks>
        /// Keep in mind, the console screen buffer size represents the number of columns as well 
        /// as the number of lines.
        /// </remarks>
        /// <seealso cref="BufferSize"/>
        /// <seealso cref="SetScreenBufferSize(Dimension)"/>
        private Dimension GetScreenBufferSize()
        {
            Dimension result = new Dimension();

            if (this.hOutput != IntPtr.Zero && this.hOutput != INVALID_HANDLE_VALUE)
            {
                if (GetConsoleScreenBufferInfo(this.hOutput, out CONSOLE_SCREEN_BUFFER_INFO bufferInfo))
                {
                    result.Width = bufferInfo.dwSize.X;
                    result.Lines = bufferInfo.dwSize.Y;
                }
            }

            return result;
        }

        /// <summary>
        /// This method sets currently applied screen buffer size.
        /// </summary>
        /// <remarks>
        /// Be aware, nothing will happen if provided size is empty or if provided size is less 
        /// than current console window size.
        /// </remarks>
        /// <param name="value">
        /// The new screen buffer size to be applied.
        /// </param>
        /// <seealso cref="BufferSize"/>
        /// <seealso cref="GetScreenBufferSize()"/>
        private void SetScreenBufferSize(Dimension value)
        {
            if (value.IsValid && this.hOutput != IntPtr.Zero && this.hOutput != INVALID_HANDLE_VALUE)
            {
                if (GetConsoleScreenBufferInfo(this.hOutput, out CONSOLE_SCREEN_BUFFER_INFO bufferInfo))
                {
                    Dimension window = new Dimension(
                        (bufferInfo.srWindow.Right - bufferInfo.srWindow.Left) + 1,
                        (bufferInfo.srWindow.Bottom - bufferInfo.srWindow.Top) + 1);

                    // NOTE: Do not use any size less than current window size.
                    if (value.Width < window.Width) { value.Width = window.Width; }
                    if (value.Lines < window.Lines) { value.Lines = window.Lines; }

                    COORD coord = new COORD
                    {
                        X = (Int16)value.Width,
                        Y = (Int16)value.Lines
                    };

                    if (!SetConsoleScreenBufferSize(this.hOutput, coord))
                    {
                        // TODO: Error handling.
                    }
                }
            }
        }

        #endregion

        #region Win32 API stuff

        /// <exclude />
        private readonly IntPtr INVALID_HANDLE_VALUE = new IntPtr(-1);

        /// <exclude />
        private const UInt16 INVALID_RESULT_VALUE = unchecked((UInt16)(-1));

        /// <exclude />
        private const Int32 MAX_PATH = 260;

        /// <exclude />
        private const UInt32 GENERIC_READ = 0x80000000;

        /// <exclude />
        private const UInt32 GENERIC_WRITE = 0x40000000;

        /// <exclude />
        private const UInt32 FILE_SHARE_READ = 0x00000001;

        /// <exclude />
        private const UInt32 FILE_SHARE_WRITE = 0x00000002;

        /// <exclude />
        private const UInt32 OPEN_EXISTING = 3;

        /// <exclude />
        private const UInt16 FOREGROUND_BLUE = 0x0001;

        /// <exclude />
        private const UInt16 FOREGROUND_GREEN = 0x0002;

        /// <exclude />
        private const UInt16 FOREGROUND_RED = 0x0004;

        /// <exclude />
        private const UInt16 FOREGROUND_INTENSITY = 0x0008;

        /// <exclude />
        private const UInt16 FOREGROUND_MASK = FOREGROUND_BLUE | FOREGROUND_GREEN | FOREGROUND_RED | FOREGROUND_INTENSITY;

        /// <exclude />
        private const UInt16 BACKGROUND_BLUE = 0x0010;

        /// <exclude />
        private const UInt16 BACKGROUND_GREEN = 0x0020;

        /// <exclude />
        private const UInt16 BACKGROUND_RED = 0x0040;

        /// <exclude />
        private const UInt16 BACKGROUND_INTENSITY = 0x0080;

        /// <exclude />
        private const UInt16 BACKGROUND_MASK = BACKGROUND_BLUE | BACKGROUND_GREEN | BACKGROUND_RED | BACKGROUND_INTENSITY;

        /// <exclude />
        private const UInt32 ENABLE_QUICK_EDIT_MODE = 0x40;

        /// <exclude />
        private const UInt32 ENABLE_EXTENDED_FLAGS = 0x80;

        /// <exclude />
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Winapi)]
        private static extern IntPtr GetConsoleWindow();

        /// <exclude />
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Winapi)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern Boolean AllocConsole();

        /// <exclude />
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Winapi)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern Boolean FreeConsole();

        /// <exclude />
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Winapi)]
        internal static extern UInt32 GetConsoleTitle(StringBuilder title, UInt32 size);

        /// <exclude />
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Winapi)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern Boolean SetConsoleTitle(String title);

        /// <exclude />
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Winapi)]
        private static extern IntPtr CreateFile(String filename, UInt32 access, UInt32 share, IntPtr security, UInt32 creation, UInt32 flags, IntPtr template);

        /// <exclude />
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Winapi)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern Boolean CloseHandle(IntPtr hHandle);

        /// <exclude />
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Winapi)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern Boolean WriteConsole(IntPtr hConsole, String message, UInt32 length, out UInt32 written, IntPtr reserved);

        /// <exclude />
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Winapi)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern Boolean GetConsoleScreenBufferInfo(IntPtr hConsole, out CONSOLE_SCREEN_BUFFER_INFO bufferInfo);

        /// <exclude />
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Winapi)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern Boolean SetConsoleTextAttribute(IntPtr hConsole, UInt16 attributes);

        /// <exclude />
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Winapi)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern Boolean GetConsoleMode(IntPtr hConsole, out UInt32 mode);

        /// <exclude />
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Winapi)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern Boolean SetConsoleMode(IntPtr hConsole, UInt32 mode);

        /// <exclude />
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.Winapi)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern Boolean SetConsoleScreenBufferSize(IntPtr hConsole, COORD size);

        /// <exclude />
        [StructLayout(LayoutKind.Sequential)]
        private struct CONSOLE_SCREEN_BUFFER_INFO
        {
            public COORD dwSize;
            public COORD dwCursorPosition;
            public UInt16 wAttributes;
            public SMALL_RECT srWindow;
            public COORD dwMaximumWindowSize;
        }

        /// <exclude />
        [StructLayout(LayoutKind.Sequential)]
        private struct COORD
        {
            public Int16 X;
            public Int16 Y;
        }

        /// <exclude />
        [StructLayout(LayoutKind.Sequential)]
        private struct SMALL_RECT
        {
            public Int16 Left;
            public Int16 Top;
            public Int16 Right;
            public Int16 Bottom;

        }

        #endregion
    }
}
