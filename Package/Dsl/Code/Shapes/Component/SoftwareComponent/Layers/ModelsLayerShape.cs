using System;
using Microsoft.VisualStudio.Modeling.Diagrams;
using Microsoft.VisualStudio.Modeling.Shell;
using Microsoft.VisualStudio.OLE.Interop;
using Microsoft.VisualStudio.Shell.Interop;
using IServiceProvider=System.IServiceProvider;

namespace DSLFactory.Candle.SystemModel
{
    partial class DataLayerShape
    {
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
        /// Ouverture du diagramme dédié
        /// </summary>
        /// <param name="e">The diagram point event arguments.</param>
        public override void OnDoubleClick(DiagramPointEventArgs e)
        {
            base.OnDoubleClick(e);

            // TODO dans un helper
            Guid logicalViewGuid = new Guid(LogicalViewID.ProjectSpecificEditor);
            ModelElementLocator locator =
                new ModelElementLocator(
                    (IServiceProvider) Microsoft.VisualStudio.Shell.Package.GetGlobalService(typeof (IObjectWithSite)));
            ModelingDocView view = locator.FindDocView(logicalViewGuid, Diagram);

            ModelingDocData docdata = view.DocData;
            if (docdata != null)
            {
                OpenDiagram(docdata.FileName);
            }
        }

        /// <summary>
        /// Opens the diagram.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        private static void OpenDiagram(string fileName)
        {
            ServiceLocator.Instance.ShellHelper.EnsureDocumentOpen(fileName,
                                                                   new Guid("56AF6F2B-EF94-4297-9857-8653A0AE02D8"));
        }
    }
}