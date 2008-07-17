using System;
using System.Windows.Forms;
using DSLFactory.Candle.SystemModel.Repository.Forms;

namespace DSLFactory.Candle.SystemModel.Repository
{
    /// <summary>
    /// 
    /// </summary>
    public partial class RepositoryTreeForm : Form
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RepositoryTreeForm"/> class.
        /// </summary>
        public RepositoryTreeForm() : this(false, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RepositoryTreeForm"/> class.
        /// </summary>
        /// <param name="showCreate">if set to <c>true</c> [show create].</param>
        /// <param name="componentFilter">The component filter.</param>
        public RepositoryTreeForm(bool showCreate, ComponentType? componentFilter)
        {
            InitializeComponent();
            repositoryTree.Populate(false, componentFilter, null);
            btnCreate.Visible = showCreate;
        }

        /// <summary>
        /// Gets the selected item.
        /// </summary>
        /// <value>The selected item.</value>
        public ComponentModelMetadata SelectedItem
        {
            get { return repositoryTree.GetSelectedData(); }
        }


        /// <summary>
        /// Handles the ModelSelected event of the repositoryTree control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="DSLFactory.Candle.SystemModel.Repository.Forms.ModelSelectedEventArgs"/> instance containing the event data.</param>
        private void repositoryTree_ModelSelected( object sender, ModelSelectedEventArgs e )
        {
            btnSelect.Enabled = e.Item != null;
        }

        /// <summary>
        /// Handles the Click event of the btnSelect control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void btnSelect_Click( object sender, EventArgs e )
        {
            this.Hide();
        }

        /// <summary>
        /// Handles the Click event of the button1 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void button1_Click( object sender, EventArgs e )
        {
            this.Hide();
        }
    }
}