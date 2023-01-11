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
    /// This enumeration defines all currently supported network address types.
    /// </summary>
    /// <remarks>
    /// The network address type is used to determine which network address family 
    /// has to be used. This is especially important for UDP and TCP network connections.
    /// </remarks>
    public enum Address
    {
        /// <summary>
        /// The address type is unknown and should only be use for initialization 
        /// or together with protocol <see cref="Protocol.Web"/>.
        /// </summary>
        Unknown,

        /// <summary>
        /// The IPv4 address family is used for communication. Ses also 
        /// <see cref="System.Net.Sockets.AddressFamily.InterNetwork"/>.
        /// </summary>
        IPv4,

        /// <summary>
        /// The IPv6 address family is used for communication. Ses also 
        /// <see cref="System.Net.Sockets.AddressFamily.InterNetworkV6"/>.
        /// </summary>
        IPv6,

        /// <summary>
        /// The default address type, which is set to <see cref="IPv4"/>. 
        /// It is used for example as initial address type.
        /// </summary>
        Default = IPv4
    }
}
