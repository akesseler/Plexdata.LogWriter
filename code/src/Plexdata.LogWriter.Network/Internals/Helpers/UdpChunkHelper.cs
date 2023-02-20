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

using Plexdata.LogWriter.Abstraction;
using Plexdata.LogWriter.Definitions;
using Plexdata.LogWriter.Internals.Abstraction;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Plexdata.LogWriter.Internals.Helpers
{
    /// <summary>
    /// A class to allow splitting of payloads into chunks.
    /// </summary>
    /// <remarks>
    /// This class represents the implementation of a helper 
    /// that allows splitting of payloads into chunks.
    /// </remarks>
    internal class UdpChunkHelper : IUdpChunkHelper
    {
        #region Private fields

        /// <summary>
        /// The internal factory to create required dependencies.
        /// </summary>
        /// <remarks>
        /// This field holds the internal factory to be able to create needed dependencies.
        /// </remarks>
        private readonly INetworkInternalFactory factory;

        /// <summary>
        /// An instance of the settings to use.
        /// </summary>
        /// <remarks>
        /// This field holds an instance of the used settings.
        /// </remarks>
        private readonly INetworkLoggerSettings settings;

        /// <summary>
        /// An instance of the compression facade.
        /// </summary>
        /// <remarks>
        /// This field holds an instance of the GZip facade created through the factory.
        /// </remarks>
        private readonly IGZipFacade zipper;

        /// <summary>
        /// An instance of the random facade.
        /// </summary>
        /// <remarks>
        /// This field holds an instance of the random facade created through the factory.
        /// </remarks>
        private readonly IRandomFacade random;

        #endregion

        #region Construction

        /// <summary>
        /// The only class constructor.
        /// </summary>
        /// <remarks>
        /// This constructor initializes all its fields and properties.
        /// </remarks>
        /// <param name="factory">
        /// An instance of the factory to use.
        /// </param>
        /// <param name="settings">
        /// An instance of the settings to use.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// This exception is thrown if one of the parameters is <c>null</c>.
        /// </exception>
        public UdpChunkHelper(INetworkInternalFactory factory, INetworkLoggerSettings settings)
            : base()
        {
            this.factory = factory ?? throw new ArgumentNullException(nameof(factory));
            this.settings = settings ?? throw new ArgumentNullException(nameof(settings));

            this.zipper = this.factory.Create<IGZipFacade>();
            this.random = this.factory.Create<IRandomFacade>();
        }

        #endregion

        #region Public methods

        /// <inheritdoc />
        public IEnumerable<Byte[]> GetChunks(Byte[] payload)
        {
            if (!payload?.Any() ?? true)
            {
                Debug.WriteLine($"{nameof(IUdpChunkHelper)}::{nameof(GetChunks)}(): No buffer to get any kind of chunk from.");

                return Enumerable.Empty<Byte[]>();
            }

            if (this.settings.LogType != LogType.Gelf)
            {
                return this.GetPureChunks(payload, this.settings.Maximum);
            }

            return this.GetGelfChunks(payload, this.settings.Compressed, this.settings.Threshold, this.settings.Maximum);
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Splits provided payload into pure chunks.
        /// </summary>
        /// <remarks>
        /// This method splits provided payload into pure chunks, which means into 
        /// chunks without any additional information to be able to reassemble them.
        /// </remarks>
        /// <param name="payload">
        /// The payload to split into chunks.
        /// </param>
        /// <param name="maximum">
        /// The maximum chunk size.
        /// </param>
        /// <returns>
        /// A list of payload chunks.
        /// </returns>
        private IEnumerable<Byte[]> GetPureChunks(Byte[] payload, Int32 maximum)
        {
            Int32 sequences = (payload.Length / maximum) + ((payload.Length % maximum == 0) ? 0 : 1);

            for (Int32 sequence = 0; sequence < sequences; sequence++)
            {
                Int32 offset = sequence * maximum;

                Int32 count = Math.Min(payload.Length - offset, maximum);

                Byte[] result = new Byte[count];

                Array.ConstrainedCopy(payload, offset, result, 0, count);

                yield return result;
            }
        }

        /// <summary>
        /// Splits provided payload into GELF chunks.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This method splits provided payload into GELF chunks.
        /// </para>
        /// <para>
        /// Each GELF chunk consists of two magic bytes, followed by a unique message ID, 
        /// followed by one byte that holds current sequence number, and finally followed 
        /// by one byte holding the total number of chunks.
        /// </para>
        /// </remarks>
        /// <param name="payload">
        /// The payload to split into chunks.
        /// </param>
        /// <param name="compressed">
        /// True if the payload has to be compressed and false if not.
        /// </param>
        /// <param name="threshold">
        /// The number of payload bytes at which a compression has to be done.
        /// </param>
        /// <param name="maximum">
        /// The maximum chunk size.
        /// </param>
        /// <returns>
        /// A list of payload chunks.
        /// </returns>
        private IEnumerable<Byte[]> GetGelfChunks(Byte[] payload, Boolean compressed, Int32 threshold, Int32 maximum)
        {
            if (compressed && payload.Length > threshold)
            {
                Int32 original = payload.Length;

                // BUG: How to compress a GELF message is unclear.
                //
                //      Section COMPRESSION of the official GELF documentation is unclear regarding
                //      the usage of compression.
                //
                //      What does this mean: "When using UDP as transport layer, GELF messages can
                //                            be sent uncompressed or compressed with either GZIP or
                //                            ZLIB. Graylog nodes automatically detect the compression
                //                            type in the GELF magic byte header."
                //
                //      Does it mean: 1) The whole GELF message can be compressed?
                //                    2) The whole payload can be compressed at once?
                //                    3) Each chunk of the whole payload can be compressed?
                //
                //      It's not clear, but for the moment, the whole payload will be compressed and
                //      each chunk contains one piece of that compressed payload.
                //
                //      But all implementation example that has ever been seen zipping the payload at
                //      once and split it into chunks afterwards, like done below.

                payload = this.zipper.Compress(payload);

                Debug.WriteLine($"{nameof(IUdpChunkHelper)}::{nameof(GetChunks)}(): Input buffer has been zipped. Original length: {original}, Zipped length: {payload.Length}.");
            }

            if (payload.Length < maximum)
            {
                Debug.WriteLine($"{nameof(IUdpChunkHelper)}::{nameof(GetChunks)}(): Buffer length is less than {maximum} and splitting into chunks is not needed.");

                yield return payload;
                yield break;
            }

            // Magic number are always bad. But for sure the length of 12 bytes for the message
            // header might never change.
            //
            // BUG: The resulting maximum body "length" might become negative.
            //      In such a case, the number of sequences becomes zero and nothing happens.
            //      But this is indeed a theoretical problem, because each GELF message has to
            //      consist at least of fields "version", "host" and "short_message".
            Int32 length = maximum - 12;

            Int32 sequences = (Int32)Math.Ceiling(payload.Length / (Double)length);

            // The GELF specification states: "A message MUST NOT consist of more than
            // 128 chunks." Therefore, another magic number can be used here as well.
            if (sequences > 128)
            {
                Debug.WriteLine($"{nameof(IUdpChunkHelper)}::{nameof(GetChunks)}(): The message contains {sequences} chunks and exceeds the maximum of 128 chunks.");

                yield break;
            }

            Byte[] header = this.CreateInitialHeader(sequences);

            for (Int32 sequence = 0; sequence < sequences; sequence++)
            {
                this.UpdateSequenceNumber(header, sequence);

                Int32 offset = sequence * length;

                Int32 count = Math.Min(payload.Length - offset, length);

                Byte[] result = new Byte[header.Length + count];

                Array.Copy(header, result, header.Length);

                Array.ConstrainedCopy(payload, offset, result, header.Length, count);

                yield return result;
            }
        }

        /// <summary>
        /// Creates and initializes a GELF header.
        /// </summary>
        /// <remarks>
        /// This method creates and initializes a GELF header with its magic bytes, its 
        /// unique message ID, a placeholder for current sequence number as well as with 
        /// its total sequences.
        /// </remarks>
        /// <param name="sequences">
        /// The total number of sequences.
        /// </param>
        /// <returns>
        /// An initial GELF message header.
        /// </returns>
        private Byte[] CreateInitialHeader(Int32 sequences)
        {
            Byte[] header = new Byte[12];

            // Set chunked GELF magic bytes (2 bytes)!
            header[0] = 0x1E;
            header[1] = 0x0F;

            Byte[] messageId = new Byte[8];
            this.random.NextBytes(messageId);

            // Set message ID (8 bytes).
            header[2] = messageId[0];
            header[3] = messageId[1];
            header[4] = messageId[2];
            header[5] = messageId[3];
            header[6] = messageId[4];
            header[7] = messageId[5];
            header[8] = messageId[6];
            header[9] = messageId[7];

            // Set sequence number (1 byte) and total Sequence count (1 byte).
            header[10] = 0;
            header[11] = (Byte)(sequences & 0x000000FF);

            return header;
        }

        /// <summary>
        /// Updates the sequence number byte of a GELF header.
        /// </summary>
        /// <remarks>
        /// This method updates the sequence number <paramref name="header"/> byte and 
        /// sets value of byte 11 to its current <paramref name="sequence"/> number.
        /// </remarks>
        /// <param name="header">
        /// The header to be updated.
        /// </param>
        /// <param name="sequence">
        /// The sequence number to be set.
        /// </param>
        private void UpdateSequenceNumber(Byte[] header, Int32 sequence)
        {
            header[10] = (Byte)(sequence & 0x000000FF);
        }

        #endregion
    }
}
