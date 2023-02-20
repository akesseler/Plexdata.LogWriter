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
using System.Linq;
using System.Text;

namespace Plexdata.LogWriter.Settings
{
    /// <summary>
    /// The class to provide settings for all of the mail logging writers.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This class allows a configuration of classes derived from mail logger 
    /// interfaces. But keep in mind, the availability of particular setting items 
    /// depends of which of the available mail loggers is actually used.
    /// </para>
    /// <para>
    /// Attention, logging type RAW is used as the default logging type.
    /// </para>
    /// </remarks>
    public class MailLoggerSettings : LoggerSettings, IMailLoggerSettings
    {
        #region Private constants

        /// <summary>
        /// The default address value.
        /// </summary>
        /// <remarks>
        /// The default address value is set to <em>empty</em>.
        /// </remarks>
        private static readonly String DefaultAddress = String.Empty;

        /// <summary>
        /// The default username value.
        /// </summary>
        /// <remarks>
        /// The default username value is set to <em>empty</em>.
        /// </remarks>
        private static readonly String DefaultUsername = String.Empty;

        /// <summary>
        /// The default password value.
        /// </summary>
        /// <remarks>
        /// The default password value is set to <em>empty</em>.
        /// </remarks>
        private static readonly String DefaultPassword = String.Empty;

        /// <summary>
        /// The default SMTP host value.
        /// </summary>
        /// <remarks>
        /// The default SMTP host value is set to <em>empty</em>.
        /// </remarks>
        private static readonly String DefaultSmtpHost = String.Empty;

        /// <summary>
        /// The default SMTP port value.
        /// </summary>
        /// <remarks>
        /// The default SMTP port value is set to <c>587</c>.
        /// </remarks>
        private static readonly Int32 DefaultSmtpPort = 587;

        /// <summary>
        /// The default value of SSL usage.
        /// </summary>
        /// <remarks>
        /// The default value of SSL usage is set to <c>enabled</c>.
        /// </remarks>
        private static readonly Boolean DefaultUseSsl = true;

        /// <summary>
        /// The default encoding value.
        /// </summary>
        /// <remarks>
        /// The default encoding value is set to <c>UTF-8</c>.
        /// </remarks>
        private static readonly Encoding DefaultEncoding = Encoding.UTF8;

        /// <summary>
        /// The default subject value.
        /// </summary>
        /// <remarks>
        /// The default subject value is set to <em>empty</em>.
        /// </remarks>
        private static readonly String DefaultSubject = String.Empty;

        /// <summary>
        /// The default receivers value.
        /// </summary>
        /// <remarks>
        /// The default receivers value is set to <em>empty</em>.
        /// </remarks>
        private static readonly IEnumerable<String> DefaultReceivers = Enumerable.Empty<String>();

        /// <summary>
        /// The default clear copies value.
        /// </summary>
        /// <remarks>
        /// The default clear copies value is set to <em>empty</em>.
        /// </remarks>
        private static readonly IEnumerable<String> DefaultClearCopies = Enumerable.Empty<String>();

        /// <summary>
        /// The default blind copies value.
        /// </summary>
        /// <remarks>
        /// The default blind copies value is set to <em>empty</em>.
        /// </remarks>
        private static readonly IEnumerable<String> DefaultBlindCopies = Enumerable.Empty<String>();

        #endregion

        #region Private fields

        /// <summary>
        /// This field holds the chosen address.
        /// </summary>
        /// <remarks>
        /// The value of <see cref="MailLoggerSettings.DefaultAddress"/> 
        /// is used as initial value.
        /// </remarks>
        private String address;

        /// <summary>
        /// This field holds the chosen username.
        /// </summary>
        /// <remarks>
        /// The value of <see cref="MailLoggerSettings.DefaultUsername"/> 
        /// is used as initial value.
        /// </remarks>
        private String username;

        /// <summary>
        /// This field holds the chosen password.
        /// </summary>
        /// <remarks>
        /// The value of <see cref="MailLoggerSettings.DefaultPassword"/> 
        /// is used as initial value.
        /// </remarks>
        private String password;

        /// <summary>
        /// This field holds the chosen SMTP host.
        /// </summary>
        /// <remarks>
        /// The value of <see cref="MailLoggerSettings.DefaultSmtpHost"/> 
        /// is used as initial value.
        /// </remarks>
        private String smtpHost;

