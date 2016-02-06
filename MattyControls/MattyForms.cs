using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace MattyControls
{
    public class MattyForm : Form
    {
        private List<MattyUserControl> userControls;    // The list of usercontrols
        private MattyUserControl lastVisited;           // The last visited usercontrol (not the active one)
        private SettingsSingleton settings;             // I don't want to use the static getter inside this class, because the singleton can be subclassed. So I just use this, it works, and it's private, so ok.

        public MattyForm(Size minimumSize, SettingsSingleton settings) {
            this.userControls = new List<MattyUserControl>();
            this.lastVisited = null;
            this.settings = settings;

            this.StartPosition = FormStartPosition.Manual;
            this.Location = this.settings.Position;
            this.ClientSize = new Size(this.settings.Size);

            this.MinimumSize = minimumSize;

            // Register events
            this.LocationChanged += (o, e) => { this.settings.Position = this.Location; };
            this.ResizeEnd += onResizeEnd;
        }

        void onResizeEnd(object o, EventArgs e) {
            // Save the size to the settings
            this.settings.Size = new Point(this.ClientSize);

            // Resize the user controls
            foreach (MattyUserControl u in this.userControls) {
                u.Size = this.ClientSize;
                u.OnResize();
            }
        }

        /// <summary>
        /// Add a whole bunch of usercontrols to the form
        /// </summary>
        /// <param name="usercontrols"></param>
        public void AddUserControl(IEnumerable<MattyUserControl> usercontrols) {
            foreach (MattyUserControl u in usercontrols)
                this.AddUserControl(u);
        }
        /// <summary>
        /// Add a usercontrol to the form
        /// </summary>
        /// <param name="usercontrol"></param>
        public void AddUserControl(MattyUserControl usercontrol) {
            this.userControls.Add(usercontrol);
            usercontrol.Size = this.ClientSize;
            usercontrol.Hide();
            this.Controls.Add(usercontrol);
            usercontrol.OnResize();
        }

        /// <summary>
        /// Get the first usercontrol with a specific class T
        /// </summary>
        public T GetUserControl<T>() where T : MattyUserControl {
            return (T)this.userControls.First(u => u is T);
        }

        /// <summary>
        /// Show the first usercontrol with a specific class T (and hide all others)
        /// </summary>
        public void ShowUserControl<T>() where T : MattyUserControl {
            this.ShowUserControl(this.GetUserControl<T>());
        }
        /// <summary>
        /// Show the usercontrol (and hide all others)
        /// </summary>
        public void ShowUserControl(MattyUserControl usercontrol) {
            foreach (MattyUserControl u in this.userControls.Where(u => u.Visible)) {
                this.lastVisited = u;
                u.Hide();
            }
            usercontrol.Show();
            usercontrol.Size = this.ClientSize;
            usercontrol.OnResize();
        }
        /// <summary>
        /// Show the last visited usercontrol (and hide all others)
        /// </summary>
        public void ShowLastVisitedUserControl() {
            if (this.lastVisited != null)
                this.ShowUserControl(this.lastVisited);
        }
    }

    public class MattyUserControl : UserControl
    {
        public MattyForm ParentMattyForm {
            get { return (MattyForm)this.Parent; }
        }

        public MattyUserControl() { }

        /// <summary>
        /// Show the first usercontrol with a specific class T (and hide all others)
        /// </summary>
        public void ShowUserControl<T>() where T : MattyUserControl {
            this.ParentMattyForm.ShowUserControl<T>();
        }
        /// <summary>
        /// Show the usercontrol (and hide all others)
        /// </summary>
        public void ShowUserControl(MattyUserControl usercontrol) {
            this.ParentMattyForm.ShowUserControl(usercontrol);
        }
        /// <summary>
        /// Show the last visited usercontrol (and hide all others)
        /// </summary>
        public void ShowLastVisitedUserControl() {
            this.ParentMattyForm.ShowLastVisitedUserControl();
        }

        /// <summary>
        /// This method gets called after the usercontrol is resized
        /// </summary>
        public virtual void OnResize() { }
    }
}
