using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.Modeling;

namespace DSLFactory.Candle.SystemModel.Strategies
{
    /// <summary>
    /// 
    /// </summary>
    [CLSCompliant(false)]
    public interface IStrategyManager
    {
        /// <summary>
        /// Target language
        /// </summary>
        /// <value>The target language.</value>
        LanguageConfiguration TargetLanguage { get; }

        /// <summary>
        /// Nom du fichier contenant les stratégies
        /// </summary>
        /// <value>The name of the file.</value>
        string FileName { get; }

        /// <summary>
        /// Gets the naming strategy.
        /// </summary>
        /// <value>The naming strategy.</value>
        INamingStrategy NamingStrategy { get; }

        /// <summary>
        /// List of strategies availaibles in this project
        /// </summary>
        /// <param name="owner">The owner.</param>
        /// <param name="includeGlobalStrategies">if set to <c>true</c> [include global strategies].</param>
        /// <returns></returns>
        List<StrategyBase> GetStrategies(CandleElement owner, bool includeGlobalStrategies);

        /// <summary>
        /// Add a strategy in the strategies file
        /// </summary>
        /// <param name="owner">The owner.</param>
        /// <param name="manifest">The manifest.</param>
        /// <returns></returns>
        StrategyBase AddStrategy(CandleElement owner, StrategyManifest manifest);

        /// <summary>
        /// Delete a strategy in the strategies file
        /// </summary>
        /// <param name="owner">The owner.</param>
        /// <param name="strategy">The strategy.</param>
        void RemoveStrategy(CandleElement owner, StrategyBase strategy);

        /// <summary>
        /// Sauvegarde
        /// </summary>
        /// <param name="store">The store.</param>
        void Save(Store store);

        /// <summary>
        /// Gets the assembly extension.
        /// </summary>
        /// <param name="layer">The layer.</param>
        /// <returns></returns>
        string GetAssemblyExtension(SoftwareLayer layer);

        /// <summary>
        /// Gets the package.
        /// </summary>
        /// <param name="packageName">Name of the package.</param>
        /// <returns></returns>
        InternalPackage GetPackage(string packageName);

        /// <summary>
        /// Unloads the strategies.
        /// </summary>
        void UnloadStrategies();
    }
}