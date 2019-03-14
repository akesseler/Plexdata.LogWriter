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
using Plexdata.LogWriter.Definitions.Console;
using System;
using System.Text;

namespace Plexdata.LogWriter.Logging.Console
{
    /// <summary>
    /// This abstract class serves as base class of all other console logger classes.
    /// </summary>
    /// <remarks>
    /// Task of this base is to share global functionality between derived classes.
    /// </remarks>
    public abstract class ConsoleLoggerBase : LoggerBase<IConsoleLoggerSettings>, IDisposable
    {
        // TODO: Review and/or complete documentation.

        #region Private fields

        /// <summary>
        /// The instance of console logger facade.
        /// </summary>
        /// <remarks>
        /// The console logger facade allows access to the physical writing 
        /// functionality.
        /// </remarks>
        private readonly IConsoleLoggerFacade facade = null;

        #endregion

        #region Construction

        /// <summary>
        /// The only class constructor.
        /// </summary>
        /// <remarks>
        /// This constructor performs a parameter validation, attaches 
        /// the facade and initializes other properties.
        /// </remarks>
        /// <param name="settings">
        /// The settings to be used.
        /// </param>
        /// <param name="facade">
        /// The facade to be used.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// This exception is thrown either if the <paramref name="settings"/> 
        /// or the <paramref name="facade"/> is <c>null</c>.
        /// </exception>
        protected ConsoleLoggerBase(IConsoleLoggerSettings settings, IConsoleLoggerFacade facade)
            : base(settings)
        {
            this.facade = facade ?? throw new ArgumentNullException(nameof(facade));

            this.IsDisposed = false;

            this.facade.Attach();
            this.facade.WindowTitle = base.Settings.WindowTitle;
            this.facade.QuickEdit = base.Settings.QuickEdit;
            this.facade.BufferSize = base.Settings.BufferSize;
        }

        /// <summary>
        /// The class destructor.
        /// </summary>
        /// <remarks>
        /// The destructor performs an object disposal.
        /// </remarks>
        ~ConsoleLoggerBase()
        {
            this.Dispose(false);
        }

        #endregion

        #region Public properties

        /// <summary>
        /// Determines whether this instance has been disposed.
        /// </summary>
        /// <remarks>
        /// After calling method <see cref="Dispose()"/> the object 
        /// is no longer functional. This property can be queried to 
        /// determine if this instance is disposed already.
        /// </remarks>
        /// <value>
        /// True if the object has been disposed and false otherwise.
        /// </value>
        public Boolean IsDisposed { get; private set; }

        #endregion

        #region Public methods

        /// <summary>
        /// This method performs the object disposal.
        /// </summary>
        /// <remarks>
        /// The method represents the implementation of interface 
        /// <see cref="IDisposable"/>.
        /// </remarks>
        /// <seealso cref="Dispose(Boolean)"/>
        public void Dispose()
        {
            this.Dispose(true);

            GC.SuppressFinalize(this);
        }

        #endregion

        #region Protected methods

        /// <summary>
        /// This method actually does the object disposal.
        /// </summary>
        /// <remarks>
        /// This method detaches from console logger facade 
        /// and marks this object as disposed.
        /// </remarks>
        /// <param name="disposing">
        /// True to dispose all managed resources and false 
        /// to dispose only unmanaged resources.
        /// </param>
        protected virtual void Dispose(Boolean disposing)
        {
            if (!this.IsDisposed)
            {
                if (disposing) { /* Dispose all managed resources */ }

                this.facade.Detach();

                this.IsDisposed = true;
            }
        }

        /// <summary>
        /// This method determines if logging is enabled at all.
        /// </summary>
        /// <remarks>
        /// The method checks if current logging level is equal 
        /// to <see cref="Definitions.LogLevel.Disabled"/>.
        /// </remarks>
        /// <returns>
        /// True if logging is disabled and false if not.
        /// </returns>
        protected Boolean CheckDisabled()
        {
            return base.Settings.LogLevel == LogLevel.Disabled;
        }

        /// <summary>
        /// This method determines if a particular logging message 
        /// can be written.
        /// </summary>
        /// <remarks>
        /// The method determines if provided logging level greater 
        /// than or equal to configured logging level.
        /// </remarks>
        /// <param name="level">
        /// The logging level to be checked against the configured 
        /// logging level.
        /// </param>
        /// <returns>
        /// True if current logging message can be written and false 
        /// if provided logging level is below configured logging level.
        /// </returns>
        protected Boolean CheckEnabled(LogLevel level)
        {
            return (Int32)level >= (Int32)base.Settings.LogLevel;
        }

        /// <summary>
        /// This method resolves the logging context.
        /// </summary>
        /// <remarks>
        /// Resolving the message context is done by method 
        /// <see cref="LoggerBase{TSettings}.ResolveContext{TContext}(ILoggerSettings)"/> 
        /// of the base class.
        /// </remarks>
        /// <typeparam name="TContext">
        /// The type to get the logging context from.
        /// </typeparam>
        /// <returns>
        /// The logging context or an empty string in case of an error.
        /// </returns>
        protected String ResolveContext<TContext>()
        {
            return base.ResolveContext<TContext>(base.Settings);
        }

