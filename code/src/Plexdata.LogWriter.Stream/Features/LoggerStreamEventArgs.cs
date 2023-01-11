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
using System.Linq;

namespace Plexdata.LogWriter.Features
{
    /// <summary>
    /// Provides data for events fired by feature class 
    /// <see cref="LoggerStream"/>.
    /// </summary>
    /// <remarks>
    /// At the moment, this event arguments implementation is only used 
    /// together with event <see cref="LoggerStream.ProcessStreamData"/>.
    /// </remarks>
    public class LoggerStreamEventArgs : EventArgs
    {
        #region Private fields

        /// <summary>
        /// This field holds all messages to be processed.
        /// </summary>
        /// <remarks>
        /// Be aware, receiving an event with an instance of this 
        /// class causes the caller to remove those messages afterwards.
        /// </remarks>
        private readonly IEnumerable<String> messages;

        #endregion

        #region Construction

        /// <summary>
        /// The only constructor that initializes property 
        /// <see cref="LoggerStreamEventArgs.Messages"/>.
        /// </summary>
        /// <remarks>
        /// Intentionally, this constructor has been defined as internal 
        /// because of external program code does not need to create its 
        /// own instance.
        /// </remarks>
        /// <param name="messages">
        /// The list of messages to be processed by each receiver.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// This exception is thrown if parameter <paramref name="messages"/> 
        /// is <c>null</c>.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// This exception is thrown if parameter <paramref name="messages"/> 
        /// does not contain any valid item.
        /// </exception>
        internal LoggerStreamEventArgs(IEnumerable<String> messages)
        {
            if (messages == null)
            {
                throw new ArgumentNullException(nameof(messages));
            }

            if (!messages.Any())
            {
                throw new ArgumentOutOfRangeException(nameof(messages));
            }

            this.messages = messages;
        }

        #endregion

        #region Public properties

        /// <summary>
        /// Gets the list of assigned messages.
        /// </summary>
        /// <remarks>
        /// This property just returns the list of messages to be 
        /// processed by each attached event receiver.
        /// </remarks>
        /// <value>
        /// The list of assigned messages.
        /// </value>
        public IEnumerable<String> Messages
        {
            get
            {
                foreach (String message in this.messages)
                {
                    yield return message;
                }
            }
        }

        #endregion
    }
}
