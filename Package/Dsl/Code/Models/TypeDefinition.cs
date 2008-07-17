using System;

namespace DSLFactory.Candle.SystemModel
{
    /// <summary>
    /// Définition d'un type <see cref="TypeWithOperations.OperationExists"/>
    /// </summary>
    [CLSCompliant(true)]
    public struct TypeDefinition
    {
        /// <summary>
        /// 
        /// </summary>
        public bool IsCollection;
        /// <summary>
        /// 
        /// </summary>
        public string Name;

        /// <summary>
        /// Initializes a new instance of the <see cref="TypeDefinition"/> struct.
        /// </summary>
        /// <param name="name">The name.</param>
        public TypeDefinition(string name)
        {
            Name = name;
            IsCollection = false;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TypeDefinition"/> struct.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="isCollection">if set to <c>true</c> [is collection].</param>
        public TypeDefinition(string name, bool isCollection)
        {
            Name = name;
            IsCollection = isCollection;
        }
    }
}