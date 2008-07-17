namespace DSLFactory.Candle.SystemModel.Strategies
{
    /// <summary>
    /// 
    /// </summary>
    public interface IStrategyProjectGenerator : IStrategyProvidesProjectTemplates
    {
        /// <summary>
        /// Generates the VS project.
        /// </summary>
        /// <param name="layer">The layer.</param>
        void GenerateVSProject(ICustomizableElement layer);
    }
}