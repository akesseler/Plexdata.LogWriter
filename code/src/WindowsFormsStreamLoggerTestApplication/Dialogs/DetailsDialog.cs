﻿/*
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

using Plexdata.LogWriter.Testing.Helper.Logging;
using System;
using System.Windows.Forms;

namespace Plexdata.LogWriter.Testing.Helper.Dialogs
{
    public partial class DetailsDialog : Form
    {
        public DetailsDialog(LoggerData message)
        {
            this.InitializeComponent();
            this.SetupControls(message);
        }

        private void SetupControls(LoggerData message)
        {
            if (message == null)
            {
                return;
            }

            if (message.Details != null)
            {
                foreach (LoggerDataDetail detail in message.Details)
                {
                    ListViewItem item = new ListViewItem(new String[] { detail.Label, (detail.Value ?? String.Empty).ToString() });
                    this.lstDetails.Items.Add(item);
                }
            }

            this.txtException.Lines = (message.Exception ?? String.Empty)
                .Replace("\\\\", "\\")
                .Split(new String[] { "\\r\\n" }, StringSplitOptions.None);
        }
    }
}
