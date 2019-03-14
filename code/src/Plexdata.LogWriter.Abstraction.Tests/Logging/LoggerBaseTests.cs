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
using Plexdata.LogWriter.Logging;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace Plexdata.LogWriter.Abstraction.Tests.Logging
{
    // TODO: Review and/or apply new tests for made changes.

    [TestFixture]
    [ExcludeFromCodeCoverage]
    [TestOf(nameof(LoggerBase<ILoggerSettings>))]
    public class LoggerBaseTests
    {
        #region Prologue

        private Mock<ILoggerSettings> settings = null;
        private LoggerBaseDummyClass instance = null;

        [SetUp]
        public void Setup()
        {
            this.settings = new Mock<ILoggerSettings>();
            this.instance = new LoggerBaseDummyClass(this.settings.Object);
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
            Assert.That(this.instance.TestResolveContext<DummyClass>(this.settings.Object), Is.EqualTo(typeof(DummyClass).Name));
        }

        [Test]
        public void ResolveContext_LoggerSettingsWithFullName_ResultIsDummyClassFullName()
        {
            this.settings.Setup(x => x.FullName).Returns(true);
            Assert.That(this.instance.TestResolveContext<DummyClass>(this.settings.Object), Is.EqualTo(typeof(DummyClass).FullName));
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
            Assert.That(this.instance.TestResolveScope<DummyClass>(null, this.settings.Object), Is.EqualTo(typeof(DummyClass).Name));
        }

        [Test]
        public void ResolveScope_ScopeIsNullLoggerSettingsWithFullName_ResultIsDummyClassFullName()
        {
            this.settings.Setup(x => x.FullName).Returns(true);
            Assert.That(this.instance.TestResolveScope<DummyClass>(null, this.settings.Object), Is.EqualTo(typeof(DummyClass).FullName));
        }

        [Test]
        public void ResolveScope_ScopeIsDummyClassLoggerSettingsWithShortName_ResultIsDummyClassShortName()
        {
            this.settings.Setup(x => x.FullName).Returns(false);
            Assert.That(this.instance.TestResolveScope<DummyClass>(new DummyClass(), this.settings.Object), Is.EqualTo(typeof(DummyClass).Name));
        }

        [Test]
        public void ResolveScope_ScopeIsDummyClassLoggerSettingsWithFullName_ResultIsDummyClassFullName()
        {
            this.settings.Setup(x => x.FullName).Returns(true);
            Assert.That(this.instance.TestResolveScope<DummyClass>(new DummyClass(), this.settings.Object), Is.EqualTo(typeof(DummyClass).FullName));
        }

        [Test]
        public void ResolveScope_ScopeIsMethodBaseLoggerSettingsWithShortName_ResultIsDummyClassMethodName()
        {
            this.settings.Setup(x => x.FullName).Returns(false);
            MethodBase method = (new DummyClass()).TestMethod();
            Assert.That(this.instance.TestResolveScope<MethodBase>(method, this.settings.Object), Is.EqualTo(method.Name));
        }

        [Test]
        public void ResolveScope_ScopeIsMethodBaseLoggerSettingsWithFullName_ResultIsDummyClassMethodName()
        {
            this.settings.Setup(x => x.FullName).Returns(true);
            MethodBase method = (new DummyClass()).TestMethod();
            Assert.That(this.instance.TestResolveScope<MethodBase>(method, this.settings.Object), Is.EqualTo(method.Name));
        }

        [Test]
        public void ResolveScope_ScopeIsStringLoggerSettingsWithShortName_ResultIsExpectedString()
        {
            this.settings.Setup(x => x.FullName).Returns(false);
            String expected = "my expected string";
            Assert.That(this.instance.TestResolveScope<String>(expected, this.settings.Object), Is.EqualTo(expected));
        }

        [Test]
        public void ResolveScope_ScopeIsStringLoggerSettingsWithFullName_ResultIsExpectedString()
        {
            this.settings.Setup(x => x.FullName).Returns(true);
            String expected = "my expected string";
            Assert.That(this.instance.TestResolveScope<String>(expected, this.settings.Object), Is.EqualTo(expected));
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

            public String TestResolveContext<TContext>(ILoggerSettings settings)
            {
                return base.ResolveContext<TContext>(settings);
            }

            public String TestResolveScope<TScope>(TScope scope, ILoggerSettings settings)
            {
                return base.ResolveScope<TScope>(scope, settings);
            }
        }

        #endregion
    }
}
