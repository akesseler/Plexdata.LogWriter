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
            this.lblTest = new System.Windows.Forms.Label();
            this.spcContent = new System.Windows.Forms.SplitContainer();
            this.lstMessages = new System.Windows.Forms.ListView();
            this.colTime = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colLevel = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colContext = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colScope = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colMessage = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            ((System.ComponentModel.ISupportInitialize)(this.spcContent)).BeginInit();
            this.spcContent.Panel1.SuspendLayout();
            this.spcContent.Panel2.SuspendLayout();
            this.spcContent.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblTest
            // 
            this.lblTest.BackColor = System.Drawing.SystemColors.ControlDark;
            this.lblTest.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblTest.Location = new System.Drawing.Point(0, 0);
            this.lblTest.Name = "lblTest";
            this.lblTest.Size = new System.Drawing.Size(784, 350);
            this.lblTest.TabIndex = 2;
            this.lblTest.Text = "Click here to generate messages.\r\n\r\nDouble-click on of the list items to show mes" +
    "sage details.";
            this.lblTest.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
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
            this.spcContent.Panel1.Controls.Add(this.lblTest);
            // 
            // spcContent.Panel2
            // 
            this.spcContent.Panel2.Controls.Add(this.lstMessages);
            this.spcContent.Size = new System.Drawing.Size(784, 562);
            this.spcContent.SplitterDistance = 350;
            this.spcContent.TabIndex = 3;
            // 
            // lstMessages
            // 
            this.lstMessages.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colTime,
            this.colLevel,
            this.colContext,
            this.colScope,
            this.colMessage});
            this.lstMessages.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstMessages.FullRowSelect = true;
            this.lstMessages.Location = new System.Drawing.Point(0, 0);
            this.lstMessages.MultiSelect = false;
            this.lstMessages.Name = "lstMessages";
            this.lstMessages.Size = new System.Drawing.Size(784, 208);
            this.lstMessages.TabIndex = 0;
            this.lstMessages.UseCompatibleStateImageBehavior = false;
            this.lstMessages.View = System.Windows.Forms.View.Details;
            this.lstMessages.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.OnMessagesMouseDoubleClick);
            // 
            // colTime
            // 
            this.colTime.Text = "Time";
            this.colTime.Width = 120;
            // 
            // colLevel
            // 
            this.colLevel.Text = "Level";
            this.colLevel.Width = 100;
            // 
            // colContext
            // 
            this.colContext.Text = "Context";
            this.colContext.Width = 80;
            // 
            // colScope
            // 
            this.colScope.Text = "Scope";
            this.colScope.Width = 80;
            // 
            // colMessage
            // 
            this.colMessage.Text = "Message";
            this.colMessage.Width = 400;
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 562);
            this.Controls.Add(this.spcContent);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainWindow";
            this.Text = "Stream Logger Test Window";
            this.spcContent.Panel1.ResumeLayout(false);
            this.spcContent.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.spcContent)).EndInit();
            this.spcContent.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Label lblTest;
        private System.Windows.Forms.SplitContainer spcContent;
        private System.Windows.Forms.ListView lstMessages;
        private System.Windows.Forms.ColumnHeader colTime;
        private System.Windows.Forms.ColumnHeader colLevel;
        private System.Windows.Forms.ColumnHeader colContext;
        private System.Windows.Forms.ColumnHeader colScope;
        private System.Windows.Forms.ColumnHeader colMessage;
    }
}

