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

using System;
using System.Collections.Generic;
using System.Globalization;

namespace Plexdata.LogWriter.Internals.Extensions
{
    /// <summary>
    /// This extension allows to convert values into strings.
    /// </summary>
    /// <remarks>
    /// The value conversion into strings takes place by using culture-depended 
    /// formats. With this it becomes possible to convert a date time value for 
    /// example into US standard date time format.
    /// </remarks>
    internal static class TypeFormatterExtension
    {
        #region Field declaration

        /// <summary>
        /// This field represents the default format provider to be used.
        /// </summary>
        /// <remarks>
        /// The default format provider is initialized as "en-US" culture.
        /// </remarks>
        public readonly static IFormatProvider DefaultFormat = null;

        /// <summary>
        /// This field represents the internal mapping of value types and conversion 
        /// methods.
        /// </summary>
        /// <remarks>
        /// The mapping list is initialized inside the static constructor.
        /// </remarks>
        private readonly static Dictionary<Type, Delegate> mappings = null;

        #endregion

        #region Construction

        /// <summary>
        /// The static class constructor.
        /// </summary>
        /// <remarks>
        /// This constructor does nothing else but the initialization of all static 
        /// fields.
        /// </remarks>
        static TypeFormatterExtension()
        {
            TypeFormatterExtension.DefaultFormat = new CultureInfo("en-US");

            mappings = new Dictionary<Type, Delegate>
            {
                [typeof(String)] = new Func<Object, String, IFormatProvider, String>(TypeFormatterExtension.ToStringString),
                [typeof(Boolean)] = new Func<Object, String, IFormatProvider, String>(TypeFormatterExtension.ToBooleanString),
                [typeof(Char)] = new Func<Object, String, IFormatProvider, String>(TypeFormatterExtension.ToCharString),
                [typeof(SByte)] = new Func<Object, String, IFormatProvider, String>(TypeFormatterExtension.ToSByteString),
                [typeof(Byte)] = new Func<Object, String, IFormatProvider, String>(TypeFormatterExtension.ToByteString),
                [typeof(Int16)] = new Func<Object, String, IFormatProvider, String>(TypeFormatterExtension.ToInt16String),
                [typeof(UInt16)] = new Func<Object, String, IFormatProvider, String>(TypeFormatterExtension.ToUInt16String),
                [typeof(Int32)] = new Func<Object, String, IFormatProvider, String>(TypeFormatterExtension.ToInt32String),
                [typeof(UInt32)] = new Func<Object, String, IFormatProvider, String>(TypeFormatterExtension.ToUInt32String),
                [typeof(Int64)] = new Func<Object, String, IFormatProvider, String>(TypeFormatterExtension.ToInt64String),
                [typeof(UInt64)] = new Func<Object, String, IFormatProvider, String>(TypeFormatterExtension.ToUInt64String),
                [typeof(Single)] = new Func<Object, String, IFormatProvider, String>(TypeFormatterExtension.ToSingleString),
                [typeof(Double)] = new Func<Object, String, IFormatProvider, String>(TypeFormatterExtension.ToDoubleString),
                [typeof(Decimal)] = new Func<Object, String, IFormatProvider, String>(TypeFormatterExtension.ToDecimalString),
                [typeof(DateTime)] = new Func<Object, String, IFormatProvider, String>(TypeFormatterExtension.ToDateTimeString),
                [typeof(Guid)] = new Func<Object, String, IFormatProvider, String>(TypeFormatterExtension.ToGuidString),
            };
        }

        #endregion

        #region Public methods

