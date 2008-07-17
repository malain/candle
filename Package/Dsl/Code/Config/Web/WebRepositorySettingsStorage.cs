using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using DSLFactory.Candle.SystemModel.Repository;

namespace DSLFactory.Candle.SystemModel.Config.Web
{
    /// <summary>
    /// Stockage des données de config dans le context HTTP
    /// </summary>
    public class WebRepositorySettingsStorage : IRepositorySettingsStorage
    {
        private string _repositoryPath;
        private string _modelsFolder;
        private bool _generationTraceEnabled;
        private int _repositoryDelaiCache;
        private string _licenseId;

        /// <summary>
        /// Gets or sets the repository models folder.
        /// </summary>
        /// <value>The repository models folder.</value>
        public string RepositoryModelsFolder
        {
            get
            {
                if (_modelsFolder == null)
                {
                    string key = "repositoryModelsFolder";
                    string tmp = ConfigurationManager.AppSettings[key];
                    if (!String.IsNullOrEmpty(tmp))
                        _modelsFolder = tmp;
                    else
                        _modelsFolder = Path.Combine(BaseDirectory, RepositoryCategory.Models.ToString());
                }
                return _modelsFolder;
            }
            set { _modelsFolder = value; }
        }

        /// <summary>
        /// Gets or sets the base directory.
        /// </summary>
        /// <value>The base directory.</value>
        public string BaseDirectory
        {
            get
            {
                if (_repositoryPath == null)
                {
                    _repositoryPath = ConfigurationManager.AppSettings["repositoryPath"];
                    if (String.IsNullOrEmpty(_repositoryPath))
                        _repositoryPath = Path.Combine(System.Web.HttpContext.Current.Request.PhysicalApplicationPath, "App_Data");
                }
                return _repositoryPath;
            }
            set { _repositoryPath = value; }
        }

        /// <summary>
        /// Gets the repository URL.
        /// </summary>
        /// <value>The repository URL.</value>
        public string RepositoryUrl
        {
            get { return null; }
            set { }
        }

        /// <summary>
        /// Gets or sets the global servers list.
        /// </summary>
        /// <value>The global servers.</value>
        public List<string> GlobalServers
        {
            get { return null; }
            set { }
        }

        /// <summary>
        /// Gets the configuration file path.
        /// </summary>
        /// <param name="category">The category.</param>
        /// <param name="path">The path.</param>
        /// <returns></returns>
        public string GetConfigurationFilePath(RepositoryCategory category, string path)
        {
            return RepositoryManager.ResolvePath(category, path);
        }

        #region IRepositorySettingsStorage Members

        /// <summary>
        /// Gets the license id.
        /// </summary>
        /// <value>The license id.</value>
        public string LicenseId
        {
            get { return _licenseId; }
            set { _licenseId = value; }
        }

        /// <summary>
        /// Gets a value indicating whether [generation trace enabled].
        /// </summary>
        /// <value>
        /// 	<c>true</c> if [generation trace enabled]; otherwise, <c>false</c>.
        /// </value>
        public bool GenerationTraceEnabled
        {
            get { return _generationTraceEnabled; }
            set { _generationTraceEnabled = value; }
        }

        /// <summary>
        /// Gets the current domain id.
        /// </summary>
        /// <value>The current domain id.</value>
        public string CurrentDomainId
        {
            get { return String.Empty; }
        }

        /// <summary>
        /// Gets the repository delai cache.
        /// </summary>
        /// <value>The repository delai cache.</value>
        public int RepositoryDelaiCache
        {
            get { return _repositoryDelaiCache; }
            set { _repositoryDelaiCache = value; }
        }

        /// <summary>
        /// Gets a value indicating whether [use default proxy].
        /// </summary>
        /// <value><c>true</c> if [use default proxy]; otherwise, <c>false</c>.</value>
        public bool UseDefaultProxy
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        /// <summary>
        /// Gets the proxy address.
        /// </summary>
        /// <value>The proxy address.</value>
        public string ProxyAddress
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        /// <summary>
        /// Gets the proxy user.
        /// </summary>
        /// <value>The proxy user.</value>
        public string ProxyUser
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        /// <summary>
        /// Gets the proxy password.
        /// </summary>
        /// <value>The proxy password.</value>
        public string ProxyPassword
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        #endregion
    }

}
