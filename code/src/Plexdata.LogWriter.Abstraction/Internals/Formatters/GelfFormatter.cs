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

using Plexdata.LogWriter.Abstraction;
using Plexdata.LogWriter.Definitions;
using Plexdata.LogWriter.Internals.Extensions;
using Plexdata.LogWriter.Internals.Facades;
using System;
using System.Globalization;
using System.Text;

namespace Plexdata.LogWriter.Internals.Formatters
{
    /// <summary>
    /// This internal class provides functionality to transform 
    /// logging massages into GELF format.
    /// </summary>
    /// <remarks>
    /// Each logging message transformed by this formatter just 
    /// creates a GELF string. For more information about GELF 
    /// see under https://go2docs.graylog.org/5-0/getting_in_log_data/gelf.html.
    /// </remarks>
    internal class GelfFormatter : FormatterBase, ILogEventFormatter
    {
        #region Private fields

        /// <summary>
        /// This field holds the instance of IResolverFacade.
        /// </summary>
        /// <remarks>
        /// The resolver is used to determine some system 
        /// details such as the local host name.
        /// </remarks>
        private readonly IResolverFacade resolver;

        #endregion

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
        /// <param name="resolver">
        /// The resolver instance to determine local host name.
        /// </param>
        /// <exception cref="NotSupportedException">
        /// This exception is thrown if the logging type within provided settings 
        /// is not set to <see cref="LogType.Gelf"/>.
        /// </exception>
        /// <seealso cref="FormatterBase(ILoggerSettings)"/>
        public GelfFormatter(ILoggerSettings settings, IResolverFacade resolver)
            : base(settings)
        {
            if (base.Settings.LogType != LogType.Gelf)
            {
                throw new NotSupportedException($"Logging type of {base.Settings.LogType.ToString().ToUpper()} is not supported by this formatter.");
            }

            this.resolver = resolver ?? throw new ArgumentNullException(nameof(resolver), "Resolver facade must not be null.");
        }

        #endregion

        #region Public methods

        /// <inheritdoc />
        /// <remarks>
        /// <para>
        /// The GELF formatter turns parameter <paramref name="value"/> into a valid GELF message consisting 
        /// at least of fields <c>version</c>, <c>host</c>, <c>short_message</c> and <c>level</c>. Each logging 
        /// level is converted into its corresponding <em>Syslog Severity Level</em> automatically.
        /// </para>
        /// <para>
        /// Optional field <c>timestamp</c> can be used by enabling property <see cref="ILoggerSettings.ShowTime"/>. 
        /// Indeed, the usage of <c>timestamp</c> at client side is recommended.
        /// </para>
        /// <para>
        /// Optional field <c>full_message</c> is used as <em>message text</em> only if <c>short_message</c> 
        /// contains a template. However, a usage of templates is not yet supported. This in turn means that 
        /// <c>full_message</c> remains empty for the moment.
        /// </para>
        /// </remarks>
        /// <exception cref="ArgumentNullException">
        /// This exception is thrown as soon as one the parameters is <c>null</c>.
        /// </exception>
        /// <seealso cref="ILoggerSettings.ShowKey"/>
        /// <seealso cref="ILoggerSettings.ShowTime"/>
        /// <seealso cref="DateTimeExtension.ToUnixEpoch(DateTime)"/>
        /// <seealso cref="LogLevelExtension.ToUnixSeverityLevel(LogLevel)"/>
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

            // Method "CreateOutput()" in "LoggerBase" checks for "ILogEvent.IsValid" and
            // therefore each "ILogEvent.Message" should never be invalid. Other formatters
            // do it as well and ensure a valid message by calling "base.GetMessage()".

            this.AddVersion(builder, split);

            this.AddHost(builder, split);

            this.AddShortMessage(builder, value.Template, value.Message, split);

            this.AddFullMessage(builder, value.Template, value.Message, split);

            this.AddTimestamp(builder, value.Time, split);

            this.AddLevel(builder, value.Level, split);

            this.AddKey(builder, nameof(value.Key), value.Key, split);

            this.AddContext(builder, nameof(value.Context), value.Context, split);

            this.AddScope(builder, nameof(value.Scope), value.Scope, split);

            this.AddException(builder, nameof(value.Exception), value.Exception, split);

            this.AddDetails(builder, value.Details, split);

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
        /// This method is not used and throws an exception.
        /// </remarks>
        /// <exception cref="NotSupportedException">
        /// This method is not used and throws this exception.
        /// </exception>
        protected override String ToValue(String value, Char split)
        {
            throw new NotSupportedException();
        }

