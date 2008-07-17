using DSLFactory.Candle.SystemModel.Strategies;
using Microsoft.VisualStudio.Modeling.Diagrams;

namespace DSLFactory.Candle.SystemModel.Commands
{
    /// <summary>
    /// Importation d'assemblies externes
    /// </summary>
    public class ShowStrategiesCommand : ICommand
    {
        private readonly CandleElement _model;
        private readonly SoftwareComponent _component;

        /// <summary>
        /// Initializes a new instance of the <see cref="ShowStrategiesCommand"/> class.
        /// </summary>
        /// <param name="component">The component.</param>
        /// <param name="element">The element.</param>
        /// <param name="fileName">Name of the file.</param>
        public ShowStrategiesCommand(SoftwareComponent component, object element, string fileName )
        {
            this._component = component;
            PresentationElement pel = element as PresentationElement;
            if( pel != null )
                this._model = pel.ModelElement as CandleElement;
        }

        /// <summary>
        /// Execute the command
        /// </summary>
        public void Exec()
        {
            if( _model == null )
                return;

            StrategiesForm dlg = new StrategiesForm(_component, _model);
            dlg.ShowDialog();
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="ICommand"/> is visible.
        /// </summary>
        /// <returns></returns>
        /// <value><c>true</c> if visible; otherwise, <c>false</c>.</value>
        public bool Visible()
        {
            return _model != null ;
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="ICommand"/> is enabled.
        /// </summary>
        /// <value><c>true</c> if enabled; otherwise, <c>false</c>.</value>
        public bool Enabled
        {
            get { return _model != null && _component != null; }
        }
    }
}
