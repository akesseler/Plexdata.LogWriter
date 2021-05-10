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

using Plexdata.LogWriter.Definitions;
using System;
using System.Reflection;

namespace Plexdata.LogWriter.Abstraction
{
    /// <summary>
    /// The general interface of all provided logger implementations.
    /// </summary>
    /// <remarks>
    /// This general interface serves as the base of all supported logger 
    /// implementations and defines the minimum of the supported logging 
    /// functionality.
    /// </remarks>
    public interface ILogger
    {
        #region Management stuff

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
        Boolean IsDisabled { get; }

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
        Boolean IsEnabled(LogLevel level);

        /// <summary>
        /// Begins a logical operation scope.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This method begins a logical operation scope.
        /// </para>
        /// <list type="bullet">
        /// <item><description>
        /// If parameter <paramref name="scope"/> is for example of type <see cref="String"/> then this string is used.
        /// </description></item>
        /// <item><description>
        /// If parameter <paramref name="scope"/> is for example of type <see cref="Guid"/> then the guid's string representation 
        /// is taken by calling <see cref="Guid.ToString()"/>.
        /// </description></item>
        /// <item><description>
        /// If parameter <paramref name="scope"/> is for example of type <see cref="MemberInfo"/> then the referenced member's 
        /// <see cref="MemberInfo.Name"/> is taken instead. 
        /// </description></item>
        /// <item><description>
        /// In all other cases the scope text is determined by using an object's <see cref="Object.ToString()"/> method.
        /// </description></item>
        /// </list>
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
        IDisposable BeginScope<TScope>(TScope scope);

        #endregion

        #region Write methods

        /// <summary>
        /// This method writes the <paramref name="message"/> into the logging target using provided logging 
        /// <paramref name="level"/>.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This method performs the actual writing of logging data into the logging target using a particular logging level. 
        /// Be aware, nothing will happen if <paramref name="message"/> is <c>null</c>, <em>empty</em> or consists only of 
        /// <em>whitespaces</em>.
        /// </para>
        /// </remarks>
        /// <param name="level">
        /// The logging level to be used to tag the written logging data.
        /// </param>
        /// <param name="message">
        /// The message to be written.
        /// </param>
        void Write(LogLevel level, String message);

        /// <summary>
        /// This method writes the <paramref name="message"/> into the logging target using provided logging 
        /// <paramref name="level"/>.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This method performs the actual writing of logging data into the logging target using a particular logging level. 
        /// Be aware, nothing will happen if <paramref name="message"/> is <c>null</c>, <em>empty</em> or consists only of 
        /// <em>whitespaces</em>.
        /// </para>
        /// </remarks>
        /// <param name="level">
        /// The logging level to be used to tag the written logging data.
        /// </param>
        /// <param name="message">
        /// The message to be written.
        /// </param>
        /// <param name="details">
        /// An optional list of label-value-pair combinations containing additional information.
        /// </param>
        void Write(LogLevel level, String message, params (String Label, Object Value)[] details);

        /// <summary>
        /// This method writes the <paramref name="exception"/> into the logging target using provided logging 
        /// <paramref name="level"/>.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This method performs the actual writing of logging data into the logging target using a particular logging level. 
        /// Be aware, nothing will happen if <paramref name="exception"/> is <c>null</c>. 
        /// </para>
        /// </remarks>
        /// <param name="level">
        /// The logging level to be used to tag the written logging data.
        /// </param>
        /// <param name="exception">
        /// The exception to be written.
        /// </param>
        void Write(LogLevel level, Exception exception);

        /// <summary>
        /// This method writes the <paramref name="exception"/> into the logging target using provided logging 
        /// <paramref name="level"/>.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This method performs the actual writing of logging data into the logging target using a particular logging level. 
        /// Be aware, nothing will happen if <paramref name="exception"/> is <c>null</c>. 
        /// </para>
        /// </remarks>
        /// <param name="level">
        /// The logging level to be used to tag the written logging data.
        /// </param>
        /// <param name="exception">
        /// The exception to be written.
        /// </param>
        /// <param name="details">
        /// An optional list of label-value-pair combinations containing additional information.
        /// </param>
        void Write(LogLevel level, Exception exception, params (String Label, Object Value)[] details);

