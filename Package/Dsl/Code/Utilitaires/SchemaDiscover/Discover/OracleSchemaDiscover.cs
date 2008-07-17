using System.Collections.Generic;
using System.Data;
using System.Data.OracleClient;

namespace DSLFactory.Candle.SystemModel.Utilities.SchemaDiscover
{
    /// <summary>
    /// 
    /// </summary>
    class OracleSchemaDiscover : GenericSchemaDiscover
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OracleSchemaDiscover"/> class.
        /// </summary>
        /// <param name="connection">The connection.</param>
        public OracleSchemaDiscover( IDbConnection connection ) : base( connection )
        {
        }

        /// <summary>
        /// Liste des index d'une table
        /// </summary>
        /// <param name="table">The table.</param>
        /// <returns></returns>
        public override List<DbIndex> GetIndexes( DbTable table )
        {
            List<DbIndex> results = new List<DbIndex>();
            string[] restrictions = new string[4];
            restrictions[2] = table.Owner;
            restrictions[3] = table.Name;
            DataTable indexes = ( (OracleConnection)connection ).GetSchema( "Indexes", restrictions );
            foreach( DataRow row in indexes.Rows )
            {
                string indexName = row["index_name"].ToString();

                DbIndex ind = new DbIndex();
                ind.Name = indexName;
                results.Add( ind );

                restrictions[0] = table.Owner;
                restrictions[1] = indexName;
                DataTable indexColumns = ( (OracleConnection)connection ).GetSchema( "IndexColumns", restrictions );
                foreach( DataRow row2 in indexColumns.Rows )
                {
                    ind.Columns.Add( table.FindColumn( row2["column_name"].ToString() ) );
                }
            }
            return results;
        }


        /// <summary>
        /// Récupére les colonnes d'une table
        /// </summary>
        /// <param name="table">The table.</param>
        /// <returns></returns>
        /// <remarks>
        /// Ne doit pas mettre à jour directement les colonnes dans la table
        /// </remarks>
        public override List<DbColumn> GetColumns( DbTable table )
        {
            List<DbColumn> columns = base.GetColumns( table );

            // On repasse car bug dans getschema de Oracle qui ne renvoit pas toujours les clés primaires
            OracleCommand command = new OracleCommand();
            command.CommandType = CommandType.Text;
            command.CommandText = @"SELECT     BATCH.TABLE_NAME, COL2.COLUMN_NAME as ColName
FROM         SYS.ALL_CONSTRAINTS BATCH, SYS.ALL_CONS_COLUMNS COL2
WHERE     BATCH.CONSTRAINT_NAME = COL2.CONSTRAINT_NAME AND BATCH.OWNER = COL2.OWNER AND (BATCH.TABLE_NAME = :pTable) AND 
                      (BATCH.OWNER = :pOwner) AND (BATCH.CONSTRAINT_TYPE = 'P')";

            command.Connection = connection as OracleConnection;

            OracleParameter parm = new OracleParameter( "pOwner", OracleType.VarChar, 128 );
            parm.Value = table.Owner;
            command.Parameters.Add( parm );
            parm = new OracleParameter( "pTable", OracleType.VarChar, 128 );
            parm.Value = table.Name;
            command.Parameters.Add( parm );
            OracleDataReader reader = command.ExecuteReader();

            try
            {
                while( reader.Read() )
                {
                    string colName = reader["ColName"].ToString();
                    foreach( DbColumn col in columns )
                    {
                        if( col.Name == colName )
                        {
                            col.InPrimaryKey = true;
                            break;
                        }
                    }
                }
                // TODO serverType à vérifier
            }
            finally
            {
                reader.Close();
            }
            return columns;
        }


