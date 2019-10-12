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
    [TestOf(nameof(LoggerSettingsXml))]
    public class LoggerSettingsXmlTests
    {
        #region LoggerSettingsXml

        [Test]
        public void LoggerSettingsXml_StreamIsNull_ParentIsNull()
        {
            LoggerSettingsXmlUnderTest instance = new LoggerSettingsXmlUnderTest();

            Assert.That(instance.GetParent(), Is.Null);
        }

        [Test]
        public void LoggerSettingsXml_StreamIsValid_ParentIsNotNull()
        {
            using (Stream stream = this.CreateValidStream())
            {
                LoggerSettingsXmlUnderTest instance = new LoggerSettingsXmlUnderTest(stream);

                Assert.That(instance.GetParent(), Is.Not.Null);
            }
        }

        [Test]
        public void LoggerSettingsXml_StreamIsInvalid_ParentIsNull()
        {
            using (Stream stream = this.CreateInvalidStream())
            {
                LoggerSettingsXmlUnderTest instance = new LoggerSettingsXmlUnderTest(stream);

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
                LoggerSettingsXmlUnderTest instance = new LoggerSettingsXmlUnderTest(stream);

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
                LoggerSettingsXmlUnderTest instance = new LoggerSettingsXmlUnderTest(stream);

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
                LoggerSettingsXmlUnderTest instance = new LoggerSettingsXmlUnderTest(stream);

                var actual = String.Join(",", instance.GetValues(key));

                Assert.That(actual, Is.EqualTo(result));
            }
        }

        [Test]
        public void GetValues_ValuesSectionKey_ResultAsExpected()
        {
            using (Stream stream = this.CreateValidValuesStream())
            {
                LoggerSettingsXmlUnderTest instance = new LoggerSettingsXmlUnderTest(stream);

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
                LoggerSettingsXmlUnderTest instance = new LoggerSettingsXmlUnderTest(stream);

                ILoggerSettingsSection actual = instance.GetSection(path);

                Assert.That(actual[value], Is.EqualTo(result));
            }
        }

        #endregion

        #region Private helpers

        private Stream CreateValidStream()
        {
            const String content =
                "<?xml version=\"1.0\" encoding=\"utf-8\" ?>" +
                "<root>" +
                "<level1>" +
                "<level2>" +
                "<level3>" +
                "<item1>value1</item1>" +
                "<item2>value2</item2>" +
                "<item3>value3</item3>" +
                "<itemZ><subitem1>valueZ1</subitem1></itemZ>" +
                "</level3>" +
                "</level2>" +
                "</level1>" +
                "</root>";

            return new MemoryStream(Encoding.UTF8.GetBytes(content));
        }

        private Stream CreateValidValuesStream()
        {
            const String content =
                "<?xml version=\"1.0\" encoding=\"utf-8\" ?>" +
                "<root>" +
                "<level1>" +
                "<level2>" +
                "<level3>" +
                "<values>" +
                "<item>value1</item>" +
                "<item>value2</item>" +
                "<item>value3</item>" +
                "</values>" +
                "</level3>" +
                "</level2>" +
                "</level1>" +
                "</root>";

            return new MemoryStream(Encoding.UTF8.GetBytes(content));
        }

        private Stream CreateInvalidStream()
        {
            const String content =
                "<?xml version=\"1.0\" encoding=\"utf-8\" ?>" +
                "<root>" +
                "<level1>" +
                "<level2>" +
                "<level3>" +
                "<item1>value1</item1>" +
                "<item2>value2</item2>" +
                "<item3>value3</item3>" +
                "<itemZ><subitem1>valueZ1</subitem1></itemZ>" +
                "";

            return new MemoryStream(Encoding.UTF8.GetBytes(content));
        }

        private class LoggerSettingsXmlUnderTest : LoggerSettingsXml
        {
            public LoggerSettingsXmlUnderTest()
                : this(null)
            {
            }

            public LoggerSettingsXmlUnderTest(Stream stream)
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
