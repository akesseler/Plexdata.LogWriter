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
using System;
using System.Collections.Generic;

namespace Plexdata.LogWriter.Settings
{
    /// <summary>
    /// The class to provide settings for all of the console logging writers.
    /// </summary>
    /// <remarks>
    /// This class allows a configuration of classes derived from console logger 
    /// interfaces. But keep in mind, the availability of particular setting items 
    /// depends of which of the available console loggers is actually used.
    /// </remarks>
    public class ConsoleLoggerSettings : LoggerSettings, IConsoleLoggerSettings
    {
        #region Private fields

        /// <summary>
        /// This field holds the value of color usage.
        /// </summary>
        /// <remarks>
        /// The value of <see cref="ConsoleLoggerSettings.DefaultUseColors"/> 
        /// is used as initial value.
        /// </remarks>
        private Boolean useColors;

        /// <summary>
        /// This field holds the value of used window title.
        /// </summary>
        /// <remarks>
        /// The value of <see cref="ConsoleLoggerSettings.DefaultWindowTitle"/> 
        /// is used as initial value.
        /// </remarks>
        private String windowTitle;

        /// <summary>
        /// This field holds the value of quick edit usage.
        /// </summary>
        /// <remarks>
        /// The value of <see cref="ConsoleLoggerSettings.DefaultQuickEdit"/> 
        /// is used as initial value.
        /// </remarks>
        private Boolean quickEdit;

        /// <summary>
        /// This field holds the value of used buffer size.
        /// </summary>
        /// <remarks>
        /// The value of <see cref="ConsoleLoggerSettings.DefaultBufferSize"/> 
        /// is used as initial value.
        /// </remarks>
        private Dimension bufferSize;

        #endregion

        #region Private constants

        /// <summary>
        /// The default color usage value.
        /// </summary>
        /// <remarks>
        /// The default color usage value is set to <c>true</c>, which means 
        /// coloring is initially enabled.
        /// </remarks>
        private static readonly Boolean DefaultUseColors = true;

        /// <summary>
        /// The default window title.
        /// </summary>
        /// <remarks>
        /// The default window title is <em>empty</em>, which means the original 
        /// window title remains unchanged.
        /// </remarks>
        private static readonly String DefaultWindowTitle = String.Empty;

        /// <summary>
        /// The default quick edit value.
        /// </summary>
        /// <remarks>
        /// The default quick edit value is set to <c>false</c>, which means the 
        /// quick edit mode is initially disabled.
        /// </remarks>
        private static readonly Boolean DefaultQuickEdit = false;

        /// <summary>
        /// The default buffer size value.
        /// </summary>
        /// <remarks>
        /// The default buffer size is set to <c>zero</c>, which means the original 
        /// buffer size remains unchanged.
        /// </remarks>
        private static readonly Dimension DefaultBufferSize = new Dimension();

        #endregion

        #region Construction

        /// <summary>
        /// The static class constructor.
        /// </summary>
        /// <remarks>
        /// This constructor does actually nothing.
        /// </remarks>
        static ConsoleLoggerSettings() { }

        /// <summary>
        /// The default class constructor.
        /// </summary>
        /// <remarks>
        /// This constructor just initializes all properties with 
        /// its default values.
        /// </remarks>
        public ConsoleLoggerSettings()
            : base()
        {
            this.UseColors = ConsoleLoggerSettings.DefaultUseColors;
            this.WindowTitle = ConsoleLoggerSettings.DefaultWindowTitle;
            this.QuickEdit = ConsoleLoggerSettings.DefaultQuickEdit;
            this.BufferSize = ConsoleLoggerSettings.DefaultBufferSize;
            this.Coloring = new Dictionary<LogLevel, Coloring> {
                { LogLevel.Trace, new Coloring(ConsoleColor.Gray, ConsoleColor.Black) },
                { LogLevel.Debug, new Coloring(ConsoleColor.Gray, ConsoleColor.Black) },
                { LogLevel.Verbose, new Coloring(ConsoleColor.White, ConsoleColor.Black) },
                { LogLevel.Message, new Coloring(ConsoleColor.White, ConsoleColor.Black) },
                { LogLevel.Warning, new Coloring(ConsoleColor.Yellow, ConsoleColor.Black) },
                { LogLevel.Error, new Coloring(ConsoleColor.Red, ConsoleColor.Black) },
                { LogLevel.Fatal, new Coloring(ConsoleColor.Gray, ConsoleColor.DarkRed) },
                { LogLevel.Critical, new Coloring(ConsoleColor.Black, ConsoleColor.Red) },
                { LogLevel.Disaster, new Coloring(ConsoleColor.Black, ConsoleColor.Red) },
            };
        }

        /// <summary>
        /// The extended constructor that initializes all properties from provided 
        /// <paramref name="configuration"/> instance.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Usually, it is no necessary to call this constructor manually. This is 
        /// because of, this constructor is actually reserved for the mechanism of 
        /// dependency injection.
        /// </para>
        /// <para>
        /// Please note, the default values are taken if one or more properties are 
        /// not included in the configuration.
        /// </para>
        /// </remarks>
        /// <param name="configuration">
        /// The configuration to read all property values from.
        /// </param>
        /// <seealso cref="LoggerSettings.LoadSettings(ILoggerSettingsSection)"/>
        /// <seealso cref="ConsoleLoggerSettings.LoadSettings(ILoggerSettingsSection)"/>
        public ConsoleLoggerSettings(ILoggerSettingsSection configuration)
            : this()
        {
            this.LoadSettings(configuration);
        }

        #endregion

        #region Public properties

