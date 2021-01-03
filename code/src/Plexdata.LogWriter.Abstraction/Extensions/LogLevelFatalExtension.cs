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

namespace Plexdata.LogWriter.Extensions
{
    /// <summary>
    /// This extension represents a set of convenience methods for an easier
    /// access of <see cref="LogLevel.Fatal"/> logging messages.
    /// </summary>
    /// <remarks>
    /// Internally, all these methods simply call the logger's write method
    /// by providing logging level <see cref="LogLevel.Fatal"/>.
    /// </remarks>
    /// <example>
    /// Code snippet below shows a fully qualified example of how to use logger 
    /// extension method <see cref="LogLevelFatalExtension.Fatal(ILogger, String)"/>.
    /// <code>
    /// using Plexdata.LogWriter.Abstraction;
    /// using Plexdata.LogWriter.Extensions;
    /// using System;
    /// 
    /// namespace Plexdata.LogWriter.Example
    /// {
    ///     public class SomeClass
    ///     {
    ///         private readonly ILogger logger;
    /// 
    ///         public SomeClass(ILogger logger)
    ///         {
    ///             this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
    ///         }
    /// 
    ///         public void SomeMethod()
    ///         {
    ///             this.logger.Fatal("This is a Fatal logging entry.");
    ///         }
    ///     }
    /// }
    /// </code>
    /// </example>
    public static class LogLevelFatalExtension
    {
        #region Fatal logging methods

        /// <summary>
        /// This method writes the <paramref name="message"/> into the <paramref name="logger"/> using <see cref="LogLevel.Fatal"/> 
        /// as logging level.
        /// </summary>
        /// <remarks>
        /// Keep in mind, nothing is gonna happen if <paramref name="logger"/> is <c>null</c> or if <paramref name="message"/> 
        /// is <c>null</c>, <c>empty</c> or consists only of white spaces. 
        /// </remarks>
        /// <param name="logger">
        /// The logger used to write the message.
        /// </param>
        /// <param name="message">
        /// The message to be written.
        /// </param>
        public static void Fatal(this ILogger logger, String message)
        {
            if (logger != null)
            {
                logger.Write(LogLevel.Fatal, message);
            }
        }

        /// <summary>
        /// This method writes the <paramref name="message"/> into the <paramref name="logger"/> using <see cref="LogLevel.Fatal"/> 
        /// as logging level.
        /// </summary>
        /// <remarks>
        /// Keep in mind, nothing is gonna happen if <paramref name="logger"/> is <c>null</c> or if <paramref name="message"/> 
        /// is <c>null</c>, <c>empty</c> or consists only of white spaces. 
        /// </remarks>
        /// <param name="logger">
        /// The logger used to write the message.
        /// </param>
        /// <param name="message">
        /// The message to be written.
        /// </param>
        /// <param name="details">
        /// An optional list of label-value-pair combinations containing additional information.
        /// </param>
        public static void Fatal(this ILogger logger, String message, params (String Label, Object Value)[] details)
        {
            if (logger != null)
            {
                logger.Write(LogLevel.Fatal, message, details);
            }
        }

        /// <summary>
        /// This method writes the <paramref name="message"/> for provided <paramref name="scope"/> into the <paramref name="logger"/> 
        /// using <see cref="LogLevel.Fatal"/> as logging level.
        /// </summary>
        /// <remarks>
        /// <para>
        /// The logging scope is intended to be more than just the type name. If type of parameter <paramref name="scope"/> 
        /// is for example a string then this string is used. If type of parameter <paramref name="scope"/> is for example of 
        /// type <see cref="System.Reflection.MemberInfo"/> then the referenced member's <see cref="System.Reflection.MemberInfo.Name"/> 
        /// is taken instead. In all other cases the scope text is either taken from the type's <see cref="Type.FullName"/> 
        /// or it is just the <see cref="System.Reflection.MemberInfo.Name"/> of type <typeparamref name="TScope"/>. The 
        /// usage of full or short name is determined from current settings.
        /// </para>
        /// <para>
        /// Keep in mind, nothing is gonna happen if <paramref name="logger"/> is <c>null</c> or if <paramref name="message"/> 
        /// is <c>null</c>, <c>empty</c> or consists only of white spaces. 
        /// </para>
        /// </remarks>
        /// <typeparam name="TScope">
        /// The type that describes the logging scope. 
        /// </typeparam>
        /// <param name="logger">
        /// The logger used to write the message.
        /// </param>
        /// <param name="scope">
        /// The logging scope instance to be used.
        /// </param>
        /// <param name="message">
        /// The message to be written.
        /// </param>
        public static void Fatal<TScope>(this ILogger logger, TScope scope, String message)
        {
            if (logger != null)
            {
                logger.Write(scope, LogLevel.Fatal, message);
            }
        }