        /// <inheritdoc />
        /// <remarks>
        /// This method is not used and throws an exception.
        /// </remarks>
        /// <exception cref="NotSupportedException">
        /// This method is not used and throws this exception.
        /// </exception>
        protected override String ToOutput(String value, Char split)
        {
            throw new NotSupportedException();
        }

        /// <inheritdoc />
        /// <remarks>
        /// This method is not used and throws an exception.
        /// </remarks>
        /// <exception cref="NotSupportedException">
        /// This method is not used and throws this exception.
        /// </exception>
        protected override String ToOutput(String label, String value, Char split)
        {
            throw new NotSupportedException();
        }

        #endregion

        #region Private methods

        /// <summary>
        /// This method prepares provided <paramref name="value"/> for output.
        /// </summary>
        /// <remarks>
        /// The value is converted on current needs.
        /// </remarks>
        /// <param name="value">
        /// The value to be prepared.
        /// </param>
        /// <param name="plain">
        /// If <c>false</c> the value is surrounded by <em>double quotes</em> 
        /// and otherwise not.
        /// </param>
        /// <returns>
        /// The value prepared for output.
        /// </returns>
        /// <seealso cref="ToOutput(String, Boolean)"/>
        private String ToValue(String value, Boolean plain)
        {
            return this.ToOutput(value, plain);
        }

        /// <summary>
        /// This method converts <paramref name="value"/> into its output representation.
        /// </summary>
        /// <remarks>
        /// Provided value is manipulated to (hopefully) fit all requirements of RFC 7159.
        /// This for example means that backslashes, double quotes, tabulators, etc. are 
        /// escaped accordingly.
        /// </remarks>
        /// <param name="value">
        /// The value to be converted.
        /// </param>
        /// <param name="plain">
        /// If <c>false</c> the value is surrounded by <em>double quotes</em> 
        /// and otherwise not.
        /// </param>
        /// <returns>
        /// Output representation of provided value.
        /// </returns>
        private String ToOutput(String value, Boolean plain)
        {
            if (value == null || value == base.NullValue)
            {
                return String.Format(this.GetFormat(plain), base.NullValue);
            }

            if (String.IsNullOrWhiteSpace(value))
            {
                return "\"\"";
            }

            // BUG: There is still a bug somewhere. The position of a backspace affects, but it shouldn't.
            value = value
                .Replace(this.resolver.GetNewLine(), "\n")
                .Replace("\\", "\\\\") // Replace backslash by escaped backslash.
                .Replace("\"", "\\\"") // Replace double quote by escaped double quote.
                .Replace("\r", "\\r")  // Replace carriage return by escaped carriage return.
                .Replace("\n", "\\n")  // Replace line feed by escaped line feed.
                .Replace("\f", "\\f")  // Replace form feed by escaped form feed.
                .Replace("\b", "\\b")  // Replace backspace by escaped backspace.
                .Replace("\t", "\\t")  // Replace tab by escaped tab.
                .Replace("\\", "\\\\");// Re-escape any of already escaped backslashes (bugfix).

            return String.Format(this.GetFormat(plain), value);
        }

        /// <summary>
        /// This method converts <paramref name="label"/> and <paramref name="value"/> 
        /// into its output representation.
        /// </summary>
        /// <remarks>
        /// This method formats each <paramref name="label"/> and <paramref name="value"/> 
        /// combination and separates them by a colon. Finally, the provided <paramref name="split"/> 
        /// character is added.
        /// </remarks>
        /// <param name="label">
        /// The label to be converted.
        /// </param>
        /// <param name="value">
        /// The value to be converted.
        /// </param>
        /// <param name="split">
        /// The character to append as delimiter for label and value.
        /// </param>
        /// <param name="plain">
        /// If <c>false</c> the value is surrounded by <em>double quotes</em> 
        /// and otherwise not.
        /// </param>
        /// <returns>
        /// Output representation of provided label and value.
        /// </returns>
        /// <seealso cref="ToLabel(String)"/>
        /// <seealso cref="ToValue(String, Boolean)"/>
        private String ToOutput(String label, String value, Char split, Boolean plain)
        {
            return $"{this.ToLabel(label)}:{this.ToValue(value, plain)}{split}";
        }

