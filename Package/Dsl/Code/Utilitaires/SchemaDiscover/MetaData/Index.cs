using System.Collections.Generic;

namespace DSLFactory.Candle.SystemModel.Utilities.SchemaDiscover
{
    /// <summary>
    /// 
    /// </summary>
    public class DbIndex
    {
        /// <summary>
        /// 
        /// </summary>
        public string Name;
        /// <summary>
        /// 
        /// </summary>
        public DbTable Table;
        /// <summary>
        /// 
        /// </summary>
        public List<DbColumn> Columns = new List<DbColumn>();
    }
}
