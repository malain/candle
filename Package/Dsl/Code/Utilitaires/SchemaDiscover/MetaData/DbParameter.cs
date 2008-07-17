using System;
using System.Data;

namespace DSLFactory.Candle.SystemModel.Utilities.SchemaDiscover
{
    /// <summary>
    /// 
    /// </summary>
    public class DbParameter
    {
        /// <summary>
        /// 
        /// </summary>
        public string Name;
        /// <summary>
        /// 
        /// </summary>
        public ParameterDirection ParameterDirection;
        /// <summary>
        /// 
        /// </summary>
        public string ServerType;
        /// <summary>
        /// 
        /// </summary>
        public Type ClrType;
    }
}