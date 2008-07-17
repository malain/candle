using System;
using DSLFactory.Candle.SystemModel.Strategies;

namespace DSLFactory.Candle.SystemModel.Commands
{
    /// <summary>
    /// Génération sur tous les éléments
    /// </summary>
    public class GenerateAllCommand : ICommand
    {
        private readonly string _fileName=null;
        private readonly IServiceProvider _serviceProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="GenerateAllCommand"/> class.
        /// </summary>
        /// <param name="serviceProvider">The service provider.</param>
        /// <param name="fileName">Name of the file.</param>
        public GenerateAllCommand(IServiceProvider serviceProvider, string fileName)
        {
            this._serviceProvider = serviceProvider;
            if (fileName != null)
                this._fileName = fileName;
            //else
            //{
            //    ModelVisitor visitor = new ModelVisitor();
            //    VSHierarchyWalker walker = new VSHierarchyWalker(visitor);
            //    walker.Traverse(serviceProvider);

            //    if (visitor.Models.Count > 0)
            //        fileName = visitor.Models[0];
            //}
        }

        #region ICommand Members
        /// <summary>
        /// Gets a value indicating whether this <see cref="ICommand"/> is enabled.
        /// </summary>
        /// <value><c>true</c> if enabled; otherwise, <c>false</c>.</value>
        public bool Enabled
        {
            get { return Visible() &&  StrategyManager.GetAvailableManifests().Count > 0; }
        }

        /// <summary>
        /// Vérification si la commande peut s'activer
        /// </summary>
        /// <returns></returns>
        /// <value><c>true</c> if visible; otherwise, <c>false</c>.</value>
        public bool Visible()
        {
            // Fichier null 
            if (String.IsNullOrEmpty(_fileName)) 
                return false;

            // Est on bien sur le modèle de la solution ?
            IShellHelper sh = ServiceLocator.Instance.GetService<IShellHelper>();
            if (sh != null)
            {
                string currentModel = sh.GetSolutionAssociatedModelName();
                if (currentModel != null && !Utils.StringCompareEquals(_fileName, currentModel))
                    return false;
            }
            return true;
        }

        /// <summary>
        /// Execute the command
        /// </summary>
        public void Exec()
        {
            DSLFactory.Candle.SystemModel.CodeGeneration.Generator.Generate(_serviceProvider, _fileName, null);
        }

        #endregion
    }
}
