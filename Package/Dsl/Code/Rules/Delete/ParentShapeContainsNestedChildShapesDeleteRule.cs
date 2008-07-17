using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Diagrams;

namespace DSLFactory.Candle.SystemModel.Rules
{
    /// <summary>
    /// 
    /// </summary>
    [RuleOn(typeof (ParentShapeContainsNestedChildShapes), FireTime = TimeToFire.TopLevelCommit,
        InitiallyDisabled = false)]
    public class ParentShapeContainsNestedChildShapesDeleteRule : DeleteRule
    {
        /// <summary>
        /// </summary>
        /// <param name="e">Provides data for the ElementDeleted event.</param>
        public override void ElementDeleted(ElementDeletedEventArgs e)
        {
            // Test the element
            ParentShapeContainsNestedChildShapes link = e.ModelElement as ParentShapeContainsNestedChildShapes;
            if (link == null)
                return;

            SoftwareComponentShape shape = link.ParentShape as SoftwareComponentShape;
            if (shape != null)
                shape.ArrangeShapes();
            else if (link.NestedChildShapes.ModelElement is Layer)
            {
                LayerPackageShape shape2 = link.ParentShape as LayerPackageShape;
                if (shape2 != null)
                    shape2.ArrangeShapes();
            }
        }
    }
}