using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;
using DSLFactory.Candle.SystemModel.Repository;
using DSLFactory.Candle.SystemModel.Utilities;

namespace DSLFactory.Candle.SystemModel.Strategies
{
    /// <summary>
    /// Sélection d'une stratégie
    /// </summary>
    public partial class SelectStrategyDialog : Form
    {
        private ManifestCollection _manifests;

        ///// <summary>
        ///// Stocke les stratégies par groupe pour pouvoir sélectionner les regroupements
        ///// </summary>
        //private Dictionary<string, List<TreeNode>> strategyByGroups = new Dictionary<string, List<TreeNode>>();

        /// <summary>
        /// Initializes a new instance of the <see cref="SelectStrategyDialog"/> class.
        /// </summary>
        public SelectStrategyDialog()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Retourne la (ou les) stratégie sélectionnée ou null.
        /// </summary>
        public ManifestCollection SelectedStrategies
        {
            get
            {
                // Les stratégies peuvent être liées par leur nom de groupe
                ManifestCollection results = new ManifestCollection();

                // Sélection dans l'arbre
                if (rbTree.Checked)
                {
                    TreeViewWalker visitor = new TreeViewWalker(tvStrategies);

                    // Parcours des stratégies 'checkées' 
                    visitor.Traverse(delegate(TreeNode node) { if (node.Checked) AddStrategies(results, node.Tag); });

                    // Si il n'y a rien on prend la stratégie courante
                    if (results.Count == 0 && tvStrategies.SelectedNode != null)
                        AddStrategies(results, tvStrategies.SelectedNode.Tag);
                }
                else
                {
                    foreach (ListViewItem item in lvStrategies.CheckedItems)
                    {
                        AddStrategies(results, item.Tag);
                    }

                    // Si il n'y a rien on prend la stratégie courante
                    if (results.Count == 0 && lvStrategies.SelectedItems.Count == 1)
                        AddStrategies(results, lvStrategies.SelectedItems[0].Tag);
                }
                return results;
            }
        }

        /// <summary>
        /// Initialisation de la liste des stratégies à partir de celles disponibles sur le
        /// repository central + les stratégies internes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SelectStrategyDialog_Load(object sender, EventArgs e)
        {
            _manifests = StrategyManager.GetAvailableManifests();
            PopulateListView();
        }

        /// <summary>
        /// Chargement des stratégies disponibles sous forme d'arbre organisé 
        /// par StrategyPath
        /// </summary>
        private void PopulateListView()
        {
            ILogger logger = ServiceLocator.Instance.GetService<ILogger>();
            lvStrategies.Dock = DockStyle.Fill;
            lvStrategies.Visible = true;
            tvStrategies.Visible = false;

            if (_manifests == null)
                return;

            lvStrategies.Items.Clear();

            Dictionary<string, ListViewGroup> groups = new Dictionary<string, ListViewGroup>();

            //
            // Lecture de toutes les strategies disponibles
            //
            foreach (StrategyManifest manifest in _manifests)
            {
                if (!String.IsNullOrEmpty(manifest.DisplayName))
                {
                    // Ajout du noeud
                    ListViewItem item = new ListViewItem(manifest.DisplayName);
                    item.Group = FindGroup(groups, manifest.StrategyGroup);
                    item.SubItems.Add(manifest.StrategyTypeName);
                    item.SubItems.Add(manifest.PackageName ?? "Internal");
                    NodeData nodeData = new NodeData();
                    nodeData.Value = manifest;
                    nodeData.Description = manifest.Description;
                    item.Tag = nodeData;
                    item.ImageIndex = 0;
                    lvStrategies.Items.Add(item);
                }
                else
                {
                    string txt = manifest.StrategyConfiguration != null
                                     ? manifest.StrategyConfiguration.OuterXml
                                     : String.Empty;
                    if (logger != null)
                        logger.Write("Strategy manifest", String.Format("Skip manifest with no name - {0}", txt),
                                     LogType.Info);
                }
            }

            ListViewGroup[] temp = new ListViewGroup[groups.Values.Count];
            groups.Values.CopyTo(temp, 0);
            //Array.Sort(temp);
            lvStrategies.Groups.AddRange(temp);
        }

        /// <summary>
        /// Finds the group.
        /// </summary>
        /// <param name="groups">The groups.</param>
        /// <param name="strategyGroup">The strategy group.</param>
        /// <returns></returns>
        private static ListViewGroup FindGroup(Dictionary<string, ListViewGroup> groups, string strategyGroup)
        {
            ListViewGroup group = null;
            if (String.IsNullOrEmpty(strategyGroup))
                strategyGroup = "Misc";

            if (groups.TryGetValue(strategyGroup.ToLower(), out group))
                return group;

            group = new ListViewGroup(strategyGroup);
            groups.Add(strategyGroup.ToLower(), group);
            return group;
        }

