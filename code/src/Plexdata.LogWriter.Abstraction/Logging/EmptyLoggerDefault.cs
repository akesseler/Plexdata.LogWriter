/*
 * MIT License
 * 
 * Copyright (c) 2021 plexdata.de
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

namespace Plexdata.LogWriter.Logging
{
    /// <inheritdoc />
    public class EmptyLogger : IEmptyLogger
    {
        #region Construction

        /// <summary>
        /// The default class constructor.
        /// </summary>
        /// <remarks>
        /// This constructor does indeed do nothing.
        /// </remarks>
        public EmptyLogger() : base() { }

        #endregion

        #region Public properties

        /// <inheritdoc />
        /// <remarks>
        /// This property always returns <c>true</c>.
        /// </remarks>
        public virtual Boolean IsDisabled { get { return true; } }

        #endregion

        #region Public methods

        /// <inheritdoc />
        /// <remarks>
        /// This method always returns <c>false</c>.
        /// </remarks>
        public virtual Boolean IsEnabled(LogLevel level) { return false; }

        #endregion

        #region Write methods

        // Can't be moved into base class(es) because of otherwise the context can't be resolved anymore.

        /// <inheritdoc />
        public void Write(LogLevel level, String message) { }

        /// <inheritdoc />
        public void Write(LogLevel level, String message, params (String Label, Object Value)[] details) { }

        /// <inheritdoc />
        public void Write(LogLevel level, Exception exception) { }

        /// <inheritdoc />
        public void Write(LogLevel level, Exception exception, params (String Label, Object Value)[] details) { }

        /// <inheritdoc />
        public void Write(LogLevel level, String message, Exception exception) { }

        /// <inheritdoc />
        public void Write(LogLevel level, String message, Exception exception, params (String Label, Object Value)[] details) { }

        /// <inheritdoc />
        public void Write<TScope>(TScope scope, LogLevel level, String message) { }

        /// <inheritdoc />
        public void Write<TScope>(TScope scope, LogLevel level, String message, params (String Label, Object Value)[] details) { }

        /// <inheritdoc />
        public void Write<TScope>(TScope scope, LogLevel level, Exception exception) { }

        /// <inheritdoc />
        public void Write<TScope>(TScope scope, LogLevel level, Exception exception, params (String Label, Object Value)[] details) { }

        /// <inheritdoc />
        public void Write<TScope>(TScope scope, LogLevel level, String message, Exception exception) { }

        /// <inheritdoc />
        public void Write<TScope>(TScope scope, LogLevel level, String message, Exception exception, params (String Label, Object Value)[] details) { }

        #endregion
    }
}
