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
using Plexdata.LogWriter.Logging;
using Plexdata.LogWriter.Settings;
using System;
using System.IO;

namespace Plexdata.LogWriter.Extensions
{
    /// <summary>
    /// Allows an easy registration of any of the supported logger types.
    /// </summary>
    /// <remarks>
    /// This extension provides various methods to easily assign various 
    /// logger types to a composite logger instance.
    /// </remarks>
    public static class CompositeLoggerExtension
    {
        #region Console logger

        /// <summary>
        /// Adds a new console logger with default settings.
        /// </summary>
        /// <remarks>
        /// This method adds a new console logger using default console 
        /// logger settings.
        /// </remarks>
        /// <param name="parent">
        /// The composite logger to add the stantard console logger to.
        /// </param>
        /// <returns>
        /// The provided composite logger.
        /// </returns>
        public static ICompositeLogger AddConsoleLogger(this ICompositeLogger parent)
        {
            return CompositeLoggerExtension.AddConsoleLogger(parent, false);
        }

        /// <summary>
        /// Adds a new console logger default settings.
        /// </summary>
        /// <remarks>
        /// This method adds a new console logger using default console 
        /// logger settings. Depending on value <paramref name="windows"/> 
        /// either the Windows compatible logger or the standard logger 
        /// is chosen.
        /// </remarks>
        /// <param name="parent">
        /// The composite logger to add the console logger to.
        /// </param>
        /// <param name="windows">
        /// True to choose the windows version and false to take the standard 
        /// version instead.
        /// </param>
        /// <returns>
        /// The provided composite logger.
        /// </returns>
        public static ICompositeLogger AddConsoleLogger(this ICompositeLogger parent, Boolean windows)
        {
            return CompositeLoggerExtension.AddConsoleLogger(parent, new ConsoleLoggerSettings(), windows);
        }

        /// <summary>
        /// Adds a new console logger with settings that are read from 
        /// <paramref name="section"/> parameter.
        /// </summary>
        /// <remarks>
        /// This method adds a new stantard console logger using the 
        /// <paramref name="section"/> parameter to determine the 
        /// settings from.
        /// </remarks>
        /// <param name="parent">
        /// The composite logger to add the stantard console logger to.
        /// </param>
        /// <param name="section">
        /// The configuration section to get all console logger settings 
        /// from.
        /// </param>
        /// <returns>
        /// The provided composite logger.
        /// </returns>
        public static ICompositeLogger AddConsoleLogger(this ICompositeLogger parent, ILoggerSettingsSection section)
        {
            return CompositeLoggerExtension.AddConsoleLogger(parent, section, false);
        }

        /// <summary>
        /// Adds a new console logger with settings that are read from 
        /// <paramref name="section"/> parameter.
        /// </summary>
        /// <remarks>
        /// This method adds a new console logger using the <paramref name="section"/> 
        /// parameter to determine the settings from.
        /// </remarks>
        /// <param name="parent">
        /// The composite logger to add the console logger to.
        /// </param>
        /// <param name="section">
        /// The configuration section to get all console logger settings from.
        /// </param>
        /// <param name="windows">
        /// True to choose the windows version and false to take the standard 
        /// version instead.
        /// </param>
        /// <returns>
        /// The provided composite logger.
        /// </returns>
        public static ICompositeLogger AddConsoleLogger(this ICompositeLogger parent, ILoggerSettingsSection section, Boolean windows)
        {
            return CompositeLoggerExtension.AddConsoleLogger(parent, new ConsoleLoggerSettings(section), windows);
        }

        /// <summary>
        /// Adds a new console logger with setting of provided 
        /// <paramref name="settings"/> parameter.
        /// </summary>
        /// <remarks>
        /// This method adds a new stantard console logger using the 
        /// <paramref name="settings"/> parameter to read settings 
        /// from.
        /// </remarks>
        /// <param name="parent">
        /// The composite logger to add the console logger to.
        /// </param>
        /// <param name="settings">
        /// The settings to get all console logger settings from.
        /// </param>
        /// <returns>
        /// The provided composite logger.
        /// </returns>
        public static ICompositeLogger AddConsoleLogger(this ICompositeLogger parent, IConsoleLoggerSettings settings)
        {
            return CompositeLoggerExtension.AddConsoleLogger(parent, settings, false);
        }