        /// <summary>
        /// This method writes the <paramref name="message"/> for provided <paramref name="scope"/> into the <paramref name="logger"/> 
        /// using <see cref="LogLevel.Fatal"/> as logging level.
        /// </summary>
        /// <remarks>
        /// <para>
        /// The logging scope is intended to be more than just the type name. If type of parameter <paramref name="scope"/> 
        /// is for example a string then this string is used. If type of parameter <paramref name="scope"/> is for example of 
        /// type <see cref="System.Reflection.MemberInfo"/> then the referenced member's <see cref="System.Reflection.MemberInfo.Name"/> 
        /// is taken instead. In all other cases the scope text is either taken from the type's <see cref="Type.FullName"/> 
        /// or it is just the <see cref="System.Reflection.MemberInfo.Name"/> of type <typeparamref name="TScope"/>. The 
        /// usage of full or short name is determined from current settings.
        /// </para>
        /// <para>
        /// Keep in mind, nothing is gonna happen if <paramref name="logger"/> is <c>null</c> or if <paramref name="message"/> 
        /// is <c>null</c>, <c>empty</c> or consists only of white spaces. 
        /// </para>
        /// </remarks>
        /// <typeparam name="TScope">
        /// The type that describes the logging scope. 
        /// </typeparam>
        /// <param name="logger">
        /// The logger used to write the message.
        /// </param>
        /// <param name="scope">
        /// The logging scope instance to be used.
        /// </param>
        /// <param name="message">
        /// The message to be written.
        /// </param>
        /// <param name="details">
        /// An optional list of label-value-pair combinations containing additional information.
        /// </param>
        public static void Fatal<TScope>(this ILogger logger, TScope scope, String message, params (String Label, Object Value)[] details)
        {
            if (logger != null)
            {
                logger.Write(scope, LogLevel.Fatal, message, details);
            }
        }

        /// <summary>
        /// This method writes the <paramref name="message"/> for current <typeparamref name="TContext"/> into the 
        /// <paramref name="logger"/> using <see cref="LogLevel.Fatal"/> as logging level.
        /// </summary>
        /// <remarks>
        /// Keep in mind, nothing is gonna happen if <paramref name="logger"/> is <c>null</c> or if <paramref name="message"/> 
        /// is <c>null</c>, <c>empty</c> or consists only of white spaces. 
        /// </remarks>
        /// <typeparam name="TContext">
        /// The type to get the logging context from. Such a context might be a class and shall help to find out the source 
        /// that causes a particular logging message.
        /// </typeparam>
        /// <param name="logger">
        /// The logger used to write the message.
        /// </param>
        /// <param name="message">
        /// The message to be written.
        /// </param>
        public static void Fatal<TContext>(this ILogger<TContext> logger, String message)
        {
            if (logger != null)
            {
                logger.Write(LogLevel.Fatal, message);
            }
        }

        /// <summary>
        /// This method writes the <paramref name="message"/> for current <typeparamref name="TContext"/> into the 
        /// <paramref name="logger"/> using <see cref="LogLevel.Fatal"/> as logging level.
        /// </summary>
        /// <remarks>
        /// Keep in mind, nothing is gonna happen if <paramref name="logger"/> is <c>null</c> or if <paramref name="message"/> 
        /// is <c>null</c>, <c>empty</c> or consists only of white spaces. 
        /// </remarks>
        /// <typeparam name="TContext">
        /// The type to get the logging context from. Such a context might be a class and shall help to find out the source 
        /// that causes a particular logging message.
        /// </typeparam>
        /// <param name="logger">
        /// The logger used to write the message.
        /// </param>
        /// <param name="message">
        /// The message to be written.
        /// </param>
        /// <param name="details">
        /// An optional list of label-value-pair combinations containing additional information.
        /// </param>
        public static void Fatal<TContext>(this ILogger<TContext> logger, String message, params (String Label, Object Value)[] details)
        {
            if (logger != null)
            {
                logger.Write(LogLevel.Fatal, message, details);
            }
        }

