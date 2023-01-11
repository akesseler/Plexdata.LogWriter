/*
 * MIT License
 * 
 * Copyright (c) 2023 plexdata.de
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
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Plexdata.LogWriter.Console.Tests.Logging
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    [Category(TestType.UnitTest)]
    [TestOf(nameof(ConsoleLoggerBase))]
    public class ConsoleLoggerBaseTests
    {
        #region Prologue

        private Mock<IConsoleLoggerSettings> settings = null;
        private Mock<IConsoleLoggerFacade> facade = null;
        private Mock<IDictionary<LogLevel, Coloring>> colorings = null;

        [SetUp]
        public void Setup()
        {
            this.settings = new Mock<IConsoleLoggerSettings>();
            this.facade = new Mock<IConsoleLoggerFacade>();
            this.colorings = new Mock<IDictionary<LogLevel, Coloring>>();
        }

        #endregion

        #region Construction

        [Test]
        public void ConsoleLoggerBase_SettingsNull_ThrowsArgumentNullException()
        {
            Assert.That(() => new DummyClass(null, this.facade.Object), Throws.ArgumentNullException);
        }

        [Test]
        public void ConsoleLoggerBase_FacadeNull_ThrowsArgumentNullException()
        {
            Assert.That(() => new DummyClass(this.settings.Object, null), Throws.ArgumentNullException);
        }

        [Test]
        public void ConsoleLoggerBase_ValidArguments_AttachCalledOnce()
        {
            DummyClass temp = new DummyClass(this.settings.Object, this.facade.Object);

            this.facade.Verify(x => x.Attach(), Times.Once);
        }

        [Test]
        public void ConsoleLoggerBase_ValidArguments_WindowTitleCalledOnceWithExpected()
        {
            String actual = "window title";

            this.settings.Setup(x => x.WindowTitle).Returns(actual);

            DummyClass temp = new DummyClass(this.settings.Object, this.facade.Object);

            this.facade.VerifySet(x => x.WindowTitle = actual, Times.Once);
        }

        [Test]
        public void ConsoleLoggerBase_ValidArguments_QuickEditCalledOnceWithExpected()
        {
            Boolean actual = true;

            this.settings.Setup(x => x.QuickEdit).Returns(actual);

            DummyClass temp = new DummyClass(this.settings.Object, this.facade.Object);

            this.facade.VerifySet(x => x.QuickEdit = actual, Times.Once);
        }

        [Test]
        public void ConsoleLoggerBase_ValidArguments_BufferSizeCalledOnceWithExpected()
        {
            Dimension actual = new Dimension();

            this.settings.Setup(x => x.BufferSize).Returns(actual);

            DummyClass temp = new DummyClass(this.settings.Object, this.facade.Object);

            this.facade.VerifySet(x => x.BufferSize = actual, Times.Once);
        }

        #endregion

        #region Dispose

        [Test]
        public void Dispose_ManyDisposals_DetachCalledOnce([Values(1, 2, 3)] Int32 count)
        {
            DummyClass temp = new DummyClass(this.settings.Object, this.facade.Object);

            while ((count--) > 0) { temp.Dispose(); }

            this.facade.Verify(x => x.Detach(), Times.Once);
        }

        #endregion

        #region Write

        [Test]
        public void Write_DisposedIsTrue_FacadeCalledNever()
        {
            DummyClass instance = new DummyClass(this.settings.Object, this.facade.Object);

            instance.Dispose();

            instance.TestWrite(LogLevel.Message, null, null, null, null, null);

            this.facade.VerifyGet(x => x.UseColors, Times.Never);
            this.facade.VerifySet(x => x.UseColors = It.IsAny<Boolean>(), Times.Never);
            this.facade.VerifySet(x => x.Foreground = It.IsAny<ConsoleColor>(), Times.Never);
            this.facade.VerifySet(x => x.Background = It.IsAny<ConsoleColor>(), Times.Never);
            this.facade.Verify(x => x.Write(It.IsAny<String>()), Times.Never);
        }

        [Test]
        public void Write_CheckDisabledIsTrue_FacadeCalledNever()
        {
            this.settings.SetupGet(x => x.LogLevel).Returns(LogLevel.Disabled);

            DummyClass instance = new DummyClass(this.settings.Object, this.facade.Object);

            instance.TestWrite(LogLevel.Message, null, null, null, null, null);

            this.facade.VerifyGet(x => x.UseColors, Times.Never);
            this.facade.VerifySet(x => x.UseColors = It.IsAny<Boolean>(), Times.Never);
            this.facade.VerifySet(x => x.Foreground = It.IsAny<ConsoleColor>(), Times.Never);
            this.facade.VerifySet(x => x.Background = It.IsAny<ConsoleColor>(), Times.Never);
            this.facade.Verify(x => x.Write(It.IsAny<String>()), Times.Never);
        }

        [Test]
        public void Write_CheckEnabledIsFalse_FacadeCalledNever()
        {
            this.settings.SetupGet(x => x.LogLevel).Returns(LogLevel.Critical);

            DummyClass instance = new DummyClass(this.settings.Object, this.facade.Object);

            instance.TestWrite(LogLevel.Message, null, null, null, null, null);

            this.facade.VerifyGet(x => x.UseColors, Times.Never);
            this.facade.VerifySet(x => x.UseColors = It.IsAny<Boolean>(), Times.Never);
            this.facade.VerifySet(x => x.Foreground = It.IsAny<ConsoleColor>(), Times.Never);
            this.facade.VerifySet(x => x.Background = It.IsAny<ConsoleColor>(), Times.Never);
            this.facade.Verify(x => x.Write(It.IsAny<String>()), Times.Never);
        }

        [Test]
        public void Write_OutputIsEmpty_FacadeCalledNever()
        {
            this.settings.SetupGet(x => x.LogLevel).Returns(LogLevel.Trace);

            DummyClass instance = new DummyClass(this.settings.Object, this.facade.Object);

            instance.TestWrite(LogLevel.Message, null, null, null, null, null);

            this.facade.VerifyGet(x => x.UseColors, Times.Never);
            this.facade.VerifySet(x => x.UseColors = It.IsAny<Boolean>(), Times.Never);
            this.facade.VerifySet(x => x.Foreground = It.IsAny<ConsoleColor>(), Times.Never);
            this.facade.VerifySet(x => x.Background = It.IsAny<ConsoleColor>(), Times.Never);
            this.facade.Verify(x => x.Write(It.IsAny<String>()), Times.Never);
        }

        [TestCase(true, 1, 2)]
        [TestCase(false, 1, 1)]
        public void Write_AffectedColoringHandling_FacadePropertiesCalledAsExpected(Boolean used, Int32 gets, Int32 sets)
        {
            LogLevel level = LogLevel.Message;
            Coloring coloring = new Coloring(ConsoleColor.Cyan, ConsoleColor.DarkCyan);

            this.settings.SetupGet(x => x.LogLevel).Returns(LogLevel.Trace);
            this.settings.SetupGet(x => x.UseColors).Returns(used);
            this.settings.SetupGet(x => x.Coloring).Returns(this.colorings.Object);
            this.colorings.Setup(x => x.TryGetValue(level, out coloring)).Returns(true);

            DummyClass instance = new DummyClass(this.settings.Object, this.facade.Object);

            instance.TestWrite(level, null, null, "message", null, null);

            this.facade.VerifyGet(x => x.UseColors, Times.Exactly(gets));
            this.facade.VerifySet(x => x.UseColors = It.IsAny<Boolean>(), Times.Exactly(sets));
            this.facade.VerifySet(x => x.Foreground = ConsoleColor.Cyan, Times.Exactly(sets - 1));
            this.facade.VerifySet(x => x.Background = ConsoleColor.DarkCyan, Times.Exactly(sets - 1));
        }

        [Test]
        public void Write_AffectedColoringHandlingWithInvalidLevel_FacadeColoringCalledWithDefaults()
        {
            LogLevel level = (LogLevel)42;
            Coloring coloring = null;

            this.settings.SetupGet(x => x.LogLevel).Returns(LogLevel.Trace);
            this.settings.SetupGet(x => x.UseColors).Returns(true);
            this.settings.SetupGet(x => x.Coloring).Returns(this.colorings.Object);
            this.colorings.Setup(x => x.TryGetValue(level, out coloring)).Returns(false);

            DummyClass instance = new DummyClass(this.settings.Object, this.facade.Object);

            instance.TestWrite(level, null, null, "message", null, null);

            this.facade.VerifySet(x => x.Foreground = ConsoleColor.Gray, Times.Once);
            this.facade.VerifySet(x => x.Background = ConsoleColor.Black, Times.Once);
        }

        [TestCase(LogLevel.Trace, ConsoleColor.Gray, ConsoleColor.Black)]
        [TestCase(LogLevel.Debug, ConsoleColor.Gray, ConsoleColor.Black)]
        [TestCase(LogLevel.Verbose, ConsoleColor.White, ConsoleColor.Black)]
        [TestCase(LogLevel.Message, ConsoleColor.White, ConsoleColor.Black)]
        [TestCase(LogLevel.Warning, ConsoleColor.Yellow, ConsoleColor.Black)]
        [TestCase(LogLevel.Error, ConsoleColor.Red, ConsoleColor.Black)]
        [TestCase(LogLevel.Fatal, ConsoleColor.Gray, ConsoleColor.DarkRed)]
        [TestCase(LogLevel.Critical, ConsoleColor.Black, ConsoleColor.Red)]
        [TestCase(LogLevel.Disaster, ConsoleColor.Black, ConsoleColor.Red)]
        public void Write_MultipleColoringHandling_FacadePropertiesCalledAsExpected(LogLevel level, ConsoleColor foreground, ConsoleColor background)
        {
            Coloring coloring = new Coloring(foreground, background);

            this.settings.SetupGet(x => x.LogLevel).Returns(LogLevel.Trace);
            this.settings.SetupGet(x => x.UseColors).Returns(true);
            this.settings.SetupGet(x => x.Coloring).Returns(this.colorings.Object);
            this.colorings.Setup(x => x.TryGetValue(level, out coloring)).Returns(true);

            DummyClass instance = new DummyClass(this.settings.Object, this.facade.Object);

            instance.TestWrite(level, null, null, "message", null, null);

            this.facade.VerifySet(x => x.Foreground = foreground, Times.Once);
            this.facade.VerifySet(x => x.Background = background, Times.Once);
        }

        [Test]
        public void Write_WriteFacade_FacadeWriteCalledOnce()
        {
            this.settings.SetupGet(x => x.LogLevel).Returns(LogLevel.Trace);
            this.settings.SetupGet(x => x.UseColors).Returns(false);

            DummyClass instance = new DummyClass(this.settings.Object, this.facade.Object);

            instance.TestWrite(LogLevel.Message, null, null, "message", null, null);

            this.facade.Verify(x => x.Write(It.IsAny<String>()), Times.Once);
        }

        [Test]
        public void Write_FlushFacade_FacadeFlushCalledOnce()
        {
            this.settings.SetupGet(x => x.LogLevel).Returns(LogLevel.Trace);
            this.settings.SetupGet(x => x.UseColors).Returns(false);

            DummyClass instance = new DummyClass(this.settings.Object, this.facade.Object);

            instance.TestWrite(LogLevel.Message, null, null, "message", null, null);

            this.facade.Verify(x => x.Flush(), Times.Once);
        }

        #endregion

        #region Private test class implementations

        private class DummyClass : ConsoleLoggerBase
        {
            public DummyClass(IConsoleLoggerSettings settings, IConsoleLoggerFacade facade)
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
