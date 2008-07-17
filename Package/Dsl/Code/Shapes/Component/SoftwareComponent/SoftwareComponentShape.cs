using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using DSLFactory.Candle.SystemModel.Commands;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Diagrams;

namespace DSLFactory.Candle.SystemModel
{
    //public class ComponentTitleField : TextField
    //{
    //    private const float Height = 2f;

    //    public ComponentTitleField(string name): base(name)
    //    {
    //    }

    //    public override void DoPaint( DiagramPaintEventArgs e, ShapeElement parentShape )
    //    {
    //        base.DoPaint( e, parentShape );

    //        if( this.GetVisible( parentShape ) )
    //        {
    //            RectangleD ed = this.GetBounds( parentShape );
    //            RectangleF ed1 = RectangleD.ToRectangleF( ed );
    //            Color color2 = Color.Black;

    //            SizeF textSize = SizeD.ToSizeF( ShapeHelper.MeasureTextFieldSize( parentShape, this ) );                
    //            textSize.Width = Math.Max( 2.0f, textSize.Width + ed1.X + 0.1f );

    //            Pen pen1 = this.GetPen( e.View, parentShape, ref color2 );

    //            GraphicsPath path = new GraphicsPath();
    //            path.StartFigure();
    //            // Gauche
    //            path.AddLine(0, 0, 0, -Height);
    //            // Haut
    //            path.AddLine( 0, -Height, textSize.Width, -Height );
    //            // Droit
    //            path.AddLine( textSize.Width, 0.2f, textSize.Width-0.1f, 0.3f );
    //            path.AddLine( textSize.Width-0.1f, 0.3f, 0, 0.3f );
    //          //  e.Graphics.DrawPath( pen1, path );
    //            pen1.Color = color2;
    //        }
    //    }
    //}

    //internal class SoftwareComponentShapeGeometry : RectangleShapeGeometry
    //{
    //    private static SoftwareComponentShapeGeometry instance = new SoftwareComponentShapeGeometry();

    //    internal static SoftwareComponentShapeGeometry Instance
    //    {
    //        get { return instance; }
    //    }

    //    protected override void DoPaintGeometry(DiagramPaintEventArgs e, IGeometryHost geometryHost)
    //    {
    //        base.DoPaintGeometry(e, geometryHost);

    //        StyleSet styleSet = geometryHost.GeometryStyleSet;
    //        Pen pen = styleSet.GetPen(this.GetOutlinePenId(geometryHost));
    //        RectangleD boundingBox = geometryHost.GeometryBoundingBox;
    //        float pt = (float)(boundingBox.Right - SoftwareComponentShape.RIGHT_MARGIN);

    //        if (pen != null)
    //        {
    //            Color color = geometryHost.UpdateGeometryLuminosity(e.View, pen);
    //            e.Graphics.DrawLine(pen, pt, (float)boundingBox.Top, pt, (float)boundingBox.Bottom);
    //            pen.Color = color;
    //        }
    //    }
    //}

    /// <summary>
    /// 
    /// </summary>
    partial class SoftwareComponentShape : ISupportArrangeShapes
    {
        /// <summary>
        /// 
        /// </summary>
        public const double HEADER_HEIGHT = 0.35;

        /// <summary>
        /// 
        /// </summary>
        public const double MARGIN = 0.1;

        private readonly StyleSetResourceId headerBackgroundBrushId =
            new StyleSetResourceId("DSLFactory.Candle", "headerBackgroundBrushId");

        //public override ShapeGeometry ShapeGeometry
        //{
        //    get
        //    {
        //        return SoftwareComponentShapeGeometry.Instance;
        //    }
        //}

        /// <summary>
        /// Gets the margin between the parent shape and its nested child shapes.
        /// </summary>
        /// <value></value>
        /// <returns>The margin between the parent shape and its nested child shapes.</returns>
        public override SizeD DefaultContainerMargin
        {
            get { return new SizeD(MARGIN, HEADER_HEIGHT); }
        }

