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

using NUnit.Framework;
using Plexdata.LogWriter.Facades;
using Plexdata.Utilities.Testing;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Net.Mail;
using System.Reflection;
using System.Text;

namespace Plexdata.LogWriter.Mail.Tests.Facades
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    [Category(TestType.IntegrationTest)]
    [TestOf(nameof(MailLoggerFacade))]
    public class MailLoggerFacadeTests
    {
        #region Prologue

        private MailLoggerFacade instance = null;

        [SetUp]
        public void Setup()
        {
            this.instance = new MailLoggerFacade();
        }

        #endregion

        #region Write

        [Test]
        public void Write_EncodingIsNull_NothingProcessed()
        {
            this.instance.Write("address", new String[] { "addr" }, new String[] { "copy" }, new String[] { "blind" }, null, "subject", "content");
        }

        [Test]
        public void Write_AddressInvalid_NothingProcessed([Values(null, "", " ")] String address)
        {
            this.instance.Write(address, new String[] { "addr" }, new String[] { "copy" }, new String[] { "blind" }, Encoding.UTF8, "subject", "content");
        }

        [Test]
        public void Write_SubjectInvalid_NothingProcessed([Values(null, "", " ")] String subject)
        {
            this.instance.Write("address", new String[] { "addr" }, new String[] { "copy" }, new String[] { "blind" }, Encoding.UTF8, subject, "content");
        }

        [Test]
        public void Write_ContentInvalid_NothingProcessed([Values(null, "", " ")] String content)
        {
            this.instance.Write("address", new String[] { "addr" }, new String[] { "copy" }, new String[] { "blind" }, Encoding.UTF8, "subject", content);
        }

        [Test]
        public void Write_AllRecipientsNull_NothingProcessed()
        {
            this.instance.Write("address", null, null, null, Encoding.UTF8, "subject", "content");
        }

        [Test]
        public void Write_AllRecipientsEmpty_NothingProcessed()
        {
            this.instance.Write("address", new String[0], new String[0], new String[0], Encoding.UTF8, "subject", "content");
        }

        [Test]
        public void Write_AllRecipientsInvalid_NothingProcessed([Values(null, "", " ")] String addr1, [Values(null, "", " ")] String addr2, [Values(null, "", " ")] String addr3)
        {
            this.instance.Write("address", new String[] { addr1 }, new String[] { addr2 }, new String[] { addr3 }, Encoding.UTF8, "subject", "content");
        }

        #endregion

        #region CreateMessage

        [Test]
        public void CreateMessage_ParameterValid_PropertiesInitializedAsExpected()
        {
            String address = "address@example.org";
            IEnumerable<String> receivers = new List<String>() { "address@example.org" };
            IEnumerable<String> clearCopies = null;
            IEnumerable<String> blindCopies = null;
            Encoding encoding = Encoding.ASCII;
            String subject = "subject";
            String content = "content";

            MethodInfo method = this.instance.GetType().GetMethod("CreateMessage", BindingFlags.NonPublic | BindingFlags.Instance);
            using (MailMessage actual = method.Invoke(this.instance, new Object[] { address, receivers, clearCopies, blindCopies, encoding, subject, content }) as MailMessage)
            {
                Assert.That(actual.IsBodyHtml, Is.False);
                Assert.That(actual.From.Address, Is.EqualTo(address));
                Assert.That(actual.Subject, Is.EqualTo(subject));
                Assert.That(actual.SubjectEncoding, Is.EqualTo(encoding));
                Assert.That(actual.Body, Is.EqualTo(content));
                Assert.That(actual.BodyEncoding, Is.EqualTo(encoding));
                Assert.That(actual.To.Count, Is.EqualTo(1));
                Assert.That(actual.CC, Is.Empty);
                Assert.That(actual.Bcc, Is.Empty);
            }
        }

        #endregion

        #region AddAddresses

        [Test]
        public void AddAddresses_AddressListIsNull_MailAddressCollectionCountIsZero()
        {
            MailAddressCollection collection = new MailAddressCollection();
            IEnumerable<String> addresses = null;

            MethodInfo method = this.instance.GetType().GetMethod("AddAddresses", BindingFlags.NonPublic | BindingFlags.Instance);
            method.Invoke(this.instance, new Object[] { collection, addresses });

            Assert.That(collection.Count, Is.Zero);
        }

        [Test]
        public void AddAddresses_AddressListWithInvalidAddresses_MailAddressCollectionCountIsZero()
        {
            MailAddressCollection collection = new MailAddressCollection();
            IEnumerable<String> addresses = new String[] { null, "", " " };

            MethodInfo method = this.instance.GetType().GetMethod("AddAddresses", BindingFlags.NonPublic | BindingFlags.Instance);
            method.Invoke(this.instance, new Object[] { collection, addresses });

            Assert.That(collection.Count, Is.Zero);
        }

        [Test]
        public void AddAddresses_AddressListWithOneValidAddress_MailAddressCollectionCountIsOne()
        {
            MailAddressCollection collection = new MailAddressCollection();
            IEnumerable<String> addresses = new String[] { null, "", " ", "address@example.org" };

            MethodInfo method = this.instance.GetType().GetMethod("AddAddresses", BindingFlags.NonPublic | BindingFlags.Instance);
            method.Invoke(this.instance, new Object[] { collection, addresses });

            Assert.That(collection.Count, Is.EqualTo(1));
        }

        #endregion
    }
}
