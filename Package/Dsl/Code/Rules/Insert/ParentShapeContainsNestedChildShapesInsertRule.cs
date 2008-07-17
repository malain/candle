using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Diagrams;

namespace DSLFactory.Candle.SystemModel.Rules
{
    /// <summary>
    /// Positionnement des éléments
    /// </summary>
    [RuleOn(typeof (ParentShapeContainsNestedChildShapes),
        Priority = DiagramFixupConstants.AddConnectionRulePriority + 1, FireTime = TimeToFire.TopLevelCommit,
        InitiallyDisabled = false)]
    public class ParentShapeContainsNestedChildShapesInsertRule : AddRule
    {
        // Ajout de l'élément
        /// <summary>
        /// Alerts listeners that a rule has been used.
        /// </summary>
        /// <param name="e">An ElementAddedEventArgs that contains the event data.</param>
        public override void ElementAdded(ElementAddedEventArgs e)
        {
            // Test the element
            ParentShapeContainsNestedChildShapes link = e.ModelElement as ParentShapeContainsNestedChildShapes;
            if (link == null)
                return;
            if (link.Store.TransactionManager.CurrentTransaction.TopLevelTransaction.IsSerializing ||
                link.Store.InUndoRedoOrRollback)
                return;

            // Création d'une référence multi-couches. On essaye de positionner tous les éléments dans le même axe
            double X = UnplacedModelHelper.GetInitialPosition(link.Store);
            List<NodeShape> unplacedShapes = UnplacedModelHelper.GetUnplacedShapes(link.Store);
            if (unplacedShapes != null && X != 0.0)
            {
                foreach (NodeShape shape in unplacedShapes)
                {
                    RectangleD rec = shape.AbsoluteBounds;
                    rec.X = Math.Min(X, shape.ParentShape.AbsoluteBoundingBox.Right - rec.Width);
                    rec.Y = ((NodeShape) shape.ParentShape).AbsoluteBounds.Top + 0.15;
                    shape.AbsoluteBounds = rec;
                }
                return;
            }


            if (link.NestedChildShapes.ModelElement is ISortedLayer)
            {
                SoftwareComponentShape shape = link.ParentShape as SoftwareComponentShape;
                if (shape != null)
                {
                    shape.ArrangeShapes();
                    return;
                }
            }

            LayerPackageShape lps = link.ParentShape as LayerPackageShape;
            if (lps != null)
            {
                lps.ArrangeShapes();
                return;
            }

            if ((link.NestedChildShapes.ModelElement is ServiceContract ||
                 link.NestedChildShapes.ModelElement is ClassImplementation ||
                 link.NestedChildShapes.ModelElement is Process) && link.NestedChildShapes.BoundingBox.X == 0.0)
            {
                X = ((NodeShape) link.ParentShape).AbsoluteBounds.X;
                double Y = ((NodeShape) link.ParentShape).AbsoluteBounds.Y + 0.15;
                foreach (PresentationElement pel in link.ParentShape.NestedChildShapes)
                {
                    NodeShape child = pel as NodeShape;
                    if (child != null && child.AbsoluteBounds.Right > X)
                    {
                        Y = child.AbsoluteBounds.Top;
                        X = child.AbsoluteBounds.Right;
                    }
                }

                NodeShape shape = (NodeShape) link.NestedChildShapes;
                RectangleD rec = shape.AbsoluteBounds;
                rec.X = X + 0.15;
                rec.Y = Y;
                shape.AbsoluteBounds = rec;
            }
        }
    }
}