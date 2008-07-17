using System;
using Microsoft.VisualStudio.Modeling.Validation;

namespace DSLFactory.Candle.SystemModel.Dependencies
{
    /// <summary>
    /// 
    /// </summary>
    public class CheckReferenceVisitor : ReferenceVisitor
    {
        private readonly ValidationContext _validationContext;
        private readonly bool _loadIfNotExistsLocally;

        /// <summary>
        /// Initializes a new instance of the <see cref="CheckReferenceVisitor"/> class.
        /// </summary>
        /// <param name="loadIfNotExistsLocally">if set to <c>true</c> [load if not exists locally].</param>
        /// <param name="scope">The scope.</param>
        /// <param name="valCtx">The val CTX.</param>
        public CheckReferenceVisitor(bool loadIfNotExistsLocally, ReferenceScope scope, ValidationContext valCtx)
            : base(scope)
        {
            _loadIfNotExistsLocally = loadIfNotExistsLocally;
            _validationContext = valCtx;
        }

        /// <summary>
        /// Logs the warning.
        /// </summary>
        /// <param name="msg">The MSG.</param>
        private void LogWarning(string msg)
        {
            if (_validationContext != null)
                _validationContext.LogWarning(msg, "DEP001");
            ILogger logger = ServiceLocator.Instance.GetService<ILogger>();
            if (logger != null)
                logger.Write("Checking reference", msg, LogType.Warning);
        }

        /// <summary>
        /// Appelée pour chaque modèle
        /// </summary>
        /// <param name="model"></param>
        /// <param name="isInitialModel"></param>
        protected override void AcceptModel(CandleModel model, bool isInitialModel)
        {
            // Vérification si les composants appelés sont à jour
            if (!isInitialModel)
            {
                if (!model.MetaData.IsLastVersion())
                {
                    if (!model.MetaData.ExistsLocally && _loadIfNotExistsLocally)
                    {
                        Repository.RepositoryManager.Instance.ModelsMetadata.GetModelInLocalRepository(model.MetaData);
                    }
                    else
                        LogWarning(
                            String.Format("Model {0} version {1} is not the last version.", model.Name, model.Version));
                }
            }

            foreach (CandleModel other in Models)
            {
                // Vérification si les n° de version sont compatibles
                if (other.Id == model.Id)
                {
                    if (!model.Version.Equals(other.Version))
                        LogWarning(
                            String.Format(
                                "Version dependency incompatibility with model '{0}'. Two differents versions are used {1} & {2}",
                                model.Name, model.Version, other.Version));
                }
                else
                {
                    // Et les frameworks
                    if (other.DotNetFrameworkVersion != model.DotNetFrameworkVersion)
                    {
                        // TODO
                        //                            LogWarning(String.Format("Framework version incompatibility between model '{0}' and model {1}. Two differents versions are used {2} & {3}", model.Name, other.Name, model.DotNetFrameworkVersion, other.DotNetFrameworkVersion));
                    }
                }
            }

            // Important en dernier
            base.AcceptModel(model, isInitialModel);
        }
    }
}