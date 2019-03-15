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

namespace Plexdata.LogWriter.Logging.Console
{
    /// <summary>
    /// This abstract class serves as base class of all other console logger classes.
    /// </summary>
    /// <remarks>
    /// Task of this base is to share global functionality between derived classes.
    /// </remarks>
    public abstract class ConsoleLoggerBase : LoggerBase<IConsoleLoggerSettings>
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
        /// After calling method <see cref="IDisposable.Dispose()"/> the 
        /// object is no longer functional. This property can be queried 
        /// to determine if this instance is disposed already.
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

        /// <inheritdoc />
        protected override void Write(LogLevel level, String context, String scope, String message, Exception exception, params (String Label, Object Value)[] details)
        {
            if (this.IsDisposed) { return; }

            if (base.IsDisabled) { return; }

            if (!base.IsEnabled(level)) { return; }

            String output = base.CreateOutput(level, context, scope, message, exception, details);

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
