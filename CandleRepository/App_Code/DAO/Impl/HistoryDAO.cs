using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Data.SqlClient;
using DSLFactory.Candle.SystemModel;
using System.Collections.Generic;

namespace DSLFactory.Candle.Repository
{
    /// <summary>
    /// DAO pour SQL Server
    /// </summary>
    public class SQLCandleRepositoryDAO : ICandleRepositoryDAO
    {

        private const string LAST_UPLOAD = "select UserName, LicenseId, ModelId, Version, Date from Candle_UploadLog order by date desc";
        private const string HISTORIC_UPLOAD = "select UserName, LicenseId, ModelId, Version, Date from Candle_UploadLog  where modelId=@modelId and version=@version";
        private const string INSERT_UPLOAD = "INSERT INTO Candle_UploadLog (UserName, LicenseId, ModelId, Version, Date) VALUES(@userName, @licenseId, @modelId, @version, @date)";

        private const string INSERT_DOWNLOAD = "INSERT INTO Candle_DownloadLog (UserName, LicenseId, Category, FileName, Counter) VALUES(@userName, @licenseId, @category, @fileName, 1)";
        private const string UPDATE_DOWNLOAD = "UPDATE Candle_DownloadLog set Counter=Counter+1 where UserName=@userName and LicenseId=@licenseId and Category = @category and FileName=@fileName";

        #region IHistoryDAO Members

        /// <summary>
        /// Historique des publications de modèles
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="licenseId"></param>
        /// <param name="fileName"></param>
        public void WriteUploadModelLog(string userName, string licenseId, string fileName)
        {
            userName = userName == null ? "anonymous" : userName.ToLower();

            try
            {
                string[] parts = fileName.Split(System.IO.Path.DirectorySeparatorChar);
                Guid modelId = new Guid(parts[0]);
                string version = parts[1];
                SqlCommand cmd = new SqlCommand(INSERT_UPLOAD);
                AddCommonParameters(cmd, userName, licenseId);

                SqlParameter parm = cmd.CreateParameter();
                parm.ParameterName = "@modelId";
                parm.SqlDbType = SqlDbType.UniqueIdentifier;
                parm.Value = modelId;
                cmd.Parameters.Add(parm);

                parm = cmd.CreateParameter();
                parm.ParameterName = "@version";
                parm.SqlDbType = SqlDbType.NVarChar;
                parm.Value = version.ToString();
                cmd.Parameters.Add(parm);

                parm = cmd.CreateParameter();
                parm.ParameterName = "@date";
                parm.SqlDbType = SqlDbType.DateTime;
                parm.Value = DateTime.Now;
                cmd.Parameters.Add(parm);
                ExecuteNonQuery(cmd);
            }
            catch (Exception ex)
            {
                ILogger logger = ServiceLocator.Instance.GetService<ILogger>();
                if (logger != null)
                    logger.WriteError("WriteUploadModelLog", String.Format("UserName={0}, fileName={1}", userName, fileName), ex);
            }
        }

        /// <summary>
        /// Incrémente le nombre de fois qu'un fichier est téléchargé
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="licenseId"></param>
        /// <param name="category"></param>
        /// <param name="fileName"></param>
        public void IncrementDownloadFileCounter(string userName, string licenseId, DSLFactory.Candle.SystemModel.Repository.RepositoryCategory category, string fileName)
        {
            fileName = fileName.ToLower();
            userName = userName == null ? "anonymous" : userName.ToLower();

            try
            {
                SqlCommand cmd = new SqlCommand(UPDATE_DOWNLOAD);
                AddCommonParameters(cmd, userName, licenseId);

                SqlParameter parm = cmd.CreateParameter();
                parm.ParameterName = "@category";
                parm.SqlDbType = SqlDbType.Int;
                parm.Value = (int)category;
                cmd.Parameters.Add(parm);

                parm = cmd.CreateParameter();
                parm.ParameterName = "@fileName";
                parm.SqlDbType = SqlDbType.NVarChar;
                parm.Value = fileName;
                cmd.Parameters.Add(parm);

                if (ExecuteNonQuery(cmd) == 0)
                {
                    cmd = new SqlCommand(INSERT_DOWNLOAD);
                    AddCommonParameters(cmd, userName, licenseId);
                    parm = cmd.CreateParameter();
                    parm.ParameterName = "@category";
                    parm.SqlDbType = SqlDbType.Int;
                    parm.Value = (int)category;
                    cmd.Parameters.Add(parm);

                    parm = cmd.CreateParameter();
                    parm.ParameterName = "@fileName";
                    parm.SqlDbType = SqlDbType.NVarChar;
                    parm.Value = fileName;
                    cmd.Parameters.Add(parm);

                    ExecuteNonQuery(cmd);
                }
            }
            catch (Exception ex)
            {
                ILogger logger = ServiceLocator.Instance.GetService<ILogger>();
                if (logger != null)
                    logger.WriteError("IncrementDownloadFileCounter", String.Format("UserName={0}, fileName={1}", userName, fileName), ex);
            }
        }

