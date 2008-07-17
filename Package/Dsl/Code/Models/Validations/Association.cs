using System;
using DSLFactory.Candle.SystemModel.Strategies;
using Microsoft.VisualStudio.Modeling.Validation;

namespace DSLFactory.Candle.SystemModel
{
    [ValidationState(ValidationState.Enabled)]
    partial class Association
    {
        /// <summary>
        /// Validates the role names.
        /// </summary>
        /// <param name="context">The context.</param>
        [ValidationMethod(ValidationCategories.Save | ValidationCategories.Menu)]
        protected void ValidateRoleNames(ValidationContext context)
        {
            if (TargetMultiplicity != Multiplicity.NotApplicable)
            {
                if (String.IsNullOrEmpty(TargetRoleName))
                {
                    context.LogError(
                        String.Format("Target role name required for relation between {0} & {1}", Source.Name,
                                      Target.Name), "REL001", this);
                }
                else if (!StrategyManager.GetInstance(Store).NamingStrategy.IsClassNameValid(TargetRoleName))
                    context.LogError(
                        String.Format("Invalid target role name for relation between {0} & {1}", Source.Name,
                                      Target.Name), "REL002", this);
            }

            if (SourceMultiplicity != Multiplicity.NotApplicable)
            {
                if (String.IsNullOrEmpty(SourceRoleName))
                {
                    context.LogError(
                        String.Format("Target role name required for relation between {0} & {1}", Source.Name,
                                      Target.Name), "REL001", this);
                }
                else if (!StrategyManager.GetInstance(Store).NamingStrategy.IsClassNameValid(SourceRoleName))
                    context.LogError(
                        String.Format("Invalid target role name for relation between {0} & {1}", Source.Name,
                                      Target.Name), "REL002", this);
            }
        }
    }
}