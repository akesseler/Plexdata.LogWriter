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
    /// This enumeration defines all currently supported network protocol types.
    /// </summary>
    /// <remarks>
    /// The network protocol type is used to determine which network writer has 
    /// to be used.
    /// </remarks>
    /// <seealso cref="Internals.Abstraction.IUdpNetworkWriter"/>
    /// <seealso cref="Internals.Abstraction.IUdpClientSocket"/>
    /// <seealso cref="Internals.Abstraction.ITcpNetworkWriter"/>
    /// <seealso cref="Internals.Abstraction.ITcpClientSocket"/>
    /// <seealso cref="Internals.Abstraction.IWebNetworkWriter"/>
    /// <seealso cref="Internals.Abstraction.IWebClientSocket"/>
    public enum Protocol
    {
        /// <summary>
        /// The protocol type is unknown and should only be use for initialization.
        /// </summary>
        Unknown,

        /// <summary>
        /// The UDP network writer and socket is used. See 
        /// <see cref="Internals.Abstraction.IUdpNetworkWriter"/> 
        /// and <see cref="Internals.Abstraction.IUdpClientSocket"/>.
        /// </summary>
        Udp,

        /// <summary>
        /// The TCP network writer and socket is used. See 
        /// <see cref="Internals.Abstraction.ITcpNetworkWriter"/> 
        /// and <see cref="Internals.Abstraction.ITcpClientSocket"/>.
        /// </summary>
        Tcp,

        /// <summary>
        /// The WEB/HTTP network writer and socket is used. See 
        /// <see cref="Internals.Abstraction.IWebNetworkWriter"/> 
        /// and <see cref="Internals.Abstraction.IWebClientSocket"/>.
        /// </summary>
        Web,

        /// <summary>
        /// The default protocol type, which is set to <see cref="Udp"/>. 
        /// It is used for example as initial protocol type.
        /// </summary>
        Default = Udp
    }
}
