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

using Plexdata.LogWriter.Abstraction;
using Plexdata.LogWriter.Definitions;
using Plexdata.LogWriter.Extensions;
using Plexdata.LogWriter.Internals.Extensions;
using Plexdata.LogWriter.Settings;
using System;
using System.Text;

namespace Plexdata.LogWriter.Internals.Formatters
{
    /// <summary>
    /// This internal class represents the base class of all other formatter implementations.
    /// </summary>
    /// <remarks>
    /// This base class provides functionality that will be shared by all other formatter 
    /// classes. Intentionally, this class is defined as abstract to prevent a direct usage.
    /// </remarks>
    internal abstract class FormatterBase
    {
        #region Construction

        /// <summary>
        /// The parameterized constructor just initializes all properties.
        /// </summary>
        /// <remarks>
        /// Formatting data depends on some logger settings. Therefore, derived 
        /// classes must provide an instance of <see cref="ILoggerSettings"/>.
        /// </remarks>
        /// <param name="settings">
        /// The settings to be used for data formatting.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// This exception is thrown is parameter <paramref name="settings"/> is 
        /// <c>null</c>.
        /// </exception>
        protected FormatterBase(ILoggerSettings settings)
            : base()
        {
            this.Settings = settings ?? throw new ArgumentNullException(nameof(settings));
        }

        #endregion

        #region Protected properties

        /// <summary>
        /// Gets the string of a “null” value.
        /// </summary>
        /// <remarks>
        /// The “null” value string can be used to tag formatted 
        /// data as invalid.
        /// </remarks>
        /// <value>
        /// A string representing the “null” value.
        /// </value>
        protected String NullValue { get { return "null"; } }

        /// <summary>
        /// Gets the string of an “empty” value.
        /// </summary>
        /// <remarks>
        /// The “empty” value string can be used to tag formatted 
        /// data as invalid.
        /// </remarks>
        /// <value>
        /// A string representing the “empty” value.
        /// </value>
        protected String EmptyValue { get { return "empty"; } }

        /// <summary>
        /// Gets the instance of logger settings.
        /// </summary>
        /// <remarks>
        /// The logger settings are provided during construction. 
        /// </remarks>
        /// <value>
        /// The instance of used logger settings. 
        /// </value>
        protected ILoggerSettings Settings { get; private set; }

        #endregion

        #region Protected abstract methods

        /// <summary>
        /// This method converts a label into its output representation.
        /// </summary>
        /// <remarks>
        /// In derived classes this method converts the parameter <paramref name="label"/> 
        /// into the output representation according to specific needs.
        /// </remarks>
        /// <param name="label">
        /// The label to be converted.
        /// </param>
        /// <returns>
        /// The converted label.
        /// </returns>
        protected abstract String ToLabel(String label);

        /// <summary>
        /// This method converts a value into its output representation.
        /// </summary>
        /// <remarks>
        /// In derived classes this method converts the parameter <paramref name="value"/> 
        /// into the output representation according to specific needs.
        /// </remarks>
        /// <param name="value">
        /// The value to be converted.
        /// </param>
        /// <param name="split">
        /// The split character to be appended.
        /// </param>
        /// <returns>
        /// The converted value.
        /// </returns>
        protected abstract String ToValue(String value, Char split);

        /// <summary>
        /// This method converts a value into its output format.
        /// </summary>
        /// <remarks>
        /// In derived classes this method converts the parameter <paramref name="value"/> 
        /// into the output format according to specific needs.
        /// </remarks>
        /// <param name="value">
        /// The value to be converted.
        /// </param>
        /// <param name="split">
        /// The split character to be appended.
        /// </param>
        /// <returns>
        /// The converted value that is ready for output.
        /// </returns>
        protected abstract String ToOutput(String value, Char split);

        /// <summary>
        /// This method converts a combination of label and value into its output format.
        /// </summary>
        /// <remarks>
        /// In derived classes this method converts the parameters <paramref name="label"/> 
        /// and <paramref name="value"/> into the output format according to specific needs.
        /// </remarks>
        /// <param name="label">
        /// The label to be included.
        /// </param>
        /// <param name="value">
        /// The value to be included.
        /// </param>
        /// <param name="split">
        /// The split character to be appended.
        /// </param>
        /// <returns>
        /// The combination of converted label and value that are ready for output.
        /// </returns>
        protected abstract String ToOutput(String label, String value, Char split);

