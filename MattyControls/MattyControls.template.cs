﻿using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;

namespace MattyControls
{
    public class MattyControl : Control
    {
        public static int Distance = 10;
        public static int LabelWidth = 100;
        public enum Horizontal { Left, CopyLeft, Center, CopyRight /* Pun intended */, Right };
        public enum Vertical { Top, CopyTop, Middle, CopyBottom, Bottom };

        // The static methods that actually do the work
        public static void LocateInside(Control ctrl, Control parentCtrl, MattyControl.Horizontal h, MattyControl.Vertical v, int d) {
            if (d == -1)
                d = MattyControl.Distance;
            int x = 0;
            int y = 0;

            if (h == Horizontal.Left)
                x = d;
            if (h == Horizontal.CopyLeft)
                x = parentCtrl.Location.X;
            if (h == Horizontal.Center)
                x = (parentCtrl.ClientSize.Width - ctrl.Size.Width) / 2;
            if (h == Horizontal.CopyRight)
                x = parentCtrl.Location.X + parentCtrl.Size.Width - ctrl.Size.Width;
            if (h == Horizontal.Right)
                x = parentCtrl.ClientSize.Width - ctrl.Size.Width - d;

            if (v == Vertical.Top)
                y = d;
            if (v == Vertical.CopyTop)
                y = parentCtrl.Location.Y;
            if (v == Vertical.Middle)
                y = (parentCtrl.ClientSize.Height - ctrl.Size.Height) / 2;
            if (v == Vertical.CopyBottom)
                y = parentCtrl.Location.Y + parentCtrl.ClientSize.Height - ctrl.Size.Height;
            if (v == Vertical.Bottom)
                y = parentCtrl.ClientSize.Height - ctrl.Size.Height - d;

            ctrl.Location = new Point(x, y);
        }

        public static void LocateFrom(Control ctrl, Control c, MattyControl.Horizontal h, MattyControl.Vertical v, int d) {
            if (d == -1)
                d = MattyControl.Distance;
            int x = 0;
            int y = 0;

            if (h == Horizontal.Left)
                x = c.Location.X - ctrl.Size.Width - d;
            if (h == Horizontal.CopyLeft)
                x = c.Location.X;
            if (h == Horizontal.Center)
                x = c.Location.X + (c.Size.Width - ctrl.Size.Width) / 2;
            if (h == Horizontal.CopyRight)
                x = c.Location.X + c.Size.Width - ctrl.Size.Width;
            if (h == Horizontal.Right)
                x = c.Location.X + c.Size.Width + d;

            if (v == Vertical.Top)
                y = c.Location.Y - ctrl.Size.Height - d;
            if (v == Vertical.CopyTop)
                y = c.Location.Y;
            if (v == Vertical.Middle)
                y = c.Location.Y + (c.Size.Height - ctrl.Size.Height) / 2;
            if (v == Vertical.CopyBottom)
                y = c.Location.Y + c.Size.Height - ctrl.Size.Height;
            if (v == Vertical.Bottom)
                y = c.Location.Y + c.Size.Height + d;

            ctrl.Location = new Point(x, y);
        }

        public static Lbl AddLabel(Control ctrl, string text, int d, bool moveCtrl, int labelWidth) {
            // Create a new label
            Lbl label = new Lbl(text, ctrl.Parent);

            // Set its width
            if (labelWidth != 0)
                label.Size = new Size(labelWidth, label.Height);

            // Give it the right position
            if (moveCtrl) {
                label.LocateFrom(ctrl, Horizontal.CopyLeft, Vertical.Middle, d);
                LocateFrom(ctrl, label, Horizontal.Right, Vertical.Middle, d);
            }
            else {
                label.LocateFrom(ctrl, Horizontal.Left, Vertical.Middle, d);
            }

            // Return the label, for the sake of easyness
            return label;
        }

        public static void StretchRight(Control ctrl, float factor) {
            ctrl.Size = new Size((int)(ctrl.Width * factor), ctrl.Height);
        }
        public static void StretchRight(Control ctrl, UserControl parent, int d) {
            if (d == -1)
                d = MattyControl.Distance;
            ctrl.Size = new Size(parent.ClientSize.Width - ctrl.Location.X - d, ctrl.Height);
        }
        public static void StretchRight(Control ctrl, Control c, int d) {
            if (d == -1)
                d = MattyControl.Distance;
            ctrl.Size = new Size(c.Location.X - ctrl.Location.X - d, ctrl.Height);
        }

