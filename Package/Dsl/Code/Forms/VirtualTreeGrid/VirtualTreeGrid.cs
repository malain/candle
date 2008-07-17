using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using DSLFactory.Candle.SystemModel.Commands;
using DSLFactory.Candle.SystemModel.Strategies;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Diagrams;
using Microsoft.VisualStudio.Modeling.Shell;

namespace DSLFactory.Candle.SystemModel.Utilities
{
    /// <summary>
    /// 
    /// </summary>
    public class VirtualTreeGrid : DataGridView
    {
        private List<VirtualTreeGridCategory> _categories;

        private string _childSeparators;
        private DataGridViewTextBoxColumn _colDescription;
        private DataGridViewComboBoxColumn _colDirection;
        private DataGridViewIsCollectionColumn _colIsCollection;
        private DataGridViewNameColumn _colName;
        private DataGridViewDropDownListColumn _colType;
        private SoftwareComponent _component;
        private int _contextMenuRowIndex;
        private bool _inPlaceEditMode;
        private DataList _members;
        private string _memberSeparators;
        private IHasChildren _root;
        private string _title;

        /// <summary>
        /// Initializes a new instance of the <see cref="VirtualTreeGrid"/> class.
        /// </summary>
        public VirtualTreeGrid()
        {
            InitializeComponent();
            ClipboardCopyMode = DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
        }

        /// <summary>
        /// Chaine contenant les caractères servant à séparer les lignes enfants (pour forcer le changement de ligne)
        /// </summary>
        public string ChildSeparators
        {
            get { return _childSeparators; }
            set { _childSeparators = value; }
        }

        /// <summary>
        /// Chaine contenant les caractères servant à séparer les lignes parents (pour forcer le changement de ligne)
        /// </summary>
        public string MemberSeparators
        {
            get { return _memberSeparators; }
            set { _memberSeparators = value; }
        }

        /// <summary>
        /// Occurs when [data changed].
        /// </summary>
        public event DataChangedEventHandler DataChanged;

        /// <summary>
        /// Processes window messages.
        /// </summary>
        /// <param name="m">A <see cref="T:System.Windows.Forms.Message"></see>, passed by reference, that represents the window message to process.</param>
        protected override void WndProc(ref Message m)
        {
            try
            {
                base.WndProc(ref m);
            }
            catch (Exception ex)
            {
                ILogger logger = ServiceLocator.Instance.GetService<ILogger>();
                if (logger != null)
                    logger.WriteError("VirtualTreeGrid", "Unhandled exception", ex);
            }
        }

        /// <summary>
        /// Affichage du menu contextuel
        /// </summary>
        /// <param name="e">A <see cref="T:System.Windows.Forms.DataGridViewRowContextMenuStripNeededEventArgs"></see> that contains the event data.</param>
        protected override void OnRowContextMenuStripNeeded(DataGridViewRowContextMenuStripNeededEventArgs e)
        {
            base.OnRowContextMenuStripNeeded(e);

            if (e.RowIndex > 0)
            {
                VirtualTreeGridItem data = GetRowValue(e.RowIndex);
                if (data == null || data.Kind == ModelKind.Root || data.Kind == ModelKind.Category)
                    return;

                e.ContextMenuStrip = new ContextMenuStrip();
                _contextMenuRowIndex = e.RowIndex;

                // Ajout de l'option de suppression
                ToolStripItem item = new ToolStripMenuItem("Copy");
                item.Enabled = data.DataItem != null;
                item.Click += RowCopy_Click;
                e.ContextMenuStrip.Items.Add(item);

                //if (e.RowIndex > 1 && data.DataItem != null)
                //{
                //    item = new ToolStripMenuItem("Up");
                //    item.Enabled = true;
                //    item.Click += new EventHandler(RowUp_Click);
                //    e.ContextMenuStrip.Items.Add(item);                
                //}

                // Recherche si on est sur la dernière ligne
                //if( data.DataItem != null)
                //{
                //    bool lastRow = true;

                //    if (data.Kind == ModelKind.Child )
                //    {
                //        VirtualTreeGridItem d = GetRowValue(e.RowIndex+1);
                //        lastRow = d == null || d.DataItem == null;
                //    }
                //    else if (data.Kind == ModelKind.Member)
                //    {
                //        // En dessous d'un membre, il ne doit y avoir que des arguments ou des lignes vides.
                //        // Si on rencontre autre chose, c'est qu'on est pas la dernière ligne
                //        for (int i = e.RowIndex + 1; i < this.Rows.Count; i++)
                //        {
                //            VirtualTreeGridItem d = GetRowValue(i);
                //            if (d != null && (d.Kind != ModelKind.Child && d.DataItem != null))
                //            {
                //                lastRow = false;
                //                break;
                //            }
                //        }
                //    }

                //    if (lastRow)
                //    {
                //        item = new ToolStripMenuItem("Down");
                //        item.Enabled = true;
                //        item.Click += new EventHandler(RowDown_Click);
                //        e.ContextMenuStrip.Items.Add(item);
                //    }
                //}

                if (!data.ReadOnly)
                {
                    item = new ToolStripMenuItem("Delete");
                    item.Enabled = data.DataItem != null;
                    item.Click += RowDelete_Click;
                    e.ContextMenuStrip.Items.Add(item);
                }
            }
        }

