using System;
using DSLFactory.Candle.SystemModel.Repository;
using DSLFactory.Candle.SystemModel.Utilities;
using Microsoft.VisualStudio.Modeling.Diagrams;

namespace DSLFactory.Candle.SystemModel.Commands
{
    /// <summary>
    /// Publication du modele
    /// </summary>
    public class PublishModelCommand : ICommand
    {
        private readonly string _fileName=null;
        private CandleModel _model;

        /// <summary>
        /// Initializes a new instance of the <see cref="PublishModelCommand"/> class 
        /// � partir du menu g�n�ral
        /// </summary>
        /// <param name="serviceProvider">The service provider.</param>
        public PublishModelCommand(IServiceProvider serviceProvider)
        {
            ModelVisitor visitor = new ModelVisitor();
            VSHierarchyWalker walker =  new VSHierarchyWalker(visitor);
            walker.Traverse(serviceProvider);

            if (visitor.Models.Count > 0)
                _fileName = visitor.Models[0];
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PublishModelCommand"/> class.
        /// � partir du menu contectuel
        /// </summary>
        /// <param name="selection">The selection.</param>
        /// <param name="fileName">Name of the file.</param>
        public PublishModelCommand(object selection, string fileName)
        {
            this._fileName = fileName;
            NodeShape shape = selection as NodeShape;
            if (shape != null)
                this._model = shape.ModelElement as CandleModel;
        }

        #region ICommand Members
        /// <summary>
        /// Gets a value indicating whether this <see cref="ICommand"/> is enabled.
        /// </summary>
        /// <value><c>true</c> if enabled; otherwise, <c>false</c>.</value>
        public bool Enabled
        {
            get { return Visible(); }
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="ICommand"/> is visible.
        /// </summary>
        /// <returns></returns>
        /// <value><c>true</c> if visible; otherwise, <c>false</c>.</value>
        public bool Visible()
        {
            IShellHelper sh = ServiceLocator.Instance.GetService<IShellHelper>();
            if (String.IsNullOrEmpty(_fileName)  )
                return false;

            if (sh != null)
            {
                // On ne peut pas publier un mod�le qui n'est pas le mod�le courant 
                // sauf si c'est un composant binaire (pour permettre la mise � jour)
                if (_model != null && _model.BinaryComponent != null)
                    return true;

                // V�rif si c'est le mod�le associ� � la solution courante
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
            if (_model == null)
            {
                ModelLoader loader = ModelLoader.GetLoader(_fileName, true);
                if (loader != null)
                    _model = loader.Model;
            }

            if( !String.IsNullOrEmpty(_model.Path) && _model.Version != null && !String.IsNullOrEmpty(_model.Name) )
            {
                RepositoryManager.Instance.ModelsMetadata.PublishModel(_model, _fileName, true);
            }
            else
                ServiceLocator.Instance.IDEHelper.ShowMessage("The path, version and name properties must be provided to publish the model");
        }

        #endregion
    }
}
