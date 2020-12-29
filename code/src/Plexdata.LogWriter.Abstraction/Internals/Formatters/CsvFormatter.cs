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

using Plexdata.LogWriter.Abstraction;
using Plexdata.LogWriter.Definitions;
using System;
using System.Text;

namespace Plexdata.LogWriter.Internals.Formatters
{
    /// <summary>
    /// This internal class provides functionality to transform 
    /// logging massages into CSV format.
    /// </summary>
    /// <remarks>
    /// Each logging message transformed by this formatter creates 
    /// a string that is compatible to RFC 4180.
    /// </remarks>
    internal class CsvFormatter : FormatterBase, ILogEventFormatter
    {
        #region Construction

        /// <summary>
        /// The parameterized constructor just initializes all properties.
        /// </summary>
        /// <remarks>
        /// Formatting data depends on some logger settings. Therefore, consumers 
        /// of this class must provide an instance of <see cref="ILoggerSettings"/>.
        /// </remarks>
        /// <param name="settings">
        /// The settings to be used for data formatting.
        /// </param>
        /// <exception cref="NotSupportedException">
        /// This exception is thrown if the logging type within provided settings 
        /// is not set to <see cref="LogType.Csv"/>.
        /// </exception>
        /// <seealso cref="FormatterBase(ILoggerSettings)"/>
        public CsvFormatter(ILoggerSettings settings)
            : base(settings)
        {
            if (base.Settings.LogType != LogType.Csv)
            {
                throw new NotSupportedException($"Logging type of {base.Settings.LogType.ToString().ToUpper()} is not supported by this formatter.");
            }
        }

        #endregion

        #region Public methods

        /// <inheritdoc />
        /// <remarks>
        /// Intentionally, the CSV formatter does not skip any of the message parts. 
        /// This means that the <c>null</c> string or an empty string is used if a 
        /// particular part is not valid.
        /// </remarks>
        /// <exception cref="ArgumentNullException">
        /// This exception is thrown as soon as one the parameters is <c>null</c>.
        /// </exception>
        public void Format(StringBuilder builder, ILogEvent value)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            builder.Clear();

            Char split = base.Settings.PartSplit;

            this.AddKey(builder, value.Key, split);

            this.AddTime(builder, value.Time, split);

            this.AddLevel(builder, value.Level, split);

            this.AddContext(builder, value.Context, split);

            this.AddScope(builder, value.Scope, split);

            this.AddMessage(builder, value.Message, split);

            this.AddDetails(builder, value.Details, split);

            this.AddException(builder, value.Exception, split);

            base.TrimEnd(builder, split); // RFC 4180 does not include a terminating split character.
        }

        #endregion

        #region Protected methods

        /// <inheritdoc />
        /// <remarks>
        /// This method is only used for value formatting. Furthermore, a <c>null</c> 
        /// string is used if a label-value-pair contains an invalid label text.
        /// </remarks>
        protected override String ToLabel(String label)
        {
            return (label ?? base.NullValue).Trim();
        }

        /// <inheritdoc />
        /// <remarks>
        /// This method is only used for value formatting. Furthermore, a <c>null</c> 
        /// string is used if a label-value-pair contains an invalid label text.
        /// </remarks>
        protected override String ToValue(String value, Char split)
        {
            return (value ?? base.NullValue).Trim();
        }

        /// <inheritdoc />
        /// <remarks>
        /// This method takes the value and checks if it includes at least one double 
        /// quote character or one carriage return character or one line feed character 
        /// or one of the provided split character. If this the case then the whole 
        /// value is surrounded by double quotes. Additionally, each of the double 
        /// quotes within the value are escaped according to the rules of RFC 4180.
        /// </remarks>
        protected override String ToOutput(String value, Char split)
        {
            const Char DQ = '"';  // Double Quote 
            const Char CR = '\r'; // Carriage Return
            const Char LF = '\n'; // Line Feed

            if (String.IsNullOrWhiteSpace(value))
            {
                value = String.Empty;
            }

            if (value.IndexOfAny(new Char[] { DQ, CR, LF, split }) >= 0)
            {
                if (value.IndexOf(DQ) >= 0)
                {
                    value = value.Replace($"{DQ}", $"{DQ}{DQ}");
                }

                return $"{DQ}{value}{DQ}{split}";
            }
            else
            {
                return $"{value}{split}";
            }
        }