        /// <summary>
        /// This method writes the <paramref name="message"/> as well as the <paramref name="exception"/> into 
        /// the logging target using provided logging <paramref name="level"/>.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This method performs the actual writing of logging data into the logging target using a particular logging level. 
        /// Be aware, nothing will happen if <paramref name="message"/> is <c>null</c>, <em>empty</em> or consists only of 
        /// <em>whitespaces</em> and if <paramref name="exception"/> is <c>null</c>. 
        /// </para>
        /// <para>
        /// On the other hand, if the <paramref name="message"/> is invalid but the <paramref name="exception"/> is valid 
        /// then the message text is taken from the exception instead.
        /// </para>
        /// </remarks>
        /// <param name="level">
        /// The logging level to be used to tag the written logging data.
        /// </param>
        /// <param name="message">
        /// The message to be written.
        /// </param>
        /// <param name="exception">
        /// The exception to be written.
        /// </param>
        void Write(LogLevel level, String message, Exception exception);

        /// <summary>
        /// This method writes the <paramref name="message"/> as well as the <paramref name="exception"/> into 
        /// the logging target using provided logging <paramref name="level"/>.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This method performs the actual writing of logging data into the logging target using a particular logging level. 
        /// Be aware, nothing will happen if <paramref name="message"/> is <c>null</c>, <em>empty</em> or consists only of 
        /// <em>whitespaces</em> and if <paramref name="exception"/> is <c>null</c>. 
        /// </para>
        /// <para>
        /// On the other hand, if the <paramref name="message"/> is invalid but the <paramref name="exception"/> is valid 
        /// then the message text is taken from the exception instead.
        /// </para>
        /// </remarks>
        /// <param name="level">
        /// The logging level to be used to tag the written logging data.
        /// </param>
        /// <param name="message">
        /// The message to be written.
        /// </param>
        /// <param name="exception">
        /// The exception to be written.
        /// </param>
        /// <param name="details">
        /// An optional list of label-value-pair combinations containing additional information.
        /// </param>
        void Write(LogLevel level, String message, Exception exception, params (String Label, Object Value)[] details);

        /// <summary>
        /// This method writes the <paramref name="message"/> for provided <paramref name="scope"/> into the 
        /// logging target using provided logging <paramref name="level"/>.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This method performs the actual writing of logging data into the logging target using a particular logging level. 
        /// Be aware, nothing will happen if <paramref name="message"/> is <c>null</c>, <em>empty</em> or consists only of 
        /// <em>whitespaces</em>.
        /// </para>
        /// <para>
        /// The logging scope is intended to be more than just the type name.
        /// </para>
        /// <list type="bullet">
        /// <item><description>
        /// If parameter <paramref name="scope"/> is for example of type <see cref="String"/> then this string is used.
        /// </description></item>
        /// <item><description>
        /// If parameter <paramref name="scope"/> is for example of type <see cref="Guid"/> then the guid's string representation 
        /// is taken by calling <see cref="Guid.ToString()"/>. Using a <see cref="Guid"/> as scope type can be seen as some kind 
        /// of <em>correlation ID</em>, especially if this <see cref="Guid"/> is the same for multiple calls. 
        /// </description></item>
        /// <item><description>
        /// If parameter <paramref name="scope"/> is for example of type <see cref="MemberInfo"/> then the referenced member's 
        /// <see cref="MemberInfo.Name"/> is taken instead. 
        /// </description></item>
        /// <item><description>
        /// In all other cases the scope text is either taken from the type's <see cref="Type.FullName"/> or it is just the 
        /// <see cref="MemberInfo.Name"/> of type <typeparamref name="TScope"/>. The usage of full or short name is determined 
        /// from current settings.
        /// </description></item>
        /// </list>
        /// </remarks>
        /// <typeparam name="TScope">
        /// The type to get the logging scope from.
        /// </typeparam>
        /// <param name="scope">
        /// The instance of the type to get the logging scope from.
        /// </param>
        /// <param name="level">
        /// The logging level to be used to tag the written logging data.
        /// </param>
        /// <param name="message">
        /// The message to be written.
        /// </param>
        void Write<TScope>(TScope scope, LogLevel level, String message);

