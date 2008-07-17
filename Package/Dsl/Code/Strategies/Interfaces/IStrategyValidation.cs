using System;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Validation;

namespace DSLFactory.Candle.SystemModel.Strategies
{
    /// <summary>
    /// Stratégie de validation
    /// </summary>
    [CLSCompliant(false)]
    public interface IStrategyValidator
    {
        /// <summary>
        /// Appelé lors de la validation.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <param name="logInfo">The log info.</param>
        void Validate(ModelElement element, ValidationContext logInfo);
    }
}