        /// <inheritdoc />
        /// <remarks>
        /// This method just calls method <see cref="ToOutput(String, Char)"/>
        /// </remarks>
        /// <seealso cref="ToValue(String, Char)"/>
        protected override String ToOutput(String label, String value, Char split)
        {
            return this.ToOutput(value, split);
        }

        #endregion

        #region Private methods

        /// <summary>
        /// The method puts the <paramref name="key"/> into the string 
        /// <paramref name="builder"/> and appends the <paramref name="split"/> 
        /// character.
        /// </summary>
        /// <remarks>
        /// The key is always appended and cannot be disabled.
        /// </remarks>
        /// <param name="builder">
        /// The string builder to be used.
        /// </param>
        /// <param name="key">
        /// The key to be used.
        /// </param>
        /// <param name="split">
        /// The split character to be appended.
        /// </param>
        /// <see cref="FormatterBase.GetKey(Guid)"/>
        /// <seealso cref="ToOutput(String, Char)"/>
        private void AddKey(StringBuilder builder, Guid key, Char split)
        {
            builder.Append(this.ToOutput(base.GetKey(key), split));
        }

        /// <summary>
        /// The method puts the <paramref name="time"/> into the string 
        /// <paramref name="builder"/> and appends the <paramref name="split"/> 
        /// character.
        /// </summary>
        /// <remarks>
        /// The time stamp is always appended and cannot be disabled.
        /// </remarks>
        /// <param name="builder">
        /// The string builder to be used.
        /// </param>
        /// <param name="time">
        /// The time stamp to be used.
        /// </param>
        /// <param name="split">
        /// The split character to be appended.
        /// </param>
        /// <see cref="FormatterBase.GetTime(DateTime)"/>
        /// <seealso cref="ToOutput(String, Char)"/>
        private void AddTime(StringBuilder builder, DateTime time, Char split)
        {
            builder.Append(this.ToOutput(base.GetTime(time), split));
        }

        /// <summary>
        /// The method puts the <paramref name="level"/> into the string 
        /// <paramref name="builder"/> and appends the <paramref name="split"/> 
        /// character.
        /// </summary>
        /// <remarks>
        /// The logging level is always appended and cannot be disabled.
        /// </remarks>
        /// <param name="builder">
        /// The string builder to be used.
        /// </param>
        /// <param name="level">
        /// The logging level to be appended.
        /// </param>
        /// <param name="split">
        /// The split character to be appended.
        /// </param>
        /// <see cref="FormatterBase.GetLevel(LogLevel)"/>
        /// <seealso cref="ToOutput(String, Char)"/>
        private void AddLevel(StringBuilder builder, LogLevel level, Char split)
        {
            builder.Append(this.ToOutput(base.GetLevel(level), split));
        }

        /// <summary>
        /// The method puts the <paramref name="context"/> into the string 
        /// <paramref name="builder"/> and appends the <paramref name="split"/> 
        /// character.
        /// </summary>
        /// <remarks>
        /// The context is always appended and cannot be disabled, but might be empty.
        /// </remarks>
        /// <param name="builder">
        /// The string builder to be used.
        /// </param>
        /// <param name="context">
        /// The context to be appended.
        /// </param>
        /// <param name="split">
        /// The split character to be appended.
        /// </param>
        /// <see cref="FormatterBase.GetContext(String)"/>
        /// <seealso cref="ToOutput(String, Char)"/>
        private void AddContext(StringBuilder builder, String context, Char split)
        {
            builder.Append(this.ToOutput(base.GetContext(context, String.Empty), split));
        }

