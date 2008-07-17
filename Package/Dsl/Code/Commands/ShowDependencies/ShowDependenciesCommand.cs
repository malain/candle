using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Diagrams;

namespace DSLFactory.Candle.SystemModel.Commands
{
    /// <summary>
    /// Importation d'assemblies externes
    /// </summary>
    public class ShowDependenciesCommand : ICommand
    {
        private readonly ModelElement _system;

        /// <summary>
        /// Initializes a new instance of the <see cref="ShowDependenciesCommand"/> class.
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <param name="fileName">Name of the file.</param>
        public ShowDependenciesCommand( object obj, string fileName )
        {
            NodeShape shape = obj as NodeShape;
            if( shape != null )
                _system = shape.ModelElement;
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
            ShowDependenciesForm form = new ShowDependenciesForm( _system );
            form.ShowDialog();            
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="ICommand"/> is visible.
        /// </summary>
        /// <returns></returns>
        /// <value><c>true</c> if visible; otherwise, <c>false</c>.</value>
        public bool Visible()
        {
            return _system != null;
        }       
    }
}
