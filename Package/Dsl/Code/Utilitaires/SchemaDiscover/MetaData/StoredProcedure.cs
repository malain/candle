using System.Collections.Generic;

namespace DSLFactory.Candle.SystemModel.Utilities.SchemaDiscover
{
    /// <summary>
    /// 
    /// </summary>
    public class DbStoredProcedure : DbContainer
    {
        /// <summary>
        /// 
        /// </summary>
        public List<DbParameter> Parameters = new List<DbParameter>();
    }
}
