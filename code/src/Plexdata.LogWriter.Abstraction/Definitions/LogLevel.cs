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

namespace Plexdata.LogWriter.Definitions
{
    /// <summary>
    /// This enumeration defines all currently supported logging levels.
    /// </summary>
    /// <remarks>
    /// Generally spoken, a logging level represents the granularity of information that are 
    /// put into a logging target. This in turn means, the higher the logging level the less 
    /// are the messages written into the log. Right here applies, the logging level rises from 
    /// <c>Trace</c> up to <c>Disaster</c>. And this means, if the logging level is for example 
    /// set to <c>Warning</c>, every message with a log-level below <c>Warning</c> will be 
    /// suppressed and every message with a log-level equal to or higher than <c>Warning</c> 
    /// is written to logging target.
    /// </remarks>
    public enum LogLevel
    {
        /// <summary>
        /// This logging level indicates that logging is disabled at all.
        /// </summary>
        Disabled,

        /// <summary>
        /// This is the lowest possible logging level and can be used for 
        /// example to track a call of a method.
        /// </summary>
        Trace,

        /// <summary>
        /// This logging level might be used for any debug purpose.
        /// </summary>
        Debug,

        /// <summary>
        /// The verbose logging level could be used for example to inform 
        /// about the program version, start-up time or something similar.
        /// </summary>
        Verbose,

        /// <summary>
        /// The message logging level could be used for example to track 
        /// the current state of a program.
        /// </summary>
        Message,

        /// <summary>
        /// The warning logging level could be used to inform the outside 
        /// world about possible problem that could cause a crash later on.
        /// </summary>
        Warning,

        /// <summary>
        /// The error logging level could be used to indicate for example 
        /// problems with other components.
        /// </summary>
        Error,

        /// <summary>
        /// The fatal logging level could be used to indicate for example 
        /// real problems. Such problems may result in that a program cannot 
        /// longer function as expected.
        /// </summary>
        Fatal,

        /// <summary>
        /// The critical logging level could be used to indicate for example 
        /// a really bad program state. Such a critical program state may cause 
        /// the program to give up and to terminate unexpectedly.
        /// </summary>
        Critical,

        /// <summary>
        /// The disaster logging level could be used to indicate for example 
        /// a really dangerous program state. Such a disaster program state may 
        /// cause the program to give up in an unwanted state and to terminate 
        /// unexpectedly.
        /// </summary>
        Disaster,

        /// <summary>
        /// The default logging level, which is set to <see cref="Message"/>. 
        /// It is used for example as initial logging level.
        /// </summary>
        Default = Message, // ATTENTION! If this value changes, then this must be done in Convert(LogLevel) as well!
    }
}
