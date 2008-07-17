using Microsoft.VisualStudio.Modeling;

namespace DSLFactory.Candle.SystemModel.Rules
{
    /// <summary>
    /// Si c'est un lien interne, on force le scope
    /// </summary>
    [RuleOn(typeof (ClassUsesOperations), FireTime = TimeToFire.TopLevelCommit)]
    public class ClassUsesOperationsInsertRule : AddRule
    {
        // Ajout de l'élément
        /// <summary>
        /// Alerts listeners that a rule has been used.
        /// </summary>
        /// <param name="e">An ElementAddedEventArgs that contains the event data.</param>
        public override void ElementAdded(ElementAddedEventArgs e)
        {
            ClassUsesOperations link = e.ModelElement as ClassUsesOperations;
            if (link == null)
                return;

            // Cette régle ne s'applique pas quand on charge le modèle
            if (link.Store.TransactionManager.CurrentTransaction.TopLevelTransaction.IsSerializing ||
                link.Store.InUndoRedoOrRollback)
                return;

            // Mise à jour du nom du service
            link.Name = link.TargetService.Name;

            if (link.InternalTargetService != null)
                link.Scope = ReferenceScope.Compilation | ReferenceScope.Publish | ReferenceScope.Runtime;
        }
    }
}