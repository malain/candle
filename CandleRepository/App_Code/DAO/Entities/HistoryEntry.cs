using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using DSLFactory.Candle.SystemModel.Repository;
using DSLFactory.Candle.SystemModel;

namespace DSLFactory.Candle.Repository
{

    /// <summary>
    /// Summary description for HistoryEntry
    /// </summary>
    public class HistoryEntry
    {
        private string userName;
        private DateTime date;
        private RepositoryCategory category;
        private string fileName;
        private VersionInfo version;
        private string licenseId;
        private Guid modelId;
        private string modelName;

        public Guid ModelId
        {
            get { return modelId; }
            set { modelId = value; }
        }

        public string ModelName
        {
            get { return modelName; }
            set { modelName = value; }
        }

        public string License
        {
            get { return licenseId; }
            set { licenseId = value; }
        }

        public VersionInfo Version
        {
            get { return version; }
            set { version = value; }
        }

        public string FileName
        {
            get { return fileName; }
            set { fileName = value; }
        }

        public RepositoryCategory Category
        {
            get { return category; }
            set { category = value; }
        }

        public DateTime Date
        {
            get { return date; }
            set { date = value; }
        }

        public string UserName
        {
            get { return userName; }
            set { userName = value; }
        }
    }
}