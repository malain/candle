using DSLFactory.Candle.SystemModel.Repository;
using Microsoft.VisualStudio.Modeling.Diagrams;

namespace DSLFactory.Candle.SystemModel.Commands
{
    /// <summary>
    /// Importation d'assemblies externes
    /// </summary>
    public class ShowPropertiesCommand : ICommand
    {
        private readonly ComponentModelMetadata _metadata;

        /// <summary>
        /// Initializes a new instance of the <see cref="ShowPropertiesCommand"/> class.
        /// </summary>
        /// <param name="shape">The shape.</param>
        public ShowPropertiesCommand(object shape)
        {
            if (shape == null)
                return;
                
            ExternalComponent ext = ((PresentationElement)shape).ModelElement as ExternalComponent;
            if (ext != null)
            {
                _metadata = ext.MetaData;
                return;
            }

            CandleModel model = ((PresentationElement)shape).ModelElement as CandleModel;
            if (model != null)
                _metadata = model.MetaData;

        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="ICommand"/> is enabled.
        /// </summary>
        /// <value><c>true</c> if enabled; otherwise, <c>false</c>.</value>
        public bool Enabled
        {
            get
            {
                return _metadata != null;
            }
        }

        /// <summary>
        /// Execute the command
        /// </summary>
        public void Exec()
        {
            RepositoryPropertiesForm dlg = new RepositoryPropertiesForm(_metadata);
            dlg.ShowDialog();
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="ICommand"/> is visible.
        /// </summary>
        /// <returns></returns>
        /// <value><c>true</c> if visible; otherwise, <c>false</c>.</value>
        public bool Visible()
        {
            return _metadata != null;
        }
    }
}
