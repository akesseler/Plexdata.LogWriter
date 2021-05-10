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

using System;
using System.Diagnostics;
using System.Reflection;

namespace Plexdata.LogWriter.Internals.Logging
{
    /// <summary>
    /// This internal class provides functionality to manage logging scopes.
    /// </summary>
    /// <remarks>
    /// Such a logging scope is enabled by calling method 
    /// <see cref="LogWriter.Logging.LoggerBase{TSettings}.CreateScope{TScope}(TScope)"/> 
    /// of class <see cref="LogWriter.Logging.LoggerBase{TSettings}"/>.
    /// </remarks>
    [DebuggerDisplay("{DebuggerDisplay,nq}")]
    internal class LoggingScope : IDisposable
    {
        #region Public events

        /// <summary>
        /// Occurs when the object is being disposed.
        /// </summary>
        /// <remarks>
        /// This event occurs as soon as an instance of this class is being disposed.
        /// </remarks>
        /// <seealso cref="Dispose()"/>
        /// <seealso cref="Dispose(Boolean)"/>
        public event EventHandler<EventArgs> Disposing;

        #endregion

        #region Construction

        /// <summary>
        /// The default constructor initializes instances of this class.
        /// </summary>
        /// <remarks>
        /// This constructor just calls constructor <see cref="LoggingScope(Object)"/> 
        /// with parameter <c>null</c>.
        /// </remarks>
        /// <seealso cref="LoggingScope(Object)"/>
        public LoggingScope()
            : this(null)
        {
        }

        /// <summary>
        /// This constructor initializes instances of this class using provided 
        /// <paramref name="value"/>.
        /// </summary>
        /// <remarks>
        /// This constructor just takes parameter <paramref name="value"/> and 
        /// assigns it to property <see cref="Value"/>.
        /// </remarks>
        /// <param name="value">
        /// The value to get a scope text from.
        /// </param>
        /// <seealso cref="LoggingScope()"/>
        public LoggingScope(Object value)
            : base()
        {
            this.Value = value;
        }

        /// <summary>
        /// The class destructor.
        /// </summary>
        /// <remarks>
        /// The destructor cleans its resources.
        /// </remarks>
        /// <see cref="Dispose(Boolean)"/>
        ~LoggingScope()
        {
            this.Dispose(false);
        }

        #endregion

        #region Public properties

        /// <summary>
        /// An object representing the assigned scope.
        /// </summary>
        /// <remarks>
        /// The object to be used to get the scope text from.
        /// </remarks>
        /// <value>
        /// The value can be anything. But keep in mind, list 
        /// types are not yet supported.
        /// </value>
        public Object Value { get; private set; } = null;

        /// <summary>
        /// Determines whether an instance of this class has been 
        /// disposed already.
        /// </summary>
        /// <remarks>
        /// This property allows to check if an instance of this 
        /// class has been disposed already.
        /// </remarks>
        /// <value>
        /// True if the instance has been disposed and false otherwise.
        /// </value>
        /// <seealso cref="Dispose()"/>
        /// <seealso cref="Dispose(Boolean)"/>
        public Boolean IsDisposed { get; private set; } = false;

        #endregion

        #region Public methods

        /// <summary>
        /// Returns the scope's string representation.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This method returns the scope's string representation.
        /// </para>
        /// <list type="bullet">
        /// <item><description>
        /// If property <see cref="Value"/> is for example of type <see cref="String"/> then this string is used.
        /// </description></item>
        /// <item><description>
        /// If property <see cref="Value"/> is for example of type <see cref="Guid"/> then the guid's string representation 
        /// is taken by calling <see cref="Guid.ToString()"/>.
        /// </description></item>
        /// <item><description>
        /// If property <see cref="Value"/> is for example of type <see cref="MemberInfo"/> then the referenced member's 
        /// <see cref="MemberInfo.Name"/> is taken instead. 
        /// </description></item>
        /// <item><description>
        /// In all other cases the scope text is determined by using an object's <see cref="Object.ToString()"/> method.
        /// </description></item>
        /// </list>
        /// </remarks>
        /// <returns>
        /// The string representation of the scope object.
        /// </returns>
        public String ToDisplay()
        {
            if (this.Value is String)
            {
                return this.Value as String;
            }

            if (this.Value is Guid)
            {
                return this.Value.ToString();
            }

            if (this.Value is MemberInfo)
            {
                return (this.Value as MemberInfo).Name;
            }

            return this.Value?.ToString() ?? null;
        }

        /// <summary>
        /// Frees all disposable instance resources.
        /// </summary>
        /// <remarks>
        /// This method frees all disposable instance resources.
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
        /// Frees all disposable instance resources.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This method does the actual work of freeing and releasing 
        /// all its instance resources.
        /// </para>
        /// <para>
        /// If parameter <paramref name="disposing"/> is true and event 
        /// <see cref="Disposing"/> has been assigned then this event will 
        /// be invoked.
        /// </para>
        /// <para>
        /// An object assigned to property <see cref="Value"/> will never 
        /// be disposed.
        /// </para>
        /// </remarks>
        /// <param name="disposing">
        /// True if freeing of managed resources is needed and false 
        /// otherwise.
        /// </param>
        protected virtual void Dispose(Boolean disposing)
        {
            if (!this.IsDisposed)
            {
                if (disposing)
                {
                    try
                    {
                        this.Disposing?.Invoke(this, EventArgs.Empty);
                    }
                    catch (Exception exception)
                    {
                        Debug.WriteLine(exception);
                    }

                    // Intentionally, do never dispose an assigned
                    // value, no matter if it would be possible.
                    this.Value = null;
                }

                this.IsDisposed = true;
            }
        }

        #endregion

        #region Private properties

        /// <summary>
        /// Gets a debug representation of this instance.
        /// </summary>
        /// <remarks>
        /// This property is only for debugging and allows to get a string 
        /// representation of the assigned value.
        /// </remarks>
        /// <value>
        /// The string representation of the assigned value.
        /// </value>
        private String DebuggerDisplay
        {
            get
            {
                return $"Type: \"{this.Value?.GetType()?.Name ?? "null"}\", {nameof(this.Value)}: \"{this.ToDisplay() ?? "null"}\"";
            }
        }

        #endregion
    }
}
