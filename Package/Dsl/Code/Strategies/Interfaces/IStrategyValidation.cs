using System;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Validation;

namespace DSLFactory.Candle.SystemModel.Strategies
{
    /// <summary>
    /// Strat�gie de validation
    /// </summary>
    [CLSCompliant(false)]
    public interface IStrategyValidator
    {
        /// <summary>
        /// Appel� lors de la validation.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <param name="logInfo">The log info.</param>
        void Validate(ModelElement element, ValidationContext logInfo);
    }
}