        /// <summary>
        /// Handles the Click event of the RowCopy control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void RowCopy_Click(object sender, EventArgs e)
        {
            VirtualTreeGridItem item = GetRowValue(_contextMenuRowIndex);
            ArrayList list = new ArrayList();
            list.Add(item.DataItem);

            CopyCommand cc = new CopyCommand(((ModelElement) _root).Store, list);
            if (cc.Enabled && cc.Visible())
                cc.Exec();
        }

        /// <summary>
        /// Suppression d'une ligne
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void RowDelete_Click(object sender, EventArgs e)
        {
            VirtualTreeGridItem item = GetRowValue(_contextMenuRowIndex);
            OnDataChanged(new VirtualTreeGridDataChangedEventsArgs(item, true));
            Rows.RemoveAt(_contextMenuRowIndex);
        }

        /// <summary>
        /// Initializes the component.
        /// </summary>
        private void InitializeComponent()
        {
            RowTemplate.Height = 20;
            AutoGenerateColumns = false;
            RowsDefaultCellStyle.Font = new Font("Tahoma", 8);
            AllowUserToResizeColumns = true;
            SelectionMode = DataGridViewSelectionMode.CellSelect;
            EditMode = DataGridViewEditMode.EditOnEnter;
            CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            MultiSelect = false;

            AllowUserToAddRows = false;
            AllowUserToDeleteRows = true;
            AllowUserToOrderColumns = false;

            _colName = new DataGridViewNameColumn();
            _colType = new DataGridViewDropDownListColumn();
            _colDescription = new DataGridViewTextBoxColumn();
            _colIsCollection = new DataGridViewIsCollectionColumn();
            _colDirection = new DataGridViewDirectionColumn();

            ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            Columns.AddRange(new DataGridViewColumn[]
                                 {
                                     _colName,
                                     _colType,
                                     _colIsCollection,
                                     _colDirection,
                                     _colDescription
                                 });

            // 
            // colDirection
            // 
            _colDirection.AutoComplete = true;
            _colDirection.DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing;
            _colDirection.DataPropertyName = "Direction";
            _colDirection.HeaderText = "Direction";
            _colDirection.Name = "colDirection";
            _colDirection.Resizable = DataGridViewTriState.False;
            _colDirection.SortMode = DataGridViewColumnSortMode.NotSortable;
            _colDirection.Width = 100;
            _colDirection.Items.AddRange(Enum.GetNames(typeof (ArgumentDirection)));
            // 
            // colIsCollection
            // 
            _colIsCollection.DataPropertyName = "IsCollection";
            _colIsCollection.HeaderText = "List";
            _colIsCollection.Name = "colIsCollection";
            _colIsCollection.SortMode = DataGridViewColumnSortMode.NotSortable;
            _colIsCollection.Width = 25;

            // 
            // colName
            // 
            _colName.DataPropertyName = "Name";
            _colName.HeaderText = "Name";
            _colName.Name = "colName";
            _colName.SortMode = DataGridViewColumnSortMode.NotSortable;
            _colName.Width = 250;
            _colName.ExpandCollapse += colName_ExpandCollapse;
            // 
            // colType
            // 
            _colType.AutoComplete = true;
            _colType.DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing;
            _colType.DataPropertyName = "Type";
            _colType.HeaderText = "Type";
            _colType.Name = "colType";
            _colType.Resizable = DataGridViewTriState.True;
            _colType.SortMode = DataGridViewColumnSortMode.NotSortable;
            _colType.Width = 350;
            // 
            // colDescription
            // 
            _colDescription.DataPropertyName = "Comment";
            _colDescription.HeaderText = "Description";
            _colDescription.Name = "colDescription";
            _colDescription.SortMode = DataGridViewColumnSortMode.NotSortable;
            _colDescription.Width = 250;
            _colDescription.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        }

