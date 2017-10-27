using System;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;

namespace MattyControls
{
    public class MattyControl : Control
    {
        public static int Distance = 10;
        public static int LabelWidth = 100;
        public enum Horizontal { Left, CopyLeft, Center, CopyRight /* No pun intended */, Right };
        public enum Vertical { Top, CopyTop, Middle, CopyBottom, Bottom };

        // The static methods that actually do the work
        public static void LocateInside(Control ctrl, Control parentCtrl, MattyControl.Horizontal h, MattyControl.Vertical v, int d = -1) {
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

        public static void LocateFrom(Control ctrl, Control other, MattyControl.Horizontal h, MattyControl.Vertical v, int d = -1) {
            if (d == -1)
                d = MattyControl.Distance;
            int x = 0;
            int y = 0;

            if (h == Horizontal.Left)
                x = other.Location.X - ctrl.Size.Width - d;
            if (h == Horizontal.CopyLeft)
                x = other.Location.X;
            if (h == Horizontal.Center)
                x = other.Location.X + (other.Size.Width - ctrl.Size.Width) / 2;
            if (h == Horizontal.CopyRight)
                x = other.Location.X + other.Size.Width - ctrl.Size.Width;
            if (h == Horizontal.Right)
                x = other.Location.X + other.Size.Width + d;

            if (v == Vertical.Top)
                y = other.Location.Y - ctrl.Size.Height - d;
            if (v == Vertical.CopyTop)
                y = other.Location.Y;
            if (v == Vertical.Middle)
                y = other.Location.Y + (other.Size.Height - ctrl.Size.Height) / 2;
            if (v == Vertical.CopyBottom)
                y = other.Location.Y + other.Size.Height - ctrl.Size.Height;
            if (v == Vertical.Bottom)
                y = other.Location.Y + other.Size.Height + d;

            ctrl.Location = new Point(x, y);
        }

        public static Lbl AddLabel(Control ctrl, string text, bool moveCtrl, int labelWidth, int d = -1) {
            // Create a new label
            Lbl label = new Lbl(text, ctrl.Parent);

            // Set its width
            if (labelWidth != 0)
                label.Size = new Size(labelWidth, label.Height);

            label.AutoSize = true;
            label.MaximumSize = new Size(label.Size.Width, 0);
            label.TextAlign = ContentAlignment.MiddleLeft;

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

        // Quick positioning relative to other controls
        public static void PositionRightOf(Control ctrl, Control other, int d = -1) {
            if (d == -1)
                d = MattyControl.Distance;
            LocateFrom(ctrl, other, MattyControl.Horizontal.Right, MattyControl.Vertical.CopyTop, d);
        }
        public static void PositionLeftOf(Control ctrl, Control other, int d = -1) {
            if (d == -1)
                d = MattyControl.Distance;
            LocateFrom(ctrl, other, MattyControl.Horizontal.Left, MattyControl.Vertical.CopyTop, d);
        }
        public static void PositionBelow(Control ctrl, Control other, int d = -1) {
            if (d == -1)
                d = MattyControl.Distance;
            LocateFrom(ctrl, other, MattyControl.Horizontal.CopyLeft, MattyControl.Vertical.Bottom, d);
        }
        public static void PositionAbove(Control ctrl, Control other, int d = -1) {
            if (d == -1)
                d = MattyControl.Distance;
            LocateFrom(ctrl, other, MattyControl.Horizontal.CopyLeft, MattyControl.Vertical.Top, d);
        }

        // Quick positioning inside the parent
        public static void PositionTopLeftInside(Control ctrl, Control parent, int d = -1) {
            if (d == -1)
                d = MattyControl.Distance;
            LocateInside(ctrl, parent, MattyControl.Horizontal.Left, MattyControl.Vertical.Top, d);
        }
        public static void PositionTopRightInside(Control ctrl, Control parent, int d = -1) {
            if (d == -1)
                d = MattyControl.Distance;
            LocateInside(ctrl, parent, MattyControl.Horizontal.Right, MattyControl.Vertical.Top, d);
        }
        public static void PositionBottomLeftInside(Control ctrl, Control parent, int d = -1) {
            if (d == -1)
                d = MattyControl.Distance;
            LocateInside(ctrl, parent, MattyControl.Horizontal.Left, MattyControl.Vertical.Bottom, d);
        }
        public static void PositionBottomRightInside(Control ctrl, Control parent, int d = -1) {
            if (d == -1)
                d = MattyControl.Distance;
            LocateInside(ctrl, parent, MattyControl.Horizontal.Right, MattyControl.Vertical.Bottom, d);
        }

        // Stretch right
        public static void StretchRightFactor(Control ctrl, float factor) {
            ctrl.Size = new Size((int)(ctrl.Size.Width * factor), ctrl.Size.Height);
        }
        public static void StretchRightFixed(Control ctrl, int amount) {
            ctrl.Size = new Size(ctrl.Size.Width + amount, ctrl.Height);
        }
        public static void StretchRightInside(Control ctrl, Control parent, int d = -1) {
            if (d == -1)
                d = MattyControl.Distance;
            ctrl.Size = new Size(parent.ClientSize.Width - ctrl.Location.X - d, ctrl.Size.Height);
        }
        public static void StretchRightTo(Control ctrl, Control other, int d = -1) {
            if (d == -1)
                d = MattyControl.Distance;
            ctrl.Size = new Size(other.Location.X - ctrl.Location.X - d, ctrl.Size.Height);
        }

        // Stretch down
        public static void StretchDownFactor(Control ctrl, float factor) {
            ctrl.Size = new Size(ctrl.Size.Width, (int)(ctrl.Size.Height * factor));
        }
        public static void StretchDownFixed(Control ctrl, int amount) {
            ctrl.Size = new Size(ctrl.Size.Width, ctrl.Size.Height + amount);
        }
        public static void StretchDownInside(Control ctrl, Control parent, int d = -1) {
            if (d == -1)
                d = MattyControl.Distance;
            ctrl.Size = new Size(ctrl.Size.Width, parent.ClientSize.Height - ctrl.Location.Y - d);
        }
        public static void StretchDownTo(Control ctrl, Control other, int d = -1) {
            if (d == -1)
                d = MattyControl.Distance;
            ctrl.Size = new Size(ctrl.Size.Width, other.Location.Y - ctrl.Location.Y - d);
        }

        // Stretch left
        public static void StretchLeftFactor(Control ctrl, float factor) {
            int oldWidth = ctrl.Size.Width;
            StretchRightFactor(ctrl, factor);
            MoveLeft(ctrl, ctrl.Size.Width - oldWidth);
        }
        public static void StretchLeftFixed(Control ctrl, int amount) {
            StretchRightFixed(ctrl, amount);
            MoveLeft(ctrl, amount);
        }
        public static void StretchLeftInside(Control ctrl, Control parent, int d = -1) {
            if (d == -1)
                d = MattyControl.Distance;
            int oldWidth = ctrl.Size.Width;
            ctrl.Size = new Size(ctrl.Location.X + ctrl.Size.Width - d, ctrl.Size.Height);
            MoveLeft(ctrl, ctrl.Size.Width - oldWidth);
        }
        public static void StretchLeftTo(Control ctrl, Control other, int d = -1) {
            if (d == -1)
                d = MattyControl.Distance;
            int oldWidth = ctrl.Size.Width;
            ctrl.Size = new Size(ctrl.Location.X + ctrl.Size.Width - other.Location.X - other.Size.Width - d, ctrl.Size.Height);
            MoveLeft(ctrl, ctrl.Size.Width - oldWidth);
        }

        // Stretch up
        public static void StretchUpFactor(Control ctrl, float factor) {
            int oldHeight = ctrl.Size.Height;
            StretchDownFactor(ctrl, factor);
            MoveUp(ctrl, ctrl.Size.Height - oldHeight);
        }
        public static void StretchUpFixed(Control ctrl, int amount) {
            StretchDownFixed(ctrl, amount);
            MoveUp(ctrl, amount);
        }
        public static void StretchUpInside(Control ctrl, Control parent, int d = -1) {
            if (d == -1)
                d = MattyControl.Distance;
            int oldHeight = ctrl.Size.Height;
            ctrl.Size = new Size(ctrl.Size.Width, ctrl.Location.Y + ctrl.Size.Height - d);
            MoveUp(ctrl, ctrl.Size.Height - oldHeight);
        }
        public static void StretchUpTo(Control ctrl, Control other, int d = -1) {
            if (d == -1)
                d = MattyControl.Distance;
            int oldHeight = ctrl.Size.Height;
            ctrl.Size = new Size(ctrl.Size.Width, ctrl.Location.Y + ctrl.Size.Height - other.Location.Y - other.Size.Height - d);
            MoveUp(ctrl, ctrl.Size.Height - oldHeight);
        }

        // Move
        public static void MoveRight(Control ctrl, int amount) {
            ctrl.Location = new Point(ctrl.Location.X + amount, ctrl.Location.Y);
        }
        public static void MoveLeft(Control ctrl, int amount) {
            MoveRight(ctrl, -amount);
        }
        public static void MoveDown(Control ctrl, int amount) {
            ctrl.Location = new Point(ctrl.Location.X, ctrl.Location.Y + amount);
        }
        public static void MoveUp(Control ctrl, int amount) {
            MoveDown(ctrl, -amount);
        }

        // -- begin types --
        // -- Btn : Button
        public Btn(string text, Control parent) {
            this.Text = text;
            parent.Controls.Add(this);
        }
        public Btn(string text, Control parent, EventHandler onButtonClick) {
            this.Text = text;
            this.Click += onButtonClick;
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
        /// <param name="parent">It's parent</param>
        /// <param name="h">The horizontal placement</param>
        /// <param name="v">The vertical placement</param>
        /// <param name="d">The margin (distance), use MattyControl.Distance if -1</param>
        public void LocateInside(Control parent, MattyControl.Horizontal h = MattyControl.Horizontal.Left, MattyControl.Vertical v = MattyControl.Vertical.Top, int d = -1) {
            MattyControl.LocateInside(this, parent, h, v, d);
        }

        /// <summary>
        /// Locate this control adjacent to the other control in a specific way
        /// </summary>
        /// <param name="c">The other control</param>
        /// <param name="h">The horizontal placement</param>
        /// <param name="v">The vertical placement</param>
        /// <param name="d">The margin (distance), use MattyControl.Distance if -1</param>
        public void LocateFrom(Control other, MattyControl.Horizontal h = MattyControl.Horizontal.CopyLeft, MattyControl.Vertical v = MattyControl.Vertical.CopyTop, int d = -1) {
            MattyControl.LocateFrom(this, other, h, v, d);
        }

        /// <summary>
        /// Add a label to this control
        /// </summary>
        /// <param name="text">The text of the label</param>
        /// <param name="d">The distance between the label and the control</param>
        /// <param name="moveCtrl">Whether the control should be moved or not</param>
        /// <param name="labelWidth">The width of the label. Set to 0 to keep the original width</param>
        public void AddLabel(string text, bool moveCtrl = true, int labelWidth = 0, int d = -1) {
            if (this.Label != null)
                this.Parent.Controls.Remove(this.Label);
            this.Label = MattyControl.AddLabel(this, text, moveCtrl, labelWidth, d);
        }

        // -- foreach name, description in [('RightOf', 'right of'), ('LeftOf', 'left of'), ('Above', 'above'), ('Below', 'below')] --
        /// <summary>
        /// Position the control to the {{description}} the other control
        /// </summary>
        /// <param name="parent">The parent user control</param>
        /// <param name="d">The distance between the parent's border and the control</param>
        public void Position{{name}}(Control other, int d = -1) {
            MattyControl.Position{{name}}(this, other, d);
        }
        // -- endforeach --

        // -- foreach name, description in [('TopRight', 'top right'), ('TopLeft', 'top left'), ('BottomRight', 'bottom right'), ('BottomLeft', 'bottom left')] --
        /// <summary>
        /// Position the control in the {{description}}
        /// </summary>
        /// <param name="parent">The parent user control</param>
        /// <param name="d">The distance between the parent's border and the control</param>
        public void Position{{name}}Inside(Control parent, int d = -1) {
            MattyControl.Position{{name}}Inside(this, parent, d);
        }
        // -- endforeach --

        // -- foreach name, description in [('Right', 'to the right'), ('Down', 'downwards'), ('Left', 'to the left'), ('Up', 'upwards')] --
        /// <summary>
        /// Stretch the control {{description}}
        /// </summary>
        /// <param name="factor">The factor to stretch</param>
        public void Stretch{{name}}Factor(float factor) {
            MattyControl.Stretch{{name}}Factor(this, factor);
        }
        /// <summary>
        /// Stretch the control {{description}}
        /// </summary>
        /// <param name="amount">The amount to stretch</param>
        public void Stretch{{name}}Fixed(int amount) {
            MattyControl.Stretch{{name}}Fixed(this, amount);
        }
        /// <summary>
        /// Stretch the control {{description}}
        /// </summary>
        /// <param name="parent">The parent user control</param>
        /// <param name="d">The distance between the label and the control</param>
        public void Stretch{{name}}Inside(Control parent, int d = -1) {
            MattyControl.Stretch{{name}}Inside(this, parent, d);
        }
        /// <summary>
        /// Stretch the control {{description}}
        /// </summary>
        /// <param name="c">The other control</param>
        /// <param name="d">The distance between the label and the control</param>
        public void Stretch{{name}}To(Control other, int d = -1) {
            MattyControl.Stretch{{name}}To(this, other, d);
        }

        // -- endforeach --
        // -- foreach name, description in [('Right', 'to the right'), ('Down', 'downwards'), ('Left', 'to the left'), ('Up', 'upwards')] --
        /// <summary>
        /// Move the control {{description}}
        /// </summary>
        /// <param name="amount">The number of pixels to move</param>
        public void Move{{name}}(int amount) {
            MattyControl.Move{{name}}(this, amount);
        }
        // -- endforeach --
        // -- end control copy --
    }
    // -- write controls --
}
