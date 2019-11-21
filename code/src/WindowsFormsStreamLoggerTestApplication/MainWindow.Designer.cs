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
            this.mainMenu = new System.Windows.Forms.MenuStrip();
            this.mnuFile = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuFileExit = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuTools = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuToolsLevel = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuToolsFormat = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.mnuToolsDumps = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuHelp = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuHelpAbout = new System.Windows.Forms.ToolStripMenuItem();
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
            this.mainMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblTest
            // 
            this.lblTest.BackColor = System.Drawing.SystemColors.ControlDark;
            this.lblTest.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblTest.Location = new System.Drawing.Point(0, 24);
            this.lblTest.Name = "lblTest";
            this.lblTest.Size = new System.Drawing.Size(784, 326);
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
            this.spcContent.Panel1.Controls.Add(this.mainMenu);
            // 
            // spcContent.Panel2
            // 
            this.spcContent.Panel2.Controls.Add(this.lstMessages);
            this.spcContent.Size = new System.Drawing.Size(784, 562);
            this.spcContent.SplitterDistance = 350;
            this.spcContent.TabIndex = 3;
            // 
            // mainMenu
            // 
            this.mainMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuFile,
            this.mnuTools,
            this.mnuHelp});
            this.mainMenu.Location = new System.Drawing.Point(0, 0);
            this.mainMenu.Name = "mainMenu";
            this.mainMenu.Size = new System.Drawing.Size(784, 24);
            this.mainMenu.TabIndex = 4;
            this.mainMenu.Text = "Main Menu";
            // 
            // mnuFile
            // 
            this.mnuFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuFileExit});
            this.mnuFile.Name = "mnuFile";
            this.mnuFile.Size = new System.Drawing.Size(37, 20);
            this.mnuFile.Text = "&File";
            // 
            // mnuFileExit
            // 
            this.mnuFileExit.Name = "mnuFileExit";
            this.mnuFileExit.Size = new System.Drawing.Size(92, 22);
            this.mnuFileExit.Text = "E&xit";
            this.mnuFileExit.Click += new System.EventHandler(this.OnMenuExitClicked);
            // 
            // mnuTools
            // 
            this.mnuTools.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuToolsLevel,
            this.mnuToolsFormat,
            this.mnuSeparator1,
            this.mnuToolsDumps});
            this.mnuTools.Name = "mnuTools";
            this.mnuTools.Size = new System.Drawing.Size(48, 20);
            this.mnuTools.Text = "&Tools";
            // 
            // mnuToolsLevel
            // 
            this.mnuToolsLevel.Name = "mnuToolsLevel";
            this.mnuToolsLevel.Size = new System.Drawing.Size(180, 22);
            this.mnuToolsLevel.Text = "Level";
            this.mnuToolsLevel.DropDownOpening += new System.EventHandler(this.OnLevelOpening);
            // 
            // mnuToolsFormat
            // 
            this.mnuToolsFormat.Name = "mnuToolsFormat";
            this.mnuToolsFormat.Size = new System.Drawing.Size(180, 22);
            this.mnuToolsFormat.Text = "Format";
            this.mnuToolsFormat.DropDownOpening += new System.EventHandler(this.OnFormatOpening);
            // 
            // mnuSeparator1
            // 
            this.mnuSeparator1.Name = "mnuSeparator1";
            this.mnuSeparator1.Size = new System.Drawing.Size(177, 6);
            // 
            // mnuToolsDumps
            // 
            this.mnuToolsDumps.Name = "mnuToolsDumps";
            this.mnuToolsDumps.Size = new System.Drawing.Size(180, 22);
            this.mnuToolsDumps.Text = "&Dumps";
            this.mnuToolsDumps.Click += new System.EventHandler(this.OnMenuDumpsClicked);
            // 
            // mnuHelp
            // 
            this.mnuHelp.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuHelpAbout});
            this.mnuHelp.Name = "mnuHelp";
            this.mnuHelp.Size = new System.Drawing.Size(44, 20);
            this.mnuHelp.Text = "&Help";
            // 
            // mnuHelpAbout
            // 
            this.mnuHelpAbout.Name = "mnuHelpAbout";
            this.mnuHelpAbout.Size = new System.Drawing.Size(116, 22);
            this.mnuHelpAbout.Text = "&About...";
            this.mnuHelpAbout.Click += new System.EventHandler(this.OnMenuAboutClicked);
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
            this.lstMessages.HideSelection = false;
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
            this.MainMenuStrip = this.mainMenu;
            this.Name = "MainWindow";
            this.Text = "Stream Logger Test Window";
            this.spcContent.Panel1.ResumeLayout(false);
            this.spcContent.Panel1.PerformLayout();
            this.spcContent.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.spcContent)).EndInit();
            this.spcContent.ResumeLayout(false);
            this.mainMenu.ResumeLayout(false);
            this.mainMenu.PerformLayout();
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
        private System.Windows.Forms.MenuStrip mainMenu;
        private System.Windows.Forms.ToolStripMenuItem mnuFile;
        private System.Windows.Forms.ToolStripMenuItem mnuFileExit;
        private System.Windows.Forms.ToolStripMenuItem mnuTools;
        private System.Windows.Forms.ToolStripMenuItem mnuHelp;
        private System.Windows.Forms.ToolStripMenuItem mnuHelpAbout;
        private System.Windows.Forms.ToolStripMenuItem mnuToolsDumps;
        private System.Windows.Forms.ToolStripMenuItem mnuToolsFormat;
        private System.Windows.Forms.ToolStripMenuItem mnuToolsLevel;
        private System.Windows.Forms.ToolStripSeparator mnuSeparator1;
    }
}

