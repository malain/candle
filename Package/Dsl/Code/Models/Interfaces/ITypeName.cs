using System;

namespace DSLFactory.Candle.SystemModel
{
    /// <summary>
    /// 
    /// </summary>
    [CLSCompliant(true)]
    public interface ITypeMember
    {
        /// <summary>
        /// Gets the id.
        /// </summary>
        /// <value>The id.</value>
        Guid Id { get; }

        /// <summary>
        /// Gets the full name of the type.
        /// </summary>
        /// <value>The full name of the type.</value>
        string FullTypeName { get; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        string Name { get; set; }

        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        /// <value>The type.</value>
        string Type { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is collection.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is collection; otherwise, <c>false</c>.
        /// </value>
        bool IsCollection { get; set; }

        /// <summary>
        /// Gets or sets the comment.
        /// </summary>
        /// <value>The comment.</value>
        string Comment { get; set; }
    }

    /// <summary>
    /// Interface permettant de retrouver le parent qui contient la définition du
    /// namesapce.
    /// </summary>
    [CLSCompliant(true)]
    public interface IHasNamespace
    {
        /// <summary>
        /// Recherche itérative du parent 
        /// </summary>
        string NamespaceDeclaration { get; }
    }
}