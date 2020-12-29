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
using System.Collections.Generic;
using System.Text;

namespace Plexdata.LogWriter.Internals.Formatters
{
    /// <summary>
    /// This internal class provides functionality to transform 
    /// logging massages into XML format.
    /// </summary>
    /// <remarks>
    /// Each logging message transformed by this formatter just 
    /// creates a simple XML string.
    /// </remarks>
    internal class XmlFormatter : FormatterBase, ILogEventFormatter
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
        /// is not set to <see cref="LogType.Xml"/>.
        /// </exception>
        /// <seealso cref="FormatterBase(ILoggerSettings)"/>
        public XmlFormatter(ILoggerSettings settings)
            : base(settings)
        {
            if (base.Settings.LogType != LogType.Xml)
            {
                throw new NotSupportedException($"Logging type of {base.Settings.LogType.ToString().ToUpper()} is not supported by this formatter.");
            }
        }

        #endregion

        #region Public methods

        /// <inheritdoc />
        /// <remarks>
        /// Intentionally, the XML formatter does not skip any of the message parts. 
        /// This means that an <c>empty</c> string is used if a particular part is not 
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

            // BUG: UTF-8 may not be correct because which encoding is really used?
            builder.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?><logging><notification>");

            Char split = '\0';

            this.AddKey(builder, nameof(value.Key), value.Key, split);

            this.AddTime(builder, nameof(value.Time), value.Time, split);

            this.AddLevel(builder, nameof(value.Level), value.Level, split);

            this.AddContext(builder, nameof(value.Context), value.Context, split);

            this.AddScope(builder, nameof(value.Scope), value.Scope, split);

            this.AddMessage(builder, nameof(value.Message), value.Message, split);

            this.AddDetails(builder, nameof(value.Details), value.Details, split);

            this.AddException(builder, nameof(value.Exception), value.Exception, split);

