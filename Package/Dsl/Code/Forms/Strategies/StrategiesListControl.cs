using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Microsoft.VisualStudio.Modeling;

namespace DSLFactory.Candle.SystemModel.Strategies
{
    /// <summary>
    /// Fenetre listant les stratégies d'un modèle
    /// </summary>
    public partial class StrategiesListControl : UserControl
    {
        private bool _initialize;
        private Store _store;
        private CandleElement _strategiesOwner;

        /// <summary>
        /// Initializes a new instance of the <see cref="StrategiesListControl"/> class.
        /// </summary>
        public StrategiesListControl()
        {
            InitializeComponent();
            lvStrategies.Dock = DockStyle.Fill;
            tvStrategies.Visible = false;
        }

        /// <summary>
        /// Occurs when [strategy removed].
        /// </summary>
        public event EventHandler<StrategyRemovedEventArgs> StrategyRemoved;

        /// <summary>
        /// Initializes the specified store.
        /// </summary>
        /// <param name="store">The store.</param>
        /// <param name="strategiesOwner">The strategies owner.</param>
        public void Initialize(Store store, CandleElement strategiesOwner)
        {
            this._store = store;
            Populate(strategiesOwner, null);
        }

        /// <summary>
        /// Called when [selection changed].
        /// </summary>
        /// <param name="strategy">The strategy.</param>
        private void OnSelectionChanged(StrategyBase strategy)
        {
            if (strategy == null)
            {
                txtComment.Text = String.Empty;
                propGrid.SelectedObject = null;
                return;
            }

            // On affiche sa description
            txtComment.Text = strategy.Description;
            // On initialise la fenetre des propriétes
            propGrid.SelectedObject = strategy;
        }

        /// <summary>
        /// Ajout d'une stratégie
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void btnAdd_Click(object sender, EventArgs e)
        {
            SelectStrategyDialog dlg = new SelectStrategyDialog();
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                foreach (StrategyManifest manifest in dlg.SelectedStrategies)
                {
                    // On ne rajoute la stratégie que si elle n'y est pas dèjà
                    StrategyBase strategy = StrategyManager.GetInstance(_store).AddStrategy(_strategiesOwner, manifest);
                    if (strategy != null)
                        Populate(_strategiesOwner, strategy);
                }
            }
        }

