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

using Plexdata.LogWriter.Definitions;
using System;
using System.Text;

namespace Plexdata.LogWriter.Abstraction
{
    /// <summary>
    /// This interface represents the scope of settings used together with the 
    /// network logger.
    /// </summary>
    /// <remarks>
    /// The network logger settings extend the basic logger settings by additional 
    /// information that are only used in conjunction with the network logger.
    /// </remarks>
    /// <example>
    /// <para><strong>UDP Examples</strong></para>
    /// <para>An example of a UDP/IPv4 configuration with IP address.</para>
    /// <code>
    /// settings.Protocol = Protocol.Udp;
    /// settings.Address = Address.IPv4;
    /// settings.Host = "127.0.0.1";
    /// settings.Port = 42011;
    /// </code>
    /// <para>An example of a UDP/IPv4 configuration with DNS name.</para>
    /// <code>
    /// settings.Protocol = Protocol.Udp;
    /// settings.Address = Address.IPv4;
    /// settings.Host = "localhost";
    /// settings.Port = 42011;
    /// </code>
    /// <para>An example of a UDP/IPv6 configuration with IP address.</para>
    /// <code>
    /// Protocol = Protocol.Udp;
    /// Address = Address.IPv6;
    /// Host = "::1";
    /// Port = 42012;
    /// </code>
    /// <para>An example of a UDP/IPv6 configuration with DNS name.</para>
    /// <code>
    /// settings.Protocol = Protocol.Udp;
    /// settings.Address = Address.IPv6;
    /// settings.Host = "localhost";
    /// settings.Port = 42012;
    /// </code>
    /// <para><strong>TCP Examples</strong></para>
    /// <para>An example of a TCP/IPv4 configuration with IP address.</para>
    /// <code>
    /// settings.Protocol = Protocol.Tcp;
    /// settings.Address = Address.IPv4;
    /// settings.Host = "127.0.0.1";
    /// settings.Port = 42021;
    /// </code>
    /// <para>An example of a TCP/IPv4 configuration with DNS name.</para>
    /// <code>
    /// settings.Protocol = Protocol.Tcp;
    /// settings.Address = Address.IPv4;
    /// settings.Host = "localhost";
    /// settings.Port = 42021;
    /// </code>
    /// <para>An example of a TCP/IPv6 configuration with IP address.</para>
    /// <code>
    /// settings.Protocol = Protocol.Tcp;
    /// settings.Address = Address.IPv6;
    /// settings.Host = "::1";
    /// settings.Port = 42022;
    /// </code>
    /// <para>An example of a TCP/IPv6 configuration with DNS name.</para>
    /// <code>
    /// settings.Protocol = Protocol.Tcp;
    /// settings.Address = Address.IPv6;
    /// settings.Host = "localhost";
    /// settings.Port = 42022;
    /// </code>
    /// <para><strong>WEB Examples</strong></para>
    /// <para>An example of a WEB/HTTP configuration with URL and extra port.</para>
    /// <code>
    /// settings.Protocol = Protocol.Web;
    /// settings.Address = Address.Unknown; // Don't care.
    /// settings.Host = "http://localhost/gelf"; // Port is taken from property 'Port'.
    /// settings.Port = 42031; // Replace standard port by this value.
    /// </code>
    /// <para>An example of a WEB/HTTP configuration with URL and integrated port.</para>
    /// <code>
    /// settings.Protocol = Protocol.Web;
    /// settings.Address = Address.Unknown; // Don't care.
    /// settings.Host = "http://localhost:42031/gelf"; // Port is taken from URL.
    /// settings.Port = 0; // Determine port from URL.
    /// </code>
    /// <para>An example of a WEB/HTTP configuration with URL and standard port.</para>
    /// <code>
    /// settings.Protocol = Protocol.Web;
    /// settings.Address = Address.Unknown; // Don't care.
    /// settings.Host = "http://localhost/gelf"; // Standard port of HTTP is used.
    /// settings.Port = 0; // Determine port from URL.
    /// </code>
    /// </example>
    public interface INetworkLoggerSettings : ILoggerSettings
    {
        #region General properties

