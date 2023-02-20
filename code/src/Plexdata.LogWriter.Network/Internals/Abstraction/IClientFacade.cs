/*
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
    /// An interface to allow physical data transmission.
    /// </summary>
    /// <remarks>
    /// This interface represents the abstraction of a 
    /// facade of a WEB/HTTP client.
    /// </remarks>
    internal interface IClientFacade : IDisposable
    {
        /// <summary>
        /// Sends provided <paramref name="payload"/> using 
        /// underlying WEB/HTTP client.
        /// </summary>
        /// <remarks>
        /// This method sends provided <paramref name="payload"/> 
        /// using underlying WEB/HTTP client.
        /// </remarks>
        /// <param name="payload">
        /// The payload to send.
        /// </param>
        /// <returns>
        /// The number of transmitted bytes or zero in case of 
        /// any error.
        /// </returns>
        /// <seealso cref="System.Net.HttpWebRequest"/>
        Int32 Send(Byte[] payload);
    }
}
