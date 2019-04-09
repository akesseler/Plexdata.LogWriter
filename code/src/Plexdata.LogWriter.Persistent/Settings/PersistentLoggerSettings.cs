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
using Plexdata.LogWriter.Internals.Extensions;
using System;
using System.IO;
using System.Text;

namespace Plexdata.LogWriter.Settings
{
    /// <summary>
    /// The class to provide settings for all of the persistent logging writers.
    /// </summary>
    /// <remarks>
    /// This class allows a configuration of classes derived from persistent logger 
    /// interfaces. But keep in mind, the availability of particular setting items 
    /// depends of which of the available persistent loggers is actually used.
    /// </remarks>
    public class PersistentLoggerSettings : LoggerSettings, IPersistentLoggerSettings
    {
        #region Private fields

        /// <summary>
        /// The filename to be used.
        /// </summary>
        /// <remarks>
        /// This field holds the fully qualified filename 
        /// assigned to this settings instance.
        /// </remarks>
        private String filename;

        /// <summary>
        /// The chosen rolling mode.
        /// </summary>
        /// <remarks>
        /// The default value is <c>false</c>.
        /// </remarks>
        private Boolean rolling;

        /// <summary>
        /// The chosen queuing mode.
        /// </summary>
        /// <remarks>
        /// The default value is <c>false</c>.
        /// </remarks>
        private Boolean queuing;

        /// <summary>
        /// The chosen threshold.
        /// </summary>
        /// <remarks>
        /// The default value is <c>-1</c>.
        /// </remarks>
        private Int32 threshold;

        /// <summary>
        /// The chosen encoding.
        /// </summary>
        /// <remarks>
        /// The default value is <c>UTF-8</c>.
        /// </remarks>
        private Encoding encoding;

        #endregion

        #region Construction

        /// <summary>
        /// The standard class constructor.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This constructor calls the extended constructor and uses 
        /// the default logging filename.
        /// </para>
        /// <para>
        /// The name of default logging file is set to <c>plexdata.log</c> 
        /// and the path points to the current user's temporary folder.
        /// </para>
        /// </remarks>
        public PersistentLoggerSettings()
            : this(Path.Combine(Path.GetTempPath(), "plexdata.log"))
        {
        }

        /// <summary>
        /// The extended class constructor.
        /// </summary>
        /// <remarks>
        /// This constructor just initializes all properties with 
        /// its default values.
        /// </remarks>
        /// <param name="filename">
        /// The fully qualified filename assigned to this settings instance.
        /// </param>
        /// <exception cref="Exception">
        /// This constructor may throw several exceptions. For more information about these exceptions 
        /// please see <see cref="SettingsValidationExtension.EnsureFullPathAndWriteAccessOrThrow(String)"/>.
        /// </exception>
        /// <seealso cref="SettingsValidationExtension.EnsureFullPathAndWriteAccessOrThrow(String)"/>
        public PersistentLoggerSettings(String filename)
            : base()
        {
            this.Filename = filename;
            this.rolling = false;
            this.queuing = false;
            this.threshold = -1;
            this.encoding = Encoding.UTF8;
        }

        #endregion

        #region Public properties

        /// <inheritdoc />
        /// <exception cref="Exception">
        /// This property may throw several exceptions. For more information about these exceptions 
        /// please see <see cref="SettingsValidationExtension.EnsureFullPathAndWriteAccessOrThrow(String)"/>.
        /// </exception>
        /// <seealso cref="SettingsValidationExtension.EnsureFullPathAndWriteAccessOrThrow(String)"/>
        public String Filename
        {
            get
            {
                return this.filename;
            }
            private set // TODO: Make property setter publicly accessible.
            {
                if (this.filename != value)
                {
                    this.filename = value.EnsureFullPathAndWriteAccessOrThrow();
                    base.RaisePropertyChanged(nameof(this.Filename));
                }
            }
        }

        /// <inheritdoc />
        public Boolean IsRolling
        {
            get
            {
                return this.rolling;
            }
            set
            {
                if (this.rolling != value)
                {
                    this.rolling = value;
                    base.RaisePropertyChanged(nameof(this.IsRolling));
                }
            }
        }

        /// <inheritdoc />
        public Boolean IsQueuing
        {
            get
            {
                return this.queuing;
            }
            set
            {
                if (this.queuing != value)
                {
                    this.queuing = value;
                    base.RaisePropertyChanged(nameof(this.IsQueuing));
                }
            }
        }

        /// <inheritdoc />
        public Int32 Threshold
        {
            get
            {
                return this.threshold;
            }
            set
            {
                if (this.threshold != value)
                {
                    this.threshold = value;
                    base.RaisePropertyChanged(nameof(this.Threshold));
                }
            }
        }

        /// <inheritdoc />
        public Encoding Encoding
        {
            get
            {
                return this.encoding;
            }
            set
            {
                if (this.encoding != value)
                {
                    this.encoding = value;
                    base.RaisePropertyChanged(nameof(this.Encoding));
                }
            }
        }

        #endregion
    }
}
