/*
 * MIT License
 * 
 * Copyright (c) 2021 plexdata.de
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
    partial class AboutDialog
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AboutDialog));
            this.picLogo = new System.Windows.Forms.PictureBox();
            this.lblProduct = new System.Windows.Forms.Label();
            this.lblVersion = new System.Windows.Forms.Label();
            this.lblCopyright = new System.Windows.Forms.Label();
            this.btnClose = new System.Windows.Forms.Button();
            this.ttpHelper = new System.Windows.Forms.ToolTip(this.components);
            this.flpSummary = new System.Windows.Forms.FlowLayoutPanel();
            this.tlpContent = new System.Windows.Forms.TableLayoutPanel();
            this.grpDependencies = new System.Windows.Forms.GroupBox();
            this.lstDependencies = new System.Windows.Forms.ListBox();
            this.grpDescription = new System.Windows.Forms.GroupBox();
            this.lblDescription = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.picLogo)).BeginInit();
            this.flpSummary.SuspendLayout();
            this.tlpContent.SuspendLayout();
            this.grpDependencies.SuspendLayout();
            this.grpDescription.SuspendLayout();
            this.SuspendLayout();
            // 
            // picLogo
            // 
            this.picLogo.Cursor = System.Windows.Forms.Cursors.Hand;
            resources.ApplyResources(this.picLogo, "picLogo");
            this.picLogo.Name = "picLogo";
            this.picLogo.TabStop = false;
            this.ttpHelper.SetToolTip(this.picLogo, resources.GetString("picLogo.ToolTip"));
            this.picLogo.Click += new System.EventHandler(this.OnLogoClicked);
            // 
            // lblProduct
            // 
            this.lblProduct.AutoEllipsis = true;
            resources.ApplyResources(this.lblProduct, "lblProduct");
            this.lblProduct.Name = "lblProduct";
            // 
            // lblVersion
            // 
            this.lblVersion.AutoEllipsis = true;
            resources.ApplyResources(this.lblVersion, "lblVersion");
            this.lblVersion.Name = "lblVersion";
            // 
            // lblCopyright
            // 
            this.lblCopyright.AutoEllipsis = true;
            resources.ApplyResources(this.lblCopyright, "lblCopyright");
            this.lblCopyright.Name = "lblCopyright";
            // 
            // btnClose
            // 
            resources.ApplyResources(this.btnClose, "btnClose");
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Name = "btnClose";
            this.btnClose.Click += new System.EventHandler(this.OnCloseClicked);
            // 
            // ttpHelper
            // 
            this.ttpHelper.ShowAlways = true;
            // 
            // flpSummary
            // 
            resources.ApplyResources(this.flpSummary, "flpSummary");
            this.flpSummary.Controls.Add(this.lblProduct);
            this.flpSummary.Controls.Add(this.lblVersion);
            this.flpSummary.Controls.Add(this.lblCopyright);
            this.flpSummary.Name = "flpSummary";
            // 
            // tlpContent
            // 
            resources.ApplyResources(this.tlpContent, "tlpContent");
            this.tlpContent.Controls.Add(this.flpSummary, 0, 0);
            this.tlpContent.Controls.Add(this.picLogo, 1, 0);
            this.tlpContent.Controls.Add(this.grpDependencies, 0, 2);
            this.tlpContent.Controls.Add(this.grpDescription, 0, 1);
            this.tlpContent.Name = "tlpContent";
            // 
            // grpDependencies
            // 
            resources.ApplyResources(this.grpDependencies, "grpDependencies");
            this.tlpContent.SetColumnSpan(this.grpDependencies, 2);
            this.grpDependencies.Controls.Add(this.lstDependencies);
            this.grpDependencies.Name = "grpDependencies";
            this.grpDependencies.TabStop = false;
            // 
            // lstDependencies
            // 
            resources.ApplyResources(this.lstDependencies, "lstDependencies");
            this.lstDependencies.FormattingEnabled = true;
            this.lstDependencies.Name = "lstDependencies";
            this.lstDependencies.Sorted = true;
            this.lstDependencies.TabStop = false;
            // 
            // grpDescription
            // 
            resources.ApplyResources(this.grpDescription, "grpDescription");
            this.tlpContent.SetColumnSpan(this.grpDescription, 2);
            this.grpDescription.Controls.Add(this.lblDescription);
            this.grpDescription.Name = "grpDescription";
            this.grpDescription.TabStop = false;
            // 
            // lblDescription
            // 
            this.lblDescription.AutoEllipsis = true;
            resources.ApplyResources(this.lblDescription, "lblDescription");
            this.lblDescription.Name = "lblDescription";
            // 
            // AboutDialog
            // 
            this.AcceptButton = this.btnClose;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(248)))), ((int)(((byte)(248)))));
            this.CancelButton = this.btnClose;
            this.Controls.Add(this.tlpContent);
            this.Controls.Add(this.btnClose);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AboutDialog";
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            ((System.ComponentModel.ISupportInitialize)(this.picLogo)).EndInit();
            this.flpSummary.ResumeLayout(false);
            this.flpSummary.PerformLayout();
            this.tlpContent.ResumeLayout(false);
            this.tlpContent.PerformLayout();
            this.grpDependencies.ResumeLayout(false);
            this.grpDescription.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox picLogo;
        private System.Windows.Forms.Label lblProduct;
        private System.Windows.Forms.Label lblVersion;
        private System.Windows.Forms.Label lblCopyright;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.ToolTip ttpHelper;
        private System.Windows.Forms.FlowLayoutPanel flpSummary;
        private System.Windows.Forms.TableLayoutPanel tlpContent;
        private System.Windows.Forms.GroupBox grpDependencies;
        private System.Windows.Forms.ListBox lstDependencies;
        private System.Windows.Forms.GroupBox grpDescription;
        private System.Windows.Forms.Label lblDescription;
    }
}
