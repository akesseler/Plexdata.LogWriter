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
using Plexdata.LogWriter.Internals.Factories;
using System;
using System.Reflection;

namespace Plexdata.LogWriter.Logging
{
    /// <summary>
    /// The abstract base class of all logger classes.
    /// </summary>
    /// <remarks>
    /// This abstract base class provides all functionalities 
    /// to be shared with all other logger classes.
    /// </remarks>
    public abstract class LoggerBase
    {
        #region Construction

        /// <summary>
        /// The default constructor.
        /// </summary>
        /// <remarks>
        /// This constructor initializes the new instance using default logging 
        /// factory.
        /// </remarks>
        /// <seealso cref="LoggerBase(ILoggerFactory)"/>
        protected LoggerBase()
            : this(new LoggerFactory())
        {
        }

        /// <summary>
        /// The parameterized constructor.
        /// </summary>
        /// <remarks>
        /// This constructor initializes the new instance using provided logging 
        /// factory.
        /// </remarks>
        /// <param name="factory">
        /// The logging factory instance to be used.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// This exception is thrown if parameter <paramref name="factory"/> is 
        /// <c>null</c>.
        /// </exception>
        /// <seealso cref="LoggerBase()"/>
        protected LoggerBase(ILoggerFactory factory)
            : base()
        {
            this.LoggerFactory = factory ?? throw new ArgumentNullException(nameof(factory));
        }

        /// <summary>
        /// This class destructor.
        /// </summary>
        /// <remarks>
        /// The destructor cleans its resources.
        /// </remarks>
        ~LoggerBase()
        {
        }

        #endregion

        #region Protected properties

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
        protected ILoggerFactory LoggerFactory { get; private set; }

        #endregion

        #region Protected methods

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
        protected String ResolveContext<TContext>(ILoggerSettings settings)
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
        protected String ResolveScope<TScope>(TScope scope, ILoggerSettings settings)
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

        #endregion
    }
}
