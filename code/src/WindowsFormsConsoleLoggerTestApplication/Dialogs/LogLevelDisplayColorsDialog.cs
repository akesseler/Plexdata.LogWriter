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

using Plexdata.Controls;
using Plexdata.LogWriter.Definitions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Plexdata.LogWriter.Testing.Helper.Dialogs
{
    public partial class LogLevelDisplayColorsDialog : Form
    {
        private IDictionary<LogLevel, Coloring> coloring = null;
        private readonly List<Color> usedColors;
        private readonly Color colorBlack;
        private readonly Color colorDarkBlue;
        private readonly Color colorDarkGreen;
        private readonly Color colorDarkCyan;
        private readonly Color colorDarkRed;
        private readonly Color colorDarkMagenta;
        private readonly Color colorDarkYellow;
        private readonly Color colorGray;
        private readonly Color colorDarkGray;
        private readonly Color colorBlue;
        private readonly Color colorGreen;
        private readonly Color colorCyan;
        private readonly Color colorRed;
        private readonly Color colorMagenta;
        private readonly Color colorYellow;
        private readonly Color colorWhite;

        public LogLevelDisplayColorsDialog(IDictionary<LogLevel, Coloring> coloring)
        {
            this.coloring = coloring ?? throw new ArgumentNullException(nameof(coloring));

            this.InitializeComponent();

            this.colorBlack = Color.FromArgb(0, 0, 0);
            this.colorDarkBlue = Color.FromArgb(0, 0, 128);
            this.colorDarkGreen = Color.FromArgb(0, 128, 0);
            this.colorDarkCyan = Color.FromArgb(0, 128, 128);
            this.colorDarkRed = Color.FromArgb(128, 0, 0);
            this.colorDarkMagenta = Color.FromArgb(128, 0, 128);
            this.colorDarkYellow = Color.FromArgb(128, 128, 0);
            this.colorGray = Color.FromArgb(192, 192, 192);
            this.colorDarkGray = Color.FromArgb(128, 128, 128);
            this.colorBlue = Color.FromArgb(0, 0, 255);
            this.colorGreen = Color.FromArgb(0, 255, 0);
            this.colorCyan = Color.FromArgb(0, 255, 255);
            this.colorRed = Color.FromArgb(255, 0, 0);
            this.colorMagenta = Color.FromArgb(255, 0, 255);
            this.colorYellow = Color.FromArgb(255, 255, 0);
            this.colorWhite = Color.FromArgb(244, 244, 244);

            this.usedColors = new List<Color>{
                this.colorBlack,
                this.colorDarkBlue,
                this.colorDarkGreen,
                this.colorDarkCyan,
                this.colorDarkRed,
                this.colorDarkMagenta,
                this.colorDarkYellow,
                this.colorGray,
                this.colorDarkGray,
                this.colorBlue,
                this.colorGreen,
                this.colorCyan,
                this.colorRed,
                this.colorMagenta,
                this.colorYellow,
                this.colorWhite,
            };
        }

        protected override void OnLoad(EventArgs args)
        {
            base.OnLoad(args);
            this.Setup();
        }

        protected override void OnClosing(CancelEventArgs args)
        {
            if (this.DialogResult == DialogResult.OK)
            {
                try
                {
                    this.coloring[LogLevel.Trace] = this.GetColoring(nameof(LogLevel.Trace));
                    this.coloring[LogLevel.Debug] = this.GetColoring(nameof(LogLevel.Debug));
                    this.coloring[LogLevel.Verbose] = this.GetColoring(nameof(LogLevel.Verbose));
                    this.coloring[LogLevel.Message] = this.GetColoring(nameof(LogLevel.Message));
                    this.coloring[LogLevel.Warning] = this.GetColoring(nameof(LogLevel.Warning));
                    this.coloring[LogLevel.Error] = this.GetColoring(nameof(LogLevel.Error));
                    this.coloring[LogLevel.Fatal] = this.GetColoring(nameof(LogLevel.Fatal));
                    this.coloring[LogLevel.Critical] = this.GetColoring(nameof(LogLevel.Critical));
                }
                catch (Exception exception)
                {
                    MessageBox.Show(this, exception.Message, "Save Values", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    args.Cancel = true;
                    return;
                }
            }

            base.OnClosing(args);
        }

        private void OnButtonRestoreClick(Object sender, EventArgs args)
        {
            Cursor cursor = this.Cursor;

            try
            {
                this.Cursor = Cursors.WaitCursor;

                this.SetColoring(nameof(LogLevel.Trace), new Coloring(ConsoleColor.Gray, ConsoleColor.Black));
                this.SetColoring(nameof(LogLevel.Debug), new Coloring(ConsoleColor.Gray, ConsoleColor.Black));
                this.SetColoring(nameof(LogLevel.Verbose), new Coloring(ConsoleColor.White, ConsoleColor.Black));
                this.SetColoring(nameof(LogLevel.Message), new Coloring(ConsoleColor.White, ConsoleColor.Black));
                this.SetColoring(nameof(LogLevel.Warning), new Coloring(ConsoleColor.Yellow, ConsoleColor.Black));
                this.SetColoring(nameof(LogLevel.Error), new Coloring(ConsoleColor.Red, ConsoleColor.Black));
                this.SetColoring(nameof(LogLevel.Fatal), new Coloring(ConsoleColor.Gray, ConsoleColor.DarkRed));
                this.SetColoring(nameof(LogLevel.Critical), new Coloring(ConsoleColor.Black, ConsoleColor.Red));
            }
            finally
            {
                this.Cursor = cursor;
            }
        }

        private void Setup()
        {
            this.SetColoring(nameof(LogLevel.Trace), this.coloring[LogLevel.Trace]);
            this.SetColoring(nameof(LogLevel.Debug), this.coloring[LogLevel.Debug]);
            this.SetColoring(nameof(LogLevel.Verbose), this.coloring[LogLevel.Verbose]);
            this.SetColoring(nameof(LogLevel.Message), this.coloring[LogLevel.Message]);
            this.SetColoring(nameof(LogLevel.Warning), this.coloring[LogLevel.Warning]);
            this.SetColoring(nameof(LogLevel.Error), this.coloring[LogLevel.Error]);
            this.SetColoring(nameof(LogLevel.Fatal), this.coloring[LogLevel.Fatal]);
            this.SetColoring(nameof(LogLevel.Critical), this.coloring[LogLevel.Critical]);
        }

        private void SetColoring(String level, Coloring coloring)
        {
            ColorComboBox control;

            control = this.Controls.Find($"cmb{level}Foreground", true).FirstOrDefault() as ColorComboBox;
            if (control == null) { throw new NotSupportedException(); }
            control.ReplaceAllColorsBy(this.usedColors);
            control.SelectedColor = this.ToDisplayColor(coloring.Foreground);

            control = this.Controls.Find($"cmb{level}Background", true).FirstOrDefault() as ColorComboBox;
            if (control == null) { throw new NotSupportedException(); }
            control.ReplaceAllColorsBy(this.usedColors);
            control.SelectedColor = this.ToDisplayColor(coloring.Background);
        }

        private Coloring GetColoring(String level)
        {
            ColorComboBox control;

            control = this.Controls.Find($"cmb{level}Foreground", true).FirstOrDefault() as ColorComboBox;
            if (control == null) { throw new NotSupportedException(); }
            ConsoleColor foreground = this.ToConsoleColor(control.SelectedColor);

            control = this.Controls.Find($"cmb{level}Background", true).FirstOrDefault() as ColorComboBox;
            if (control == null) { throw new NotSupportedException(); }
            ConsoleColor background = this.ToConsoleColor(control.SelectedColor);

            return new Coloring(foreground, background);
        }

        private Color ToDisplayColor(ConsoleColor color)
        {
            if (color == ConsoleColor.Black) { return this.colorBlack; }
            if (color == ConsoleColor.DarkBlue) { return this.colorDarkBlue; }
            if (color == ConsoleColor.DarkGreen) { return this.colorDarkGreen; }
            if (color == ConsoleColor.DarkCyan) { return this.colorDarkCyan; }
            if (color == ConsoleColor.DarkRed) { return this.colorDarkRed; }
            if (color == ConsoleColor.DarkMagenta) { return this.colorDarkMagenta; }
            if (color == ConsoleColor.DarkYellow) { return this.colorDarkYellow; }
            if (color == ConsoleColor.Gray) { return this.colorGray; }
            if (color == ConsoleColor.DarkGray) { return this.colorDarkGray; }
            if (color == ConsoleColor.Blue) { return this.colorBlue; }
            if (color == ConsoleColor.Green) { return this.colorGreen; }
            if (color == ConsoleColor.Cyan) { return this.colorCyan; }
            if (color == ConsoleColor.Red) { return this.colorRed; }
            if (color == ConsoleColor.Magenta) { return this.colorMagenta; }
            if (color == ConsoleColor.Yellow) { return this.colorYellow; }
            if (color == ConsoleColor.White) { return this.colorWhite; }

            throw new NotSupportedException();
        }

        private ConsoleColor ToConsoleColor(Color color)
        {
            if (color == this.colorBlack) { return ConsoleColor.Black; }
            if (color == this.colorDarkBlue) { return ConsoleColor.DarkBlue; }
            if (color == this.colorDarkGreen) { return ConsoleColor.DarkGreen; }
            if (color == this.colorDarkCyan) { return ConsoleColor.DarkCyan; }
            if (color == this.colorDarkRed) { return ConsoleColor.DarkRed; }
            if (color == this.colorDarkMagenta) { return ConsoleColor.DarkMagenta; }
            if (color == this.colorDarkYellow) { return ConsoleColor.DarkYellow; }
            if (color == this.colorGray) { return ConsoleColor.Gray; }
            if (color == this.colorDarkGray) { return ConsoleColor.DarkGray; }
            if (color == this.colorBlue) { return ConsoleColor.Blue; }
            if (color == this.colorGreen) { return ConsoleColor.Green; }
            if (color == this.colorCyan) { return ConsoleColor.Cyan; }
            if (color == this.colorRed) { return ConsoleColor.Red; }
            if (color == this.colorMagenta) { return ConsoleColor.Magenta; }
            if (color == this.colorYellow) { return ConsoleColor.Yellow; }
            if (color == this.colorWhite) { return ConsoleColor.White; }

            throw new NotSupportedException();
        }
    }
}
