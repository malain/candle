using System.Collections.Generic;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Diagrams;

namespace DSLFactory.Candle.SystemModel.Rules
{
    /// <summary>
    /// Suppression des ports sur les vues de la couche UI
    /// </summary>
    [RuleOn(typeof (LayerPackage), FireTime = TimeToFire.TopLevelCommit)]
    public class LayerPackageDeleteRule : DeleteRule
    {
        /// <summary>
        /// </summary>
        /// <param name="e">Provides data for the ElementDeleted event.</param>
        public override void ElementDeleted(ElementDeletedEventArgs e)
        {
            LayerPackage layerPackage = e.ModelElement as LayerPackage;
            if (layerPackage == null)
                return;

            if (layerPackage.Store.InUndoRedoOrRollback)
                return;

            SoftwareComponent component = CandleModel.GetInstance(layerPackage.Store).SoftwareComponent;
            if (component != null)
            {
                // Suppression de la couche d'interface
                AbstractLayer interfaceLayer = null;
                foreach (AbstractLayer al in component.Layers)
                {
                    ISortedLayer sl = al as ISortedLayer;
                    if (sl != null && sl.Level == layerPackage.LayerLevel)
                    {
                        interfaceLayer = al;
                        break;
                    }
                }
                if (interfaceLayer != null)
                {
                    interfaceLayer.Delete();
                }
                IList<PresentationViewsSubject> shapes = PresentationViewsSubject.GetLinksToPresentation(component);
                foreach (PresentationViewsSubject link in shapes)
                {
                    ((SoftwareComponentShape) link.Presentation).ArrangeShapes();
                }
            }
        }
    }
}