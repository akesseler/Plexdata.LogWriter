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

using NUnit.Framework;
using Plexdata.LogWriter.Internals.Extensions;
using Plexdata.Utilities.Testing;
using System;
using System.Diagnostics.CodeAnalysis;

namespace Plexdata.LogWriter.Abstraction.Tests.Internals.Extensions
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    [Category(TestType.UnitTest)]
    [TestOf(nameof(DateTimeExtension))]
    public class DateTimeExtensionTests
    {
        [TestCase(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc, 0)]
        [TestCase(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Local, 0)] // It's ok for Germany but other time zones may cause trouble.
        [TestCase(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified, 0)]
        [TestCase(1967, 10, 29, 17, 23, 05, 666, DateTimeKind.Utc, 0)]
        [TestCase(1967, 10, 29, 17, 23, 05, 666, DateTimeKind.Local, 0)]
        [TestCase(1967, 10, 29, 17, 23, 05, 666, DateTimeKind.Unspecified, 0)]
        [TestCase(2022, 10, 29, 17, 23, 05, 666, DateTimeKind.Utc, 1667064185.666)]
        [TestCase(2022, 10, 29, 17, 23, 05, 666, DateTimeKind.Local, 1667056985.666)]
        [TestCase(2022, 10, 29, 17, 23, 05, 666, DateTimeKind.Unspecified, 1667056985.666)]
        public void ToUnixEpoch_DateTimeAsProvided_ResultAsExpected(Int32 year, Int32 month, Int32 day, Int32 hour, Int32 minute, Int32 second, Int32 millisecond, DateTimeKind kind, Decimal expected)
        {
            DateTime value = new DateTime(year, month, day, hour, minute, second, millisecond, kind);

            Decimal actual = value.ToUnixEpoch();

            Assert.That(actual, Is.EqualTo(expected));
        }
    }
}
