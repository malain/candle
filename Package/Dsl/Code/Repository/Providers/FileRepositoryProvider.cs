using System;
using System.Collections.Generic;
using System.IO;
using DSLFactory.Candle.SystemModel.Strategies;

namespace DSLFactory.Candle.SystemModel.Repository.Providers
{
    /// <summary>
    /// 
    /// </summary>
    public class FileRepositoryProvider : IRepositoryProvider
    {
        private readonly string _modelPath;
        private readonly string _path;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileRepositoryProvider"/> class.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="modelPath">The model path.</param>
        public FileRepositoryProvider(string path, string modelPath)
        {
            _path = path;
            _modelPath = modelPath;
        }

        #region IRepositoryProvider Members

        /// <summary>
        /// Nom du provider
        /// </summary>
        /// <value></value>
        public string Name
        {
            get { return "Local repository :" + _path; }
        }

        /// <summary>
        /// Récupère un fichier dans le repository
        /// </summary>
        /// <param name="category">Category du fichier à récupérer</param>
        /// <param name="path">Chemin d'accés relatif à la catégorie</param>
        /// <param name="localFile">Chemin absolu local de destination</param>
        /// <returns>Status du chargement</returns>
        public RepositoryServerItemStatus GetFile(RepositoryCategory category, string path, string localFile)
        {
            ILogger logger = ServiceLocator.Instance.GetService<ILogger>();
            string sourcePath = RepositoryManager.ResolvePath(category, path);
            if (!File.Exists(sourcePath))
                return RepositoryServerItemStatus.NotFound;

            // C'est le même fichier
            if (Utils.StringCompareEquals(sourcePath, localFile))
                return RepositoryServerItemStatus.NotModified;

            try
            {
                Utils.CopyFile(sourcePath, localFile);
                return RepositoryServerItemStatus.Loaded;
            }
            catch (Exception ex)
            {
                if (logger != null)
                    logger.WriteError("Get remote file", String.Format("Error : unable to copy the file {0}", path), ex);
            }
            return RepositoryServerItemStatus.NotFound;
        }

        //public List<RepositoryItem> GetAllVersionsForApplication(string path, string name)
        //{
        //    DirectoryInfo di = new DirectoryInfo( Path.Combine( _modelPath, name) );
        //    if (di.Exists)
        //    {
        //        return GetModelFromDirectory(di.FullName);
        //    }
        //    else
        //    {
        //        di.Create();
        //    }
        //    return new List<RepositoryItem>();
        //}

        /// <summary>
        /// Gets the model metadata.
        /// </summary>
        /// <param name="modelId">The model id.</param>
        /// <param name="version">The version.</param>
        /// <returns></returns>
        public ComponentModelMetadata GetModelMetadata(Guid modelId, VersionInfo version)
        {
            if (modelId == Guid.Empty || version == null)
                return null;

            string path = Path.Combine(_modelPath, Path.Combine(modelId.ToString(), version.ToString()));

            // Recherche du modèle
            if (Directory.Exists(path))
            {
                string[] fileName = Directory.GetFiles(path, ModelConstants.FilterExtension);
                if (fileName.Length > 0)
                    return ComponentModelMetadata.RetrieveMetadata(fileName[0]);
            }
            return null;
        }

        /// <summary>
        /// Liste des modèles dans le repository
        /// </summary>
        /// <returns></returns>
        public List<ComponentModelMetadata> GetAllMetadata()
        {
            DirectoryInfo di = new DirectoryInfo(_modelPath);
            if (di.Exists)
            {
                return GetModelFromDirectory(di.FullName);
            }
            else
            {
                di.Create();
            }

            return new List<ComponentModelMetadata>();
        }

        /// <summary>
        /// Récupération d'un modèle (cml)
        /// </summary>
        /// <param name="metadata">Caractèristiques du modèle</param>
        /// <returns></returns>
        public RepositoryServerItemStatus GetModel(ComponentModelMetadata metadata)
        {
            throw new Exception("Not implemented");
        }

        /// <summary>
        /// Retourne la liste des manifests des stratégies
        /// </summary>
        /// <returns>null pour les stratégies locales</returns>
        public StrategyManifest[] GetStrategyManifests()
        {
            return StrategyManager.GetLocalManifests(false).ToArray();
        }

