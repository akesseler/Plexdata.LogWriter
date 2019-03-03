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
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace Plexdata.Controls
{
    public class ColorComboBox : ComboBox
    {
        private const String DefaultSelectorText = "More...";

        private Int32 previousIndex = -1;
        private Boolean lockSelecting = false;
        private Dictionary<Int32, String> defaultColors = null;

        public ColorComboBox()
            : base()
        {
            base.DoubleBuffered = true;
            base.DrawMode = DrawMode.OwnerDrawFixed;
            base.DropDownStyle = ComboBoxStyle.DropDownList;

            // NOTE: It is unfortunately strictly necessary to deal with RGB values 
            //       instead of Colors. Otherwise we get in trouble with comparing 
            //       colors. More clearly, this means that a color's equals-operator 
            //       firstly compares the name and secondly it compares the RGB value. 
            this.defaultColors = new Dictionary<Int32, String>();
            this.defaultColors.Add(Color.Black.ToArgb(), "01");
            this.defaultColors.Add(Color.White.ToArgb(), "02");
            this.defaultColors.Add(Color.Silver.ToArgb(), "03");
            this.defaultColors.Add(Color.Gray.ToArgb(), "04");
            this.defaultColors.Add(Color.Fuchsia.ToArgb(), "05");
            this.defaultColors.Add(Color.Purple.ToArgb(), "06");
            this.defaultColors.Add(Color.Blue.ToArgb(), "07");
            this.defaultColors.Add(Color.Navy.ToArgb(), "08");
            this.defaultColors.Add(Color.Aqua.ToArgb(), "09");
            this.defaultColors.Add(Color.Teal.ToArgb(), "10");
            this.defaultColors.Add(Color.Lime.ToArgb(), "11");
            this.defaultColors.Add(Color.Green.ToArgb(), "12");
            this.defaultColors.Add(Color.Yellow.ToArgb(), "13");
            this.defaultColors.Add(Color.Olive.ToArgb(), "14");
            this.defaultColors.Add(Color.Red.ToArgb(), "15");
            this.defaultColors.Add(Color.Maroon.ToArgb(), "16");

            foreach (Int32 color in this.defaultColors.Keys)
            {
                this.Items.Add(color);
            }

            this.SelectorText = ColorComboBox.DefaultSelectorText;
            base.SelectedIndex = 0; // Select Black color.
        }

        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new ComboBox.ObjectCollection Items { get { return base.Items; } }

        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new DrawMode DrawMode { get { return base.DrawMode; } set { } }

        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new ComboBoxStyle DropDownStyle { get { return base.DropDownStyle; } set { } }

        [Browsable(true)]
        [Category("Appearance")]
        [DefaultValue(true)]
        [RefreshProperties(RefreshProperties.Repaint)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [Description("Determines whether the label of the default colors is shown or not.")]
        public Boolean ShowLabels
        {
            get
            {
                return this.showLabels;
            }
            set
            {
                if (this.showLabels != value)
                {
                    this.showLabels = value;
                    this.Invalidate();
                }
            }
        }
        private Boolean showLabels = true;

        [Browsable(true)]
        [Localizable(true)]
        [Category("Appearance")]
        [DefaultValue(ColorComboBox.DefaultSelectorText)]
        [RefreshProperties(RefreshProperties.Repaint)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [Description("Defines the text used as selector to open the external color selection dialog. If this " +
                     "text is null or empty the usage of the external color selection dialog is disabled.")]
        public String SelectorText
        {
            get
            {
                Int32 count = this.Items.Count;
                if (count > 0 && this.Items[count - 1] is String)
                {
                    return this.Items[count - 1] as String;
                }
                else
                {
                    return String.Empty;
                }
            }
            set
            {
                Int32 count = this.Items.Count;
                if (String.IsNullOrEmpty(value))
                {
                    if (count > 0 && this.Items[count - 1] is String)
                    {
                        this.Items.RemoveAt(count - 1);
                    }
                }
                else
                {
                    if (count > 0 && this.Items[count - 1] is String)
                    {
                        this.Items[count - 1] = value;
                    }
                    else
                    {
                        this.Items.Add(value);
                    }
                    this.Invalidate();
                }
            }
        }

        [Browsable(true)]
        [Category("Appearance")]
        [DefaultValue(typeof(Color), "Black")]
        [RefreshProperties(RefreshProperties.Repaint)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [Description("Gets or sets the color currently selected in the control. " +
                     "If given color does not exist then it is added automatically.")]
        public Color SelectedColor
        {
            get
            {
                if (this.SelectedIndex != -1 && this.Items[this.SelectedIndex] is Int32)
                {
                    return Color.FromArgb((Int32)this.Items[this.SelectedIndex]);
                }
                else
                {
                    throw new ArgumentOutOfRangeException("SelectedColor");
                }
            }
            set
            {
                if (!this.defaultColors.ContainsKey(value.ToArgb()) && !this.Items.Contains(value.ToArgb()))
                {
                    // BUGFIX: Value type must be an integer!
                    this.Items.Insert(0, value.ToArgb());
                }
                this.SelectedItem = value.ToArgb();
            }
        }

        public void ReplaceAllColorsBy(IList<Color> colors)
        {
            if (colors == null || colors.Count < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(colors));
            }

            this.defaultColors.Clear();
            this.Items.Clear();

            foreach (Color color in colors)
            {
                this.Items.Add(color.ToArgb());
            }
        }

        protected override void OnMeasureItem(MeasureItemEventArgs args)
        {
            args.ItemHeight = this.ItemHeight;
            args.ItemWidth = this.ClientSize.Width;

            base.OnMeasureItem(args);
        }

        protected override void OnDrawItem(DrawItemEventArgs args)
        {
            base.OnDrawItem(args);

            args.DrawBackground();

            try
            {
                if (args.Index < this.Items.Count)
                {
                    if (this.Items[args.Index] is Int32)
                    {
                        TextFormatFlags flags = TextFormatFlags.Left | TextFormatFlags.SingleLine | TextFormatFlags.NoPadding;
                        Int32 color = (Int32)this.Items[args.Index];
                        Rectangle bounds = Rectangle.Inflate(args.Bounds, -1, -1);

                        using (Brush brush = new SolidBrush(Color.FromArgb(color)))
                        {
                            Int32 cx = 0;
                            if (this.defaultColors.ContainsKey(color) && this.showLabels)
                            {
                                String label = this.defaultColors[color];

                                cx = TextRenderer.MeasureText(
                                    args.Graphics, label, args.Font, Size.Empty, flags).Width + 4;

                                TextRenderer.DrawText(
                                    args.Graphics, label, args.Font, bounds, args.ForeColor, flags);
                            }

                            bounds.X += cx;
                            bounds.Width -= cx;

                            args.Graphics.FillRectangle(brush, bounds);

                            bounds.Width -= 1;
                            bounds.Height -= 1;

                            args.Graphics.DrawRectangle(Pens.Black, bounds);
                        }
                    }
                    else if (this.Items[args.Index] is String)
                    {
                        TextFormatFlags flags = TextFormatFlags.Left | TextFormatFlags.SingleLine;
                        if (args.Index == this.Items.Count - 1)
                        {
                            // Check if given font supports style Underline.
                            FontStyle style = args.Font.FontFamily.IsStyleAvailable(FontStyle.Underline) ? FontStyle.Underline : args.Font.Style;

                            using (Font font = new Font(args.Font, style))
                            {
                                Color color = Color.Blue; // Use typical link color as default.
                                if ((args.State & DrawItemState.Selected) == DrawItemState.Selected)
                                {
                                    color = args.ForeColor;
                                }

                                TextRenderer.DrawText(
                                    args.Graphics, this.Items[args.Index] as String,
                                    font, args.Bounds, color, flags);
                            }
                        }
                    }
                    // TODO: Paint other strings or objects.

                    if ((args.State & DrawItemState.Selected) != 0 && (args.State & DrawItemState.ComboBoxEdit) == 0)
                    {
                        ControlPaint.DrawFocusRectangle(args.Graphics, args.Bounds, this.BackColor, this.ForeColor);
                    }
                }
            }
            catch (Exception exception)
            {
                Debug.WriteLine(exception);
            }
        }

        protected override void OnSelectedIndexChanged(EventArgs args)
        {
            if (this.lockSelecting) { return; }

            if (this.SelectedIndex == this.Items.Count - 1 && this.Items[this.SelectedIndex] is String)
            {
                try
                {
                    this.lockSelecting = true;
                    this.SelectedIndex = this.previousIndex;

                    ColorDialog dialog = new ColorDialog();
                    if (this.Items[this.SelectedIndex] is Int32)
                    {
                        dialog.Color = Color.FromArgb((Int32)this.Items[this.SelectedIndex]);
                    }

                    if (dialog.ShowDialog(this) == DialogResult.OK)
                    {
                        if (!this.defaultColors.ContainsKey(dialog.Color.ToArgb()) && !this.Items.Contains(dialog.Color.ToArgb()))
                        {
                            this.Items.Insert(0, dialog.Color.ToArgb());
                        }

                        this.SelectedItem = dialog.Color.ToArgb();
                        this.previousIndex = this.SelectedIndex;
                        base.OnSelectedIndexChanged(args);
                    }
                }
                finally
                {
                    this.lockSelecting = false;
                }
            }
            else
            {
                this.previousIndex = this.SelectedIndex;
                base.OnSelectedIndexChanged(args);
            }
        }
    }
}