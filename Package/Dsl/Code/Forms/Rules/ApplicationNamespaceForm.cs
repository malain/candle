using System;
using System.Diagnostics;
using System.Windows.Forms;
using DSLFactory.Candle.SystemModel.Configuration;
using DSLFactory.Candle.SystemModel.Repository;
using DSLFactory.Candle.SystemModel.Strategies;
using DSLFactory.Candle.SystemModel.Wizard;

namespace DSLFactory.Candle.SystemModel.Rules.Wizards
{
    /// <summary>
    /// 
    /// </summary>
    public partial class ApplicationNamespaceForm : Form
    {
        private readonly SoftwareComponent _component;

        /// <summary>
        /// Coinstructeur
        /// </summary>
        /// <param name="component">The component.</param>
        public ApplicationNamespaceForm(SoftwareComponent component)
        {
            Debug.Assert(component != null);

            _component = component;
            InitializeComponent();

            string txt = component.Namespace;
            if (String.IsNullOrEmpty(txt) && component.Model != null)
            {
                txt = component.Model.Name;
            }

            if (txt == "?")
            {
                txt = string.Empty;
                try
                {
                    if (component.Model != null)
                        txt = StrategyManager.GetInstance(component.Store).NamingStrategy.DefaultNamespace;
                }
                catch
                {
                    txt = String.Empty;
                }

                try
                {
                    if (txt != null && txt.Length > 0)
                        txt += '.';
                    txt += (string) ServiceLocator.Instance.ShellHelper.Solution.Properties.Item(9).Value;
                }
                catch
                {
                    txt = String.Empty;
                }
            }
            txtNamespace.Text = txt;

            UpdateApplicationName();

            if (component.Model != null)
            {
                ckLibrary.Checked = component.Model.IsLibrary;
                VersionInfo version = component.Model.Version;
                if (version != null)
                {
                    txtVersionBuild.Value = version.Build > 0 ? version.Build : 0;
                    txtVersionMajor.Value = version.Major > 0 ? version.Major : 0;
                    txtVersionMinor.Value = version.Minor > 0 ? version.Minor : 0;
                    txtVersionRevision.Value = version.Revision > 0 ? version.Revision : 0;
                }
            }
            txtDomainPath.Text = component.Model.Path;
            txtDescription.Text = component.Comment;
        }

        /// <summary>
        /// Gets the namespace.
        /// </summary>
        /// <value>The namespace.</value>
        public string Namespace
        {
            get { return txtNamespace.Text; }
        }

        /// <summary>
        /// Gets the domain path.
        /// </summary>
        /// <value>The domain path.</value>
        public string DomainPath
        {
            get { return txtDomainPath.Text; }
        }

