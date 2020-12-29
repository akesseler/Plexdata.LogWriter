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
using Plexdata.LogWriter.Definitions;
using Plexdata.LogWriter.Logging;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Text;

namespace Plexdata.LogWriter.Abstraction.Tests.Logging
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    [TestOf(nameof(LoggerBase<ILoggerSettings>))]
    public class LoggerBaseTests
    {
        #region Prologue

        private const String formatted = "formatted test message";

        private Mock<ILoggerSettings> settings = null;
        private Mock<ILoggerFactory> factory = null;
        private Mock<ILogEvent> message = null;
        private Mock<ILogEventFormatter> formatter = null;
        private LoggerBaseDummyClass instance = null;

        [SetUp]
        public void Setup()
        {
            this.settings = new Mock<ILoggerSettings>();
            this.message = new Mock<ILogEvent>();
            this.factory = new Mock<ILoggerFactory>();
            this.formatter = new Mock<ILogEventFormatter>();

            this.factory
                .Setup(x => x.CreateLogEvent(
                    It.IsAny<LogLevel>(), It.IsAny<DateTime>(), It.IsAny<String>(), It.IsAny<String>(),
                    It.IsAny<String>(), It.IsAny<Exception>(), It.IsAny<ValueTuple<String, Object>[]>()))
                .Returns(this.message.Object);

            this.factory
                .Setup(x => x.CreateLogEventFormatter(It.IsAny<ILoggerSettings>()))
                .Returns(this.formatter.Object);

            this.formatter.Setup(x => x.Format(It.IsAny<StringBuilder>(), It.IsAny<ILogEvent>()))
                .Callback((StringBuilder x, ILogEvent y) => { x.Append(formatted); });

            this.instance = new LoggerBaseDummyClass(this.settings.Object, this.factory.Object);
        }

        #endregion

        #region Construction

        [Test]
        public void LoggerBaseStandard_SettingsAreNull_ThrowsArgumentNullException()
        {
            Assert.That(() => new LoggerBaseDummyClass(null), Throws.ArgumentNullException);
        }

        [Test]
        public void LoggerBaseExtended_SettingsAreNull_ThrowsArgumentNullException()
        {
            Assert.That(() => new LoggerBaseDummyClass(null, null), Throws.ArgumentNullException);
        }

        [Test]
        public void LoggerBaseExtended_FactoryIsNull_ThrowsArgumentNullException()
        {
            Assert.That(() => new LoggerBaseDummyClass(this.settings.Object, null), Throws.ArgumentNullException);
        }

        #endregion

        #region IsDisabled

        [Test]
        [TestCase(LogLevel.Disabled, true)]
        [TestCase(LogLevel.Trace, false)]
        [TestCase(LogLevel.Debug, false)]
        [TestCase(LogLevel.Verbose, false)]
        [TestCase(LogLevel.Message, false)]
        [TestCase(LogLevel.Warning, false)]
        [TestCase(LogLevel.Error, false)]
        [TestCase(LogLevel.Fatal, false)]
        [TestCase(LogLevel.Critical, false)]
        public void IsDisabled_LogLevelAsDefined_ResultAsExpected(LogLevel level, Boolean expected)
        {
            this.settings.SetupGet(x => x.LogLevel).Returns(level);
            Assert.That(this.instance.TestIsDisabled(), Is.EqualTo(expected));
        }

        #endregion

        #region IsEnabled

        [TestCase(LogLevel.Disabled, false)]
        [TestCase(LogLevel.Trace, false)]
        [TestCase(LogLevel.Debug, false)]
        [TestCase(LogLevel.Verbose, false)]
        [TestCase(LogLevel.Message, true)]
        [TestCase(LogLevel.Warning, true)]
        [TestCase(LogLevel.Error, true)]
        [TestCase(LogLevel.Fatal, true)]
        [TestCase(LogLevel.Critical, true)]
        public void IsEnabled_LogLevelAsDefined_ResultAsExpected(LogLevel actual, Boolean expected)
        {
            this.settings.SetupGet(x => x.LogLevel).Returns(LogLevel.Message);
            Assert.That(this.instance.TestIsEnabled(actual), Is.EqualTo(expected));
        }

        #endregion

        #region ResolveContext

        [Test]
        public void ResolveContext_LoggerSettingsAreNull_ResultIsStringEmpty()
        {
            Assert.That(this.instance.TestResolveContext<DummyClass>(null), Is.EqualTo(String.Empty));
        }

        [Test]
        public void ResolveContext_LoggerSettingsWithShortName_ResultIsDummyClassShortName()
        {
            this.settings.Setup(x => x.FullName).Returns(false);
            Assert.That(this.instance.TestResolveContext<DummyClass>(), Is.EqualTo(typeof(DummyClass).Name));
        }

        [Test]
        public void ResolveContextSettings_LoggerSettingsWithShortName_ResultIsDummyClassShortName()
        {
            Mock<ILoggerSettings> other = new Mock<ILoggerSettings>();
            other.Setup(x => x.FullName).Returns(false);
            Assert.That(this.instance.TestResolveContext<DummyClass>(other.Object), Is.EqualTo(typeof(DummyClass).Name));
        }

        [Test]
        public void ResolveContext_LoggerSettingsWithFullName_ResultIsDummyClassFullName()
        {
            this.settings.Setup(x => x.FullName).Returns(true);
            Assert.That(this.instance.TestResolveContext<DummyClass>(), Is.EqualTo(typeof(DummyClass).FullName));
        }

        [Test]
        public void ResolveContextSettings_LoggerSettingsWithFullName_ResultIsDummyClassFullName()
        {
            Mock<ILoggerSettings> other = new Mock<ILoggerSettings>();
            other.Setup(x => x.FullName).Returns(true);
            Assert.That(this.instance.TestResolveContext<DummyClass>(other.Object), Is.EqualTo(typeof(DummyClass).FullName));
        }

        #endregion

        #region  ResolveScope

        [Test]
        public void ResolveScope_ScopeAndLoggerSettingsAreNull_ResultIsStringEmpty()
        {
            Assert.That(this.instance.TestResolveScope<DummyClass>(null, null), Is.EqualTo(String.Empty));
        }

        [Test]
        public void ResolveScope_ScopeIsNullLoggerSettingsWithShortName_ResultIsDummyClassShortName()
        {
            this.settings.Setup(x => x.FullName).Returns(false);
            Assert.That(this.instance.TestResolveScope<DummyClass>(null), Is.EqualTo(typeof(DummyClass).Name));
        }

        [Test]
        public void ResolveScopeSettings_ScopeIsNullLoggerSettingsWithShortName_ResultIsDummyClassShortName()
        {
            Mock<ILoggerSettings> other = new Mock<ILoggerSettings>();
            other.Setup(x => x.FullName).Returns(false);
            Assert.That(this.instance.TestResolveScope<DummyClass>(null, other.Object), Is.EqualTo(typeof(DummyClass).Name));
        }

        [Test]
        public void ResolveScope_ScopeIsNullLoggerSettingsWithFullName_ResultIsDummyClassFullName()
        {
            this.settings.Setup(x => x.FullName).Returns(true);
            Assert.That(this.instance.TestResolveScope<DummyClass>(null), Is.EqualTo(typeof(DummyClass).FullName));
        }

        [Test]
        public void ResolveScopeSettings_ScopeIsNullLoggerSettingsWithFullName_ResultIsDummyClassFullName()
        {
            Mock<ILoggerSettings> other = new Mock<ILoggerSettings>();
            other.Setup(x => x.FullName).Returns(true);
            Assert.That(this.instance.TestResolveScope<DummyClass>(null, other.Object), Is.EqualTo(typeof(DummyClass).FullName));
        }

        [Test]
        public void ResolveScope_ScopeIsDummyClassLoggerSettingsWithShortName_ResultIsDummyClassShortName()
        {
            this.settings.Setup(x => x.FullName).Returns(false);
            Assert.That(this.instance.TestResolveScope<DummyClass>(new DummyClass()), Is.EqualTo(typeof(DummyClass).Name));
        }

        [Test]
        public void ResolveScopeSettings_ScopeIsDummyClassLoggerSettingsWithShortName_ResultIsDummyClassShortName()
        {
            Mock<ILoggerSettings> other = new Mock<ILoggerSettings>();
            other.Setup(x => x.FullName).Returns(false);
            Assert.That(this.instance.TestResolveScope<DummyClass>(new DummyClass(), other.Object), Is.EqualTo(typeof(DummyClass).Name));
        }

        [Test]
        public void ResolveScope_ScopeIsDummyClassLoggerSettingsWithFullName_ResultIsDummyClassFullName()
        {
            this.settings.Setup(x => x.FullName).Returns(true);
            Assert.That(this.instance.TestResolveScope<DummyClass>(new DummyClass()), Is.EqualTo(typeof(DummyClass).FullName));
        }

        [Test]
        public void ResolveScopeSettings_ScopeIsDummyClassLoggerSettingsWithFullName_ResultIsDummyClassFullName()
        {
            Mock<ILoggerSettings> other = new Mock<ILoggerSettings>();
            other.Setup(x => x.FullName).Returns(true);
            Assert.That(this.instance.TestResolveScope<DummyClass>(new DummyClass(), other.Object), Is.EqualTo(typeof(DummyClass).FullName));
        }

        [Test]
        public void ResolveScope_ScopeIsMethodBaseLoggerSettingsWithShortName_ResultIsDummyClassMethodName()
        {
            this.settings.Setup(x => x.FullName).Returns(false);
            MethodBase method = (new DummyClass()).TestMethod();
            Assert.That(this.instance.TestResolveScope<MethodBase>(method), Is.EqualTo(method.Name));
        }

        [Test]
        public void ResolveScopeSettings_ScopeIsMethodBaseLoggerSettingsWithShortName_ResultIsDummyClassMethodName()
        {
            Mock<ILoggerSettings> other = new Mock<ILoggerSettings>();
            other.Setup(x => x.FullName).Returns(false);
            MethodBase method = (new DummyClass()).TestMethod();
            Assert.That(this.instance.TestResolveScope<MethodBase>(method, other.Object), Is.EqualTo(method.Name));
        }

        [Test]
        public void ResolveScope_ScopeIsMethodBaseLoggerSettingsWithFullName_ResultIsDummyClassMethodName()
        {
            this.settings.Setup(x => x.FullName).Returns(true);
            MethodBase method = (new DummyClass()).TestMethod();
            Assert.That(this.instance.TestResolveScope<MethodBase>(method), Is.EqualTo(method.Name));
        }

        [Test]
        public void ResolveScopeSettings_ScopeIsMethodBaseLoggerSettingsWithFullName_ResultIsDummyClassMethodName()
        {
            Mock<ILoggerSettings> other = new Mock<ILoggerSettings>();
            other.Setup(x => x.FullName).Returns(true);
            MethodBase method = (new DummyClass()).TestMethod();
            Assert.That(this.instance.TestResolveScope<MethodBase>(method, other.Object), Is.EqualTo(method.Name));
        }

        [Test]
        public void ResolveScope_ScopeIsStringLoggerSettingsWithShortName_ResultIsExpectedString()
        {
            this.settings.Setup(x => x.FullName).Returns(false);
            String expected = "my expected string";
            Assert.That(this.instance.TestResolveScope<String>(expected), Is.EqualTo(expected));
        }

        [Test]
        public void ResolveScopeSettings_ScopeIsStringLoggerSettingsWithShortName_ResultIsExpectedString()
        {
            Mock<ILoggerSettings> other = new Mock<ILoggerSettings>();
            other.Setup(x => x.FullName).Returns(false);
            String expected = "my expected string";
            Assert.That(this.instance.TestResolveScope<String>(expected, other.Object), Is.EqualTo(expected));
        }

        [Test]
        public void ResolveScope_ScopeIsStringLoggerSettingsWithFullName_ResultIsExpectedString()
        {
            this.settings.Setup(x => x.FullName).Returns(true);
            String expected = "my expected string";
            Assert.That(this.instance.TestResolveScope<String>(expected), Is.EqualTo(expected));
        }

        [Test]
        public void ResolveScopeSettings_ScopeIsStringLoggerSettingsWithFullName_ResultIsExpectedString()
        {
            Mock<ILoggerSettings> other = new Mock<ILoggerSettings>();
            other.Setup(x => x.FullName).Returns(true);
            String expected = "my expected string";
            Assert.That(this.instance.TestResolveScope<String>(expected, other.Object), Is.EqualTo(expected));
        }

        #endregion

        #region CreateOutput

        [Test]
        public void CreateOutput_SettingsValid_FactoryCreateLogEventWasCalledOnce()
        {
            this.instance.TestCreateOutput(LogLevel.Disabled, null, null, null, null, null);

            this.factory.Verify(x => x.CreateLogEvent(
                It.IsAny<LogLevel>(), It.IsAny<DateTime>(), It.IsAny<String>(), It.IsAny<String>(),
                It.IsAny<String>(), It.IsAny<Exception>(), It.IsAny<ValueTuple<String, Object>[]>()), Times.Once);
        }

        [Test]
        public void CreateOutput_SettingsValidMessageValid_FactoryCreateLogEventFormatterWasCalledOnce()
        {
            this.message.SetupGet(x => x.IsValid).Returns(true);

            this.instance.TestCreateOutput(LogLevel.Disabled, null, null, null, null, null);

            this.factory.Verify(x => x.CreateLogEventFormatter(this.settings.Object), Times.Once);
        }

        [Test]
        public void CreateOutput_SettingsValidMessageValid_FactoryCreateLogEventFormatterFormatWasCalledOnce()
        {
            this.message.SetupGet(x => x.IsValid).Returns(true);

            this.instance.TestCreateOutput(LogLevel.Disabled, null, null, null, null, null);

            this.formatter.Verify(x => x.Format(It.IsAny<StringBuilder>(), this.message.Object), Times.Once);
        }

        [Test]
        public void CreateOutput_SettingsValidMessageInvalid_ReturnsEmptyString()
        {
            this.message.SetupGet(x => x.IsValid).Returns(false);

            Assert.That(this.instance.TestCreateOutput(LogLevel.Disabled, null, null, null, null, null), Is.Empty);
        }

        [Test]
        public void CreateOutput_SettingsValidMessageValid_ReturnsExpectedString()
        {
            this.message.SetupGet(x => x.IsValid).Returns(true);

            Assert.That(this.instance.TestCreateOutput(LogLevel.Disabled, null, null, null, null, null), Is.EqualTo(LoggerBaseTests.formatted));
        }

        [Test]
        public void CreateOutputSettings_SettingsAreNull_ThrowsArgumentNullException()
        {
            Assert.That(() => this.instance.TestCreateOutput(null, LogLevel.Disabled, null, null, null, null, null), Throws.ArgumentNullException);
        }

        [Test]
        public void CreateOutputSettings_SettingsValid_FactoryCreateLogEventWasCalledOnce()
        {
            Mock<ILoggerSettings> other = new Mock<ILoggerSettings>();

            this.instance.TestCreateOutput(other.Object, LogLevel.Disabled, null, null, null, null, null);

            this.factory.Verify(x => x.CreateLogEvent(It.IsAny<LogLevel>(), It.IsAny<DateTime>(), It.IsAny<String>(), It.IsAny<String>(), It.IsAny<String>(), It.IsAny<Exception>(), It.IsAny<ValueTuple<String, Object>[]>()), Times.Once);
        }

        [Test]
        public void CreateOutputSettings_SettingsValidMessageValid_FactoryCreateLogEventFormatterWasCalledOnce()
        {
            Mock<ILoggerSettings> other = new Mock<ILoggerSettings>();

            this.message.SetupGet(x => x.IsValid).Returns(true);

            this.instance.TestCreateOutput(other.Object, LogLevel.Disabled, null, null, null, null, null);

            this.factory.Verify(x => x.CreateLogEventFormatter(other.Object), Times.Once);
        }

        [Test]
        public void CreateOutputSettings_SettingsValidMessageValid_FactoryCreateLogEventFormatterFormatWasCalledOnce()
        {
            Mock<ILoggerSettings> other = new Mock<ILoggerSettings>();

            this.message.SetupGet(x => x.IsValid).Returns(true);

            this.instance.TestCreateOutput(other.Object, LogLevel.Disabled, null, null, null, null, null);

            this.formatter.Verify(x => x.Format(It.IsAny<StringBuilder>(), this.message.Object), Times.Once);
        }

        [Test]
        public void CreateOutputSettings_SettingsValidMessageInvalid_ReturnsEmptyString()
        {
            Mock<ILoggerSettings> other = new Mock<ILoggerSettings>();

            this.message.SetupGet(x => x.IsValid).Returns(false);

            Assert.That(this.instance.TestCreateOutput(other.Object, LogLevel.Disabled, null, null, null, null, null), Is.Empty);
        }

        [Test]
        public void CreateOutputSettings_SettingsValidMessageValid_ReturnsExpectedString()
        {
            Mock<ILoggerSettings> other = new Mock<ILoggerSettings>();

            this.message.SetupGet(x => x.IsValid).Returns(true);

            Assert.That(this.instance.TestCreateOutput(other.Object, LogLevel.Disabled, null, null, null, null, null), Is.EqualTo(LoggerBaseTests.formatted));
        }

        #endregion

        #region Private test class implementations

        private class DummyClass
        {
            public MethodBase TestMethod()
            {
                return MethodBase.GetCurrentMethod();
            }
        }

        private class LoggerBaseDummyClass : LoggerBase<ILoggerSettings>
        {
            public LoggerBaseDummyClass(ILoggerSettings settings)
                : base(settings)
            {
            }

            public LoggerBaseDummyClass(ILoggerSettings settings, ILoggerFactory factory)
                : base(settings, factory)
            {
            }

            public String TestResolveContext<TContext>()
            {
                return base.ResolveContext<TContext>();
            }

            public String TestResolveContext<TContext>(ILoggerSettings settings)
            {
                return base.ResolveContext<TContext>(settings);
            }

            public Boolean TestIsDisabled()
            {
                return base.IsDisabled;
            }

            public Boolean TestIsEnabled(LogLevel level)
            {
                return base.IsEnabled(level);
            }

            public String TestResolveScope<TScope>(TScope scope)
            {
                return base.ResolveScope<TScope>(scope);
            }

            public String TestResolveScope<TScope>(TScope scope, ILoggerSettings settings)
            {
                return base.ResolveScope<TScope>(scope, settings);
            }

            public String TestCreateOutput(LogLevel level, String context, String scope, String message, Exception exception, params (String Label, Object Value)[] details)
            {
                return base.CreateOutput(level, context, scope, message, exception, details);
            }

            public String TestCreateOutput(ILoggerSettings settings, LogLevel level, String context, String scope, String message, Exception exception, params (String Label, Object Value)[] details)
            {
                return base.CreateOutput(settings, level, context, scope, message, exception, details);
            }

            protected override void Write(LogLevel level, String context, String scope, String message, Exception exception, params (String Label, Object Value)[] details)
            {
                throw new NotImplementedException();
            }
        }

        #endregion
    }
}
