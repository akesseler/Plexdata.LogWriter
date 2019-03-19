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
using Plexdata.LogWriter.Internals.Factories;
using System;
using System.Reflection;
using System.Text;

namespace Plexdata.LogWriter.Logging
{
    /// <summary>
    /// The abstract base class of all logger classes.
    /// </summary>
    /// <remarks>
    /// This abstract base class provides all functionalities 
    /// to be shared with all other logger classes.
    /// </remarks>
    /// <typeparam name="TSettings">
    /// The type of <typeparamref name="TSettings"/> must be of 
    /// type <see cref="ILoggerSettings"/>.
    /// </typeparam>
    public abstract class LoggerBase<TSettings> where TSettings : ILoggerSettings
    {
        #region Construction

        /// <summary>
        /// The default constructor.
        /// </summary>
        /// <remarks>
        /// This constructor initializes the new instance using default logging 
        /// factory.
        /// </remarks>
        /// <param name="settings">
        /// The settings instance to be applied.
        /// </param>
        /// <seealso cref="LoggerBase(TSettings, ILoggerFactory)"/>
        protected LoggerBase(TSettings settings)
            : this(settings, new LoggerFactory())
        {
        }

        /// <summary>
        /// The parameterized constructor.
        /// </summary>
        /// <remarks>
        /// This constructor initializes the new instance using provided logging 
        /// factory.
        /// </remarks>
        /// <param name="settings">
        /// The settings instance to be applied.
        /// </param>
        /// <param name="factory">
        /// The logging factory instance to be used.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// This exception is thrown if parameter <paramref name="settings"/> 
        /// or if parameter <paramref name="factory"/> is <c>null</c>.
        /// </exception>
        /// <seealso cref="LoggerBase(TSettings)"/>
        protected LoggerBase(TSettings settings, ILoggerFactory factory)
            : base()
        {
            // Operator ?? cannot be used together with "throw"
            if (settings == null) { throw new ArgumentNullException(nameof(settings)); }

            this.Settings = settings;

            this.Factory = factory ?? throw new ArgumentNullException(nameof(factory));
        }

        /// <summary>
        /// The class destructor.
        /// </summary>
        /// <remarks>
        /// The destructor cleans its resources.
        /// </remarks>
        ~LoggerBase()
        {
        }

        #endregion

        #region Public properties

        /// <summary>
        /// Determines if logging is disabled or not.
        /// </summary>
        /// <remarks>
        /// This is just a convenient property to be able to easily find out if logging is possible 
        /// or not.
        /// </remarks>
        /// <value>
        /// True is returned if current logging level is equal to <see cref="LogLevel.Disabled"/>. 
        /// False is returned in any other case.
        /// </value>
        public virtual Boolean IsDisabled
        {
            get
            {
                return this.Settings.LogLevel == LogLevel.Disabled;
            }
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Determines if a particular logging level is enabled.
        /// </summary>
        /// <remarks>
        /// The method actually checks if currently configured logging level is equal to or greater 
        /// than the provided logging level. In other words, the enable check is more or less some 
        /// kind of range check.
        /// </remarks>
        /// <param name="level">
        /// The logging level to be verified.
        /// </param>
        /// <returns>
        /// True is returned if provided logging level is enabled and false if not.
        /// </returns>
        public virtual Boolean IsEnabled(LogLevel level)
        {
            return (Int32)level >= (Int32)this.Settings.LogLevel;
        }

        #endregion

        #region Protected properties

        /// <summary>
        /// The instance of applied logger settings.
        /// </summary>
        /// <remarks>
        /// The applied logger settings are shared between this class 
        /// and derived classes.
        /// </remarks>
        /// <value>
        /// An instance of type <typeparamref name="TSettings"/> which 
        /// must be of type <see cref="ILoggerSettings"/>.
        /// </value>
        protected TSettings Settings { get; private set; }

        /// <summary>
        /// Gets the assigned logging factory.
        /// </summary>
        /// <remarks>
        /// The logging factory is actually used to create instances of type 
        /// <see cref="ILogEvent"/> and of type <see cref="ILogEventFormatter"/>.
        /// </remarks>
        /// <value>
        /// An instance of type <see cref="ILoggerFactory"/>.
        /// </value>
        protected ILoggerFactory Factory { get; private set; }

        #endregion

        #region Protected methods

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
        /// <seealso cref="IsDisabled"/>
        /// <seealso cref="IsEnabled(LogLevel)"/>
        /// <seealso cref="CreateOutput(LogLevel, String, String, String, Exception, ValueTuple{String, Object}[])"/>
        protected abstract void Write(LogLevel level, String context, String scope, String message, Exception exception, params (String Label, Object Value)[] details);

        /// <summary>
        /// This method resolves the logging context.
        /// </summary>
        /// <remarks>
        /// Resolving the message context is done by method 
        /// <see cref="LoggerBase{TSettings}.ResolveContext{TContext}(TSettings)"/>.
        /// </remarks>
        /// <typeparam name="TContext">
        /// The type to get the logging context from.
        /// </typeparam>
        /// <returns>
        /// The logging context or an empty string in case of an error.
        /// </returns>
        protected String ResolveContext<TContext>()
        {
            return this.ResolveContext<TContext>(this.Settings);
        }

        /// <summary>
        /// This method resolves the logging context.
        /// </summary>
        /// <remarks>
        /// The logging context is either the full name or the short name of the type 
        /// of <typeparamref name="TContext"/>. If the full name or short name is used 
        /// will be determined from current <paramref name="settings"/>.
        /// </remarks>
        /// <typeparam name="TContext">
        /// The type to get the logging context from.
        /// </typeparam>
        /// <param name="settings">
        /// The settings to be used to determine whether the full name or the short 
        /// name is use.
        /// </param>
        /// <returns>
        /// The logging context or an empty string in case of an error.
        /// </returns>
        protected String ResolveContext<TContext>(TSettings settings)
        {
            try
            {
                return settings.FullName ? typeof(TContext).FullName : typeof(TContext).Name;
            }
            catch (Exception /*error*/)
            {
                return String.Empty;
            }
        }

        /// <summary>
        /// This method resolves the logging scope.
        /// </summary>
        /// <remarks>
        /// Resolving the message scope is done by method 
        /// <see cref="LoggerBase{TSettings}.ResolveScope{TScope}(TScope, TSettings)"/>.
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
            return this.ResolveScope<TScope>(scope, this.Settings);
        }

