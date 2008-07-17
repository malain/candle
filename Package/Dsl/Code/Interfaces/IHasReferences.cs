namespace DSLFactory.Candle.SystemModel.Dependencies
{
    /// <summary>
    /// Interface implémentés par les élements ayant des références devant être prises en compte par le gestionnaire de dépendances
    /// </summary>
    public interface IHasReferences
    {
        /// <summary>
        /// Gets the references.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        System.Collections.Generic.IEnumerable<ReferenceItem> GetReferences(ReferenceContext context);
    }
}
