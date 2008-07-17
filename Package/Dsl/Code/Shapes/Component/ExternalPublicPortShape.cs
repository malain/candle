using System.Collections.Generic;
using System.Windows.Forms;
using Microsoft.VisualStudio.Modeling.Diagrams;

namespace DSLFactory.Candle.SystemModel
{
    /// <summary>
    /// 
    /// </summary>
    internal class ReadOnlyTextField : TextField
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ReadOnlyTextField"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        public ReadOnlyTextField(string name) : base(name)
        {
        }

        /// <summary>
        /// Determines whether this instance [can edit value] the specified parent shape.
        /// </summary>
        /// <param name="parentShape">The parent shape.</param>
        /// <param name="view">The view.</param>
        /// <returns>
        /// 	<c>true</c> if this instance [can edit value] the specified parent shape; otherwise, <c>false</c>.
        /// </returns>
        public override bool CanEditValue(ShapeElement parentShape, DiagramClientView view)
        {
            return false;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public partial class ExternalPublicPortShape
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
            field.DefaultText =
                CandleDomainModel.SingletonResourceManager.GetString(
                    "ExternalServiceContractShapeNameDecoratorDefaultText");
            field.DefaultFocusable = true;
            field.DefaultAutoSize = true;
            field.AnchoringBehavior.MinimumHeightInLines = 1;
            field.AnchoringBehavior.MinimumWidthInCharacters = 1;
            field.DefaultAccessibleState = AccessibleStates.Invisible;
            shapeFields.Add(field);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public partial class ExternalPublicPortShapeBase
    {
        /// <summary>
        /// Gets the variable tooltip text.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns></returns>
        private string GetVariableTooltipText(DiagramItem item)
        {
            return ((ExternalPublicPort) item.Shape.ModelElement).Name;
        }
    }
}