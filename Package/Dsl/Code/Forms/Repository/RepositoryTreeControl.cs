using System;
using System.ComponentModel;
using System.Windows.Forms;
using DSLFactory.Candle.SystemModel.Commands;
using DSLFactory.Candle.SystemModel.Configuration;
using DSLFactory.Candle.SystemModel.Configuration.VisualStudio;

namespace DSLFactory.Candle.SystemModel.Repository.Forms
{
    /// <summary>
    /// Controle affichant les metadatas des référentiels avec possibilité de les manipuler (refresh)
    /// </summary>
    public partial class RepositoryTreeControl : UserControl
    {
        private readonly BackgroundWorker _worker = new BackgroundWorker();
        private string _filterName = null;
        private ComponentType? _filterType = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="RepositoryTreeControl"/> class.
        /// </summary>
        public RepositoryTreeControl() : this(false)
        {
        }

        /// <summary>
        /// Constructeur
        /// </summary>
        /// <param name="canDragAndDrop">if set to <c>true</c> [can drag and drop].</param>
        public RepositoryTreeControl(bool canDragAndDrop)
        {
            InitializeComponent();

#if DEBUG
            if( ServiceLocator.IsDesignMode )
                return;
#endif
            // Activation du disposer
            components.Add(new InternalDisposer(CustomDispose));

            _worker.DoWork += worker_DoWork;
            _worker.RunWorkerCompleted += worker_RunWorkerCompleted;

            if (canDragAndDrop)
            {
                _menu = new ContextMenu();
                _treeCtrl.ContextMenu = _menu;
                _menu.Popup += menu_Popup;

                _removeLocalMenuItem = new MenuItem();
                _removeLocalMenuItem.Text = "Remove local";
                _removeLocalMenuItem.Click += removeLocal_Click;

                _showDocumentationMenuItem = new MenuItem();
                _showDocumentationMenuItem.Text = "Show documentation";
                _showDocumentationMenuItem.Click += showDocumentation_Click;
                _showDocumentationMenuItem.Popup += showDocumentationMenuItem_Popup;

                _propertiesMenuItem_ = new MenuItem();
                _propertiesMenuItem_.Text = "Properties";
                _propertiesMenuItem_.Click += properties_Click;

                _getLastVersionMenuItem = new MenuItem();
                _getLastVersionMenuItem.Text = "Get last version";
                _getLastVersionMenuItem.Click += getLastVersionMenuItem_Click;

                _publishMenuItem = new MenuItem();
                _publishMenuItem.Text = "Publish";
                _publishMenuItem.Click += publish_Click;
                _menu.MenuItems.AddRange(new MenuItem[] {_showDocumentationMenuItem, _removeLocalMenuItem, _publishMenuItem, _propertiesMenuItem_, _getLastVersionMenuItem});

                _treeCtrl.ItemDrag += treeCtrl_ItemDrag;
                _treeCtrl.NodeMouseClick += treeCtrl_NodeMouseClick;
                _treeCtrl.NodeMouseDoubleClick += treeCtrl_NodeMouseDoubleClick;
            }

            _treeCtrl.AfterSelect += treeCtrl_AfterSelect;

            RefreshTreeView();

            RepositoryManager.Instance.ModelsMetadata.Metadatas.MetadataChanged += ModelsMetadata_MetadataChanged;

            // Abonnements aux changements des options
            ICandleNotifier notifier = ServiceLocator.Instance.GetService<ICandleNotifier>();
            if (notifier != null)
                notifier.OptionsChanged += CandleOptionsChanged;
        }

        /// <summary>
        /// Occurs when [model selected].
        /// </summary>
        public event EventHandler<ModelSelectedEventArgs> ModelSelected;

        /// <summary>
        /// Customs the dispose.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void CustomDispose(object sender, EventArgs e)
        {
            // Code personnalisé
            RepositoryManager.Instance.ModelsMetadata.Metadatas.MetadataChanged -= ModelsMetadata_MetadataChanged;

            // Abonnements aux changements des options
            ICandleNotifier notifier = ServiceLocator.Instance.GetService<ICandleNotifier>();
            if (notifier != null)
                notifier.OptionsChanged -= CandleOptionsChanged;
        }

