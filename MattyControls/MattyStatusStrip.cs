using System.Drawing;
using System.Windows.Forms;

namespace MattyControls
{
    public class MattyStatusStrip : StatusStrip
    {
        /// <summary>
        /// Add a label to the status strip
        /// </summary>
        /// <param name="displayStyle"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public ToolStripStatusLabel AddLabel(ToolStripItemDisplayStyle displayStyle) {
            return this.AddLabel(displayStyle, null, null);
        }
        /// <summary>
        /// Add a label to the status strip
        /// </summary>
        /// <param name="text"></param>
        /// <param name="width"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public ToolStripStatusLabel AddLabel(string text = "", int width = 0) {
            return AddLabel(null, text, width);
        }
        /// <summary>
        /// Add a label to the status strip
        /// </summary>
        /// <param name="image"></param>
        /// <param name="text"></param>
        /// <param name="width"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public ToolStripStatusLabel AddLabel(Image image, string text, int width = 0) {
            var displayStyle = ToolStripItemDisplayStyle.ImageAndText;
            if (image == null)
                displayStyle = ToolStripItemDisplayStyle.Text;
            if (text == null)
                displayStyle = ToolStripItemDisplayStyle.Image;

            return this.AddLabel(displayStyle, image, text, width);
        }
        /// <summary>
        /// Add a label to the status strip
        /// </summary>
        /// <param name="displayStyle"></param>
        /// <param name="image"></param>
        /// <param name="text"></param>
        /// <param name="width"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public ToolStripStatusLabel AddLabel(ToolStripItemDisplayStyle displayStyle, Image image, string text, int width = 0) {
            var label = new ToolStripStatusLabel {
                Text = text,
                Image = image,
                DisplayStyle = displayStyle
            };
            if (width != 0)
                label.Width = width;

            this.Items.Add(label);

            return label;
        }

        /// <summary>
        /// Add a progress bar to the status strip
        /// </summary>
        /// <returns></returns>
        public ToolStripProgressBar AddProgressBar() {
            var progressBar = new ToolStripProgressBar();

            this.Items.Add(progressBar);

            return progressBar;
        }
    }
}