        /// <summary>
        /// The method puts the <paramref name="scope"/> into the string 
        /// <paramref name="builder"/> and appends the <paramref name="split"/> 
        /// character.
        /// </summary>
        /// <remarks>
        /// The scope is always appended and cannot be disabled, but might be empty.
        /// </remarks>
        /// <param name="builder">
        /// The string builder to be used.
        /// </param>
        /// <param name="scope">
        /// The scope to be appended.
        /// </param>
        /// <param name="split">
        /// The split character to be appended.
        /// </param>
        /// <see cref="FormatterBase.GetScope(String)"/>
        /// <seealso cref="ToOutput(String, Char)"/>
        private void AddScope(StringBuilder builder, String scope, Char split)
        {
            builder.Append(this.ToOutput(base.GetScope(scope, String.Empty), split));
        }

        /// <summary>
        /// The method puts the <paramref name="message"/> into the string 
        /// <paramref name="builder"/> and appends the <paramref name="split"/> 
        /// character.
        /// </summary>
        /// <remarks>
        /// The message is always appended and cannot be disabled, but might be empty.
        /// </remarks>
        /// <param name="builder">
        /// The string builder to be used.
        /// </param>
        /// <param name="message">
        /// The message to be appended.
        /// </param>
        /// <param name="split">
        /// The split character to be appended.
        /// </param>
        /// <see cref="FormatterBase.GetMessage(String)"/>
        /// <seealso cref="ToOutput(String, Char)"/>
        private void AddMessage(StringBuilder builder, String message, Char split)
        {
            builder.Append(this.ToOutput(base.GetMessage(message, String.Empty), split));
        }

        /// <summary>
        /// The method puts the <paramref name="details"/> into the string 
        /// <paramref name="builder"/> and appends the <paramref name="split"/> 
        /// character.
        /// </summary>
        /// <remarks>
        /// <para>
        /// The details are always appended and cannot be disabled, but might be empty.
        /// </para>
        /// <para>
        /// If a value list is available, then each of them is put into a string with 
        /// surrounding square brackets. Each of the label-value-pairs is separated by 
        /// one comma. 
        /// </para>
        /// </remarks>
        /// <param name="builder">
        /// The string builder to be used.
        /// </param>
        /// <param name="details">
        /// The details to be appended.
        /// </param>
        /// <param name="split">
        /// The split character to be appended.
        /// </param>
        /// <seealso cref="ToLabel(String)"/>
        /// <seealso cref="ToValue(String, Char)"/>
        /// <seealso cref="ToOutput(String, Char)"/>
        /// <see cref="FormatterBase.GetConverted(Object)"/>
        private void AddDetails(StringBuilder builder, (String Label, Object Value)[] details, Char split)
        {
            StringBuilder helper = new StringBuilder(512);

            if (details != null && details.Length > 0)
            {
                Char temp = ',';

                foreach ((String Label, Object Value) in details)
                {
                    helper.Append($"[{this.ToLabel(Label)}={this.ToValue(base.GetConverted(Value), '\0')}]{temp}");
                }

                base.TrimEnd(helper, temp);
            }

            builder.Append(this.ToOutput(helper.ToString(), split));
        }

        /// <summary>
        /// The method puts the <paramref name="exception"/> into the string 
        /// <paramref name="builder"/> and adds the <paramref name="split"/> 
        /// character.
        /// </summary>
        /// <remarks>
        /// The exception is always appended and cannot be disabled, but might be empty.
        /// </remarks>
        /// <param name="builder">
        /// The string builder to be used.
        /// </param>
        /// <param name="exception">
        /// The exception to be used.
        /// </param>
        /// <param name="split">
        /// The split character to be used.
        /// </param>
        /// <seealso cref="FormatterBase.GetException(Exception, String)"/>
        /// <seealso cref="ToOutput(String, Char)"/>
        private void AddException(StringBuilder builder, Exception exception, Char split)
        {
            builder.Append(this.ToOutput(base.GetException(exception, String.Empty), split));
        }

        #endregion
    }
}