        /// <summary>
        /// Adds a new console logger with setting of provided 
        /// <paramref name="settings"/> parameter.
        /// </summary>
        /// <remarks>
        /// This method adds a new console logger using the 
        /// <paramref name="settings"/> parameter to read settings 
        /// from.
        /// </remarks>
        /// <param name="parent">
        /// The composite logger to add the console logger to.
        /// </param>
        /// <param name="settings">
        /// The settings to get all console logger settings from.
        /// </param>
        /// <param name="windows">
        /// True to choose the windows version and false to take 
        /// the standard version instead.
        /// </param>
        /// <returns>
        /// The provided composite logger.
        /// </returns>
        public static ICompositeLogger AddConsoleLogger(this ICompositeLogger parent, IConsoleLoggerSettings settings, Boolean windows)
        {
            if (windows)
            {
                return CompositeLoggerExtension.AddConsoleLogger(parent, new Logging.Windows.ConsoleLogger(settings));
            }
            else
            {
                return CompositeLoggerExtension.AddConsoleLogger(parent, new Logging.Standard.ConsoleLogger(settings));
            }
        }

        /// <summary>
        /// Adds a new console logger.
        /// </summary>
        /// <remarks>
        /// This method adds a new console logger.
        /// </remarks>
        /// <param name="parent">
        /// The composite logger to add the console logger to.
        /// </param>
        /// <param name="logger">
        /// The console logger instance to add.
        /// </param>
        /// <returns>
        /// The provided composite logger.
        /// </returns>
        public static ICompositeLogger AddConsoleLogger(this ICompositeLogger parent, IConsoleLogger logger)
        {
            if (parent != null)
            {
                parent.AddLogger(logger as ILogger);
            }

            return parent;
        }

        /// <summary>
        /// Adds a new console logger with default settings.
        /// </summary>
        /// <remarks>
        /// This method adds a new console logger using default console 
        /// logger settings.
        /// </remarks>
        /// <typeparam name="TContext">
        /// The type of the logger context.
        /// </typeparam>
        /// <param name="parent">
        /// The composite logger to add the stantard console logger to.
        /// </param>
        /// <returns>
        /// The provided composite logger.
        /// </returns>
        public static ICompositeLogger<TContext> AddConsoleLogger<TContext>(this ICompositeLogger<TContext> parent)
        {
            return CompositeLoggerExtension.AddConsoleLogger(parent, false);
        }

        /// <summary>
        /// Adds a new console logger default settings.
        /// </summary>
        /// <remarks>
        /// This method adds a new console logger using default console 
        /// logger settings. Depending on value <paramref name="windows"/> 
        /// either the Windows compatible logger or the standard logger 
        /// is chosen.
        /// </remarks>
        /// <typeparam name="TContext">
        /// The type of the logger context.
        /// </typeparam>
        /// <param name="parent">
        /// The composite logger to add the console logger to.
        /// </param>
        /// <param name="windows">
        /// True to choose the windows version and false to take the standard 
        /// version instead.
        /// </param>
        /// <returns>
        /// The provided composite logger.
        /// </returns>
        public static ICompositeLogger<TContext> AddConsoleLogger<TContext>(this ICompositeLogger<TContext> parent, Boolean windows)
        {
            return CompositeLoggerExtension.AddConsoleLogger(parent, new ConsoleLoggerSettings(), windows);
        }

        /// <summary>
        /// Adds a new console logger with settings that are read from 
        /// <paramref name="section"/> parameter.
        /// </summary>
        /// <remarks>
        /// This method adds a new console logger using the <paramref name="section"/> 
        /// parameter to determine the settings from.
        /// </remarks>
        /// <typeparam name="TContext">
        /// The type of the logger context.
        /// </typeparam>
        /// <param name="parent">
        /// The composite logger to add the console logger to.
        /// </param>
        /// <param name="section">
        /// The configuration section to get all console logger settings from.
        /// </param>
        /// <returns>
        /// The provided composite logger.
        /// </returns>
        public static ICompositeLogger<TContext> AddConsoleLogger<TContext>(this ICompositeLogger<TContext> parent, ILoggerSettingsSection section)
        {
            return CompositeLoggerExtension.AddConsoleLogger(parent, section, false);
        }

