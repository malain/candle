using System.Collections.Generic;

namespace DSLFactory.Candle.SystemModel.Utilities.SchemaDiscover
{
    /// <summary>
    /// 
    /// </summary>
    public class DbTable : DbContainer 
    {
        /// <summary>
        /// Liste des indexs
        /// </summary>
        public List<DbIndex> Indexes;

        /// <summary>
        /// Get all the primary key columns
        /// </summary>
        public IEnumerator<DbColumn> PrimaryKeys
        {
            get
            {
                if (Columns != null)
                {
                    foreach (DbColumn col in Columns)
                    {
                        yield return col;
                    }
                }
            }
        }
    }
}
