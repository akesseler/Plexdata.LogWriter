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
using Plexdata.LogWriter.Internals.Settings;
using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text;

namespace Plexdata.LogWriter.Abstraction.Tests.Internals.Settings
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    [Category("IntegrationTest")]
    [TestOf(nameof(LoggerSettingsJson))]
    public class LoggerSettingsJsonTests
    {
        #region LoggerSettingsJson

        [Test]
        public void LoggerSettingsJson_StreamIsNull_ParentIsNull()
        {
            LoggerSettingsJsonUnderTest instance = new LoggerSettingsJsonUnderTest();

            Assert.That(instance.GetParent(), Is.Null);
        }

        [Test]
        public void LoggerSettingsJson_StreamIsValid_ParentIsNotNull()
        {
            using (Stream stream = this.CreateValidStream())
            {
                LoggerSettingsJsonUnderTest instance = new LoggerSettingsJsonUnderTest(stream);

                Assert.That(instance.GetParent(), Is.Not.Null);
            }
        }

        [Test]
        public void LoggerSettingsJson_StreamIsInvalid_ParentIsNull()
        {
            using (Stream stream = this.CreateInvalidStream())
            {
                LoggerSettingsJsonUnderTest instance = new LoggerSettingsJsonUnderTest(stream);

                Assert.That(instance.GetParent(), Is.Null);
            }
        }

        #endregion

        #region GetSection

        [Test]
        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        [TestCase("does:not:exist")]
        [TestCase("level1:level2:level3")]
        [TestCase("level1:level2:level3:itemZ")]
        [TestCase("level1:level2:level3:wrong")]
        public void GetSection_VariousSectionKeys_ResultIsNewInstance(String key)
        {
            using (Stream stream = this.CreateValidStream())
            {
                LoggerSettingsJsonUnderTest instance = new LoggerSettingsJsonUnderTest(stream);

                ILoggerSettingsSection actual = instance.GetSection(key);

                Assert.That(actual, Is.Not.Null);
                Assert.That(actual, Is.Not.SameAs(instance));
            }
        }

        #endregion

        #region GetValues

        [Test]
        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        [TestCase("does:not:exist")]
        [TestCase("level1:level2:level3:wrong")]
        public void GetValues_VariousSectionKeys_ResultIsEmpty(String key)
        {
            using (Stream stream = this.CreateValidStream())
            {
                LoggerSettingsJsonUnderTest instance = new LoggerSettingsJsonUnderTest(stream);

                var actual = instance.GetValues(key);

                Assert.That(actual, Is.Empty);
            }
        }

        [Test]
        [TestCase("level1:level2:level3", "value1,value2,value3,valueZ1")]
        [TestCase("level1:level2:level3:itemZ", "valueZ1")]
        public void GetValues_VariousSectionKeys_ResultAsExpected(String key, String result)
        {
            using (Stream stream = this.CreateValidStream())
            {
                LoggerSettingsJsonUnderTest instance = new LoggerSettingsJsonUnderTest(stream);

                var actual = String.Join(",", instance.GetValues(key));

                Assert.That(actual, Is.EqualTo(result));
            }
        }

        [Test]
        public void GetValues_ValuesSectionKey_ResultAsExpected()
        {
            using (Stream stream = this.CreateValidValuesStream())
            {
                LoggerSettingsJsonUnderTest instance = new LoggerSettingsJsonUnderTest(stream);

                var actual = String.Join(",", instance.GetValues("level1:level2:level3:values"));

                Assert.That(actual, Is.EqualTo("value1,value2,value3"));
            }
        }

        #endregion

        #region GetValue

        [Test]
        [TestCase(null, "item1", "")]
        [TestCase("", "item1", "")]
        [TestCase(" ", "item1", "")]
        [TestCase("does:not:exist", "item1", "")]
        [TestCase("level1:level2:level3", "item1", "value1")]
        [TestCase("level1:level2:level3:itemZ", "subitem1", "valueZ1")]
        [TestCase("level1:level2:level3:wrong", "item1", "")]
        public void GetValue_VariousSectionKeys_ResultAsExpected(String path, String value, String result)
        {
            using (Stream stream = this.CreateValidStream())
            {
                LoggerSettingsJsonUnderTest instance = new LoggerSettingsJsonUnderTest(stream);

                ILoggerSettingsSection actual = instance.GetSection(path);

                Assert.That(actual[value], Is.EqualTo(result));
            }
        }

        #endregion

        #region Private helpers

        private Stream CreateValidStream()
        {
            const String content =
                "{ \"level1\": {" +
                "\"level2\": {" +
                "\"level3\": {" +
                "\"item1\": \"value1\"," +
                "\"item2\": \"value2\"," +
                "\"item3\": \"value3\"," +
                "\"itemZ\": {" +
                "\"subitem1\": \"valueZ1\"" +
                "}}}}}";

            return new MemoryStream(Encoding.UTF8.GetBytes(content));
        }

        private Stream CreateValidValuesStream()
        {
            const String content =
                "{ \"level1\": {" +
                "\"level2\": {" +
                "\"level3\": {" +
                "\"values\": [ \"value1\", \"value2\", \"value3\" ]" +
                "}}}}";

            return new MemoryStream(Encoding.UTF8.GetBytes(content));
        }

        private Stream CreateInvalidStream()
        {
            const String content =
                "{ \"level1\": {" +
                "\"level2\": {" +
                "\"level3\": {" +
                "\"item1\": \"value1\"," +
                "\"item2\": \"value2\"," +
                "\"item3\": \"value3\"," +
                "\"itemZ\": " +
                "\"subitem1\": \"valueZ1\"" +
                "";

            return new MemoryStream(Encoding.UTF8.GetBytes(content));
        }

        private class LoggerSettingsJsonUnderTest : LoggerSettingsJson
        {
            public LoggerSettingsJsonUnderTest()
                : this(null)
            {
            }

            public LoggerSettingsJsonUnderTest(Stream stream)
                : base(stream)
            {
            }

            public Object GetParent()
            {
                return base.Parent;
            }
        }

        #endregion
    }
}
