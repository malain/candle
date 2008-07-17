using DSLFactory.Candle.SystemModel.Repository;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Diagrams;

namespace DSLFactory.Candle.SystemModel.Commands
{
    /// <summary>
    /// Affectation d'un modele sur un system externe vide
    /// </summary>
    public class ModelAffectationCommand : ICommand
    {
        private readonly NodeShape _shape;

        /// <summary>
        /// Initializes a new instance of the <see cref="ModelAffectationCommand"/> class.
        /// </summary>
        /// <param name="shape">The shape.</param>
        public ModelAffectationCommand(object shape)
        {
            _shape = shape as NodeShape;
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
        /// Execute the command
        /// </summary>
        public void Exec()
        {
            ExternalComponent system = _shape.ModelElement as ExternalComponent;
            if (system == null)
                return;

            using (Transaction transaction = system.Store.TransactionManager.BeginTransaction("Model affectation"))
            {
                ComponentModelMetadata metaData;
                if (!RepositoryManager.Instance.ModelsMetadata.SelectModel(system, out metaData))
                {
                    // On annule toute l'insertion
                    system.Store.TransactionManager.CurrentTransaction.Rollback();
                    return;
                }
                transaction.Commit();
            }
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="ICommand"/> is visible.
        /// </summary>
        /// <returns></returns>
        /// <value><c>true</c> if visible; otherwise, <c>false</c>.</value>
        public bool Visible()
        {
            if (_shape == null)
                return false;

            if (_shape.ModelElement is ExternalComponent)
            {
                if (((ExternalComponent) _shape.ModelElement).MetaData == null)
                    return true;
            }
            return false;
        }

        #endregion
    }
}