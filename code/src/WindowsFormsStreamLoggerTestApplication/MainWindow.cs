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

using Newtonsoft.Json;
using Plexdata.LogWriter.Abstraction;
using Plexdata.LogWriter.Definitions;
using Plexdata.LogWriter.Extensions;
using Plexdata.LogWriter.Logging;
using Plexdata.LogWriter.Settings;
using Plexdata.LogWriter.Testing.Helper.Helper;
using Plexdata.LogWriter.Testing.Helper.Logging;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace Plexdata.LogWriter.Testing.Helper
{
    public partial class MainWindow : Form
    {
        private EventDrivenStream source;
        private IStreamLogger logger;
        private IStreamLoggerSettings settings;

        private Exception exception;

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

        public MainWindow()
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


            this.source = new EventDrivenStream();
            this.source.StreamDataWritten += this.OnSourceStreamDataWritten;

            this.settings = new StreamLoggerSettings()
            {
                LogType = LogType.Json,
                LogLevel = LogLevel.Trace,
                Stream = this.source,
            };

            this.source.Encoding = this.settings.Encoding;

            this.logger = new StreamLogger(this.settings);

            this.lblTest.MouseClick += this.OnTestLabelMouseClick;
        }

        private void OnSourceStreamDataWritten(Object sender, StreamEventArgs args)
        {
            foreach (String current in args.Messages)
            {
                try
                {
                    LoggerData message = JsonConvert.DeserializeObject<LoggerData>(current);

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
                LoggerDataDetailDialog dialog = new LoggerDataDetailDialog(selected);
                dialog.ShowDialog(this);
            }
        }

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
    }
}
