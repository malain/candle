using System.Windows.Forms;
using DSLFactory.Candle.SystemModel.Strategies;

namespace DSLFactory.Candle.SystemModel.Utilities.SchemaDiscover
{
    /// <summary>
    /// 
    /// </summary>
    public partial class EntityNameConfirmation : Form
    {
        private readonly DataLayer _layer;

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityNameConfirmation"/> class.
        /// </summary>
        /// <param name="layer">The layer.</param>
        /// <param name="initialName">The initial name.</param>
        public EntityNameConfirmation(DataLayer layer, string initialName)
        {
            InitializeComponent();
            _layer = layer;
            txtRootName.Text = StrategyManager.GetInstance(layer.Store).NamingStrategy.ToPascalCasing(initialName);
            txtEntityName.Text =
                StrategyManager.GetInstance(layer.Store).NamingStrategy.CreateElementName(layer, txtRootName.Text);
        }

        /// <summary>
        /// Gets the name of the root.
        /// </summary>
        /// <value>The name of the root.</value>
        public string RootName
        {
            get { return txtRootName.Text; }
        }

        /// <summary>
        /// Gets the name of the entity.
        /// </summary>
        /// <value>The name of the entity.</value>
        public string EntityName
        {
            get { return txtEntityName.Text; }
        }

        /// <summary>
        /// Handles the KeyUp event of the txtRootName control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Forms.KeyEventArgs"/> instance containing the event data.</param>
        private void txtRootName_KeyUp(object sender, KeyEventArgs e)
        {
            txtEntityName.Text =
                StrategyManager.GetInstance(_layer.Store).NamingStrategy.CreateElementName(_layer, txtRootName.Text);
        }
    }
}