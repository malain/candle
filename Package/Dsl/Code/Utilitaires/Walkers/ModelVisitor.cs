using System.Collections.Generic;
using System.IO;
using EnvDTE;

namespace DSLFactory.Candle.SystemModel.Utilities
{
    /// <summary>
    /// Visitor permettant de récupérer tous les fichiers modèles dans la solution (s'utilise avec VSHierarchyWalker)
    /// </summary>
    public class ModelVisitor : IVSHierarchyVisitor
    {
        private List<string> _models = new List<string>();

        /// <summary>
        /// Gets or sets the models.
        /// </summary>
        /// <value>The models.</value>
        public List<string> Models
        {
            get { return _models; }
            set { _models = value; }
        }

        #region IVSHierarchyVisitor Members

        /// <summary>
        /// Accepts the specified project item.
        /// </summary>
        /// <param name="projectItem">The project item.</param>
        void IVSHierarchyVisitor.Accept(ProjectItem projectItem)
        {
            if (projectItem.FileCount > 0)
            {
                string modelFileName = projectItem.get_FileNames(1);
                if (Utils.StringCompareEquals(ModelConstants.FileNameExtension, Path.GetExtension(modelFileName)))
                    _models.Add(modelFileName);
            }
        }

        #endregion
    }
}