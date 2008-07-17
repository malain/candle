using System;
using System.Collections.Generic;
using DSLFactory.Candle.SystemModel.Configuration.VisualStudio;
using DSLFactory.Candle.SystemModel.Repository;

namespace DSLFactory.Candle.SystemModel.Config
{
    /// <summary>
    /// 
    /// </summary>
    public class IDERepositorySettingsStorage : IRepositorySettingsStorage
    {
        private string _repositoryPath;
        private string _repositoryUrl;
        private string _modelsFolder;
        private List<string> _globalServers;

        /// <summary>
        /// Gets or sets the repository models folder.
        /// </summary>
        /// <value>The repository models folder.</value>
        public string RepositoryModelsFolder
        {
            get { return String.IsNullOrEmpty(_modelsFolder) ? OptionsPage.Instance.RepositoryPath : _modelsFolder; }
            set { _modelsFolder = value; }
        }

        /// <summary>
        /// Gets or sets the base directory.
        /// </summary>
        /// <value>The base directory.</value>
        public string BaseDirectory
        {
            get { return String.IsNullOrEmpty(_repositoryPath) ? OptionsPage.Instance.BaseDirectory : _repositoryPath; }
            set { _repositoryPath = value; }
        }

        /// <summary>
        /// Gets the repository URL.
        /// </summary>
        /// <value>The repository URL.</value>
        public string RepositoryUrl
        {
            get
            {
                if (String.IsNullOrEmpty(_repositoryUrl))
                {
                    if (OptionsPage.Instance.RepositoryEnabled)
                        return OptionsPage.Instance.RepositoryUrl;
                }
                return _repositoryUrl;
            }
            set { _repositoryUrl = value; }
        }

        /// <summary>
        /// Gets or sets the global servers list.
        /// </summary>
        /// <value>The global servers.</value>
        public List<string> GlobalServers
        {
            get { return _globalServers ?? OptionsServersPage.Instance.GlobalServers; }
            set { _globalServers = value; }
        }

        /// <summary>
        /// Gets the configuration file path.
        /// </summary>
        /// <param name="category">The category.</param>
        /// <param name="path">The path.</param>
        /// <returns></returns>
        public string GetConfigurationFilePath(RepositoryCategory category, string path)
        {
            RepositoryFile rfi = new RepositoryFile(category, path);
            return rfi.LocalPhysicalPath;
        }

        #region IRepositorySettingsStorage Members


        /// <summary>
        /// Gets the license id.
        /// </summary>
        /// <value>The license id.</value>
        public string LicenseId
        {
            get { return OptionsPage.Instance.LicenseId; }
        }

        /// <summary>
        /// Gets a value indicating whether [generation trace enabled].
        /// </summary>
        /// <value>
        /// 	<c>true</c> if [generation trace enabled]; otherwise, <c>false</c>.
        /// </value>
        public bool GenerationTraceEnabled
        {
            get { return OptionsPage.Instance.GenerationTraceEnabled; }
        }

        /// <summary>
        /// Gets the current domain id.
        /// </summary>
        /// <value>The current domain id.</value>
        public string CurrentDomainId
        {
            get { return OptionsPage.Instance.CurrentDomainId; }
        }

        /// <summary>
        /// Gets the repository delai cache.
        /// </summary>
        /// <value>The repository delai cache.</value>
        public int RepositoryDelaiCache
        {
            get { return OptionsPage.Instance.RepositoryDelaiCache; }
        }

        /// <summary>
        /// Gets a value indicating whether [use default proxy].
        /// </summary>
        /// <value><c>true</c> if [use default proxy]; otherwise, <c>false</c>.</value>
        public bool UseDefaultProxy
        {
            get { return OptionsServersPage.Instance.UseDefaultProxy; }
        }

        /// <summary>
        /// Gets the proxy address.
        /// </summary>
        /// <value>The proxy address.</value>
        public string ProxyAddress
        {
            get { return OptionsServersPage.Instance.ProxyAddress; }
        }

        /// <summary>
        /// Gets the proxy user.
        /// </summary>
        /// <value>The proxy user.</value>
        public string ProxyUser
        {
            get { return OptionsServersPage.Instance.ProxyUser; }
        }

        /// <summary>
        /// Gets the proxy password.
        /// </summary>
        /// <value>The proxy password.</value>
        public string ProxyPassword
        {
            get { return OptionsServersPage.Instance.ProxyPassword; }
        }

        #endregion
    }
}