        /// <summary>
        /// This method writes the <paramref name="message"/> for current <typeparamref name="TContext"/> taking provided 
        /// <paramref name="scope"/> into the <paramref name="logger"/> using <see cref="LogLevel.Fatal"/> as logging level.
        /// </summary>
        /// <remarks>
        /// <para>
        /// The logging context is intended to be some kind of message grouping. Against this background, the context can 
        /// be used to identify the source (for example a class) of a logging message. In combination with the message scope 
        /// it would become possible to uniquely identify the caller of a logging message.
        /// </para>
        /// <para>
        /// The logging scope is intended to be more than just the type name. If type of parameter <paramref name="scope"/> 
        /// is for example a string then this string is used. If type of parameter <paramref name="scope"/> is for example of 
        /// type <see cref="System.Reflection.MemberInfo"/> then the referenced member's <see cref="System.Reflection.MemberInfo.Name"/> 
        /// is taken instead. In all other cases the scope text is either taken from the type's <see cref="Type.FullName"/> 
        /// or it is just the <see cref="System.Reflection.MemberInfo.Name"/> of type <typeparamref name="TScope"/>. The 
        /// usage of full or short name is determined from current settings.
        /// </para>
        /// <para>
        /// Keep in mind, nothing is gonna happen if <paramref name="logger"/> is <c>null</c> or if <paramref name="message"/> 
        /// is <c>null</c>, <c>empty</c> or consists only of white spaces. 
        /// </para>
        /// </remarks>
        /// <typeparam name="TContext">
        /// The type to get the logging context from. Such a context might be a class and shall help to find out the source 
        /// that causes a particular logging message.
        /// </typeparam>
        /// <typeparam name="TScope">
        /// The type that describes the logging scope. 
        /// </typeparam>
        /// <param name="logger">
        /// The logger used to write the message.
        /// </param>
        /// <param name="scope">
        /// The logging scope instance to be used.
        /// </param>
        /// <param name="message">
        /// The message to be written.
        /// </param>
        public static void Fatal<TContext, TScope>(this ILogger<TContext> logger, TScope scope, String message)
        {
            if (logger != null)
            {
                logger.Write(scope, LogLevel.Fatal, message);
            }
        }

        /// <summary>
        /// This method writes the <paramref name="message"/> for current <typeparamref name="TContext"/> taking provided 
        /// <paramref name="scope"/> into the <paramref name="logger"/> using <see cref="LogLevel.Fatal"/> as logging level.
        /// </summary>
        /// <remarks>
        /// <para>
        /// The logging context is intended to be some kind of message grouping. Against this background, the context can 
        /// be used to identify the source (for example a class) of a logging message. In combination with the message scope 
        /// it would become possible to uniquely identify the caller of a logging message.
        /// </para>
        /// <para>
        /// The logging scope is intended to be more than just the type name. If type of parameter <paramref name="scope"/> 
        /// is for example a string then this string is used. If type of parameter <paramref name="scope"/> is for example of 
        /// type <see cref="System.Reflection.MemberInfo"/> then the referenced member's <see cref="System.Reflection.MemberInfo.Name"/> 
        /// is taken instead. In all other cases the scope text is either taken from the type's <see cref="Type.FullName"/> 
        /// or it is just the <see cref="System.Reflection.MemberInfo.Name"/> of type <typeparamref name="TScope"/>. The 
        /// usage of full or short name is determined from current settings.
        /// </para>
        /// <para>
        /// Keep in mind, nothing is gonna happen if <paramref name="logger"/> is <c>null</c> or if <paramref name="message"/> 
        /// is <c>null</c>, <c>empty</c> or consists only of white spaces. 
        /// </para>
        /// </remarks>
        /// <typeparam name="TContext">
        /// The type to get the logging context from. Such a context might be a class and shall help to find out the source 
        /// that causes a particular logging message.
        /// </typeparam>
        /// <typeparam name="TScope">
        /// The type that describes the logging scope. 
        /// </typeparam>
        /// <param name="logger">
        /// The logger used to write the message.
        /// </param>
        /// <param name="scope">
        /// The logging scope instance to be used.
        /// </param>
        /// <param name="message">
        /// The message to be written.
        /// </param>
        /// <param name="details">
        /// An optional list of label-value-pair combinations containing additional information.
        /// </param>
        public static void Fatal<TContext, TScope>(this ILogger<TContext> logger, TScope scope, String message, params (String Label, Object Value)[] details)
        {
            if (logger != null)
            {
                logger.Write(scope, LogLevel.Fatal, message, details);
            }
        }

        /// <summary>
        /// This method writes the <paramref name="exception"/> into the <paramref name="logger"/> using <see cref="LogLevel.Fatal"/> 
        /// as logging level.
        /// </summary>
        /// <remarks>
        /// Keep in mind, nothing is gonna happen if <paramref name="logger"/> is <c>null</c> or if <paramref name="exception"/> 
        /// is <c>null</c>. 
        /// </remarks>
        /// <param name="logger">
        /// The logger used to write the message.
        /// </param>
        /// <param name="exception">
        /// The message to be written.
        /// </param>
        public static void Fatal(this ILogger logger, Exception exception)
        {
            if (logger != null)
            {
                logger.Write(LogLevel.Fatal, exception);
            }
        }

