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

namespace Plexdata.LogWriter.Testing.Helper.Dialogs
{
    partial class LogLevelDisplayTextDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LogLevelDisplayTextDialog));
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnApply = new System.Windows.Forms.Button();
            this.lblTrace = new System.Windows.Forms.Label();
            this.lblDebug = new System.Windows.Forms.Label();
            this.lblVerbose = new System.Windows.Forms.Label();
            this.lblMessage = new System.Windows.Forms.Label();
            this.lblWarning = new System.Windows.Forms.Label();
            this.lblError = new System.Windows.Forms.Label();
            this.lblFatal = new System.Windows.Forms.Label();
            this.lblCritical = new System.Windows.Forms.Label();
            this.txtTrace = new System.Windows.Forms.TextBox();
            this.txtDebug = new System.Windows.Forms.TextBox();
            this.txtVerbose = new System.Windows.Forms.TextBox();
            this.txtMessage = new System.Windows.Forms.TextBox();
            this.txtWarning = new System.Windows.Forms.TextBox();
            this.txtError = new System.Windows.Forms.TextBox();
            this.txtFatal = new System.Windows.Forms.TextBox();
            this.txtCritical = new System.Windows.Forms.TextBox();
            this.btnRestore = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(297, 227);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 18;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnApply
            // 
            this.btnApply.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnApply.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnApply.Location = new System.Drawing.Point(216, 227);
            this.btnApply.Name = "btnApply";
            this.btnApply.Size = new System.Drawing.Size(75, 23);
            this.btnApply.TabIndex = 17;
            this.btnApply.Text = "&Apply";
            this.btnApply.UseVisualStyleBackColor = true;
            // 
            // lblTrace
            // 
            this.lblTrace.AutoSize = true;
            this.lblTrace.Location = new System.Drawing.Point(12, 15);
            this.lblTrace.Name = "lblTrace";
            this.lblTrace.Size = new System.Drawing.Size(35, 13);
            this.lblTrace.TabIndex = 0;
            this.lblTrace.Text = "Trace";
            // 
            // lblDebug
            // 
            this.lblDebug.AutoSize = true;
            this.lblDebug.Location = new System.Drawing.Point(12, 41);
            this.lblDebug.Name = "lblDebug";
            this.lblDebug.Size = new System.Drawing.Size(39, 13);
            this.lblDebug.TabIndex = 2;
            this.lblDebug.Text = "Debug";
            // 
            // lblVerbose
            // 
            this.lblVerbose.AutoSize = true;
            this.lblVerbose.Location = new System.Drawing.Point(12, 67);
            this.lblVerbose.Name = "lblVerbose";
            this.lblVerbose.Size = new System.Drawing.Size(46, 13);
            this.lblVerbose.TabIndex = 4;
            this.lblVerbose.Text = "Verbose";
            // 
            // lblMessage
            // 
            this.lblMessage.AutoSize = true;
            this.lblMessage.Location = new System.Drawing.Point(12, 93);
            this.lblMessage.Name = "lblMessage";
            this.lblMessage.Size = new System.Drawing.Size(50, 13);
            this.lblMessage.TabIndex = 6;
            this.lblMessage.Text = "Message";
            // 
            // lblWarning
            // 
            this.lblWarning.AutoSize = true;
            this.lblWarning.Location = new System.Drawing.Point(12, 119);
            this.lblWarning.Name = "lblWarning";
            this.lblWarning.Size = new System.Drawing.Size(47, 13);
            this.lblWarning.TabIndex = 8;
            this.lblWarning.Text = "Warning";
            // 
            // lblError
            // 
            this.lblError.AutoSize = true;
            this.lblError.Location = new System.Drawing.Point(12, 145);
            this.lblError.Name = "lblError";
            this.lblError.Size = new System.Drawing.Size(29, 13);
            this.lblError.TabIndex = 10;
            this.lblError.Text = "Error";
            // 
            // lblFatal
            // 
            this.lblFatal.AutoSize = true;
            this.lblFatal.Location = new System.Drawing.Point(12, 171);
            this.lblFatal.Name = "lblFatal";
            this.lblFatal.Size = new System.Drawing.Size(30, 13);
            this.lblFatal.TabIndex = 12;
            this.lblFatal.Text = "Fatal";
            // 
            // lblCritical
            // 
            this.lblCritical.AutoSize = true;
            this.lblCritical.Location = new System.Drawing.Point(12, 197);
            this.lblCritical.Name = "lblCritical";
            this.lblCritical.Size = new System.Drawing.Size(38, 13);
            this.lblCritical.TabIndex = 14;
            this.lblCritical.Text = "Critical";
            // 
            // txtTrace
            // 
            this.txtTrace.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtTrace.Location = new System.Drawing.Point(79, 12);
            this.txtTrace.Name = "txtTrace";
            this.txtTrace.Size = new System.Drawing.Size(293, 20);
            this.txtTrace.TabIndex = 1;
            // 
            // txtDebug
            // 
            this.txtDebug.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtDebug.Location = new System.Drawing.Point(79, 38);
            this.txtDebug.Name = "txtDebug";
            this.txtDebug.Size = new System.Drawing.Size(293, 20);
            this.txtDebug.TabIndex = 3;
            // 
            // txtVerbose
            // 
            this.txtVerbose.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtVerbose.Location = new System.Drawing.Point(79, 64);
            this.txtVerbose.Name = "txtVerbose";
            this.txtVerbose.Size = new System.Drawing.Size(293, 20);
            this.txtVerbose.TabIndex = 5;
            // 
            // txtMessage
            // 
            this.txtMessage.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtMessage.Location = new System.Drawing.Point(79, 90);
            this.txtMessage.Name = "txtMessage";
            this.txtMessage.Size = new System.Drawing.Size(293, 20);
            this.txtMessage.TabIndex = 7;
            // 
            // txtWarning
            // 
            this.txtWarning.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtWarning.Location = new System.Drawing.Point(79, 116);
            this.txtWarning.Name = "txtWarning";
            this.txtWarning.Size = new System.Drawing.Size(293, 20);
            this.txtWarning.TabIndex = 9;
            // 
            // txtError
            // 
            this.txtError.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtError.Location = new System.Drawing.Point(79, 142);
            this.txtError.Name = "txtError";
            this.txtError.Size = new System.Drawing.Size(293, 20);
            this.txtError.TabIndex = 11;
            // 
            // txtFatal
            // 
            this.txtFatal.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtFatal.Location = new System.Drawing.Point(79, 168);
            this.txtFatal.Name = "txtFatal";
            this.txtFatal.Size = new System.Drawing.Size(293, 20);
            this.txtFatal.TabIndex = 13;
            // 
            // txtCritical
            // 
            this.txtCritical.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtCritical.Location = new System.Drawing.Point(79, 194);
            this.txtCritical.Name = "txtCritical";
            this.txtCritical.Size = new System.Drawing.Size(293, 20);
            this.txtCritical.TabIndex = 15;
            // 
            // btnRestore
            // 
            this.btnRestore.Location = new System.Drawing.Point(12, 227);
            this.btnRestore.Name = "btnRestore";
            this.btnRestore.Size = new System.Drawing.Size(75, 23);
            this.btnRestore.TabIndex = 16;
            this.btnRestore.Text = "&Restore";
            this.btnRestore.UseVisualStyleBackColor = true;
            this.btnRestore.Click += new System.EventHandler(this.OnButtonRestoreClick);
            // 
            // LogLevelDisplayTextDialog
            // 
            this.AcceptButton = this.btnApply;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(384, 262);
            this.Controls.Add(this.btnRestore);
            this.Controls.Add(this.txtCritical);
            this.Controls.Add(this.txtFatal);
            this.Controls.Add(this.txtError);
            this.Controls.Add(this.txtWarning);
            this.Controls.Add(this.txtMessage);
            this.Controls.Add(this.txtVerbose);
            this.Controls.Add(this.txtDebug);
            this.Controls.Add(this.txtTrace);
            this.Controls.Add(this.lblCritical);
            this.Controls.Add(this.lblFatal);
            this.Controls.Add(this.lblError);
            this.Controls.Add(this.lblWarning);
            this.Controls.Add(this.lblMessage);
            this.Controls.Add(this.lblVerbose);
            this.Controls.Add(this.lblDebug);
            this.Controls.Add(this.lblTrace);
            this.Controls.Add(this.btnApply);
            this.Controls.Add(this.btnCancel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "LogLevelDisplayTextDialog";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Log-Level Display Text";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnApply;
        private System.Windows.Forms.Label lblTrace;
        private System.Windows.Forms.Label lblDebug;
        private System.Windows.Forms.Label lblVerbose;
        private System.Windows.Forms.Label lblMessage;
        private System.Windows.Forms.Label lblWarning;
        private System.Windows.Forms.Label lblError;
        private System.Windows.Forms.Label lblFatal;
        private System.Windows.Forms.Label lblCritical;
        private System.Windows.Forms.TextBox txtTrace;
        private System.Windows.Forms.TextBox txtDebug;
        private System.Windows.Forms.TextBox txtVerbose;
        private System.Windows.Forms.TextBox txtMessage;
        private System.Windows.Forms.TextBox txtWarning;
        private System.Windows.Forms.TextBox txtError;
        private System.Windows.Forms.TextBox txtFatal;
        private System.Windows.Forms.TextBox txtCritical;
        private System.Windows.Forms.Button btnRestore;
    }
}