using System.Collections.Generic;
using System.ComponentModel;
using DSLFactory.Candle.SystemModel.CodeGeneration;
using DSLFactory.Candle.SystemModel.Strategies;
using Microsoft.VisualStudio.Modeling;

namespace DSLFactory.Candle.SystemModel
{
    /// <summary>
    /// 
    /// </summary>
    [TypeDescriptionProvider(typeof (StrategyProviderTypeDescriptorProvider))]
    partial class Action : ICustomizableElement
    {
        #region ICustomizableElement Members

        /// <summary>
        /// Execution d'un wizard lors de la création d'un élément
        /// </summary>
        /// <param name="element">The element.</param>
        /// <param name="defaultWizard">The default wizard.</param>
        /// <returns></returns>
        public bool ExecuteWizard(ModelElement element, IStrategyWizard defaultWizard)
        {
            return true;
        }

        /// <summary>
        /// Liste des stratégies concernant ce modèle
        /// </summary>
        /// <param name="specific"></param>
        /// <returns></returns>
        public List<StrategyBase> GetStrategies(bool specific)
        {
            return StrategyManager.GetStrategies(StrategiesOwner, specific ? this : null);
        }

        /// <summary>
        /// Recherche la propriété personnalisé et la crée si elle n'existe pas
        /// </summary>
        /// <param name="strategyId">Identifiant de la stratégie</param>
        /// <param name="propertyName">Nom de la propriété</param>
        /// <param name="createIfNotExists">if set to <c>true</c> [create if not exists].</param>
        /// <returns></returns>
        public DependencyProperty GetStrategyCustomProperty(string strategyId, string propertyName,
                                                            bool createIfNotExists)
        {
            foreach (StrategyBase strategy in GetStrategies(false))
            {
                if (Utils.StringCompareEquals(strategy.StrategyId, strategyId))
                {
                    foreach (DependencyProperty property in DependencyProperties)
                    {
                        if (Utils.StringCompareEquals(property.Name, propertyName))
                            return property;
                    }
                }

                // Si pas trouvé, on crée
                if (createIfNotExists && strategy.CheckPropertyValid(this, propertyName))
                {
                    using (
                        Transaction transaction = Store.TransactionManager.BeginTransaction("Initialise property value")
                        )
                    {
                        DependencyProperty propertyInfo = new DependencyProperty(Store);
                        DependencyProperties.Add(propertyInfo);
                        propertyInfo.StrategyId = strategyId;
                        propertyInfo.Name = propertyName;
                        propertyInfo.Value = null;
                        transaction.Commit();
                        return propertyInfo;
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// Gets the strategies owner.
        /// </summary>
        /// <value>The strategies owner.</value>
        public CandleElement StrategiesOwner
        {
            get { return ViewSource.StrategiesOwner; }
        }

        /// <summary>
        /// Gets the owner.
        /// </summary>
        /// <value>The owner.</value>
        public ICustomizableElement Owner
        {
            get { return ViewSource; }
        }

        /// <summary>
        /// Nom complet de l'élément
        /// </summary>
        /// <value></value>
        public string FullName
        {
            get { return Name; }
        }

        #endregion

        /// <summary>
        /// Generates the code.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        internal virtual bool GenerateCode(GenerationContext context)
        {
            if (context.CanGenerate(Id))
            {
                Generator.ApplyStrategies(this, context);
                if (context.IsModelSelected(Id))
                    return true;
            }
            return false;
        }
    }
}