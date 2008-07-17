using Microsoft.VisualStudio.Modeling;

namespace DSLFactory.Candle.SystemModel.Rules
{
    /// <summary>
    /// Quand on supprime un élément qui est lié à un fichier généré on l'enregistre pour
    /// pouvoir supprimer physiquement le fichier au moment de la sauvegarde.
    /// </summary>
    [RuleOn(typeof (CandleElement), FireTime = TimeToFire.TopLevelCommit)]
    public class CustomizableElementDeleteRule : DeleteRule
    {
        /// <summary>
        /// </summary>
        /// <param name="e">Provides data for the ElementDeleted event.</param>
        public override void ElementDeleted(ElementDeletedEventArgs e)
        {
            CandleElement elem = e.ModelElement as CandleElement;
            if (elem == null)
                return;

            if (elem.Store.InUndoRedoOrRollback)
                return;

            CandleModel.GetInstance(elem.Store).RegisterElementPendingDelete(elem);
        }
    }
}