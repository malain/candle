using DSLFactory.Candle.SystemModel.Dependencies;
using Microsoft.VisualStudio.Modeling.Validation;

namespace DSLFactory.Candle.SystemModel
{
    [ValidationState(ValidationState.Enabled)]
    public partial class CandleModel
    {
        /// <summary>
        /// Cette m�thode est appel�e � la validation et son appel est forc�e lors de l'ouverture du mod�le.
        /// </summary>
        /// <param name="context"></param>
        [ValidationMethod(ValidationCategories.Save | ValidationCategories.Menu | ValidationCategories.Open)]
        internal void ValidateReferences(ValidationContext context)
        {
            //// Synchronization des metadonn�es des composants externes
            //// et r�cup�ration des mod�les (pas les sous-mod�les) si il ne sont pas en local
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

            // Validation si tous les mod�les et sous-mod�les sont � jour.
            ReferencesHelper.CheckReferences(false, context, new ConfigurationMode(), ReferenceScope.Runtime, this);
        }
    }
}