        /// <summary>
        /// This method checks whether a particular value type is supported by 
        /// this converter.
        /// </summary>
        /// <remarks>
        /// At the moment, this converter supports the types String, Boolean, Char, 
        /// SByte, Byte, Int16, UInt16, Int32, UInt32, Int64, UInt64, Single, Double, 
        /// Decimal, DateTime and Guid.
        /// </remarks>
        /// <typeparam name="TValue">
        /// The type to be verified.
        /// </typeparam>
        /// <param name="value">
        /// The value to be verified.
        /// </param>
        /// <returns>
        /// True if requested type is supported and false if not.
        /// </returns>
        public static Boolean IsSupported<TValue>(this TValue value)
        {
            Type type = typeof(TValue);

            // Special treatment of supported object types that can be null.
            if (value == null && typeof(TValue) == typeof(String))
            {
                return true;
            }

            // Check if value type is nullable and if so, take the 
            // underlying type. Otherwise take the real value type.
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                type = Nullable.GetUnderlyingType(type);
            }
            else
            {
                type = value?.GetType();
            }

            return type != null && TypeFormatterExtension.mappings.ContainsKey(type);
        }

        /// <summary>
        /// This method converts provided value into string using default 
        /// format provider.
        /// </summary>
        /// <remarks>
        /// The provided value is converted using default format provider 
        /// and does not use any specific format string.
        /// </remarks>
        /// <typeparam name="TValue">
        /// The type to be converted.
        /// </typeparam>
        /// <param name="value">
        /// The value to be converted.
        /// </param>
        /// <returns>
        /// The string representation of provided value.
        /// </returns>
        /// <exception cref="NotSupportedException">
        /// This exception is thrown if provided value type is not supported.
        /// </exception>
        /// <exception cref="Exception">
        /// Any other exception thrown by <see cref="Convert"/> is 
        /// possible as well.
        /// </exception>
        public static String Format<TValue>(this TValue value)
        {
            return value.Format<TValue>(null, TypeFormatterExtension.DefaultFormat);
        }

        /// <summary>
        /// This method converts provided value into string using provided 
        /// format provider.
        /// </summary>
        /// <remarks>
        /// The provided value is converted using provided format provider 
        /// and does not use any specific format string.
        /// </remarks>
        /// <typeparam name="TValue">
        /// The type to be converted.
        /// </typeparam>
        /// <param name="value">
        /// The value to be converted.
        /// </param>
        /// <param name="provider">
        /// The format provider to be used for conversion.
        /// </param>
        /// <returns>
        /// The string representation of provided value.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// This exception is thrown if provided format provider is <c>null</c>.
        /// </exception>
        /// <exception cref="NotSupportedException">
        /// This exception is thrown if provided value type is not supported.
        /// </exception>
        /// <exception cref="Exception">
        /// Any other exception thrown by <see cref="Convert"/> is 
        /// possible as well.
        /// </exception>
        public static String Format<TValue>(this TValue value, IFormatProvider provider)
        {
            return value.Format<TValue>(null, provider);
        }

        /// <summary>
        /// This method converts provided value into string using provided 
        /// format as well as provided format provider.
        /// </summary>
        /// <remarks>
        /// The provided value is converted using provided format provider 
        /// and does specific string formatting according to provided format.
        /// </remarks>
        /// <typeparam name="TValue">
        /// The type to be converted.
        /// </typeparam>
        /// <param name="value">
        /// The value to be converted.
        /// </param>
        /// <param name="format">
        /// The format string to be used for conversion.
        /// </param>
        /// <param name="provider">
        /// The format provider to be used for conversion.
        /// </param>
        /// <returns>
        /// The string representation of provided value.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// This exception is thrown if provided format provider is <c>null</c>.
        /// </exception>
        /// <exception cref="NotSupportedException">
        /// This exception is thrown if provided value type is not supported.
        /// </exception>
        /// <exception cref="Exception">
        /// Any other exception thrown by <see cref="Convert"/> is 
        /// possible as well.
        /// </exception>
        public static String Format<TValue>(this TValue value, String format, IFormatProvider provider)
        {
            if (provider == null)
            {
                throw new ArgumentNullException(nameof(provider));
            }

            // The null value conversion is supported by 
            // default. So just return null in this case.
            if (value == null) { return null; }

            if (!value.IsSupported())
            {
                throw new NotSupportedException();
            }

            Type type = typeof(TValue);

            // Check if value type is nullable and if so, take the 
            // underlying type. Otherwise take the real value type.
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                type = Nullable.GetUnderlyingType(type);
            }
            else
            {
                type = value.GetType();
            }