        /// <summary>
        /// This method resolves the logging scope.
        /// </summary>
        /// <remarks>
        /// The logging scope is intended to be more than just the type name. If the 
        /// <paramref name="scope"/> type is for example a string then this string is 
        /// taken as it is. Or if the <paramref name="scope"/> type is for example of 
        /// type of <see cref="MemberInfo"/> then member name is taken instead. In all 
        /// other cases the logging scope is either the full name or the short name 
        /// of the type of <typeparamref name="TScope"/>. If the full name or short 
        /// name is used will be determined from current <paramref name="settings"/>.
        /// </remarks>
        /// <typeparam name="TScope">
        /// The type to get the logging scope from.
        /// </typeparam>
        /// <param name="scope">
        /// The instance of the type to get the logging scope from.
        /// </param>
        /// <param name="settings">
        /// The settings to be used to determine whether the full name or the short 
        /// name is use.
        /// </param>
        /// <returns>
        /// The logging scope or an empty string in case of an error.
        /// </returns>
        protected String ResolveScope<TScope>(TScope scope, TSettings settings)
        {
            try
            {
                if (scope == null)
                {
                    return settings.FullName ? typeof(TScope).FullName : typeof(TScope).Name;
                }

                if (scope is String)
                {
                    return scope as String;
                }

                if (scope is MemberInfo)
                {
                    return (scope as MemberInfo).Name;
                }

                return settings.FullName ? scope.GetType().FullName : scope.GetType().Name;
            }
            catch (Exception /*error*/)
            {
                return String.Empty;
            }
        }

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
        /// <seealso cref="CreateOutput(TSettings, LogLevel, String, String, String, Exception, ValueTuple{String, Object}[])"/>
        protected virtual String CreateOutput(LogLevel level, String context, String scope, String message, Exception exception, params (String Label, Object Value)[] details)
        {
            return this.CreateOutput(this.Settings, level, context, scope, message, exception, details);
        }

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
        /// <param name="settings">
        /// The settings to be used for massage creation.
        /// </param>
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
        /// <exception cref="ArgumentNullException">
        /// This exception is thrown if provided settings instance is <c>null</c>.
        /// </exception>
        /// <seealso cref="CreateOutput(LogLevel, String, String, String, Exception, ValueTuple{String, Object}[])"/>
        protected virtual String CreateOutput(TSettings settings, LogLevel level, String context, String scope, String message, Exception exception, params (String Label, Object Value)[] details)
        {
            if (settings == null) { throw new ArgumentNullException(nameof(settings)); }

            ILogEvent output = this.Factory.CreateLogEvent(level, DateTime.Now, context, scope, message, exception, details);

            if (output.IsValid)
            {
                StringBuilder builder = new StringBuilder(512);

                // This might not be optimizable because of the logging type for example may change during runtime.
                this.Factory.CreateLogEventFormatter(settings).Format(builder, output);

                return builder.ToString();
            }

            return String.Empty;
        }

        #endregion
    }
}
