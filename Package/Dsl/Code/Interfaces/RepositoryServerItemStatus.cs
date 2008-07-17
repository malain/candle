namespace DSLFactory.Candle.SystemModel.Repository
{
    /// <summary>
    /// Etat renvoy� lors de la r�cup�ration d'un fichier
    /// </summary>
    public enum RepositoryServerItemStatus
    {
        /// <summary>
        /// Nouvelle version charg�e
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