using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Diagrams;

namespace DSLFactory.Candle.SystemModel.Commands
{
    /// <summary>
    /// Importation d'assemblies externes
    /// </summary>
    public class ManageArtifactsCommand : ICommand
    {
        private readonly AbstractLayer _layer;

        /// <summary>
        /// Initializes a new instance of the <see cref="ManageArtifactsCommand"/> class.
        /// </summary>
        /// <param name="model">The model.</param>
        public ManageArtifactsCommand( object model )
        {
            _layer = null;
            if( model is NodeShape )
            {
                _layer = ((NodeShape)model).ModelElement as AbstractLayer;
            }
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
            using( Transaction transaction = _layer.Store.TransactionManager.BeginTransaction( "Update artifacts" ) )
            {
                ArtifactEditorDialog dlg = new ArtifactEditorDialog(_layer);
                if( dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK )
                {
                    transaction.Commit();
                }
            }
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="ICommand"/> is visible.
        /// </summary>
        /// <returns></returns>
        /// <value><c>true</c> if visible; otherwise, <c>false</c>.</value>
        public bool Visible()
        {
            return _layer != null;
        }       
    }
}
