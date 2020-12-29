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

using System.IO;
using System.Text;

namespace Plexdata.LogWriter.Abstraction
{
    /// <summary>
    /// This interface represents the scope of settings used together with 
    /// the stream logger.
    /// </summary>
    /// <remarks>
    /// The stream logger settings extend the basic logger settings by 
    /// additional information that are only used in conjunction with the 
    /// stream logger.
    /// </remarks>
    public interface IStreamLoggerSettings : ILoggerSettings
    {
        /// <summary>
        /// Gets and sets the stream to be used as message sink.
        /// </summary>
        /// <remarks>
        /// This property allows to change the underlying stream. Please note, 
        /// nothing is going to be logged as long as this property is <c>null</c>.
        /// </remarks>
        /// <value>
        /// The stream to be used as message sink.
        /// </value>
        Stream Stream { get; set; }

        /// <summary>
        /// Gets or sets the used file encoding.
        /// </summary>
        /// <remarks>
        /// This property allows to change the stream encoding to be used.
        /// </remarks>
        /// <value>
        /// The stream encoding to be used.
        /// </value>
        Encoding Encoding { get; set; }
    }
}
