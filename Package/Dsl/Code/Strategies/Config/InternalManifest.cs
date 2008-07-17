using System;

namespace DSLFactory.Candle.SystemModel.Strategies
{
    /// <summary>
    /// Manifest pour une stratégie interne (sans package)
    /// </summary>
    public class InternalManifest : StrategyManifest
    {
        private readonly string _description;
        private readonly string _displayName;
        private readonly string _path;
        private readonly string _strategyGroup;

        /// <summary>
        /// Initializes a new instance of the <see cref="InternalManifest"/> class.
        /// </summary>
        /// <param name="strategy">The strategy.</param>
        public InternalManifest(StrategyBase strategy)
        {
            Type strategyType = strategy.GetType();
            _strategyGroup = "Standard";
            _displayName = strategyType.FullName;
            StrategyTypeName = strategyType.FullName;

            if (!String.IsNullOrEmpty(strategy.DisplayName))
                _displayName = strategy.DisplayName;
            if (!String.IsNullOrEmpty(strategy.StrategyGroup))
                _strategyGroup = strategy.StrategyGroup;
            if (!String.IsNullOrEmpty(strategy.StrategyPath))
                _path = strategy.StrategyPath;
            if (!String.IsNullOrEmpty(strategy.Description))
                _description = strategy.Description;

            foreach (
                StrategyAttribute customAttribute in strategyType.GetCustomAttributes(typeof (StrategyAttribute), false)
                )
            {
                if (!String.IsNullOrEmpty(customAttribute.Description))
                    _description = customAttribute.Description;
                if (!String.IsNullOrEmpty(customAttribute.StrategyGroup))
                    _strategyGroup = customAttribute.StrategyGroup;
            }
        }

        /// <summary>
        /// Permet de faire des regroupements lors de l'affichage
        /// </summary>
        public override string StrategyGroup
        {
            get { return _strategyGroup; }
        }

        /// <summary>
        /// Gets the display name.
        /// </summary>
        /// <value>The display name.</value>
        public override string DisplayName
        {
            get { return _displayName; }
        }

        /// <summary>
        /// Gets the strategy path.
        /// </summary>
        /// <value>The strategy path.</value>
        public override string StrategyPath
        {
            get { return _path; }
        }

        /// <summary>
        /// Gets the description.
        /// </summary>
        /// <value>The description.</value>
        public override string Description
        {
            get { return _description; }
        }
    }
}