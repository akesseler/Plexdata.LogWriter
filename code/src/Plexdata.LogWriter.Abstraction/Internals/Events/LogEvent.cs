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

namespace Plexdata.LogWriter.Internals.Events
{
    /// <summary>
    /// This class represents one logging event.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Logging events represent nothing else but a summarizing of 
    /// logging dependent information.
    /// </para>
    /// <para>
    /// But on the other hand, this class is more than just data 
    /// transfer object because it does some additional tasks for 
    /// a safer access.
    /// </para>
    /// </remarks>
    internal class LogEvent : ILogEvent
    {
        #region Construction

        /// <summary>
        /// The basic class constructor with the minimum set of parameters.
        /// </summary>
        /// <remarks>
        /// This constructor does the basic initialization of its properties.
        /// </remarks>
        /// <param name="level">
        /// The logging level to be assigned.
        /// </param>
        /// <param name="time">
        /// The time stamp to be assigned.
        /// </param>
        /// <param name="context">
        /// The optional context to be assigned.
        /// </param>
        /// <param name="scope">
        /// The optional scope to be assigned.
        /// </param>
        /// <param name="message">
        /// The mandatory message to be assigned.
        /// </param>
        /// <param name="exception">
        /// The optional exception to be assigned.
        /// </param>
        /// <param name="details">
        /// An optional list of key-values-pairs to be assigned.
        /// </param>
        /// <seealso cref="LogEvent(Guid, LogLevel, DateTime, String, String, String, Exception, ValueTuple{String, Object}[])"/>
        public LogEvent(LogLevel level, DateTime time, String context, String scope, String message, Exception exception, params (String Label, Object Value)[] details)
            : this(Guid.NewGuid(), level, time, context, scope, message, exception, details)
        {
        }

        /// <summary>
        /// The extended class constructor with the complete set of parameters.
        /// </summary>
        /// <remarks>
        /// Right here an additional note about message seems to be useful. Indeed, 
        /// the logging message is defined as mandatory parameter. But if parameter 
        /// <paramref name="message"/> is invalid and parameter <paramref name="exception"/> 
        /// is not <c>null</c>, then the message of the exception is used as logging 
        /// message!
        /// </remarks>
        /// <param name="key">
        /// The logging event key to be assigned.
        /// </param>
        /// <param name="level">
        /// The logging level to be assigned.
        /// </param>
        /// <param name="time">
        /// The time stamp to be assigned.
        /// </param>
        /// <param name="context">
        /// The optional context to be assigned.
        /// </param>
        /// <param name="scope">
        /// The optional scope to be assigned.
        /// </param>
        /// <param name="message">
        /// The mandatory message to be assigned.
        /// </param>
        /// <param name="exception">
        /// The optional exception to be assigned.
        /// </param>
        /// <param name="details">
        /// An optional list of key-values-pairs to be assigned.
        /// </param>
        /// <seealso cref="LogEvent(LogLevel, DateTime, String, String, String, Exception, ValueTuple{String, Object}[])"/>
        public LogEvent(Guid key, LogLevel level, DateTime time, String context, String scope, String message, Exception exception, params (String Label, Object Value)[] details)
        {
            this.Key = key;
            this.Level = this.GetLevel(level);
            this.Time = time;
            this.Context = this.GetContext(context);
            this.Scope = this.GetScope(scope);
            this.Message = this.GetMessage(message, exception);
            this.Exception = exception;
            this.Details = details;
        }

        #endregion

        #region Public properties

        /// <inheritdoc />
        public Boolean IsValid
        {
            get
            {
                // TODO: Maybe add more validation conditions...
                return !String.IsNullOrWhiteSpace(this.Message);
            }
        }

        /// <inheritdoc />
        public Guid Key { get; private set; }

        /// <inheritdoc />
        public LogLevel Level { get; private set; }

        /// <inheritdoc />
        public DateTime Time { get; private set; }

        /// <inheritdoc />
        public String Context { get; private set; }

        /// <inheritdoc />
        public String Scope { get; private set; }

        /// <inheritdoc />
        public String Message { get; private set; }

        /// <inheritdoc />
        public Exception Exception { get; private set; }

        /// <inheritdoc />
        public (String Label, Object Value)[] Details { get; private set; }

        #endregion

        #region Private methods

        /// <summary>
        /// This method gets a valid logging level and returns it.
        /// </summary>
        /// <remarks>
        /// This method returns the default logging level if provided 
        /// parameter <paramref name="level"/> is not part of the logging 
        /// level enumeration.
        /// </remarks>
        /// <param name="level">
        /// The logging level to be validated.
        /// </param>
        /// <returns>
        /// The validated logging level.
        /// </returns>
        private LogLevel GetLevel(LogLevel level)
        {
            if (Enum.IsDefined(typeof(LogLevel), level))
            {
                return level;
            }

            return LogLevel.Default;
        }

        /// <summary>
        /// This method gets a valid logging context and returns it.
        /// </summary>
        /// <remarks>
        /// This method returns an empty string if provided parameter 
        /// <paramref name="context"/> is <c>null</c>, empty or only 
        /// consists of white spaces. Furthermore, the returned context 
        /// does not include leading and trailing whitespaces.
        /// </remarks>
        /// <param name="context">
        /// The logging context to be validated.
        /// </param>
        /// <returns>
        /// The validated logging context or and empty string.
        /// </returns>
        private String GetContext(String context)
        {
            return (context ?? String.Empty).Trim();
        }

        /// <summary>
        /// This method gets a valid logging scope and returns it.
        /// </summary>
        /// <remarks>
        /// This method returns an empty string if provided parameter 
        /// <paramref name="scope"/> is <c>null</c>, empty or only 
        /// consists of white spaces. Furthermore, the returned scope 
        /// does not include leading and trailing whitespaces.
        /// </remarks>
        /// <param name="scope">
        /// The logging scope to be validated.
        /// </param>
        /// <returns>
        /// The validated logging scope or and empty string.
        /// </returns>
        private String GetScope(String scope)
        {
            return (scope ?? String.Empty).Trim();
        }

        /// <summary>
        /// This method gets a valid logging message and returns it.
        /// </summary>
        /// <remarks>
        /// This method checks if provided <paramref name="message"/> is valid and 
        /// returns it. If the message is invalid then the method takes over the 
        /// exception's message, but only if parameter <paramref name="exception"/> 
        /// is not null. An empty string is returned in any other case. Furthermore, 
        /// the returned message does not include leading and trailing whitespaces.
        /// </remarks>
        /// <param name="message">
        /// The logging message to be validated.
        /// </param>
        /// <param name="exception">
        /// An exception to get the message from if provided message 
        /// is invalid.
        /// </param>
        /// <returns>
        /// The validated logging message or and empty string.
        /// </returns>
        private String GetMessage(String message, Exception exception)
        {
            if (!String.IsNullOrWhiteSpace(message))
            {
                return message.Trim();
            }

            if (exception != null)
            {
                return exception.Message.Trim();
            }

            return String.Empty;
        }

        #endregion
    }
}
