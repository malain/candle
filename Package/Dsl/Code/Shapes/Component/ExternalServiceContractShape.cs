using System.Collections.Generic;
using Microsoft.VisualStudio.Modeling.Diagrams;

namespace DSLFactory.Candle.SystemModel
{
    /// <summary>
    /// 
    /// </summary>
    public partial class ExternalServiceContractShape
    {
        /// <summary>
        /// Provides support for background gradients.
        /// </summary>
        /// <param name="shapeFields">The fields on the shape that can have a background gradient.</param>
        protected override void InitializeShapeFields(IList<ShapeField> shapeFields)
        {
            base.InitializeShapeFields(shapeFields);

            TextField field = null;
            foreach (ShapeField sf in shapeFields)
            {
                if (sf.Name == "NameDecorator")
                {
                    field = sf as TextField;
                    break;
                }
            }
            if (field != null)
                shapeFields.Remove(field);
            field = new ReadOnlyTextField("NameDecorator");
            field.DefaultText = global::DSLFactory.Candle.SystemModel.CandleDomainModel.SingletonResourceManager.GetString("ExternalServiceContractShapeNameDecoratorDefaultText");
            field.DefaultFocusable = true;
            field.DefaultAutoSize = true;
            field.AnchoringBehavior.MinimumHeightInLines = 1;
            field.AnchoringBehavior.MinimumWidthInCharacters = 1;
            field.DefaultAccessibleState = System.Windows.Forms.AccessibleStates.Invisible;
            shapeFields.Add(field);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public partial class ExternalServiceContractShapeBase
    {
        /// <summary>
        /// Gets the variable tooltip text.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns></returns>
        private string GetVariableTooltipText(Microsoft.VisualStudio.Modeling.Diagrams.DiagramItem item)
        {
            return ((ExternalServiceContract)item.Shape.ModelElement).Name;
        }
    }
}