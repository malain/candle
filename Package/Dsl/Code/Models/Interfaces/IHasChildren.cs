using System;
using System.Collections;
using DSLFactory.Candle.SystemModel.Utilities;
using Microsoft.VisualStudio.Modeling;

namespace DSLFactory.Candle.SystemModel
{
    /// <summary>
    /// Permet d'accéder aux enfants 
    /// </summary>
    [CLSCompliant(true)]
    public interface IHasChildren 
    {
        /// <summary>
        /// Gets the store.
        /// </summary>
        /// <value>The store.</value>
        Store Store { get;}
        /// <summary>
        /// Gets the id.
        /// </summary>
        /// <value>The id.</value>
        Guid Id { get;}
        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>The name.</value>
        string Name { get;}
        /// <summary>
        /// Gets the children for category.
        /// </summary>
        /// <param name="category">The category.</param>
        /// <returns></returns>
        IList GetChildrenForCategory(VirtualTreeGridCategory category);
    }
}
