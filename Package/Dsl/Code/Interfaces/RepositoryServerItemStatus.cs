namespace DSLFactory.Candle.SystemModel.Repository
{
    /// <summary>
    /// Etat renvoyé lors de la récupération d'un fichier
    /// </summary>
    public enum RepositoryServerItemStatus
    {
        /// <summary>
        /// Nouvelle version chargée
        /// </summary>
        Loaded,
        /// <summary>
        /// Fichier inconnu (n'existe pas sur le serveur)
        /// </summary>
        NotFound,
        /// <summary>
        /// Les fichiers sont identiques
        /// </summary>
        NotModified
    }
}