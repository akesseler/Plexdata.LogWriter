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
using System.Net;

namespace Plexdata.LogWriter.Internals.Facades
{
    /// <summary>
    /// This interface provides methods to get system information such as 
    /// the local host name.
    /// </summary>
    /// <remarks>
    /// For the moment it is only possible to query the local host name.
    /// </remarks>
    internal interface IResolverFacade
    {
        /// <summary>
        /// Gets the local host name.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This method queries the local host name from the system and returns it.
        /// </para>
        /// <para>
        /// As first it is tried to obtain local machine name from current environment by 
        /// calling <see cref="Environment.MachineName"/>. Secondly it is tried to obtain 
        /// local machine name from underlying DNS service by calling <see cref="Dns.GetHostName()"/>.
        /// A <c>null</c> value is returned if none of above calls leads in a valid result.
        /// </para>
        /// </remarks>
        /// <returns>
        /// The local host name. A <c>null</c> value might be returned if determining local 
        /// machine name failed for any reason.
        /// </returns>
        /// <seealso cref="Environment.MachineName"/>
        /// <seealso cref="Dns.GetHostName()"/>
        String GetLocalHostName();

        /// <summary>
        /// Gets the newline string defined for this environment.
        /// </summary>
        /// <remarks>
        /// Unfortunately, the original documentation of <see cref="Environment.NewLine"/> only 
        /// mentions non-Unix and Unix platforms and not Apple platforms, which should use '\r' 
        /// instead. Hopefully it is done right for Apple platforms as well.
        /// </remarks>
        /// <returns>
        /// A string containing the platform-specific value for a new line. 
        /// </returns>
        /// <seealso cref="Environment.NewLine"/>
        String GetNewLine();
    }
}
