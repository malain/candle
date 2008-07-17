using System.Collections.Generic;
using System.Data;

namespace DSLFactory.Candle.SystemModel.Utilities.SchemaDiscover
{
    /// <summary>
    /// 
    /// </summary>
    public class DbContainer
    {
        /// <summary>
        /// 
        /// </summary>
        public IDbConnection Connection;
        /// <summary>
        /// 
        /// </summary>
        public string Name;
        /// <summary>
        /// 
        /// </summary>
        public string Owner;
        /// <summary>
        /// 
        /// </summary>
        public List<DbColumn> Columns;
        /// <summary>
        /// 
        /// </summary>
        public Entity Entity;

        /// <summary>
        /// Find a column by its name
        /// </summary>
        /// <param name="name">Column's name</param>
        /// <returns></returns>
        public DbColumn FindColumn(string name)
        {
            if (Columns == null)
                return null;
            return Columns.Find(delegate(DbColumn col) { return col.Name == name; });
        }
    }
}