        /// <summary>
        /// Adds a new console logger with settings that are read from 
        /// <paramref name="section"/> parameter.
        /// </summary>
        /// <remarks>
        /// This method adds a new console logger using the <paramref name="section"/> 
        /// parameter to determine the settings from.
        /// </remarks>
        /// <typeparam name="TContext">
        /// The type of the logger context.
        /// </typeparam>
        /// <param name="parent">
        /// The composite logger to add the console logger to.
        /// </param>
        /// <param name="section">
        /// The configuration section to get all console logger settings from.
        /// </param>
        /// <param name="windows">
        /// True to choose the windows version and false to take the standard 
        /// version instead.
        /// </param>
        /// <returns>
        /// The provided composite logger.
        /// </returns>
        public static ICompositeLogger<TContext> AddConsoleLogger<TContext>(this ICompositeLogger<TContext> parent, ILoggerSettingsSection section, Boolean windows)
        {
            return CompositeLoggerExtension.AddConsoleLogger(parent, new ConsoleLoggerSettings(section), windows);
        }

        /// <summary>
        /// Adds a new console logger with setting of provided 
        /// <paramref name="settings"/> parameter.
        /// </summary>
        /// <remarks>
        /// This method adds a new stantard console logger using the 
        /// <paramref name="settings"/> parameter to read settings 
        /// from.
        /// </remarks>
        /// <typeparam name="TContext">
        /// The type of the logger context.
        /// </typeparam>
        /// <param name="parent">
        /// The composite logger to add the console logger to.
        /// </param>
        /// <param name="settings">
        /// The settings to get all console logger settings from.
        /// </param>
        /// <returns>
        /// The provided composite logger.
        /// </returns>
        public static ICompositeLogger<TContext> AddConsoleLogger<TContext>(this ICompositeLogger<TContext> parent, IConsoleLoggerSettings settings)
        {
            return CompositeLoggerExtension.AddConsoleLogger(parent, settings, false);
        }

        /// <summary>
        /// Adds a new console logger with setting of provided 
        /// <paramref name="settings"/> parameter.
        /// </summary>
        /// <remarks>
        /// This method adds a new console logger using the 
        /// <paramref name="settings"/> parameter to read settings 
        /// from.
        /// </remarks>
        /// <typeparam name="TContext">
        /// The type of the logger context.
        /// </typeparam>
        /// <param name="parent">
        /// The composite logger to add the console logger to.
        /// </param>
        /// <param name="settings">
        /// The settings to get all console logger settings from.
        /// </param>
        /// <param name="windows">
        /// True to choose the windows version and false to take 
        /// the standard version instead.
        /// </param>
        /// <returns>
        /// The provided composite logger.
        /// </returns>
        public static ICompositeLogger<TContext> AddConsoleLogger<TContext>(this ICompositeLogger<TContext> parent, IConsoleLoggerSettings settings, Boolean windows)
        {
            if (windows)
            {
                return CompositeLoggerExtension.AddConsoleLogger(parent, new Logging.Windows.ConsoleLogger<TContext>(settings));
            }
            else
            {
                return CompositeLoggerExtension.AddConsoleLogger(parent, new Logging.Standard.ConsoleLogger<TContext>(settings));
            }
        }

        /// <summary>
        /// Adds a new console logger.
        /// </summary>
        /// <remarks>
        /// This method adds a new console logger.
        /// </remarks>
        /// <typeparam name="TContext">
        /// The type of the logger context.
        /// </typeparam>
        /// <param name="parent">
        /// The composite logger to add the console logger to.
        /// </param>
        /// <param name="logger">
        /// The console logger instance to add.
        /// </param>
        /// <returns>
        /// The provided composite logger.
        /// </returns>
        public static ICompositeLogger<TContext> AddConsoleLogger<TContext>(this ICompositeLogger<TContext> parent, IConsoleLogger<TContext> logger)
        {
            if (parent != null)
            {
                parent.AddLogger(logger as ILogger<TContext>);
            }

            return parent;
        }

        #endregion

        #region Persistent logger