        /// <summary>
        /// Gets or sets the target host.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This property is the most important property of these settings because it references the target server. 
        /// However, it is also the property that is most difficult to configure, as its importance and value is 
        /// highly dependent on other properties. Therefore have a look at the examples in the description of section 
        /// <see cref="INetworkLoggerSettings"/>
        /// </para>
        /// <para>
        /// Furthermore, this property can represent an IP address (IPv4 or IPv6), or a DNS host name entry, or as 
        /// well a fully qualified URL. Which of these possible interpretations is taken at runtime depends on other 
        /// properties. For more details see following overview.
        /// </para>
        /// <table>
        /// <tr><th>Protocol</th><th>Address</th><th>Host</th><th>Remarks</th></tr>
        /// <tr>
        ///   <td><see cref="Protocol.Udp"/>, <see cref="Protocol.Tcp"/></td>
        ///   <td><see cref="Address.IPv4"/></td><td>IP&#160;address</td>
        ///   <td>The <em>Host</em> property can contain an IPv4 address, such as <c>127.0.0.1</c> for example. 
        ///   In this case the remote server must allow access over IPv4.</td>
        /// </tr>
        /// <tr>
        ///   <td><see cref="Protocol.Udp"/>, <see cref="Protocol.Tcp"/></td>
        ///   <td><see cref="Address.IPv4"/></td><td>DNS&#160;name</td>
        ///   <td>The <em>Host</em> property can contain a DNS host name, such as <c>localhost</c> for example, 
        ///   and will resolve to an IPv4 address. In this case the remote server must allow access over IPv4.</td>
        /// </tr>
        /// <tr>
        ///   <td><see cref="Protocol.Udp"/>, <see cref="Protocol.Tcp"/></td>
        ///   <td><see cref="Address.IPv6"/></td><td>IP&#160;address</td>
        ///   <td>The <em>Host</em> property can contain an IPv6 address, such as <c>::1</c> for example. In this 
        ///   case the remote server must allow access over IPv6.</td>
        /// </tr>
        /// <tr>
        ///   <td><see cref="Protocol.Udp"/>, <see cref="Protocol.Tcp"/></td>
        ///   <td><see cref="Address.IPv6"/></td><td>DNS&#160;name</td>
        ///   <td>The <em>Host</em> property can contain a DNS host name, such as <c>localhost</c> for example, 
        ///   and will resolve to an IPv6 address. In this case the remote server must allow access over IPv6.</td>
        /// </tr>
        /// <tr>
        ///   <td><see cref="Protocol.Web"/></td><td>Ignored</td><td>URL</td>
        ///   <td>
        ///   <p>The <em>Host</em> property should (must) contain a fully qualified URL, such as <c>http://localhost/gelf</c> 
        ///   or <c>http://localhost:42031/gelf</c> for example.</p>
        ///   <ul>
        ///   <li>The default port number is taken (port 80 for HTTP and port 443 for HTTPS) if property 
        ///   <see cref="Port"/> is set to zero.</li>
        ///   <li>The port number is taken from property <see cref="Port"/> if no port is included in the URL and 
        ///   property <see cref="Port"/> is not set to zero.</li>
        ///   <li>The port number is taken from the URL and if property <see cref="Port"/> is not set to zero.</li>
        ///   <li>The port number is taken from property <see cref="Port"/> if it is not set to zero and the port 
        ///   number in the URL is different from value of property <see cref="Port"/>.</li>
        ///   </ul>
        ///   <p>See also <see cref="Plexdata.LogWriter.Internals.Sockets.WebClientSocket.BuildRemoteUri(String, Int32)"/>.</p>
        ///   </td>
        /// </tr>
        /// </table>
        /// </remarks>
        /// <value>
        /// The target host to be used.
        /// </value>
        /// <seealso cref="Port"/>
        /// <seealso cref="Address"/>
        /// <seealso cref="Protocol"/>
        String Host { get; set; }

        /// <summary>
        /// Gets or sets the target port.
        /// </summary>
        /// <remarks>
        /// This property defines the port on which the target computer is listened to.
        /// </remarks>
        /// <value>
        /// The target port to be used.
        /// </value>
        /// <seealso cref="Host"/>
        /// <seealso cref="Address"/>
        /// <seealso cref="Protocol"/>
        Int32 Port { get; set; }

        /// <summary>
        /// Gets or sets the address type.
        /// </summary>
        /// <remarks>
        /// This property defines the address type used to establish communication with 
        /// the target computer.
        /// </remarks>
        /// <value>
        /// The address type to be used.
        /// </value>
        /// <seealso cref="Host"/>
        /// <seealso cref="Port"/>
        /// <seealso cref="Protocol"/>
        Address Address { get; set; }

        /// <summary>
        /// Gets or sets the protocol type.
        /// </summary>
        /// <remarks>
        /// This property defines the interface type to be used for communication with 
        /// the target computer. 
        /// </remarks>
        /// <value>
        /// The protocol type to be used.
        /// </value>
        /// <seealso cref="Host"/>
        /// <seealso cref="Port"/>
        /// <seealso cref="Address"/>
        Protocol Protocol { get; set; }

        /// <summary>
        /// Gets or sets the used encoding.
        /// </summary>
        /// <remarks>
        /// This property allows to change the encoding to be used. Default value is 
        /// <c>UTF-8</c>.
        /// </remarks>
        /// <value>
        /// The encoding to be used.
        /// </value>
        Encoding Encoding { get; set; }

        #endregion

