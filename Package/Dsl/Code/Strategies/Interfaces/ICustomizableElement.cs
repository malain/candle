using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.Modeling;

namespace DSLFactory.Candle.SystemModel.Strategies
{
    // TODO voir avec StrategyCandidateElement
    /// <summary>
    /// 
    /// </summary>
    [CLSCompliant(true)]
    public interface ICustomizableElement
    {
        /// <summary>
        /// Gets the store.
        /// </summary>
        /// <value>The store.</value>
        Store Store { get; }

        /// <summary>
        /// Gets the owner.
        /// </summary>
        /// <value>The owner.</value>
        ICustomizableElement Owner { get; }

        /// <summary>
        /// Nom de l'�l�ment
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Nom complet de l'�l�ment
        /// </summary>
        string FullName { get; }

        /// <summary>
        /// Instance unique
        /// </summary>
        Guid Id { get; }

        /// <summary>
        /// Gets the strategies owner.
        /// </summary>
        /// <value>The strategies owner.</value>
        CandleElement StrategiesOwner { get; }

        /// <summary>
        /// Liste des strat�gies concernant ce mod�le
        /// </summary>
        List<StrategyBase> GetStrategies(bool specific);

        /// <summary>
        /// Propri�t�s personnalis�es pour une strat�gie associ�es � l'instance du mod�le
        /// </summary>
        /// <param name="strategyId">The strategy id.</param>
        /// <param name="propertyName">Name of the property.</param>
        /// <param name="createIfNotExists">if set to <c>true</c> [create if not exists].</param>
        /// <returns></returns>
        DependencyProperty GetStrategyCustomProperty(string strategyId, string propertyName, bool createIfNotExists);

        /// <summary>
        /// Execution d'un wizard lors de la cr�ation d'un �l�ment
        /// </summary>
        /// <param name="element">The element.</param>
        /// <param name="defaultWizard">The default wizard.</param>
        /// <returns></returns>
        bool ExecuteWizard(ModelElement element, IStrategyWizard defaultWizard);
    }
}