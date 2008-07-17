using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace DSLFactory.Candle.SystemModel.Utilities.SchemaDiscover
{
    /// <summary>
    /// 
    /// </summary>
    internal class SQLServerSchemaDiscover : GenericSchemaDiscover
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SQLServerSchemaDiscover"/> class.
        /// </summary>
        /// <param name="connection">The connection.</param>
        public SQLServerSchemaDiscover(IDbConnection connection)
            : base(connection)
        {
        }

        /// <summary>
        /// Liste des index d'une table
        /// </summary>
        /// <param name="table">The table.</param>
        /// <returns></returns>
        public override List<DbIndex> GetIndexes(DbTable table)
        {
            List<DbIndex> results = new List<DbIndex>();
            string[] restrictions = new string[4];
            restrictions[1] = table.Owner;
            restrictions[2] = table.Name;
            DataTable schema =
                ((SqlConnection) connection).GetSchema(SqlClientMetaDataCollectionNames.IndexColumns, restrictions);
            foreach (DataRow row in schema.Rows)
            {
                string indexName = row["index_name"].ToString();
                DbIndex ind = results.Find(delegate(DbIndex i) { return i.Name == indexName; });
                if (ind == null)
                {
                    ind = new DbIndex();
                    ind.Name = indexName;
                    results.Add(ind);
                }
                ind.Columns.Add(table.FindColumn(row["column_name"].ToString()));
            }
            return results;
        }

        /// <summary>
        /// Liste des relations à partir et vers une table
        /// </summary>
        /// <param name="table">The table.</param>
        /// <returns></returns>
        public override List<DbRelationShip> GetRelations(DbTable table)
        {
            List<DbRelationShip> results = new List<DbRelationShip>();

            SqlCommand command = new SqlCommand("sp_fkeys", (SqlConnection) connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.Add("@pktable_name", SqlDbType.NVarChar, 128).Value = null;
            command.Parameters.Add("@pktable_owner ", SqlDbType.NVarChar, 128).Value = null;
            command.Parameters.Add("@fktable_name", SqlDbType.NVarChar, 128).Value = table.Name;
            command.Parameters.Add("@fktable_owner", SqlDbType.NVarChar, 128).Value = table.Owner;
            using (SqlDataReader reader = command.ExecuteReader())
            {
                PopulateRelationShips(results, reader);
            }

            command.Parameters.Clear();
            command.Parameters.Add("@pktable_name", SqlDbType.NVarChar, 128).Value = table.Name;
            command.Parameters.Add("@pktable_owner ", SqlDbType.NVarChar, 128).Value = table.Owner;
            command.Parameters.Add("@fktable_name", SqlDbType.NVarChar, 128).Value = null;
            command.Parameters.Add("@fktable_owner", SqlDbType.NVarChar, 128).Value = null;
            using (SqlDataReader reader = command.ExecuteReader())
            {
                PopulateRelationShips(results, reader);
            }

            return results;
        }

        /// <summary>
        /// Populates the relation ships.
        /// </summary>
        /// <param name="results">The results.</param>
        /// <param name="reader">The reader.</param>
        private static void PopulateRelationShips(List<DbRelationShip> results, SqlDataReader reader)
        {
            while (reader.Read())
            {
                string constraintName = reader["FK_NAME"].ToString();
                DbRelationShip relation =
                    results.Find(delegate(DbRelationShip fk) { return fk.Name == constraintName; });

                if (relation == null)
                {
                    relation = new DbRelationShip();
                    relation.Name = constraintName;
                    results.Add(relation);
                }

                if (relation.SourceTableName == null)
                {
                    relation.SourceTableName = reader["FKTABLE_NAME"].ToString();
                    relation.SourceTableOwner = reader["FKTABLE_OWNER"].ToString();
                }

                relation.SourceColumnNames.Add(reader["FKCOLUMN_NAME"].ToString());

                if (relation.TargetTableName == null)
                {
                    relation.TargetTableName = reader["PKTABLE_NAME"].ToString();
                    relation.TargetTableOwner = reader["PKTABLE_OWNER"].ToString();
                }

                relation.TargetColumnNames.Add(reader["PKCOLUMN_NAME"].ToString());
            }
        }
    }
}