        /// <summary>
        /// Mise à jour de l'affichage
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ModelsMetadata_MetadataChanged(object sender, MetadataChangedEventArg e)
        {
            if (_treeCtrl.Nodes.Count > 0 && e.Metadata != null)
                PopulateNode(_treeCtrl.Nodes[0], e.Metadata);
        }

        /// <summary>
        /// La configuration de Candle a changé, on réaffiche tout.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CandleOptionsChanged(object sender, EventArgs e)
        {
            RefreshTreeView();
        }

        /// <summary>
        /// Click sur le nom du serveur, on affiche sa page d'accueil
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void lbConnected_Click(object sender, EventArgs e)
        {
            ToolStripLabel lb = sender as ToolStripLabel;
            if (lb != null)
                if (lb.IsLink)
                    System.Diagnostics.Process.Start("IEXPLORE.EXE", lb.Tag.ToString());
        }

        /// <summary>
        /// Création de l'arborescence
        /// </summary>
        /// <param name="forceUpdate">if set to <c>true</c> [force update].</param>
        /// <param name="componentFilter">The component filter.</param>
        /// <param name="nameFilter">The name filter.</param>
        internal void Populate(bool forceUpdate, ComponentType? componentFilter, string nameFilter)
        {
            if (_worker.IsBusy)
                return;

            if (OptionsPage.Instance == null)
                return; // design mode

            _filterName = nameFilter;
            _filterType = componentFilter;

            _treeCtrl.Nodes.Clear();
            _treeCtrl.Nodes.Add("Loading...");

            _worker.RunWorkerAsync(forceUpdate);
        }

        /// <summary>
        /// Lecture du repository en arrière plan
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.ComponentModel.DoWorkEventArgs"/> instance containing the event data.</param>
        private static void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            bool forceUpdate = (bool) e.Argument;

            try
            {
                if (forceUpdate)
                    RepositoryManager.Instance.ModelsMetadata.Metadatas.Clear();
                e.Result = RepositoryManager.Instance.ModelsMetadata.Metadatas.LoadAll();
            }
            catch
            {
            }
        }

