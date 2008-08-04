using System;
using System.Collections.Generic;
using System.IO;
using System.Web.Services;
using DSLFactory.Candle.SystemModel.Repository;
using DSLFactory.Candle.SystemModel.Repository.Providers;
using DSLFactory.Candle.SystemModel.Strategies;
using System.Xml.Serialization;
using DSLFactory.Candle.SystemModel.Configuration;
using DSLFactory.Candle.SystemModel;
using DSLFactory.Candle.Repository;

namespace Candle.Repository
{

    /// <summary>
    /// Summary description for Files
    /// </summary>
    [WebService(Namespace = DSLFactory.Candle.SystemModel.ModelConstants.ApplicationUriNamespace)]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class RepositoryFileService : WebService
    {
        [WebMethod]
        public void CreateDomainPath(string path)
        {
            DomainManager.Instance.CreateDomainPath(path);
            CandleRepositoryController.Instance.NotifyAction(Context.User, Context.Request["id"], "CreateDomainPath", path);
        }

        [WebMethod]
        public void RemoveDomainPath(string path)
        {
            DomainManager.Instance.RemoveDomainPath(path);
            CandleRepositoryController.Instance.NotifyAction(Context.User, Context.Request["id"], "RemoveDomainPath", path);
        }

        [WebMethod]
        public Version GetVersion()
        {
            CandleRepositoryController.Instance.NotifyAction(Context.User, Context.Request["id"], "GetVersion");
            return new Version(1, 0);
        }

        [WebMethod]
        public ComponentModelMetadata GetMetadata(Guid modelId, VersionInfo version)
        {
            CandleRepositoryController.Instance.NotifyAction(Context.User, Context.Request["id"], "GetMetadata", modelId, version);
            FileRepositoryProvider provider = new FileRepositoryProvider(CandleSettings.BaseDirectory, RepositoryManager.GetFolderPath(RepositoryCategory.Models));
            return provider.GetModelMetadata(modelId, version);
        }

        [WebMethod]
        public List<ComponentModelMetadata> GetAllMetadata()
        {
            CandleRepositoryController.Instance.NotifyAction(Context.User, Context.Request["id"], "GetAllMetadata");
            FileRepositoryProvider provider = new FileRepositoryProvider(CandleSettings.BaseDirectory, RepositoryManager.GetFolderPath(RepositoryCategory.Models));
            return provider.GetAllMetadata();
        }

        [WebMethod]
        public List<RepositoryFileInfo> EnumerateCategory(RepositoryCategory category, string filter, bool recursive)
        {
            CandleRepositoryController.Instance.NotifyAction(Context.User, Context.Request["id"], "EnumerateCategory", category, filter, recursive);
            DirectoryInfo di = new DirectoryInfo(RepositoryManager.GetFolderPath(category));
            List<RepositoryFileInfo> results = FileRepositoryProvider.EnumerateRecursive(di.FullName.Length + 1, di, category, filter, recursive);
            results.Sort();
            return results;
        }
    }
}
