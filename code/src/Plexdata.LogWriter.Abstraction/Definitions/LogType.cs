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

namespace Plexdata.LogWriter.Definitions
{
    /// <summary>
    /// This enumeration defines all currently supported logging types.
    /// </summary>
    /// <remarks>
    /// For the moment a raw-text logger, a CSV logger, a JSON logger, 
    /// an XML logger as well as a GELF logger are the supported types.
    /// </remarks>
    public enum LogType
    {
        /// <summary>
        /// Use raw output format. The output is surrounded by double 
        /// quotes as soon as the part split character occurs inside 
        /// the value.
        /// </summary>
        Raw,

        /// <summary>
        /// Use CSV output format. The output is formatted according to 
        /// the rules of RFC 4180.
        /// </summary>
        Csv,

        /// <summary>
        /// Use JSON output format. The output is formatted in JavaScript 
        /// Object Notation.
        /// </summary>
        Json,

        /// <summary>
        /// Use XML output format. The output is formatted in Extensible 
        /// Markup Language format.
        /// </summary>
        Xml,

        /// <summary>
        /// Use GELF output format. The output is formatted in Graylog 
        /// Extended Log Format.
        /// </summary>
        Gelf,

        /// <summary>
        /// Use default output format, which is set to <see cref="Raw"/> 
        /// format.
        /// </summary>
        Default = Raw,
    }
}
