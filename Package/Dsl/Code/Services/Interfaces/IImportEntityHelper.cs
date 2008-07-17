namespace DSLFactory.Candle.SystemModel
{
    /// <summary>
    /// Import an entity from a source file name. The source must be in a visual studio project
    /// </summary>
    interface IImportEntityHelper
    {
        /// <summary>
        /// Imports the properties.
        /// </summary>
        /// <param name="package">The package.</param>
        /// <param name="entity">The entity.</param>
        /// <param name="fileName">Name of the file.</param>
        /// <returns>true if the import is ok</returns>
        bool ImportProperties(Package package, Entity entity, string fileName);
    }
}
