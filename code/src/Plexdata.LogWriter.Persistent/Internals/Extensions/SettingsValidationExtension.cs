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

using System;
using System.Collections.Generic;
using System.IO;

namespace Plexdata.LogWriter.Internals.Extensions
{
    /// <summary>
    /// Provides settings related operations for the persistent logger.
    /// </summary>
    /// <remarks>
    /// This extension class provides operations related to settings validation for 
    /// the persistent logger. For the moment these operations are limited to file 
    /// name and file path validation as well as to the validation of required file 
    /// permissions.
    /// </remarks>
    internal static class SettingsValidationExtension
    {
        #region Private fields

        /// <summary>
        /// The list of supported path separator characters.
        /// </summary>
        /// <remarks>
        /// This field holds the list of supported path separator characters. 
        /// These characters are the slash (<c>/</c>) as well as the backslash 
        /// (<c>\</c>).
        /// </remarks>
        /// <seealso cref="Path.DirectorySeparatorChar"/>
        /// <seealso cref="Path.AltDirectorySeparatorChar"/>
        private static readonly Char[] PathSeparatorCharacters = null;

        /// <summary>
        /// The list of invalid path name characters.
        /// </summary>
        /// <remarks>
        /// This field holds the list of path name characters that are considered as 
        /// invalid. Additionally to the system list of invalid path name characters, 
        /// this list also contains the asterisk character (<c>*</c>) and the question 
        /// mark character (<c>?</c>).
        /// </remarks>
        /// <seealso cref="Path.GetInvalidPathChars()"/>
        private static readonly Char[] InvalidPathNameCharacters = null;

        /// <summary>
        /// The list of invalid filename characters.
        /// </summary>
        /// <remarks>
        /// This field just holds the system list of filename characters that are 
        /// considered as invalid.
        /// </remarks>
        /// <seealso cref="Path.GetInvalidFileNameChars()"/>
        private static readonly Char[] InvalidFileNameCharacters = null;

        /// <summary>
        /// The default directory to be used as fallback if the filename does not 
        /// include a folder path.
        /// </summary>
        /// <remarks>
        /// This field just holds the default directory, which is set to subdirectory 
        /// <c>logging</c> of current user's temporary directory. 
        /// </remarks>
        private static readonly String DefaultDirectory = null;

        #endregion

        #region Construction

