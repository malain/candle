using System;
using System.Windows.Forms;

namespace DSLFactory.Candle.SystemModel.Repository
{
    /// <summary>
    /// 
    /// </summary>
    public partial class RepositoryPropertiesForm : Form
    {
        private readonly ComponentModelMetadata _metadata;

        /// <summary>
        /// Initializes a new instance of the <see cref="RepositoryPropertiesForm"/> class.
        /// </summary>
        /// <param name="metadata">The metadata.</param>
        public RepositoryPropertiesForm(ComponentModelMetadata metadata)
        {
            _metadata = metadata;
            InitializeComponent();
        }

        /// <summary>
        /// Handles the Load event of the RepositoryPropertiesForm control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void RepositoryPropertiesForm_Load(object sender, EventArgs e)
        {
            txtComponentType.Text = _metadata.ComponentType.ToString();
            txtDescription.Text = _metadata.Description;
            txtId.Text = _metadata.Id.ToString();
            txtLocalPath.Text = _metadata.GetFileName(PathKind.Absolute);
            txtModelFileName.Text = _metadata.ModelFileName;
            txtName.Text = _metadata.Name;
            txtRepositoryPath.Text = _metadata.Path;
            txtVersion.Text = _metadata.Version.ToString();
            txtDoc.Text = _metadata.DocUrl;

            txtTestAddress.Text = _metadata.TestBaseAddress;
            txtServerVersion.Text = _metadata.ServerVersion.ToString();
            txtLocalVersion.Text = _metadata.LocalVersion.ToString();
            txtOrigin.Text = _metadata.Location.ToString();
            txtServerAddress.Text = _metadata.ServerUrl;
        }
    }
}