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

using System;

namespace Plexdata.LogWriter.Internals.Extensions
{
    /// <summary>
    /// This extension allows to perform date/time conversions.
    /// </summary>
    /// <remarks>
    /// For the moment only a conversion into Unix epoch is possible.
    /// </remarks>
    internal static class DateTimeExtension
    {
        #region Field declaration

        /// <summary>
        /// This field holds the Unix epoch start date.
        /// </summary>
        /// <remarks>
        /// The Unix epoch begins on 1970-01-01 at 00:00:00.
        /// </remarks>
        private static readonly DateTime unixEpochOffset = default(DateTime);

        /// <summary>
        /// This field holds the fraction of milliseconds.
        /// </summary>
        /// <remarks>
        /// One second consists of 1000 milliseconds.
        /// </remarks>
        private static readonly Decimal millisecondFraction = Decimal.MaxValue;

        #endregion

        #region Construction

        /// <summary>
        /// The static class constructor.
        /// </summary>
        /// <remarks>
        /// This constructor does nothing else but the initialization of all static 
        /// fields.
        /// </remarks>
        static DateTimeExtension()
        {
            DateTimeExtension.unixEpochOffset = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            DateTimeExtension.millisecondFraction = 1000.0M;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Converts the <paramref name="timestamp"/> into its Unix epoch representation.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This method converts the <paramref name="timestamp"/> into its Unix epoch representation.
        /// </para>
        /// <para>
        /// But note, if the <paramref name="timestamp"/> is less than 1970-01-01 00:00:00 then the 
        /// <paramref name="timestamp"/> is set to that date. In this case the return value is zero.
        /// </para>
        /// </remarks>
        /// <param name="timestamp">
        /// The date/time value to be converted.
        /// </param>
        /// <returns>
        /// The seconds since the beginning of the Unix epoch (including millisecond als decimal digits).
        /// </returns>
        public static Decimal ToUnixEpoch(this DateTime timestamp)
        {
            if (timestamp.Kind != DateTimeKind.Utc)
            {
                timestamp = timestamp.ToUniversalTime();
            }

            if (timestamp < DateTimeExtension.unixEpochOffset)
            {
                timestamp = DateTimeExtension.unixEpochOffset;
            }

            DateTimeOffset dto = new DateTimeOffset(timestamp);

            Decimal uts = dto.ToUnixTimeSeconds();
            Decimal utm = dto.ToUnixTimeMilliseconds() - (uts * DateTimeExtension.millisecondFraction);

            return (uts + (utm / DateTimeExtension.millisecondFraction));
        }

        #endregion
    }
}