        /// <summary>
        /// This method writes the <paramref name="exception"/> into the <paramref name="logger"/> using <see cref="LogLevel.Fatal"/> 
        /// as logging level.
        /// </summary>
        /// <remarks>
        /// Keep in mind, nothing is gonna happen if <paramref name="logger"/> is <c>null</c> or if <paramref name="exception"/> 
        /// is <c>null</c>. 
        /// </remarks>
        /// <param name="logger">
        /// The logger used to write the message.
        /// </param>
        /// <param name="exception">
        /// The message to be written.
        /// </param>
        /// <param name="details">
        /// An optional list of label-value-pair combinations containing additional information.
        /// </param>
        public static void Fatal(this ILogger logger, Exception exception, params (String Label, Object Value)[] details)
        {
            if (logger != null)
            {
                logger.Write(LogLevel.Fatal, exception, details);
            }
        }

        /// <summary>
        /// This method writes the <paramref name="exception"/> for provided <paramref name="scope"/> into the <paramref name="logger"/> 
        /// using <see cref="LogLevel.Fatal"/> as logging level.
        /// </summary>
        /// <remarks>
        /// <para>
        /// The logging scope is intended to be more than just the type name. If type of parameter <paramref name="scope"/> 
        /// is for example a string then this string is used. If type of parameter <paramref name="scope"/> is for example of 
        /// type <see cref="System.Reflection.MemberInfo"/> then the referenced member's <see cref="System.Reflection.MemberInfo.Name"/> 
        /// is taken instead. In all other cases the scope text is either taken from the type's <see cref="Type.FullName"/> 
        /// or it is just the <see cref="System.Reflection.MemberInfo.Name"/> of type <typeparamref name="TScope"/>. The 
        /// usage of full or short name is determined from current settings.
        /// </para>
        /// <para>
        /// Keep in mind, nothing is gonna happen if <paramref name="logger"/> is <c>null</c> or if <paramref name="exception"/> 
        /// is <c>null</c>. 
        /// </para>
        /// </remarks>
        /// <typeparam name="TScope">
        /// The type that describes the logging scope. 
        /// </typeparam>
        /// <param name="logger">
        /// The logger used to write the message.
        /// </param>
        /// <param name="scope">
        /// The logging scope instance to be used.
        /// </param>
        /// <param name="exception">
        /// The exception to be written.
        /// </param>
        public static void Fatal<TScope>(this ILogger logger, TScope scope, Exception exception)
        {
            if (logger != null)
            {
                logger.Write(scope, LogLevel.Fatal, exception);
            }
        }

        /// <summary>
        /// This method writes the <paramref name="exception"/> for provided <paramref name="scope"/> into the <paramref name="logger"/> 
        /// using <see cref="LogLevel.Fatal"/> as logging level.
        /// </summary>
        /// <remarks>
        /// <para>
        /// The logging scope is intended to be more than just the type name. If type of parameter <paramref name="scope"/> 
        /// is for example a string then this string is used. If type of parameter <paramref name="scope"/> is for example of 
        /// type <see cref="System.Reflection.MemberInfo"/> then the referenced member's <see cref="System.Reflection.MemberInfo.Name"/> 
        /// is taken instead. In all other cases the scope text is either taken from the type's <see cref="Type.FullName"/> 
        /// or it is just the <see cref="System.Reflection.MemberInfo.Name"/> of type <typeparamref name="TScope"/>. The 
        /// usage of full or short name is determined from current settings.
        /// </para>
        /// <para>
        /// Keep in mind, nothing is gonna happen if <paramref name="logger"/> is <c>null</c> or if <paramref name="exception"/> 
        /// is <c>null</c>. 
        /// </para>
        /// </remarks>
        /// <typeparam name="TScope">
        /// The type that describes the logging scope. 
        /// </typeparam>
        /// <param name="logger">
        /// The logger used to write the message.
        /// </param>
        /// <param name="scope">
        /// The logging scope instance to be used.
        /// </param>
        /// <param name="exception">
        /// The exception to be written.
        /// </param>
        /// <param name="details">
        /// An optional list of label-value-pair combinations containing additional information.
        /// </param>
        public static void Fatal<TScope>(this ILogger logger, TScope scope, Exception exception, params (String Label, Object Value)[] details)
        {
            if (logger != null)
            {
                logger.Write(scope, LogLevel.Fatal, exception, details);
            }
        }

        /// <summary>
        /// This method writes the <paramref name="exception"/> for current <typeparamref name="TContext"/> into the 
        /// <paramref name="logger"/> using <see cref="LogLevel.Fatal"/> as logging level.
        /// </summary>
        /// <remarks>
        /// Keep in mind, nothing is gonna happen if <paramref name="logger"/> is <c>null</c> or if <paramref name="exception"/> 
        /// is <c>null</c>. 
        /// </remarks>
        /// <typeparam name="TContext">
        /// The type to get the logging context from. Such a context might be a class and shall help to find out the source 
        /// that causes a particular logging message.
        /// </typeparam>
        /// <param name="logger">
        /// The logger used to write the message.
        /// </param>
        /// <param name="exception">
        /// The exception to be written.
        /// </param>
        public static void Fatal<TContext>(this ILogger<TContext> logger, Exception exception)
        {
            if (logger != null)
            {
                logger.Write(LogLevel.Fatal, exception);
            }
        }

