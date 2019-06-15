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
using Plexdata.LogWriter.Internals.Extensions;
using Plexdata.LogWriter.Queuing;
using System;

namespace Plexdata.LogWriter.Logging
{
    /// <summary>
    /// This abstract class serves as base class of all other persistent logger classes.
    /// </summary>
    /// <remarks>
    /// Task of this base class is to share global functionality between derived classes.
    /// </remarks>
    public abstract class PersistentLoggerBase : LoggerBase<IPersistentLoggerSettings>
    {
        #region Private fields

        /// <summary>
        /// The instance of persistent logger facade.
        /// </summary>
        /// <remarks>
        /// The persistent logger facade allows access to the physical 
        /// writing functionality.
        /// </remarks>
        private readonly IPersistentLoggerFacade facade = null;

        /// <summary>
        /// The instance of the used observable queue.
        /// </summary>
        /// <remarks>
        /// The observable queue handles all message queuing requests.
        /// </remarks>
        private readonly IObservableQueue<String> scheduler = null;

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
        /// <seealso cref="PersistentLoggerBase(IPersistentLoggerSettings, IPersistentLoggerFacade, IObservableQueue{String})"/>
        protected PersistentLoggerBase(IPersistentLoggerSettings settings, IPersistentLoggerFacade facade)
            : this(settings, facade, new ObservableQueue<String>())
        {
        }

        /// <summary>
        /// The extended class constructor.
        /// </summary>
        /// <remarks>
        /// This constructor performs a parameter validation, attaches the 
        /// facade as well as the scheduler and initializes other properties.
        /// </remarks>
        /// <param name="settings">
        /// The settings to be used.
        /// </param>
        /// <param name="facade">
        /// The facade to be used.
        /// </param>
        /// <param name="scheduler">
        /// The scheduler to be used.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// This exception is thrown either if the <paramref name="settings"/> or the 
        /// <paramref name="facade"/> or the <paramref name="scheduler"/> is <c>null</c>.
        /// </exception>
        /// <seealso cref="PersistentLoggerBase(IPersistentLoggerSettings, IPersistentLoggerFacade)"/>
        protected PersistentLoggerBase(IPersistentLoggerSettings settings, IPersistentLoggerFacade facade, IObservableQueue<String> scheduler)
            : base(settings)
        {
            this.facade = facade ?? throw new ArgumentNullException(nameof(facade));
            this.scheduler = scheduler ?? throw new ArgumentNullException(nameof(scheduler));

            this.IsDisposed = false;

            this.scheduler.Enqueued += this.OnMessageEnqueued;
        }

        /// <summary>
        /// The class destructor.
        /// </summary>
        /// <remarks>
        /// The destructor performs an object disposal.
        /// </remarks>
        ~PersistentLoggerBase()
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
                if (disposing)
                {
                    // Dispose all managed resources.
                    this.scheduler.Enqueued -= this.OnMessageEnqueued;
                    this.scheduler.Clear();
                }

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

            if (base.Settings.IsQueuing)
            {
                this.scheduler.Enqueue(output);
            }
            else
            {
                this.WriteInternal(output);
            }
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Event handler for enqueued messages.
        /// </summary>
        /// <remarks>
        /// This method represents the event handler for all enqueued messages.
        /// </remarks>
        /// <param name="sender">
        /// The sender of the enqueue event.
        /// </param>
        /// <param name="args">
        /// The enqueue event arguments.
        /// </param>
        /// <seealso cref="WriteInternal(String[])"/>
        private void OnMessageEnqueued(Object sender, EventArgs args)
        {
            String[] messages = this.scheduler.DequeueAll();

            if (messages != null && messages.Length > 0)
            {
                System.Diagnostics.Debug.WriteLine($"Got {messages.Length} dequeue.");
                this.WriteInternal(messages);
            }
        }

        /// <summary>
        /// Internal handler to write outstanding messages.
        /// </summary>
        /// <remarks>
        /// Task of this method is to physically write all provided messages.
        /// </remarks>
        /// <param name="messages">
        /// The list of message to write.
        /// </param>
        private void WriteInternal(params String[] messages)
        {
            String filename = base.Settings.GetCurrentFilename(out Boolean dispose);
            Random random = new Random((Int32)DateTime.Now.Ticks);

            if (dispose)
            {
                while (!this.facade.Empty(filename))
                {
                    if (this.IsDisposed) { return; }

                    Int32 delay = random.Next(10, 200);
                    System.Diagnostics.Debug.WriteLine($"Discarding content of file '{filename}' has failed. Next try in {delay} milliseconds.");
                    System.Threading.Thread.Sleep(delay);
                }
            }

            while (!this.facade.Write(filename, base.Settings.Encoding, messages))
            {
                if (this.IsDisposed) { return; }

                Int32 delay = random.Next(10, 200);
                System.Diagnostics.Debug.WriteLine($"Writing to file '{filename}' has failed. Next try in {delay} milliseconds.");
                System.Threading.Thread.Sleep(delay);
            }
        }

        #endregion
    }
}