        /// <summary>
        /// Affichage de l'url de l'aide si disponible
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Forms.TreeNodeMouseClickEventArgs"/> instance containing the event data.</param>
        private void tvStrategies_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
        }

        /// <summary>
        /// Suppression d'une stratégie
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void btnRemove_Click(object sender, EventArgs e)
        {
            StrategyBase strategy = SelectedStrategy;
            if (strategy != null && ServiceLocator.Instance.IDEHelper.Confirm())
            {
                StrategyManager.GetInstance(_store).RemoveStrategy(_strategiesOwner, strategy);

                propGrid.SelectedObject = null;
                Populate(_strategiesOwner, null);

                OnStrategyRemoved(
                    new StrategyRemovedEventArgs(strategy, StrategyManager.GetStrategyOwnerName(_strategiesOwner)));
            }
        }

        /// <summary>
        /// Raises the <see cref="E:StrategyRemovedEventArgs"/> event.
        /// </summary>
        /// <param name="e">The <see cref="DSLFactory.Candle.SystemModel.Strategies.StrategyRemovedEventArgs"/> instance containing the event data.</param>
        public virtual void OnStrategyRemoved(StrategyRemovedEventArgs e)
        {
            if (StrategyRemoved != null)
                StrategyRemoved(this, e);
        }

        /// <summary>
        /// Commits the changes.
        /// </summary>
        /// <returns></returns>
        public bool CommitChanges()
        {
            IList<StrategyBase> strategies = GetSelectedValues();

            Dictionary<String, StrategyBase> keys = new Dictionary<string, StrategyBase>();
            foreach (StrategyBase strategy in strategies)
            {
                if (keys.ContainsKey(strategy.StrategyId))
                {
                    MessageBox.Show(String.Format("Duplicate strategy id {0}", strategy.DisplayName));
                    return false;
                }
                char[] invalidChars = new char[] {'<', '/', '\\', '>', '[', ']', '!'};
                if (strategy.StrategyId.IndexOfAny(invalidChars) >= 0)
                {
                    MessageBox.Show(String.Format("Incorrect format name for {0}", strategy.DisplayName));
                    return false;
                }

                string err = strategy.CommitChanges();
                if (err != null)
                {
                    MessageBox.Show(String.Format("{0} Error : {1}", strategy.DisplayName, err));
                    return false;
                }

                keys.Add(strategy.StrategyId, null);
            }
            return true;
        }

        /// <summary>
        /// Handles the PropertyValueChanged event of the propGrid control.
        /// </summary>
        /// <param name="s">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Forms.PropertyValueChangedEventArgs"/> instance containing the event data.</param>
        private void propGrid_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            if (e.ChangedItem.PropertyDescriptor.Name == "StrategyId")
            {
                //TODO avertissement
            }
        }

        #region Commun

        /// <summary>
        /// Gets the selected strategy.
        /// </summary>
        /// <value>The selected strategy.</value>
        private StrategyBase SelectedStrategy
        {
            get
            {
                if (lvStrategies.SelectedItems.Count > 0)
                    return lvStrategies.SelectedItems[0].Tag as StrategyBase;
                if (tvStrategies.SelectedNode != null)
                    return tvStrategies.SelectedNode.Tag as StrategyBase;
                return null;
            }
        }

        /// <summary>
        /// Populates the specified strategies owner.
        /// </summary>
        /// <param name="strategiesOwner">The strategies owner.</param>
        /// <param name="selectedStrategy">The selected strategy.</param>
        private void Populate(CandleElement strategiesOwner, StrategyBase selectedStrategy)
        {
            PopulateListView(strategiesOwner, selectedStrategy);
        }

        /// <summary>
        /// Retourne la liste des stratégies actives
        /// </summary>
        /// <returns></returns>
        public IList<StrategyBase> GetSelectedValues()
        {
            return GetSelectedValuesFromListView();
        }

        /// <summary>
        /// Toutes les stratégies affichables
        /// </summary>
        /// <returns></returns>
        internal IList<StrategyBase> GetValues()
        {
            return GetValuesFromListView();
        }

        #endregion

        #region ListView

        /// <summary>
        /// Chargement des stratégies disponibles sous forme d'arbre organisé
        /// par StrategyPath
        /// </summary>
        /// <param name="strategiesOwner">The strategies owner.</param>
        /// <param name="selectedStrategy">The selected strategy.</param>
        private void PopulateListView(CandleElement strategiesOwner, StrategyBase selectedStrategy)
        {
            this._strategiesOwner = strategiesOwner != null ? strategiesOwner.StrategiesOwner : null;

            Dictionary<string, ListViewGroup> groups = new Dictionary<string, ListViewGroup>();

            // Evite l'evenement OnItemChecked
            _initialize = true;
            lvStrategies.Items.Clear();

            //
            // Lecture de toutes les strategies disponibles
            //
            foreach (StrategyBase strategy in StrategyManager.GetInstance(_store).GetStrategies(strategiesOwner, false))
            {
                // Création du noeud de la stratégie
                ListViewItem item = new ListViewItem(strategy.DisplayName);
                //
                // Recherche si les strategies possédent des attributs de description
                //
                string strategyGroup = "Standard";
                foreach (
                    StrategyAttribute customAttribute in
                        strategy.GetType().GetCustomAttributes(typeof (StrategyAttribute), false))
                {
                    if (!String.IsNullOrEmpty(customAttribute.Description))
                        strategy.Description = customAttribute.Description;
                    if (!String.IsNullOrEmpty(customAttribute.StrategyGroup))
                    {
                        strategyGroup = customAttribute.StrategyGroup;
                        strategy.StrategyGroup = strategyGroup;
                    }
                }
                item.Group = FindGroup(groups, strategyGroup);
                item.Tag = strategy;
                item.Checked = strategy.IsEnabled;
                lvStrategies.Items.Add(item);
                item.Selected = selectedStrategy != null && selectedStrategy == strategy;
            }

            _initialize = false;

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
        private ListViewGroup FindGroup(Dictionary<string, ListViewGroup> groups, string strategyGroup)
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
        /// Retourne la liste des stratégies actives
        /// </summary>
        /// <returns></returns>
        private IList<StrategyBase> GetSelectedValuesFromListView()
        {
            List<StrategyBase> strategies = new List<StrategyBase>();

            foreach (ListViewItem item in lvStrategies.CheckedItems)
            {
                if (item.Checked)
                {
                    strategies.Add(item.Tag as StrategyBase);
                }
            }
            return strategies;
        }

        /// <summary>
        /// Toutes les stratégies affichables
        /// </summary>
        /// <returns></returns>
        private IList<StrategyBase> GetValuesFromListView()
        {
            List<StrategyBase> strategies = new List<StrategyBase>();

            foreach (ListViewItem item in lvStrategies.CheckedItems)
            {
                strategies.Add(item.Tag as StrategyBase);
            }
            return strategies;
        }

        /// <summary>
        /// Handles the ItemSelectionChanged event of the lvStrategies control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Forms.ListViewItemSelectionChangedEventArgs"/> instance containing the event data.</param>
        private void lvStrategies_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            StrategyBase strategy = (StrategyBase) e.Item.Tag;
            OnSelectionChanged(strategy);
        }

        /// <summary>
        /// Handles the ItemChecked event of the lvStrategies control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Forms.ItemCheckedEventArgs"/> instance containing the event data.</param>
        private void lvStrategies_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            if (_initialize)
                return;
            StrategyBase strategy = (StrategyBase) e.Item.Tag;
            strategy.IsEnabled = e.Item.Checked;
        }

        #endregion

        #region TreeView

        /// <summary>
        /// Initialisation de la liste des stratégies
        /// </summary>
        /// <param name="strategiesOwner">The strategies owner.</param>
        /// <param name="selectedStrategy">The selected strategy.</param>
        private void PopulateTreeView(CandleElement strategiesOwner, StrategyBase selectedStrategy)
        {
            this._strategiesOwner = strategiesOwner != null ? strategiesOwner.StrategiesOwner : null;
            _initialize = true;
            tvStrategies.Nodes.Clear();

            // Remplissage à partir des stratégies liées au modèle
            TreeNode selectedNode = null;
            foreach (StrategyBase strategy in StrategyManager.GetInstance(_store).GetStrategies(strategiesOwner, false))
            {
                TreeNodeCollection nodes = tvStrategies.Nodes;

                // Insertion dans l'arbre en tenant compte du path
                if (!String.IsNullOrEmpty(strategy.StrategyPath))
                {
                    string[] pathParts = strategy.StrategyPath.Split('/');
                    // Création ou recherche de l'arborescence liée au path
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
                            parent.Expand();
                        }
                    }
                }

                // Création du noeud de la stratégie
                TreeNode tmpNode = new TreeNode(strategy.DisplayName);
                tmpNode.Tag = strategy;
                nodes.Add(tmpNode);
                tmpNode.Checked = strategy.IsEnabled;
                if (selectedStrategy != null && selectedStrategy.StrategyId == strategy.StrategyId)
                    selectedNode = tmpNode;
            }

            if (selectedNode != null)
                tvStrategies.SelectedNode = selectedNode;

            _initialize = false;
        }

        /// <summary>
        /// Sélection d'un noeud. Affichage des propriétés
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Forms.TreeViewEventArgs"/> instance containing the event data.</param>
        private void tvStrategies_AfterSelect(object sender, TreeViewEventArgs e)
        {
            StrategyBase strategy = (StrategyBase) e.Node.Tag;
            OnSelectionChanged(strategy);
        }

        /// <summary>
        /// Activation ou désactivation d'une stratégie
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Forms.TreeViewEventArgs"/> instance containing the event data.</param>
        private void tvStrategies_AfterCheck(object sender, TreeViewEventArgs e)
        {
            if (_initialize)
                return;

            StrategyBase strategy = (StrategyBase) e.Node.Tag;
            strategy.IsEnabled = e.Node.Checked;
        }

        /// <summary>
        /// Récupération des stratégies sélectionnées
        /// </summary>
        /// <param name="nodes">The nodes.</param>
        /// <param name="checkedOnly">if set to <c>true</c> [checked only].</param>
        /// <param name="strategies">The strategies.</param>
        /// <returns></returns>
        private List<StrategyBase> RetrieveStrategiesFromTreeView(TreeNodeCollection nodes, bool checkedOnly,
                                                                  List<StrategyBase> strategies)
        {
            if (strategies == null)
                strategies = new List<StrategyBase>();

            foreach (TreeNode node in nodes)
            {
                if (node.Tag == null)
                {
                    RetrieveStrategiesFromTreeView(node.Nodes, checkedOnly, strategies);
                }
                else if (!checkedOnly || node.Checked)
                {
                    strategies.Add(node.Tag as StrategyBase);
                }
            }
            return strategies;
        }

        #endregion
    }
}