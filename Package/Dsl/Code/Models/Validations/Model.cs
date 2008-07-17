using DSLFactory.Candle.SystemModel.Dependencies;
using Microsoft.VisualStudio.Modeling.Validation;

namespace DSLFactory.Candle.SystemModel
{
    [ValidationState(ValidationState.Enabled)]
    public partial class CandleModel
    {
        /// <summary>
        /// Cette méthode est appelée à la validation et son appel est forcée lors de l'ouverture du modèle.
        /// </summary>
        /// <param name="context"></param>
        [ValidationMethod(ValidationCategories.Save | ValidationCategories.Menu | ValidationCategories.Open)]
        internal void ValidateReferences(ValidationContext context)
        {
            //// Synchronization des metadonnées des composants externes
            //// et récupération des modèles (pas les sous-modèles) si il ne sont pas en local
            //try
            //{
            //    DSLFactory.Candle.SystemModel.Repository.RepositoryManager.Instance.ModelsMetadata.SynchronizeExternalComponentsMetadata(this);
            //}
            //catch (Exception ex)
            //{
            //    ILogger logger = ServiceLocator.Instance.GetService<ILogger>();
            //    if (logger != null)
            //        logger.WriteError("Validate model", "Synchronize external models", ex);
            //}

            // Validation si tous les modèles et sous-modéles sont à jour.
            ReferencesHelper.CheckReferences(false, context, new ConfigurationMode(), ReferenceScope.Runtime, this);
        }
    }
}