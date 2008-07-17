using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows.Forms;
using DSLFactory.Candle.SystemModel.Repository;
using DSLFactory.Candle.SystemModel.Repository.Forms;

namespace DSLFactory.Candle.SystemModel.Rules.Wizards
{
    /// <summary>
    /// Classe permettant lors d'un import d'assembly d'associer des assemblies à un modèle existant
    /// </summary>
    public partial class ReferencedAssembliesForm : Form
    {
        private readonly List<ComponentMetadataMap> _assemblyBindings = new List<ComponentMetadataMap>();

        /// <summary>
        /// Initializes a new instance of the <see cref="ReferencedAssembliesForm"/> class.
        /// </summary>
        /// <param name="componentType">Type of the component.</param>
        protected ReferencedAssembliesForm(ComponentType? componentType)
        {
            InitializeComponent();
            repositoryTreeControl1.Populate(false, componentType, null);

            dgAssemblies.AutoGenerateColumns = false;
            ColAssemblyName.DataPropertyName = "AssemblyName";
            ColVersion.DataPropertyName = "Version";
            ColModelFileName.DataPropertyName = "Name";
        }

        /// <summary>
        /// Sélection à partir d'un modèle
        /// </summary>
        /// <param name="modelId">The model id.</param>
        /// <param name="name">The name.</param>
        /// <param name="version">The version.</param>
        /// <param name="componentType">Type of the component.</param>
        public ReferencedAssembliesForm(Guid modelId, string name, VersionInfo version, ComponentType? componentType)
            : this(componentType)
        {
            ComponentMetadataMap item = new ComponentMetadataMap();
            item.Version = version;
            item.Name = name;
            item.AssemblyName = name;
            item.InitialComponentType = componentType;
            _assemblyBindings.Add(item);
            dgAssemblies.DataSource = _assemblyBindings;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ReferencedAssembliesForm"/> class.
        /// </summary>
        /// <param name="externalAssembly">The external assembly.</param>
        /// <param name="referencedAssemblies">The referenced assemblies.</param>
        public ReferencedAssembliesForm(DotNetAssembly externalAssembly, AssemblyName[] referencedAssemblies)
            : this(ComponentType.Library)
        {
            Text += " for " + externalAssembly.Name;

            foreach (AssemblyName an in referencedAssemblies)
            {
                ComponentMetadataMap item = new ComponentMetadataMap();
                item.Version = new VersionInfo(an.Version);
                ExternalComponent externalComponent = externalAssembly.Component.Model.FindExternalComponentByName(an.Name);
                if (externalComponent == null)
                {
                    item.Name = "<select a model>";
                }
                else
                {
                    item.AlreadyExists = true;
                    item.Name = externalComponent.Name;
                    item.MetaData = externalComponent.MetaData;
                }
                item.InitialComponentType = ComponentType.Library;
                item.AssemblyName = an.Name;
                _assemblyBindings.Add(item);
            }

            dgAssemblies.DataSource = _assemblyBindings;
        }

        /// <summary>
        /// Gets the selected assembly bindings.
        /// </summary>
        /// <value>The selected assembly bindings.</value>
        public List<ComponentMetadataMap> SelectedAssemblyBindings
        {
            get { return _assemblyBindings; }
        }

        /// <summary>
        /// Handles the Click event of the buttonOK control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void buttonOK_Click(object sender, EventArgs e)
        {
            Hide();
        }

        /// <summary>
        /// Handles the ModelSelected event of the repositoryTreeControl1 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="DSLFactory.Candle.SystemModel.Repository.Forms.ModelSelectedEventArgs"/> instance containing the event data.</param>
        private void repositoryTreeControl1_ModelSelected(object sender, ModelSelectedEventArgs e)
        {
            if (dgAssemblies.SelectedRows.Count > 0)
            {
                ComponentMetadataMap data = (ComponentMetadataMap) dgAssemblies.SelectedRows[0].DataBoundItem;
                if (data != null &&
                    (data.InitialComponentType == null || data.InitialComponentType == e.Item.ComponentType))
                {
                    data.Name = e.Item.Name;
                    data.MetaData = e.Item;
                    dgAssemblies.SelectedRows[0].Cells[2].ErrorText = null;
                    dgAssemblies.Refresh();
                }
            }
        }

        /// <summary>
        /// Handles the CellValidating event of the dgAssemblies control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Forms.DataGridViewCellValidatingEventArgs"/> instance containing the event data.</param>
        private void dgAssemblies_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            if (e.ColumnIndex == 2)
            {
                DataGridViewRow row = dgAssemblies.Rows[e.RowIndex];
                ComponentMetadataMap data = (ComponentMetadataMap) row.DataBoundItem;
                row.Cells[2].ErrorText = data == null || data.MetaData == null ? "model not defined" : null;
            }
        }
    }
}