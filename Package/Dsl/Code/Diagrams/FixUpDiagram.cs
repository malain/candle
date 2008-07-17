using DSLFactory.Candle.SystemModel.Strategies;

namespace DSLFactory.Candle.SystemModel
{
    partial class FixUpDiagram
    {
        /// <summary>
        /// Gets the parent for layer package.
        /// </summary>
        /// <param name="layerPackage">The layer package.</param>
        /// <returns></returns>
        private Microsoft.VisualStudio.Modeling.ModelElement GetParentForLayerPackage( LayerPackage layerPackage )
        {
            return layerPackage.Component;
        }

        /// <summary>
        /// Gets the parent for package.
        /// </summary>
        /// <param name="package">The package.</param>
        /// <returns></returns>
        private Microsoft.VisualStudio.Modeling.ModelElement GetParentForPackage( Package package )
        {
            return package.Layer;
        }

        /// <summary>
        /// Gets the parent for UI view.
        /// </summary>
        /// <param name="uIView">The u I view.</param>
        /// <returns></returns>
        private Microsoft.VisualStudio.Modeling.ModelElement GetParentForUIView( UIView uIView )
        {
            return uIView.Scenario;
        }

        /// <summary>
        /// Gets the parent for dot net assembly.
        /// </summary>
        /// <param name="dotNetAssembly">The dot net assembly.</param>
        /// <returns></returns>
        private Microsoft.VisualStudio.Modeling.ModelElement GetParentForDotNetAssembly( DotNetAssembly dotNetAssembly )
        {
            return dotNetAssembly.Component;
        }

        /// <summary>
        /// Gets the parent for interface layer.
        /// </summary>
        /// <param name="childElement">The child element.</param>
        /// <returns></returns>
        private Microsoft.VisualStudio.Modeling.ModelElement GetParentForInterfaceLayer(InterfaceLayer childElement)
        {
            return childElement.Component;
        }

        /// <summary>
        /// Gets the parent for data layer.
        /// </summary>
        /// <param name="modelsLayer">The models layer.</param>
        /// <returns></returns>
        private Microsoft.VisualStudio.Modeling.ModelElement GetParentForDataLayer( DataLayer modelsLayer )
        {
            return modelsLayer.Component;
        }

        /// <summary>
        /// Gets the parent for UI workflow layer.
        /// </summary>
        /// <param name="uIWorkflowLayer">The u I workflow layer.</param>
        /// <returns></returns>
        private Microsoft.VisualStudio.Modeling.ModelElement GetParentForUIWorkflowLayer( UIWorkflowLayer uIWorkflowLayer )
        {
            return uIWorkflowLayer.Component;
        }

        /// <summary>
        /// Gets the parent for data access layer.
        /// </summary>
        /// <param name="dALLayer">The d AL layer.</param>
        /// <returns></returns>
        private Microsoft.VisualStudio.Modeling.ModelElement GetParentForDataAccessLayer( DataAccessLayer dALLayer )
        {
            return dALLayer.Component;
        }

        /// <summary>
        /// Gets the parent for presentation layer.
        /// </summary>
        /// <param name="presentationLayer">The presentation layer.</param>
        /// <returns></returns>
        private Microsoft.VisualStudio.Modeling.ModelElement GetParentForPresentationLayer( PresentationLayer presentationLayer )
        {
            return presentationLayer.Component;
        }

        /// <summary>
        /// Gets the parent for business layer.
        /// </summary>
        /// <param name="businessLayer">The business layer.</param>
        /// <returns></returns>
        private Microsoft.VisualStudio.Modeling.ModelElement GetParentForBusinessLayer( BusinessLayer businessLayer )
        {
            return businessLayer.Component;
        }

        /// <summary>
        /// Gets the parent for class uses operations.
        /// </summary>
        /// <param name="classUsesOperations">The class uses operations.</param>
        /// <returns></returns>
        private Microsoft.VisualStudio.Modeling.ModelElement GetParentForClassUsesOperations(ClassUsesOperations classUsesOperations)
        {
            ICustomizableElement clazz = classUsesOperations.Source as ICustomizableElement;
            System.Diagnostics.Debug.Assert(clazz != null);
            return ((SoftwareLayer)clazz.Owner).Component.Model;
        }
    }
}
