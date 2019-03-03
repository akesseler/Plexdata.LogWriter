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

namespace Plexdata.LogWriter.Testing.Helper
{
    partial class MainWindow
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainWindow));
            this.chkShowTime = new System.Windows.Forms.CheckBox();
            this.cmbLogTime = new System.Windows.Forms.ComboBox();
            this.lblLogTime = new System.Windows.Forms.Label();
            this.cmbLogLevel = new System.Windows.Forms.ComboBox();
            this.lblLogLevel = new System.Windows.Forms.Label();
            this.cmbLogType = new System.Windows.Forms.ComboBox();
            this.lblLogType = new System.Windows.Forms.Label();
            this.lblTest = new System.Windows.Forms.Label();
            this.panSettings = new System.Windows.Forms.Panel();
            this.cmbCulture = new System.Windows.Forms.ComboBox();
            this.chkValues = new System.Windows.Forms.CheckBox();
            this.chkException = new System.Windows.Forms.CheckBox();
            this.btnDisplayColors = new System.Windows.Forms.Button();
            this.lblCulture = new System.Windows.Forms.Label();
            this.lblDisplayColors = new System.Windows.Forms.Label();
            this.lblDisplayText = new System.Windows.Forms.Label();
            this.btnDisplayText = new System.Windows.Forms.Button();
            this.txtPartSplit = new System.Windows.Forms.TextBox();
            this.lblPartSplit = new System.Windows.Forms.Label();
            this.txtMessage = new System.Windows.Forms.TextBox();
            this.lblMessage = new System.Windows.Forms.Label();
            this.chkScope = new System.Windows.Forms.CheckBox();
            this.chkContext = new System.Windows.Forms.CheckBox();
            this.chkFullName = new System.Windows.Forms.CheckBox();
            this.panSettings.SuspendLayout();
            this.SuspendLayout();
            // 
            // chkShowTime
            // 
            this.chkShowTime.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.chkShowTime.AutoSize = true;
            this.chkShowTime.Location = new System.Drawing.Point(106, 122);
            this.chkShowTime.Name = "chkShowTime";
            this.chkShowTime.Size = new System.Drawing.Size(79, 17);
            this.chkShowTime.TabIndex = 8;
            this.chkShowTime.Text = "Show Time";
            this.chkShowTime.UseVisualStyleBackColor = true;
            // 
            // cmbLogTime
            // 
            this.cmbLogTime.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbLogTime.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbLogTime.FormattingEnabled = true;
            this.cmbLogTime.Location = new System.Drawing.Point(106, 68);
            this.cmbLogTime.Name = "cmbLogTime";
            this.cmbLogTime.Size = new System.Drawing.Size(128, 21);
            this.cmbLogTime.TabIndex = 5;
            // 
            // lblLogTime
            // 
            this.lblLogTime.AutoSize = true;
            this.lblLogTime.Location = new System.Drawing.Point(10, 71);
            this.lblLogTime.Name = "lblLogTime";
            this.lblLogTime.Size = new System.Drawing.Size(51, 13);
            this.lblLogTime.TabIndex = 4;
            this.lblLogTime.Text = "Log Time";
            // 
            // cmbLogLevel
            // 
            this.cmbLogLevel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbLogLevel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbLogLevel.FormattingEnabled = true;
            this.cmbLogLevel.Location = new System.Drawing.Point(106, 41);
            this.cmbLogLevel.Name = "cmbLogLevel";
            this.cmbLogLevel.Size = new System.Drawing.Size(128, 21);
            this.cmbLogLevel.TabIndex = 3;
            // 
            // lblLogLevel
            // 
            this.lblLogLevel.AutoSize = true;
            this.lblLogLevel.Location = new System.Drawing.Point(10, 44);
            this.lblLogLevel.Name = "lblLogLevel";
            this.lblLogLevel.Size = new System.Drawing.Size(54, 13);
            this.lblLogLevel.TabIndex = 2;
            this.lblLogLevel.Text = "Log Level";
            // 
            // cmbLogType
            // 
            this.cmbLogType.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbLogType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbLogType.FormattingEnabled = true;
            this.cmbLogType.Location = new System.Drawing.Point(106, 14);
            this.cmbLogType.Name = "cmbLogType";
            this.cmbLogType.Size = new System.Drawing.Size(128, 21);
            this.cmbLogType.TabIndex = 1;
            // 
            // lblLogType
            // 
            this.lblLogType.AutoSize = true;
            this.lblLogType.Location = new System.Drawing.Point(10, 17);
            this.lblLogType.Name = "lblLogType";
            this.lblLogType.Size = new System.Drawing.Size(52, 13);
            this.lblLogType.TabIndex = 0;
            this.lblLogType.Text = "Log Type";
            // 
            // lblTest
            // 
            this.lblTest.BackColor = System.Drawing.SystemColors.ControlDark;
            this.lblTest.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblTest.Location = new System.Drawing.Point(250, 0);
            this.lblTest.Name = "lblTest";
            this.lblTest.Size = new System.Drawing.Size(454, 387);
            this.lblTest.TabIndex = 1;
            this.lblTest.Text = "Click here to generate messages.";
            this.lblTest.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panSettings
            // 
            this.panSettings.AutoScroll = true;
            this.panSettings.Controls.Add(this.cmbCulture);
            this.panSettings.Controls.Add(this.chkValues);
            this.panSettings.Controls.Add(this.chkException);
            this.panSettings.Controls.Add(this.btnDisplayColors);
            this.panSettings.Controls.Add(this.lblCulture);
            this.panSettings.Controls.Add(this.lblDisplayColors);
            this.panSettings.Controls.Add(this.lblDisplayText);
            this.panSettings.Controls.Add(this.btnDisplayText);
            this.panSettings.Controls.Add(this.txtPartSplit);
            this.panSettings.Controls.Add(this.lblPartSplit);
            this.panSettings.Controls.Add(this.txtMessage);
            this.panSettings.Controls.Add(this.lblMessage);
            this.panSettings.Controls.Add(this.chkScope);
            this.panSettings.Controls.Add(this.chkContext);
            this.panSettings.Controls.Add(this.chkFullName);
            this.panSettings.Controls.Add(this.chkShowTime);
            this.panSettings.Controls.Add(this.cmbLogType);
            this.panSettings.Controls.Add(this.cmbLogTime);
            this.panSettings.Controls.Add(this.lblLogType);
            this.panSettings.Controls.Add(this.lblLogTime);
            this.panSettings.Controls.Add(this.lblLogLevel);
            this.panSettings.Controls.Add(this.cmbLogLevel);
            this.panSettings.Dock = System.Windows.Forms.DockStyle.Left;
            this.panSettings.Location = new System.Drawing.Point(0, 0);
            this.panSettings.Name = "panSettings";
            this.panSettings.Padding = new System.Windows.Forms.Padding(0, 0, 10, 0);
            this.panSettings.Size = new System.Drawing.Size(250, 387);
            this.panSettings.TabIndex = 0;
            // 
            // cmbCulture
            // 
            this.cmbCulture.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbCulture.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbCulture.FormattingEnabled = true;
            this.cmbCulture.Location = new System.Drawing.Point(106, 95);
            this.cmbCulture.Name = "cmbCulture";
            this.cmbCulture.Size = new System.Drawing.Size(128, 21);
            this.cmbCulture.TabIndex = 7;
            // 
            // chkValues
            // 
            this.chkValues.AutoSize = true;
            this.chkValues.Location = new System.Drawing.Point(106, 238);
            this.chkValues.Name = "chkValues";
            this.chkValues.Size = new System.Drawing.Size(80, 17);
            this.chkValues.TabIndex = 13;
            this.chkValues.Text = "Use Values";
            this.chkValues.UseVisualStyleBackColor = true;
            // 
            // chkException
            // 
            this.chkException.AutoSize = true;
            this.chkException.Location = new System.Drawing.Point(106, 215);
            this.chkException.Name = "chkException";
            this.chkException.Size = new System.Drawing.Size(95, 17);
            this.chkException.TabIndex = 12;
            this.chkException.Text = "Use Exception";
            this.chkException.UseVisualStyleBackColor = true;
            // 
            // btnDisplayColors
            // 
            this.btnDisplayColors.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDisplayColors.Location = new System.Drawing.Point(106, 342);
            this.btnDisplayColors.Name = "btnDisplayColors";
            this.btnDisplayColors.Size = new System.Drawing.Size(128, 23);
            this.btnDisplayColors.TabIndex = 21;
            this.btnDisplayColors.Text = "Change...";
            this.btnDisplayColors.UseVisualStyleBackColor = true;
            this.btnDisplayColors.Click += new System.EventHandler(this.OnButtonDisplayColorsClick);
            // 
            // lblCulture
            // 
            this.lblCulture.AutoSize = true;
            this.lblCulture.Location = new System.Drawing.Point(10, 98);
            this.lblCulture.Name = "lblCulture";
            this.lblCulture.Size = new System.Drawing.Size(40, 13);
            this.lblCulture.TabIndex = 6;
            this.lblCulture.Text = "Culture";
            // 
            // lblDisplayColors
            // 
            this.lblDisplayColors.AutoSize = true;
            this.lblDisplayColors.Location = new System.Drawing.Point(10, 347);
            this.lblDisplayColors.Name = "lblDisplayColors";
            this.lblDisplayColors.Size = new System.Drawing.Size(86, 13);
            this.lblDisplayColors.TabIndex = 20;
            this.lblDisplayColors.Text = "Log-Level Colors";
            // 
            // lblDisplayText
            // 
            this.lblDisplayText.AutoSize = true;
            this.lblDisplayText.Location = new System.Drawing.Point(10, 318);
            this.lblDisplayText.Name = "lblDisplayText";
            this.lblDisplayText.Size = new System.Drawing.Size(78, 13);
            this.lblDisplayText.TabIndex = 18;
            this.lblDisplayText.Text = "Log Level Text";
            // 
            // btnDisplayText
            // 
            this.btnDisplayText.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDisplayText.Location = new System.Drawing.Point(106, 313);
            this.btnDisplayText.Name = "btnDisplayText";
            this.btnDisplayText.Size = new System.Drawing.Size(128, 23);
            this.btnDisplayText.TabIndex = 19;
            this.btnDisplayText.Text = "Change...";
            this.btnDisplayText.UseVisualStyleBackColor = true;
            this.btnDisplayText.Click += new System.EventHandler(this.OnButtonDisplayTextClick);
            // 
            // txtPartSplit
            // 
            this.txtPartSplit.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtPartSplit.Location = new System.Drawing.Point(106, 287);
            this.txtPartSplit.Name = "txtPartSplit";
            this.txtPartSplit.Size = new System.Drawing.Size(128, 20);
            this.txtPartSplit.TabIndex = 17;
            // 
            // lblPartSplit
            // 
            this.lblPartSplit.AutoSize = true;
            this.lblPartSplit.Location = new System.Drawing.Point(10, 290);
            this.lblPartSplit.Name = "lblPartSplit";
            this.lblPartSplit.Size = new System.Drawing.Size(49, 13);
            this.lblPartSplit.TabIndex = 16;
            this.lblPartSplit.Text = "Part Split";
            // 
            // txtMessage
            // 
            this.txtMessage.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtMessage.Location = new System.Drawing.Point(106, 261);
            this.txtMessage.Name = "txtMessage";
            this.txtMessage.Size = new System.Drawing.Size(128, 20);
            this.txtMessage.TabIndex = 15;
            // 
            // lblMessage
            // 
            this.lblMessage.AutoSize = true;
            this.lblMessage.Location = new System.Drawing.Point(11, 264);
            this.lblMessage.Name = "lblMessage";
            this.lblMessage.Size = new System.Drawing.Size(50, 13);
            this.lblMessage.TabIndex = 14;
            this.lblMessage.Text = "Message";
            // 
            // chkScope
            // 
            this.chkScope.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.chkScope.AutoSize = true;
            this.chkScope.Location = new System.Drawing.Point(106, 191);
            this.chkScope.Name = "chkScope";
            this.chkScope.Size = new System.Drawing.Size(79, 17);
            this.chkScope.TabIndex = 11;
            this.chkScope.Text = "Use Scope";
            this.chkScope.UseVisualStyleBackColor = true;
            // 
            // chkContext
            // 
            this.chkContext.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.chkContext.AutoSize = true;
            this.chkContext.Location = new System.Drawing.Point(106, 168);
            this.chkContext.Name = "chkContext";
            this.chkContext.Size = new System.Drawing.Size(84, 17);
            this.chkContext.TabIndex = 10;
            this.chkContext.Text = "Use Context";
            this.chkContext.UseVisualStyleBackColor = true;
            // 
            // chkFullName
            // 
            this.chkFullName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.chkFullName.AutoSize = true;
            this.chkFullName.Location = new System.Drawing.Point(106, 145);
            this.chkFullName.Name = "chkFullName";
            this.chkFullName.Size = new System.Drawing.Size(95, 17);
            this.chkFullName.TabIndex = 9;
            this.chkFullName.Text = "Use Full Name";
            this.chkFullName.UseVisualStyleBackColor = true;
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(704, 387);
            this.Controls.Add(this.lblTest);
            this.Controls.Add(this.panSettings);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainWindow";
            this.Text = "Console Logger Test Window";
            this.panSettings.ResumeLayout(false);
            this.panSettings.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.ComboBox cmbLogLevel;
        private System.Windows.Forms.Label lblLogLevel;
        private System.Windows.Forms.ComboBox cmbLogType;
        private System.Windows.Forms.Label lblLogType;
        private System.Windows.Forms.ComboBox cmbLogTime;
        private System.Windows.Forms.Label lblLogTime;
        private System.Windows.Forms.CheckBox chkShowTime;
        private System.Windows.Forms.Label lblTest;
        private System.Windows.Forms.Panel panSettings;
        private System.Windows.Forms.TextBox txtMessage;
        private System.Windows.Forms.Label lblMessage;
        private System.Windows.Forms.CheckBox chkScope;
        private System.Windows.Forms.CheckBox chkContext;
        private System.Windows.Forms.CheckBox chkFullName;
        private System.Windows.Forms.TextBox txtPartSplit;
        private System.Windows.Forms.Label lblPartSplit;
        private System.Windows.Forms.Label lblDisplayText;
        private System.Windows.Forms.Button btnDisplayText;
        private System.Windows.Forms.Label lblDisplayColors;
        private System.Windows.Forms.Button btnDisplayColors;
        private System.Windows.Forms.CheckBox chkValues;
        private System.Windows.Forms.CheckBox chkException;
        private System.Windows.Forms.ComboBox cmbCulture;
        private System.Windows.Forms.Label lblCulture;
    }
}

