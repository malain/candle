using DSLFactory.Candle.SystemModel.Commands;
using DSLFactory.Candle.SystemModel.Utilities.SchemaDiscover;
using Microsoft.VisualStudio.Modeling;

namespace DSLFactory.Candle.SystemModel.Rules
{
    /// <summary>
    /// 
    /// </summary>
    [RuleOn(typeof (Association), FireTime=TimeToFire.TopLevelCommit, InitiallyDisabled=false)]
    public class AssociationInsertRule : AddRule
    {
        /// <summary>
        /// Alerts listeners that a rule has been used.
        /// </summary>
        /// <param name="e">An ElementAddedEventArgs that contains the event data.</param>
        public override void ElementAdded(ElementAddedEventArgs e)
        {
            #region Condition

            // Test the element
            Association model = e.ModelElement as Association;
            if (model == null)
                return;

            // Teste si on est en train de charger le modèle
            if (model.Store.TransactionManager.CurrentTransaction.TopLevelTransaction.IsSerializing ||
                model.Store.InUndoRedoOrRollback)
                return;

            // Si on est en train d'importer un schéma, on n'affiche pas la fenetre
            object obj;
            if (
                model.Store.TransactionManager.CurrentTransaction.Context.ContextInfo.TryGetValue(
                    DatabaseImporter.ImportedRelationInfo, out obj))
                return;

            #endregion

            #region Traitement

            ShowAssociationPropertiesCommand cmd = new ShowAssociationPropertiesCommand(e.ModelElement);
            cmd.Exec();

            #endregion
        }
    }
}