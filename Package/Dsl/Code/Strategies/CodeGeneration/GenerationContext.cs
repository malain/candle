using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using DSLFactory.Candle.SystemModel.Strategies;
using EnvDTE;
using VsWebSite;

namespace DSLFactory.Candle.SystemModel
{
    /// <summary>
    /// 
    /// </summary>
    public class GenerationContext
    {
        private string _codeFolder;
        private StrategyBase _currentStrategy;
        private ConfigurationMode _mode;
        private CandleModel _model;
        private string _modelFileName;
        private GenerationPass _pass;
        private Project _project;

        private string _projectFolder;
        private string _relativeGeneratedFileName;
        private Guid _selectedModel;
        private List<StrategyBase> _selectedStrategies;
        private string _template;

        /// <summary>
        /// Initializes a new instance of the <see cref="GenerationContext"/> class.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="modelFileName">Name of the model file.</param>
        /// <param name="selectedModel">The selected model.</param>
        public GenerationContext(CandleModel model, string modelFileName, Guid selectedModel)
        {
            _model = model;
            _modelFileName = modelFileName;
            _selectedModel = selectedModel;
            _mode = new ConfigurationMode();
        }

        /// <summary>
        /// Gets the physical project code folder.
        /// </summary>
        /// <value>The physical project code folder.</value>
        public string PhysicalProjectCodeFolder
        {
            get { return Path.Combine(ProjectFolder, RelativeProjectCodeFolder); }
        }

        /// <summary>
        /// Gets or sets the name of the relative generated file.
        /// </summary>
        /// <value>The name of the relative generated file.</value>
        public string RelativeGeneratedFileName
        {
            [DebuggerStepThrough]
            get { return _relativeGeneratedFileName; }
            [DebuggerStepThrough]
            set { _relativeGeneratedFileName = value; }
        }

        /// <summary>
        /// Gets or sets the relative project code folder.
        /// </summary>
        /// <value>The relative project code folder.</value>
        public string RelativeProjectCodeFolder
        {
            [DebuggerStepThrough]
            get { return _codeFolder; }
            [DebuggerStepThrough]
            set { _codeFolder = value; }
        }

        /// <summary>
        /// Gets or sets the current strategy.
        /// </summary>
        /// <value>The current strategy.</value>
        public StrategyBase CurrentStrategy
        {
            get { return _currentStrategy; }
            internal set { _currentStrategy = value; }
        }

        /// <summary>
        /// Gets or sets the project folder.
        /// </summary>
        /// <value>The project folder.</value>
        public string ProjectFolder
        {
            [DebuggerStepThrough]
            get { return _projectFolder; }
            [DebuggerStepThrough]
            set { _projectFolder = value; }
        }

        /// <summary>
        /// Gets or sets the template.
        /// </summary>
        /// <value>The template.</value>
        public string Template
        {
            [DebuggerStepThrough]
            get { return _template; }
            [DebuggerStepThrough]
            set { _template = value; }
        }

        /// <summary>
        /// Gets or sets the model.
        /// </summary>
        /// <value>The model.</value>
        public CandleModel Model
        {
            [DebuggerStepThrough]
            get { return _model; }
            [DebuggerStepThrough]
            set { _model = value; }
        }

        /// <summary>
        /// Gets or sets the generation pass.
        /// </summary>
        /// <value>The generation pass.</value>
        public GenerationPass GenerationPass
        {
            [DebuggerStepThrough]
            get { return _pass; }
            [DebuggerStepThrough]
            set { _pass = value; }
        }

        /// <summary>
        /// Gets or sets the selected strategies.
        /// </summary>
        /// <value>The selected strategies.</value>
        internal List<StrategyBase> SelectedStrategies
        {
            [DebuggerStepThrough]
            get { return _selectedStrategies; }
            [DebuggerStepThrough]
            set { _selectedStrategies = value; }
        }

