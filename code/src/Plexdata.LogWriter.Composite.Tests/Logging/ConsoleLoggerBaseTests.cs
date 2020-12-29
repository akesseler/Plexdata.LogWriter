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

using NUnit.Framework;
using Plexdata.LogWriter.Abstraction;
using Plexdata.LogWriter.Definitions;
using Plexdata.LogWriter.Logging;
using System;
using System.Diagnostics.CodeAnalysis;

namespace Plexdata.LogWriter.Composite.Tests.Logging
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    [TestOf(nameof(CompositeLoggerBase))]
    public class CompositeLoggerBaseTests
    {
        #region Construction

        [Test]
        public void CompositeLoggerBase_SettingsNull_ThrowsArgumentNullException()
        {
            Assert.That(() => new DummyClass(null), Throws.ArgumentNullException);
        }

        #endregion

        #region Private test class implementations

        private class DummyClass : CompositeLoggerBase
        {
            public DummyClass(ICompositeLoggerSettings settings)
                : base(settings)
            {
            }

            protected override void Write(LogLevel level, String context, String scope, String message, Exception exception, params (String Label, Object Value)[] details)
            {
                throw new NotImplementedException();
            }
        }

        #endregion
    }
}