        #endregion

        #region Protected methods

        /// <summary>
        /// This method converts parameter <paramref name="key"/> into its string 
        /// representation.
        /// </summary>
        /// <remarks>
        /// The key is converted into a hexadecimal string consisting of 32 digits that 
        /// are separated by hyphens and may look like 00000000-0000-0000-0000-000000000000. 
        /// Furthermore, the result is turned into upper cases.
        /// </remarks>
        /// <param name="key">
        /// The key to be converted.
        /// </param>
        /// <returns>
        /// The string representation of converted key.
        /// </returns>
        protected String GetKey(Guid key)
        {
            return key.ToString("D").ToUpper();
        }

        /// <summary>
        /// This method converts parameter <paramref name="level"/> into its string 
        /// representation.
        /// </summary>
        /// <remarks>
        /// The actual conversion of provided logging level is done by the logging 
        /// level extension. For more information please refer to the description of 
        /// class <see cref="LogLevelDisplayExtension"/>.
        /// </remarks>
        /// <param name="level">
        /// The logging level to be converted.
        /// </param>
        /// <returns>
        /// The string representation of converted logging level.
        /// </returns>
        /// <exception cref="NotSupportedException">
        /// This exception is thrown if parameter <paramref name="level"/> is not 
        /// considered as valid logging level.
        /// </exception>
        /// <seealso cref="LogLevelDisplayExtension.ToDisplayText(LogLevel)"/>
        protected String GetLevel(LogLevel level)
        {
            switch (level)
            {
                case LogLevel.Trace:
                case LogLevel.Debug:
                case LogLevel.Verbose:
                case LogLevel.Message:
                case LogLevel.Warning:
                case LogLevel.Error:
                case LogLevel.Fatal:
                case LogLevel.Critical:
                case LogLevel.Disaster:
                    return level.ToDisplayText();
                default:
                    throw new NotSupportedException($"The value \"{nameof(level)}\" of type logging level is not supported.");
            }
        }

        /// <summary>
        /// This method converts parameter <paramref name="time"/> into its string 
        /// representation.
        /// </summary>
        /// <remarks>
        /// The time conversion is indeed a multi-step procedure. First of all, the provided 
        /// time is transformed into UTC or into local time. The transformation type is taken 
        /// from current settings. Thereafter, the resulting time value is converted into its 
        /// string representation according to current settings.
        /// </remarks>
        /// <param name="time">
        /// The time to be converted.
        /// </param>
        /// <returns>
        /// The string representation of converted time.
        /// </returns>
        /// <seealso cref="ILoggerSettings"/>
        /// <seealso cref="GetAdjustedTime(DateTime)"/>
        /// <seealso cref="GetFormattedTime(DateTime, String)"/>
        protected String GetTime(DateTime time)
        {
            return this.GetFormattedTime(this.GetAdjustedTime(time), this.Settings.TimeFormat);
        }

        /// <summary>
        /// This method converts provided parameter <paramref name="context"/> into its 
        /// string representation.
        /// </summary>
        /// <remarks>
        /// The method actually calls method <see cref="GetContext(String, String)"/>, 
        /// providing the <see cref="NullValue"/> as standard parameter.
        /// </remarks>
        /// <param name="context">
        /// The context to be converted.
        /// </param>
        /// <returns>
        /// The string representation of converted context.
        /// </returns>
        /// <seealso cref="NullValue"/>
        /// <seealso cref="GetContext(String, String)"/>
        protected String GetContext(String context)
        {
            return this.GetContext(context, this.NullValue);
        }

        /// <summary>
        /// This method converts provided parameter <paramref name="context"/> 
        /// into its string representation.
        /// </summary>
        /// <remarks>
        /// The method simply returns the trimmed version of provided parameter 
        /// <paramref name="context"/>, if it is neither <c>null</c> nor <em>empty</em> 
        /// nor consists only of <em>whitespaces</em>.
        /// </remarks>
        /// <param name="context">
        /// The context to be converted.
        /// </param>
        /// <param name="standard">
        /// The value to be used as standard result.
        /// </param>
        /// <returns>
        /// The string representation of converted context.
        /// </returns>
        protected String GetContext(String context, String standard)
        {
            if (!String.IsNullOrWhiteSpace(context))
            {
                return context.Trim();
            }
            else
            {
                return standard;
            }
        }

