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
using Plexdata.LogWriter.Facades.Console.Windows;
using System;

namespace Plexdata.LogWriter.Logging.Console.Windows
{
    /// <summary>
    /// This class represents the context related console 
    /// logger implementation for .NET Standard.
    /// </summary>
    /// <remarks>
    /// The class writes context related logging messages 
    /// using the context type to determine the context name.
    /// </remarks>
    /// <typeparam name="TContext">
    /// The type to get the logging context from.
    /// </typeparam>
    public class ConsoleLogger<TContext> : ConsoleLoggerBase, IConsoleLogger<TContext>
    {
        #region Construction

        /// <summary>
        /// The standard class constructor.
        /// </summary>
        /// <remarks>
        /// This constructor just calls the extended constructor providing 
        /// the default console logger facade as parameter.
        /// </remarks>
        /// <param name="settings">
        /// The settings to be used.
        /// </param>
        /// <seealso cref="ConsoleLogger{TContext}.ConsoleLogger(IConsoleLoggerSettings, IConsoleLoggerFacade)"/>
        /// <seealso cref="Plexdata.LogWriter.Facades.Console.Windows.ConsoleLoggerFacade"/>
        public ConsoleLogger(IConsoleLoggerSettings settings)
            : this(settings, new ConsoleLoggerFacade())
        {
        }

        /// <summary>
        /// The extended class constructor.
        /// </summary>
        /// <remarks>
        /// This constructor just calls its base class constructor 
        /// and hands over the instances of all parameters.
        /// </remarks>
        /// <param name="settings">
        /// The settings to be used.
        /// </param>
        /// <param name="facade">
        /// The facade to be used.
        /// </param>
        public ConsoleLogger(IConsoleLoggerSettings settings, IConsoleLoggerFacade facade)
            : base(settings, facade)
        {
        }

        /// <inheritdoc />
        ~ConsoleLogger()
        {
            this.Dispose(false);
        }

        #endregion

        #region Public properties

        /// <inheritdoc />
        public virtual Boolean IsDisabled
        {
            get
            {
                return base.CheckDisabled();
            }
        }

        #endregion

        #region Public methods

        /// <inheritdoc />
        public virtual Boolean IsEnabled(LogLevel level)
        {
            return base.CheckEnabled(level);
        }

        #endregion

        #region Write methods

        /// <inheritdoc />
        public void Write(LogLevel level, String message)
        {
            base.Write(level, base.ResolveContext<TContext>(), null, message, null, null);
        }

        /// <inheritdoc />
        public void Write(LogLevel level, String message, params (String Label, Object Value)[] details)
        {
            base.Write(level, base.ResolveContext<TContext>(), null, message, null, details);
        }

        /// <inheritdoc />
        public void Write(LogLevel level, Exception exception)
        {
            base.Write(level, base.ResolveContext<TContext>(), null, null, exception, null);
        }

        /// <inheritdoc />
        public void Write(LogLevel level, Exception exception, params (String Label, Object Value)[] details)
        {
            base.Write(level, base.ResolveContext<TContext>(), null, null, exception, details);
        }

        /// <inheritdoc />
        public void Write(LogLevel level, String message, Exception exception)
        {
            base.Write(level, base.ResolveContext<TContext>(), null, message, exception, null);
        }

        /// <inheritdoc />
        public void Write(LogLevel level, String message, Exception exception, params (String Label, Object Value)[] details)
        {
            base.Write(level, base.ResolveContext<TContext>(), null, message, exception, details);
        }

        /// <inheritdoc />
        public void Write<TScope>(TScope scope, LogLevel level, String message)
        {
            base.Write(level, base.ResolveContext<TContext>(), base.ResolveScope<TScope>(scope), message, null, null);
        }

        /// <inheritdoc />
        public void Write<TScope>(TScope scope, LogLevel level, String message, params (String Label, Object Value)[] details)
        {
            base.Write(level, base.ResolveContext<TContext>(), base.ResolveScope<TScope>(scope), message, null, details);
        }

        /// <inheritdoc />
        public void Write<TScope>(TScope scope, LogLevel level, Exception exception)
        {
            base.Write(level, base.ResolveContext<TContext>(), base.ResolveScope<TScope>(scope), null, exception, null);
        }

        /// <inheritdoc />
        public void Write<TScope>(TScope scope, LogLevel level, Exception exception, params (String Label, Object Value)[] details)
        {
            base.Write(level, base.ResolveContext<TContext>(), base.ResolveScope<TScope>(scope), null, exception, details);
        }

        /// <inheritdoc />
        public void Write<TScope>(TScope scope, LogLevel level, String message, Exception exception)
        {
            base.Write(level, base.ResolveContext<TContext>(), base.ResolveScope<TScope>(scope), message, exception, null);
        }

        /// <inheritdoc />
        public void Write<TScope>(TScope scope, LogLevel level, String message, Exception exception, params (String Label, Object Value)[] details)
        {
            base.Write(level, base.ResolveContext<TContext>(), base.ResolveScope<TScope>(scope), message, exception, details);
        }

        #endregion

        #region Protected methods

        /// <inheritdoc />
        protected override void Dispose(Boolean disposing)
        {
            base.Dispose(disposing);
        }

        #endregion
    }
}