        /// <summary>
        /// Adds a combination of label and value to the string builder.
        /// </summary>
        /// <remarks>
        /// This method adds the combination of label and value to the string builder.
        /// </remarks>
        /// <param name="builder">
        /// The string builder to be used.
        /// </param>
        /// <param name="label">
        /// The label to be used.
        /// </param>
        /// <param name="value">
        /// The value to be used.
        /// </param>
        /// <param name="split">
        /// The split character to be added.
        /// </param>
        /// <param name="plain">
        /// If <c>false</c> the value is surrounded by <em>double quotes</em> 
        /// and otherwise not.
        /// </param>
        /// <see cref="ToOutput(String, String, Char, Boolean)"/>
        private void AddValue(StringBuilder builder, String label, String value, Char split, Boolean plain)
        {
            builder.Append(this.ToOutput(label, value, split, plain));
        }

        /// <summary>
        /// Adds field <c>version</c> to the string builder.
        /// </summary>
        /// <remarks>
        /// The <c>version</c> field is always added with surrounding double quotes 
        /// and the version value is always set to <em>1.1</em>.
        /// </remarks>
        /// <param name="builder">
        /// The string builder to be used.
        /// </param>
        /// <param name="split">
        /// The split character to be added.
        /// </param>
        private void AddVersion(StringBuilder builder, Char split)
        {
            this.AddValue(builder, "version ", "1.1", split, false);
        }

        /// <summary>
        /// Adds field <c>host</c> to the string builder.
        /// </summary>
        /// <remarks>
        /// The <c>host</c> field is always added with surrounding double quotes 
        /// and its value depends on current machine name.
        /// </remarks>
        /// <param name="builder">
        /// The string builder to be used.
        /// </param>
        /// <param name="split">
        /// The split character to be added.
        /// </param>
        /// <seealso cref="IResolverFacade.GetLocalHostName()"/>
        private void AddHost(StringBuilder builder, Char split)
        {
            this.AddValue(builder, "host", this.resolver.GetLocalHostName(), split, false);
        }

        /// <summary>
        /// Adds field <c>short_message</c> to the string builder.
        /// </summary>
        /// <remarks>
        /// This field either contains the message template or the message text 
        /// depending on current value state.
        /// </remarks>
        /// <param name="builder">
        /// The string builder to be used.
        /// </param>
        /// <param name="template">
        /// The message template to be used. But note, message templates are 
        /// not yet supported. Thus, the message text is always used.
        /// </param>
        /// <param name="message">
        /// The message text to be used.
        /// </param>
        /// <param name="split">
        /// The split character to be added.
        /// </param>
        private void AddShortMessage(StringBuilder builder, String template, String message, Char split)
        {
            if (!String.IsNullOrWhiteSpace(template))
            {
                this.AddValue(builder, "short_message", template, split, false);
            }
            else
            {
                this.AddValue(builder, "short_message", message, split, false);
            }
        }

        /// <summary>
        /// Adds field <c>full_message</c> to the string builder.
        /// </summary>
        /// <remarks>
        /// This field remains empty as long as message templates are not supported.
        /// </remarks>
        /// <param name="builder">
        /// The string builder to be used.
        /// </param>
        /// <param name="template">
        /// The message template to be used. But note, message templates are 
        /// not yet supported. Thus, this field is actually unused.
        /// </param>
        /// <param name="message">
        /// The message text to be used.
        /// </param>
        /// <param name="split">
        /// The split character to be added.
        /// </param>
        private void AddFullMessage(StringBuilder builder, String template, String message, Char split)
        {
            if (!String.IsNullOrWhiteSpace(template))
            {
                this.AddValue(builder, "full_message", message, split, false);
            }
        }

        /// <summary>
        /// Adds field <c>timestamp</c> to the string builder.
        /// </summary>
        /// <remarks>
        /// The timestamp is added as UTC time in seconds since Unix epoch followed 
        /// by optional milliseconds, but only if <see cref="ILoggerSettings.ShowTime"/> 
        /// is enabled.
        /// </remarks>
        /// <param name="builder">
        /// The string builder to be used.
        /// </param>
        /// <param name="timestamp">
        /// The timestamp to be used in seconds at Unix epoch.
        /// </param>
        /// <param name="split">
        /// The split character to be added.
        /// </param>
        /// <seealso cref="DateTimeExtension.ToUnixEpoch(DateTime)"/>
        private void AddTimestamp(StringBuilder builder, DateTime timestamp, Char split)
        {
            // See notes at Graylog: "...SHOULD be set by the client library.
            // If absent, the timestamp will be set to the current time (now)."
            if (base.Settings.ShowTime)
            {
                this.AddValue(builder, "timestamp", timestamp.ToUnixEpoch().ToString(NumberFormatInfo.InvariantInfo), split, true);
            }
        }

