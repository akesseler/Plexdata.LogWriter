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
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Plexdata.LogWriter.Internals.Extensions
{
    /// <summary>
    /// Provides settings related policies and rule checks.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This extension class provides settings related policies and rule checks. These policies and rule checks are 
    /// briefly explained below.
    /// </para>
    /// <para>
    /// Major task of this extension class is to determine the current filename to be used to write a particular 
    /// logging message into.
    /// </para>
    /// <para>
    /// For this purpose it must be distinguished between file rolling and standard behaviour. This is because of 
    /// file rolling works significantly different as it standard behaviour does.
    /// </para>
    /// <para>
    /// Another important parameter affecting the currently used filename is the threshold. The threshold, in detail, 
    /// is responsible to limit the size of the output file.
    /// </para>
    /// <para>
    /// Summarising does it means nothing else but that the combination of rolling and threshold affects the name of 
    /// the real output file. See below for the details about what is going to happen with a particular combination 
    /// of rolling and threshold.
    /// </para>
    /// <para>
    /// <h3>Rolling is Off</h3>
    /// <p>
    /// The original filename from the settings instance is returned if <see cref="IPersistentLoggerSettings.Threshold"/> 
    /// is less than or equal to zero. Otherwise it is tried to find the last file that contains a time stamp as suffix.
    /// </p>
    /// <p>
    /// A new filename with current time stamp as suffix is created and returned if such a file does not exist or if 
    /// the size of the found file is greater than or equal to the <see cref="IPersistentLoggerSettings.Threshold"/>. 
    /// Please keep in mind, which kind of time stamp is used (UTC or Local) depends on current value of property 
    /// <see cref="ILoggerSettings.LogTime"/>.
    /// </p>
    /// <p>
    /// In any other case, the fully qualified name of the last found file is returned.
    /// </p>
    /// <h3>Rolling is On</h3>
    /// <p>
    /// As first, two filenames are created, one with suffix <c>one</c> and another with suffix <c>two</c>. As next, the 
    /// required threshold is determined as follows: If current value of property <see cref="IPersistentLoggerSettings.Threshold"/> 
    /// is greater than zero, this value is taken. Otherwise, the default threshold is used.
    /// </p>
    /// <p>
    /// After that it is determined if one of those files already exists. In case of none of those files exist the fully 
    /// qualified name of file one is returned. If file one exists but file two doesn't then it is checked if the size 
    /// of file one is less than the threshold and if so, the fully qualified name of file one is returned. If not, then 
    /// the fully qualified name of file two is returned instead. 
    /// </p>
    /// <p>
    /// The other way round, if file two exists but file one doesn't the above described check is performed, but exactly 
    /// in the opposite way.
    /// </p>
    /// <p>
    /// In case of both files already exist then it is checked if size of file one is less than the threshold and if size 
    /// of file two is greater than or equal to the threshold. If so, the fully qualified name of file one is returned.
    /// </p>
    /// <p>
    /// The other way round, the fully qualified name of file two is returned instead if size of file two is less than the 
    /// threshold and if size of file one is greater than or equal to the threshold.
    /// </p>
    /// <p>
    /// In any other case, which means both files exist and both files have reached the threshold, the time stamp of last 
    /// write access is used to determine which of those files is used as next. In case of file one is older than file two, 
    /// the fully qualified name of file one is returned. Vice versa, if file two is older than file own the fully qualified 
    /// name of file two is returned instead. Please note, the <c>dispose</c> flag is set to <c>true</c> in both of these cases.
    /// </p>
    /// </para>
    /// </remarks>
    public static class SettingsPoliciesExtension
    {
        #region Private fields

        /// <summary>
        /// This field holds the value of the default threshold.
        /// </summary>
        /// <remarks>
        /// This default threshold is set to five megabytes.
        /// </remarks>
        public static Int32 DefaultThreshold = 5 * 1024 * 1024;

        #endregion

        #region Construction

        /// <summary>
        /// Default static constructor.
        /// </summary>
        /// <remarks>
        /// This constructor just initializes all static fields.
        /// </remarks>
        static SettingsPoliciesExtension()
        {
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Calculates the current filename and returns it.
        /// </summary>
        /// <remarks>
        /// This method calculates the current filename and returns it. The 
        /// determination of current filename takes place dependent on provided 
        /// logger settings.
        /// </remarks>
        /// <param name="settings">
        /// The logger settings to be used.
        /// </param>
        /// <param name="dispose">
        /// If true, then the file identified by returned filename should be 
        /// disposed before it is reused.
        /// </param>
        /// <returns>
        /// The calculated filename.
        /// </returns>
        public static String GetCurrentFilename(this IPersistentLoggerSettings settings, out Boolean dispose)
        {
            dispose = false;

            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            if (settings.IsRolling)
            {
                return settings.GetRollingFilename(out dispose);
            }
            else
            {
                return settings.GetStandardFilename();
            }
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Calculates the current rolling filename and returns it.
        /// </summary>
        /// <remarks>
        /// This method calculates the current rolling filename and returns it.
        /// </remarks>
        /// <param name="settings">
        /// The logger settings to be used.
        /// </param>
        /// <param name="dispose">
        /// If true, then the file identified by returned filename should be 
        /// disposed before it is reused.
        /// </param>
        /// <returns>
        /// The calculated filename.
        /// </returns>
        private static String GetRollingFilename(this IPersistentLoggerSettings settings, out Boolean dispose)
        {
            dispose = false;

            String path = Path.GetDirectoryName(settings.Filename);
            String name = Path.GetFileNameWithoutExtension(settings.Filename);
            String ext = Path.GetExtension(settings.Filename);

            FileInfo fileOne = new FileInfo(Path.Combine(path, $"{name}_one{ext}"));
            FileInfo fileTwo = new FileInfo(Path.Combine(path, $"{name}_two{ext}"));

            Int32 threshold = settings.Threshold > 0 ? settings.GetThresholdInBytes() : SettingsPoliciesExtension.DefaultThreshold;

            #region None of the files does exist yet.

            if (!fileOne.Exists && !fileTwo.Exists)
            {
                return fileOne.FullName;
            }

            #endregion

            #region File *one* exists, but file *two* doesn't.

            if (fileOne.Exists && !fileTwo.Exists)
            {
                if (fileOne.Length < threshold)
                {
                    // File *one* still has remaining capacity.
                    return fileOne.FullName;
                }
                else
                {
                    // File *two* should be created afterwards.
                    return fileTwo.FullName;
                }
            }

            #endregion

            #region File *two* exists, but file *one* doesn't.

            if (!fileOne.Exists && fileTwo.Exists)
            {
                if (fileTwo.Length < threshold)
                {
                    // File *two* still has remaining capacity.
                    return fileTwo.FullName;
                }
                else
                {
                    // File *one* should be created afterwards.
                    return fileOne.FullName;
                }
            }

            #endregion

            #region From here it's clear, both files exist...

            if (fileOne.Length < threshold && fileTwo.Length >= threshold)
            {
                // File *one* was the last used file.
                return fileOne.FullName;
            }

            if (fileOne.Length >= threshold && fileTwo.Length < threshold)
            {
                // File *two* was the last used file.
                return fileTwo.FullName;
            }

            #endregion

            #region From here it's clear, both files have reached the threshold...

            if (fileOne.LastWriteTimeUtc < fileTwo.LastWriteTimeUtc)
            {
                // File *one* is older than file *two*...
                dispose = true; // File *one* should be disposed...
                return fileOne.FullName;
            }

            // Finally, file *two* is older than file *one*...
            dispose = true; // File *two* should be disposed...
            return fileTwo.FullName;

            #endregion
        }

        /// <summary>
        /// Calculates the current standard filename and returns it.
        /// </summary>
        /// <remarks>
        /// This method calculates the current standard filename and returns it.
        /// </remarks>
        /// <param name="settings">
        /// The logger settings to be used.
        /// </param>
        /// <returns>
        /// The calculated filename.
        /// </returns>
        private static String GetStandardFilename(this IPersistentLoggerSettings settings)
        {
            if (settings.Threshold <= 0)
            {
                // Rolling is off, threshold not used => only one file...
                return settings.Filename;
            }

            String path = Path.GetDirectoryName(settings.Filename);
            String name = Path.GetFileNameWithoutExtension(settings.Filename);
            String ext = Path.GetExtension(settings.Filename);

            String style = $"yyyyMMddHHmmss";
            String query = $"{name}_??????????????{ext}";
            String match = $"{name}_[0-9]{{14}}\\{ext}";

            FileInfo info = (new DirectoryInfo(path))
                .GetFiles(query, SearchOption.TopDirectoryOnly)
                .OrderByDescending(x => x.Name)
                .Where(y => Regex.IsMatch(y.Name, match, RegexOptions.IgnoreCase))
                .FirstOrDefault();

            if (info == null || info.Length >= settings.GetThresholdInBytes())
            {
                // BUG: May cause duplicates because of UTC/local time mismatches!
                String time = (settings.LogTime == LogTime.Utc ? DateTime.UtcNow : DateTime.Now).ToString(style);

                return Path.Combine(path, $"{name}_{time}{ext}");
            }
            else
            {
                return info.FullName;
            }
        }

        /// <summary>
        /// Calculates the threshold as number of bytes.
        /// </summary>
        /// <remarks>
        /// This method calculates the threshold as number of bytes and returns it.
        /// </remarks>
        /// <param name="settings">
        /// The logger settings to be used.
        /// </param>
        /// <returns>
        /// The number of bytes for the threshold.
        /// </returns>
        private static Int32 GetThresholdInBytes(this IPersistentLoggerSettings settings)
        {
            return settings.Threshold * 1024;
        }

        #endregion
    }
}
