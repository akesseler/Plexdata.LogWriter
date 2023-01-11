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

using Plexdata.LogWriter.Definitions;
using System;

namespace Plexdata.LogWriter.Internals.Extensions
{
    /// <summary>
    /// This extension allows to perform <see cref="LogLevel"/> conversions.
    /// </summary>
    /// <remarks>
    /// For the moment only a conversion into Unix severity levels is possible.
    /// </remarks>
    internal static class LogLevelExtension
    {
        /// <summary>
        /// Maps the logging <paramref name="level"/> into Unix severity level.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This method maps the logging <paramref name="level"/> into Unix 
        /// severity level. For more information about Unix severity levels 
        /// see here https://en.wikipedia.org/wiki/Syslog#Severity_level.
        /// </para>
        /// <para>
        /// See table below for all assignment details.
        /// </para>
        /// <list type="table">
        /// <listheader><term>Logging Level</term><description>Unix Severity Level</description></listheader>
        /// <item><term><see cref="LogLevel.Disabled"/></term><description>Causes an exception.</description></item>
        /// <item><term><see cref="LogLevel.Trace"/></term><description><c>7</c>: Debug</description></item>
        /// <item><term><see cref="LogLevel.Debug"/></term><description><c>7</c>: Debug</description></item>
        /// <item><term><see cref="LogLevel.Verbose"/></term><description><c>6</c>: Informational</description></item>
        /// <item><term><see cref="LogLevel.Message"/></term><description><c>5</c>: Notice</description></item>
        /// <item><term><see cref="LogLevel.Warning"/></term><description><c>4</c>: Warning</description></item>
        /// <item><term><see cref="LogLevel.Error"/></term><description><c>3</c>: Error</description></item>
        /// <item><term><see cref="LogLevel.Fatal"/></term><description><c>2</c>: Critical</description></item>
        /// <item><term><see cref="LogLevel.Critical"/></term><description><c>1</c>: Alert</description></item>
        /// <item><term><see cref="LogLevel.Disaster"/></term><description><c>0</c>: Emergency</description></item>
        /// </list>
        /// </remarks>
        /// <param name="level">
        /// The logging level to convert.
        /// </param>
        /// <returns>
        /// One of the corresponding Unix severity levels.
        /// </returns>
        public static Int32 ToUnixSeverityLevel(this LogLevel level)
        {
            switch (level)
            {
                case LogLevel.Trace:
                case LogLevel.Debug:
                    return 7; // Debug: debug-level messages
                case LogLevel.Verbose:
                    return 6; // Informational: informational messages
                case LogLevel.Message:
                    return 5; // Notice: normal but significant condition
                case LogLevel.Warning:
                    return 4; // Warning: warning conditions
                case LogLevel.Error:
                    return 3; // Error: error conditions
                case LogLevel.Fatal:
                    return 2; // Critical: critical conditions
                case LogLevel.Critical:
                    return 1; // Alert: action must be taken immediately
                case LogLevel.Disaster:
                    return 0; // Emergency: system is unusable
                case LogLevel.Disabled:
                default:
                    throw new ArgumentOutOfRangeException(nameof(level), $"Found unsupported logging level '{level}' to convert.");
            }
        }
    }
}
