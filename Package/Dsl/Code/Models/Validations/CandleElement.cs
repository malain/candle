using DSLFactory.Candle.SystemModel.Strategies;
using Microsoft.VisualStudio.Modeling.Validation;

namespace DSLFactory.Candle.SystemModel
{
    /// <summary>
    /// 
    /// </summary>
    [ValidationState(ValidationState.Enabled)]
    public partial class CandleElement
    {
        // Define when the validation occurs
        /// <summary>
        /// Validates the strategies.
        /// </summary>
        /// <param name="context">The context.</param>
        [ValidationMethod(ValidationCategories.Save | ValidationCategories.Menu)]
        protected void ValidateStrategies(ValidationContext context)
        {
            //doublon du naming
            foreach (StrategyBase strategy in GetStrategies(false))
            {
                IStrategyValidator sv = strategy as IStrategyValidator;
                if (sv != null)
                    sv.Validate(this, context);
            }
        }
    }
}