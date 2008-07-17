using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace DSLFactory.Candle.SystemModel.Wizard
{
    /// <summary>
    /// 
    /// </summary>
    public partial class CandleWizardForm : Form
    {
        private readonly Dictionary<string, object> _userData = new Dictionary<string, object>();

        /// <summary>
        /// 
        /// </summary>
        protected bool _canceled;

        /// <summary>
        /// 
        /// </summary>
        protected int _currentIndex;

        /// <summary>
        /// 
        /// </summary>
        protected CandleWizardPage _currentPage;

        /// <summary>
        /// 
        /// </summary>
        protected List<CandleWizardPage> _pages = new List<CandleWizardPage>();

        /// <summary>
        /// Initializes a new instance of the <see cref="CandleWizardForm"/> class.
        /// </summary>
        public CandleWizardForm()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CandleWizardForm"/> class.
        /// </summary>
        /// <param name="title">The title.</param>
        public CandleWizardForm(string title)
        {
            InitializeComponent();
            Text = title;
        }

        /// <summary>
        /// Gets a value indicating whether this instance is last page.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is last page; otherwise, <c>false</c>.
        /// </value>
        public virtual bool IsLastPage
        {
            get { return _currentIndex == _pages.Count - 1; }
        }

        /// <summary>
        /// Occurs when [wizard finished].
        /// </summary>
        public event WizardFinishedHandler WizardFinished;

        /// <summary>
        /// Occurs when [wizard canceled].
        /// </summary>
        public event EventHandler WizardCanceled;

        /// <summary>
        /// Starts this instance.
        /// </summary>
        /// <returns></returns>
        public DialogResult Start()
        {
            if (_pages.Count == 0)
                return DialogResult.Cancel;

            ActivatePage(0);
            ShowDialog();
            return _canceled ? DialogResult.Cancel : DialogResult.OK;
        }

        /// <summary>
        /// Called when [cancel].
        /// </summary>
        protected virtual void OnCancel()
        {
            if (WizardCanceled != null)
                WizardCanceled(this, new EventArgs());
        }

        /// <summary>
        /// Called when [finish].
        /// </summary>
        protected virtual void OnFinish()
        {
            if (WizardFinished != null)
                WizardFinished(this, new WizardFinishedEventArgs(_userData));
        }

        /// <summary>
        /// Activates the page.
        /// </summary>
        /// <param name="index">The index.</param>
        internal void ActivatePage(int index)
        {
            _currentIndex = index;
            _currentPage = _pages[index];

            _interiorPagePanel.Controls.Clear();
            _interiorPagePanel.Controls.Add(_currentPage);
            //  _currentPage.Dock = DockStyle.Fill;
            _headerLabel.Text = _currentPage.HeaderText;
            EnablesButtons();
        }

        /// <summary>
        /// Enableses the buttons.
        /// </summary>
        private void EnablesButtons()
        {
            EnableButton(ButtonType.Previous, _currentIndex > 0);
            EnableButton(ButtonType.Next, !IsLastPage);
            EnableButton(ButtonType.Finish, IsLastPage);
        }

        /// <summary>
        /// Does the default.
        /// </summary>
        public void DoDefault()
        {
            if (btnNext.Enabled)
                btnNext.PerformClick();
            else if (btnFinish.Enabled)
                btnFinish.PerformClick();
        }

        /// <summary>
        /// Enables the button.
        /// </summary>
        /// <param name="buttonType">Type of the button.</param>
        /// <param name="enabled">if set to <c>true</c> [enabled].</param>
        public void EnableButton(ButtonType buttonType, bool enabled)
        {
            switch (buttonType)
            {
                case ButtonType.Cancel:
                    btnCancel.Enabled = enabled;
                    break;
                case ButtonType.Finish:
                    btnFinish.Enabled = enabled;
                    break;
                case ButtonType.Next:
                    btnNext.Enabled = enabled;
                    break;
                case ButtonType.Previous:
                    btnPrevious.Enabled = enabled;
                    break;
            }
        }

        /// <summary>
        /// Handles the Click event of the btnCancel control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            SetCanceled();
            OnCancel();
            base.Close();
        }

        /// <summary>
        /// Handles the Click event of the btnFinish control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void btnFinish_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            try
            {
                if (_currentPage.OnDeactivated(true))
                {
                    OnFinish();
                    base.Close();
                }
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        /// <summary>
        /// Handles the Click event of the btnNext control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void btnNext_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            try
            {
                if (_currentPage.OnDeactivated(false))
                    ActivatePage(_currentIndex + 1);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        /// <summary>
        /// Handles the Click event of the btnPrevious control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void btnPrevious_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            try
            {
                ActivatePage(_currentIndex - 1);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        /// <summary>
        /// Sets the user data.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        public void SetUserData(string key, object value)
        {
            _userData[key] = value;
        }

        /// <summary>
        /// Gets the user data.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public T GetUserData<T>(string key)
        {
            if (_userData.ContainsKey(key))
                return (T) _userData[key];
            return default(T);
        }

        /// <summary>
        /// Adds the page.
        /// </summary>
        /// <param name="page">The page.</param>
        public void AddPage(CandleWizardPage page)
        {
            _pages.Add(page);
        }

        /// <summary>
        /// Sets the canceled.
        /// </summary>
        internal void SetCanceled()
        {
            _canceled = true;
        }
    }
}