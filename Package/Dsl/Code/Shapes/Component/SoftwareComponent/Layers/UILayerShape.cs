using System;
using DSLFactory.Candle.SystemModel.Commands;
using Microsoft.VisualStudio.Modeling.Diagrams;
using Microsoft.VisualStudio.Modeling.Shell;
using Microsoft.VisualStudio.OLE.Interop;
using Microsoft.VisualStudio.Shell.Interop;
using IServiceProvider=System.IServiceProvider;

namespace DSLFactory.Candle.SystemModel
{
    partial class UIWorkflowLayerShape : ISupportArrangeShapes
    {
        ///// <summary>
        ///// Texte des ports en vertical
        ///// </summary>
        ///// <param name="shapeFields"></param>
        ///// <param name="decorators"></param>
        //protected override void InitializeDecorators(IList<ShapeField> shapeFields, IList<Decorator> decorators)
        //{
        //    base.InitializeDecorators(shapeFields, decorators);

        //    // TIPS ecriture en vertical
        //    //Decorator decorator = FindDecorator(decorators, "NameDecorator");
        //    //TextField field = decorator.Field as TextField;
        //    ////   field.DefaultIsHorizontal = false;
        //    //field.DefaultStringFormat = new StringFormat(StringFormatFlags.DirectionVertical);
        //}

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
        /// Gets the child shape and checks to see whether its parent shape should be resized when the absolute bounds for the child shape change.
        /// </summary>
        /// <value></value>
        /// <returns>true if the parent shape should be resized when the absolute bounds for the child shape change; otherwise, false.</returns>
        public override bool AutoResizeParentOnBoundsChange
        {
            get { return true; }
        }

        /// <summary>
        /// Gets the margin between the parent shape and its nested child shapes.
        /// </summary>
        /// <value></value>
        /// <returns>The margin between the parent shape and its nested child shapes.</returns>
        public override SizeD DefaultContainerMargin
        {
            get { return new SizeD(0.1, 0.03); }
        }

        /// <summary>
        /// Gets the shape and checks to see whether it has a shadow.
        /// </summary>
        /// <value></value>
        /// <returns>true if the shape has a shadow; otherwise, false.</returns>
        public override bool HasShadow
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
        /// Positionne les ports
        /// </summary>
        void ISupportArrangeShapes.ArrangeShapes()
        {
            LayerHelper.ArrangeShapes(this);
        }

        #endregion

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
                                                                   new Guid("2A9B689E-9B03-4159-8AE3-0C4D51B67614"));
        }
    }
}