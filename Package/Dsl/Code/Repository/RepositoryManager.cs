using System;
using System.Collections.Generic;
using System.IO;
using DSLFactory.Candle.SystemModel.Configuration;
using DSLFactory.Candle.SystemModel.Dependencies;
using DSLFactory.Candle.SystemModel.Repository.Providers;
using DSLFactory.Candle.SystemModel.Strategies;

namespace DSLFactory.Candle.SystemModel.Repository
{
    /// <summary>
    /// Gestionnaire du référentiel des modèles
    /// </summary>
    public class RepositoryManager
    {
        /// <summary>
        /// Cache pour les emplacements des assemblies des stratégies
        /// </summary>
        private static readonly Dictionary<string, string> s_assemblyLocationCache = new Dictionary<string, string>();

        private readonly List<IRepositoryProvider> _providers;
        private readonly IRepositoryProvider _wsRepository;
        private readonly IRepositoryProvider _localRepository;

        private IModelsMetadata _modelsMetadata = new ModelsRepositoryService();

        private static RepositoryManager s_instance;


        /// <summary>
        /// Initializes a new instance of the <see cref="RepositoryManager"/> class.
        /// </summary>
        private RepositoryManager()
            : this(CandleSettings.BaseDirectory, CandleSettings.RepositoryModelsLocalFolder, CandleSettings.RepositoryUrl, CandleSettings.GlobalServers)
        {
        }

        /// <summary>
        /// List 
        /// </summary>
        /// <returns></returns>
        public List<IRepositoryProvider> GetRemoteProviders()
        {
            List<IRepositoryProvider> list = new List<IRepositoryProvider>();
            foreach (IRepositoryProvider provider in _providers)
            {
                if (provider != _localRepository)
                    list.Add(provider);
            }
            return list;
        }

        /// <summary>
        /// Gets the local provider.
        /// </summary>
        /// <returns></returns>
        public IRepositoryProvider GetLocalProvider()
        {
            return _localRepository;
        }

        /// <summary>
        /// Gets the main remote provider.
        /// </summary>
        /// <returns></returns>
        public IRepositoryProvider GetMainRemoteProvider()
        {
            return _wsRepository;
        }

        /// <summary>
        /// Gets or sets the models metadata.
        /// </summary>
        /// <value>The models metadata.</value>
        public IModelsMetadata ModelsMetadata
        {
            [System.Diagnostics.DebuggerStepThrough]
            get { return _modelsMetadata; }
            [System.Diagnostics.DebuggerStepThrough]
            set { _modelsMetadata = value; }
        }

        /// <summary>
        /// Singleton
        /// </summary>
        public static RepositoryManager Instance
        {
            [global::System.Diagnostics.DebuggerStepThrough]
            get
            {
                if (s_instance == null)
                {
                    s_instance = new RepositoryManager();
                    ICandleNotifier notifier = ServiceLocator.Instance.GetService<ICandleNotifier>();
                    if (notifier != null)
                    {
                        notifier.OptionsChanged += ReloadOnChangeEvents;
                        notifier.SolutionOpened += ReloadOnChangeEvents;
                    }
                }
                return s_instance;
            }
        }

