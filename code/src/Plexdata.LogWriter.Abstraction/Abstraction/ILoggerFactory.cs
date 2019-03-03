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

using Plexdata.LogWriter.Definitions;
using System;

namespace Plexdata.LogWriter.Abstraction
{
    /// <summary>
    /// This interface provides methods that allow to create 
    /// implementation independent instances of various logging 
    /// library types.
    /// </summary>
    /// <remarks>
    /// <para>
    /// For the moment this interface allows to create instances of 
    /// <see cref="ILogEvent"/> and <see cref="ILogEventFormatter"/> 
    /// derived classes.
    /// </para>
    /// <para>
    /// Actually, this interface is only necessary to share general 
    /// logging independent instances between all other logging libraries
    /// In other words, usually users do not get in contact with this 
    /// factory.
    /// </para>
    /// </remarks>
    public interface ILoggerFactory
    {
        /// <summary>
        /// This method create an instance of <see cref="ILogEvent"/>.
        /// </summary>
        /// <remarks>
        /// This factory method allows creating instances of classes 
        /// that implement the interface <see cref="ILogEvent"/>. 
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
        /// <returns>
        /// An instance of a logging event.
        /// </returns>
        ILogEvent CreateLogEvent(LogLevel level, DateTime time, String context, String scope, String message, Exception exception, params (String Label, Object Value)[] details);

        /// <summary>
        /// This method create an instance of <see cref="ILogEventFormatter"/>.
        /// </summary>
        /// <remarks>
        /// This factory method allows creating instances of classes 
        /// that implement the interface <see cref="ILogEventFormatter"/>. 
        /// </remarks>
        /// <param name="settings">
        /// An instance of a class implementing interface <see cref="ILoggerSettings"/>.
        /// </param>
        /// <returns>
        /// An instance of a logging event formatter.
        /// </returns>
        ILogEventFormatter CreateLogEventFormatter(ILoggerSettings settings);
    }
}