        /// <summary>
        /// This method writes the <paramref name="exception"/> for current <typeparamref name="TContext"/> into the 
        /// <paramref name="logger"/> using <see cref="LogLevel.Fatal"/> as logging level.
        /// </summary>
        /// <remarks>
        /// Keep in mind, nothing is gonna happen if <paramref name="logger"/> is <c>null</c> or if <paramref name="exception"/> 
        /// is <c>null</c>. 
        /// </remarks>
        /// <typeparam name="TContext">
        /// The type to get the logging context from. Such a context might be a class and shall help to find out the source 
        /// that causes a particular logging message.
        /// </typeparam>
        /// <param name="logger">
        /// The logger used to write the message.
        /// </param>
        /// <param name="exception">
        /// The exception to be written.
        /// </param>
        /// <param name="details">
        /// An optional list of label-value-pair combinations containing additional information.
        /// </param>
        public static void Fatal<TContext>(this ILogger<TContext> logger, Exception exception, params (String Label, Object Value)[] details)
        {
            if (logger != null)
            {
                logger.Write(LogLevel.Fatal, exception, details);
            }
        }

        /// <summary>
        /// This method writes the <paramref name="exception"/> for current <typeparamref name="TContext"/> taking provided 
        /// <paramref name="scope"/> into the <paramref name="logger"/> using <see cref="LogLevel.Fatal"/> as logging level.
        /// </summary>
        /// <remarks>
        /// <para>
        /// The logging context is intended to be some kind of message grouping. Against this background, the context can 
        /// be used to identify the source (for example a class) of a logging message. In combination with the message scope 
        /// it would become possible to uniquely identify the caller of a logging message.
        /// </para>
        /// <para>
        /// The logging scope is intended to be more than just the type name. If type of parameter <paramref name="scope"/> 
        /// is for example a string then this string is used. If type of parameter <paramref name="scope"/> is for example of 
        /// type <see cref="System.Reflection.MemberInfo"/> then the referenced member's <see cref="System.Reflection.MemberInfo.Name"/> 
        /// is taken instead. In all other cases the scope text is either taken from the type's <see cref="Type.FullName"/> 
        /// or it is just the <see cref="System.Reflection.MemberInfo.Name"/> of type <typeparamref name="TScope"/>. The 
        /// usage of full or short name is determined from current settings.
        /// </para>
        /// <para>
        /// Keep in mind, nothing is gonna happen if <paramref name="logger"/> is <c>null</c> or if <paramref name="exception"/> 
        /// is <c>null</c>. 
        /// </para>
        /// </remarks>
        /// <typeparam name="TContext">
        /// The type to get the logging context from. Such a context might be a class and shall help to find out the source 
        /// that causes a particular logging message.
        /// </typeparam>
        /// <typeparam name="TScope">
        /// The type that describes the logging scope. 
        /// </typeparam>
        /// <param name="logger">
        /// The logger used to write the message.
        /// </param>
        /// <param name="scope">
        /// The logging scope instance to be used.
        /// </param>
        /// <param name="exception">
        /// The exception to be written.
        /// </param>
        public static void Fatal<TContext, TScope>(this ILogger<TContext> logger, TScope scope, Exception exception)
        {
            if (logger != null)
            {
                logger.Write(scope, LogLevel.Fatal, exception);
            }
        }

        /// <summary>
        /// This method writes the <paramref name="exception"/> for current <typeparamref name="TContext"/> taking provided 
        /// <paramref name="scope"/> into the <paramref name="logger"/> using <see cref="LogLevel.Fatal"/> as logging level.
        /// </summary>
        /// <remarks>
        /// <para>
        /// The logging context is intended to be some kind of message grouping. Against this background, the context can 
        /// be used to identify the source (for example a class) of a logging message. In combination with the message scope 
        /// it would become possible to uniquely identify the caller of a logging message.
        /// </para>
        /// <para>
        /// The logging scope is intended to be more than just the type name. If type of parameter <paramref name="scope"/> 
        /// is for example a string then this string is used. If type of parameter <paramref name="scope"/> is for example of 
        /// type <see cref="System.Reflection.MemberInfo"/> then the referenced member's <see cref="System.Reflection.MemberInfo.Name"/> 
        /// is taken instead. In all other cases the scope text is either taken from the type's <see cref="Type.FullName"/> 
        /// or it is just the <see cref="System.Reflection.MemberInfo.Name"/> of type <typeparamref name="TScope"/>. The 
        /// usage of full or short name is determined from current settings.
        /// </para>
        /// <para>
        /// Keep in mind, nothing is gonna happen if <paramref name="logger"/> is <c>null</c> or if <paramref name="exception"/> 
        /// is <c>null</c>. 
        /// </para>
        /// </remarks>
        /// <typeparam name="TContext">
        /// The type to get the logging context from. Such a context might be a class and shall help to find out the source 
        /// that causes a particular logging message.
        /// </typeparam>
        /// <typeparam name="TScope">
        /// The type that describes the logging scope. 
        /// </typeparam>
        /// <param name="logger">
        /// The logger used to write the message.
        /// </param>
        /// <param name="scope">
        /// The logging scope instance to be used.
        /// </param>
        /// <param name="exception">
        /// The exception to be written.
        /// </param>
        /// <param name="details">
        /// An optional list of label-value-pair combinations containing additional information.
        /// </param>
        public static void Fatal<TContext, TScope>(this ILogger<TContext> logger, TScope scope, Exception exception, params (String Label, Object Value)[] details)
        {
            if (logger != null)
            {
                logger.Write(scope, LogLevel.Fatal, exception, details);
            }
        }

