//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using VSShellInterop=Microsoft.VisualStudio.Shell.Interop;
using DslShell=Microsoft.VisualStudio.Modeling.Shell;

namespace DSLFactory.Candle.SystemModel
{
    /// <summary>
    /// Factory for creating our editors
    /// </summary>
    [global::System.Runtime.InteropServices.Guid( "56AF6F2B-EF94-4297-9857-8653A0AE02D8" )]
    internal class DataLayerEditorFactory : DslShell::ModelingEditorFactory
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="serviceProvider">Service provider used to access VS services.</param>
        public DataLayerEditorFactory( global::System.IServiceProvider serviceProvider )
            : base( serviceProvider )
        {
        }

        /// <summary>
        /// Called by the shell to ask the editor to create a new document object.
        /// </summary>
        protected override DslShell::ModelingDocData CreateDocData( string fileName, VSShellInterop::IVsHierarchy hierarchy, uint itemId )
        {
            // Create the document type supported by this editor.
            return new CandleDocData( this.ServiceProvider, typeof( DataLayerEditorFactory ).GUID );
        }

        /// <summary>
        /// Called by the shell to ask the editor to create a new view object.
        /// </summary>
        protected override DslShell::ModelingDocView CreateDocView( DslShell::ModelingDocData docData, string physicalView, out string editorCaption )
        {
            // Create the view type supported by this editor.
            editorCaption = " [Models]";
            return new DataLayerDocView( docData, this.ServiceProvider );
        }
    }
}

