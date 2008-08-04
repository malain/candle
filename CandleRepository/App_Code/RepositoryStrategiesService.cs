using System;
using System.Web;
using System.Collections;
using System.Web.Services;
using System.Web.Services.Protocols;
using DSLFactory.Candle.SystemModel.Repository;
using DSLFactory.Candle.SystemModel.Strategies;
using System.IO;
using System.Xml.Serialization;
using System.Collections.Generic;
using DSLFactory.Candle.SystemModel;
using DSLFactory.Candle.SystemModel.Repository.Providers;
using DSLFactory.Candle.Repository;

namespace Candle.Repository
{
    /// <summary>
    /// Description résumée de RepositoryStrategiesService
    /// </summary>
    [WebService(Namespace = DSLFactory.Candle.SystemModel.ModelConstants.ApplicationUriNamespace)]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class RepositoryStrategiesService : System.Web.Services.WebService
    {
        [WebMethod]
        public StrategyManifest[] GetStrategyManifests()
        {
            CandleRepositoryController.Instance.NotifyAction(Context.User, Context.Request["id"], "GetStrategyManifests");
            FileRepositoryProvider provider = new FileRepositoryProvider(DSLFactory.Candle.SystemModel.Configuration.CandleSettings.BaseDirectory, RepositoryManager.GetFolderPath(RepositoryCategory.Models));
            return provider.GetStrategyManifests();
        }
    }
}
