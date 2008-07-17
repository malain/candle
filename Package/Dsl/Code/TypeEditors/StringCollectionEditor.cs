using System;
using System.ComponentModel.Design;

namespace DSLFactory.Candle.SystemModel.Editor
{
    /// <summary>
    /// 
    /// </summary>
    public class StringCollectionEditor : CollectionEditor
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StringCollectionEditor"/> class.
        /// </summary>
        /// <param name="type">The type of the collection for this editor to edit.</param>
        public StringCollectionEditor( Type type )
            : base( type )
        {

        }
        /// <summary>
        /// Creates a new instance of the specified collection item type.
        /// </summary>
        /// <param name="itemType">The type of item to create.</param>
        /// <returns>A new instance of the specified object.</returns>
        protected override object CreateInstance( Type itemType )
        {
            if( itemType == typeof( String ) )
                return new String( ' ', 0 );
            return base.CreateInstance( itemType );
        }
    }
}