        /// <summary>
        /// This method writes the <paramref name="message"/> for provided <paramref name="scope"/> into the 
        /// logging target using provided logging <paramref name="level"/>.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This method performs the actual writing of logging data into the logging target using a particular logging level. 
        /// Be aware, nothing will happen if <paramref name="message"/> is <c>null</c>, <em>empty</em> or consists only of 
        /// <em>whitespaces</em>.
        /// </para>
        /// <para>
        /// The logging scope is intended to be more than just the type name.
        /// </para>
        /// <list type="bullet">
        /// <item><description>
        /// If parameter <paramref name="scope"/> is for example of type <see cref="String"/> then this string is used.
        /// </description></item>
        /// <item><description>
        /// If parameter <paramref name="scope"/> is for example of type <see cref="Guid"/> then the guid's string representation 
        /// is taken by calling <see cref="Guid.ToString()"/>. Using a <see cref="Guid"/> as scope type can be seen as some kind 
        /// of <em>correlation ID</em>, especially if this <see cref="Guid"/> is the same for multiple calls. 
        /// </description></item>
        /// <item><description>
        /// If parameter <paramref name="scope"/> is for example of type <see cref="MemberInfo"/> then the referenced member's 
        /// <see cref="MemberInfo.Name"/> is taken instead. 
        /// </description></item>
        /// <item><description>
        /// In all other cases the scope text is either taken from the type's <see cref="Type.FullName"/> or it is just the 
        /// <see cref="MemberInfo.Name"/> of type <typeparamref name="TScope"/>. The usage of full or short name is determined 
        /// from current settings.
        /// </description></item>
        /// </list>
        /// </remarks>
        /// <typeparam name="TScope">
        /// The type to get the logging scope from.
        /// </typeparam>
        /// <param name="scope">
        /// The instance of the type to get the logging scope from.
        /// </param>
        /// <param name="level">
        /// The logging level to be used to tag the written logging data.
        /// </param>
        /// <param name="message">
        /// The message to be written.
        /// </param>
        /// <param name="details">
        /// An optional list of label-value-pair combinations containing additional information.
        /// </param>
        void Write<TScope>(TScope scope, LogLevel level, String message, params (String Label, Object Value)[] details);

        /// <summary>
        /// This method writes the <paramref name="exception"/> for provided <paramref name="scope"/> into the 
        /// logging target using provided logging <paramref name="level"/>.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This method performs the actual writing of logging data into the logging target using a particular logging level. 
        /// Be aware, nothing will happen if <paramref name="exception"/> is <c>null</c>. 
        /// </para>
        /// <para>
        /// The logging scope is intended to be more than just the type name.
        /// </para>
        /// <list type="bullet">
        /// <item><description>
        /// If parameter <paramref name="scope"/> is for example of type <see cref="String"/> then this string is used.
        /// </description></item>
        /// <item><description>
        /// If parameter <paramref name="scope"/> is for example of type <see cref="Guid"/> then the guid's string representation 
        /// is taken by calling <see cref="Guid.ToString()"/>. Using a <see cref="Guid"/> as scope type can be seen as some kind 
        /// of <em>correlation ID</em>, especially if this <see cref="Guid"/> is the same for multiple calls. 
        /// </description></item>
        /// <item><description>
        /// If parameter <paramref name="scope"/> is for example of type <see cref="MemberInfo"/> then the referenced member's 
        /// <see cref="MemberInfo.Name"/> is taken instead. 
        /// </description></item>
        /// <item><description>
        /// In all other cases the scope text is either taken from the type's <see cref="Type.FullName"/> or it is just the 
        /// <see cref="MemberInfo.Name"/> of type <typeparamref name="TScope"/>. The usage of full or short name is determined 
        /// from current settings.
        /// </description></item>
        /// </list>
        /// </remarks>
        /// <typeparam name="TScope">
        /// The type to get the logging scope from.
        /// </typeparam>
        /// <param name="scope">
        /// The instance of the type to get the logging scope from.
        /// </param>
        /// <param name="level">
        /// The logging level to be used to tag the written logging data.
        /// </param>
        /// <param name="exception">
        /// The exception to be written.
        /// </param>
        void Write<TScope>(TScope scope, LogLevel level, Exception exception);

