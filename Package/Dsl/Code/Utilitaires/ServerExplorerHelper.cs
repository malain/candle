using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using DSLFactory.Candle.SystemModel.Configuration;
using DSLFactory.Candle.SystemModel.Utilities.SchemaDiscover;

namespace DSLFactory.Candle.SystemModel.Utilities
{
    /// <summary>
    /// Interface permettant de manipuler les objets déplacés à partir de l'explorateur de serveur
    /// </summary>
    public interface IDSRefNavigator : IDisposable
    {
        /// <summary>
        /// Gets a value indicating whether [contains only stored procedures].
        /// </summary>
        /// <value>
        /// 	<c>true</c> if [contains only stored procedures]; otherwise, <c>false</c>.
        /// </value>
        bool ContainsOnlyStoredProcedures { get; }
        /// <summary>
        /// Gets a value indicating whether [contains only tables].
        /// </summary>
        /// <value><c>true</c> if [contains only tables]; otherwise, <c>false</c>.</value>
        bool ContainsOnlyTables { get; }
        /// <summary>
        /// Gets the data connection.
        /// </summary>
        /// <value>The data connection.</value>
        IDbConnection DataConnection { get; }
        /// <summary>
        /// Gets the tables.
        /// </summary>
        /// <param name="serviceProvider">The service provider.</param>
        /// <returns></returns>
        IEnumerable<DbTable> GetTables(IServiceProvider serviceProvider);
        /// <summary>
        /// Gets the stored procedures.
        /// </summary>
        /// <param name="serviceProvider">The service provider.</param>
        /// <returns></returns>
        IEnumerable<DbContainer> GetStoredProcedures(IServiceProvider serviceProvider);
    }

    /// <summary>
    /// Classe utilitaire permettant de gérer le glisser/déplacer à partir de 
    /// l'explorateur de serveurs
    /// </summary>
    public class ServerExplorerHelper
    {
        /// <summary>
        /// Signature de l'objet data du drag'n drop
        /// </summary>
        public static readonly string DataSourceReferenceFormat = "CF_DSREF";

        private static Type s_dsRefNavigatorType;

        /// <summary>
        /// Importation des tables de la base de données
        /// </summary>
        /// <param name="parentElement">The parent element.</param>
        /// <param name="serviceProvider">The service provider.</param>
        /// <param name="dataObject">The data object.</param>
        /// <returns></returns>
        public static bool ImportTables(Package parentElement, IServiceProvider serviceProvider, IDataObject dataObject)
        {
            List<DbContainer> tables = new List<DbContainer>();
            IDSRefNavigator navigator = null;
            try
            {
                navigator = GetDsRefNavigatorInstance(dataObject);
                if (navigator != null)
                {
                    foreach (DbTable table in navigator.GetTables(serviceProvider))
                    {
                        tables.Add(table);
                    }
                    ServiceLocator.Instance.DatabaseImporter.Import(navigator.DataConnection, parentElement, tables,
                                                                    DatabaseType.Table);
                }
            }
            finally
            {
                if (navigator != null)
                    navigator.Dispose();
            }
            return tables.Count > 0;
        }

        /// <summary>
        /// Imports the stored procedures.
        /// </summary>
        /// <param name="parentElement">The parent element.</param>
        /// <param name="serviceProvider">The service provider.</param>
        /// <param name="dataObject">The data object.</param>
        /// <returns></returns>
        public static bool ImportStoredProcedures(Package parentElement, IServiceProvider serviceProvider,
                                                  IDataObject dataObject)
        {
            List<DbContainer> procedures = new List<DbContainer>();
            IDSRefNavigator navigator = null;
            try
            {
                navigator = GetDsRefNavigatorInstance(dataObject);
                if (navigator != null)
                {
                    foreach (DbContainer proc in navigator.GetStoredProcedures(serviceProvider))
                    {
                        procedures.Add(proc);
                    }
                    ServiceLocator.Instance.DatabaseImporter.Import(navigator.DataConnection, parentElement, procedures,
                                                                    DatabaseType.StoredProcedure);
                }
            }
            finally
            {
                if (navigator != null)
                    navigator.Dispose();
            }
            return procedures.Count > 0;
        }

        /// <summary>
        /// Indique si le dataObject contient une ou des références de table
        /// </summary>
        /// <param name="dataObject">The data object.</param>
        /// <returns>
        /// 	<c>true</c> if [contains stored procedures] [the specified data object]; otherwise, <c>false</c>.
        /// </returns>
        public static bool ContainsStoredProcedures(IDataObject dataObject)
        {
            IDSRefNavigator navigator = null;
            try
            {
                navigator = GetDsRefNavigatorInstance(dataObject);

                if (navigator != null && navigator.ContainsOnlyStoredProcedures)
                    return true;
            }
            finally
            {
                if (navigator != null)
                    navigator.Dispose();
            }
            return false;
        }

        /// <summary>
        /// Indique si le dataObject contient une ou des références de table
        /// </summary>
        /// <param name="dataObject">The data object.</param>
        /// <returns>
        /// 	<c>true</c> if the specified data object contains table; otherwise, <c>false</c>.
        /// </returns>
        public static bool ContainsTable(IDataObject dataObject)
        {
            IDSRefNavigator navigator = null;
            try
            {
                navigator = GetDsRefNavigatorInstance(dataObject);

                if (navigator != null && navigator.ContainsOnlyTables)
                    return true;
            }
            finally
            {
                if (navigator != null)
                    navigator.Dispose();
            }
            return false;
        }

        // Bidouille pour faire une référence dynamique sur le DsRefNavigator car si on fait une référence statique le site web du repository
        // plante car la dll Microsoft.VisualStudio.Data n'est que partiellement signée. 
        // TODO : A revoir quand la dll aura été correctement signée
        /// <summary>
        /// Gets the ds ref navigator instance.
        /// </summary>
        /// <param name="dataObject">The data object.</param>
        /// <returns></returns>
        private static IDSRefNavigator GetDsRefNavigatorInstance(IDataObject dataObject)
        {
            if (s_dsRefNavigatorType == null)
            {
                s_dsRefNavigatorType =
                    Type.GetType(
                        "DSLFactory.Candle.SystemModel.ServerExplorer.Utils.DSRefNavigator, DSLFactory.Candle.SystemModel.ServerExplorer.Utils, Version=" +
                        CandleSettings.Version + ", Culture=neutral, PublicKeyToken=13017b9653baedb6");
            }
            if (s_dsRefNavigatorType != null)
            {
                try
                {
                    // Create a navigator for the DSRef Consumer (and dispose it when finished)
                    object[] parms = new object[1];
                    parms[0] = dataObject.GetData(DataSourceReferenceFormat);
                    return Activator.CreateInstance(s_dsRefNavigatorType, parms) as IDSRefNavigator;
                }
                catch
                {
                }
            }
            return null;
        }
    }
}