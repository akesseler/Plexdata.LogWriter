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

using Plexdata.LogWriter.Internals.Abstraction;
using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.IO.Compression;

namespace Plexdata.LogWriter.Internals.Facades
{
    /// <summary>
    /// A class to allow data compression.
    /// </summary>
    /// <remarks>
    /// This class represents the implementation of a facade 
    /// of <see cref="GZipStream"/>.
    /// </remarks>
    [ExcludeFromCodeCoverage]
    internal class GZipFacade : IGZipFacade
    {
        #region Construction

        /// <summary>
        /// The standard constructor.
        /// </summary>
        /// <remarks>
        /// This constructor initialize a new instance of this class.
        /// </remarks>
        public GZipFacade()
            : base()
        {
        }

        #endregion

        #region Public methods

        /// <inheritdoc />
        public Byte[] Compress(Byte[] source)
        {
            source = source ?? new Byte[0];

            using (MemoryStream stream = new MemoryStream())
            {
                using (GZipStream zipper = new GZipStream(stream, CompressionMode.Compress))
                {
                    zipper.Write(source, 0, source.Length);
                }

                return stream.ToArray();
            }
        }

        #endregion
    }
}