        /// <summary>
        /// Gets the name of the application.
        /// </summary>
        /// <value>The name of the application.</value>
        public string ApplicationName
        {
            get { return txtApplicationName.Text; }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is library.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is library; otherwise, <c>false</c>.
        /// </value>
        public bool IsLibrary
        {
            get { return ckLibrary.Checked; }
        }

        /// <summary>
        /// Gets the URL.
        /// </summary>
        /// <value>The URL.</value>
        public string URL
        {
            get { return txtURL.Text; }
        }

        /// <summary>
        /// Gets the description.
        /// </summary>
        /// <value>The description.</value>
        public string Description
        {
            get { return txtDescription.Text; }
        }

        /// <summary>
        /// Gets the version.
        /// </summary>
        /// <value>The version.</value>
        public VersionInfo Version
        {
            get
            {
                VersionInfo vi =
                    new VersionInfo((int) txtVersionMajor.Value, (int) txtVersionMinor.Value,
                                    (int) txtVersionBuild.Value, (int) txtVersionRevision.Value);
                return vi;
            }
        }

        /// <summary>
        /// Handles the KeyUp event of the txtNamespace control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Forms.KeyEventArgs"/> instance containing the event data.</param>
        private void txtNamespace_KeyUp(object sender, KeyEventArgs e)
        {
            UpdateApplicationName();
        }

        /// <summary>
        /// Updates the name of the application.
        /// </summary>
        private void UpdateApplicationName()
        {
            txtApplicationName.Text = "";
            int pos = txtNamespace.Text.LastIndexOf('.');
            if (pos > 0)
                txtApplicationName.Text = txtNamespace.Text.Substring(pos + 1);
            else
                txtApplicationName.Text = txtNamespace.Text;
        }

        //internal void InitProperties( RepositoryItem repositoryItem )
        //{
        //    txtNamespace.Text = repositoryItem.Name;
        //    UpdateApplicationName();
        //    txtVersionBuild.Value = repositoryItem.Version.Revision;
        //    txtVersionMajor.Value = repositoryItem.Version.Major;
        //    txtVersionMinor.Value = repositoryItem.Version.Minor;
        //    txtDescription.Text = repositoryItem.Description;
        //}

        /// <summary>
        /// Validates the data.
        /// </summary>
        /// <returns></returns>
        private bool ValidateData()
        {
            bool error = false;
            errors.Clear();

            if (!StrategyManager.GetInstance(_component.Store).NamingStrategy.IsClassNameValid(txtApplicationName.Text))
            {
                errors.SetError(txtApplicationName, "Invalid name");
                error = true;
            }
            else if (RepositoryManager.Instance.ModelsMetadata.Metadatas.NameExists(txtApplicationName.Text))
            {
                errors.SetError(txtApplicationName,
                                "Invalid name. This name is already used. You must provide an unique appplication name.");
                error = true;
            }

            if (txtDomainPath.Text.Length == 0)
            {
                errors.SetError(txtDomainPath, "The domain path is mandatory.");
                error = true;
            }

            if (!StrategyManager.GetInstance(_component.Store).NamingStrategy.IsNamespaceValid(txtNamespace.Text))
            {
                errors.SetError(txtNamespace, "Invalid Namespace");
                error = true;
            }
            return !error;
        }

        /// <summary>
        /// Handles the Click event of the btnDomainPathSelector control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void btnDomainPathSelector_Click(object sender, EventArgs e)
        {
            using (DomainPathSelector selector = new DomainPathSelector(CandleSettings.CurrentDomainId))
            {
                if (selector.ShowDialog() == DialogResult.OK && selector.SelectedPath != null)
                {
                    DomainItem item = selector.SelectedDomainItem;
                    if (item != null)
                    {
                        txtDomainPath.Text = item.Path;
                    }
                    else
                    {
                        txtDomainPath.Text = String.Empty;
                    }
                }
            }
        }

        /// <summary>
        /// Handles the Click event of the buttonOK control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void buttonOK_Click(object sender, EventArgs e)
        {
            if (ValidateData())
            {
                DialogResult = DialogResult.OK;
                Hide();
            }
        }

        /// <summary>
        /// Shows the help on focus.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void ShowHelpOnFocus(object sender, EventArgs e)
        {
            lbHelp.Text = String.Empty;
            if (sender == txtNamespace)
                lbHelp.Text =
                    "Saisissez le namespace qui servira à générer les namespaces de toutes les couches du composant.";

            if (sender == txtApplicationName)
                lbHelp.Text =
                    "Saisisssez le nom du composant.\r\nAttention, il doit être unique pour tout le référentiel.\r\nPensez à distinguer le nom du composant serveur et client.";

            if (sender == txtDescription)
                lbHelp.Text = "Saisissez la description qui sera affichée dans le référentiel";
            if (sender == txtDomainPath)
                lbHelp.Text =
                    "Saisissez l'arborescence de publication dans le référentiel.\r\nVous pouvez saisir n'importe quel chemin (sous la forme xxx/xxx/...) ou choisissez un chemin préinitialisé";
            if (sender == ckLibrary)
                lbHelp.Text =
                    "Indique si le composant sera une library, c'est à dire un ensemble de bibliothéque qui seront référencée de manière statique dans d'autres composants.";
            if (sender == txtURL)
                lbHelp.Text =
                    "Si elle existe, vous pouvez saisir ici, une adresse http permettant d'afficher une page de documentation du projet.";
        }
    }
}