using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace MattyControls
{
    public static class ControlHelpers
    {
        public static void AnchorLoop(IEnumerable<Control> controls, Action<IMattyControl> firstAction, Action<Control, IMattyControl> othersAction) {
            var controlArray = controls.ToArray();
            if (controls.Count() > 0) {
                firstAction((IMattyControl)controls.First());
            }
            for (int i = 1; i < controls.Count(); i++) {
                othersAction(controlArray[i - 1], (IMattyControl)controlArray[i]);
            }
        }

        public static void StretchControlsHorizontallyInside(UserControl parent, params Control[] controls)
            => StretchControlsHorizontallyInside(parent, controls.AsEnumerable());
        public static void StretchControlsHorizontallyInside(UserControl parent, IEnumerable<Control> controls) {
            if (controls.Count() == 0) {
                return;
            }

            const int min_width = 10;
            int width = Math.Max(min_width, (parent.Width - MattyControl.Distance - controls.Count() * MattyControl.Distance) / controls.Count());

            var first = controls.First();
            first.Location = new Point(MattyControl.Distance, first.Location.Y);
            first.Size = new Size(width, first.Height);

            AnchorLoop(controls, mc => { }, (other, me) => {
                ((Control)me).Size = other.Size;
                me.PositionRightOf(other);
            });
        }

        public static void StretchControlsVerticallyInside(UserControl parent, params Control[] controls)
            => StretchControlsVerticallyInside(parent, controls.AsEnumerable());
        public static void StretchControlsVerticallyInside(UserControl parent, IEnumerable<Control> controls) {
            if (controls.Count() == 0) {
                return;
            }

            const int min_height = 10;
            int height = Math.Max(min_height, (parent.Height - MattyControl.Distance - controls.Count() * MattyControl.Distance) / controls.Count());

            var first = controls.First();
            first.Location = new Point(first.Location.Y, MattyControl.Distance);
            first.Size = new Size(first.Width, height);

            AnchorLoop(controls, mc => { }, (other, me) => {
                ((Control)me).Size = other.Size;
                me.PositionRightOf(other);
            });
        }
    }
}
