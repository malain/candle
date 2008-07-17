using System;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Diagrams;

namespace DSLFactory.Candle.SystemModel
{
    /// <summary>
    /// 
    /// </summary>
    public class ComponentShapeBoundsRule : BoundsRules
    {
        private static ComponentShapeBoundsRule s_instance = new ComponentShapeBoundsRule();

        /// <summary>
        /// Gets or sets the instance.
        /// </summary>
        /// <value>The instance.</value>
        public static ComponentShapeBoundsRule Instance
        {
            get { return s_instance; }
            set { s_instance = value; }
        }

        /// <summary>
        /// Called to validate the new shape or position of a shape.
        /// During resizing/reshaping, called repeatedly as the user moves the mouse.
        /// Provides feedback to the user in the rubber band.
        /// On the user moving a shape, this is called once to validate the new position.
        /// </summary>
        /// <param name="shape">Shape the user is moving or reshaping/resizing</param>
        /// <param name="proposedBounds">Bounds the user is asking for</param>
        /// <returns>Acceptable bounds</returns>
        public override RectangleD GetCompliantBounds(ShapeElement shape, RectangleD proposedBounds)
        {
            try
            {
                if (IsDeserializing(shape))
                    return proposedBounds;

                // La taille ne peut pas être plus petite que l'emplacement des enfants
                if (proposedBounds.Height < shape.AbsoluteBoundingBox.Height ||
                    proposedBounds.Width < shape.AbsoluteBoundingBox.Width)
                {
                    // resizing
                    SizeD minSize = ((NodeShape) shape).CalculateMinimumSizeBasedOnChildren();
                    if (proposedBounds.Width < minSize.Width)
                        proposedBounds.Width = minSize.Width;
                    if (proposedBounds.Height < minSize.Height)
                        proposedBounds.Height = minSize.Height;

                    return proposedBounds;
//                    return RestrictResize(component, componentShape, proposedBounds);
                }
                else
                {
                    return proposedBounds; // RestrictMovement(component, componentShape, proposedBounds);
                }
            }
            catch (NullReferenceException)
            {
                return proposedBounds;
            }
        }


        ///// <summary>
        ///// Get the shape that presents a component in a diagram.
        ///// </summary>
        ///// <param name="component"></param>
        ///// <param name="diagram">Diagram we're expecting the shape to be in.</param>
        ///// <returns></returns>
        //internal static T ShapeOf(Component component, Diagram diagram)
        //{
        //    foreach (PresentationElement pel in PresentationViewsSubject.GetPresentation(component))
        //    {
        //        T componentShape = pel as T;
        //        if (componentShape != null && componentShape.Diagram == diagram) return componentShape;
        //    }
        //    return null;
        //}

        /// <summary>
        /// Are we currently deserializing?
        /// </summary>
        /// <param name="element">Any item in the store</param>
        /// <returns>True iff currently deserializing.</returns>
        internal static bool IsDeserializing(ModelElement element)
        {
            Transaction transaction = element.Store.TransactionManager.CurrentTransaction;
            return transaction != null && transaction.IsSerializing;
        }

        /// <summary>
        /// Compare values ignoring rounding errors to 1 part in a million.
        /// </summary>
        /// <param name="a">A.</param>
        /// <param name="b">The b.</param>
        /// <returns></returns>
        internal static bool ApproxEqual(double a, double b)
        {
            return Math.Round(a, 6) == (Math.Round(b, 6));
        }

        ///// <summary>
        ///// As the user moves a side or corner, restrict movement so that child shapes are enclosed and the parent shape encloses.
        ///// </summary>
        ///// <param name="component"></param>
        ///// <param name="componentShape"></param>
        ///// <param name="proposedBounds"></param>
        ///// <returns></returns>
        //private static RectangleD RestrictResize(Component component, T componentShape, RectangleD proposedBounds)
        //{
        //    return new RectangleD(
        //    RectangleD acceptableBounds = proposedBounds;
        //    // Ensure we don't reduce so much as to exclude children's shapes. Consider only if we're reducing size.
        //    if (proposedBounds.Height < componentShape.AbsoluteBoundingBox.Height || proposedBounds.Width < componentShape.AbsoluteBoundingBox.Width)
        //    {
        //        RectangleD minimumBounds = ChildBounds(component, componentShape);
        //        if (minimumBounds.Height > 0.0)
        //        {
        //            // must enclose existing children
        //            acceptableBounds = RectangleD.Union(proposedBounds, minimumBounds);
        //        }
        //    }

        //    // Ensure we don't increase size beyond parent's boundary; consider only if we're increasing size.
        //    if (proposedBounds.Height > componentShape.AbsoluteBoundingBox.Height || proposedBounds.Width > componentShape.AbsoluteBoundingBox.Width)
        //    {
        //        if (component.Parent != null)
        //        {
        //            T parentShape = ShapeOf(component.Parent, componentShape.Diagram);
        //            if (parentShape != null)
        //            {
        //                acceptableBounds = RectangleD.Intersect(acceptableBounds, parentShape.AbsoluteBounds);
        //            }
        //        }
        //    }
        //    return acceptableBounds;
        //}


        ///// <summary>
        ///// Find minimal rectangle that encloses shapes of children
        ///// </summary>
        ///// <param name="componentShape"></param>
        ///// <returns>Empty rectangle if no children</returns>
        //private static RectangleD ChildBounds(Component component, T componentShape)
        //{
        //    RectangleD minimal = RectangleD.Empty;

        //    bool doneFirstChild = false;
        //    foreach (Component child in component.Children)
        //    {
        //        T childShape = ShapeOf(child, componentShape.Diagram);
        //        if (childShape != null)
        //        {
        //            if (!doneFirstChild)
        //            {
        //                minimal = childShape.AbsoluteBounds;
        //                doneFirstChild = true;
        //            }
        //            else
        //            {
        //                minimal = RectangleD.Union(minimal, childShape.AbsoluteBounds);
        //            }
        //        }
        //    }

        //    return minimal;
        //}
    }
}