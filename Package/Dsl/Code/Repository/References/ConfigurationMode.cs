namespace DSLFactory.Candle.SystemModel
{
    /// <summary>
    /// Etat de la configuration Visual studio (debug, release...)
    /// </summary>
    [System.CLSCompliant(true)]
    public class ConfigurationMode
    {
        private string _currentMode;
        private readonly string _binPath=string.Empty;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigurationMode"/> class.
        /// </summary>
        /// <param name="solutionConfigurationName">The visual studio solution configuration name or * for current</param>
        public ConfigurationMode( string solutionConfigurationName )
        {
            _currentMode = solutionConfigurationName;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigurationMode"/> class.
        /// </summary>
        /// <param name="solutionConfigurationName">The visual studio solution configuration name or * for current</param>
        /// <param name="binPath">The bin path.</param>
        public ConfigurationMode(string solutionConfigurationName, string binPath)
        {
            this._binPath = binPath;
            this._currentMode = solutionConfigurationName;
        }

        /// <summary>
        /// Etat de la configuration du projet courant
        /// </summary>
        /// <param name="project"></param>
        public ConfigurationMode(EnvDTE.Project project)
        {
            _currentMode = "*";
            if (project != null)
            {
                if (project.Object is VsWebSite.VSWebSite)
                    _binPath = "bin";
                else
                    _binPath = project.ConfigurationManager.ActiveConfiguration.Properties.Item("OutputPath").Value as string;
                _currentMode = project.ConfigurationManager.ActiveConfiguration.ConfigurationName;
            }
            else
                SetCurrentModeFromVisualStudioContext();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigurationMode"/> class with the current solution configuration
        /// </summary>
        public ConfigurationMode()
        {
            _currentMode = "*";
            SetCurrentModeFromVisualStudioContext();
        }


        /// <summary>
        /// Sets the current mode from visual studio context.
        /// </summary>
        private void SetCurrentModeFromVisualStudioContext()
        {
            IShellHelper shell = ServiceLocator.Instance.ShellHelper;
            if (shell!=null && shell.Solution != null && shell.Solution.SolutionBuild != null && shell.Solution.SolutionBuild.ActiveConfiguration != null)
                _currentMode = shell.Solution.SolutionBuild.ActiveConfiguration.Name;
        }

        /// <summary>
        /// Vérifie si le mode est actif
        /// </summary>
        /// <param name="configurationMode">Mode à tester (*=tous)</param>
        /// <returns></returns>
        public bool CheckConfigurationMode( string configurationMode )
        {
            return configurationMode == "*" || _currentMode == "*" || Utils.StringCompareEquals(configurationMode, _currentMode);
        }

        /// <summary>
        /// Gets the bin path.
        /// </summary>
        /// <value>The bin path.</value>
        public string BinPath
        {
            get { return _binPath; }
        }

        /// <summary>
        /// Gets the current mode.
        /// </summary>
        /// <value>The current mode.</value>
        public string CurrentMode
        {
            get { return _currentMode; }
        }
    }
}