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

using System.Text;

namespace Plexdata.LogWriter.Abstraction
{
    /// <summary>
    /// The interface defines the set of methods necessary to bring 
    /// any kind of logging event into human-readable form.
    /// </summary>
    /// <remarks>
    /// Classes derived from this interface do the actual work of 
    /// logging event formatting according to their specifications.
    /// </remarks>
    public interface ILogEventFormatter
    {
        /// <summary>
        /// This method formats provided logging event and writes the 
        /// result back into provided string builder.
        /// </summary>
        /// <remarks>
        /// Formatting a logging event takes places in different ways. User can 
        /// choose one of the supported formatters by defining an appropriated 
        /// <see cref="Plexdata.LogWriter.Definitions.LogType"/> within the logger 
        /// settings.
        /// </remarks>
        /// <param name="builder">
        /// The string builder instance that takes the formatted logging 
        /// event.
        /// </param>
        /// <param name="value">
        /// The logging event instance to be formatted.
        /// </param>
        void Format(StringBuilder builder, ILogEvent value);
    }
}
