using System;
using System.ComponentModel;
using System.Windows.Forms;
using DSLFactory.Candle.SystemModel.Dependencies;
using Microsoft.VisualStudio.Modeling;

namespace DSLFactory.Candle.SystemModel.Commands
{
    /// <summary>
    /// Sélection d'une stratégie
    /// </summary>
    public partial class ConfigurationEditorDialog : Form
    {
        private SoftwareComponent _currentComponent;
        private ItemList _items;


        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigurationEditorDialog"/> class.
        /// </summary>
        /// <param name="externalComponent">The external component.</param>
        public ConfigurationEditorDialog(ExternalComponent externalComponent)
        {
            InitializeComponent();
            Populate(null, externalComponent.ReferencedModel);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigurationEditorDialog"/> class.
        /// </summary>
        /// <param name="currentComponent">The current component.</param>
        public ConfigurationEditorDialog(SoftwareComponent currentComponent)
        {
            InitializeComponent();
            Populate(currentComponent, currentComponent.Model);
        }

        /// <summary>
        /// Populates the specified current component.
        /// </summary>
        /// <param name="currentComponent">The current component.</param>
        /// <param name="model">The model.</param>
        private void Populate(SoftwareComponent currentComponent, CandleModel model)
        {
            _currentComponent = currentComponent;
            lstInheritedConfigurations.AutoGenerateColumns = false;
            lstConfigurations.AutoGenerateColumns = false;

            if (currentComponent != null)
            {
                foreach (SoftwareLayer layer in currentComponent.Layers)
                {
                    ColLayer.Items.Add(layer.Name);
                }

                colVisibility.Items.Add(Visibility.Public);
                colVisibility.Items.Add(Visibility.Private);

                // Remplissage des configurations courantes
                _items = new ItemList();
                foreach (SoftwareLayer layer in currentComponent.Layers)
                {
                    foreach (ConfigurationPart cfg in layer.Configurations)
                    {
                        Item item = new Item();
                        item.ConfigName = cfg.Name;
                        item.XmlContent = cfg.XmlContent;
                        item.LayerName = layer.Name;
                        item.LayerId = layer.Id;
                        item.InitialEnabled = item.Enabled = cfg.Enabled;
                        item.IsExternal = false;
                        item.Visibility = cfg.Visibility;
                        _items.Add(item);
                    }
                }
                lstConfigurations.DataSource = _items;
            }
            else // On enlève cette page
                tabControl1.TabPages.RemoveAt(0);

            // Configurations externes
            ReferenceWalker walker = new ReferenceWalker(ReferenceScope.Runtime, new ConfigurationMode());
            ConfigurationVisitor visitor = new ConfigurationVisitor(true);
            walker.Traverse(visitor, model);

            lstInheritedConfigurations.DataSource =
                visitor.Configurations.FindAll(delegate(ConfigurationPart p) { return p.Enabled; });
        }

        /// <summary>
        /// Handles the Click event of the btnOK control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void btnOK_Click(object sender, EventArgs e)
        {
            if (_currentComponent == null)
                return;

            using (
                Transaction transaction =
                    _currentComponent.Store.TransactionManager.BeginTransaction("Set configuration status"))
            {
                bool flag = false;
                foreach (Item item in _items)
                {
                    if (item.InitialEnabled != item.Enabled && !item.IsExternal)
                    {
                        AbstractLayer layer =
                            (AbstractLayer) _currentComponent.Store.ElementDirectory.FindElement(item.LayerId);
                        ConfigurationPart part =
                            layer.Configurations.Find(
                                delegate(ConfigurationPart cp) { return cp.Name == item.ConfigName; });
                        if (part != null)
                        {
                            part.Visibility = item.Visibility;
                            part.Enabled = item.Enabled;
                            flag = true; // Il va falloir commiter
                        }
                    }
                }

                if (flag)
                    transaction.Commit();
            }
        }

        /// <summary>
        /// Handles the RowsAdded event of the lstConfigurations control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Forms.DataGridViewRowsAddedEventArgs"/> instance containing the event data.</param>
        private void lstConfigurations_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            lstConfigurations.Rows[e.RowIndex].ReadOnly = false;
        }

        #region Nested type: Item

        /// <summary>
        /// 
        /// </summary>
        private class Item
        {
            private string configName;
            private bool enabled;
            private bool initialEnabled;
            private bool isExternal;
            private Guid layerId;
            private string layerName;
            private Visibility visibility;
            private string xmlContent;

            /// <summary>
            /// Gets or sets the visibility.
            /// </summary>
            /// <value>The visibility.</value>
            public Visibility Visibility
            {
                get { return visibility; }
                set { visibility = value; }
            }

            /// <summary>
            /// Gets or sets the layer id.
            /// </summary>
            /// <value>The layer id.</value>
            public Guid LayerId
            {
                get { return layerId; }
                set { layerId = value; }
            }

            /// <summary>
            /// Gets or sets the name of the layer.
            /// </summary>
            /// <value>The name of the layer.</value>
            public string LayerName
            {
                get { return layerName; }
                set { layerName = value; }
            }

            /// <summary>
            /// Gets or sets the name of the config.
            /// </summary>
            /// <value>The name of the config.</value>
            public string ConfigName
            {
                get { return configName; }
                set { configName = value; }
            }

            /// <summary>
            /// Gets or sets the content of the XML.
            /// </summary>
            /// <value>The content of the XML.</value>
            public string XmlContent
            {
                get { return xmlContent; }
                set { xmlContent = value; }
            }

            /// <summary>
            /// Gets or sets a value indicating whether this <see cref="Item"/> is enabled.
            /// </summary>
            /// <value><c>true</c> if enabled; otherwise, <c>false</c>.</value>
            public bool Enabled
            {
                get { return enabled; }
                set { enabled = value; }
            }

            /// <summary>
            /// Gets or sets a value indicating whether [initial enabled].
            /// </summary>
            /// <value><c>true</c> if [initial enabled]; otherwise, <c>false</c>.</value>
            public bool InitialEnabled
            {
                get { return initialEnabled; }
                set { initialEnabled = value; }
            }

            /// <summary>
            /// Gets or sets a value indicating whether this instance is external.
            /// </summary>
            /// <value>
            /// 	<c>true</c> if this instance is external; otherwise, <c>false</c>.
            /// </value>
            public bool IsExternal
            {
                get { return isExternal; }
                set { isExternal = value; }
            }
        }

        #endregion

        #region Nested type: ItemList

        private class ItemList : BindingList<Item>
        {
        }

        #endregion
    }
}