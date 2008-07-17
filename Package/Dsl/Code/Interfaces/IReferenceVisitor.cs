namespace DSLFactory.Candle.SystemModel.Dependencies
{
    /// <summary>
    /// Interface à implémenter pour parcourir le graphe de dépendance
    /// </summary>
    public interface IReferenceVisitor
    {
        /// <summary>
        /// Prend en compte l'élément
        /// </summary>
        /// <param name="reference">The reference.</param>
        /// <returns>
        /// false si les références de cet élément doivent être ignorées
        /// </returns>
        bool Accept(ReferenceItem reference);

        /// <summary>
        /// Exits from the element.
        /// </summary>
        /// <param name="reference">The reference.</param>
        void ExitElement(ReferenceItem reference);
    }
}
