/*
 * MIT License
 * 
 * Copyright (c) 2022 plexdata.de
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

namespace Plexdata.LogWriter.Abstraction
{
    /// <summary>
    /// This interface represents a concretization of the general logging interface. 
    /// </summary>
    /// <remarks>
    /// The interface is derived from interface ILogger and is used especially for 
    /// composite logging.
    /// </remarks>
    /// <typeparam name="TContext">
    /// The type to get the logging context from. Such a context might be a class and 
    /// shall help to find out the source that causes a particular logging message.
    /// </typeparam>
    public interface ICompositeLogger<TContext> : ILogger<TContext>, IDisposable
    {
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
        Boolean IsDisposed { get; }

        /// <summary>
        /// Adds a logger instance to the internal list of assigned loggers.
        /// </summary>
        /// <remarks>
        /// This method adds provided logger instance to the internal list 
        /// of assigned loggers.
        /// </remarks>
        /// <param name="logger">
        /// The logger to be added.
        /// </param>
        void AddLogger(ILogger<TContext> logger);
    }
}
