namespace DSLFactory.Candle.SystemModel
{
    /// <summary>
    /// Interface pour les mod�les pouvant afficher le code par double-click dans la fenetre de d�tail
    /// </summary>
    public interface IShowCodeProperties
    {
        /// <summary>
        /// Gets the name of the layer.
        /// </summary>
        /// <value>The name of the layer.</value>
        string LayerName { get;}
        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>The name.</value>
        string Name { get;}
        /// <summary>
        /// Gets the name of the member.
        /// </summary>
        /// <param name="initialName">The initial name.</param>
        /// <returns></returns>
        string GetMemberName(string initialName);
    }

}
