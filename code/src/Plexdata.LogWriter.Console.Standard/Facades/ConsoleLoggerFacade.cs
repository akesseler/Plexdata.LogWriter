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
using System;
using System.Diagnostics.CodeAnalysis;

namespace Plexdata.LogWriter.Facades.Standard
{
    /// <summary>
    /// This class represents the default implementation of interface 
    /// <see cref="IConsoleLoggerFacade"/> for all applications that 
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
        /// This field holds the instance of the internal synchronization object.
        /// </summary>
        /// <remarks>
        /// Writing data into a console window should be thread-safely locked. 
        /// For this purpose this object is used.
        /// </remarks>
        /// <seealso cref="Attach"/>
        /// <seealso cref="Detach"/>
        /// <seealso cref="Write(String)"/>
        private readonly Object synchronizer = new Object();

        #endregion

        #region Construction

        /// <summary>
        /// The default constructor initializes all its fields and properties.
        /// </summary>
        /// <remarks>
        /// This constructor does actually nothing.
        /// </remarks>
        public ConsoleLoggerFacade()
            : base()
        {
        }

        #endregion

        #region Public properties

        /// <inheritdoc />
        /// <remarks>
        /// This property always returns zero.
        /// </remarks>
        public Int32 References
        {
            get
            {
                return 0;
            }
        }

        /// <inheritdoc />
        /// <remarks>
        /// This property always returns true.
        /// </remarks>
        public Boolean IsAttached
        {
            get
            {
                return true;
            }
        }

        /// <inheritdoc />
        /// <remarks>
        /// This property always returns false.
        /// </remarks>
        public Boolean MustDetach
        {
            get
            {
                return false;
            }
        }

        /// <inheritdoc />
        public Boolean UseColors
        {
            get;
            set;
        }

        /// <inheritdoc />
        public ConsoleColor Foreground
        {
            get;
            set;
        }

        /// <inheritdoc />
        public ConsoleColor Background
        {
            get;
            set;
        }

        /// <inheritdoc />
        public String WindowTitle
        {
            get
            {
                return System.Console.Title;
            }
            set
            {
                if (!String.IsNullOrWhiteSpace(value))
                {
                    System.Console.Title = value;
                }
            }
        }

        /// <inheritdoc />
        /// <remarks>
        /// This property can be changed but it may not 
        /// have any effect in this context.
        /// </remarks>
        public Boolean QuickEdit
        {
            get;
            set;
        }

        /// <inheritdoc />
        public Dimension BufferSize
        {
            get
            {
                return new Dimension(System.Console.BufferWidth, System.Console.BufferHeight);
            }
            set
            {
                if (value != null && value.IsValid)
                {
                    System.Console.BufferWidth = value.Width;
                    System.Console.BufferHeight = value.Lines;
                }
            }
        }

        #endregion

        #region Public methods

        /// <inheritdoc />
        public void Attach()
        {
            // Nothing to do... Except counting of references, but why.
        }

        /// <inheritdoc />
        public void Detach()
        {
            // Nothing to do... Except counting of references, but why.
        }

        /// <inheritdoc />
        public void Write(String message)
        {
            if (!this.IsAttached) { return; }

            if (String.IsNullOrWhiteSpace(message)) { return; }

            lock (this.synchronizer)
            {
                if (message.EndsWith(Environment.NewLine))
                {
                    message = message.Remove(message.Length - Environment.NewLine.Length, Environment.NewLine.Length);
                }

                Coloring coloring = null;

                if (this.UseColors)
                {
                    coloring = new Coloring(System.Console.ForegroundColor, System.Console.BackgroundColor);

                    System.Console.ForegroundColor = this.Foreground;
                    System.Console.BackgroundColor = this.Background;
                }

                System.Console.Write(message);

                if (coloring != null)
                {
                    System.Console.ForegroundColor = coloring.Foreground;
                    System.Console.BackgroundColor = coloring.Background;
                }

                // Writing the "End of Line" independently fixes a bug in conjunction 
                // with coloring. This coloring bug occurs when the written message 
                // reaches the end of the console screen buffer.
                System.Console.Write(Environment.NewLine);
            }
        }

        /// <inheritdoc />
        public void Flush()
        {
            lock (this.synchronizer)
            {
                System.Console.Out.Flush();
            }
        }

        #endregion
    }
}
