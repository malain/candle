using System.Collections.Generic;
using System.ComponentModel;
using DSLFactory.Candle.SystemModel.Strategies;
using Microsoft.VisualStudio.Modeling;

namespace DSLFactory.Candle.SystemModel
{
    /// <summary>
    /// 
    /// </summary>
    [TypeDescriptionProvider(typeof (StrategyProviderTypeDescriptorProvider))]
    partial class ClassUsesOperations : ICustomizableElement
    {
        /// <summary>
        /// Gets the internal target service.
        /// </summary>
        /// <value>The internal target service.</value>
        public ServiceContract InternalTargetService
        {
            get { return TargetService as ServiceContract; }
        }

        /// <summary>
        /// Gets the internal target class.
        /// </summary>
        /// <value>The internal target class.</value>
        public ClassImplementation InternalTargetClass
        {
            get { return TargetService as ClassImplementation; }
        }

        /// <summary>
        /// Gets the external target service.
        /// </summary>
        /// <value>The external target service.</value>
        public ExternalServiceContract ExternalTargetService
        {
            get { return TargetService as ExternalServiceContract; }
        }

        #region ICustomizableElement Membres

        /// <summary>
        /// Gets the owner.
        /// </summary>
        /// <value>The owner.</value>
        public ICustomizableElement Owner
        {
            get { return Source; }
        }

        /// <summary>
        /// Nom complet de l'�l�ment
        /// </summary>
        /// <value></value>
        public string FullName
        {
            get { return Name; }
        }

        /// <summary>
        /// Gets the strategies owner.
        /// </summary>
        /// <value>The strategies owner.</value>
        public CandleElement StrategiesOwner
        {
            get { return Source.StrategiesOwner; }
        }

        /// <summary>
        /// Liste des strat�gies concernant ce mod�le
        /// </summary>
        /// <param name="specific"></param>
        /// <returns></returns>
        public List<StrategyBase> GetStrategies(bool specific)
        {
            return StrategyManager.GetStrategies(StrategiesOwner, specific ? this : null);
        }

        /// <summary>
        /// Recherche la propri�t� personnalis� et la cr�e si elle n'existe pas
        /// </summary>
        /// <param name="strategyId">Identifiant de la strat�gie</param>
        /// <param name="propertyName">Nom de la propri�t�</param>
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

                // Si pas trouv�, on cr�e
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
        /// Execution d'un wizard lors de la cr�ation d'un �l�ment
        /// </summary>
        /// <param name="element">The element.</param>
        /// <param name="defaultWizard">The default wizard.</param>
        /// <returns></returns>
        public bool ExecuteWizard(ModelElement element, IStrategyWizard defaultWizard)
        {
            return true;
        }

        #endregion
    }
}