        /// <summary>
        /// Adds a new persistent logger with default settings.
        /// </summary>
        /// <remarks>
        /// This method adds a new persistent logger using default persistent 
        /// logger settings.
        /// </remarks>
        /// <param name="parent">
        /// The composite logger to add the stantard persistent logger to.
        /// </param>
        /// <returns>
        /// The provided composite logger.
        /// </returns>
        public static ICompositeLogger AddPersistentLogger(this ICompositeLogger parent)
        {
            return CompositeLoggerExtension.AddPersistentLogger(parent, (String)null);
        }

        /// <summary>
        /// Adds a new persistent logger with default settings.
        /// </summary>
        /// <remarks>
        /// This method adds a new persistent logger using default persistent 
        /// logger settings but changes the used logging filename if not invalid.
        /// </remarks>
        /// <param name="parent">
        /// The composite logger to add the stantard persistent logger to.
        /// </param>
        /// <param name="filename">
        /// The fully qualified logging filename.
        /// </param>
        /// <returns>
        /// The provided composite logger.
        /// </returns>
        public static ICompositeLogger AddPersistentLogger(this ICompositeLogger parent, String filename)
        {
            return CompositeLoggerExtension.AddPersistentLogger(parent, new PersistentLoggerSettings(), filename);
        }

        /// <summary>
        /// Adds a new persistent logger with settings that are read from 
        /// <paramref name="section"/> parameter.
        /// </summary>
        /// <remarks>
        /// This method adds a new persistent logger using the 
        /// <paramref name="section"/> parameter to determine the 
        /// settings from.
        /// </remarks>
        /// <param name="parent">
        /// The composite logger to add the persistent logger to.
        /// </param>
        /// <param name="section">
        /// The configuration section to get all persistent logger settings 
        /// from.
        /// </param>
        /// <returns>
        /// The provided composite logger.
        /// </returns>
        public static ICompositeLogger AddPersistentLogger(this ICompositeLogger parent, ILoggerSettingsSection section)
        {
            return CompositeLoggerExtension.AddPersistentLogger(parent, section, null);
        }

        /// <summary>
        /// Adds a new persistent logger with settings that are read from 
        /// <paramref name="section"/> parameter but changes the used logging 
        /// filename if not invalid.
        /// </summary>
        /// <remarks>
        /// This method adds a new persistent logger using the 
        /// <paramref name="section"/> parameter to determine the 
        /// settings from.
        /// </remarks>
        /// <param name="parent">
        /// The composite logger to add the persistent logger to.
        /// </param>
        /// <param name="section">
        /// The configuration section to get all persistent logger settings 
        /// from.
        /// </param>
        /// <param name="filename">
        /// The fully qualified logging filename.
        /// </param>
        /// <returns>
        /// The provided composite logger.
        /// </returns>
        public static ICompositeLogger AddPersistentLogger(this ICompositeLogger parent, ILoggerSettingsSection section, String filename)
        {
            return CompositeLoggerExtension.AddPersistentLogger(parent, new PersistentLoggerSettings(section), filename);
        }

        /// <summary>
        /// Adds a new persistent logger with setting of provided 
        /// <paramref name="settings"/> parameter.
        /// </summary>
        /// <remarks>
        /// This method adds a new persistent logger using the 
        /// <paramref name="settings"/> parameter to determine the 
        /// settings from.
        /// </remarks>
        /// <param name="parent">
        /// The composite logger to add the persistent logger to.
        /// </param>
        /// <param name="settings">
        /// The settings to get all persistent logger settings from.
        /// </param>
        /// <returns>
        /// The provided composite logger.
        /// </returns>
        public static ICompositeLogger AddPersistentLogger(this ICompositeLogger parent, IPersistentLoggerSettings settings)
        {
            return CompositeLoggerExtension.AddPersistentLogger(parent, settings, null);
        }

        /// <summary>
        /// Adds a new persistent logger with setting of provided 
        /// <paramref name="settings"/> parameter but changes the 
        /// used logging filename if not invalid.
        /// </summary>
        /// <remarks>
        /// This method adds a new persistent logger using the 
        /// <paramref name="settings"/> parameter to determine the 
        /// settings from.
        /// </remarks>
        /// <param name="parent">
        /// The composite logger to add the persistent logger to.
        /// </param>
        /// <param name="settings">
        /// The settings to get all persistent logger settings from.
        /// </param>
        /// <param name="filename">
        /// The fully qualified logging filename.
        /// </param>
        /// <returns>
        /// The provided composite logger.
        /// </returns>
        public static ICompositeLogger AddPersistentLogger(this ICompositeLogger parent, IPersistentLoggerSettings settings, String filename)
        {
            if (settings != null && !String.IsNullOrWhiteSpace(filename))
            {
                settings.Filename = filename;
            }

            return CompositeLoggerExtension.AddPersistentLogger(parent, new PersistentLogger(settings));
        }