        /// <summary>
        /// Default static constructor.
        /// </summary>
        /// <remarks>
        /// This constructor just initializes all static fields.
        /// </remarks>
        static SettingsValidationExtension()
        {
            SettingsValidationExtension.PathSeparatorCharacters = new Char[] { Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar };

            List<Char> temp = new List<Char>(Path.GetInvalidPathChars());
            temp.AddRange(new Char[] { '*', '?' });

            SettingsValidationExtension.InvalidPathNameCharacters = temp.ToArray();

            SettingsValidationExtension.InvalidFileNameCharacters = Path.GetInvalidFileNameChars();

            SettingsValidationExtension.DefaultDirectory = Path.Combine(Path.GetTempPath(), "logging");
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Validates filename and path.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This method just checks if provided <paramref name="filename"/> is valid. 
        /// As result the returned filename is converted into a fully qualified path.
        /// </para>
        /// <para>
        /// Keep in mind, value of <see cref="SettingsValidationExtension.DefaultDirectory"/> 
        /// is used as fallback in case of the path part of provided filename is not set.
        /// </para>
        /// <para>
        /// Additionally, be aware the provided parameter <paramref name="filename"/> 
        /// can contain environment variables, such as <c>%TMP%</c> for example. But note, 
        /// no matter which platform is used, each of the environment variables must be 
        /// surrounded by a percentage character (%). Otherwise, resolving environment 
        /// variables may fail and ends up in an exception.
        /// </para>
        /// </remarks>
        /// <example>
        /// Below find some examples of how to use environment variables in filenames.
        /// <code language="C#">
        /// settings.Filename = "%TMP%\\output.log";
        /// settings.Filename = "%TEMP%\\output.log";
        /// settings.Filename = "%LOCALAPPDATA%\\Temp\\output.log";
        /// settings.Filename = "%HOMEDRIVE%%HOMEPATH%\\AppData\\Local\\Temp\\output.log";
        /// </code>
        /// </example>
        /// <param name="filename">
        /// The filename to be checked.
        /// </param>
        /// <returns>
        /// The fully qualified and trimmed filename.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// This exception is thrown in case of parameter <paramref name="filename"/> is 
        /// <c>null</c>, <em>empty</em> or consists only of <em>whitespaces</em> or if this 
        /// parameter includes a folder path only.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// This exception is thrown in case of the <paramref name="filename"/> contains 
        /// invalid characters or the filename references a directory.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// This exception is thrown in case of the directory path of the file could not 
        /// be created. In such a case the inner exception contains detailed information.
        /// </exception>
        /// <seealso cref="SettingsValidationExtension.EnsureFullPathAndWriteAccessOrThrow(String)"/>
        public static String EnsureFullFilePathOrThrow(this String filename)
        {
            if (String.IsNullOrWhiteSpace(filename))
            {
                throw new ArgumentOutOfRangeException(nameof(filename), "The provided filename is invalid.");
            }

            filename = filename.Trim();

            String path = String.Empty;
            String file = String.Empty;

            Int32 index = filename.LastIndexOfAny(SettingsValidationExtension.PathSeparatorCharacters);

            if (index >= 0)
            {
                path = filename.Substring(0, index).TrimEnd(SettingsValidationExtension.PathSeparatorCharacters);
                file = filename.Substring(index + 1);
            }
            else
            {
                file = filename;
            }

            if (String.IsNullOrWhiteSpace(path))
            {
                path = SettingsValidationExtension.DefaultDirectory;
            }

            path = Environment.ExpandEnvironmentVariables(path);

            if (path.IndexOfAny(SettingsValidationExtension.InvalidPathNameCharacters) >= 0)
            {
                throw new ArgumentException($"The path \"{path}\" contains invalid characters.", nameof(filename));
            }

            if (String.IsNullOrWhiteSpace(file))
            {
                throw new ArgumentOutOfRangeException(nameof(filename), $"The file \"{file}\" of provided filename is invalid.");
            }

            if (file.IndexOfAny(SettingsValidationExtension.InvalidFileNameCharacters) >= 0)
            {
                throw new ArgumentException($"The file \"{file}\" contains invalid characters.", nameof(filename));
            }

            if (!Directory.Exists(path))
            {
                try
                {
                    Directory.CreateDirectory(path);
                }
                catch (Exception exception)
                {
                    throw new InvalidOperationException($"Directory \"{path}\" could not be created.", exception);
                }
            }

            FileInfo info = new FileInfo(Path.Combine(path, file));
            if (info.Attributes > 0 && (info.Attributes & FileAttributes.Directory) == FileAttributes.Directory)
            {
                throw new ArgumentException($"The filename \"{filename}\" is a directory.", nameof(filename));
            }

            return info.FullName;
        }

        /// <summary>
        /// Validates filename, path and file access.
        /// </summary>
        /// <remarks>
        /// This method validates the <paramref name="filename"/>, the file path as well 
        /// as the file access rights. If the file does not yet exists it is created but 
        /// deleted afterwards.
        /// </remarks>
        /// <param name="filename">
        /// The fully qualified filename to be checked.
        /// </param>
        /// <returns>
        /// The fully qualified and trimmed filename.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// This exception is thrown in case of the full name, the path name or the name of 
        /// the file is <c>null</c>, <em>empty</em> or consists only of <em>whitespaces</em>.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// This exception is thrown in case of the <paramref name="filename"/> contains 
        /// invalid characters.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// This exception is thrown in case of the directory path of the file could not be 
        /// created. In such a case the inner exception contains detailed information.
        /// </exception>
        /// <exception cref="UnauthorizedAccessException">
        /// This exception is thrown in case of the file could not be created or opened for 
        /// write operations.
        /// </exception>
        /// <exception cref="IOException">
        /// This exception is thrown in case of an I/O error occurred while opening the file.
        /// </exception>
        /// <exception cref="Exception">
        /// Other exceptions may be occur as well.
        /// </exception>
        /// <seealso cref="SettingsValidationExtension.EnsureFullFilePathOrThrow(String)"/>
        public static String EnsureFullPathAndWriteAccessOrThrow(this String filename)
        {
            filename = filename.EnsureFullFilePathOrThrow();

            // For sure, I'm not happy with this solution. But, for example, the 
            // class FileIOPermission is not available in a .NET Standard project.

            Boolean remove = !File.Exists(filename);

            using (FileStream stream = File.Open(filename, FileMode.OpenOrCreate, FileAccess.Write, FileShare.ReadWrite))
            {
                stream.Close();
            }

            if (remove && File.Exists(filename))
            {
                try { File.Delete(filename); } catch { }
            }

            return filename;
        }

        #endregion
    }
}
