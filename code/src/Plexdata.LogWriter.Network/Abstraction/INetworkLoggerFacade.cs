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

namespace Plexdata.LogWriter.Abstraction
{
    /// <summary>
    /// This interface represents all actions needed to natively write messages 
    /// into a network sink.
    /// </summary>
    /// <remarks>
    /// The interface is required as an abstraction between the network logger 
    /// itself and the implementation that performs the actual writing of logging 
    /// message.
    /// becomes necessary.
    /// </remarks>
    public interface INetworkLoggerFacade : IDisposable
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
        /// Applies new settings.
        /// </summary>
        /// <remarks>
        /// <para>
        /// All settings must be reapplied as soon as at least one of them 
        /// changes and this is task of this method.
        /// </para>
        /// <para>
        /// Please note, applying new settings might change the underlying 
        /// logging message writer, from e.g. UTP to TCP.
        /// </para>
        /// </remarks>
        /// <param name="settings">
        /// The settings to be applied.
        /// </param>
        void ApplySettings(INetworkLoggerSettings settings);

        /// <summary>
        /// Writes a message out to the network using currently assigned 
        /// network writer.
        /// </summary>
        /// <remarks>
        /// This method writes a message out to the network using currently 
        /// assigned network writer.
        /// </remarks>
        /// <param name="message">
        /// The message to be written.
        /// </param>
        void Write(String message);
    }
}