        /// <summary>
        /// Adds field <c>level</c> to the string builder.
        /// </summary>
        /// <remarks>
        /// Be aware, the logging level is converted into its <em>Syslog Severity Level</em> 
        /// representation automatically.
        /// </remarks>
        /// <param name="builder">
        /// The string builder to be used.
        /// </param>
        /// <param name="level">
        /// The logging level to be used.
        /// </param>
        /// <param name="split">
        /// The split character to be added.
        /// </param>
        /// <seealso cref="LogLevelExtension.ToUnixSeverityLevel(LogLevel)"/>
        private void AddLevel(StringBuilder builder, LogLevel level, Char split)
        {
            this.AddValue(builder, "level ", level.ToUnixSeverityLevel().ToString(NumberFormatInfo.InvariantInfo), split, true);
        }

        /// <summary>
        /// Adds <c>Key</c> as additional field to the string builder.
        /// </summary>
        /// <remarks>
        /// This field is added as additional field and appears as <c>_Key</c> inside messages. 
        /// Please note, this field is only added if <see cref="ILoggerSettings.ShowKey"/> is 
        /// enabled.
        /// </remarks>
        /// <param name="builder">
        /// The string builder to be used.
        /// </param>
        /// <param name="label">
        /// The label to be used, which is set to <c>Key</c>.
        /// </param>
        /// <param name="key">
        /// The key value to be used.
        /// </param>
        /// <param name="split">
        /// The split character to be added.
        /// </param>
        private void AddKey(StringBuilder builder, String label, Guid key, Char split)
        {
            if (base.Settings.ShowKey)
            {
                this.AddValue(builder, this.GetAdditionalFieldLabel(label), base.GetKey(key), split, false);
            }
        }

        /// <summary>
        /// Adds <c>Context</c> as additional field to the string builder but only if available.
        /// </summary>
        /// <remarks>
        /// This field is added as additional field and appears as <c>_Context</c> inside messages. 
        /// Please note, this field is only added if available.
        /// </remarks>
        /// <param name="builder">
        /// The string builder to be used.
        /// </param>
        /// <param name="label">
        /// The label to be used, which is set to <c>Context</c>.
        /// </param>
        /// <param name="context">
        /// The context value to be used.
        /// </param>
        /// <param name="split">
        /// The split character to be added.
        /// </param>
        private void AddContext(StringBuilder builder, String label, String context, Char split)
        {
            if (!String.IsNullOrWhiteSpace(context))
            {
                this.AddValue(builder, this.GetAdditionalFieldLabel(label), base.GetContext(context, base.NullValue), split, false);
            }
        }

        /// <summary>
        /// Adds <c>Scope</c> as additional field to the string builder but only if available.
        /// </summary>
        /// <remarks>
        /// This field is added as additional field and appears as <c>_Scope</c> inside messages. 
        /// Please note, this field is only added if available.
        /// </remarks>
        /// <param name="builder">
        /// The string builder to be used.
        /// </param>
        /// <param name="label">
        /// The label to be used, which is set to <c>Scope</c>.
        /// </param>
        /// <param name="scope">
        /// The scope value to be used.
        /// </param>
        /// <param name="split">
        /// The split character to be added.
        /// </param>
        private void AddScope(StringBuilder builder, String label, String scope, Char split)
        {
            if (!String.IsNullOrWhiteSpace(scope))
            {
                this.AddValue(builder, this.GetAdditionalFieldLabel(label), base.GetScope(scope, base.NullValue), split, false);
            }
        }

        /// <summary>
        /// Adds <c>Exception</c> as additional field to the string builder but only if available.
        /// </summary>
        /// <remarks>
        /// This field is added as additional field and appears as <c>_Exception</c> inside messages. 
        /// Please note, this field is only added if available.
        /// </remarks>
        /// <param name="builder">
        /// The string builder to be used.
        /// </param>
        /// <param name="label">
        /// The label to be used, which is set to <c>Exception</c>.
        /// </param>
        /// <param name="exception">
        /// The exception value to be used.
        /// </param>
        /// <param name="split">
        /// The split character to be added.
        /// </param>
        private void AddException(StringBuilder builder, String label, Exception exception, Char split)
        {
            if (exception != null)
            {
                this.AddValue(builder, this.GetAdditionalFieldLabel(label), base.GetException(exception, base.NullValue), split, false);
            }
        }

