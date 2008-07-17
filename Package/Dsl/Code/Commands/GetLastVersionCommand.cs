using System.Collections.Generic;
using DSLFactory.Candle.SystemModel.Dependencies;
using DSLFactory.Candle.SystemModel.Repository;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Diagrams;

namespace DSLFactory.Candle.SystemModel.Commands
{
    /// <summary>
    /// Importation d'assemblies externes
    /// </summary>
    public class GetLastVersionCommand : ICommand
    {
        private readonly CandleModel _externalModel;
        private readonly ExternalComponent _externalComponent;
        private readonly bool _force;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetLastVersionCommand"/> class.
        /// </summary>
        /// <param name="shape">The shape.</param>
        public GetLastVersionCommand(object shape)
        {
            if (shape != null && shape is PresentationElement)
            {
                _externalComponent = ((PresentationElement)shape).ModelElement as ExternalComponent;
                if (_externalComponent != null)
                {
                    _externalModel = _externalComponent.ReferencedModel;
                    this._force = _externalComponent.MetaData.IsLastVersion();
                }
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GetLastVersionCommand"/> class.
        /// </summary>
        /// <param name="metadata">The metadata.</param>
        /// <param name="force">if set to <c>true</c> [force].</param>
        public GetLastVersionCommand(ComponentModelMetadata metadata, bool force)
        {
            if (metadata != null)
            {
                this._force = force;
                ModelLoader loader = ModelLoader.GetLoader(metadata);
                if (loader != null)
                    _externalModel = loader.Model;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="ICommand"/> is visible.
        /// </summary>
        /// <returns></returns>
        /// <value><c>true</c> if visible; otherwise, <c>false</c>.</value>
        public bool Visible()
        {
            return true;
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="ICommand"/> is enabled.
        /// </summary>
        /// <value><c>true</c> if enabled; otherwise, <c>false</c>.</value>
        public bool Enabled
        {
            get
            {
                return _externalModel != null && _externalModel.MetaData != null;
            }
        }

        /// <summary>
        /// Execute the command
        /// </summary>
        public void Exec()
        {
            if (!Enabled)
                return;

            // 
            ReferenceWalker walker = new ReferenceWalker(ReferenceScope.All, new ConfigurationMode());
            ReferenceVisitor visitor = new ReferenceVisitor(ReferenceScope.All);
            walker.Traverse(visitor, _externalModel);
            List<CandleModel> models = new List<CandleModel>();
            foreach (CandleModel model in visitor.Models)
            {
                if (_force || model.MetaData.IsLastVersion() == false)
                    models.Add(model);
            }

            SelectModelForm form = new SelectModelForm(models, visitor.Models);
            if (form.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                foreach (CandleModel model in form.SelectedModels)
                {
                    RepositoryManager.Instance.ModelsMetadata.GetModelInLocalRepository(model.MetaData);
                }

                if (_externalComponent != null)
                {
                    using (Transaction transaction = _externalComponent.Store.TransactionManager.BeginTransaction("Update ports"))
                    {
                        if( _externalComponent.UpdateFromModel() )
                            transaction.Commit();
                    }
                }
            }
        }
    }
}
