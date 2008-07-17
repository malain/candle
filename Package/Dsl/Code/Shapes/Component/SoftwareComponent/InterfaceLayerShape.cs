using Microsoft.VisualStudio.Modeling.Diagrams;
using DslModeling=Microsoft.VisualStudio.Modeling;

namespace DSLFactory.Candle.SystemModel
{
    //internal class InterfaceLayerShapeGeometry : RectangleShapeGeometry
    //{
    //     private static InterfaceLayerShapeGeometry instance = new InterfaceLayerShapeGeometry();

    //     internal static InterfaceLayerShapeGeometry Instance
    //     {
    //         get { return instance; }
    //     }

    //     protected override void DoPaintGeometry(DiagramPaintEventArgs e, IGeometryHost geometryHost)
    //     {
    ////         base.DoPaintGeometry(e, geometryHost);

    //         StyleSet styleSet = geometryHost.GeometryStyleSet;
    //         Pen pen = styleSet.GetPen(this.GetOutlinePenId(geometryHost));
    //         RectangleD boundingBox = geometryHost.GeometryBoundingBox;

    //         if (pen != null)
    //         {
    //             Color color = geometryHost.UpdateGeometryLuminosity(e.View, pen);
    //             e.Graphics.DrawLine(pen, (float)boundingBox.Left, (float)boundingBox.Bottom, (float)boundingBox.Right, (float)boundingBox.Bottom);
    //             // Ligne du dessus sauf la 1ére
    //             if( boundingBox.Y > 0 )
    //                 e.Graphics.DrawLine(pen, (float)boundingBox.Left, (float)boundingBox.Bottom, (float)boundingBox.Right, (float)boundingBox.Bottom);
    //             pen.Color = color;
    //         }
    //     }
    // }

    partial class InterfaceLayerShape //: ISupportArrangeShapes
    {
        //public override ShapeGeometry ShapeGeometry
        //{
        //    get
        //    {
        //        return InterfaceLayerShapeGeometry.Instance;
        //    }
        //}

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
        /// Gets the bounds rules for the shape.
        /// </summary>
        /// <value></value>
        /// <returns>The bounds rules for the shape.</returns>
        public override BoundsRules BoundsRules
        {
            get { return ComponentShapeBoundsRule.Instance; }
        }

        /// <summary>
        /// Gets the shape and checks to see whether it has an outline.
        /// </summary>
        /// <value></value>
        /// <returns>true if the shape has an outline; otherwise, false.</returns>
        public override bool HasOutline
        {
            get { return false; }
        }

        /// <summary>
        /// Gets the shape and checks to see whether it has a shadow.
        /// </summary>
        /// <value></value>
        /// <returns>true if the shape has a shadow; otherwise, false.</returns>
        public override bool HasShadow
        {
            get { return false; }
        }

        /// <summary>
        /// Gets the shape and checks to see whether a user can move it.
        /// </summary>
        /// <value></value>
        /// <returns>true if the user can move the shape; otherwise, false.</returns>
        public override bool CanMove
        {
            get { return false; }
        }

        /// <summary>
        /// Gets the margin between the parent shape and its nested child shapes.
        /// </summary>
        /// <value></value>
        /// <returns>The margin between the parent shape and its nested child shapes.</returns>
        public override SizeD DefaultContainerMargin
        {
            get { return new SizeD(0.1, 0.03); }
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
        ///// Positionne les ports
        ///// </summary>
        //void ISupportArrangeShapes.ArrangeShapes()
        //{
        //    LayerHelper.ArrangeShapes( this );
        //}
    }
}