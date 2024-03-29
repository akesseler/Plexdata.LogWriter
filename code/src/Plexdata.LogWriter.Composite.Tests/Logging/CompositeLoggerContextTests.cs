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
using Plexdata.LogWriter.Abstraction;
using Plexdata.LogWriter.Definitions;
using Plexdata.LogWriter.Logging;
using Plexdata.Utilities.Testing;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Threading;

namespace Plexdata.LogWriter.Composite.Tests.Logging
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    [Category(TestType.UnitTest)]
    [TestOf(nameof(CompositeLogger<Object>))]
    public class CompositeLoggerContextTests
    {
        #region Prologue

        private Mock<IEmptyLogger<Object>> emptyLogger1 = null;
        private Mock<IEmptyLogger<Object>> emptyLogger2 = null;

        [SetUp]
        public void Setup()
        {
            this.emptyLogger1 = new Mock<IEmptyLogger<Object>>();
            this.emptyLogger2 = new Mock<IEmptyLogger<Object>>();
        }

        #endregion

        #region Construction

        [Test]
        public void CompositeLogger_DefaultConstruction_ThrowsNothing()
        {
            Assert.That(() => new CompositeLogger<Object>(), Throws.Nothing);
        }

        [Test]
        public void CompositeLogger_DefaultConstruction_DefaultSettingsAsExpected()
        {
            CompositeLogger<Object> instance = new CompositeLogger<Object>();

            Assert.That(this.GetPropertyValueOf<ICompositeLoggerSettings>(instance, "Settings").LogLevel, Is.EqualTo(LogLevel.Trace));
        }

        [Test]
        public void CompositeLogger_DefaultConstruction_NoLoggersAssigned()
        {
            Assert.That(this.GetFieldValueOf<IList<ILogger<Object>>>(new CompositeLogger<Object>(), "loggers"), Is.Empty);
        }

        #endregion

        #region Add Logger

        [Test]
        public void AddLogger_InstanceSuitableLoggersIdentical_AssignedLoggerCountAsExpected([Values(1, 2, 3)] Int32 count)
        {
            CompositeLogger<Object> instance = new CompositeLogger<Object>();

            Dummy1<Object> logger = new Dummy1<Object>();

            switch (count)
            {
                case 1:
                    instance.AddLogger(logger);
                    break;
                case 2:
                    instance.AddLogger(logger);
                    instance.AddLogger(logger);
                    break;
                case 3:
                    instance.AddLogger(logger);
                    instance.AddLogger(logger);
                    instance.AddLogger(logger);
                    break;
            }

            IList<ILogger<Object>> actual = this.GetFieldValueOf<IList<ILogger<Object>>>(instance, "loggers");

            Assert.That(actual.Count, Is.EqualTo(1));
        }

        [Test]
        public void AddLogger_InstanceSuitableLoggersDifferent_AssignedLoggerCountAsExpected([Values(0, 1, 2, 3)] Int32 count)
        {
            CompositeLogger<Object> instance = new CompositeLogger<Object>();

            switch (count)
            {
                case 0:
                    instance.AddLogger(null);
                    break;
                case 1:
                    instance.AddLogger(new Dummy1<Object>());
                    break;
                case 2:
                    instance.AddLogger(new Dummy1<Object>());
                    instance.AddLogger(new Dummy2<Object>());
                    break;
                case 3:
                    instance.AddLogger(new Dummy1<Object>());
                    instance.AddLogger(new Dummy2<Object>());
                    instance.AddLogger(new Dummy3<Object>());
                    break;
            }

            IList<ILogger<Object>> actual = this.GetFieldValueOf<IList<ILogger<Object>>>(instance, "loggers");

            Assert.That(actual.Count, Is.EqualTo(count));
        }

        [Test]
        public void AddLogger_InstanceDisposedLoggersIdentical_AssignedLoggerCountAsExpected([Values(1, 2, 3)] Int32 count)
        {
            CompositeLogger<Object> instance = new CompositeLogger<Object>();
            instance.Dispose();

            Dummy1<Object> logger = new Dummy1<Object>();

            switch (count)
            {
                case 1:
                    instance.AddLogger(logger);
                    break;
                case 2:
                    instance.AddLogger(logger);
                    instance.AddLogger(logger);
                    break;
                case 3:
                    instance.AddLogger(logger);
                    instance.AddLogger(logger);
                    instance.AddLogger(logger);
                    break;
            }

            IList<ILogger<Object>> actual = this.GetFieldValueOf<IList<ILogger<Object>>>(instance, "loggers");

            Assert.That(actual.Count, Is.Zero);
        }

        [Test]
        public void AddLogger_InstanceDisposedLoggersDifferent_AssignedLoggerCountAsExpected([Values(0, 1, 2, 3)] Int32 count)
        {
            CompositeLogger<Object> instance = new CompositeLogger<Object>();
            instance.Dispose();

            switch (count)
            {
                case 0:
                    instance.AddLogger(null);
                    break;
                case 1:
                    instance.AddLogger(new Dummy1<Object>());
                    break;
                case 2:
                    instance.AddLogger(new Dummy1<Object>());
                    instance.AddLogger(new Dummy2<Object>());
                    break;
                case 3:
                    instance.AddLogger(new Dummy1<Object>());
                    instance.AddLogger(new Dummy2<Object>());
                    instance.AddLogger(new Dummy3<Object>());
                    break;
            }

            IList<ILogger<Object>> actual = this.GetFieldValueOf<IList<ILogger<Object>>>(instance, "loggers");

            Assert.That(actual.Count, Is.Zero);
        }

        #endregion

        #region Write simple

        [Test]
        public void WriteSimple_OneLoggerLevelMessage_EachWriteWasCalledOnce()
        {
            CompositeLogger<Object> instance = new CompositeLogger<Object>();

            this.emptyLogger1.Setup(x => x.Write(It.IsAny<LogLevel>(), It.IsAny<String>()));

            instance.AddLogger(this.emptyLogger1.Object);

            instance.Write(LogLevel.Trace, "message");

            Thread.Sleep(100); // Increase waiting time if test fails.

            this.emptyLogger1.Verify(x => x.Write(It.IsAny<LogLevel>(), It.IsAny<String>()), Times.Once);
        }

        [Test]
        public void WriteSimple_TwoLoggerLevelMessage_EachWriteWasCalledOnce()
        {
            CompositeLogger<Object> instance = new CompositeLogger<Object>();

            this.emptyLogger1.Setup(x => x.Write(It.IsAny<LogLevel>(), It.IsAny<String>()));
            this.emptyLogger2.Setup(x => x.Write(It.IsAny<LogLevel>(), It.IsAny<String>()));

            instance.AddLogger(this.emptyLogger1.Object);
            instance.AddLogger(this.emptyLogger2.Object);

            instance.Write(LogLevel.Trace, "message");

            Thread.Sleep(100); // Increase waiting time if test fails.

            this.emptyLogger1.Verify(x => x.Write(It.IsAny<LogLevel>(), It.IsAny<String>()), Times.Once);
            this.emptyLogger2.Verify(x => x.Write(It.IsAny<LogLevel>(), It.IsAny<String>()), Times.Once);
        }

        [Test]
        public void WriteSimple_OneLoggerLevelMessageDetails_EachWriteWasCalledOnce()
        {
            CompositeLogger<Object> instance = new CompositeLogger<Object>();

            this.emptyLogger1.Setup(x => x.Write(It.IsAny<LogLevel>(), It.IsAny<String>(), It.IsAny<(String Label, Object Value)[]>()));

            instance.AddLogger(this.emptyLogger1.Object);

            instance.Write(LogLevel.Trace, "message", (Label: "label", Value: "value"));

            Thread.Sleep(100); // Increase waiting time if test fails.

            this.emptyLogger1.Verify(x => x.Write(It.IsAny<LogLevel>(), It.IsAny<String>(), It.IsAny<(String Label, Object Value)[]>()), Times.Once);
        }

        [Test]
        public void WriteSimple_TwoLoggerLevelMessageDetails_EachWriteWasCalledOnce()
        {
            CompositeLogger<Object> instance = new CompositeLogger<Object>();

            this.emptyLogger1.Setup(x => x.Write(It.IsAny<LogLevel>(), It.IsAny<String>(), It.IsAny<(String Label, Object Value)[]>()));
            this.emptyLogger2.Setup(x => x.Write(It.IsAny<LogLevel>(), It.IsAny<String>(), It.IsAny<(String Label, Object Value)[]>()));

            instance.AddLogger(this.emptyLogger1.Object);
            instance.AddLogger(this.emptyLogger2.Object);

            instance.Write(LogLevel.Trace, "message", (Label: "label", Value: "value"));

            Thread.Sleep(100); // Increase waiting time if test fails.

            this.emptyLogger1.Verify(x => x.Write(It.IsAny<LogLevel>(), It.IsAny<String>(), It.IsAny<(String Label, Object Value)[]>()), Times.Once);
            this.emptyLogger2.Verify(x => x.Write(It.IsAny<LogLevel>(), It.IsAny<String>(), It.IsAny<(String Label, Object Value)[]>()), Times.Once);
        }

        [Test]
        public void WriteSimple_OneLoggerLevelException_EachWriteWasCalledOnce()
        {
            CompositeLogger<Object> instance = new CompositeLogger<Object>();

            this.emptyLogger1.Setup(x => x.Write(It.IsAny<LogLevel>(), It.IsAny<Exception>()));

            instance.AddLogger(this.emptyLogger1.Object);

            instance.Write(LogLevel.Trace, new NotSupportedException());

            Thread.Sleep(100); // Increase waiting time if test fails.

            this.emptyLogger1.Verify(x => x.Write(It.IsAny<LogLevel>(), It.IsAny<Exception>()), Times.Once);
        }

        [Test]
        public void WriteSimple_TwoLoggerLevelException_EachWriteWasCalledOnce()
        {
            CompositeLogger<Object> instance = new CompositeLogger<Object>();

            this.emptyLogger1.Setup(x => x.Write(It.IsAny<LogLevel>(), It.IsAny<Exception>()));
            this.emptyLogger2.Setup(x => x.Write(It.IsAny<LogLevel>(), It.IsAny<Exception>()));

            instance.AddLogger(this.emptyLogger1.Object);
            instance.AddLogger(this.emptyLogger2.Object);

            instance.Write(LogLevel.Trace, new NotSupportedException());

            Thread.Sleep(100); // Increase waiting time if test fails.

            this.emptyLogger1.Verify(x => x.Write(It.IsAny<LogLevel>(), It.IsAny<Exception>()), Times.Once);
            this.emptyLogger2.Verify(x => x.Write(It.IsAny<LogLevel>(), It.IsAny<Exception>()), Times.Once);
        }

        [Test]
        public void WriteSimple_OneLoggerLevelExceptionDetails_EachWriteWasCalledOnce()
        {
            CompositeLogger<Object> instance = new CompositeLogger<Object>();

            this.emptyLogger1.Setup(x => x.Write(It.IsAny<LogLevel>(), It.IsAny<Exception>(), It.IsAny<(String Label, Object Value)[]>()));

            instance.AddLogger(this.emptyLogger1.Object);

            instance.Write(LogLevel.Trace, new NotSupportedException(), (Label: "label", Value: "value"));

            Thread.Sleep(100); // Increase waiting time if test fails.

            this.emptyLogger1.Verify(x => x.Write(It.IsAny<LogLevel>(), It.IsAny<Exception>(), It.IsAny<(String Label, Object Value)[]>()), Times.Once);
        }

        [Test]
        public void WriteSimple_TwoLoggerLevelExceptionDetails_EachWriteWasCalledOnce()
        {
            CompositeLogger<Object> instance = new CompositeLogger<Object>();

            this.emptyLogger1.Setup(x => x.Write(It.IsAny<LogLevel>(), It.IsAny<Exception>(), It.IsAny<(String Label, Object Value)[]>()));
            this.emptyLogger2.Setup(x => x.Write(It.IsAny<LogLevel>(), It.IsAny<Exception>(), It.IsAny<(String Label, Object Value)[]>()));

            instance.AddLogger(this.emptyLogger1.Object);
            instance.AddLogger(this.emptyLogger2.Object);

            instance.Write(LogLevel.Trace, new NotSupportedException(), (Label: "label", Value: "value"));

            Thread.Sleep(100); // Increase waiting time if test fails.

            this.emptyLogger1.Verify(x => x.Write(It.IsAny<LogLevel>(), It.IsAny<Exception>(), It.IsAny<(String Label, Object Value)[]>()), Times.Once);
            this.emptyLogger2.Verify(x => x.Write(It.IsAny<LogLevel>(), It.IsAny<Exception>(), It.IsAny<(String Label, Object Value)[]>()), Times.Once);
        }

        [Test]
        public void WriteSimple_OneLoggerLevelMessageException_EachWriteWasCalledOnce()
        {
            CompositeLogger<Object> instance = new CompositeLogger<Object>();

            this.emptyLogger1.Setup(x => x.Write(It.IsAny<LogLevel>(), It.IsAny<String>(), It.IsAny<Exception>()));

            instance.AddLogger(this.emptyLogger1.Object);

            instance.Write(LogLevel.Trace, "message", new NotSupportedException());

            Thread.Sleep(100); // Increase waiting time if test fails.

            this.emptyLogger1.Verify(x => x.Write(It.IsAny<LogLevel>(), It.IsAny<String>(), It.IsAny<Exception>()), Times.Once);
        }

        [Test]
        public void WriteSimple_TwoLoggerLevelMessageException_EachWriteWasCalledOnce()
        {
            CompositeLogger<Object> instance = new CompositeLogger<Object>();

            this.emptyLogger1.Setup(x => x.Write(It.IsAny<LogLevel>(), It.IsAny<String>(), It.IsAny<Exception>()));
            this.emptyLogger2.Setup(x => x.Write(It.IsAny<LogLevel>(), It.IsAny<String>(), It.IsAny<Exception>()));

            instance.AddLogger(this.emptyLogger1.Object);
            instance.AddLogger(this.emptyLogger2.Object);

            instance.Write(LogLevel.Trace, "message", new NotSupportedException());

            Thread.Sleep(100); // Increase waiting time if test fails.

            this.emptyLogger1.Verify(x => x.Write(It.IsAny<LogLevel>(), It.IsAny<String>(), It.IsAny<Exception>()), Times.Once);
            this.emptyLogger2.Verify(x => x.Write(It.IsAny<LogLevel>(), It.IsAny<String>(), It.IsAny<Exception>()), Times.Once);
        }

        [Test]
        public void WriteSimple_OneLoggerLevelMessageExceptionDetails_EachWriteWasCalledOnce()
        {
            CompositeLogger<Object> instance = new CompositeLogger<Object>();

            this.emptyLogger1.Setup(x => x.Write(It.IsAny<LogLevel>(), It.IsAny<String>(), It.IsAny<Exception>(), It.IsAny<(String Label, Object Value)[]>()));

            instance.AddLogger(this.emptyLogger1.Object);

            instance.Write(LogLevel.Trace, "message", new NotSupportedException(), (Label: "label", Value: "value"));

            Thread.Sleep(100); // Increase waiting time if test fails.

            this.emptyLogger1.Verify(x => x.Write(It.IsAny<LogLevel>(), It.IsAny<String>(), It.IsAny<Exception>(), It.IsAny<(String Label, Object Value)[]>()), Times.Once);
        }

        [Test]
        public void WriteSimple_TwoLoggerLevelMessageExceptionDetails_EachWriteWasCalledOnce()
        {
            CompositeLogger<Object> instance = new CompositeLogger<Object>();

            this.emptyLogger1.Setup(x => x.Write(It.IsAny<LogLevel>(), It.IsAny<String>(), It.IsAny<Exception>(), It.IsAny<(String Label, Object Value)[]>()));
            this.emptyLogger2.Setup(x => x.Write(It.IsAny<LogLevel>(), It.IsAny<String>(), It.IsAny<Exception>(), It.IsAny<(String Label, Object Value)[]>()));

            instance.AddLogger(this.emptyLogger1.Object);
            instance.AddLogger(this.emptyLogger2.Object);

            instance.Write(LogLevel.Trace, "message", new NotSupportedException(), (Label: "label", Value: "value"));

            Thread.Sleep(100); // Increase waiting time if test fails.

            this.emptyLogger1.Verify(x => x.Write(It.IsAny<LogLevel>(), It.IsAny<String>(), It.IsAny<Exception>(), It.IsAny<(String Label, Object Value)[]>()), Times.Once);
            this.emptyLogger2.Verify(x => x.Write(It.IsAny<LogLevel>(), It.IsAny<String>(), It.IsAny<Exception>(), It.IsAny<(String Label, Object Value)[]>()), Times.Once);
        }

        #endregion

        #region Write scope

        [Test]
        public void WriteScope_OneLoggerLevelMessage_EachWriteWasCalledOnce()
        {
            CompositeLogger<Object> instance = new CompositeLogger<Object>();

            this.emptyLogger1.Setup(x => x.Write(It.IsAny<String>(), It.IsAny<LogLevel>(), It.IsAny<String>()));

            instance.AddLogger(this.emptyLogger1.Object);

            instance.Write<String>("scope", LogLevel.Trace, "message");

            Thread.Sleep(100); // Increase waiting time if test fails.

            this.emptyLogger1.Verify(x => x.Write(It.IsAny<String>(), It.IsAny<LogLevel>(), It.IsAny<String>()), Times.Once);
        }

        [Test]
        public void WriteScope_TwoLoggerLevelMessage_EachWriteWasCalledOnce()
        {
            CompositeLogger<Object> instance = new CompositeLogger<Object>();

            this.emptyLogger1.Setup(x => x.Write(It.IsAny<String>(), It.IsAny<LogLevel>(), It.IsAny<String>()));
            this.emptyLogger2.Setup(x => x.Write(It.IsAny<String>(), It.IsAny<LogLevel>(), It.IsAny<String>()));

            instance.AddLogger(this.emptyLogger1.Object);
            instance.AddLogger(this.emptyLogger2.Object);

            instance.Write<String>("scope", LogLevel.Trace, "message");

            Thread.Sleep(100); // Increase waiting time if test fails.

            this.emptyLogger1.Verify(x => x.Write(It.IsAny<String>(), It.IsAny<LogLevel>(), It.IsAny<String>()), Times.Once);
            this.emptyLogger2.Verify(x => x.Write(It.IsAny<String>(), It.IsAny<LogLevel>(), It.IsAny<String>()), Times.Once);
        }

        [Test]
        public void WriteScope_OneLoggerLevelMessageDetails_EachWriteWasCalledOnce()
        {
            CompositeLogger<Object> instance = new CompositeLogger<Object>();

            this.emptyLogger1.Setup(x => x.Write(It.IsAny<String>(), It.IsAny<LogLevel>(), It.IsAny<String>(), It.IsAny<(String Label, Object Value)[]>()));

            instance.AddLogger(this.emptyLogger1.Object);

            instance.Write<String>("scope", LogLevel.Trace, "message", (Label: "label", Value: "value"));

            Thread.Sleep(100); // Increase waiting time if test fails.

            this.emptyLogger1.Verify(x => x.Write(It.IsAny<String>(), It.IsAny<LogLevel>(), It.IsAny<String>(), It.IsAny<(String Label, Object Value)[]>()), Times.Once);
        }

        [Test]
        public void WriteScope_TwoLoggerLevelMessageDetails_EachWriteWasCalledOnce()
        {
            CompositeLogger<Object> instance = new CompositeLogger<Object>();

            this.emptyLogger1.Setup(x => x.Write(It.IsAny<String>(), It.IsAny<LogLevel>(), It.IsAny<String>(), It.IsAny<(String Label, Object Value)[]>()));
            this.emptyLogger2.Setup(x => x.Write(It.IsAny<String>(), It.IsAny<LogLevel>(), It.IsAny<String>(), It.IsAny<(String Label, Object Value)[]>()));

            instance.AddLogger(this.emptyLogger1.Object);
            instance.AddLogger(this.emptyLogger2.Object);

            instance.Write<String>("scope", LogLevel.Trace, "message", (Label: "label", Value: "value"));

            Thread.Sleep(100); // Increase waiting time if test fails.

            this.emptyLogger1.Verify(x => x.Write(It.IsAny<String>(), It.IsAny<LogLevel>(), It.IsAny<String>(), It.IsAny<(String Label, Object Value)[]>()), Times.Once);
            this.emptyLogger2.Verify(x => x.Write(It.IsAny<String>(), It.IsAny<LogLevel>(), It.IsAny<String>(), It.IsAny<(String Label, Object Value)[]>()), Times.Once);
        }

        [Test]
        public void WriteScope_OneLoggerLevelException_EachWriteWasCalledOnce()
        {
            CompositeLogger<Object> instance = new CompositeLogger<Object>();

            this.emptyLogger1.Setup(x => x.Write(It.IsAny<String>(), It.IsAny<LogLevel>(), It.IsAny<Exception>()));

            instance.AddLogger(this.emptyLogger1.Object);

            instance.Write<String>("scope", LogLevel.Trace, new NotSupportedException());

            Thread.Sleep(100); // Increase waiting time if test fails.

            this.emptyLogger1.Verify(x => x.Write(It.IsAny<String>(), It.IsAny<LogLevel>(), It.IsAny<Exception>()), Times.Once);
        }

        [Test]
        public void WriteScope_TwoLoggerLevelException_EachWriteWasCalledOnce()
        {
            CompositeLogger<Object> instance = new CompositeLogger<Object>();

            this.emptyLogger1.Setup(x => x.Write(It.IsAny<String>(), It.IsAny<LogLevel>(), It.IsAny<Exception>()));
            this.emptyLogger2.Setup(x => x.Write(It.IsAny<String>(), It.IsAny<LogLevel>(), It.IsAny<Exception>()));

            instance.AddLogger(this.emptyLogger1.Object);
            instance.AddLogger(this.emptyLogger2.Object);

            instance.Write<String>("scope", LogLevel.Trace, new NotSupportedException());

            Thread.Sleep(100); // Increase waiting time if test fails.

            this.emptyLogger1.Verify(x => x.Write(It.IsAny<String>(), It.IsAny<LogLevel>(), It.IsAny<Exception>()), Times.Once);
            this.emptyLogger2.Verify(x => x.Write(It.IsAny<String>(), It.IsAny<LogLevel>(), It.IsAny<Exception>()), Times.Once);
        }

        [Test]
        public void WriteScope_OneLoggerLevelExceptionDetails_EachWriteWasCalledOnce()
        {
            CompositeLogger<Object> instance = new CompositeLogger<Object>();

            this.emptyLogger1.Setup(x => x.Write(It.IsAny<String>(), It.IsAny<LogLevel>(), It.IsAny<Exception>(), It.IsAny<(String Label, Object Value)[]>()));

            instance.AddLogger(this.emptyLogger1.Object);

            instance.Write<String>("scope", LogLevel.Trace, new NotSupportedException(), (Label: "label", Value: "value"));

            Thread.Sleep(100); // Increase waiting time if test fails.

            this.emptyLogger1.Verify(x => x.Write(It.IsAny<String>(), It.IsAny<LogLevel>(), It.IsAny<Exception>(), It.IsAny<(String Label, Object Value)[]>()), Times.Once);
        }

        [Test]
        public void WriteScope_TwoLoggerLevelExceptionDetails_EachWriteWasCalledOnce()
        {
            CompositeLogger<Object> instance = new CompositeLogger<Object>();

            this.emptyLogger1.Setup(x => x.Write(It.IsAny<String>(), It.IsAny<LogLevel>(), It.IsAny<Exception>(), It.IsAny<(String Label, Object Value)[]>()));
            this.emptyLogger2.Setup(x => x.Write(It.IsAny<String>(), It.IsAny<LogLevel>(), It.IsAny<Exception>(), It.IsAny<(String Label, Object Value)[]>()));

            instance.AddLogger(this.emptyLogger1.Object);
            instance.AddLogger(this.emptyLogger2.Object);

            instance.Write<String>("scope", LogLevel.Trace, new NotSupportedException(), (Label: "label", Value: "value"));

            Thread.Sleep(100); // Increase waiting time if test fails.

            this.emptyLogger1.Verify(x => x.Write(It.IsAny<String>(), It.IsAny<LogLevel>(), It.IsAny<Exception>(), It.IsAny<(String Label, Object Value)[]>()), Times.Once);
            this.emptyLogger2.Verify(x => x.Write(It.IsAny<String>(), It.IsAny<LogLevel>(), It.IsAny<Exception>(), It.IsAny<(String Label, Object Value)[]>()), Times.Once);
        }

        [Test]
        public void WriteScope_OneLoggerLevelMessageException_EachWriteWasCalledOnce()
        {
            CompositeLogger<Object> instance = new CompositeLogger<Object>();

            this.emptyLogger1.Setup(x => x.Write(It.IsAny<String>(), It.IsAny<LogLevel>(), It.IsAny<String>(), It.IsAny<Exception>()));

            instance.AddLogger(this.emptyLogger1.Object);

            instance.Write<String>("scope", LogLevel.Trace, "message", new NotSupportedException());

            Thread.Sleep(100); // Increase waiting time if test fails.

            this.emptyLogger1.Verify(x => x.Write(It.IsAny<String>(), It.IsAny<LogLevel>(), It.IsAny<String>(), It.IsAny<Exception>()), Times.Once);
        }

        [Test]
        public void WriteScope_TwoLoggerLevelMessageException_EachWriteWasCalledOnce()
        {
            CompositeLogger<Object> instance = new CompositeLogger<Object>();

            this.emptyLogger1.Setup(x => x.Write(It.IsAny<String>(), It.IsAny<LogLevel>(), It.IsAny<String>(), It.IsAny<Exception>()));
            this.emptyLogger2.Setup(x => x.Write(It.IsAny<String>(), It.IsAny<LogLevel>(), It.IsAny<String>(), It.IsAny<Exception>()));

            instance.AddLogger(this.emptyLogger1.Object);
            instance.AddLogger(this.emptyLogger2.Object);

            instance.Write<String>("scope", LogLevel.Trace, "message", new NotSupportedException());

            Thread.Sleep(100); // Increase waiting time if test fails.

            this.emptyLogger1.Verify(x => x.Write(It.IsAny<String>(), It.IsAny<LogLevel>(), It.IsAny<String>(), It.IsAny<Exception>()), Times.Once);
            this.emptyLogger2.Verify(x => x.Write(It.IsAny<String>(), It.IsAny<LogLevel>(), It.IsAny<String>(), It.IsAny<Exception>()), Times.Once);
        }

        [Test]
        public void WriteScope_OneLoggerLevelMessageExceptionDetails_EachWriteWasCalledOnce()
        {
            CompositeLogger<Object> instance = new CompositeLogger<Object>();

            this.emptyLogger1.Setup(x => x.Write(It.IsAny<String>(), It.IsAny<LogLevel>(), It.IsAny<String>(), It.IsAny<Exception>(), It.IsAny<(String Label, Object Value)[]>()));

            instance.AddLogger(this.emptyLogger1.Object);

            instance.Write<String>("scope", LogLevel.Trace, "message", new NotSupportedException(), (Label: "label", Value: "value"));

            Thread.Sleep(100); // Increase waiting time if test fails.

            this.emptyLogger1.Verify(x => x.Write(It.IsAny<String>(), It.IsAny<LogLevel>(), It.IsAny<String>(), It.IsAny<Exception>(), It.IsAny<(String Label, Object Value)[]>()), Times.Once);
        }

        [Test]
        public void WriteScope_TwoLoggerLevelMessageExceptionDetails_EachWriteWasCalledOnce()
        {
            CompositeLogger<Object> instance = new CompositeLogger<Object>();

            this.emptyLogger1.Setup(x => x.Write(It.IsAny<String>(), It.IsAny<LogLevel>(), It.IsAny<String>(), It.IsAny<Exception>(), It.IsAny<(String Label, Object Value)[]>()));
            this.emptyLogger2.Setup(x => x.Write(It.IsAny<String>(), It.IsAny<LogLevel>(), It.IsAny<String>(), It.IsAny<Exception>(), It.IsAny<(String Label, Object Value)[]>()));

            instance.AddLogger(this.emptyLogger1.Object);
            instance.AddLogger(this.emptyLogger2.Object);

            instance.Write<String>("scope", LogLevel.Trace, "message", new NotSupportedException(), (Label: "label", Value: "value"));

            Thread.Sleep(100); // Increase waiting time if test fails.

            this.emptyLogger1.Verify(x => x.Write(It.IsAny<String>(), It.IsAny<LogLevel>(), It.IsAny<String>(), It.IsAny<Exception>(), It.IsAny<(String Label, Object Value)[]>()), Times.Once);
            this.emptyLogger2.Verify(x => x.Write(It.IsAny<String>(), It.IsAny<LogLevel>(), It.IsAny<String>(), It.IsAny<Exception>(), It.IsAny<(String Label, Object Value)[]>()), Times.Once);
        }

        #endregion

        #region Private stuff

        private TType GetPropertyValueOf<TType>(CompositeLogger<Object> instance, String name)
        {
            PropertyInfo info = instance.GetType().GetProperty(name, BindingFlags.Instance | BindingFlags.NonPublic);

            if (info != null)
            {
                return (TType)info.GetValue(instance);
            }

            return default(TType);
        }

        private TType GetFieldValueOf<TType>(CompositeLogger<Object> instance, String name)
        {
            FieldInfo info = instance.GetType().GetField(name, BindingFlags.Instance | BindingFlags.NonPublic);

            if (info != null)
            {
                return (TType)info.GetValue(instance);
            }

            return default(TType);
        }

        private interface IDummy1<Object> : IEmptyLogger<Object> { }
        private interface IDummy2<Object> : IEmptyLogger<Object> { }
        private interface IDummy3<Object> : IEmptyLogger<Object> { }

        private class Dummy1<Object> : EmptyLogger, IDummy1<Object> { }
        private class Dummy2<Object> : EmptyLogger, IDummy2<Object> { }
        private class Dummy3<Object> : EmptyLogger, IDummy3<Object> { }

        #endregion
    }
}