            builder.Append("</notification></logging>");
        }

        #endregion

        #region Protected methods

        /// <inheritdoc />
        /// <remarks>
        /// This method is used to format every label. Furthermore, a <c>null</c> 
        /// string is used if a label contains an invalid descriptor. All spaces 
        /// are replaced by dashes. Finally, the resulting label is converted in 
        /// lower cases.
        /// </remarks>
        protected override String ToLabel(String label)
        {
            return (label ?? base.NullValue).Trim().Replace(" ", "-").ToLower();
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
        /// This method takes the value and checks if it includes at least one of the 
        /// characters "less than", "greater than", "double quote", "single quote" or 
        /// "ampersand". Each of those characters is replaced by its XML expression 
        /// if this is the case.
        /// </remarks>
        protected override String ToOutput(String value, Char split)
        {
            if (value == null || value == base.NullValue || String.IsNullOrWhiteSpace(value))
            {
                return String.Empty;
            }

            return value
                 .Replace("&", "&amp;")   // Replace "ampersand" by escaped "ampersand" (must be first).
                 .Replace("<", "&lt;")    // Replace "less than" by escaped "less than".
                 .Replace(">", "&gt;")    // Replace "greater than" by escaped "greater than".
                 .Replace("\"", "&quot;") // Replace "double quote" by escaped "double quote".
                 .Replace("'", "&apos;"); // Replace "single quote" by escaped "single quote".
        }

        /// <inheritdoc />
        /// <remarks>
        /// This method formats each <paramref name="label"/> and 
        /// <paramref name="value"/> combination as XML string. The 
        /// provided <paramref name="split"/> character is not use.
        /// </remarks>
        /// <seealso cref="ToLabel(String)"/>
        /// <seealso cref="ToValue(String, Char)"/>
        protected override String ToOutput(String label, String value, Char split)
        {
            label = this.ToLabel(label);
            value = this.ToValue(value, split);

            if (String.IsNullOrWhiteSpace(value))
            {
                return $"<{label} />";
            }
            else
            {
                return $"<{label}>{value}</{label}>";
            }
        }

        #endregion

        #region Private methods

        /// <summary>
        /// The method puts the <paramref name="key"/> into the string 
        /// <paramref name="builder"/>.
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
        /// The split character is ignored.
        /// </param>
        /// <see cref="FormatterBase.GetKey(Guid)"/>
        /// <seealso cref="ToOutput(String, String, Char)"/>
        private void AddKey(StringBuilder builder, String label, Guid key, Char split)
        {
            builder.Append(this.ToOutput(label, base.GetKey(key), split));
        }

        /// <summary>
        /// The method puts the <paramref name="time"/> into the string 
        /// <paramref name="builder"/>.
        /// </summary>
        /// <remarks>
        /// The time stamp is always appended and cannot be disabled.
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
        /// The split character is ignored.
        /// </param>
        /// <see cref="FormatterBase.GetTime(DateTime)"/>
        /// <seealso cref="ToOutput(String, String, Char)"/>
        private void AddTime(StringBuilder builder, String label, DateTime time, Char split)
        {
            builder.Append(this.ToOutput(label, base.GetTime(time), split));
        }

        /// <summary>
        /// The method puts the <paramref name="level"/> into the string 
        /// <paramref name="builder"/>.
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
        /// The split character is ignored.
        /// </param>
        /// <see cref="FormatterBase.GetLevel(LogLevel)"/>
        /// <seealso cref="ToOutput(String, String, Char)"/>
        private void AddLevel(StringBuilder builder, String label, LogLevel level, Char split)
        {
            builder.Append(this.ToOutput(label, base.GetLevel(level), split));
        }

        /// <summary>
        /// The method puts the <paramref name="context"/> into the string 
        /// <paramref name="builder"/>.
        /// </summary>
        /// <remarks>
        /// The context is always added and cannot be disabled, but might 
        /// be <c>empty</c>.
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
        /// The split character is ignored.
        /// </param>
        /// <see cref="FormatterBase.GetContext(String)"/>
        /// <seealso cref="ToOutput(String, String, Char)"/>
        private void AddContext(StringBuilder builder, String label, String context, Char split)
        {
            builder.Append(this.ToOutput(label, base.GetContext(context, String.Empty), split));
        }

        /// <summary>
        /// The method puts the <paramref name="scope"/> into the string 
        /// <paramref name="builder"/>.
        /// </summary>
        /// <remarks>
        /// The scope is always added and cannot be disabled, but might 
        /// be <c>empty</c>.
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
        /// The split character is ignored.
        /// </param>
        /// <see cref="FormatterBase.GetScope(String)"/>
        /// <seealso cref="ToOutput(String, String, Char)"/>
        private void AddScope(StringBuilder builder, String label, String scope, Char split)
        {
            builder.Append(this.ToOutput(label, base.GetScope(scope, String.Empty), split));
        }

        /// <summary>
        /// The method puts the <paramref name="message"/> into the string 
        /// <paramref name="builder"/>.
        /// </summary>
        /// <remarks>
        /// The message is always added and cannot be disabled, but might 
        /// be <c>empty</c>.
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
        /// The split character is ignored.
        /// </param>
        /// <see cref="FormatterBase.GetMessage(String)"/>
        /// <seealso cref="ToOutput(String, String, Char)"/>
        private void AddMessage(StringBuilder builder, String label, String message, Char split)
        {
            builder.Append(this.ToOutput(label, base.GetMessage(message, String.Empty), split));
        }

        /// <summary>
        /// The method puts the <paramref name="details"/> into the string 
        /// <paramref name="builder"/>.
        /// </summary>
        /// <remarks>
        /// The details are always added and cannot be disabled, but might be 
        /// <c>empty</c>.
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
        /// The split character is ignored.
        /// </param>
        /// <seealso cref="ToLabel(String)"/>
        /// <seealso cref="ToValue(String, Char)"/>
        /// <seealso cref="ToOutput(String, Char)"/>
        /// <see cref="FormatterBase.GetConverted(Object)"/>
        private void AddDetails(StringBuilder builder, String label, (String Label, Object Value)[] details, Char split)
        {
            label = this.ToLabel(label);

            if (details != null && details.Length > 0)
            {
                StringBuilder helper = new StringBuilder(512);

                helper.Append($"<{label}>");

                foreach ((String Label, Object Value) in details)
                {
                    helper.Append(this.ToOutput(Label, base.GetConverted(Value), split));
                }

                helper.Append($"</{label}>");

                builder.Append(helper.ToString());
            }
            else
            {
                builder.Append($"<{label} />");
            }
        }

        /// <summary>
        /// The method puts the <paramref name="exception"/> into the string 
        /// <paramref name="builder"/>.
        /// </summary>
        /// <remarks>
        /// The exception is always added and cannot be disabled, but might 
        /// be <c>empty</c>.
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
        /// The split character is ignored.
        /// </param>
        /// <seealso cref="FormatterBase.GetException(Exception, String)"/>
        /// <seealso cref="ToOutput(String, String, Char)"/>
        private void AddException(StringBuilder builder, String label, Exception exception, Char split)
        {
            builder.Append(this.ToOutput(label, base.GetException(exception, String.Empty), split));
        }

        #endregion
    }
}
