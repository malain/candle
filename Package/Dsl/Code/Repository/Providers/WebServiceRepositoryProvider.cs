using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Web;
using DSLFactory.Candle.SystemModel.Configuration;
using DSLFactory.Candle.SystemModel.Strategies;
using DSLFactory.Candle.SystemModel.WSReferentiel;

namespace DSLFactory.Candle.SystemModel.Repository.Providers
{
    /// <summary>
    /// Provider d'accés au repository via des services web
    /// </summary>
    public class WebServiceRepositoryProvider : IRepositoryProvider
    {
        private readonly string _baseUrl;

        /// <summary>
        /// Création d'un provider associé à un serveur
        /// </summary>
        /// <param name="url">Url de base du serveur distant</param>
        public WebServiceRepositoryProvider(string url)
        {
            if (String.IsNullOrEmpty(url))
                throw new ArgumentNullException("url");
            _baseUrl = url;
        }

        #region IRepositoryProvider Members

        /// <summary>
        /// Nom du provider (correspond à l'url de base)
        /// </summary>
        public string Name
        {
            get { return _baseUrl; }
        }

        /// <summary>
        /// Récupèration d'un fichier contenant un modèle
        /// </summary>
        /// <param name="metadata">Caractèristiques du modèle à charger</param>
        /// <returns></returns>
        public RepositoryServerItemStatus GetModel(ComponentModelMetadata metadata)
        {
            if (metadata == null)
                throw new ArgumentNullException("metadata");
            string url =
                String.Format("{0}/services/DownloadFile.ashx?c={1}&p={2}", _baseUrl, RepositoryCategory.Models,
                              metadata.GetFileName(PathKind.Relative));
            return GetFile(url, metadata.GetFileName(PathKind.Absolute));
        }

        /// <summary>
        /// Copie un fichier du repository en local
        /// </summary>
        /// <param name="category">Category du fichier à récupérer</param>
        /// <param name="path">Chemin d'accés relatif à la catégorie</param>
        /// <param name="localFile">Chemin absolu local de destination</param>
        /// <returns>Status du chargement</returns>
        public RepositoryServerItemStatus GetFile(RepositoryCategory category, string path, string localFile)
        {
            string url =
                String.Format("{0}/services/DownloadFile.ashx?c={1}&p={2}", _baseUrl, category,
                              HttpUtility.UrlEncode(path));
            return GetFile(url, localFile);
        }

        /// <summary>
        /// Gets the model metadata.
        /// </summary>
        /// <param name="modelId">The model id.</param>
        /// <param name="version">The version.</param>
        /// <returns></returns>
        public ComponentModelMetadata GetModelMetadata(Guid modelId, VersionInfo version)
        {
            ILogger logger = ServiceLocator.Instance.GetService<ILogger>();
            try
            {
                RepositoryFileService fh = GetFileRepositoryProxy();
                return fh.GetMetadata(modelId, version);
            }
            catch (Exception ex)
            {
                if (logger != null)
                    logger.WriteError("Get metadata", "Reading metadata", ex);
            }
            return null;
        }

        /// <summary>
        /// Retourne la liste des métadatas de tous les modéles présent dans le repository
        /// </summary>
        /// <returns></returns>
        public List<ComponentModelMetadata> GetAllMetadata()
        {
            ILogger logger = ServiceLocator.Instance.GetService<ILogger>();
            RepositoryFileService fh = GetFileRepositoryProxy();
            List<ComponentModelMetadata> models = null;

            try
            {
                models = new List<ComponentModelMetadata>(fh.GetAllMetadata());
            }
            catch (Exception ex)
            {
                if (logger != null)
                    logger.WriteError("Get metadatas", "Repository", ex);
            }
            return models;
        }

        /// <summary>
        /// Retourne la liste des manifests des stratégies
        /// </summary>
        /// <returns>null pour les stratégies locales</returns>
        public StrategyManifest[] GetStrategyManifests()
        {
            ILogger logger = ServiceLocator.Instance.GetService<ILogger>();
            RepositoryStrategiesService fh = GetStrategiesRepositoryProxy();

            try
            {
                return fh.GetStrategyManifests();
            }
            catch (Exception ex)
            {
                if (logger != null)
                    logger.WriteError("Repository", "GetUpdatableStrategies", ex);
            }
            return new StrategyManifest[] {};
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
            string url =
                String.Format("{0}/services/uploadModel.ashx?n={1}&id={2}&c={3}&v={4}", _baseUrl,
                              HttpUtility.UrlEncode(remoteName), CandleSettings.LicenseId, category,
                              CandleSettings.Version);

            WebClient wc = new WebClient();
            wc.Proxy = GetProxy();
            wc.UploadFile(url, fileName);

            return fileName;
        }

        /// <summary>
        /// Publishes the model.
        /// </summary>
        /// <param name="systemDefinition">The system definition.</param>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="remoteName">Name of the remote.</param>
        /// <returns></returns>
        public string PublishModel(CandleModel systemDefinition, string fileName, string remoteName)
        {
            string relativeName = systemDefinition.MetaData.ResolvePath(remoteName, PathKind.Relative);
            if (relativeName == String.Empty)
                return string.Empty;

            string url =
                String.Format("{0}/services/uploadModel.ashx?n={1}&id={2}&v={3}", _baseUrl,
                              HttpUtility.UrlEncode(relativeName), CandleSettings.LicenseId, CandleSettings.Version);

            WebClient wc = new WebClient();
            wc.Proxy = GetProxy();
            wc.UploadFile(url, fileName);

            return fileName;
        }