        /// <summary>
        /// Adds all details as additional fields to the string builder but 
        /// only if available.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Each available detail is added as additional field and appears as 
        /// <c>_&lt;Value of <paramref name="details"/>.Label&gt;</c> inside 
        /// messages. 
        /// </para>
        /// </remarks>
        /// <example>
        /// <para>
        /// Consider this set of details as parameter of some <see cref="ILogEvent"/> value.
        /// </para>
        /// <code>
        /// var details = new (String, Object)[]
        /// {
        ///     ("SellerId", 42),
        ///     ("SellerName", "seller name"),
        ///     ("IsEnabled", true),
        ///     ("JoinDate", DateTime.Now)
        /// };
        /// </code>
        /// <para>
        /// The above details would result in a GELF message like this.
        /// </para>
        /// <code>
        /// {
        ///     "version": "1.1",
        ///     "host": "machine-name",
        ///     "short_message": "Seller created and joined",
        ///     "timestamp": 1572368742.678,
        ///     "level": 5,
        ///     "_SellerId": 42,
        ///     "_SellerName": "seller name",
        ///     "_IsEnabled": "true",
        ///     "_JoinDate": "10/29/2019 5:05:42 PM"
        /// }
        /// </code>
        /// </example>
        /// <param name="builder">
        /// The string builder to be used.
        /// </param>
        /// <param name="details">
        /// The details to be used.
        /// </param>
        /// <param name="split">
        /// The split character to be added.
        /// </param>
        private void AddDetails(StringBuilder builder, (String Label, Object Value)[] details, Char split)
        {
            if (details != null)
            {
                foreach ((String label, Object value) in details)
                {
                    this.AddValue(builder, this.GetAdditionalFieldLabel(label), base.GetConverted(value, base.NullValue), split, value.IsNumber());
                }
            }
        }

        /// <summary>
        /// Returns a format string depending parameter <paramref name="plain"/>.
        /// </summary>
        /// <remarks>
        /// This method returns a format string depending parameter <paramref name="plain"/>.
        /// </remarks>
        /// <param name="plain">
        /// If <c>false</c> the format string is surrounded by <em>double quotes</em> 
        /// and otherwise not.
        /// </param>
        /// <returns>
        /// The format string to be used.
        /// </returns>
        private String GetFormat(Boolean plain)
        {
            return plain ? "{0}" : "\"{0}\"";
        }

        /// <summary>
        /// Turns the <paramref name="label"/> into its GELF representation for additional fields.
        /// </summary>
        /// <remarks>
        /// <para>
        /// The Graylog specification has some restrictions for the naming of additional fields 
        /// and this method tries to apply all these restriction.
        /// </para>
        /// <para>
        /// First of all, GELF messages do not support additional fields named <c>id</c> because 
        /// they would be transformed into <c>_id</c> and might be in conflict of the ID field 
        /// of a <em>MongoDB</em>. Therefore, each detail label that consists of <em>ID</em> (no 
        /// matter if upper or lower case) is transformed into <c>_id_field</c>, whereby the 
        /// character casing of each individual <em>ID</em> is kept.
        /// </para>
        /// <para>
        /// Secondly, each character that is neither a letter, nor a digit, nor an underscore, 
        /// nor a dash or nor a dot is replaced by an underscore.
        /// </para>
        /// <para>
        /// Finally, an underscore is placed in front of the result.
        /// </para>
        /// </remarks>
        /// <param name="label">
        /// The label to be transformed.
        /// </param>
        /// <returns>
        /// The transformed label for additional fields.
        /// </returns>
        private String GetAdditionalFieldLabel(String label)
        {
            // See notes at Graylog: "Libraries SHOULD not allow to send id as additional field (_id)."
            if (String.Equals(label, "id", StringComparison.OrdinalIgnoreCase))
            {
                return String.Format("_{0}_field", label);
            }

            // See notes at Graylog: "Allowed characters in field names are any word character (letter,
            // number, underscore), dashes and dots." => So each label has to be "sanitized".
            StringBuilder builder = new StringBuilder(label);

            for (Int32 index = 0; index < builder.Length; index++)
            {
                Char current = builder[index];

                if (!Char.IsLetterOrDigit(current) && current != '_' && current != '-' && current != '.')
                {
                    builder[index] = '_';
                }
            }

            return String.Format("_{0}", builder.ToString());
        }

        #endregion
    }
}
