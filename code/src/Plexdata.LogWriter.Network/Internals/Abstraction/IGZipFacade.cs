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

using System;

namespace Plexdata.LogWriter.Internals.Abstraction
{
    /// <summary>
    /// An interface to allow data compression.
    /// </summary>
    /// <remarks>
    /// This interface represents the abstraction of a facade 
    /// of <see cref="System.IO.Compression.GZipStream"/>.
    /// </remarks>
    internal interface IGZipFacade
    {
        /// <summary>
        /// Compresses provided <paramref name="source"/> using 
        /// GZip algorithm.
        /// </summary>
        /// <remarks>
        /// This method compresses provided <paramref name="source"/> 
        /// using GZip algorithm.
        /// </remarks>
        /// <param name="source">
        /// The source buffer to be compressed.
        /// </param>
        /// <returns>
        /// The compressed result buffer.
        /// </returns>
        Byte[] Compress(Byte[] source);
    }
}
