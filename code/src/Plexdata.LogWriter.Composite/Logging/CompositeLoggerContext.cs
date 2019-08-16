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
using Plexdata.LogWriter.Settings;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Plexdata.LogWriter.Logging
{
    /// <summary>
    /// This class represents the context related default composite
    /// logger implementation for platform independent applications.
    /// </summary>
    /// <remarks>
    /// This class redirects incoming logging message to all assigned 
    /// logger instances.
    /// </remarks>
    /// <typeparam name="TContext">
    /// The type to get the logging context from.
    /// </typeparam>
    public class CompositeLogger<TContext> : CompositeLoggerBase, ICompositeLogger<TContext>
    {
        #region Private fields

        /// <summary>
        /// The internal list of assigned context loggers.
        /// </summary>
        /// <remarks>
        /// A list that contains all context logger instances 
        /// assigned to this composite logger instance.
        /// </remarks>
        private readonly IList<ILogger<TContext>> loggers = null;

        #endregion

        #region Construction

        /// <summary>
        /// The default constructor.
        /// </summary>
        /// <remarks>
        /// This constructor just calls its extended constructor by using a blank 
        /// instance of <see cref="ICompositeLoggerSettings"/>.  Additionally, the 
        /// default logging level is set to <see cref="LogLevel.Trace"/>.
        /// </remarks>
        /// <seealso cref="CompositeLogger{TContext}.CompositeLogger(ICompositeLoggerSettings)"/>
        public CompositeLogger()
            : this(new CompositeLoggerSettings())
        {
            base.Settings.LogLevel = LogLevel.Trace;
        }

        /// <summary>
        /// The extended constructor.
        /// </summary>
        /// <remarks>
        /// This constructor calls its base class constructor and hands over the 
        /// provided instances of of <paramref name="settings"/>. Additionally, 
        /// this constructor initializes its internal list of assigned loggers.
        /// </remarks>
        /// <param name="settings">
        /// The settings to be used.
        /// </param>
        public CompositeLogger(ICompositeLoggerSettings settings)
            : base(settings)
        {
            this.loggers = new List<ILogger<TContext>>();
        }

        /// <summary>
        /// The class destructor.
        /// </summary>
        /// <remarks>
        /// The destructor performs an object disposal.
        /// </remarks>
        ~CompositeLogger()
        {
            this.Dispose(false);
        }

        #endregion

        #region Public methods

        /// <inheritdoc />
        public void AddLogger(ILogger<TContext> logger)
        {
            if (this.IsDisposed) { return; }

            if (logger != null && !this.loggers.Contains(logger))
            {
                this.loggers.Add(logger);
            }
        }

        #endregion

        #region Write methods

        // NOTE: Can't be moved into base class(es) because of otherwise the context can't be resolved anymore.

        /// <inheritdoc />
        public void Write(LogLevel level, String message)
        {
            if (!base.IsPermitted(level)) { return; }

            List<Task> tasks = new List<Task>();

            foreach (ILogger<TContext> logger in this.loggers)
            {
                tasks.Add(Task.Run(() => { logger.Write(level, message); }));
            }

            Task.WaitAll(tasks.ToArray());
        }

        /// <inheritdoc />
        public void Write(LogLevel level, String message, params (String Label, Object Value)[] details)
        {
            if (!base.IsPermitted(level)) { return; }

            List<Task> tasks = new List<Task>();

            foreach (ILogger<TContext> logger in this.loggers)
            {
                tasks.Add(Task.Run(() => { logger.Write(level, message, details); }));
            }

            Task.WaitAll(tasks.ToArray());
        }

        /// <inheritdoc />
        public void Write(LogLevel level, Exception exception)
        {
            if (!base.IsPermitted(level)) { return; }

            List<Task> tasks = new List<Task>();

            foreach (ILogger<TContext> logger in this.loggers)
            {
                tasks.Add(Task.Run(() => { logger.Write(level, exception); }));
            }

            Task.WaitAll(tasks.ToArray());
        }

        /// <inheritdoc />
        public void Write(LogLevel level, Exception exception, params (String Label, Object Value)[] details)
        {
            if (!base.IsPermitted(level)) { return; }

            List<Task> tasks = new List<Task>();

            foreach (ILogger<TContext> logger in this.loggers)
            {
                tasks.Add(Task.Run(() => { logger.Write(level, exception, details); }));
            }

            Task.WaitAll(tasks.ToArray());
        }

        /// <inheritdoc />
        public void Write(LogLevel level, String message, Exception exception)
        {
            if (!base.IsPermitted(level)) { return; }

            List<Task> tasks = new List<Task>();

            foreach (ILogger<TContext> logger in this.loggers)
            {
                tasks.Add(Task.Run(() => { logger.Write(level, message, exception); }));
            }

            Task.WaitAll(tasks.ToArray());
        }

        /// <inheritdoc />
        public void Write(LogLevel level, String message, Exception exception, params (String Label, Object Value)[] details)
        {
            if (!base.IsPermitted(level)) { return; }

            List<Task> tasks = new List<Task>();

            foreach (ILogger<TContext> logger in this.loggers)
            {
                tasks.Add(Task.Run(() => { logger.Write(level, message, exception, details); }));
            }

            Task.WaitAll(tasks.ToArray());
        }

        /// <inheritdoc />
        public void Write<TScope>(TScope scope, LogLevel level, String message)
        {
            if (!base.IsPermitted(level)) { return; }

            List<Task> tasks = new List<Task>();

            foreach (ILogger<TContext> logger in this.loggers)
            {
                tasks.Add(Task.Run(() => { logger.Write<TScope>(scope, level, message); }));
            }

            Task.WaitAll(tasks.ToArray());
        }

        /// <inheritdoc />
        public void Write<TScope>(TScope scope, LogLevel level, String message, params (String Label, Object Value)[] details)
        {
            if (!base.IsPermitted(level)) { return; }

            List<Task> tasks = new List<Task>();

            foreach (ILogger<TContext> logger in this.loggers)
            {
                tasks.Add(Task.Run(() => { logger.Write<TScope>(scope, level, message, details); }));
            }

            Task.WaitAll(tasks.ToArray());
        }

        /// <inheritdoc />
        public void Write<TScope>(TScope scope, LogLevel level, Exception exception)
        {
            if (!base.IsPermitted(level)) { return; }

            List<Task> tasks = new List<Task>();

            foreach (ILogger<TContext> logger in this.loggers)
            {
                tasks.Add(Task.Run(() => { logger.Write<TScope>(scope, level, exception); }));
            }

            Task.WaitAll(tasks.ToArray());
        }

        /// <inheritdoc />
        public void Write<TScope>(TScope scope, LogLevel level, Exception exception, params (String Label, Object Value)[] details)
        {
            if (!base.IsPermitted(level)) { return; }

            List<Task> tasks = new List<Task>();

            foreach (ILogger<TContext> logger in this.loggers)
            {
                tasks.Add(Task.Run(() => { logger.Write<TScope>(scope, level, exception, details); }));
            }

            Task.WaitAll(tasks.ToArray());
        }

        /// <inheritdoc />
        public void Write<TScope>(TScope scope, LogLevel level, String message, Exception exception)
        {
            if (!base.IsPermitted(level)) { return; }

            List<Task> tasks = new List<Task>();

            foreach (ILogger<TContext> logger in this.loggers)
            {
                tasks.Add(Task.Run(() => { logger.Write<TScope>(scope, level, message, exception); }));
            }

            Task.WaitAll(tasks.ToArray());
        }

        /// <inheritdoc />
        public void Write<TScope>(TScope scope, LogLevel level, String message, Exception exception, params (String Label, Object Value)[] details)
        {
            if (!base.IsPermitted(level)) { return; }

            List<Task> tasks = new List<Task>();

            foreach (ILogger<TContext> logger in this.loggers)
            {
                tasks.Add(Task.Run(() => { logger.Write<TScope>(scope, level, message, exception, details); }));
            }

            Task.WaitAll(tasks.ToArray());
        }

        #endregion

        #region Protected methods

        /// <inheritdoc />
        protected override void Write(LogLevel level, String context, String scope, String message, Exception exception, params (String Label, Object Value)[] details)
        {
            throw new NotSupportedException();
        }

        /// <inheritdoc />
        protected override void Dispose(Boolean disposing)
        {
            if (!this.IsDisposed)
            {
                if (disposing)
                {
                    foreach (ILogger<TContext> logger in this.loggers)
                    {
                        if (logger is IDisposable)
                        {
                            (logger as IDisposable).Dispose();
                        }
                    }
                }

                this.loggers.Clear();
            }

            base.Dispose(disposing);
        }

        #endregion
    }
}

