using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace DSLFactory.Candle.SystemModel.Editor
{
    /// <summary>
    /// 
    /// </summary>
    public partial class TargetModelSelector : Form
    {
        private readonly Type _filterType;
        private readonly List<string> _initialValues;

        /// <summary>
        /// Initializes a new instance of the <see cref="TargetModelSelector"/> class.
        /// </summary>
        /// <param name="initialValues">The initial values.</param>
        /// <param name="filterType">Type of the filter.</param>
        public TargetModelSelector(List<string> initialValues, Type filterType)
        {
            _initialValues = initialValues;
            _filterType = filterType;
            InitializeComponent();
        }

        /// <summary>
        /// Gets the selected values.
        /// </summary>
        /// <value>The selected values.</value>
        public List<string> SelectedValues
        {
            get
            {
                List<string> values = new List<string>();
                AppendValue(values, tvModelTypes.Nodes);
                return values;
            }
        }

        /// <summary>
        /// Handles the Load event of the TargetModelSelector control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void TargetModelSelector_Load(object sender, EventArgs e)
        {
            tvModelTypes.Nodes.Clear();
            foreach (Type type in GetType().Assembly.GetTypes())
            {
                if (type.IsClass && type.IsSubclassOf(typeof (CandleElement)))
                {
                    if (_filterType == null || type.IsSubclassOf(_filterType))
                    {
                        TreeNode node = InsertInTreeView(type);
                        node.Checked = _initialValues != null && _initialValues.Contains((string) node.Tag);
                        if (node.Checked)
                            node.Expand();
                    }
                }
            }
        }

        /// <summary>
        /// Inserts the in tree view.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        private TreeNode InsertInTreeView(Type type)
        {
            // Création de l'arborescence
            if (type == typeof (CandleElement))
                return null;

            TreeNode parentNode = InsertInTreeView(type.BaseType);

            TreeNode node = FindNode(tvModelTypes.Nodes, type.FullName);
            if (node == null)
            {
                node = new TreeNode(type.Name);
                node.Tag = type.FullName;
                if (parentNode == null)
                    tvModelTypes.Nodes.Add(node);
                else
                    parentNode.Nodes.Add(node);
                node.Expand();
            }
            return node;
        }

        /// <summary>
        /// Finds the node.
        /// </summary>
        /// <param name="nodes">The nodes.</param>
        /// <param name="typeName">Name of the type.</param>
        /// <returns></returns>
        private static TreeNode FindNode(TreeNodeCollection nodes, string typeName)
        {
            foreach (TreeNode node in nodes)
            {
                if ((string) node.Tag == typeName)
                    return node;
                TreeNode tmpNode = FindNode(node.Nodes, typeName);
                if (tmpNode != null)
                    return tmpNode;
            }
            return null;
        }

        /// <summary>
        /// Handles the AfterCheck event of the tvModelTypes control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Forms.TreeViewEventArgs"/> instance containing the event data.</param>
        private static void tvModelTypes_AfterCheck(object sender, TreeViewEventArgs e)
        {
        }

        /// <summary>
        /// Appends the value.
        /// </summary>
        /// <param name="values">The values.</param>
        /// <param name="nodes">The nodes.</param>
        private static void AppendValue(List<string> values, TreeNodeCollection nodes)
        {
            foreach (TreeNode node in nodes)
            {
                AppendValue(values, node.Nodes);
                if (node.Checked)
                    values.Add((string) node.Tag);
            }
        }
    }
}