namespace DSLFactory.Candle.SystemModel
{
    /// <summary>
    /// Visibilité d'un composant par rapport à un domaine
    /// </summary>
    public enum DomainVisibility
    {
        /// <summary>
        /// Interne au domaine
        /// </summary>
        DomainPrivate,

        /// <summary>
        /// Public à tous les domaines
        /// </summary>
        DomainPublic
    }
}