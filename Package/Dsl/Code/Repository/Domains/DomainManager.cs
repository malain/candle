using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace DSLFactory.Candle.SystemModel.Repository
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    [XmlRoot("domainsConfiguration")]
    public class DomainManager
    {
        /// <summary>
        /// 
        /// </summary>
        public const char PathSeparator = '/';
        private static string s_filePath;
        private static DomainManager s_instance;
        private DomainItemCollection _domains = new DomainItemCollection(null);

        /// <summary>
        /// Singleton
        /// </summary>
        public static DomainManager Instance
        {
            get
            {
                if (s_instance == null)
                {
                    try
                    {
                        RepositoryFile rfi = new RepositoryFile(RepositoryCategory.Configuration, "domains.config");
                        s_filePath = rfi.LocalPhysicalPath;
                        using (StreamReader reader = new StreamReader(s_filePath))
                        {
                            // Désérialization de la description
                            XmlSerializer serializer = new XmlSerializer(typeof (DomainManager));
                            s_instance = (DomainManager) serializer.Deserialize(reader);
                        }
                    }
                    catch (Exception ex)
                    {
                        ILogger logger = ServiceLocator.Instance.GetService<ILogger>();
                        if (logger != null)
                            logger.WriteError("Domain manager", "Unable to read domains file - Create an empty file", ex);
                        s_instance = new DomainManager();
                        s_instance.CreateDomainPath("Misc");
                        s_instance.Save();
                    }

                    // Abonnements aux changements des options
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
        /// Gets or sets the domains.
        /// </summary>
        /// <value>The domains.</value>
        [XmlArray("domains")]
        [XmlArrayItem("domain")]
        public DomainItemCollection Domains
        {
            get { return _domains; }
            set { _domains = value; }
        }

        /// <summary>
        /// Reloads the on change events.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private static void ReloadOnChangeEvents(object sender, EventArgs e)
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
        /// Saves this instance.
        /// </summary>
        private void Save()
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(s_filePath))
                {
                    // Désérialization de la description
                    XmlSerializer serializer = new XmlSerializer(typeof (DomainManager));
                    serializer.Serialize(writer, this);
                }
            }
            catch (Exception ex)
            {
                ILogger logger = ServiceLocator.Instance.GetService<ILogger>();
                if (logger != null)
                    logger.WriteError("Domain Manager", "Error when saving domains file", ex);
            }
        }

        /// <summary>
        /// Gets all paths.
        /// </summary>
        /// <param name="domainFilter">The domain filter.</param>
        /// <returns></returns>
        public List<string> GetAllPaths(string domainFilter)
        {
            List<string> result = new List<string>();
            if (domainFilter == null || domainFilter.Length == 0)
            {
                foreach (DomainItem item in Domains)
                {
                    item.RetrievePaths(result);
                }
            }
            else
            {
                DomainItem domain = Domains.FindItem(domainFilter);
                if (domain != null)
                    domain.RetrievePaths(result);
            }
            return result;
        }

        /// <summary>
        /// Finds the item.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns></returns>
        public DomainItem FindItem(string path)
        {
            return Domains.FindItem(path);
        }

        /// <summary>
        /// Création d'un nouveau path
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns></returns>
        public DomainItem CreateDomainPath(string path)
        {
            if (String.IsNullOrEmpty(path))
                throw new ArgumentException("Invalid path", path);

            DomainItem item = FindItem(path);
            if (item == null)
            {
                item = Domains.CreateItem(path);
                if (item != null)
                {
                    Save();
                }
            }
            return item;
        }

        /// <summary>
        /// Suppression d'un domaine
        /// </summary>
        /// <param name="path">The path.</param>
        public void RemoveDomainPath(string path)
        {
            if (String.IsNullOrEmpty(path))
                throw new ArgumentException("Invalid path", path);

            DomainItem item = FindItem(path);
            if (item == null)
                return;

            item.Delete();
            Save();
        }
    }
}