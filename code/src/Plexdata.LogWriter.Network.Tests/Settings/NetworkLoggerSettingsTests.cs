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
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Plexdata.LogWriter.Network.Tests.Settings
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    [Category(TestType.UnitTest)]
    [TestOf(nameof(NetworkLoggerSettings))]
    public class NetworkLoggerSettingsTests
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

            this.section
                .Setup(x => x.GetSection(It.IsAny<String>()))
                .Returns(this.section.Object);
        }

        #endregion

        #region NetworkLoggerSettings

        [Test]
        public void NetworkLoggerSettings_ValidateDefaultSettings_DefaultSettingsAsExpected()
        {
            NetworkLoggerSettings instance = new NetworkLoggerSettings();

            Assert.That(instance.Host, Is.EqualTo("localhost"));
            Assert.That(instance.Port, Is.EqualTo(12201));
            Assert.That(instance.Address, Is.EqualTo(Address.Default));
            Assert.That(instance.Protocol, Is.EqualTo(Protocol.Default));
            Assert.That(instance.Encoding, Is.EqualTo(Encoding.UTF8));
            Assert.That(instance.Compressed, Is.True);
            Assert.That(instance.Threshold, Is.EqualTo(512));
            Assert.That(instance.Maximum, Is.EqualTo(8192));
            Assert.That(instance.Termination, Is.False);
            Assert.That(instance.Timeout, Is.EqualTo(100000));
        }

        [Test]
        public void NetworkLoggerSettings_ValidateDefaultSettingsForInvalidConfiguration_DefaultSettingsAsExpected()
        {
            NetworkLoggerSettings instance = new NetworkLoggerSettings(null);

            Assert.That(instance.Host, Is.EqualTo("localhost"));
            Assert.That(instance.Port, Is.EqualTo(12201));
            Assert.That(instance.Address, Is.EqualTo(Address.Default));
            Assert.That(instance.Protocol, Is.EqualTo(Protocol.Default));
            Assert.That(instance.Encoding, Is.EqualTo(Encoding.UTF8));
            Assert.That(instance.Compressed, Is.True);
            Assert.That(instance.Threshold, Is.EqualTo(512));
            Assert.That(instance.Maximum, Is.EqualTo(8192));
            Assert.That(instance.Termination, Is.False);
            Assert.That(instance.Timeout, Is.EqualTo(100000));
        }

        [Test]
        public void NetworkLoggerSettings_ConfigurationValid_GetSectionCalledAsExpected()
        {
            NetworkLoggerSettings instance = new NetworkLoggerSettings(this.configuration.Object);

            this.configuration.Verify(x => x.GetSection("Plexdata:LogWriter:Settings"), Times.Exactly(2));
        }

        [Test]
        public void NetworkLoggerSettings_ConfigurationValid_GetSectionValueForLogTypeIsDefault()
        {
            this.section.SetupGet(x => x["LogType"]).Returns((String)null);

            NetworkLoggerSettings instance = new NetworkLoggerSettings(this.configuration.Object);

            Assert.That(instance.LogType, Is.EqualTo(LogType.Gelf));
        }

        [TestCase(null, "localhost")]
        [TestCase("", "localhost")]
        [TestCase(" ", "localhost")]
        [TestCase("some-host-name", "some-host-name")]
        public void NetworkLoggerSettings_ConfigurationValid_GetSectionValueForHostAsExpected(String value, Object expected)
        {
            this.section.SetupGet(x => x["Host"]).Returns(value);

            NetworkLoggerSettings instance = new NetworkLoggerSettings(this.configuration.Object);

            Assert.That(instance.Host, Is.EqualTo(expected));
        }

        [TestCase(null, 12201)]
        [TestCase("", 12201)]
        [TestCase(" ", 12201)]
        [TestCase("invalid", 12201)]
        [TestCase("-1", -1)]
        [TestCase("0", 0)]
        [TestCase("42", 42)]
        [TestCase("42.0", 12201)]
        public void NetworkLoggerSettings_ConfigurationValid_GetSectionValueForPortAsExpected(String value, Object expected)
        {
            this.section.SetupGet(x => x["Port"]).Returns(value);

            NetworkLoggerSettings instance = new NetworkLoggerSettings(this.configuration.Object);

            Assert.That(instance.Port, Is.EqualTo(expected));
        }

        [TestCase(null, Address.Default)]
        [TestCase("", Address.Default)]
        [TestCase(" ", Address.Default)]
        [TestCase("invalid", Address.Default)]
        [TestCase("Unknown", Address.Unknown)]
        [TestCase("unknown", Address.Unknown)]
        [TestCase("IPv4", Address.IPv4)]
        [TestCase("ipv4", Address.IPv4)]
        [TestCase("IPv6", Address.IPv6)]
        [TestCase("ipv6", Address.IPv6)]
        public void NetworkLoggerSettings_ConfigurationValid_GetSectionValueForAddressAsExpected(String value, Object expected)
        {
            this.section.SetupGet(x => x["Address"]).Returns(value);

            NetworkLoggerSettings instance = new NetworkLoggerSettings(this.configuration.Object);

            Assert.That(instance.Address, Is.EqualTo(expected));
        }

        [TestCase(null, Protocol.Default)]
        [TestCase("", Protocol.Default)]
        [TestCase(" ", Protocol.Default)]
        [TestCase("invalid", Protocol.Default)]
        [TestCase("Unknown", Protocol.Unknown)]
        [TestCase("unknown", Protocol.Unknown)]
        [TestCase("Udp", Protocol.Udp)]
        [TestCase("udp", Protocol.Udp)]
        [TestCase("Tcp", Protocol.Tcp)]
        [TestCase("tcp", Protocol.Tcp)]
        [TestCase("Web", Protocol.Web)]
        [TestCase("web", Protocol.Web)]
        public void NetworkLoggerSettings_ConfigurationValid_GetSectionValueForProtocolAsExpected(String value, Object expected)
        {
            this.section.SetupGet(x => x["Protocol"]).Returns(value);

            NetworkLoggerSettings instance = new NetworkLoggerSettings(this.configuration.Object);

            Assert.That(instance.Protocol, Is.EqualTo(expected));
        }

        [TestCaseSource(nameof(EncodingTestItemList))]
        public void NetworkLoggerSettings_ConfigurationValid_GetSectionValueForEncodingAsExpected(Object current)
        {
            EncodingTestItem nominee = (EncodingTestItem)current;

            this.section.SetupGet(x => x["Encoding"]).Returns(nominee.Value);

            NetworkLoggerSettings instance = new NetworkLoggerSettings(this.configuration.Object);

            Assert.That(instance.Encoding.BodyName, Is.EqualTo(nominee.Result.BodyName));
        }

        [TestCase(null, true)]
        [TestCase("", true)]
        [TestCase(" ", true)]
        [TestCase("invalid", true)]
        [TestCase("true", true)]
        [TestCase("TRUE", true)]
        [TestCase("True", true)]
        [TestCase("false", false)]
        [TestCase("FALSE", false)]
        [TestCase("False", false)]
        public void NetworkLoggerSettings_ConfigurationValid_GetSectionValueForCompressedAsExpected(String value, Object expected)
        {
            this.section.SetupGet(x => x["Compressed"]).Returns(value);

            NetworkLoggerSettings instance = new NetworkLoggerSettings(this.configuration.Object);

            Assert.That(instance.Compressed, Is.EqualTo(expected));
        }

        [TestCase(null, 512)]
        [TestCase("", 512)]
        [TestCase(" ", 512)]
        [TestCase("invalid", 512)]
        [TestCase("-1", -1)]
        [TestCase("0", 0)]
        [TestCase("42", 42)]
        [TestCase("42.0", 512)]
        public void NetworkLoggerSettings_ConfigurationValid_GetSectionValueForThresholdAsExpected(String value, Object expected)
        {
            this.section.SetupGet(x => x["Threshold"]).Returns(value);

            NetworkLoggerSettings instance = new NetworkLoggerSettings(this.configuration.Object);

            Assert.That(instance.Threshold, Is.EqualTo(expected));
        }

        [TestCase(null, 8192)]
        [TestCase("", 8192)]
        [TestCase(" ", 8192)]
        [TestCase("invalid", 8192)]
        [TestCase("-1", -1)]
        [TestCase("0", 0)]
        [TestCase("42", 42)]
        [TestCase("42.0", 8192)]
        public void NetworkLoggerSettings_ConfigurationValid_GetSectionValueForMaximumAsExpected(String value, Object expected)
        {
            this.section.SetupGet(x => x["Maximum"]).Returns(value);

            NetworkLoggerSettings instance = new NetworkLoggerSettings(this.configuration.Object);

            Assert.That(instance.Maximum, Is.EqualTo(expected));
        }

        [TestCase(null, false)]
        [TestCase("", false)]
        [TestCase(" ", false)]
        [TestCase("invalid", false)]
        [TestCase("true", true)]
        [TestCase("TRUE", true)]
        [TestCase("True", true)]
        [TestCase("false", false)]
        [TestCase("FALSE", false)]
        [TestCase("False", false)]
        public void NetworkLoggerSettings_ConfigurationValid_GetSectionValueForTerminationAsExpected(String value, Object expected)
        {
            this.section.SetupGet(x => x["Termination"]).Returns(value);

            NetworkLoggerSettings instance = new NetworkLoggerSettings(this.configuration.Object);

            Assert.That(instance.Termination, Is.EqualTo(expected));
        }

        [TestCase(null, 100000)]
        [TestCase("", 100000)]
        [TestCase(" ", 100000)]
        [TestCase("invalid", 100000)]
        [TestCase("100000", 100000)]
        [TestCase("1000", 1000)]
        [TestCase("-1", -1)]
        [TestCase("0", 0)]
        [TestCase("-4711", -4711)]
        public void NetworkLoggerSettings_ConfigurationValid_GetSectionValueForTimeoutAsExpected(String value, Object expected)
        {
            this.section.SetupGet(x => x["Timeout"]).Returns(value);

            NetworkLoggerSettings instance = new NetworkLoggerSettings(this.configuration.Object);

            Assert.That(instance.Timeout, Is.EqualTo(expected));
        }

        #endregion

        #region Host

        [Test]
        public void Host_ValueChanged_PropertyChangedFired()
        {
            Boolean fired = false;
            NetworkLoggerSettings instance = new NetworkLoggerSettings();

            instance.Host = "host-1";
            instance.PropertyChanged += (sender, args) => { fired = true; };
            instance.Host = "host-2";

            Assert.That(fired, Is.True);
        }

        [Test]
        public void Host_ValueUnchanged_PropertyChangedNotFired()
        {
            Boolean fired = false;
            NetworkLoggerSettings instance = new NetworkLoggerSettings();

            instance.Host = "host-1";
            instance.PropertyChanged += (sender, args) => { fired = true; };
            instance.Host = "host-1";

            Assert.That(fired, Is.False);
        }

        #endregion

        #region Port

        [Test]
        public void Port_ValueChanged_PropertyChangedFired()
        {
            Boolean fired = false;
            NetworkLoggerSettings instance = new NetworkLoggerSettings();

            instance.Port = 23;
            instance.PropertyChanged += (sender, args) => { fired = true; };
            instance.Port = 42;

            Assert.That(fired, Is.True);
        }

        [Test]
        public void Port_ValueUnchanged_PropertyChangedNotFired()
        {
            Boolean fired = false;
            NetworkLoggerSettings instance = new NetworkLoggerSettings();

            instance.Port = 23;
            instance.PropertyChanged += (sender, args) => { fired = true; };
            instance.Port = 23;

            Assert.That(fired, Is.False);
        }

        #endregion

        #region Address

        [Test]
        public void Address_ValueChanged_PropertyChangedFired()
        {
            Boolean fired = false;
            NetworkLoggerSettings instance = new NetworkLoggerSettings();

            instance.Address = Address.Unknown;
            instance.PropertyChanged += (sender, args) => { fired = true; };
            instance.Address = Address.Default;

            Assert.That(fired, Is.True);
        }

        [Test]
        public void Address_ValueUnchanged_PropertyChangedNotFired()
        {
            Boolean fired = false;
            NetworkLoggerSettings instance = new NetworkLoggerSettings();

            instance.Address = Address.Default;
            instance.PropertyChanged += (sender, args) => { fired = true; };
            instance.Address = Address.Default;

            Assert.That(fired, Is.False);
        }

        #endregion

        #region Protocol

        [Test]
        public void Protocol_ValueChanged_PropertyChangedFired()
        {
            Boolean fired = false;
            NetworkLoggerSettings instance = new NetworkLoggerSettings();

            instance.Protocol = Protocol.Unknown;
            instance.PropertyChanged += (sender, args) => { fired = true; };
            instance.Protocol = Protocol.Default;

            Assert.That(fired, Is.True);
        }

        [Test]
        public void Protocol_ValueUnchanged_PropertyChangedNotFired()
        {
            Boolean fired = false;
            NetworkLoggerSettings instance = new NetworkLoggerSettings();

            instance.Protocol = Protocol.Default;
            instance.PropertyChanged += (sender, args) => { fired = true; };
            instance.Protocol = Protocol.Default;

            Assert.That(fired, Is.False);
        }

        #endregion

        #region Encoding

        [Test]
        public void Encoding_ValueChanged_PropertyChangedFired()
        {
            Boolean fired = false;
            NetworkLoggerSettings instance = new NetworkLoggerSettings();

            instance.Encoding = Encoding.UTF7;
            instance.PropertyChanged += (sender, args) => { fired = true; };
            instance.Encoding = Encoding.UTF32;

            Assert.That(fired, Is.True);
        }

        [Test]
        public void Encoding_ValueUnchanged_PropertyChangedNotFired()
        {
            Boolean fired = false;
            NetworkLoggerSettings instance = new NetworkLoggerSettings();

            instance.Encoding = Encoding.ASCII;
            instance.PropertyChanged += (sender, args) => { fired = true; };
            instance.Encoding = Encoding.ASCII;

            Assert.That(fired, Is.False);
        }

        #endregion

        #region Compressed

        [Test]
        public void Compressed_ValueChanged_PropertyChangedFired()
        {
            Boolean fired = false;
            NetworkLoggerSettings instance = new NetworkLoggerSettings();

            instance.Compressed = false;
            instance.PropertyChanged += (sender, args) => { fired = true; };
            instance.Compressed = true;

            Assert.That(fired, Is.True);
        }

        [Test]
        public void Compressed_ValueUnchanged_PropertyChangedNotFired()
        {
            Boolean fired = false;
            NetworkLoggerSettings instance = new NetworkLoggerSettings();

            instance.Compressed = true;
            instance.PropertyChanged += (sender, args) => { fired = true; };
            instance.Compressed = true;

            Assert.That(fired, Is.False);
        }

        #endregion

        #region Threshold

        [Test]
        public void Threshold_ValueChanged_PropertyChangedFired()
        {
            Boolean fired = false;
            NetworkLoggerSettings instance = new NetworkLoggerSettings();

            instance.Threshold = 23;
            instance.PropertyChanged += (sender, args) => { fired = true; };
            instance.Threshold = 42;

            Assert.That(fired, Is.True);
        }

        [Test]
        public void Threshold_ValueUnchanged_PropertyChangedNotFired()
        {
            Boolean fired = false;
            NetworkLoggerSettings instance = new NetworkLoggerSettings();

            instance.Threshold = 23;
            instance.PropertyChanged += (sender, args) => { fired = true; };
            instance.Threshold = 23;

            Assert.That(fired, Is.False);
        }

        #endregion

        #region Maximum

        [Test]
        public void Maximum_ValueChanged_PropertyChangedFired()
        {
            Boolean fired = false;
            NetworkLoggerSettings instance = new NetworkLoggerSettings();

            instance.Maximum = 23;
            instance.PropertyChanged += (sender, args) => { fired = true; };
            instance.Maximum = 42;

            Assert.That(fired, Is.True);
        }

        [Test]
        public void Maximum_ValueUnchanged_PropertyChangedNotFired()
        {
            Boolean fired = false;
            NetworkLoggerSettings instance = new NetworkLoggerSettings();

            instance.Maximum = 23;
            instance.PropertyChanged += (sender, args) => { fired = true; };
            instance.Maximum = 23;

            Assert.That(fired, Is.False);
        }

        #endregion

        #region Termination

        [Test]
        public void Termination_ValueChanged_PropertyChangedFired()
        {
            Boolean fired = false;
            NetworkLoggerSettings instance = new NetworkLoggerSettings();

            instance.Termination = false;
            instance.PropertyChanged += (sender, args) => { fired = true; };
            instance.Termination = true;

            Assert.That(fired, Is.True);
        }

        [Test]
        public void Termination_ValueUnchanged_PropertyChangedNotFired()
        {
            Boolean fired = false;
            NetworkLoggerSettings instance = new NetworkLoggerSettings();

            instance.Termination = true;
            instance.PropertyChanged += (sender, args) => { fired = true; };
            instance.Termination = true;

            Assert.That(fired, Is.False);
        }

        #endregion

        #region Timeout

        [Test]
        public void Timeout_ValueChanged_PropertyChangedFired()
        {
            Boolean fired = false;
            NetworkLoggerSettings instance = new NetworkLoggerSettings();

            instance.Timeout = 100000;
            instance.PropertyChanged += (sender, args) => { fired = true; };
            instance.Timeout = 100;

            Assert.That(fired, Is.True);
        }

        [Test]
        public void Timeout_ValueUnchanged_PropertyChangedNotFired()
        {
            Boolean fired = false;
            NetworkLoggerSettings instance = new NetworkLoggerSettings();

            instance.Timeout = 100000;
            instance.PropertyChanged += (sender, args) => { fired = true; };
            instance.Timeout = 100000;

            Assert.That(fired, Is.False);
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
