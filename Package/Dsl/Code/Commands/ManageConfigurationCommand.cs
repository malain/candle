using Microsoft.VisualStudio.Modeling.Diagrams;

namespace DSLFactory.Candle.SystemModel.Commands
{
    /// <summary>
    /// Importation d'assemblies externes
    /// </summary>
    public class ManageConfigurationCommand : ICommand
    {
        private readonly SoftwareComponent _component;
        private readonly ExternalComponent _externalComponent;

        /// <summary>
        /// Initializes a new instance of the <see cref="ManageConfigurationCommand"/> class.
        /// </summary>
        /// <param name="shape">The shape.</param>
        public ManageConfigurationCommand(object shape)
        {
            if (shape != null)
            {
                _component = ((PresentationElement)shape).ModelElement as SoftwareComponent;
                if (_component == null)
                {
                    CandleModel model = ((PresentationElement)shape).ModelElement as CandleModel;
                    if (model != null)
                        _component = model.SoftwareComponent;
                    else
                    {
                        _externalComponent = ((PresentationElement)shape).ModelElement as ExternalComponent;
                    }
                }
            }
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="ICommand"/> is enabled.
        /// </summary>
        /// <value><c>true</c> if enabled; otherwise, <c>false</c>.</value>
        public bool Enabled
        {
            get
            {
                return Visible();
            }
        }

        /// <summary>
        /// Execute the command
        /// </summary>
        public void Exec()
        {
            ConfigurationEditorDialog dlg = _component != null ? new ConfigurationEditorDialog(_component) : new ConfigurationEditorDialog(_externalComponent);
            dlg.ShowDialog();
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="ICommand"/> is visible.
        /// </summary>
        /// <returns></returns>
        /// <value><c>true</c> if visible; otherwise, <c>false</c>.</value>
        public bool Visible()
        {
            return _component != null || (_externalComponent != null && _externalComponent.ReferencedModel!=null);
        }
    }
}
