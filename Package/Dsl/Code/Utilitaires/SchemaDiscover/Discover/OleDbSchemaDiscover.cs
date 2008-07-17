using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;

namespace DSLFactory.Candle.SystemModel.Utilities.SchemaDiscover
{
    /// <summary>
    /// 
    /// </summary>
    class OleDbSchemaDiscover : GenericSchemaDiscover
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OleDbSchemaDiscover"/> class.
        /// </summary>
        /// <param name="connection">The connection.</param>
        public OleDbSchemaDiscover( IDbConnection connection ) : base(connection)
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
            string[] restrictions = new string[5];
            restrictions[4] = table.Name;
            DataTable schema = ( (OleDbConnection)connection ).GetSchema( "Indexes", restrictions );
            foreach( DataRow row in schema.Rows )
            {
                string indexName = row["index_name"].ToString();
                DbIndex ind = results.Find( delegate( DbIndex i ) { return i.Name == indexName; } );
                if( ind == null )
                {
                    ind = new DbIndex();
                    ind.Name = indexName;
                    results.Add( ind );
                }
                ind.Columns.Add( table.FindColumn( row["column_name"].ToString() ) );
            }
            return results;
        }

        /// <summary>
        /// Liste des relations à partir et vers une table
        /// </summary>
        /// <param name="table">The table.</param>
        /// <returns></returns>
        public override List<DbRelationShip> GetRelations(DbTable table )
        {
            List<DbRelationShip> results = new List<DbRelationShip>();
            // TODO
            //OleDbDataReader reader = null;

            //try
            //{
            //    while( reader.Read() )
            //    {
            //        string constraintName = reader["FK_NAME"].ToString();
            //        RelationShip relation =  results.Find( delegate( RelationShip fk ) { return fk.Name == constraintName; } );
            //        bool addRelation = false;
            //        if( relation == null )
            //        {
            //            addRelation = true;
            //            relation = new RelationShip();
            //            relation.Name = constraintName;
            //            relation.SourceTable = table;
            //  results.Add( relation );
            //        }
            //        relation.SourceColumns.Add( table.FindColumn( reader["FKCOLUMN_NAME"].ToString() ));

            //        relation.TargetTable = tables.Find( delegate( Table currentTable ) { return currentTable.Name == reader["PKTABLE_NAME"].ToString(); } );
            //        if( relation.TargetTable != null )
            //        {
            //            relation.TargetColumns.Add( relation.TargetTable.FindColumn( reader["PKCOLUMN_NAME"].ToString() ) );
            //            
            //        }
            //    }
            //}
            //finally
            //{
            //    reader.Close();
            //}
            return results;
        }

    }
}
