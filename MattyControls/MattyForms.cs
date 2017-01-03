using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace MattyControls
{
    public class MattyForm : Form
    {
        private List<MattyUserControl> userControls;
        private MattyUserControl lastVisited; // Not the active one
        private SettingsSingleton settings;   // I can't use the static getter inside this class, because the singleton can be subclassed.

        public List<MattyStatusStrip> StatusStrips { get; private set; }
        public MattyStatusStrip StatusStrip {
            get {
                if (this.StatusStrips.Count < 1)
                    throw new ArgumentOutOfRangeException("No status strips found, call this.UseStatusStrip() to create one.");
                else if (this.StatusStrips.Count > 1)
                    throw new ArgumentException("There are multiple statusstrips, use this.StatusStrips instead.");
                return this.StatusStrips.First();
            }
        }


        public MattyForm(Size minimumSize, SettingsSingleton settings)
            : this(minimumSize, minimumSize, settings) { }

        public MattyForm(Size minimumSize, Size defaultSize, SettingsSingleton settings) {
            this.MinimumSize = minimumSize;
            if (settings.Size.IsEmpty) {
                settings.Size = defaultSize;
            }

            this.userControls = new List<MattyUserControl>();
            this.lastVisited = null;
            this.settings = settings;
            this.StatusStrips = new List<MattyStatusStrip>();

            this.StartPosition = FormStartPosition.Manual;
            this.Location = this.settings.Position;
            this.ClientSize = this.settings.Size;

            // Register events
            this.LocationChanged += (o, e) => { this.settings.Position = this.Location; };
            this.ResizeEnd += onResizeEnd;
        }

        void onResizeEnd(object o, EventArgs e) {
            // Save the size to the settings
            this.settings.Size = this.ClientSize;

            this.OnResize();

            // Resize the user controls
            foreach (MattyUserControl u in this.userControls) {
                this.resizeUsercontrol(u);
            }
        }

        /// <summary>
        /// This method gets called when the form is resized
        /// </summary>
        public virtual void OnResize() { }


        // User coontrols
        /// <summary>
        /// Add a whole bunch of usercontrols to the form
        /// </summary>
        /// <param name="usercontrols"></param>
        public void AddUserControl(params MattyUserControl[] usercontrols) {
            this.AddUserControl(usercontrols as IEnumerable<MattyUserControl>);
        }
        /// <summary>
        /// Add a whole bunch of usercontrols to the form
        /// </summary>
        /// <param name="usercontrols"></param>
        public void AddUserControl(IEnumerable<MattyUserControl> usercontrols) {
            foreach (MattyUserControl u in usercontrols) {
                this.AddUserControl(u);
            }
        }
        /// <summary>
        /// Add a usercontrol to the form
        /// </summary>
        /// <param name="usercontrol"></param>
        public void AddUserControl(MattyUserControl usercontrol) {
            this.userControls.Add(usercontrol);
            usercontrol.Hide();
            this.Controls.Add(usercontrol);
            this.resizeUsercontrol(usercontrol);
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

                u.OnVisibilityChanged();
                u.OnHide();
                u.Hide();
            }

            usercontrol.Show();
            usercontrol.OnVisibilityChanged();
            usercontrol.OnShow();

            this.resizeUsercontrol(usercontrol);
        }
        /// <summary>
        /// Show the last visited usercontrol (and hide all others)
        /// </summary>
        public void ShowLastVisitedUserControl() {
            if (this.lastVisited != null) {
                this.ShowUserControl(this.lastVisited);
            }
        }

        private void resizeUsercontrol(MattyUserControl usercontrol) {
            usercontrol.Size = new Size(this.ClientSize.Width, this.ClientSize.Height - this.StatusStrips.Sum(s => s.Height));
            usercontrol.OnResize();
        }


        // Status strip
        /// <summary>
        /// Add a status strip to the form
        /// </summary>
        /// <param name="name"></param>
        public MattyStatusStrip UseStatusStrip(string name = null) {
            var statusStrip = new MattyStatusStrip();
            statusStrip.Name = name;

            this.StatusStrips.Add(statusStrip);
            this.Controls.Add(statusStrip);

            this.onResizeEnd(this, new EventArgs());

            return statusStrip;
        }
    }
}