        /// <summary>
        /// Reloads the on change events.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        static void ReloadOnChangeEvents(object sender, EventArgs e)
        {
            ICandleNotifier notifier = ServiceLocator.Instance.GetService<ICandleNotifier>();
            if (notifier != null)
            {
                notifier.OptionsChanged -= ReloadOnChangeEvents;
                notifier.SolutionOpened -= ReloadOnChangeEvents;
            }

            s_instance = null;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RepositoryManager"/> class.
        /// </summary>
        /// <param name="localPath">The local path.</param>
        /// <param name="modelPath">The model path.</param>
        /// <param name="remoteUrl">The remote URL.</param>
        /// <param name="servers">The servers.</param>
        internal RepositoryManager(string localPath, string modelPath, string remoteUrl, IList<string> servers)
        {
            // Initialisation des providers
            _providers = new List<IRepositoryProvider>();

            // En premier, l'accés au disque local
            if (!String.IsNullOrEmpty(localPath))
            {
                _localRepository = new FileRepositoryProvider(localPath, modelPath);
                _providers.Add(_localRepository);
            }

            // Ensuite le repository central
            if (!String.IsNullOrEmpty(remoteUrl))
            {
                _wsRepository = GetRemoteRepository(remoteUrl);
                _providers.Add(_wsRepository);
            }

            if (servers != null)
            {
                foreach (string url in servers)
                {
                    if (!String.IsNullOrEmpty(url))
                        _providers.Add(GetRemoteRepository(url));
                }
            }
        }

        /// <summary>
        /// Récupére un fichier en privilégiant la version du repository central
        /// </summary>
        /// <param name="category"></param>
        /// <param name="path"></param>
        /// <param name="targetFileName"></param>
        /// <returns></returns>
        public bool GetFileFromRepository(RepositoryCategory category, string path, string targetFileName)
        {
            if (_wsRepository != null)
            {
                if (_wsRepository.GetFile(category, path, targetFileName) != RepositoryServerItemStatus.NotFound)
                    return true;
            }

            if (_localRepository != null)
            {
                if (_localRepository.GetFile(category, path, targetFileName) != RepositoryServerItemStatus.NotFound)
                    return true;
            }

            // Si toujours pas, on va lire dans les autres serveurs 
            foreach (IRepositoryProvider provider in _providers)
            {
                if (provider != _wsRepository && provider != _localRepository)
                {
                    if (provider.GetFile(category, path, targetFileName) != RepositoryServerItemStatus.NotFound)
                        return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Liste des modèles 
        /// </summary>
        public List<string> GetTemplateModelList()
        {
            return GetListForCategory(RepositoryCategory.Configuration, ModelConstants.FilterExtension);
        }

        /// <summary>
        /// Gets the strategies configuration list.
        /// </summary>
        /// <returns></returns>
        public List<string> GetStrategiesConfigurationList()
        {
            return GetListForCategory(RepositoryCategory.Configuration, "*" + StrategyManager.DefaultStrategiesFileNameExtension);
        }

        /// <summary>
        /// Gets the t4 templates list.
        /// </summary>
        /// <returns></returns>
        public List<string> GetT4TemplatesList()
        {
            return GetListForCategory(RepositoryCategory.T4Templates, "*.T4");
        }

        /// <summary>
        /// Gets the list for category.
        /// </summary>
        /// <param name="category">The category.</param>
        /// <param name="filter">The filter.</param>
        /// <returns></returns>
        private List<string> GetListForCategory(RepositoryCategory category, string filter)
        {
            List<string> templates = new List<string>();
            IRepositoryProvider provider = _wsRepository;
            if (provider == null)
            {
                provider = _localRepository;
                if (provider == null)
                    return templates;
            }

            List<RepositoryFileInfo> files = null;
            try
            {
                files = provider.EnumerateCategory(category, filter, true);
            }
            catch
            {
                if (provider == _wsRepository && _localRepository != null)
                {
                    files = _localRepository.EnumerateCategory(category, filter, true);
                }
            }

            if (files != null)
            {
                foreach (RepositoryFileInfo rfi in files)
                {
                    templates.Add(rfi.FileName);
                }
            }

            return templates;
        }

        /// <summary>
        /// Création d'un nouveau domaine
        /// </summary>
        /// <param name="path"></param>
        public void CreateDomainPath(string path)
        {
            DomainManager.Instance.CreateDomainPath(path);
            IRepositoryProvider provider = _wsRepository;
            if (provider != null)
            {
                provider.CreateDomainPath(path);
            }
        }

        /// <summary>
        /// Création d'un nouveau domaine
        /// </summary>
        /// <param name="path"></param>
        public void RemoveDomainPath(string path)
        {
            DomainManager.Instance.RemoveDomainPath(path);
            IRepositoryProvider provider = _wsRepository;
            if (provider != null)
            {
                provider.RemoveDomainPath(path);
            }
        }



        /// <summary>
        /// Déduit à partir d'un chemin physique, le chemin relatif et la catégorie du fichier.
        /// </summary>
        /// <param name="physicalPath">Chemin physique dans le référentiel local</param>
        /// <param name="category">Catégorie du fichier</param>
        public static string MakeRelative(string physicalPath, out RepositoryCategory category)
        {
            if (Utils.StringStartsWith(physicalPath, CandleSettings.RepositoryModelsLocalFolder))
            {
                category = RepositoryCategory.Models;
                return physicalPath.Substring(CandleSettings.RepositoryModelsLocalFolder.Length + 1);
            }

            string path = GetFolderPath(RepositoryCategory.Strategies);
            if (physicalPath.StartsWith(path, StringComparison.CurrentCultureIgnoreCase))
            {
                category = RepositoryCategory.Strategies;
                return physicalPath.Substring(path.Length + 1);
            }
            path = GetFolderPath(RepositoryCategory.T4Templates);
            if (physicalPath.StartsWith(path, StringComparison.CurrentCultureIgnoreCase))
            {
                category = RepositoryCategory.T4Templates;
                return physicalPath.Substring(path.Length + 1);
            }

            path = GetFolderPath(RepositoryCategory.Configuration);
            if (physicalPath.StartsWith(path, StringComparison.CurrentCultureIgnoreCase))
            {
                category = RepositoryCategory.Configuration;
                return physicalPath.Substring(path.Length + 1);
            }
            throw new Exception("Unable to find the category for this file.");
        }


        /// <summary>
        /// Création du chemin physique à partir du chemin relatif
        /// </summary>
        /// <param name="category">Catégorie ou est stocké le fichier</param>
        /// <param name="relativePath">Chemin relatif</param>
        /// <returns>Chemin physique en local</returns>
        public static string ResolvePath(RepositoryCategory category, string relativePath)
        {
            switch (category)
            {
                case RepositoryCategory.Models:
                    return Path.Combine(CandleSettings.RepositoryModelsLocalFolder, relativePath);

                default:
                    return Path.Combine(GetFolderPath(category), relativePath);
            }
        }

        /// <summary>
        /// Gets the folder path.
        /// </summary>
        /// <param name="category">The category.</param>
        /// <returns></returns>
        public static string GetFolderPath(RepositoryCategory category)
        {
            return CandleSettings.GetFolderPath(category);
        }

        /// <summary>
        /// Copie les executables et les artefacts dans le répertoire d'exécution
        /// </summary>
        /// <param name="model">The model.</param>
        public static void CopyToRuntimeFolder(CandleModel model)
        {
            if (model.Component == null)
                return;

            ILogger logger = ServiceLocator.Instance.GetService<ILogger>();
            string targetFolder = string.Empty;

            try
            {
                // Recherche du projet de démarrage pour savoir dans quel répertoire copier les artifacts
                object[] files = (object[])ServiceLocator.Instance.ShellHelper.Solution.SolutionBuild.StartupProjects;
                if (files == null || files.Length == 0)
                    return; // Pas trouvé

                string startupProjectName = (string)files[0];
                startupProjectName = Path.GetFileNameWithoutExtension(startupProjectName);
                EnvDTE.Project startupProject = ServiceLocator.Instance.ShellHelper.FindProjectByName(startupProjectName);
                if (startupProject == null)
                {
                    // Recherche diffèrente pour un projet web
                    startupProjectName = Path.GetFileName(startupProjectName);
                    startupProject = ServiceLocator.Instance.ShellHelper.FindProjectByName(startupProjectName);
                    if (startupProject == null)
                    {
                        if (logger != null)
                            logger.Write("Copy binary to runtime folder error", "Unable to find the startup project " + startupProjectName, LogType.Warning);
                        return;
                    }
                }

                // Vérification des dépendances
                ReferencesHelper.CheckReferences(false, null, new ConfigurationMode(), ReferenceScope.Runtime, model);

                ConfigurationMode mode = new ConfigurationMode(startupProject);

                // Répertoire destination
                targetFolder = Path.Combine(Path.GetDirectoryName(startupProject.FullName), mode.BinPath);
                
                CopyInFolder(targetFolder, model, mode);
            }
            catch (Exception ex)
            {
                if (logger != null)
                    logger.WriteError("Copy to runtime folder", String.Format("erreur when copying runtime artifacts to {0}", targetFolder), ex);
            }
        }

        /// <summary>
        /// Copie des fichiers référencés dans un répertoire
        /// </summary>
        /// <param name="targetFolder">Répertoire destination</param>
        /// <param name="model">The model.</param>
        /// <param name="mode">The mode.</param>
        internal static void CopyInFolder(string targetFolder, CandleModel model, ConfigurationMode mode)
        {
            if (targetFolder == null) throw new ArgumentNullException("targetFolder");
            if (model == null) throw new ArgumentNullException("model");
            if (mode == null) throw new ArgumentNullException("mode");

            if( !Directory.Exists(targetFolder))
                return;

            // Recherche des artifacts à copier 
            ReferencesCollection references = DSLFactory.Candle.SystemModel.Dependencies.ReferencesHelper.GetReferences(mode, ReferenceScope.Runtime, ServiceLocator.Instance.ShellHelper.SolutionFolder, model);

            ILogger logger = ServiceLocator.Instance.GetService<ILogger>();
            foreach (string fileName in references)
            {
                string target = Path.Combine(targetFolder, Path.GetFileName(fileName));
                // string source = Path.Combine( sourceFolder, fileName );

                try
                {
                    if (!Utils.StringCompareEquals(target, fileName) && File.GetLastWriteTime(fileName) > File.GetLastWriteTime(target))
                    {
                        Utils.CopyFile(fileName, target);
                        Utils.UnsetReadOnly(target);
                        if( logger!=null)
                            logger.Write("Copy local", String.Format("Runtime references : Copy file {0} to {1}", fileName, target), LogType.Info);
                        // Il y a peut-être un pdb
                        string pdbFileName = Path.ChangeExtension(fileName, ".pdb");
                        string pdbTargetName = Path.ChangeExtension(target, ".pdb");
                        Utils.CopyFile(pdbFileName, pdbTargetName);
                        Utils.UnsetReadOnly(pdbTargetName);
                        // Et un xml
                        string xmlFileName = Path.ChangeExtension(fileName, ".xml");
                        string xmlTargetName = Path.ChangeExtension(target, ".xml");
                        Utils.CopyFile(xmlFileName, xmlTargetName);
                        Utils.UnsetReadOnly(xmlTargetName);
                    }
                }
                catch (Exception ex)
                {
                    if( logger != null )
                        logger.WriteError("Copy local", String.Format("Runtime references : Error when copying file {0} to {1}", fileName, target), ex);
                }
            }
        }

        /// <summary>
        /// Permet de savoir si un chemin fait partie du repository local
        /// </summary>
        /// <param name="fileName">Chemin physique</param>
        /// <returns></returns>
        public static bool IsFileInRepository(string fileName)
        {
            return fileName.StartsWith(CandleSettings.BaseDirectory, StringComparison.CurrentCultureIgnoreCase);
        }

        /// <summary>
        /// Test si l'adresse du repository est valide
        /// </summary>
        /// <param name="url"></param>
        internal static void CheckRepository(string url)
        {
            RepositoryManager rm = new RepositoryManager(null, null, url, null);
            if (rm.GetVersion() == null)
                throw new Exception("Repository not accessible.");
        }

        /// <summary>
        /// Gets the version.
        /// </summary>
        /// <returns></returns>
        private Version GetVersion()
        {
            if (_wsRepository == null)
                return null;
            return _wsRepository.GetVersion();
        }

        /// <summary>
        /// Lecture de toutes les strategies presentes sur le serveur
        /// </summary>
        /// <returns></returns>
        public StrategyManifest[] GetExternalManifests()
        {
            ManifestCollection manifests = new ManifestCollection();
            if (_wsRepository != null)
            {
                manifests.AddRange(_wsRepository.GetStrategyManifests());
            }

            return manifests.ToArray();
        }

        /// <summary>
        /// Gets the external manifests.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <returns></returns>
        public static StrategyManifest[] GetExternalManifests(string url)
        {
            ManifestCollection manifests = new ManifestCollection();
            WebServiceRepositoryProvider ws = new WebServiceRepositoryProvider(url);
            manifests.AddRange(ws.GetStrategyManifests());
            return manifests.ToArray();
        }

        /// <summary>
        /// Resolves the assembly location.
        /// </summary>
        /// <param name="assemblyName">Name of the assembly.</param>
        /// <returns></returns>
        public static string ResolveAssemblyLocation(string assemblyName)
        {
            int pos = assemblyName.IndexOf(',');
            if (pos > 0)
                assemblyName = assemblyName.Substring(0, pos).Trim();
            string extension = assemblyName.EndsWith(".dll", StringComparison.CurrentCultureIgnoreCase) ? String.Empty : ".dll";
            assemblyName = String.Format("{0}{1}", assemblyName, extension);
            string location;
            // Recherche dans le cache
            if (s_assemblyLocationCache.TryGetValue(assemblyName, out location))
                return location;

            location = RepositoryManager.ResolvePath(RepositoryCategory.Strategies, Path.Combine(assemblyName, assemblyName));
            if (!File.Exists(location))
            {
                List<String> fileNames = Utils.SearchFile(RepositoryManager.ResolvePath(RepositoryCategory.Strategies, String.Empty), assemblyName);
                if (fileNames.Count > 0)
                    location = fileNames[0];
            }
            s_assemblyLocationCache.Add(assemblyName, location);
            return location;
        }

        //public void RemoveLocalModel(string relativeModelFileName)
        //{
        //    string fileName = RepositoryManager.ResolvePath(RepositoryCategory.Models, relativeModelFileName);
        //    string folder = Path.GetDirectoryName(fileName);
        //    Utils.RemoveDirectory(folder);
        //}

        /// <summary>
        /// Gets the remote repository.
        /// </summary>
        /// <param name="serverUrl">The server URL.</param>
        /// <returns></returns>
        public IRepositoryProvider GetRemoteRepository(string serverUrl)
        {
            if (serverUrl != null)
                return new DSLFactory.Candle.SystemModel.Repository.Providers.WebServiceRepositoryProvider(serverUrl);
            return _wsRepository;
        }

        /// <summary>
        /// Gets the remote provider.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <returns></returns>
        internal IRepositoryProvider GetRemoteProvider(string url)
        {
            if (String.IsNullOrEmpty(url))
                return _wsRepository;

            foreach (IRepositoryProvider provider in _providers)
            {
                if (provider.Name == url)
                    return provider;
            }
            return new WebServiceRepositoryProvider(url);
        }
    }
}
