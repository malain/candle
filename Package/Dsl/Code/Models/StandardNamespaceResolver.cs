using System;

namespace DSLFactory.Candle.SystemModel
{
    /// <summary>
    /// Création du nom complet du type
    /// </summary>
    [CLSCompliant(true)]
    public class StandardNamespaceResolver
    {
        /// <summary>
        /// Resolves the specified @namespace.
        /// </summary>
        /// <param name="namespace">The @namespace.</param>
        /// <returns></returns>
        public virtual string Resolve(string @namespace)
        {
            return @namespace;
        }
    }
}