using System.Collections.Generic;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Diagrams;

namespace DSLFactory.Candle.SystemModel.Rules
{
    /// <summary>
    /// Suppression des ports sur les vues de la couche UI
    /// </summary>
    [RuleOn(typeof (Layer), FireTime = TimeToFire.TopLevelCommit)]
    public class LayerDeleteRule : DeleteRule
    {
        /// <summary>
        /// </summary>
        /// <param name="e">Provides data for the ElementDeleted event.</param>
        public override void ElementDeleted(ElementDeletedEventArgs e)
        {
            Layer layer = e.ModelElement as Layer;
            if (layer == null)
                return;

            if (layer.Store.InUndoRedoOrRollback)
                return;

            if (layer.LayerPackage != null)
            {
                IList<PresentationViewsSubject> shapes =
                    PresentationViewsSubject.GetLinksToPresentation(layer.LayerPackage);
                foreach (PresentationViewsSubject link in shapes)
                {
                    ((SoftwareComponentShape) link.Presentation).ArrangeShapes();
                }
            }
        }
    }
}