        /// <summary>
        /// Adds a new persistent logger.
        /// </summary>
        /// <remarks>
        /// This method adds a new persistent logger.
        /// </remarks>
        /// <param name="parent">
        /// The composite logger to add the persistent logger to.
        /// </param>
        /// <param name="logger">
        /// The persistent logger instance to add.
        /// </param>
        /// <returns>
        /// The provided composite logger.
        /// </returns>
        public static ICompositeLogger AddPersistentLogger(this ICompositeLogger parent, IPersistentLogger logger)
        {
            if (parent != null)
            {
                parent.AddLogger(logger as ILogger);
            }

            return parent;
        }

        /// <summary>
        /// Adds a new persistent logger with default settings.
        /// </summary>
        /// <remarks>
        /// This method adds a new persistent logger using default persistent 
        /// logger settings.
        /// </remarks>
        /// <typeparam name="TContext">
        /// The type of the logger context.
        /// </typeparam>
        /// <param name="parent">
        /// The composite logger to add the stantard persistent logger to.
        /// </param>
        /// <returns>
        /// The provided composite logger.
        /// </returns>
        public static ICompositeLogger<TContext> AddPersistentLogger<TContext>(this ICompositeLogger<TContext> parent)
        {
            return CompositeLoggerExtension.AddPersistentLogger(parent, (String)null);
        }

        /// <summary>
        /// Adds a new persistent logger with default settings.
        /// </summary>
        /// <remarks>
        /// This method adds a new persistent logger using default persistent 
        /// logger settings but changes the used logging filename if not invalid.
        /// </remarks>
        /// <typeparam name="TContext">
        /// The type of the logger context.
        /// </typeparam>
        /// <param name="parent">
        /// The composite logger to add the stantard persistent logger to.
        /// </param>
        /// <param name="filename">
        /// The fully qualified logging filename.
        /// </param>
        /// <returns>
        /// The provided composite logger.
        /// </returns>
        public static ICompositeLogger<TContext> AddPersistentLogger<TContext>(this ICompositeLogger<TContext> parent, String filename)
        {
            return CompositeLoggerExtension.AddPersistentLogger(parent, new PersistentLoggerSettings(), filename);
        }

        /// <summary>
        /// Adds a new persistent logger with settings that are read from 
        /// <paramref name="section"/> parameter.
        /// </summary>
        /// <remarks>
        /// This method adds a new persistent logger using the 
        /// <paramref name="section"/> parameter to determine the 
        /// settings from.
        /// </remarks>
        /// <typeparam name="TContext">
        /// The type of the logger context.
        /// </typeparam>
        /// <param name="parent">
        /// The composite logger to add the persistent logger to.
        /// </param>
        /// <param name="section">
        /// The configuration section to get all persistent logger settings 
        /// from.
        /// </param>
        /// <returns>
        /// The provided composite logger.
        /// </returns>
        public static ICompositeLogger<TContext> AddPersistentLogger<TContext>(this ICompositeLogger<TContext> parent, ILoggerSettingsSection section)
        {
            return CompositeLoggerExtension.AddPersistentLogger(parent, section, null);
        }

        /// <summary>
        /// Adds a new persistent logger with settings that are read from 
        /// <paramref name="section"/> parameter but changes the used logging 
        /// filename if not invalid.
        /// </summary>
        /// <remarks>
        /// This method adds a new persistent logger using the 
        /// <paramref name="section"/> parameter to determine the 
        /// settings from.
        /// </remarks>
        /// <typeparam name="TContext">
        /// The type of the logger context.
        /// </typeparam>
        /// <param name="parent">
        /// The composite logger to add the persistent logger to.
        /// </param>
        /// <param name="section">
        /// The configuration section to get all persistent logger settings 
        /// from.
        /// </param>
        /// <param name="filename">
        /// The fully qualified logging filename.
        /// </param>
        /// <returns>
        /// The provided composite logger.
        /// </returns>
        public static ICompositeLogger<TContext> AddPersistentLogger<TContext>(this ICompositeLogger<TContext> parent, ILoggerSettingsSection section, String filename)
        {
            return CompositeLoggerExtension.AddPersistentLogger(parent, new PersistentLoggerSettings(section), null);
        }