        public static void StretchDown(Control ctrl, float factor) {
            ctrl.Size = new Size(ctrl.Width, (int)(ctrl.Height * factor));
        }
        public static void StretchDown(Control ctrl, UserControl parent, int d) {
            if (d == -1)
                d = MattyControl.Distance;
            ctrl.Size = new Size(ctrl.Width, parent.ClientSize.Height - ctrl.Location.Y - d);
        }
        public static void StretchDown(Control ctrl, Control c, int d) {
            if (d == -1)
                d = MattyControl.Distance;
            ctrl.Size = new Size(ctrl.Width, c.Location.Y - ctrl.Location.Y - d);
        }

        // -- begin types --
        // -- Btn : Button
        public Btn(string text, Control parent) {
            this.Text = text;
            parent.Controls.Add(this);
        }
        // -- Cb : CheckBox
        public Cb(string text, Control parent) {
            this.Text = text;
            parent.Controls.Add(this);
        }
        // -- Tb : TextBox
        public Tb(Control parent) {
            parent.Controls.Add(this);
        }
        // -- RichTb : RichTextBox
        public RichTb(Control parent) {
            parent.Controls.Add(this);
        }
        // -- Lb : ListBox
        public Lb(Control parent) {
            parent.Controls.Add(this);
        }
        // -- Db : ComboBox
        public Db(Control parent) {
            parent.Controls.Add(this);
            this.DropDownStyle = ComboBoxStyle.DropDownList;
        }
        // -- Lbl : Label
        public Lbl(string text, Control parent) {
            this.Text = text;
            parent.Controls.Add(this);
        }
        // -- end types --

        // -- begin control copy --
        /// <summary>
        /// The label for this control
        /// </summary>
        public Lbl Label;

        /// <summary>
        /// Locate this control inside its parent in a specific way
        /// </summary>
        /// <param name="c">It's parent</param>
        /// <param name="h">The horizontal placement</param>
        /// <param name="v">The vertical placement</param>
        /// <param name="d">The margin (distance), use MattyControl.Distance if -1</param>
        public void LocateInside(Control c, MattyControl.Horizontal h = MattyControl.Horizontal.Left, MattyControl.Vertical v = MattyControl.Vertical.Top, int d = -1) {
            MattyControl.LocateInside(this, c, h, v, d);
        }

        /// <summary>
        /// Locate this control adjacent to the other control in a specific way
        /// </summary>
        /// <param name="c">The other control</param>
        /// <param name="h">The horizontal placement</param>
        /// <param name="v">The vertical placement</param>
        /// <param name="distance">The margin</param>
        public void LocateFrom(Control c, MattyControl.Horizontal h = MattyControl.Horizontal.CopyLeft, MattyControl.Vertical v = MattyControl.Vertical.CopyTop, int d = -1) {
            MattyControl.LocateFrom(this, c, h, v, d);
        }

        /// <summary>
        /// Add a label to this control
        /// </summary>
        /// <param name="text">The text of the label</param>
        /// <param name="d">The distance between the label and the control</param>
        /// <param name="moveCtrl">Whether the control should be moved or not</param>
        /// <param name="labelWidth">The width of the label. Set to 0 to keep the original width</param>
        public void AddLabel(string text, int d = -1, bool moveCtrl = true, int labelWidth = 0) {
            if (this.Label != null)
                this.Parent.Controls.Remove(this.Label);
            this.Label = MattyControl.AddLabel(this, text, d, moveCtrl, labelWidth);
        }

        /// <summary>
        /// Stretch the control right
        /// </summary>
        /// <param name="factor">The factor to stretch</param>
        public void StretchRight(float factor) {
            MattyControl.StretchRight(this, factor);
        }
        /// <summary>
        /// Stretch the control right
        /// </summary>
        /// <param name="parent">The parent user control</param>
        /// <param name="d">The distance between the label and the control</param>
        public void StretchRight(UserControl parent, int d = -1) {
            MattyControl.StretchRight(this, parent, d);
        }
        /// <summary>
        /// Stretch the control right
        /// </summary>
        /// <param name="c">The other control</param>
        /// <param name="d">The distance between the label and the control</param>
        public void StretchRight(Control c, int d = -1) {
            MattyControl.StretchRight(this, c, d);
        }

        /// <summary>
        /// Stretch the control down
        /// </summary>
        /// <param name="factor">The factor to stretch</param>
        public void StretchDown(float factor) {
            MattyControl.StretchDown(this, factor);
        }
        /// <summary>
        /// Stretch the control down
        /// </summary>
        /// <param name="parent">The parent user control</param>
        /// <param name="d">The distance between the label and the control</param>
        public void StretchDown(UserControl parent, int d = -1) {
            MattyControl.StretchDown(this, parent, d);
        }
        /// <summary>
        /// Stretch the control down
        /// </summary>
        /// <param name="c">The other control</param>
        /// <param name="d">The distance between the label and the control</param>
        public void StretchDown(Control c, int d = -1) {
            MattyControl.StretchDown(this, c, d);
        }
        // -- end control copy --
    }
    // -- write controls --
}
