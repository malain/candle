using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Collections.Generic;
using DSLFactory.Candle.SystemModel;
using DSLFactory.Candle.SystemModel.Repository;
using System.IO;
using DSLFactory.Candle.SystemModel.Repository.Providers;
using DSLFactory.Candle.SystemModel.Configuration;
using System.Reflection;
using DSLFactory.Candle.SystemModel.Dependencies;

namespace DSLFactory.Candle.Repository
{

    /// <summary>
    /// Summary description for CandleEngine
    /// </summary>
    public class CandleRepositoryController
    {
        private ICandleRepositoryDAO _historyDao;
        private RepositoryCache _cache;
        private static CandleRepositoryController _instance = new CandleRepositoryController();

        public CandleRepositoryController()
        {
            HttpContext.Current.Trace.Write(RepositoryManager.GetFolderPath(RepositoryCategory.Models));

            _historyDao = DAOProviderFactory.CreateDAOProviderInstance();
            _cache = new RepositoryCache();
        }

        public static CandleRepositoryController Instance
        {
            get { return _instance; }
        }

        #region Metadata
        public ComponentModelMetadata GetMetadata(string id, string version)
        {
            try
            {
                Guid modelId = new Guid(id);
                Version v = new Version(version);
                VersionInfo vi = new VersionInfo(v);
                return GetMetadata(modelId, vi);
            }
            catch
            {
                return null;
            }
        }

        [System.ComponentModel.DataObjectMethod( System.ComponentModel.DataObjectMethodType.Select)]
        public ComponentModelMetadata GetMetadata(Guid id, VersionInfo version)
        {
            return GetAllMetadata().Find(delegate(ComponentModelMetadata m) { return m.Id == id && version.Equals(m.Version); });
        }

                /// <summary>
        /// Liste des tags avec leurs poids
        /// </summary>
        /// <param name="minWeight"></param>
        /// <param name="maxWeight"></param>
        /// <returns></returns>
        public Dictionary<string, int> GetTaggings(out int minWeight, out int maxWeight)
        {
            return _cache.GetTaggings(out minWeight, out maxWeight);
        }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
        public List<ComponentModelMetadata> GetAllMetadata()
        {
            return _cache.GetAllMetadata();
        }

        [System.ComponentModel.DataObjectMethod(System.ComponentModel.DataObjectMethodType.Fill)]
        public List<ComponentModelMetadata> GetAllSortedMetadata(List<string> tag, string path)
        {
            List<ComponentModelMetadata> list = GetAllMetadata();

            // Si besoin de filtrer, on travaille sur une copie
            if (list != null && (tag != null || path != null))
            {
                // Clone
                list = new List<ComponentModelMetadata>(list);

                // Filtrage
                list.RemoveAll(delegate(ComponentModelMetadata m)
                {
                    if (tag != null)
                    {
                        string[] parts = m.Path.Split(DomainManager.PathSeparator);
                        return !Array.Exists<string>(parts, delegate(string p) { return tag.IndexOf(p)>=0; });
                    }
                    if (path != null)
                        return !m.Path.StartsWith(path);
                    return false;
                });
            }

            if( list!=null)
                list.Sort(delegate(ComponentModelMetadata a, ComponentModelMetadata b) { return String.Concat(a.Path, a.Name).CompareTo(String.Concat(b.Path, b.Name)); });            
        
            return list;
        }

        #endregion

        public List<string> GetArtifacts(ComponentModelMetadata metadata)
        {
            return _cache.GetArtifacts(metadata);
        }

        public List<DependencyGraphVisitor.RelationShip> GetAllRelations()
        {
            return _cache.GetAllRelations();
        }

        public void NotifyUploadModel(System.Security.Principal.IPrincipal principal, string licenseId, string fileName)
        {
            _cache.Clear();
            WriteUploadModelLog(principal, licenseId, fileName);
        }

        public void WriteUploadModelLog(System.Security.Principal.IPrincipal principal, string licenseId, string fileName)
        {
            if( _historyDao!=null)
                _historyDao.WriteUploadModelLog(principal.Identity.Name, licenseId, fileName);
        }

        public void NotifyDownloadFile(System.Security.Principal.IPrincipal principal, string licenseId, RepositoryCategory category, string path)
        {
            if (_historyDao != null)
                _historyDao.IncrementDownloadFileCounter(principal.Identity.Name, licenseId, category, path);
        }

        internal void NotifyAction(System.Security.Principal.IPrincipal principal, string licenseId, string commandName, params object[] args)
        {
        }

        public System.Collections.Generic.List<HistoryEntry> GetLastUpload()
        {
            if (_historyDao != null)
            {
                System.Collections.Generic.List<HistoryEntry> list = _historyDao.GetLastUpload(15);
                if (list != null)
                {
                    foreach (HistoryEntry entry in list)
                    {
                        entry.ModelName = GetMetadata(entry.ModelId, entry.Version).Name;
                    }
                    return list;
                }
            }
            return new List<HistoryEntry>();
        }

        public List<HistoryEntry> GetModelHistoric(Guid modelId, DSLFactory.Candle.SystemModel.VersionInfo version)
        {
            if (_historyDao != null)
                return _historyDao.GetModelHistoric(modelId, version);
            return new List<HistoryEntry>();
        }

        public ServiceLocator ServiceLocator
        {
            get { return ServiceLocator.Instance; }
        }

    }
}