        /// <summary>
        /// Enumère le contenu d'une catégorie
        /// </summary>
        /// <param name="category">Type de la catégorie</param>
        /// <param name="filter">Filtre sur les fichiers ou null pour tous (*.xxx)</param>
        /// <param name="recursive">Prend en compte les sous-répertoires</param>
        /// <returns></returns>
        public List<RepositoryFileInfo> EnumerateCategory(RepositoryCategory category, string filter, bool recursive)
        {
            RepositoryFileService fh = GetFileRepositoryProxy();
            return new List<RepositoryFileInfo>(fh.EnumerateCategory(category, filter, recursive));
        }

        /// <summary>
        /// N° de version du repository
        /// </summary>
        /// <returns></returns>
        public Version GetVersion()
        {
            RepositoryFileService fh = GetFileRepositoryProxy();
            return fh.GetVersion();
        }

        /// <summary>
        /// Suppression d'un path
        /// </summary>
        /// <param name="path">Chemin (xxx/../xxx)</param>
        public void RemoveDomainPath(string path)
        {
            RepositoryFileService fh = GetFileRepositoryProxy();
            fh.RemoveDomainPath(path);
        }

        /// <summary>
        /// Création d'un path
        /// </summary>
        /// <param name="path">Chemin (xxx/../xxx)</param>
        public void CreateDomainPath(string path)
        {
            RepositoryFileService fh = GetFileRepositoryProxy();
            fh.CreateDomainPath(path);
        }

        #endregion

        /// <summary>
        /// Récupération du fichier sur le serveur distant
        /// </summary>
        /// <param name="url">Url avec les paramètres</param>
        /// <param name="localFile">Chemin du fichier qui sera créé en local</param>
        /// <returns></returns>
        private RepositoryServerItemStatus GetFile(string url, string localFile)
        {
            ILogger logger = ServiceLocator.Instance.GetService<ILogger>();
            try
            {
                // On fait passer la date du fichier local pour éviter de recharger le fichier si il est 
                // à jour.
                DateTime localFileDate = DateTime.MinValue; // Par défaut
                if (File.Exists(localFile)) localFileDate = File.GetLastWriteTimeUtc(localFile);

                // On rajoute les paramètres systèmes à l'url
                string id = CandleSettings.LicenseId;
                url =
                    String.Concat(url,
                                  String.Format("&d={0}&id={1}&v={2}", localFileDate.Ticks, id, CandleSettings.Version));

                // Création de la requète
                HttpWebRequest request = (HttpWebRequest) WebRequest.Create(url);
                request.ServicePoint.UseNagleAlgorithm = false;
                request.UnsafeAuthenticatedConnectionSharing = true;
                request.Proxy = GetProxy();

                // Attente de la réponse
                HttpWebResponse response = (HttpWebResponse) request.GetResponse();
                Stream reader = response.GetResponseStream();

                // Téléchargement du fichier
                Directory.CreateDirectory(Path.GetDirectoryName(localFile));

                // Ecriture dans le fichier local
                using (Stream writer = new FileStream(localFile, FileMode.Create, FileAccess.Write))
                {
                    byte[] buffer = new byte[0x1000];
                    int cb = reader.Read(buffer, 0, buffer.Length);
                    while (cb > 0)
                    {
                        writer.Write(buffer, 0, cb);
                        cb = reader.Read(buffer, 0, buffer.Length);
                    }
                    writer.Flush();
                }
                if (logger != null)
                    logger.Write("Get remote file", String.Format("File {0} retrieved from repository", localFile),
                                 LogType.Info);
                return RepositoryServerItemStatus.Loaded;
            }
            catch (WebException ex)
            {
                if (ex.Response != null)
                {
                    // Le fichier n'existe plus sur le serveur
                    if (((HttpWebResponse) ex.Response).StatusCode == HttpStatusCode.NotFound)
                        return RepositoryServerItemStatus.NotFound;

                    // Les fichiers sont identiques
                    if (((HttpWebResponse) ex.Response).StatusCode == HttpStatusCode.NotModified)
                        return RepositoryServerItemStatus.NotModified;
                }
            }
            catch (Exception ex)
            {
                if (logger != null)
                    logger.WriteError("Get remote file",
                                      String.Format("Error : unable to retrieve the file {0} from repository", localFile),
                                      ex);
            }

            // Ici c'est qu'il y a eu une erreur
            if (File.Exists(localFile))
                return RepositoryServerItemStatus.NotModified;

            return RepositoryServerItemStatus.NotFound;
        }

        /// <summary>
        /// Gets the proxy.
        /// </summary>
        /// <returns></returns>
        private static IWebProxy GetProxy()
        {
            return null;
        }

        /// <summary>
        /// Gets the file repository proxy.
        /// </summary>
        /// <returns></returns>
        private RepositoryFileService GetFileRepositoryProxy()
        {
            RepositoryFileService fh = new RepositoryFileService();
            fh.Url =
                String.Format("{0}/services/repositoryfileservice.asmx?id={1}&v={2}", _baseUrl, CandleSettings.LicenseId,
                              CandleSettings.Version);
            return fh;
        }

        /// <summary>
        /// Gets the strategies repository proxy.
        /// </summary>
        /// <returns></returns>
        private RepositoryStrategiesService GetStrategiesRepositoryProxy()
        {
            RepositoryStrategiesService fh = new RepositoryStrategiesService();

            fh.Url =
                String.Format("{0}/services/repositorystrategiesservice.asmx?id={1}&v={2}", _baseUrl,
                              CandleSettings.LicenseId, CandleSettings.Version);
            return fh;
        }
    }
}