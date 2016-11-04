using System.Windows.Forms;

namespace MattyControls
{
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

        /// <summary>
        /// This method gets called after the usercontrol is shown
        /// </summary>
        public virtual void OnShow() { }

        /// <summary>
        /// This method gets called before the usercontrol is hidden
        /// </summary>
        public virtual void OnHide() { }

        /// <summary>
        /// This method gets called before the usercontrol is hidden or after the usercontrol is shown (before show or hide)
        /// </summary>
        public virtual void OnVisibilityChanged() { }
    }
}
