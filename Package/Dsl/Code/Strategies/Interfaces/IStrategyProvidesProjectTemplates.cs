
namespace DSLFactory.Candle.SystemModel.Strategies
{
    /// <summary>
    /// Strat�gie proposant un template de projet
    /// </summary>
    public interface IStrategyProvidesProjectTemplates
    {
        /// <summary>
        /// Gets the project template.
        /// </summary>
        /// <param name="layer">The layer.</param>
        /// <returns></returns>
        string GetProjectTemplate(SoftwareLayer layer);
        /// <summary>
        /// Extension de l'assembly g�n�r�e
        /// </summary>
        string GetAssemblyExtension(SoftwareLayer layer);
    }
}