        /// <inheritdoc />
        public Boolean UseColors
        {
            get
            {
                return this.useColors;
            }
            set
            {
                if (this.useColors != value)
                {
                    this.useColors = value;
                    base.RaisePropertyChanged(nameof(this.UseColors));
                }
            }
        }

        /// <inheritdoc />
        public String WindowTitle
        {
            get
            {
                return this.windowTitle;
            }
            set
            {
                if (this.windowTitle != value)
                {
                    this.windowTitle = value;
                    base.RaisePropertyChanged(nameof(this.WindowTitle));
                }
            }
        }

        /// <inheritdoc />
        public Boolean QuickEdit
        {
            get
            {
                return this.quickEdit;
            }
            set
            {
                if (this.quickEdit != value)
                {
                    this.quickEdit = value;
                    base.RaisePropertyChanged(nameof(this.QuickEdit));
                }
            }
        }

        /// <inheritdoc />
        public Dimension BufferSize
        {
            get
            {
                return this.bufferSize;
            }
            set
            {
                if (this.bufferSize != value)
                {
                    this.bufferSize = value;
                    base.RaisePropertyChanged(nameof(this.BufferSize));
                }
            }
        }

        /// <inheritdoc />
        /// <remarks>
        /// <para>
        /// The coloring list is actually a dictionary with the logging level as 
        /// key. Each logging level is assigned to a particular color setup for 
        /// the foreground and the background.
        /// </para>
        /// <para>
        /// Be aware, this property does not raise any property change event in 
        /// case of something has changed!
        /// </para>
        /// </remarks>
        public IDictionary<LogLevel, Coloring> Coloring { get; private set; }

        #endregion

        #region Protected methods

        /// <summary>
        /// The method loads all settings from provided <paramref name="configuration"/> 
        /// and initializes all properties accordingly.
        /// </summary>
        /// <remarks>
        /// This method has been overwritten and calls the inherited base class method 
        /// to ensure all settings are loaded accordingly.
        /// </remarks>
        /// <param name="configuration">
        /// An instance of <see cref="Plexdata.LogWriter.Abstraction.ILoggerSettingsSection"/> 
        /// that represents the settings to be applied.
        /// </param>
        /// <seealso cref="LoggerSettings.LoadSettings(ILoggerSettingsSection)"/>
        /// <seealso cref="ConsoleLoggerSettings.GetBufferSize(ILoggerSettingsSection)"/>
        /// <seealso cref="ConsoleLoggerSettings.GetColoring(ILoggerSettingsSection, IDictionary{LogLevel, Coloring})"/>
        protected override void LoadSettings(ILoggerSettingsSection configuration)
        {
            if (configuration == null) { return; }

            base.LoadSettings(configuration);

            ILoggerSettingsSection section = configuration.GetSection(LoggerSettings.SettingsPath);

            this.UseColors = base.GetValue(section[nameof(this.UseColors)], ConsoleLoggerSettings.DefaultUseColors);
            this.WindowTitle = base.GetValue(section[nameof(this.WindowTitle)], ConsoleLoggerSettings.DefaultWindowTitle);
            this.QuickEdit = base.GetValue(section[nameof(this.QuickEdit)], ConsoleLoggerSettings.DefaultQuickEdit);
            this.BufferSize = this.GetBufferSize(section.GetSection(nameof(this.BufferSize)));
            this.Coloring = this.GetColoring(section.GetSection(nameof(this.Coloring)), this.Coloring);
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Reads the width and the lines from provided configuration.
        /// </summary>
        /// <remarks>
        /// This method tries to read the width and the lines of a <see cref="Dimension"/> 
        /// instance from provided configuration.
        /// </remarks>
        /// <param name="section">
        /// The configuration section to read data from.
        /// </param>
        /// <returns>
        /// A new instance of class <see cref="Dimension"/> with the width in characters 
        /// and the number of lines.
        /// </returns>
        private Dimension GetBufferSize(ILoggerSettingsSection section)
        {
            Dimension result = new Dimension(
                base.GetValue(section[nameof(Dimension.Width)], ConsoleLoggerSettings.DefaultBufferSize.Width),
                base.GetValue(section[nameof(Dimension.Lines)], ConsoleLoggerSettings.DefaultBufferSize.Lines)
            );

            if (result.IsValid)
            {
                return result;
            }

            return ConsoleLoggerSettings.DefaultBufferSize;
        }

        /// <summary>
        /// Reads the list of coloring assignments from provided configuration.
        /// </summary>
        /// <remarks>
        /// This method tries to read the list of coloring assignments from provided 
        /// configuration.
        /// </remarks>
        /// <param name="parent">
        /// The parent <see cref="Coloring"/> section to obtain the list items from.
        /// </param>
        /// <param name="source">
        /// The Coloring source that serves as initial items as well.
        /// </param>
        /// <returns>
        /// The fully initialized Coloring assignment list to be used. It contains the 
        /// default assignments for all coloring settings that were not specified within 
        /// provided configuration section.
        /// </returns>
        private IDictionary<LogLevel, Coloring> GetColoring(ILoggerSettingsSection parent, IDictionary<LogLevel, Coloring> source)
        {
            try
            {
                Dictionary<LogLevel, Coloring> result = new Dictionary<LogLevel, Coloring>();

                foreach (LogLevel level in source.Keys)
                {
                    Coloring value = source[level];

                    // NOTE: The interface implementation just returns the first item of any duplicate key-value combination.
                    ILoggerSettingsSection section = parent.GetSection(level.ToString());

                    result.Add(level, new Coloring(
                        base.GetValue(section[nameof(value.Foreground)], value.Foreground),
                        base.GetValue(section[nameof(value.Background)], value.Background)
                    ));
                }

                return result;
            }
            catch
            {
                return source;
            }
        }

        #endregion
    }
}
