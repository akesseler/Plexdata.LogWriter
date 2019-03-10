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
using Plexdata.LogWriter.Definitions.Console;
using Plexdata.LogWriter.Internals.Console.Windows.Native;
using System;
using System.Diagnostics.CodeAnalysis;

namespace Plexdata.LogWriter.Facades.Console.Windows
{
    /// <summary>
    /// This class represents the default implementation of interface 
    /// <see cref="IConsoleLoggerFacade"/> for Windows applications that 
    /// are able to deal with .NET Standard libraries.
    /// </summary>
    /// <remarks>
    /// Major task of this class is the abstraction of all writing operations 
    /// of the console logger of the real writing operation into the console 
    /// window.
    /// </remarks>
    [ExcludeFromCodeCoverage]
    public class ConsoleLoggerFacade : IConsoleLoggerFacade
    {
        #region Private fields

        /// <summary>
        /// This field represents the instance of the native console window.
        /// </summary>
        /// <remarks>
        /// Task of the native console window is to perform all physical 
        /// writing operations.
        /// </remarks>
        private readonly NativeConsole native = null;

        #endregion

        #region Construction

        /// <summary>
        /// The default constructor initializes all its fields and properties.
        /// </summary>
        /// <remarks>
        /// This constructor actually gets its instance of the native logger 
        /// which in turn runs the initialization of the native logger, but 
        /// only if not yet done.
        /// </remarks>
        public ConsoleLoggerFacade()
            : base()
        {
            this.native = NativeConsole.Instance;
        }

        #endregion

        #region Public properties

        /// <inheritdoc />
        public Int32 References
        {
            get
            {
                return this.native.References;
            }
        }

        /// <inheritdoc />
        public Boolean IsAttached
        {
            get
            {
                return this.native.IsAttached;
            }
        }

        /// <inheritdoc />
        public Boolean MustDetach
        {
            get
            {
                return this.native.MustDetach;
            }
        }

        /// <inheritdoc />
        public Boolean UseColors
        {
            get
            {
                return this.native.UseColors;
            }
            set
            {
                this.native.UseColors = value;
            }
        }

        /// <inheritdoc />
        public ConsoleColor Foreground
        {
            get
            {
                return this.native.Foreground;
            }
            set
            {
                this.native.Foreground = value;
            }
        }

        /// <inheritdoc />
        public ConsoleColor Background
        {
            get
            {
                return this.native.Background;
            }
            set
            {
                this.native.Background = value;
            }
        }

        /// <inheritdoc />
        public String WindowTitle
        {
            get
            {
                return this.native.WindowTitle;
            }
            set
            {
                this.native.WindowTitle = value;
            }
        }

        /// <inheritdoc />
        public Boolean QuickEdit
        {
            get
            {
                return this.native.QuickEdit;
            }
            set
            {
                this.native.QuickEdit = value;
            }
        }

        /// <inheritdoc />
        public Dimension BufferSize
        {
            get
            {
                return this.native.BufferSize;
            }
            set
            {
                this.native.BufferSize = value;
            }
        }

        #endregion

        #region Public methods

        /// <inheritdoc />
        public void Attach()
        {
            this.native.Attach();
        }

        /// <inheritdoc />
        public void Detach()
        {
            this.native.Detach();
        }

        /// <inheritdoc />
        public void Write(String message)
        {
            this.native.Write(message);
        }

        /// <inheritdoc />
        public void Flush()
        {
            this.native.Flush();
        }

        #endregion
    }
}
