using System;
using DSLFactory.Candle.SystemModel.Strategies;
using Microsoft.VisualStudio.Modeling;

namespace DSLFactory.Candle.SystemModel.Rules
{
    /// <summary>
    /// Mise à jour du nom du namespace et du nom du package Layer
    /// </summary>
    [RuleOn(typeof (SoftwareLayer), FireTime = TimeToFire.TopLevelCommit, InitiallyDisabled = false)]
    public class LayerNameChangeRule : ChangeRule
    {
        /// <summary>
        /// Alerts listeners that a property for an element has changed.
        /// </summary>
        /// <param name="e">Provides data for the ElementPropertyChanged event.</param>
        public override void ElementPropertyChanged(ElementPropertyChangedEventArgs e)
        {
            // Modifications possibles :
            // - Name
            // - AssemblyName
            // - VSProject
            // - Namespace
            //
            // Regles de dépendences
            // Name -> layerPackage.Name & interface.Name & VSProject 
            // Namespace -> component.Namespace & layer.Name|layer.VSProject
            // AssemblyName -> Name | Namespace |vsproject

            int pos;
            object flag;

            // Cast on the element
            SoftwareLayer layer = e.ModelElement as SoftwareLayer;
            if (layer == null || layer.Store.InUndoRedoOrRollback)
                return;

            // Modification du nom
            if (e.DomainProperty.Id == Layer.NameDomainPropertyId &&
                !layer.Store.TransactionManager.CurrentTransaction.TopLevelTransaction.IsSerializing)
            {
                // Le nom du project
                UpdateVsProject((string) e.OldValue, layer);

                // Le layer package si il n'est pas renseigné et
                // la couche d'interface si elle n'est pas renseignée
                UpdateInterfaceAndLayerPackageName(layer);

                // Modif du namespace
                UpdateNamespace((string) e.OldValue, layer);

                return;
            }

            // Passe aussi dans le cas du modif du nom car celui ci modifie le vsproject
            if (e.DomainProperty.Id == Layer.VSProjectNameDomainPropertyId &&
                !layer.Store.TransactionManager.CurrentTransaction.TopLevelTransaction.IsSerializing)
            {
                // Modif du namespace
                UpdateNamespace((string) e.OldValue, layer);
                UpdateAssemblyName(layer);
                return;
            }

            // Le namespace
            if (e.DomainProperty.Id == Layer.NamespaceDomainPropertyId)
            {
                string oldName = (string) e.OldValue;
                if (
                    layer.Store.TransactionManager.CurrentTransaction.Context.ContextInfo.TryGetValue(
                        "InitializeComponentWizard", out flag))
                    oldName = "?"; // Force la mise à jour
                if (layer.Store.TransactionManager.CurrentTransaction.TopLevelTransaction.IsSerializing &&
                    oldName != "?")
                    return;
                string newName = (string) e.NewValue;

                // Modif du nom de l'assembly
                if (newName != "?" && layer.Component != null &&
                    (layer.AssemblyName.StartsWith(oldName) || String.IsNullOrEmpty(layer.AssemblyName)))
                {
                    UpdateAssemblyName(layer);
                }

                // Modification du namespace sur le modelsLayer
                //  --> Modif des namespaces des packages
                if (layer is DataLayer)
                {
                    DataLayer ml = layer as DataLayer;

                    foreach (Package package in ml.Packages)
                    {
                        if (oldName == "?")
                        {
                            pos = package.Name.LastIndexOf('.');
                            if (pos > 0)
                                package.Name = String.Concat(newName, package.Name.Substring(pos));
                        }
                        else if ((!String.IsNullOrEmpty(oldName) && package.Name.StartsWith(oldName)) ||
                                 String.IsNullOrEmpty(package.Name))
                            package.Name = newName + package.Name.Substring(oldName.Length);
                    }
                }
            }
        }

        /// <summary>
        /// Updates the name of the assembly.
        /// </summary>
        /// <param name="layer">The layer.</param>
        private static void UpdateAssemblyName(SoftwareLayer layer)
        {
            layer.AssemblyName = StrategyManager.GetInstance(layer.Store).NamingStrategy.CreateAssemblyName(layer);
        }

        /// <summary>
        /// Updates the vs project.
        /// </summary>
        /// <param name="oldValue">The old value.</param>
        /// <param name="layer">The layer.</param>
        private static void UpdateVsProject(string oldValue, SoftwareLayer layer)
        {
            if (layer.VSProjectName == oldValue || String.IsNullOrEmpty(layer.VSProjectName))
                layer.VSProjectName = layer.Name;
        }

        /// <summary>
        /// Updates the namespace.
        /// </summary>
        /// <param name="oldValue">The old value.</param>
        /// <param name="layer">The layer.</param>
        private static void UpdateNamespace(string oldValue, SoftwareLayer layer)
        {
            string oldNamespace =
                StrategyManager.GetInstance(layer.Store).NamingStrategy.CreateNamespace(layer.Component.Namespace,
                                                                                        oldValue, layer);
            if (oldNamespace == layer.Namespace || String.IsNullOrEmpty(layer.Namespace))
                layer.Namespace =
                    StrategyManager.GetInstance(layer.Store).NamingStrategy.CreateNamespace(layer.Component.Namespace,
                                                                                            layer.Name, layer);
        }

        /// <summary>
        /// Mise à jour des noms du package et de la couche d'interface
        /// </summary>
        /// <param name="slayer">The slayer.</param>
        private static void UpdateInterfaceAndLayerPackageName(SoftwareLayer slayer)
        {
            Layer layer = slayer as Layer;
            if (layer == null)
                return;

            LayerPackage package = layer.LayerPackage;
            if (String.IsNullOrEmpty(package.Name) && package.Name != "?")
                package.Name = StrategyManager.GetInstance(layer.Store).NamingStrategy.GetLayerName(layer);

            if (package.InterfaceLayer != null)
            {
                if (String.IsNullOrEmpty(package.InterfaceLayer.Name))
                    package.InterfaceLayer.Name =
                        StrategyManager.GetInstance(layer.Store).NamingStrategy.CreateLayerName(package,
                                                                                                package.InterfaceLayer,
                                                                                                layer.Name);
            }
        }
    }
}