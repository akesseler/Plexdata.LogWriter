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
using Plexdata.LogWriter.Definitions;
using Plexdata.Utilities.Testing;
using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

namespace Plexdata.LogWriter.Console.Tests.Definitions
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    [NUnit.Framework.Category(TestType.UnitTest)]
    [TestOf(nameof(Coloring))]
    public class ColoringTests
    {
        [Test]
        public void Coloring_StandardConstruction_ColorsAsExpected()
        {
            Coloring instance = new Coloring();

            Assert.That(instance.Foreground, Is.EqualTo(ConsoleColor.Gray));
            Assert.That(instance.Background, Is.EqualTo(ConsoleColor.Black));
        }

        [Test]
        public void Coloring_ExtendedConstruction_ColorsAsExpected()
        {
            Coloring instance = new Coloring(ConsoleColor.Cyan, ConsoleColor.Yellow);

            Assert.That(instance.Foreground, Is.EqualTo(ConsoleColor.Cyan));
            Assert.That(instance.Background, Is.EqualTo(ConsoleColor.Yellow));
        }

        [TestCase(-1, ConsoleColor.Black)]
        [TestCase(16, ConsoleColor.Black)]
        [TestCase(ConsoleColor.Black, -1)]
        [TestCase(ConsoleColor.Black, 16)]
        public void Coloring_ExtendedConstructionWrongArguments_ThrowsInvalidEnumArgumentException(ConsoleColor foreground, ConsoleColor background)
        {
            Assert.That(() => new Coloring(foreground, background), Throws.InstanceOf<InvalidEnumArgumentException>());
        }

        [Test]
        public void ToString_DefaultColoring_StringAsExpected()
        {
            Coloring coloring = new Coloring();
            Assert.That(coloring.ToString(), Is.EqualTo("Foreground=Gray, Background=Black"));
        }
    }
}