        /// <summary>
        /// This field holds the chosen SMTP port.
        /// </summary>
        /// <remarks>
        /// The value of <see cref="MailLoggerSettings.DefaultSmtpPort "/> 
        /// is used as initial value.
        /// </remarks>
        private Int32 smtpPort;

        /// <summary>
        /// This field holds the chosen SSL enabled state.
        /// </summary>
        /// <remarks>
        /// The value of <see cref="MailLoggerSettings.DefaultUseSsl"/> 
        /// is used as initial value.
        /// </remarks>
        private Boolean useSsl;

        /// <summary>
        /// This field holds the chosen encoding.
        /// </summary>
        /// <remarks>
        /// The value of <see cref="MailLoggerSettings.DefaultEncoding"/> 
        /// is used as initial value.
        /// </remarks>
        private Encoding encoding;

        /// <summary>
        /// This field holds the chosen subject.
        /// </summary>
        /// <remarks>
        /// The value of <see cref="MailLoggerSettings.DefaultSubject"/> 
        /// is used as initial value.
        /// </remarks>
        private String subject;

        /// <summary>
        /// This field holds the chosen receivers list.
        /// </summary>
        /// <remarks>
        /// The value of <see cref="MailLoggerSettings.DefaultReceivers"/> 
        /// is used as initial value.
        /// </remarks>
        private IEnumerable<String> receivers;

        /// <summary>
        /// This field holds the chosen clear copies list.
        /// </summary>
        /// <remarks>
        /// The value of <see cref="MailLoggerSettings.DefaultClearCopies"/> 
        /// is used as initial value.
        /// </remarks>
        private IEnumerable<String> clearCopies;

        /// <summary>
        /// This field holds the chosen blind copies list.
        /// </summary>
        /// <remarks>
        /// The value of <see cref="MailLoggerSettings.DefaultBlindCopies"/> 
        /// is used as initial value.
        /// </remarks>
        private IEnumerable<String> blindCopies;

        #endregion

        #region Construction

        /// <summary>
        /// The static class constructor.
        /// </summary>
        /// <remarks>
        /// This constructor does actually nothing.
        /// </remarks>
        static MailLoggerSettings() { }

