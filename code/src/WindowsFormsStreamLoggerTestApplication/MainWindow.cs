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

using Newtonsoft.Json;
using Plexdata.LogWriter.Abstraction;
using Plexdata.LogWriter.Definitions;
using Plexdata.LogWriter.Extensions;
using Plexdata.LogWriter.Features;
using Plexdata.LogWriter.Logging;
using Plexdata.LogWriter.Settings;
using Plexdata.LogWriter.Testing.Helper.Dialogs;
using Plexdata.LogWriter.Testing.Helper.Logging;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Xml;

namespace Plexdata.LogWriter.Testing.Helper
{
    public partial class MainWindow : Form
    {
        private DumpsDialog dumps;
        private LoggerStream source;
        private IStreamLogger logger;
        private IStreamLoggerSettings settings;
        private Exception exception;

        #region Construction

        public MainWindow()
            : base()
        {
            this.InitializeComponent();

            try
            {
                this.TestMethod1();
            }
            catch (Exception exception)
            {
                this.exception = exception;
            }

            this.SetupLogging();

            this.SetupControls();
        }

        #endregion

        #region Menu events

        private void OnMenuExitClicked(Object sender, EventArgs args)
        {
            this.dumps.CanClose = true;
            this.dumps.Close();

            this.Close();
            Application.Exit();
        }

        private void OnFormatOpening(Object sender, EventArgs args)
        {
            if (sender is ToolStripMenuItem menu)
            {
                foreach (ToolStripItem current in menu.DropDownItems)
                {
                    if (current is ToolStripMenuItem item)
                    {
                        item.Checked = (LogType)item.Tag == this.settings.LogType;
                    }
                }
            }
        }

        private void OnMenuLevelChildClick(Object sender, EventArgs args)
        {
            if (sender is ToolStripMenuItem item)
            {
                this.settings.LogLevel = (LogLevel)item.Tag;
            }
        }

        private void OnMenuAboutClicked(Object sender, EventArgs args)
        {
            AboutDialog dialog = new AboutDialog();
            dialog.ShowDialog(this);
        }

        private void OnMenuFormatChildClick(Object sender, EventArgs args)
        {
            if (sender is ToolStripMenuItem item)
            {
                this.settings.LogType = (LogType)item.Tag;
            }
        }

        private void OnLevelOpening(Object sender, EventArgs args)
        {
            if (sender is ToolStripMenuItem menu)
            {
                foreach (ToolStripItem current in menu.DropDownItems)
                {
                    if (current is ToolStripMenuItem item)
                    {
                        item.Checked = (LogLevel)item.Tag == this.settings.LogLevel;
                    }
                }
            }
        }

        private void OnMenuDumpsClicked(Object sender, EventArgs args)
        {
            this.dumps.Visible = true;
        }

        #endregion

        #region Other events

        private void OnSourceProcessStreamData(Object sender, LoggerStreamEventArgs args)
        {
            this.dumps.AddMessages(args.Messages);

            foreach (String current in args.Messages)
            {
                try
                {
                    LoggerData message = null;

                    switch (this.settings.LogType)
                    {
                        case LogType.Json:
                            message = this.GetLoggerDataFromJson(current);
                            break;
                        case LogType.Xml:
                            message = this.GetLoggerDataFromXml(current);
                            break;
                        case LogType.Csv:
                            message = this.GetLoggerDataFromCsv(current);
                            break;
                        default:
                            return;
                    }

                    ListViewItem item = new ListViewItem
                    {
                        Tag = message,
                        Text = message.Time.ToString("s"),
                        BackColor = this.GetBackColor(message.Level),
                        ForeColor = this.GetForeColor(message.Level)
                    };

                    item.SubItems.Add(new ListViewItem.ListViewSubItem(item, message.Level));
                    item.SubItems.Add(new ListViewItem.ListViewSubItem(item, message.Context));
                    item.SubItems.Add(new ListViewItem.ListViewSubItem(item, message.Scope));
                    item.SubItems.Add(new ListViewItem.ListViewSubItem(item, message.Message));

                    this.lstMessages.Items.Add(item);
                    this.lstMessages.EnsureVisible(this.lstMessages.Items.Count - 1);
                }
                catch (Exception exception)
                {
                    System.Diagnostics.Debug.WriteLine(exception);
                }
            }
        }

        private void OnTestLabelMouseClick(Object sender, MouseEventArgs args)
        {
            (String Label, Object Value)[] parameters = new (String Label, Object Value)[] {
                ( Label: "X",  Value: args.Location.X ),
                ( Label: "Y",  Value: args.Location.Y ),
            };

            this.logger.Trace("Trace messge", this.exception, parameters);
            this.logger.Debug("Debug messge", this.exception, parameters);
            this.logger.Verbose("Verbose messge", this.exception, parameters);
            this.logger.Message("Message messge", this.exception, parameters);
            this.logger.Warning("Warning messge", this.exception, parameters);
            this.logger.Error("Error messge", this.exception, parameters);
            this.logger.Fatal("Fatal messge", this.exception, parameters);
            this.logger.Critical("Critical messge", this.exception, parameters);
        }

        private void OnMessagesMouseDoubleClick(Object sender, MouseEventArgs args)
        {
            if (this.lstMessages.SelectedItems.Count > 0 && this.lstMessages.SelectedItems[0].Tag is LoggerData selected)
            {
                DetailsDialog dialog = new DetailsDialog(selected);
                dialog.ShowDialog(this);
            }
        }

