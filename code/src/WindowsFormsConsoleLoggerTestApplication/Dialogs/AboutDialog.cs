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

using Plexdata.Utilities.Assembly;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;

namespace Plexdata.LogWriter.Testing.Helper.Dialogs
{
    partial class AboutDialog : Form
    {
        private class AssemblyInfoItem
        {
            private readonly AssemblyInfo parent = null;

            public AssemblyInfoItem(AssemblyInfo parent)
            {
                this.parent = parent ?? throw new ArgumentNullException(nameof(parent));
            }

            public override String ToString()
            {
                return String.Format("{0} ({1})", this.parent.FileName, this.parent.Version);
            }
        }

        public AboutDialog()
        {
            this.InitializeComponent();

            AssemblyInfo info = AppDomain.CurrentDomain.GetCurrentAssemblyInformation();

            this.Text += info.Title;
            this.lblProduct.Text = info.Product;
            this.lblVersion.Text = $"{this.lblVersion.Text} {info.Version}";
            this.lblCopyright.Text = info.Copyright;
            this.lblDescription.Text = info.Description;
            this.lstDependencies.DataSource = this.GetAssemblyInfoItems();
        }

        private void OnLogoClicked(Object sender, EventArgs args)
        {
            Process.Start("http://www.plexdata.de/");
        }

        private void OnCloseClicked(Object sender, EventArgs args)
        {
            this.Close();
        }

        protected override void OnShown(EventArgs args)
        {
            if (!(this.Owner is null))
            {
                this.Icon = this.Owner.Icon;
            }

            base.OnShown(args);
        }

        private IList<AssemblyInfoItem> GetAssemblyInfoItems()
        {
            return AppDomain.CurrentDomain
                .GetAssemblyInformation()
                .Select(x => new AssemblyInfoItem(x))
                .ToList();
        }
    }
}
