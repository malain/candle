using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using DSLFactory.Candle.SystemModel.Repository;

namespace DSLFactory.Candle.SystemModel.Strategies
{
    /// <summary>
    /// Un package correspond � un fichier zip contenant les fichiers
    /// manifests et les assemblies
    /// </summary>
    [Serializable]
    public class StrategyPackage : InternalPackage
    {
        /// <summary>
        /// 
        /// </summary>
        public const string ManifestExtension = ".manifest";

        private List<Assembly> _assemblies;
        private bool _initialized;
        private bool _isValid;
        private ManifestCollection _manifests;
        private string _name;
        private string _serverUrl;

        /// <summary>
        /// Un package correspond au fichier zip contenant une strat�gie
        /// </summary>
        public StrategyPackage(string packageName)
        {
            _name = Path.GetFileName(packageName);
        }

        /// <summary>
        /// Un package correspond au fichier zip contenant une strat�gie
        /// </summary>
        public StrategyPackage()
        {
        }

        /// <summary>
        /// Liste des manifestes contenues dans le package
        /// </summary>
        /// <value>The manifests.</value>
        [XmlIgnore]
        public ManifestCollection Manifests
        {
            get
            {
                if (_manifests == null)
                    ExtractManifests();
                return _manifests;
            }
        }

        /// <summary>
        /// Nom du package. Le nom peut-�tre qualifi� par l'url du
        /// serveur contenant le package sous la forme :
        /// nom@url
        /// </summary>
        /// <value>The name.</value>
        [XmlAttribute("name")]
        public string Name
        {
            get { return _name; }
            set
            {
                int pos = value.IndexOf('@');
                if (pos > 0)
                {
                    _name = value.Substring(0, pos);
                    if (pos + 1 < value.Length)
                        _serverUrl = value.Substring(pos + 1);
                }
                else
                {
                    _serverUrl = null;
                    _name = value;
                }
            }
        }

        /// <summary>
        /// Repertoire du package
        /// </summary>
        /// <value></value>
        internal override string PackageFolder
        {
            get
            {
                return
                    RepositoryManager.ResolvePath(RepositoryCategory.Strategies, Path.GetFileNameWithoutExtension(_name));
            }
        }

        /// <summary>
        /// Chemin du fichier
        /// </summary>
        private string FullName
        {
            get { return RepositoryManager.ResolvePath(RepositoryCategory.Strategies, _name); }
        }

        /// <summary>
        /// S'assure que la version locale correspond � la derni�re version du serveur
        /// </summary>
        public bool Synchronize()
        {
            // Si la derni�re initialisation s'est mal pass�e, ce n'est pas la peine de continuer.
            // Si le fichier de strategie n'existe plus (parce qu'il a pu �tre supprim� par l'utilisateur 
            // depuis la derni�re initialisation), on re-synchronise.
            if (_initialized && (File.Exists(FullName) || !_isValid))
                return _isValid;

            _isValid = true; // Par d�faut

            ILogger logger = ServiceLocator.Instance.GetService<ILogger>();

            // R�cup�ration du provider du serveur
            IRepositoryProvider ws = RepositoryManager.Instance.GetRemoteRepository(_serverUrl);
            if (ws != null)
            {
                try
                {
                    RepositoryServerItemStatus status = RepositoryServerItemStatus.NotModified;
                    // TODO tester le cache
                    status = ws.GetFile(RepositoryCategory.Strategies, _name, FullName);

                    switch (status)
                    {
                        case RepositoryServerItemStatus.NotModified:
                            Initialize();
                            return true;

                        case RepositoryServerItemStatus.NotFound:

                            // COMMENT-BEGIN : Non pas de suppression car cette strat�gie
                            // est peut-�tre utilis� ailleurs et on veut la garder
                            // On se contente d'�mettre un message

                            // Suppression du fichier package
                            //Utils.DeleteFile(FullName);
                            // Puis suppression du r�pertoire. 
                            // Nota : Il peut arriver que la suppression du r�pertoire ne se fasse
                            // pas compl�tement car l'assembly est v�rouill�. Pour r�soudre ce cas,
                            // � chaque chargement du StrategyManager, on supprimera tous les r�pertoires
                            // qui n'ont pas de fichier package associ�.
                            //Utils.RemoveDirectory(PackageFolder);
                            // COMMENT-END
                            if (logger != null)
                                logger.Write("Strategy Synchronization",
                                             String.Format("Warning : Le package {0} n'existe plus sur le serveur",
                                                           _name), LogType.Warning);

                            if (File.Exists(FullName))
                            {
                                // Si il existe on le prend tel quel
                                Initialize();
                                return true;
                            }

                            // Sinon on indique qu'on a rien
                            _isValid = false;
                            return false;

                        case RepositoryServerItemStatus.Loaded:
                            if (logger != null)
                                logger.Write("Strategy Synchronization",
                                             String.Format("Getting new version for strategy {0}", _name), LogType.Info);
                            // Extraction des fichiers
                            ExtractAndInitialize();
                            return true;
                    }
                }
                catch (Exception ex)
                {
                    if (logger != null)
                        logger.WriteError("Synchronize", String.Format("Synchronize package {0}", _name), ex);
                    Initialize(); // Pour avoir un package propre
                }
            }
            else
            {
                ExtractAndInitialize();
                // Ne pas oublier de faire au moins Initialize() si on met du cache
            }
            return true;
        }