        #endregion

        #region Private methods

        private Color GetBackColor(String level)
        {
            switch (level.ToUpper())
            {
                case "TRACE":
                    return Color.White;
                case "DEBUG":
                    return Color.White;
                case "VERBOSE":
                    return Color.White;
                case "MESSAGE":
                    return Color.White;
                case "WARNING":
                    return Color.Yellow;
                case "ERROR":
                    return Color.Red;
                case "FATAL":
                    return Color.Red;
                case "CRITICAL":
                    return Color.DarkRed;
                default:
                    return Color.White;
            }
        }

        private Color GetForeColor(String level)
        {
            switch (level.ToUpper())
            {
                case "TRACE":
                    return Color.DarkSlateGray;
                case "DEBUG":
                    return Color.DarkSlateGray;
                case "VERBOSE":
                    return Color.Black;
                case "MESSAGE":
                    return Color.Black;
                case "WARNING":
                    return Color.Black;
                case "ERROR":
                    return Color.Black;
                case "FATAL":
                    return Color.WhiteSmoke;
                case "CRITICAL":
                    return Color.WhiteSmoke;
                default:
                    return Color.Black;
            }
        }

        private LoggerData GetLoggerDataFromJson(String message)
        {
            return JsonConvert.DeserializeObject<LoggerData>(message);
        }

        private LoggerData GetLoggerDataFromXml(String message)
        {
            LoggerData result = new LoggerData();

            XmlDocument document = new XmlDocument();

            document.LoadXml(message);

            XmlNode root = document.DocumentElement;

            foreach (XmlNode parent in root.SelectNodes("/logging/notification"))
            {
                result.Key = Guid.Parse(parent.SelectSingleNode("key").InnerText);
                result.Level = parent.SelectSingleNode("level").InnerText;
                result.Time = DateTime.Parse(parent.SelectSingleNode("time").InnerText);
                result.Context = parent.SelectSingleNode("context").InnerText;
                result.Scope = parent.SelectSingleNode("scope").InnerText;
                result.Message = parent.SelectSingleNode("message").InnerText;
                result.Exception = parent.SelectSingleNode("exception").InnerText.Replace("\\r", "\r").Replace("\\n", "\n");

                List<LoggerDataDetail> items = new List<LoggerDataDetail>();
                foreach (XmlNode detail in parent.SelectSingleNode("details").ChildNodes)
                {
                    items.Add(new LoggerDataDetail(detail.Name, detail.InnerText));
                }
                result.Details = items;
            }

            return result;
        }

        private LoggerData GetLoggerDataFromCsv(String message)
        {
            LoggerData result = new LoggerData();

            String[] items = message.Split(this.settings.PartSplit);

            result.Key = Guid.Parse(items[0]);
            result.Time = DateTime.Parse(items[1]);
            result.Level = items[2];
            result.Context = items[3];
            result.Scope = items[4];
            result.Message = items[5];
            result.Exception = items[7];

            List<LoggerDataDetail> details = new List<LoggerDataDetail>();
            String data = items[6];
            data = data.Remove(data.IndexOf('['), 1).Remove(data.LastIndexOf(']') - 1, 1);
            foreach (String current in data.Split(new String[] { "],[" }, StringSplitOptions.None))
            {
                String[] pieces = current.Split('=');
                details.Add(new LoggerDataDetail(pieces[0], pieces[1]));
            }
            result.Details = details;

            return result;
        }

        private void SetupLogging()
        {
            this.source = new LoggerStream();
            this.source.ProcessStreamData += this.OnSourceProcessStreamData;

            this.settings = new StreamLoggerSettings()
            {
                LogType = LogType.Json,
                LogLevel = LogLevel.Trace,
                Stream = this.source,
            };

            this.source.Encoding = this.settings.Encoding;

            this.logger = new StreamLogger(this.settings);
        }

        private void SetupControls()
        {
            List<ToolStripItem> items = new List<ToolStripItem>();

            foreach (LogType current in new LogType[] { LogType.Json, LogType.Xml, LogType.Csv })
            {
                ToolStripMenuItem item = new ToolStripMenuItem()
                {
                    Text = current.ToString().ToUpper(),
                    Tag = current,
                };

                item.Click += this.OnMenuFormatChildClick;
                items.Add(item);
            }

            this.mnuToolsFormat.DropDownItems.AddRange(items.ToArray());
            items.Clear();

            foreach (LogLevel current in ((LogLevel[])Enum.GetValues(typeof(LogLevel))).Distinct().ToArray())
            {
                ToolStripMenuItem item = new ToolStripMenuItem()
                {
                    Text = current.ToString().ToUpper(),
                    Tag = current,
                };

                item.Click += this.OnMenuLevelChildClick;
                items.Add(item);
            }

            this.mnuToolsLevel.DropDownItems.AddRange(items.ToArray());
            items.Clear();

            this.lblTest.MouseClick += this.OnTestLabelMouseClick;

            this.dumps = new DumpsDialog();
        }

        #endregion

        #region Exception helper

        private void TestMethod1()
        {
            this.TestMethod2();
        }

        private void TestMethod2()
        {
            this.TestMethod3();
        }

        private void TestMethod3()
        {
            throw new FormatException("test format exception");
        }

        #endregion
    }
}
