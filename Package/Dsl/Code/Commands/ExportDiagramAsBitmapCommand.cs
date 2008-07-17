using System;
using Microsoft.VisualStudio.Modeling.Diagrams;
using Microsoft.VisualStudio.Modeling.Shell;

namespace DSLFactory.Candle.SystemModel.Commands
{
    /// <summary>
    /// 
    /// </summary>
    public class ExportDiagramAsBitmapCommand : ICommand
    {
        private readonly Diagram _diagram;
        private readonly IServiceProvider _serviceProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExportDiagramAsBitmapCommand"/> class.
        /// </summary>
        /// <param name="serviceProvider">The service provider.</param>
        /// <param name="element">The element.</param>
        public ExportDiagramAsBitmapCommand(IServiceProvider serviceProvider, object element)
        {
            this._serviceProvider = serviceProvider;
            _diagram = element as Diagram;
        }

        #region ICommand Members
        /// <summary>
        /// Gets a value indicating whether this <see cref="ICommand"/> is enabled.
        /// </summary>
        /// <value><c>true</c> if enabled; otherwise, <c>false</c>.</value>
        public bool Enabled
        {
            get { return Visible(); }
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="ICommand"/> is visible.
        /// </summary>
        /// <returns></returns>
        /// <value><c>true</c> if visible; otherwise, <c>false</c>.</value>
        public bool Visible()
        {
            return (_diagram != null);
        }

        /// <summary>
        /// Execute the command
        /// </summary>
        public void Exec()
        {
            ModelElementLocator locator = new ModelElementLocator( _serviceProvider );
            ModelingDocView view = locator.FindDocView( Guid.Empty, _diagram );
            if( view != null )
            {
                System.Windows.Forms.FolderBrowserDialog ofd = new System.Windows.Forms.FolderBrowserDialog();
                ofd.ShowNewFolderButton = true;
                if( ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK )
                {
                    string fileName = view.DocData.FileName;
                    DiagramExporter exporter = new DiagramExporter( _serviceProvider );
                    exporter.ExportErrorEvent += exporter_ExportErrorEvent;
                    exporter.ExportDiagram( fileName, ofd.SelectedPath, System.Drawing.Imaging.ImageFormat.Png, true );
                    exporter.ExportErrorEvent -= exporter_ExportErrorEvent;
                }
            }
        }

        /// <summary>
        /// Handles the ExportErrorEvent event of the exporter control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="Microsoft.VisualStudio.Modeling.Shell.ExportErrorEventArgs"/> instance containing the event data.</param>
        void exporter_ExportErrorEvent(object sender, ExportErrorEventArgs e)
        {
        }
        #endregion
    }
}
