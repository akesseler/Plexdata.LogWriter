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
    /// This internal class provides functionality to transform logging 
    /// massages into JSON format.
    /// </summary>
    /// <remarks>
    /// <para>
    /// JSON (JavaScript Object Notation) is a format that allows to share objects 
    /// between different programs. Against this background, the JSON formatter can 
    /// be used to transform logging messages in this special data format. For more 
    /// information about JSON see under https://www.json.org.
    /// </para>
    /// <para>
    /// For the moment, all values in the output (except null values) are treated 
    /// as string. This means in detail they are surrounded by double quotes.
    /// </para>
    /// </remarks>
    internal class JsonFormatter : FormatterBase, ILogEventFormatter
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
        /// is not set to <see cref="LogType.Json"/>.
        /// </exception>
        /// <seealso cref="FormatterBase(ILoggerSettings)"/>
        public JsonFormatter(ILoggerSettings settings)
            : base(settings)
        {
            if (base.Settings.LogType != LogType.Json)
            {
                throw new NotSupportedException($"Logging type of {base.Settings.LogType.ToString().ToUpper()} is not supported by this formatter.");
            }
        }

        #endregion

        #region Public methods

        /// <inheritdoc />
        /// <remarks>
        /// Intentionally, the JSON formatter does not skip any of the message parts. 
        /// This means that the <c>null</c> string is used if a particular part is not 
        /// valid.
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

            builder.Append("{");

            Char split = ',';

            this.AddKey(builder, nameof(value.Key), value.Key, split);

            this.AddTime(builder, nameof(value.Time), value.Time, split);

            this.AddLevel(builder, nameof(value.Level), value.Level, split);

            this.AddContext(builder, nameof(value.Context), value.Context, split);

            this.AddScope(builder, nameof(value.Scope), value.Scope, split);

            this.AddMessage(builder, nameof(value.Message), value.Message, split);

            this.AddDetails(builder, nameof(value.Details), value.Details, split);

            this.AddException(builder, nameof(value.Exception), value.Exception, split);

            base.TrimEnd(builder, split);

            builder.Append("}");
        }

        #endregion

        #region Protected methods

        /// <inheritdoc />
        /// <remarks>
        /// This method is used to format every label. Furthermore, a <c>null</c> 
        /// string is used if a label contains an invalid descriptor.
        /// </remarks>
        protected override String ToLabel(String label)
        {
            return $"\"{(label ?? base.NullValue).Trim()}\"";
        }

        /// <inheritdoc />
        /// <remarks>
        /// This method simply calls method <see cref="ToOutput(String, Char)"/>
        /// </remarks>
        protected override String ToValue(String value, Char split)
        {
            return this.ToOutput(value, split);
        }

        /// <inheritdoc />
        /// <remarks>
        /// <para>
        /// This method takes the value and checks if it includes at least one the 
        /// characters backslash, double quote, carriage return, line feed, form 
        /// feed, backspace or tab. If this is the case then each of those control 
        /// characters is escaped by a backslash. Additionally, each result string 
        /// (except for null values) is surrounded by double quotes.
        /// </para>
        /// <list type="bullet">
        /// <item><description>
        /// If <paramref name="value"/> is <c>null</c> the method returns a null 
        /// string without double quotes.
        /// </description></item>
        /// <item><description>
        /// If <paramref name="value"/> contains the string null the method returns 
        /// a null string without double quotes.
        /// </description></item>
        /// <item><description>
        /// If <paramref name="value"/> is empty or whitespace the method returns an 
        /// empty string including double quotes.
        /// </description></item>
        /// </list>
        /// </remarks>
        protected override String ToOutput(String value, Char split)
        {
            if (value == null || value == base.NullValue)
            {
                return $"{base.NullValue}";
            }

            if (String.IsNullOrWhiteSpace(value))
            {
                return $"\"{String.Empty}\"";
            }

            // BUG: There is still a bug somewhere. The position of a backspace affects, but it shouldn't.
            value = value
                .Replace("\\", "\\\\") // Replace backslash by escaped backslash.
                .Replace("\"", "\\\"") // Replace double quote by escaped double quote.
                .Replace("\r", "\\r")  // Replace carriage return by escaped carriage return.
                .Replace("\n", "\\n")  // Replace line feed by escaped line feed.
                .Replace("\f", "\\f")  // Replace form feed by escaped form feed.
                .Replace("\b", "\\b")  // Replace backspace by escaped backspace.
                .Replace("\t", "\\t")  // Replace tab by escaped tab.
                .Replace("\\", "\\\\");// Re-escape any of already escaped backslashes (bugfix).

            return $"\"{value}\"";
        }

        /// <inheritdoc />
        /// <remarks>
        /// This method formats each <paramref name="label"/> and 
        /// <paramref name="value"/> combination and separates them 
        /// by a colon. Finally, the provided <paramref name="split"/> 
        /// character is appended.
        /// </remarks>
        /// <seealso cref="ToLabel(String)"/>
        /// <seealso cref="ToValue(String, Char)"/>
        protected override String ToOutput(String label, String value, Char split)
        {
            return $"{this.ToLabel(label)}:{this.ToValue(value, split)}{split}";
        }

        #endregion

        #region Private methods

        /// <summary>
        /// The method puts the <paramref name="key"/> into the string 
        /// <paramref name="builder"/> and adds the <paramref name="split"/> 
        /// character.
        /// </summary>
        /// <remarks>
        /// The key is always added and cannot be disabled.
        /// </remarks>
        /// <param name="builder">
        /// The string builder to be used.
        /// </param>
        /// <param name="label">
        /// The value label to be used.
        /// </param>
        /// <param name="key">
        /// The key to be used.
        /// </param>
        /// <param name="split">
        /// The split character to be appended.
        /// </param>
        /// <see cref="FormatterBase.GetKey(Guid)"/>
        /// <seealso cref="ToOutput(String, String, Char)"/>
        private void AddKey(StringBuilder builder, String label, Guid key, Char split)
        {
            builder.Append(this.ToOutput(label, base.GetKey(key), split));
        }

        /// <summary>
        /// The method puts the <paramref name="time"/> into the string 
        /// <paramref name="builder"/> and adds the <paramref name="split"/> 
        /// character.
        /// </summary>
        /// <remarks>
        /// The time stamp is always added and cannot be disabled. Keep in 
        /// mind, the time format within the settings is ignored in any case!
        /// </remarks>
        /// <param name="builder">
        /// The string builder to be used.
        /// </param>
        /// <param name="label">
        /// The value label to be used.
        /// </param>
        /// <param name="time">
        /// The time stamp to be used.
        /// </param>
        /// <param name="split">
        /// The split character to be appended.
        /// </param>
        /// <see cref="FormatterBase.GetTime(DateTime)"/>
        /// <seealso cref="ToOutput(String, String, Char)"/>
        private void AddTime(StringBuilder builder, String label, DateTime time, Char split)
        {
            if (time.Kind == DateTimeKind.Unspecified)
            {
                DateTime.SpecifyKind(time, base.Settings.LogTime == LogTime.Utc ? DateTimeKind.Utc : DateTimeKind.Local);
            }

            String format = base.Settings.TimeFormat;

            // It doesn't make any sense to support a user-defined date/time format in this context. 
            // Therefore, replace current date/time format by UTC format with time-zone information.
            base.Settings.TimeFormat = "O"; // => Same as format "yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fffffffK" 

            builder.Append(this.ToOutput(label, base.GetTime(time), split));

            base.Settings.TimeFormat = format;
        }

        /// <summary>
        /// The method puts the <paramref name="level"/> into the string 
        /// <paramref name="builder"/> and adds the <paramref name="split"/> 
        /// character.
        /// </summary>
        /// <remarks>
        /// The logging level is always added and cannot be disabled.
        /// </remarks>
        /// <param name="builder">
        /// The string builder to be used.
        /// </param>
        /// <param name="label">
        /// The value label to be used.
        /// </param>
        /// <param name="level">
        /// The logging level to be appended.
        /// </param>
        /// <param name="split">
        /// The split character to be appended.
        /// </param>
        /// <see cref="FormatterBase.GetLevel(LogLevel)"/>
        /// <seealso cref="ToOutput(String, String, Char)"/>
        private void AddLevel(StringBuilder builder, String label, LogLevel level, Char split)
        {
            builder.Append(this.ToOutput(label, base.GetLevel(level), split));
        }

        /// <summary>
        /// The method puts the <paramref name="context"/> into the string 
        /// <paramref name="builder"/> and adds the <paramref name="split"/> 
        /// character.
        /// </summary>
        /// <remarks>
        /// The context is always added and cannot be disabled, but might be <c>null</c>.
        /// </remarks>
        /// <param name="builder">
        /// The string builder to be used.
        /// </param>
        /// <param name="label">
        /// The value label to be used.
        /// </param>
        /// <param name="context">
        /// The context to be appended.
        /// </param>
        /// <param name="split">
        /// The split character to be appended.
        /// </param>
        /// <see cref="FormatterBase.GetContext(String)"/>
        /// <seealso cref="ToOutput(String, String, Char)"/>
        private void AddContext(StringBuilder builder, String label, String context, Char split)
        {
            builder.Append(this.ToOutput(label, base.GetContext(context, base.NullValue), split));
        }

        /// <summary>
        /// The method puts the <paramref name="scope"/> into the string 
        /// <paramref name="builder"/> and adds the <paramref name="split"/> 
        /// character.
        /// </summary>
        /// <remarks>
        /// The scope is always added and cannot be disabled, but might be <c>null</c>.
        /// </remarks>
        /// <param name="builder">
        /// The string builder to be used.
        /// </param>
        /// <param name="label">
        /// The value label to be used.
        /// </param>
        /// <param name="scope">
        /// The scope to be appended.
        /// </param>
        /// <param name="split">
        /// The split character to be appended.
        /// </param>
        /// <see cref="FormatterBase.GetScope(String)"/>
        /// <seealso cref="ToOutput(String, String, Char)"/>
        private void AddScope(StringBuilder builder, String label, String scope, Char split)
        {
            builder.Append(this.ToOutput(label, base.GetScope(scope, base.NullValue), split));
        }

        /// <summary>
        /// The method puts the <paramref name="message"/> into the string 
        /// <paramref name="builder"/> and adds the <paramref name="split"/> 
        /// character.
        /// </summary>
        /// <remarks>
        /// The message is always added and cannot be disabled, but might be <c>null</c>.
        /// </remarks>
        /// <param name="builder">
        /// The string builder to be used.
        /// </param>
        /// <param name="label">
        /// The value label to be used.
        /// </param>
        /// <param name="message">
        /// The message to be appended.
        /// </param>
        /// <param name="split">
        /// The split character to be appended.
        /// </param>
        /// <see cref="FormatterBase.GetMessage(String)"/>
        /// <seealso cref="ToOutput(String, String, Char)"/>
        private void AddMessage(StringBuilder builder, String label, String message, Char split)
        {
            builder.Append(this.ToOutput(label, base.GetMessage(message, base.NullValue), split));
        }

        /// <summary>
        /// The method puts the <paramref name="details"/> into the string 
        /// <paramref name="builder"/> and adds the <paramref name="split"/> 
        /// character.
        /// </summary>
        /// <remarks>
        /// <para>
        /// The details are always added and cannot be disabled, but might be <c>empty</c>.
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
        /// <param name="label">
        /// The value label to be used.
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
        private void AddDetails(StringBuilder builder, String label, (String Label, Object Value)[] details, Char split)
        {
            if (details != null && details.Length > 0)
            {
                StringBuilder helper = new StringBuilder(512);

                helper.Append("[");

                foreach ((String Label, Object Value) in details)
                {
                    helper.Append($"{{{this.ToLabel(Label)}:{this.ToValue(base.GetConverted(Value), split)}}}{split}");
                }

                base.TrimEnd(helper, split);

                helper.Append("]");

                builder.Append($"{this.ToLabel(label)}:{helper.ToString()}{split}");
            }
            else
            {
                builder.Append(this.ToOutput(label, base.NullValue, split));
            }
        }

        /// <summary>
        /// The method puts the <paramref name="exception"/> into the string 
        /// <paramref name="builder"/> and adds the <paramref name="split"/> 
        /// character.
        /// </summary>
        /// <remarks>
        /// The exception is always added and cannot be disabled, but might be <c>null</c>.
        /// </remarks>
        /// <param name="builder">
        /// The string builder to be used.
        /// </param>
        /// <param name="label">
        /// The value label to be used.
        /// </param>
        /// <param name="exception">
        /// The exception to be used.
        /// </param>
        /// <param name="split">
        /// The split character to be used.
        /// </param>
        /// <seealso cref="FormatterBase.GetException(Exception, String)"/>
        /// <seealso cref="ToOutput(String, String, Char)"/>
        private void AddException(StringBuilder builder, String label, Exception exception, Char split)
        {
            builder.Append(this.ToOutput(label, base.GetException(exception, base.NullValue), split));
        }

        #endregion
    }
}
