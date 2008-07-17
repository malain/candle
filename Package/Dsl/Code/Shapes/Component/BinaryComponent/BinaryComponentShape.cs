using System.Collections.Generic;
using DSLFactory.Candle.SystemModel.Commands;
using Microsoft.VisualStudio.Modeling.Diagrams;

namespace DSLFactory.Candle.SystemModel
{
    partial class BinaryComponentShape : ISupportArrangeShapes
    {
        ///// <summary>
        ///// Gets the shape and checks to see whether it has a shadow.
        ///// </summary>
        ///// <value></value>
        ///// TODO a faire
        //public override void OnDragOver(DiagramDragEventArgs e)
        //{
        //    base.OnDragOver(e);
        //    Project p = SolutionExplorerHelper.GetProjectReferenceFromDragAndDrop(e.Data, this.Store);
        //    if (p!=null)
        //    {
        //        // Vérif si c'est une dll
        //        e.Effect = DragDropEffects.Link;
        //    }
        //}
        //public override void OnDragDrop(DiagramDragEventArgs e)
        //{
        //    base.OnDragDrop(e);
        //    Project p = SolutionExplorerHelper.GetProjectReferenceFromDragAndDrop(e.Data, this.Store);
        //    if (p == null)
        //        return;
        //    import de l'assembly et ajout de ces références
        //}

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
        /// Gets the child shape and checks to see whether its parent shape can be resized when the child shape is resized.
        /// </summary>
        /// <value></value>
        /// <returns>true if the parent of a child shape can be resized when the child shape is resized; otherwise, false.</returns>
        public override bool AllowsChildrenToResizeParent
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
        /// Arranges the shapes.
        /// </summary>
        public void ArrangeShapes()
        {
            double verticalStartPoint = 0.4;

            BinaryComponent component = ModelElement as BinaryComponent;
            if (component == null)
                return;
            Dictionary<ShapeElement, bool> shapes = new Dictionary<ShapeElement, bool>();
            foreach (ShapeElement shape in NestedChildShapes)
            {
                if (shape is NodeShape)
                    shapes.Add(shape, false);
            }

            ShapeHelper.ArrangeChildShapes(this, NestedChildShapes, AbsoluteBounds.Width, 0,
                                           new PointD(0.2, verticalStartPoint), 0.3, 0.5);
        }

        #endregion

        /// <summary>
        /// TIPS placement d'un shape
        /// </summary>
        /// <param name="fixupState">The state of the bounds.</param>
        /// <param name="iteration">The iteration.</param>
        /// <param name="createdDuringViewFixup">true if a child shape was created during the fix up process; otherwise, false.</param>
        public override void OnBoundsFixup(BoundsFixupState fixupState, int iteration, bool createdDuringViewFixup)
        {
            base.OnBoundsFixup(fixupState, iteration, createdDuringViewFixup);
            AbsoluteBounds = new RectangleD(0, 0, Size.Width, Size.Height);
        }
    }
}