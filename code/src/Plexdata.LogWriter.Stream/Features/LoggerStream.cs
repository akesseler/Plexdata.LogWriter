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
using System.Linq;
using System.Text;

namespace Plexdata.LogWriter.Features
{
    /// <summary>
    /// This feature class represents one additional implementation 
    /// of the abstract class <see cref="System.IO.Stream"/>.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Main feature of this class is that it is event driven. This 
    /// in turn means that writing into this stream and flushing it 
    /// causes an event which informs every assigned listener about 
    /// receiving new data.
    /// </para>
    /// <para>
    /// This stream implementation might be used together with 
    /// <see cref="IStreamLogger"/> or with <see cref="IStreamLogger{TContext}"/> 
    /// and can be applied using <see cref="IStreamLoggerSettings"/>.
    /// </para>
    /// <para>
    /// But pay attention, this stream implementation only supports 
    /// stream writing. The other way round, reading or seeking this 
    /// stream is intentionally impossible!
    /// </para>
    /// </remarks>
    public class LoggerStream : Stream
    {
        #region Public events

        /// <summary>
        /// Occurs when all available stream data should be processed by any 
        /// attached event listener.
        /// </summary>
        /// <remarks>
        /// This event is fired if an event handler is attached and as soon as 
        /// method <see cref="LoggerStream.Flush()"/> is called.
        /// </remarks>
        public event LoggerStreamEventHandler ProcessStreamData;

        #endregion

        #region Private fields

        /// <summary>
        /// This field holds the chosen encoding.
        /// </summary>
        /// <remarks>
        /// The value of <see cref="LoggerStream.DefaultEncoding"/> 
        /// is used as initial value.
        /// </remarks>
        private Encoding encoding;

        /// <summary>
        /// This field holds the available list of messages.
        /// </summary>
        /// <remarks>
        /// The list of currently assigned messages is filled by method 
        /// <see cref="LoggerStream.Write(Byte[], Int32, Int32)"/> and should 
        /// be retrieve as soon as event <see cref="LoggerStream.ProcessStreamData"/> 
        /// occurs.
        /// </remarks>
        private readonly List<String> messages = new List<String>();

        #endregion

        #region Private constants

        /// <summary>
        /// The default encoding value.
        /// </summary>
        /// <remarks>
        /// The default encoding value is set to <c>UTF-8</c>.
        /// </remarks>
        private static readonly Encoding DefaultEncoding = Encoding.UTF8;

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
        /// The static class constructor.
        /// </summary>
        /// <remarks>
        /// This constructor does actually nothing.
        /// </remarks>
        static LoggerStream() { }

        /// <summary>
        /// The standard class constructor.
        /// </summary>
        /// <remarks>
        /// This constructor just initializes all properties with its default 
        /// values.
        /// </remarks>
        /// <seealso cref="LoggerStream.LoggerStream(Encoding)"/>
        public LoggerStream()
            : this(LoggerStream.DefaultEncoding)
        {
        }

        /// <summary>
        /// The extended class constructor.
        /// </summary>
        /// <remarks>
        /// This constructor initializes all properties with its default 
        /// values, except property <see cref="LoggerStream.Encoding"/>. 
        /// The provided <paramref name="encoding"/> instance is used to 
        /// initialize this property instead.
        /// </remarks>
        /// <param name="encoding">
        /// The encoding to be used.
        /// </param>
        public LoggerStream(Encoding encoding)
        {
            this.Encoding = encoding;
        }

        #endregion

        #region Public properties

