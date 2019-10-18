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

using System;
using System.Collections.Generic;
using System.Text;

namespace Plexdata.LogWriter.Abstraction
{
    /// <summary>
    /// This interface represents all actions needed to natively write messages 
    /// into a logging mail.
    /// </summary>
    /// <remarks>
    /// The interface is required as an abstraction between the mail logger 
    /// itself and the implementation of the native mail writing. This interface 
    /// might be re-implemented if a different access to the native mail writing 
    /// becomes necessary.
    /// </remarks>
    public interface IMailLoggerFacade
    {
        /// <summary>
        /// Gets and sets the e-mail account username.
        /// </summary>
        /// <remarks>
        /// This property allows to get or set the e-mail account username. 
        /// </remarks>
        /// <value>
        /// The e-mail account username.
        /// </value>
        /// <seealso cref="IMailLoggerSettings.Username"/>
        String Username { get; set; }

        /// <summary>
        /// Gets and sets the e-mail account password.
        /// </summary>
        /// <remarks>
        /// This property allows to get or set the e-mail account password.
        /// Be aware, there might be a security issue if such a password is 
        /// put into an unencrypted configuration file!
        /// </remarks>
        /// <value>
        /// The e-mail account password.
        /// </value>
        /// <seealso cref="IMailLoggerSettings.Password"/>
        String Password { get; set; }

        /// <summary>
        /// Gets and sets the SMTP host name.
        /// </summary>
        /// <remarks>
        /// This property allows to get or set the SMTP host name.
        /// </remarks>
        /// <value>
        /// The SMTP host name.
        /// </value>
        /// <seealso cref="IMailLoggerSettings.SmtpHost"/>
        String SmtpHost { get; set; }

        /// <summary>
        /// Gets and sets the SMTP port name.
        /// </summary>
        /// <remarks>
        /// This property allows to get or set the SMTP port name. Usually, 
        /// such a port is set to <c>587</c> for new SMTP servers. Legacy 
        /// SMTP servers may use <c>25</c> as SMTP port.
        /// </remarks>
        /// <value>
        /// The SMTP port name.
        /// </value>
        /// <seealso cref="IMailLoggerSettings.SmtpPort"/>
        Int32 SmtpPort { get; set; }

        /// <summary>
        /// Determines whether the usage of SSL is enabled or not.
        /// </summary>
        /// <remarks>
        /// This property allows to determine whether the usage of 
        /// SSL is enabled or not.
        /// </remarks>
        /// <value>
        /// The usage of SSL is enabled or not.
        /// </value>
        /// <seealso cref="IMailLoggerSettings.UseSsl"/>
        Boolean UseSsl { get; set; }

        /// <summary>
        /// Sends one message to configured mail server using provided arguments.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This method sends one message to configured mail server using provided arguments. Such a 
        /// mail message is only sent if all provided arguments are valid. Valid means in this case 
        /// that <paramref name="address"/>, <paramref name="subject"/> and <paramref name="content"/> 
        /// are neither <c>null</c>, nor <c>empty</c> nor consist only of white spaces. Furthermore, 
        /// encoding must not be <c>null</c>. Finally, at least one of the receiver addresses 
        /// (<paramref name="receivers"/>, <paramref name="clearCopies"/>, <paramref name="blindCopies"/>) 
        /// must be set. No message is sent if at least one of these conditions is violated!
        /// </para>
        /// <para>
        /// In this context important to know, each message is sent in its own thread without any chance 
        /// to abort. In addition, no exception is thrown. The other way round, each exception is caught 
        /// and in best case written into the Console's error output.
        /// </para>
        /// </remarks>
        /// <param name="address">
        /// The sender address (From, <see cref="IMailLoggerSettings.Address"/>).
        /// </param>
        /// <param name="receivers">
        /// The mail receiver address list (To, <see cref="IMailLoggerSettings.Receivers"/>).
        /// </param>
        /// <param name="clearCopies">
        /// The mail copy receiver address list (CC, <see cref="IMailLoggerSettings.ClearCopies"/>).
        /// </param>
        /// <param name="blindCopies">
        /// The mail blind copy receiver address list (BCC, <see cref="IMailLoggerSettings.BlindCopies"/>).
        /// </param>
        /// <param name="encoding">
        /// The mail encoding, which is used for <paramref name="subject"/> as well as for <paramref name="content"/>.
        /// </param>
        /// <param name="subject">
        /// The mail subject.
        /// </param>
        /// <param name="content">
        /// The mail content.
        /// </param>
        void Write(String address, IEnumerable<String> receivers, IEnumerable<String> clearCopies, IEnumerable<String> blindCopies, Encoding encoding, String subject, String content);
    }
}
