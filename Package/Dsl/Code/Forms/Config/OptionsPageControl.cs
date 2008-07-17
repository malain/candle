using System;
using System.IO;
using System.Windows.Forms;
using DSLFactory.Candle.SystemModel.Configuration.VisualStudio;
using DSLFactory.Candle.SystemModel.Repository;
using DSLFactory.Candle.SystemModel.Wizard;

namespace DSLFactory.Candle.SystemModel.Configuration
{
    /// <summary>
    /// Ecran de saisie des options de configuration du package
    /// </summary>
    public partial class OptionsPageControl : UserControl
    {
        private bool _initialized = false;
        private OptionsPage _optionsPage;

        /// <summary>
        /// Initializes a new instance of the <see cref="OptionsPageControl"/> class.
        /// </summary>
        public OptionsPageControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Gets or sets the options page.
        /// </summary>
        /// <value>The options page.</value>
        public OptionsPage OptionsPage
        {
            get { return _optionsPage; }
            set
            {
                _optionsPage = value;
                if (value != null)
                {
                    _initialized = false;
                }
            }
        }

        /// <summary>
        /// Occurs when [data valid].
        /// </summary>
        public event DataValidHandler DataValid;

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.UserControl.Load"></see> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"></see> that contains the event data.</param>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (!_initialized)
            {
                InitData();
                _initialized = true;
            }
        }

        /// <summary>
        /// Inits the data.
        /// </summary>
        private void InitData()
        {
            txtDomain.Text = _optionsPage.CurrentDomainId;
            txtLicenseId.Text = _optionsPage.LicenseId;
            txtBaseDirectory.Text = _optionsPage.BaseDirectory;
            txtRepositoryPath.Text = _optionsPage.RepositoryPath;
            txtRepositoryUrl.Text = _optionsPage.RepositoryUrl;
            chkRepository.Checked = _optionsPage.RepositoryEnabled;
            txtRepositoryUrl.Enabled = chkRepository.Checked;
            numCacheDelai.Value = _optionsPage.RepositoryDelaiCache;
            chkTrace.Checked = _optionsPage.GenerationTraceEnabled;
        }

        /// <summary>
        /// Commits the changes.
        /// </summary>
        internal void CommitChanges()
        {
            _optionsPage.GenerationTraceEnabled = chkTrace.Checked;
            _optionsPage.LicenseId = txtLicenseId.Text;
            _optionsPage.BaseDirectory = txtBaseDirectory.Text;
            _optionsPage.RepositoryPath = txtRepositoryPath.Text;
            _optionsPage.RepositoryUrl = txtRepositoryUrl.Text;
            _optionsPage.RepositoryEnabled = chkRepository.Checked;
            _optionsPage.RepositoryDelaiCache = (int) numCacheDelai.Value;
            _optionsPage.CurrentDomainId = txtDomain.Text;
        }

        /// <summary>
        /// Handles the Click event of the btnBrowseConfig control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void btnBrowseConfig_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.ShowNewFolderButton = true;
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                txtBaseDirectory.Text = fbd.SelectedPath;
                txtBaseDirectory_Leave(this, new EventArgs());
            }
        }

        /// <summary>
        /// Handles the Click event of the btnBrowseRepository control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void btnBrowseRepository_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.ShowNewFolderButton = true;
            if (fbd.ShowDialog() == DialogResult.OK)
                txtRepositoryPath.Text = fbd.SelectedPath;
        }

        /// <summary>
        /// Handles the CheckedChanged event of the checkBox1 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            txtRepositoryUrl.Enabled = chkRepository.Checked;
        }

        /// <summary>
        /// Handles the Click event of the btnTest control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void btnTest_Click(object sender, EventArgs e)
        {
            try
            {
                RepositoryManager.CheckRepository(txtRepositoryUrl.Text);
                ServiceLocator.Instance.IDEHelper.ShowMessage("Success!!");
            }
            catch (Exception ex)
            {
                ServiceLocator.Instance.IDEHelper.ShowMessage(String.Format("Error - {0}", ex.Message));
            }
        }


        /// <summary>
        /// Handles the TextChanged event of the txtBaseDirectory control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void txtBaseDirectory_TextChanged(object sender, EventArgs e)
        {
            bool isValid = txtBaseDirectory.Text != null && txtBaseDirectory.Text.Length > 0 &&
                           txtBaseDirectory.Text.IndexOfAny(Path.GetInvalidPathChars()) < 0;

            if (isValid)
            {
                isValid = !chkRepository.Checked || txtRepositoryUrl.Text.Length > 0;
            }

            if (DataValid != null)
            {
                DataValid(isValid);
            }
        }

        /// <summary>
        /// Handles the Leave event of the txtBaseDirectory control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void txtBaseDirectory_Leave(object sender, EventArgs e)
        {
            bool isValid = txtBaseDirectory.Text != null && txtBaseDirectory.Text.Length > 0 &&
                           txtBaseDirectory.Text.IndexOfAny(Path.GetInvalidPathChars()) < 0;

            // Déduction du répertoire des modèles
            if (isValid && txtRepositoryPath.Text.Length == 0)
            {
                txtRepositoryPath.Text =
                    Path.Combine(txtBaseDirectory.Text, RepositoryCategory.Models.ToString());
            }
        }

        /// <summary>
        /// Handles the Click event of the btnDomainPathSelector control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void btnDomainPathSelector_Click(object sender, EventArgs e)
        {
            using (DomainPathSelector selector = new DomainPathSelector(null))
            {
                if (selector.ShowDialog() == DialogResult.OK && selector.SelectedPath != null)
                {
                    DomainItem item = selector.SelectedDomainItem;
                    if (item != null)
                    {
                        txtDomain.Text = item.Path;
                        return;
                    }
                }

                txtDomain.Text = String.Empty;
            }
        }
    }
}