        /// <summary>
        /// Adds a new persistent logger with setting of provided 
        /// <paramref name="settings"/> parameter.
        /// </summary>
        /// <remarks>
        /// This method adds a new persistent logger using the 
        /// <paramref name="settings"/> parameter to determine the 
        /// settings from.
        /// </remarks>
        /// <typeparam name="TContext">
        /// The type of the logger context.
        /// </typeparam>
        /// <param name="parent">
        /// The composite logger to add the persistent logger to.
        /// </param>
        /// <param name="settings">
        /// The settings to get all persistent logger settings from.
        /// </param>
        /// <returns>
        /// The provided composite logger.
        /// </returns>
        public static ICompositeLogger<TContext> AddPersistentLogger<TContext>(this ICompositeLogger<TContext> parent, IPersistentLoggerSettings settings)
        {
            return CompositeLoggerExtension.AddPersistentLogger(parent, new PersistentLogger<TContext>(settings));
        }

        /// <summary>
        /// Adds a new persistent logger with settings logger with setting of 
        /// provided <paramref name="settings"/> parameter but changes the used 
        /// logging filename if not invalid.
        /// </summary>
        /// <remarks>
        /// This method adds a new persistent logger using the 
        /// <paramref name="settings"/> parameter to determine the 
        /// settings from.
        /// </remarks>
        /// <typeparam name="TContext">
        /// The type of the logger context.
        /// </typeparam>
        /// <param name="parent">
        /// The composite logger to add the persistent logger to.
        /// </param>
        /// <param name="settings">
        /// The settings to get all persistent logger settings from.
        /// </param>
        /// <param name="filename">
        /// The fully qualified logging filename.
        /// </param>
        /// <returns>
        /// The provided composite logger.
        /// </returns>
        public static ICompositeLogger<TContext> AddPersistentLogger<TContext>(this ICompositeLogger<TContext> parent, IPersistentLoggerSettings settings, String filename)
        {
            if (settings != null && !String.IsNullOrWhiteSpace(filename))
            {
                settings.Filename = filename;
            }

            return CompositeLoggerExtension.AddPersistentLogger(parent, new PersistentLogger<TContext>(settings));
        }

        /// <summary>
        /// Adds a new persistent logger.
        /// </summary>
        /// <remarks>
        /// This method adds a new persistent logger.
        /// </remarks>
        /// <typeparam name="TContext">
        /// The type of the logger context.
        /// </typeparam>
        /// <param name="parent">
        /// The composite logger to add the persistent logger to.
        /// </param>
        /// <param name="logger">
        /// The persistent logger instance to add.
        /// </param>
        /// <returns>
        /// The provided composite logger.
        /// </returns>
        public static ICompositeLogger<TContext> AddPersistentLogger<TContext>(this ICompositeLogger<TContext> parent, IPersistentLogger<TContext> logger)
        {
            if (parent != null)
            {
                parent.AddLogger(logger as ILogger<TContext>);
            }

            return parent;
        }

        #endregion

        #region Stream logger

        /// <summary>
        /// Adds a new stream logger with default settings.
        /// </summary>
        /// <remarks>
        /// This method adds a new stream logger using default stream 
        /// logger settings.
        /// </remarks>
        /// <param name="parent">
        /// The composite logger to add the stantard stream logger to.
        /// </param>
        /// <returns>
        /// The provided composite logger.
        /// </returns>
        public static ICompositeLogger AddStreamLogger(this ICompositeLogger parent)
        {
            return CompositeLoggerExtension.AddStreamLogger(parent, (Stream)null);
        }

        /// <summary>
        /// Adds a new stream logger with default settings.
        /// </summary>
        /// <remarks>
        /// This method adds a new stream logger using default stream 
        /// logger settings but uses provided stream.
        /// </remarks>
        /// <param name="parent">
        /// The composite logger to add the stantard stream logger to.
        /// </param>
        /// <param name="stream">
        /// The stream to be used.
        /// </param>
        /// <returns>
        /// The provided composite logger.
        /// </returns>
        public static ICompositeLogger AddStreamLogger(this ICompositeLogger parent, Stream stream)
        {
            return CompositeLoggerExtension.AddStreamLogger(parent, new StreamLoggerSettings(stream));
        }