        /// <summary>
        /// Execute 
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        private int ExecuteNonQuery(SqlCommand cmd)
        {
            using (SqlConnection cnx = new SqlConnection(ConfigurationManager.ConnectionStrings["candle"].ConnectionString))
            {
                cnx.Open();
                cmd.Connection = cnx;
                return cmd.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="userName"></param>
        /// <param name="licenseId"></param>
        private void AddCommonParameters(SqlCommand cmd, string userName, string licenseId)
        {
            SqlParameter parm = cmd.CreateParameter();
            parm.ParameterName = "@userName";
            parm.Value = userName;
            parm.SqlDbType = SqlDbType.NChar;
            cmd.Parameters.Add(parm);

            parm = cmd.CreateParameter();
            parm.ParameterName = "@licenseId";
            parm.SqlDbType = SqlDbType.NChar;
            parm.Value = licenseId ?? "??";
            cmd.Parameters.Add(parm);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="nb"></param>
        /// <returns></returns>
        public System.Collections.Generic.List<HistoryEntry> GetLastUpload(int nb)
        {
            SqlCommand cmd = new SqlCommand(LAST_UPLOAD);
            return PopulateHistoryEntry(cmd, nb);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="modelId"></param>
        /// <param name="version"></param>
        /// <returns></returns>
        public List<HistoryEntry> GetModelHistoric(Guid modelId, DSLFactory.Candle.SystemModel.VersionInfo version)
        {
            SqlCommand cmd = new SqlCommand(HISTORIC_UPLOAD);

            SqlParameter parm = cmd.CreateParameter();
            parm.ParameterName = "@modelId";
            parm.SqlDbType = SqlDbType.UniqueIdentifier;
            parm.Value = modelId;
            cmd.Parameters.Add(parm);

            parm = cmd.CreateParameter();
            parm.ParameterName = "@version";
            parm.SqlDbType = SqlDbType.NVarChar;
            parm.Value = version.ToString();
            cmd.Parameters.Add(parm);

            return PopulateHistoryEntry(cmd, Int32.MaxValue);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cmd"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        private List<HistoryEntry> PopulateHistoryEntry(SqlCommand cmd, int max)
        {
            List<HistoryEntry> list = new List<HistoryEntry>();
            ILogger logger = ServiceLocator.Instance.GetService<ILogger>();
            try
            {
                int cx = 0;
                using (SqlConnection cnx = new SqlConnection(ConfigurationManager.ConnectionStrings["candle"].ConnectionString))
                {
                    if (logger != null)
                        logger.Write("PopulateHistoryEntry", cmd.CommandText, LogType.Info);

                    cnx.Open();
                    cmd.Connection = cnx;
                    foreach (System.Data.Common.DbDataRecord reader in cmd.ExecuteReader(CommandBehavior.SingleRow))
                    {
                        if (cx++ == max)
                            break;
                        HistoryEntry entry = new HistoryEntry();
                        // Pas de champs nullables
                        entry.UserName = reader["userName"] as String;
                        entry.Category = DSLFactory.Candle.SystemModel.Repository.RepositoryCategory.Models;
                        entry.Date = (DateTime)reader["date"];
                        entry.ModelId = (Guid)reader["ModelId"];
                        entry.License = reader["licenseId"] as string;
                        entry.Version = VersionInfo.TryParse(reader["version"] as string);
                        list.Add(entry);
                    }
                }

                    if (logger != null)
                        logger.Write("PopulateHistoryEntry", cx.ToString(), LogType.Info);
            }
            catch (Exception ex)
            {
                if (logger != null)
                    logger.WriteError("PopulateHistoryEntry", "reading", ex);
            }
            return list;
        }
        #endregion
    }
}