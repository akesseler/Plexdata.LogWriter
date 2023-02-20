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
using Plexdata.LogWriter.Internals.Settings;
using Plexdata.Utilities.Testing;
using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace Plexdata.LogWriter.Abstraction.Tests.Internals.Settings
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    [Category(TestType.IntegrationTest)]
    [TestOf(nameof(LoggerSettingsBase))]
    public class LoggerSettingsBaseTests
    {
        #region Load

        [Test]
        public void Load_ReaderIsNull_ResultIsNull()
        {
            LoggerSettingsBaseUnderTest instance = new LoggerSettingsBaseUnderTest();

            Assert.That(instance.TestLoadReader(null), Is.Null);
        }

        [Test]
        public void Load_ReaderIsValid_ResultIsXElement()
        {
            LoggerSettingsBaseUnderTest instance = new LoggerSettingsBaseUnderTest();

            using (Stream stream = this.CreateStream())
            {
                using (XmlReader reader = this.CreateReader(stream))
                {
                    Assert.That(instance.TestLoadReader(reader), Is.InstanceOf<XElement>());
                }
            }
        }

        #endregion

        #region FindSection

        [Test]
        public void FindSection_ValidKeyItemButParentIsNull_ResultIsNull()
        {
            LoggerSettingsBaseUnderTest instance = new LoggerSettingsBaseUnderTest();

            Assert.That(instance.TestFindSection("level1:level2:level3"), Is.Null);
        }

        [Test]
        public void FindSection_ValidKeyListButParentIsNull_ResultIsNull()
        {
            LoggerSettingsBaseUnderTest instance = new LoggerSettingsBaseUnderTest();

            Assert.That(instance.TestFindSection(new String[] { "level1", "level2", "level3" }), Is.Null);
        }

        [TestCase(0)]
        [TestCase(1)]
        public void FindSection_InvalidKeyList_ResultIsNull(Int32 count)
        {
            String[] keys = count == 0 ? null : new String[count - 1];

            using (Stream stream = this.CreateStream())
            {
                using (XmlReader reader = this.CreateReader(stream))
                {
                    LoggerSettingsBaseUnderTest instance = new LoggerSettingsBaseUnderTest(reader);

                    Assert.That(instance.TestFindSection(keys), Is.Null);
                }
            }
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        [TestCase("levelX")]
        [TestCase("level1", null)]
        [TestCase("level1", "")]
        [TestCase("level1", " ")]
        [TestCase("level1", "levelX")]
        [TestCase("level1", "level2", null)]
        [TestCase("level1", "level2", "")]
        [TestCase("level1", "level2", " ")]
        [TestCase("level1", "level2", "levelX")]
        [TestCase("level1", "level2", "level3", null)]
        [TestCase("level1", "level2", "level3", "")]
        [TestCase("level1", "level2", "level3", " ")]
        [TestCase("level1", "level2", "level3", "levelX")]
        public void FindSection_InvalidKeyAtListEnd_ResultIsNull(params String[] keys)
        {
            using (Stream stream = this.CreateStream())
            {
                using (XmlReader reader = this.CreateReader(stream))
                {
                    LoggerSettingsBaseUnderTest instance = new LoggerSettingsBaseUnderTest(reader);

                    Assert.That(instance.TestFindSection(keys), Is.Null);
                }
            }
        }

        [TestCase("level1", "level1")]
        [TestCase("level2", "level1", "level2")]
        [TestCase("level3", "level1", "level2", "level3")]
        public void FindSection_ValidKeyList_ResultIsExpectedName(String name, params String[] keys)
        {
            using (Stream stream = this.CreateStream())
            {
                using (XmlReader reader = this.CreateReader(stream))
                {
                    LoggerSettingsBaseUnderTest instance = new LoggerSettingsBaseUnderTest(reader);

                    XElement actual = instance.TestFindSection(keys) as XElement;

                    Assert.That(actual, Is.Not.Null);
                    Assert.That(actual.Name.LocalName, Is.EqualTo(name));
                }
            }
        }

        #endregion

        #region FindValue

        [Test]
        public void FindValue_ValidKeyItemButParentIsNull_ResultIsEmpty()
        {
            LoggerSettingsBaseUnderTest instance = new LoggerSettingsBaseUnderTest();

            Assert.That(instance.TestFindValue("item1"), Is.Empty);
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        [TestCase("itemX")]
        [TestCase("itemZ")]
        public void FindValue_InvalidKeyItem_ResultIsEmpty(String key)
        {
            using (Stream stream = this.CreateStream())
            {
                using (XmlReader reader = this.CreateReader(stream))
                {
                    LoggerSettingsBaseUnderTest instance = new LoggerSettingsBaseUnderTest(reader, new String[] { "level1", "level2", "level3" });

                    Assert.That(instance.TestFindValue(key), Is.Empty);
                }
            }
        }

        [TestCase("item1", "value1")]
        [TestCase("item2", "value2")]
        [TestCase("item3", "value3")]
        public void FindValue_ValidKeyItem_ResultIsExpectedValue(String key, String value)
        {
            using (Stream stream = this.CreateStream())
            {
                using (XmlReader reader = this.CreateReader(stream))
                {
                    LoggerSettingsBaseUnderTest instance = new LoggerSettingsBaseUnderTest(reader, new String[] { "level1", "level2", "level3" });

                    Assert.That(instance.TestFindValue(key), Is.EqualTo(value));
                }
            }
        }

        #endregion

        #region Private helpers

        private Stream CreateStream()
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

        private XmlReader CreateReader(Stream stream)
        {
            return XmlReader.Create(stream, new XmlReaderSettings());
        }

        private class LoggerSettingsBaseUnderTest : LoggerSettingsBase
        {
            public LoggerSettingsBaseUnderTest()
                : base()
            {
            }

            public LoggerSettingsBaseUnderTest(XmlReader reader)
                : base()
            {
                base.Parent = base.Load(reader);
            }

            public LoggerSettingsBaseUnderTest(XmlReader reader, String[] keys)
                : base()
            {
                base.Parent = base.Load(reader);
                base.Parent = base.FindSection(keys);
            }

            public Object TestLoadReader(XmlReader reader)
            {
                return base.Load(reader);
            }

            public Object TestFindSection(String key)
            {
                return base.FindSection(key);
            }

            public Object TestFindSection(String[] keys)
            {
                return base.FindSection(keys);
            }

            public Object TestFindValue(String key)
            {
                return base.FindValue(key);
            }

            protected override XElement Load(Stream stream)
            {
                throw new NotSupportedException();
            }
        }

        #endregion
    }
}
