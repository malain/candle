using System;
using Microsoft.VisualStudio.Modeling.Diagrams;
using Microsoft.VisualStudio.Modeling.Shell;
using Microsoft.VisualStudio.Shell.Interop;

namespace DSLFactory.Candle.SystemModel.Commands
{
    /// <summary>
    /// Importation d'assemblies externes
    /// </summary>
    public class ShowDataLayerCommand : ICommand
    {
        private readonly Diagram _diagram;
        private readonly CandleModel _candleModel;

        /// <summary>
        /// Initializes a new instance of the <see cref="ShowDataLayerCommand"/> class.
        /// </summary>
        /// <param name="serviceProvider">The service provider.</param>
        /// <param name="component">The component.</param>
        /// <param name="diagram">The diagram.</param>
        public ShowDataLayerCommand(IServiceProvider serviceProvider, object component, Diagram diagram)
        {
            this._candleModel = component as CandleModel;
            this._diagram = diagram;
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="ICommand"/> is enabled.
        /// </summary>
        /// <value><c>true</c> if enabled; otherwise, <c>false</c>.</value>
        public bool Enabled
        {
            get { return Visible(); }
        }

        /// <summary>
        /// Execute the command
        /// </summary>
        public void Exec()
        {

            Guid logicalViewGuid = new Guid(LogicalViewID.ProjectSpecificEditor);
            ModelElementLocator locator = new ModelElementLocator((IServiceProvider)Microsoft.VisualStudio.Shell.Package.GetGlobalService(typeof(Microsoft.VisualStudio.OLE.Interop.IObjectWithSite)));
            ModelingDocView view = locator.FindDocView(logicalViewGuid, this._diagram);

            ModelingDocData docdata = view.DocData as ModelingDocData;
            if (docdata != null && docdata.FileName != null)
            {
                // Guid du DataLayerEditorFactory
                Guid guid1 = new Guid("56AF6F2B-EF94-4297-9857-8653A0AE02D8");
                ServiceLocator.Instance.IDEHelper.OpenModelsDiagram(docdata.FileName, guid1);
            }
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="ICommand"/> is visible.
        /// </summary>
        /// <returns></returns>
        /// <value><c>true</c> if visible; otherwise, <c>false</c>.</value>
        public bool Visible()
        {
            return _candleModel != null && _candleModel.SoftwareComponent != null && _candleModel.SoftwareComponent.IsDataLayerExists;
        }
    }
}
