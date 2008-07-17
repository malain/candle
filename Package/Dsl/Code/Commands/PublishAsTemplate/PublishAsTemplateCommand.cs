using System.IO;
using DSLFactory.Candle.SystemModel.Repository;
using DSLFactory.Candle.SystemModel.Strategies;
using DSLFactory.Candle.SystemModel.Wizard;
using Microsoft.VisualStudio.Modeling.Diagrams;

namespace DSLFactory.Candle.SystemModel.Commands
{
    /// <summary>
    /// Publication du modele en tant que template
    /// </summary>
    public class PublishAsTemplateCommand : ICommand
    {
        private readonly string _fileName;
        private readonly CandleModel _model;

        /// <summary>
        /// Initializes a new instance of the <see cref="PublishAsTemplateCommand"/> class.
        /// </summary>
        /// <param name="selection">The selection.</param>
        /// <param name="fileName">Name of the file.</param>
        public PublishAsTemplateCommand(object selection, string fileName)
        {
            this._fileName = fileName;
            NodeShape shape = selection as NodeShape;
            if (shape != null)
                this._model = shape.ModelElement as CandleModel;
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
            if (_model == null)
                return false;
            return _model.SoftwareComponent != null;
        }

        /// <summary>
        /// Execute the command
        /// </summary>
        public void Exec()
        {
            CandleWizardForm wizard = new CandleWizardForm("Publish as template");
            wizard.AddPage(new PublishAsTemplateWizardPage(wizard));
            wizard.SetUserData("ModelName", Path.GetFileNameWithoutExtension(_fileName));

            if (wizard.Start() == System.Windows.Forms.DialogResult.OK)
            {
                string remoteModelFile = wizard.GetUserData<string>("ModelName");
                string remoteStrategiesFile = wizard.GetUserData<string>("StrategiesName");
                string strategiesFile = StrategyManager.GetInstance(_model.Store).FileName;
                RepositoryManager.Instance.ModelsMetadata.PublishModelAsTemplate(_fileName, remoteModelFile, strategiesFile, remoteStrategiesFile);
            }
        }

        #endregion
    }
}
