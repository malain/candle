using System.Collections.Generic;

namespace DSLFactory.Candle.SystemModel
{
    /// <summary>
    /// Stocke la liste des références en s'assurant de leurs unicités
    /// </summary>
    public class ReferencesCollection : List<string>
    {
        /// <summary>
        /// On ignore les doublons et les chaines vides
        /// </summary>
        /// <param name="item">The object to be added to the end of the <see cref="T:System.Collections.Generic.List`1"></see>. The value can be null for reference types.</param>
        public new void Add( string item )
        {
            if( string.IsNullOrEmpty( item ) )
                return;
            item = item.ToLower();
            if( this.Contains( item ) )
                return;
            base.Add( item );
        }

        /// <summary>
        /// Adds the elements of the specified collection to the end of the <see cref="T:System.Collections.Generic.List`1"></see>.
        /// </summary>
        /// <param name="collection">The collection whose elements should be added to the end of the <see cref="T:System.Collections.Generic.List`1"></see>. The collection itself cannot be null, but it can contain elements that are null, if type T is a reference type.</param>
        /// <exception cref="T:System.ArgumentNullException">collection is null.</exception>
        public new void AddRange( IEnumerable<string> collection )
        {
            foreach( string item in collection )
            {
                Add( item );
            }
        }
    }
}
