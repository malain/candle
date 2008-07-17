using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Windows.Forms;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Shell;
using Microsoft.VisualStudio.Shell.Interop;

namespace DSLFactory.Candle.SystemModel.Designer
{
    /// <summary>
    /// 
    /// </summary>
    [CLSCompliant(false)]
    public class PortWindowPane : ToolWindow, IVsWindowFrameNotify
    {
        private OperationsDesignerForm _form;
        private IMonitorSelectionService monitorSelection;

        /// <summary>
        /// Initializes a new instance of the <see cref="PortWindowPane"/> class.
        /// </summary>
        /// <param name="serviceProvider">The service provider that is used to retrieve shell services.</param>
        public PortWindowPane(IServiceProvider serviceProvider)
            : base(serviceProvider)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Microsoft.VisualStudio.Modeling.Shell.ToolWindow"></see> class.
        /// </summary>
        protected override void Initialize()
        {
            base.Initialize();
            monitorSelection = GetService(typeof (IMonitorSelectionService)) as IMonitorSelectionService;
            if (monitorSelection != null)
            {
                monitorSelection.SelectionChanged += MonitorSelection_SelectionChanged;
            }
        }

        /// <summary>
        /// Releases the unmanaged resources used by the <see cref="T:Microsoft.VisualStudio.Modeling.Shell.ToolWindow"></see> and optionally releases the managed resources.
        /// </summary>
        /// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
        protected override void Dispose(bool disposing)
        {
            if (monitorSelection != null)
            {
                monitorSelection.SelectionChanged -= MonitorSelection_SelectionChanged;
                if (monitorSelection is IDisposable)
                {
                    IDisposable disposable = monitorSelection as IDisposable;
                    if (disposable != null) disposable.Dispose();
                }
                monitorSelection = null;
            }
            base.Dispose(disposing);
        }

        /// <summary>
        /// Gets the form.
        /// </summary>
        /// <value>The form.</value>
        public OperationsDesignerForm Form
        {
            get
            {
                if (_form == null)
                {
                    _form = new OperationsDesignerForm();
                    _form.Font = base.ToolWindowFont;
                }
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
            //this.OnShowToolWindow((__FRAMESHOW)fShow);
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
        /// Handles the SelectionChanged event of the MonitorSelection control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="Microsoft.VisualStudio.Modeling.Shell.MonitorSelectionEventArgs"/> instance containing the event data.</param>
        private void MonitorSelection_SelectionChanged(object sender, MonitorSelectionEventArgs e)
        {
            if (((e.NewValue != null) && (e.NewValue != this)) && (e.NewValue is ISelectionService))
            {
                ISelectionService selectionService = e.NewValue as ISelectionService;
                OnMonitorSelectionChanged(selectionService);
            }
        }

        /// <summary>
        /// Called when [monitor selection changed].
        /// </summary>
        /// <param name="selection">The selection.</param>
        protected virtual void OnMonitorSelectionChanged(ISelectionService selection)
        {
            if (selection.PrimarySelection != null)
            {
                SetSelection(selection.PrimarySelection as ModelElement);
            }
        }

        /// <summary>
        /// Sets the selection.
        /// </summary>
        /// <param name="mel">The mel.</param>
        public void SetSelection(ModelElement mel)
        {
            if (Form.SetSelection(mel))
                ShowNoActivate();
        }

        /// <summary>
        /// Notifies derived classes if a change is made in the Document Window.
        /// </summary>
        /// <param name="oldView">The view for the Document Window in the previous frame.</param>
        /// <param name="newView">The view for the Document Window in the current frame.</param>
        protected override void OnDocumentWindowChanged(ModelingDocView oldView, ModelingDocView newView)
        {
            base.OnDocumentWindowChanged(oldView, newView);

            if (((newView != null) && (newView.DocData != null)))
            {
                Store store = newView.DocData.Store;
                IList<SoftwareComponent> list = store.ElementDirectory.FindElements<SoftwareComponent>(true);
                if (list.Count > 0)
                {
                    //this.Form.Populate(list[0]);
                    Form.Show();
                }
                return;
            }

            SetSelection(null);
        }

        /// <summary>
        /// Gets the title of the tool window.
        /// </summary>
        /// <value></value>
        /// <returns>The title of the tool window.</returns>
        public override string WindowTitle
        {
            get { return "Details"; }
        }
    }
}