using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Diagrams;

namespace DSLFactory.Candle.SystemModel
{
    partial class EnumTypeShape
    {
        /// <summary>
        /// Sets value for the <see cref="P:Microsoft.VisualStudio.Modeling.Diagrams.NodeShape.IsExpanded"></see> property.
        /// </summary>
        /// <param name="newValue">The new value for the value for the <see cref="P:Microsoft.VisualStudio.Modeling.Diagrams.NodeShape.IsExpanded"></see> property.</param>
        protected override void SetIsExpandedValue(bool newValue)
        {
            // On place le shape devant tous les autres
            if (newValue && !Store.InUndoRedoOrRollback)
            {
                ParentShape.NestedChildShapes.Move(this, ParentShape.NestedChildShapes.Count - 1);
            }
            base.SetIsExpandedValue(newValue);
        }

        /// <summary>
        /// Alerts listeners when the mouse is double-clicked over the shape.
        /// </summary>
        /// <param name="e">The diagram point event arguments.</param>
        public override void OnDoubleClick(DiagramPointEventArgs e)
        {
            base.OnDoubleClick(e);

            using (Transaction transaction = Store.TransactionManager.BeginTransaction("Adjust size"))
            {
                ShapeHelper.ResizeToContent(this);
                transaction.Commit();
            }
        }

        /// <summary>
        /// Alerts listeners that the shape has been assigned as a child shape to a parent shape.
        /// </summary>
        public override void OnShapeInserted()
        {
            base.OnShapeInserted();
            using (Transaction transaction = Store.TransactionManager.BeginTransaction("Adjust size"))
            {
                ShapeHelper.ResizeToContent(this);
                IsExpanded = false;
                transaction.Commit();
            }
        }
    }
}