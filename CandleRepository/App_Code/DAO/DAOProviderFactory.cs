using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Reflection;

namespace DSLFactory.Candle.Repository
{
    /// <summary>
    /// Factory de création des providers DAO
    /// </summary>
    internal sealed class DAOProviderFactory
    {
        /// <summary>
        /// Création de l'instance du provider DAO
        /// </summary>
        /// <returns></returns>
        internal static ICandleRepositoryDAO CreateDAOProviderInstance()
        {
            try
            {
                string providerType = ConfigurationManager.AppSettings["CandleRepositoryProvider"];
                if (providerType != null)
                {
                    string assemblyName = null;
                    string typeName = providerType;

                    int pos = providerType.IndexOf(',');
                    if (pos > 0)
                    {
                        assemblyName = providerType.Substring(pos + 1).Trim();
                        typeName = providerType.Substring(0, pos).Trim();
                    }

                    Assembly assembly = null;
                    if (!String.IsNullOrEmpty(assemblyName))
                        assembly = Assembly.LoadWithPartialName(assemblyName);
                    Type type = null;
                    if (assembly == null)
                        type = Type.GetType(typeName);
                    else
                        type = assembly.GetType(typeName, false);

                    if (type != null)
                        return (ICandleRepositoryDAO)Activator.CreateInstance(type);
                }
            }
            catch { }

            return null;
        }
    }
}