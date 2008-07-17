namespace DSLFactory.Candle.SystemModel.Repository
{
    /// <summary>
    /// Cat�gories de documents g�r�s dans le repository
    /// </summary>
    public enum RepositoryCategory
    {
        /// <summary>
        /// Dossier des strat�gies (en .zip uniquement)
        /// </summary>
        Strategies,

        /// <summary>
        /// Fichier de configuration initial d'une solution (strategies.xml)
        /// </summary>
        Configuration,

        /// <summary>
        /// Mod�les contenus dans le repository (format zip)
        /// </summary>
        Models,

        /// <summary>
        /// Fichier T4
        /// </summary>
        T4Templates
    }
}
