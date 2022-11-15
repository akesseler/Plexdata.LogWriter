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

using Plexdata.LogWriter.Abstraction;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Plexdata.LogWriter.Facades
{
    /// <summary>
    /// This class represents the default implementation of interface 
    /// <see cref="IStreamLoggerFacade"/>.
    /// </summary>
    /// <remarks>
    /// Major task of this default implementation is to handle writing 
    /// of messages into its assigned stream.
    /// </remarks>
    public class StreamLoggerFacade : IStreamLoggerFacade
    {
        #region Private fields

        /// <summary>
        /// This field holds the instance of the internal synchronization object.
        /// </summary>
        /// <remarks>
        /// Writing data into a file should be thread-safely locked. For this purpose 
        /// this object is used.
        /// </remarks>
        private static readonly Object interlock = null;

        #endregion

        #region Public constants

        /// <summary>
        /// The End of Transmission Block control character.
        /// </summary>
        /// <remarks>
        /// The ETB control character is used to split a written 
        /// buffer into a list of messages.
        /// </remarks>
        public static readonly String ETB = new String((Char)0x17, 1);

        #endregion

        #region Construction

        /// <summary>
        /// The standard constructor.
        /// </summary>
        /// <remarks>
        /// This constructor initialize a new instance of this class.
        /// </remarks>
        public StreamLoggerFacade()
            : base()
        {
        }

        /// <summary>
        /// The static class constructor.
        /// </summary>
        /// <remarks>
        /// This constructor just initializes all static fields.
        /// </remarks>
        static StreamLoggerFacade()
        {
            StreamLoggerFacade.interlock = new Object();
        }

        /// <summary>
        /// The class destructor.
        /// </summary>
        /// <remarks>
        /// The destructor cleans its resources.
        /// </remarks>
        ~StreamLoggerFacade()
        {
        }

        #endregion

        #region Public methods

        /// <inheritdoc />
        /// <exception cref="ArgumentNullException">
        /// This exception is thrown if one of provided parameters <paramref name="stream"/>
        /// or <paramref name="encoding"/> is <c>null</c>. 
        /// </exception>
        /// <seealso cref="IStreamLoggerFacade.Write(Stream, Encoding, IEnumerable{String})"/>
        /// <seealso cref="StreamLoggerFacade.Write(Stream, Encoding, IEnumerable{String})"/>
        public Boolean Write(Stream stream, Encoding encoding, String message)
        {
            return this.Write(stream, encoding, new String[] { message });
        }

        /// <inheritdoc />
        /// <exception cref="ArgumentNullException">
        /// This exception is thrown if one of provided parameters <paramref name="stream"/>, 
        /// <paramref name="encoding"/> or <paramref name="messages"/> is <c>null</c>. 
        /// </exception>
        /// <seealso cref="IStreamLoggerFacade.Write(Stream, Encoding, String)"/>
        /// <seealso cref="StreamLoggerFacade.Write(Stream, Encoding, String)"/>
        public Boolean Write(Stream stream, Encoding encoding, IEnumerable<String> messages)
        {
            if (stream == null)
            {
                throw new ArgumentNullException(nameof(stream), "The stream cannot be null.");
            }

            if (encoding == null)
            {
                throw new ArgumentNullException(nameof(encoding), "The encoding cannot be null.");
            }

            if (messages == null)
            {
                throw new ArgumentNullException(nameof(messages), "The messages to write cannot be null.");
            }

            return StreamLoggerFacade.WriteInternal(stream, encoding, messages);
        }

        #endregion

        #region Private methods

        /// <summary>
        /// The internal message write method.
        /// </summary>
        /// <remarks>
        /// This method thread-safely writes provided list of messages. 
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
        private static Boolean WriteInternal(Stream stream, Encoding encoding, IEnumerable<String> messages)
        {
            try
            {
                lock (StreamLoggerFacade.interlock)
                {
                    if (!stream.CanWrite)
                    {
                        return false;
                    }

                    StringBuilder builder = new StringBuilder(String.Join(StreamLoggerFacade.ETB, StreamLoggerFacade.GetCleanMessages(messages))).Append(StreamLoggerFacade.ETB);

                    if (builder.Length < 2) // Check if less than last ETB.
                    {
                        return false;
                    }

                    Byte[] buffer = encoding.GetBytes(builder.ToString());

                    stream.Write(buffer, 0, buffer.Length);
                    stream.Flush();

                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Prepares a list of messages for output.
        /// </summary>
        /// <remarks>
        /// This method iterates through provided list of messages and returns a 
        /// cleaned up message list. This means that all invalid or empty message 
        /// are striped out and each returned message does not have any leading or 
        /// tailing whitespaces.
        /// </remarks>
        /// <param name="messages">
        /// The list of messages to be prepared.
        /// </param>
        /// <returns>
        /// The prepared list of messages.
        /// </returns>
        private static IEnumerable<String> GetCleanMessages(IEnumerable<String> messages)
        {
            foreach (String message in messages)
            {
                if (String.IsNullOrWhiteSpace(message))
                {
                    continue;
                }

                // Keep in mind, all whitespaces (including carriage return and line feed) are removed!
                yield return message.Trim();
            }
        }

        #endregion
    }
}
