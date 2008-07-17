namespace DSLFactory.Candle.SystemModel.Dependencies
{
    /// <summary>
    /// Interface � impl�menter pour parcourir le graphe de d�pendance
    /// </summary>
    public interface IReferenceVisitor
    {
        /// <summary>
        /// Prend en compte l'�l�ment
        /// </summary>
        /// <param name="reference">The reference.</param>
        /// <returns>
        /// false si les r�f�rences de cet �l�ment doivent �tre ignor�es
        /// </returns>
        bool Accept(ReferenceItem reference);

        /// <summary>
        /// Exits from the element.
        /// </summary>
        /// <param name="reference">The reference.</param>
        void ExitElement(ReferenceItem reference);
    }
}
