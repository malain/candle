using System.Collections.Generic;

namespace DSLFactory.Candle.SystemModel.Utilities.SchemaDiscover
{
    /// <summary>
    /// 
    /// </summary>
    public class DbRelationShip
    {
        /// <summary>
        /// 
        /// </summary>
        public string Name;
        /// <summary>
        /// 
        /// </summary>
        public string SourceTableName;
        /// <summary>
        /// 
        /// </summary>
        public string SourceTableOwner;
        /// <summary>
        /// 
        /// </summary>
        public List<string> SourceColumnNames = new List<string>();
       // public DbTable TargetTable;
        /// <summary>
        /// 
        /// </summary>
        public string TargetTableName;
        /// <summary>
        /// 
        /// </summary>
        public string TargetTableOwner;
        /// <summary>
        /// 
        /// </summary>
        public List<string> TargetColumnNames = new List<string>();
    }
}
