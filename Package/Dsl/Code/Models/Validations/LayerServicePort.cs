using System;
using DSLFactory.Candle.SystemModel.Strategies;
using Microsoft.VisualStudio.Modeling.Validation;

namespace DSLFactory.Candle.SystemModel
{
    [ValidationState(ValidationState.Enabled)]
    public partial class ClassImplementation
    {
        // Define when the validation occurs
        /// <summary>
        /// Checks the name.
        /// </summary>
        /// <param name="context">The context.</param>
        [ValidationMethod(ValidationCategories.Save | ValidationCategories.Menu)]
        protected void CheckName(ValidationContext context)
        {
            string msg = "Invalid className";
            bool isValid = false;
            try
            {
                // Set the test boolean to true, if validation is correct.
                isValid = !String.IsNullOrEmpty(Name) &&
                          StrategyManager.GetInstance(Store).NamingStrategy.IsClassNameValid(Name);
            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }

            if (!isValid)
            {
                context.LogError(
                    msg,
                    "1", // Unique error number
                    this);
            }
        }
    }
}