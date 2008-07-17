using System;

namespace DSLFactory.Candle.SystemModel.Strategies
{
    /// <summary>
    /// 
    /// </summary>
    public class StrategyRemovedEventArgs : EventArgs
    {
        private string _owner;
        private StrategyBase _strategy;


        /// <summary>
        /// Initializes a new instance of the <see cref="StrategyRemovedEventArgs"/> class.
        /// </summary>
        /// <param name="strategy">The strategy.</param>
        /// <param name="owner">The owner.</param>
        public StrategyRemovedEventArgs(StrategyBase strategy, string owner)
        {
            _owner = owner;
            _strategy = strategy;
        }

        /// <summary>
        /// Gets or sets the owner.
        /// </summary>
        /// <value>The owner.</value>
        public string Owner
        {
            get { return _owner; }
            set { _owner = value; }
        }

        /// <summary>
        /// Gets or sets the strategy.
        /// </summary>
        /// <value>The strategy.</value>
        public StrategyBase Strategy
        {
            get { return _strategy; }
            set { _strategy = value; }
        }
    }
}