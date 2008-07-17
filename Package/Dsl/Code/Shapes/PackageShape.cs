using System.Windows.Forms;
using DSLFactory.Candle.SystemModel.Commands;
using DSLFactory.Candle.SystemModel.Utilities;
using EnvDTE;
using Microsoft.VisualStudio.Modeling.Diagrams;
using Microsoft.VisualStudio.Modeling.Shell;
using Microsoft.VisualStudio.OLE.Interop;
using Microsoft.VisualStudio.Shell;

namespace DSLFactory.Candle.SystemModel
{
    partial class PackageShape : ISupportArrangeShapes
    {
        /// <summary>
        /// Gets a shape and checks to see whether its nested child shapes should be automatically positioned on the diagram.
        /// </summary>
        /// <value></value>
        /// <returns>true if the nested child shapes should be automatically positioned; otherwise, false.</returns>
        public override bool ShouldAutoPlaceChildShapes
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
        /// Gets the child shape and checks to see whether its parent shape can be resized when the child shape is resized.
        /// </summary>
        /// <value></value>
        /// <returns>true if the parent of a child shape can be resized when the child shape is resized; otherwise, false.</returns>
        public override bool AllowsChildrenToResizeParent
        {
            get { return true; }
        }

        #region ISupportArrangeShapes Members

        /// <summary>
        /// Arranges the shapes.
        /// </summary>
        public void ArrangeShapes()
        {
            ShapeHelper.ArrangeChildShapes(this, NestedChildShapes, 0, 3, new PointD(0.2, 0.2), 0.1, 0.1);
        }

        #region Drag and drop d'une table du serveur explorer

        /// <summary>
        /// Alerts listeners when the shape is dragged and dropped.
        /// </summary>
        /// <param name="e">The diagram drag event arguments.</param>
        public override void OnDragDrop(DiagramDragEventArgs e)
        {
            base.OnDragDrop(e);

            bool canArrangeShapes = NestedChildShapes.Count == 0;

            if (DragDropHelper.OnDragDropOnPackage(this, e))
            {
                if (canArrangeShapes)
                    ArrangeShapes();
                return;
            }

            if (e.Data.GetDataPresent(ServerExplorerHelper.DataSourceReferenceFormat))
            {
                if (ServerExplorerHelper.ContainsTable(e.Data))
                {
                    if (ServerExplorerHelper.ImportTables(ModelElement as Package,
                                                          new ServiceProvider(
                                                              (IServiceProvider)
                                                              ModelingPackage.GetGlobalService(typeof (DTE))),
                                                          e.Data))
                    {
                        if (canArrangeShapes)
                            ArrangeShapes();
                    }
                }
                if (ServerExplorerHelper.ContainsStoredProcedures(e.Data))
                {
                    if (ServerExplorerHelper.ImportStoredProcedures(ModelElement as Package,
                                                                    new ServiceProvider(
                                                                        (IServiceProvider)
                                                                        ModelingPackage.GetGlobalService(typeof (DTE))),
                                                                    e.Data))
                    {
                        if (canArrangeShapes)
                            ArrangeShapes();
                    }
                }
            }
        }

        /// <summary>
        /// Alerts listeners when the shape is dragged over its bounds.
        /// </summary>
        /// <param name="e">The diagram drag event arguments.</param>
        public override void OnDragOver(DiagramDragEventArgs e)
        {
            base.OnDragOver(e);

            if (DragDropHelper.OnDragOverPackage(this, e))
                return;

            if (e.Data.GetDataPresent(ServerExplorerHelper.DataSourceReferenceFormat))
            {
                if (ServerExplorerHelper.ContainsTable(e.Data) || ServerExplorerHelper.ContainsStoredProcedures(e.Data))
                    e.Effect = DragDropEffects.Copy;
            }
        }

        #endregion

        #endregion
    }
}