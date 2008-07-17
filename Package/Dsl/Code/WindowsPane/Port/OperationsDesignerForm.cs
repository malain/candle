using System;
using System.Collections.Generic;
using System.Windows.Forms;
using DSLFactory.Candle.SystemModel.Strategies;
using DSLFactory.Candle.SystemModel.Utilities;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Diagrams;

namespace DSLFactory.Candle.SystemModel.Designer
{
    /// <summary>
    /// Classe permettant d'afficher le détail d'une classe
    /// </summary>
    public partial class OperationsDesignerForm : Form
    {
        private bool _binding = false;
        private SoftwareComponent _component;
        private ISupportDetailView _selectedObject;
        private readonly VirtualTreeGrid _treeview;
        private string _title;
        private List<VirtualTreeGridCategory> _categories;

        /// <summary>
        /// Initializes a new instance of the <see cref="OperationsDesignerForm"/> class.
        /// </summary>
        public OperationsDesignerForm()
        {
            _treeview = new VirtualTreeGrid();
            _treeview.Visible = false;
            _treeview.Dock = DockStyle.Fill;
            Controls.Add(_treeview);

            InitializeComponent();
            _treeview.DataChanged += Treeview_DataChanged;
        }

        /// <summary>
        /// Mise à jour des données du modèle avec les données saisies par l'utilisateur
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The e.</param>
        private void Treeview_DataChanged(object sender, VirtualTreeGridDataChangedEventsArgs e)
        {
            ModelElement elem = e.Item.DataItem as ModelElement;
            if (_selectedObject == null || _selectedObject.Store == null || (elem != null && elem.IsDeleted))
                return;
            try
            {
                using (
                    Transaction transaction = _selectedObject.Store.TransactionManager.BeginTransaction("Update member")
                    )
                {
                    ITypeMember op = e.Item.DataItem;

                    // Suppression
                    if (e.IsDelete)
                    {
                        e.Item.Remove();
                    }
                    else
                    {
                        // Création
                        if (e.Item.IsNewValue)
                        {
                            op = _selectedObject.CreateModel(e.Item.Kind);
                        }
                            // Si rien n'a changé, on ne fait rien
                        else if (e.Item.IsCollection == op.IsCollection && e.Item.Name == op.Name &&
                                 e.Item.Type == op.Type && e.Item.Comment == op.Comment)
                        {
                            return;
                        }

                        // ---------------------------------------
                        // Mise à jour
                        // Nom
                        op.Name = e.Item.Name;
                        op.Comment = e.Item.Comment;
                        op.IsCollection = e.Item.IsCollection;
                        if (op is IArgument && !String.IsNullOrEmpty(e.Item.Direction))
                        {
                            ((IArgument) op).Direction =
                                (ArgumentDirection) Enum.Parse(typeof (ArgumentDirection), e.Item.Direction);
                        }

                        // Type
                        if (!String.IsNullOrEmpty(e.Item.Type))
                        {
                            try
                            {
                                op.Type =
                                    e.Item.Type = ClrTypeParser.Parse(e.Item.Type, _component.CreateTypeIfNotExists);
                            }
                            catch
                            {
                                _treeview.CancelEdit();
                                e.Cancel = true;
                                return;
                            }
                        }
                        else
                            op.Type = e.Item.Type = _selectedObject.GetDefaultType(e.Item.Kind);

                        // Commit met à jour la liste des enfants du parent
                        e.Item.Commit(op);
                    }

                    transaction.Commit();

                    // Force le update dans le designer
                    ModelElement mel = ((TypeMember) e.Item.DataItem).Owner as ModelElement;
                    if (mel != null)
                    {
                        IList<PresentationElement> pels = PresentationViewsSubject.GetPresentation(mel);
                        if (pels.Count > 0)
                            ((ShapeElement) pels[0]).Invalidate();
                    }
                }
            }
            catch (Exception ex)
            {
                // Ne doit jamais arriver (bug quelque part le store de l'élément est quelquefois à null!!!!!)
                ILogger logger = ServiceLocator.Instance.GetService<ILogger>();
                if (logger != null)
                    logger.WriteError("Details form", String.Format("DataGridView error on {0}", elem!=null?elem.Id:Guid.Empty), ex);
            }
        }

        /// <summary>
        /// Gets or sets the selected object.
        /// </summary>
        /// <value>The selected object.</value>
        private ISupportDetailView SelectedObject
        {
            get { return _selectedObject; }
            set
            {
                _selectedObject = value;
                _treeview.Visible = value != null;
                _title = String.Empty;

                if (value != null)
                {
                    string memberSeparators;
                    string childSeparators;
                    value.GetEditProperties(out _title, out _categories, out memberSeparators, out childSeparators);
                    _treeview.MemberSeparators = memberSeparators;
                    _treeview.ChildSeparators = childSeparators;
                    // SubscribeEvents(value);
                }
            }
        }

        /// <summary>
        /// Sets the selection.
        /// </summary>
        /// <param name="mel">The mel.</param>
        /// <returns></returns>
        internal bool SetSelection(ModelElement mel)
        {
            if (_binding)
                return false;

            SuspendLayout();
            try
            {
                lblWarning.Visible = true;
                _treeview.Visible = false;

                if (mel == null)
                    return false;

                ModelElement elem = mel;
                if (mel is PresentationElement)
                    elem = ((PresentationElement) mel).ModelElement;

                if (elem == null || elem.Store == null)
                    return false;

                // Recherche du composant principal
                IList<SoftwareComponent> list = elem.Store.ElementDirectory.FindElements<SoftwareComponent>(true);
                if (list.Count == 0)
                    return false;

                _component = list[0];
                _treeview.SetComponent(_component);

                ISupportDetailView mainElem = elem as ISupportDetailView;
                // Cas ou on a sélectionné un enfant
                if (mainElem == null && elem is ICustomizableElement)
                    mainElem = ((ICustomizableElement) elem).Owner as ISupportDetailView;                

                _binding = true;
                SelectedObject = mainElem;

                lblWarning.Visible = false;
                if (SelectedObject == null)
                {
                    lblWarning.Visible = true;
                    return false;
                }

                try
                {
                    _treeview.BindData(SelectedObject, _title, _categories);
                    return true;
                }
                catch
                {
                    lblWarning.Visible = true;
                    _treeview.Visible = false;
                }
            }
            finally
            {
                _binding = false;
                ResumeLayout();
            }
            return false;
        }
    }
}