        /// <summary>
        /// Adds a new stream logger with settings that are read from 
        /// <paramref name="section"/> parameter.
        /// </summary>
        /// <remarks>
        /// This method adds a new stream logger using the 
        /// <paramref name="section"/> parameter to determine the 
        /// settings from.
        /// </remarks>
        /// <param name="parent">
        /// The composite logger to add the stream logger to.
        /// </param>
        /// <param name="section">
        /// The configuration section to get all stream logger settings 
        /// from.
        /// </param>
        /// <returns>
        /// The provided composite logger.
        /// </returns>
        public static ICompositeLogger AddStreamLogger(this ICompositeLogger parent, ILoggerSettingsSection section)
        {
            return CompositeLoggerExtension.AddStreamLogger(parent, section, null);
        }

        /// <summary>
        /// Adds a new stream logger with settings that are read from 
        /// <paramref name="section"/> parameter but uses provided stream.
        /// </summary>
        /// <remarks>
        /// This method adds a new stream logger using the 
        /// <paramref name="section"/> parameter to determine the 
        /// settings from.
        /// </remarks>
        /// <param name="parent">
        /// The composite logger to add the stream logger to.
        /// </param>
        /// <param name="section">
        /// The configuration section to get all stream logger settings 
        /// from.
        /// </param>
        /// <param name="stream">
        /// The stream to be used.
        /// </param>
        /// <returns>
        /// The provided composite logger.
        /// </returns>
        public static ICompositeLogger AddStreamLogger(this ICompositeLogger parent, ILoggerSettingsSection section, Stream stream)
        {
            return CompositeLoggerExtension.AddStreamLogger(parent, new StreamLoggerSettings(section, stream));
        }

        /// <summary>
        /// Adds a new stream logger with setting of provided 
        /// <paramref name="settings"/> parameter.
        /// </summary>
        /// <remarks>
        /// This method adds a new stream logger using the 
        /// <paramref name="settings"/> parameter to determine the 
        /// settings from.
        /// </remarks>
        /// <param name="parent">
        /// The composite logger to add the stream logger to.
        /// </param>
        /// <param name="settings">
        /// The configuration section to get all stream logger settings 
        /// from.
        /// </param>
        /// <returns>
        /// The provided composite logger.
        /// </returns>
        public static ICompositeLogger AddStreamLogger(this ICompositeLogger parent, IStreamLoggerSettings settings)
        {
            return CompositeLoggerExtension.AddStreamLogger(parent, new StreamLogger(settings));
        }

        /// <summary>
        /// Adds a new stream logger.
        /// </summary>
        /// <remarks>
        /// This method adds a new stream logger.
        /// </remarks>
        /// <param name="parent">
        /// The composite logger to add the stream logger to.
        /// </param>
        /// <param name="logger">
        /// The stream logger instance to add.
        /// </param>
        /// <returns>
        /// The provided composite logger.
        /// </returns>
        public static ICompositeLogger AddStreamLogger(this ICompositeLogger parent, IStreamLogger logger)
        {
            if (parent != null)
            {
                parent.AddLogger(logger as ILogger);
            }

            return parent;
        }

        /// <summary>
        /// Adds a new stream logger with default settings.
        /// </summary>
        /// <remarks>
        /// This method adds a new stream logger using default stream 
        /// logger settings.
        /// </remarks>
        /// <typeparam name="TContext">
        /// The type of the logger context.
        /// </typeparam>
        /// <param name="parent">
        /// The composite logger to add the stantard stream logger to.
        /// </param>
        /// <returns>
        /// The provided composite logger.
        /// </returns>
        public static ICompositeLogger<TContext> AddStreamLogger<TContext>(this ICompositeLogger<TContext> parent)
        {
            return CompositeLoggerExtension.AddStreamLogger(parent, (Stream)null);
        }