        /// <summary>
        /// Gets or sets the name of the model file.
        /// </summary>
        /// <value>The name of the model file.</value>
        public string ModelFileName
        {
            [DebuggerStepThrough]
            get { return _modelFileName; }
            [DebuggerStepThrough]
            set { _modelFileName = value; }
        }

        /// <summary>
        /// Gets or sets the mode.
        /// </summary>
        /// <value>The mode.</value>
        public ConfigurationMode Mode
        {
            [DebuggerStepThrough]
            get { return _mode; }
            [DebuggerStepThrough]
            set { _mode = value; }
        }

        /// <summary>
        /// Gets or sets the selected element.
        /// </summary>
        /// <value>The selected element.</value>
        public Guid SelectedElement
        {
            [DebuggerStepThrough]
            get { return _selectedModel; }
            //     [global::System.Diagnostics.DebuggerStepThrough]
            internal
                set { _selectedModel = value; }
        }


        /// <summary>
        /// Gets or sets the project.
        /// </summary>
        /// <value>The project.</value>
        public Project Project
        {
            [DebuggerStepThrough]
            get { return _project; }
            [DebuggerStepThrough]
            set
            {
                _project = value;
                _projectFolder = _codeFolder = String.Empty;
                if (_project != null)
                {
                    ProjectFolder = Path.GetDirectoryName(_project.FileName);
                    RelativeProjectCodeFolder = _project.Object is VSWebSite ? "App_Code" : String.Empty;
                }
            }
        }

        /// <summary>
        /// Vérification si une stratégie n'a pas les droits de génération exclusif sur cet élément
        /// </summary>
        /// <param name="element">The element.</param>
        /// <param name="strategy">The strategy.</param>
        /// <param name="generatedFileName">Name of the generated file.</param>
        /// <returns>
        /// 	<c>true</c> if [is generation locked] [the specified element]; otherwise, <c>false</c>.
        /// </returns>
        public bool IsGenerationLocked(ICustomizableElement element, StrategyBase strategy, string generatedFileName)
        {
            ILogger logger = ServiceLocator.Instance.GetService<ILogger>();
            if (logger != null)
            {
                logger.BeginStep("Check generation lock", LogType.Debug);
                logger.Write("Check generation lock",
                             String.Concat("Current strategy ", strategy.StrategyId, " for outputfile ",
                                           generatedFileName ?? "(no file specified)"), LogType.Debug);
            }

            foreach (StrategyBase otherStrategy in element.GetStrategies(false))
            {
                if (strategy.StrategyId != otherStrategy.StrategyId &&
                    otherStrategy.IsModelGenerationExclusive(this, element, generatedFileName))
                {
                    if (logger != null)
                    {
                        logger.Write("Check generation lock",
                                     String.Format("Element {0} generation locked by strategy {1}", element.Name,
                                                   otherStrategy.StrategyId), LogType.Debug);
                        logger.EndStep();
                    }
                    return true;
                }
            }

            if (logger != null)
                logger.EndStep();
            return false;
        }

        /// <summary>
        /// Determines whether this instance can generate the specified model GUID.
        /// </summary>
        /// <param name="modelGuid">The model GUID.</param>
        /// <returns>
        /// 	<c>true</c> if this instance can generate the specified model GUID; otherwise, <c>false</c>.
        /// </returns>
        public bool CanGenerate(Guid modelGuid)
        {
            return _selectedModel == Guid.Empty || IsModelSelected(modelGuid);
        }

        /// <summary>
        /// Determines whether [is model selected] [the specified model GUID].
        /// </summary>
        /// <param name="modelGuid">The model GUID.</param>
        /// <returns>
        /// 	<c>true</c> if [is model selected] [the specified model GUID]; otherwise, <c>false</c>.
        /// </returns>
        public bool IsModelSelected(Guid modelGuid)
        {
            return _selectedModel != Guid.Empty && _selectedModel == modelGuid;
        }
    }
}