        /// <summary>
        /// Cr�ation de l'instance de la strat�gie 
        /// </summary>
        /// <param name="strategyTypeName">Nom de la strat�gie</param>
        /// <param name="node">Noeud contenant les donn�es de s�rialisation sp�cifique � cette strat�gie</param>
        /// <returns>L'instance de la strat�gie ou null</returns>
        internal override StrategyBase CreateStrategyInstance(string strategyTypeName, XmlNode node)
        {
            StrategyBase strategy = null;
            try
            {
                Type type = GetStrategyType(strategyTypeName);
                strategy = CreateStrategyInstance(type, node);
                strategy.PackageName = Name;
                strategy.StrategyFolder = PackageFolder;
            }
            catch (Exception ex)
            {
                ILogger logger = ServiceLocator.Instance.GetService<ILogger>();
                if (logger != null)
                {
                    string txt = strategy != null ? strategy.DisplayName : strategyTypeName ?? Name;
                    StringBuilder msg = new StringBuilder(txt);
                    msg.Append(" - Error : ");
                    msg.Append(ex.Message);
                    Exception tmp = ex.InnerException;
                    while (tmp != null)
                    {
                        msg.Append(" > ");
                        msg.Append(tmp.Message);
                        tmp = tmp.InnerException;
                    }
                    logger.Write("StrategyPackage", msg.ToString(), LogType.Error);
                }
            }
            return strategy;
        }

        /// <summary>
        /// Recherche du type de la strategy
        /// </summary>
        /// <param name="strategyTypeName">Name of the strategy type.</param>
        /// <returns></returns>
        internal override Type GetStrategyType(string strategyTypeName)
        {
            // Le type est soit :
            //  - celui du fichier manifest (pass� en param�tre)
            //  - soit se trouve dans dans l'assembly du package
            //  - sinon on prend la GenericStrategy
            if (!String.IsNullOrEmpty(strategyTypeName))
            {
                string assemblyName = null;
                string typeName = strategyTypeName;

                int pos = strategyTypeName.IndexOf(',');
                if (pos > 0)
                {
                    assemblyName = strategyTypeName.Substring(pos + 1).Trim();
                    typeName = strategyTypeName.Substring(0, pos).Trim();
                }

                Assembly assembly = null;
                if (!String.IsNullOrEmpty(assemblyName))
                    assembly =
                        _assemblies.Find(
                            delegate(Assembly a) { return Utils.StringCompareEquals(a.GetName().Name, assemblyName); });

                try
                {
                    if (assembly == null)
                        return Type.GetType(typeName, true);
                    return assembly.GetType(typeName, true);
                }
                catch (Exception ex)
                {
                    ILogger logger = ServiceLocator.Instance.GetService<ILogger>();
                    if (logger != null)
                    {
                        StringBuilder msg = new StringBuilder(strategyTypeName);
                        msg.Append(" - Error : ");
                        msg.Append(ex.Message);
                        Exception tmp = ex.InnerException;
                        while (tmp != null)
                        {
                            msg.Append(" > ");
                            msg.Append(tmp.Message);
                            tmp = tmp.InnerException;
                        }
                        logger.Write("StrategyPackage Loading type", msg.ToString(), LogType.Error);
                    }
                }
                return null;
            }

            // Si pas de type indiqu�, on cherche dans les assemblies du package
            foreach (Assembly asm in _assemblies)
            {
                List<Type> types = FindAllTypes<StrategyBase>(asm);
                if (types.Count == 1)
                {
                    return types[0];
                }
                // Si il y en a plusieurs, on ne peut pas savoir lequel prendre
                if (types.Count > 1)
                    throw new Exception("You must precise the strategy type name");
            }

            // Type par d�faut
            return typeof (GenericStrategy);
        }

        /// <summary>
        /// Chargement de l'assembly
        /// </summary>
        /// <param name="assemblyName">Name of the assembly.</param>
        /// <returns></returns>
        public Assembly LoadAssembly(string assemblyName)
        {
            if (String.IsNullOrEmpty(assemblyName))
                return GetType().Assembly;

            try
            {
                AppDomain.CurrentDomain.AssemblyResolve += AssemblyResolve;
                return LoadAssemblyInternal(assemblyName);
            }
            finally
            {
                AppDomain.CurrentDomain.AssemblyResolve -= AssemblyResolve;
            }
        }

