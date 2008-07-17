using Microsoft.VisualStudio.Modeling;

namespace DSLFactory.Candle.SystemModel.Rules
{
    // Si la valeur change, on force la propriété HasValue à true
    /// <summary>
    /// 
    /// </summary>
    [RuleOn(typeof (EnumValue), FireTime=TimeToFire.TopLevelCommit, InitiallyDisabled=false)]
    public class EnumValueChangeRule : ChangeRule
    {
        /// <summary>
        /// Alerts listeners that a property for an element has changed.
        /// </summary>
        /// <param name="e">Provides data for the ElementPropertyChanged event.</param>
        public override void ElementPropertyChanged(ElementPropertyChangedEventArgs e)
        {
            #region Condition

            // Cast on the element
            EnumValue model = e.ModelElement as EnumValue;
            if (model == null)
                return;

            if (model.Store.TransactionManager.CurrentTransaction.TopLevelTransaction.IsSerializing ||
                model.Store.InUndoRedoOrRollback)
                return;

            #endregion

            #region Traitement

            // Here, we are in an active transaction

            // Check the property
            if (e.DomainProperty.Id != EnumValue.ValueDomainPropertyId)
                return;

            if ((int) e.OldValue != (int) e.NewValue)
                model.HasValue = true;

            #endregion
        }
    }
}