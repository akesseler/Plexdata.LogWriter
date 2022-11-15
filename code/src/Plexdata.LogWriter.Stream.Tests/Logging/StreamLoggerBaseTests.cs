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
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text;

namespace Plexdata.LogWriter.Stream.Tests.Logging
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    [Category(TestType.UnitTest)]
    [TestOf(nameof(StreamLoggerBase))]
    public class StreamLoggerBaseTests
    {
        #region Prologue

        private MemoryStream stream;
        private DummyClass instance = null;
        private Mock<IStreamLoggerSettings> settings = null;
        private Mock<IStreamLoggerFacade> facade = null;

        [SetUp]
        public void Setup()
        {
            this.stream = new MemoryStream();

            this.settings = new Mock<IStreamLoggerSettings>();
            this.facade = new Mock<IStreamLoggerFacade>();

            this.settings.SetupGet(x => x.Stream).Returns(this.stream);

            this.instance = new DummyClass(this.settings.Object, this.facade.Object);
        }

        [TearDown]
        public void Cleanup()
        {
            this.stream.Dispose();
            this.stream = null;
        }

        #endregion

        #region Construction

        [Test]
        public void StreamLoggerBase_SettingsNull_ThrowsArgumentNullException()
        {
            Assert.That(() => new DummyClass(null, this.facade.Object), Throws.ArgumentNullException);
        }

        [Test]
        public void StreamLoggerBase_FacadeNull_ThrowsArgumentNullException()
        {
            Assert.That(() => new DummyClass(this.settings.Object, null), Throws.ArgumentNullException);
        }

        #endregion

        #region Write

        [Test]
        public void Write_CheckDisposedIsTrue_NothingIsExecuted()
        {
            this.instance.Dispose();

            this.instance.TestWrite(LogLevel.Message, null, null, null, null, null);

            this.settings.VerifyGet(x => x.Stream, Times.Never);
            this.facade.Verify(x => x.Write(It.IsAny<System.IO.Stream>(), It.IsAny<Encoding>(), It.IsAny<String>()), Times.Never);
            this.facade.Verify(x => x.Write(It.IsAny<System.IO.Stream>(), It.IsAny<Encoding>(), It.IsAny<IEnumerable<String>>()), Times.Never);
        }

        [Test]
        public void Write_CheckDisabledIsTrue_NothingIsExecuted()
        {
            this.settings.SetupGet(x => x.LogLevel).Returns(LogLevel.Disabled);

            this.instance.TestWrite(LogLevel.Message, null, null, null, null, null);

            this.settings.VerifyGet(x => x.Stream, Times.Never);
            this.facade.Verify(x => x.Write(It.IsAny<System.IO.Stream>(), It.IsAny<Encoding>(), It.IsAny<String>()), Times.Never);
            this.facade.Verify(x => x.Write(It.IsAny<System.IO.Stream>(), It.IsAny<Encoding>(), It.IsAny<IEnumerable<String>>()), Times.Never);
        }

        [Test]
        public void Write_CheckEnabledIsFalse_NothingIsExecuted()
        {
            this.settings.SetupGet(x => x.LogLevel).Returns(LogLevel.Critical);

            this.instance.TestWrite(LogLevel.Message, null, null, null, null, null);

            this.settings.VerifyGet(x => x.Stream, Times.Never);
            this.facade.Verify(x => x.Write(It.IsAny<System.IO.Stream>(), It.IsAny<Encoding>(), It.IsAny<String>()), Times.Never);
            this.facade.Verify(x => x.Write(It.IsAny<System.IO.Stream>(), It.IsAny<Encoding>(), It.IsAny<IEnumerable<String>>()), Times.Never);
        }

        [Test]
        public void Write_CheckStreamIsNull_NothingIsExecuted()
        {
            this.settings.SetupGet(x => x.LogLevel).Returns(LogLevel.Trace);
            this.settings.SetupGet(x => x.Stream).Returns((System.IO.Stream)null);

            this.instance.TestWrite(LogLevel.Message, null, null, null, null, null);

            this.settings.VerifyGet(x => x.Stream, Times.Once);
            this.facade.Verify(x => x.Write(It.IsAny<System.IO.Stream>(), It.IsAny<Encoding>(), It.IsAny<String>()), Times.Never);
            this.facade.Verify(x => x.Write(It.IsAny<System.IO.Stream>(), It.IsAny<Encoding>(), It.IsAny<IEnumerable<String>>()), Times.Never);
        }

        [Test]
        public void Write_OutputIsEmpty_NothingIsExecuted()
        {
            this.settings.SetupGet(x => x.LogLevel).Returns(LogLevel.Trace);

            this.instance.TestWrite(LogLevel.Message, null, null, null, null, null);

            this.settings.VerifyGet(x => x.Stream, Times.Once);
            this.facade.Verify(x => x.Write(It.IsAny<System.IO.Stream>(), It.IsAny<Encoding>(), It.IsAny<String>()), Times.Never);
            this.facade.Verify(x => x.Write(It.IsAny<System.IO.Stream>(), It.IsAny<Encoding>(), It.IsAny<IEnumerable<String>>()), Times.Never);
        }

        [TestCase(true)]
        [TestCase(false)]
        public void Write_CheckCallFacadeWrite_FacadeWriteSingleWasCalledOnce(Boolean returns)
        {
            this.settings.SetupGet(x => x.LogLevel).Returns(LogLevel.Trace);
            this.settings.SetupGet(x => x.Encoding).Returns(Encoding.UTF8);

            this.facade.Setup(x => x.Write(It.IsAny<System.IO.Stream>(), It.IsAny<Encoding>(), It.IsAny<String>())).Returns(returns);
            this.facade.Setup(x => x.Write(It.IsAny<System.IO.Stream>(), It.IsAny<Encoding>(), It.IsAny<IEnumerable<String>>())).Returns(() => { throw new Exception(); });

            this.instance.TestWrite(LogLevel.Message, null, null, "message", null, null);

            this.facade.Verify(x => x.Write(It.IsAny<System.IO.Stream>(), It.IsAny<Encoding>(), It.IsAny<String>()), Times.Once);
            this.facade.Verify(x => x.Write(It.IsAny<System.IO.Stream>(), It.IsAny<Encoding>(), It.IsAny<IEnumerable<String>>()), Times.Never);
        }

        #endregion

        #region Private test class implementations

        private class DummyClass : StreamLoggerBase
        {
            public DummyClass(IStreamLoggerSettings settings, IStreamLoggerFacade facade)
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
