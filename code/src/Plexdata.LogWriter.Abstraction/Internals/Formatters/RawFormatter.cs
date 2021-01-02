/*
 * MIT License
 * 
 * Copyright (c) 2021 plexdata.de
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
    /// massages into raw format.
    /// </summary>
    /// <remarks>
    /// Raw format typically means that such a logging message consists of 
    /// a leading time stamp and is followed by a particular logging level 
    /// as well as a meaningful message. Additionally, such a raw logging 
    /// message may include a message context and/or a message scope. Other 
    /// additional information might be included as well.
    /// </remarks>
    internal class RawFormatter : FormatterBase, ILogEventFormatter
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
        /// is not set to <see cref="LogType.Raw"/>.
        /// </exception>
        /// <seealso cref="FormatterBase(ILoggerSettings)"/>
        public RawFormatter(ILoggerSettings settings)
            : base(settings)
        {
            if (base.Settings.LogType != LogType.Raw)
            {
                throw new NotSupportedException($"Logging type of {base.Settings.LogType.ToString().ToUpper()} is not supported by this formatter.");
            }
        }

        #endregion

        #region Public methods

        /// <inheritdoc />
        /// <remarks>
        /// Intentionally, the raw formatter only supports the timestamp (if configured), 
        /// the logging level, the context (if available), the scope (if available), the 
        /// message (or <c>empty</c>, if not available), the list of values (if available) 
        /// and finally the exception (but only if not <c>null</c>). Keep in mind, an exception 
        /// (if not <c>null</c>) is put into a new line.
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

            this.AddTime(builder, value.Time, split);

            this.AddLevel(builder, value.Level, split);

            this.AddContext(builder, value.Context, split);

            this.AddScope(builder, value.Scope, split);

            this.AddMessage(builder, value.Message, split);

            this.AddDetails(builder, value.Details, split);

            base.TrimEnd(builder, split);

            this.AddException(builder, value.Exception, split);
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
        /// This method just calls method <see cref="ToOutput(String, Char)"/>
        /// </remarks>
        /// <seealso cref="ToOutput(String, Char)"/>
        protected override String ToValue(String value, Char split)
        {
            return this.ToOutput(value, split);
        }

        /// <inheritdoc />
        /// <remarks>
        /// This method simply takes the <paramref name="value"/> an appends the <paramref name="split"/> 
        /// character. An empty string is used, if provided value is <c>null</c>, <c>empty</c> or consists 
        /// only of white spaces.
        /// </remarks>
        protected override String ToOutput(String value, Char split)
        {
            if (String.IsNullOrWhiteSpace(value))
            {
                value = String.Empty;
            }

            return $"{value}{split}";
        }

        /// <inheritdoc />
        /// <remarks>
        /// This method just calls method <see cref="ToOutput(String, Char)"/>
        /// </remarks>
        /// <seealso cref="ToValue(String, Char)"/>
        protected override String ToOutput(String label, String value, Char split)
        {
            return this.ToValue(value, split);
        }

        #endregion

        #region Private methods

        /// <summary>
        /// The method puts the <paramref name="time"/> into the string 
        /// <paramref name="builder"/> and appends the <paramref name="split"/> 
        /// character.
        /// </summary>
        /// <remarks>
        /// The time stamp is only added if it is enabled within current logger settings.
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
        /// <seealso cref="ILoggerSettings.ShowTime"/>
        /// <see cref="FormatterBase.GetTime(DateTime)"/>
        /// <seealso cref="ToOutput(String, Char)"/>
        private void AddTime(StringBuilder builder, DateTime time, Char split)
        {
            if (base.Settings.ShowTime)
            {
                builder.Append(this.ToOutput(base.GetTime(time), split));
            }
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
        /// The context is only appended if it is neither <c>null</c> nor <c>empty</c> 
        /// nor consists only of white spaces.
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
            if (!String.IsNullOrWhiteSpace(context))
            {
                builder.Append(this.ToOutput(base.GetContext(context), split));
            }
        }

        /// <summary>
        /// The method puts the <paramref name="scope"/> into the string 
        /// <paramref name="builder"/> and appends the <paramref name="split"/> 
        /// character.
        /// </summary>
        /// <remarks>
        /// The scope is only appended if it is neither <c>null</c> nor <c>empty</c> 
        /// nor consists only of white spaces.
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
            if (!String.IsNullOrWhiteSpace(scope))
            {
                builder.Append(this.ToOutput(base.GetScope(scope), split));
            }
        }

        /// <summary>
        /// The method puts the <paramref name="message"/> into the string 
        /// <paramref name="builder"/> and appends the <paramref name="split"/> 
        /// character.
        /// </summary>
        /// <remarks>
        /// The message is always appended and cannot be disabled.
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
            builder.Append(this.ToOutput(base.GetMessage(message, base.EmptyValue), split));
        }

        /// <summary>
        /// The method puts the <paramref name="details"/> into the string 
        /// <paramref name="builder"/> and appends the <paramref name="split"/> 
        /// character.
        /// </summary>
        /// <remarks>
        /// The details are only appended if the list is neither <c>null</c> nor <c>empty</c>.
        /// </remarks>
        /// <example>
        /// If a value list is available, then each of them is put into a string with 
        /// surrounding square brackets. Additionally, each label-value-pair is put into 
        /// the result like shown below.
        /// <code>
        /// [Label0=Value0,Label1=Value1,Label2=Value2]
        /// </code>
        /// </example>
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
            if (details != null && details.Length > 0)
            {
                StringBuilder helper = new StringBuilder(512);

                helper.Append("[");

                foreach ((String Label, Object Value) in details)
                {
                    helper.Append($"{this.ToLabel(Label)}={this.ToValue(base.GetConverted(Value), split)}");
                }

                base.TrimEnd(helper, split);

                helper.Append("]");

                builder.Append(this.ToOutput(helper.ToString(), split));
            }
        }

        /// <summary>
        /// The method puts the <paramref name="exception"/> into the string 
        /// <paramref name="builder"/> but does not append the <paramref name="split"/> 
        /// character.
        /// </summary>
        /// <remarks>
        /// The exception is only appended if it is not <c>null</c>. Furthermore, a new 
        /// line is prepended.
        /// </remarks>
        /// <param name="builder">
        /// The string builder to be used.
        /// </param>
        /// <param name="exception"></param>
        /// <param name="split">
        /// The split character is not used at the moment.
        /// </param>
        /// <seealso cref="FormatterBase.GetException(Exception, String)"/>
        private void AddException(StringBuilder builder, Exception exception, Char split)
        {
            if (exception != null)
            {
                builder.Append($"{Environment.NewLine}{base.GetException(exception)}");
            }
        }

        #endregion
    }
}
