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

using Moq;
using NUnit.Framework;
using Plexdata.LogWriter.Abstraction;
using Plexdata.LogWriter.Definitions;
using Plexdata.LogWriter.Settings;
using Plexdata.Utilities.Testing;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;

namespace Plexdata.LogWriter.Mail.Tests.Settings
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    [Category(TestType.UnitTest)]
    [TestOf(nameof(MailLoggerSettings))]
    public class MailLoggerSettingsTests
    {
        #region Prologue

        private Mock<ILoggerSettingsSection> section;
        private Mock<ILoggerSettingsSection> configuration;

        [SetUp]
        public void Setup()
        {
            this.section = new Mock<ILoggerSettingsSection>();
            this.configuration = new Mock<ILoggerSettingsSection>();

            this.configuration
                .Setup(x => x.GetSection(It.IsAny<String>()))
                .Returns(this.section.Object);
        }

        [TearDown]
        public void Cleanup()
        {
        }

        #endregion

        #region Construction

        [Test]
        public void MailLoggerSettings_ValidateDefaultSettings_DefaultSettingsAsExpected()
        {
            MailLoggerSettings instance = new MailLoggerSettings();

            Assert.That(instance.LogType, Is.EqualTo(LogType.Raw));
            Assert.That(instance.Address, Is.Empty);
            Assert.That(instance.Username, Is.Empty);
            Assert.That(instance.Password, Is.Empty);
            Assert.That(instance.SmtpHost, Is.Empty);
            Assert.That(instance.SmtpPort, Is.EqualTo(587));
            Assert.That(instance.UseSsl, Is.True);
            Assert.That(instance.Encoding, Is.EqualTo(Encoding.UTF8));
            Assert.That(instance.Subject, Is.Empty);
            Assert.That(instance.Receivers, Is.Empty);
            Assert.That(instance.ClearCopies, Is.Empty);
            Assert.That(instance.BlindCopies, Is.Empty);
        }

        [Test]
        public void MailLoggerSettings_ValidateDefaultSettingsForInvalidConfiguration_DefaultSettingsAsExpected()
        {
            MailLoggerSettings instance = new MailLoggerSettings((ILoggerSettingsSection)null);

            Assert.That(instance.LogType, Is.EqualTo(LogType.Raw));
            Assert.That(instance.Address, Is.Empty);
            Assert.That(instance.Username, Is.Empty);
            Assert.That(instance.Password, Is.Empty);
            Assert.That(instance.SmtpHost, Is.Empty);
            Assert.That(instance.SmtpPort, Is.EqualTo(587));
            Assert.That(instance.UseSsl, Is.True);
            Assert.That(instance.Encoding, Is.EqualTo(Encoding.UTF8));
            Assert.That(instance.Subject, Is.Empty);
            Assert.That(instance.Receivers, Is.Empty);
            Assert.That(instance.ClearCopies, Is.Empty);
            Assert.That(instance.BlindCopies, Is.Empty);
        }

        [Test]
        public void MailLoggerSettings_ConfigurationValid_GetSectionCalledAsExpected()
        {
            MailLoggerSettings instance = new MailLoggerSettings(this.configuration.Object);

            this.configuration.Verify(x => x.GetSection("Plexdata:LogWriter:Settings"), Times.Exactly(2));
        }

        [Test]
        public void MailLoggerSettings_ConfigurationValid_GetSectionValueForLogTypeIsDefault()
        {
            this.section.SetupGet(x => x["LogType"]).Returns((String)null);

            MailLoggerSettings instance = new MailLoggerSettings(this.configuration.Object);

            Assert.That(instance.LogType, Is.EqualTo(LogType.Raw));
        }

        [TestCaseSource(nameof(EncodingTestItemList))]
        public void MailLoggerSettings_ConfigurationValid_GetSectionValueForEncodingAsExpected(Object current)
        {
            EncodingTestItem nominee = (EncodingTestItem)current;

            this.section.SetupGet(x => x["Encoding"]).Returns(nominee.Value);

            MailLoggerSettings instance = new MailLoggerSettings(this.configuration.Object);

            Assert.That(instance.Encoding.BodyName, Is.EqualTo(nominee.Result.BodyName));
        }

        #endregion

        #region Address

        [Test]
        public void Address_ValueChanged_PropertyChangedFired()
        {
            Boolean fired = false;
            MailLoggerSettings instance = new MailLoggerSettings();

            instance.Address = "Address1";
            instance.PropertyChanged += (sender, args) => { fired = true; };
            instance.Address = "Address2";

            Assert.That(fired, Is.True);
        }

        [Test]
        public void Address_ValueUnchanged_PropertyChangedNotFired()
        {
            Boolean fired = false;
            MailLoggerSettings instance = new MailLoggerSettings();

            instance.Address = "Address1";
            instance.PropertyChanged += (sender, args) => { fired = true; };
            instance.Address = "Address1";

            Assert.That(fired, Is.False);
        }

        #endregion

        #region Username

        [Test]
        public void Username_ValueChanged_PropertyChangedFired()
        {
            Boolean fired = false;
            MailLoggerSettings instance = new MailLoggerSettings();

            instance.Username = "Username1";
            instance.PropertyChanged += (sender, args) => { fired = true; };
            instance.Username = "Username2";

            Assert.That(fired, Is.True);
        }

        [Test]
        public void Username_ValueUnchanged_PropertyChangedNotFired()
        {
            Boolean fired = false;
            MailLoggerSettings instance = new MailLoggerSettings();

            instance.Username = "Username1";
            instance.PropertyChanged += (sender, args) => { fired = true; };
            instance.Username = "Username1";

            Assert.That(fired, Is.False);
        }

        #endregion

        #region Password

        [Test]
        public void Password_ValueChanged_PropertyChangedFired()
        {
            Boolean fired = false;
            MailLoggerSettings instance = new MailLoggerSettings();

            instance.Password = "Password1";
            instance.PropertyChanged += (sender, args) => { fired = true; };
            instance.Password = "Password2";

            Assert.That(fired, Is.True);
        }

        [Test]
        public void Password_ValueUnchanged_PropertyChangedNotFired()
        {
            Boolean fired = false;
            MailLoggerSettings instance = new MailLoggerSettings();

            instance.Password = "Password1";
            instance.PropertyChanged += (sender, args) => { fired = true; };
            instance.Password = "Password1";

            Assert.That(fired, Is.False);
        }

        #endregion

        #region SmtpHost

        [Test]
        public void SmtpHost_ValueChanged_PropertyChangedFired()
        {
            Boolean fired = false;
            MailLoggerSettings instance = new MailLoggerSettings();

            instance.SmtpHost = "SmtpHost1";
            instance.PropertyChanged += (sender, args) => { fired = true; };
            instance.SmtpHost = "SmtpHost2";

            Assert.That(fired, Is.True);
        }

        [Test]
        public void SmtpHost_ValueUnchanged_PropertyChangedNotFired()
        {
            Boolean fired = false;
            MailLoggerSettings instance = new MailLoggerSettings();

            instance.SmtpHost = "SmtpHost1";
            instance.PropertyChanged += (sender, args) => { fired = true; };
            instance.SmtpHost = "SmtpHost1";

            Assert.That(fired, Is.False);
        }

        #endregion

        #region SmtpPort

        [Test]
        public void SmtpPort_ValueChanged_PropertyChangedFired()
        {
            Boolean fired = false;
            MailLoggerSettings instance = new MailLoggerSettings();

            instance.SmtpPort = 1;
            instance.PropertyChanged += (sender, args) => { fired = true; };
            instance.SmtpPort = 2;

            Assert.That(fired, Is.True);
        }

        [Test]
        public void SmtpPort_ValueUnchanged_PropertyChangedNotFired()
        {
            Boolean fired = false;
            MailLoggerSettings instance = new MailLoggerSettings();

            instance.SmtpPort = 1;
            instance.PropertyChanged += (sender, args) => { fired = true; };
            instance.SmtpPort = 1;

            Assert.That(fired, Is.False);
        }

        #endregion

        #region UseSsl

        [Test]
        public void UseSsl_ValueChanged_PropertyChangedFired()
        {
            Boolean fired = false;
            MailLoggerSettings instance = new MailLoggerSettings();

            instance.UseSsl = false;
            instance.PropertyChanged += (sender, args) => { fired = true; };
            instance.UseSsl = true;

            Assert.That(fired, Is.True);
        }

        [Test]
        public void UseSsl_ValueUnchanged_PropertyChangedNotFired()
        {
            Boolean fired = false;
            MailLoggerSettings instance = new MailLoggerSettings();

            instance.UseSsl = true;
            instance.PropertyChanged += (sender, args) => { fired = true; };
            instance.UseSsl = true;

            Assert.That(fired, Is.False);
        }

        #endregion

        #region Encoding

        [Test]
        public void Encoding_ValueChanged_PropertyChangedFired()
        {
            Boolean fired = false;
            MailLoggerSettings instance = new MailLoggerSettings();

            instance.Encoding = Encoding.UTF7;
            instance.PropertyChanged += (sender, args) => { fired = true; };
            instance.Encoding = Encoding.UTF32;

            Assert.That(fired, Is.True);
        }

        [Test]
        public void Encoding_ValueUnchanged_PropertyChangedNotFired()
        {
            Boolean fired = false;
            MailLoggerSettings instance = new MailLoggerSettings();

            instance.Encoding = Encoding.ASCII;
            instance.PropertyChanged += (sender, args) => { fired = true; };
            instance.Encoding = Encoding.ASCII;

            Assert.That(fired, Is.False);
        }

        #endregion

        #region Subject

        [Test]
        public void Subject_ValueChanged_PropertyChangedFired()
        {
            Boolean fired = false;
            MailLoggerSettings instance = new MailLoggerSettings();

            instance.Subject = "Subject1";
            instance.PropertyChanged += (sender, args) => { fired = true; };
            instance.Subject = "Subject2";

            Assert.That(fired, Is.True);
        }

        [Test]
        public void Subject_ValueUnchanged_PropertyChangedNotFired()
        {
            Boolean fired = false;
            MailLoggerSettings instance = new MailLoggerSettings();

            instance.Subject = "Subject1";
            instance.PropertyChanged += (sender, args) => { fired = true; };
            instance.Subject = "Subject1";

            Assert.That(fired, Is.False);
        }

        #endregion

        #region Receivers

        [Test]
        public void Receivers_ValueChanged_PropertyChangedFired()
        {
            Boolean fired = false;
            MailLoggerSettings instance = new MailLoggerSettings();

            instance.Receivers = new List<String>() { "Receivers1", "Receivers2", "Receivers3" };
            instance.PropertyChanged += (sender, args) => { fired = true; };
            instance.Receivers = new List<String>() { "Receivers1", "Receivers2", "ReceiversA" };

            Assert.That(fired, Is.True);
        }

        [Test]
        public void Receivers_ValueUnchanged_PropertyChangedNotFired()
        {
            Boolean fired = false;
            MailLoggerSettings instance = new MailLoggerSettings();

            instance.Receivers = new List<String>() { "Receivers1", "Receivers2", "Receivers3" };
            instance.PropertyChanged += (sender, args) => { fired = true; };
            instance.Receivers = new List<String>() { "Receivers1", "Receivers2", "Receivers3" };

            Assert.That(fired, Is.False);
        }

        [Test]
        public void Receivers_ValueSetToNull_PropertyIsEmpty()
        {
            MailLoggerSettings instance = new MailLoggerSettings();

            instance.Receivers = new List<String>() { "Receivers1", "Receivers2", "Receivers3" };
            instance.Receivers = null;

            Assert.That(instance.Receivers, Is.Empty);
        }

        [Test]
        public void Receivers_ValueSetToEmpty_PropertyIsEmpty()
        {
            MailLoggerSettings instance = new MailLoggerSettings();

            instance.Receivers = new List<String>() { "Receivers1", "Receivers2", "Receivers3" };
            instance.Receivers = new List<String>();

            Assert.That(instance.Receivers, Is.Empty);
        }

        [Test]
        public void Receivers_ValueSetToEmptyItems_PropertyIsEmpty([Values(null, "", " ")] String item1, [Values(null, "", " ")] String item2, [Values(null, "", " ")] String item3)
        {
            MailLoggerSettings instance = new MailLoggerSettings();

            instance.Receivers = new List<String>() { "Receivers1", "Receivers2", "Receivers3" };
            instance.Receivers = new List<String>() { item1, item2, item3 };

            Assert.That(instance.Receivers, Is.Empty);
        }

        [Test]
        public void Receivers_ValueSetToMixedItems_PropertyAsExpected([Values(null, "", " ")] String item2)
        {
            List<String> expected = new List<String>() { "ReceiversA", "ReceiversC" };
            MailLoggerSettings instance = new MailLoggerSettings();

            instance.Receivers = new List<String>() { "Receivers1", "Receivers2", "Receivers3" };
            instance.Receivers = new List<String>() { "ReceiversA", item2, "ReceiversC" };

            Assert.That(instance.Receivers.SequenceEqual(expected), Is.True);
        }

        #endregion

        #region ClearCopies

        [Test]
        public void ClearCopies_ValueChanged_PropertyChangedFired()
        {
            Boolean fired = false;
            MailLoggerSettings instance = new MailLoggerSettings();

            instance.ClearCopies = new List<String>() { "ClearCopies1", "ClearCopies2", "ClearCopies3" };
            instance.PropertyChanged += (sender, args) => { fired = true; };
            instance.ClearCopies = new List<String>() { "ClearCopies1", "ClearCopies2", "ClearCopiesA" };

            Assert.That(fired, Is.True);
        }

        [Test]
        public void ClearCopies_ValueUnchanged_PropertyChangedNotFired()
        {
            Boolean fired = false;
            MailLoggerSettings instance = new MailLoggerSettings();

            instance.ClearCopies = new List<String>() { "ClearCopies1", "ClearCopies2", "ClearCopies3" };
            instance.PropertyChanged += (sender, args) => { fired = true; };
            instance.ClearCopies = new List<String>() { "ClearCopies1", "ClearCopies2", "ClearCopies3" };

            Assert.That(fired, Is.False);
        }

        [Test]
        public void ClearCopies_ValueSetToNull_PropertyIsEmpty()
        {
            MailLoggerSettings instance = new MailLoggerSettings();

            instance.ClearCopies = new List<String>() { "ClearCopies1", "ClearCopies2", "ClearCopies3" };
            instance.ClearCopies = null;

            Assert.That(instance.ClearCopies, Is.Empty);
        }

        [Test]
        public void ClearCopies_ValueSetToEmpty_PropertyIsEmpty()
        {
            MailLoggerSettings instance = new MailLoggerSettings();

            instance.ClearCopies = new List<String>() { "ClearCopies1", "ClearCopies2", "ClearCopies3" };
            instance.ClearCopies = new List<String>();

            Assert.That(instance.ClearCopies, Is.Empty);
        }

        [Test]
        public void ClearCopies_ValueSetToEmptyItems_PropertyIsEmpty([Values(null, "", " ")] String item1, [Values(null, "", " ")] String item2, [Values(null, "", " ")] String item3)
        {
            MailLoggerSettings instance = new MailLoggerSettings();

            instance.ClearCopies = new List<String>() { "ClearCopies1", "ClearCopies2", "ClearCopies3" };
            instance.ClearCopies = new List<String>() { item1, item2, item3 };

            Assert.That(instance.ClearCopies, Is.Empty);
        }

        [Test]
        public void ClearCopies_ValueSetToMixedItems_PropertyAsExpected([Values(null, "", " ")] String item2)
        {
            List<String> expected = new List<String>() { "ClearCopiesA", "ClearCopiesC" };
            MailLoggerSettings instance = new MailLoggerSettings();

            instance.ClearCopies = new List<String>() { "ClearCopies1", "ClearCopies2", "ClearCopies3" };
            instance.ClearCopies = new List<String>() { "ClearCopiesA", item2, "ClearCopiesC" };

            Assert.That(instance.ClearCopies.SequenceEqual(expected), Is.True);
        }

        #endregion

        #region BlindCopies

        [Test]
        public void BlindCopies_ValueChanged_PropertyChangedFired()
        {
            Boolean fired = false;
            MailLoggerSettings instance = new MailLoggerSettings();

            instance.BlindCopies = new List<String>() { "BlindCopies1", "BlindCopies2", "BlindCopies3" };
            instance.PropertyChanged += (sender, args) => { fired = true; };
            instance.BlindCopies = new List<String>() { "BlindCopies1", "BlindCopies2", "BlindCopiesA" };

            Assert.That(fired, Is.True);
        }

        [Test]
        public void BlindCopies_ValueUnchanged_PropertyChangedNotFired()
        {
            Boolean fired = false;
            MailLoggerSettings instance = new MailLoggerSettings();

            instance.BlindCopies = new List<String>() { "BlindCopies1", "BlindCopies2", "BlindCopies3" };
            instance.PropertyChanged += (sender, args) => { fired = true; };
            instance.BlindCopies = new List<String>() { "BlindCopies1", "BlindCopies2", "BlindCopies3" };

            Assert.That(fired, Is.False);
        }

        [Test]
        public void BlindCopies_ValueSetToNull_PropertyIsEmpty()
        {
            MailLoggerSettings instance = new MailLoggerSettings();

            instance.BlindCopies = new List<String>() { "BlindCopies1", "BlindCopies2", "BlindCopies3" };
            instance.BlindCopies = null;

            Assert.That(instance.BlindCopies, Is.Empty);
        }

        [Test]
        public void BlindCopies_ValueSetToEmpty_PropertyIsEmpty()
        {
            MailLoggerSettings instance = new MailLoggerSettings();

            instance.BlindCopies = new List<String>() { "BlindCopies1", "BlindCopies2", "BlindCopies3" };
            instance.BlindCopies = new List<String>();

            Assert.That(instance.BlindCopies, Is.Empty);
        }

        [Test]
        public void BlindCopies_ValueSetToEmptyItems_PropertyIsEmpty([Values(null, "", " ")] String item1, [Values(null, "", " ")] String item2, [Values(null, "", " ")] String item3)
        {
            MailLoggerSettings instance = new MailLoggerSettings();

            instance.BlindCopies = new List<String>() { "BlindCopies1", "BlindCopies2", "BlindCopies3" };
            instance.BlindCopies = new List<String>() { item1, item2, item3 };

            Assert.That(instance.BlindCopies, Is.Empty);
        }

        [Test]
        public void BlindCopies_ValueSetToMixedItems_PropertyAsExpected([Values(null, "", " ")] String item2)
        {
            List<String> expected = new List<String>() { "BlindCopiesA", "BlindCopiesC" };
            MailLoggerSettings instance = new MailLoggerSettings();

            instance.BlindCopies = new List<String>() { "BlindCopies1", "BlindCopies2", "BlindCopies3" };
            instance.BlindCopies = new List<String>() { "BlindCopiesA", item2, "BlindCopiesC" };

            Assert.That(instance.BlindCopies.SequenceEqual(expected), Is.True);
        }

        #endregion

        #region Other privates

        private class EncodingTestItem
        {
            public EncodingTestItem(String value, Encoding result)
            {
                this.Value = value;
                this.Result = result;
            }

            public String Value { get; private set; }

            public Encoding Result { get; private set; }

            public override String ToString()
            {
                return $"Value=\"{(this.Value ?? "<null>")}\", Result=\"{this.Result.BodyName}\"";
            }
        }

        public static Object[] EncodingTestItemList = new Object[] {
            new EncodingTestItem(null,          Encoding.UTF8),
            new EncodingTestItem("",            Encoding.UTF8),
            new EncodingTestItem(" ",           Encoding.UTF8),
            new EncodingTestItem("invalid",     Encoding.UTF8),
            new EncodingTestItem("utf-7",       Encoding.UTF7),
            new EncodingTestItem("utf-16be",    Encoding.BigEndianUnicode),
            new EncodingTestItem("utf-16",      Encoding.Unicode),
            new EncodingTestItem("iso-8859-1",  Encoding.Default),
            new EncodingTestItem("iso-8859-15", Encoding.GetEncoding("iso-8859-15")),
            new EncodingTestItem("us-ascii",    Encoding.ASCII),
            new EncodingTestItem("utf-8",       Encoding.UTF8),
            new EncodingTestItem("utf-32",      Encoding.UTF32),
            new EncodingTestItem("UTF-7",       Encoding.UTF7),
            new EncodingTestItem("UTF-16BE",    Encoding.BigEndianUnicode),
            new EncodingTestItem("UTF-16",      Encoding.Unicode),
            new EncodingTestItem("ISO-8859-1",  Encoding.Default),
            new EncodingTestItem("US-ASCII",    Encoding.ASCII),
            new EncodingTestItem("UTF-8",       Encoding.UTF8),
            new EncodingTestItem("UTF-32",      Encoding.UTF32),
            new EncodingTestItem("ISO-8859-15", Encoding.GetEncoding("iso-8859-15")),
        };

        #endregion
    }
}
