using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using DSLFactory.Candle.SystemModel.Repository;
using Microsoft.VisualStudio.Modeling;

namespace DSLFactory.Candle.SystemModel.Strategies
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    [XmlRoot("configuration")]
    public class StrategyManager : IStrategyManager
    {
        private List<StrategyPackage> _packages;
        private List<StrategyTypeReference> _strategyTypes;
        private static Guid s_currentModelId = Guid.Empty;
        private static IStrategyManager s_currentStrategyManager;

        /// <summary>Watcher pour recharger le fichier en cas de modification</summary>
        private static FileSystemWatcher s_fileSystemWatcher;

        private static string s_folder;

        #region Constantes
        /// <summary>Solution Folder pour stocker le fichier des strat�gies</summary>
        public const string ConfigFolder = "Solution Items";// "Config";

        /// <summary>Nom du fichier des strat�gies</summary>
        public const string DefaultStrategiesFileName = "strategies" + DefaultStrategiesFileNameExtension;

        /// <summary>Extension du fichier</summary>
        public const string DefaultStrategiesFileNameExtension = ".strategies";

        /// <summary>Permet d'�viter d'appeler les assistants quand on fait du reverse par exemple</summary>
        public const string IgnoreStrategyWizards = "{E2A52AC3-F916-4089-81C5-270D2C283BE3}";

        #endregion

        #region Empty strategy file content
        private const string EmptyStrategyFileContent = @"<?xml version=""1.0"" encoding=""utf-8""?>
<configuration xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"">
  <strategyTypes />
  <strategies xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" />
</configuration>";


        #endregion

        private string _fileName;
        private InternalPackage _internalPackage;
        private LanguageConfiguration _languageConfiguration;
        private INamingStrategy _namingStrategy;
        private XmlNode _namingStrategyNode;
        private StrategyCollection _strategies = new StrategyCollection();
        private XmlNode _strategiesNode;

        /// <summary>
        /// Permet de stoker la s�rialisation de la strat�gy de nommage
        /// La s�rialisation se fait � part car il faut d'abord connaitre
        /// les types r�f�renc�s pour pouvoir cr�er le 'serialiser'
        /// </summary>
        [XmlAnyElement("NamingStrategy")]
        public XmlNode NamingStrategyNode
        {
            get { return _namingStrategyNode; }
            set { _namingStrategyNode = value; }
        }

        /// <summary>
        /// Liste des packages
        /// </summary>
        public List<StrategyPackage> Packages
        {
            get { if (_packages == null) _packages = new List<StrategyPackage>(); return _packages; }
            set { _packages = value; }
        }

        /// <summary>
        /// Permet de stoker la s�rialisation des strat�gies.
        /// La s�rialisation se fait � part car il faut d'abord connaitre
        /// les types r�f�renc�s pour pouvoir cr�er le 'serialiser'
        /// </summary>
        [XmlAnyElement("strategies")]
        public XmlNode StrategiesNode
        {
            get { return _strategiesNode; }
            set { _strategiesNode = value; }
        }

        /// <summary>
        /// Liste des types r�f�renc�s (utiles pour la s�rialisation)
        /// </summary>
        [XmlArray("strategyTypes")]
        [XmlArrayItem("add")]
        public List<StrategyTypeReference> StrategyTypes
        {
            get { return _strategyTypes; }
            set { _strategyTypes = value; }
        }

        #region Chargement/ Dechargement
        /// <summary>
        /// D�chargement de toutes les strat�gies de toutes les instances
        /// </summary>
        public static void ClearAll()
        {
            ICandleNotifier notifier = ServiceLocator.Instance.GetService<ICandleNotifier>();
            if (notifier != null)
            {
                notifier.OptionsChanged -= new EventHandler(ReloadOnChangeEvents);
                notifier.SolutionOpened -= new EventHandler(ReloadOnChangeEvents);
                notifier.SolutionClosed -= new EventHandler(ReloadOnChangeEvents);
            }

            if (s_fileSystemWatcher != null)
            {
                s_fileSystemWatcher.EnableRaisingEvents = false;
                s_fileSystemWatcher.Changed -= new FileSystemEventHandler(fileSystemWatcher_Changed);
            }

            if( s_currentStrategyManager != null )
            {
                try { s_currentStrategyManager.UnloadStrategies(); }
                catch { }
            }

            s_currentModelId = Guid.Empty;
            s_currentStrategyManager = null;
        }

        /// <summary>
        /// R�cup�re les strat�gies du mod�le courant.
        /// </summary>
        /// <param name="store"></param>
        /// <remarks>
        /// Si un autre mod�le est ouvert dans la solution, on retourne un manager ne contenant aucune strategie.
        /// </remarks>
        /// <returns></returns>
        public static IStrategyManager GetInstance(Store store)
        {
            // Mod�le correspondant au store
            CandleModel model = CandleModel.GetInstance(store);
            if (model == null) // On a pas trouv� le mod�le, on retourne un strategyManager vide
                return new StrategyManager();

            // Si on a d�ja charg� les strat�gies, on les retourne
            if (s_currentModelId != Guid.Empty && s_currentModelId == model.Id)
                return s_currentStrategyManager;

            // Est ce que c'est le mod�le courant
            CandleModel currentModel = CandleModel.GetModelFromCurrentSolution();
            if (currentModel == null || currentModel.Id != model.Id)
                return new StrategyManager();

            // Sinon on va cr�er les strat�gies
            SuspendWatcher(false);

            // Flag indiquant si on peut persister les strat�gies ou seulement les initialiser.
            // On ne peut pas persister si on est dans le cas d'une initialisation du mod�le puisqu'on ne connait pas
            // encore le nom du mod�le
            bool canPersist = true;
            string strategyFileName = null;

            try
            {
                // Cr�ation d'un nouveau fichier de strategies ayant le m�me nom que le mod�le mais avec
                // l'extension .strategies
                string modelFileName = model.FileName;

                // Dans le cas d'une cr�ation d'un mod�le, on ne peut pas r�cup�rer le filepath du mod�le car
                // il n'y a pas encore de vue active. Dans ce cas, on essaye de le d�duire.
                if (modelFileName == null)
                {
                    string name = model.Name;

                    // On est dans le cas d'une initialisation de mod�le, on ne va cr�er le fichier des strat�gies tout de suite
                    // on attend que le mod�le soit initialis�
                    if (name.Contains("?"))
                    {
                        canPersist = false;
                        strategyFileName = Path.GetTempFileName();  // Fichier temporaire pour r�cup�rer les strat�gies pr�-initialis�es
                        Utils.DeleteFile(strategyFileName);         // Oblig� car GetTempFileName cr�e le fichier
                    }
                    else
                    {
                        IShellHelper shell = ServiceLocator.Instance.GetService<IShellHelper>();
                        if (shell != null)
                        {
                            string tmp = Path.GetFileNameWithoutExtension(shell.GetSolutionAssociatedModelName());
                            if (!String.IsNullOrEmpty(tmp))
                                name = tmp;
                            else
                            {
                                tmp = shell.Solution.FullName;
                                if (!String.IsNullOrEmpty(tmp))
                                    name = Path.GetFileNameWithoutExtension(tmp);
                            }
                        }

                        strategyFileName = String.Concat(Path.Combine(ServiceLocator.Instance.ShellHelper.SolutionFolder, name), DefaultStrategiesFileNameExtension);
                    }
                }
                else
                {
                    strategyFileName = Path.ChangeExtension(modelFileName, DefaultStrategiesFileNameExtension);
                }

                // Initialisation du fichier de strat�gies � partir d'un mod�le
                if (!String.IsNullOrEmpty(model.StrategyTemplate))
                    EnsureStrategiesFileExists(strategyFileName, model.StrategyTemplate, canPersist);

                StrategyManager manager = null;
                if (File.Exists(strategyFileName))
                {
                    manager = Load(store, strategyFileName);
                }
                else
                {
                    manager = new StrategyManager();
                }

                if (canPersist)
                {
                    manager.FileName = strategyFileName;
                    // Cache pour �viter de le relire
                    s_currentModelId = model.Id;
                    s_currentStrategyManager = manager;
                }

                return manager;
            }
            finally
            {
                SuspendWatcher(true);

                // Suppression du fichier temporaire
                if (!canPersist)
                    Utils.DeleteFile(strategyFileName);
            }
        }

        /// <summary>
        /// Unloads the strategies.
        /// </summary>
        public void UnloadStrategies()
        {
            foreach (StrategyBase strategy in _strategies)
            {
                strategy.OnUnloading(strategy, new EventArgs());
            }
            _strategies.Clear();
        }
        #endregion

        #region Constructeurs
        /// <summary>
        /// Initializes a new instance of the <see cref="StrategyManager"/> class.
        /// </summary>
        public StrategyManager()
        {
            ICandleNotifier notifier = ServiceLocator.Instance.GetService<ICandleNotifier>();
            if (notifier != null)
            {
                notifier.OptionsChanged += new EventHandler(ReloadOnChangeEvents);
                notifier.SolutionOpened += new EventHandler(ReloadOnChangeEvents);
                notifier.SolutionClosed += new EventHandler(ReloadOnChangeEvents);
            }
        }
        #endregion

        #region Gestion des strategies
        /// <summary>
        /// R�cup�re la liste des strat�gies applicables et actives sur un mod�le
        /// </summary>
        /// <param name="strategiesOwner">The strategies owner.</param>
        /// <param name="element">El�ment concern� ou null pour tous</param>
        /// <returns></returns>
        internal static List<StrategyBase> GetStrategies(CandleElement strategiesOwner, ICustomizableElement element)
        {
            ILogger logger = ServiceLocator.Instance.GetService<ILogger>();

            List<StrategyBase> strategies = new List<StrategyBase>();
            if (strategiesOwner == null)
                return strategies;

            if( element != null && logger!=null)
                logger.BeginStep(String.Concat("Get enabled strategies for element ", element.Name , " (id=", element.Id, ")"), LogType.Debug);            

            foreach (StrategyBase strategy in StrategyManager.GetInstance(strategiesOwner.Store).GetStrategies(strategiesOwner, true))
            {
                if (strategy.IsEnabled)
                {
                    strategies.Add(strategy);
                }
                else
                    if (element != null && logger!=null)
                        logger.Write("GetStrategies", String.Concat("Strategy ", strategy.StrategyId, " ignored because it is disabled"), LogType.Debug);
            }
            if (element != null && logger!=null)
                logger.EndStep();

            return strategies;
        }

        /// <summary>
        /// Gets the name of the strategy owner.
        /// </summary>
        /// <param name="sp">The sp.</param>
        /// <returns></returns>
        internal static string GetStrategyOwnerName(CandleElement sp)
        {
            return sp == null ? "globals" : String.Format("{0}-{1:D}", sp.GetType().Name, sp.Id);
        }

        /// <summary>
        /// Deletes the strategy.
        /// </summary>
        /// <param name="current">The current.</param>
        private void DeleteStrategy(StrategyBase current)
        {
            current.OnRemoving(this, new EventArgs());
            // TODO suppression des custom properties
        }

        /// <summary>
        /// Gets the strategies.
        /// </summary>
        /// <param name="ownerName">Name of the owner.</param>
        /// <returns></returns>
        public List<StrategyBase> GetStrategies(string ownerName)
        {
            return _strategies.FindAll(
                delegate(StrategyBase current) { return current.Owner == ownerName; });
        }

        /// <summary>
        /// Cr�ation de l'instance de la strategie contenu dans le manifest
        /// </summary>
        /// <param name="owner"></param>
        /// <param name="manifest">Manifest contenant la strat�gie</param>
        /// <returns></returns>
        public StrategyBase AddStrategy(CandleElement owner, StrategyManifest manifest)
        {
            InternalPackage package = GetPackage(manifest.PackageName);
            if (package == null)
                return null;

            StrategyBase strategy = package.CreateStrategyInstance(manifest.StrategyTypeName, manifest.StrategyConfiguration);
            if (strategy != null)
            {
                strategy.Owner = GetStrategyOwnerName(owner);

                // V�rification des doublons
                foreach (StrategyBase other in _strategies)
                {
                    if (other.Owner == strategy.Owner && other.StrategyId == strategy.StrategyId)
                        return other;
                }
                try
                {
                    strategy.OnLoading(this, new EventArgs());
                    _strategies.Add(strategy);
                }
                catch (Exception ex)
                {
                    IIDEHelper ide = ServiceLocator.Instance.GetService<IIDEHelper>();
                    if (ide != null)
                    {
                        ide.ShowMessageBox("Error when loading strategy " + strategy.DisplayName + " : " + ex.Message, "Error", System.Windows.Forms.MessageBoxButtons.OK);
                        ide.LogError(false, "Error when loading strategy " + strategy.DisplayName + " : " + ex.Message, 0, 0, "StrategyManager");
                    }
                }
            }

            return strategy;
        }

        /// <summary>
        /// Renvoi le package associ� au manifest en s'assurant qu'il soit � jour.
        /// </summary>
        /// <param name="packageName">Nom du package ou vide si strategyInterne</param>
        /// <returns></returns>
        public InternalPackage GetPackage(string packageName)
        {
            if (String.IsNullOrEmpty(packageName))
            {
                if (_internalPackage == null)
                    _internalPackage = new InternalPackage();
                return _internalPackage;
            }

            StrategyPackage package = Packages.Find(delegate(StrategyPackage p) { return Utils.StringCompareEquals(p.Name, packageName); });
            if (package == null)
            {
                package = new StrategyPackage(packageName);
                Packages.Add(package);
            }
            if (package.Synchronize())
                return package;
            return null;
        }

        /// <summary>
        /// Liste des strat�gies disponibles
        /// </summary>
        /// <param name="sp">null si on est au niveau global</param>
        /// <param name="includeGlobalStrategies">if set to <c>true</c> [include global strategies].</param>
        /// <returns></returns>
        public List<StrategyBase> GetStrategies(CandleElement sp, bool includeGlobalStrategies)
        {
            ILogger logger = ServiceLocator.Instance.GetService<ILogger>();
            string n = GetStrategyOwnerName(sp);
            if (logger != null)
                logger.Write("GetStrategies", String.Concat("Get all strategies for ", n), LogType.Debug);
            List<StrategyBase> strategies = GetStrategies(n);

            if (sp != null && includeGlobalStrategies)
            {
                if (logger != null)
                    logger.Write("GetStrategies", "Merging with globals strategies", LogType.Debug);
                foreach (StrategyBase strategy in GetStrategies(GetStrategyOwnerName(null)))
                {
                    // Si la strat�gie existe d�j� dans la couche, on ne la rajoute pas.
                    if (!strategies.Exists(delegate(StrategyBase currentStrategy) { return currentStrategy.StrategyId == strategy.StrategyId; }))
                        strategies.Add(strategy);
                    else if (logger != null)
                        logger.Write("GetStrategies", String.Concat("Global strategy ", strategy.StrategyId, " ignored because this strategy is already defined in ", n), LogType.Debug);
                }
            }
            return strategies;
        }

        /// <summary>
        /// Suppresion d'une strat�gie
        /// </summary>
        /// <param name="owner"></param>
        /// <param name="strategy"></param>
        public void RemoveStrategy(CandleElement owner, StrategyBase strategy)
        {
            Debug.Assert(strategy != null);
            string n = GetStrategyOwnerName(owner);

            _strategies.RemoveAll(
                delegate(StrategyBase current) 
                {
                    if (current.Owner == n && current.StrategyId == strategy.StrategyId)
                    {
                        DeleteStrategy(current);
                        return true;
                    }
                    return false;
                });
        }
        #endregion

        #region Initialisation
        /// <summary>
        /// V�rification si le fichier est pr�sent sinon l'initialise � partir du template
        /// </summary>
        /// <param name="strategiesFilePath">Chemin absolu du fichier des strat�gies</param>
        /// <param name="defaultStrategyTemplateName">Nom du template</param>
        /// <param name="canPersist">Doit on rajouter ce fichier � la solution</param>
        /// <returns>true si le fichier a �t� cr��</returns>
        private static void EnsureStrategiesFileExists(string strategiesFilePath, string defaultStrategyTemplateName, bool canPersist)
        {
            ILogger logger = ServiceLocator.Instance.GetService<ILogger>();
            //
            // Ancienne version de candle
            // Il faudra renommer le fichier
            // TODO a virer
            if (!File.Exists(strategiesFilePath))
            {
                string oldStrategiesFileName = Path.Combine(Path.GetDirectoryName(strategiesFilePath), "strategies.strategies");
                if (File.Exists(oldStrategiesFileName))
                {
                    // On copie l'ancien contenu dans le nouveau et on supprime l'ancien
                    string str = File.ReadAllText(oldStrategiesFileName);
                    File.WriteAllText(strategiesFilePath, str);
                    ServiceLocator.Instance.ShellHelper.DeleteFileFromProject(null, oldStrategiesFileName);
                }
            }

            // Cr�ation d'un fichier si existe pas
            if (!File.Exists(strategiesFilePath) )
            {
                // A partir d'un template
                if (!String.IsNullOrEmpty(defaultStrategyTemplateName))
                {
                    if (!Path.HasExtension(defaultStrategyTemplateName))
                        defaultStrategyTemplateName = String.Format("Strategies/{0}{1}", defaultStrategyTemplateName, DefaultStrategiesFileNameExtension);

                    if (!RepositoryManager.Instance.GetFileFromRepository(RepositoryCategory.Configuration, defaultStrategyTemplateName, strategiesFilePath))
                    {
                        // Si pas trouv�, on cr�e un fichier vide
                        Directory.CreateDirectory(Path.GetDirectoryName(strategiesFilePath));
                        File.WriteAllText(strategiesFilePath, EmptyStrategyFileContent);
                    }
                }
                // Ou fichier vide
                else 
                {
                    File.WriteAllText(strategiesFilePath, "<configuration/>");
                }
                if( logger!=null)
                    logger.Write("Strategy manager", "Creating the strategies file.", LogType.Info);
            }
            
            if( canPersist && File.Exists(strategiesFilePath))
                ServiceLocator.Instance.ShellHelper.AddFileToSolution(strategiesFilePath);
        }

        #endregion

        #region Manifests
        /// <summary>
        /// Gets the available manifests.
        /// </summary>
        /// <returns></returns>
        public static ManifestCollection GetAvailableManifests()
        {
            ManifestCollection manifests = new ManifestCollection();
            manifests.AddRange( RepositoryManager.Instance.GetExternalManifests());
            manifests.AddRange(GetLocalManifests(true));
            return manifests;
        }

        /// <summary>
        /// Liste des manifestes internes
        /// </summary>
        /// <returns></returns>
        public static ManifestCollection GetInternalManifests()
        {
            ManifestCollection manifests = new ManifestCollection();
            List<Type> types = StrategyPackage.FindAllTypes<StrategyBase>(typeof(StrategyBase).Assembly);

            foreach (Type strategyType in types)
            {
                InternalManifest manifest = new InternalManifest((StrategyBase)Activator.CreateInstance(strategyType));
                manifests.Add(manifest);
            }
            return manifests;
        }

        /// <summary>
        /// Liste des manifestes locaux
        /// </summary>
        /// <param name="includeInternals">if set to <c>true</c> [include internals].</param>
        /// <returns></returns>
        public static ManifestCollection GetLocalManifests(bool includeInternals)
        {
            ManifestCollection manifests = new ManifestCollection();
            DirectoryInfo strategiesFolder = new DirectoryInfo(RepositoryManager.ResolvePath(RepositoryCategory.Strategies, String.Empty));
            if (strategiesFolder.Exists)
            {
                foreach (FileInfo fi in strategiesFolder.GetFiles("*.zip"))
                {
                    StrategyPackage package = new StrategyPackage(fi.Name);
                    foreach (StrategyManifest mf in package.Manifests)
                        manifests.Add(mf);
                }

                // Les fichiers manifest seuls
                // COMMENT: pour l'instant, le internalManifest ne serailize pas ces propri�t�s. Il
                // faut le faire.
                //foreach (FileInfo fi in strategiesFolder.GetFiles(String.Concat("*", StrategyPackage.ManifestExtension)))
                //{
                //    StrategyManifest manifest = new InternalManifest();
                //    manifest.FileName = fi.Name;
                //    manifests.Add(manifest);
                //}
            }

            if (includeInternals)
                manifests.AddRange(GetInternalManifests());

            return manifests;
        }

        #endregion

        #region Persistence
        /// <summary>
        /// Chargement
        /// </summary>
        /// <param name="store">The store.</param>
        /// <param name="fileName">Name of the file.</param>
        /// <returns></returns>
        public static StrategyManager Load(Store store, string fileName)
        {
            ILogger logger = ServiceLocator.Instance.GetService<ILogger>();
            if (String.IsNullOrEmpty(fileName))
                return null;

            StrategyManager sm = null;
            try
            {
                using (StreamReader reader = new StreamReader(fileName))
                {
                    // D�s�rialization de la description
                    XmlSerializer serializer = new XmlSerializer(typeof(StrategyManager));
                    sm = (StrategyManager)serializer.Deserialize(reader);

                    // Lecture et chargement des types
                    List<Type> types = new List<Type>();
                    foreach (StrategyTypeReference str in sm.StrategyTypes)
                    {
                        InternalPackage package = sm.GetPackage(str.PackageName);
                        if (package != null)
                        {
                            Type type = package.GetStrategyType(str.StrategyTypeName);
                            if (type != null)
                                types.Add(type);
                        }
                    }

                    // D'abord chargement de la strat�gie de nommage
                    if (sm.NamingStrategyNode != null)
                    {
                        using (XmlNodeReader nr = new XmlNodeReader(sm.NamingStrategyNode))
                        {
                            serializer = new XmlSerializer(typeof(BaseNamingStrategy), types.ToArray());
                            sm._namingStrategy = (INamingStrategy)serializer.Deserialize(nr);
                        }
                    }

                    // Chargement des strategies
                    if (sm.StrategiesNode != null)
                    {
                        using (XmlNodeReader nr = new XmlNodeReader(sm.StrategiesNode))
                        {
                            serializer = new XmlSerializer(typeof(StrategyCollection), types.ToArray());
                            sm._strategies = (StrategyCollection)serializer.Deserialize(nr);
                        }

                        foreach (StrategyBase strategy in sm._strategies)
                        {
                            try
                            {
                                strategy.OnLoading(sm, new EventArgs());
                                StrategyPackage package = sm.GetPackage(strategy.PackageName) as StrategyPackage;
                                if( package!=null)
                                    strategy.StrategyFolder = package.PackageFolder;
                            }
                            catch (Exception ex)
                            {
                                IIDEHelper ide = ServiceLocator.Instance.GetService<IIDEHelper>();
                                if (ide != null)
                                {
                                    ide.ShowMessageBox("Error when loading strategy " + strategy.DisplayName + " : " + ex.Message, "Error", System.Windows.Forms.MessageBoxButtons.OK);
                                    ide.LogError(false, "Error when loading strategy " + strategy.DisplayName + " : " + ex.Message, 0, 0, "StrategyManager");
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                string text = ex.Message;
                XmlException xex = ex as XmlException;
                StringBuilder sb = new StringBuilder(ex.Message);
                if (xex != null)
                    sb.AppendFormat(" line={0}, column={1}", xex.LineNumber, xex.LinePosition);
                Exception iex = ex.InnerException;
                while (iex != null)
                {
                    sb.Append(" - ");
                    sb.Append(iex.Message);
                    iex = iex.InnerException;
                }
                text = sb.ToString();
                if( logger != null ) 
                    logger.WriteError("StrategyManager", String.Format("Loading error {0}", text), ex);
                IIDEHelper ide = ServiceLocator.Instance.GetService<IIDEHelper>();
                if (ide != null)
                {
                    ide.ShowMessage("Error when reading the strategies file (see error in the task list). A new empty file will be initialized. The current strategies file will be saved with a .bak extension");
                }
                try
                {                
                    // Sauvegarde du fichier en erreur
                    if (File.Exists(fileName))
                        Utils.CopyFile(fileName, fileName + ".bak");
                }
                catch { }
            }

            if (sm == null)
            {
                // G�n�ration d'un fichier par d�faut
                sm = new StrategyManager();
                sm.FileName = fileName;
                sm.Save(store);
            }
            else
                sm.FileName = fileName;

            return sm;
        }


        /// <summary>
        /// Rechargement de toutes les strat�gies disponibles
        /// </summary>
        static void ReloadOnChangeEvents(object sender, EventArgs e)
        {
            ClearAll();
        }

        /// <summary>
        /// Permet de s�rilalizer un objet � part
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="types">The types.</param>
        /// <param name="obj">The obj.</param>
        /// <returns></returns>
        private XmlNode SerializeFragment(Type type, List<Type> types, object obj)
        {
            StringBuilder sb = new StringBuilder();
            using (StringWriter ms = new StringWriter(sb))
            {
                XmlSerializer serializer = new XmlSerializer(type, types.ToArray());
                serializer.Serialize(ms, obj);

                // On met le r�sultat dans le noeud d�di�
                XmlDocument xdoc = new XmlDocument();
                xdoc.LoadXml(sb.ToString().Substring(39));
                return xdoc.FirstChild;
            }
        }

        /// <summary>
        /// Sauvegarde des strat�gies
        /// </summary>
        /// <param name="store"></param>
        public void Save(Store store)
        {
            if (_fileName == null)
                return;
            SuspendWatcher(false);
            try
            {
                _strategyTypes = _strategies.GetTypes(this.NamingStrategy);
                List<Type> types = new List<Type>();
                foreach (StrategyTypeReference t in _strategyTypes)
                {
                    types.Add(t.StrategyType);
                }

                // s�rialization des strat�gies
                this.StrategiesNode = SerializeFragment(typeof(StrategyCollection), types, _strategies);

                // s�rialization de la strat�gie de nommage
                this.NamingStrategyNode = SerializeFragment(typeof(BaseNamingStrategy), types, NamingStrategy);

                ServiceLocator.Instance.ShellHelper.EnsureDocumentIsNotInRDT(_fileName);

                // S�rialization du document avec les noeuds � jour
                using (SafeStreamWriter writer = new SafeStreamWriter(_fileName))
                {
                    // D�s�rialization de la description
                    XmlSerializer serializer = new XmlSerializer(typeof(StrategyManager));
                    serializer.Serialize(writer, this);
                }
                ServiceLocator.Instance.ShellHelper.AddFileToSolution(_fileName);
            }
            catch(Exception ex)
            {
                ServiceLocator.Instance.IDEHelper.ShowError("Unable to save the strategies file - " + ex.Message);
            }
            finally
            {
                SuspendWatcher(true);
            }
        }
        #endregion

        #region Watcher
        /// <summary>
        /// Handles the Changed event of the fileSystemWatcher control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.IO.FileSystemEventArgs"/> instance containing the event data.</param>
        static void fileSystemWatcher_Changed(object sender, FileSystemEventArgs e)
        {
            ClearAll();
        }

        /// <summary>
        /// D�marre le watcher sur le fichier des strat�gies
        /// </summary>
        ///<param name="enabled">Active ou pas</param>
        private static void SuspendWatcher(bool enabled)
        {
            //if (fileSystemWatcher != null)
            //{
            //    fileSystemWatcher.EnableRaisingEvents = false;
            //    fileSystemWatcher.Changed -= new FileSystemEventHandler(fileSystemWatcher_Changed);
            //    fileSystemWatcher.Dispose();
            //}

            if (s_folder != null && Directory.Exists(s_folder) && s_fileSystemWatcher==null)
            {
                s_fileSystemWatcher = new FileSystemWatcher(s_folder);
                // Lors de la modif d'un fichier dans visual studio, ce n'est pas le r��llement le fichier
                // qui est modifi� mais une copie avec une extension bizzare d'ou le ~* � la fin du filtre.
                s_fileSystemWatcher.Filter = "*" + DefaultStrategiesFileNameExtension + "~*"; 
                s_fileSystemWatcher.NotifyFilter = NotifyFilters.LastWrite;
                s_fileSystemWatcher.Changed += new FileSystemEventHandler(fileSystemWatcher_Changed);
            }

            if( s_fileSystemWatcher!=null)
                s_fileSystemWatcher.EnableRaisingEvents = enabled;            
        }
        #endregion
    
        #region IStrategyManager Members

        /// <summary>
        /// Nom du fichier
        /// </summary>
        [XmlIgnore]
        public string FileName
        {
            [System.Diagnostics.DebuggerStepThrough()]
            get { return _fileName; }
            set
            {
                _fileName = value;
                if (s_folder == null)
                    s_folder = Path.GetDirectoryName(value);
            }
        }

        /// <summary>
        /// Strat�gie de nommage
        /// </summary>
        [XmlIgnore]
        public INamingStrategy NamingStrategy
        {
            get
            {
                if (_namingStrategy == null)
                    _namingStrategy = (INamingStrategy)new BaseNamingStrategy();
                return _namingStrategy;
            }
        }

        /// <summary>
        /// Target language
        /// </summary>
        /// <value></value>
        [XmlElement("language")]
        public LanguageConfiguration TargetLanguage
        {
            get
            {
                if (_languageConfiguration == null)
                    _languageConfiguration = new LanguageConfiguration();
                return _languageConfiguration;
            }
            [global::System.Diagnostics.DebuggerStepThrough]
            set { _languageConfiguration = value; }
        }

        /// <summary>
        /// Gets the assembly extension.
        /// </summary>
        /// <param name="layer">The layer.</param>
        /// <returns></returns>
        public string GetAssemblyExtension(SoftwareLayer layer)
        {
            IList<StrategyBase> strategies = layer.GetStrategies(true);

            // D'abord ex�cution des strat�gies g�n�rant les projets
            foreach (StrategyBase strategy in strategies)
            {
                if (strategy is IStrategyProvidesProjectTemplates)
                {
                    string ext = ((IStrategyProvidesProjectTemplates)strategy).GetAssemblyExtension(layer);
                    if (!String.IsNullOrEmpty(ext))
                        return ext;
                }
            }
            return ".dll"; // defaut
        }

        #endregion

    }
}
