using Microsoft.VisualStudio.Modeling;

namespace DSLFactory.Candle.SystemModel.Rules
{
    /// <summary>
    /// Modification du nom de la connection lors du changement de nom d'un service
    /// </summary>
    [RuleOn(typeof (ServiceContract), FireTime=TimeToFire.TopLevelCommit, InitiallyDisabled=false)]
    public class LayerServicePortChangeRule : ChangeRule
    {
        /// <summary>
        /// Alerts listeners that a property for an element has changed.
        /// </summary>
        /// <param name="e">Provides data for the ElementPropertyChanged event.</param>
        public override void ElementPropertyChanged(ElementPropertyChangedEventArgs e)
        {
            #region Condition

            // Teste l'�l�ment
            ServiceContract model = e.ModelElement as ServiceContract;
            if (model == null)
                return;

            // Teste si on est en train de charger le mod�le
            // Permet d'eviter de d�clencher une r�gle lors du chargement du mod�le 
            if (model.Store.TransactionManager.CurrentTransaction.TopLevelTransaction.IsSerializing ||
                model.Store.InUndoRedoOrRollback)
                return;

            #endregion

            #region Traitement

            // Traitement suivant la propri�t�	
            if (e.DomainProperty.Id == ServiceContract.NameDomainPropertyId)
            {
                // Recherche si il y a une relation avec une couche
                foreach (Implementation relation in Implementation.GetLinksToImplementations(model))
                {
                    // Si il y en a une, on modifie son nom (si il n'a pas �t� forc�e)
                    if (relation.Name == (string) e.OldValue && relation.Name != (string) e.NewValue)
                        relation.Name = (string) e.NewValue;
                }
            }

            #endregion
        }
    }
}