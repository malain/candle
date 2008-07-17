using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using DSLFactory.Candle.SystemModel.Repository;
using System.Windows.Forms;
using DSLFactory.Candle.SystemModel.Configuration;

namespace DSLFactory.Candle.SystemModel.Wizard
{
    /// <summary>
    /// Affichage de l'arborescence des domaines
    /// </summary>
    internal class DomainPathTreeView : System.Windows.Forms.TreeView
    {
        public DomainPathTreeView(string domainFilter)
        {
            PopulateData(DomainManager.Instance.GetAllPaths(domainFilter));
            this.PathSeparator = DomainManager.PathSeparator.ToString();
        }

        public void AddLevel(string name)
        {
            TreeNodeCollection nodes = this.SelectedNode == null ? this.Nodes : this.SelectedNode.Nodes;
            TreeNode node = nodes.Add(name);
            this.SelectedNode = node;
            RepositoryManager.Instance.CreateDomainPath(node.FullPath);
        }

        public void DeleteCurrentLevel()
        {
            TreeNode node = this.SelectedNode;
            if (node != null)
            {
                if (MessageBox.Show("Are you sure ?", "Warning", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    RepositoryManager.Instance.RemoveDomainPath(node.FullPath);
                    node.Remove();
                }
            }
        }

        public string SelectedPath
        {
            get
            {
                if (SelectedNode != null)
                {
                   // if ((bool)SelectedNode.Tag)
                    {
                        return SelectedNode.FullPath;
                    }
                }
                return null;
            }
        }

        /// <summary>
        /// Remplissage du treeview en créant un regroupement à chaque
        /// séparateur
        /// </summary>
        /// <param name="data">Données sous la forme grand pere/pere/fils</param>
        private void PopulateData(List<String> data)
        {
            // D'abord on tri la liste
            data.Sort();
            List<TreeNode> hierarchy = new List<TreeNode>();

            // Puis on rajoute au treeview
            foreach (string item in data)
            {
                string[] parts = item.Split(DomainManager.PathSeparator);
                for (int i = 0; i < parts.Length; i++)
                {
                    if (i < hierarchy.Count && parts[i] != hierarchy[i].Text)
                        hierarchy.RemoveRange(i, hierarchy.Count - i);

                    if (i >= hierarchy.Count)
                    {
                        TreeNode node = new TreeNode(parts[i]);
//                        node.Tag = (i == parts.Length - 1 ); // On indique si on est une feuille
                        TreeNodeCollection parent = i > 0 ? hierarchy[i - 1].Nodes : this.Nodes;
                        parent.Add(node);
                        hierarchy.Add(node);
                    }
                }
            }
            this.ExpandAll();
        }
    }
}