        /// <summary>
        /// Liste des relations à partir et vers une table
        /// </summary>
        /// <param name="table">The table.</param>
        /// <returns></returns>
        public override List<DbRelationShip> GetRelations(  DbTable table )
        {
            List<DbRelationShip> results = new List<DbRelationShip>();

            OracleCommand command = new OracleCommand();
            command.CommandType = CommandType.Text;
            command.CommandText = @"SELECT BATCH.TABLE_NAME as SourceTable, COL2.COLUMN_NAME AS SourceCol, BATCH.CONSTRAINT_NAME as ConstraintName, REFER.TABLE_NAME AS TargetTable, 
                      BATCH.R_CONSTRAINT_NAME as RefConstraintName, BATCH.OWNER as SourceOwner, REFER.OWNER as TargetOwner
            FROM         SYS.ALL_CONSTRAINTS REFER, SYS.ALL_CONSTRAINTS BATCH, SYS.ALL_CONS_COLUMNS COL2
            WHERE     REFER.CONSTRAINT_NAME = BATCH.R_CONSTRAINT_NAME AND REFER.OWNER = BATCH.R_OWNER AND 
                      BATCH.CONSTRAINT_NAME = COL2.CONSTRAINT_NAME AND BATCH.OWNER = COL2.OWNER AND (BATCH.TABLE_NAME = :pTable) AND 
                      (BATCH.OWNER = :pOwner) AND (BATCH.CONSTRAINT_TYPE = 'R') AND (REFER.CONSTRAINT_TYPE = 'P' OR
                      REFER.CONSTRAINT_TYPE = 'U')";
            command.Connection = connection as OracleConnection;

            OracleParameter parm = new OracleParameter("pOwner", OracleType.VarChar, 128 );
            parm.Value = table.Owner;
            command.Parameters.Add( parm );
            parm = new OracleParameter( "pTable", OracleType.VarChar, 128 );
            parm.Value = table.Name;
            command.Parameters.Add( parm );

            PopulateRelationShips(results, command);

            command.CommandText = @"SELECT BATCH.TABLE_NAME as SourceTable, COL2.COLUMN_NAME AS SourceCol, BATCH.CONSTRAINT_NAME as ConstraintName, REFER.TABLE_NAME AS TargetTable, 
                      BATCH.R_CONSTRAINT_NAME as RefConstraintName, BATCH.OWNER as SourceOwner, REFER.OWNER as TargetOwner
            FROM         SYS.ALL_CONSTRAINTS REFER, SYS.ALL_CONSTRAINTS BATCH, SYS.ALL_CONS_COLUMNS COL2
            WHERE     REFER.CONSTRAINT_NAME = BATCH.R_CONSTRAINT_NAME AND REFER.OWNER = BATCH.R_OWNER AND 
                      BATCH.CONSTRAINT_NAME = COL2.CONSTRAINT_NAME AND BATCH.OWNER = COL2.OWNER AND (REFER.TABLE_NAME = :pTable) AND 
                      (BATCH.OWNER = :pOwner) AND (BATCH.CONSTRAINT_TYPE = 'R') AND (REFER.CONSTRAINT_TYPE = 'P' OR
                      REFER.CONSTRAINT_TYPE = 'U')";

            PopulateRelationShips(results, command);

            return results;
        }

        /// <summary>
        /// Populates the relation ships.
        /// </summary>
        /// <param name="results">The results.</param>
        /// <param name="command">The command.</param>
        private void PopulateRelationShips(List<DbRelationShip> results, OracleCommand command)
        {
            using (OracleDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    string constraintName = reader["ConstraintName"].ToString();
                    DbRelationShip relation = results.Find(delegate(DbRelationShip fk) { return fk.Name == constraintName; });
                    if (relation == null)
                    {
                        relation = new DbRelationShip();
                        relation.Name = constraintName;
                        results.Add(relation);
                    }

                    if (relation.SourceTableName == null)
                    {
                        relation.SourceTableName = reader["SourceTable"].ToString();
                        relation.SourceTableOwner = reader["SourceOwner"].ToString();
                    }

                    relation.SourceColumnNames.Add(reader["SourceCol"].ToString());
                    if (relation.TargetTableName == null)
                    {
                        relation.TargetTableOwner = reader["TargetOwner"].ToString();
                        relation.TargetTableName = reader["TargetTable"].ToString();
                    }
                    RetrieveTargetColumns(connection, relation, reader["RefConstraintName"].ToString());
                }
            }
        }

        /// <summary>
        /// Retrieves the target columns.
        /// </summary>
        /// <param name="connection">The connection.</param>
        /// <param name="relation">The relation.</param>
        /// <param name="refConstraintName">Name of the ref constraint.</param>
        private void RetrieveTargetColumns( IDbConnection connection, DbRelationShip relation, string refConstraintName )
        {
            OracleCommand command = new OracleCommand();
            command.CommandType = CommandType.Text;
            command.CommandText = @"SELECT     BATCH.TABLE_NAME, COL.COLUMN_NAME as ColName
                FROM         SYS.ALL_CONSTRAINTS BATCH, SYS.ALL_CONS_COLUMNS COL
                WHERE     BATCH.CONSTRAINT_NAME = COL.CONSTRAINT_NAME AND BATCH.OWNER = COL.OWNER AND (BATCH.OWNER = :pOwner) AND 
                      (BATCH.CONSTRAINT_TYPE = 'P') AND (BATCH.TABLE_NAME = :pTable) AND (BATCH.CONSTRAINT_NAME = :ConstraintName)";
            command.Connection = connection as OracleConnection;

            OracleParameter parm = new OracleParameter( "pOwner", OracleType.VarChar, 128 );
            parm.Value = relation.TargetTableOwner;
            command.Parameters.Add( parm );
            parm = new OracleParameter( "pTable", OracleType.VarChar, 128 );
            parm.Value = relation.TargetTableName;
            command.Parameters.Add( parm );
            parm = new OracleParameter( "ConstraintName", OracleType.VarChar, 128 );
            parm.Value = refConstraintName;
            command.Parameters.Add( parm );

            OracleDataReader reader = command.ExecuteReader();

            try
            {
                while( reader.Read() )
                {
                    relation.TargetColumnNames.Add( reader["ColName"].ToString() );
                }
            }
            finally
            {
                reader.Close();
            }
        }

    }
}
