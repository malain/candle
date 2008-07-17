using System.Windows.Forms;
using DSLFactory.Candle.SystemModel.Strategies;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Diagrams;

namespace DSLFactory.Candle.SystemModel
{
    public partial class ServiceContractShape
    {
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
        /// Gets the child compartment shape and checks to see whether it can resize its parent compartment shape.
        /// </summary>
        /// <value></value>
        /// <returns>true if a child compartment shape can resize its parent compartment shape; otherwise, false.</returns>
        public override bool AllowsChildrenToResizeParent
        {
            get { return true; }
        }

        /// <summary>
        /// Gets the mouse action to perform for a MouseDown event over a specific point in the diagram.
        /// </summary>
        /// <param name="mouseButtons">The mouse buttons that can cause the MouseDown event.</param>
        /// <param name="point">The point on the diagram, relative to the top-left point of the diagram.</param>
        /// <param name="hitTestInfo">The hit test information.</param>
        /// <returns>
        /// The mouse action to perform for a MouseDown event over a specific point in the diagram.
        /// </returns>
        public override MouseAction GetPotentialMouseAction(MouseButtons mouseButtons, PointD point,
                                                            DiagramHitTestInfo hitTestInfo)
        {
            MouseAction action = base.GetPotentialMouseAction(mouseButtons, point, hitTestInfo);
            if (Utils.IsKeyPressed(Keys.Alt))
            {
                return ((ComponentModelDiagram) Diagram).ReferenceConnectAction;
            }
            return action;
        }

        #region Affichage au dessus

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

        #endregion

        #region AdjusteSize

        /// <summary>
        /// Alerts listeners when the mouse is double-clicked over the shape.
        /// </summary>
        /// <param name="e">The diagram point event arguments.</param>
        public override void OnDoubleClick(DiagramPointEventArgs e)
        {
            base.OnDoubleClick(e);

            // Avec shift = AdjustSize
            if (Utils.IsKeyPressed(Keys.Shift))
            {
                using (Transaction transaction = Store.TransactionManager.BeginTransaction("Adjust size"))
                {
                    ShapeHelper.ResizeToContent(this);
                    transaction.Commit();
                }
                return;
            }

            ServiceContract contract = ModelElement as ServiceContract;
            if (contract != null) Mapper.Instance.ShowCode(contract.Id, contract.Name, null);
        }

        /// <summary>
        /// Alerts listeners that the shape has been assigned as a child shape to a parent shape.
        /// </summary>
        public override void OnShapeInserted()
        {
            base.OnShapeInserted();
            if (!Store.InUndoRedoOrRollback)
            {
                using (Transaction transaction = Store.TransactionManager.BeginTransaction("Adjust size"))
                {
                    ShapeHelper.ResizeToContent(this);
                    transaction.Commit();
                }
            }
        }

        /// <summary>
        /// Arranges the shapes.
        /// </summary>
        public void ArrangeShapes()
        {
            ShapeHelper.ResizeToContent(this);
        }

        #endregion

        ///// <summary>
        ///// On veut que les ports restent sur le haut des couches. On utilise un <see cref="BoundsRules"/> 
        ///// pour forcer son emplacement sauf pour les externals layers
        ///// </summary>
        //public override Microsoft.VisualStudio.Modeling.Diagrams.BoundsRules BoundsRules
        //{
        //    get
        //    {
        //        if( this.ModelElement == null )//  || ( (ServicePort)this.ModelElement ).Parent is DotNetAssembly )
        //            return base.BoundsRules;

        //        return new ConstrainedPortBoundsRules( ConstrainedPortBoundsRules.PortPosition.Center);
        //    }
        //}
    }
}