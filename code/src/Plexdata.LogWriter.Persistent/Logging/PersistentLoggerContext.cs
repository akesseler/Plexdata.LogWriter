﻿/*
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

using Plexdata.LogWriter.Abstraction;
using Plexdata.LogWriter.Definitions;
using Plexdata.LogWriter.Facades;
using System;

namespace Plexdata.LogWriter.Logging
{
    /// <summary>
    /// This class represents the context related default persistent
    /// logger implementation for platform independent applications.
    /// </summary>
    /// <remarks>
    /// The class writes context related logging messages using the 
    /// context type to determine the context name.
    /// </remarks>
    /// <typeparam name="TContext">
    /// The type to get the logging context from.
    /// </typeparam>
    public class PersistentLogger<TContext> : PersistentLoggerBase, IPersistentLogger<TContext>
    {
        #region Construction

        /// <summary>
        /// The standard class constructor.
        /// </summary>
        /// <remarks>
        /// This constructor just calls the extended constructor providing 
        /// the default persistent logger facade as parameter.
        /// </remarks>
        /// <param name="settings">
        /// The settings to be used.
        /// </param>
        /// <seealso cref="PersistentLogger{TContext}.PersistentLogger(IPersistentLoggerSettings, IPersistentLoggerFacade)"/>
        /// <seealso cref="Plexdata.LogWriter.Facades.PersistentLoggerFacade"/>
        public PersistentLogger(IPersistentLoggerSettings settings)
            : this(settings, new PersistentLoggerFacade())
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
        public PersistentLogger(IPersistentLoggerSettings settings, IPersistentLoggerFacade facade)
            : base(settings, facade)
        {
        }

        #endregion

        #region Write methods

        // Can't be moved into base class(es) because of otherwise the context can't be resolved anymore.

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
    }
}
