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

using Plexdata.LogWriter.Definitions;
using System;

namespace Plexdata.LogWriter.Abstraction
{
    /// <summary>
    /// The interface represents any kind of logging event.
    /// </summary>
    /// <remarks>
    /// Classes derived from this interface are used as data container 
    /// and represent a fully qualified logging event.
    /// </remarks>
    public interface ILogEvent
    {
        /// <summary>
        /// Determines whether the logging event is valid.
        /// </summary>
        /// <remarks>
        /// Any logging event is considered as valid as soon as 
        /// its message contains a proper text.
        /// </remarks>
        /// <value>
        /// True if the logging event is valid and false otherwise.
        /// </value>
        Boolean IsValid { get; }

        /// <summary>
        /// Gets the unique key of any particular logging event.
        /// </summary>
        /// <remarks>
        /// The key represents some kind of handle to uniquely identify a 
        /// particular logging event. This key is generated internally as 
        /// soon as an instance of this interface is created.
        /// </remarks>
        /// <value>
        /// The unique key representing this logging event.
        /// </value>
        Guid Key { get; }

        /// <summary>
        /// Get the level of this logging event.
        /// </summary>
        /// <remarks>
        /// The level represents the severity of a particular logging event. Additionally 
        /// note, the logging level is assigned by a particular logger implementation.
        /// </remarks>
        /// <value>
        /// One of the values of enumeration <see cref="LogLevel"/>.
        /// </value>
        LogLevel Level { get; }

        /// <summary>
        /// Gets the time stamp of this logging event.
        /// </summary>
        /// <remarks>
        /// The time stamp represents the time at when a particular logging event has occurred. 
        /// Additionally note, the time stamp is assigned by a particular logger implementation.
        /// </remarks>
        /// <value>
        /// The time stamp of this logging event. 
        /// </value>
        DateTime Time { get; }

        /// <summary>
        /// Gets the context of this logging event.
        /// </summary>
        /// <remarks>
        /// The context is in fact an optional argument and should help to identify source of 
        /// a logging event. For example such a source could be a program, a library or a class.
        /// </remarks>
        /// <value>
        /// The logging event context.
        /// </value>
        String Context { get; }

        /// <summary>
        /// Gets the scope of this logging event.
        /// </summary>
        /// <remarks>
        /// The scope is in fact an optional argument and should help to identify source of 
        /// a logging event. For example such a source could be a class or a method.
        /// </remarks>
        /// <value>
        /// The logging event scope.
        /// </value>
        String Scope { get; }

        /// <summary>
        /// Gets the message of this logging event.
        /// </summary>
        /// <remarks>
        /// Each logging event should have an appropriate message text 
        /// that contains some details why this issue has happened.
        /// </remarks>
        /// <value>
        /// The logging event message.
        /// </value>
        String Message { get; }

        /// <summary>
        /// Gets the exception assigned to this logging event.
        /// </summary>
        /// <remarks>
        /// An exception might be useful in case of an error. Therefore, 
        /// this property should be used to be able to track any kind of 
        /// program failures.
        /// </remarks>
        /// <value>
        /// The assigned exception instance or <c>null</c>, 
        /// if no exception has been assigned.
        /// </value>
        Exception Exception { get; }

        /// <summary>
        /// Gets a set of label-value-pair assignments.
        /// </summary>
        /// <remarks>
        /// The details assignments might be useful to track the current data 
        /// state of a program.
        /// </remarks>
        /// <value>
        /// An array of value tuples containing a combination of label-value-pairs.
        /// </value>
        (String Label, Object Value)[] Details { get; }
    }
}
