using System;
using System.Xml.Serialization;

namespace DSLFactory.Candle.SystemModel.Strategies
{
    /// <summary>
    /// Stockage des types de stratégie référencés
    /// </summary>
    public struct StrategyTypeReference
    {
        /// <summary>
        /// 
        /// </summary>
        [XmlAttribute("package")] 
        public string PackageName;
        /// <summary>
        /// 
        /// </summary>
        [XmlIgnore] 
        public Type StrategyType;
        /// <summary>
        /// 
        /// </summary>
        [XmlAttribute("type")] 
        public string StrategyTypeName;
    }
}