        /// <summary>
        /// This method writes the <paramref name="exception"/> for provided <paramref name="scope"/> into the 
        /// logging target using provided logging <paramref name="level"/>.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This method performs the actual writing of logging data into the logging target using a particular logging level. 
        /// Be aware, nothing will happen if <paramref name="exception"/> is <c>null</c>. 
        /// </para>
        /// <para>
        /// The logging scope is intended to be more than just the type name.
        /// </para>
        /// <list type="bullet">
        /// <item><description>
        /// If parameter <paramref name="scope"/> is for example of type <see cref="String"/> then this string is used.
        /// </description></item>
        /// <item><description>
        /// If parameter <paramref name="scope"/> is for example of type <see cref="Guid"/> then the guid's string representation 
        /// is taken by calling <see cref="Guid.ToString()"/>. Using a <see cref="Guid"/> as scope type can be seen as some kind 
        /// of <em>correlation ID</em>, especially if this <see cref="Guid"/> is the same for multiple calls. 
        /// </description></item>
        /// <item><description>
        /// If parameter <paramref name="scope"/> is for example of type <see cref="MemberInfo"/> then the referenced member's 
        /// <see cref="MemberInfo.Name"/> is taken instead. 
        /// </description></item>
        /// <item><description>
        /// In all other cases the scope text is either taken from the type's <see cref="Type.FullName"/> or it is just the 
        /// <see cref="MemberInfo.Name"/> of type <typeparamref name="TScope"/>. The usage of full or short name is determined 
        /// from current settings.
        /// </description></item>
        /// </list>
        /// </remarks>
        /// <typeparam name="TScope">
        /// The type to get the logging scope from.
        /// </typeparam>
        /// <param name="scope">
        /// The instance of the type to get the logging scope from.
        /// </param>
        /// <param name="level">
        /// The logging level to be used to tag the written logging data.
        /// </param>
        /// <param name="exception">
        /// The exception to be written.
        /// </param>
        /// <param name="details">
        /// An optional list of label-value-pair combinations containing additional information.
        /// </param>
        void Write<TScope>(TScope scope, LogLevel level, Exception exception, params (String Label, Object Value)[] details);

