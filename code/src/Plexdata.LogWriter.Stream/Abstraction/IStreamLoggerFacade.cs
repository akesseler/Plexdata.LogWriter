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
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Plexdata.LogWriter.Abstraction
{
    /// <summary>
    /// This interface represents all actions needed to natively write messages 
    /// into a logging stream.
    /// </summary>
    /// <remarks>
    /// The interface is required as an abstraction between the stream logger 
    /// itself and the implementation of the native stream writing. This interface 
    /// might be re-implemented if a different access to the native stream writing 
    /// becomes necessary.
    /// </remarks>
    public interface IStreamLoggerFacade
    {
        /// <summary>
        /// Writes one message into provided stream using provided encoding.
        /// </summary>
        /// <remarks>
        /// This method writes one single message into provided stream. The 
        /// message writing takes place by using provided encoding.
        /// </remarks>
        /// <param name="stream">
        /// The stream to write into.
        /// </param>
        /// <param name="encoding">
        /// The encoding to be used to write the message.
        /// </param>
        /// <param name="message">
        /// The message to write.
        /// </param>
        /// <returns>
        /// True is returned if message writing was successful and false if not.
        /// </returns>
        Boolean Write(Stream stream, Encoding encoding, String message);

        /// <summary>
        /// Writes a list of messages at once into provided stream using provided 
        /// encoding.
        /// </summary>
        /// <remarks>
        /// This method writes a list of messages into provided stream. The messages 
        /// are written by using provided encoding.
        /// </remarks>
        /// <param name="stream">
        /// The stream to write into.
        /// </param>
        /// <param name="encoding">
        /// The encoding to be used to write the message.
        /// </param>
        /// <param name="messages">
        /// The list of messages to write.
        /// </param>
        /// <returns>
        /// True is returned if message writing was successful and false if not.
        /// </returns>
        Boolean Write(Stream stream, Encoding encoding, IEnumerable<String> messages);
    }
}
