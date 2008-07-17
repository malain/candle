using System;
using DSLFactory.Candle.SystemModel.Strategies;
using Microsoft.VisualStudio.Modeling.Validation;

namespace DSLFactory.Candle.SystemModel
{
    [ValidationState(ValidationState.Enabled)]
    public partial class Layer
    {
        // Define when the validation occurs
        /// <summary>
        /// Validates the name.
        /// </summary>
        /// <param name="context">The context.</param>
        [ValidationMethod(ValidationCategories.Save | ValidationCategories.Menu)]
        protected void ValidateName(ValidationContext context)
        {
            if (String.IsNullOrEmpty(Name))
                return; // voir validation pour NamedElement

            string msg = "Name is not valid ({0}).";
            bool isValid = false;
            try
            {
                // Set the test boolean to true, if validation is correct.
                isValid = StrategyManager.GetInstance(Store).NamingStrategy.IsClassNameValid(Name);
            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }

            if (!isValid)
            {
                context.LogError(
                    String.Format(msg, Name),
                    "InvalidName1", // Unique error number
                    this);
            }
        }

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
                test = !String.IsNullOrEmpty(Namespace) &&
                       StrategyManager.GetInstance(Store).NamingStrategy.IsNamespaceValid(Namespace);
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
                     !Namespace.StartsWith(StrategyManager.GetInstance(Store).NamingStrategy.DefaultNamespace))
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