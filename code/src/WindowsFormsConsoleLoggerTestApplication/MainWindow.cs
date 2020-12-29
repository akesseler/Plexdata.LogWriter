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
using Plexdata.LogWriter.Extensions;
using Plexdata.LogWriter.Facades.Windows;
using Plexdata.LogWriter.Logging.Windows;
using Plexdata.LogWriter.Settings;
using Plexdata.LogWriter.Testing.Helper.Dialogs;
using Plexdata.Utilities.Assembly;
using System;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace Plexdata.LogWriter.Testing.Helper
{
    public partial class MainWindow : Form
    {
        private SystemMenuHelper handler;
        private IConsoleLoggerFacade facade = null;
        private IConsoleLoggerSettings settings = null;
        private IConsoleLogger defaultLogger = null;
        private IConsoleLogger<MainWindow> contextLogger = null;
        private Exception exception = null;
        private (String, Object)[] details = null;

        public MainWindow()
        {
            this.InitializeComponent();

            this.handler = new SystemMenuHelper(this);
            this.handler.AddSeparator();
            this.handler.AddCommand("&About...", this.OnSystemMenuAbout);

            this.facade = new ConsoleLoggerFacade();
            this.settings = new ConsoleLoggerSettings
            {
                LogLevel = LogLevel.Trace,
                LogTime = LogTime.Local,
                LogType = LogType.Raw,
                ShowTime = false,
                FullName = false,
                BufferSize = new Dimension(500, 1000),
                WindowTitle = this.Text,
            };

            this.defaultLogger = new ConsoleLogger(this.settings, this.facade);
            this.contextLogger = new ConsoleLogger<MainWindow>(this.settings, this.facade);

            this.SetupControls();
        }

        #region Action handlers

        private void OnSystemMenuAbout()
        {
            (new AboutDialog()).ShowDialog(this);
        }

        #endregion

        #region Event handlers

        private void OnEditBoxTextChanged(Object sender, EventArgs args)
        {
            if (sender == this.txtPartSplit)
            {
                String helper = this.txtPartSplit.Text.Trim();
                if (helper.Length > 0)
                {
                    this.settings.PartSplit = helper[0];
                }
            }
            else
            {
                Debug.Assert(false);
            }
        }

        private void OnTestLabelMouseClick(Object sender, MouseEventArgs args)
        {
            String messge = null;

            if (!String.IsNullOrWhiteSpace(this.txtMessage.Text))
            {
                messge = $"{this.txtMessage.Text} -> MouseClick: x={args.Location.X}, y={args.Location.Y}";
            }

            if (this.chkContext.Checked)
            {
                this.DoContextLogging(messge, this.chkScope.Checked);
            }
            else
            {
                this.DoDefaultLogging(messge, this.chkScope.Checked);
            }
        }

        private void OnCheckBoxCheckedChanged(Object sender, EventArgs args)
        {
            if (sender == this.chkShowTime)
            {
                this.settings.ShowTime = this.chkShowTime.Checked;
            }
            else if (sender == this.chkFullName)
            {
                this.settings.FullName = this.chkFullName.Checked;
            }
            else
            {
                Debug.Assert(false);
            }
        }

        private void OnComboBoxSelectedIndexChanged(Object sender, EventArgs args)
        {
            if (sender == this.cmbLogLevel)
            {
                this.settings.LogLevel = (LogLevel)this.cmbLogLevel.SelectedItem;
            }
            else if (sender == this.cmbLogType)
            {
                this.settings.LogType = (LogType)this.cmbLogType.SelectedItem;
            }
            else if (sender == this.cmbLogTime)
            {
                this.settings.LogTime = (LogTime)this.cmbLogTime.SelectedItem;
            }
            else
            {
                Debug.Assert(false);
            }
        }

        private void OnButtonDisplayTextClick(Object sender, EventArgs args)
        {
            (new LogLevelDisplayTextDialog()).ShowDialog(this);
        }

        private void OnButtonDisplayColorsClick(Object sender, EventArgs args)
        {
            (new LogLevelDisplayColorsDialog(this.settings.Coloring)).ShowDialog(this);
        }

        #endregion

        #region Other helpers 

        private void SetupControls()
        {
            this.cmbLogLevel.DataSource = this.GetEnumValues(this.settings.LogLevel);
            this.cmbLogLevel.SelectedItem = this.settings.LogLevel;
            this.cmbLogLevel.SelectedIndexChanged += this.OnComboBoxSelectedIndexChanged;

            this.cmbLogType.DataSource = this.GetEnumValues(this.settings.LogType);
            this.cmbLogType.SelectedItem = this.settings.LogType;
            this.cmbLogType.SelectedIndexChanged += this.OnComboBoxSelectedIndexChanged;

            this.cmbLogTime.DataSource = this.GetEnumValues(this.settings.LogTime);
            this.cmbLogTime.SelectedItem = this.settings.LogTime;
            this.cmbLogTime.SelectedIndexChanged += this.OnComboBoxSelectedIndexChanged;

            this.chkShowTime.Checked = this.settings.ShowTime;
            this.chkShowTime.CheckedChanged += this.OnCheckBoxCheckedChanged;

            this.chkFullName.Checked = this.settings.FullName;
            this.chkFullName.CheckedChanged += this.OnCheckBoxCheckedChanged;

            this.txtMessage.Text = "Message to write";
            this.txtPartSplit.Text = this.settings.PartSplit.ToString();
            this.txtPartSplit.TextChanged += this.OnEditBoxTextChanged;

            this.cmbCulture.Sorted = true;
            this.cmbCulture.DataSource = CultureInfo.GetCultures(CultureTypes.AllCultures);
            this.cmbCulture.ValueMember = "DisplayName";
            this.cmbCulture.SelectedItem = this.settings.Culture;
            this.cmbCulture.SelectedValueChanged += this.OnComboBoxCultureSelectedValueChanged;

            this.lblTest.MouseClick += this.OnTestLabelMouseClick;
        }

        private void OnComboBoxCultureSelectedValueChanged(Object sender, EventArgs args)
        {
            this.settings.Culture = this.cmbCulture.SelectedItem as CultureInfo;
        }

        private void DoContextLogging(String messge, Boolean scope)
        {
            if (scope)
            {
                MethodBase method = MethodBase.GetCurrentMethod();

                this.contextLogger.Trace(method, messge, this.GetException(), this.GetDetails());
                this.contextLogger.Debug(method, messge, this.GetException(), this.GetDetails());
                this.contextLogger.Verbose(method, messge, this.GetException(), this.GetDetails());
                this.contextLogger.Message(method, messge, this.GetException(), this.GetDetails());
                this.contextLogger.Warning(method, messge, this.GetException(), this.GetDetails());
                this.contextLogger.Error(method, messge, this.GetException(), this.GetDetails());
                this.contextLogger.Fatal(method, messge, this.GetException(), this.GetDetails());
                this.contextLogger.Critical(method, messge, this.GetException(), this.GetDetails());
            }
            else
            {
                this.contextLogger.Trace(messge, this.GetException(), this.GetDetails());
                this.contextLogger.Debug(messge, this.GetException(), this.GetDetails());
                this.contextLogger.Verbose(messge, this.GetException(), this.GetDetails());
                this.contextLogger.Message(messge, this.GetException(), this.GetDetails());
                this.contextLogger.Warning(messge, this.GetException(), this.GetDetails());
                this.contextLogger.Error(messge, this.GetException(), this.GetDetails());
                this.contextLogger.Fatal(messge, this.GetException(), this.GetDetails());
                this.contextLogger.Critical(messge, this.GetException(), this.GetDetails());
            }
        }

        private void DoDefaultLogging(String messge, Boolean scope)
        {
            if (scope)
            {
                MethodBase method = MethodBase.GetCurrentMethod();

                this.defaultLogger.Trace(method, messge, this.GetException(), this.GetDetails());
                this.defaultLogger.Debug(method, messge, this.GetException(), this.GetDetails());
                this.defaultLogger.Verbose(method, messge, this.GetException(), this.GetDetails());
                this.defaultLogger.Message(method, messge, this.GetException(), this.GetDetails());
                this.defaultLogger.Warning(method, messge, this.GetException(), this.GetDetails());
                this.defaultLogger.Error(method, messge, this.GetException(), this.GetDetails());
                this.defaultLogger.Fatal(method, messge, this.GetException(), this.GetDetails());
                this.defaultLogger.Critical(method, messge, this.GetException(), this.GetDetails());
            }
            else
            {
                this.defaultLogger.Trace(messge, this.GetException(), this.GetDetails());
                this.defaultLogger.Debug(messge, this.GetException(), this.GetDetails());
                this.defaultLogger.Verbose(messge, this.GetException(), this.GetDetails());
                this.defaultLogger.Message(messge, this.GetException(), this.GetDetails());
                this.defaultLogger.Warning(messge, this.GetException(), this.GetDetails());
                this.defaultLogger.Error(messge, this.GetException(), this.GetDetails());
                this.defaultLogger.Fatal(messge, this.GetException(), this.GetDetails());
                this.defaultLogger.Critical(messge, this.GetException(), this.GetDetails());
            }
        }

        private TEnum[] GetEnumValues<TEnum>(TEnum value)
        {
            return ((TEnum[])Enum.GetValues(typeof(TEnum))).Distinct().ToArray();
        }

        private Exception GetException()
        {
            if (!this.chkException.Checked)
            {
                return null;
            }

            if (this.exception == null)
            {
                try
                {
                    this.RaiseException(3);
                }
                catch (Exception ex)
                {
                    this.exception = ex;
                }
            }

            return this.exception;
        }

        private (String, Object)[] GetDetails()
        {
            if (!this.chkValues.Checked)
            {
                return null;
            }

            if (this.details == null)
            {
                this.details = new (String, Object)[] {
                    ( "Boolean",  true ),( "Double",   1234567.89 ),( "Decimal",  1234567.89m ),
                    ( "DateTime", DateTime.Parse("2019-10-29T17:05:42.6789") ),( "Object",   new Object() )
                };
            }

            return this.details;
        }

        private void RaiseException(Int32 depth)
        {
            if (depth > 0)
            {
                this.RaiseException(--depth);
            }

            throw new ArgumentException("Value of depth must be greater than zero.", nameof(depth));
        }

        #endregion
    }
}
