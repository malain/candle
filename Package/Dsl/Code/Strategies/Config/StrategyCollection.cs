using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace DSLFactory.Candle.SystemModel.Strategies
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    [XmlRoot("strategies")]
    public class StrategyCollection : List<StrategyBase>
    {
        /// <summary>
        /// Gets the types.
        /// </summary>
        /// <param name="namingStrategy">The naming strategy.</param>
        /// <returns></returns>
        internal List<StrategyTypeReference> GetTypes(IPackagedStrategy namingStrategy)
        {
            List<StrategyTypeReference> types = new List<StrategyTypeReference>();
            foreach (IPackagedStrategy s in this)
            {
                AddReference(types, s);
            }

            if (namingStrategy != null)
                AddReference(types, namingStrategy);

            return types;
        }

        /// <summary>
        /// Adds a reference.
        /// </summary>
        /// <param name="types">The types.</param>
        /// <param name="s">The s.</param>
        private static void AddReference(List<StrategyTypeReference> types, IPackagedStrategy s)
        {
            if (!types.Exists(delegate(StrategyTypeReference c) { return c.StrategyType == s.GetType(); }))
            {
                StrategyTypeReference str;
                str.StrategyType = s.GetType();
                str.PackageName = s.PackageName;
                if (s.GetType().Assembly == typeof (StrategyBase).Assembly)
                    str.StrategyTypeName = s.GetType().FullName; // Type interne
                else
                    str.StrategyTypeName = String.Concat(s.GetType().FullName, ",", s.GetType().Assembly.GetName().Name);
                types.Add(str);
            }
        }
    }
}