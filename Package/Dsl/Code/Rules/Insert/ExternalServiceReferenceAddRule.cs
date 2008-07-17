using Microsoft.VisualStudio.Modeling;

namespace DSLFactory.Candle.SystemModel.Rules
{
    /// <summary>
    /// Ajout d'une référence vers un composant externe.
    /// Si la référence pointe sur un composant qui n'est pas dans le GAC, on rajoute le scope runtime.
    /// </summary>
    [RuleOn(typeof (ExternalServiceReference), FireTime = TimeToFire.TopLevelCommit)]
    public class ExternalServiceReferenceAddRule : AddRule
    {
        // 
        /// <summary>
        /// Alerts listeners that a rule has been used.
        /// </summary>
        /// <param name="e">An ElementAddedEventArgs that contains the event data.</param>
        public override void ElementAdded(ElementAddedEventArgs e)
        {
            ExternalServiceReference link = e.ModelElement as ExternalServiceReference;
            if (link == null)
                return;

            // Cette régle ne s'applique pas quand on charge le modèle
            if (link.Store.TransactionManager.CurrentTransaction.TopLevelTransaction.IsSerializing &&
                !link.Store.InUndoRedoOrRollback)
                return;


            if (!link.ExternalPublicPort.IsInGac)
            {
                link.Scope |= ReferenceScope.Runtime;
            }
        }
    }
}