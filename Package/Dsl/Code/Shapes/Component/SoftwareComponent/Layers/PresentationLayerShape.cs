using DSLFactory.Candle.SystemModel.Commands;
using Microsoft.VisualStudio.Modeling.Diagrams;

namespace DSLFactory.Candle.SystemModel
{
    partial class PresentationLayerShape : ISupportArrangeShapes
    {
        #region Import d'un service par drag'n drop

        /// <summary>
        /// Import d'un service
        /// </summary>
        /// <param name="e">The diagram drag event arguments.</param>
        public override void OnDragDrop(DiagramDragEventArgs e)
        {
            base.OnDragDrop(e);
            DragDropHelper.OnDragDropOnLayer(this, e);
        }

        /// <summary>
        /// Alerts listeners when the shape is dragged over its bounds.
        /// </summary>
        /// <param name="e">The diagram drag event arguments.</param>
        public override void OnDragOver(DiagramDragEventArgs e)
        {
            base.OnDragOver(e);
            DragDropHelper.OnDragOverLayer(this, e);
        }

        #endregion

        ///// <summary>
        ///// Texte des ports en vertical
        ///// </summary>
        ///// <param name="shapeFields"></param>
        ///// <param name="decorators"></param>
        //protected override void InitializeDecorators(IList<ShapeField> shapeFields, IList<Decorator> decorators)
        //{
        //    base.InitializeDecorators(shapeFields, decorators);

        //    // TIPS ecriture en vertical
        //    //Decorator decorator = FindDecorator(decorators, "NameDecorator");
        //    //TextField field = decorator.Field as TextField;
        //    ////   field.DefaultIsHorizontal = false;
        //    //field.DefaultStringFormat = new StringFormat(StringFormatFlags.DirectionVertical);
        //}

        /// <summary>
        /// Gets the child shape and checks to see whether its parent shape can be resized when the child shape is resized.
        /// </summary>
        /// <value></value>
        /// <returns>true if the parent of a child shape can be resized when the child shape is resized; otherwise, false.</returns>
        public override bool AllowsChildrenToResizeParent
        {
            get { return true; }
        }

        /// <summary>
        /// Gets the child shape and checks to see whether its parent shape should be resized when the absolute bounds for the child shape change.
        /// </summary>
        /// <value></value>
        /// <returns>true if the parent shape should be resized when the absolute bounds for the child shape change; otherwise, false.</returns>
        public override bool AutoResizeParentOnBoundsChange
        {
            get { return true; }
        }

        /// <summary>
        /// Gets the shape and checks to see whether it has a shadow.
        /// </summary>
        /// <value></value>
        /// <returns>true if the shape has a shadow; otherwise, false.</returns>
        public override bool HasShadow
        {
            get { return true; }
        }

        /// <summary>
        /// Gets the shape and checks to see whether it is highlighted.
        /// </summary>
        /// <value></value>
        /// <returns>true if the shape is highlighted; otherwise, false. </returns>
        public override bool HasHighlighting
        {
            get { return false; }
        }

        #region ISupportArrangeShapes Members

        /// <summary>
        /// Positionne les ports
        /// </summary>
        void ISupportArrangeShapes.ArrangeShapes()
        {
            LayerHelper.ArrangeShapes(this);
        }

        #endregion
    }
}