        /// <summary>
        /// Copie dans le repository local
        /// </summary>
        /// <param name="model">Le modèle à publier</param>
        /// <param name="fileName">Chemin absolu du modèle à publier</param>
        /// <param name="remoteName">Nom du fichier modèle dans le repository</param>
        /// <returns></returns>
        public string PublishModel(CandleModel model, string fileName, string remoteName)
        {
            ILogger logger = ServiceLocator.Instance.GetService<ILogger>();
            remoteName = Path.GetFileName(remoteName);
            string targetFileName = model.MetaData.ResolvePath(remoteName, PathKind.Absolute);

            // On ne copie que si c'est nécessaire
            if (targetFileName != String.Empty && File.Exists(fileName)
                && !Utils.StringCompareEquals(targetFileName, fileName)
                && !Utils.FileDateEquals(targetFileName, fileName))
            {
                try
                {
                    Utils.CopyFile(fileName, targetFileName);
                }
                catch (Exception ex)
                {
                    if (logger != null)
                        logger.WriteError("Repository", "Publish model", ex);
                }
            }
            else if (logger != null)
                logger.Write("Repository", "Skip copy of " + fileName + " (same version)", LogType.Info);

            return targetFileName;
        }

        /// <summary>
        /// Enumère les fichiers disponibles dans une catègorie
        /// </summary>
        /// <param name="category">Catégorie à lister</param>
        /// <param name="filter">Filtre de fichier</param>
        /// <param name="recursive">Indique si on examine les sous-répertoires</param>
        /// <returns></returns>
        public List<RepositoryFileInfo> EnumerateCategory(RepositoryCategory category, string filter, bool recursive)
        {
            DirectoryInfo di = new DirectoryInfo(RepositoryManager.ResolvePath(category, String.Empty));
            return EnumerateRecursive(di.FullName.Length + 1, di, category, filter, recursive);
        }

        /// <summary>
        /// N° de version du repository
        /// </summary>
        /// <returns></returns>
        public Version GetVersion()
        {
            return GetType().Assembly.GetName().Version;
        }

        /// <summary>
        /// Publie un fichier sur le référentiel
        /// </summary>
        /// <param name="fileName">Chemin relatif du fichier</param>
        /// <param name="category">Categorie d'appartenance</param>
        /// <param name="remoteName">Nom du fichier destination</param>
        /// <returns></returns>
        public string PublishFile(string fileName, RepositoryCategory category, string remoteName)
        {
            throw new Exception("PublishFile not implemented");
        }

        /// <summary>
        /// Création d'un path
        /// </summary>
        /// <param name="path">Chemin (xxx/../xxx)</param>
        public void CreateDomainPath(string path)
        {
            DomainManager.Instance.CreateDomainPath(path);
        }

        /// <summary>
        /// Suppression d'un path
        /// </summary>
        /// <param name="path">Chemin (xxx/../xxx)</param>
        public void RemoveDomainPath(string path)
        {
            DomainManager.Instance.RemoveDomainPath(path);
        }

        #endregion

        /// <summary>
        /// Parcours d'un répertoire pour récupèrer les métadata des modèles
        /// </summary>
        /// <param name="directoryName">Name of the directory.</param>
        /// <returns></returns>
        private static List<ComponentModelMetadata> GetModelFromDirectory(string directoryName)
        {
            List<ComponentModelMetadata> items = new List<ComponentModelMetadata>();
            foreach (string fileName in Utils.SearchFile(directoryName, ModelConstants.FilterExtension))
            {
                ComponentModelMetadata item = ComponentModelMetadata.RetrieveMetadata(fileName);
                items.Add(item);
            }
            return items;
        }

        /// <summary>
        /// Enumerates the recursive.
        /// </summary>
        /// <param name="pathRootLength">Length of the path root.</param>
        /// <param name="di">The di.</param>
        /// <param name="category">The category.</param>
        /// <param name="filter">The filter.</param>
        /// <param name="recursive">if set to <c>true</c> [recursive].</param>
        /// <returns></returns>
        public static List<RepositoryFileInfo> EnumerateRecursive(int pathRootLength, DirectoryInfo di,
                                                                  RepositoryCategory category, string filter,
                                                                  bool recursive)
        {
            if (String.IsNullOrEmpty(filter))
                filter = "*.*";

            List<RepositoryFileInfo> results = new List<RepositoryFileInfo>();
            if (di.Exists)
            {
                foreach (FileInfo fi in di.GetFiles(filter))
                {
                    RepositoryFileInfo rfi = new RepositoryFileInfo();
                    rfi.FileName = fi.FullName.Substring(pathRootLength);
                    rfi.LastWriteTimeUtc = fi.LastWriteTimeUtc;
                    results.Add(rfi);
                }

                if (recursive)
                {
                    foreach (DirectoryInfo childDi in di.GetDirectories())
                    {
                        results.AddRange(EnumerateRecursive(pathRootLength, childDi, category, filter, true));
                    }
                }
            }
            return results;
        }
    }
}