using Microsoft.VisualStudio.Modeling.Validation;

namespace DSLFactory.Candle.SystemModel
{
    /// <summary>
    /// 
    /// </summary>
    [ValidationState(ValidationState.Enabled)]
    public partial class DataLayer
    {
        // Define when the validation occurs
        /// <summary>
        /// Validates the namespace collisions.
        /// </summary>
        /// <param name="context">The context.</param>
        [ValidationMethod(ValidationCategories.Save | ValidationCategories.Menu)]
        protected void ValidateNamespaceCollisions(ValidationContext context)
        {
            string msg = "Namespace must be unique.";
            Package packageInError = null;

            foreach (Package package in Packages)
            {
                foreach (Package package2 in Packages)
                {
                    if (package.Id != package2.Id && Utils.StringCompareEquals(package2.Name, package.Name))
                    {
                        packageInError = package;
                        break;
                    }
                }
                if (packageInError != null)
                    break;
            }

            if (packageInError != null)
            {
                context.LogError(
                    msg,
                    "1", // Unique error number
                    packageInError);
            }
        }
    }
}