using System.Windows.Forms;
using DSLFactory.Candle.SystemModel.Commands;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Diagrams;

namespace DSLFactory.Candle.SystemModel
{
    partial class LayerPackageShape : ISupportArrangeShapes
    {
        private const double MARGIN = 0.25;

        //private static StyleSetResourceId dalBackroundBrushId;
        //private static StyleSetResourceId bllBackroundBrushId;
        //private static StyleSetResourceId uiBackroundBrushId;

        /// <summary>
        /// Initializes the <see cref="LayerPackageShape"/> class.
        /// </summary>
        static LayerPackageShape()
        {
            //dalBackroundBrushId = new StyleSetResourceId( ModelConstants.ApplicationName, "dalBackroundBrushId" );
            //bllBackroundBrushId = new StyleSetResourceId( ModelConstants.ApplicationName, "bllBackroundBrushId" );
            //uiBackroundBrushId = new StyleSetResourceId( ModelConstants.ApplicationName, "uiBackroundBrushId" );
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

        //protected override void InitializeResources( StyleSet classStyleSet )
        //{
        //    base.InitializeResources( classStyleSet );

        //    //// Couleurs de fond dépend du niveau
        //    //BrushSettings backgroundBrush = new BrushSettings();
        //    //backgroundBrush.Color = global::System.Drawing.Color.FromArgb( 255, 243, 240, 161 );
        //    //classStyleSet.AddBrush( dalBackroundBrushId, DiagramBrushes.ShapeBackground, backgroundBrush );

        //    //backgroundBrush = new BrushSettings();
        //    //backgroundBrush.Color = global::System.Drawing.Color.FromArgb( 255, 211, 229, 248 );
        //    //classStyleSet.AddBrush( bllBackroundBrushId, DiagramBrushes.ShapeBackground, backgroundBrush );

        //    //backgroundBrush = new BrushSettings();
        //    //backgroundBrush.Color = global::System.Drawing.Color.FromArgb( 255, 205, 231, 143 );
        //    //classStyleSet.AddBrush( uiBackroundBrushId, DiagramBrushes.ShapeBackground, backgroundBrush );
        //}

        ///// <summary>
        ///// Dépend du niveau
        ///// </summary>
        //public override StyleSetResourceId BackgroundBrushId
        //{
        //    get
        //    {
        //        LayerPackage package = this.ModelElement as LayerPackage;
        //        if( package.Level <= 10 )
        //            return dalBackroundBrushId;
        //        if( package.Level <= 50 )
        //            return bllBackroundBrushId;
        //        return uiBackroundBrushId;
        //    }
        //}
        /// <summary>
        /// Gets the value that determines whether the parent of a child shape can be decreased when the size of the child shape decreases.
        /// </summary>
        /// <value></value>
        /// <returns>The direction in which the parent shape can be decreased.</returns>
        public override ResizeDirection AllowsChildrenToShrinkParent
        {
            get { return ResizeDirection.Height; }
        }

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
        /// Gets the margin between the shape's bounding box and its nested node shapes.
        /// </summary>
        /// <value></value>
        /// <returns>The margin between the shape's bounding box and its nested node shapes.</returns>
        public override SizeD NestedShapesMargin
        {
            get { return new SizeD(MARGIN, MARGIN); }
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
        /// Gets the shape and checks to see whether it is highlighted.
        /// </summary>
        /// <value></value>
        /// <returns>true if the shape is highlighted; otherwise, false. </returns>
        public override bool HasHighlighting
        {
            get { return false; }
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
        /// Gets the margin between the parent shape and its nested child shapes.
        /// </summary>
        /// <value></value>
        /// <returns>The margin between the parent shape and its nested child shapes.</returns>
        public override SizeD DefaultContainerMargin
        {
            get { return new SizeD(MARGIN, MARGIN); }
        }

        #region ISupportArrangeShapes Members

        /// <summary>
        /// Arranges the shapes.
        /// </summary>
        public void ArrangeShapes()
        {
            //double width = (this.AbsoluteBounds.Width - ((this.NestedChildShapes.Count + 1) * MARGIN)) / this.NestedChildShapes.Count;

            // Placement des couches horizontalement
            using (Transaction transaction = Store.TransactionManager.BeginTransaction("Arrange shapes"))
            {
                double X = AbsoluteBounds.Left + MARGIN;
                foreach (NodeShape shape2 in NestedChildShapes)
                {
                    shape2.AbsoluteBounds = new RectangleD(X,
                                                           AbsoluteBounds.Top + MARGIN,
                                                           shape2.AbsoluteBounds.Width,
                                                           shape2.AbsoluteBounds.Height);
                    X = shape2.AbsoluteBounds.Right + MARGIN;
                }

                transaction.Commit();
            }
        }

        #endregion

        /// <summary>
        /// Drag drop d'un projet
        /// </summary>
        /// <param name="e">The diagram drag event arguments.</param>
        public override void OnDragOver(DiagramDragEventArgs e)
        {
            base.OnDragOver(e);
            if (e.Data.GetDataPresent("CF_VSREFPROJECTS"))
                e.Effect = DragDropEffects.Link;
        }

        /// <summary>
        /// Alerts listeners when the shape is dragged and dropped.
        /// </summary>
        /// <param name="e">The diagram drag event arguments.</param>
        public override void OnDragDrop(DiagramDragEventArgs e)
        {
            base.OnDragDrop(e);
            if (e.Data.GetDataPresent("CF_VSREFPROJECTS"))
            {
                LayerPackage package = ModelElement as LayerPackage;
                if (package != null)
                    ImportProjectHelper.Import(e.Data.GetData("CF_VSREFPROJECTS"), package.Component, package);
            }
        }
    }
}