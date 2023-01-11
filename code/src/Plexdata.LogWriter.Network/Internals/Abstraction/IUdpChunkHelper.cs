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
using Plexdata.LogWriter.Definitions;
using System;
using System.Collections.Generic;

namespace Plexdata.LogWriter.Internals.Abstraction
{
    /// <summary>
    /// An interface to allow splitting of payloads into chunks.
    /// </summary>
    /// <remarks>
    /// This interface represents the abstraction of a helper 
    /// that allows splitting of payloads into chunks.
    /// </remarks>
    internal interface IUdpChunkHelper
    {
        /// <summary>
        /// Splits the <paramref name="payload"/> into chunks according to current settings.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This method splits provided <paramref name="payload"/> into chunks according to current 
        /// settings. But attention, this method is primarily intended to create GELF chunks!
        /// </para>
        /// <para>
        /// In case of GELF messages (see <see cref="ILoggerSettings.LogType"/> and <see cref="LogType.Gelf"/>), 
        /// this method splits provided <paramref name="payload"/> into GELF compliant pieces. Furthermore, 
        /// the settings <see cref="INetworkLoggerSettings.Compressed"/>,  <see cref="INetworkLoggerSettings.Threshold"/> 
        /// and <see cref="INetworkLoggerSettings.Maximum"/> are taken into account. For more information 
        /// about GELF chunks please see https://go2docs.graylog.org/5-0/getting_in_log_data/gelf.html.
        /// </para>
        /// <para>
        /// In case of non-GELF messages (<see cref="ILoggerSettings.LogType"/>), this method simply splits 
        /// the <paramref name="payload"/> into pieces and uses the <see cref="INetworkLoggerSettings.Maximum"/> 
        /// as chunk size limitation. Attention! No chunk labelling (or something similar to identify chunks 
        /// in any way) is applied, which makes it almost impossible to reassemble a chunked message. Therefore, 
        /// is is not recommended to split payloads without GELF type!
        /// </para>
        /// </remarks>
        /// <param name="payload">
        /// The payload to split into chunks.
        /// </param>
        /// <returns>
        /// A list of payload chunks according to current settings.
        /// </returns>
        IEnumerable<Byte[]> GetChunks(Byte[] payload);
    }
}
