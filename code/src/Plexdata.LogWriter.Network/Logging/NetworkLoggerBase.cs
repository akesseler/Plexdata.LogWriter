﻿/*
 * MIT License
 * 
 * Copyright (c) 2023 plexdata.de
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
using Plexdata.LogWriter.Internals.Extensions;
using System;
using System.ComponentModel;

namespace Plexdata.LogWriter.Logging
{
    /// <summary>
    /// This abstract class serves as base class of all other network logger classes.
    /// </summary>
    /// <remarks>
    /// Task of this base class is to share global functionality between derived classes.
    /// </remarks>
    public abstract class NetworkLoggerBase : LoggerBase<INetworkLoggerSettings>
    {
        #region Private fields

        /// <summary>
        /// The instance of network logger facade.
        /// </summary>
        /// <remarks>
        /// The network logger facade allows access to the physical 
        /// writing functionality.
        /// </remarks>
        private readonly INetworkLoggerFacade facade = null;

        #endregion

        #region Construction

        /// <summary>
        /// The standard class constructor.
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
        protected NetworkLoggerBase(INetworkLoggerSettings settings, INetworkLoggerFacade facade)
            : base(settings)
        {
            this.facade = facade ?? throw new ArgumentNullException(nameof(facade));

            base.Settings.PropertyChanged += this.OnSettingsPropertyChanged;

            this.facade.ApplySettings(base.Settings);

            this.IsDisposed = false;
        }

        /// <summary>
        /// The class destructor.
        /// </summary>
        /// <remarks>
        /// The destructor performs an object disposal.
        /// </remarks>
        ~NetworkLoggerBase()
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
        /// Begins a logical operation scope.
        /// </summary>
        /// <remarks>
        /// This method begins a logical operation scope by calling method 
        /// <see cref="LoggerBase{TSettings}.CreateScope{TScope}(TScope)"/> 
        /// of its base class.
        /// </remarks>
        /// <typeparam name="TScope">
        /// The type to begin scope for.
        /// </typeparam>
        /// <param name="scope">
        /// The identifier for the scope.
        /// </param>
        /// <returns>
        /// A disposable object that ends the logical operation scope on dispose.
        /// </returns>
        /// <seealso cref="ILogger.BeginScope{TScope}(TScope)"/>
        /// <seealso cref="LoggerBase{TSettings}.CreateScope{TScope}(TScope)"/>
        public IDisposable BeginScope<TScope>(TScope scope)
        {
            return base.CreateScope(scope);
        }

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
        /// This method detaches from the event handler and 
        /// marks this object as disposed.
        /// </remarks>
        /// <param name="disposing">
        /// True to dispose all managed resources and false 
        /// to dispose only unmanaged resources.
        /// </param>
        protected virtual void Dispose(Boolean disposing)
        {
            if (this.IsDisposed) { return; }

            if (disposing)
            {
                base.Settings.PropertyChanged -= this.OnSettingsPropertyChanged;

                this.facade.SafeDispose();
            }

            this.IsDisposed = true;
        }

        /// <inheritdoc />
        protected override void Write(LogLevel level, String context, String scope, String message, Exception exception, params (String Label, Object Value)[] details)
        {
            if (this.IsDisposed) { return; }

            if (base.IsDisabled) { return; }

            if (!base.IsEnabled(level)) { return; }

            String output = base.CreateOutput(level, context, scope, message, exception, details);

            if (String.IsNullOrWhiteSpace(output)) { return; }

            this.facade.Write(output);
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Event handler to observe settings changes.
        /// </summary>
        /// <remarks>
        /// The event handler to observe settings changes.
        /// All settings are observed actually.
        /// </remarks>
        /// <param name="sender">
        /// The sender of the event.
        /// </param>
        /// <param name="args">
        /// The event arguments to be used.
        /// </param>
        private void OnSettingsPropertyChanged(Object sender, PropertyChangedEventArgs args)
        {
            this.facade.ApplySettings(base.Settings);
        }

        #endregion
    }
}
