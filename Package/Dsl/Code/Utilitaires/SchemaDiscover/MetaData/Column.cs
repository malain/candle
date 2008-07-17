using System;

namespace DSLFactory.Candle.SystemModel.Utilities.SchemaDiscover
{
    /// <summary>
    /// 
    /// </summary>
    public class DbColumn
    {
        /// <summary>
        /// 
        /// </summary>
        public string Name;
        /// <summary>
        /// 
        /// </summary>
        public DbContainer Parent;
        /// <summary>
        /// 
        /// </summary>
        public Type ClrType;
        /// <summary>
        /// 
        /// </summary>
        public string ServerType;
        /// <summary>
        /// 
        /// </summary>
        public int Length;
        /// <summary>
        /// 
        /// </summary>
        public int Precision;
        /// <summary>
        /// 
        /// </summary>
        public int Scale;
        /// <summary>
        /// 
        /// </summary>
        public bool InPrimaryKey;
        /// <summary>
        /// 
        /// </summary>
        public bool InUniqueKey;
        /// <summary>
        /// 
        /// </summary>
        public bool IsNullable;
        /// <summary>
        /// 
        /// </summary>
        public bool IsAutoIncrement;
        /// <summary>
        /// 
        /// </summary>
        public Property Property;
    }
}
