using System;
using System.Collections.Generic;
using System.Windows.Forms;
using DSLFactory.Candle.SystemModel.Configuration.VisualStudio;

namespace DSLFactory.Candle.SystemModel.Configuration
{
    /// <summary>
    /// 
    /// </summary>
    public delegate void DataValidHandler(bool isValid);

    /// <summary>
    /// Ecran de saisie des options de configuration du package
    /// </summary>
    public partial class OptionsServersPageControl : UserControl
    {
        private OptionsServersPage _optionsPage;

        /// <summary>
        /// Initializes a new instance of the <see cref="OptionsServersPageControl"/> class.
        /// </summary>
        public OptionsServersPageControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Gets or sets the options page.
        /// </summary>
        /// <value>The options page.</value>
        public OptionsServersPage OptionsPage
        {
            get { return _optionsPage; }
            set
            {
                _optionsPage = value;
                if (value != null)
                {
                    InitData();
                }
            }
        }

        /// <summary>
        /// Occurs when [data valid].
        /// </summary>
        public event DataValidHandler DataValid;

        /// <summary>
        /// Inits the data.
        /// </summary>
        internal void InitData()
        {
            chkDefaultProxy.Checked = _optionsPage.UseDefaultProxy;
            txtPassword.Text = _optionsPage.ProxyPassword;
            txtUser.Text = _optionsPage.ProxyUser;
            txtProxy.Text = _optionsPage.ProxyAddress;
            lstServers.Items.Clear();
            foreach (string url in _optionsPage.GlobalServers)
            {
                lstServers.Items.Add(url);
            }
        }

        /// <summary>
        /// Commits the changes.
        /// </summary>
        internal void CommitChanges()
        {
            _optionsPage.UseDefaultProxy = chkDefaultProxy.Checked;
            _optionsPage.ProxyPassword = txtPassword.Text;
            _optionsPage.ProxyUser = txtUser.Text;
            _optionsPage.ProxyAddress = txtProxy.Text;
            _optionsPage.GlobalServers = new List<string>();
            foreach (string item in lstServers.Items)
            {
                _optionsPage.GlobalServers.Add(item);
            }
        }

        /// <summary>
        /// Handles the Click event of the btnAddServer control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void btnAddServer_Click(object sender, EventArgs e)
        {
            lstServers.Items.Add(txtServer.Text);
            txtServer.Text = "http://";
        }

        /// <summary>
        /// Handles the KeyUp event of the lstServers control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Forms.KeyEventArgs"/> instance containing the event data.</param>
        private void lstServers_KeyUp(object sender, KeyEventArgs e)
        {
            if (lstServers.SelectedIndex >= 0)
                lstServers.Items.RemoveAt(lstServers.SelectedIndex);
        }


        /// <summary>
        /// Handles the CheckedChanged event of the chkDefaultProxy control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void chkDefaultProxy_CheckedChanged(object sender, EventArgs e)
        {
            txtProxy.Enabled = txtUser.Enabled = txtPassword.Enabled = !chkDefaultProxy.Checked;
        }
    }
}