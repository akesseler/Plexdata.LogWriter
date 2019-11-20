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
using Plexdata.LogWriter.Extensions;
using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace Plexdata.LogWriter.Testing.Helper.Dialogs
{
    public partial class LogLevelDisplayTextDialog : Form
    {
        public LogLevelDisplayTextDialog()
        {
            this.InitializeComponent();
        }

        protected override void OnLoad(EventArgs args)
        {
            base.OnLoad(args);
            this.Setup();
        }

        protected override void OnClosing(CancelEventArgs args)
        {
            if (this.DialogResult == DialogResult.OK)
            {
                try
                {
                    LogLevel.Trace.RegisterDisplayText(this.txtTrace.Text);
                    LogLevel.Debug.RegisterDisplayText(this.txtDebug.Text);
                    LogLevel.Verbose.RegisterDisplayText(this.txtVerbose.Text);
                    LogLevel.Message.RegisterDisplayText(this.txtMessage.Text);
                    LogLevel.Warning.RegisterDisplayText(this.txtWarning.Text);
                    LogLevel.Error.RegisterDisplayText(this.txtError.Text);
                    LogLevel.Fatal.RegisterDisplayText(this.txtFatal.Text);
                    LogLevel.Critical.RegisterDisplayText(this.txtCritical.Text);
                }
                catch (Exception exception)
                {
                    MessageBox.Show(this, exception.Message, "Save Values", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    args.Cancel = true;
                    return;
                }
            }

            base.OnClosing(args);
        }

        private void OnButtonRestoreClick(Object sender, EventArgs args)
        {
            Cursor cursor = this.Cursor;

            try
            {
                this.Cursor = Cursors.WaitCursor;
                LogLevel.Trace.RestoreDisplayText();
                LogLevel.Debug.RestoreDisplayText();
                LogLevel.Verbose.RestoreDisplayText();
                LogLevel.Message.RestoreDisplayText();
                LogLevel.Warning.RestoreDisplayText();
                LogLevel.Error.RestoreDisplayText();
                LogLevel.Fatal.RestoreDisplayText();
                LogLevel.Critical.RestoreDisplayText();

                this.Setup();
            }
            catch (Exception exception)
            {
                MessageBox.Show(this, exception.Message, "Restore Defaults", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                this.Cursor = cursor;
            }
        }

        private void Setup()
        {
            try
            {
                this.txtTrace.Text = LogLevel.Trace.ToDisplayText();
                this.txtDebug.Text = LogLevel.Debug.ToDisplayText();
                this.txtVerbose.Text = LogLevel.Verbose.ToDisplayText();
                this.txtMessage.Text = LogLevel.Message.ToDisplayText();
                this.txtWarning.Text = LogLevel.Warning.ToDisplayText();
                this.txtError.Text = LogLevel.Error.ToDisplayText();
                this.txtFatal.Text = LogLevel.Fatal.ToDisplayText();
                this.txtCritical.Text = LogLevel.Critical.ToDisplayText();
            }
            catch (Exception exception)
            {
                MessageBox.Show(this, exception.Message, "Setup Values", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