        /// <summary>
        /// This method resolves the logging scope.
        /// </summary>
        /// <remarks>
        /// Resolving the message scope is done by method 
        /// <see cref="LoggerBase{TSettings}.ResolveScope{TScope}(TScope, ILoggerSettings)"/> 
        /// of the base class.
        /// </remarks>
        /// <typeparam name="TScope">
        /// The type to get the logging scope from.
        /// </typeparam>
        /// <param name="scope">
        /// The type to get the logging scope from.
        /// </param>
        /// <returns>
        /// The logging scope or an empty string in case of an error.
        /// </returns>
        protected String ResolveScope<TScope>(TScope scope)
        {
            return base.ResolveScope<TScope>(scope, base.Settings);
        }

        /// <summary>
        /// This method writes a logging message.
        /// </summary>
        /// <remarks>
        /// <para>
        /// The logging message to write depends of current logger settings.
        /// </para>
        /// <para>
        /// Nothing is gonna happen if logging is disabled, or if provided 
        /// logging level is below configured logging level, or if the complete 
        /// logging message is empty.
        /// </para>
        /// </remarks>
        /// <param name="level">
        /// The logging level of the message to write.
        /// </param>
        /// <param name="context">
        /// The logging context of the message to write.
        /// </param>
        /// <param name="scope">
        /// The logging scope of the message to write.
        /// </param>
        /// <param name="message">
        /// The real logging message of the message to write.
        /// </param>
        /// <param name="exception">
        /// The exception to be included in the logging message.
        /// </param>
        /// <param name="details">
        /// The list of label-value-pairs to be included in the 
        /// logging message.
        /// </param>
        /// <seealso cref="CheckDisabled()"/>
        /// <seealso cref="CheckEnabled(LogLevel)"/>
        /// <seealso cref="CreateOutput(LogLevel, String, String, String, Exception, ValueTuple{String, Object}[])"/>
        /// <seealso cref="SetupConsoleColors(LogLevel)"/>
        protected void Write(LogLevel level, String context, String scope, String message, Exception exception, params (String Label, Object Value)[] details)
        {
            if (this.IsDisposed) { return; }

            if (this.CheckDisabled()) { return; }

            if (!this.CheckEnabled(level)) { return; }

            String output = this.CreateOutput(level, context, scope, message, exception, details);

            if (String.IsNullOrWhiteSpace(output)) { return; }

            Boolean oldUseColors = this.facade.UseColors;

            if (base.Settings.UseColors)
            {
                this.facade.UseColors = true;
                this.SetupConsoleColors(level);
            }

            this.facade.Write(output);

            this.facade.Flush();

            this.facade.UseColors = oldUseColors;
        }

        #endregion

        #region Private methods

        /// <summary>
        /// This method creates to message text to be written.
        /// </summary>
        /// <remarks>
        /// <para>
        /// The message text to be written is generated according to 
        /// current settings and the result may vary during runtime.
        /// </para>
        /// <para>
        /// The time stamp of the logging message is set in this method.
        /// </para>
        /// </remarks>
        /// <param name="level">
        /// The logging level of the message to write.
        /// </param>
        /// <param name="context">
        /// The logging context of the message to write.
        /// </param>
        /// <param name="scope">
        /// The logging scope of the message to write.
        /// </param>
        /// <param name="message">
        /// The real logging message of the message to write.
        /// </param>
        /// <param name="exception">
        /// The exception to be included in the logging message.
        /// </param>
        /// <param name="details">
        /// The list of label-value-pairs to be included in the 
        /// logging message.
        /// </param>
        /// <returns>
        /// The formatted logging message to be written or an empty string.
        /// </returns>
        private String CreateOutput(LogLevel level, String context, String scope, String message, Exception exception, params (String Label, Object Value)[] details)
        {
            ILogEvent output = base.LoggerFactory.CreateLogEvent(level, DateTime.Now, context, scope, message, exception, details);

            if (output.IsValid)
            {
                StringBuilder builder = new StringBuilder(512);

                // This might not be optimizable because of the logging type for example may change during runtime.
                base.LoggerFactory.CreateLogEventFormatter(base.Settings).Format(builder, output);

                return builder.ToString();
            }

            return String.Empty;
        }

        /// <summary>
        /// This method changes current message coloring.
        /// </summary>
        /// <remarks>
        /// The colors to be used are taken from current 
        /// settings and are adjusted in the facade.
        /// </remarks>
        /// <param name="level">
        /// The logging level to adjust the coloring for.
        /// </param>
        private void SetupConsoleColors(LogLevel level)
        {
            if (base.Settings.Coloring.TryGetValue(level, out Coloring coloring))
            {
                this.facade.Foreground = coloring.Foreground;
                this.facade.Background = coloring.Background;
            }
            else
            {
                this.facade.Foreground = Coloring.DefaultForeground;
                this.facade.Background = Coloring.DefaultBackground;
            }
        }

        #endregion
    }
}
