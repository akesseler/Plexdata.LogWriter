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

using Plexdata.LogWriter.Definitions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;

namespace Plexdata.LogWriter.Extensions
{
    /// <summary>
    /// This extension allows to register a user-defined display text for a particular 
    /// logging level.
    /// </summary>
    /// <remarks>
    /// Registering a user-defined display text for a particular logging level might 
    /// be useful for example to provide a localized logging level label.
    /// </remarks>
    /// <example>
    /// Below find an example of how to register user-defined logging level display 
    /// text labels. But be aware, changing them will have a global effect!
    /// <code language="C#">
    /// LogLevel.Trace.RegisterDisplayText("TRC");
    /// LogLevel.Debug.RegisterDisplayText("DBG");
    /// LogLevel.Verbose.RegisterDisplayText("VBS");
    /// LogLevel.Message.RegisterDisplayText("MSG");
    /// LogLevel.Warning.RegisterDisplayText("WRN");
    /// LogLevel.Error.RegisterDisplayText("ERR");
    /// LogLevel.Fatal.RegisterDisplayText("FAT");
    /// LogLevel.Critical.RegisterDisplayText("CRT");
    /// </code>
    /// </example>
    public static class LogLevelDisplayExtension
    {
        #region Private fields

        /// <summary>
        /// The internal mapping of logging level assignments.
        /// </summary>
        /// <remarks>
        /// A dictionary that contains the logging levels and their assigned display names.
        /// </remarks>
        private static readonly IDictionary<LogLevel, String> mappings = null;

        #endregion

        #region Construction

