using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using DSLFactory.Candle.SystemModel.Repository;

namespace DSLFactory.Candle.SystemModel.Configuration
{       
    /// <summary>
    /// Classe helper pour accéder aux données de config
    /// </summary>
    public class CandleSettings
    {
        /// <summary>
        /// Version de candle
        /// </summary>
        public const string Version = "0.9.0.0";

        /// <summary>
        /// Attention : Ne doit être appelé que par RepositoryManager.StrategiesFolder
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        internal static string GetFolderPath( RepositoryCategory category )
        {
            if( category == RepositoryCategory.Models )
                return RepositoryModelsLocalFolder;

            return Path.Combine( BaseDirectory, category.ToString() );
        }

        /// <summary>
        /// URL du repository global
        /// </summary>
        /// <returns></returns>
        public static string RepositoryUrl
        {
            get { return ServiceLocator.Instance.RepositorySettingsStorage.RepositoryUrl;}
        }

        /// <summary>
        /// Gets the global servers.
        /// </summary>
        /// <value>The global servers.</value>
        public static IList<string> GlobalServers
        {
            get { return ServiceLocator.Instance.RepositorySettingsStorage.GlobalServers; }
        }

        /// <summary>
        /// Gets the license id.
        /// </summary>
        /// <value>The license id.</value>
        public static string LicenseId
        {
            get { return "08"; /* ServiceLocator.Instance.RepositorySettingsStorage.LicenseId; */}
        }

        /// <summary>
        /// Gets the base directory.
        /// </summary>
        /// <value>The base directory.</value>
        public static string BaseDirectory
        {
            get { return ServiceLocator.Instance.RepositorySettingsStorage.BaseDirectory; }
        }

        /// <summary>
        /// Gets a value indicating whether [generation trace enabled].
        /// </summary>
        /// <value>
        /// 	<c>true</c> if [generation trace enabled]; otherwise, <c>false</c>.
        /// </value>
        public static bool GenerationTraceEnabled
        {
            get { return ServiceLocator.Instance.RepositorySettingsStorage.GenerationTraceEnabled; }
        }

        /// <summary>
        /// Gets the current domain id.
        /// </summary>
        /// <value>The current domain id.</value>
        public static String CurrentDomainId
        {
            get { return ServiceLocator.Instance.RepositorySettingsStorage.CurrentDomainId; }
        }

        /// <summary>
        /// Gets the repository delai cache.
        /// </summary>
        /// <value>The repository delai cache.</value>
        public static int RepositoryDelaiCache
        {
            get { return ServiceLocator.Instance.RepositorySettingsStorage.RepositoryDelaiCache; }
        }

        /// <summary>
        /// Répertoire du repository local
        /// </summary>
        internal static string RepositoryModelsLocalFolder
        {
            get {
                return ServiceLocator.Instance.RepositorySettingsStorage.RepositoryModelsFolder; 
            }
        }

        /// <summary>
        /// Calcule le chemin d'accés d'un template
        /// </summary>
        /// <param name="templateName">Nom du template (sans l'extension)</param>
        /// <returns>Path local du template</returns>
        public static string GetT4TemplateFileName( string templateName )
        {
            if( !Utils.StringCompareEquals( System.IO.Path.GetExtension( templateName ), ".t4" ) )
                templateName += ".t4";
            return Path.Combine( GetFolderPath( RepositoryCategory.T4Templates ), templateName );
        }

        /// <summary>
        /// Gets the proxy.
        /// </summary>
        /// <returns></returns>
        public IWebProxy GetProxy()
        {
            IRepositorySettingsStorage settingsStorage = ServiceLocator.Instance.RepositorySettingsStorage;
            if (settingsStorage.UseDefaultProxy)
                return WebProxy.GetDefaultProxy();

            if (!String.IsNullOrEmpty(settingsStorage.ProxyAddress))
            {
                Uri uri = new Uri(settingsStorage.ProxyAddress);
                IWebProxy proxy = new WebProxy( uri );
                if (!String.IsNullOrEmpty(settingsStorage.ProxyUser) && !String.IsNullOrEmpty(settingsStorage.ProxyPassword))
                    proxy.Credentials = new NetworkCredential(settingsStorage.ProxyUser, settingsStorage.ProxyPassword);
                return proxy;
            }
            return null;
        }

        /// <summary>
        /// Teste si le cache à expiré
        /// </summary>
        /// <param name="currentDateTime">The current date time.</param>
        /// <returns></returns>
        internal static bool CacheExpired(DateTime? currentDateTime)
        {
            if( !currentDateTime.HasValue )
                return true;
            if (RepositoryDelaiCache == 0)
                return true;
            if (RepositoryDelaiCache < 0)
                return false;

            TimeSpan timeInCache = DateTime.Now - currentDateTime.Value;
            return timeInCache.TotalMinutes > CandleSettings.RepositoryDelaiCache;
        }
    }
}
