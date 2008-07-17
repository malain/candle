using System;
using Microsoft.VisualStudio.Modeling;

namespace DSLFactory.Candle.SystemModel.Rules
{
    /// <summary>
    /// 
    /// </summary>
    [RuleOn(typeof (ExternalComponent), FireTime=TimeToFire.TopLevelCommit)]
    public class ExternalComponentChangeRule : ChangeRule
    {
        /// <summary>
        /// Alerts listeners that a property for an element has changed.
        /// </summary>
        /// <param name="e">Provides data for the ElementPropertyChanged event.</param>
        public override void ElementPropertyChanged(ElementPropertyChangedEventArgs e)
        {
            if (e.DomainProperty.Id == ExternalComponent.VersionDomainPropertyId)
            {
                ExternalComponent sys = e.ModelElement as ExternalComponent;
                if (sys != null && sys.ModelMoniker != null)
                {
                    object flag;
                    if (
                        sys.Store.TransactionManager.CurrentTransaction.Context.ContextInfo.TryGetValue(
                            "InExternalComponentChangeRule", out flag))
                    {
                        return;
                    }

                    sys.Store.TransactionManager.CurrentTransaction.Context.ContextInfo["InExternalComponentChangeRule"]
                        = true;

                    if (
                        sys.Store.TransactionManager.CurrentTransaction.TopLevelTransaction.Context.ContextInfo.
                            TryGetValue("InModelLoader", out flag))
                        return;

                    CandleModel model = sys.ReferencedModel;
                    if (model != null)
                    {
                        sys.UpdateFromModel();
                    }
                    else
                    {
                        if (String.IsNullOrEmpty(sys.Name))
                            sys.Name = "unknow model";
                    }
                }
            }
        }
    }
}