        /// <summary>
        /// This static constructor initializes all supported display text values 
        /// with their default labels.
        /// </summary>
        /// <remarks>
        /// The default display text is nothing else but the name of each of the 
        /// enumeration values in capital letters.
        /// </remarks>
        static LogLevelDisplayExtension()
        {
            LogLevelDisplayExtension.mappings = new Dictionary<LogLevel, String>
            {
                { LogLevel.Disabled, LogLevel.Disabled.Convert() },
                { LogLevel.Trace,    LogLevel.Trace.Convert()    },
                { LogLevel.Debug,    LogLevel.Debug.Convert()    },
                { LogLevel.Verbose,  LogLevel.Verbose.Convert()  },
                { LogLevel.Message,  LogLevel.Message.Convert()  },
                { LogLevel.Warning,  LogLevel.Warning.Convert()  },
                { LogLevel.Error,    LogLevel.Error.Convert()    },
                { LogLevel.Fatal,    LogLevel.Fatal.Convert()    },
                { LogLevel.Critical, LogLevel.Critical.Convert() },
            };

            LogLevelDisplayExtension.AssertOnInvalidMappings();
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Returns the registered display text for provided logging level.
        /// </summary>
        /// <remarks>
        /// This method tries to find a registered text for the provided logging 
        /// level. If a registration could not be found then the default display 
        /// text is returned.
        /// </remarks>
        /// <param name="level">
        /// The logging level to get the display text for.
        /// </param>
        /// <returns>
        /// The registered display text for provided logging level or its default 
        /// value.
        /// </returns>
        /// <exception cref="InvalidEnumArgumentException">
        /// This exception is thrown as soon as provided <paramref name="level"/> 
        /// is not defined inside the internal registration.
        /// </exception>
        /// <seealso cref="LogLevel"/>
        /// <seealso cref="ThrowIfInvalid(LogLevel)"/>
        public static String ToDisplayText(this LogLevel level)
        {
            level.ThrowIfInvalid();

            return LogLevelDisplayExtension.mappings[level];
        }

        /// <summary>
        /// Restores the default display text for the provided logging level.
        /// </summary>
        /// <remarks>
        /// The default display text is nothing else but the name of the enumeration 
        /// value in upper cases.
        /// </remarks>
        /// <param name="level">
        /// The logging level to restore the default display text for.
        /// </param>
        /// <exception cref="InvalidEnumArgumentException">
        /// This exception is thrown as soon as provided <paramref name="level"/> 
        /// is not defined inside the internal registration.
        /// </exception>
        /// <seealso cref="LogLevel"/>
        /// <seealso cref="ThrowIfInvalid(LogLevel)"/>
        public static void RestoreDisplayText(this LogLevel level)
        {
            level.ThrowIfInvalid();

            LogLevelDisplayExtension.mappings[level] = level.Convert();
        }

        /// <summary>
        /// Registers a display text for a particular logging level.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Be aware, the default display text is used in case of provided display 
        /// text is null, empty or only consists of whitespaces.
        /// </para>
        /// <para>
        /// <b>Attention!</b> Changing some or all default values will have a global 
        /// effect! Therefore, use this feature very carefully.
        /// </para>
        /// </remarks>
        /// <param name="level">
        /// The logging level to register the display text for.
        /// </param>
        /// <param name="text">
        /// The display text to be used for the provided logging level.
        /// </param>
        /// <exception cref="InvalidEnumArgumentException">
        /// This exception is thrown as soon as provided <paramref name="level"/> 
        /// is not defined inside the internal registration.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// This exception is thrown as soon as provided <paramref name="text"/> 
        /// is <c>null</c>, <c>empty</c> or consists only of white spaces. 
        /// </exception>
        /// <seealso cref="LogLevel"/>
        /// <seealso cref="ThrowIfInvalid(LogLevel)"/>
        public static void RegisterDisplayText(this LogLevel level, String text)
        {
            level.ThrowIfInvalid();

            if (String.IsNullOrWhiteSpace(text))
            {
                throw new ArgumentOutOfRangeException(nameof(text));
            }

            LogLevelDisplayExtension.mappings[level] = text;
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Converts provided logging level into its upper case string version.
        /// </summary>
        /// <remarks>
        /// As mentioned, this method just takes the name of provided logging 
        /// level and converts it into its upper case string representation.
        /// </remarks>
        /// <param name="level">
        /// The logging level to be converted.
        /// </param>
        /// <returns>
        /// The converted logging level text.
        /// </returns>
        private static String Convert(this LogLevel level)
        {
            return level.ToString().ToUpper();
        }

        /// <summary>
        /// This method validates provided logging level.
        /// </summary>
        /// <remarks>
        /// In detail, this method checks if an internal registration exists 
        /// for provided logging level. If yes, then nothing is going to happen. 
        /// But if not, then an exception is thrown.
        /// </remarks>
        /// <param name="level">
        /// The logging level to be validated.
        /// </param>
        /// <exception cref="InvalidEnumArgumentException">
        /// This exception is thrown as soon as provided <paramref name="level"/> 
        /// is not defined inside the internal registration.
        /// </exception>
        /// <seealso cref="LogLevel"/>
        private static void ThrowIfInvalid(this LogLevel level)
        {
            if (!LogLevelDisplayExtension.mappings.ContainsKey(level))
            {
                throw new InvalidEnumArgumentException(nameof(level), (Int32)level, typeof(LogLevel));
            }
        }

        /// <summary>
        /// This method causes an assertion as soon as recognizing a mismatch of 
        /// enumeration item count.
        /// </summary>
        /// <remarks>
        /// Safety first... This assertion becomes true as soon as a new value is 
        /// put into the logging level enumeration but it wasn't added here. But 
        /// keep in mind, this method is available only in DEBUG mode.
        /// </remarks>
        [Conditional("DEBUG")]
        private static void AssertOnInvalidMappings()
        {
            Int32 existing = Enum.GetValues(typeof(LogLevel)).Length - 1;
            Int32 expected = LogLevelDisplayExtension.mappings.Keys.Count;

            Debug.Assert(existing == expected, $"Found {existing} supported items inside the logging level enumeration but only {expected} mappings have been configured.");
        }

        #endregion
    }
}
