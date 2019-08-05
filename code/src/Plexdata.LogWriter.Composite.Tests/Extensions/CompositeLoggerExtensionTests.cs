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
using Plexdata.LogWriter.Extensions;
using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace Plexdata.LogWriter.Composite.Tests.Extensions
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    [TestOf(nameof(CompositeLoggerExtension))]
    public class CompositeLoggerExtensionTests
    {
        #region Prologue

        private class DefaultNullMock : Mock<ICompositeLogger>
        {
            public override ICompositeLogger Object
            {
                get
                {
                    return null;
                }
            }
        }

        private class DefaultInstMock : Mock<ICompositeLogger>
        {
            public override ICompositeLogger Object
            {
                get
                {
                    return base.Object;
                }
            }
        }

        private class ContextNullMock : Mock<ICompositeLogger<Object>>
        {
            public override ICompositeLogger<Object> Object
            {
                get
                {
                    return null;
                }
            }
        }

        private class ContextInstMock : Mock<ICompositeLogger<Object>>
        {
            public override ICompositeLogger<Object> Object
            {
                get
                {
                    return base.Object;
                }
            }
        }

        private DefaultNullMock defaultNullMock = null;
        private DefaultInstMock defaultInstMock = null;
        private ContextNullMock contextNullMock = null;
        private ContextInstMock contextInstMock = null;

        private Mock<ILoggerSettingsSection> loggerSettingsSection;

        private Mock<IConsoleLoggerSettings> consoleLoggerSettings;
        private Mock<IConsoleLogger> defaultConsoleLogger;
        private Mock<IConsoleLogger<Object>> contextConsoleLogger;

        private Mock<IPersistentLoggerSettings> persistentLoggerSettings;
        private Mock<IPersistentLogger> defaultPersistentLogger;
        private Mock<IPersistentLogger<Object>> contextPersistentLogger;

        private Mock<IStreamLoggerSettings> streamLoggerSettings;
        private Mock<IStreamLogger> defaultStreamLogger;
        private Mock<IStreamLogger<Object>> contextStreamLogger;

        [SetUp]
        public void Setup()
        {
            this.defaultNullMock = new DefaultNullMock();
            this.defaultInstMock = new DefaultInstMock();
            this.contextNullMock = new ContextNullMock();
            this.contextInstMock = new ContextInstMock();

            this.loggerSettingsSection = new Mock<ILoggerSettingsSection>();
            this.loggerSettingsSection.SetupAllProperties();
            this.loggerSettingsSection.Setup(x => x.GetSection(It.IsAny<String>())).Returns(this.loggerSettingsSection.Object);

            this.consoleLoggerSettings = new Mock<IConsoleLoggerSettings>();
            this.consoleLoggerSettings.SetupAllProperties();
            this.consoleLoggerSettings.SetupGet(x => x.BufferSize).Returns(new Dimension(10, 10));

            this.defaultConsoleLogger = new Mock<IConsoleLogger>();
            this.contextConsoleLogger = new Mock<IConsoleLogger<Object>>();

            this.persistentLoggerSettings = new Mock<IPersistentLoggerSettings>();
            this.persistentLoggerSettings.SetupAllProperties();

            this.defaultPersistentLogger = new Mock<IPersistentLogger>();
            this.contextPersistentLogger = new Mock<IPersistentLogger<Object>>();

            this.streamLoggerSettings = new Mock<IStreamLoggerSettings>();
            this.defaultStreamLogger = new Mock<IStreamLogger>();
            this.contextStreamLogger = new Mock<IStreamLogger<Object>>();
        }

        #endregion

        #region Console logger

        [Test]
        public void AddConsoleLogger_NoParameters_AddLoggerWasCalledAsExpected()
        {
            this.defaultNullMock.Object.AddConsoleLogger();
            this.defaultInstMock.Object.AddConsoleLogger();
            this.contextNullMock.Object.AddConsoleLogger();
            this.contextInstMock.Object.AddConsoleLogger();

            this.defaultNullMock.Verify(x => x.AddLogger(It.IsAny<ILogger>()), Times.Never);
            this.defaultInstMock.Verify(x => x.AddLogger(It.IsAny<ILogger>()), Times.Once);
            this.contextNullMock.Verify(x => x.AddLogger(It.IsAny<ILogger<Object>>()), Times.Never);
            this.contextInstMock.Verify(x => x.AddLogger(It.IsAny<ILogger<Object>>()), Times.Once);
        }

        [Test]
        public void AddConsoleLogger_Boolean_AddLoggerWasCalledAsExpected([Values(true, false)] Boolean windows)
        {
            this.defaultNullMock.Object.AddConsoleLogger(windows);
            this.defaultInstMock.Object.AddConsoleLogger(windows);
            this.contextNullMock.Object.AddConsoleLogger(windows);
            this.contextInstMock.Object.AddConsoleLogger(windows);

            this.defaultNullMock.Verify(x => x.AddLogger(It.IsAny<ILogger>()), Times.Never);
            this.defaultInstMock.Verify(x => x.AddLogger(It.IsAny<ILogger>()), Times.Once);
            this.contextNullMock.Verify(x => x.AddLogger(It.IsAny<ILogger<Object>>()), Times.Never);
            this.contextInstMock.Verify(x => x.AddLogger(It.IsAny<ILogger<Object>>()), Times.Once);
        }

        [Test]
        public void AddConsoleLogger_ILoggerSettingsSection_AddLoggerWasCalledAsExpected()
        {
            this.defaultNullMock.Object.AddConsoleLogger(this.loggerSettingsSection.Object);
            this.defaultInstMock.Object.AddConsoleLogger(this.loggerSettingsSection.Object);
            this.contextNullMock.Object.AddConsoleLogger(this.loggerSettingsSection.Object);
            this.contextInstMock.Object.AddConsoleLogger(this.loggerSettingsSection.Object);

            this.defaultNullMock.Verify(x => x.AddLogger(It.IsAny<ILogger>()), Times.Never);
            this.defaultInstMock.Verify(x => x.AddLogger(It.IsAny<ILogger>()), Times.Once);
            this.contextNullMock.Verify(x => x.AddLogger(It.IsAny<ILogger<Object>>()), Times.Never);
            this.contextInstMock.Verify(x => x.AddLogger(It.IsAny<ILogger<Object>>()), Times.Once);
        }

        [Test]
        public void AddConsoleLogger_ILoggerSettingsSectionBoolean_AddLoggerWasCalledAsExpected([Values(true, false)] Boolean windows)
        {
            this.defaultNullMock.Object.AddConsoleLogger(this.loggerSettingsSection.Object, windows);
            this.defaultInstMock.Object.AddConsoleLogger(this.loggerSettingsSection.Object, windows);
            this.contextNullMock.Object.AddConsoleLogger(this.loggerSettingsSection.Object, windows);
            this.contextInstMock.Object.AddConsoleLogger(this.loggerSettingsSection.Object, windows);

            this.defaultNullMock.Verify(x => x.AddLogger(It.IsAny<ILogger>()), Times.Never);
            this.defaultInstMock.Verify(x => x.AddLogger(It.IsAny<ILogger>()), Times.Once);
            this.contextNullMock.Verify(x => x.AddLogger(It.IsAny<ILogger<Object>>()), Times.Never);
            this.contextInstMock.Verify(x => x.AddLogger(It.IsAny<ILogger<Object>>()), Times.Once);
        }

        [Test]
        public void AddConsoleLogger_IConsoleLoggerSettings_AddLoggerWasCalledAsExpected()
        {
            this.defaultNullMock.Object.AddConsoleLogger(this.consoleLoggerSettings.Object);
            this.defaultInstMock.Object.AddConsoleLogger(this.consoleLoggerSettings.Object);
            this.contextNullMock.Object.AddConsoleLogger(this.consoleLoggerSettings.Object);
            this.contextInstMock.Object.AddConsoleLogger(this.consoleLoggerSettings.Object);

            this.defaultNullMock.Verify(x => x.AddLogger(It.IsAny<ILogger>()), Times.Never);
            this.defaultInstMock.Verify(x => x.AddLogger(It.IsAny<ILogger>()), Times.Once);
            this.contextNullMock.Verify(x => x.AddLogger(It.IsAny<ILogger<Object>>()), Times.Never);
            this.contextInstMock.Verify(x => x.AddLogger(It.IsAny<ILogger<Object>>()), Times.Once);
        }

        [Test]
        public void AddConsoleLogger_IConsoleLoggerSettingsBoolean_AddLoggerWasCalledAsExpected([Values(true, false)] Boolean windows)
        {
            this.defaultNullMock.Object.AddConsoleLogger(this.consoleLoggerSettings.Object, windows);
            this.defaultInstMock.Object.AddConsoleLogger(this.consoleLoggerSettings.Object, windows);
            this.contextNullMock.Object.AddConsoleLogger(this.consoleLoggerSettings.Object, windows);
            this.contextInstMock.Object.AddConsoleLogger(this.consoleLoggerSettings.Object, windows);

            this.defaultNullMock.Verify(x => x.AddLogger(It.IsAny<ILogger>()), Times.Never);
            this.defaultInstMock.Verify(x => x.AddLogger(It.IsAny<ILogger>()), Times.Once);
            this.contextNullMock.Verify(x => x.AddLogger(It.IsAny<ILogger<Object>>()), Times.Never);
            this.contextInstMock.Verify(x => x.AddLogger(It.IsAny<ILogger<Object>>()), Times.Once);
        }

        [Test]
        public void AddConsoleLogger_IConsoleLogger_AddLoggerWasCalledAsExpected()
        {
            this.defaultNullMock.Object.AddConsoleLogger(this.defaultConsoleLogger.Object);
            this.defaultInstMock.Object.AddConsoleLogger(this.defaultConsoleLogger.Object);
            this.contextNullMock.Object.AddConsoleLogger(this.contextConsoleLogger.Object);
            this.contextInstMock.Object.AddConsoleLogger(this.contextConsoleLogger.Object);

            this.defaultNullMock.Verify(x => x.AddLogger(It.IsAny<ILogger>()), Times.Never);
            this.defaultInstMock.Verify(x => x.AddLogger(It.IsAny<ILogger>()), Times.Once);
            this.contextNullMock.Verify(x => x.AddLogger(It.IsAny<ILogger<Object>>()), Times.Never);
            this.contextInstMock.Verify(x => x.AddLogger(It.IsAny<ILogger<Object>>()), Times.Once);
        }

        #endregion

        #region Persistent logger

        [Test]
        public void AddPersistentLogger_NoParameters_AddLoggerWasCalledAsExpected()
        {
            this.defaultNullMock.Object.AddPersistentLogger();
            this.defaultInstMock.Object.AddPersistentLogger();
            this.contextNullMock.Object.AddPersistentLogger();
            this.contextInstMock.Object.AddPersistentLogger();

            this.defaultNullMock.Verify(x => x.AddLogger(It.IsAny<ILogger>()), Times.Never);
            this.defaultInstMock.Verify(x => x.AddLogger(It.IsAny<ILogger>()), Times.Once);
            this.contextNullMock.Verify(x => x.AddLogger(It.IsAny<ILogger<Object>>()), Times.Never);
            this.contextInstMock.Verify(x => x.AddLogger(It.IsAny<ILogger<Object>>()), Times.Once);
        }

        [Test]
        public void AddPersistentLogger_String_AddLoggerWasCalledAsExpected()
        {
            this.defaultNullMock.Object.AddPersistentLogger("filename");
            this.defaultInstMock.Object.AddPersistentLogger("filename");
            this.contextNullMock.Object.AddPersistentLogger("filename");
            this.contextInstMock.Object.AddPersistentLogger("filename");

            this.defaultNullMock.Verify(x => x.AddLogger(It.IsAny<ILogger>()), Times.Never);
            this.defaultInstMock.Verify(x => x.AddLogger(It.IsAny<ILogger>()), Times.Once);
            this.contextNullMock.Verify(x => x.AddLogger(It.IsAny<ILogger<Object>>()), Times.Never);
            this.contextInstMock.Verify(x => x.AddLogger(It.IsAny<ILogger<Object>>()), Times.Once);
        }

        [Test]
        public void AddPersistentLogger_ILoggerSettingsSection_AddLoggerWasCalledAsExpected()
        {
            this.defaultNullMock.Object.AddPersistentLogger(this.loggerSettingsSection.Object);
            this.defaultInstMock.Object.AddPersistentLogger(this.loggerSettingsSection.Object);
            this.contextNullMock.Object.AddPersistentLogger(this.loggerSettingsSection.Object);
            this.contextInstMock.Object.AddPersistentLogger(this.loggerSettingsSection.Object);

            this.defaultNullMock.Verify(x => x.AddLogger(It.IsAny<ILogger>()), Times.Never);
            this.defaultInstMock.Verify(x => x.AddLogger(It.IsAny<ILogger>()), Times.Once);
            this.contextNullMock.Verify(x => x.AddLogger(It.IsAny<ILogger<Object>>()), Times.Never);
            this.contextInstMock.Verify(x => x.AddLogger(It.IsAny<ILogger<Object>>()), Times.Once);
        }

        [Test]
        public void AddPersistentLogger_ILoggerSettingsSectionString_AddLoggerWasCalledAsExpected()
        {
            this.defaultNullMock.Object.AddPersistentLogger(this.loggerSettingsSection.Object, "filename");
            this.defaultInstMock.Object.AddPersistentLogger(this.loggerSettingsSection.Object, "filename");
            this.contextNullMock.Object.AddPersistentLogger(this.loggerSettingsSection.Object, "filename");
            this.contextInstMock.Object.AddPersistentLogger(this.loggerSettingsSection.Object, "filename");

            this.defaultNullMock.Verify(x => x.AddLogger(It.IsAny<ILogger>()), Times.Never);
            this.defaultInstMock.Verify(x => x.AddLogger(It.IsAny<ILogger>()), Times.Once);
            this.contextNullMock.Verify(x => x.AddLogger(It.IsAny<ILogger<Object>>()), Times.Never);
            this.contextInstMock.Verify(x => x.AddLogger(It.IsAny<ILogger<Object>>()), Times.Once);
        }

        [Test]
        public void AddPersistentLogger_IPersistentLoggerSettings_AddLoggerWasCalledAsExpected()
        {
            this.defaultNullMock.Object.AddPersistentLogger(this.persistentLoggerSettings.Object);
            this.defaultInstMock.Object.AddPersistentLogger(this.persistentLoggerSettings.Object);
            this.contextNullMock.Object.AddPersistentLogger(this.persistentLoggerSettings.Object);
            this.contextInstMock.Object.AddPersistentLogger(this.persistentLoggerSettings.Object);

            this.defaultNullMock.Verify(x => x.AddLogger(It.IsAny<ILogger>()), Times.Never);
            this.defaultInstMock.Verify(x => x.AddLogger(It.IsAny<ILogger>()), Times.Once);
            this.contextNullMock.Verify(x => x.AddLogger(It.IsAny<ILogger<Object>>()), Times.Never);
            this.contextInstMock.Verify(x => x.AddLogger(It.IsAny<ILogger<Object>>()), Times.Once);
        }

        [Test]
        public void AddPersistentLogger_IPersistentLoggerSettingsString_AddLoggerWasCalledAsExpected()
        {
            this.defaultNullMock.Object.AddPersistentLogger(this.persistentLoggerSettings.Object, "filename");
            this.defaultInstMock.Object.AddPersistentLogger(this.persistentLoggerSettings.Object, "filename");
            this.contextNullMock.Object.AddPersistentLogger(this.persistentLoggerSettings.Object, "filename");
            this.contextInstMock.Object.AddPersistentLogger(this.persistentLoggerSettings.Object, "filename");

            this.defaultNullMock.Verify(x => x.AddLogger(It.IsAny<ILogger>()), Times.Never);
            this.defaultInstMock.Verify(x => x.AddLogger(It.IsAny<ILogger>()), Times.Once);
            this.contextNullMock.Verify(x => x.AddLogger(It.IsAny<ILogger<Object>>()), Times.Never);
            this.contextInstMock.Verify(x => x.AddLogger(It.IsAny<ILogger<Object>>()), Times.Once);
        }

        [Test]
        public void AddPersistentLogger_IPersistentLogger_AddLoggerWasCalledAsExpected()
        {
            this.defaultNullMock.Object.AddPersistentLogger(this.defaultPersistentLogger.Object);
            this.defaultInstMock.Object.AddPersistentLogger(this.defaultPersistentLogger.Object);
            this.contextNullMock.Object.AddPersistentLogger(this.contextPersistentLogger.Object);
            this.contextInstMock.Object.AddPersistentLogger(this.contextPersistentLogger.Object);

            this.defaultNullMock.Verify(x => x.AddLogger(It.IsAny<ILogger>()), Times.Never);
            this.defaultInstMock.Verify(x => x.AddLogger(It.IsAny<ILogger>()), Times.Once);
            this.contextNullMock.Verify(x => x.AddLogger(It.IsAny<ILogger<Object>>()), Times.Never);
            this.contextInstMock.Verify(x => x.AddLogger(It.IsAny<ILogger<Object>>()), Times.Once);
        }

        #endregion

        #region Stream logger

        [Test]
        public void AddStreamLogger_NoParameters_AddLoggerWasCalledAsExpected()
        {
            this.defaultNullMock.Object.AddStreamLogger();
            this.defaultInstMock.Object.AddStreamLogger();
            this.contextNullMock.Object.AddStreamLogger();
            this.contextInstMock.Object.AddStreamLogger();

            this.defaultNullMock.Verify(x => x.AddLogger(It.IsAny<ILogger>()), Times.Never);
            this.defaultInstMock.Verify(x => x.AddLogger(It.IsAny<ILogger>()), Times.Once);
            this.contextNullMock.Verify(x => x.AddLogger(It.IsAny<ILogger<Object>>()), Times.Never);
            this.contextInstMock.Verify(x => x.AddLogger(It.IsAny<ILogger<Object>>()), Times.Once);
        }

        [Test]
        public void AddStreamLogger_Stream_AddLoggerWasCalledAsExpected()
        {
            this.defaultNullMock.Object.AddStreamLogger(new MemoryStream());
            this.defaultInstMock.Object.AddStreamLogger(new MemoryStream());
            this.contextNullMock.Object.AddStreamLogger(new MemoryStream());
            this.contextInstMock.Object.AddStreamLogger(new MemoryStream());

            this.defaultNullMock.Verify(x => x.AddLogger(It.IsAny<ILogger>()), Times.Never);
            this.defaultInstMock.Verify(x => x.AddLogger(It.IsAny<ILogger>()), Times.Once);
            this.contextNullMock.Verify(x => x.AddLogger(It.IsAny<ILogger<Object>>()), Times.Never);
            this.contextInstMock.Verify(x => x.AddLogger(It.IsAny<ILogger<Object>>()), Times.Once);
        }

        [Test]
        public void AddStreamLogger_ILoggerSettingsSection_AddLoggerWasCalledAsExpected()
        {
            this.defaultNullMock.Object.AddStreamLogger(this.loggerSettingsSection.Object);
            this.defaultInstMock.Object.AddStreamLogger(this.loggerSettingsSection.Object);
            this.contextNullMock.Object.AddStreamLogger(this.loggerSettingsSection.Object);
            this.contextInstMock.Object.AddStreamLogger(this.loggerSettingsSection.Object);

            this.defaultNullMock.Verify(x => x.AddLogger(It.IsAny<ILogger>()), Times.Never);
            this.defaultInstMock.Verify(x => x.AddLogger(It.IsAny<ILogger>()), Times.Once);
            this.contextNullMock.Verify(x => x.AddLogger(It.IsAny<ILogger<Object>>()), Times.Never);
            this.contextInstMock.Verify(x => x.AddLogger(It.IsAny<ILogger<Object>>()), Times.Once);
        }

        [Test]
        public void AddStreamLogger_ILoggerSettingsSectionStream_AddLoggerWasCalledAsExpected()
        {
            this.defaultNullMock.Object.AddStreamLogger(this.loggerSettingsSection.Object, new MemoryStream());
            this.defaultInstMock.Object.AddStreamLogger(this.loggerSettingsSection.Object, new MemoryStream());
            this.contextNullMock.Object.AddStreamLogger(this.loggerSettingsSection.Object, new MemoryStream());
            this.contextInstMock.Object.AddStreamLogger(this.loggerSettingsSection.Object, new MemoryStream());

            this.defaultNullMock.Verify(x => x.AddLogger(It.IsAny<ILogger>()), Times.Never);
            this.defaultInstMock.Verify(x => x.AddLogger(It.IsAny<ILogger>()), Times.Once);
            this.contextNullMock.Verify(x => x.AddLogger(It.IsAny<ILogger<Object>>()), Times.Never);
            this.contextInstMock.Verify(x => x.AddLogger(It.IsAny<ILogger<Object>>()), Times.Once);
        }

        [Test]
        public void AddStreamLogger_IStreamLoggerSettings_AddLoggerWasCalledAsExpected()
        {
            this.defaultNullMock.Object.AddStreamLogger(this.streamLoggerSettings.Object);
            this.defaultInstMock.Object.AddStreamLogger(this.streamLoggerSettings.Object);
            this.contextNullMock.Object.AddStreamLogger(this.streamLoggerSettings.Object);
            this.contextInstMock.Object.AddStreamLogger(this.streamLoggerSettings.Object);

            this.defaultNullMock.Verify(x => x.AddLogger(It.IsAny<ILogger>()), Times.Never);
            this.defaultInstMock.Verify(x => x.AddLogger(It.IsAny<ILogger>()), Times.Once);
            this.contextNullMock.Verify(x => x.AddLogger(It.IsAny<ILogger<Object>>()), Times.Never);
            this.contextInstMock.Verify(x => x.AddLogger(It.IsAny<ILogger<Object>>()), Times.Once);
        }

        [Test]
        public void AddStreamLogger_IStreamLogger_AddLoggerWasCalledAsExpected()
        {
            this.defaultNullMock.Object.AddStreamLogger(this.defaultStreamLogger.Object);
            this.defaultInstMock.Object.AddStreamLogger(this.defaultStreamLogger.Object);
            this.contextNullMock.Object.AddStreamLogger(this.contextStreamLogger.Object);
            this.contextInstMock.Object.AddStreamLogger(this.contextStreamLogger.Object);

            this.defaultNullMock.Verify(x => x.AddLogger(It.IsAny<ILogger>()), Times.Never);
            this.defaultInstMock.Verify(x => x.AddLogger(It.IsAny<ILogger>()), Times.Once);
            this.contextNullMock.Verify(x => x.AddLogger(It.IsAny<ILogger<Object>>()), Times.Never);
            this.contextInstMock.Verify(x => x.AddLogger(It.IsAny<ILogger<Object>>()), Times.Once);
        }

        #endregion
    }
}
