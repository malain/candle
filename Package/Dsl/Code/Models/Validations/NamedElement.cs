using System;
using Microsoft.VisualStudio.Modeling.Validation;

namespace DSLFactory.Candle.SystemModel
{
    [ValidationState(ValidationState.Enabled)]
    public partial class NamedElement
    {
        // Define when the validation occurs
        /// <summary>
        /// Validates the name not null.
        /// </summary>
        /// <param name="context">The context.</param>
        [ValidationMethod(ValidationCategories.Save | ValidationCategories.Menu)]
        protected void ValidateNameNotNull(ValidationContext context)
        {
            string msg = "Name required.";
            bool isValid = false;
            try
            {
                // Set the test boolean to true, if validation is correct.
                isValid = !String.IsNullOrEmpty(Name);
                    // && StrategyManager.GetInstance(clazz.Store).NamingStrategy.IsClassNameValid( this.Name );        
            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }

            if (!isValid)
            {
                context.LogError(
                    msg,
                    "InvalidName1", // Unique error number
                    this);
            }
        }
    }
}