        #region UDP dependent properties

        /// <summary>
        /// Enables or disables message compression.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Messages are compressed if enabled and the number of bytes of a message is 
        /// greater than the <see cref="Threshold"/>.
        /// </para>
        /// <para>
        /// Using compression is a matter of CPU load or network traffic.
        /// </para>
        /// <para>
        /// According to the Graylog/GELF documentation GZip is used as compression default 
        /// algorithm. And this algorithm is taken here as well. The ZLib algorithm is not 
        /// supported at the moment.
        /// </para>
        /// <para>
        /// Keep in mind, this property is used only together with protocol type 
        /// <see cref="Protocol.Udp"/>.
        /// </para>
        /// <para>
        /// Additionally note, compression is never used for non-GELF messages.
        /// </para>
        /// </remarks>
        /// <value>
        /// True to enable compression and false to disable it. Default is true.
        /// </value>
        /// <seealso cref="Threshold"/>
        /// <seealso cref="Maximum"/>
        /// <seealso cref="Protocol"/>
        Boolean Compressed { get; set; }

        /// <summary>
        /// Gets and sets the compression threshold size.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Messages are compressed using GZip if compression is enabled and a message is 
        /// bigger than this threshold.
        /// </para>
        /// <para>
        /// Keep in mind, this property is used only together with protocol type 
        /// <see cref="Protocol.Udp"/> and if <see cref="Compressed"/> is enabled. 
        /// </para>
        /// </remarks>
        /// <value>
        /// The threshold at when message compression is performed. Default 
        /// is 512.
        /// </value>
        /// <seealso cref="Compressed"/>
        /// <seealso cref="Maximum"/>
        /// <seealso cref="Protocol"/>
        Int32 Threshold { get; set; }

        /// <summary>
        /// Gets and sets the maximum chunk size. 
        /// </summary>
        /// <remarks>
        /// <para>
        /// According to the specification "GELF VIA UDP" please note: "Some Graylog components 
        /// are limited to processing up to 8192 bytes."
        /// </para>
        /// <para>
        /// Be aware, the value of this property may never be less than or equal to 12 bytes!
        /// </para>
        /// <para>
        /// Known bugs: Datagrams are not sent in case of a message is less than the maximum 
        /// and the maximum is less than 12 bytes.
        /// </para>
        /// <para>
        /// Recommendation: Never change this value.
        /// </para>
        /// <para>
        /// <strong>Exception:</strong> This setting can be used to limit the UDP datagram payload length 
        /// in case of transmitting non-GELF messages together with protocol <see cref="Protocol.Udp"/>. 
        /// But be aware, do never use values greater than the allowed UDP datagram payload length!
        /// </para>
        /// <para>
        /// Keep in mind, this property is used only together with protocol type <see cref="Protocol.Udp"/>.
        /// </para>
        /// </remarks>
        /// <value>
        /// The maximum number of bytes the Graylog UDP-API can process. Default 
        /// is 8192.
        /// </value>
        /// <seealso cref="Compressed"/>
        /// <seealso cref="Threshold"/>
        /// <seealso cref="Protocol"/>
        Int32 Maximum { get; set; }

        #endregion

        #region TCP dependent properties

        /// <summary>
        /// Enables or disables the usage of zero termination.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This property allows to enable or to disable the usage of a zero termination for messages 
        /// sent to remote host. The zero termination is only append if it hasn't been appended yet.
        /// </para>
        /// <para>
        /// According to the specification "GELF VIA TCP" please note: "GELF TCP does not support 
        /// compression due to the use of the null byte (<c>\0</c>) as frame delimiter."
        /// </para>
        /// <para>
        /// Attention, any client connection is closed automatically if this property is set to 
        /// <c>false</c> OR a message already ends with a zero termination! Otherwise the client 
        /// connection is kept open.
        /// </para>
        /// <para>
        /// Keep in mind, this property is used only together with protocol type TCP.
        /// </para>
        /// </remarks>
        /// <value>
        /// True to enable zero termination and false to disable it. Default is false.
        /// </value>
        /// <seealso cref="Protocol"/>
        Boolean Termination { get; set; }

        #endregion

        #region WEB dependent properties

        /// <summary>
        /// Gets and sets the timeout in milliseconds to be used.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This property allows to change the timeout value to be used. For the 
        /// moment the timeout value is only used together with WEB requests!
        /// </para>
        /// <para>
        /// Allowed is each number greater than or equal to zero as well as 
        /// <see cref="System.Threading.Timeout.Infinite"/>. Any other value 
        /// may completely disable the logger by causing unwanted exception.
        /// </para>
        /// </remarks>
        /// <value>
        /// The timeout value in milliseconds to use. The default is 100000 (100 seconds).
        /// </value>
        Int32 Timeout { get; set; }

        #endregion
    }
}
