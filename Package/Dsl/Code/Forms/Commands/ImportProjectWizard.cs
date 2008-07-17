using System;
using System.Windows.Forms;

namespace DSLFactory.Candle.SystemModel.Commands.Reverse
{
    /// <summary>
    /// 
    /// </summary>
    public partial class ImportProjectWizard : Form
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ImportProjectWizard"/> class.
        /// </summary>
        public ImportProjectWizard()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Creates the layer.
        /// </summary>
        /// <param name="component">The component.</param>
        /// <param name="package">The package.</param>
        /// <returns></returns>
        public SoftwareLayer CreateLayer(SoftwareComponent component, LayerPackage package)
        {
            SoftwareLayer layer = null;
            if (rbPres.Checked)
            {
                layer = new PresentationLayer(component.Store);
            }
            else if (rbUI.Checked)
            {
                layer = new UIWorkflowLayer(component.Store);
            }
            else if (rbDAL.Checked)
            {
                layer = new DataAccessLayer(component.Store);
            }
            else if (rbBLL.Checked)
            {
                layer = new BusinessLayer(component.Store);
            }
            else if (rbModels.Checked)
            {
                layer = new DataLayer(component.Store);
            }
            else if (rbInterface.Checked)
            {
                layer = new InterfaceLayer(component.Store);
                ((InterfaceLayer) layer).Level = (short) (package.Level + 1);
                package.InterfaceLayer = (InterfaceLayer) layer;
            }

            Layer tmp = layer as Layer;
            if (tmp != null)
            {
                package = component.LayerPackages.Find(delegate(LayerPackage p) { return p.Level == tmp.Level; });
                if (package == null)
                {
                    package = new LayerPackage(component.Store);
                    package.Level = tmp.Level;
                    component.LayerPackages.Add(package);
                }
                package.Layers.Add(tmp);
            }

            component.Layers.Add(layer);
            return layer;
        }

        /// <summary>
        /// Handles the Click event of the btnOK control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void btnOK_Click(object sender, EventArgs e)
        {
            if (rbPres.Checked || rbUI.Checked || rbDAL.Checked || rbModels.Checked || rbBLL.Checked ||
                rbInterface.Checked)
            {
                DialogResult = DialogResult.OK;
            }
            else
                MessageBox.Show("You must select a type");
        }
    }
}