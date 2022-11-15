/*
 * MIT License
 * 
 * Copyright (c) 2022 plexdata.de
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
    partial class DetailsDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DetailsDialog));
            this.btnClose = new System.Windows.Forms.Button();
            this.panBottom = new System.Windows.Forms.Panel();
            this.lstDetails = new System.Windows.Forms.ListView();
            this.colLabel = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colValue = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.txtException = new System.Windows.Forms.TextBox();
            this.spcContent = new System.Windows.Forms.SplitContainer();
            this.panBottom.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.spcContent)).BeginInit();
            this.spcContent.Panel1.SuspendLayout();
            this.spcContent.Panel2.SuspendLayout();
            this.spcContent.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(397, 11);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 0;
            this.btnClose.Text = "&Close";
            this.btnClose.UseVisualStyleBackColor = true;
            // 
            // panBottom
            // 
            this.panBottom.Controls.Add(this.btnClose);
            this.panBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panBottom.Location = new System.Drawing.Point(0, 316);
            this.panBottom.Name = "panBottom";
            this.panBottom.Size = new System.Drawing.Size(484, 46);
            this.panBottom.TabIndex = 1;
            // 
            // lstDetails
            // 
            this.lstDetails.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colLabel,
            this.colValue});
            this.lstDetails.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstDetails.FullRowSelect = true;
            this.lstDetails.Location = new System.Drawing.Point(0, 0);
            this.lstDetails.Name = "lstDetails";
            this.lstDetails.Size = new System.Drawing.Size(484, 120);
            this.lstDetails.TabIndex = 0;
            this.lstDetails.UseCompatibleStateImageBehavior = false;
            this.lstDetails.View = System.Windows.Forms.View.Details;
            // 
            // colLabel
            // 
            this.colLabel.Text = "Label";
            // 
            // colValue
            // 
            this.colValue.Text = "Value";
            // 
            // txtException
            // 
            this.txtException.BackColor = System.Drawing.SystemColors.Window;
            this.txtException.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtException.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtException.Location = new System.Drawing.Point(0, 0);
            this.txtException.Multiline = true;
            this.txtException.Name = "txtException";
            this.txtException.ReadOnly = true;
            this.txtException.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtException.Size = new System.Drawing.Size(484, 192);
            this.txtException.TabIndex = 0;
            this.txtException.WordWrap = false;
            // 
            // spcContent
            // 
            this.spcContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.spcContent.Location = new System.Drawing.Point(0, 0);
            this.spcContent.Name = "spcContent";
            this.spcContent.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // spcContent.Panel1
            // 
            this.spcContent.Panel1.Controls.Add(this.lstDetails);
            // 
            // spcContent.Panel2
            // 
            this.spcContent.Panel2.Controls.Add(this.txtException);
            this.spcContent.Size = new System.Drawing.Size(484, 316);
            this.spcContent.SplitterDistance = 120;
            this.spcContent.TabIndex = 0;
            // 
            // DetailsDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(484, 362);
            this.Controls.Add(this.spcContent);
            this.Controls.Add(this.panBottom);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DetailsDialog";
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Logger Data Details";
            this.panBottom.ResumeLayout(false);
            this.spcContent.Panel1.ResumeLayout(false);
            this.spcContent.Panel2.ResumeLayout(false);
            this.spcContent.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.spcContent)).EndInit();
            this.spcContent.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Panel panBottom;
        private System.Windows.Forms.ListView lstDetails;
        private System.Windows.Forms.ColumnHeader colLabel;
        private System.Windows.Forms.ColumnHeader colValue;
        private System.Windows.Forms.TextBox txtException;
        private System.Windows.Forms.SplitContainer spcContent;
    }
}