        /// <summary>
        /// This method writes the <paramref name="message"/> as well as the <paramref name="exception"/> for provided 
        /// <paramref name="scope"/> into the logging target using provided logging <paramref name="level"/>.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This method performs the actual writing of logging data into the logging target using a particular logging level. 
        /// Be aware, nothing will happen if <paramref name="message"/> is <c>null</c>, <em>empty</em> or consists only of 
        /// <em>whitespaces</em> and if <paramref name="exception"/> is <c>null</c>. 
        /// </para>
        /// <para>
        /// On the other hand, if the <paramref name="message"/> is invalid but the <paramref name="exception"/> is valid 
        /// then the message text is taken from the exception instead.
        /// </para>
        /// <para>
        /// The logging scope is intended to be more than just the type name.
        /// </para>
        /// <list type="bullet">
        /// <item><description>
        /// If parameter <paramref name="scope"/> is for example of type <see cref="String"/> then this string is used.
        /// </description></item>
        /// <item><description>
        /// If parameter <paramref name="scope"/> is for example of type <see cref="Guid"/> then the guid's string representation 
        /// is taken by calling <see cref="Guid.ToString()"/>. Using a <see cref="Guid"/> as scope type can be seen as some kind 
        /// of <em>correlation ID</em>, especially if this <see cref="Guid"/> is the same for multiple calls. 
        /// </description></item>
        /// <item><description>
        /// If parameter <paramref name="scope"/> is for example of type <see cref="MemberInfo"/> then the referenced member's 
        /// <see cref="MemberInfo.Name"/> is taken instead. 
        /// </description></item>
        /// <item><description>
        /// In all other cases the scope text is either taken from the type's <see cref="Type.FullName"/> or it is just the 
        /// <see cref="MemberInfo.Name"/> of type <typeparamref name="TScope"/>. The usage of full or short name is determined 
        /// from current settings.
        /// </description></item>
        /// </list>
        /// </remarks>
        /// <typeparam name="TScope">
        /// The type to get the logging scope from.
        /// </typeparam>
        /// <param name="scope">
        /// The instance of the type to get the logging scope from.
        /// </param>
        /// <param name="level">
        /// The logging level to be used to tag the written logging data.
        /// </param>
        /// <param name="message">
        /// The message to be written.
        /// </param>
        /// <param name="exception">
        /// The exception to be written.
        /// </param>
        void Write<TScope>(TScope scope, LogLevel level, String message, Exception exception);

        /// <summary>
        /// This method writes the <paramref name="message"/> as well as the <paramref name="exception"/> for provided 
        /// <paramref name="scope"/> into the logging target using provided logging <paramref name="level"/>.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This method performs the actual writing of logging data into the logging target using a particular logging level. 
        /// Be aware, nothing will happen if <paramref name="message"/> is <c>null</c>, <em>empty</em> or consists only of 
        /// <em>whitespaces</em> and if <paramref name="exception"/> is <c>null</c>. 
        /// </para>
        /// <para>
        /// On the other hand, if the <paramref name="message"/> is invalid but the <paramref name="exception"/> is valid 
        /// then the message text is taken from the exception instead.
        /// </para>
        /// <para>
        /// The logging scope is intended to be more than just the type name.
        /// </para>
        /// <list type="bullet">
        /// <item><description>
        /// If parameter <paramref name="scope"/> is for example of type <see cref="String"/> then this string is used.
        /// </description></item>
        /// <item><description>
        /// If parameter <paramref name="scope"/> is for example of type <see cref="Guid"/> then the guid's string representation 
        /// is taken by calling <see cref="Guid.ToString()"/>. Using a <see cref="Guid"/> as scope type can be seen as some kind 
        /// of <em>correlation ID</em>, especially if this <see cref="Guid"/> is the same for multiple calls. 
        /// </description></item>
        /// <item><description>
        /// If parameter <paramref name="scope"/> is for example of type <see cref="MemberInfo"/> then the referenced member's 
        /// <see cref="MemberInfo.Name"/> is taken instead. 
        /// </description></item>
        /// <item><description>
        /// In all other cases the scope text is either taken from the type's <see cref="Type.FullName"/> or it is just the 
        /// <see cref="MemberInfo.Name"/> of type <typeparamref name="TScope"/>. The usage of full or short name is determined 
        /// from current settings.
        /// </description></item>
        /// </list>
        /// </remarks>
        /// <typeparam name="TScope">
        /// The type to get the logging scope from.
        /// </typeparam>
        /// <param name="scope">
        /// The instance of the type to get the logging scope from.
        /// </param>
        /// <param name="level">
        /// The logging level to be used to tag the written logging data.
        /// </param>
        /// <param name="message">
        /// The message to be written.
        /// </param>
        /// <param name="exception">
        /// The exception to be written.
        /// </param>
        /// <param name="details">
        /// An optional list of label-value-pair combinations containing additional information.
        /// </param>
        void Write<TScope>(TScope scope, LogLevel level, String message, Exception exception, params (String Label, Object Value)[] details);

        #endregion
    }
}
