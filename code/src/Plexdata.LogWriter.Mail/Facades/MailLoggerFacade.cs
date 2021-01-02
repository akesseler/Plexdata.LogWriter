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

using Plexdata.LogWriter.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Plexdata.LogWriter.Facades
{
    /// <summary>
    /// This class represents the default implementation of interface 
    /// <see cref="IMailLoggerFacade"/>.
    /// </summary>
    /// <remarks>
    /// Major task of this default implementation is to handle writing 
    /// of messages into its assigned mail targets.
    /// </remarks>
    public class MailLoggerFacade : IMailLoggerFacade
    {
        #region Construction

        /// <summary>
        /// The standard constructor.
        /// </summary>
        /// <remarks>
        /// This constructor initialize a new instance of this class.
        /// </remarks>
        public MailLoggerFacade()
            : base()
        {
        }

        /// <summary>
        /// The class destructor.
        /// </summary>
        /// <remarks>
        /// The destructor cleans its resources.
        /// </remarks>
        ~MailLoggerFacade()
        {
        }

        #endregion

        #region Public properties

        /// <inheritdoc />
        public String Username { get; set; }

        /// <inheritdoc />
        public String Password { get; set; }

        /// <inheritdoc />
        public String SmtpHost { get; set; }

        /// <inheritdoc />
        public Int32 SmtpPort { get; set; }

        /// <inheritdoc />
        public Boolean UseSsl { get; set; }

        #endregion

        #region Public methods

        /// <inheritdoc />
        public void Write(String address, IEnumerable<String> receivers, IEnumerable<String> clearCopies, IEnumerable<String> blindCopies, Encoding encoding, String subject, String content)
        {
            Boolean processable = (encoding != null) &&
                !String.IsNullOrWhiteSpace(address) &&
                !String.IsNullOrWhiteSpace(subject) &&
                !String.IsNullOrWhiteSpace(content) &&
                (
                    (receivers != null && receivers.Where(x => !String.IsNullOrWhiteSpace(x)).Any()) ||
                    (clearCopies != null && clearCopies.Where(x => !String.IsNullOrWhiteSpace(x)).Any()) ||
                    (blindCopies != null && blindCopies.Where(x => !String.IsNullOrWhiteSpace(x)).Any())
                );

            if (!processable) { return; }

            try
            {
                Task.Run(() =>
                {
                    try
                    {
                        using (MailMessage message = this.CreateMessage(address, receivers, clearCopies, blindCopies, encoding, subject, content))
                        {
                            using (SmtpClient client = this.CreateSmtpClient())
                            {
                                client.Send(message);
                            }
                        }
                    }
                    catch (Exception exception)
                    {
                        Console.Error.WriteLine(exception);
                    }
                })
                .ConfigureAwait(false);
            }
            catch (Exception exception)
            {
                Console.Error.WriteLine(exception);
            }
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Creates a new instance of a SMTP Client.
        /// </summary>
        /// <remarks>
        /// This method creates a new instance of a SMTP Client.
        /// Be aware, any kind of system exception can occur.
        /// </remarks>
        /// <returns>
        /// The new SMTP Client instance.
        /// </returns>
        private SmtpClient CreateSmtpClient()
        {
            return new SmtpClient
            {
                Host = this.SmtpHost,
                Port = this.SmtpPort,
                Credentials = new NetworkCredential() { UserName = this.Username, Password = this.Password },
                EnableSsl = this.UseSsl
            };
        }

        /// <summary>
        /// Creates a new mail message instance.
        /// </summary>
        /// <remarks>
        /// This method creates a new mail message instance from provided 
        /// arguments.
        /// </remarks>
        /// <param name="address">
        /// The sender address (From).
        /// </param>
        /// <param name="receivers">
        /// The mail receiver address list (To).
        /// </param>
        /// <param name="clearCopies">
        /// The mail copy receiver address list (CC).
        /// </param>
        /// <param name="blindCopies">
        /// The mail blind copy receiver address list (BCC).
        /// </param>
        /// <param name="encoding">
        /// The mail encoding, which is used for <paramref name="subject"/> 
        /// as well as for <paramref name="content"/>.
        /// </param>
        /// <param name="subject">
        /// The mail subject.
        /// </param>
        /// <param name="content">
        /// The mail content.
        /// </param>
        /// <returns>
        /// The new mail message instance.
        /// </returns>
        private MailMessage CreateMessage(String address, IEnumerable<String> receivers, IEnumerable<String> clearCopies, IEnumerable<String> blindCopies, Encoding encoding, String subject, String content)
        {
            MailMessage message = new MailMessage
            {
                From = new MailAddress(address),
                SubjectEncoding = encoding,
                Subject = subject,
                BodyEncoding = encoding,
                Body = content
            };

            this.AddAddresses(message.To, receivers);
            this.AddAddresses(message.CC, clearCopies);
            this.AddAddresses(message.Bcc, blindCopies);

            return message;
        }

        /// <summary>
        /// Adds mail addresses to provided mail address list.
        /// </summary>
        /// <remarks>
        /// This method adds mail addresses to provided mail address list. Nothing is added 
        /// if <paramref name="addresses"/> is <c>null</c>. Further, each empty address is 
        /// striped out.
        /// </remarks>
        /// <param name="collection">
        /// The mail address list to add items from <paramref name="addresses"/>.
        /// </param>
        /// <param name="addresses">
        /// The list of addresses to add to <paramref name="collection"/>.
        /// </param>
        private void AddAddresses(MailAddressCollection collection, IEnumerable<String> addresses)
        {
            if (addresses == null)
            {
                return;
            }

            foreach (String address in addresses.Where(x => !String.IsNullOrWhiteSpace(x)))
            {
                collection.Add(address.Trim());
            }
        }

        #endregion
    }
}
