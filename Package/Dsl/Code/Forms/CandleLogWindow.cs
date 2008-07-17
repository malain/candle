using System;
using System.Windows.Forms;

namespace DSLFactory.Candle.SystemModel
{
    /// <summary>
    /// 
    /// </summary>
    public partial class CandleLogWindow : Form
    {
        private bool _hasErrors;
        private ListViewGroup _lastGroup;

        /// <summary>
        /// Initializes a new instance of the <see cref="CandleLogWindow"/> class.
        /// </summary>
        /// <param name="closeAuto">if set to <c>true</c> [close auto].</param>
        public CandleLogWindow(bool closeAuto)
        {
            InitializeComponent();
            Cursor = Cursors.WaitCursor;
            chkAutoClose.Checked = closeAuto;
        }

        /// <summary>
        /// Ends this instance.
        /// </summary>
        public void End()
        {
            if (!_hasErrors && chkAutoClose.Checked)
            {
                Hide();
                return;
            }

            SuspendLayout();
            btnClose.Visible = true;
            progressBar.Visible = false;
            lstMessage.Height += lstMessage.Top - progressBar.Top;
            lstMessage.Top = progressBar.Top;
            ResumeLayout();
            BringToFront();
            TopMost = true;
            WindowState = FormWindowState.Normal;
            Activate();
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Form.Shown"></see> event.
        /// </summary>
        /// <param name="e">A <see cref="T:System.EventArgs"></see> that contains the event data.</param>
        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            Cursor = Cursors.Default;
            Application.DoEvents();
        }

        /// <summary>
        /// Adds the step.
        /// </summary>
        /// <param name="msg">The MSG.</param>
        public void AddStep(string msg)
        {
            if (_lastGroup == null || _lastGroup.Name != msg)
            {
                _lastGroup = lstMessage.Groups.Add(msg, msg);
                AddText("Executing ...", LogType.Info);
            }
        }

        /// <summary>
        /// Adds the text.
        /// </summary>
        /// <param name="msg">The MSG.</param>
        /// <param name="status">The status.</param>
        public void AddText(string msg, LogType status)
        {
            ListViewItem lvi = lstMessage.Items.Add(msg);
            if (_lastGroup != null)
                lvi.Group = _lastGroup;
            lvi.ImageIndex = (int) status;
            lvi.EnsureVisible();
            Application.DoEvents();
            if (status == LogType.Error || status == LogType.Warning)
                _hasErrors = true;
        }

        /// <summary>
        /// Handles the Click event of the btnClose control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void btnClose_Click(object sender, EventArgs e)
        {
            Hide();
        }
    }
}