        /// <summary>
        /// Gets the minimum size for the shape.
        /// </summary>
        /// <value></value>
        /// <returns>The minimum size for the shape.</returns>
        public override SizeD MinimumSize
        {
            get { return new SizeD(6, 2); }
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
        /// Gets the value that determines whether the parent of a child shape can be decreased when the size of the child shape decreases.
        /// </summary>
        /// <value></value>
        /// <returns>The direction in which the parent shape can be decreased.</returns>
        public override ResizeDirection AllowsChildrenToShrinkParent
        {
            get { return ResizeDirection.Both; }
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

        #region ISupportArrangeShapes Members

        /// <summary>
        /// Placement des couches dans le composant
        /// </summary>
        public void ArrangeShapes()
        {
            try
            {
                using (Transaction transaction = Store.TransactionManager.BeginTransaction("Arrange Shapes"))
                {
                    // Tri des couches par niveau de la plus haute à la plus basse
                    SortedList<int, NodeShape> shapes =
                        new SortedList<int, NodeShape>(new SoftwareComponent.LayerLevelComparer());
                    foreach (NodeShape shape2 in NestedChildShapes)
                    {
                        if (shape2.ModelElement is ISortedLayer)
                        {
                            shapes.Add(((ISortedLayer) shape2.ModelElement).Level, shape2);
                        }
                    }

                    // Placement des couches horizontalement
                    NodeShape shape1 = null;
                    foreach (int level in shapes.Keys)
                    {
                        NodeShape shape2 = shapes[level];
                        double Y = (shape1 != null)
                                       ? shape1.AbsoluteBounds.Bottom + (MARGIN)
                                       : AbsoluteBounds.Y + HEADER_HEIGHT + MARGIN;

                        shape2.AbsoluteBounds = new RectangleD(
                            AbsoluteBounds.Left + MARGIN,
                            Y,
                            shape2.AbsoluteBounds.Width,
                            shape2.AbsoluteBounds.Height);
                        shape1 = shape2;
                    }
                    transaction.Commit();
                }
            }
            catch
            {
            }
        }

        #endregion

        /// <summary>
        /// Ensure outer decorators are placed appropriately.  This is called during view fixup,
        /// after the shape has been associated with the model element.
        /// </summary>
        /// <param name="fixupState"></param>
        /// <param name="iteration"></param>
        /// <param name="createdDuringViewFixup"></param>
        public override void OnBoundsFixup(BoundsFixupState fixupState, int iteration, bool createdDuringViewFixup)
        {
            base.OnBoundsFixup(fixupState, iteration, createdDuringViewFixup);
            ArrangeShapes();
        }

        /// <summary>
        /// Configures the port on the shape when the shape is being added to the diagram.
        /// </summary>
        /// <param name="child">The port assigned to the shape.</param>
        /// <param name="createdDuringViewFixup"></param>
        protected override void OnChildConfiguring(ShapeElement child, bool createdDuringViewFixup)
        {
            base.OnChildConfiguring(child, createdDuringViewFixup);

            if (!Store.TransactionManager.CurrentTransaction.TopLevelTransaction.IsSerializing)
                ArrangeShapes();
        }

        /// <summary>
        /// Shape instance initialization.
        /// </summary>
        public override void OnInitialize()
        {
            base.OnInitialize();
            if (!Store.TransactionManager.CurrentTransaction.TopLevelTransaction.IsSerializing)
            {
                Bounds = new RectangleD(new PointD(0.2, 0.2), DefaultSize);
            }
        }

        //public override bool CanMove
        //{
        //    get
        //    {
        //        return false;
        //    }
        //}

        /// <summary>
        /// Avant l'affichage, on s'assure que tous les layerspackages sont bien en arrière plan
        /// </summary>
        protected override void OnBeforePaint()
        {
            base.OnBeforePaint();

            bool sendToBack = false;
            for (int idx = 0; idx < NestedChildShapes.Count; idx++)
            {
                ShapeElement shape = NestedChildShapes[idx];

                if (shape is LayerPackageShape || shape is InterfaceLayerShape)
                {
                    if (sendToBack)
                    {
                        // TIPS Zorder=0
                        // Back to front
                        using (
                            Transaction transaction =
                                Store.TransactionManager.BeginTransaction("Send layerPackage to back"))
                        {
                            NestedChildShapes.Move(shape, 0);
                            transaction.Commit();
                        }
                    }
                }
                else
                {
                    // Si on rencontre, un autre enfant avant,
                    // les layerPackages suivant devront être déplacés
                    sendToBack = true;
                }
            }
        }

        /// <summary>
        /// TIPS Affectation d'un parent graphique diffèrent que le parent du modèle à un enfant
        /// </summary>
        /// <param name="childShape">The child shape.</param>
        /// <returns>The parent shape for the child shape.</returns>
        public override ShapeElement ChooseParentShape(ShapeElement childShape)
        {
            // Les layers apparaissent graphiquement dans un PackageLayer mais leur père reste
            // le composant
            if (childShape.ModelElement is Layer)
            {
                // Recherche du shape du layerPackage
                LayerPackage package = ((Layer) childShape.ModelElement).LayerPackage;
                if (package != null)
                {
                    IList<PresentationElement> shapes = PresentationViewsSubject.GetPresentation(package);
                    if (shapes.Count > 0)
                    {
                        NodeShape parentShape = (NodeShape) shapes[0];
                        return parentShape;
                    }
                }
            }

            if (childShape.ModelElement is DataLayer)
            {
                CandleModel model = ((DataLayer) childShape.ModelElement).Component.Model;
                if (model != null)
                {
                    IList<PresentationElement> shapes = PresentationViewsSubject.GetPresentation(model);
                    if (shapes.Count > 0)
                    {
                        NodeShape parentShape = (NodeShape) shapes[0];
                        return parentShape;
                    }
                }
            }

            return base.ChooseParentShape(childShape);
        }

        /// <summary>
        /// Initializes style set resources for this shape type
        /// </summary>
        /// <param name="classStyleSet">The style set for this shape class</param>
        protected override void InitializeResources(StyleSet classStyleSet)
        {
            base.InitializeResources(classStyleSet);

            BrushSettings brushSettings = new BrushSettings();
            brushSettings.Color = Color.FromArgb(255, 234, 197);
            classStyleSet.OverrideBrush(headerBackgroundBrushId, brushSettings);
        }

        /// <summary>
        /// Initialize the collection of shape fields associated with this shape type.
        /// </summary>
        /// <param name="shapeFields"></param>
        protected override void InitializeShapeFields(IList<ShapeField> shapeFields)
        {
            base.InitializeShapeFields(shapeFields);

            AreaField area = new AreaField("HeaderArea", 0f);
            area.DrawBorder = false;
            area.DefaultVisibility = true;
            area.DefaultAccessibleDescription = "Component Header";
            area.FillBackground = true;
            area.GradientEndingColor = Color.White;
            area.DefaultLinearGradientMode = LinearGradientMode.Vertical;
            area.DefaultBackgroundBrushId = headerBackgroundBrushId;
            area.DefaultFocusable = false;
            area.DefaultReflectParentSelectedState = false;
            area.DefaultSelectable = false;
            area.AnchoringBehavior.SetTopAnchor(AnchoringBehavior.Edge.Top, 0.01);
            area.AnchoringBehavior.SetLeftAnchor(AnchoringBehavior.Edge.Left, 0.01);
            area.AnchoringBehavior.SetRightAnchor(AnchoringBehavior.Edge.Right, 0.01);
            area.DefaultHeight = HEADER_HEIGHT - 0.01;
            shapeFields.Insert(0, area);
        }

        /// <summary>
        /// Alerts listeners when the shape is dragged over its bounds.
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
                SoftwareComponent component = ModelElement as SoftwareComponent;
                ImportProjectHelper.Import(e.Data.GetData("CF_VSREFPROJECTS"), component, null);
            }
        }

        //public void ArrangeShapes()
        //{
        //    // Tri des couches par niveau de la plus haute à la plus basse
        //    SortedList<int, NodeShape> shapes = new SortedList<int, NodeShape>(new SoftwareComponent.LayerLevelComparer());
        //    foreach (NodeShape shape2 in this.NestedChildShapes)
        //    {
        //        if (shape2.ModelElement is ISortedLayer)
        //        {
        //            shapes.Add(((ISortedLayer)shape2.ModelElement).Level, shape2);
        //        }
        //    }

        //    // Placement des couches horizontalement
        //    double border = 0.0;
        //    Pen p = StyleSet.GetPen(OutlinePenId);
        //    if (p != null)
        //        border = p.Width;
        //    double Y = MARGIN;
        //    foreach (int level in shapes.Keys)
        //    {
        //        NodeShape shape = shapes[level];
        //        RectangleD rec = new RectangleD(border, Y, this.Bounds.Width - (2 * border), shape.Bounds.Height - (2 * border));
        //        shape.Bounds = rec;
        //        Y += shape.Bounds.Height;
        //    }
        //}
    }
}