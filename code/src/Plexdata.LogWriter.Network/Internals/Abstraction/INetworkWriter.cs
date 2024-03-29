﻿/*
 * MIT License
 * 
 * Copyright (c) 2023 plexdata.de
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

namespace Plexdata.LogWriter.Internals.Abstraction
{
    /// <summary>
    /// An interface allowing data transmission via network.
    /// </summary>
    /// <remarks>
    /// This interface represents the abstraction that allows 
    /// general data transmission via network.
    /// </remarks>
    /// <seealso cref="IUdpNetworkWriter"/>
    /// <seealso cref="ITcpNetworkWriter"/>
    /// <seealso cref="IWebNetworkWriter"/>
    internal interface INetworkWriter : IDisposable
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
        /// Writes provided message using underlying network connection.
        /// </summary>
        /// <remarks>
        /// this method writes provided message using underlying network 
        /// connection.
        /// </remarks>
        /// <param name="message">
        /// The message to write.
        /// </param>
        void Write(String message);
    }
}
