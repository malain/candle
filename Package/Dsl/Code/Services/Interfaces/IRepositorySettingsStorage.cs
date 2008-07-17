using System;
using System.Collections.Generic;
using DSLFactory.Candle.SystemModel.Repository;

namespace DSLFactory.Candle.SystemModel
{
    /// <summary>
    /// Service permettant d'accéder aux paramètres de configuration du référentiel
    /// </summary>
    public interface IRepositorySettingsStorage
    {
        /// <summary>
        /// Gets or sets the repository models folder.
        /// </summary>
        /// <value>The repository models folder.</value>
        string RepositoryModelsFolder { get; set;}
        /// <summary>
        /// Gets or sets the base directory.
        /// </summary>
        /// <value>The base directory.</value>
        string BaseDirectory { get; set;}
        /// <summary>
        /// Gets the configuration file path.
        /// </summary>
        /// <param name="category">The category.</param>
        /// <param name="path">The path.</param>
        /// <returns></returns>
        string GetConfigurationFilePath(RepositoryCategory category, string path);
        /// <summary>
        /// Gets the repository URL.
        /// </summary>
        /// <value>The repository URL.</value>
        string RepositoryUrl { get;}
        /// <summary>
        /// Gets or sets the global servers list.
        /// </summary>
        /// <value>The global servers.</value>
        List<string> GlobalServers { get;set;}
        /// <summary>
        /// Gets the license id.
        /// </summary>
        /// <value>The license id.</value>
        string LicenseId { get;}
        /// <summary>
        /// Gets a value indicating whether [generation trace enabled].
        /// </summary>
        /// <value>
        /// 	<c>true</c> if [generation trace enabled]; otherwise, <c>false</c>.
        /// </value>
        bool GenerationTraceEnabled {get;}
        /// <summary>
        /// Gets the current domain id.
        /// </summary>
        /// <value>The current domain id.</value>
        String CurrentDomainId {get;}
        /// <summary>
        /// Gets the repository delai cache.
        /// </summary>
        /// <value>The repository delai cache.</value>
        int RepositoryDelaiCache { get;}
        /// <summary>
        /// Gets a value indicating whether [use default proxy].
        /// </summary>
        /// <value><c>true</c> if [use default proxy]; otherwise, <c>false</c>.</value>
        bool UseDefaultProxy { get;}
        /// <summary>
        /// Gets the proxy address.
        /// </summary>
        /// <value>The proxy address.</value>
        string ProxyAddress { get;}
        /// <summary>
        /// Gets the proxy user.
        /// </summary>
        /// <value>The proxy user.</value>
        string ProxyUser { get;}
        /// <summary>
        /// Gets the proxy password.
        /// </summary>
        /// <value>The proxy password.</value>
        string ProxyPassword { get;}
    }
}