        /// <summary>
        /// This method writes the <paramref name="message"/> and the <paramref name="exception"/> into the <paramref name="logger"/> 
        /// using <see cref="LogLevel.Fatal"/> as logging level.
        /// </summary>
        /// <remarks>
        /// Keep in mind, nothing is gonna happen if <paramref name="logger"/> is <c>null</c> or if <paramref name="message"/> 
        /// is <c>null</c>, <c>empty</c> or consists only of white spaces or if <paramref name="exception"/> is <c>null</c>.
        /// In case of the <paramref name="message"/> is invalid but the <paramref name="exception"/> is valid, the message text 
        /// is taken from the exception instead.
        /// </remarks>
        /// <param name="logger">
        /// The logger used to write the message.
        /// </param>
        /// <param name="message">
        /// The message to be written.
        /// </param>
        /// <param name="exception">
        /// The exception to be written.
        /// </param>
        public static void Fatal(this ILogger logger, String message, Exception exception)
        {
            if (logger != null)
            {
                logger.Write(LogLevel.Fatal, message, exception);
            }
        }

        /// <summary>
        /// This method writes the <paramref name="message"/> and the <paramref name="exception"/> into the <paramref name="logger"/> 
        /// using <see cref="LogLevel.Fatal"/> as logging level.
        /// </summary>
        /// <remarks>
        /// Keep in mind, nothing is gonna happen if <paramref name="logger"/> is <c>null</c> or if <paramref name="message"/> 
        /// is <c>null</c>, <c>empty</c> or consists only of white spaces or if <paramref name="exception"/> is <c>null</c>.
        /// In case of the <paramref name="message"/> is invalid but the <paramref name="exception"/> is valid, the message text 
        /// is taken from the exception instead.
        /// </remarks>
        /// <param name="logger">
        /// The logger used to write the message.
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
        public static void Fatal(this ILogger logger, String message, Exception exception, params (String Label, Object Value)[] details)
        {
            if (logger != null)
            {
                logger.Write(LogLevel.Fatal, message, exception, details);
            }
        }

        /// <summary>
        /// This method writes the <paramref name="message"/> and the <paramref name="exception"/> for provided <paramref name="scope"/> 
        /// into the <paramref name="logger"/> using <see cref="LogLevel.Fatal"/> as logging level.
        /// </summary>
        /// <remarks>
        /// <para>
        /// The logging scope is intended to be more than just the type name. If type of parameter <paramref name="scope"/> 
        /// is for example a string then this string is used. If type of parameter <paramref name="scope"/> is for example of 
        /// type <see cref="System.Reflection.MemberInfo"/> then the referenced member's <see cref="System.Reflection.MemberInfo.Name"/> 
        /// is taken instead. In all other cases the scope text is either taken from the type's <see cref="Type.FullName"/> 
        /// or it is just the <see cref="System.Reflection.MemberInfo.Name"/> of type <typeparamref name="TScope"/>. The 
        /// usage of full or short name is determined from current settings.
        /// </para>
        /// <para>
        /// Keep in mind, nothing is gonna happen if <paramref name="logger"/> is <c>null</c> or if <paramref name="message"/> 
        /// is <c>null</c>, <c>empty</c> or consists only of white spaces or if <paramref name="exception"/> is <c>null</c>.
        /// In case of the <paramref name="message"/> is invalid but the <paramref name="exception"/> is valid, the message text 
        /// is taken from the exception instead.
        /// </para>
        /// </remarks>
        /// <typeparam name="TScope">
        /// The type that describes the logging scope. 
        /// </typeparam>
        /// <param name="logger">
        /// The logger used to write the message.
        /// </param>
        /// <param name="scope">
        /// The logging scope instance to be used.
        /// </param>
        /// <param name="message">
        /// The message to be written.
        /// </param>
        /// <param name="exception">
        /// The exception to be written.
        /// </param>
        public static void Fatal<TScope>(this ILogger logger, TScope scope, String message, Exception exception)
        {
            if (logger != null)
            {
                logger.Write(scope, LogLevel.Fatal, message, exception);
            }
        }