        /// <summary>
        /// Handles the ExpandCollapse event of the colName control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Forms.DataGridViewCellEventArgs"/> instance containing the event data.</param>
        private void colName_ExpandCollapse(object sender, DataGridViewCellEventArgs e)
        {
            VirtualTreeGridItem methodValue = GetRowValue(e.RowIndex);
            ExpandCollapseRow(e.RowIndex, methodValue.Collapse);
        }

        /// <summary>
        /// Affichage du code
        /// </summary>
        /// <param name="e">A <see cref="T:System.Windows.Forms.DataGridViewCellEventArgs"></see> that contains the event data.</param>
        /// <exception cref="T:System.ArgumentOutOfRangeException">The value of the <see cref="P:System.Windows.Forms.DataGridViewCellEventArgs.ColumnIndex"></see> property of e is greater than the number of columns in the control minus one.-or-The value of the <see cref="P:System.Windows.Forms.DataGridViewCellEventArgs.RowIndex"></see> property of e is greater than the number of rows in the control minus one.</exception>
        protected override void OnCellDoubleClick(DataGridViewCellEventArgs e)
        {
            base.OnCellDoubleClick(e);

            // Essaye d'afficher le code
            VirtualTreeGridItem item = GetRowValue(e.RowIndex);
            if (item == null)
                return;

            IShowCodeProperties data = _root as IShowCodeProperties;
            if (data != null && item.DataItem is IHasChildren)
            {
                Mapper.Instance.ShowCode(_root.Id, data.Name, data.GetMemberName(item.DataItem.Name));
            }
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.DataGridView.DataError"></see> event.
        /// </summary>
        /// <param name="displayErrorDialogIfNoHandler">true to display an error dialog box if there is no handler for the <see cref="E:System.Windows.Forms.DataGridView.DataError"></see> event.</param>
        /// <param name="e">A <see cref="T:System.Windows.Forms.DataGridViewDataErrorEventArgs"></see> that contains the event data.</param>
        protected override void OnDataError(bool displayErrorDialogIfNoHandler, DataGridViewDataErrorEventArgs e)
        {
            e.ThrowException = true;
            base.OnDataError(false, e);
        }

        /// <summary>
        /// Sets the component.
        /// </summary>
        /// <param name="component">The component.</param>
        internal void SetComponent(SoftwareComponent component)
        {
            if (_component == component)
                return;

            if (_component != null && _component.Store != null)
            {
                _component.Store.EventManagerDirectory.ElementPropertyChanged.Remove(
                    new EventHandler<ElementPropertyChangedEventArgs>(OnPropertyChanged));
            }

            _component = component;

            if (_component != null && _component.Store != null)
            {
                _component.Store.EventManagerDirectory.ElementPropertyChanged.Add(
                    new EventHandler<ElementPropertyChangedEventArgs>(OnPropertyChanged));
            }
        }

        /// <summary>
        /// Remplissage de l'arbre
        /// </summary>
        /// <param name="elem">The elem.</param>
        /// <param name="headerName">Name of the header.</param>
        /// <param name="categories">The categories.</param>
        public void BindData(IHasChildren elem, string headerName, List<VirtualTreeGridCategory> categories)
        {
            Debug.Assert(elem != null);
            Debug.Assert(categories != null && categories.Count > 0);

            _root = elem;
            _title = headerName;
            _categories = categories;
            RefreshData();
        }

        /// <summary>
        /// Refreshes the data.
        /// </summary>
        private void RefreshData()
        {
            _members = new DataList();
            _colName.HeaderName = _title;

            // Remplissage de la liste des types
            _colType.Items.Clear();
            foreach (string typeName in _component.GetDefinedTypeNames())
            {
                _colType.Items.Add(typeName);
            }

            // La racine
            VirtualTreeGridItem item = new VirtualTreeGridItem(_root);
            _members.Add(item);

            foreach (VirtualTreeGridCategory category in _categories)
            {
                // On ne met rien si il n'y a qu'une catégorie par défaut
                if (!category.IsEmpty)
                    _members.Add(
                        new VirtualTreeGridItem(new VirtualCategoryInstance(category.Name), category, ModelKind.Category));

                IList operations = _root.GetChildrenForCategory(category);
                if (operations != null)
                {
                    foreach (ITypeMember operation in operations)
                    {
                        _members.Add(new VirtualTreeGridItem(category, _root, operation, ModelKind.Member));

                        if (operation is IHasChildren)
                        {
                            IList arguments = ((IHasChildren) operation).GetChildrenForCategory(null);
                            if (arguments != null)
                            {
                                foreach (ITypeMember arg in arguments)
                                {
                                    _members.Add(
                                        new VirtualTreeGridItem(category, (IHasChildren) operation, arg, ModelKind.Child));
                                }
                                // Un vide pour pouvoir en rajouter
                                _members.Add(
                                    new VirtualTreeGridItem(category, (IHasChildren) operation, ModelKind.Child));
                            }
                        }
                    }

                    // Un vide pour pouvoir en rajouter
                    if (!category.ReadOnly)
                        _members.Add(new VirtualTreeGridItem(category, _root, ModelKind.Member));
                }

                DataSource = _members;

                foreach (DataGridViewRow row in Rows)
                {
                    VirtualTreeGridItem data = (VirtualTreeGridItem) row.DataBoundItem;
                    if (data.Kind == ModelKind.Member)
                        ExpandCollapseRow(row.Index, false);
                    row.ReadOnly = data.ReadOnly;
                }
            }
        }

        /// <summary>
        /// Gets the row value.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <returns></returns>
        internal VirtualTreeGridItem GetRowValue(int index)
        {
            if (index < 0 || index >= Rows.Count)
                return null;
            DataGridViewRow row = Rows[index];
            if (row == null)
                return null;
            VirtualTreeGridItem item = row.DataBoundItem as VirtualTreeGridItem;
            return item;
        }

        /// <summary>
        /// Quand la cellule pert le focus, on ajoute une ligne
        /// en dessous suivant le contexte si c'est possible
        /// </summary>
        /// <param name="e">A <see cref="T:System.Windows.Forms.DataGridViewCellEventArgs"></see> that contains the event data.</param>
        /// <exception cref="T:System.ArgumentOutOfRangeException">The value of the <see cref="P:System.Windows.Forms.DataGridViewCellEventArgs.ColumnIndex"></see> property of e is greater than the number of columns in the control minus one.-or-The value of the <see cref="P:System.Windows.Forms.DataGridViewCellEventArgs.RowIndex"></see> property of e is greater than the number of rows in the control minus one.</exception>
        protected override void OnCellValidated(DataGridViewCellEventArgs e)
        {
            base.OnCellValidated(e);

            try
            {
                // 1 ére ligne (Entete)
                if (e.RowIndex == 0 || _root == null || _root.Store == null)
                    return;

                // Si on est sur une catégorie, on ne fait toujours rien
                VirtualTreeGridItem value = GetRowValue(e.RowIndex);
                // Si le nom saisie n'est pas valable ou si on est en readonly, on ne valide pas 
                if (value == null || value.ReadOnly || String.IsNullOrEmpty(value.Name) ||
                    !StrategyManager.GetInstance(_root.Store).NamingStrategy.IsClassNameValid(value.Name))
                    return;

                bool isNewValue = value.IsNewValue; // Est on sur une ligne nouvelle ?

                // Vérification si l'utilisateur peut sortir de la cellule
                VirtualTreeGridDataChangedEventsArgs arg = new VirtualTreeGridDataChangedEventsArgs(value);
                OnDataChanged(arg);
                if (arg.Cancel)
                {
                    return;
                }

                // Si on était sur la 1ère colonne, création d'une ligne supplémentaire
                if (e.ColumnIndex == 0)
                {
                    if (isNewValue)
                    {
                        if (value.Kind == ModelKind.Member)
                        {
                            //VirtualTreeGridItem methodValue = value as VirtualTreeGridItem;

                            if (value.DataItem is IHasChildren)
                            {
                                value.Collapse = false;
                                _members.Add(
                                    new VirtualTreeGridItem(value.Category, (IHasChildren) value.DataItem,
                                                            ModelKind.Child));
                            }

                            _members.Add(new VirtualTreeGridItem(value.Category, value.Parent, ModelKind.Member));

                            if (value.DataItem is IHasChildren)
                            {
                                ExpandCollapseRow(Rows.Count - 2, false);
                            }
                        }
                        else if (value.Kind == ModelKind.Child)
                        {
                            //VirtualTreeGridItem argValue = value as VirtualTreeGridItem;
                            _members.Insert(e.RowIndex + 1,
                                            new VirtualTreeGridItem(value.Category, value.Parent, ModelKind.Child));
                        }
                    }
                }
            }
            finally
            {
                _inPlaceEditMode = false;
            }
        }

        /// <summary>
        /// Called when [data changed].
        /// </summary>
        /// <param name="arg">The arg.</param>
        private void OnDataChanged(VirtualTreeGridDataChangedEventsArgs arg)
        {
            if (DataChanged != null)
                DataChanged(this, arg);
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.DataGridView.RowEnter"></see> event.
        /// </summary>
        /// <param name="e">A <see cref="T:System.Windows.Forms.DataGridViewCellEventArgs"></see> that contains the event data.</param>
        protected override void OnRowEnter(DataGridViewCellEventArgs e)
        {
            base.OnRowEnter(e);

            if (_root.Store == null)
                return;

            IMonitorSelectionService monitorSelectionService =
                (IMonitorSelectionService) ((ModelElement) _root).Store.GetService(typeof (IMonitorSelectionService));
            if (monitorSelectionService != null && e.RowIndex > 0)
            {
                ISelectionService selectionService =
                    monitorSelectionService.CurrentSelectionContainer as ISelectionService;
                VirtualTreeGridItem value = GetRowValue(e.RowIndex);
                if (value != null && value.DataItem != null)
                    if (selectionService != null) selectionService.SetSelectedComponents(new object[] {value.DataItem});
            }
        }

        /// <summary>
        /// Quand on commence à éditer une cellule, on supprime la sélection dans le diagramme pour être sur de capturer
        /// tous les événements
        /// (Pour corriger le BUG: Quand on appuie sur la touche DEL lors de l'édition d'une cellule, c'est le shape sélectionné qui disparait)
        /// </summary>
        /// <param name="e">A <see cref="T:System.Windows.Forms.DataGridViewCellCancelEventArgs"></see> that contains the event data.</param>
        /// <exception cref="T:System.ArgumentOutOfRangeException">The value of the <see cref="P:System.Windows.Forms.DataGridViewCellCancelEventArgs.ColumnIndex"></see> property of e is greater than the number of columns in the control minus one.-or-The value of the <see cref="P:System.Windows.Forms.DataGridViewCellCancelEventArgs.RowIndex"></see> property of e is greater than the number of rows in the control minus one.</exception>
        protected override void OnCellBeginEdit(DataGridViewCellCancelEventArgs e)
        {
            base.OnCellBeginEdit(e);

            ModelElement elem = _root as ModelElement;

            // On enlève la sélection sur le shape
            if (elem != null)
            {
                IList<PresentationElement> shapes = PresentationViewsSubject.GetPresentation(elem);
                if (shapes.Count > 0)
                {
                    NodeShape ns = shapes[0] as NodeShape;
                    if (ns != null && ns.Diagram != null && ns.Diagram.ClientViews.Count > 0)
                    {
                        DiagramClientView clientView = ns.Diagram.ClientViews[0] as DiagramClientView;
                        if (clientView != null) clientView.Selection.Clear();
                    }
                }
            }

            // L'entete n'est pas éditable ou une catégorie
            VirtualTreeGridItem item = GetRowValue(e.RowIndex);
            if (item == null || item.Kind == ModelKind.Root || item.Kind == ModelKind.Category)
                e.Cancel = true;

            _inPlaceEditMode = true;
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.DataGridView.CellEndEdit"></see> event.
        /// </summary>
        /// <param name="e">A <see cref="T:System.Windows.Forms.DataGridViewCellEventArgs"></see> that contains the event data.</param>
        /// <exception cref="T:System.ArgumentOutOfRangeException">The value of the <see cref="P:System.Windows.Forms.DataGridViewCellEventArgs.ColumnIndex"></see> property of e is greater than the number of columns in the control minus one.-or-The value of the <see cref="P:System.Windows.Forms.DataGridViewCellEventArgs.RowIndex"></see> property of e is greater than the number of rows in the control minus one.</exception>
        protected override void OnCellEndEdit(DataGridViewCellEventArgs e)
        {
            base.OnCellEndEdit(e);
            _inPlaceEditMode = false;
        }

        /// <summary>
        /// Raises the <see cref="E:LostFocus"/> event.
        /// </summary>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected override void OnLostFocus(EventArgs e)
        {
            base.OnLostFocus(e);
            EndEdit();
        }

        /// <summary>
        /// Processes keys used for navigating in the <see cref="T:System.Windows.Forms.DataGridView"></see>.
        /// </summary>
        /// <param name="e">Contains information about the key that was pressed.</param>
        /// <returns>
        /// true if the key was processed; otherwise, false.
        /// </returns>
        /// <exception cref="T:System.InvalidCastException">The key pressed would cause the control to enter edit mode, but the <see cref="P:System.Windows.Forms.DataGridViewCell.EditType"></see> property of the current cell does not indicate a class that derives from <see cref="T:System.Windows.Forms.Control"></see> and implements <see cref="T:System.Windows.Forms.IDataGridViewEditingControl"></see>.</exception>
        /// <exception cref="T:System.Exception">This action would commit a cell value or enter edit mode, but an error in the data source prevents the action and either there is no handler for the <see cref="E:System.Windows.Forms.DataGridView.DataError"></see> event or the handler has set the <see cref="P:System.Windows.Forms.DataGridViewDataErrorEventArgs.ThrowException"></see> property to true.-or-The DELETE key would delete one or more rows, but an error in the data source prevents the deletion and either there is no handler for the <see cref="E:System.Windows.Forms.DataGridView.DataError"></see> event or the handler has set the <see cref="P:System.Windows.Forms.DataGridViewDataErrorEventArgs.ThrowException"></see> property to true. </exception>
        protected override bool ProcessDataGridViewKey(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                return false;

            return base.ProcessDataGridViewKey(e);
        }

        /// <summary>
        /// Passage à la ligne d'en dessous
        /// </summary>
        /// <param name="rowIndex">Index de la ligne courante</param>
        /// <param name="forceMethod">if set to <c>true</c> [force method].</param>
        public void NavigateNext(int rowIndex, bool forceMethod)
        {
            // Recherche du type de la ligne suivante
            int nextIndex = rowIndex + 1;
            VirtualTreeGridItem value = GetRowValue(rowIndex);

            // Il n'y a plus rien, on ne peut pas descendre
            if (value == null)
                return;

            // Si c'est une catégorie, on la saute
            if (value.Kind == ModelKind.Category)
            {
                value = GetRowValue(++nextIndex);
                if (value == null)
                    return;
            }

            // Le passage sur une ligne, la déploie automatiquement
            if (value.Kind == ModelKind.Member && value.Collapse)
            {
                ExpandCollapseRow(rowIndex, true);
            }

            if (value.Kind == ModelKind.Child && forceMethod)
                nextIndex++;

            // On passe à la cellule suivante
            ProcessTabKey(Keys.Tab);

            // Et on se met en mode édition
            ClearSelection();
            if (nextIndex < Rows.Count)
            {
                SetSelectedCellCore(0, nextIndex, true);
                SetCurrentCellAddressCore(0, nextIndex, false, true, false);
            }
        }

        /// <summary>
        /// Ouverture ou fermeture d'une branche
        /// </summary>
        /// <param name="rowIndex">Index de la branche</param>
        /// <param name="expand">true pour l'ouverture</param>
        private void ExpandCollapseRow(int rowIndex, bool expand)
        {
            SuspendLayout();

            VirtualTreeGridItem methodValue = GetRowValue(rowIndex);
            // Est ce qu'on est sur une ligne qui peut s'ouvrir
            if (methodValue != null && methodValue.Kind == ModelKind.Member)
            {
                int index = rowIndex + 1;
                VirtualTreeGridItem value = GetRowValue(index);

                // L'affichage des enfants se fait en rendant leurs propriétés visible à true
                while (value != null && value.Kind == ModelKind.Child)
                {
                    Rows[index].Visible = expand;
                    value = GetRowValue(++index);
                }
                methodValue.Collapse = !expand;
            }

            ResumeLayout();
        }

        /// <summary>
        /// Called when [property changed].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="Microsoft.VisualStudio.Modeling.ElementPropertyChangedEventArgs"/> instance containing the event data.</param>
        private void OnPropertyChanged(object sender, ElementPropertyChangedEventArgs e)
        {
            if (_root == null || _root.Store == null || _inPlaceEditMode)
                return;

            RefreshData();
        }

        #region Nested type: DataList

        /// <summary>
        /// Liste des données. L'arbre est stocké à plat, une ligne par branche. On connait ce
        /// que contient la ligne en testant sa propriété Kind
        /// </summary>
        private class DataList : BindingList<VirtualTreeGridItem>
        {
            /// <summary>
            /// Règles à appliquer lors de la suppression
            /// - On ne peut pas supprimer la classe ni la dernière ligne (si c'est une méthode)
            /// - Quand on supprime une mèthode on supprime aussi les lignes de ses arguments
            /// </summary>
            /// <param name="index">The zero-based index of the item to remove.</param>
            protected override void RemoveItem(int index)
            {
                // On ne peut pas supprimer la classe ni la dernière ligne (si c'est une méthode)
                if (index == 0)
                    return;

                if (this[index].Kind == ModelKind.Member)
                {
                    if (index == Count - 1)
                        return;

                    // Suppression des arguments
                    int index2 = index + 1;
                    while (index2 < Count && this[index2].Kind == ModelKind.Child)
                    {
                        RemoveItem(index2);
                    }
                }

                base.RemoveItem(index);
            }
        }

        #endregion

        #region Nested type: VirtualCategoryInstance

        /// <summary>
        /// 
        /// </summary>
        private class VirtualCategoryInstance : ITypeMember
        {
            private string name;

            /// <summary>
            /// Initializes a new instance of the <see cref="VirtualCategoryInstance"/> class.
            /// </summary>
            /// <param name="name">The name.</param>
            public VirtualCategoryInstance(string name)
            {
                this.name = name;
            }

            /// <summary>
            /// Gets or sets the direction.
            /// </summary>
            /// <value>The direction.</value>
            public ArgumentDirection Direction
            {
                get { return ArgumentDirection.In; }
                set { }
            }

            #region ITypeMember Members

            /// <summary>
            /// Gets or sets the name.
            /// </summary>
            /// <value>The name.</value>
            public string Name
            {
                get { return name; }
                set { name = value; }
            }

            /// <summary>
            /// Gets the full name of the type.
            /// </summary>
            /// <value>The full name of the type.</value>
            public string FullTypeName
            {
                get { return name; }
            }

            /// <summary>
            /// Gets or sets the type.
            /// </summary>
            /// <value>The type.</value>
            public string Type
            {
                get { return null; }
                set { }
            }

            /// <summary>
            /// Gets or sets the comment.
            /// </summary>
            /// <value>The comment.</value>
            public string Comment
            {
                get { return null; }
                set { }
            }

            /// <summary>
            /// Gets the id.
            /// </summary>
            /// <value>The id.</value>
            public Guid Id
            {
                get { return Guid.Empty; }
            }

            /// <summary>
            /// Gets or sets a value indicating whether this instance is collection.
            /// </summary>
            /// <value>
            /// 	<c>true</c> if this instance is collection; otherwise, <c>false</c>.
            /// </value>
            public bool IsCollection
            {
                get { return false; }
                set { }
            }

            #endregion
        }

        #endregion
    }
}