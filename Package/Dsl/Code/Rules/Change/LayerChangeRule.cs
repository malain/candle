using Microsoft.VisualStudio.Modeling;

namespace DSLFactory.Candle.SystemModel.Rules
{
    /// <summary>
    /// 
    /// </summary>
    [RuleOn(typeof (Layer), FireTime=TimeToFire.TopLevelCommit, InitiallyDisabled=false)]
    public class LayerChangeRule : ChangeRule
    {
        /// <summary>
        /// Alerts listeners that a property for an element has changed.
        /// </summary>
        /// <param name="e">Provides data for the ElementPropertyChanged event.</param>
        public override void ElementPropertyChanged(ElementPropertyChangedEventArgs e)
        {
            #region Condition

            // Check the property
            if (e.DomainProperty.Id != Layer.HostingContextDomainPropertyId &&
                e.DomainProperty.Id != Layer.StartupProjectDomainPropertyId)
                return;

            // Cast on the element
            Layer model = e.ModelElement as Layer;
            if (model == null)
                return;

            if (model.Store.TransactionManager.CurrentTransaction.TopLevelTransaction.IsSerializing ||
                model.Store.InUndoRedoOrRollback)
                return;

            #endregion

            #region Traitement

            // Changement du HostingContext
            if (e.DomainProperty.Id == Layer.HostingContextDomainPropertyId)
            {
                // Quand le hostingContext change et passe à autre chose que None. On force toutes les autres 
                // couches qui ne sont pas du même niveau à None
                if (((HostingContext) e.NewValue) != HostingContext.None)
                {
                    // On vient de forcer une valeur
                    //foreach (AbstractLayer otherLayer in model.Component.Layers)
                    //{
                    //    if (model != otherLayer && otherLayer is Layer && model.Level != ((Layer)otherLayer).Level)
                    //        ((Layer)otherLayer).HostingContext = HostingContext.None;
                    //}
                }
                else
                {
                    // Si la couche courante est à None, on recherche la couche principale et on lui donne l'ancienne
                    // valeur (si il y avait None)
                    Layer mainLayer = model.SoftwareComponent.GetMainLayer();
                    if (mainLayer == null)
                    {
                        mainLayer = model.SoftwareComponent.SuggestMainLayer(model);
                        if (mainLayer != null)
                            mainLayer.StartupProject = true;
                    }
                    if (mainLayer != null && mainLayer != model && mainLayer.HostingContext == HostingContext.None)
                        mainLayer.HostingContext = (HostingContext) e.OldValue;
                }
            }

            // Changement de la propriété StartupProject
            if (e.DomainProperty.Id == Layer.StartupProjectDomainPropertyId)
            {
                if ((bool) e.NewValue)
                {
                    // On force tous les autres à false
                    foreach (AbstractLayer otherLayer in model.Component.Layers)
                    {
                        if (model != otherLayer && otherLayer is Layer)
                            ((Layer) otherLayer).StartupProject = false;
                    }
                }
                else
                {
                    Layer mainLayer = model.SoftwareComponent.SuggestMainLayer(model);
                    if (mainLayer != null)
                        mainLayer.StartupProject = true;
                }
            }

            #endregion
        }
    }
}