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
using Plexdata.LogWriter.Definitions;
using Plexdata.LogWriter.Internals.Events;
using Plexdata.LogWriter.Internals.Formatters;
using System;

namespace Plexdata.LogWriter.Internals.Factories
{
    /// <summary>
    /// This class represents the internally used logging factory.
    /// </summary>
    /// <remarks>
    /// The logging factory is used only internally. Against this 
    /// background, users might never get in contact with this class.
    /// </remarks>
    internal class LoggerFactory : ILoggerFactory
    {
        #region Construction

        /// <summary>
        /// The default constructor initializes instances of this class.
        /// </summary>
        /// <remarks>
        /// This constructor does actually do nothing.
        /// </remarks>
        public LoggerFactory()
        {
        }

        #endregion

        #region Public methods

        /// <inheritdoc />
        public ILogEvent CreateLogEvent(LogLevel level, DateTime time, String context, String scope, String message, Exception exception, params (String Label, Object Value)[] details)
        {
            return new LogEvent(level, time, context, scope, message, exception, details);
        }

        /// <inheritdoc />
        public ILogEventFormatter CreateLogEventFormatter(ILoggerSettings settings)
        {
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            switch (settings.LogType)
            {
                case LogType.Json:
                    return new JsonFormatter(settings);
                case LogType.Csv:
                    return new CsvFormatter(settings);
                default:
                    return new RawFormatter(settings);
            }
        }

        #endregion
    }
}
