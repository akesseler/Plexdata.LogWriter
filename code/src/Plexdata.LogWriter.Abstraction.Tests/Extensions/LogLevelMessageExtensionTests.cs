﻿/*
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
using Plexdata.LogWriter.Definitions;
using Plexdata.LogWriter.Extensions;
using Plexdata.Utilities.Testing;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace Plexdata.LogWriter.Abstraction.Tests.Extensions
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    [Category(TestType.UnitTest)]
    [TestOf(nameof(LogLevelMessageExtension))]
    public class LogLevelMessageExtensionTests
    {
        #region Prologue

        private Mock<ILogger> defaultLogger = null;
        private Mock<IMock<ILogger>> defaultNullLogger = null;
        private Mock<ILogger<DummyClass>> contextLogger = null;
        private Mock<IMock<ILogger<DummyClass>>> contextNullLogger = null;
        private DummyClass dummyClass = null;
        private String message = null;
        private Exception exception = null;
        private MethodBase method = null;
        private (String Label, Object Value)[] details = null;

        [SetUp]
        public void Setup()
        {
            this.defaultLogger = new Mock<ILogger>();
            this.defaultNullLogger = new Mock<IMock<ILogger>>();

            this.contextLogger = new Mock<ILogger<DummyClass>>();
            this.contextNullLogger = new Mock<IMock<ILogger<DummyClass>>>();

            this.dummyClass = new DummyClass();

            this.defaultNullLogger.SetupGet(x => x.Object).Returns(() => { return (ILogger)null; });
            this.contextNullLogger.SetupGet(x => x.Object).Returns(() => { return (ILogger<DummyClass>)null; });

            this.message = "message";
            this.exception = new Exception();
            this.method = this.dummyClass.TestMethod();
            this.details = new (String Label, Object Value)[] { (Label: "Label", Value: "Value") };
        }

        #endregion

        #region Message

        [Test]
        public void Message_DefaultNullLoggerMessage_WriteMessageCalledNever()
        {
            this.defaultNullLogger.Object.Object.Message(this.message);
            this.defaultNullLogger.Verify(x => x.Object.Write(LogLevel.Message, this.message), Times.Never);
        }

        [Test]
        public void Message_DefaultNullLoggerMessageDetails_WriteMessageDetailsCalledNever()
        {
            this.defaultNullLogger.Object.Object.Message(this.message, this.details);
            this.defaultNullLogger.Verify(x => x.Object.Write(LogLevel.Message, this.message, this.details), Times.Never);
        }

        [Test]
        public void Message_DefaultLoggerMessage_WriteMessageCalledOnce()
        {
            this.defaultLogger.Object.Message(this.message);
            this.defaultLogger.Verify(x => x.Write(LogLevel.Message, this.message), Times.Once);
        }

        [Test]
        public void Message_DefaultLoggerMessageDetails_WriteMessageDetailsCalledOnce()
        {
            this.defaultLogger.Object.Message(this.message, this.details);
            this.defaultLogger.Verify(x => x.Write(LogLevel.Message, this.message, this.details), Times.Once);
        }

        [Test]
        public void Message_DefaultNullLoggerException_WriteExceptionCalledNever()
        {
            this.defaultNullLogger.Object.Object.Message(this.exception);
            this.defaultNullLogger.Verify(x => x.Object.Write(LogLevel.Message, this.exception), Times.Never);
        }

        [Test]
        public void Message_DefaultNullLoggerExceptionDetails_WriteExceptionDetailsCalledNever()
        {
            this.defaultNullLogger.Object.Object.Message(this.exception, this.details);
            this.defaultNullLogger.Verify(x => x.Object.Write(LogLevel.Message, this.exception, this.details), Times.Never);
        }

        [Test]
        public void Message_DefaultLoggerException_WriteExceptionCalledOnce()
        {
            this.defaultLogger.Object.Message(this.exception);
            this.defaultLogger.Verify(x => x.Write(LogLevel.Message, this.exception), Times.Once);
        }

        [Test]
        public void Message_DefaultLoggerExceptionDetails_WriteExceptionDetailsCalledOnce()
        {
            this.defaultLogger.Object.Message(this.exception, this.details);
            this.defaultLogger.Verify(x => x.Write(LogLevel.Message, this.exception, this.details), Times.Once);
        }

        [Test]
        public void Message_DefaultNullLoggerMessageException_WriteMessageExceptionCalledNever()
        {
            this.defaultNullLogger.Object.Object.Message(this.message, this.exception);
            this.defaultNullLogger.Verify(x => x.Object.Write(LogLevel.Message, this.message, this.exception), Times.Never);
        }

        [Test]
        public void Message_DefaultNullLoggerMessageExceptionDetails_WriteMessageExceptionDetailsCalledNever()
        {
            this.defaultNullLogger.Object.Object.Message(this.message, this.exception, this.details);
            this.defaultNullLogger.Verify(x => x.Object.Write(LogLevel.Message, this.message, this.exception, this.details), Times.Never);
        }

        [Test]
        public void Message_DefaultLoggerMessageException_WriteMessageExceptionCalledOnce()
        {
            this.defaultLogger.Object.Message(this.message, this.exception);
            this.defaultLogger.Verify(x => x.Write(LogLevel.Message, this.message, this.exception), Times.Once);
        }

        [Test]
        public void Message_DefaultLoggerMessageExceptionDetails_WriteMessageExceptionDetailsCalledOnce()
        {
            this.defaultLogger.Object.Message(this.message, this.exception, this.details);
            this.defaultLogger.Verify(x => x.Write(LogLevel.Message, this.message, this.exception, this.details), Times.Once);
        }

        [Test]
        public void Message_DefaultNullLoggerScopeMessage_WriteMessageCalledNever()
        {
            this.defaultNullLogger.Object.Object.Message(this.method, this.message);
            this.defaultNullLogger.Verify(x => x.Object.Write(this.method, LogLevel.Message, this.message), Times.Never);
        }

        [Test]
        public void Message_DefaultNullLoggerScopeMessageDetails_WriteMessageDetailsCalledNever()
        {
            this.defaultNullLogger.Object.Object.Message(this.method, this.message, this.details);
            this.defaultNullLogger.Verify(x => x.Object.Write(this.method, LogLevel.Message, this.message, this.details), Times.Never);
        }

        [Test]
        public void Message_DefaultLoggerScopeMessage_WriteMessageCalledOnce()
        {
            this.defaultLogger.Object.Message(this.method, this.message);
            this.defaultLogger.Verify(x => x.Write(this.method, LogLevel.Message, this.message), Times.Once);
        }

        [Test]
        public void Message_DefaultLoggerScopeMessageDetails_WriteMessageDetailsCalledOnce()
        {
            this.defaultLogger.Object.Message(this.method, this.message, this.details);
            this.defaultLogger.Verify(x => x.Write(this.method, LogLevel.Message, this.message, this.details), Times.Once);
        }

        [Test]
        public void Message_DefaultNullLoggerScopeException_WriteExceptionCalledNever()
        {
            this.defaultNullLogger.Object.Object.Message(this.method, this.exception);
            this.defaultNullLogger.Verify(x => x.Object.Write(this.method, LogLevel.Message, this.exception), Times.Never);
        }

        [Test]
        public void Message_DefaultNullLoggerScopeExceptionDetails_WriteExceptionDetailsCalledNever()
        {
            this.defaultNullLogger.Object.Object.Message(this.method, this.exception, this.details);
            this.defaultNullLogger.Verify(x => x.Object.Write(this.method, LogLevel.Message, this.exception, this.details), Times.Never);
        }

        [Test]
        public void Message_DefaultLoggerScopeException_WriteExceptionCalledOnce()
        {
            this.defaultLogger.Object.Message(this.method, this.exception);
            this.defaultLogger.Verify(x => x.Write(this.method, LogLevel.Message, this.exception), Times.Once);
        }

        [Test]
        public void Message_DefaultLoggerScopeExceptionDetails_WriteExceptionDetailsCalledOnce()
        {
            this.defaultLogger.Object.Message(this.method, this.exception, this.details);
            this.defaultLogger.Verify(x => x.Write(this.method, LogLevel.Message, this.exception, this.details), Times.Once);
        }

        [Test]
        public void Message_DefaultNullLoggerScopeMessageException_WriteMessageExceptionCalledNever()
        {
            this.defaultNullLogger.Object.Object.Message(this.method, this.message, this.exception);
            this.defaultNullLogger.Verify(x => x.Object.Write(this.method, LogLevel.Message, this.message, this.exception), Times.Never);
        }

        [Test]
        public void Message_DefaultNullLoggerScopeMessageExceptionDetails_WriteMessageExceptionDetailsCalledNever()
        {
            this.defaultNullLogger.Object.Object.Message(this.method, this.message, this.exception, this.details);
            this.defaultNullLogger.Verify(x => x.Object.Write(this.method, LogLevel.Message, this.message, this.exception, this.details), Times.Never);
        }

        [Test]
        public void Message_DefaultLoggerScopeMessageException_WriteMessageExceptionCalledOnce()
        {
            this.defaultLogger.Object.Message(this.method, this.message, this.exception);
            this.defaultLogger.Verify(x => x.Write(this.method, LogLevel.Message, this.message, this.exception), Times.Once);
        }

        [Test]
        public void Message_DefaultLoggerScopeMessageExceptionDetails_WriteMessageExceptionDetailsCalledOnce()
        {
            this.defaultLogger.Object.Message(this.method, this.message, this.exception, this.details);
            this.defaultLogger.Verify(x => x.Write(this.method, LogLevel.Message, this.message, this.exception, this.details), Times.Once);
        }

        [Test]
        public void Message_ContextNullLoggerMessage_WriteMessageCalledNever()
        {
            this.contextNullLogger.Object.Object.Message(this.message);
            this.contextNullLogger.Verify(x => x.Object.Write(LogLevel.Message, this.message), Times.Never);
        }

        [Test]
        public void Message_ContextNullLoggerMessageDetails_WriteMessageDetailsCalledNever()
        {
            this.contextNullLogger.Object.Object.Message(this.message, this.details);
            this.contextNullLogger.Verify(x => x.Object.Write(LogLevel.Message, this.message, this.details), Times.Never);
        }

        [Test]
        public void Message_ContextLoggerValidMessage_WriteMessageCalledOnce()
        {
            this.contextLogger.Object.Message(this.message);
            this.contextLogger.Verify(x => x.Write(LogLevel.Message, this.message), Times.Once);
        }

        [Test]
        public void Message_ContextLoggerValidMessageDetails_WriteMessageDetailsCalledOnce()
        {
            this.contextLogger.Object.Message(this.message, this.details);
            this.contextLogger.Verify(x => x.Write(LogLevel.Message, this.message, this.details), Times.Once);
        }

        [Test]
        public void Message_ContextNullLoggerException_WriteExceptionCalledNever()
        {
            this.contextNullLogger.Object.Object.Message(this.exception);
            this.contextNullLogger.Verify(x => x.Object.Write(LogLevel.Message, this.exception), Times.Never);
        }

        [Test]
        public void Message_ContextNullLoggerExceptionDetails_WriteExceptionDetailsCalledNever()
        {
            this.contextNullLogger.Object.Object.Message(this.exception, this.details);
            this.contextNullLogger.Verify(x => x.Object.Write(LogLevel.Message, this.exception, this.details), Times.Never);
        }

        [Test]
        public void Message_ContextLoggerValidException_WriteExceptionCalledOnce()
        {
            this.contextLogger.Object.Message(this.exception);
            this.contextLogger.Verify(x => x.Write(LogLevel.Message, this.exception), Times.Once);
        }

        [Test]
        public void Message_ContextLoggerValidExceptionDetails_WriteExceptionDetailsCalledOnce()
        {
            this.contextLogger.Object.Message(this.exception, this.details);
            this.contextLogger.Verify(x => x.Write(LogLevel.Message, this.exception, this.details), Times.Once);
        }

        [Test]
        public void Message_ContextNullLoggerMessageException_WriteMessageExceptionCalledNever()
        {
            this.contextNullLogger.Object.Object.Message(this.message, this.exception);
            this.contextNullLogger.Verify(x => x.Object.Write(LogLevel.Message, this.message, this.exception), Times.Never);
        }

        [Test]
        public void Message_ContextNullLoggerMessageExceptionDetails_WriteMessageExceptionDetailsCalledNever()
        {
            this.contextNullLogger.Object.Object.Message(this.message, this.exception, this.details);
            this.contextNullLogger.Verify(x => x.Object.Write(LogLevel.Message, this.message, this.exception, this.details), Times.Never);
        }

        [Test]
        public void Message_ContextLoggerValidMessageException_WriteMessageExceptionCalledOnce()
        {
            this.contextLogger.Object.Message(this.message, this.exception);
            this.contextLogger.Verify(x => x.Write(LogLevel.Message, this.message, this.exception), Times.Once);
        }

        [Test]
        public void Message_ContextLoggerValidMessageExceptionDetails_WriteMessageExceptionDetailsCalledOnce()
        {
            this.contextLogger.Object.Message(this.message, this.exception, this.details);
            this.contextLogger.Verify(x => x.Write(LogLevel.Message, this.message, this.exception, this.details), Times.Once);
        }

        [Test]
        public void Message_ContextNullLoggerScopeMessage_WriteMessageCalledNever()
        {
            this.contextNullLogger.Object.Object.Message(this.method, this.message);
            this.contextNullLogger.Verify(x => x.Object.Write(this.method, LogLevel.Message, this.message), Times.Never);
        }

        [Test]
        public void Message_ContextNullLoggerScopeMessageDetails_WriteMessageDetailsCalledNever()
        {
            this.contextNullLogger.Object.Object.Message(this.method, this.message, this.details);
            this.contextNullLogger.Verify(x => x.Object.Write(this.method, LogLevel.Message, this.message, this.details), Times.Never);
        }

        [Test]
        public void Message_ContextLoggerScopeMessage_WriteMessageCalledOnce()
        {
            this.contextLogger.Object.Message(this.method, this.message);
            this.contextLogger.Verify(x => x.Write(this.method, LogLevel.Message, this.message), Times.Once);
        }

        [Test]
        public void Message_ContextLoggerScopeMessageDetails_WriteMessageDetailsCalledOnce()
        {
            this.contextLogger.Object.Message(this.method, this.message, this.details);
            this.contextLogger.Verify(x => x.Write(this.method, LogLevel.Message, this.message, this.details), Times.Once);
        }

        [Test]
        public void Message_ContextNullLoggerScopeException_WriteExceptionCalledNever()
        {
            this.contextNullLogger.Object.Object.Message(this.method, this.exception);
            this.contextNullLogger.Verify(x => x.Object.Write(this.method, LogLevel.Message, this.exception), Times.Never);
        }

        [Test]
        public void Message_ContextNullLoggerScopeExceptionDetails_WriteExceptionDetailsCalledNever()
        {
            this.contextNullLogger.Object.Object.Message(this.method, this.exception, this.details);
            this.contextNullLogger.Verify(x => x.Object.Write(this.method, LogLevel.Message, this.exception, this.details), Times.Never);
        }

        [Test]
        public void Message_ContextLoggerScopeException_WriteExceptionCalledOnce()
        {
            this.contextLogger.Object.Message(this.method, this.exception);
            this.contextLogger.Verify(x => x.Write(this.method, LogLevel.Message, this.exception), Times.Once);
        }

        [Test]
        public void Message_ContextLoggerScopeExceptionDetails_WriteExceptionDetailsCalledOnce()
        {
            this.contextLogger.Object.Message(this.method, this.exception, this.details);
            this.contextLogger.Verify(x => x.Write(this.method, LogLevel.Message, this.exception, this.details), Times.Once);
        }

        [Test]
        public void Message_ContextNullLoggerScopeMessageException_WriteMessageExceptionCalledNever()
        {
            this.contextNullLogger.Object.Object.Message(this.method, this.message, this.exception);
            this.contextNullLogger.Verify(x => x.Object.Write(this.method, LogLevel.Message, this.message, this.exception), Times.Never);
        }

        [Test]
        public void Message_ContextNullLoggerScopeMessageExceptionDetails_WriteMessageExceptionDetailsCalledNever()
        {
            this.contextNullLogger.Object.Object.Message(this.method, this.message, this.exception, this.details);
            this.contextNullLogger.Verify(x => x.Object.Write(this.method, LogLevel.Message, this.message, this.exception, this.details), Times.Never);
        }

        [Test]
        public void Message_ContextLoggerScopeMessageException_WriteMessageExceptionCalledOnce()
        {
            this.contextLogger.Object.Message(this.method, this.message, this.exception);
            this.contextLogger.Verify(x => x.Write(this.method, LogLevel.Message, this.message, this.exception), Times.Once);
        }

        [Test]
        public void Message_ContextLoggerScopeMessageExceptionDetails_WriteMessageExceptionDetailsCalledOnce()
        {
            this.contextLogger.Object.Message(this.method, this.message, this.exception, this.details);
            this.contextLogger.Verify(x => x.Write(this.method, LogLevel.Message, this.message, this.exception, this.details), Times.Once);
        }

        #endregion

        #region Public test class implementations

        public class DummyClass // The mocked mocker requires this as public class.
        {
            public MethodBase TestMethod()
            {
                return MethodBase.GetCurrentMethod();
            }
        }

        #endregion
    }
}
