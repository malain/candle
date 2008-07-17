using System;
using System.Windows.Forms;
using DSLFactory.Candle.SystemModel.Repository.Forms;
using Microsoft.VisualStudio.Modeling.Shell;
using Microsoft.VisualStudio.Shell.Interop;

namespace DSLFactory.Candle.SystemModel.Designer
{
    /// <summary>
    /// 
    /// </summary>
    [CLSCompliant(false)]
    public class RepositoryWindowPane : ToolWindow, IVsWindowFrameNotify
    {
        private RepositoryTreeControl _form;

        /// <summary>
        /// Initializes a new instance of the <see cref="RepositoryWindowPane"/> class.
        /// </summary>
        /// <param name="serviceProvider">The service provider that is used to retrieve shell services.</param>
        public RepositoryWindowPane(IServiceProvider serviceProvider)
            : base(serviceProvider)
        {
            // Set the image that will appear on the tab of the window frame
            // when docked with another window.
            // The resource ID corresponds to the one defined in Resources.resx,
            // while the Index is the offset in the bitmap strip. Each image in
            // the strip is 16x16.
            //this.BitmapResourceID = 301;
            //this.BitmapIndex = 1;
        }

        /// <summary>
        /// Gets the form.
        /// </summary>
        /// <value>The form.</value>
        public RepositoryTreeControl Form
        {
            get
            {
                if (_form == null) _form = new RepositoryTreeControl(true);
                return _form;
            }
        }

        /// <summary>
        /// Gets the window associated with this window pane.
        /// </summary>
        /// <value></value>
        /// <returns><see cref="T:System.Windows.Forms.IWin32Window"></see>.</returns>
        public override IWin32Window Window
        {
            get { return Form; }
        }

        #region IVsWindowFrameNotify Members

        /// <summary>
        /// Called when [dockable change].
        /// </summary>
        /// <param name="fDockable">The f dockable.</param>
        /// <returns></returns>
        public int OnDockableChange(int fDockable)
        {
            return 0;
        }

        /// <summary>
        /// Called when [move].
        /// </summary>
        /// <returns></returns>
        public int OnMove()
        {
            return 0;
        }

        /// <summary>
        /// Called when [show].
        /// </summary>
        /// <param name="fShow">The f show.</param>
        /// <returns></returns>
        public int OnShow(int fShow)
        {
            //   this.OnShowToolWindow((__FRAMESHOW)fShow);
            return 0;
        }

        /// <summary>
        /// Called when [size].
        /// </summary>
        /// <returns></returns>
        public int OnSize()
        {
            return 0;
        }

        #endregion

        /// <summary>
        /// Gets the title of the tool window.
        /// </summary>
        /// <value></value>
        /// <returns>The title of the tool window.</returns>
        public override string WindowTitle
        {
            get { return "Model Repository"; }
        }
    }
}