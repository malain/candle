namespace DSLFactory.Candle.SystemModel.Dependencies
{
    /// <summary>
    /// Interface impl�ment�s par les �lements ayant des r�f�rences devant �tre prises en compte par le gestionnaire de d�pendances
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
