using DSLFactory.Candle.SystemModel.Strategies;
using Microsoft.VisualStudio.Modeling;

namespace DSLFactory.Candle.SystemModel.Rules
{
    /// <summary>
    /// Modification du nom du composant met à jour les namespaces
    /// </summary>
    [RuleOn(typeof (SoftwareComponent), FireTime=TimeToFire.TopLevelCommit, InitiallyDisabled=false)]
    public class SoftwareComponentChangeRule : ChangeRule
    {
        /// <summary>
        /// Alerts listeners that a property for an element has changed.
        /// </summary>
        /// <param name="e">Provides data for the ElementPropertyChanged event.</param>
        public override void ElementPropertyChanged(ElementPropertyChangedEventArgs e)
        {
            SoftwareComponent model = e.ModelElement as SoftwareComponent;
            if (model == null)
                return;

            if (e.DomainProperty.Id == SoftwareComponent.NamespaceDomainPropertyId)
            {
                OnNameChanged(e, model);
            }
        }

        private static void OnNameChanged(ElementPropertyChangedEventArgs e, SoftwareComponent model)
        {
            string oldName = (string) e.OldValue;

            if (oldName != "?" &&
                (model.Store.TransactionManager.CurrentTransaction.TopLevelTransaction.IsSerializing ||
                 model.Store.InUndoRedoOrRollback))
                return;

            string newName = (string) e.NewValue;

            foreach (SoftwareLayer layer in model.Layers)
            {
                if (oldName == "?")
                {
                    if (layer.Name == "?")
                    {
                        LayerPackage lp = null;
                        if (layer is Layer)
                            lp = ((Layer) layer).LayerPackage;
                        layer.Name =
                            StrategyManager.GetInstance(layer.Store).NamingStrategy.CreateLayerName(lp, layer, null);
                        // Le namespace sera mis à jour via la regle LayerNameChangeRule.
                    }
                    else
                        layer.Namespace =
                            StrategyManager.GetInstance(model.Store).NamingStrategy.CreateNamespace(newName, layer.Name,
                                                                                                    layer);
                }
                else if (layer.Namespace.StartsWith(oldName))
                    layer.Namespace = newName + layer.Namespace.Substring(oldName.Length);
            }
        }
    }
}