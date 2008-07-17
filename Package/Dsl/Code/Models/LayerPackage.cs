using System;
using Microsoft.VisualStudio.Modeling;

namespace DSLFactory.Candle.SystemModel
{
    /// <summary>
    /// 
    /// </summary>
    partial class LayerPackage : ISortedLayer
    {
        /// <summary>
        /// Gets the layer level.
        /// </summary>
        /// <value>The layer level.</value>
        internal short LayerLevel
        {
            get { return (short) (Level + 1); }
        }

        /// <summary>
        /// Called by the model before the element is deleted.
        /// </summary>
        protected override void OnDeleting()
        {
            base.OnDeleting();

            if (Store.InUndoRedoOrRollback)
                return;

            using (Transaction transaction = Store.TransactionManager.BeginTransaction("Remove layer"))
            {
                while (Layers.Count > 0)
                {
                    Layers[0].Delete();
                }
                transaction.Commit();
            }
        }

        /// <summary>
        /// Merges the disconnect layer.
        /// </summary>
        /// <param name="sourceElement">The source element.</param>
        private void MergeDisconnectLayer(ModelElement sourceElement)
        {
            Layer layer = sourceElement as Layer;
            if (layer != null)
            {
                foreach (ElementLink link in LayerPackageContainsLayers.GetLinks(this, layer))
                {
                    // Delete the link, but without possible delete propagation to the element since it's moving to a new location.
                    link.Delete(LayerPackageContainsLayers.LayerDomainRoleId,
                                LayerPackageContainsLayers.LayerPackageDomainRoleId);
                }

                foreach (ElementLink link in SoftwareComponentHasLayers.GetLinks(Component, layer))
                {
                    // Delete the link, but without possible delete propagation to the element since it's moving to a new location.
                    link.Delete(SoftwareComponentHasLayers.SoftwareComponentDomainRoleId,
                                SoftwareComponentHasLayers.SoftwareLayerDomainRoleId);
                }
            }
        }

        /// <summary>
        /// Merges the disconnect interface layer.
        /// </summary>
        /// <param name="sourceElement">The source element.</param>
        private void MergeDisconnectInterfaceLayer(ModelElement sourceElement)
        {
            InterfaceLayer layer = sourceElement as InterfaceLayer;
            if (layer != null)
            {
                foreach (ElementLink link in LayerPackageReferencesInterfaceLayer.GetLinks(this, layer))
                {
                    // Delete the link, but without possible delete propagation to the element since it's moving to a new location.
                    link.Delete(LayerPackageReferencesInterfaceLayer.InterfaceLayerDomainRoleId,
                                LayerPackageReferencesInterfaceLayer.LayerPackageDomainRoleId);
                }

                foreach (ElementLink link in SoftwareComponentHasLayers.GetLinks(Component, layer))
                {
                    // Delete the link, but without possible delete propagation to the element since it's moving to a new location.
                    link.Delete(SoftwareComponentHasLayers.SoftwareComponentDomainRoleId,
                                SoftwareComponentHasLayers.SoftwareLayerDomainRoleId);
                }
            }
        }

        /// <summary>
        /// Le père n'est pas le composant graphique sur lequel il est laché
        /// </summary>
        /// <param name="sourceElement">The source element.</param>
        /// <param name="elementGroup">The element group.</param>
        private void MergeRelateLayer(ModelElement sourceElement, ElementGroup elementGroup)
        {
            Component.Layers.Add((Layer) sourceElement);
            // Création du lien entre les 2
            Layers.Add((Layer) sourceElement);
        }

        /// <summary>
        /// Merges the relate interface layer.
        /// </summary>
        /// <param name="sourceElement">The source element.</param>
        /// <param name="elementGroup">The element group.</param>
        private void MergeRelateInterfaceLayer(ModelElement sourceElement, ElementGroup elementGroup)
        {
            InterfaceLayer iLayer = sourceElement as InterfaceLayer;
            InterfaceLayer = iLayer;
            iLayer.Level = LayerLevel;
            Component.Layers.Add(iLayer);
        }

        /// <summary>
        /// Determines whether this instance [can merge interface layer] the specified root element.
        /// </summary>
        /// <param name="rootElement">The root element.</param>
        /// <param name="elementGroupPrototype">The element group prototype.</param>
        /// <returns>
        /// 	<c>true</c> if this instance [can merge interface layer] the specified root element; otherwise, <c>false</c>.
        /// </returns>
        private bool CanMergeInterfaceLayer(ProtoElementBase rootElement, ElementGroupPrototype elementGroupPrototype)
        {
            if (elementGroupPrototype == null)
                throw new ArgumentNullException("elementGroupPrototype");
            return InterfaceLayer == null;
        }

        /// <summary>
        /// Determines whether this instance [can merge layer] the specified root element.
        /// </summary>
        /// <param name="rootElement">The root element.</param>
        /// <param name="elementGroupPrototype">The element group prototype.</param>
        /// <returns>
        /// 	<c>true</c> if this instance [can merge layer] the specified root element; otherwise, <c>false</c>.
        /// </returns>
        private bool CanMergeLayer(ProtoElementBase rootElement, ElementGroupPrototype elementGroupPrototype)
        {
            if (elementGroupPrototype == null)
                throw new ArgumentNullException("elementGroupPrototype");

            if (rootElement != null)
            {
                DomainClassInfo rootElementDomainInfo =
                    Partition.DomainDataDirectory.GetDomainClass(rootElement.DomainClassId);

                if (rootElementDomainInfo.IsDerivedFrom(Layer.DomainClassId))
                {
                    return Component.GetLayerLevel(rootElement.DomainClassId) == Level;
                }
            }
            return false;
        }

        /// <summary>
        /// Indique si on est sur la couche la plus haute
        /// </summary>
        /// <returns>
        /// 	<c>true</c> if [is higher level]; otherwise, <c>false</c>.
        /// </returns>
        internal bool IsHigherLevel()
        {
            int maxLevel = -1;
            foreach (LayerPackage pack in Component.LayerPackages)
            {
                if (pack.Level > maxLevel)
                    maxLevel = pack.Level;
            }
            return Level == maxLevel;
        }
    }
}