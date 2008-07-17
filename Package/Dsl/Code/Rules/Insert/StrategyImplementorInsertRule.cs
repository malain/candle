using DSLFactory.Candle.SystemModel.Rules.Wizards;
using DSLFactory.Candle.SystemModel.Strategies;
using Microsoft.VisualStudio.Modeling;

namespace DSLFactory.Candle.SystemModel.Rules
{
    /// <summary>
    /// Règle appelée lors de l'ajout d'un modèle. Permet de donner la main aux stratégies lors de l'ajout
    /// pour initialiser le modèle
    /// </summary>
    [RuleOn(typeof (CandleElement), FireTime=TimeToFire.LocalCommit, InitiallyDisabled=false)]
    // Puisqu'on ne peut pas faire hériter une connection, on prend ElementLink
    [RuleOn(typeof (ElementLink), FireTime=TimeToFire.TopLevelCommit)]
    public class CustomizableElementInsertRule : AddRule
    {
        // Ajout de l'élément
        /// <summary>
        /// Alerts listeners that a rule has been used.
        /// </summary>
        /// <param name="e">An ElementAddedEventArgs that contains the event data.</param>
        public override void ElementAdded(ElementAddedEventArgs e)
        {
            #region Condition

            // Test the element
            ICustomizableElement model = e.ModelElement as ICustomizableElement;
            if (model == null)
                return;

            if (
                e.ModelElement.Store.TransactionManager.CurrentTransaction.TopLevelTransaction.TopLevelTransaction.
                    IsSerializing || e.ModelElement.Store.InUndoRedoOrRollback
                ||
                e.ModelElement.Store.TransactionManager.CurrentTransaction.TopLevelTransaction.Context.ContextInfo.
                    ContainsKey(StrategyManager.IgnoreStrategyWizards))
                return;

            #endregion

            #region Traitement

            // A ce stade, on est dans une transaction
            IStrategyWizard defaultWizard = GetDefaultWizard(e.ModelElement);
            if (!model.ExecuteWizard(e.ModelElement, defaultWizard))
            {
                // L'utilisateur a annulé
                throw new CanceledByUser();
            }

            #endregion
        }

        private IStrategyWizard GetDefaultWizard(ModelElement model)
        {
            // On désactive sur le contract, on laisse faire la regle qui 
            // force le nom du contrat par rapport au nom de la classe (ClassNameChangeRule)
            if ( /*model is ServiceContract ||*/ model is ClassImplementation)
                // Wizard qui crée l'interface associée à l'implémentation
                return new NewServiceImplementation();

            if (model is Entity)
            {
                // On est en train de créer cette entité à partir de la fenêtre détail (voir SoftwareComponent.NormalizeTypeName)
                if (model.Store.TransactionManager.CurrentTransaction.Context.ContextInfo.ContainsKey("Generated"))
                    return null;
                Entity entity = model as Entity;
                DataAccessLayer dal =
                    (DataAccessLayer)
                    entity.DataLayer.Component.Layers.Find(
                        delegate(SoftwareLayer layer) { return layer is DataAccessLayer; });
                if (dal != null)
                    return new DAOSelectorDlg(dal, entity);
            }

            return null;
        }
    }
}