        /// <summary>
        /// The standard class constructor.
        /// </summary>
        /// <remarks>
        /// This constructor just initializes all properties with its default 
        /// values.
        /// </remarks>
        public MailLoggerSettings()
            : base()
        {
            base.LogType = LogType.Raw;

            this.Address = MailLoggerSettings.DefaultAddress;
            this.Username = MailLoggerSettings.DefaultUsername;
            this.Password = MailLoggerSettings.DefaultPassword;
            this.SmtpHost = MailLoggerSettings.DefaultSmtpHost;
            this.SmtpPort = MailLoggerSettings.DefaultSmtpPort;
            this.UseSsl = MailLoggerSettings.DefaultUseSsl;
            this.Encoding = MailLoggerSettings.DefaultEncoding;
            this.Subject = MailLoggerSettings.DefaultSubject;
            this.Receivers = MailLoggerSettings.DefaultReceivers;
            this.ClearCopies = MailLoggerSettings.DefaultClearCopies;
            this.BlindCopies = MailLoggerSettings.DefaultBlindCopies;
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
        public MailLoggerSettings(ILoggerSettingsSection configuration)
            : this()
        {
            this.LoadSettings(configuration);
        }

        #endregion

        #region Public properties

        /// <inheritdoc />
        public String Address
        {
            get
            {
                return this.address;
            }
            set
            {
                if (this.address != value)
                {
                    this.address = value;
                    base.RaisePropertyChanged(nameof(this.Address));
                }
            }
        }

        /// <inheritdoc />
        public String Username
        {
            get
            {
                return this.username;
            }
            set
            {
                if (this.username != value)
                {
                    this.username = value;
                    base.RaisePropertyChanged(nameof(this.Username));
                }
            }
        }

        /// <inheritdoc />
        public String Password
        {
            get
            {
                return this.password;
            }
            set
            {
                if (this.password != value)
                {
                    this.password = value;
                    base.RaisePropertyChanged(nameof(this.Password));
                }
            }
        }

        /// <inheritdoc />
        public String SmtpHost
        {
            get
            {
                return this.smtpHost;
            }
            set
            {
                if (this.smtpHost != value)
                {
                    this.smtpHost = value;
                    base.RaisePropertyChanged(nameof(this.SmtpHost));
                }
            }
        }

        /// <inheritdoc />
        public Int32 SmtpPort
        {
            get
            {
                return this.smtpPort;
            }
            set
            {
                if (this.smtpPort != value)
                {
                    this.smtpPort = value;
                    base.RaisePropertyChanged(nameof(this.SmtpPort));
                }
            }
        }

        /// <inheritdoc />
        public Boolean UseSsl
        {
            get
            {
                return this.useSsl;
            }
            set
            {
                if (this.useSsl != value)
                {
                    this.useSsl = value;
                    base.RaisePropertyChanged(nameof(this.UseSsl));
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

        /// <inheritdoc />
        public String Subject
        {
            get
            {
                return this.subject;
            }
            set
            {
                if (this.subject != value)
                {
                    this.subject = value;
                    base.RaisePropertyChanged(nameof(this.Subject));
                }
            }
        }

        /// <inheritdoc />
        public IEnumerable<String> Receivers
        {
            get
            {
                return this.receivers;
            }
            set
            {
                if (value == null)
                {
                    value = MailLoggerSettings.DefaultReceivers;
                }

                value = value.Where(x => !String.IsNullOrWhiteSpace(x));

                if (this.receivers == null || !Enumerable.SequenceEqual(this.receivers, value))
                {
                    this.receivers = value;
                    base.RaisePropertyChanged(nameof(this.Receivers));
                }
            }
        }

        /// <inheritdoc />
        public IEnumerable<String> ClearCopies
        {
            get
            {
                return this.clearCopies;
            }
            set
            {
                if (value == null)
                {
                    value = MailLoggerSettings.DefaultClearCopies;
                }

                value = value.Where(x => !String.IsNullOrWhiteSpace(x));

                if (this.clearCopies == null || !Enumerable.SequenceEqual(this.clearCopies, value))
                {
                    this.clearCopies = value;
                    base.RaisePropertyChanged(nameof(this.ClearCopies));
                }
            }
        }

        /// <inheritdoc />
        public IEnumerable<String> BlindCopies
        {
            get
            {
                return this.blindCopies;
            }
            set
            {
                if (value == null)
                {
                    value = MailLoggerSettings.DefaultBlindCopies;
                }

                value = value.Where(x => !String.IsNullOrWhiteSpace(x));

                if (this.blindCopies == null || !Enumerable.SequenceEqual(this.blindCopies, value))
                {
                    this.blindCopies = value;
                    base.RaisePropertyChanged(nameof(this.BlindCopies));
                }
            }
        }

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
        protected override void LoadSettings(ILoggerSettingsSection configuration)
        {
            if (configuration == null) { return; }

            base.LoadSettings(configuration);

            ILoggerSettingsSection section = configuration.GetSection(LoggerSettings.SettingsPath);

            base.LogType = base.GetValue(section[nameof(this.LogType)], LogType.Raw);

            this.Address = base.GetValue(section[nameof(this.Address)], MailLoggerSettings.DefaultAddress);
            this.Username = base.GetValue(section[nameof(this.Username)], MailLoggerSettings.DefaultUsername);
            this.Password = base.GetValue(section[nameof(this.Password)], MailLoggerSettings.DefaultPassword);
            this.SmtpHost = base.GetValue(section[nameof(this.SmtpHost)], MailLoggerSettings.DefaultSmtpHost);
            this.SmtpPort = base.GetValue(section[nameof(this.SmtpPort)], MailLoggerSettings.DefaultSmtpPort);
            this.UseSsl = base.GetValue(section[nameof(this.UseSsl)], MailLoggerSettings.DefaultUseSsl);
            this.Encoding = base.GetValue(section[nameof(this.Encoding)], MailLoggerSettings.DefaultEncoding);
            this.Subject = base.GetValue(section[nameof(this.Subject)], MailLoggerSettings.DefaultSubject);
            this.Receivers = base.GetSectionValues(section, nameof(this.Receivers), MailLoggerSettings.DefaultReceivers);
            this.ClearCopies = base.GetSectionValues(section, nameof(this.ClearCopies), MailLoggerSettings.DefaultClearCopies);
            this.BlindCopies = base.GetSectionValues(section, nameof(this.BlindCopies), MailLoggerSettings.DefaultBlindCopies);
        }

        #endregion
    }
}

