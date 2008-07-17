using DslModeling=Microsoft.VisualStudio.Modeling;
using DslDiagrams=Microsoft.VisualStudio.Modeling.Diagrams;

namespace DSLFactory.Candle.SystemModel
{
    /// <summary>
    /// 
    /// </summary>
    partial class LayerPackageContainsLayersBuilder
    {
        /// <summary>
        /// Determines whether this instance [can accept layer as target] the specified layer.
        /// </summary>
        /// <param name="layer">The layer.</param>
        /// <returns>
        /// 	<c>true</c> if this instance [can accept layer as target] the specified layer; otherwise, <c>false</c>.
        /// </returns>
        private static bool CanAcceptLayerAsTarget( Layer layer )
        {
            return layer.LayerPackage == null;
        }

        /// <summary>
        /// Determines whether this instance [can accept layer package and layer as source and target] the specified source layer package.
        /// </summary>
        /// <param name="sourceLayerPackage">The source layer package.</param>
        /// <param name="targetLayer">The target layer.</param>
        /// <returns>
        /// 	<c>true</c> if this instance [can accept layer package and layer as source and target] the specified source layer package; otherwise, <c>false</c>.
        /// </returns>
        private static bool CanAcceptLayerPackageAndLayerAsSourceAndTarget(LayerPackage sourceLayerPackage, Layer targetLayer)
        {
            ISortedLayer sl = targetLayer as ISortedLayer;
            if (sl == null)
                return false;
            return sourceLayerPackage.Level == sl.Level;
        }

    }
}