            return (String)TypeFormatterExtension.mappings[type].DynamicInvoke(value, format, provider);
        }

        /// <summary>
        /// This method tries to convert provided value into string using default 
        /// format provider.
        /// </summary>
        /// <remarks>
        /// The provided value is converted using default format provider and does 
        /// not use any specific format string.
        /// </remarks>
        /// <typeparam name="TValue">
        /// The type to be converted.
        /// </typeparam>
        /// <param name="value">
        /// The value to be converted.
        /// </param>
        /// <param name="result">
        /// The string representation of provided value if conversion was successful, 
        /// and <c>null</c> if not.
        /// </param>
        /// <returns>
        /// True if conversion was successful and false otherwise.
        /// </returns>
        public static Boolean TryFormat<TValue>(this TValue value, out String result)
        {
            return value.TryFormat(null, TypeFormatterExtension.DefaultFormat, out result, out Exception error);
        }

        /// <summary>
        /// This method tries to convert provided value into string using provided 
        /// format provider.
        /// </summary>
        /// <remarks>
        /// The provided value is converted using provided format provider and does 
        /// not use any specific format string.
        /// </remarks>
        /// <typeparam name="TValue">
        /// The type to be converted.
        /// </typeparam>
        /// <param name="value">
        /// The value to be converted.
        /// </param>
        /// <param name="provider">
        /// The format provider to be used for conversion.
        /// </param>
        /// <param name="result">
        /// The string representation of provided value if conversion was successful, 
        /// and <c>null</c> if not.
        /// </param>
        /// <returns>
        /// True if conversion was successful and false otherwise.
        /// </returns>
        public static Boolean TryFormat<TValue>(this TValue value, IFormatProvider provider, out String result)
        {
            return value.TryFormat(null, provider, out result, out Exception error);
        }

        /// <summary>
        /// This method tries to convert provided value into string using provided 
        /// format as well as provided format provider.
        /// </summary>
        /// <remarks>
        /// The provided value is converted using provided format provider and does 
        /// specific string formatting according to provided format.
        /// </remarks>
        /// <typeparam name="TValue">
        /// The type to be converted.
        /// </typeparam>
        /// <param name="value">
        /// The value to be converted.
        /// </param>
        /// <param name="format">
        /// Any of the allowed format string of the type to be converted.
        /// </param>
        /// <param name="provider">
        /// The format provider to be used for conversion.
        /// </param>
        /// <param name="result">
        /// The string representation of provided value if conversion was successful, 
        /// and <c>null</c> if not.
        /// </param>
        /// <returns>
        /// True if conversion was successful and false otherwise.
        /// </returns>
        public static Boolean TryFormat<TValue>(this TValue value, String format, IFormatProvider provider, out String result)
        {
            return value.TryFormat(format, provider, out result, out Exception error);
        }

        /// <summary>
        /// This method tries to convert provided value into string using default 
        /// format provider.
        /// </summary>
        /// <remarks>
        /// The provided value is converted using default format provider and does 
        /// not use any specific format string.
        /// </remarks>
        /// <typeparam name="TValue">
        /// The type to be converted.
        /// </typeparam>
        /// <param name="value">
        /// The value to be converted.
        /// </param>
        /// <param name="result">
        /// The string representation of provided value if conversion was successful, 
        /// and <c>null</c> if not.
        /// </param>
        /// <param name="error">
        /// An exception instance to get extended error information.
        /// </param>
        /// <returns>
        /// True if conversion was successful and false otherwise.
        /// </returns>
        public static Boolean TryFormat<TValue>(this TValue value, out String result, out Exception error)
        {
            return value.TryFormat(null, TypeFormatterExtension.DefaultFormat, out result, out error);
        }

        /// <summary>
        /// This method tries to convert provided value into string using provided 
        /// format provider.
        /// </summary>
        /// <remarks>
        /// The provided value is converted using provided format provider and does 
        /// not use any specific format string.
        /// </remarks>
        /// <typeparam name="TValue">
        /// The type to be converted.
        /// </typeparam>
        /// <param name="value">
        /// The value to be converted.
        /// </param>
        /// <param name="provider">
        /// The format provider to be used for conversion.
        /// </param>
        /// <param name="result">
        /// The string representation of provided value if conversion was successful, 
        /// and <c>null</c> if not.
        /// </param>
        /// <param name="error">
        /// An exception instance to get extended error information.
        /// </param>
        /// <returns>
        /// True if conversion was successful and false otherwise.
        /// </returns>
        public static Boolean TryFormat<TValue>(this TValue value, IFormatProvider provider, out String result, out Exception error)
        {
            return value.TryFormat(null, provider, out result, out error);
        }

        /// <summary>
        /// This method tries to convert provided value into string using provided 
        /// format as well as provided format provider.
        /// </summary>
        /// <remarks>
        /// <para>
        /// The provided value is converted using provided format provider and does 
        /// specific string formatting according to provided format.
        /// </para>
        /// <para>
        /// This method always returns false either if type of <typeparamref name="TValue"/> 
        /// is not supported, or if parameter <paramref name="provider"/> is <c>null</c>, or 
        /// if any exception is thrown.
        /// </para>
        /// </remarks>
        /// <typeparam name="TValue">
        /// The type to be converted.
        /// </typeparam>
        /// <param name="value">
        /// The value to be converted.
        /// </param>
        /// <param name="format">
        /// Any of the allowed format string of the type to be converted.
        /// </param>
        /// <param name="provider">
        /// The format provider to be used for conversion.
        /// </param>
        /// <param name="result">
        /// The string representation of provided value if conversion was successful, 
        /// and <c>null</c> if not.
        /// </param>
        /// <param name="error">
        /// An exception instance to get extended error information.
        /// </param>
        /// <returns>
        /// True if conversion was successful and false otherwise.
        /// </returns>
        public static Boolean TryFormat<TValue>(this TValue value, String format, IFormatProvider provider, out String result, out Exception error)
        {
            result = null;
            error = null;

            try
            {
                if (value.IsSupported() && provider != null)
                {
                    result = value.Format(format, provider);
                    return true;
                }
            }
            catch (Exception exception)
            {
                error = exception;
            }

            return false;
        }

        #endregion

        #region Private methods

        /// <summary>
        /// This method converts the value into its string representation.
        /// </summary>
        /// <remarks>
        /// The value is converted by using <see cref="Convert.ToString(Object, IFormatProvider)"/>.
        /// </remarks>
        /// <param name="value">
        /// The value to be converted.
        /// </param>
        /// <param name="format">
        /// The format to be used.
        /// </param>
        /// <param name="provider">
        /// The format provider to be used.
        /// </param>
        /// <returns>
        /// The string representation of provided value.
        /// </returns>
        /// <exception cref="Exception">
        /// Any other exception thrown by <see cref="Convert"/> is 
        /// possible as well.
        /// </exception>
        private static String ToStringString(Object value, String format, IFormatProvider provider)
        {
            return Convert.ToString(value, provider);
        }

        /// <summary>
        /// This method converts the value into its boolean representation.
        /// </summary>
        /// <remarks>
        /// The value is converted by using <see cref="Convert.ToBoolean(Object, IFormatProvider)"/>.
        /// </remarks>
        /// <param name="value">
        /// The value to be converted.
        /// </param>
        /// <param name="format">
        /// The format to be used.
        /// </param>
        /// <param name="provider">
        /// The format provider to be used.
        /// </param>
        /// <returns>
        /// The string representation of provided value in lower cases.
        /// </returns>
        /// <exception cref="Exception">
        /// Any other exception thrown by <see cref="Convert"/> is 
        /// possible as well.
        /// </exception>
        private static String ToBooleanString(Object value, String format, IFormatProvider provider)
        {
            return (Convert.ToBoolean(value, provider) ? Boolean.TrueString : Boolean.FalseString).ToLower();
        }

        /// <summary>
        /// This method converts the value into its character representation.
        /// </summary>
        /// <remarks>
        /// The value is converted by using <see cref="Convert.ToChar(Object, IFormatProvider)"/> 
        /// as well as <see cref="Char.ToString(IFormatProvider)"/>.
        /// </remarks>
        /// <param name="value">
        /// The value to be converted.
        /// </param>
        /// <param name="format">
        /// The format to be used.
        /// </param>
        /// <param name="provider">
        /// The format provider to be used.
        /// </param>
        /// <returns>
        /// The string representation of provided value.
        /// </returns>
        /// <exception cref="Exception">
        /// Any other exception thrown by <see cref="Convert"/> is possible as well.
        /// </exception>
        private static String ToCharString(Object value, String format, IFormatProvider provider)
        {
            return Convert.ToChar(value, provider).ToString(provider);
        }

        /// <summary>
        /// This method converts the value into its signed byte representation.
        /// </summary>
        /// <remarks>
        /// The value is converted by using <see cref="Convert.ToSByte(Object, IFormatProvider)"/> 
        /// as well as <see cref="SByte.ToString(IFormatProvider)"/> respectively 
        /// <see cref="SByte.ToString(String, IFormatProvider)"/> if <paramref name="format"/> 
        /// is not <c>null</c>.
        /// </remarks>
        /// <param name="value">
        /// The value to be converted.
        /// </param>
        /// <param name="format">
        /// The format to be used.
        /// </param>
        /// <param name="provider">
        /// The format provider to be used.
        /// </param>
        /// <returns>
        /// The string representation of provided value.
        /// </returns>
        /// <exception cref="Exception">
        /// Any other exception thrown by <see cref="Convert"/> is possible as well.
        /// </exception>
        private static String ToSByteString(Object value, String format, IFormatProvider provider)
        {
            if (String.IsNullOrWhiteSpace(format))
            {
                return Convert.ToSByte(value, provider).ToString(provider);
            }
            else
            {
                return Convert.ToSByte(value, provider).ToString(format, provider);
            }
        }

        /// <summary>
        /// This method converts the value into its unsigned byte representation.
        /// </summary>
        /// <remarks>
        /// The value is converted by using <see cref="Convert.ToByte(Object, IFormatProvider)"/> 
        /// as well as <see cref="Byte.ToString(IFormatProvider)"/> respectively 
        /// <see cref="Byte.ToString(String, IFormatProvider)"/> if <paramref name="format"/> 
        /// is not <c>null</c>.
        /// </remarks>
        /// <param name="value">
        /// The value to be converted.
        /// </param>
        /// <param name="format">
        /// The format to be used.
        /// </param>
        /// <param name="provider">
        /// The format provider to be used.
        /// </param>
        /// <returns>
        /// The string representation of provided value.
        /// </returns>
        /// <exception cref="Exception">
        /// Any other exception thrown by <see cref="Convert"/> is possible as well.
        /// </exception>
        private static String ToByteString(Object value, String format, IFormatProvider provider)
        {
            if (String.IsNullOrWhiteSpace(format))
            {
                return Convert.ToByte(value, provider).ToString(provider);
            }
            else
            {
                return Convert.ToByte(value, provider).ToString(format, provider);
            }
        }

        /// <summary>
        /// This method converts the value into its signed 16-bit integer representation.
        /// </summary>
        /// <remarks>
        /// The value is converted by using <see cref="Convert.ToInt16(Object, IFormatProvider)"/> 
        /// as well as <see cref="Int16.ToString(IFormatProvider)"/> respectively 
        /// <see cref="Int16.ToString(String, IFormatProvider)"/> if <paramref name="format"/> 
        /// is not <c>null</c>.
        /// </remarks>
        /// <param name="value">
        /// The value to be converted.
        /// </param>
        /// <param name="format">
        /// The format to be used.
        /// </param>
        /// <param name="provider">
        /// The format provider to be used.
        /// </param>
        /// <returns>
        /// The string representation of provided value.
        /// </returns>
        /// <exception cref="Exception">
        /// Any other exception thrown by <see cref="Convert"/> is possible as well.
        /// </exception>
        private static String ToInt16String(Object value, String format, IFormatProvider provider)
        {
            if (String.IsNullOrWhiteSpace(format))
            {
                return Convert.ToInt16(value, provider).ToString(provider);
            }
            else
            {
                return Convert.ToInt16(value, provider).ToString(format, provider);
            }
        }

        /// <summary>
        /// This method converts the value into its unsigned 16-bit integer representation.
        /// </summary>
        /// <remarks>
        /// The value is converted by using <see cref="Convert.ToUInt16(Object, IFormatProvider)"/> 
        /// as well as <see cref="UInt16.ToString(IFormatProvider)"/> respectively 
        /// <see cref="UInt16.ToString(String, IFormatProvider)"/> if <paramref name="format"/> 
        /// is not <c>null</c>.
        /// </remarks>
        /// <param name="value">
        /// The value to be converted.
        /// </param>
        /// <param name="format">
        /// The format to be used.
        /// </param>
        /// <param name="provider">
        /// The format provider to be used.
        /// </param>
        /// <returns>
        /// The string representation of provided value.
        /// </returns>
        /// <exception cref="Exception">
        /// Any other exception thrown by <see cref="Convert"/> is possible as well.
        /// </exception>
        private static String ToUInt16String(Object value, String format, IFormatProvider provider)
        {
            if (String.IsNullOrWhiteSpace(format))
            {
                return Convert.ToUInt16(value, provider).ToString(provider);
            }
            else
            {
                return Convert.ToUInt16(value, provider).ToString(format, provider);
            }
        }

        /// <summary>
        /// This method converts the value into its signed 32-bit integer representation.
        /// </summary>
        /// <remarks>
        /// The value is converted by using <see cref="Convert.ToInt32(Object, IFormatProvider)"/> 
        /// as well as <see cref="Int32.ToString(IFormatProvider)"/> respectively 
        /// <see cref="Int32.ToString(String, IFormatProvider)"/> if <paramref name="format"/> 
        /// is not <c>null</c>.
        /// </remarks>
        /// <param name="value">
        /// The value to be converted.
        /// </param>
        /// <param name="format">
        /// The format to be used.
        /// </param>
        /// <param name="provider">
        /// The format provider to be used.
        /// </param>
        /// <returns>
        /// The string representation of provided value.
        /// </returns>
        /// <exception cref="Exception">
        /// Any other exception thrown by <see cref="Convert"/> is possible as well.
        /// </exception>
        private static String ToInt32String(Object value, String format, IFormatProvider provider)
        {
            if (String.IsNullOrWhiteSpace(format))
            {
                return Convert.ToInt32(value, provider).ToString(provider);
            }
            else
            {
                return Convert.ToInt32(value, provider).ToString(format, provider);
            }
        }

        /// <summary>
        /// This method converts the value into its unsigned 32-bit integer representation.
        /// </summary>
        /// <remarks>
        /// The value is converted by using <see cref="Convert.ToUInt32(Object, IFormatProvider)"/> 
        /// as well as <see cref="UInt32.ToString(IFormatProvider)"/> respectively 
        /// <see cref="UInt32.ToString(String, IFormatProvider)"/> if <paramref name="format"/> 
        /// is not <c>null</c>.
        /// </remarks>
        /// <param name="value">
        /// The value to be converted.
        /// </param>
        /// <param name="format">
        /// The format to be used.
        /// </param>
        /// <param name="provider">
        /// The format provider to be used.
        /// </param>
        /// <returns>
        /// The string representation of provided value.
        /// </returns>
        /// <exception cref="Exception">
        /// Any other exception thrown by <see cref="Convert"/> is possible as well.
        /// </exception>
        private static String ToUInt32String(Object value, String format, IFormatProvider provider)
        {
            if (String.IsNullOrWhiteSpace(format))
            {
                return Convert.ToUInt32(value, provider).ToString(provider);
            }
            else
            {
                return Convert.ToUInt32(value, provider).ToString(format, provider);
            }
        }

        /// <summary>
        /// This method converts the value into its signed 64-bit integer representation.
        /// </summary>
        /// <remarks>
        /// The value is converted by using <see cref="Convert.ToInt64(Object, IFormatProvider)"/> 
        /// as well as <see cref="Int64.ToString(IFormatProvider)"/> respectively 
        /// <see cref="Int64.ToString(String, IFormatProvider)"/> if <paramref name="format"/> 
        /// is not <c>null</c>.
        /// </remarks>
        /// <param name="value">
        /// The value to be converted.
        /// </param>
        /// <param name="format">
        /// The format to be used.
        /// </param>
        /// <param name="provider">
        /// The format provider to be used.
        /// </param>
        /// <returns>
        /// The string representation of provided value.
        /// </returns>
        /// <exception cref="Exception">
        /// Any other exception thrown by <see cref="Convert"/> is possible as well.
        /// </exception>
        private static String ToInt64String(Object value, String format, IFormatProvider provider)
        {
            if (String.IsNullOrWhiteSpace(format))
            {
                return Convert.ToInt64(value, provider).ToString(provider);
            }
            else
            {
                return Convert.ToInt64(value, provider).ToString(format, provider);
            }
        }

        /// <summary>
        /// This method converts the value into its unsigned 64-bit integer representation.
        /// </summary>
        /// <remarks>
        /// The value is converted by using <see cref="Convert.ToUInt64(Object, IFormatProvider)"/> 
        /// as well as <see cref="UInt64.ToString(IFormatProvider)"/> respectively 
        /// <see cref="UInt64.ToString(String, IFormatProvider)"/> if <paramref name="format"/> 
        /// is not <c>null</c>.
        /// </remarks>
        /// <param name="value">
        /// The value to be converted.
        /// </param>
        /// <param name="format">
        /// The format to be used.
        /// </param>
        /// <param name="provider">
        /// The format provider to be used.
        /// </param>
        /// <returns>
        /// The string representation of provided value.
        /// </returns>
        /// <exception cref="Exception">
        /// Any other exception thrown by <see cref="Convert"/> is possible as well.
        /// </exception>
        private static String ToUInt64String(Object value, String format, IFormatProvider provider)
        {
            if (String.IsNullOrWhiteSpace(format))
            {
                return Convert.ToUInt64(value, provider).ToString(provider);
            }
            else
            {
                return Convert.ToUInt64(value, provider).ToString(format, provider);
            }
        }

        /// <summary>
        /// This method converts the value into its float representation.
        /// </summary>
        /// <remarks>
        /// The value is converted by using <see cref="Convert.ToSingle(Object, IFormatProvider)"/> 
        /// as well as <see cref="Single.ToString(IFormatProvider)"/> respectively 
        /// <see cref="Single.ToString(String, IFormatProvider)"/> if <paramref name="format"/> 
        /// is not <c>null</c>.
        /// </remarks>
        /// <param name="value">
        /// The value to be converted.
        /// </param>
        /// <param name="format">
        /// The format to be used.
        /// </param>
        /// <param name="provider">
        /// The format provider to be used.
        /// </param>
        /// <returns>
        /// The string representation of provided value.
        /// </returns>
        /// <exception cref="Exception">
        /// Any other exception thrown by <see cref="Convert"/> is possible as well.
        /// </exception>
        private static String ToSingleString(Object value, String format, IFormatProvider provider)
        {
            if (String.IsNullOrWhiteSpace(format))
            {
                return Convert.ToSingle(value, provider).ToString(provider);
            }
            else
            {
                return Convert.ToSingle(value, provider).ToString(format, provider);
            }
        }

        /// <summary>
        /// This method converts the value into its double representation.
        /// </summary>
        /// <remarks>
        /// The value is converted by using <see cref="Convert.ToDouble(Object, IFormatProvider)"/> 
        /// as well as <see cref="Double.ToString(IFormatProvider)"/> respectively 
        /// <see cref="Double.ToString(String, IFormatProvider)"/> if <paramref name="format"/> 
        /// is not <c>null</c>.
        /// </remarks>
        /// <param name="value">
        /// The value to be converted.
        /// </param>
        /// <param name="format">
        /// The format to be used.
        /// </param>
        /// <param name="provider">
        /// The format provider to be used.
        /// </param>
        /// <returns>
        /// The string representation of provided value.
        /// </returns>
        /// <exception cref="Exception">
        /// Any other exception thrown by <see cref="Convert"/> is possible as well.
        /// </exception>
        private static String ToDoubleString(Object value, String format, IFormatProvider provider)
        {
            if (String.IsNullOrWhiteSpace(format))
            {
                return Convert.ToDouble(value, provider).ToString(provider);
            }
            else
            {
                return Convert.ToDouble(value, provider).ToString(format, provider);
            }
        }

        /// <summary>
        /// This method converts the value into its decimal representation.
        /// </summary>
        /// <remarks>
        /// The value is converted by using <see cref="Convert.ToDecimal(Object, IFormatProvider)"/> 
        /// as well as <see cref="Decimal.ToString(IFormatProvider)"/> respectively 
        /// <see cref="Decimal.ToString(String, IFormatProvider)"/> if <paramref name="format"/> 
        /// is not <c>null</c>.
        /// </remarks>
        /// <param name="value">
        /// The value to be converted.
        /// </param>
        /// <param name="format">
        /// The format to be used.
        /// </param>
        /// <param name="provider">
        /// The format provider to be used.
        /// </param>
        /// <returns>
        /// The string representation of provided value.
        /// </returns>
        /// <exception cref="Exception">
        /// Any other exception thrown by <see cref="Convert"/> is possible as well.
        /// </exception>
        private static String ToDecimalString(Object value, String format, IFormatProvider provider)
        {
            if (String.IsNullOrWhiteSpace(format))
            {
                return Convert.ToDecimal(value, provider).ToString(provider);
            }
            else
            {
                return Convert.ToDecimal(value, provider).ToString(format, provider);
            }
        }

        /// <summary>
        /// This method converts the value into its date/time representation.
        /// </summary>
        /// <remarks>
        /// The value is converted by using <see cref="Convert.ToDateTime(Object, IFormatProvider)"/> 
        /// as well as <see cref="DateTime.ToString(IFormatProvider)"/> respectively 
        /// <see cref="DateTime.ToString(String, IFormatProvider)"/> if <paramref name="format"/> 
        /// is not <c>null</c>.
        /// </remarks>
        /// <param name="value">
        /// The value to be converted.
        /// </param>
        /// <param name="format">
        /// The format to be used.
        /// </param>
        /// <param name="provider">
        /// The format provider to be used.
        /// </param>
        /// <returns>
        /// The string representation of provided value.
        /// </returns>
        /// <exception cref="Exception">
        /// Any other exception thrown by <see cref="Convert"/> is possible as well.
        /// </exception>
        private static String ToDateTimeString(Object value, String format, IFormatProvider provider)
        {
            if (String.IsNullOrWhiteSpace(format))
            {
                return Convert.ToDateTime(value, provider).ToString(provider);
            }
            else
            {
                return Convert.ToDateTime(value, provider).ToString(format, provider);
            }
        }

        /// <summary>
        /// This method converts the value into its GUID representation.
        /// </summary>
        /// <remarks>
        /// The value is converted by using a type-cast operation as well as 
        /// <see cref="Guid.ToString()"/> respectively <see cref="Guid.ToString(String, IFormatProvider)"/> 
        /// if <paramref name="format"/> is not <c>null</c>.
        /// </remarks>
        /// <param name="value">
        /// The value to be converted.
        /// </param>
        /// <param name="format">
        /// The format to be used.
        /// </param>
        /// <param name="provider">
        /// The format provider to be used.
        /// </param>
        /// <returns>
        /// The string representation of provided value in upper cases.
        /// </returns>
        /// <exception cref="Exception">
        /// Any other exception thrown by <see cref="Convert"/> is possible as well.
        /// </exception>
        private static String ToGuidString(Object value, String format, IFormatProvider provider)
        {
            if (String.IsNullOrWhiteSpace(format))
            {
                return ((Guid)value).ToString().ToUpper();
            }
            else
            {
                return ((Guid)value).ToString(format, provider).ToUpper();
            }
        }

        #endregion
    }
}
