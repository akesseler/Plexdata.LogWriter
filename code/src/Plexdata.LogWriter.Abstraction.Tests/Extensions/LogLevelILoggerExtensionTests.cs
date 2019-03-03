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
using Plexdata.LogWriter.Extensions;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace Plexdata.LogWriter.Abstraction.Tests.Extensions
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    [TestOf(nameof(LogLevelILoggerExtension))]
    public class LogLevelILoggerExtensionTests
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

        #region Trace

        [Test]
        public void Trace_DefaultNullLoggerMessage_WriteMessageCalledNever()
        {
            this.defaultNullLogger.Object.Object.Trace(this.message);
            this.defaultNullLogger.Verify(x => x.Object.Write(LogLevel.Trace, this.message), Times.Never);
        }

        [Test]
        public void Trace_DefaultNullLoggerMessageDetails_WriteMessageDetailsCalledNever()
        {
            this.defaultNullLogger.Object.Object.Trace(this.message, this.details);
            this.defaultNullLogger.Verify(x => x.Object.Write(LogLevel.Trace, this.message, this.details), Times.Never);
        }

        [Test]
        public void Trace_DefaultLoggerMessage_WriteMessageCalledOnce()
        {
            this.defaultLogger.Object.Trace(this.message);
            this.defaultLogger.Verify(x => x.Write(LogLevel.Trace, this.message), Times.Once);
        }

        [Test]
        public void Trace_DefaultLoggerMessageDetails_WriteMessageDetailsCalledOnce()
        {
            this.defaultLogger.Object.Trace(this.message, this.details);
            this.defaultLogger.Verify(x => x.Write(LogLevel.Trace, this.message, this.details), Times.Once);
        }

        [Test]
        public void Trace_DefaultNullLoggerException_WriteExceptionCalledNever()
        {
            this.defaultNullLogger.Object.Object.Trace(this.exception);
            this.defaultNullLogger.Verify(x => x.Object.Write(LogLevel.Trace, this.exception), Times.Never);
        }

        [Test]
        public void Trace_DefaultNullLoggerExceptionDetails_WriteExceptionDetailsCalledNever()
        {
            this.defaultNullLogger.Object.Object.Trace(this.exception, this.details);
            this.defaultNullLogger.Verify(x => x.Object.Write(LogLevel.Trace, this.exception, this.details), Times.Never);
        }

        [Test]
        public void Trace_DefaultLoggerException_WriteExceptionCalledOnce()
        {
            this.defaultLogger.Object.Trace(this.exception);
            this.defaultLogger.Verify(x => x.Write(LogLevel.Trace, this.exception), Times.Once);
        }

        [Test]
        public void Trace_DefaultLoggerExceptionDetails_WriteExceptionDetailsCalledOnce()
        {
            this.defaultLogger.Object.Trace(this.exception, this.details);
            this.defaultLogger.Verify(x => x.Write(LogLevel.Trace, this.exception, this.details), Times.Once);
        }

        [Test]
        public void Trace_DefaultNullLoggerMessageException_WriteMessageExceptionCalledNever()
        {
            this.defaultNullLogger.Object.Object.Trace(this.message, this.exception);
            this.defaultNullLogger.Verify(x => x.Object.Write(LogLevel.Trace, this.message, this.exception), Times.Never);
        }

        [Test]
        public void Trace_DefaultNullLoggerMessageExceptionDetails_WriteMessageExceptionDetailsCalledNever()
        {
            this.defaultNullLogger.Object.Object.Trace(this.message, this.exception, this.details);
            this.defaultNullLogger.Verify(x => x.Object.Write(LogLevel.Trace, this.message, this.exception, this.details), Times.Never);
        }

        [Test]
        public void Trace_DefaultLoggerMessageException_WriteMessageExceptionCalledOnce()
        {
            this.defaultLogger.Object.Trace(this.message, this.exception);
            this.defaultLogger.Verify(x => x.Write(LogLevel.Trace, this.message, this.exception), Times.Once);
        }

        [Test]
        public void Trace_DefaultLoggerMessageExceptionDetails_WriteMessageExceptionDetailsCalledOnce()
        {
            this.defaultLogger.Object.Trace(this.message, this.exception, this.details);
            this.defaultLogger.Verify(x => x.Write(LogLevel.Trace, this.message, this.exception, this.details), Times.Once);
        }

        [Test]
        public void Trace_DefaultNullLoggerScopeMessage_WriteMessageCalledNever()
        {
            this.defaultNullLogger.Object.Object.Trace(this.method, this.message);
            this.defaultNullLogger.Verify(x => x.Object.Write(this.method, LogLevel.Trace, this.message), Times.Never);
        }

        [Test]
        public void Trace_DefaultNullLoggerScopeMessageDetails_WriteMessageDetailsCalledNever()
        {
            this.defaultNullLogger.Object.Object.Trace(this.method, this.message, this.details);
            this.defaultNullLogger.Verify(x => x.Object.Write(this.method, LogLevel.Trace, this.message, this.details), Times.Never);
        }

        [Test]
        public void Trace_DefaultLoggerScopeMessage_WriteMessageCalledOnce()
        {
            this.defaultLogger.Object.Trace(this.method, this.message);
            this.defaultLogger.Verify(x => x.Write(this.method, LogLevel.Trace, this.message), Times.Once);
        }

        [Test]
        public void Trace_DefaultLoggerScopeMessageDetails_WriteMessageDetailsCalledOnce()
        {
            this.defaultLogger.Object.Trace(this.method, this.message, this.details);
            this.defaultLogger.Verify(x => x.Write(this.method, LogLevel.Trace, this.message, this.details), Times.Once);
        }

        [Test]
        public void Trace_DefaultNullLoggerScopeException_WriteExceptionCalledNever()
        {
            this.defaultNullLogger.Object.Object.Trace(this.method, this.exception);
            this.defaultNullLogger.Verify(x => x.Object.Write(this.method, LogLevel.Trace, this.exception), Times.Never);
        }

        [Test]
        public void Trace_DefaultNullLoggerScopeExceptionDetails_WriteExceptionDetailsCalledNever()
        {
            this.defaultNullLogger.Object.Object.Trace(this.method, this.exception, this.details);
            this.defaultNullLogger.Verify(x => x.Object.Write(this.method, LogLevel.Trace, this.exception, this.details), Times.Never);
        }

        [Test]
        public void Trace_DefaultLoggerScopeException_WriteExceptionCalledOnce()
        {
            this.defaultLogger.Object.Trace(this.method, this.exception);
            this.defaultLogger.Verify(x => x.Write(this.method, LogLevel.Trace, this.exception), Times.Once);
        }

        [Test]
        public void Trace_DefaultLoggerScopeExceptionDetails_WriteExceptionDetailsCalledOnce()
        {
            this.defaultLogger.Object.Trace(this.method, this.exception, this.details);
            this.defaultLogger.Verify(x => x.Write(this.method, LogLevel.Trace, this.exception, this.details), Times.Once);
        }

        [Test]
        public void Trace_DefaultNullLoggerScopeMessageException_WriteMessageExceptionCalledNever()
        {
            this.defaultNullLogger.Object.Object.Trace(this.method, this.message, this.exception);
            this.defaultNullLogger.Verify(x => x.Object.Write(this.method, LogLevel.Trace, this.message, this.exception), Times.Never);
        }

        [Test]
        public void Trace_DefaultNullLoggerScopeMessageExceptionDetails_WriteMessageExceptionDetailsCalledNever()
        {
            this.defaultNullLogger.Object.Object.Trace(this.method, this.message, this.exception, this.details);
            this.defaultNullLogger.Verify(x => x.Object.Write(this.method, LogLevel.Trace, this.message, this.exception, this.details), Times.Never);
        }

        [Test]
        public void Trace_DefaultLoggerScopeMessageException_WriteMessageExceptionCalledOnce()
        {
            this.defaultLogger.Object.Trace(this.method, this.message, this.exception);
            this.defaultLogger.Verify(x => x.Write(this.method, LogLevel.Trace, this.message, this.exception), Times.Once);
        }

        [Test]
        public void Trace_DefaultLoggerScopeMessageExceptionDetails_WriteMessageExceptionDetailsCalledOnce()
        {
            this.defaultLogger.Object.Trace(this.method, this.message, this.exception, this.details);
            this.defaultLogger.Verify(x => x.Write(this.method, LogLevel.Trace, this.message, this.exception, this.details), Times.Once);
        }

        [Test]
        public void Trace_ContextNullLoggerMessage_WriteMessageCalledNever()
        {
            this.contextNullLogger.Object.Object.Trace(this.message);
            this.contextNullLogger.Verify(x => x.Object.Write(LogLevel.Trace, this.message), Times.Never);
        }

        [Test]
        public void Trace_ContextNullLoggerMessageDetails_WriteMessageDetailsCalledNever()
        {
            this.contextNullLogger.Object.Object.Trace(this.message, this.details);
            this.contextNullLogger.Verify(x => x.Object.Write(LogLevel.Trace, this.message, this.details), Times.Never);
        }

        [Test]
        public void Trace_ContextLoggerValidMessage_WriteMessageCalledOnce()
        {
            this.contextLogger.Object.Trace(this.message);
            this.contextLogger.Verify(x => x.Write(LogLevel.Trace, this.message), Times.Once);
        }

        [Test]
        public void Trace_ContextLoggerValidMessageDetails_WriteMessageDetailsCalledOnce()
        {
            this.contextLogger.Object.Trace(this.message, this.details);
            this.contextLogger.Verify(x => x.Write(LogLevel.Trace, this.message, this.details), Times.Once);
        }

        [Test]
        public void Trace_ContextNullLoggerException_WriteExceptionCalledNever()
        {
            this.contextNullLogger.Object.Object.Trace(this.exception);
            this.contextNullLogger.Verify(x => x.Object.Write(LogLevel.Trace, this.exception), Times.Never);
        }

        [Test]
        public void Trace_ContextNullLoggerExceptionDetails_WriteExceptionDetailsCalledNever()
        {
            this.contextNullLogger.Object.Object.Trace(this.exception, this.details);
            this.contextNullLogger.Verify(x => x.Object.Write(LogLevel.Trace, this.exception, this.details), Times.Never);
        }

        [Test]
        public void Trace_ContextLoggerValidException_WriteExceptionCalledOnce()
        {
            this.contextLogger.Object.Trace(this.exception);
            this.contextLogger.Verify(x => x.Write(LogLevel.Trace, this.exception), Times.Once);
        }

        [Test]
        public void Trace_ContextLoggerValidExceptionDetails_WriteExceptionDetailsCalledOnce()
        {
            this.contextLogger.Object.Trace(this.exception, this.details);
            this.contextLogger.Verify(x => x.Write(LogLevel.Trace, this.exception, this.details), Times.Once);
        }

        [Test]
        public void Trace_ContextNullLoggerMessageException_WriteMessageExceptionCalledNever()
        {
            this.contextNullLogger.Object.Object.Trace(this.message, this.exception);
            this.contextNullLogger.Verify(x => x.Object.Write(LogLevel.Trace, this.message, this.exception), Times.Never);
        }

        [Test]
        public void Trace_ContextNullLoggerMessageExceptionDetails_WriteMessageExceptionDetailsCalledNever()
        {
            this.contextNullLogger.Object.Object.Trace(this.message, this.exception, this.details);
            this.contextNullLogger.Verify(x => x.Object.Write(LogLevel.Trace, this.message, this.exception, this.details), Times.Never);
        }

        [Test]
        public void Trace_ContextLoggerValidMessageException_WriteMessageExceptionCalledOnce()
        {
            this.contextLogger.Object.Trace(this.message, this.exception);
            this.contextLogger.Verify(x => x.Write(LogLevel.Trace, this.message, this.exception), Times.Once);
        }

        [Test]
        public void Trace_ContextLoggerValidMessageExceptionDetails_WriteMessageExceptionDetailsCalledOnce()
        {
            this.contextLogger.Object.Trace(this.message, this.exception, this.details);
            this.contextLogger.Verify(x => x.Write(LogLevel.Trace, this.message, this.exception, this.details), Times.Once);
        }

        [Test]
        public void Trace_ContextNullLoggerScopeMessage_WriteMessageCalledNever()
        {
            this.contextNullLogger.Object.Object.Trace(this.method, this.message);
            this.contextNullLogger.Verify(x => x.Object.Write(this.method, LogLevel.Trace, this.message), Times.Never);
        }

        [Test]
        public void Trace_ContextNullLoggerScopeMessageDetails_WriteMessageDetailsCalledNever()
        {
            this.contextNullLogger.Object.Object.Trace(this.method, this.message, this.details);
            this.contextNullLogger.Verify(x => x.Object.Write(this.method, LogLevel.Trace, this.message, this.details), Times.Never);
        }

        [Test]
        public void Trace_ContextLoggerScopeMessage_WriteMessageCalledOnce()
        {
            this.contextLogger.Object.Trace(this.method, this.message);
            this.contextLogger.Verify(x => x.Write(this.method, LogLevel.Trace, this.message), Times.Once);
        }

        [Test]
        public void Trace_ContextLoggerScopeMessageDetails_WriteMessageDetailsCalledOnce()
        {
            this.contextLogger.Object.Trace(this.method, this.message, this.details);
            this.contextLogger.Verify(x => x.Write(this.method, LogLevel.Trace, this.message, this.details), Times.Once);
        }

        [Test]
        public void Trace_ContextNullLoggerScopeException_WriteExceptionCalledNever()
        {
            this.contextNullLogger.Object.Object.Trace(this.method, this.exception);
            this.contextNullLogger.Verify(x => x.Object.Write(this.method, LogLevel.Trace, this.exception), Times.Never);
        }

        [Test]
        public void Trace_ContextNullLoggerScopeExceptionDetails_WriteExceptionDetailsCalledNever()
        {
            this.contextNullLogger.Object.Object.Trace(this.method, this.exception, this.details);
            this.contextNullLogger.Verify(x => x.Object.Write(this.method, LogLevel.Trace, this.exception, this.details), Times.Never);
        }

        [Test]
        public void Trace_ContextLoggerScopeException_WriteExceptionCalledOnce()
        {
            this.contextLogger.Object.Trace(this.method, this.exception);
            this.contextLogger.Verify(x => x.Write(this.method, LogLevel.Trace, this.exception), Times.Once);
        }

        [Test]
        public void Trace_ContextLoggerScopeExceptionDetails_WriteExceptionDetailsCalledOnce()
        {
            this.contextLogger.Object.Trace(this.method, this.exception, this.details);
            this.contextLogger.Verify(x => x.Write(this.method, LogLevel.Trace, this.exception, this.details), Times.Once);
        }

        [Test]
        public void Trace_ContextNullLoggerScopeMessageException_WriteMessageExceptionCalledNever()
        {
            this.contextNullLogger.Object.Object.Trace(this.method, this.message, this.exception);
            this.contextNullLogger.Verify(x => x.Object.Write(this.method, LogLevel.Trace, this.message, this.exception), Times.Never);
        }

        [Test]
        public void Trace_ContextNullLoggerScopeMessageExceptionDetails_WriteMessageExceptionDetailsCalledNever()
        {
            this.contextNullLogger.Object.Object.Trace(this.method, this.message, this.exception, this.details);
            this.contextNullLogger.Verify(x => x.Object.Write(this.method, LogLevel.Trace, this.message, this.exception, this.details), Times.Never);
        }

        [Test]
        public void Trace_ContextLoggerScopeMessageException_WriteMessageExceptionCalledOnce()
        {
            this.contextLogger.Object.Trace(this.method, this.message, this.exception);
            this.contextLogger.Verify(x => x.Write(this.method, LogLevel.Trace, this.message, this.exception), Times.Once);
        }

        [Test]
        public void Trace_ContextLoggerScopeMessageExceptionDetails_WriteMessageExceptionDetailsCalledOnce()
        {
            this.contextLogger.Object.Trace(this.method, this.message, this.exception, this.details);
            this.contextLogger.Verify(x => x.Write(this.method, LogLevel.Trace, this.message, this.exception, this.details), Times.Once);
        }

        #endregion

        #region Debug

        [Test]
        public void Debug_DefaultNullLoggerMessage_WriteMessageCalledNever()
        {
            this.defaultNullLogger.Object.Object.Debug(this.message);
            this.defaultNullLogger.Verify(x => x.Object.Write(LogLevel.Debug, this.message), Times.Never);
        }

        [Test]
        public void Debug_DefaultNullLoggerMessageDetails_WriteMessageDetailsCalledNever()
        {
            this.defaultNullLogger.Object.Object.Debug(this.message, this.details);
            this.defaultNullLogger.Verify(x => x.Object.Write(LogLevel.Debug, this.message, this.details), Times.Never);
        }

        [Test]
        public void Debug_DefaultLoggerMessage_WriteMessageCalledOnce()
        {
            this.defaultLogger.Object.Debug(this.message);
            this.defaultLogger.Verify(x => x.Write(LogLevel.Debug, this.message), Times.Once);
        }

        [Test]
        public void Debug_DefaultLoggerMessageDetails_WriteMessageDetailsCalledOnce()
        {
            this.defaultLogger.Object.Debug(this.message, this.details);
            this.defaultLogger.Verify(x => x.Write(LogLevel.Debug, this.message, this.details), Times.Once);
        }

        [Test]
        public void Debug_DefaultNullLoggerException_WriteExceptionCalledNever()
        {
            this.defaultNullLogger.Object.Object.Debug(this.exception);
            this.defaultNullLogger.Verify(x => x.Object.Write(LogLevel.Debug, this.exception), Times.Never);
        }

        [Test]
        public void Debug_DefaultNullLoggerExceptionDetails_WriteExceptionDetailsCalledNever()
        {
            this.defaultNullLogger.Object.Object.Debug(this.exception, this.details);
            this.defaultNullLogger.Verify(x => x.Object.Write(LogLevel.Debug, this.exception, this.details), Times.Never);
        }

        [Test]
        public void Debug_DefaultLoggerException_WriteExceptionCalledOnce()
        {
            this.defaultLogger.Object.Debug(this.exception);
            this.defaultLogger.Verify(x => x.Write(LogLevel.Debug, this.exception), Times.Once);
        }

        [Test]
        public void Debug_DefaultLoggerExceptionDetails_WriteExceptionDetailsCalledOnce()
        {
            this.defaultLogger.Object.Debug(this.exception, this.details);
            this.defaultLogger.Verify(x => x.Write(LogLevel.Debug, this.exception, this.details), Times.Once);
        }

        [Test]
        public void Debug_DefaultNullLoggerMessageException_WriteMessageExceptionCalledNever()
        {
            this.defaultNullLogger.Object.Object.Debug(this.message, this.exception);
            this.defaultNullLogger.Verify(x => x.Object.Write(LogLevel.Debug, this.message, this.exception), Times.Never);
        }

        [Test]
        public void Debug_DefaultNullLoggerMessageExceptionDetails_WriteMessageExceptionDetailsCalledNever()
        {
            this.defaultNullLogger.Object.Object.Debug(this.message, this.exception, this.details);
            this.defaultNullLogger.Verify(x => x.Object.Write(LogLevel.Debug, this.message, this.exception, this.details), Times.Never);
        }

        [Test]
        public void Debug_DefaultLoggerMessageException_WriteMessageExceptionCalledOnce()
        {
            this.defaultLogger.Object.Debug(this.message, this.exception);
            this.defaultLogger.Verify(x => x.Write(LogLevel.Debug, this.message, this.exception), Times.Once);
        }

        [Test]
        public void Debug_DefaultLoggerMessageExceptionDetails_WriteMessageExceptionDetailsCalledOnce()
        {
            this.defaultLogger.Object.Debug(this.message, this.exception, this.details);
            this.defaultLogger.Verify(x => x.Write(LogLevel.Debug, this.message, this.exception, this.details), Times.Once);
        }

        [Test]
        public void Debug_DefaultNullLoggerScopeMessage_WriteMessageCalledNever()
        {
            this.defaultNullLogger.Object.Object.Debug(this.method, this.message);
            this.defaultNullLogger.Verify(x => x.Object.Write(this.method, LogLevel.Debug, this.message), Times.Never);
        }

        [Test]
        public void Debug_DefaultNullLoggerScopeMessageDetails_WriteMessageDetailsCalledNever()
        {
            this.defaultNullLogger.Object.Object.Debug(this.method, this.message, this.details);
            this.defaultNullLogger.Verify(x => x.Object.Write(this.method, LogLevel.Debug, this.message, this.details), Times.Never);
        }

        [Test]
        public void Debug_DefaultLoggerScopeMessage_WriteMessageCalledOnce()
        {
            this.defaultLogger.Object.Debug(this.method, this.message);
            this.defaultLogger.Verify(x => x.Write(this.method, LogLevel.Debug, this.message), Times.Once);
        }

        [Test]
        public void Debug_DefaultLoggerScopeMessageDetails_WriteMessageDetailsCalledOnce()
        {
            this.defaultLogger.Object.Debug(this.method, this.message, this.details);
            this.defaultLogger.Verify(x => x.Write(this.method, LogLevel.Debug, this.message, this.details), Times.Once);
        }

        [Test]
        public void Debug_DefaultNullLoggerScopeException_WriteExceptionCalledNever()
        {
            this.defaultNullLogger.Object.Object.Debug(this.method, this.exception);
            this.defaultNullLogger.Verify(x => x.Object.Write(this.method, LogLevel.Debug, this.exception), Times.Never);
        }

        [Test]
        public void Debug_DefaultNullLoggerScopeExceptionDetails_WriteExceptionDetailsCalledNever()
        {
            this.defaultNullLogger.Object.Object.Debug(this.method, this.exception, this.details);
            this.defaultNullLogger.Verify(x => x.Object.Write(this.method, LogLevel.Debug, this.exception, this.details), Times.Never);
        }

        [Test]
        public void Debug_DefaultLoggerScopeException_WriteExceptionCalledOnce()
        {
            this.defaultLogger.Object.Debug(this.method, this.exception);
            this.defaultLogger.Verify(x => x.Write(this.method, LogLevel.Debug, this.exception), Times.Once);
        }

        [Test]
        public void Debug_DefaultLoggerScopeExceptionDetails_WriteExceptionDetailsCalledOnce()
        {
            this.defaultLogger.Object.Debug(this.method, this.exception, this.details);
            this.defaultLogger.Verify(x => x.Write(this.method, LogLevel.Debug, this.exception, this.details), Times.Once);
        }

        [Test]
        public void Debug_DefaultNullLoggerScopeMessageException_WriteMessageExceptionCalledNever()
        {
            this.defaultNullLogger.Object.Object.Debug(this.method, this.message, this.exception);
            this.defaultNullLogger.Verify(x => x.Object.Write(this.method, LogLevel.Debug, this.message, this.exception), Times.Never);
        }

        [Test]
        public void Debug_DefaultNullLoggerScopeMessageExceptionDetails_WriteMessageExceptionDetailsCalledNever()
        {
            this.defaultNullLogger.Object.Object.Debug(this.method, this.message, this.exception, this.details);
            this.defaultNullLogger.Verify(x => x.Object.Write(this.method, LogLevel.Debug, this.message, this.exception, this.details), Times.Never);
        }

        [Test]
        public void Debug_DefaultLoggerScopeMessageException_WriteMessageExceptionCalledOnce()
        {
            this.defaultLogger.Object.Debug(this.method, this.message, this.exception);
            this.defaultLogger.Verify(x => x.Write(this.method, LogLevel.Debug, this.message, this.exception), Times.Once);
        }

        [Test]
        public void Debug_DefaultLoggerScopeMessageExceptionDetails_WriteMessageExceptionDetailsCalledOnce()
        {
            this.defaultLogger.Object.Debug(this.method, this.message, this.exception, this.details);
            this.defaultLogger.Verify(x => x.Write(this.method, LogLevel.Debug, this.message, this.exception, this.details), Times.Once);
        }

        [Test]
        public void Debug_ContextNullLoggerMessage_WriteMessageCalledNever()
        {
            this.contextNullLogger.Object.Object.Debug(this.message);
            this.contextNullLogger.Verify(x => x.Object.Write(LogLevel.Debug, this.message), Times.Never);
        }

        [Test]
        public void Debug_ContextNullLoggerMessageDetails_WriteMessageDetailsCalledNever()
        {
            this.contextNullLogger.Object.Object.Debug(this.message, this.details);
            this.contextNullLogger.Verify(x => x.Object.Write(LogLevel.Debug, this.message, this.details), Times.Never);
        }

        [Test]
        public void Debug_ContextLoggerValidMessage_WriteMessageCalledOnce()
        {
            this.contextLogger.Object.Debug(this.message);
            this.contextLogger.Verify(x => x.Write(LogLevel.Debug, this.message), Times.Once);
        }

        [Test]
        public void Debug_ContextLoggerValidMessageDetails_WriteMessageDetailsCalledOnce()
        {
            this.contextLogger.Object.Debug(this.message, this.details);
            this.contextLogger.Verify(x => x.Write(LogLevel.Debug, this.message, this.details), Times.Once);
        }

        [Test]
        public void Debug_ContextNullLoggerException_WriteExceptionCalledNever()
        {
            this.contextNullLogger.Object.Object.Debug(this.exception);
            this.contextNullLogger.Verify(x => x.Object.Write(LogLevel.Debug, this.exception), Times.Never);
        }

        [Test]
        public void Debug_ContextNullLoggerExceptionDetails_WriteExceptionDetailsCalledNever()
        {
            this.contextNullLogger.Object.Object.Debug(this.exception, this.details);
            this.contextNullLogger.Verify(x => x.Object.Write(LogLevel.Debug, this.exception, this.details), Times.Never);
        }

        [Test]
        public void Debug_ContextLoggerValidException_WriteExceptionCalledOnce()
        {
            this.contextLogger.Object.Debug(this.exception);
            this.contextLogger.Verify(x => x.Write(LogLevel.Debug, this.exception), Times.Once);
        }

        [Test]
        public void Debug_ContextLoggerValidExceptionDetails_WriteExceptionDetailsCalledOnce()
        {
            this.contextLogger.Object.Debug(this.exception, this.details);
            this.contextLogger.Verify(x => x.Write(LogLevel.Debug, this.exception, this.details), Times.Once);
        }

        [Test]
        public void Debug_ContextNullLoggerMessageException_WriteMessageExceptionCalledNever()
        {
            this.contextNullLogger.Object.Object.Debug(this.message, this.exception);
            this.contextNullLogger.Verify(x => x.Object.Write(LogLevel.Debug, this.message, this.exception), Times.Never);
        }

        [Test]
        public void Debug_ContextNullLoggerMessageExceptionDetails_WriteMessageExceptionDetailsCalledNever()
        {
            this.contextNullLogger.Object.Object.Debug(this.message, this.exception, this.details);
            this.contextNullLogger.Verify(x => x.Object.Write(LogLevel.Debug, this.message, this.exception, this.details), Times.Never);
        }

        [Test]
        public void Debug_ContextLoggerValidMessageException_WriteMessageExceptionCalledOnce()
        {
            this.contextLogger.Object.Debug(this.message, this.exception);
            this.contextLogger.Verify(x => x.Write(LogLevel.Debug, this.message, this.exception), Times.Once);
        }

        [Test]
        public void Debug_ContextLoggerValidMessageExceptionDetails_WriteMessageExceptionDetailsCalledOnce()
        {
            this.contextLogger.Object.Debug(this.message, this.exception, this.details);
            this.contextLogger.Verify(x => x.Write(LogLevel.Debug, this.message, this.exception, this.details), Times.Once);
        }

        [Test]
        public void Debug_ContextNullLoggerScopeMessage_WriteMessageCalledNever()
        {
            this.contextNullLogger.Object.Object.Debug(this.method, this.message);
            this.contextNullLogger.Verify(x => x.Object.Write(this.method, LogLevel.Debug, this.message), Times.Never);
        }

        [Test]
        public void Debug_ContextNullLoggerScopeMessageDetails_WriteMessageDetailsCalledNever()
        {
            this.contextNullLogger.Object.Object.Debug(this.method, this.message, this.details);
            this.contextNullLogger.Verify(x => x.Object.Write(this.method, LogLevel.Debug, this.message, this.details), Times.Never);
        }

        [Test]
        public void Debug_ContextLoggerScopeMessage_WriteMessageCalledOnce()
        {
            this.contextLogger.Object.Debug(this.method, this.message);
            this.contextLogger.Verify(x => x.Write(this.method, LogLevel.Debug, this.message), Times.Once);
        }

        [Test]
        public void Debug_ContextLoggerScopeMessageDetails_WriteMessageDetailsCalledOnce()
        {
            this.contextLogger.Object.Debug(this.method, this.message, this.details);
            this.contextLogger.Verify(x => x.Write(this.method, LogLevel.Debug, this.message, this.details), Times.Once);
        }

        [Test]
        public void Debug_ContextNullLoggerScopeException_WriteExceptionCalledNever()
        {
            this.contextNullLogger.Object.Object.Debug(this.method, this.exception);
            this.contextNullLogger.Verify(x => x.Object.Write(this.method, LogLevel.Debug, this.exception), Times.Never);
        }

        [Test]
        public void Debug_ContextNullLoggerScopeExceptionDetails_WriteExceptionDetailsCalledNever()
        {
            this.contextNullLogger.Object.Object.Debug(this.method, this.exception, this.details);
            this.contextNullLogger.Verify(x => x.Object.Write(this.method, LogLevel.Debug, this.exception, this.details), Times.Never);
        }

        [Test]
        public void Debug_ContextLoggerScopeException_WriteExceptionCalledOnce()
        {
            this.contextLogger.Object.Debug(this.method, this.exception);
            this.contextLogger.Verify(x => x.Write(this.method, LogLevel.Debug, this.exception), Times.Once);
        }

        [Test]
        public void Debug_ContextLoggerScopeExceptionDetails_WriteExceptionDetailsCalledOnce()
        {
            this.contextLogger.Object.Debug(this.method, this.exception, this.details);
            this.contextLogger.Verify(x => x.Write(this.method, LogLevel.Debug, this.exception, this.details), Times.Once);
        }

        [Test]
        public void Debug_ContextNullLoggerScopeMessageException_WriteMessageExceptionCalledNever()
        {
            this.contextNullLogger.Object.Object.Debug(this.method, this.message, this.exception);
            this.contextNullLogger.Verify(x => x.Object.Write(this.method, LogLevel.Debug, this.message, this.exception), Times.Never);
        }

        [Test]
        public void Debug_ContextNullLoggerScopeMessageExceptionDetails_WriteMessageExceptionDetailsCalledNever()
        {
            this.contextNullLogger.Object.Object.Debug(this.method, this.message, this.exception, this.details);
            this.contextNullLogger.Verify(x => x.Object.Write(this.method, LogLevel.Debug, this.message, this.exception, this.details), Times.Never);
        }

        [Test]
        public void Debug_ContextLoggerScopeMessageException_WriteMessageExceptionCalledOnce()
        {
            this.contextLogger.Object.Debug(this.method, this.message, this.exception);
            this.contextLogger.Verify(x => x.Write(this.method, LogLevel.Debug, this.message, this.exception), Times.Once);
        }

        [Test]
        public void Debug_ContextLoggerScopeMessageExceptionDetails_WriteMessageExceptionDetailsCalledOnce()
        {
            this.contextLogger.Object.Debug(this.method, this.message, this.exception, this.details);
            this.contextLogger.Verify(x => x.Write(this.method, LogLevel.Debug, this.message, this.exception, this.details), Times.Once);
        }

        #endregion

        #region Verbose

        [Test]
        public void Verbose_DefaultNullLoggerMessage_WriteMessageCalledNever()
        {
            this.defaultNullLogger.Object.Object.Verbose(this.message);
            this.defaultNullLogger.Verify(x => x.Object.Write(LogLevel.Verbose, this.message), Times.Never);
        }

        [Test]
        public void Verbose_DefaultNullLoggerMessageDetails_WriteMessageDetailsCalledNever()
        {
            this.defaultNullLogger.Object.Object.Verbose(this.message, this.details);
            this.defaultNullLogger.Verify(x => x.Object.Write(LogLevel.Verbose, this.message, this.details), Times.Never);
        }

        [Test]
        public void Verbose_DefaultLoggerMessage_WriteMessageCalledOnce()
        {
            this.defaultLogger.Object.Verbose(this.message);
            this.defaultLogger.Verify(x => x.Write(LogLevel.Verbose, this.message), Times.Once);
        }

        [Test]
        public void Verbose_DefaultLoggerMessageDetails_WriteMessageDetailsCalledOnce()
        {
            this.defaultLogger.Object.Verbose(this.message, this.details);
            this.defaultLogger.Verify(x => x.Write(LogLevel.Verbose, this.message, this.details), Times.Once);
        }

        [Test]
        public void Verbose_DefaultNullLoggerException_WriteExceptionCalledNever()
        {
            this.defaultNullLogger.Object.Object.Verbose(this.exception);
            this.defaultNullLogger.Verify(x => x.Object.Write(LogLevel.Verbose, this.exception), Times.Never);
        }

        [Test]
        public void Verbose_DefaultNullLoggerExceptionDetails_WriteExceptionDetailsCalledNever()
        {
            this.defaultNullLogger.Object.Object.Verbose(this.exception, this.details);
            this.defaultNullLogger.Verify(x => x.Object.Write(LogLevel.Verbose, this.exception, this.details), Times.Never);
        }

        [Test]
        public void Verbose_DefaultLoggerException_WriteExceptionCalledOnce()
        {
            this.defaultLogger.Object.Verbose(this.exception);
            this.defaultLogger.Verify(x => x.Write(LogLevel.Verbose, this.exception), Times.Once);
        }

        [Test]
        public void Verbose_DefaultLoggerExceptionDetails_WriteExceptionDetailsCalledOnce()
        {
            this.defaultLogger.Object.Verbose(this.exception, this.details);
            this.defaultLogger.Verify(x => x.Write(LogLevel.Verbose, this.exception, this.details), Times.Once);
        }

        [Test]
        public void Verbose_DefaultNullLoggerMessageException_WriteMessageExceptionCalledNever()
        {
            this.defaultNullLogger.Object.Object.Verbose(this.message, this.exception);
            this.defaultNullLogger.Verify(x => x.Object.Write(LogLevel.Verbose, this.message, this.exception), Times.Never);
        }

        [Test]
        public void Verbose_DefaultNullLoggerMessageExceptionDetails_WriteMessageExceptionDetailsCalledNever()
        {
            this.defaultNullLogger.Object.Object.Verbose(this.message, this.exception, this.details);
            this.defaultNullLogger.Verify(x => x.Object.Write(LogLevel.Verbose, this.message, this.exception, this.details), Times.Never);
        }

        [Test]
        public void Verbose_DefaultLoggerMessageException_WriteMessageExceptionCalledOnce()
        {
            this.defaultLogger.Object.Verbose(this.message, this.exception);
            this.defaultLogger.Verify(x => x.Write(LogLevel.Verbose, this.message, this.exception), Times.Once);
        }

        [Test]
        public void Verbose_DefaultLoggerMessageExceptionDetails_WriteMessageExceptionDetailsCalledOnce()
        {
            this.defaultLogger.Object.Verbose(this.message, this.exception, this.details);
            this.defaultLogger.Verify(x => x.Write(LogLevel.Verbose, this.message, this.exception, this.details), Times.Once);
        }

        [Test]
        public void Verbose_DefaultNullLoggerScopeMessage_WriteMessageCalledNever()
        {
            this.defaultNullLogger.Object.Object.Verbose(this.method, this.message);
            this.defaultNullLogger.Verify(x => x.Object.Write(this.method, LogLevel.Verbose, this.message), Times.Never);
        }

        [Test]
        public void Verbose_DefaultNullLoggerScopeMessageDetails_WriteMessageDetailsCalledNever()
        {
            this.defaultNullLogger.Object.Object.Verbose(this.method, this.message, this.details);
            this.defaultNullLogger.Verify(x => x.Object.Write(this.method, LogLevel.Verbose, this.message, this.details), Times.Never);
        }

        [Test]
        public void Verbose_DefaultLoggerScopeMessage_WriteMessageCalledOnce()
        {
            this.defaultLogger.Object.Verbose(this.method, this.message);
            this.defaultLogger.Verify(x => x.Write(this.method, LogLevel.Verbose, this.message), Times.Once);
        }

        [Test]
        public void Verbose_DefaultLoggerScopeMessageDetails_WriteMessageDetailsCalledOnce()
        {
            this.defaultLogger.Object.Verbose(this.method, this.message, this.details);
            this.defaultLogger.Verify(x => x.Write(this.method, LogLevel.Verbose, this.message, this.details), Times.Once);
        }

        [Test]
        public void Verbose_DefaultNullLoggerScopeException_WriteExceptionCalledNever()
        {
            this.defaultNullLogger.Object.Object.Verbose(this.method, this.exception);
            this.defaultNullLogger.Verify(x => x.Object.Write(this.method, LogLevel.Verbose, this.exception), Times.Never);
        }

        [Test]
        public void Verbose_DefaultNullLoggerScopeExceptionDetails_WriteExceptionDetailsCalledNever()
        {
            this.defaultNullLogger.Object.Object.Verbose(this.method, this.exception, this.details);
            this.defaultNullLogger.Verify(x => x.Object.Write(this.method, LogLevel.Verbose, this.exception, this.details), Times.Never);
        }

        [Test]
        public void Verbose_DefaultLoggerScopeException_WriteExceptionCalledOnce()
        {
            this.defaultLogger.Object.Verbose(this.method, this.exception);
            this.defaultLogger.Verify(x => x.Write(this.method, LogLevel.Verbose, this.exception), Times.Once);
        }

        [Test]
        public void Verbose_DefaultLoggerScopeExceptionDetails_WriteExceptionDetailsCalledOnce()
        {
            this.defaultLogger.Object.Verbose(this.method, this.exception, this.details);
            this.defaultLogger.Verify(x => x.Write(this.method, LogLevel.Verbose, this.exception, this.details), Times.Once);
        }

        [Test]
        public void Verbose_DefaultNullLoggerScopeMessageException_WriteMessageExceptionCalledNever()
        {
            this.defaultNullLogger.Object.Object.Verbose(this.method, this.message, this.exception);
            this.defaultNullLogger.Verify(x => x.Object.Write(this.method, LogLevel.Verbose, this.message, this.exception), Times.Never);
        }

        [Test]
        public void Verbose_DefaultNullLoggerScopeMessageExceptionDetails_WriteMessageExceptionDetailsCalledNever()
        {
            this.defaultNullLogger.Object.Object.Verbose(this.method, this.message, this.exception, this.details);
            this.defaultNullLogger.Verify(x => x.Object.Write(this.method, LogLevel.Verbose, this.message, this.exception, this.details), Times.Never);
        }

        [Test]
        public void Verbose_DefaultLoggerScopeMessageException_WriteMessageExceptionCalledOnce()
        {
            this.defaultLogger.Object.Verbose(this.method, this.message, this.exception);
            this.defaultLogger.Verify(x => x.Write(this.method, LogLevel.Verbose, this.message, this.exception), Times.Once);
        }

        [Test]
        public void Verbose_DefaultLoggerScopeMessageExceptionDetails_WriteMessageExceptionDetailsCalledOnce()
        {
            this.defaultLogger.Object.Verbose(this.method, this.message, this.exception, this.details);
            this.defaultLogger.Verify(x => x.Write(this.method, LogLevel.Verbose, this.message, this.exception, this.details), Times.Once);
        }

        [Test]
        public void Verbose_ContextNullLoggerMessage_WriteMessageCalledNever()
        {
            this.contextNullLogger.Object.Object.Verbose(this.message);
            this.contextNullLogger.Verify(x => x.Object.Write(LogLevel.Verbose, this.message), Times.Never);
        }

        [Test]
        public void Verbose_ContextNullLoggerMessageDetails_WriteMessageDetailsCalledNever()
        {
            this.contextNullLogger.Object.Object.Verbose(this.message, this.details);
            this.contextNullLogger.Verify(x => x.Object.Write(LogLevel.Verbose, this.message, this.details), Times.Never);
        }

        [Test]
        public void Verbose_ContextLoggerValidMessage_WriteMessageCalledOnce()
        {
            this.contextLogger.Object.Verbose(this.message);
            this.contextLogger.Verify(x => x.Write(LogLevel.Verbose, this.message), Times.Once);
        }

        [Test]
        public void Verbose_ContextLoggerValidMessageDetails_WriteMessageDetailsCalledOnce()
        {
            this.contextLogger.Object.Verbose(this.message, this.details);
            this.contextLogger.Verify(x => x.Write(LogLevel.Verbose, this.message, this.details), Times.Once);
        }

        [Test]
        public void Verbose_ContextNullLoggerException_WriteExceptionCalledNever()
        {
            this.contextNullLogger.Object.Object.Verbose(this.exception);
            this.contextNullLogger.Verify(x => x.Object.Write(LogLevel.Verbose, this.exception), Times.Never);
        }

        [Test]
        public void Verbose_ContextNullLoggerExceptionDetails_WriteExceptionDetailsCalledNever()
        {
            this.contextNullLogger.Object.Object.Verbose(this.exception, this.details);
            this.contextNullLogger.Verify(x => x.Object.Write(LogLevel.Verbose, this.exception, this.details), Times.Never);
        }

        [Test]
        public void Verbose_ContextLoggerValidException_WriteExceptionCalledOnce()
        {
            this.contextLogger.Object.Verbose(this.exception);
            this.contextLogger.Verify(x => x.Write(LogLevel.Verbose, this.exception), Times.Once);
        }

        [Test]
        public void Verbose_ContextLoggerValidExceptionDetails_WriteExceptionDetailsCalledOnce()
        {
            this.contextLogger.Object.Verbose(this.exception, this.details);
            this.contextLogger.Verify(x => x.Write(LogLevel.Verbose, this.exception, this.details), Times.Once);
        }

        [Test]
        public void Verbose_ContextNullLoggerMessageException_WriteMessageExceptionCalledNever()
        {
            this.contextNullLogger.Object.Object.Verbose(this.message, this.exception);
            this.contextNullLogger.Verify(x => x.Object.Write(LogLevel.Verbose, this.message, this.exception), Times.Never);
        }

        [Test]
        public void Verbose_ContextNullLoggerMessageExceptionDetails_WriteMessageExceptionDetailsCalledNever()
        {
            this.contextNullLogger.Object.Object.Verbose(this.message, this.exception, this.details);
            this.contextNullLogger.Verify(x => x.Object.Write(LogLevel.Verbose, this.message, this.exception, this.details), Times.Never);
        }

        [Test]
        public void Verbose_ContextLoggerValidMessageException_WriteMessageExceptionCalledOnce()
        {
            this.contextLogger.Object.Verbose(this.message, this.exception);
            this.contextLogger.Verify(x => x.Write(LogLevel.Verbose, this.message, this.exception), Times.Once);
        }

        [Test]
        public void Verbose_ContextLoggerValidMessageExceptionDetails_WriteMessageExceptionDetailsCalledOnce()
        {
            this.contextLogger.Object.Verbose(this.message, this.exception, this.details);
            this.contextLogger.Verify(x => x.Write(LogLevel.Verbose, this.message, this.exception, this.details), Times.Once);
        }

        [Test]
        public void Verbose_ContextNullLoggerScopeMessage_WriteMessageCalledNever()
        {
            this.contextNullLogger.Object.Object.Verbose(this.method, this.message);
            this.contextNullLogger.Verify(x => x.Object.Write(this.method, LogLevel.Verbose, this.message), Times.Never);
        }

        [Test]
        public void Verbose_ContextNullLoggerScopeMessageDetails_WriteMessageDetailsCalledNever()
        {
            this.contextNullLogger.Object.Object.Verbose(this.method, this.message, this.details);
            this.contextNullLogger.Verify(x => x.Object.Write(this.method, LogLevel.Verbose, this.message, this.details), Times.Never);
        }

        [Test]
        public void Verbose_ContextLoggerScopeMessage_WriteMessageCalledOnce()
        {
            this.contextLogger.Object.Verbose(this.method, this.message);
            this.contextLogger.Verify(x => x.Write(this.method, LogLevel.Verbose, this.message), Times.Once);
        }

        [Test]
        public void Verbose_ContextLoggerScopeMessageDetails_WriteMessageDetailsCalledOnce()
        {
            this.contextLogger.Object.Verbose(this.method, this.message, this.details);
            this.contextLogger.Verify(x => x.Write(this.method, LogLevel.Verbose, this.message, this.details), Times.Once);
        }

        [Test]
        public void Verbose_ContextNullLoggerScopeException_WriteExceptionCalledNever()
        {
            this.contextNullLogger.Object.Object.Verbose(this.method, this.exception);
            this.contextNullLogger.Verify(x => x.Object.Write(this.method, LogLevel.Verbose, this.exception), Times.Never);
        }

        [Test]
        public void Verbose_ContextNullLoggerScopeExceptionDetails_WriteExceptionDetailsCalledNever()
        {
            this.contextNullLogger.Object.Object.Verbose(this.method, this.exception, this.details);
            this.contextNullLogger.Verify(x => x.Object.Write(this.method, LogLevel.Verbose, this.exception, this.details), Times.Never);
        }

        [Test]
        public void Verbose_ContextLoggerScopeException_WriteExceptionCalledOnce()
        {
            this.contextLogger.Object.Verbose(this.method, this.exception);
            this.contextLogger.Verify(x => x.Write(this.method, LogLevel.Verbose, this.exception), Times.Once);
        }

        [Test]
        public void Verbose_ContextLoggerScopeExceptionDetails_WriteExceptionDetailsCalledOnce()
        {
            this.contextLogger.Object.Verbose(this.method, this.exception, this.details);
            this.contextLogger.Verify(x => x.Write(this.method, LogLevel.Verbose, this.exception, this.details), Times.Once);
        }

        [Test]
        public void Verbose_ContextNullLoggerScopeMessageException_WriteMessageExceptionCalledNever()
        {
            this.contextNullLogger.Object.Object.Verbose(this.method, this.message, this.exception);
            this.contextNullLogger.Verify(x => x.Object.Write(this.method, LogLevel.Verbose, this.message, this.exception), Times.Never);
        }

        [Test]
        public void Verbose_ContextNullLoggerScopeMessageExceptionDetails_WriteMessageExceptionDetailsCalledNever()
        {
            this.contextNullLogger.Object.Object.Verbose(this.method, this.message, this.exception, this.details);
            this.contextNullLogger.Verify(x => x.Object.Write(this.method, LogLevel.Verbose, this.message, this.exception, this.details), Times.Never);
        }

        [Test]
        public void Verbose_ContextLoggerScopeMessageException_WriteMessageExceptionCalledOnce()
        {
            this.contextLogger.Object.Verbose(this.method, this.message, this.exception);
            this.contextLogger.Verify(x => x.Write(this.method, LogLevel.Verbose, this.message, this.exception), Times.Once);
        }

        [Test]
        public void Verbose_ContextLoggerScopeMessageExceptionDetails_WriteMessageExceptionDetailsCalledOnce()
        {
            this.contextLogger.Object.Verbose(this.method, this.message, this.exception, this.details);
            this.contextLogger.Verify(x => x.Write(this.method, LogLevel.Verbose, this.message, this.exception, this.details), Times.Once);
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

        #region Warning

        [Test]
        public void Warning_DefaultNullLoggerMessage_WriteMessageCalledNever()
        {
            this.defaultNullLogger.Object.Object.Warning(this.message);
            this.defaultNullLogger.Verify(x => x.Object.Write(LogLevel.Warning, this.message), Times.Never);
        }

        [Test]
        public void Warning_DefaultNullLoggerMessageDetails_WriteMessageDetailsCalledNever()
        {
            this.defaultNullLogger.Object.Object.Warning(this.message, this.details);
            this.defaultNullLogger.Verify(x => x.Object.Write(LogLevel.Warning, this.message, this.details), Times.Never);
        }

        [Test]
        public void Warning_DefaultLoggerMessage_WriteMessageCalledOnce()
        {
            this.defaultLogger.Object.Warning(this.message);
            this.defaultLogger.Verify(x => x.Write(LogLevel.Warning, this.message), Times.Once);
        }

        [Test]
        public void Warning_DefaultLoggerMessageDetails_WriteMessageDetailsCalledOnce()
        {
            this.defaultLogger.Object.Warning(this.message, this.details);
            this.defaultLogger.Verify(x => x.Write(LogLevel.Warning, this.message, this.details), Times.Once);
        }

        [Test]
        public void Warning_DefaultNullLoggerException_WriteExceptionCalledNever()
        {
            this.defaultNullLogger.Object.Object.Warning(this.exception);
            this.defaultNullLogger.Verify(x => x.Object.Write(LogLevel.Warning, this.exception), Times.Never);
        }

        [Test]
        public void Warning_DefaultNullLoggerExceptionDetails_WriteExceptionDetailsCalledNever()
        {
            this.defaultNullLogger.Object.Object.Warning(this.exception, this.details);
            this.defaultNullLogger.Verify(x => x.Object.Write(LogLevel.Warning, this.exception, this.details), Times.Never);
        }

        [Test]
        public void Warning_DefaultLoggerException_WriteExceptionCalledOnce()
        {
            this.defaultLogger.Object.Warning(this.exception);
            this.defaultLogger.Verify(x => x.Write(LogLevel.Warning, this.exception), Times.Once);
        }

        [Test]
        public void Warning_DefaultLoggerExceptionDetails_WriteExceptionDetailsCalledOnce()
        {
            this.defaultLogger.Object.Warning(this.exception, this.details);
            this.defaultLogger.Verify(x => x.Write(LogLevel.Warning, this.exception, this.details), Times.Once);
        }

        [Test]
        public void Warning_DefaultNullLoggerMessageException_WriteMessageExceptionCalledNever()
        {
            this.defaultNullLogger.Object.Object.Warning(this.message, this.exception);
            this.defaultNullLogger.Verify(x => x.Object.Write(LogLevel.Warning, this.message, this.exception), Times.Never);
        }

        [Test]
        public void Warning_DefaultNullLoggerMessageExceptionDetails_WriteMessageExceptionDetailsCalledNever()
        {
            this.defaultNullLogger.Object.Object.Warning(this.message, this.exception, this.details);
            this.defaultNullLogger.Verify(x => x.Object.Write(LogLevel.Warning, this.message, this.exception, this.details), Times.Never);
        }

        [Test]
        public void Warning_DefaultLoggerMessageException_WriteMessageExceptionCalledOnce()
        {
            this.defaultLogger.Object.Warning(this.message, this.exception);
            this.defaultLogger.Verify(x => x.Write(LogLevel.Warning, this.message, this.exception), Times.Once);
        }

        [Test]
        public void Warning_DefaultLoggerMessageExceptionDetails_WriteMessageExceptionDetailsCalledOnce()
        {
            this.defaultLogger.Object.Warning(this.message, this.exception, this.details);
            this.defaultLogger.Verify(x => x.Write(LogLevel.Warning, this.message, this.exception, this.details), Times.Once);
        }

        [Test]
        public void Warning_DefaultNullLoggerScopeMessage_WriteMessageCalledNever()
        {
            this.defaultNullLogger.Object.Object.Warning(this.method, this.message);
            this.defaultNullLogger.Verify(x => x.Object.Write(this.method, LogLevel.Warning, this.message), Times.Never);
        }

        [Test]
        public void Warning_DefaultNullLoggerScopeMessageDetails_WriteMessageDetailsCalledNever()
        {
            this.defaultNullLogger.Object.Object.Warning(this.method, this.message, this.details);
            this.defaultNullLogger.Verify(x => x.Object.Write(this.method, LogLevel.Warning, this.message, this.details), Times.Never);
        }

        [Test]
        public void Warning_DefaultLoggerScopeMessage_WriteMessageCalledOnce()
        {
            this.defaultLogger.Object.Warning(this.method, this.message);
            this.defaultLogger.Verify(x => x.Write(this.method, LogLevel.Warning, this.message), Times.Once);
        }

        [Test]
        public void Warning_DefaultLoggerScopeMessageDetails_WriteMessageDetailsCalledOnce()
        {
            this.defaultLogger.Object.Warning(this.method, this.message, this.details);
            this.defaultLogger.Verify(x => x.Write(this.method, LogLevel.Warning, this.message, this.details), Times.Once);
        }

        [Test]
        public void Warning_DefaultNullLoggerScopeException_WriteExceptionCalledNever()
        {
            this.defaultNullLogger.Object.Object.Warning(this.method, this.exception);
            this.defaultNullLogger.Verify(x => x.Object.Write(this.method, LogLevel.Warning, this.exception), Times.Never);
        }

        [Test]
        public void Warning_DefaultNullLoggerScopeExceptionDetails_WriteExceptionDetailsCalledNever()
        {
            this.defaultNullLogger.Object.Object.Warning(this.method, this.exception, this.details);
            this.defaultNullLogger.Verify(x => x.Object.Write(this.method, LogLevel.Warning, this.exception, this.details), Times.Never);
        }

        [Test]
        public void Warning_DefaultLoggerScopeException_WriteExceptionCalledOnce()
        {
            this.defaultLogger.Object.Warning(this.method, this.exception);
            this.defaultLogger.Verify(x => x.Write(this.method, LogLevel.Warning, this.exception), Times.Once);
        }

        [Test]
        public void Warning_DefaultLoggerScopeExceptionDetails_WriteExceptionDetailsCalledOnce()
        {
            this.defaultLogger.Object.Warning(this.method, this.exception, this.details);
            this.defaultLogger.Verify(x => x.Write(this.method, LogLevel.Warning, this.exception, this.details), Times.Once);
        }

        [Test]
        public void Warning_DefaultNullLoggerScopeMessageException_WriteMessageExceptionCalledNever()
        {
            this.defaultNullLogger.Object.Object.Warning(this.method, this.message, this.exception);
            this.defaultNullLogger.Verify(x => x.Object.Write(this.method, LogLevel.Warning, this.message, this.exception), Times.Never);
        }

        [Test]
        public void Warning_DefaultNullLoggerScopeMessageExceptionDetails_WriteMessageExceptionDetailsCalledNever()
        {
            this.defaultNullLogger.Object.Object.Warning(this.method, this.message, this.exception, this.details);
            this.defaultNullLogger.Verify(x => x.Object.Write(this.method, LogLevel.Warning, this.message, this.exception, this.details), Times.Never);
        }

        [Test]
        public void Warning_DefaultLoggerScopeMessageException_WriteMessageExceptionCalledOnce()
        {
            this.defaultLogger.Object.Warning(this.method, this.message, this.exception);
            this.defaultLogger.Verify(x => x.Write(this.method, LogLevel.Warning, this.message, this.exception), Times.Once);
        }

        [Test]
        public void Warning_DefaultLoggerScopeMessageExceptionDetails_WriteMessageExceptionDetailsCalledOnce()
        {
            this.defaultLogger.Object.Warning(this.method, this.message, this.exception, this.details);
            this.defaultLogger.Verify(x => x.Write(this.method, LogLevel.Warning, this.message, this.exception, this.details), Times.Once);
        }

        [Test]
        public void Warning_ContextNullLoggerMessage_WriteMessageCalledNever()
        {
            this.contextNullLogger.Object.Object.Warning(this.message);
            this.contextNullLogger.Verify(x => x.Object.Write(LogLevel.Warning, this.message), Times.Never);
        }

        [Test]
        public void Warning_ContextNullLoggerMessageDetails_WriteMessageDetailsCalledNever()
        {
            this.contextNullLogger.Object.Object.Warning(this.message, this.details);
            this.contextNullLogger.Verify(x => x.Object.Write(LogLevel.Warning, this.message, this.details), Times.Never);
        }

        [Test]
        public void Warning_ContextLoggerValidMessage_WriteMessageCalledOnce()
        {
            this.contextLogger.Object.Warning(this.message);
            this.contextLogger.Verify(x => x.Write(LogLevel.Warning, this.message), Times.Once);
        }

        [Test]
        public void Warning_ContextLoggerValidMessageDetails_WriteMessageDetailsCalledOnce()
        {
            this.contextLogger.Object.Warning(this.message, this.details);
            this.contextLogger.Verify(x => x.Write(LogLevel.Warning, this.message, this.details), Times.Once);
        }

        [Test]
        public void Warning_ContextNullLoggerException_WriteExceptionCalledNever()
        {
            this.contextNullLogger.Object.Object.Warning(this.exception);
            this.contextNullLogger.Verify(x => x.Object.Write(LogLevel.Warning, this.exception), Times.Never);
        }

        [Test]
        public void Warning_ContextNullLoggerExceptionDetails_WriteExceptionDetailsCalledNever()
        {
            this.contextNullLogger.Object.Object.Warning(this.exception, this.details);
            this.contextNullLogger.Verify(x => x.Object.Write(LogLevel.Warning, this.exception, this.details), Times.Never);
        }

        [Test]
        public void Warning_ContextLoggerValidException_WriteExceptionCalledOnce()
        {
            this.contextLogger.Object.Warning(this.exception);
            this.contextLogger.Verify(x => x.Write(LogLevel.Warning, this.exception), Times.Once);
        }

        [Test]
        public void Warning_ContextLoggerValidExceptionDetails_WriteExceptionDetailsCalledOnce()
        {
            this.contextLogger.Object.Warning(this.exception, this.details);
            this.contextLogger.Verify(x => x.Write(LogLevel.Warning, this.exception, this.details), Times.Once);
        }

        [Test]
        public void Warning_ContextNullLoggerMessageException_WriteMessageExceptionCalledNever()
        {
            this.contextNullLogger.Object.Object.Warning(this.message, this.exception);
            this.contextNullLogger.Verify(x => x.Object.Write(LogLevel.Warning, this.message, this.exception), Times.Never);
        }

        [Test]
        public void Warning_ContextNullLoggerMessageExceptionDetails_WriteMessageExceptionDetailsCalledNever()
        {
            this.contextNullLogger.Object.Object.Warning(this.message, this.exception, this.details);
            this.contextNullLogger.Verify(x => x.Object.Write(LogLevel.Warning, this.message, this.exception, this.details), Times.Never);
        }

        [Test]
        public void Warning_ContextLoggerValidMessageException_WriteMessageExceptionCalledOnce()
        {
            this.contextLogger.Object.Warning(this.message, this.exception);
            this.contextLogger.Verify(x => x.Write(LogLevel.Warning, this.message, this.exception), Times.Once);
        }

        [Test]
        public void Warning_ContextLoggerValidMessageExceptionDetails_WriteMessageExceptionDetailsCalledOnce()
        {
            this.contextLogger.Object.Warning(this.message, this.exception, this.details);
            this.contextLogger.Verify(x => x.Write(LogLevel.Warning, this.message, this.exception, this.details), Times.Once);
        }

        [Test]
        public void Warning_ContextNullLoggerScopeMessage_WriteMessageCalledNever()
        {
            this.contextNullLogger.Object.Object.Warning(this.method, this.message);
            this.contextNullLogger.Verify(x => x.Object.Write(this.method, LogLevel.Warning, this.message), Times.Never);
        }

        [Test]
        public void Warning_ContextNullLoggerScopeMessageDetails_WriteMessageDetailsCalledNever()
        {
            this.contextNullLogger.Object.Object.Warning(this.method, this.message, this.details);
            this.contextNullLogger.Verify(x => x.Object.Write(this.method, LogLevel.Warning, this.message, this.details), Times.Never);
        }

        [Test]
        public void Warning_ContextLoggerScopeMessage_WriteMessageCalledOnce()
        {
            this.contextLogger.Object.Warning(this.method, this.message);
            this.contextLogger.Verify(x => x.Write(this.method, LogLevel.Warning, this.message), Times.Once);
        }

        [Test]
        public void Warning_ContextLoggerScopeMessageDetails_WriteMessageDetailsCalledOnce()
        {
            this.contextLogger.Object.Warning(this.method, this.message, this.details);
            this.contextLogger.Verify(x => x.Write(this.method, LogLevel.Warning, this.message, this.details), Times.Once);
        }

        [Test]
        public void Warning_ContextNullLoggerScopeException_WriteExceptionCalledNever()
        {
            this.contextNullLogger.Object.Object.Warning(this.method, this.exception);
            this.contextNullLogger.Verify(x => x.Object.Write(this.method, LogLevel.Warning, this.exception), Times.Never);
        }

        [Test]
        public void Warning_ContextNullLoggerScopeExceptionDetails_WriteExceptionDetailsCalledNever()
        {
            this.contextNullLogger.Object.Object.Warning(this.method, this.exception, this.details);
            this.contextNullLogger.Verify(x => x.Object.Write(this.method, LogLevel.Warning, this.exception, this.details), Times.Never);
        }

        [Test]
        public void Warning_ContextLoggerScopeException_WriteExceptionCalledOnce()
        {
            this.contextLogger.Object.Warning(this.method, this.exception);
            this.contextLogger.Verify(x => x.Write(this.method, LogLevel.Warning, this.exception), Times.Once);
        }

        [Test]
        public void Warning_ContextLoggerScopeExceptionDetails_WriteExceptionDetailsCalledOnce()
        {
            this.contextLogger.Object.Warning(this.method, this.exception, this.details);
            this.contextLogger.Verify(x => x.Write(this.method, LogLevel.Warning, this.exception, this.details), Times.Once);
        }

        [Test]
        public void Warning_ContextNullLoggerScopeMessageException_WriteMessageExceptionCalledNever()
        {
            this.contextNullLogger.Object.Object.Warning(this.method, this.message, this.exception);
            this.contextNullLogger.Verify(x => x.Object.Write(this.method, LogLevel.Warning, this.message, this.exception), Times.Never);
        }

        [Test]
        public void Warning_ContextNullLoggerScopeMessageExceptionDetails_WriteMessageExceptionDetailsCalledNever()
        {
            this.contextNullLogger.Object.Object.Warning(this.method, this.message, this.exception, this.details);
            this.contextNullLogger.Verify(x => x.Object.Write(this.method, LogLevel.Warning, this.message, this.exception, this.details), Times.Never);
        }

        [Test]
        public void Warning_ContextLoggerScopeMessageException_WriteMessageExceptionCalledOnce()
        {
            this.contextLogger.Object.Warning(this.method, this.message, this.exception);
            this.contextLogger.Verify(x => x.Write(this.method, LogLevel.Warning, this.message, this.exception), Times.Once);
        }

        [Test]
        public void Warning_ContextLoggerScopeMessageExceptionDetails_WriteMessageExceptionDetailsCalledOnce()
        {
            this.contextLogger.Object.Warning(this.method, this.message, this.exception, this.details);
            this.contextLogger.Verify(x => x.Write(this.method, LogLevel.Warning, this.message, this.exception, this.details), Times.Once);
        }

        #endregion

        #region Error

        [Test]
        public void Error_DefaultNullLoggerMessage_WriteMessageCalledNever()
        {
            this.defaultNullLogger.Object.Object.Error(this.message);
            this.defaultNullLogger.Verify(x => x.Object.Write(LogLevel.Error, this.message), Times.Never);
        }

        [Test]
        public void Error_DefaultNullLoggerMessageDetails_WriteMessageDetailsCalledNever()
        {
            this.defaultNullLogger.Object.Object.Error(this.message, this.details);
            this.defaultNullLogger.Verify(x => x.Object.Write(LogLevel.Error, this.message, this.details), Times.Never);
        }

        [Test]
        public void Error_DefaultLoggerMessage_WriteMessageCalledOnce()
        {
            this.defaultLogger.Object.Error(this.message);
            this.defaultLogger.Verify(x => x.Write(LogLevel.Error, this.message), Times.Once);
        }

        [Test]
        public void Error_DefaultLoggerMessageDetails_WriteMessageDetailsCalledOnce()
        {
            this.defaultLogger.Object.Error(this.message, this.details);
            this.defaultLogger.Verify(x => x.Write(LogLevel.Error, this.message, this.details), Times.Once);
        }

        [Test]
        public void Error_DefaultNullLoggerException_WriteExceptionCalledNever()
        {
            this.defaultNullLogger.Object.Object.Error(this.exception);
            this.defaultNullLogger.Verify(x => x.Object.Write(LogLevel.Error, this.exception), Times.Never);
        }

        [Test]
        public void Error_DefaultNullLoggerExceptionDetails_WriteExceptionDetailsCalledNever()
        {
            this.defaultNullLogger.Object.Object.Error(this.exception, this.details);
            this.defaultNullLogger.Verify(x => x.Object.Write(LogLevel.Error, this.exception, this.details), Times.Never);
        }

        [Test]
        public void Error_DefaultLoggerException_WriteExceptionCalledOnce()
        {
            this.defaultLogger.Object.Error(this.exception);
            this.defaultLogger.Verify(x => x.Write(LogLevel.Error, this.exception), Times.Once);
        }

        [Test]
        public void Error_DefaultLoggerExceptionDetails_WriteExceptionDetailsCalledOnce()
        {
            this.defaultLogger.Object.Error(this.exception, this.details);
            this.defaultLogger.Verify(x => x.Write(LogLevel.Error, this.exception, this.details), Times.Once);
        }

        [Test]
        public void Error_DefaultNullLoggerMessageException_WriteMessageExceptionCalledNever()
        {
            this.defaultNullLogger.Object.Object.Error(this.message, this.exception);
            this.defaultNullLogger.Verify(x => x.Object.Write(LogLevel.Error, this.message, this.exception), Times.Never);
        }

        [Test]
        public void Error_DefaultNullLoggerMessageExceptionDetails_WriteMessageExceptionDetailsCalledNever()
        {
            this.defaultNullLogger.Object.Object.Error(this.message, this.exception, this.details);
            this.defaultNullLogger.Verify(x => x.Object.Write(LogLevel.Error, this.message, this.exception, this.details), Times.Never);
        }

        [Test]
        public void Error_DefaultLoggerMessageException_WriteMessageExceptionCalledOnce()
        {
            this.defaultLogger.Object.Error(this.message, this.exception);
            this.defaultLogger.Verify(x => x.Write(LogLevel.Error, this.message, this.exception), Times.Once);
        }

        [Test]
        public void Error_DefaultLoggerMessageExceptionDetails_WriteMessageExceptionDetailsCalledOnce()
        {
            this.defaultLogger.Object.Error(this.message, this.exception, this.details);
            this.defaultLogger.Verify(x => x.Write(LogLevel.Error, this.message, this.exception, this.details), Times.Once);
        }

        [Test]
        public void Error_DefaultNullLoggerScopeMessage_WriteMessageCalledNever()
        {
            this.defaultNullLogger.Object.Object.Error(this.method, this.message);
            this.defaultNullLogger.Verify(x => x.Object.Write(this.method, LogLevel.Error, this.message), Times.Never);
        }

        [Test]
        public void Error_DefaultNullLoggerScopeMessageDetails_WriteMessageDetailsCalledNever()
        {
            this.defaultNullLogger.Object.Object.Error(this.method, this.message, this.details);
            this.defaultNullLogger.Verify(x => x.Object.Write(this.method, LogLevel.Error, this.message, this.details), Times.Never);
        }

        [Test]
        public void Error_DefaultLoggerScopeMessage_WriteMessageCalledOnce()
        {
            this.defaultLogger.Object.Error(this.method, this.message);
            this.defaultLogger.Verify(x => x.Write(this.method, LogLevel.Error, this.message), Times.Once);
        }

        [Test]
        public void Error_DefaultLoggerScopeMessageDetails_WriteMessageDetailsCalledOnce()
        {
            this.defaultLogger.Object.Error(this.method, this.message, this.details);
            this.defaultLogger.Verify(x => x.Write(this.method, LogLevel.Error, this.message, this.details), Times.Once);
        }

        [Test]
        public void Error_DefaultNullLoggerScopeException_WriteExceptionCalledNever()
        {
            this.defaultNullLogger.Object.Object.Error(this.method, this.exception);
            this.defaultNullLogger.Verify(x => x.Object.Write(this.method, LogLevel.Error, this.exception), Times.Never);
        }

        [Test]
        public void Error_DefaultNullLoggerScopeExceptionDetails_WriteExceptionDetailsCalledNever()
        {
            this.defaultNullLogger.Object.Object.Error(this.method, this.exception, this.details);
            this.defaultNullLogger.Verify(x => x.Object.Write(this.method, LogLevel.Error, this.exception, this.details), Times.Never);
        }

        [Test]
        public void Error_DefaultLoggerScopeException_WriteExceptionCalledOnce()
        {
            this.defaultLogger.Object.Error(this.method, this.exception);
            this.defaultLogger.Verify(x => x.Write(this.method, LogLevel.Error, this.exception), Times.Once);
        }

        [Test]
        public void Error_DefaultLoggerScopeExceptionDetails_WriteExceptionDetailsCalledOnce()
        {
            this.defaultLogger.Object.Error(this.method, this.exception, this.details);
            this.defaultLogger.Verify(x => x.Write(this.method, LogLevel.Error, this.exception, this.details), Times.Once);
        }

        [Test]
        public void Error_DefaultNullLoggerScopeMessageException_WriteMessageExceptionCalledNever()
        {
            this.defaultNullLogger.Object.Object.Error(this.method, this.message, this.exception);
            this.defaultNullLogger.Verify(x => x.Object.Write(this.method, LogLevel.Error, this.message, this.exception), Times.Never);
        }

        [Test]
        public void Error_DefaultNullLoggerScopeMessageExceptionDetails_WriteMessageExceptionDetailsCalledNever()
        {
            this.defaultNullLogger.Object.Object.Error(this.method, this.message, this.exception, this.details);
            this.defaultNullLogger.Verify(x => x.Object.Write(this.method, LogLevel.Error, this.message, this.exception, this.details), Times.Never);
        }

        [Test]
        public void Error_DefaultLoggerScopeMessageException_WriteMessageExceptionCalledOnce()
        {
            this.defaultLogger.Object.Error(this.method, this.message, this.exception);
            this.defaultLogger.Verify(x => x.Write(this.method, LogLevel.Error, this.message, this.exception), Times.Once);
        }

        [Test]
        public void Error_DefaultLoggerScopeMessageExceptionDetails_WriteMessageExceptionDetailsCalledOnce()
        {
            this.defaultLogger.Object.Error(this.method, this.message, this.exception, this.details);
            this.defaultLogger.Verify(x => x.Write(this.method, LogLevel.Error, this.message, this.exception, this.details), Times.Once);
        }

        [Test]
        public void Error_ContextNullLoggerMessage_WriteMessageCalledNever()
        {
            this.contextNullLogger.Object.Object.Error(this.message);
            this.contextNullLogger.Verify(x => x.Object.Write(LogLevel.Error, this.message), Times.Never);
        }

        [Test]
        public void Error_ContextNullLoggerMessageDetails_WriteMessageDetailsCalledNever()
        {
            this.contextNullLogger.Object.Object.Error(this.message, this.details);
            this.contextNullLogger.Verify(x => x.Object.Write(LogLevel.Error, this.message, this.details), Times.Never);
        }

        [Test]
        public void Error_ContextLoggerValidMessage_WriteMessageCalledOnce()
        {
            this.contextLogger.Object.Error(this.message);
            this.contextLogger.Verify(x => x.Write(LogLevel.Error, this.message), Times.Once);
        }

        [Test]
        public void Error_ContextLoggerValidMessageDetails_WriteMessageDetailsCalledOnce()
        {
            this.contextLogger.Object.Error(this.message, this.details);
            this.contextLogger.Verify(x => x.Write(LogLevel.Error, this.message, this.details), Times.Once);
        }

        [Test]
        public void Error_ContextNullLoggerException_WriteExceptionCalledNever()
        {
            this.contextNullLogger.Object.Object.Error(this.exception);
            this.contextNullLogger.Verify(x => x.Object.Write(LogLevel.Error, this.exception), Times.Never);
        }

        [Test]
        public void Error_ContextNullLoggerExceptionDetails_WriteExceptionDetailsCalledNever()
        {
            this.contextNullLogger.Object.Object.Error(this.exception, this.details);
            this.contextNullLogger.Verify(x => x.Object.Write(LogLevel.Error, this.exception, this.details), Times.Never);
        }

        [Test]
        public void Error_ContextLoggerValidException_WriteExceptionCalledOnce()
        {
            this.contextLogger.Object.Error(this.exception);
            this.contextLogger.Verify(x => x.Write(LogLevel.Error, this.exception), Times.Once);
        }

        [Test]
        public void Error_ContextLoggerValidExceptionDetails_WriteExceptionDetailsCalledOnce()
        {
            this.contextLogger.Object.Error(this.exception, this.details);
            this.contextLogger.Verify(x => x.Write(LogLevel.Error, this.exception, this.details), Times.Once);
        }

        [Test]
        public void Error_ContextNullLoggerMessageException_WriteMessageExceptionCalledNever()
        {
            this.contextNullLogger.Object.Object.Error(this.message, this.exception);
            this.contextNullLogger.Verify(x => x.Object.Write(LogLevel.Error, this.message, this.exception), Times.Never);
        }

        [Test]
        public void Error_ContextNullLoggerMessageExceptionDetails_WriteMessageExceptionDetailsCalledNever()
        {
            this.contextNullLogger.Object.Object.Error(this.message, this.exception, this.details);
            this.contextNullLogger.Verify(x => x.Object.Write(LogLevel.Error, this.message, this.exception, this.details), Times.Never);
        }

        [Test]
        public void Error_ContextLoggerValidMessageException_WriteMessageExceptionCalledOnce()
        {
            this.contextLogger.Object.Error(this.message, this.exception);
            this.contextLogger.Verify(x => x.Write(LogLevel.Error, this.message, this.exception), Times.Once);
        }

        [Test]
        public void Error_ContextLoggerValidMessageExceptionDetails_WriteMessageExceptionDetailsCalledOnce()
        {
            this.contextLogger.Object.Error(this.message, this.exception, this.details);
            this.contextLogger.Verify(x => x.Write(LogLevel.Error, this.message, this.exception, this.details), Times.Once);
        }

        [Test]
        public void Error_ContextNullLoggerScopeMessage_WriteMessageCalledNever()
        {
            this.contextNullLogger.Object.Object.Error(this.method, this.message);
            this.contextNullLogger.Verify(x => x.Object.Write(this.method, LogLevel.Error, this.message), Times.Never);
        }

        [Test]
        public void Error_ContextNullLoggerScopeMessageDetails_WriteMessageDetailsCalledNever()
        {
            this.contextNullLogger.Object.Object.Error(this.method, this.message, this.details);
            this.contextNullLogger.Verify(x => x.Object.Write(this.method, LogLevel.Error, this.message, this.details), Times.Never);
        }

        [Test]
        public void Error_ContextLoggerScopeMessage_WriteMessageCalledOnce()
        {
            this.contextLogger.Object.Error(this.method, this.message);
            this.contextLogger.Verify(x => x.Write(this.method, LogLevel.Error, this.message), Times.Once);
        }

        [Test]
        public void Error_ContextLoggerScopeMessageDetails_WriteMessageDetailsCalledOnce()
        {
            this.contextLogger.Object.Error(this.method, this.message, this.details);
            this.contextLogger.Verify(x => x.Write(this.method, LogLevel.Error, this.message, this.details), Times.Once);
        }

        [Test]
        public void Error_ContextNullLoggerScopeException_WriteExceptionCalledNever()
        {
            this.contextNullLogger.Object.Object.Error(this.method, this.exception);
            this.contextNullLogger.Verify(x => x.Object.Write(this.method, LogLevel.Error, this.exception), Times.Never);
        }

        [Test]
        public void Error_ContextNullLoggerScopeExceptionDetails_WriteExceptionDetailsCalledNever()
        {
            this.contextNullLogger.Object.Object.Error(this.method, this.exception, this.details);
            this.contextNullLogger.Verify(x => x.Object.Write(this.method, LogLevel.Error, this.exception, this.details), Times.Never);
        }

        [Test]
        public void Error_ContextLoggerScopeException_WriteExceptionCalledOnce()
        {
            this.contextLogger.Object.Error(this.method, this.exception);
            this.contextLogger.Verify(x => x.Write(this.method, LogLevel.Error, this.exception), Times.Once);
        }

        [Test]
        public void Error_ContextLoggerScopeExceptionDetails_WriteExceptionDetailsCalledOnce()
        {
            this.contextLogger.Object.Error(this.method, this.exception, this.details);
            this.contextLogger.Verify(x => x.Write(this.method, LogLevel.Error, this.exception, this.details), Times.Once);
        }

        [Test]
        public void Error_ContextNullLoggerScopeMessageException_WriteMessageExceptionCalledNever()
        {
            this.contextNullLogger.Object.Object.Error(this.method, this.message, this.exception);
            this.contextNullLogger.Verify(x => x.Object.Write(this.method, LogLevel.Error, this.message, this.exception), Times.Never);
        }

        [Test]
        public void Error_ContextNullLoggerScopeMessageExceptionDetails_WriteMessageExceptionDetailsCalledNever()
        {
            this.contextNullLogger.Object.Object.Error(this.method, this.message, this.exception, this.details);
            this.contextNullLogger.Verify(x => x.Object.Write(this.method, LogLevel.Error, this.message, this.exception, this.details), Times.Never);
        }

        [Test]
        public void Error_ContextLoggerScopeMessageException_WriteMessageExceptionCalledOnce()
        {
            this.contextLogger.Object.Error(this.method, this.message, this.exception);
            this.contextLogger.Verify(x => x.Write(this.method, LogLevel.Error, this.message, this.exception), Times.Once);
        }

        [Test]
        public void Error_ContextLoggerScopeMessageExceptionDetails_WriteMessageExceptionDetailsCalledOnce()
        {
            this.contextLogger.Object.Error(this.method, this.message, this.exception, this.details);
            this.contextLogger.Verify(x => x.Write(this.method, LogLevel.Error, this.message, this.exception, this.details), Times.Once);
        }

        #endregion

        #region Fatal

        [Test]
        public void Fatal_DefaultNullLoggerMessage_WriteMessageCalledNever()
        {
            this.defaultNullLogger.Object.Object.Fatal(this.message);
            this.defaultNullLogger.Verify(x => x.Object.Write(LogLevel.Fatal, this.message), Times.Never);
        }

        [Test]
        public void Fatal_DefaultNullLoggerMessageDetails_WriteMessageDetailsCalledNever()
        {
            this.defaultNullLogger.Object.Object.Fatal(this.message, this.details);
            this.defaultNullLogger.Verify(x => x.Object.Write(LogLevel.Fatal, this.message, this.details), Times.Never);
        }

        [Test]
        public void Fatal_DefaultLoggerMessage_WriteMessageCalledOnce()
        {
            this.defaultLogger.Object.Fatal(this.message);
            this.defaultLogger.Verify(x => x.Write(LogLevel.Fatal, this.message), Times.Once);
        }

        [Test]
        public void Fatal_DefaultLoggerMessageDetails_WriteMessageDetailsCalledOnce()
        {
            this.defaultLogger.Object.Fatal(this.message, this.details);
            this.defaultLogger.Verify(x => x.Write(LogLevel.Fatal, this.message, this.details), Times.Once);
        }

        [Test]
        public void Fatal_DefaultNullLoggerException_WriteExceptionCalledNever()
        {
            this.defaultNullLogger.Object.Object.Fatal(this.exception);
            this.defaultNullLogger.Verify(x => x.Object.Write(LogLevel.Fatal, this.exception), Times.Never);
        }

        [Test]
        public void Fatal_DefaultNullLoggerExceptionDetails_WriteExceptionDetailsCalledNever()
        {
            this.defaultNullLogger.Object.Object.Fatal(this.exception, this.details);
            this.defaultNullLogger.Verify(x => x.Object.Write(LogLevel.Fatal, this.exception, this.details), Times.Never);
        }

        [Test]
        public void Fatal_DefaultLoggerException_WriteExceptionCalledOnce()
        {
            this.defaultLogger.Object.Fatal(this.exception);
            this.defaultLogger.Verify(x => x.Write(LogLevel.Fatal, this.exception), Times.Once);
        }

        [Test]
        public void Fatal_DefaultLoggerExceptionDetails_WriteExceptionDetailsCalledOnce()
        {
            this.defaultLogger.Object.Fatal(this.exception, this.details);
            this.defaultLogger.Verify(x => x.Write(LogLevel.Fatal, this.exception, this.details), Times.Once);
        }

        [Test]
        public void Fatal_DefaultNullLoggerMessageException_WriteMessageExceptionCalledNever()
        {
            this.defaultNullLogger.Object.Object.Fatal(this.message, this.exception);
            this.defaultNullLogger.Verify(x => x.Object.Write(LogLevel.Fatal, this.message, this.exception), Times.Never);
        }

        [Test]
        public void Fatal_DefaultNullLoggerMessageExceptionDetails_WriteMessageExceptionDetailsCalledNever()
        {
            this.defaultNullLogger.Object.Object.Fatal(this.message, this.exception, this.details);
            this.defaultNullLogger.Verify(x => x.Object.Write(LogLevel.Fatal, this.message, this.exception, this.details), Times.Never);
        }

        [Test]
        public void Fatal_DefaultLoggerMessageException_WriteMessageExceptionCalledOnce()
        {
            this.defaultLogger.Object.Fatal(this.message, this.exception);
            this.defaultLogger.Verify(x => x.Write(LogLevel.Fatal, this.message, this.exception), Times.Once);
        }

        [Test]
        public void Fatal_DefaultLoggerMessageExceptionDetails_WriteMessageExceptionDetailsCalledOnce()
        {
            this.defaultLogger.Object.Fatal(this.message, this.exception, this.details);
            this.defaultLogger.Verify(x => x.Write(LogLevel.Fatal, this.message, this.exception, this.details), Times.Once);
        }

        [Test]
        public void Fatal_DefaultNullLoggerScopeMessage_WriteMessageCalledNever()
        {
            this.defaultNullLogger.Object.Object.Fatal(this.method, this.message);
            this.defaultNullLogger.Verify(x => x.Object.Write(this.method, LogLevel.Fatal, this.message), Times.Never);
        }

        [Test]
        public void Fatal_DefaultNullLoggerScopeMessageDetails_WriteMessageDetailsCalledNever()
        {
            this.defaultNullLogger.Object.Object.Fatal(this.method, this.message, this.details);
            this.defaultNullLogger.Verify(x => x.Object.Write(this.method, LogLevel.Fatal, this.message, this.details), Times.Never);
        }

        [Test]
        public void Fatal_DefaultLoggerScopeMessage_WriteMessageCalledOnce()
        {
            this.defaultLogger.Object.Fatal(this.method, this.message);
            this.defaultLogger.Verify(x => x.Write(this.method, LogLevel.Fatal, this.message), Times.Once);
        }

        [Test]
        public void Fatal_DefaultLoggerScopeMessageDetails_WriteMessageDetailsCalledOnce()
        {
            this.defaultLogger.Object.Fatal(this.method, this.message, this.details);
            this.defaultLogger.Verify(x => x.Write(this.method, LogLevel.Fatal, this.message, this.details), Times.Once);
        }

        [Test]
        public void Fatal_DefaultNullLoggerScopeException_WriteExceptionCalledNever()
        {
            this.defaultNullLogger.Object.Object.Fatal(this.method, this.exception);
            this.defaultNullLogger.Verify(x => x.Object.Write(this.method, LogLevel.Fatal, this.exception), Times.Never);
        }

        [Test]
        public void Fatal_DefaultNullLoggerScopeExceptionDetails_WriteExceptionDetailsCalledNever()
        {
            this.defaultNullLogger.Object.Object.Fatal(this.method, this.exception, this.details);
            this.defaultNullLogger.Verify(x => x.Object.Write(this.method, LogLevel.Fatal, this.exception, this.details), Times.Never);
        }

        [Test]
        public void Fatal_DefaultLoggerScopeException_WriteExceptionCalledOnce()
        {
            this.defaultLogger.Object.Fatal(this.method, this.exception);
            this.defaultLogger.Verify(x => x.Write(this.method, LogLevel.Fatal, this.exception), Times.Once);
        }

        [Test]
        public void Fatal_DefaultLoggerScopeExceptionDetails_WriteExceptionDetailsCalledOnce()
        {
            this.defaultLogger.Object.Fatal(this.method, this.exception, this.details);
            this.defaultLogger.Verify(x => x.Write(this.method, LogLevel.Fatal, this.exception, this.details), Times.Once);
        }

        [Test]
        public void Fatal_DefaultNullLoggerScopeMessageException_WriteMessageExceptionCalledNever()
        {
            this.defaultNullLogger.Object.Object.Fatal(this.method, this.message, this.exception);
            this.defaultNullLogger.Verify(x => x.Object.Write(this.method, LogLevel.Fatal, this.message, this.exception), Times.Never);
        }

        [Test]
        public void Fatal_DefaultNullLoggerScopeMessageExceptionDetails_WriteMessageExceptionDetailsCalledNever()
        {
            this.defaultNullLogger.Object.Object.Fatal(this.method, this.message, this.exception, this.details);
            this.defaultNullLogger.Verify(x => x.Object.Write(this.method, LogLevel.Fatal, this.message, this.exception, this.details), Times.Never);
        }

        [Test]
        public void Fatal_DefaultLoggerScopeMessageException_WriteMessageExceptionCalledOnce()
        {
            this.defaultLogger.Object.Fatal(this.method, this.message, this.exception);
            this.defaultLogger.Verify(x => x.Write(this.method, LogLevel.Fatal, this.message, this.exception), Times.Once);
        }

        [Test]
        public void Fatal_DefaultLoggerScopeMessageExceptionDetails_WriteMessageExceptionDetailsCalledOnce()
        {
            this.defaultLogger.Object.Fatal(this.method, this.message, this.exception, this.details);
            this.defaultLogger.Verify(x => x.Write(this.method, LogLevel.Fatal, this.message, this.exception, this.details), Times.Once);
        }

        [Test]
        public void Fatal_ContextNullLoggerMessage_WriteMessageCalledNever()
        {
            this.contextNullLogger.Object.Object.Fatal(this.message);
            this.contextNullLogger.Verify(x => x.Object.Write(LogLevel.Fatal, this.message), Times.Never);
        }

        [Test]
        public void Fatal_ContextNullLoggerMessageDetails_WriteMessageDetailsCalledNever()
        {
            this.contextNullLogger.Object.Object.Fatal(this.message, this.details);
            this.contextNullLogger.Verify(x => x.Object.Write(LogLevel.Fatal, this.message, this.details), Times.Never);
        }

        [Test]
        public void Fatal_ContextLoggerValidMessage_WriteMessageCalledOnce()
        {
            this.contextLogger.Object.Fatal(this.message);
            this.contextLogger.Verify(x => x.Write(LogLevel.Fatal, this.message), Times.Once);
        }

        [Test]
        public void Fatal_ContextLoggerValidMessageDetails_WriteMessageDetailsCalledOnce()
        {
            this.contextLogger.Object.Fatal(this.message, this.details);
            this.contextLogger.Verify(x => x.Write(LogLevel.Fatal, this.message, this.details), Times.Once);
        }

        [Test]
        public void Fatal_ContextNullLoggerException_WriteExceptionCalledNever()
        {
            this.contextNullLogger.Object.Object.Fatal(this.exception);
            this.contextNullLogger.Verify(x => x.Object.Write(LogLevel.Fatal, this.exception), Times.Never);
        }

        [Test]
        public void Fatal_ContextNullLoggerExceptionDetails_WriteExceptionDetailsCalledNever()
        {
            this.contextNullLogger.Object.Object.Fatal(this.exception, this.details);
            this.contextNullLogger.Verify(x => x.Object.Write(LogLevel.Fatal, this.exception, this.details), Times.Never);
        }

        [Test]
        public void Fatal_ContextLoggerValidException_WriteExceptionCalledOnce()
        {
            this.contextLogger.Object.Fatal(this.exception);
            this.contextLogger.Verify(x => x.Write(LogLevel.Fatal, this.exception), Times.Once);
        }

        [Test]
        public void Fatal_ContextLoggerValidExceptionDetails_WriteExceptionDetailsCalledOnce()
        {
            this.contextLogger.Object.Fatal(this.exception, this.details);
            this.contextLogger.Verify(x => x.Write(LogLevel.Fatal, this.exception, this.details), Times.Once);
        }

        [Test]
        public void Fatal_ContextNullLoggerMessageException_WriteMessageExceptionCalledNever()
        {
            this.contextNullLogger.Object.Object.Fatal(this.message, this.exception);
            this.contextNullLogger.Verify(x => x.Object.Write(LogLevel.Fatal, this.message, this.exception), Times.Never);
        }

        [Test]
        public void Fatal_ContextNullLoggerMessageExceptionDetails_WriteMessageExceptionDetailsCalledNever()
        {
            this.contextNullLogger.Object.Object.Fatal(this.message, this.exception, this.details);
            this.contextNullLogger.Verify(x => x.Object.Write(LogLevel.Fatal, this.message, this.exception, this.details), Times.Never);
        }

        [Test]
        public void Fatal_ContextLoggerValidMessageException_WriteMessageExceptionCalledOnce()
        {
            this.contextLogger.Object.Fatal(this.message, this.exception);
            this.contextLogger.Verify(x => x.Write(LogLevel.Fatal, this.message, this.exception), Times.Once);
        }

        [Test]
        public void Fatal_ContextLoggerValidMessageExceptionDetails_WriteMessageExceptionDetailsCalledOnce()
        {
            this.contextLogger.Object.Fatal(this.message, this.exception, this.details);
            this.contextLogger.Verify(x => x.Write(LogLevel.Fatal, this.message, this.exception, this.details), Times.Once);
        }

        [Test]
        public void Fatal_ContextNullLoggerScopeMessage_WriteMessageCalledNever()
        {
            this.contextNullLogger.Object.Object.Fatal(this.method, this.message);
            this.contextNullLogger.Verify(x => x.Object.Write(this.method, LogLevel.Fatal, this.message), Times.Never);
        }

        [Test]
        public void Fatal_ContextNullLoggerScopeMessageDetails_WriteMessageDetailsCalledNever()
        {
            this.contextNullLogger.Object.Object.Fatal(this.method, this.message, this.details);
            this.contextNullLogger.Verify(x => x.Object.Write(this.method, LogLevel.Fatal, this.message, this.details), Times.Never);
        }

        [Test]
        public void Fatal_ContextLoggerScopeMessage_WriteMessageCalledOnce()
        {
            this.contextLogger.Object.Fatal(this.method, this.message);
            this.contextLogger.Verify(x => x.Write(this.method, LogLevel.Fatal, this.message), Times.Once);
        }

        [Test]
        public void Fatal_ContextLoggerScopeMessageDetails_WriteMessageDetailsCalledOnce()
        {
            this.contextLogger.Object.Fatal(this.method, this.message, this.details);
            this.contextLogger.Verify(x => x.Write(this.method, LogLevel.Fatal, this.message, this.details), Times.Once);
        }

        [Test]
        public void Fatal_ContextNullLoggerScopeException_WriteExceptionCalledNever()
        {
            this.contextNullLogger.Object.Object.Fatal(this.method, this.exception);
            this.contextNullLogger.Verify(x => x.Object.Write(this.method, LogLevel.Fatal, this.exception), Times.Never);
        }

        [Test]
        public void Fatal_ContextNullLoggerScopeExceptionDetails_WriteExceptionDetailsCalledNever()
        {
            this.contextNullLogger.Object.Object.Fatal(this.method, this.exception, this.details);
            this.contextNullLogger.Verify(x => x.Object.Write(this.method, LogLevel.Fatal, this.exception, this.details), Times.Never);
        }

        [Test]
        public void Fatal_ContextLoggerScopeException_WriteExceptionCalledOnce()
        {
            this.contextLogger.Object.Fatal(this.method, this.exception);
            this.contextLogger.Verify(x => x.Write(this.method, LogLevel.Fatal, this.exception), Times.Once);
        }

        [Test]
        public void Fatal_ContextLoggerScopeExceptionDetails_WriteExceptionDetailsCalledOnce()
        {
            this.contextLogger.Object.Fatal(this.method, this.exception, this.details);
            this.contextLogger.Verify(x => x.Write(this.method, LogLevel.Fatal, this.exception, this.details), Times.Once);
        }

        [Test]
        public void Fatal_ContextNullLoggerScopeMessageException_WriteMessageExceptionCalledNever()
        {
            this.contextNullLogger.Object.Object.Fatal(this.method, this.message, this.exception);
            this.contextNullLogger.Verify(x => x.Object.Write(this.method, LogLevel.Fatal, this.message, this.exception), Times.Never);
        }

        [Test]
        public void Fatal_ContextNullLoggerScopeMessageExceptionDetails_WriteMessageExceptionDetailsCalledNever()
        {
            this.contextNullLogger.Object.Object.Fatal(this.method, this.message, this.exception, this.details);
            this.contextNullLogger.Verify(x => x.Object.Write(this.method, LogLevel.Fatal, this.message, this.exception, this.details), Times.Never);
        }

        [Test]
        public void Fatal_ContextLoggerScopeMessageException_WriteMessageExceptionCalledOnce()
        {
            this.contextLogger.Object.Fatal(this.method, this.message, this.exception);
            this.contextLogger.Verify(x => x.Write(this.method, LogLevel.Fatal, this.message, this.exception), Times.Once);
        }

        [Test]
        public void Fatal_ContextLoggerScopeMessageExceptionDetails_WriteMessageExceptionDetailsCalledOnce()
        {
            this.contextLogger.Object.Fatal(this.method, this.message, this.exception, this.details);
            this.contextLogger.Verify(x => x.Write(this.method, LogLevel.Fatal, this.message, this.exception, this.details), Times.Once);
        }

        #endregion

        #region Critical

        [Test]
        public void Critical_DefaultNullLoggerMessage_WriteMessageCalledNever()
        {
            this.defaultNullLogger.Object.Object.Critical(this.message);
            this.defaultNullLogger.Verify(x => x.Object.Write(LogLevel.Critical, this.message), Times.Never);
        }

        [Test]
        public void Critical_DefaultNullLoggerMessageDetails_WriteMessageDetailsCalledNever()
        {
            this.defaultNullLogger.Object.Object.Critical(this.message, this.details);
            this.defaultNullLogger.Verify(x => x.Object.Write(LogLevel.Critical, this.message, this.details), Times.Never);
        }

        [Test]
        public void Critical_DefaultLoggerMessage_WriteMessageCalledOnce()
        {
            this.defaultLogger.Object.Critical(this.message);
            this.defaultLogger.Verify(x => x.Write(LogLevel.Critical, this.message), Times.Once);
        }

        [Test]
        public void Critical_DefaultLoggerMessageDetails_WriteMessageDetailsCalledOnce()
        {
            this.defaultLogger.Object.Critical(this.message, this.details);
            this.defaultLogger.Verify(x => x.Write(LogLevel.Critical, this.message, this.details), Times.Once);
        }

        [Test]
        public void Critical_DefaultNullLoggerException_WriteExceptionCalledNever()
        {
            this.defaultNullLogger.Object.Object.Critical(this.exception);
            this.defaultNullLogger.Verify(x => x.Object.Write(LogLevel.Critical, this.exception), Times.Never);
        }

        [Test]
        public void Critical_DefaultNullLoggerExceptionDetails_WriteExceptionDetailsCalledNever()
        {
            this.defaultNullLogger.Object.Object.Critical(this.exception, this.details);
            this.defaultNullLogger.Verify(x => x.Object.Write(LogLevel.Critical, this.exception, this.details), Times.Never);
        }

        [Test]
        public void Critical_DefaultLoggerException_WriteExceptionCalledOnce()
        {
            this.defaultLogger.Object.Critical(this.exception);
            this.defaultLogger.Verify(x => x.Write(LogLevel.Critical, this.exception), Times.Once);
        }

        [Test]
        public void Critical_DefaultLoggerExceptionDetails_WriteExceptionDetailsCalledOnce()
        {
            this.defaultLogger.Object.Critical(this.exception, this.details);
            this.defaultLogger.Verify(x => x.Write(LogLevel.Critical, this.exception, this.details), Times.Once);
        }

        [Test]
        public void Critical_DefaultNullLoggerMessageException_WriteMessageExceptionCalledNever()
        {
            this.defaultNullLogger.Object.Object.Critical(this.message, this.exception);
            this.defaultNullLogger.Verify(x => x.Object.Write(LogLevel.Critical, this.message, this.exception), Times.Never);
        }

        [Test]
        public void Critical_DefaultNullLoggerMessageExceptionDetails_WriteMessageExceptionDetailsCalledNever()
        {
            this.defaultNullLogger.Object.Object.Critical(this.message, this.exception, this.details);
            this.defaultNullLogger.Verify(x => x.Object.Write(LogLevel.Critical, this.message, this.exception, this.details), Times.Never);
        }

        [Test]
        public void Critical_DefaultLoggerMessageException_WriteMessageExceptionCalledOnce()
        {
            this.defaultLogger.Object.Critical(this.message, this.exception);
            this.defaultLogger.Verify(x => x.Write(LogLevel.Critical, this.message, this.exception), Times.Once);
        }

        [Test]
        public void Critical_DefaultLoggerMessageExceptionDetails_WriteMessageExceptionDetailsCalledOnce()
        {
            this.defaultLogger.Object.Critical(this.message, this.exception, this.details);
            this.defaultLogger.Verify(x => x.Write(LogLevel.Critical, this.message, this.exception, this.details), Times.Once);
        }

        [Test]
        public void Critical_DefaultNullLoggerScopeMessage_WriteMessageCalledNever()
        {
            this.defaultNullLogger.Object.Object.Critical(this.method, this.message);
            this.defaultNullLogger.Verify(x => x.Object.Write(this.method, LogLevel.Critical, this.message), Times.Never);
        }

        [Test]
        public void Critical_DefaultNullLoggerScopeMessageDetails_WriteMessageDetailsCalledNever()
        {
            this.defaultNullLogger.Object.Object.Critical(this.method, this.message, this.details);
            this.defaultNullLogger.Verify(x => x.Object.Write(this.method, LogLevel.Critical, this.message, this.details), Times.Never);
        }

        [Test]
        public void Critical_DefaultLoggerScopeMessage_WriteMessageCalledOnce()
        {
            this.defaultLogger.Object.Critical(this.method, this.message);
            this.defaultLogger.Verify(x => x.Write(this.method, LogLevel.Critical, this.message), Times.Once);
        }

        [Test]
        public void Critical_DefaultLoggerScopeMessageDetails_WriteMessageDetailsCalledOnce()
        {
            this.defaultLogger.Object.Critical(this.method, this.message, this.details);
            this.defaultLogger.Verify(x => x.Write(this.method, LogLevel.Critical, this.message, this.details), Times.Once);
        }

        [Test]
        public void Critical_DefaultNullLoggerScopeException_WriteExceptionCalledNever()
        {
            this.defaultNullLogger.Object.Object.Critical(this.method, this.exception);
            this.defaultNullLogger.Verify(x => x.Object.Write(this.method, LogLevel.Critical, this.exception), Times.Never);
        }

        [Test]
        public void Critical_DefaultNullLoggerScopeExceptionDetails_WriteExceptionDetailsCalledNever()
        {
            this.defaultNullLogger.Object.Object.Critical(this.method, this.exception, this.details);
            this.defaultNullLogger.Verify(x => x.Object.Write(this.method, LogLevel.Critical, this.exception, this.details), Times.Never);
        }

        [Test]
        public void Critical_DefaultLoggerScopeException_WriteExceptionCalledOnce()
        {
            this.defaultLogger.Object.Critical(this.method, this.exception);
            this.defaultLogger.Verify(x => x.Write(this.method, LogLevel.Critical, this.exception), Times.Once);
        }

        [Test]
        public void Critical_DefaultLoggerScopeExceptionDetails_WriteExceptionDetailsCalledOnce()
        {
            this.defaultLogger.Object.Critical(this.method, this.exception, this.details);
            this.defaultLogger.Verify(x => x.Write(this.method, LogLevel.Critical, this.exception, this.details), Times.Once);
        }

        [Test]
        public void Critical_DefaultNullLoggerScopeMessageException_WriteMessageExceptionCalledNever()
        {
            this.defaultNullLogger.Object.Object.Critical(this.method, this.message, this.exception);
            this.defaultNullLogger.Verify(x => x.Object.Write(this.method, LogLevel.Critical, this.message, this.exception), Times.Never);
        }

        [Test]
        public void Critical_DefaultNullLoggerScopeMessageExceptionDetails_WriteMessageExceptionDetailsCalledNever()
        {
            this.defaultNullLogger.Object.Object.Critical(this.method, this.message, this.exception, this.details);
            this.defaultNullLogger.Verify(x => x.Object.Write(this.method, LogLevel.Critical, this.message, this.exception, this.details), Times.Never);
        }

        [Test]
        public void Critical_DefaultLoggerScopeMessageException_WriteMessageExceptionCalledOnce()
        {
            this.defaultLogger.Object.Critical(this.method, this.message, this.exception);
            this.defaultLogger.Verify(x => x.Write(this.method, LogLevel.Critical, this.message, this.exception), Times.Once);
        }

        [Test]
        public void Critical_DefaultLoggerScopeMessageExceptionDetails_WriteMessageExceptionDetailsCalledOnce()
        {
            this.defaultLogger.Object.Critical(this.method, this.message, this.exception, this.details);
            this.defaultLogger.Verify(x => x.Write(this.method, LogLevel.Critical, this.message, this.exception, this.details), Times.Once);
        }

        [Test]
        public void Critical_ContextNullLoggerMessage_WriteMessageCalledNever()
        {
            this.contextNullLogger.Object.Object.Critical(this.message);
            this.contextNullLogger.Verify(x => x.Object.Write(LogLevel.Critical, this.message), Times.Never);
        }

        [Test]
        public void Critical_ContextNullLoggerMessageDetails_WriteMessageDetailsCalledNever()
        {
            this.contextNullLogger.Object.Object.Critical(this.message, this.details);
            this.contextNullLogger.Verify(x => x.Object.Write(LogLevel.Critical, this.message, this.details), Times.Never);
        }

        [Test]
        public void Critical_ContextLoggerValidMessage_WriteMessageCalledOnce()
        {
            this.contextLogger.Object.Critical(this.message);
            this.contextLogger.Verify(x => x.Write(LogLevel.Critical, this.message), Times.Once);
        }

        [Test]
        public void Critical_ContextLoggerValidMessageDetails_WriteMessageDetailsCalledOnce()
        {
            this.contextLogger.Object.Critical(this.message, this.details);
            this.contextLogger.Verify(x => x.Write(LogLevel.Critical, this.message, this.details), Times.Once);
        }

        [Test]
        public void Critical_ContextNullLoggerException_WriteExceptionCalledNever()
        {
            this.contextNullLogger.Object.Object.Critical(this.exception);
            this.contextNullLogger.Verify(x => x.Object.Write(LogLevel.Critical, this.exception), Times.Never);
        }

        [Test]
        public void Critical_ContextNullLoggerExceptionDetails_WriteExceptionDetailsCalledNever()
        {
            this.contextNullLogger.Object.Object.Critical(this.exception, this.details);
            this.contextNullLogger.Verify(x => x.Object.Write(LogLevel.Critical, this.exception, this.details), Times.Never);
        }

        [Test]
        public void Critical_ContextLoggerValidException_WriteExceptionCalledOnce()
        {
            this.contextLogger.Object.Critical(this.exception);
            this.contextLogger.Verify(x => x.Write(LogLevel.Critical, this.exception), Times.Once);
        }

        [Test]
        public void Critical_ContextLoggerValidExceptionDetails_WriteExceptionDetailsCalledOnce()
        {
            this.contextLogger.Object.Critical(this.exception, this.details);
            this.contextLogger.Verify(x => x.Write(LogLevel.Critical, this.exception, this.details), Times.Once);
        }

        [Test]
        public void Critical_ContextNullLoggerMessageException_WriteMessageExceptionCalledNever()
        {
            this.contextNullLogger.Object.Object.Critical(this.message, this.exception);
            this.contextNullLogger.Verify(x => x.Object.Write(LogLevel.Critical, this.message, this.exception), Times.Never);
        }

        [Test]
        public void Critical_ContextNullLoggerMessageExceptionDetails_WriteMessageExceptionDetailsCalledNever()
        {
            this.contextNullLogger.Object.Object.Critical(this.message, this.exception, this.details);
            this.contextNullLogger.Verify(x => x.Object.Write(LogLevel.Critical, this.message, this.exception, this.details), Times.Never);
        }

        [Test]
        public void Critical_ContextLoggerValidMessageException_WriteMessageExceptionCalledOnce()
        {
            this.contextLogger.Object.Critical(this.message, this.exception);
            this.contextLogger.Verify(x => x.Write(LogLevel.Critical, this.message, this.exception), Times.Once);
        }

        [Test]
        public void Critical_ContextLoggerValidMessageExceptionDetails_WriteMessageExceptionDetailsCalledOnce()
        {
            this.contextLogger.Object.Critical(this.message, this.exception, this.details);
            this.contextLogger.Verify(x => x.Write(LogLevel.Critical, this.message, this.exception, this.details), Times.Once);
        }

        [Test]
        public void Critical_ContextNullLoggerScopeMessage_WriteMessageCalledNever()
        {
            this.contextNullLogger.Object.Object.Critical(this.method, this.message);
            this.contextNullLogger.Verify(x => x.Object.Write(this.method, LogLevel.Critical, this.message), Times.Never);
        }

        [Test]
        public void Critical_ContextNullLoggerScopeMessageDetails_WriteMessageDetailsCalledNever()
        {
            this.contextNullLogger.Object.Object.Critical(this.method, this.message, this.details);
            this.contextNullLogger.Verify(x => x.Object.Write(this.method, LogLevel.Critical, this.message, this.details), Times.Never);
        }

        [Test]
        public void Critical_ContextLoggerScopeMessage_WriteMessageCalledOnce()
        {
            this.contextLogger.Object.Critical(this.method, this.message);
            this.contextLogger.Verify(x => x.Write(this.method, LogLevel.Critical, this.message), Times.Once);
        }

        [Test]
        public void Critical_ContextLoggerScopeMessageDetails_WriteMessageDetailsCalledOnce()
        {
            this.contextLogger.Object.Critical(this.method, this.message, this.details);
            this.contextLogger.Verify(x => x.Write(this.method, LogLevel.Critical, this.message, this.details), Times.Once);
        }

        [Test]
        public void Critical_ContextNullLoggerScopeException_WriteExceptionCalledNever()
        {
            this.contextNullLogger.Object.Object.Critical(this.method, this.exception);
            this.contextNullLogger.Verify(x => x.Object.Write(this.method, LogLevel.Critical, this.exception), Times.Never);
        }

        [Test]
        public void Critical_ContextNullLoggerScopeExceptionDetails_WriteExceptionDetailsCalledNever()
        {
            this.contextNullLogger.Object.Object.Critical(this.method, this.exception, this.details);
            this.contextNullLogger.Verify(x => x.Object.Write(this.method, LogLevel.Critical, this.exception, this.details), Times.Never);
        }

        [Test]
        public void Critical_ContextLoggerScopeException_WriteExceptionCalledOnce()
        {
            this.contextLogger.Object.Critical(this.method, this.exception);
            this.contextLogger.Verify(x => x.Write(this.method, LogLevel.Critical, this.exception), Times.Once);
        }

        [Test]
        public void Critical_ContextLoggerScopeExceptionDetails_WriteExceptionDetailsCalledOnce()
        {
            this.contextLogger.Object.Critical(this.method, this.exception, this.details);
            this.contextLogger.Verify(x => x.Write(this.method, LogLevel.Critical, this.exception, this.details), Times.Once);
        }

        [Test]
        public void Critical_ContextNullLoggerScopeMessageException_WriteMessageExceptionCalledNever()
        {
            this.contextNullLogger.Object.Object.Critical(this.method, this.message, this.exception);
            this.contextNullLogger.Verify(x => x.Object.Write(this.method, LogLevel.Critical, this.message, this.exception), Times.Never);
        }

        [Test]
        public void Critical_ContextNullLoggerScopeMessageExceptionDetails_WriteMessageExceptionDetailsCalledNever()
        {
            this.contextNullLogger.Object.Object.Critical(this.method, this.message, this.exception, this.details);
            this.contextNullLogger.Verify(x => x.Object.Write(this.method, LogLevel.Critical, this.message, this.exception, this.details), Times.Never);
        }

        [Test]
        public void Critical_ContextLoggerScopeMessageException_WriteMessageExceptionCalledOnce()
        {
            this.contextLogger.Object.Critical(this.method, this.message, this.exception);
            this.contextLogger.Verify(x => x.Write(this.method, LogLevel.Critical, this.message, this.exception), Times.Once);
        }

        [Test]
        public void Critical_ContextLoggerScopeMessageExceptionDetails_WriteMessageExceptionDetailsCalledOnce()
        {
            this.contextLogger.Object.Critical(this.method, this.message, this.exception, this.details);
            this.contextLogger.Verify(x => x.Write(this.method, LogLevel.Critical, this.message, this.exception, this.details), Times.Once);
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
