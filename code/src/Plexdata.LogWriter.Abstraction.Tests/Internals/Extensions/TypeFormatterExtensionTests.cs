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
using Plexdata.LogWriter.Internals.Extensions;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace Plexdata.LogWriter.Abstraction.Tests.Internals.Extensions
{
    [TestFixture]
    [ExcludeFromCodeCoverage]
    [TestOf(nameof(TypeFormatterExtension))]
    public class TypeFormatterExtensionTests
    {
        #region DefaultCulture

        [Test]
        public void DefaultCulture_ValidateDefaultCulture_ResultIsEnUs()
        {
            Assert.That(((CultureInfo)TypeFormatterExtension.DefaultFormat).Name, Is.EqualTo("en-US"));
        }

        #endregion

        #region IsSupported

        [Test]
        [TestCase(null)]
        [TestCase("")]
        [TestCase("string")]
        public void IsSupported_StringStandard_ResultIsTrue(String value)
        {
            Assert.That(value.IsSupported(), Is.True);
        }

        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void IsSupported_BooleanStandard_ResultIsTrue(Boolean value)
        {
            Assert.That(value.IsSupported(), Is.True);
        }

        [Test]
        [TestCase(null)]
        [TestCase(true)]
        [TestCase(false)]
        public void IsSupported_BooleanNullable_ResultIsTrue(Boolean? value)
        {
            Assert.That(value.IsSupported(), Is.True);
        }

        [Test]
        [TestCase('#')]
        public void IsSupported_CharStandard_ResultIsTrue(Char value)
        {
            Assert.That(value.IsSupported(), Is.True);
        }

        [Test]
        [TestCase(null)]
        [TestCase('#')]
        public void IsSupported_CharNullable_ResultIsTrue(Char? value)
        {
            Assert.That(value.IsSupported(), Is.True);
        }

        [Test]
        [TestCase(-42)]
        public void IsSupported_SByteStandard_ResultIsTrue(SByte value)
        {
            Assert.That(value.IsSupported(), Is.True);
        }

        [Test]
        [TestCase(null)]
        [TestCase(-42)]
        public void IsSupported_SByteNullable_ResultIsTrue(SByte? value)
        {
            Assert.That(value.IsSupported(), Is.True);
        }

        [Test]
        [TestCase(42)]
        public void IsSupported_ByteStandard_ResultIsTrue(Byte value)
        {
            Assert.That(value.IsSupported(), Is.True);
        }

        [Test]
        [TestCase(null)]
        [TestCase(42)]
        public void IsSupported_ByteNullable_ResultIsTrue(Byte? value)
        {
            Assert.That(value.IsSupported(), Is.True);
        }

        [Test]
        [TestCase(-42)]
        public void IsSupported_Int16Standard_ResultIsTrue(Int16 value)
        {
            Assert.That(value.IsSupported(), Is.True);
        }

        [Test]
        [TestCase(null)]
        [TestCase(-42)]
        public void IsSupported_Int16Nullable_ResultIsTrue(Int16? value)
        {
            Assert.That(value.IsSupported(), Is.True);
        }

        [Test]
        [TestCase((UInt16)42)]
        public void IsSupported_UInt16Standard_ResultIsTrue(UInt16 value)
        {
            Assert.That(value.IsSupported(), Is.True);
        }

        [Test]
        [TestCase(null)]
        [TestCase((UInt16)42)]
        public void IsSupported_UInt16Nullable_ResultIsTrue(UInt16? value)
        {
            Assert.That(value.IsSupported(), Is.True);
        }

        [Test]
        [TestCase(-42)]
        public void IsSupported_Int32Standard_ResultIsTrue(Int32 value)
        {
            Assert.That(value.IsSupported(), Is.True);
        }

        [Test]
        [TestCase(null)]
        [TestCase(-42)]
        public void IsSupported_Int32Nullable_ResultIsTrue(Int32? value)
        {
            Assert.That(value.IsSupported(), Is.True);
        }

        [Test]
        [TestCase((UInt32)42)]
        public void IsSupported_UInt32Standard_ResultIsTrue(UInt32 value)
        {
            Assert.That(value.IsSupported(), Is.True);
        }

        [Test]
        [TestCase(null)]
        [TestCase((UInt32)42)]
        public void IsSupported_UInt32Nullable_ResultIsTrue(UInt32? value)
        {
            Assert.That(value.IsSupported(), Is.True);
        }

        [Test]
        [TestCase(-42)]
        public void IsSupported_Int64Standard_ResultIsTrue(Int64 value)
        {
            Assert.That(value.IsSupported(), Is.True);
        }

        [Test]
        [TestCase(null)]
        [TestCase(-42)]
        public void IsSupported_Int64Nullable_ResultIsTrue(Int64? value)
        {
            Assert.That(value.IsSupported(), Is.True);
        }

        [Test]
        [TestCase((UInt64)42)]
        public void IsSupported_UInt64Standard_ResultIsTrue(UInt64 value)
        {
            Assert.That(value.IsSupported(), Is.True);
        }

        [Test]
        [TestCase(null)]
        [TestCase((UInt64)42)]
        public void IsSupported_UInt64Nullable_ResultIsTrue(UInt64? value)
        {
            Assert.That(value.IsSupported(), Is.True);
        }

        [Test]
        [TestCase(42.5F)]
        [TestCase(-42.5F)]
        public void IsSupported_SingleStandard_ResultIsTrue(Single value)
        {
            Assert.That(value.IsSupported(), Is.True);
        }

        [Test]
        [TestCase(null)]
        [TestCase(42.5F)]
        [TestCase(-42.5F)]
        public void IsSupported_SingleNullable_ResultIsTrue(Single? value)
        {
            Assert.That(value.IsSupported(), Is.True);
        }

        [Test]
        [TestCase(42.5)]
        [TestCase(-42.5)]
        public void IsSupported_DoubleStandard_ResultIsTrue(Double value)
        {
            Assert.That(value.IsSupported(), Is.True);
        }

        [Test]
        [TestCase(null)]
        [TestCase(42.5)]
        [TestCase(-42.5)]
        public void IsSupported_DoubleNullable_ResultIsTrue(Double? value)
        {
            Assert.That(value.IsSupported(), Is.True);
        }

        [Test]
        [TestCase(42.5)]
        [TestCase(-42.5)]
        public void IsSupported_DecimalStandard_ResultIsTrue(Decimal value)
        {
            Assert.That(value.IsSupported(), Is.True);
        }

        [Test]
        [TestCase(null)]
        [TestCase(42.5)]
        [TestCase(-42.5)]
        public void IsSupported_DecimalNullable_ResultIsTrue(Decimal? value)
        {
            Assert.That(value.IsSupported(), Is.True);
        }

        [Test]
        [TestCase("2019-10-29T17:05:42")]
        public void IsSupported_DateTimeStandard_ResultIsTrue(String value)
        {
            DateTime actual = DateTime.Parse(value);

            Assert.That(actual.IsSupported(), Is.True);
        }

        [Test]
        [TestCase(null)]
        [TestCase("2019-10-29T17:05:42")]
        public void IsSupported_DateTimeNullable_ResultIsTrue(String value)
        {
            DateTime? actual = value == null ? null : (DateTime?)DateTime.Parse(value);

            Assert.That(actual.IsSupported(), Is.True);
        }

        [Test]
        [TestCase("01234567-89AB-CDEF-0123-456789ABCDEF")]
        public void IsSupported_GuidStandard_ResultIsTrue(String value)
        {
            Guid actual = Guid.Parse(value);

            Assert.That(actual.IsSupported(), Is.True);
        }

        [Test]
        [TestCase(null)]
        [TestCase("01234567-89AB-CDEF-0123-456789ABCDEF")]
        public void IsSupported_GuidNullable_ResultIsTrue(String value)
        {
            Guid? actual = value == null ? null : (Guid?)Guid.Parse(value);

            Assert.That(actual.IsSupported(), Is.True);
        }

        [Test]
        [TestCase(true)]
        [TestCase(false)]
        public void IsSupported_UnsupportedType_ResultIsFalse(Boolean nullable)
        {
            Object actual = nullable ? null : new Object();

            Assert.That(actual.IsSupported(), Is.False);
        }

        #endregion

        #region Parameter validation

        [Test]
        public void Format_CultureIsNull_ThrowsArgumentNullException()
        {
            Object actual = null;
            Assert.That(() => actual.Format(null), Throws.ArgumentNullException);
        }

        [Test]
        public void Format_ValueIsNull_ResultIsNull()
        {
            Object actual = null;
            Assert.That(actual.Format(), Is.EqualTo(null));
        }

        [Test]
        public void Format_ValueIsUnsupported_ThrowsNotSupportedException()
        {
            Object actual = new Object();
            Assert.That(() => actual.Format(), Throws.InstanceOf<NotSupportedException>());
        }

        #endregion

        #region String conversion

        [Test]
        [TestCase("abcdefg", null, "abcdefg")]
        [TestCase("äüöÄÖÜß", null, "äüöÄÖÜß")]
        [TestCase("abcdefg", "de-DE", "abcdefg")]
        [TestCase("äüöÄÖÜß", "de-DE", "äüöÄÖÜß")]
        public void Format_ValueIsString_ResultAsExpected(String value, String language, String expected)
        {
            Assert.That(value.Format(this.GetProvider(language)), Is.EqualTo(expected));
        }

        #endregion

        #region Boolean conversion

        [TestCase(true, null, "true")]
        [TestCase(false, null, "false")]
        [TestCase(true, "de-DE", "true")]
        [TestCase(false, "de-DE", "false")]
        public void Format_ValueIsBoolean_ResultAsExpected(Boolean value, String language, String expected)
        {
            Assert.That(value.Format(this.GetProvider(language)), Is.EqualTo(expected));
        }

        #endregion

        #region Char conversion

        [TestCase('a', null, "a")]
        [TestCase('ä', null, "ä")]
        [TestCase('a', "de-DE", "a")]
        [TestCase('ä', "de-DE", "ä")]
        public void Format_ValueIsChar_ResultAsExpected(Char value, String language, String expected)
        {
            Assert.That(value.Format(this.GetProvider(language)), Is.EqualTo(expected));
        }

        #endregion

        #region SByte conversion

        [Test]
        [TestCase(42, null, null, "42")]
        [TestCase(-42, null, null, "-42")]
        [TestCase(42, null, "de-DE", "42")]
        [TestCase(-42, null, "de-DE", "-42")]
        [TestCase(42, "X4", null, "002A")]
        [TestCase(-42, "X4", null, "00D6")]
        [TestCase(42, "X4", "de-DE", "002A")]
        [TestCase(-42, "X4", "de-DE", "00D6")]
        public void Format_SByteIsString_ResultAsExpected(SByte value, String format, String language, String expected)
        {
            Assert.That(value.Format(format, this.GetProvider(language)), Is.EqualTo(expected));
        }

        #endregion

        #region Byte conversion

        [Test]
        [TestCase(42, null, null, "42")]
        [TestCase(42, null, "de-DE", "42")]
        [TestCase(42, "X4", null, "002A")]
        [TestCase(42, "X4", "de-DE", "002A")]
        public void Format_ByteIsString_ResultAsExpected(Byte value, String format, String language, String expected)
        {
            Assert.That(value.Format(format, this.GetProvider(language)), Is.EqualTo(expected));
        }

        #endregion

        #region Int16 conversion

        [Test]
        [TestCase(42, null, null, "42")]
        [TestCase(42, null, "de-DE", "42")]
        [TestCase(42, "X4", null, "002A")]
        [TestCase(42, "X4", "de-DE", "002A")]
        public void Format_Int16IsString_ResultAsExpected(Int16 value, String format, String language, String expected)
        {
            Assert.That(value.Format(format, this.GetProvider(language)), Is.EqualTo(expected));
        }

        #endregion

        #region UInt16 conversion

        [Test]
        [TestCase((UInt16)42, null, null, "42")]
        [TestCase((UInt16)42, null, "de-DE", "42")]
        [TestCase((UInt16)42, "X4", null, "002A")]
        [TestCase((UInt16)42, "X4", "de-DE", "002A")]
        public void Format_UInt16IsString_ResultAsExpected(UInt16 value, String format, String language, String expected)
        {
            Assert.That(value.Format(format, this.GetProvider(language)), Is.EqualTo(expected));
        }

        #endregion

        #region Int32 conversion

        [Test]
        [TestCase(42, null, null, "42")]
        [TestCase(42, null, "de-DE", "42")]
        [TestCase(42, "X4", null, "002A")]
        [TestCase(42, "X4", "de-DE", "002A")]
        public void Format_Int32IsString_ResultAsExpected(Int32 value, String format, String language, String expected)
        {
            Assert.That(value.Format(format, this.GetProvider(language)), Is.EqualTo(expected));
        }

        #endregion

        #region UInt32 conversion

        [Test]
        [TestCase((UInt32)42, null, null, "42")]
        [TestCase((UInt32)42, null, "de-DE", "42")]
        [TestCase((UInt32)42, "X4", null, "002A")]
        [TestCase((UInt32)42, "X4", "de-DE", "002A")]
        public void Format_UInt32IsString_ResultAsExpected(UInt32 value, String format, String language, String expected)
        {
            Assert.That(value.Format(format, this.GetProvider(language)), Is.EqualTo(expected));
        }

        #endregion

        #region Int64 conversion

        [Test]
        [TestCase(42, null, null, "42")]
        [TestCase(42, null, "de-DE", "42")]
        [TestCase(42, "X4", null, "002A")]
        [TestCase(42, "X4", "de-DE", "002A")]
        public void Format_Int64IsString_ResultAsExpected(Int64 value, String format, String language, String expected)
        {
            Assert.That(value.Format(format, this.GetProvider(language)), Is.EqualTo(expected));
        }

        #endregion

        #region UInt64 conversion

        [Test]
        [TestCase((UInt64)42, null, null, "42")]
        [TestCase((UInt64)42, null, "de-DE", "42")]
        [TestCase((UInt64)42, "X4", null, "002A")]
        [TestCase((UInt64)42, "X4", "de-DE", "002A")]
        public void Format_UInt64IsString_ResultAsExpected(UInt64 value, String format, String language, String expected)
        {
            Assert.That(value.Format(format, this.GetProvider(language)), Is.EqualTo(expected));
        }

        #endregion

        #region Single conversion

        [Test]
        [TestCase(42.5F, null, null, "42.5")]
        [TestCase(-42.5F, null, null, "-42.5")]
        [TestCase(42.5F, null, "de-DE", "42,5")]
        [TestCase(-42.5F, null, "de-DE", "-42,5")]
        [TestCase(42.5F, "0.00", null, "42.50")]
        [TestCase(-42.5F, "0.00", null, "-42.50")]
        [TestCase(42.5F, "0.00", "de-DE", "42,50")]
        [TestCase(-42.5F, "0.00", "de-DE", "-42,50")]
        public void Format_SingleIsString_ResultAsExpected(Single value, String format, String language, String expected)
        {
            Assert.That(value.Format(format, this.GetProvider(language)), Is.EqualTo(expected));
        }

        #endregion

        #region Double conversion

        [Test]
        [TestCase(42.5, null, null, "42.5")]
        [TestCase(-42.5, null, null, "-42.5")]
        [TestCase(42.5, null, "de-DE", "42,5")]
        [TestCase(-42.5, null, "de-DE", "-42,5")]
        [TestCase(42.5, "0.00", null, "42.50")]
        [TestCase(-42.5, "0.00", null, "-42.50")]
        [TestCase(42.5, "0.00", "de-DE", "42,50")]
        [TestCase(-42.5, "0.00", "de-DE", "-42,50")]
        public void Format_DoubleIsString_ResultAsExpected(Double value, String format, String language, String expected)
        {
            Assert.That(value.Format(format, this.GetProvider(language)), Is.EqualTo(expected));
        }

        #endregion

        #region Decimal conversion

        [Test]
        [TestCase(42.5, null, null, "42.5")]
        [TestCase(-42.5, null, null, "-42.5")]
        [TestCase(42.5, null, "de-DE", "42,5")]
        [TestCase(-42.5, null, "de-DE", "-42,5")]
        [TestCase(42.5, "0.00", null, "42.50")]
        [TestCase(-42.5, "0.00", null, "-42.50")]
        [TestCase(42.5, "0.00", "de-DE", "42,50")]
        [TestCase(-42.5, "0.00", "de-DE", "-42,50")]
        public void Format_DecimalIsString_ResultAsExpected(Decimal value, String format, String language, String expected)
        {
            Assert.That(value.Format(format, this.GetProvider(language)), Is.EqualTo(expected));
        }

        #endregion

        #region DateTime conversion

        [Test]
        [TestCase("2019-10-29T17:05:42", null, null, "10/29/2019 5:05:42 PM")]
        [TestCase("2019-10-29T17:05:42", null, "de-DE", "29.10.2019 17:05:42")]
        [TestCase("2019-10-29T17:05:42", "yyMMddHHmmss", null, "191029170542")]
        [TestCase("2019-10-29T17:05:42", "yyMMddHHmmss", "de-DE", "191029170542")]
        public void Format_DateTimeIsString_ResultAsExpected(String value, String format, String language, String expected)
        {
            Assert.That(DateTime.Parse(value).Format(format, this.GetProvider(language)), Is.EqualTo(expected));
        }

        #endregion

        #region Guid conversion

        [Test]
        [TestCase("01234567-89AB-CDEF-0123-456789ABCDEF", null, null, "01234567-89AB-CDEF-0123-456789ABCDEF")]
        [TestCase("01234567-89AB-CDEF-0123-456789ABCDEF", null, "de-DE", "01234567-89AB-CDEF-0123-456789ABCDEF")]
        [TestCase("01234567-89AB-CDEF-0123-456789ABCDEF", "N", null, "0123456789ABCDEF0123456789ABCDEF")]
        [TestCase("01234567-89AB-CDEF-0123-456789ABCDEF", "N", "de-DE", "0123456789ABCDEF0123456789ABCDEF")]
        public void Format_GuidIsString_ResultAsExpected(String value, String format, String language, String expected)
        {
            Assert.That(Guid.Parse(value).Format(format, this.GetProvider(language)), Is.EqualTo(expected));
        }

        #endregion

        #region TryFormat

        [Test]
        public void TryFormat_TypeUnsupported_ResultIsFalse()
        {
            Object actual = new Object();

            Assert.That(actual.TryFormat(out String result), Is.False);
        }

        [Test]
        public void TryFormat_ProviderIsNull_ResultIsFalse()
        {
            Object actual = new Object();
            IFormatProvider provider = null;

            Assert.That(actual.TryFormat(provider, out String result), Is.False);
        }

        [Test]
        [TestCaseSource(nameof(InvalidFormatTestItems))]
        public void TryFormat_FormatIsInvalid_ResultIsFalse(Object current)
        {
            InvalidFormatTestItem value = (InvalidFormatTestItem)current;

            Assert.That(value.Value.TryFormat(value.Format, this.GetProvider(null), out String result), Is.False);
        }

        #endregion

        #region Private stuff

        private static readonly Object[] InvalidFormatTestItems = new Object[] {
            new InvalidFormatTestItem() { Name = "Type: SByte",    Value = (SByte)(-42),    Format = "s5"         },
            new InvalidFormatTestItem() { Name = "Type: Byte",     Value = (Byte)42,        Format = "s5"         },
            new InvalidFormatTestItem() { Name = "Type: Int16",    Value = (Int16)(-42),    Format = "s5"         },
            new InvalidFormatTestItem() { Name = "Type: UInt16",   Value = (UInt16)42,      Format = "s5"         },
            new InvalidFormatTestItem() { Name = "Type: Int32",    Value = (Int32)(-42),    Format = "s5"         },
            new InvalidFormatTestItem() { Name = "Type: UInt32",   Value = (UInt32)42,      Format = "s5"         },
            new InvalidFormatTestItem() { Name = "Type: Int64",    Value = (Int64)(-42),    Format = "s5"         },
            new InvalidFormatTestItem() { Name = "Type: UInt64",   Value = (UInt64)42,      Format = "s5"         },
            new InvalidFormatTestItem() { Name = "Type: Single",   Value = (Single)(-42),   Format = "s5"         },
            new InvalidFormatTestItem() { Name = "Type: Double",   Value = (Double)(-42),   Format = "s5"         },
            new InvalidFormatTestItem() { Name = "Type: Decimal",  Value = (Decimal)(-42m), Format = "s5"         },
            new InvalidFormatTestItem() { Name = "Type: DateTime", Value = DateTime.Now,    Format = "FFFFFFFFFF" },
            new InvalidFormatTestItem() { Name = "Type: Guid",     Value = Guid.NewGuid(),  Format = "xyz"        },
        };

        private class InvalidFormatTestItem
        {
            public String Name { get; set; }
            public Object Value { get; set; }
            public String Format { get; set; }
            public override String ToString() { return this.Name; }
        }

        private IFormatProvider GetProvider(String language)
        {
            return language == null ? TypeFormatterExtension.DefaultFormat : new CultureInfo(language);
        }

        #endregion
    }
}
