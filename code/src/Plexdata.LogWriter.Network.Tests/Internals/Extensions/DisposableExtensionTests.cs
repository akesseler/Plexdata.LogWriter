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
using Plexdata.LogWriter.Internals.Extensions;
using Plexdata.Utilities.Testing;
using System;
using System.Diagnostics.CodeAnalysis;

namespace Plexdata.LogWriter.Network.Tests.Internals.Extensions
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    [Category(TestType.UnitTest)]
    [TestOf(nameof(DisposableExtension))]
    internal class DisposableExtensionTests
    {
        [Test]
        public void SafeDispose_DisposableIsNull_ThrowsNothing()
        {
            IDisposable disposable = null;

            Assert.That(() => disposable.SafeDispose(), Throws.Nothing);
        }

        [Test]
        public void SafeDispose_DisposeThrowsException_ThrowsNothing()
        {
            Mock<IDisposable> disposable = new Mock<IDisposable>();

            disposable.Setup(x => x.Dispose()).Throws(new Exception());

            Assert.That(() => disposable.Object.SafeDispose(), Throws.Nothing);
        }

        [Test]
        public void SafeDispose_DisposableIsValid_DisposableDisposeWasCalledOncw()
        {
            Mock<IDisposable> disposable = new Mock<IDisposable>();

            disposable.Object.SafeDispose();

            disposable.Verify(x => x.Dispose(), Times.Once());
        }
    }
}