        /// <summary>
        /// This method writes the <paramref name="message"/> and the <paramref name="exception"/> for provided <paramref name="scope"/> 
        /// into the <paramref name="logger"/> using <see cref="LogLevel.Fatal"/> as logging level.
        /// </summary>
        /// <remarks>
        /// <para>
        /// The logging scope is intended to be more than just the type name. If type of parameter <paramref name="scope"/> 
        /// is for example a string then this string is used. If type of parameter <paramref name="scope"/> is for example of 
        /// type <see cref="System.Reflection.MemberInfo"/> then the referenced member's <see cref="System.Reflection.MemberInfo.Name"/> 
        /// is taken instead. In all other cases the scope text is either taken from the type's <see cref="Type.FullName"/> 
        /// or it is just the <see cref="System.Reflection.MemberInfo.Name"/> of type <typeparamref name="TScope"/>. The 
        /// usage of full or short name is determined from current settings.
        /// </para>
        /// <para>
        /// Keep in mind, nothing is gonna happen if <paramref name="logger"/> is <c>null</c> or if <paramref name="message"/> 
        /// is <c>null</c>, <c>empty</c> or consists only of white spaces or if <paramref name="exception"/> is <c>null</c>.
        /// In case of the <paramref name="message"/> is invalid but the <paramref name="exception"/> is valid, the message text 
        /// is taken from the exception instead.
        /// </para>
        /// </remarks>
        /// <typeparam name="TScope">
        /// The type that describes the logging scope. 
        /// </typeparam>
        /// <param name="logger">
        /// The logger used to write the message.
        /// </param>
        /// <param name="scope">
        /// The logging scope instance to be used.
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
        public static void Fatal<TScope>(this ILogger logger, TScope scope, String message, Exception exception, params (String Label, Object Value)[] details)
        {
            if (logger != null)
            {
                logger.Write(scope, LogLevel.Fatal, message, exception, details);
            }
        }

        /// <summary>
        /// This method writes the <paramref name="message"/> and the <paramref name="exception"/> for current <typeparamref name="TContext"/> 
        /// into the <paramref name="logger"/> using <see cref="LogLevel.Fatal"/> as logging level.
        /// </summary>
        /// <remarks>
        /// Keep in mind, nothing is gonna happen if <paramref name="logger"/> is <c>null</c> or if <paramref name="message"/> 
        /// is <c>null</c>, <c>empty</c> or consists only of white spaces or if <paramref name="exception"/> is <c>null</c>.
        /// In case of the <paramref name="message"/> is invalid but the <paramref name="exception"/> is valid, the message text 
        /// is taken from the exception instead.
        /// </remarks>
        /// <typeparam name="TContext">
        /// The type to get the logging context from. Such a context might be a class and shall help to find out the source 
        /// that causes a particular logging message.
        /// </typeparam>
        /// <param name="logger">
        /// The logger used to write the message.
        /// </param>
        /// <param name="message">
        /// The message to be written.
        /// </param>
        /// <param name="exception">
        /// The exception to be written.
        /// </param>
        public static void Fatal<TContext>(this ILogger<TContext> logger, String message, Exception exception)
        {
            if (logger != null)
            {
                logger.Write(LogLevel.Fatal, message, exception);
            }
        }

        /// <summary>
        /// This method writes the <paramref name="message"/> and the <paramref name="exception"/> for current <typeparamref name="TContext"/> 
        /// into the <paramref name="logger"/> using <see cref="LogLevel.Fatal"/> as logging level.
        /// </summary>
        /// <remarks>
        /// Keep in mind, nothing is gonna happen if <paramref name="logger"/> is <c>null</c> or if <paramref name="message"/> 
        /// is <c>null</c>, <c>empty</c> or consists only of white spaces or if <paramref name="exception"/> is <c>null</c>.
        /// In case of the <paramref name="message"/> is invalid but the <paramref name="exception"/> is valid, the message text 
        /// is taken from the exception instead.
        /// </remarks>
        /// <typeparam name="TContext">
        /// The type to get the logging context from. Such a context might be a class and shall help to find out the source 
        /// that causes a particular logging message.
        /// </typeparam>
        /// <param name="logger">
        /// The logger used to write the message.
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
        public static void Fatal<TContext>(this ILogger<TContext> logger, String message, Exception exception, params (String Label, Object Value)[] details)
        {
            if (logger != null)
            {
                logger.Write(LogLevel.Fatal, message, exception, details);
            }
        }

