using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using EnvDTE;

namespace DSLFactory.Candle.SystemModel.Commands
{
    /// <summary>
    /// Sélection d'une stratégie
    /// </summary>
    public partial class ArtifactEditorDialog : Form
    {
        private readonly AbstractLayer _layer;
        private Artifact _currentArtifact;

        /// <summary>
        /// Initializes a new instance of the <see cref="ArtifactEditorDialog"/> class.
        /// </summary>
        /// <param name="layer">The layer.</param>
        public ArtifactEditorDialog(AbstractLayer layer)
        {
            _layer = layer;
            InitializeComponent();
            pnlActions.Location = pnlEdition.Location;
            PopulateList();
        }

        /// <summary>
        /// Populates the list.
        /// </summary>
        protected void PopulateList()
        {
            lstArtifacts.DataSource = _layer.Artifacts;
            pnlActions.Visible = true;
            pnlEdition.Visible = false;
        }

        /// <summary>
        /// Sélection d'un artifact, on se met en modif
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void lstArtifacts_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstArtifacts.SelectedItem is Artifact)
            {
                InitEdition((Artifact) lstArtifacts.SelectedItem);
            }
        }

        /// <summary>
        /// Inits the edition.
        /// </summary>
        /// <param name="artifact">The artifact.</param>
        private void InitEdition(Artifact artifact)
        {
            _currentArtifact = artifact;
            txtFilePath.Text = string.Empty;
            rbAssembly.Checked = false;
            rbGAC.Checked = false;
            rbFile.Checked = true;
            lstScope.Items.Clear();
            errorProvider.Clear();

            if (artifact != null)
            {
                txtFilePath.Text = artifact.FileName ?? String.Empty;
                rbAssembly.Checked = artifact.Type == ArtifactType.Assembly;
                rbFile.Checked = artifact.Type == ArtifactType.Content;
                rbGAC.Checked = artifact.Type == ArtifactType.AssemblyInGac;
                rbProject.Checked = artifact.Type == ArtifactType.Project;
                cbMode.SelectedItem = artifact.ConfigurationMode;

                if (rbProject.Checked)
                    txtFilePath.Text = txtFilePath.Text + " (" + artifact.InitialFileName + ")";

                foreach (ReferenceScope val in Enum.GetValues(typeof (ReferenceScope)))
                {
                    if (val != ReferenceScope.None && val != ReferenceScope.All)
                        lstScope.Items.Add(Enum.GetName(typeof (ReferenceScope), val), (artifact.Scope & val) == val);
                }
            }
            else
            {
                foreach (ReferenceScope val in Enum.GetValues(typeof (ReferenceScope)))
                {
                    if (val != ReferenceScope.None)
                        lstScope.Items.Add(Enum.GetName(typeof (ReferenceScope), val), false);
                }
                cbMode.SelectedItem = "*";
            }

            pnlActions.Visible = false;
            pnlEdition.Visible = true;
        }

        /// <summary>
        /// Ajout d'un artifact
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void btnAdd_Click(object sender, EventArgs e)
        {
            InitEdition(null);
        }

        /// <summary>
        /// Annulation des modifs
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void btnCancelUpdate_Click(object sender, EventArgs e)
        {
            pnlActions.Visible = true;
            pnlEdition.Visible = false;
        }

        /// <summary>
        /// Validation des modifs
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void btnValidate_Click(object sender, EventArgs e)
        {
            if (txtFilePath.Text.Length == 0)
            {
                errorProvider.SetError(txtFilePath, "The file path is required");
                return;
            }
            int scope = 0;
            foreach (string name in lstScope.CheckedItems)
            {
                ReferenceScope val = (ReferenceScope) Enum.Parse(typeof (ReferenceScope), name);
                scope += (int) val;
            }
            if (scope == 0)
            {
                errorProvider.SetError(lstScope, "You must select at least one scope");
                return;
            }

            // Type projet : On vérifie qu'il existe
            string assemblyName = null;
            if (rbProject.Checked)
            {
                int pos = txtFilePath.Text.IndexOf('(');
                if (pos > 1)
                    txtFilePath.Text = txtFilePath.Text.Substring(0, pos - 1);
                Project prj = ServiceLocator.Instance.GetService<IShellHelper>().FindProjectByName(txtFilePath.Text);
                if (prj == null)
                {
                    errorProvider.SetError(txtFilePath, "Project not found in the current solution");
                    return;
                }
                else
                {
                    try
                    {
                        assemblyName = (string) prj.Properties.Item("OutputFileName").Value;
                    }
                    catch
                    {
                        errorProvider.SetError(txtFilePath, "Invalid project type");
                        return;
                    }
                }
            }

            if (_currentArtifact == null)
            {
                _currentArtifact = new Artifact(_layer.Store);
                _layer.Artifacts.Add(_currentArtifact);
            }

            // Quand on vient de resaisir un path, on reforce le chemin initial
            if (Path.IsPathRooted(txtFilePath.Text))
                _currentArtifact.InitialFileName = txtFilePath.Text;
            if (assemblyName != null)
            {
                _currentArtifact.InitialFileName = assemblyName;
            }

            _currentArtifact.FileName = Path.GetFileName(txtFilePath.Text);
            _currentArtifact.Type = rbAssembly.Checked
                                        ? ArtifactType.Assembly
                                        : rbGAC.Checked
                                              ? ArtifactType.AssemblyInGac
                                              : rbProject.Checked ? ArtifactType.Project : ArtifactType.Content;
            _currentArtifact.ConfigurationMode = cbMode.SelectedItem.ToString();
            _currentArtifact.Scope = (ReferenceScope) scope;

            if (_currentArtifact.Type == ArtifactType.Project)
                _currentArtifact.FileName = Path.GetFileName(_currentArtifact.FileName);

            PopulateList();
        }

        /// <summary>
        /// Affichage de la boite de dialogue de sélection de fichier
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void btnSelectFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.CheckFileExists = true;
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                txtFilePath.Text = ofd.FileName;
                if (Utils.StringCompareEquals(Path.GetExtension(txtFilePath.Text), ".dll"))
                    rbAssembly.Checked = true;
            }
        }

        /// <summary>
        /// Suppression d'un artifact
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void btnRemove_Click(object sender, EventArgs e)
        {
            if (lstArtifacts.SelectedItem is Artifact)
            {
                Artifact artifact = lstArtifacts.SelectedItem as Artifact;
                _layer.Artifacts.Remove(artifact);
                PopulateList();
            }
        }

        /// <summary>
        /// Handles the Load event of the ArtifactEditorDialog control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void ArtifactEditorDialog_Load(object sender, EventArgs e)
        {
            cbMode.Items.Add("*");

            List<string> names = new List<string>();
            foreach (
                SolutionConfiguration conf in
                    ServiceLocator.Instance.ShellHelper.Solution.SolutionBuild.SolutionConfigurations)
            {
                if (!names.Contains(conf.Name))
                {
                    cbMode.Items.Add(conf.Name);
                    names.Add(conf.Name);
                }
            }
        }

        /// <summary>
        /// Handles the Enter event of the txtFilePath control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void txtFilePath_Enter(object sender, EventArgs e)
        {
            int pos = txtFilePath.Text.IndexOf('(');
            if (pos > 1)
                txtFilePath.Text = txtFilePath.Text.Substring(0, pos - 1);
        }
    }
}