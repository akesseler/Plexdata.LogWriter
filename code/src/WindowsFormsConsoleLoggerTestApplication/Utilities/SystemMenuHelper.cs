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
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Windows.Forms;

namespace Plexdata.Utilities.Assembly
{
    [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
    internal class SystemMenuHelper : NativeWindow
    {
        #region Private classes

        private struct Command
        {
            public Int32 Id { get; set; }
            public String Label { get; set; }
            public Action Action { get; set; }
            public Boolean IsSeparator { get; set; }
        }

        #endregion

        #region Private fields

        private Int32 offset = 0;
        private readonly Form parent = null;
        private readonly List<Command> pendings = null;
        private readonly Dictionary<Int32, Command> commands = null;

        #endregion

        #region Construction

        public SystemMenuHelper(Form parent)
            : base()
        {
            this.parent = parent ?? throw new ArgumentNullException(nameof(parent));
            this.pendings = new List<Command>();
            this.commands = new Dictionary<Int32, Command>();

            this.parent.HandleCreated += this.OnParentHandleCreated;
            this.parent.HandleDestroyed += this.OnParentHandleDestroyed;
        }

        #endregion

        #region Public methods

        public void AddSeparator()
        {
            this.AddCommand(new Command()
            {
                Id = 0,
                Label = null,
                Action = null,
                IsSeparator = true,
            });
        }

        public void AddCommand(String label, Action action)
        {
            if (String.IsNullOrWhiteSpace(label))
            {
                throw new ArgumentException("Menu label must not be null, empty or consists only of white spaces.", nameof(label));
            }

            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            this.AddCommand(new Command()
            {
                Id = 0,
                Label = label,
                Action = action,
                IsSeparator = false,
            });
        }

        #endregion

        #region Protected methods

        [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
        protected override void WndProc(ref Message message)
        {
            base.WndProc(ref message);

            try
            {
                if (message.Msg == SystemMenuHelper.WM_SYSCOMMAND)
                {
                    if ((Int64)message.WParam > 0 && (Int64)message.WParam <= this.offset)
                    {
                        this.commands[(Int32)message.WParam].Action.Invoke();
                    }
                }
            }
            catch (Exception exception)
            {
                Trace.WriteLine(exception);
            }
        }

        #endregion

        #region Private handlers

        private void OnParentHandleCreated(Object sender, EventArgs args)
        {
            base.AssignHandle(((Form)sender).Handle);

            IntPtr handle = SystemMenuHelper.GetSystemMenu(this.parent.Handle, false);

            foreach (Command current in this.pendings)
            {
                this.AddCommand(handle, current);
            }

            this.pendings.Clear();
        }

        private void OnParentHandleDestroyed(Object sender, EventArgs args)
        {
            base.ReleaseHandle();
        }

        #endregion

        #region Private methods

        private void AddCommand(Command command)
        {
            command.Id = ++this.offset;

            if (!this.parent.IsHandleCreated)
            {
                this.pendings.Add(command);
            }
            else
            {
                this.AddCommand(SystemMenuHelper.GetSystemMenu(this.parent.Handle, false), command);
            }
        }

        private void AddCommand(IntPtr handle, Command command)
        {
            if (command.IsSeparator)
            {
                SystemMenuHelper.AppendMenu(handle, SystemMenuHelper.MF_SEPARATOR, 0, String.Empty);
            }
            else
            {
                SystemMenuHelper.AppendMenu(handle, SystemMenuHelper.MF_STRING, command.Id, command.Label);
            }

            this.commands.Add(command.Id, command);
        }

        #endregion

        #region Native imports

        private const Int32 WM_SYSCOMMAND = 0x112;
        private const Int32 MF_SEPARATOR = 0x800;
        private const Int32 MF_STRING = 0x0;

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetSystemMenu(IntPtr hWnd, Boolean bRevert);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern Boolean AppendMenu(IntPtr hMenu, Int32 uFlags, Int32 uIDNewItem, String lpNewItem);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern Boolean InsertMenu(IntPtr hMenu, Int32 uPosition, Int32 uFlags, Int32 uIDNewItem, String lpNewItem);

        #endregion
    }
}
