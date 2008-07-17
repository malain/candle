using System.Collections.Generic;

namespace DSLFactory.Candle.SystemModel.Strategies
{
    /// <summary>
    /// Liste de manifest qui garantit qu'il n'y a pas de doublon
    /// </summary>
    public class ManifestCollection : List<StrategyManifest>
    {
        /// <summary>
        /// Adds the specified manifest.
        /// </summary>
        /// <param name="manifest">The manifest.</param>
        public new void Add(StrategyManifest manifest)
        {
            if (!Exists(delegate(StrategyManifest m) { return m.DisplayName == manifest.DisplayName; }))
                base.Add(manifest);
        }

        /// <summary>
        /// Adds the elements of the specified collection to the end of the <see cref="T:System.Collections.Generic.List`1"></see>.
        /// </summary>
        /// <param name="collection">The collection whose elements should be added to the end of the <see cref="T:System.Collections.Generic.List`1"></see>. The collection itself cannot be null, but it can contain elements that are null, if type T is a reference type.</param>
        /// <exception cref="T:System.ArgumentNullException">collection is null.</exception>
        public new void AddRange(IEnumerable<StrategyManifest> collection)
        {
            foreach (StrategyManifest m in collection)
                Add(m);
        }
    }
}