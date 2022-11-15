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
using Plexdata.LogWriter.Internals.Settings;
using Plexdata.Utilities.Testing;
using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace Plexdata.LogWriter.Abstraction.Tests.Internals.Settings
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    [Category(TestType.IntegrationTest)]
    [TestOf(nameof(LoggerSettingsEmpty))]
    public class LoggerSettingsEmptyTests
    {
        #region Load

        [Test]
        public void Load_ReaderIsNull_ResultIsNull()
        {
            LoggerSettingsEmptyUnderTest instance = new LoggerSettingsEmptyUnderTest();

            Assert.That(instance.TestLoadStream(null), Is.Null);
        }

        [Test]
        public void Load_ReaderIsValid_ResultIsNull()
        {
            LoggerSettingsEmptyUnderTest instance = new LoggerSettingsEmptyUnderTest();

            using (Stream stream = new MemoryStream())
            {
                Assert.That(instance.TestLoadStream(stream), Is.Null);
            }
        }

        #endregion

        #region GetSection

        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        [TestCase("level1")]
        [TestCase("level1:level2")]
        public void GetSection_SectionKeyDontCare_ResultIsSameInstance(String key)
        {
            LoggerSettingsEmptyUnderTest instance = new LoggerSettingsEmptyUnderTest();

            Assert.That(instance.GetSection(key), Is.SameAs(instance));
        }

        #endregion

        #region GetValues

        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        [TestCase("item1")]
        [TestCase("item2")]
        public void GetValues_ValueKeyDontCare_ResultIsEmpty(String key)
        {
            LoggerSettingsEmptyUnderTest instance = new LoggerSettingsEmptyUnderTest();

            Assert.That(instance.GetValues(key), Is.Empty);
        }

        #endregion

        #region GetValue

        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        [TestCase("item1")]
        [TestCase("item2")]
        public void GetValue_ValueKeyDontCare_ResultIsEmpty(String key)
        {
            LoggerSettingsEmptyUnderTest instance = new LoggerSettingsEmptyUnderTest();

            Assert.That(instance[key], Is.Empty);
        }

        #endregion

        #region Private helpers

        private class LoggerSettingsEmptyUnderTest : LoggerSettingsEmpty
        {
            public LoggerSettingsEmptyUnderTest()
                : base()
            {
            }

            public Object TestLoadStream(Stream stream)
            {
                return base.Load(stream);
            }
        }

        #endregion
    }
}
