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

using Plexdata.LogWriter.Abstraction;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Plexdata.LogWriter.Facades
{
    /// <summary>
    /// This class represents the default implementation of interface 
    /// <see cref="IPersistentLoggerFacade"/>.
    /// </summary>
    /// <remarks>
    /// Major task of this default implementation is to handle writing 
    /// of messages into an underlying logging file.
    /// </remarks>
    public class PersistentLoggerFacade : IPersistentLoggerFacade
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

        #region Construction

        /// <summary>
        /// The standard constructor.
        /// </summary>
        /// <remarks>
        /// This constructor initialize a new instance of this class.
        /// </remarks>
        public PersistentLoggerFacade()
            : base()
        {
        }

        /// <summary>
        /// The static class constructor.
        /// </summary>
        /// <remarks>
        /// This constructor just initializes all static fields.
        /// </remarks>
        static PersistentLoggerFacade()
        {
            PersistentLoggerFacade.interlock = new Object();
        }

        /// <summary>
        /// The class destructor.
        /// </summary>
        /// <remarks>
        /// The destructor cleans its resources.
        /// </remarks>
        ~PersistentLoggerFacade()
        {
        }

        #endregion

        #region Public methods

        /// <inheritdoc />
        /// <exception cref="ArgumentOutOfRangeException">
        /// This exception is thrown if provided parameter <paramref name="filename"/> 
        /// is <c>null</c>, <c>empty</c> or consists only of white spaces. 
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// This exception is thrown if provided parameter <paramref name="encoding"/> 
        /// is <c>null</c>. 
        /// </exception>
        /// <seealso cref="IPersistentLoggerFacade.Write(String, Encoding, IEnumerable{String})"/>
        /// <seealso cref="PersistentLoggerFacade.Write(String, Encoding, IEnumerable{String})"/>
        public Boolean Write(String filename, Encoding encoding, String message)
        {
            return this.Write(filename, encoding, new String[] { message });
        }

        /// <inheritdoc />
        /// <exception cref="ArgumentOutOfRangeException">
        /// This exception is thrown if provided parameter <paramref name="filename"/> 
        /// is <c>null</c>, <c>empty</c> or consists only of white spaces. 
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// This exception is thrown if provided parameter <paramref name="encoding"/> is 
        /// <c>null</c> or if provided parameter <paramref name="messages"/> is <c>null</c>. 
        /// </exception>
        /// <exception cref="Exception">
        /// Other exceptions, such as <see cref="DirectoryNotFoundException"/>, 
        /// <see cref="PathTooLongException"/>, <see cref="System.Security.SecurityException"/>, 
        /// <see cref="UnauthorizedAccessException"/>, and more, may occur as well. See 
        /// <see cref="File.AppendAllLines(String, IEnumerable{String}, Encoding)"/> for 
        /// more information.
        /// </exception>
        /// <seealso cref="IPersistentLoggerFacade.Write(String, Encoding, String)"/>
        /// <seealso cref="PersistentLoggerFacade.Write(String, Encoding, String)"/>
        public Boolean Write(String filename, Encoding encoding, IEnumerable<String> messages)
        {
            if (String.IsNullOrWhiteSpace(filename))
            {
                throw new ArgumentOutOfRangeException(nameof(filename), "The filename cannot be null, or empty or consists only of whitespaces.");
            }

            if (encoding == null)
            {
                throw new ArgumentNullException(nameof(encoding), "The encoding cannot be null.");
            }

            if (messages == null)
            {
                throw new ArgumentNullException(nameof(messages), "The messages to write cannot be null.");
            }

            return PersistentLoggerFacade.WriteInternal(filename, encoding, messages);
        }

        /// <inheritdoc />
        /// <exception cref="ArgumentOutOfRangeException">
        /// This exception is thrown if provided parameter <paramref name="filename"/> 
        /// is <c>null</c>, <c>empty</c> or consists only of white spaces. 
        /// </exception>
        /// <exception cref="Exception">
        /// Other exceptions, such as <see cref="DirectoryNotFoundException"/>, 
        /// <see cref="PathTooLongException"/>, <see cref="System.Security.SecurityException"/>, 
        /// <see cref="UnauthorizedAccessException"/>, and more, may occur as well. See 
        /// <see cref="File.WriteAllText(String, String)"/> for 
        /// more information.
        /// </exception>
        /// <seealso cref="IPersistentLoggerFacade.Empty(String)"/>
        public Boolean Empty(String filename)
        {
            if (String.IsNullOrWhiteSpace(filename))
            {
                throw new ArgumentOutOfRangeException(nameof(filename), "The filename cannot be null, or empty or consists only of whitespaces.");
            }

            return PersistentLoggerFacade.EmptyInternal(filename);
        }

        #endregion

        #region Private methods

        /// <summary>
        /// The internal message write method.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This method thread-safely writes provided list of messages. 
        /// </para>
        /// <para>
        /// For the record. This method does not process-safely handle access to 
        /// one and the same logging file. But if it happens, an IO exception is 
        /// thrown by the system. This exception is caught by this method and in 
        /// this case the method returns <c>false</c>. Trying to repeat a writing 
        /// operation until the method returns <c>true</c> is the only chance to 
        /// make logging messages persistent.
        /// </para>
        /// </remarks>
        /// <param name="filename">
        /// The name of the file to write into.
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
        /// <seealso cref="File.AppendAllLines(String, IEnumerable{String}, Encoding)"/>
        private static Boolean WriteInternal(String filename, Encoding encoding, IEnumerable<String> messages)
        {
            try
            {
                lock (PersistentLoggerFacade.interlock)
                {
                    // 1) Using a Mutex does not reliably fix the problem with accessing one and the 
                    //    same file from within different processes. This is because of sometimes an 
                    //    AbandonedMutexException is thrown if a process and/or a thread has been killed. 
                    //    After that nothing can be written anymore from other processes. Against this 
                    //    background, just try writing, and in case of an exception occurs simply 
                    //    try again, and again, and again until the job is done.
                    // 2) For the record, using a stream doesn't work. Some tries, tests and researches 
                    //    have shown that using a previously opened file stream will not have the expected 
                    //    effect of storing each of the messages. The same applies to opening a file stream 
                    //    right here. This actually means, that messages will be lost. Therefore, writing 
                    //    the messages this way has been chosen.
                    File.AppendAllLines(filename, PersistentLoggerFacade.GetCleanMessages(messages), encoding);

                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// The internal file empty method.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This method thread-safely empties content of provided file. 
        /// </para>
        /// <para>
        /// For the record. This method does not process-safely handle access to 
        /// one and the same logging file. But if it happens, an IO exception is 
        /// thrown by the system. This exception is caught by this method and in 
        /// this case the method returns <c>false</c>. Trying to repeat a writing 
        /// operation until the method returns <c>true</c> is the only chance to 
        /// make logging messages persistent.
        /// </para>
        /// </remarks>
        /// <param name="filename">
        /// The name of the file to empty.
        /// </param>
        /// <returns>
        /// True is returned if file content was discarded successfully and false if not.
        /// </returns>
        /// <seealso cref="File.WriteAllText(String, String)"/>
        private static Boolean EmptyInternal(String filename)
        {
            try
            {
                lock (PersistentLoggerFacade.interlock)
                {
                    // 1) Using a Mutex does not reliably fix the problem with accessing one and the 
                    //    same file from within different processes. This is because of sometimes an 
                    //    AbandonedMutexException is thrown if a process and/or a thread has been killed. 
                    //    After that nothing can be written anymore from other processes. Against this 
                    //    background, just try writing, and in case of an exception occurs simply 
                    //    try again, and again, and again until the job is done.
                    // 2) For the record, using a stream doesn't work. Some tries, tests and researches 
                    //    have shown that using a previously opened file stream will not have the expected 
                    //    effect of storing each of the messages. The same applies to opening a file stream 
                    //    right here. This actually means, that messages will be lost. Therefore, writing 
                    //    the messages this way has been chosen.
                    File.WriteAllText(filename, String.Empty);

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

                yield return message.Trim();
            }
        }

        #endregion
    }
}
