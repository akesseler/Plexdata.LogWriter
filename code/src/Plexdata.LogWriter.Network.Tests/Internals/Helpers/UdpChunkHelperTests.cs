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
using Plexdata.LogWriter.Internals.Abstraction;
using Plexdata.LogWriter.Internals.Helpers;
using Plexdata.Utilities.Testing;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Plexdata.LogWriter.Network.Tests.Internals.Helpers
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    [Category(TestType.UnitTest)]
    [TestOf(nameof(UdpChunkHelper))]
    internal class UdpChunkHelperTests
    {
        #region Prologue

        private Mock<INetworkInternalFactory> factory;
        private Mock<INetworkLoggerSettings> settings;
        private Mock<IGZipFacade> zipper;
        private Mock<IRandomFacade> random;

        [SetUp]
        public void Setup()
        {
            this.factory = new Mock<INetworkInternalFactory>();
            this.settings = new Mock<INetworkLoggerSettings>();

            this.zipper = new Mock<IGZipFacade>();
            this.random = new Mock<IRandomFacade>();

            this.factory.Setup(x => x.Create<IGZipFacade>()).Returns(this.zipper.Object);
            this.factory.Setup(x => x.Create<IRandomFacade>()).Returns(this.random.Object);

            this.settings
                .SetupGet(x => x.LogType)
                .Returns(LogType.Gelf);

            this.zipper
                .Setup(x => x.Compress(It.IsAny<Byte[]>()))
                .Returns((Byte[] payload) => payload);

            this.random
                .Setup(x => x.NextBytes(It.IsAny<Byte[]>()))
                .Callback((Byte[] buffer) =>
                {
                    buffer[0] = 0xF1;
                    buffer[1] = 0xF2;
                    buffer[2] = 0xF3;
                    buffer[3] = 0xF4;
                    buffer[4] = 0xF5;
                    buffer[5] = 0xF6;
                    buffer[6] = 0xF7;
                    buffer[7] = 0xF8;
                });
        }

        #endregion

        #region Construction

        [TestCase(typeof(INetworkInternalFactory))]
        [TestCase(typeof(INetworkLoggerSettings))]
        public void UdpChunkHelper_DependencyIsNull_ThrowsArgumentNullException(Type dependency)
        {
            this.factory = dependency == typeof(INetworkInternalFactory) ? null : this.factory;
            this.settings = dependency == typeof(INetworkLoggerSettings) ? null : this.settings;

            Assert.That(() => this.CreateInstance(), Throws.ArgumentNullException);
        }

        [Test]
        public void UdpChunkHelper_DependencyIsValid_NetworkInternalFactoryCreateCalledWithWithExpectedTypes()
        {
            this.CreateInstance();

            this.factory.Verify(x => x.Create<IGZipFacade>(), Times.Once());
            this.factory.Verify(x => x.Create<IRandomFacade>(), Times.Once());
        }

        #endregion

        #region GetChunks

        [Test]
        public void GetChunks_PayloadIsNull_ResultIsEmpty()
        {
            Assert.That(this.CreateInstance().GetChunks(null), Is.Empty);
        }

        [Test]
        public void GetChunks_PayloadIsEmpty_ResultIsEmpty()
        {
            Assert.That(this.CreateInstance().GetChunks(new Byte[0]), Is.Empty);
        }

        [TestCase(LogType.Raw)]
        [TestCase(LogType.Csv)]
        [TestCase(LogType.Json)]
        [TestCase(LogType.Xml)]
        public void GetChunks_LogTypeIsNotGelf_GelfPropertiesWereNeverCalled(LogType logType)
        {
            this.settings.SetupGet(x => x.LogType).Returns(logType);

            this.CreateInstance().GetChunks(this.CreatePayload(10));

            this.settings.VerifyGet(x => x.Compressed, Times.Never());
            this.settings.VerifyGet(x => x.Threshold, Times.Never());
            this.settings.VerifyGet(x => x.Maximum, Times.Once());
        }

        [TestCase(0, 5, 0)]
        [TestCase(1, 5, 1)]
        [TestCase(2, 5, 1)]
        [TestCase(3, 5, 1)]
        [TestCase(4, 5, 1)]
        [TestCase(5, 5, 1)]
        [TestCase(6, 5, 2)]
        [TestCase(7, 5, 2)]
        [TestCase(8, 5, 2)]
        [TestCase(9, 5, 2)]
        [TestCase(10, 5, 2)]
        [TestCase(11, 5, 3)]
        [TestCase(12, 5, 3)]
        [TestCase(13, 5, 3)]
        [TestCase(14, 5, 3)]
        [TestCase(15, 5, 3)]
        [TestCase(16, 5, 4)]
        public void GetChunks_LogTypeIsNotGelf_PayloadSplitIntoExpectedChunks(Int32 length, Int32 maximum, Int32 expectedCount)
        {
            Byte[] payload = this.CreatePayload(length);

            this.settings.SetupGet(x => x.LogType).Returns(LogType.Json);
            this.settings.SetupGet(x => x.Maximum).Returns(maximum);

            Byte[][] expected = new Byte[expectedCount][];

            for (Int32 index = 0; index < expected.Length; index++)
            {
                Int32 offset = index * maximum;

                expected[index] = new Byte[Math.Min(payload.Length - offset, maximum)];

                Array.ConstrainedCopy(payload, offset, expected[index], 0, expected[index].Length);
            }

            List<Byte[]> actual = this.CreateInstance().GetChunks(payload).ToList();

            Assert.That(actual.Count, Is.EqualTo(expectedCount));

            for (Int32 index = 0; index < actual.Count; index++)
            {
                Assert.That(Enumerable.SequenceEqual(actual[index], expected[index]), Is.True);
            }
        }

        [TestCase(LogType.Gelf)]
        public void GetChunks_LogTypeIsGelf_GelfPropertiesWereCalledOnce(LogType logType)
        {
            this.settings.SetupGet(x => x.LogType).Returns(logType);

            this.CreateInstance().GetChunks(this.CreatePayload(10));

            this.settings.VerifyGet(x => x.Compressed, Times.Once());
            this.settings.VerifyGet(x => x.Threshold, Times.Once());
            this.settings.VerifyGet(x => x.Maximum, Times.Once());
        }

        [TestCase(true, 10, 10, 0)]
        [TestCase(true, 10, 9, 1)]
        [TestCase(false, 10, 10, 0)]
        [TestCase(false, 10, 9, 0)]
        public void GetChunks_CompressedAndThresholdAsProvided_IGZipFacadeCompressCalledAsExpected(Boolean compressed, Int32 length, Int32 threshold, Int32 expected)
        {
            Byte[] payload = this.CreatePayload(length);

            this.settings.SetupGet(x => x.Compressed).Returns(compressed);
            this.settings.SetupGet(x => x.Threshold).Returns(threshold);

            this.CreateInstance().GetChunks(payload).ToList();

            this.zipper.Verify(x => x.Compress(payload), Times.Exactly(expected));
        }

        [Test]
        public void GetChunks_PayloadIsLessThanMaximum_ResultAsExpected()
        {
            Byte[] payload = this.CreatePayload(10);

            this.settings.SetupGet(x => x.Maximum).Returns(11);

            Assert.That(this.CreateInstance().GetChunks(payload).First(), Is.SameAs(payload));
        }

        [Test]
        public void GetChunks_CalculatedSequencesGreaterThanLimitation_ResultIsEmpty()
        {
            Byte[] payload = this.CreatePayload(129);

            this.settings.SetupGet(x => x.Maximum).Returns(13);

            Assert.That(this.CreateInstance().GetChunks(payload), Is.Empty);
        }

        [Test]
        public void GetChunks_TwoSequences_IRandomFacadeNextBytesCalledOnce()
        {
            Byte[] payload = this.CreatePayload(50);

            this.settings.SetupGet(x => x.Maximum).Returns(12 + 25);

            this.CreateInstance().GetChunks(payload).ToList();

            this.random.Verify(x => x.NextBytes(It.IsAny<Byte[]>()), Times.Once);
        }

        [Test]
        public void GetChunks_TwoSequences_ResultWithTwoEntries()
        {
            Byte[] payload = this.CreatePayload(50);

            this.settings.SetupGet(x => x.Maximum).Returns(12 + 25);

            List<Byte[]> actual = this.CreateInstance().GetChunks(payload).ToList();

            Assert.That(actual.Count, Is.EqualTo(2));
        }

        [Test]
        public void GetChunks_MultipleSequences_ChunksContainExpectedData()
        {
            Byte[] payload = this.CreatePayload(50);

            this.settings.SetupGet(x => x.Maximum).Returns(12 + 15);

            List<Byte[]> actual = this.CreateInstance().GetChunks(payload).ToList();

            Assert.That(actual[0].Length, Is.EqualTo(27));
            Assert.That(actual[0][0], Is.EqualTo(0x1E));
            Assert.That(actual[0][1], Is.EqualTo(0x0F));
            Assert.That(actual[0][2], Is.EqualTo(0xF1));
            Assert.That(actual[0][3], Is.EqualTo(0xF2));
            Assert.That(actual[0][4], Is.EqualTo(0xF3));
            Assert.That(actual[0][5], Is.EqualTo(0xF4));
            Assert.That(actual[0][6], Is.EqualTo(0xF5));
            Assert.That(actual[0][7], Is.EqualTo(0xF6));
            Assert.That(actual[0][8], Is.EqualTo(0xF7));
            Assert.That(actual[0][9], Is.EqualTo(0xF8));
            Assert.That(actual[0][10], Is.EqualTo(0x00));
            Assert.That(actual[0][11], Is.EqualTo(0x04));

            Assert.That(actual[1].Length, Is.EqualTo(27));
            Assert.That(actual[1][0], Is.EqualTo(0x1E));
            Assert.That(actual[1][1], Is.EqualTo(0x0F));
            Assert.That(actual[1][2], Is.EqualTo(0xF1));
            Assert.That(actual[1][3], Is.EqualTo(0xF2));
            Assert.That(actual[1][4], Is.EqualTo(0xF3));
            Assert.That(actual[1][5], Is.EqualTo(0xF4));
            Assert.That(actual[1][6], Is.EqualTo(0xF5));
            Assert.That(actual[1][7], Is.EqualTo(0xF6));
            Assert.That(actual[1][8], Is.EqualTo(0xF7));
            Assert.That(actual[1][9], Is.EqualTo(0xF8));
            Assert.That(actual[1][10], Is.EqualTo(0x01));
            Assert.That(actual[1][11], Is.EqualTo(0x04));

            Assert.That(actual[2].Length, Is.EqualTo(27));
            Assert.That(actual[2][0], Is.EqualTo(0x1E));
            Assert.That(actual[2][1], Is.EqualTo(0x0F));
            Assert.That(actual[2][2], Is.EqualTo(0xF1));
            Assert.That(actual[2][3], Is.EqualTo(0xF2));
            Assert.That(actual[2][4], Is.EqualTo(0xF3));
            Assert.That(actual[2][5], Is.EqualTo(0xF4));
            Assert.That(actual[2][6], Is.EqualTo(0xF5));
            Assert.That(actual[2][7], Is.EqualTo(0xF6));
            Assert.That(actual[2][8], Is.EqualTo(0xF7));
            Assert.That(actual[2][9], Is.EqualTo(0xF8));
            Assert.That(actual[2][10], Is.EqualTo(0x02));
            Assert.That(actual[2][11], Is.EqualTo(0x04));

            Assert.That(actual[3].Length, Is.EqualTo(17));
            Assert.That(actual[3][0], Is.EqualTo(0x1E));
            Assert.That(actual[3][1], Is.EqualTo(0x0F));
            Assert.That(actual[3][2], Is.EqualTo(0xF1));
            Assert.That(actual[3][3], Is.EqualTo(0xF2));
            Assert.That(actual[3][4], Is.EqualTo(0xF3));
            Assert.That(actual[3][5], Is.EqualTo(0xF4));
            Assert.That(actual[3][6], Is.EqualTo(0xF5));
            Assert.That(actual[3][7], Is.EqualTo(0xF6));
            Assert.That(actual[3][8], Is.EqualTo(0xF7));
            Assert.That(actual[3][9], Is.EqualTo(0xF8));
            Assert.That(actual[3][10], Is.EqualTo(0x03));
            Assert.That(actual[3][11], Is.EqualTo(0x04));
        }

        #endregion

        #region Private helpers

        private IUdpChunkHelper CreateInstance()
        {
            return new UdpChunkHelper(
                this.factory?.Object,
                this.settings?.Object
            );
        }

        private Byte[] CreatePayload(Int32 length)
        {
            Byte[] result = new Byte[length];

            for (Int32 index = 0; index < length; index++)
            {
                result[index] = (Byte)(index & 0x000000FF);
            }

            return result;
        }

        #endregion
    }
}