        /// <summary>
        /// This method writes the <paramref name="message"/> and the <paramref name="exception"/> for current <typeparamref name="TContext"/> 
        /// taking provided <paramref name="scope"/> into the <paramref name="logger"/> using <see cref="LogLevel.Fatal"/> as logging level.
        /// </summary>
        /// <remarks>
        /// <para>
        /// The logging context is intended to be some kind of message grouping. Against this background, the context can 
        /// be used to identify the source (for example a class) of a logging message. In combination with the message scope 
        /// it would become possible to uniquely identify the caller of a logging message.
        /// </para>
        /// <para>
        /// The logging scope is intended to be more than just the type name. If type of parameter <paramref name="scope"/> 
        /// is for example a string then this string is used. If type of parameter <paramref name="scope"/> is for example of 
        /// type <see cref="System.Reflection.MemberInfo"/> then the referenced member's <see cref="System.Reflection.MemberInfo.Name"/> 
        /// is taken instead. In all other cases the scope text is either taken from the type's <see cref="Type.FullName"/> 
        /// or it is just the <see cref="System.Reflection.MemberInfo.Name"/> of type <typeparamref name="TScope"/>. The 
        /// usage of full or short name is determined from current settings.
        /// </para>
        /// <para>
        /// Keep in mind, nothing is gonna happen if <paramref name="logger"/> is <c>null</c> or if <paramref name="message"/> 
        /// is <c>null</c>, <c>empty</c> or consists only of white spaces or if <paramref name="exception"/> is <c>null</c>.
        /// In case of the <paramref name="message"/> is invalid but the <paramref name="exception"/> is valid, the message text 
        /// is taken from the exception instead.
        /// </para>
        /// </remarks>
        /// <typeparam name="TContext">
        /// The type to get the logging context from. Such a context might be a class and shall help to find out the source 
        /// that causes a particular logging message.
        /// </typeparam>
        /// <typeparam name="TScope">
        /// The type that describes the logging scope. 
        /// </typeparam>
        /// <param name="logger">
        /// The logger used to write the message.
        /// </param>
        /// <param name="scope">
        /// The logging scope instance to be used.
        /// </param>
        /// <param name="message">
        /// The message to be written.
        /// </param>
        /// <param name="exception">
        /// The exception to be written.
        /// </param>
        public static void Fatal<TContext, TScope>(this ILogger<TContext> logger, TScope scope, String message, Exception exception)
        {
            if (logger != null)
            {
                logger.Write(scope, LogLevel.Fatal, message, exception);
            }
        }

        /// <summary>
        /// This method writes the <paramref name="message"/> and the <paramref name="exception"/> for current <typeparamref name="TContext"/> 
        /// taking provided <paramref name="scope"/> into the <paramref name="logger"/> using <see cref="LogLevel.Fatal"/> as logging level.
        /// </summary>
        /// <remarks>
        /// <para>
        /// The logging context is intended to be some kind of message grouping. Against this background, the context can 
        /// be used to identify the source (for example a class) of a logging message. In combination with the message scope 
        /// it would become possible to uniquely identify the caller of a logging message.
        /// </para>
        /// <para>
        /// The logging scope is intended to be more than just the type name. If type of parameter <paramref name="scope"/> 
        /// is for example a string then this string is used. If type of parameter <paramref name="scope"/> is for example of 
        /// type <see cref="System.Reflection.MemberInfo"/> then the referenced member's <see cref="System.Reflection.MemberInfo.Name"/> 
        /// is taken instead. In all other cases the scope text is either taken from the type's <see cref="Type.FullName"/> 
        /// or it is just the <see cref="System.Reflection.MemberInfo.Name"/> of type <typeparamref name="TScope"/>. The 
        /// usage of full or short name is determined from current settings.
        /// </para>
        /// <para>
        /// Keep in mind, nothing is gonna happen if <paramref name="logger"/> is <c>null</c> or if <paramref name="message"/> 
        /// is <c>null</c>, <c>empty</c> or consists only of white spaces or if <paramref name="exception"/> is <c>null</c>.
        /// In case of the <paramref name="message"/> is invalid but the <paramref name="exception"/> is valid, the message text 
        /// is taken from the exception instead.
        /// </para>
        /// </remarks>
        /// <typeparam name="TContext">
        /// The type to get the logging context from. Such a context might be a class and shall help to find out the source 
        /// that causes a particular logging message.
        /// </typeparam>
        /// <typeparam name="TScope">
        /// The type that describes the logging scope. 
        /// </typeparam>
        /// <param name="logger">
        /// The logger used to write the message.
        /// </param>
        /// <param name="scope">
        /// The logging scope instance to be used.
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
        public static void Fatal<TContext, TScope>(this ILogger<TContext> logger, TScope scope, String message, Exception exception, params (String Label, Object Value)[] details)
        {
            if (logger != null)
            {
                logger.Write(scope, LogLevel.Fatal, message, exception, details);
            }
        }

        #endregion
    }
}
