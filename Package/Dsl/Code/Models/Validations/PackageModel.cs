using System;
using DSLFactory.Candle.SystemModel.Strategies;
using Microsoft.VisualStudio.Modeling.Validation;

namespace DSLFactory.Candle.SystemModel
{
    [ValidationState(ValidationState.Enabled)]
    public partial class Package
    {
        // Define when the validation occurs
        /// <summary>
        /// Validates the namespace.
        /// </summary>
        /// <param name="context">The context.</param>
        [ValidationMethod(ValidationCategories.Save | ValidationCategories.Menu)]
        protected void ValidateNamespace(ValidationContext context)
        {
            string msg = "Invalid namespace";
            bool test = false;
            try
            {
                // Set the test boolean to true, if validation is correct.
                test = !String.IsNullOrEmpty(Name) &&
                       StrategyManager.GetInstance(Store).NamingStrategy.IsNamespaceValid(Name);
            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }

            if (!test)
            {
                context.LogError(
                    msg,
                    "1", // Unique error number
                    this);
            }
            else if (!String.IsNullOrEmpty(StrategyManager.GetInstance(Store).NamingStrategy.DefaultNamespace) &&
                     !Name.StartsWith(StrategyManager.GetInstance(Store).NamingStrategy.DefaultNamespace))
            {
                context.LogWarning(
                    String.Format("Namespace must begin with '{0}'",
                                  StrategyManager.GetInstance(Store).NamingStrategy.DefaultNamespace),
                    "2", // Unique error number
                    this);
            }
        }
    }
}