        /// <summary>
        /// Gets or sets the used file encoding.
        /// </summary>
        /// <remarks>
        /// This property allows to change the stream encoding. 
        /// The default encoding is used if provided value is 
        /// <c>null</c>.
        /// </remarks>
        /// <value>
        /// The stream encoding to be used.
        /// </value>
        public Encoding Encoding
        {
            get
            {
                return this.encoding;
            }
            set
            {
                this.encoding = value ?? LoggerStream.DefaultEncoding;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the current stream 
        /// supports reading.
        /// </summary>
        /// <remarks>
        /// This stream implementation is never readable!
        /// </remarks>
        /// <value>
        /// This property always returns <c>false</c>.
        /// </value>
        public override Boolean CanRead { get { return false; } }

        /// <summary>
        /// Gets a value indicating whether the current stream 
        /// supports seeking.
        /// </summary>
        /// <remarks>
        /// This stream implementation is never seekable!
        /// </remarks>
        /// <value>
        /// This property always returns <c>false</c>.
        /// </value>
        public override Boolean CanSeek { get { return false; } }

        /// <summary>
        /// Gets a value indicating whether the current stream 
        /// supports writing.
        /// </summary>
        /// <remarks>
        /// This stream implementation is only writable!
        /// </remarks>
        /// <value>
        /// This property always returns <c>true</c>.
        /// </value>
        public override Boolean CanWrite { get { return true; } }

        /// <summary>
        /// Gets the length in bytes of the stream.
        /// </summary>
        /// <remarks>
        /// This stream implementation does not support length querying!
        /// </remarks>
        /// <value>
        /// This property always returns <c>zero</c>.
        /// </value>
        public override Int64 Length { get { return 0L; } }

        /// <summary>
        /// Gets or sets the position within the current stream.
        /// </summary>
        /// <remarks>
        /// This stream implementation does not support changing or 
        /// querying the current stream position!
        /// </remarks>
        /// <value>
        /// This property always returns <c>zero</c>.
        /// </value>
        public override Int64 Position { get { return 0L; } set { } }

        #endregion

        #region Public methods

        /// <summary>
        /// Clears all buffers for this stream and causes any buffered data to 
        /// be written to the underlying device.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This method fires event <see cref="LoggerStream.ProcessStreamData"/> 
        /// if attached and clears all currently buffered messages. Be aware, the 
        /// buffered message list is cleared in any case, no matter if someone 
        /// listens the event or not.
        /// </para>
        /// <para>
        /// Additionally, this method does never throw any exception, but puts them 
        /// into the debug output, if occurs.
        /// </para>
        /// </remarks>
        public override void Flush()
        {
            if (this.messages.Count > 0)
            {
                try
                {
                    this.ProcessStreamData?.Invoke(this, new LoggerStreamEventArgs(this.messages));
                }
                catch (Exception exception)
                {
                    System.Diagnostics.Debug.WriteLine(exception);
                }
                finally
                {
                    this.messages.Clear();
                }
            }
        }

        /// <summary>
        /// Reads a sequence of bytes from the current stream and advances the 
        /// position within the stream by the number of bytes read.
        /// </summary>
        /// <remarks>
        /// This stream implementation does not support stream reading!
        /// </remarks>
        /// <param name="buffer">
        /// An array of bytes. 
        /// </param>
        /// <param name="offset">
        /// The zero-based byte offset in buffer.
        /// </param>
        /// <param name="count">
        /// The maximum number of bytes to read.
        /// </param>
        /// <returns>
        /// The total number of bytes read into the buffer.
        /// </returns>
        /// <exception cref="NotSupportedException">
        /// This exception is thrown in any case.
        /// </exception>
        public override Int32 Read(Byte[] buffer, Int32 offset, Int32 count)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Sets the position within the current stream.
        /// </summary>
        /// <remarks>
        /// This stream implementation does not support stream seeking!
        /// </remarks>
        /// <param name="offset">
        /// A byte offset relative to the origin parameter.
        /// </param>
        /// <param name="origin">
        /// A value of type <see cref="System.IO.SeekOrigin"/> indicating the 
        /// reference point used to obtain the new position.
        /// </param>
        /// <returns>
        /// The new position within the current stream.
        /// </returns>
        /// <exception cref="NotSupportedException">
        /// This exception is thrown in any case.
        /// </exception>
        public override Int64 Seek(Int64 offset, SeekOrigin origin)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Sets the length of the current stream.
        /// </summary>
        /// <remarks>
        /// This stream implementation does not support length changing! Against 
        /// this background, calling this method does not have any effect.
        /// </remarks>
        /// <param name="value">
        /// The desired length of the current stream in bytes.
        /// </param>
        public override void SetLength(Int64 value) { }

        /// <summary>
        /// Writes a sequence of bytes to the current stream and advances 
        /// the current position within this stream by the number of bytes 
        /// written.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This method splits the content of provided <paramref name="buffer"/> 
        /// (starting at <paramref name="offset"/> and taking <paramref name="count"/> 
        /// bytes) into multiple lines of strings. These line-splits are made at each 
        /// <see cref="LoggerStream.ETB"/>. Furthermore, every invalid or empty 
        /// line is automatically removed from the resulting list of lines!
        /// </para>
        /// <para>
        /// Additionally, this method does never throw any exception, but puts them 
        /// into the debug output, if occurs.
        /// </para>
        /// </remarks>
        /// <param name="buffer">
        /// An array of bytes.
        /// </param>
        /// <param name="offset">
        /// The zero-based byte offset in buffer at which to begin copying bytes 
        /// to the current stream.
        /// </param>
        /// <param name="count">
        /// The number of bytes to be written to the current stream.
        /// </param>
        public override void Write(Byte[] buffer, Int32 offset, Int32 count)
        {
            if (buffer != null && buffer.Length > 0)
            {
                try
                {
                    IEnumerable<String> messages = this.Encoding
                        .GetString(buffer, offset, count)
                        .Split(new String[] { LoggerStream.ETB }, StringSplitOptions.RemoveEmptyEntries)
                        .Where(x => !String.IsNullOrWhiteSpace(x))
                        .AsEnumerable();

                    if (messages.Any())
                    {
                        this.messages.AddRange(messages);
                    }
                }
                catch (Exception exception)
                {
                    System.Diagnostics.Debug.WriteLine(exception);
                }
            }
        }

        #endregion
    }
}