        /// <summary>
        /// This method converts provided parameter <paramref name="scope"/> into its 
        /// string representation.
        /// </summary>
        /// <remarks>
        /// The method actually calls method <see cref="GetScope(String, String)"/>, 
        /// providing the <see cref="NullValue"/> as standard parameter.
        /// </remarks>
        /// <param name="scope">
        /// The scope to be converted.
        /// </param>
        /// <returns>
        /// The string representation of converted scope.
        /// </returns>
        /// <seealso cref="NullValue"/>
        /// <seealso cref="GetScope(String, String)"/>
        protected String GetScope(String scope)
        {
            return this.GetScope(scope, this.NullValue);
        }

        /// <summary>
        /// This method converts provided parameter <paramref name="scope"/> 
        /// into its string representation.
        /// </summary>
        /// <remarks>
        /// The method simply returns the trimmed version of provided parameter 
        /// <paramref name="scope"/>, if it is neither <c>null</c> nor <em>empty</em> 
        /// nor consists only of <em>whitespaces</em>.
        /// </remarks>
        /// <param name="scope">
        /// The scope to be converted.
        /// </param>
        /// <param name="standard">
        /// The value to be used as standard result.
        /// </param>
        /// <returns>
        /// The string representation of converted scope.
        /// </returns>
        protected String GetScope(String scope, String standard)
        {
            if (!String.IsNullOrWhiteSpace(scope))
            {
                return scope.Trim();
            }
            else
            {
                return standard;
            }
        }

        /// <summary>
        /// This method converts provided parameter <paramref name="message"/> into its 
        /// string representation.
        /// </summary>
        /// <remarks>
        /// The method actually calls method <see cref="GetMessage(String, String)"/>, 
        /// providing the <see cref="NullValue"/> as standard parameter.
        /// </remarks>
        /// <param name="message">
        /// The message to be converted.
        /// </param>
        /// <returns>
        /// The string representation of converted message.
        /// </returns>
        /// <seealso cref="NullValue"/>
        /// <seealso cref="GetMessage(String, String)"/>
        protected String GetMessage(String message)
        {
            return this.GetMessage(message, this.NullValue);
        }

        /// <summary>
        /// This method converts provided parameter <paramref name="message"/> 
        /// into its string representation.
        /// </summary>
        /// <remarks>
        /// The method simply returns the trimmed version of provided parameter 
        /// <paramref name="message"/>, if it is neither <c>null</c> nor <em>empty</em> 
        /// nor consists only of <em>whitespaces</em>.
        /// </remarks>
        /// <param name="message">
        /// The message to be converted.
        /// </param>
        /// <param name="standard">
        /// The value to be used as standard result.
        /// </param>
        /// <returns>
        /// The string representation of converted message.
        /// </returns>
        protected String GetMessage(String message, String standard)
        {
            if (!String.IsNullOrWhiteSpace(message))
            {
                return message.Trim();
            }
            else
            {
                return standard;
            }
        }

        /// <summary>
        /// This method converts provided parameter <paramref name="exception"/> into its 
        /// string representation.
        /// </summary>
        /// <remarks>
        /// The method actually calls method <see cref="GetException(Exception, String)"/>, 
        /// providing the <see cref="NullValue"/> as standard parameter.
        /// </remarks>
        /// <param name="exception">
        /// The exception to be converted.
        /// </param>
        /// <returns>
        /// The string representation of converted exception.
        /// </returns>
        /// <seealso cref="NullValue"/>
        /// <seealso cref="GetException(Exception, String)"/>
        protected String GetException(Exception exception)
        {
            return this.GetException(exception, this.NullValue);
        }

        /// <summary>
        /// This method converts provided parameter <paramref name="exception"/> 
        /// into its string representation.
        /// </summary>
        /// <remarks>
        /// The method simply calls method <see cref="Exception.ToString()"/> of provided 
        /// parameter <paramref name="exception"/>, if it is not <c>null</c>.
        /// </remarks>
        /// <param name="exception">
        /// The exception to be converted.
        /// </param>
        /// <param name="standard">
        /// The value to be used as standard result.
        /// </param>
        /// <returns>
        /// The string representation of converted exception.
        /// </returns>
        protected String GetException(Exception exception, String standard)
        {
            if (exception != null)
            {
                return exception.ToString();
            }
            else
            {
                return standard;
            }
        }

