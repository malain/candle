using System.Collections.Generic;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Diagrams;

namespace DSLFactory.Candle.SystemModel
{
    /// <summary>
    /// Classe permettant de positionner les shapes créés au sein d'une même transaction dans leurs layers respectifs quand
    /// on tire une référence entre plusieurs couches
    /// </summary>
    public sealed class UnplacedModelHelper
    {
        /// <summary>
        /// Enregistre la position du shape initial pour aligner les autres verticalement
        /// </summary>
        /// <param name="store"></param>
        /// <param name="X">Position horizontale</param>
        public static void RegisterInitialPosition(Store store, double X)
        {
            object obj;
            store.TransactionManager.CurrentTransaction.TopLevelTransaction.Context.ContextInfo.TryGetValue(
                "{2B5191BC-1488-4863-A474-EF0918348E2D}", out obj);
            if (obj == null)
            {
                store.TransactionManager.CurrentTransaction.TopLevelTransaction.Context.ContextInfo.Add(
                    "{2B5191BC-1488-4863-A474-EF0918348E2D}", X);
            }
        }

        /// <summary>
        /// Récupère la position initiale
        /// </summary>
        /// <param name="store"></param>
        /// <returns></returns>
        public static double GetInitialPosition(Store store)
        {
            if (
                store.TransactionManager.CurrentTransaction.TopLevelTransaction.Context.ContextInfo.ContainsKey(
                    "{2B5191BC-1488-4863-A474-EF0918348E2D}"))
                return
                    (double)
                    store.TransactionManager.CurrentTransaction.TopLevelTransaction.Context.ContextInfo[
                        "{2B5191BC-1488-4863-A474-EF0918348E2D}"];
            return 0;
        }

        /// <summary>
        /// Registers the new model.
        /// </summary>
        /// <param name="store">The store.</param>
        /// <param name="element">The element.</param>
        public static void RegisterNewModel(Store store, ModelElement element)
        {
            object obj;
            List<ModelElement> elements;
            store.TransactionManager.CurrentTransaction.TopLevelTransaction.Context.ContextInfo.TryGetValue(
                "{FED4CA6E-2FFC-4751-A799-9B3808C384EA}", out obj);
            if (obj == null)
            {
                elements = new List<ModelElement>();
                store.TransactionManager.CurrentTransaction.TopLevelTransaction.Context.ContextInfo.Add(
                    "{FED4CA6E-2FFC-4751-A799-9B3808C384EA}", elements);
            }
            else
                elements = obj as List<ModelElement>;

            elements.Add(element);
        }

        /// <summary>
        /// Liste des shapes à positionner
        /// </summary>
        /// <param name="store">The store.</param>
        /// <returns></returns>
        public static List<NodeShape> GetUnplacedShapes(Store store)
        {
            object obj;
            store.TransactionManager.CurrentTransaction.TopLevelTransaction.Context.ContextInfo.TryGetValue(
                "{FED4CA6E-2FFC-4751-A799-9B3808C384EA}", out obj);
            if (obj == null)
                return null;
            List<ModelElement> elements = obj as List<ModelElement>;
            List<NodeShape> shapes = new List<NodeShape>();

            foreach (ModelElement element in elements)
            {
                foreach (PresentationElement pel in PresentationViewsSubject.GetPresentation(element))
                {
                    if (pel is NodeShape)
                    {
                        NodeShape shape = pel as NodeShape;
                        shapes.Add(shape);
                    }
                }
            }
            return shapes;
        }

        /// <summary>
        /// Gets the unplaced shapes for auto layout.
        /// </summary>
        /// <param name="store">The store.</param>
        /// <returns></returns>
        public static Dictionary<NodeShape, Dictionary<ShapeElement, bool>> GetUnplacedShapesForAutoLayout(Store store)
        {
            object obj;
            store.TransactionManager.CurrentTransaction.TopLevelTransaction.Context.ContextInfo.TryGetValue(
                "{FED4CA6E-2FFC-4751-A799-9B3808C384EA}", out obj);
            if (obj == null)
                return null;
            List<ModelElement> elements = obj as List<ModelElement>;

            Dictionary<NodeShape, Dictionary<ShapeElement, bool>> unplacedShapesMap =
                new Dictionary<NodeShape, Dictionary<ShapeElement, bool>>();

            foreach (ModelElement element in elements)
            {
                foreach (PresentationElement pel in PresentationViewsSubject.GetPresentation(element))
                {
                    if (pel is NodeShape)
                    {
                        NodeShape shape = pel as NodeShape;
                        PointD dropLocation =
                            DropTargetContext.GetDropLocation(
                                store.TransactionManager.CurrentTransaction.TopLevelTransaction);
                        shape.AbsoluteBounds =
                            new RectangleD(dropLocation.X, shape.AbsoluteBounds.Y, shape.AbsoluteBounds.Width,
                                           shape.AbsoluteBounds.Height);
                        NodeShape parentShape = shape.ParentShape as NodeShape;
                        if (parentShape != null)
                        {
                            Dictionary<ShapeElement, bool> shapeElementMap;
                            if (!unplacedShapesMap.TryGetValue(parentShape, out shapeElementMap))
                            {
                                shapeElementMap = new Dictionary<ShapeElement, bool>();
                                unplacedShapesMap.Add(parentShape, shapeElementMap);
                            }

                            shapeElementMap.Add(shape, true);
                        }
                    }
                }
            }
            return unplacedShapesMap;
        }
    }
}