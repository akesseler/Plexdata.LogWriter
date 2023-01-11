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
using Plexdata.LogWriter.Logging;
using Plexdata.Utilities.Testing;
using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

namespace Plexdata.LogWriter.Network.Tests.Logging
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    [NUnit.Framework.Category(TestType.UnitTest)]
    [TestOf(nameof(NetworkLoggerBase))]
    public class NetworkLoggerBaseTests
    {
        #region Prologue

        private TestBaseClass instance = null;
        private Mock<INetworkLoggerSettings> settings = null;
        private Mock<INetworkLoggerFacade> facade = null;

        [SetUp]
        public void Setup()
        {
            this.settings = new Mock<INetworkLoggerSettings>();
            this.facade = new Mock<INetworkLoggerFacade>();

            this.instance = new TestBaseClass(this.settings.Object, this.facade.Object);
        }

        #endregion

        #region Construction

        [Test]
        public void NetworkLoggerBase_SettingsNull_ThrowsArgumentNullException()
        {
            Assert.That(() => new TestBaseClass(null, this.facade.Object), Throws.ArgumentNullException);
        }

        [Test]
        public void NetworkLoggerBase_FacadeNull_ThrowsArgumentNullException()
        {
            Assert.That(() => new TestBaseClass(this.settings.Object, null), Throws.ArgumentNullException);
        }

        [Test]
        public void NetworkLoggerBase_ParametersValid_INetworkLoggerFacadeApplySettingsCalledOnce()
        {
            this.facade.Verify(x => x.ApplySettings(this.settings.Object), Times.Once());
        }

        #endregion

        #region Write

        [Test]
        public void Write_CheckDisposedIsTrue_NothingIsExecuted()
        {
            this.instance.Dispose();

            this.instance.TestWrite(LogLevel.Message, null, null, null, null, null);

            this.facade.Verify(x => x.Write(It.IsAny<String>()), Times.Never());
        }

        [Test]
        public void Write_CheckDisabledIsTrue_NothingIsExecuted()
        {
            this.settings.SetupGet(x => x.LogLevel).Returns(LogLevel.Disabled);

            this.instance.TestWrite(LogLevel.Message, null, null, null, null, null);

            this.facade.Verify(x => x.Write(It.IsAny<String>()), Times.Never());
        }

        [Test]
        public void Write_CheckEnabledIsFalse_NothingIsExecuted()
        {
            this.settings.SetupGet(x => x.LogLevel).Returns(LogLevel.Critical);

            this.instance.TestWrite(LogLevel.Message, null, null, null, null, null);

            this.facade.Verify(x => x.Write(It.IsAny<String>()), Times.Never());
        }

        [Test]
        public void Write_OutputIsEmpty_NothingIsExecuted()
        {
            this.settings.SetupGet(x => x.LogLevel).Returns(LogLevel.Trace);

            this.instance.TestWrite(LogLevel.Message, null, null, null, null, null);

            this.facade.Verify(x => x.Write(It.IsAny<String>()), Times.Never());
        }

        [Test]
        public void Write_CheckCallFacadeWrite_FacadeWriteWasCalledOnce()
        {
            this.settings.SetupGet(x => x.LogLevel).Returns(LogLevel.Trace);

            this.instance.TestWrite(LogLevel.Message, null, null, "message", null, null);

            this.facade.Verify(x => x.Write(It.IsAny<String>()), Times.Once());
        }

        #endregion

        #region SettingsPropertyChanged

        [Test]
        public void SettingsPropertyChanged_UsernameAffected_FacadeUsernameApplied()
        {
            this.settings.Raise(m => m.PropertyChanged += null, new PropertyChangedEventArgs(nameof(INetworkLoggerSettings.Host)));

            this.facade.Verify(x => x.ApplySettings(this.settings.Object), Times.Exactly(2));
        }

        #endregion

        #region Private test class implementations

        private class TestBaseClass : NetworkLoggerBase
        {
            public TestBaseClass(INetworkLoggerSettings settings, INetworkLoggerFacade facade)
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
