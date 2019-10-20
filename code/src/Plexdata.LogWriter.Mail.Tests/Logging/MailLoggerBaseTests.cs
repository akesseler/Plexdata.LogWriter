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

using Moq;
using NUnit.Framework;
using Plexdata.LogWriter.Abstraction;
using Plexdata.LogWriter.Definitions;
using Plexdata.LogWriter.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Plexdata.LogWriter.Mail.Tests.Logging
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    [TestOf(nameof(MailLoggerBase))]
    public class MailLoggerBaseTests
    {
        #region Prologue

        private TestClass instance = null;
        private Mock<IMailLoggerSettings> settings = null;
        private Mock<IMailLoggerFacade> facade = null;

        [SetUp]
        public void Setup()
        {
            this.settings = new Mock<IMailLoggerSettings>();
            this.facade = new Mock<IMailLoggerFacade>();

            this.settings.SetupGet(x => x.Username).Returns("username");
            this.settings.SetupGet(x => x.Password).Returns("password");
            this.settings.SetupGet(x => x.SmtpHost).Returns("smtp-host");
            this.settings.SetupGet(x => x.SmtpPort).Returns(42);
            this.settings.SetupGet(x => x.UseSsl).Returns(true);

            this.instance = new TestClass(this.settings.Object, this.facade.Object);
        }

        #endregion

        #region Construction

        [Test]
        public void MailLoggerBase_SettingsNull_ThrowsArgumentNullException()
        {
            Assert.That(() => new TestClass(null, this.facade.Object), Throws.ArgumentNullException);
        }

        [Test]
        public void MailLoggerBase_FacadeNull_ThrowsArgumentNullException()
        {
            Assert.That(() => new TestClass(this.settings.Object, null), Throws.ArgumentNullException);
        }

        [Test]
        public void MailLoggerBase_ParametersValid_FacadePropertiesApplied()
        {
            Mock<IMailLoggerSettings> settings = new Mock<IMailLoggerSettings>();
            Mock<IMailLoggerFacade> facade = new Mock<IMailLoggerFacade>();

            settings.SetupGet(x => x.Username).Returns("user");
            settings.SetupGet(x => x.Password).Returns("pass");
            settings.SetupGet(x => x.SmtpHost).Returns("host");
            settings.SetupGet(x => x.SmtpPort).Returns(23);
            settings.SetupGet(x => x.UseSsl).Returns(false);

            TestClass instance = new TestClass(settings.Object, facade.Object);

            facade.VerifySet(x => x.Username = "user", Times.Once);
            facade.VerifySet(x => x.Password = "pass", Times.Once);
            facade.VerifySet(x => x.SmtpHost = "host", Times.Once);
            facade.VerifySet(x => x.SmtpPort = 23, Times.Once);
            facade.VerifySet(x => x.UseSsl = false, Times.Once);
        }

        #endregion

        #region Write

        [Test]
        public void Write_CheckDisposedIsTrue_NothingIsExecuted()
        {
            this.instance.Dispose();

            this.instance.TestWrite(LogLevel.Message, null, null, null, null, null);

            this.facade.Verify(x => x.Write(
                It.IsAny<String>(),
                It.IsAny<IEnumerable<String>>(),
                It.IsAny<IEnumerable<String>>(),
                It.IsAny<IEnumerable<String>>(),
                It.IsAny<Encoding>(),
                It.IsAny<String>(),
                It.IsAny<String>()),
                Times.Never);
        }

        [Test]
        public void Write_CheckDisabledIsTrue_NothingIsExecuted()
        {
            this.settings.SetupGet(x => x.LogLevel).Returns(LogLevel.Disabled);

            this.instance.TestWrite(LogLevel.Message, null, null, null, null, null);

            this.facade.Verify(x => x.Write(
                It.IsAny<String>(),
                It.IsAny<IEnumerable<String>>(),
                It.IsAny<IEnumerable<String>>(),
                It.IsAny<IEnumerable<String>>(),
                It.IsAny<Encoding>(),
                It.IsAny<String>(),
                It.IsAny<String>()),
                Times.Never);
        }

        [Test]
        public void Write_CheckEnabledIsFalse_NothingIsExecuted()
        {
            this.settings.SetupGet(x => x.LogLevel).Returns(LogLevel.Critical);

            this.instance.TestWrite(LogLevel.Message, null, null, null, null, null);

            this.facade.Verify(x => x.Write(
                It.IsAny<String>(),
                It.IsAny<IEnumerable<String>>(),
                It.IsAny<IEnumerable<String>>(),
                It.IsAny<IEnumerable<String>>(),
                It.IsAny<Encoding>(),
                It.IsAny<String>(),
                It.IsAny<String>()),
                Times.Never);
        }

        [Test]
        public void Write_OutputIsEmpty_NothingIsExecuted()
        {
            this.settings.SetupGet(x => x.LogLevel).Returns(LogLevel.Trace);

            this.instance.TestWrite(LogLevel.Message, null, null, null, null, null);

            this.facade.Verify(x => x.Write(
                It.IsAny<String>(),
                It.IsAny<IEnumerable<String>>(),
                It.IsAny<IEnumerable<String>>(),
                It.IsAny<IEnumerable<String>>(),
                It.IsAny<Encoding>(),
                It.IsAny<String>(),
                It.IsAny<String>()),
                Times.Never);
        }

        [Test]
        public void Write_CheckCallFacadeWrite_FacadeWriteWasCalledOnce()
        {
            this.settings.SetupGet(x => x.LogLevel).Returns(LogLevel.Trace);
            this.settings.SetupGet(x => x.Encoding).Returns(Encoding.UTF8);

            this.instance.TestWrite(LogLevel.Message, null, null, "message", null, null);

            this.facade.Verify(x => x.Write(
                It.IsAny<String>(),
                It.IsAny<IEnumerable<String>>(),
                It.IsAny<IEnumerable<String>>(),
                It.IsAny<IEnumerable<String>>(),
                It.IsAny<Encoding>(),
                It.IsAny<String>(),
                It.IsAny<String>()),
                Times.Once);
        }

        [Test]
        [TestCase(null, "message")]
        [TestCase("", "message")]
        [TestCase(" ", "message")]
        public void Write_CheckCallFacadeWriteSubjectInvalid_FacadeWriteWasCalledWithMessage(String subject, String expected)
        {
            this.settings.SetupGet(x => x.LogLevel).Returns(LogLevel.Trace);
            this.settings.SetupGet(x => x.Encoding).Returns(Encoding.UTF8);
            this.settings.SetupGet(x => x.Subject).Returns(subject);

            this.instance.TestWrite(LogLevel.Message, null, null, expected, null, null);

            this.facade.Verify(x => x.Write(
                It.IsAny<String>(),
                It.IsAny<IEnumerable<String>>(),
                It.IsAny<IEnumerable<String>>(),
                It.IsAny<IEnumerable<String>>(),
                It.IsAny<Encoding>(),
                expected,
                It.IsAny<String>()),
                Times.Once);
        }

        [Test]
        [TestCase(null, "exception")]
        [TestCase("", "exception")]
        [TestCase(" ", "exception")]
        public void Write_CheckCallFacadeWriteSubjectInvalid_FacadeWriteWasCalledWithExceptionMessage(String subject, String expected)
        {
            this.settings.SetupGet(x => x.LogLevel).Returns(LogLevel.Trace);
            this.settings.SetupGet(x => x.Encoding).Returns(Encoding.UTF8);
            this.settings.SetupGet(x => x.Subject).Returns(subject);

            this.instance.TestWrite(LogLevel.Message, null, null, null, new Exception(expected), null);

            this.facade.Verify(x => x.Write(
                It.IsAny<String>(),
                It.IsAny<IEnumerable<String>>(),
                It.IsAny<IEnumerable<String>>(),
                It.IsAny<IEnumerable<String>>(),
                It.IsAny<Encoding>(),
                expected,
                It.IsAny<String>()),
                Times.Once);
        }

        [Test]
        public void Write_CheckCallFacadeWriteSubjectValid_FacadeWriteWasCalledWithSubject()
        {
            String expected = "subject";

            this.settings.SetupGet(x => x.LogLevel).Returns(LogLevel.Trace);
            this.settings.SetupGet(x => x.Encoding).Returns(Encoding.UTF8);
            this.settings.SetupGet(x => x.Subject).Returns(expected);

            this.instance.TestWrite(LogLevel.Message, null, null, "message", null, null);

            this.facade.Verify(x => x.Write(
                It.IsAny<String>(),
                It.IsAny<IEnumerable<String>>(),
                It.IsAny<IEnumerable<String>>(),
                It.IsAny<IEnumerable<String>>(),
                It.IsAny<Encoding>(),
                expected,
                It.IsAny<String>()),
                Times.Once);
        }

        #endregion

        #region SettingsPropertyChanged

        [Test]
        public void SettingsPropertyChanged_UsernameAffected_FacadeUsernameApplied()
        {
            String value = "new username";

            this.settings.SetupGet(x => x.Username).Returns(value);
            this.settings.Raise(m => m.PropertyChanged += null, new PropertyChangedEventArgs(nameof(IMailLoggerSettings.Username)));

            this.facade.VerifySet(x => x.Username = value, Times.Once);
        }

        [Test]
        public void SettingsPropertyChanged_PasswordAffected_FacadePasswordApplied()
        {
            String value = "new password";

            this.settings.SetupGet(x => x.Password).Returns(value);
            this.settings.Raise(m => m.PropertyChanged += null, new PropertyChangedEventArgs(nameof(IMailLoggerSettings.Password)));

            this.facade.VerifySet(x => x.Password = value, Times.Once);
        }

        [Test]
        public void SettingsPropertyChanged_SmtpHostAffected_FacadeSmtpHostApplied()
        {
            String value = "new smtp host";

            this.settings.SetupGet(x => x.SmtpHost).Returns(value);
            this.settings.Raise(m => m.PropertyChanged += null, new PropertyChangedEventArgs(nameof(IMailLoggerSettings.SmtpHost)));

            this.facade.VerifySet(x => x.SmtpHost = value, Times.Once);
        }

        [Test]
        public void SettingsPropertyChanged_SmtpPortAffected_FacadeSmtpPortApplied()
        {
            Int32 value = 1234;

            this.settings.SetupGet(x => x.SmtpPort).Returns(value);
            this.settings.Raise(m => m.PropertyChanged += null, new PropertyChangedEventArgs(nameof(IMailLoggerSettings.SmtpPort)));

            this.facade.VerifySet(x => x.SmtpPort = value, Times.Once);
        }

        [Test]
        public void SettingsPropertyChanged_UseSslAffected_FacadeUseSslApplied()
        {
            Boolean value = false;

            this.settings.SetupGet(x => x.UseSsl).Returns(value);
            this.settings.Raise(m => m.PropertyChanged += null, new PropertyChangedEventArgs(nameof(IMailLoggerSettings.UseSsl)));

            this.facade.VerifySet(x => x.UseSsl = value, Times.Once);
        }

        #endregion

        #region Private test class implementations

        private class TestClass : MailLoggerBase
        {
            public TestClass(IMailLoggerSettings settings, IMailLoggerFacade facade)
                : base(settings, facade)
            {
            }

            public void TestWrite(LogLevel level, String context, String scope, String message, Exception exception, params (String Label, Object Value)[] details)
            {
                base.Write(level, context, scope, message, exception, details);
            }
        }

        #endregion
    }
}