        /// <summary>
        /// Adds a new stream logger with default settings.
        /// </summary>
        /// <remarks>
        /// This method adds a new stream logger using default stream 
        /// logger settings but uses provided stream.
        /// </remarks>
        /// <typeparam name="TContext">
        /// The type of the logger context.
        /// </typeparam>
        /// <param name="parent">
        /// The composite logger to add the stantard stream logger to.
        /// </param>
        /// <param name="stream">
        /// The stream to be used.
        /// </param>
        /// <returns>
        /// The provided composite logger.
        /// </returns>
        public static ICompositeLogger<TContext> AddStreamLogger<TContext>(this ICompositeLogger<TContext> parent, Stream stream)
        {
            return CompositeLoggerExtension.AddStreamLogger(parent, new StreamLoggerSettings(stream));
        }

        /// <summary>
        /// Adds a new stream logger with settings that are read from 
        /// <paramref name="section"/> parameter.
        /// </summary>
        /// <remarks>
        /// This method adds a new stream logger using the 
        /// <paramref name="section"/> parameter to determine the 
        /// settings from.
        /// </remarks>
        /// <typeparam name="TContext">
        /// The type of the logger context.
        /// </typeparam>
        /// <param name="parent">
        /// The composite logger to add the stream logger to.
        /// </param>
        /// <param name="section">
        /// The configuration section to get all stream logger settings 
        /// from.
        /// </param>
        /// <returns>
        /// The provided composite logger.
        /// </returns>
        public static ICompositeLogger<TContext> AddStreamLogger<TContext>(this ICompositeLogger<TContext> parent, ILoggerSettingsSection section)
        {
            return CompositeLoggerExtension.AddStreamLogger(parent, section, null);
        }

        /// <summary>
        /// Adds a new stream logger with settings that are read from 
        /// <paramref name="section"/> parameter but uses provided stream.
        /// </summary>
        /// <remarks>
        /// This method adds a new stream logger using the 
        /// <paramref name="section"/> parameter to determine the 
        /// settings from.
        /// </remarks>
        /// <typeparam name="TContext">
        /// The type of the logger context.
        /// </typeparam>
        /// <param name="parent">
        /// The composite logger to add the stream logger to.
        /// </param>
        /// <param name="section">
        /// The configuration section to get all stream logger settings 
        /// from.
        /// </param>
        /// <param name="stream">
        /// The stream to be used.
        /// </param>
        /// <returns>
        /// The provided composite logger.
        /// </returns>
        public static ICompositeLogger<TContext> AddStreamLogger<TContext>(this ICompositeLogger<TContext> parent, ILoggerSettingsSection section, Stream stream)
        {
            return CompositeLoggerExtension.AddStreamLogger(parent, new StreamLoggerSettings(section, stream));
        }

        /// <summary>
        /// Adds a new stream logger with setting of provided 
        /// <paramref name="settings"/> parameter.
        /// </summary>
        /// <remarks>
        /// This method adds a new stream logger using the 
        /// <paramref name="settings"/> parameter to determine the 
        /// settings from.
        /// </remarks>
        /// <typeparam name="TContext">
        /// The type of the logger context.
        /// </typeparam>
        /// <param name="parent">
        /// The composite logger to add the stream logger to.
        /// </param>
        /// <param name="settings">
        /// The configuration section to get all stream logger settings 
        /// from.
        /// </param>
        /// <returns>
        /// The provided composite logger.
        /// </returns>
        public static ICompositeLogger<TContext> AddStreamLogger<TContext>(this ICompositeLogger<TContext> parent, IStreamLoggerSettings settings)
        {
            return CompositeLoggerExtension.AddStreamLogger(parent, new StreamLogger<TContext>(settings));
        }

        /// <summary>
        /// Adds a new stream logger.
        /// </summary>
        /// <remarks>
        /// This method adds a new stream logger.
        /// </remarks>
        /// <typeparam name="TContext">
        /// The type of the logger context.
        /// </typeparam>
        /// <param name="parent">
        /// The composite logger to add the stream logger to.
        /// </param>
        /// <param name="logger">
        /// The stream logger instance to add.
        /// </param>
        /// <returns>
        /// The provided composite logger.
        /// </returns>
        public static ICompositeLogger<TContext> AddStreamLogger<TContext>(this ICompositeLogger<TContext> parent, IStreamLogger<TContext> logger)
        {
            if (parent != null)
            {
                parent.AddLogger(logger as ILogger<TContext>);
            }

            return parent;
        }

        #endregion
    }
}