        /// <summary>
        /// Handles the RunWorkerCompleted event of the worker control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.ComponentModel.RunWorkerCompletedEventArgs"/> instance containing the event data.</param>
        private void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            MetadataCollection metadatas = (MetadataCollection) e.Result;
            PopulateTreeView(metadatas);
        }

        /// <summary>
        /// Populates the tree view.
        /// </summary>
        /// <param name="metadatas">The metadatas.</param>
        private void PopulateTreeView(MetadataCollection metadatas)
        {
            lbConnected.Text = "[No remote repository]";
            lbConnected.IsLink = false;
            IRepositoryProvider p = RepositoryManager.Instance.GetMainRemoteProvider();
            if (p != null)
            {
                lbConnected.Text = "Remote repository at " + p.Name;
                lbConnected.IsLink = true;
                lbConnected.Tag = p.Name;
            }

            // Racine
            _treeCtrl.BeginUpdate();
            _treeCtrl.Nodes.Clear();

            try
            {
                TreeNode tvRoot = new TreeNode("Repository");
                tvRoot.ImageIndex = tvRoot.SelectedImageIndex = 3;
                _treeCtrl.Nodes.Add(tvRoot);

                if (metadatas != null)
                {
                    string currentDomain = CandleSettings.CurrentDomainId;

                    foreach (ComponentModelMetadata data in metadatas)
                    {
                        // Filtre sur les types
                        if (_filterType != null && data.ComponentType != _filterType)
                            continue;
                        // Filtre sur les noms
                        if (!String.IsNullOrEmpty(_filterName) && !Utils.StringStartsWith(data.Name, _filterName))
                            continue;
                        // Filtre sur le domaine
                        if (!String.IsNullOrEmpty(currentDomain) && data.Visibility != Visibility.Public
                            && !Utils.StringStartsWith(data.Path, currentDomain))
                            continue;
                        PopulateNode(tvRoot, data);
                    }
                    tvRoot.Expand();
                }
            }
            finally
            {
                _treeCtrl.EndUpdate();
                btnRefresh.Enabled = true;
                btnSelect.Enabled = true;
            }
        }

        /// <summary>
        /// Remplissage d'un noeud de l'arbre
        /// </summary>
        /// <param name="tvRoot">The tv root.</param>
        /// <param name="data">The data.</param>
        private static void PopulateNode(TreeNode tvRoot, ComponentModelMetadata data)
        {
            // Recherche de l'arborescence dans l'arbre actuel
            string[] pathParts = data.Path.Split(DomainManager.PathSeparator);
            TreeNode currentNode = tvRoot;
            TreeNode tmpNode;
            TreeNode[] tmp;

            // Arborescence
            foreach (string part in pathParts)
            {
                tmp = currentNode.Nodes.Find(part, false);
                // Pas trouvé
                if (tmp == null || tmp.Length == 0)
                {
                    tmpNode = new TreeNode(part);
                    tmpNode.Name = part;
                    tmpNode.SelectedImageIndex = tmpNode.ImageIndex = 3;
                    currentNode.Nodes.Add(tmpNode);
                }
                else // trouvé
                    tmpNode = tmp[0];

                currentNode = tmpNode;
            }

            // Ajout si nécessaire du nom de l'appli
            tmp = currentNode.Nodes.Find(data.Name, false);
            if (tmp == null || tmp.Length == 0)
            {
                tmpNode = new TreeNode(data.Name);
                tmpNode.Name = data.Name;
                tmpNode.SelectedImageIndex = tmpNode.ImageIndex = 3;
                currentNode.Nodes.Add(tmpNode);
            }
            else
                tmpNode = tmp[0];
            currentNode = tmpNode;

            // Puis ajout du noeud terminal : N° de version            
            tmp = currentNode.Nodes.Find(data.Version.ToString(3), false);
            if (tmp == null || tmp.Length == 0)
            {
                tmpNode = new TreeNode();
                currentNode.Nodes.Add(tmpNode);
            }
            else
                tmpNode = tmp[0];
            currentNode = tmpNode;

            SetComponentNode(data, currentNode);
        }

        /// <summary>
        /// Initialisation du noeud terminal
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="tvNode">The tv node.</param>
        private static void SetComponentNode(ComponentModelMetadata data, TreeNode tvNode)
        {
            tvNode.Name = data.Version.ToString(3);
            tvNode.Text = String.Format("{0} ({1})", data.Version.ToString(), data.Location);
            tvNode.SelectedImageIndex = tvNode.ImageIndex = data.IsLastVersion() ? 4 : 0;

            if (!String.IsNullOrEmpty(data.Description))
            {
                if (data.Description.Length > 50)
                    tvNode.Text = String.Format("{0} - {1}...", tvNode.Text, data.Description.Substring(0, 50));
                else
                    tvNode.Text += " - " + data.Description;
            }

            tvNode.Tag = data;
            tvNode.ToolTipText = data.Description;
        }

        /// <summary>
        /// Handles the Click event of the btnRefresh control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void btnRefresh_Click(object sender, EventArgs e)
        {
            RefreshTreeView();
        }

        /// <summary>
        /// Refreshes the tree view.
        /// </summary>
        private void RefreshTreeView()
        {
            btnRefresh.Enabled = false;
            btnSelect.Enabled = false;

            componentTypeLibrary.Checked = componentTypeSoftware.Checked = true;
            _filterType = null;
            txtFilterName.Text = String.Empty;
            _filterName = null;

            Populate(true, null, null);
        }

        /// <summary>
        /// Handles the Click event of the btnSelect control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void btnSelect_Click(object sender, EventArgs e)
        {
            btnRefresh.Enabled = false;
            btnSelect.Enabled = false;
            _filterType = null;
            if (componentTypeLibrary.Checked && !componentTypeSoftware.Checked)
                _filterType = ComponentType.Library;
            if (!componentTypeLibrary.Checked && componentTypeSoftware.Checked)
                _filterType = ComponentType.Component;
            _filterName = txtFilterName.Text;
            PopulateTreeView(RepositoryManager.Instance.ModelsMetadata.Metadatas);
        }

        #region Actions

        /// <summary>
        /// Handles the Popup event of the menu control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void menu_Popup(object sender, EventArgs e)
        {
            ComponentModelMetadata item = GetSelectedData();
            _propertiesMenuItem_.Visible = item != null;
            _showDocumentationMenuItem.Visible = item != null;
            _showDocumentationMenuItem.Enabled = item != null && !String.IsNullOrEmpty(item.DocUrl);
            _getLastVersionMenuItem.Visible = item != null;
            _publishMenuItem.Visible = _removeLocalMenuItem.Visible = item != null && item.Location == RepositoryLocation.Local;
        }

        /// <summary>
        /// Handles the Click event of the getLastVersionMenuItem control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void getLastVersionMenuItem_Click(object sender, EventArgs e)
        {
            TreeNode node = _treeCtrl.SelectedNode;
            ComponentModelMetadata metaData = GetSelectedData();
            GetLastVersionCommand command = new GetLastVersionCommand(metaData, true);
            command.Exec();
            if (node != null) node.EnsureVisible();
        }

        /// <summary>
        /// Handles the Popup event of the showDocumentationMenuItem control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void showDocumentationMenuItem_Popup(object sender, EventArgs e)
        {
            ComponentModelMetadata item = GetSelectedData();
            _showDocumentationMenuItem.Enabled = item != null && !String.IsNullOrEmpty(item.DocUrl);
        }

        /// <summary>
        /// Handles the Click event of the showDocumentation control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void showDocumentation_Click(object sender, EventArgs e)
        {
            ComponentModelMetadata item = GetSelectedData();
            ShowDocumentationCommand cmd = new ShowDocumentationCommand(item);
            if (cmd.Enabled)
                cmd.Exec();
        }

        /// <summary>
        /// Handles the Click event of the properties control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void properties_Click(object sender, EventArgs e)
        {
            ComponentModelMetadata item = GetSelectedData();
            RepositoryPropertiesForm dlg = new RepositoryPropertiesForm(item);
            dlg.ShowDialog();
        }

        /// <summary>
        /// Gets the selected data.
        /// </summary>
        /// <returns></returns>
        public ComponentModelMetadata GetSelectedData()
        {
            return _treeCtrl.SelectedNode != null ? _treeCtrl.SelectedNode.Tag as ComponentModelMetadata : null;
        }

        /// <summary>
        /// Publication
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void publish_Click(object sender, EventArgs e)
        {
            TreeNode node = _treeCtrl.SelectedNode;
            ComponentModelMetadata item = GetSelectedData();
            if (item != null)
            {
                ModelLoader loader = ModelLoader.GetLoader(item.GetFileName(PathKind.Absolute), true);
                if (loader != null && loader.Model != null)
                {
                    RepositoryManager.Instance.ModelsMetadata.PublishLocalModel(loader.Model);
                    if (node != null) node.EnsureVisible();
                }
            }
        }

        /// <summary>
        /// Suppression locale
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void removeLocal_Click(object sender, EventArgs e)
        {
            TreeNode node = _treeCtrl.SelectedNode;
            ComponentModelMetadata item = GetSelectedData();
            if (item != null)
            {
                RepositoryManager.Instance.ModelsMetadata.RemoveModelInLocalRepository(item);
                if (item.ExistsOnServer)
                {
                    if (node != null)
                        node.EnsureVisible();
                }
                else
                    node.Remove();
            }
        }

        /// <summary>
        /// Handles the NodeMouseClick event of the treeCtrl control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Forms.TreeNodeMouseClickEventArgs"/> instance containing the event data.</param>
        private void treeCtrl_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                TreeNode node = _treeCtrl.GetNodeAt(e.X, e.Y);
                _treeCtrl.SelectedNode = node;
                node.EnsureVisible();
            }
        }

        /// <summary>
        /// Handles the NodeMouseDoubleClick event of the treeCtrl control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Forms.TreeNodeMouseClickEventArgs"/> instance containing the event data.</param>
        private static void treeCtrl_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Node.Tag != null && e.Node.Parent != null)
            {
                ComponentModelMetadata item = (ComponentModelMetadata) e.Node.Tag;
                ExternalComponentShapeHelper.ShowModel(item);
            }
        }

        /// <summary>
        /// Handles the ItemDrag event of the treeCtrl control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Forms.ItemDragEventArgs"/> instance containing the event data.</param>
        private void treeCtrl_ItemDrag(object sender, ItemDragEventArgs e)
        {
            TreeNode node = e.Item as TreeNode;
            if (node != null && node.Tag != null && node.Parent != null)
            {
                ComponentModelMetadata item = (ComponentModelMetadata) node.Tag;
                RepositoryDataDragDrop data = new RepositoryDataDragDrop(item);
                DoDragDrop(data, DragDropEffects.Copy);
            }
        }

        /// <summary>
        /// Called when [model selected].
        /// </summary>
        /// <param name="repositoryItem">The repository item.</param>
        private void OnModelSelected(ComponentModelMetadata repositoryItem)
        {
            if (ModelSelected != null)
                ModelSelected(this, new ModelSelectedEventArgs(repositoryItem));
        }


        /// <summary>
        /// Handles the AfterSelect event of the treeCtrl control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Forms.TreeViewEventArgs"/> instance containing the event data.</param>
        private void treeCtrl_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (e.Node.Tag != null && e.Node.Parent != null)
            {
                ComponentModelMetadata item = (ComponentModelMetadata) e.Node.Tag;
                OnModelSelected(item);
            }
        }

        #endregion

        #region Nested type: InternalDisposer

        /// <summary>
        /// Astuce pour détruire des resources dans le dispose
        /// (Car le dispose est dèjà défini dans le designer de la classe)
        /// Ajout de <code>this.components.Add(new InternalDisposer());</code> aprés le InitializeComponent()
        /// </summary>
        private class InternalDisposer : System.ComponentModel.Component
        {
            private readonly EventHandler _callback;

            /// <summary>
            /// Initializes a new instance of the <see cref="InternalDisposer"/> class.
            /// </summary>
            /// <param name="callback">The callback.</param>
            public InternalDisposer(EventHandler callback)
            {
                _callback = callback;
            }

            /// <summary>
            /// Releases the unmanaged resources used by the <see cref="T:System.ComponentModel.Component"></see> and optionally releases the managed resources.
            /// </summary>
            /// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
            protected override void Dispose(bool disposing)
            {
                base.Dispose(disposing);
                _callback(this, new EventArgs());
            }
        }

        #endregion

        #region Nested type: RepositoryDataDragDrop

        /// <summary>
        /// Classe pour le drag and drop
        /// </summary>
        public class RepositoryDataDragDrop
        {
            private readonly ComponentModelMetadata _metaData;

            /// <summary>
            /// Initializes a new instance of the <see cref="RepositoryDataDragDrop"/> class.
            /// </summary>
            /// <param name="metaData">The meta data.</param>
            public RepositoryDataDragDrop(ComponentModelMetadata metaData)
            {
                _metaData = metaData;
            }

            /// <summary>
            /// Gets the meta data.
            /// </summary>
            /// <value>The meta data.</value>
            public ComponentModelMetadata MetaData
            {
                get { return _metaData; }
            }
        }

        #region Menu

        private readonly MenuItem _getLastVersionMenuItem;
        private readonly ContextMenu _menu;
        private readonly MenuItem _propertiesMenuItem_;
        private readonly MenuItem _publishMenuItem;
        private readonly MenuItem _removeLocalMenuItem;
        private readonly MenuItem _showDocumentationMenuItem;

        #endregion

        #endregion
    }

    /// <summary>
    /// Classe d'évenement pour la sélection d'un modèle
    /// </summary>
    public class ModelSelectedEventArgs : EventArgs
    {
        private readonly ComponentModelMetadata _item;

        /// <summary>
        /// Initializes a new instance of the <see cref="ModelSelectedEventArgs"/> class.
        /// </summary>
        /// <param name="repositoryItem">The repository item.</param>
        public ModelSelectedEventArgs(ComponentModelMetadata repositoryItem)
        {
            _item = repositoryItem;
        }

        /// <summary>
        /// Gets the item.
        /// </summary>
        /// <value>The item.</value>
        public ComponentModelMetadata Item
        {
            get { return _item; }
        }
    }
}