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

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;

namespace Plexdata.LogWriter.Testing.Helper.Dialogs
{
    public partial class DumpsDialog : Form
    {
        public DumpsDialog()
        {
            this.InitializeComponent();
            this.lstMessages.HorizontalScrollbar = true;
            this.lstMessages.ScrollAlwaysVisible = true;
        }

        public Boolean CanClose { get; set; } = false;

        public void AddMessages(IEnumerable<String> messages)
        {
            if (messages != null && messages.Any())
            {
                try
                {
                    this.lstMessages.BeginUpdate();

                    this.lstMessages.Items.AddRange(messages.Select(x => x.Replace("\r", "\\r").Replace("\n", "\\n")).ToArray());
                    this.lstMessages.SelectedIndex = this.lstMessages.Items.Count - 1;
                }
                finally
                {
                    this.lstMessages.EndUpdate();
                }
            }
        }

        protected override void OnClosing(CancelEventArgs args)
        {
            args.Cancel = !this.CanClose;

            if (this.CanClose)
            {
                base.OnClosing(args);
            }
            else
            {
                this.Visible = false;
            }
        }
    }
}
