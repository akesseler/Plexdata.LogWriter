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
using Plexdata.LogWriter.Abstraction;
using Plexdata.LogWriter.Definitions;
using Plexdata.LogWriter.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text;

namespace Plexdata.LogWriter.Persistent.Tests.Logging
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    [Category("IntegrationTest")]
    [TestOf(nameof(PersistentLoggerBase))]
    public class PersistentLoggerBaseTests
    {
        #region Prologue

        private DummyClass instance = null;
        private Mock<IPersistentLoggerSettings> settings = null;
        private Mock<IPersistentLoggerFacade> facade = null;
        private Mock<IObservableQueue<String>> scheduler = null;

        [SetUp]
        public void Setup()
        {
            this.settings = new Mock<IPersistentLoggerSettings>();
            this.facade = new Mock<IPersistentLoggerFacade>();
            this.scheduler = new Mock<IObservableQueue<String>>();

            this.instance = new DummyClass(this.settings.Object, this.facade.Object, this.scheduler.Object);
        }

        #endregion

        #region Construction

        [Test]
        public void PersistentLoggerBase_SettingsNull_ThrowsArgumentNullException()
        {
            Assert.That(() => new DummyClass(null, this.facade.Object, this.scheduler.Object), Throws.ArgumentNullException);
        }

        [Test]
        public void PersistentLoggerBase_FacadeNull_ThrowsArgumentNullException()
        {
            Assert.That(() => new DummyClass(this.settings.Object, null, this.scheduler.Object), Throws.ArgumentNullException);
        }

        [Test]
        public void PersistentLoggerBase_SchedulerNull_ThrowsArgumentNullException()
        {
            Assert.That(() => new DummyClass(this.settings.Object, this.facade.Object, null), Throws.ArgumentNullException);
        }

        #endregion

        #region Dispose

        [Test]
        public void Dispose_ManyDisposals_ClearCalledOnce([Values(1, 2, 3)] Int32 count)
        {
            while ((count--) > 0) { this.instance.Dispose(); }

            this.scheduler.Verify(x => x.Clear(), Times.Once);
        }

        #endregion

        #region Write without queuing

        [Test]
        public void Write_DisposedIsTrue_NothingIsExecuted()
        {
            this.instance.Dispose();

            this.instance.TestWrite(LogLevel.Message, null, null, null, null, null);

            this.settings.VerifyGet(x => x.IsQueuing, Times.Never);
            this.facade.Verify(x => x.Empty(It.IsAny<String>()), Times.Never);
            this.facade.Verify(x => x.Write(It.IsAny<String>(), It.IsAny<Encoding>(), It.IsAny<String>()), Times.Never);
            this.facade.Verify(x => x.Write(It.IsAny<String>(), It.IsAny<Encoding>(), It.IsAny<IEnumerable<String>>()), Times.Never);
            this.scheduler.Verify(x => x.Enqueue(It.IsAny<String>()), Times.Never);
        }

        [Test]
        public void Write_CheckDisabledIsTrue_NothingIsExecuted()
        {
            this.settings.SetupGet(x => x.LogLevel).Returns(LogLevel.Disabled);

            this.instance.TestWrite(LogLevel.Message, null, null, null, null, null);

            this.settings.VerifyGet(x => x.IsQueuing, Times.Never);
            this.facade.Verify(x => x.Empty(It.IsAny<String>()), Times.Never);
            this.facade.Verify(x => x.Write(It.IsAny<String>(), It.IsAny<Encoding>(), It.IsAny<String>()), Times.Never);
            this.facade.Verify(x => x.Write(It.IsAny<String>(), It.IsAny<Encoding>(), It.IsAny<IEnumerable<String>>()), Times.Never);
            this.scheduler.Verify(x => x.Enqueue(It.IsAny<String>()), Times.Never);
        }

        [Test]
        public void Write_CheckEnabledIsFalse_NothingIsExecuted()
        {
            this.settings.SetupGet(x => x.LogLevel).Returns(LogLevel.Critical);

            this.instance.TestWrite(LogLevel.Message, null, null, null, null, null);

            this.settings.VerifyGet(x => x.IsQueuing, Times.Never);
            this.facade.Verify(x => x.Empty(It.IsAny<String>()), Times.Never);
            this.facade.Verify(x => x.Write(It.IsAny<String>(), It.IsAny<Encoding>(), It.IsAny<String>()), Times.Never);
            this.facade.Verify(x => x.Write(It.IsAny<String>(), It.IsAny<Encoding>(), It.IsAny<IEnumerable<String>>()), Times.Never);
            this.scheduler.Verify(x => x.Enqueue(It.IsAny<String>()), Times.Never);
        }

        [Test]
        public void Write_OutputIsEmpty_NothingIsExecuted()
        {
            this.settings.SetupGet(x => x.LogLevel).Returns(LogLevel.Trace);

            this.instance.TestWrite(LogLevel.Message, null, null, null, null, null);

            this.settings.VerifyGet(x => x.IsQueuing, Times.Never);
            this.facade.Verify(x => x.Empty(It.IsAny<String>()), Times.Never);
            this.facade.Verify(x => x.Write(It.IsAny<String>(), It.IsAny<Encoding>(), It.IsAny<String>()), Times.Never);
            this.facade.Verify(x => x.Write(It.IsAny<String>(), It.IsAny<Encoding>(), It.IsAny<IEnumerable<String>>()), Times.Never);
            this.scheduler.Verify(x => x.Enqueue(It.IsAny<String>()), Times.Never);
        }

        [Test]
        public void Write_CheckCallIsQueuing_IsQueuingWasCalledOnce()
        {
            this.settings.SetupGet(x => x.LogLevel).Returns(LogLevel.Trace);
            this.settings.SetupGet(x => x.Encoding).Returns(Encoding.UTF8);
            this.settings.SetupGet(x => x.Filename).Returns(@"C:\foo\bar.log");
            this.settings.SetupGet(x => x.IsQueuing).Returns(false);

            this.facade.Setup(x => x.Write(It.IsAny<String>(), It.IsAny<Encoding>(), It.IsAny<String>())).Returns(() => { throw new Exception(); });
            this.facade.Setup(x => x.Write(It.IsAny<String>(), It.IsAny<Encoding>(), It.IsAny<IEnumerable<String>>())).Returns(true);

            this.instance.TestWrite(LogLevel.Message, null, null, "message", null, null);

            this.settings.VerifyGet(x => x.IsQueuing, Times.Once);
        }

        [Test]
        public void Write_CheckCallFacadeWrite_FacadeWriteManyWasCalledOnce()
        {
            this.settings.SetupGet(x => x.LogLevel).Returns(LogLevel.Trace);
            this.settings.SetupGet(x => x.Encoding).Returns(Encoding.UTF8);
            this.settings.SetupGet(x => x.Filename).Returns(@"C:\foo\bar.log");
            this.settings.SetupGet(x => x.IsQueuing).Returns(false);

            this.facade.Setup(x => x.Write(It.IsAny<String>(), It.IsAny<Encoding>(), It.IsAny<String>())).Returns(() => { throw new Exception(); });
            this.facade.Setup(x => x.Write(It.IsAny<String>(), It.IsAny<Encoding>(), It.IsAny<IEnumerable<String>>())).Returns(true);

            this.instance.TestWrite(LogLevel.Message, null, null, "message", null, null);

            this.facade.Verify(x => x.Write(It.IsAny<String>(), It.IsAny<Encoding>(), It.IsAny<String>()), Times.Never);
            this.facade.Verify(x => x.Write(It.IsAny<String>(), It.IsAny<Encoding>(), It.IsAny<IEnumerable<String>>()), Times.Once);
        }

        [Test]
        public void Write_CheckCallFacadeWriteWithOneFail_FacadeWriteManyWasCalledTwice()
        {
            Boolean result = true;

            this.settings.SetupGet(x => x.LogLevel).Returns(LogLevel.Trace);
            this.settings.SetupGet(x => x.Encoding).Returns(Encoding.UTF8);
            this.settings.SetupGet(x => x.Filename).Returns(@"C:\foo\bar.log");
            this.settings.SetupGet(x => x.IsQueuing).Returns(false);

            this.facade.Setup(x => x.Write(It.IsAny<String>(), It.IsAny<Encoding>(), It.IsAny<String>())).Returns(() => { throw new Exception(); });
            this.facade.Setup(x => x.Write(It.IsAny<String>(), It.IsAny<Encoding>(), It.IsAny<IEnumerable<String>>())).Returns(() => { result = !result; return result; });

            this.instance.TestWrite(LogLevel.Message, null, null, "message", null, null);

            this.facade.Verify(x => x.Write(It.IsAny<String>(), It.IsAny<Encoding>(), It.IsAny<String>()), Times.Never);
            this.facade.Verify(x => x.Write(It.IsAny<String>(), It.IsAny<Encoding>(), It.IsAny<IEnumerable<String>>()), Times.Exactly(2));
        }

        [Test]
        public void Write_CheckCallFacadeWriteWithOneFailAndDisposed_FacadeWriteManyWasCalledOnce()
        {
            Boolean result = true;

            this.settings.SetupGet(x => x.LogLevel).Returns(LogLevel.Trace);
            this.settings.SetupGet(x => x.Encoding).Returns(Encoding.UTF8);
            this.settings.SetupGet(x => x.Filename).Returns(@"C:\foo\bar.log");
            this.settings.SetupGet(x => x.IsQueuing).Returns(false);

            this.facade.Setup(x => x.Write(It.IsAny<String>(), It.IsAny<Encoding>(), It.IsAny<String>())).Returns(() => { throw new Exception(); });
            this.facade.Setup(x => x.Write(It.IsAny<String>(), It.IsAny<Encoding>(), It.IsAny<IEnumerable<String>>())).Returns(() => { result = !result; this.instance.Dispose(); return result; });

            this.instance.TestWrite(LogLevel.Message, null, null, "message", null, null);

            this.facade.Verify(x => x.Write(It.IsAny<String>(), It.IsAny<Encoding>(), It.IsAny<String>()), Times.Never);
            this.facade.Verify(x => x.Write(It.IsAny<String>(), It.IsAny<Encoding>(), It.IsAny<IEnumerable<String>>()), Times.Once);
        }

        [Test]
        public void Write_CheckCallFacadeEmpty_FacadeEmptyWasCalledOnce()
        {
            String filename = Path.GetTempFileName();
            String fileOne = String.Empty;
            String fileTwo = String.Empty;

            try
            {
                fileOne = Path.Combine(Path.GetDirectoryName(filename), Path.GetFileNameWithoutExtension(filename) + "_one" + Path.GetExtension(filename));
                fileTwo = Path.Combine(Path.GetDirectoryName(filename), Path.GetFileNameWithoutExtension(filename) + "_two" + Path.GetExtension(filename));

                File.AppendAllText(fileOne, String.Empty.PadLeft(1024, '#'));
                File.Copy(fileOne, fileTwo);

                this.settings.SetupGet(x => x.LogLevel).Returns(LogLevel.Trace);
                this.settings.SetupGet(x => x.Encoding).Returns(Encoding.UTF8);
                this.settings.SetupGet(x => x.Filename).Returns(filename);
                this.settings.SetupGet(x => x.IsQueuing).Returns(false);
                this.settings.SetupGet(x => x.IsRolling).Returns(true);
                this.settings.SetupGet(x => x.Threshold).Returns(1);

                this.facade.Setup(x => x.Empty(It.IsAny<String>())).Returns(true);
                this.facade.Setup(x => x.Write(It.IsAny<String>(), It.IsAny<Encoding>(), It.IsAny<String>())).Returns(() => { throw new Exception(); });
                this.facade.Setup(x => x.Write(It.IsAny<String>(), It.IsAny<Encoding>(), It.IsAny<IEnumerable<String>>())).Returns(true);

                this.instance.TestWrite(LogLevel.Message, null, null, "message", null, null);

                this.facade.Verify(x => x.Empty(It.IsAny<String>()), Times.Once);
            }
            finally
            {
                try { if (File.Exists(fileTwo)) { File.Delete(fileTwo); } } catch { }
                try { if (File.Exists(fileOne)) { File.Delete(fileOne); } } catch { }
                try { if (File.Exists(filename)) { File.Delete(filename); } } catch { }
            }
        }

        [Test]
        public void Write_CheckCallFacadeEmptyWithOneFail_FacadeEmptyWasCalledTwice()
        {
            String filename = Path.GetTempFileName();
            String fileOne = String.Empty;
            String fileTwo = String.Empty;
            Boolean result = true;

            try
            {
                fileOne = Path.Combine(Path.GetDirectoryName(filename), Path.GetFileNameWithoutExtension(filename) + "_one" + Path.GetExtension(filename));
                fileTwo = Path.Combine(Path.GetDirectoryName(filename), Path.GetFileNameWithoutExtension(filename) + "_two" + Path.GetExtension(filename));

                File.AppendAllText(fileOne, String.Empty.PadLeft(1024, '#'));
                File.Copy(fileOne, fileTwo);

                this.settings.SetupGet(x => x.LogLevel).Returns(LogLevel.Trace);
                this.settings.SetupGet(x => x.Encoding).Returns(Encoding.UTF8);
                this.settings.SetupGet(x => x.Filename).Returns(filename);
                this.settings.SetupGet(x => x.IsQueuing).Returns(false);
                this.settings.SetupGet(x => x.IsRolling).Returns(true);
                this.settings.SetupGet(x => x.Threshold).Returns(1);

                this.facade.Setup(x => x.Empty(It.IsAny<String>())).Returns(() => { result = !result; return result; });
                this.facade.Setup(x => x.Write(It.IsAny<String>(), It.IsAny<Encoding>(), It.IsAny<String>())).Returns(() => { throw new Exception(); });
                this.facade.Setup(x => x.Write(It.IsAny<String>(), It.IsAny<Encoding>(), It.IsAny<IEnumerable<String>>())).Returns(true);

                this.instance.TestWrite(LogLevel.Message, null, null, "message", null, null);

                this.facade.Verify(x => x.Empty(It.IsAny<String>()), Times.Exactly(2));
            }
            finally
            {
                try { if (File.Exists(fileTwo)) { File.Delete(fileTwo); } } catch { }
                try { if (File.Exists(fileOne)) { File.Delete(fileOne); } } catch { }
                try { if (File.Exists(filename)) { File.Delete(filename); } } catch { }
            }
        }

        [Test]
        public void Write_CheckCallFacadeEmptyWithOneFailAndDisposed_FacadeEmptyWasCalledOnce()
        {
            String filename = Path.GetTempFileName();
            String fileOne = String.Empty;
            String fileTwo = String.Empty;
            Boolean result = true;

            try
            {
                fileOne = Path.Combine(Path.GetDirectoryName(filename), Path.GetFileNameWithoutExtension(filename) + "_one" + Path.GetExtension(filename));
                fileTwo = Path.Combine(Path.GetDirectoryName(filename), Path.GetFileNameWithoutExtension(filename) + "_two" + Path.GetExtension(filename));

                File.AppendAllText(fileOne, String.Empty.PadLeft(1024, '#'));
                File.Copy(fileOne, fileTwo);

                this.settings.SetupGet(x => x.LogLevel).Returns(LogLevel.Trace);
                this.settings.SetupGet(x => x.Encoding).Returns(Encoding.UTF8);
                this.settings.SetupGet(x => x.Filename).Returns(filename);
                this.settings.SetupGet(x => x.IsQueuing).Returns(false);
                this.settings.SetupGet(x => x.IsRolling).Returns(true);
                this.settings.SetupGet(x => x.Threshold).Returns(1);

                this.facade.Setup(x => x.Empty(It.IsAny<String>())).Returns(() => { result = !result; this.instance.Dispose(); return result; });
                this.facade.Setup(x => x.Write(It.IsAny<String>(), It.IsAny<Encoding>(), It.IsAny<String>())).Returns(() => { throw new Exception(); });
                this.facade.Setup(x => x.Write(It.IsAny<String>(), It.IsAny<Encoding>(), It.IsAny<IEnumerable<String>>())).Returns(true);

                this.instance.TestWrite(LogLevel.Message, null, null, "message", null, null);

                this.facade.Verify(x => x.Empty(It.IsAny<String>()), Times.Once);
            }
            finally
            {
                try { if (File.Exists(fileTwo)) { File.Delete(fileTwo); } } catch { }
                try { if (File.Exists(fileOne)) { File.Delete(fileOne); } } catch { }
                try { if (File.Exists(filename)) { File.Delete(filename); } } catch { }
            }
        }

        #endregion

        #region Write with queuing

        [Test]
        public void Write_QueuingIsEnabled_SchedulerEnqueueWasCalledOnce()
        {
            this.settings.SetupGet(x => x.LogLevel).Returns(LogLevel.Trace);
            this.settings.SetupGet(x => x.IsQueuing).Returns(true);

            this.instance.TestWrite(LogLevel.Message, null, null, "message", null, null);

            this.scheduler.Verify(x => x.Enqueue(It.IsAny<String>()), Times.Once);
        }

        [Test]
        public void Write_QueuingIsEnabled_SchedulerDequeueAllWasCalledOnce()
        {
            this.settings.SetupGet(x => x.LogLevel).Returns(LogLevel.Trace);
            this.settings.SetupGet(x => x.IsQueuing).Returns(true);

            this.scheduler.Setup(x => x.Enqueue(It.IsAny<String>())).Callback(() =>
            {
                this.scheduler.Raise(x => x.Enqueued += null, this.scheduler.Object, EventArgs.Empty);
            });

            this.instance.TestWrite(LogLevel.Message, null, null, "message", null, null);

            this.scheduler.Verify(x => x.DequeueAll(), Times.Once);
        }

        [Test]
        public void Write_QueuingIsEnabledButDequeueAllReturnsNull_FacadeWriteWasCalledNever()
        {
            this.settings.SetupGet(x => x.LogLevel).Returns(LogLevel.Trace);
            this.settings.SetupGet(x => x.Encoding).Returns(Encoding.UTF8);
            this.settings.SetupGet(x => x.Filename).Returns(@"C:\foo\bar.log");
            this.settings.SetupGet(x => x.IsQueuing).Returns(true);

            this.facade.Setup(x => x.Write(It.IsAny<String>(), It.IsAny<Encoding>(), It.IsAny<String>())).Returns(() => { throw new Exception(); });
            this.facade.Setup(x => x.Write(It.IsAny<String>(), It.IsAny<Encoding>(), It.IsAny<IEnumerable<String>>())).Returns(true);

            this.scheduler.Setup(x => x.Enqueue(It.IsAny<String>())).Callback(() =>
            {
                this.scheduler.Raise(x => x.Enqueued += null, EventArgs.Empty);
            });

            this.scheduler.Setup(x => x.DequeueAll()).Returns((String[])null);

            this.instance.TestWrite(LogLevel.Message, null, null, "message", null, null);

            this.facade.Verify(x => x.Write(It.IsAny<String>(), It.IsAny<Encoding>(), It.IsAny<String>()), Times.Never);
            this.facade.Verify(x => x.Write(It.IsAny<String>(), It.IsAny<Encoding>(), It.IsAny<IEnumerable<String>>()), Times.Never);
        }

        [Test]
        public void Write_QueuingIsEnabledButDequeueAllReturnsEmpty_FacadeWriteWasCalledNever()
        {
            this.settings.SetupGet(x => x.LogLevel).Returns(LogLevel.Trace);
            this.settings.SetupGet(x => x.Encoding).Returns(Encoding.UTF8);
            this.settings.SetupGet(x => x.Filename).Returns(@"C:\foo\bar.log");
            this.settings.SetupGet(x => x.IsQueuing).Returns(true);

            this.facade.Setup(x => x.Write(It.IsAny<String>(), It.IsAny<Encoding>(), It.IsAny<String>())).Returns(() => { throw new Exception(); });
            this.facade.Setup(x => x.Write(It.IsAny<String>(), It.IsAny<Encoding>(), It.IsAny<IEnumerable<String>>())).Returns(true);

            this.scheduler.Setup(x => x.Enqueue(It.IsAny<String>())).Callback(() =>
            {
                this.scheduler.Raise(x => x.Enqueued += null, EventArgs.Empty);
            });

            this.scheduler.Setup(x => x.DequeueAll()).Returns(new String[0]);

            this.instance.TestWrite(LogLevel.Message, null, null, "message", null, null);

            this.facade.Verify(x => x.Write(It.IsAny<String>(), It.IsAny<Encoding>(), It.IsAny<String>()), Times.Never);
            this.facade.Verify(x => x.Write(It.IsAny<String>(), It.IsAny<Encoding>(), It.IsAny<IEnumerable<String>>()), Times.Never);
        }

        [Test]
        public void Write_QueuingIsEnabledButDequeueAllReturnsEmpty_FacadeWriteManyWasCalledOnce()
        {
            this.settings.SetupGet(x => x.LogLevel).Returns(LogLevel.Trace);
            this.settings.SetupGet(x => x.Encoding).Returns(Encoding.UTF8);
            this.settings.SetupGet(x => x.Filename).Returns(@"C:\foo\bar.log");
            this.settings.SetupGet(x => x.IsQueuing).Returns(true);

            this.facade.Setup(x => x.Write(It.IsAny<String>(), It.IsAny<Encoding>(), It.IsAny<String>())).Returns(() => { throw new Exception(); });
            this.facade.Setup(x => x.Write(It.IsAny<String>(), It.IsAny<Encoding>(), It.IsAny<IEnumerable<String>>())).Returns(true);

            this.scheduler.Setup(x => x.Enqueue(It.IsAny<String>())).Callback(() =>
            {
                this.scheduler.Raise(x => x.Enqueued += null, EventArgs.Empty);
            });

            this.scheduler.Setup(x => x.DequeueAll()).Returns(new String[] { "message1", "message2", "message3" });

            this.instance.TestWrite(LogLevel.Message, null, null, "message", null, null);

            this.facade.Verify(x => x.Write(It.IsAny<String>(), It.IsAny<Encoding>(), It.IsAny<String>()), Times.Never);
            this.facade.Verify(x => x.Write(It.IsAny<String>(), It.IsAny<Encoding>(), It.IsAny<IEnumerable<String>>()), Times.Once);
        }

        #endregion

        #region Private test class implementations

        private class DummyClass : PersistentLoggerBase
        {
            public DummyClass(IPersistentLoggerSettings settings, IPersistentLoggerFacade facade, IObservableQueue<String> scheduler)
                : base(settings, facade, scheduler)
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
