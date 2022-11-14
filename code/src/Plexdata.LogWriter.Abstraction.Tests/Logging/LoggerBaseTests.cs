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

using Moq;
using NUnit.Framework;
using Plexdata.LogWriter.Definitions;
using Plexdata.LogWriter.Internals.Logging;
using Plexdata.LogWriter.Logging;
using Plexdata.Utilities.Testing;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Text;

namespace Plexdata.LogWriter.Abstraction.Tests.Logging
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    [Category(TestType.UnitTest)]
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

        #region CreateScope

        [TestCase(0)]
        [TestCase(1)]
        [TestCase(3)]
        [TestCase(4)]
        public void CreateScope_WithManyScopeItems_ScopesCountAsExpected(Int32 count)
        {
            for (Int32 index = 0; index < count; index++)
            {
                this.instance.TestCreateScope($"scope-{index}");
            }

            Assert.That(this.GetValueOfScopesField(this.instance).Count, Is.EqualTo(count));
        }

        [TestCase(0)]
        [TestCase(1)]
        [TestCase(3)]
        [TestCase(4)]
        public void CreateScope_WithManyScopeItems_ScopesValuesAsExpected(Int32 count)
        {
            for (Int32 index = 0; index < count; index++)
            {
                this.instance.TestCreateScope($"scope-{index}");
            }

            List<LoggingScope> scopes = this.GetValueOfScopesField(this.instance);

            for (Int32 index = 0; index < count; index++)
            {
                Assert.That(scopes[index].Value, Is.EqualTo($"scope-{index}"));
            }
        }

        [TestCase(0)]
        [TestCase(1)]
        [TestCase(3)]
        [TestCase(4)]
        public void CreateScope_WithManyScopeItems_DisposingEventsAssignedAsExpected(Int32 count)
        {
            for (Int32 index = 0; index < count; index++)
            {
                this.instance.TestCreateScope($"scope-{index}");
            }

            List<LoggingScope> scopes = this.GetValueOfScopesField(this.instance);

            for (Int32 index = 0; index < count; index++)
            {
                Assert.That(this.GetValueOfDisposingEvent(scopes[index]), Is.Not.Null);
            }
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

        [Test]
        public void ResolveScope_ScopeIsGuidLoggerSettingsWithShortName_ResultIsExpectedString()
        {
            this.settings.Setup(x => x.FullName).Returns(false);
            Guid expected = Guid.Parse("11223344-5566-7788-9900-AABBCCDDEEFF");
            Assert.That(this.instance.TestResolveScope<Guid>(expected), Is.EqualTo(expected.ToString()));
        }

        [Test]
        public void ResolveScopeSettings_ScopeIsGuidLoggerSettingsWithShortName_ResultIsExpectedString()
        {
            Mock<ILoggerSettings> other = new Mock<ILoggerSettings>();
            other.Setup(x => x.FullName).Returns(false);
            Guid expected = Guid.Parse("11223344-5566-7788-9900-AABBCCDDEEFF");
            Assert.That(this.instance.TestResolveScope<Guid>(expected, other.Object), Is.EqualTo(expected.ToString()));
        }

        [Test]
        public void ResolveScope_ScopeIsGuidLoggerSettingsWithFullName_ResultIsExpectedString()
        {
            this.settings.Setup(x => x.FullName).Returns(true);
            Guid expected = Guid.Parse("11223344-5566-7788-9900-AABBCCDDEEFF");
            Assert.That(this.instance.TestResolveScope<Guid>(expected), Is.EqualTo(expected.ToString()));
        }

        [Test]
        public void ResolveScopeSettings_ScopeIsGuidLoggerSettingsWithFullName_ResultIsExpectedString()
        {
            Mock<ILoggerSettings> other = new Mock<ILoggerSettings>();
            other.Setup(x => x.FullName).Returns(true);
            Guid expected = Guid.Parse("11223344-5566-7788-9900-AABBCCDDEEFF");
            Assert.That(this.instance.TestResolveScope<Guid>(expected, other.Object), Is.EqualTo(expected.ToString()));
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

        #region RemoveScope

        [Test]
        public void RemoveScope_WithFourScopeItems_ScopesCountAsExpected()
        {
            using (this.instance.TestCreateScope("scope-1"))
            {
                using (this.instance.TestCreateScope("scope-2"))
                {
                    using (this.instance.TestCreateScope("scope-3"))
                    {
                        using (this.instance.TestCreateScope("scope-4"))
                        {
                            Assert.That(this.GetValueOfScopesField(this.instance).Count, Is.EqualTo(4));
                        }
                        Assert.That(this.GetValueOfScopesField(this.instance).Count, Is.EqualTo(3));
                    }
                    Assert.That(this.GetValueOfScopesField(this.instance).Count, Is.EqualTo(2));
                }
                Assert.That(this.GetValueOfScopesField(this.instance).Count, Is.EqualTo(1));
            }
            Assert.That(this.GetValueOfScopesField(this.instance).Count, Is.EqualTo(0));
        }

        #endregion

        #region FetchScope

        [Test]
        public void FetchScope_ScopeValueIsValidAndNoOtherScopeAssigned_FactoryWasCalledWithExpectedScope()
        {
            String actual = null;

            this.factory
                .Setup(x => x.CreateLogEvent(
                    It.IsAny<LogLevel>(), It.IsAny<DateTime>(), It.IsAny<String>(), It.IsAny<String>(),
                    It.IsAny<String>(), It.IsAny<Exception>(), It.IsAny<ValueTuple<String, Object>[]>()))
                .Callback((LogLevel l, DateTime t, String c, String s, String m, Exception e, (String Label, Object Value)[] d) => { actual = s; })
                .Returns(this.message.Object);

            this.instance.TestCreateOutput(this.settings.Object, LogLevel.Disabled, null, "not-null", null, null, null);

            Assert.That(actual, Is.EqualTo("not-null"));
        }

        [Test]
        public void FetchScope_ScopeValueIsNullAndNoOtherScopeAssigned_FactoryWasCalledWithExpectedScope()
        {
            String actual = "not-null";

            this.factory
                .Setup(x => x.CreateLogEvent(
                    It.IsAny<LogLevel>(), It.IsAny<DateTime>(), It.IsAny<String>(), It.IsAny<String>(),
                    It.IsAny<String>(), It.IsAny<Exception>(), It.IsAny<ValueTuple<String, Object>[]>()))
                .Callback((LogLevel l, DateTime t, String c, String s, String m, Exception e, (String Label, Object Value)[] d) => { actual = s; })
                .Returns(this.message.Object);

            this.instance.TestCreateOutput(this.settings.Object, LogLevel.Disabled, null, null, null, null, null);

            Assert.That(actual, Is.Null);
        }

        [Test]
        public void FetchScope_ScopeValueIsValidAndOtherScopeAssigned_FactoryWasCalledWithExpectedScope()
        {
            String actual = null;

            this.factory
                .Setup(x => x.CreateLogEvent(
                    It.IsAny<LogLevel>(), It.IsAny<DateTime>(), It.IsAny<String>(), It.IsAny<String>(),
                    It.IsAny<String>(), It.IsAny<Exception>(), It.IsAny<ValueTuple<String, Object>[]>()))
                .Callback((LogLevel l, DateTime t, String c, String s, String m, Exception e, (String Label, Object Value)[] d) => { actual = s; })
                .Returns(this.message.Object);

            this.instance.TestCreateScope("scope-1");

            this.instance.TestCreateOutput(this.settings.Object, LogLevel.Disabled, null, "not-null", null, null, null);

            Assert.That(actual, Is.EqualTo("not-null"));
        }

        [Test]
        public void FetchScope_ScopeValueIsNullAndOtherScopeAssigned_FactoryWasCalledWithExpectedScope()
        {
            String actual = "not-null";

            this.factory
                .Setup(x => x.CreateLogEvent(
                    It.IsAny<LogLevel>(), It.IsAny<DateTime>(), It.IsAny<String>(), It.IsAny<String>(),
                    It.IsAny<String>(), It.IsAny<Exception>(), It.IsAny<ValueTuple<String, Object>[]>()))
                .Callback((LogLevel l, DateTime t, String c, String s, String m, Exception e, (String Label, Object Value)[] d) => { actual = s; })
                .Returns(this.message.Object);

            this.instance.TestCreateScope("scope-1");

            this.instance.TestCreateOutput(this.settings.Object, LogLevel.Disabled, null, null, null, null, null);

            Assert.That(actual, Is.EqualTo("scope-1"));
        }

        [Test]
        public void FetchScope_ScopeValueIsNullAndOtherScopeAssignedButWithOneInvalidValue_FactoryWasCalledWithExpectedScope()
        {
            String actual = "not-null";

            this.factory
                .Setup(x => x.CreateLogEvent(
                    It.IsAny<LogLevel>(), It.IsAny<DateTime>(), It.IsAny<String>(), It.IsAny<String>(),
                    It.IsAny<String>(), It.IsAny<Exception>(), It.IsAny<ValueTuple<String, Object>[]>()))
                .Callback((LogLevel l, DateTime t, String c, String s, String m, Exception e, (String Label, Object Value)[] d) => { actual = s; })
                .Returns(this.message.Object);

            this.instance.TestCreateScope((Object)null);

            this.instance.TestCreateOutput(this.settings.Object, LogLevel.Disabled, null, null, null, null, null);

            Assert.That(actual, Is.Null);
        }

        [Test]
        public void FetchScope_ScopeValueIsValidAndOtherScopesAssigned_FactoryWasCalledWithExpectedScope()
        {
            String actual = null;

            this.factory
                .Setup(x => x.CreateLogEvent(
                    It.IsAny<LogLevel>(), It.IsAny<DateTime>(), It.IsAny<String>(), It.IsAny<String>(),
                    It.IsAny<String>(), It.IsAny<Exception>(), It.IsAny<ValueTuple<String, Object>[]>()))
                .Callback((LogLevel l, DateTime t, String c, String s, String m, Exception e, (String Label, Object Value)[] d) => { actual = s; })
                .Returns(this.message.Object);

            this.instance.TestCreateScope("scope-1");
            this.instance.TestCreateScope("scope-2");

            this.instance.TestCreateOutput(this.settings.Object, LogLevel.Disabled, null, "not-null", null, null, null);

            Assert.That(actual, Is.EqualTo("not-null"));
        }

        [Test]
        public void FetchScope_ScopeValueIsNullAndOtherScopesAssigned_FactoryWasCalledWithExpectedScope()
        {
            String actual = "not-null";

            this.factory
                .Setup(x => x.CreateLogEvent(
                    It.IsAny<LogLevel>(), It.IsAny<DateTime>(), It.IsAny<String>(), It.IsAny<String>(),
                    It.IsAny<String>(), It.IsAny<Exception>(), It.IsAny<ValueTuple<String, Object>[]>()))
                .Callback((LogLevel l, DateTime t, String c, String s, String m, Exception e, (String Label, Object Value)[] d) => { actual = s; })
                .Returns(this.message.Object);

            this.instance.TestCreateScope("scope-1");
            this.instance.TestCreateScope("scope-2");

            this.instance.TestCreateOutput(this.settings.Object, LogLevel.Disabled, null, null, null, null, null);

            Assert.That(actual, Is.EqualTo("[scope-1,scope-2]"));
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        public void FetchScope_ScopeValueIsNullAndOtherScopesAssignedButWithOneInvalidValue_FactoryWasCalledWithExpectedScope(String invalid)
        {
            String actual = null;

            this.factory
                .Setup(x => x.CreateLogEvent(
                    It.IsAny<LogLevel>(), It.IsAny<DateTime>(), It.IsAny<String>(), It.IsAny<String>(),
                    It.IsAny<String>(), It.IsAny<Exception>(), It.IsAny<ValueTuple<String, Object>[]>()))
                .Callback((LogLevel l, DateTime t, String c, String s, String m, Exception e, (String Label, Object Value)[] d) => { actual = s; })
                .Returns(this.message.Object);

            this.instance.TestCreateScope("scope-1");
            this.instance.TestCreateScope(invalid);
            this.instance.TestCreateScope("scope-3");

            this.instance.TestCreateOutput(this.settings.Object, LogLevel.Disabled, null, null, null, null, null);

            Assert.That(actual, Is.EqualTo("[scope-1,scope-3]"));
        }

        [Test]
        public void FetchScope_ScopeValueIsNullAndOtherScopesAssignedButWithThreeInvalidValues_FactoryWasCalledWithExpectedScope()
        {
            String actual = null;

            this.factory
                .Setup(x => x.CreateLogEvent(
                    It.IsAny<LogLevel>(), It.IsAny<DateTime>(), It.IsAny<String>(), It.IsAny<String>(),
                    It.IsAny<String>(), It.IsAny<Exception>(), It.IsAny<ValueTuple<String, Object>[]>()))
                .Callback((LogLevel l, DateTime t, String c, String s, String m, Exception e, (String Label, Object Value)[] d) => { actual = s; })
                .Returns(this.message.Object);

            this.instance.TestCreateScope((Object)null);
            this.instance.TestCreateScope(" ");
            this.instance.TestCreateScope(String.Empty);

            this.instance.TestCreateOutput(this.settings.Object, LogLevel.Disabled, null, null, null, null, null);

            Assert.That(actual, Is.Null);
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

            public IDisposable TestCreateScope<TScope>(TScope scope)
            {
                return base.CreateScope<TScope>(scope);
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

        private List<LoggingScope> GetValueOfScopesField(LoggerBase<ILoggerSettings> instance)
        {
            return typeof(LoggerBase<ILoggerSettings>)
                .GetField("scopes", BindingFlags.Instance | BindingFlags.NonPublic)
                .GetValue(instance) as List<LoggingScope>;
        }

        private Object GetValueOfDisposingEvent(LoggingScope instance)
        {
            return instance.GetType()
                .GetField("Disposing", BindingFlags.NonPublic | BindingFlags.Instance)
                .GetValue(instance);
        }

        #endregion
    }
}
