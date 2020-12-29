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
using Plexdata.LogWriter.Definitions;
using Plexdata.LogWriter.Internals.Native.Windows;
using System;
using System.Diagnostics.CodeAnalysis;

namespace Plexdata.LogWriter.Facades.Windows
{
    /// <summary>
    /// This class represents the default implementation of interface 
    /// <see cref="IConsoleLoggerFacade"/> for Windows applications based on .NET Framework that 
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
        private readonly SystemConsoleWrapper instance = null;

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
            this.instance = SystemConsoleWrapper.Instance;
        }

        #endregion

        #region Public properties

        /// <inheritdoc />
        public Int32 References
        {
            get
            {
                return this.instance.References;
            }
        }

        /// <inheritdoc />
        public Boolean IsAttached
        {
            get
            {
                return this.instance.IsAttached;
            }
        }

        /// <inheritdoc />
        public Boolean MustDetach
        {
            get
            {
                return this.instance.MustDetach;
            }
        }

        /// <inheritdoc />
        public Boolean UseColors
        {
            get
            {
                return this.instance.UseColors;
            }
            set
            {
                this.instance.UseColors = value;
            }
        }

        /// <inheritdoc />
        public ConsoleColor Foreground
        {
            get
            {
                return this.instance.Foreground;
            }
            set
            {
                this.instance.Foreground = value;
            }
        }

        /// <inheritdoc />
        public ConsoleColor Background
        {
            get
            {
                return this.instance.Background;
            }
            set
            {
                this.instance.Background = value;
            }
        }

        /// <inheritdoc />
        public String WindowTitle
        {
            get
            {
                return this.instance.WindowTitle;
            }
            set
            {
                this.instance.WindowTitle = value;
            }
        }

        /// <inheritdoc />
        public Boolean QuickEdit
        {
            get
            {
                return this.instance.QuickEdit;
            }
            set
            {
                this.instance.QuickEdit = value;
            }
        }

        /// <inheritdoc />
        public Dimension BufferSize
        {
            get
            {
                return this.instance.BufferSize;
            }
            set
            {
                this.instance.BufferSize = value;
            }
        }

        #endregion

        #region Public methods

        /// <inheritdoc />
        public void Attach()
        {
            this.instance.Attach();
        }

        /// <inheritdoc />
        public void Detach()
        {
            this.instance.Detach();
        }

        /// <inheritdoc />
        public void Write(String message)
        {
            this.instance.Write(message);
        }

        /// <inheritdoc />
        public void Flush()
        {
            this.instance.Flush();
        }

        #endregion
    }
}
