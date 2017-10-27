using System.Windows.Forms;

namespace MattyControls
{
    public interface IMattyControl
    {
        void LocateInside(Control parent, MattyControl.Horizontal h = MattyControl.Horizontal.Left, MattyControl.Vertical v = MattyControl.Vertical.Top, int d = -1);
        void LocateFrom(Control other, MattyControl.Horizontal h = MattyControl.Horizontal.CopyLeft, MattyControl.Vertical v = MattyControl.Vertical.CopyTop, int d = -1);

        void AddLabel(string text, bool moveCtrl = true, int labelWidth = 0, int d = -1);

        // -- foreach name in ['RightOf', 'LeftOf', 'Above', 'Below'] --
        void Position{{name}}(Control other, int d = -1);
        // -- endforeach --

        // -- foreach name in ['TopRight', 'TopLeft', 'BottomRight', 'BottomLeft'] --
        void Position{{name}}Inside(Control parent, int d = -1);
        // -- endforeach --

        // -- foreach name in ['Right', 'Down', 'Left', 'Up'] --
        void Stretch{{name}}Factor(float factor);
        void Stretch{{name}}Fixed(int amount);
        void Stretch{{name}}Inside(Control parent, int d = -1);
        void Stretch{{name}}To(Control other, int d = -1);

        // -- endforeach --
        // -- foreach name in ['Right', 'Down', 'Left', 'Up'] --
        void Move{{name}}(int amount);
        // -- endforeach --
    }
}