        /// <summary>
        /// Chargement de l'assembly contenu dans le package.
        /// </summary>
        /// <param name="assemblyName">Name of the assembly.</param>
        /// <returns></returns>
        private Assembly LoadAssemblyInternal(string assemblyName)
        {
            Debug.Assert(_name != null);
            return Assembly.LoadFrom(Path.Combine(PackageFolder, assemblyName));
        }

        /// <summary>
        /// R�solution d'une assembly satellite de la strat�gie
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="System.ResolveEventArgs"/> instance containing the event data.</param>
        /// <returns></returns>
        private Assembly AssemblyResolve(object sender, ResolveEventArgs args)
        {
            string assemblyName = args.Name;
            if (assemblyName.IndexOf(".XmlSerializers") > 0)
                return null;
            return LoadAssemblyInternal(assemblyName);
        }

        /// <summary>
        /// Recherche des strategies pr�sentes dans l'assembly
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="asm">The asm.</param>
        /// <returns></returns>
        public static List<Type> FindAllTypes<T>(Assembly asm) where T : class
        {
            List<Type> types = new List<Type>();
            if (asm == null)
                return null;

            foreach (Type t in asm.GetTypes())
            {
                if (!t.IsClass || t.IsAbstract)
                    continue;

                if (typeof (T).IsAssignableFrom(t))
                {
                    types.Add(t);
                }
            }

            return types;
        }

        /// <summary>
        /// Extraction des fichiers du package et chargement des assemblies
        /// </summary>
        public void Extract()
        {
            Utils.RemoveDirectory(PackageFolder);
            if (Directory.Exists(PackageFolder) && Directory.GetFiles(PackageFolder, ".dll").Length > 0)
            {
                IIDEHelper ide = ServiceLocator.Instance.IDEHelper;
                if (ide != null)
                    ide.ShowMessage(
                        String.Format("Change detected in strategies package {0} - You must restart Visual Studio",
                                      _name));
            }

            Directory.CreateDirectory(PackageFolder);

            RepositoryZipFile zip = new RepositoryZipFile(FullName, true);
            zip.ExtractAll(PackageFolder);
        }

        /// <summary>
        /// Extracts  and initialize.
        /// </summary>
        public void ExtractAndInitialize()
        {
            Extract();
            Initialize();
        }

        /// <summary>
        /// R�pertorie les fichiers pr�sents dans le r�pertoire
        /// </summary>
        private void Initialize()
        {
            // Les assemblies
            _assemblies = new List<Assembly>();
            //if( !Directory.Exists(PackageFolder) )
            Extract();

            foreach (string fileName in Directory.GetFiles(PackageFolder, "*.dll"))
            {
                _assemblies.Add(LoadAssembly(fileName));
            }

            // Et les manifests
            _manifests = new ManifestCollection();
            FindManifests(PackageFolder);
            _initialized = true;
        }

        /// <summary>
        /// Extrait les fichiers manifests contenues dans le package
        /// </summary>
        private void ExtractManifests()
        {
            _manifests = new ManifestCollection();
            if (!File.Exists(FullName))
                return;

            // Extraction dans un r�pertoire temporaire
            string temporaryFolder = Path.GetTempPath();
            temporaryFolder = Path.Combine(temporaryFolder, "$$Temp");

            if (Directory.Exists(temporaryFolder))
                Utils.RemoveDirectory(temporaryFolder);
            Directory.CreateDirectory(temporaryFolder);

            // Extraction du manifest
            RepositoryZipFile zip = new RepositoryZipFile(FullName, false);
            try
            {
                zip.ExtractFileWithExtension(temporaryFolder, ManifestExtension);
                FindManifests(temporaryFolder);
            }
            finally
            {
                if (Directory.Exists(temporaryFolder))
                {
                    Utils.RemoveDirectory(temporaryFolder);
                }
            }
        }

        /// <summary>
        /// recherche des fichiers manifests
        /// </summary>
        /// <param name="folder">The folder.</param>
        private void FindManifests(string folder)
        {
            Debug.Assert(_manifests != null);
            foreach (string manifestFileName in Utils.SearchFile(folder, "*.manifest"))
            {
                try
                {
                    StrategyManifest sm = StrategyManifest.DeserializeManifest(manifestFileName);
                    sm.PackageName = _name;
                    sm.FileName = manifestFileName;
                    _manifests.Add(sm);
                }
                catch (Exception ex)
                {
                    ILogger logger = ServiceLocator.Instance.GetService<ILogger>();
                    if (logger != null)
                        logger.WriteError("Strategy package",
                                          String.Format("Error when reading the manifest file {0}", manifestFileName),
                                          ex);
                }
            }
        }
    }
}