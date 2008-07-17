using DSLFactory.Candle.SystemModel.Repository;
using Microsoft.VisualStudio.Modeling;

namespace DSLFactory.Candle.SystemModel.Rules
{
    /// <summary>
    /// Régle appelée lors de l'ajout d'un systeme externe. On affiche un wizard pour sélectionner le
    /// système dans le référentiel
    /// </summary>
    [RuleOn(typeof (ExternalComponent), FireTime = TimeToFire.LocalCommit)]
    public class ExternalComponentShapeInsertRule : AddRule
    {
        // Ajout de l'élément
        /// <summary>
        /// Alerts listeners that a rule has been used.
        /// </summary>
        /// <param name="e">An ElementAddedEventArgs that contains the event data.</param>
        public override void ElementAdded(ElementAddedEventArgs e)
        {
            ExternalComponent externalComponent = e.ModelElement as ExternalComponent;
            if (externalComponent == null)
                return;

            // Cette régle ne s'applique pas quand on charge le modèle
            if (externalComponent.Store.TransactionManager.CurrentTransaction.TopLevelTransaction.IsSerializing ||
                externalComponent.Store.InUndoRedoOrRollback)
                return;

            // Test si on est pas en train d'insèrer des assemblies externes (voir ImportAssemblyCommand)
            object dummy;
            if( externalComponent.Store.TransactionManager.CurrentTransaction.TopLevelTransaction.Context.ContextInfo.TryGetValue("Import assemblies", out dummy))
                return;

            // Insertion d'un composant à partir de la toolbar dans ce cas, on affiche
            // un wizard de sélection de modèle.
            // TODO a voir si pas redondant avec la fenetre du repository
            ComponentModelMetadata metadata;
            if (externalComponent.MetaData == null &&
                !RepositoryManager.Instance.ModelsMetadata.SelectModel(externalComponent, out metadata))
            {
                // On annule toute l'insertion
                externalComponent.Store.TransactionManager.CurrentTransaction.Rollback();
                return;
            }

            //// La synchro au chargement du modèle ne se fait que quand le modèle
            //// est chargé dans le designer (cf ComponentModelDocData.Load)
            ////RepositoryManager.SynchronizeExternalComponentFromServer(externalComponent);
        }
    }
}