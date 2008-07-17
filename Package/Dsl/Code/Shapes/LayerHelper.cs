using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using DSLFactory.Candle.SystemModel.Strategies;
using EnvDTE;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Diagrams;

namespace DSLFactory.Candle.SystemModel
{
    /// <summary>
    /// 
    /// </summary>
    internal static class DragDropHelper
    {
        #region Import d'un service par drag'n drop

        /// <summary>
        /// Import d'un service
        /// </summary>
        /// <param name="shape">The shape.</param>
        /// <param name="e">The <see cref="Microsoft.VisualStudio.Modeling.Diagrams.DiagramDragEventArgs"/> instance containing the event data.</param>
        internal static void OnDragDropOnLayer(NodeShape shape, DiagramDragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.Text))
            {
                string txt = (string) e.Data.GetData(DataFormats.Text);
                using (
                    Transaction transaction =
                        shape.ModelElement.Store.TransactionManager.BeginTransaction("Import interface"))
                {
                    // On ne veut pas que les assistants soient appelés
                    shape.ModelElement.Store.TransactionManager.CurrentTransaction.TopLevelTransaction.Context.
                        ContextInfo[StrategyManager.IgnoreStrategyWizards] = true;

                    IImportInterfaceHelper importer = ServiceLocator.Instance.GetService<IImportInterfaceHelper>();
                    if (importer == null)
                        return;
                    if (importer.ImportOperations(shape.ModelElement as SoftwareLayer, null, txt))
                    {
                        shape.RebuildShape();
                        transaction.Commit();
                    }
                }

                using (
                    Transaction transaction =
                        shape.ModelElement.Store.TransactionManager.BeginTransaction("Arrange ports"))
                {
                    LayerHelper.ArrangePorts(shape, typeof (ServiceContractShape));
                    transaction.Commit();
                }
            }
        }

        /// <summary>
        /// Drag d'élément d'un projet sur un layer
        /// </summary>
        /// <param name="shape">The shape.</param>
        /// <param name="e">The <see cref="Microsoft.VisualStudio.Modeling.Diagrams.DiagramDragEventArgs"/> instance containing the event data.</param>
        internal static void OnDragOverLayer(NodeShape shape, DiagramDragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.Text) &&
                (shape.ModelElement is InterfaceLayer || shape.ModelElement is Layer))
            {
                string txt = (string) e.Data.GetData(DataFormats.Text);
                if (File.Exists(txt))
                {
                    FileCodeModel fcm = ServiceLocator.Instance.ShellHelper.GetFileCodeModel(txt);
                    if (fcm != null)
                    {
                        foreach (CodeElement cn in fcm.CodeElements)
                        {
                            if (cn is CodeNamespace)
                            {
                                foreach (CodeElement ci in ((CodeNamespace) cn).Members)
                                {
                                    if (ci is CodeInterface && shape.ModelElement is InterfaceLayer ||
                                        // Une interface seulement sur la couche interface
                                        ci is CodeClass) // Une classe n'importe ou
                                    {
                                        e.Effect = DragDropEffects.Link;
                                        return;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        #endregion

        #region Import d'une entity par drag'n drop

        /// <summary>
        /// Import d'un service
        /// </summary>
        /// <param name="shape">The shape.</param>
        /// <param name="e">The <see cref="Microsoft.VisualStudio.Modeling.Diagrams.DiagramDragEventArgs"/> instance containing the event data.</param>
        /// <returns></returns>
        internal static bool OnDragDropOnPackage(NodeShape shape, DiagramDragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.Text))
            {
                string txt = (string) e.Data.GetData(DataFormats.Text);
                using (
                    Transaction transaction =
                        shape.ModelElement.Store.TransactionManager.BeginTransaction("Import entity"))
                {
                    // On ne veut pas que les assistants soient appelés
                    shape.ModelElement.Store.TransactionManager.CurrentTransaction.TopLevelTransaction.Context.
                        ContextInfo[StrategyManager.IgnoreStrategyWizards] = true;

                    IImportEntityHelper importer = ServiceLocator.Instance.GetService<IImportEntityHelper>();
                    if (importer == null)
                        return false;
                    if (importer.ImportProperties(shape.ModelElement as Package, null, txt))
                    {
                        shape.RebuildShape();
                        transaction.Commit();
                    }
                }

                return true;
            }
            return false;
        }

        /// <summary>
        /// Called when [drag over package].
        /// </summary>
        /// <param name="shape">The shape.</param>
        /// <param name="e">The <see cref="Microsoft.VisualStudio.Modeling.Diagrams.DiagramDragEventArgs"/> instance containing the event data.</param>
        /// <returns></returns>
        internal static bool OnDragOverPackage(NodeShape shape, DiagramDragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.Text) && (shape.ModelElement is Package))
            {
                string txt = (string) e.Data.GetData(DataFormats.Text);
                if (File.Exists(txt))
                {
                    // Il faut que ce soit une interface
                    FileCodeModel fcm = ServiceLocator.Instance.ShellHelper.GetFileCodeModel(txt);
                    if (fcm != null)
                    {
                        foreach (CodeElement cn in fcm.CodeElements)
                        {
                            if (cn is CodeNamespace)
                            {
                                foreach (CodeElement ci in ((CodeNamespace) cn).Members)
                                {
                                    if (ci is CodeClass && shape.ModelElement is Package)
                                    {
                                        e.Effect = DragDropEffects.Link;
                                        return true;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return false;
        }

        #endregion
    }

    /// <summary>
    /// 
    /// </summary>
    internal static class LayerHelper
    {
        /// <summary>
        /// Positionne les ports
        /// </summary>
        /// <param name="parentShape">The parent shape.</param>
        internal static void ArrangeShapes(NodeShape parentShape)
        {
            using (
                Transaction transaction = parentShape.Store.TransactionManager.BeginTransaction("Arrange port shapes"))
            {
                ArrangePorts(parentShape, typeof (ClassImplementationShape));
                transaction.Commit();
            }
        }

        /// <summary>
        /// Arranges the ports.
        /// </summary>
        /// <param name="parentShape">The parent shape.</param>
        /// <param name="portType">Type of the port.</param>
        internal static void ArrangePorts(NodeShape parentShape, Type portType)
        {
            List<NodeShape> portShapes = new List<NodeShape>();

            foreach (NodeShape childShape in parentShape.RelativeChildShapes)
            {
                if (childShape.GetType() == portType)
                    portShapes.Add(childShape);
            }

            if (portShapes.Count == 0)
                return;

            // Tri dans l'ordre d'affichage
            portShapes.Sort(new PortComparer());

            NodeShape portShape = portShapes[0];

            double gap = Math.Max(
                0,
                (parentShape.Bounds.Width - (2*0.15) - (portShapes.Count*portShape.Bounds.Width))/(portShapes.Count + 1)
                );

            RectangleD pos = new RectangleD(gap, portShape.Bounds.Top, portShape.Bounds.Width, portShape.Bounds.Height);

            foreach (NodeShape shape in portShapes)
            {
                shape.Bounds = pos;
                pos.X += shape.Bounds.Width + gap;
            }
        }

        #region Nested type: PortComparer

        /// <summary>
        /// 
        /// </summary>
        private class PortComparer : IComparer<NodeShape>
        {
            #region IComparer<NodeShape> Members

            /// <summary>
            /// Compares two objects and returns a value indicating whether one is less than, equal to, or greater than the other.
            /// </summary>
            /// <param name="x">The first object to compare.</param>
            /// <param name="y">The second object to compare.</param>
            /// <returns>
            /// Value Condition Less than zerox is less than y.Zerox equals y.Greater than zerox is greater than y.
            /// </returns>
            public int Compare(NodeShape x, NodeShape y)
            {
                if (x.AbsoluteBounds.Left < y.AbsoluteBounds.Left)
                {
                    return -1;
                }
                if (x.AbsoluteBounds.Left > y.AbsoluteBounds.Left)
                {
                    return 1;
                }
                return 0;
            }

            #endregion
        }

        #endregion
    }
}