//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using Microsoft.VisualStudio.Modeling;
using DslModeling=Microsoft.VisualStudio.Modeling;
using DslDesign=Microsoft.VisualStudio.Modeling.Design;
using DslDiagrams=Microsoft.VisualStudio.Modeling.Diagrams;

namespace DSLFactory.Candle.SystemModel
{
    /// <summary>
    /// Serializer SystemModelDiagramSerializer for DomainClass SystemModelDiagram.
    /// </summary>
    /// A rajouter dans SystemModelSerializationBehavior
    public partial class UIWorkflowLayerDiagramSerializer : ComponentModelDiagramSerializerBase
    {
        #region Constructor
        /// <summary>
        /// SystemModelDiagramSerializer Constructor
        /// </summary>
        public UIWorkflowLayerDiagramSerializer()
            : base()
        {
        }

        /// <summary>
        /// This is the XML tag name used to serialize an instance of SystemModelDiagram.
        /// </summary>
        public override string XmlTagName
        {
            [global::System.Diagnostics.DebuggerStepThrough]
            get { return @"uiWfLayerDiagram"; }
        }

        /// <summary>
        /// Creates the instance.
        /// </summary>
        /// <param name="serializationContext">The serialization context.</param>
        /// <param name="reader">The reader.</param>
        /// <param name="partition">The partition.</param>
        /// <returns></returns>
        protected override ModelElement CreateInstance( SerializationContext serializationContext, System.Xml.XmlReader reader, Partition partition )
        {
            return new UIWorkflowLayerDiagram( partition );
        }
        #endregion
    }



}

