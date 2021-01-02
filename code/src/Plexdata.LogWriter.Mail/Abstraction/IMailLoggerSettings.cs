/*
 * MIT License
 * 
 * Copyright (c) 2021 plexdata.de
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
    /// This interface represents the scope of settings used together with 
    /// the mail logger.
    /// </summary>
    /// <remarks>
    /// The mail logger settings extend the basic logger settings by 
    /// additional information that are only used in conjunction with the 
    /// mail logger.
    /// </remarks>
    public interface IMailLoggerSettings : ILoggerSettings
    {
        #region Account dependent settings

        /// <summary>
        /// Gets and sets the sender e-mail address.
        /// </summary>
        /// <remarks>
        /// This property allows to get or set the sender e-mail address. 
        /// Default value is <c>empty</c>.
        /// </remarks>
        /// <value>
        /// The sender e-mail address.
        /// </value>
        String Address { get; set; }

        /// <summary>
        /// Gets and sets the e-mail account username.
        /// </summary>
        /// <remarks>
        /// This property allows to get or set the e-mail account username. 
        /// Default value is <c>empty</c>.
        /// </remarks>
        /// <value>
        /// The e-mail account username.
        /// </value>
        String Username { get; set; }

        /// <summary>
        /// Gets and sets the e-mail account password.
        /// </summary>
        /// <remarks>
        /// This property allows to get or set the e-mail account password.
        /// Be aware, there might be a security issue if such a password is 
        /// put into an unencrypted configuration file! Default value is 
        /// <c>empty</c>.
        /// </remarks>
        /// <value>
        /// The e-mail account password.
        /// </value>
        String Password { get; set; }

        #endregion

        #region Email server dependent settings

        /// <summary>
        /// Gets and sets the SMTP host name.
        /// </summary>
        /// <remarks>
        /// This property allows to get or set the SMTP host name. Default 
        /// value is <c>empty</c>.
        /// </remarks>
        /// <value>
        /// The SMTP host name.
        /// </value>
        String SmtpHost { get; set; }

        /// <summary>
        /// Gets and sets the SMTP port name.
        /// </summary>
        /// <remarks>
        /// This property allows to get or set the SMTP port number. Usually, 
        /// such a port number is set to <c>587</c> for new SMTP servers. Legacy 
        /// SMTP servers may use <c>25</c> as SMTP port. Default value is 
        /// <c>587</c>.
        /// </remarks>
        /// <value>
        /// The SMTP port name.
        /// </value>
        Int32 SmtpPort { get; set; }

        /// <summary>
        /// Determines whether the usage of SSL is enabled or not.
        /// </summary>
        /// <remarks>
        /// This property allows to determine whether the usage of 
        /// SSL is enabled or not. Default value is <c>true</c>.
        /// </remarks>
        /// <value>
        /// The usage of SSL is enabled or not.
        /// </value>
        Boolean UseSsl { get; set; }

        #endregion

        #region Message dependent settings

        /// <summary>
        /// Gets or sets the used mail encoding.
        /// </summary>
        /// <remarks>
        /// This property allows to change the mail encoding to be used. 
        /// Default value is <c>UTF-8</c>.
        /// </remarks>
        /// <value>
        /// The mail encoding to be used.
        /// </value>
        Encoding Encoding { get; set; }

        /// <summary>
        /// Gets or sets the used mail subject.
        /// </summary>
        /// <remarks>
        /// This property allows to change the used mail subject. The 
        /// logging message is used as subject if this property is not 
        /// set (<c>null</c>, <c>empty</c>, <c>whitespaces</c>). Default 
        /// value is <c>empty</c>.
        /// </remarks>
        ///<value>
        /// The used mail subject.
        ///</value>
        String Subject { get; set; }

        /// <summary>
        /// Gets and sets the list of main e-mail recipients.
        /// </summary>
        /// <remarks>
        /// This property allows to get or set the list of main e-mail 
        /// recipients (<c>To</c>). Default value is <c>empty</c>.
        /// </remarks>
        /// <value>
        /// The list of main e-mail recipients.
        /// </value>
        IEnumerable<String> Receivers { get; set; }

        /// <summary>
        /// Gets and sets the list of copy e-mail recipients.
        /// </summary>
        /// <remarks>
        /// This property allows to get or set the list of copy e-mail 
        /// recipients (carbon copy <c>CC</c>). Default value is <c>empty</c>.
        /// </remarks>
        /// <value>
        /// The list of copy e-mail recipients.
        /// </value>
        IEnumerable<String> ClearCopies { get; set; }

        /// <summary>
        /// Gets and sets the list of copy e-mail recipients.
        /// </summary>
        /// <remarks>
        /// This property allows to get or set the list of blind e-mail 
        /// recipients (blind carbon copy <c>BCC</c>). Default value is 
        /// <c>empty</c>.
        /// </remarks>
        /// <value>
        /// The list of blind e-mail recipients.
        /// </value>
        IEnumerable<String> BlindCopies { get; set; }

        #endregion
    }
}
