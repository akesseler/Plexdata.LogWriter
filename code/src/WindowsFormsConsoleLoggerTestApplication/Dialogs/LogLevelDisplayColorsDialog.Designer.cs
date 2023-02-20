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

namespace Plexdata.LogWriter.Testing.Helper.Dialogs
{
    partial class LogLevelDisplayColorsDialog
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LogLevelDisplayColorsDialog));
            this.lblTrace = new System.Windows.Forms.Label();
            this.lblDebug = new System.Windows.Forms.Label();
            this.lblVerbose = new System.Windows.Forms.Label();
            this.lblMessage = new System.Windows.Forms.Label();
            this.lblWarning = new System.Windows.Forms.Label();
            this.lblError = new System.Windows.Forms.Label();
            this.lblFatal = new System.Windows.Forms.Label();
            this.lblCritical = new System.Windows.Forms.Label();
            this.lblBackground = new System.Windows.Forms.Label();
            this.lblForeground = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnApply = new System.Windows.Forms.Button();
            this.btnRestore = new System.Windows.Forms.Button();
            this.cmbCriticalBackground = new Plexdata.Controls.ColorComboBox();
            this.cmbFatalBackground = new Plexdata.Controls.ColorComboBox();
            this.cmbErrorBackground = new Plexdata.Controls.ColorComboBox();
            this.cmbWarningBackground = new Plexdata.Controls.ColorComboBox();
            this.cmbMessageBackground = new Plexdata.Controls.ColorComboBox();
            this.cmbVerboseBackground = new Plexdata.Controls.ColorComboBox();
            this.cmbDebugBackground = new Plexdata.Controls.ColorComboBox();
            this.cmbTraceBackground = new Plexdata.Controls.ColorComboBox();
            this.cmbCriticalForeground = new Plexdata.Controls.ColorComboBox();
            this.cmbFatalForeground = new Plexdata.Controls.ColorComboBox();
            this.cmbErrorForeground = new Plexdata.Controls.ColorComboBox();
            this.cmbWarningForeground = new Plexdata.Controls.ColorComboBox();
            this.cmbMessageForeground = new Plexdata.Controls.ColorComboBox();
            this.cmbVerboseForeground = new Plexdata.Controls.ColorComboBox();
            this.cmbDebugForeground = new Plexdata.Controls.ColorComboBox();
            this.cmbTraceForeground = new Plexdata.Controls.ColorComboBox();
            this.lblDisaster = new System.Windows.Forms.Label();
            this.cmbDisasterBackground = new Plexdata.Controls.ColorComboBox();
            this.cmbDisasterForeground = new Plexdata.Controls.ColorComboBox();
            this.SuspendLayout();
            // 
            // lblTrace
            // 
            this.lblTrace.AutoSize = true;
            this.lblTrace.Location = new System.Drawing.Point(33, 39);
            this.lblTrace.Name = "lblTrace";
            this.lblTrace.Size = new System.Drawing.Size(35, 13);
            this.lblTrace.TabIndex = 2;
            this.lblTrace.Text = "Trace";
            // 
            // lblDebug
            // 
            this.lblDebug.AutoSize = true;
            this.lblDebug.Location = new System.Drawing.Point(29, 66);
            this.lblDebug.Name = "lblDebug";
            this.lblDebug.Size = new System.Drawing.Size(39, 13);
            this.lblDebug.TabIndex = 5;
            this.lblDebug.Text = "Debug";
            // 
            // lblVerbose
            // 
            this.lblVerbose.AutoSize = true;
            this.lblVerbose.Location = new System.Drawing.Point(22, 93);
            this.lblVerbose.Name = "lblVerbose";
            this.lblVerbose.Size = new System.Drawing.Size(46, 13);
            this.lblVerbose.TabIndex = 8;
            this.lblVerbose.Text = "Verbose";
            // 
            // lblMessage
            // 
            this.lblMessage.AutoSize = true;
            this.lblMessage.Location = new System.Drawing.Point(18, 120);
            this.lblMessage.Name = "lblMessage";
            this.lblMessage.Size = new System.Drawing.Size(50, 13);
            this.lblMessage.TabIndex = 11;
            this.lblMessage.Text = "Message";
            // 
            // lblWarning
            // 
            this.lblWarning.AutoSize = true;
            this.lblWarning.Location = new System.Drawing.Point(21, 147);
            this.lblWarning.Name = "lblWarning";
            this.lblWarning.Size = new System.Drawing.Size(47, 13);
            this.lblWarning.TabIndex = 14;
            this.lblWarning.Text = "Warning";
            // 
            // lblError
            // 
            this.lblError.AutoSize = true;
            this.lblError.Location = new System.Drawing.Point(39, 174);
            this.lblError.Name = "lblError";
            this.lblError.Size = new System.Drawing.Size(29, 13);
            this.lblError.TabIndex = 17;
            this.lblError.Text = "Error";
            // 
            // lblFatal
            // 
            this.lblFatal.AutoSize = true;
            this.lblFatal.Location = new System.Drawing.Point(38, 201);
            this.lblFatal.Name = "lblFatal";
            this.lblFatal.Size = new System.Drawing.Size(30, 13);
            this.lblFatal.TabIndex = 20;
            this.lblFatal.Text = "Fatal";
            // 
            // lblCritical
            // 
            this.lblCritical.AutoSize = true;
            this.lblCritical.Location = new System.Drawing.Point(30, 228);
            this.lblCritical.Name = "lblCritical";
            this.lblCritical.Size = new System.Drawing.Size(38, 13);
            this.lblCritical.TabIndex = 23;
            this.lblCritical.Text = "Critical";
            // 
            // lblBackground
            // 
            this.lblBackground.AutoSize = true;
            this.lblBackground.Location = new System.Drawing.Point(229, 16);
            this.lblBackground.Name = "lblBackground";
            this.lblBackground.Size = new System.Drawing.Size(65, 13);
            this.lblBackground.TabIndex = 1;
            this.lblBackground.Text = "Background";
            // 
            // lblForeground
            // 
            this.lblForeground.AutoSize = true;
            this.lblForeground.Location = new System.Drawing.Point(104, 16);
            this.lblForeground.Name = "lblForeground";
            this.lblForeground.Size = new System.Drawing.Size(61, 13);
            this.lblForeground.TabIndex = 0;
            this.lblForeground.Text = "Foreground";
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(247, 283);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 31;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnApply
            // 
            this.btnApply.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnApply.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnApply.Location = new System.Drawing.Point(166, 283);
            this.btnApply.Name = "btnApply";
            this.btnApply.Size = new System.Drawing.Size(75, 23);
            this.btnApply.TabIndex = 30;
            this.btnApply.Text = "&Apply";
            this.btnApply.UseVisualStyleBackColor = true;
            // 
            // btnRestore
            // 
            this.btnRestore.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnRestore.Location = new System.Drawing.Point(12, 283);
            this.btnRestore.Name = "btnRestore";
            this.btnRestore.Size = new System.Drawing.Size(75, 23);
            this.btnRestore.TabIndex = 29;
            this.btnRestore.Text = "&Restore";
            this.btnRestore.UseVisualStyleBackColor = true;
            this.btnRestore.Click += new System.EventHandler(this.OnButtonRestoreClick);
            // 
            // cmbCriticalBackground
            // 
            this.cmbCriticalBackground.FormattingEnabled = true;
            this.cmbCriticalBackground.Location = new System.Drawing.Point(201, 225);
            this.cmbCriticalBackground.Name = "cmbCriticalBackground";
            this.cmbCriticalBackground.SelectedColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.cmbCriticalBackground.SelectorText = "";
            this.cmbCriticalBackground.Size = new System.Drawing.Size(121, 21);
            this.cmbCriticalBackground.TabIndex = 25;
            // 
            // cmbFatalBackground
            // 
            this.cmbFatalBackground.FormattingEnabled = true;
            this.cmbFatalBackground.Location = new System.Drawing.Point(201, 198);
            this.cmbFatalBackground.Name = "cmbFatalBackground";
            this.cmbFatalBackground.SelectedColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.cmbFatalBackground.SelectorText = "";
            this.cmbFatalBackground.Size = new System.Drawing.Size(121, 21);
            this.cmbFatalBackground.TabIndex = 22;
            // 
            // cmbErrorBackground
            // 
            this.cmbErrorBackground.FormattingEnabled = true;
            this.cmbErrorBackground.Location = new System.Drawing.Point(201, 171);
            this.cmbErrorBackground.Name = "cmbErrorBackground";
            this.cmbErrorBackground.SelectedColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.cmbErrorBackground.SelectorText = "";
            this.cmbErrorBackground.Size = new System.Drawing.Size(121, 21);
            this.cmbErrorBackground.TabIndex = 19;
            // 
            // cmbWarningBackground
            // 
            this.cmbWarningBackground.FormattingEnabled = true;
            this.cmbWarningBackground.Location = new System.Drawing.Point(201, 144);
            this.cmbWarningBackground.Name = "cmbWarningBackground";
            this.cmbWarningBackground.SelectedColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.cmbWarningBackground.SelectorText = "";
            this.cmbWarningBackground.Size = new System.Drawing.Size(121, 21);
            this.cmbWarningBackground.TabIndex = 16;
            // 
            // cmbMessageBackground
            // 
            this.cmbMessageBackground.FormattingEnabled = true;
            this.cmbMessageBackground.Location = new System.Drawing.Point(201, 117);
            this.cmbMessageBackground.Name = "cmbMessageBackground";
            this.cmbMessageBackground.SelectedColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.cmbMessageBackground.SelectorText = "";
            this.cmbMessageBackground.Size = new System.Drawing.Size(121, 21);
            this.cmbMessageBackground.TabIndex = 13;
            // 
            // cmbVerboseBackground
            // 
            this.cmbVerboseBackground.FormattingEnabled = true;
            this.cmbVerboseBackground.Location = new System.Drawing.Point(201, 90);
            this.cmbVerboseBackground.Name = "cmbVerboseBackground";
            this.cmbVerboseBackground.SelectedColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.cmbVerboseBackground.SelectorText = "";
            this.cmbVerboseBackground.Size = new System.Drawing.Size(121, 21);
            this.cmbVerboseBackground.TabIndex = 10;
            // 
            // cmbDebugBackground
            // 
            this.cmbDebugBackground.FormattingEnabled = true;
            this.cmbDebugBackground.Location = new System.Drawing.Point(201, 63);
            this.cmbDebugBackground.Name = "cmbDebugBackground";
            this.cmbDebugBackground.SelectedColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.cmbDebugBackground.SelectorText = "";
            this.cmbDebugBackground.Size = new System.Drawing.Size(121, 21);
            this.cmbDebugBackground.TabIndex = 7;
            // 
            // cmbTraceBackground
            // 
            this.cmbTraceBackground.FormattingEnabled = true;
            this.cmbTraceBackground.Location = new System.Drawing.Point(201, 36);
            this.cmbTraceBackground.Name = "cmbTraceBackground";
            this.cmbTraceBackground.SelectedColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.cmbTraceBackground.SelectorText = "";
            this.cmbTraceBackground.Size = new System.Drawing.Size(121, 21);
            this.cmbTraceBackground.TabIndex = 4;
            // 
            // cmbCriticalForeground
            // 
            this.cmbCriticalForeground.FormattingEnabled = true;
            this.cmbCriticalForeground.Location = new System.Drawing.Point(74, 225);
            this.cmbCriticalForeground.Name = "cmbCriticalForeground";
            this.cmbCriticalForeground.SelectedColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.cmbCriticalForeground.SelectorText = "";
            this.cmbCriticalForeground.Size = new System.Drawing.Size(121, 21);
            this.cmbCriticalForeground.TabIndex = 24;
            // 
            // cmbFatalForeground
            // 
            this.cmbFatalForeground.FormattingEnabled = true;
            this.cmbFatalForeground.Location = new System.Drawing.Point(74, 198);
            this.cmbFatalForeground.Name = "cmbFatalForeground";
            this.cmbFatalForeground.SelectedColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.cmbFatalForeground.SelectorText = "";
            this.cmbFatalForeground.Size = new System.Drawing.Size(121, 21);
            this.cmbFatalForeground.TabIndex = 21;
            // 
            // cmbErrorForeground
            // 
            this.cmbErrorForeground.FormattingEnabled = true;
            this.cmbErrorForeground.Location = new System.Drawing.Point(74, 171);
            this.cmbErrorForeground.Name = "cmbErrorForeground";
            this.cmbErrorForeground.SelectedColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.cmbErrorForeground.SelectorText = "";
            this.cmbErrorForeground.Size = new System.Drawing.Size(121, 21);
            this.cmbErrorForeground.TabIndex = 18;
            // 
            // cmbWarningForeground
            // 
            this.cmbWarningForeground.FormattingEnabled = true;
            this.cmbWarningForeground.Location = new System.Drawing.Point(74, 144);
            this.cmbWarningForeground.Name = "cmbWarningForeground";
            this.cmbWarningForeground.SelectedColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.cmbWarningForeground.SelectorText = "";
            this.cmbWarningForeground.Size = new System.Drawing.Size(121, 21);
            this.cmbWarningForeground.TabIndex = 15;
            // 
            // cmbMessageForeground
            // 
            this.cmbMessageForeground.FormattingEnabled = true;
            this.cmbMessageForeground.Location = new System.Drawing.Point(74, 117);
            this.cmbMessageForeground.Name = "cmbMessageForeground";
            this.cmbMessageForeground.SelectedColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.cmbMessageForeground.SelectorText = "";
            this.cmbMessageForeground.Size = new System.Drawing.Size(121, 21);
            this.cmbMessageForeground.TabIndex = 12;
            // 
            // cmbVerboseForeground
            // 
            this.cmbVerboseForeground.FormattingEnabled = true;
            this.cmbVerboseForeground.Location = new System.Drawing.Point(74, 90);
            this.cmbVerboseForeground.Name = "cmbVerboseForeground";
            this.cmbVerboseForeground.SelectedColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.cmbVerboseForeground.SelectorText = "";
            this.cmbVerboseForeground.Size = new System.Drawing.Size(121, 21);
            this.cmbVerboseForeground.TabIndex = 9;
            // 
            // cmbDebugForeground
            // 
            this.cmbDebugForeground.FormattingEnabled = true;
            this.cmbDebugForeground.Location = new System.Drawing.Point(74, 63);
            this.cmbDebugForeground.Name = "cmbDebugForeground";
            this.cmbDebugForeground.SelectedColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.cmbDebugForeground.SelectorText = "";
            this.cmbDebugForeground.Size = new System.Drawing.Size(121, 21);
            this.cmbDebugForeground.TabIndex = 6;
            // 
            // cmbTraceForeground
            // 
            this.cmbTraceForeground.FormattingEnabled = true;
            this.cmbTraceForeground.Location = new System.Drawing.Point(74, 36);
            this.cmbTraceForeground.Name = "cmbTraceForeground";
            this.cmbTraceForeground.SelectedColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.cmbTraceForeground.SelectorText = "";
            this.cmbTraceForeground.Size = new System.Drawing.Size(121, 21);
            this.cmbTraceForeground.TabIndex = 3;
            // 
            // lblDisaster
            // 
            this.lblDisaster.AutoSize = true;
            this.lblDisaster.Location = new System.Drawing.Point(23, 255);
            this.lblDisaster.Name = "lblDisaster";
            this.lblDisaster.Size = new System.Drawing.Size(45, 13);
            this.lblDisaster.TabIndex = 26;
            this.lblDisaster.Text = "Disaster";
            // 
            // cmbDisasterBackground
            // 
            this.cmbDisasterBackground.FormattingEnabled = true;
            this.cmbDisasterBackground.Location = new System.Drawing.Point(201, 252);
            this.cmbDisasterBackground.Name = "cmbDisasterBackground";
            this.cmbDisasterBackground.SelectedColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.cmbDisasterBackground.SelectorText = "";
            this.cmbDisasterBackground.Size = new System.Drawing.Size(121, 21);
            this.cmbDisasterBackground.TabIndex = 28;
            // 
            // cmbDisasterForeground
            // 
            this.cmbDisasterForeground.FormattingEnabled = true;
            this.cmbDisasterForeground.Location = new System.Drawing.Point(74, 252);
            this.cmbDisasterForeground.Name = "cmbDisasterForeground";
            this.cmbDisasterForeground.SelectedColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.cmbDisasterForeground.SelectorText = "";
            this.cmbDisasterForeground.Size = new System.Drawing.Size(121, 21);
            this.cmbDisasterForeground.TabIndex = 27;
            // 
            // LogLevelDisplayColorsDialog
            // 
            this.AcceptButton = this.btnApply;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(334, 318);
            this.Controls.Add(this.lblDisaster);
            this.Controls.Add(this.cmbDisasterBackground);
            this.Controls.Add(this.cmbDisasterForeground);
            this.Controls.Add(this.btnRestore);
            this.Controls.Add(this.btnApply);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.lblForeground);
            this.Controls.Add(this.lblBackground);
            this.Controls.Add(this.lblCritical);
            this.Controls.Add(this.lblFatal);
            this.Controls.Add(this.lblError);
            this.Controls.Add(this.lblWarning);
            this.Controls.Add(this.lblMessage);
            this.Controls.Add(this.lblVerbose);
            this.Controls.Add(this.lblDebug);
            this.Controls.Add(this.lblTrace);
            this.Controls.Add(this.cmbCriticalBackground);
            this.Controls.Add(this.cmbFatalBackground);
            this.Controls.Add(this.cmbErrorBackground);
            this.Controls.Add(this.cmbWarningBackground);
            this.Controls.Add(this.cmbMessageBackground);
            this.Controls.Add(this.cmbVerboseBackground);
            this.Controls.Add(this.cmbDebugBackground);
            this.Controls.Add(this.cmbTraceBackground);
            this.Controls.Add(this.cmbCriticalForeground);
            this.Controls.Add(this.cmbFatalForeground);
            this.Controls.Add(this.cmbErrorForeground);
            this.Controls.Add(this.cmbWarningForeground);
            this.Controls.Add(this.cmbMessageForeground);
            this.Controls.Add(this.cmbVerboseForeground);
            this.Controls.Add(this.cmbDebugForeground);
            this.Controls.Add(this.cmbTraceForeground);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "LogLevelDisplayColorsDialog";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Log-Level Display Colors";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Plexdata.Controls.ColorComboBox cmbTraceForeground;
        private Plexdata.Controls.ColorComboBox cmbTraceBackground;
        private Plexdata.Controls.ColorComboBox cmbDebugForeground;
        private Plexdata.Controls.ColorComboBox cmbDebugBackground;
        private Plexdata.Controls.ColorComboBox cmbVerboseForeground;
        private Plexdata.Controls.ColorComboBox cmbVerboseBackground;
        private Plexdata.Controls.ColorComboBox cmbMessageForeground;
        private Plexdata.Controls.ColorComboBox cmbMessageBackground;
        private Plexdata.Controls.ColorComboBox cmbWarningForeground;
        private Plexdata.Controls.ColorComboBox cmbWarningBackground;
        private Plexdata.Controls.ColorComboBox cmbErrorForeground;
        private Plexdata.Controls.ColorComboBox cmbErrorBackground;
        private Plexdata.Controls.ColorComboBox cmbFatalForeground;
        private Plexdata.Controls.ColorComboBox cmbFatalBackground;
        private Plexdata.Controls.ColorComboBox cmbCriticalForeground;
        private Plexdata.Controls.ColorComboBox cmbCriticalBackground;
        private Plexdata.Controls.ColorComboBox cmbDisasterBackground;
        private Plexdata.Controls.ColorComboBox cmbDisasterForeground;
        private System.Windows.Forms.Label lblTrace;
        private System.Windows.Forms.Label lblDebug;
        private System.Windows.Forms.Label lblVerbose;
        private System.Windows.Forms.Label lblMessage;
        private System.Windows.Forms.Label lblWarning;
        private System.Windows.Forms.Label lblError;
        private System.Windows.Forms.Label lblFatal;
        private System.Windows.Forms.Label lblCritical;
        private System.Windows.Forms.Label lblBackground;
        private System.Windows.Forms.Label lblForeground;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnApply;
        private System.Windows.Forms.Button btnRestore;
        private System.Windows.Forms.Label lblDisaster;
    }
}