namespace DSLFactory.Candle.SystemModel
{
    /// <summary>
    /// Import an interface from a source file name. The source must be in a visual studio project
    /// </summary>
    interface IImportInterfaceHelper
    {
        /// <summary>
        /// Imports the operations.
        /// </summary>
        /// <param name="layer">The layer.</param>
        /// <param name="port">The port.</param>
        /// <param name="fileName">Name of the file.</param>
        /// <returns>true if the import is ok</returns>
        bool ImportOperations(SoftwareLayer layer, TypeWithOperations port, string fileName);
    }
}
