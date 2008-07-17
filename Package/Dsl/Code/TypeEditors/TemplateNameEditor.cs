using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.IO;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using DSLFactory.Candle.SystemModel.Repository;
using DSLFactory.Candle.SystemModel.Strategies;

namespace DSLFactory.Candle.SystemModel.Editor
{
    /// <summary>
    /// Permet de sélectionner des templates
    /// </summary>
    public class TemplateNameEditor : UITypeEditor
    {
        /// <summary>
        /// Gets the name of the current category.
        /// </summary>
        /// <value>The name of the current category.</value>
        protected virtual string CurrentCategoryName
        {
            get { return "Templates for this Strategy"; }
        }

        /// <summary>
        /// Sélection des templates en prenant ceux du repository et ceux spécifiques à la strategie
        /// </summary>
        /// <param name="context"></param>
        /// <param name="provider"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            // Récupération des templates du repository
            List<string> templates = RepositoryManager.Instance.GetT4TemplatesList();

            // Puis ceux local à la stratégie
            StrategyBase strategy = context.Instance as StrategyBase;
            if (strategy != null)
            {
                DirectoryInfo di = new DirectoryInfo(strategy.MapPath(String.Empty));
                if (di.Exists)
                {
                    foreach (FileInfo fi in di.GetFiles("*.t4"))
                    {
                        templates.Add("0\\" + Path.GetFileNameWithoutExtension(fi.Name));
                    }
                }
            }

            FilterTemplates(templates);

            IWindowsFormsEditorService uiService =
                (IWindowsFormsEditorService) provider.GetService(typeof (IWindowsFormsEditorService));
            TemplateTreeView tv = new TemplateTreeView(uiService, templates, CurrentCategoryName);
            uiService.DropDownControl(tv);
            if (tv.SelectedName == null)
                return value;
            string val = tv.SelectedName;
            if (val.StartsWith(CurrentCategoryName) && val.Length > CurrentCategoryName.Length)
                return val.Substring(CurrentCategoryName.Length + 1);
            return tv.SelectedName;
        }

        /// <summary>
        /// Permet de filtrer les templates
        /// </summary>
        /// <param name="templates"></param>
        protected virtual void FilterTemplates(List<string> templates)
        {
        }

        /// <summary>
        /// Gets the editor style used by the <see cref="M:System.Drawing.Design.UITypeEditor.EditValue(System.IServiceProvider,System.Object)"></see> method.
        /// </summary>
        /// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext"></see> that can be used to gain additional context information.</param>
        /// <returns>
        /// A <see cref="T:System.Drawing.Design.UITypeEditorEditStyle"></see> value that indicates the style of editor used by the <see cref="M:System.Drawing.Design.UITypeEditor.EditValue(System.IServiceProvider,System.Object)"></see> method. If the <see cref="T:System.Drawing.Design.UITypeEditor"></see> does not support this method, then <see cref="M:System.Drawing.Design.UITypeEditor.GetEditStyle"></see> will return <see cref="F:System.Drawing.Design.UITypeEditorEditStyle.None"></see>.
        /// </returns>
        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.DropDown;
        }

        #region Nested type: TemplateTreeView

        /// <summary>
        /// 
        /// </summary>
        private class TemplateTreeView : TreeView
        {
            private readonly IWindowsFormsEditorService _edSvc;
            private readonly string currentCategoryName;

            public TemplateTreeView(IWindowsFormsEditorService editorService, List<string> data,
                                    string currentCategoryName)
            {
                this.currentCategoryName = currentCategoryName;
                _edSvc = editorService;
                Width = 400;
                PopulateData(data);
            }

            /// <summary>
            /// Gets the name of the selected.
            /// </summary>
            /// <value>The name of the selected.</value>
            public string SelectedName
            {
                get { return SelectedNode != null && ((bool) SelectedNode.Tag) ? SelectedNode.FullPath : null; }
            }

            /// <summary>
            /// Raises the <see cref="E:System.Windows.Forms.TreeView.NodeMouseClick"></see> event.
            /// </summary>
            /// <param name="e">A <see cref="T:System.Windows.Forms.TreeNodeMouseClickEventArgs"></see> that contains the event data.</param>
            protected override void OnNodeMouseClick(TreeNodeMouseClickEventArgs e)
            {
                base.OnNodeMouseClick(e);
                if (e.Node != null && ((bool) e.Node.Tag))
                    _edSvc.CloseDropDown();
            }

            /// <summary>
            /// Remplissage du treeview en créant un regroupement à chaque
            /// séparateur
            /// </summary>
            /// <param name="data">Données sous la forme grand pere/pere/fils</param>
            private void PopulateData(List<string> data)
            {
                // D'abord on tri la liste
                data.Sort();
                List<TreeNode> hierarchy = new List<TreeNode>();

                // Puis on rajoute au treeview
                foreach (string item in data)
                {
                    string[] parts = item.Split(Path.DirectorySeparatorChar);
                    for (int i = 0; i < parts.Length; i++)
                    {
                        if (i == 0 && parts[0] == "0")
                            parts[0] = currentCategoryName;

                        if (i < hierarchy.Count && parts[i] != hierarchy[i].Text)
                            hierarchy.RemoveRange(i, hierarchy.Count - i);

                        if (i >= hierarchy.Count)
                        {
                            TreeNode node = new TreeNode(parts[i]);
                            node.Tag = (i == parts.Length - 1); // est ce que c'est le noeud final ?
                            TreeNodeCollection parent = i > 0 ? hierarchy[i - 1].Nodes : Nodes;
                            parent.Add(node);
                            hierarchy.Add(node);
                        }
                    }
                }
                ExpandAll();
            }
        }

        #endregion
    }

    ///// <summary>
    ///// Editeur pour les templates d'interceptions
    ///// </summary>
    ///// <remarks>
    ///// Les templates se trouvent dans le répertoire 'Interceptors'
    ///// </remarks>
    //public class InterceptorTemplateNameEditor : TemplateNameEditor
    //{
    //    private const string InterceptorsCategoryName = "Interceptors";

    //    protected override string CurrentCategoryName
    //    {
    //        get { return InterceptorsCategoryName; }
    //    }

    //    /// <summary>
    //    /// Filtre sur le nom du répertoire
    //    /// </summary>
    //    /// <param name="templates"></param>
    //    protected override void FilterTemplates(List<string> templates)
    //    {
    //        templates.RemoveAll(delegate(string templateName)
    //                                {
    //                                    string[] parts = templateName.Split(Path.DirectorySeparatorChar);

    //                                    return !(parts.Length == 2
    //                                             && Utils.StringCompareEquals(parts[0], InterceptorsCategoryName)
    //                                             || !(parts.Length > 2 && parts[0] == "0"
    //                                                  && Utils.StringCompareEquals(parts[1], InterceptorsCategoryName))
    //                                            );
    //                                });
    //    }
    //}
}