        /// <summary>
        /// Chargement des stratégies disponibles sous forme d'arbre organisé 
        /// par StrategyPath
        /// </summary>
        private void PopulateTreeView()
        {
            tvStrategies.Visible = true;
            lvStrategies.Visible = false;

            if (_manifests == null)
                return;

            tvStrategies.Nodes.Clear();

            TreeNode root = new TreeNode("Strategies");
            tvStrategies.Nodes.Add(root);

            //
            // Lecture de toutes les strategies disponibles
            //
            foreach (StrategyManifest manifest in _manifests)
            {
                //
                // Ajout du noeud
                //
                TreeNode tmpNode = new TreeNode(manifest.DisplayName);
                NodeData nodeData = new NodeData();
                nodeData.Value = manifest;
                nodeData.Description = manifest.Description;
                tmpNode.Tag = nodeData;
                tmpNode.ImageIndex = 0;

                FindParentNodes(root, manifest.StrategyPath).Add(tmpNode);
            }

            root.Expand();
        }

        /// <summary>
        /// Recherche du noeud parent dans l'arbre en tenant compte du path de la stratégie
        /// </summary>
        /// <param name="root">Racine du treeview</param>
        /// <param name="strategyPath">Path de la strategie</param>
        /// <returns></returns>
        private static TreeNodeCollection FindParentNodes(TreeNode root, string strategyPath)
        {
            TreeNodeCollection nodes = root.Nodes;

            // Tri suivant en tenant compte du chemin (path) de la stratégie
            if (!String.IsNullOrEmpty(strategyPath))
            {
                string[] pathParts = strategyPath.Split('/');
                foreach (string part in pathParts)
                {
                    // Recherche du noeud parent pour chaque part
                    bool bFind = false;
                    foreach (TreeNode node in nodes)
                    {
                        if (Utils.StringCompareEquals(node.Text, part))
                        {
                            nodes = node.Nodes;
                            bFind = true;
                            break;
                        }
                    }

                    // Si pas trouvé, on le crèe
                    if (!bFind)
                    {
                        TreeNode parent = new TreeNode(part);
                        nodes.Add(parent);
                        nodes = parent.Nodes;
                        parent.ImageIndex = 1;
                    }
                }
            }
            return nodes;
        }

        /// <summary>
        /// Adds the strategies.
        /// </summary>
        /// <param name="results">The results.</param>
        /// <param name="node">The node.</param>
        private static void AddStrategies(ManifestCollection results, object node)
        {
            if (node != null)
            {
                Debug.Assert(node is NodeData);
                StrategyManifest manifest = ((NodeData) node).Value as StrategyManifest;
                if (manifest != null && !results.Contains(manifest))
                    results.Add(manifest);
            }
        }

        /// <summary>
        /// Affichage de la description
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Forms.TreeViewEventArgs"/> instance containing the event data.</param>
        private void tvStrategies_AfterSelect(object sender, TreeViewEventArgs e)
        {
            txtDescription.Text = string.Empty;
            TreeNode node = tvStrategies.SelectedNode;
            if (node != null)
            {
                NodeData data = node.Tag as NodeData;
                if (data != null)
                    txtDescription.Text = data.Description;
            }
        }

        /// <summary>
        /// Handles the NodeMouseDoubleClick event of the tvStrategies control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Forms.TreeNodeMouseClickEventArgs"/> instance containing the event data.</param>
        private void tvStrategies_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            // EN cas de double-click, on ne prend que le noeud en cours
            // donc on supprime tous les noeuds 'checkés'
            TreeViewWalker visitor = new TreeViewWalker(tvStrategies);
            visitor.Traverse(delegate(TreeNode node) { node.Checked = false; });

            btnOK.PerformClick();
        }

        /// <summary>
        /// Handles the SelectedIndexChanged event of the lvStrategies control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void lvStrategies_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtDescription.Text = string.Empty;
            if (lvStrategies.SelectedItems.Count == 1)
            {
                ListViewItem node = lvStrategies.SelectedItems[0];
                if (node != null)
                {
                    NodeData data = node.Tag as NodeData;
                    if (data != null)
                        txtDescription.Text = data.Description;
                }
            }
        }

        /// <summary>
        /// Handles the CheckedChanged event of the rbList control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void rbList_CheckedChanged(object sender, EventArgs e)
        {
            PopulateListView();
        }

        /// <summary>
        /// Handles the CheckedChanged event of the rbTree control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void rbTree_CheckedChanged(object sender, EventArgs e)
        {
            PopulateTreeView();
        }

        /// <summary>
        /// Handles the Click event of the btnGo control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void btnGo_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtUrl.Text.Length == 0)
                {
                    _manifests = StrategyManager.GetAvailableManifests();
                }
                else
                {
                    ManifestCollection tmp = new ManifestCollection();
                    tmp.AddRange(RepositoryManager.GetExternalManifests(txtUrl.Text));
                    foreach (StrategyManifest mf in tmp)
                    {
                        mf.PackageName = String.Format("{0}@{1}", mf.PackageName, txtUrl.Text);
                    }
                    _manifests = tmp;
                }

                if (rbTree.Checked)
                    PopulateTreeView();
                else
                    PopulateListView();
            }
            catch (Exception ex)
            {
                ServiceLocator.Instance.IDEHelper.ShowError(
                    String.Format("Unable to retrieve strategies from {0} - {1}", txtUrl.Text, ex.Message));
            }
        }

        #region Nested type: NodeData

        /// <summary>
        /// 
        /// </summary>
        private class NodeData
        {
            /// <summary>
            /// 
            /// </summary>
            public string Description;

            /// <summary>
            /// 
            /// </summary>
            public object Value;
        }

        #endregion
    }
}