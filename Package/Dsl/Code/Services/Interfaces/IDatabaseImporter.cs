namespace DSLFactory.Candle.SystemModel
{
    /// <summary>
    /// Contrat à implémenter pour importer le schema d'une base de données
    /// </summary>
    public interface IDatabaseImporter
    {
        /// <summary>
        /// Imports from a specified database connection.
        /// </summary>
        /// <param name="connection">The database connection.</param>
        /// <param name="parentPackage">The parent package.</param>
        /// <param name="tables">The tables.</param>
        /// <param name="dbType">Type of the db.</param>
        void Import(System.Data.IDbConnection connection, Package parentPackage, System.Collections.Generic.List<DSLFactory.Candle.SystemModel.Utilities.SchemaDiscover.DbContainer> tables, DatabaseType dbType);
    }
}
