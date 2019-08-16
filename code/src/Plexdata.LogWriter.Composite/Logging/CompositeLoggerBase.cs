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

namespace Plexdata.LogWriter.Logging
{
    /// <summary>
    /// This abstract class serves as base class of all other composite logger classes.
    /// </summary>
    /// <remarks>
    /// Task of this base class is to share global functionality between derived classes.
    /// </remarks>
    public abstract class CompositeLoggerBase : LoggerBase<ICompositeLoggerSettings>
    {
        #region Construction

        /// <summary>
        /// The standard class constructor.
        /// </summary>
        /// <remarks>
        /// This constructor performs the initialization of fields and properties.
        /// </remarks>
        /// <param name="settings">
        /// The settings to be used.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// This exception is thrown if the parameter <paramref name="settings"/> 
        /// is <c>null</c>.
        /// </exception>
        protected CompositeLoggerBase(ICompositeLoggerSettings settings)
            : base(settings)
        {
            this.IsDisposed = false;
        }

        /// <summary>
        /// The class destructor.
        /// </summary>
        /// <remarks>
        /// The destructor performs an object disposal.
        /// </remarks>
        ~CompositeLoggerBase()
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
        /// This method detaches from the event handler and 
        /// marks this object as disposed.
        /// </remarks>
        /// <param name="disposing">
        /// True to dispose all managed resources and false 
        /// to dispose only unmanaged resources.
        /// </param>
        protected virtual void Dispose(Boolean disposing)
        {
            if (!this.IsDisposed)
            {
                if (disposing) { /* Dispose all managed resources. */ }

                this.IsDisposed = true;
            }
        }

        /// <summary>
        /// Checks if message writing is permitted.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This method checks if message writing is permitted under 
        /// current conditions.
        /// </para>
        /// <para>
        /// Current conditions means in this context:
        /// <list type="number">  
        /// <item><description>
        /// The logger has not been disposed yet.
        /// </description></item>  
        /// <item><description>
        /// Logging is not disabled by current logger settings.
        /// </description></item>  
        /// <item><description>
        /// Logging is actually enabled for provided logging level.
        /// </description></item>  
        /// </list>          
        /// </para>
        /// </remarks>
        /// <param name="level">
        /// The logging level to be checked.
        /// </param>
        /// <returns>
        /// False is returned if the logger has been disposed or if logging 
        /// is currently disabled or if provided logging level is less than 
        /// current logging level. Otherwise true is returned.
        /// </returns>
        /// <seealso cref="ILoggerSettings"/>
        /// <seealso cref="ICompositeLoggerSettings"/>
        protected Boolean IsPermitted(LogLevel level)
        {
            if (this.IsDisposed) { return false; }

            if (base.IsDisabled) { return false; }

            if (!base.IsEnabled(level)) { return false; }

            return true;
        }

        #endregion
    }
}
