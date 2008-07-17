using System;
using System.ComponentModel.Design;

namespace DSLFactory.Candle.SystemModel.Editor
{
//    [System.ComponentModel.Editor( typeof(DSLFactory.Candle.SystemModel.Editor.GenericCollectionEditor<T>), typeof(System.Drawing.Design.UITypeEditor))]
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class GenericCollectionEditor<T> : CollectionEditor where T : new()
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GenericCollectionEditor&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="type">The type of the collection for this editor to edit.</param>
        public GenericCollectionEditor( Type type ) : base( type )
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
            else if( itemType == typeof(T) )
                return new T();
            return base.CreateInstance( itemType );
        }
    }
}
