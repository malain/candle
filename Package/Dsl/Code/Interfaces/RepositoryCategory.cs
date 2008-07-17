namespace DSLFactory.Candle.SystemModel.Repository
{
    /// <summary>
    /// Catégories de documents gérés dans le repository
    /// </summary>
    public enum RepositoryCategory
    {
        /// <summary>
        /// Dossier des stratégies (en .zip uniquement)
        /// </summary>
        Strategies,

        /// <summary>
        /// Fichier de configuration initial d'une solution (strategies.xml)
        /// </summary>
        Configuration,

        /// <summary>
        /// Modèles contenus dans le repository (format zip)
        /// </summary>
        Models,

        /// <summary>
        /// Fichier T4
        /// </summary>
        T4Templates
    }
}