        /// <summary>
        /// This method converts provided parameter <paramref name="value"/> 
        /// into its string representation.
        /// </summary>
        /// <remarks>
        /// This method uses the type formatter extension for a value conversion. 
        /// If the value conversion fails and value is not <c>null</c> then method 
        /// <see cref="Object.ToString()"/> is used. Otherwise an <em>empty</em> 
        /// string is returned.
        /// </remarks>
        /// <param name="value">
        /// The value to be converted.
        /// </param>
        /// <returns>
        /// The string representation of converted value.
        /// </returns>
        /// <seealso cref="TypeFormatterExtension.TryFormat{TValue}(TValue, IFormatProvider, out String)"/>
        protected String GetConverted(Object value)
        {
            if (!value.TryFormat(this.Settings.Culture, out String result))
            {
                result = (value ?? String.Empty).ToString();
            }

            return result;
        }

        /// <summary>
        /// This method removes the last occurrences of character <paramref name="value"/> 
        /// from provided string <paramref name="builder"/>.
        /// </summary>
        /// <remarks>
        /// This method tries to remove the last occurrences of character <paramref name="value"/> 
        /// from provided string <paramref name="builder"/>, but only if the string builder is 
        /// neither <c>null</c> nor <em>empty</em> and if provided character actually exists at the end.
        /// </remarks>
        /// <param name="builder">
        /// The string builder instance to be trimmed at its end.
        /// </param>
        /// <param name="value">
        /// The character that should be removed.
        /// </param>
        protected void TrimEnd(StringBuilder builder, Char value)
        {
            if (builder != null && builder.Length > 0 && builder[builder.Length - 1] == value)
            {
                builder.Remove(builder.Length - 1, 1);
            }
        }

        #endregion

        #region Private methods

        /// <summary>
        /// This method adjusts provided timestamp according to current timestamp 
        /// settings.
        /// </summary>
        /// <remarks>
        /// This method simply converts a timestamp in local time into UTC if necessary 
        /// and vice versa. In more details, if requested time type is UTC and provided 
        /// timestamp is NOT in UTC then the timestamp is converted into UTC. Otherwise, 
        /// if requested time type is LOCAL and provided timestamp is NOT in local time 
        /// then the timestamp is converted into local time. In any other case, the 
        /// provided timestamp is returned as it is.
        /// </remarks>
        /// <param name="value">
        /// The timestamp to be adjusted.
        /// </param>
        /// <returns>
        /// The adjusted timestamp value.
        /// </returns>
        private DateTime GetAdjustedTime(DateTime value)
        {
            if (this.Settings.LogTime == LogTime.Utc)
            {
                if (value.Kind != DateTimeKind.Utc)
                {
                    value = value.ToUniversalTime();
                }
            }
            else if (this.Settings.LogTime == LogTime.Local)
            {
                if (value.Kind != DateTimeKind.Local)
                {
                    value = value.ToLocalTime();
                }
            }

            return value;
        }

        /// <summary>
        /// This method formats provided timestamp as string.
        /// </summary>
        /// <remarks>
        /// This method tries to format provided timestamp and puts it into out 
        /// parameter result if successful. But be aware, the default time format 
        /// is used if provided format is <c>null</c>, <em>empty</em> or consists 
        /// only of <em>whitespaces</em>, or if a formatting error occurs. Keep in 
        /// mind, a deadlock might be possible but only the default time format 
        /// string is invalid.
        /// </remarks>
        /// <param name="value">
        /// The timestamp to be formatted.
        /// </param>
        /// <param name="format">
        /// The format string to be used. 
        /// </param>
        /// <returns>
        /// The resulting string representation of provided timestamp.
        /// </returns>
        /// <seealso cref="LoggerSettings.DefaultTimeFormat"/>
        private String GetFormattedTime(DateTime value, String format)
        {
            String result = String.Empty;

            if (String.IsNullOrEmpty(format))
            {
                return this.GetFormattedTime(value, LoggerSettings.DefaultTimeFormat);
            }

            try
            {
                return value.ToString(format);
            }
            catch
            {
                return this.GetFormattedTime(value, LoggerSettings.DefaultTimeFormat); // Deadlock possible!
            }
        }

        #endregion
    }
}
