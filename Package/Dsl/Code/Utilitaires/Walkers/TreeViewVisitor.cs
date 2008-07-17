using System.Windows.Forms;

namespace DSLFactory.Candle.SystemModel.Utilities
{
    /// <summary>
    /// Permet de traverser tous les noeuds d'un treeview
    /// </summary>
    public class TreeViewWalker
    {
        #region Delegates

        /// <summary>
        /// 
        /// </summary>
        public delegate void ProcessTreeNode(TreeNode node);

        #endregion

        private readonly TreeView _treeView;

        /// <summary>
        /// Initializes a new instance of the <see cref="TreeViewWalker"/> class.
        /// </summary>
        /// <param name="treeView">The tree view.</param>
        public TreeViewWalker(TreeView treeView)
        {
            _treeView = treeView;
        }

        /// <summary>
        /// Traverses the specified callback.
        /// </summary>
        /// <param name="callback">The callback.</param>
        public void Traverse(ProcessTreeNode callback)
        {
            EnumNodes(_treeView.Nodes, callback);
        }

        /// <summary>
        /// Enums the nodes.
        /// </summary>
        /// <param name="nodes">The nodes.</param>
        /// <param name="callback">The callback.</param>
        private static void EnumNodes(TreeNodeCollection nodes, ProcessTreeNode callback)
        {
            foreach (TreeNode node in nodes)
            {
                callback(node);
                if (node.Nodes.Count > 0)
                    EnumNodes(node.Nodes, callback);
            }
        }
    }
}