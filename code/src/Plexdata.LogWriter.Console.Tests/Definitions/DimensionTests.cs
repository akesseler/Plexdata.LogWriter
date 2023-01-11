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

using NUnit.Framework;
using Plexdata.LogWriter.Definitions;
using Plexdata.Utilities.Testing;
using System;
using System.Diagnostics.CodeAnalysis;

namespace Plexdata.LogWriter.Console.Tests.Definitions
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    [Category(TestType.UnitTest)]
    [TestOf(nameof(Dimension))]
    public class DimensionTests
    {
        [Test]
        public void Dimension_StandardConstruction_ValuesAsExpected()
        {
            Dimension instance = new Dimension();

            Assert.That(instance.Width, Is.EqualTo(0));
            Assert.That(instance.Lines, Is.EqualTo(0));
            Assert.That(instance.IsValid, Is.False);
        }

        [Test]
        public void Dimension_ExtendedConstruction_ValuesAsExpected()
        {
            Dimension instance = new Dimension(42, 23);

            Assert.That(instance.Width, Is.EqualTo(42));
            Assert.That(instance.Lines, Is.EqualTo(23));
            Assert.That(instance.IsValid, Is.True);
        }

        [Test]
        public void Equality_XIsNullYIsNull_BothAreEqual()
        {
            Dimension x = null;
            Dimension y = null;
            Assert.That(x == y, Is.True);
        }

        [Test]
        public void Equality_XIsNotNullYIsNotNull_BothAreEqual()
        {
            Dimension x = new Dimension(2, 2);
            Dimension y = new Dimension(2, 2);
            Assert.That(x == y, Is.True);
        }

        [Test]
        public void Equality_XIsNotNullYIsNull_BothAreNotEqual()
        {
            Dimension x = new Dimension(2, 2);
            Dimension y = null;
            Assert.That(x == y, Is.False);
        }

        [Test]
        public void Equality_XIsNullYIsNotNull_BothAreNotEqual()
        {
            Dimension x = null;
            Dimension y = new Dimension(2, 2);
            Assert.That(x == y, Is.False);
        }

        [Test]
        public void Equality_XIsNotNullYIsNotNull_BothAreNotEqual()
        {
            Dimension x = new Dimension(2, 2);
            Dimension y = new Dimension(4, 4);
            Assert.That(x == y, Is.False);
        }

        [Test]
        public void Equality_XIsNotNullYIsNotNull_BothReferencesAreEqual()
        {
            Dimension t = new Dimension(2, 2);
            Dimension x = t;
            Dimension y = t;
            Assert.That(x == y, Is.True);
        }

        [Test]
        public void Inequality_XIsNotNullYIsNotNull_BothAreEqual()
        {
            Dimension x = new Dimension(2, 2);
            Dimension y = new Dimension(2, 2);
            Assert.That(x != y, Is.False);
        }

        [Test]
        public void Inequality_XIsNotNullYIsNotNull_BothAreNotEqual()
        {
            Dimension x = new Dimension(2, 2);
            Dimension y = new Dimension(4, 4);
            Assert.That(x != y, Is.True);
        }

        [Test]
        public void Equals_XIsNotNullYIsNotNullButDifferentTypes_BothAreNotEqual()
        {
            Dimension x = new Dimension(2, 2);
            Object y = new Object();
            Assert.That(x.Equals(y), Is.False);
        }

        [Test]
        public void Equals_XIsNotNullYIsNotNull_BothReferencesAreEqual()
        {
            Dimension t = new Dimension(2, 2);
            Dimension x = t;
            Dimension y = t;
            Assert.That(x.Equals(y), Is.True);
        }

        [Test]
        public void Equals_XIsNotNullYIsNull_BothAreNotEqual()
        {
            Dimension x = new Dimension(2, 2);
            Dimension y = null;
            Assert.That(x.Equals(y), Is.False);
        }

        [Test]
        public void Equals_XIsNotNullYIsNotNull_BothAreNotEqual()
        {
            Dimension x = new Dimension(2, 2);
            Dimension y = new Dimension(4, 4);
            Assert.That(x.Equals(y), Is.False);
        }

        [Test]
        public void GetHashCode_WidthAndLines_HashCodeAsExpected()
        {
            Int32 hash = (23 * 521) ^ 42;
            Dimension dimension = new Dimension(42, 23);
            Assert.That(dimension.GetHashCode(), Is.EqualTo(hash));
        }

        [TestCase(-1, -1, "IsValid=False, Width=-1, Lines=-1")]
        [TestCase(-1, 0, "IsValid=False, Width=-1, Lines=0")]
        [TestCase(0, -1, "IsValid=False, Width=0, Lines=-1")]
        [TestCase(0, 0, "IsValid=False, Width=0, Lines=0")]
        [TestCase(1, 0, "IsValid=False, Width=1, Lines=0")]
        [TestCase(0, 1, "IsValid=False, Width=0, Lines=1")]
        [TestCase(1, 1, "IsValid=True, Width=1, Lines=1")]
        public void ToString_WidthAndLines_StringAsExpected(Int32 width, Int32 lines, String expected)
        {
            Dimension dimension = new Dimension(width, lines);
            Assert.That(dimension.ToString(), Is.EqualTo(expected));
        }
    }
}
