using System;
using Rules = DSLFactory.Candle.SystemModel.Rules;

namespace DSLFactory.Candle.SystemModel
{
    /// <summary>
    /// D�clarations des types personnalis�s
    /// </summary>
    partial class CandleDomainModel
    {        
        internal static Type[] CustomTypes = new Type[] {
                typeof(DataLayerDiagram),     // Diagramme suppl�mentaire
                typeof(UIWorkflowLayerDiagram), // Diagramme suppl�mentaire

                typeof(Rules::ExternalComponentShapeInsertRule),
                typeof(Rules::ExternalComponentChangeRule),
                typeof(Rules::GeneralizationSuperClassChangeRole),
                typeof(Rules::CustomizableElementInsertRule),
                typeof(Rules::CustomizableElementDeleteRule),

                typeof(Rules::LayerServicePortChangeRule),
                typeof(Rules::LayerNameChangeRule),
                typeof(Rules::ExternalAssemblyShapeInsertRule),
                typeof(Rules::EnumValueChangeRule),
                typeof(Rules::XmlNameChangeRule),
                typeof(Rules::LayerDeleteRule),
                typeof(Rules::ClassUsesOperationsInsertRule),
                typeof(Rules::LayerPackageInsertRule),               
              // Name affect� directement lors de la cr�ation  typeof(Rules::LayerPackageContainsLayersInsertRule),      
                typeof(Rules::AssociationInsertRule),
                typeof(Rules::LayerChangeRule),
                typeof(Rules::SoftwareComponentInsertRule),
                typeof(Rules::SoftwareComponentChangeRule),
                typeof(Rules::ParentShapeContainsNestedChildShapesInsertRule),
                typeof(Rules::ParentShapeContainsNestedChildShapesDeleteRule),
                typeof(Rules::LayerPackageDeleteRule),
                typeof(Rules::CustomizableElementChangeRule),
                typeof(Rules::ExternalServiceReferenceAddRule),
            };

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected override Type[] GetCustomDomainModelTypes()
        {
            return CustomTypes;
        }
    }
}
