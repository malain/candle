using System;

namespace DSLFactory.Candle.SystemModel.Strategies
{
    /// <summary>
    /// 
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class StrategyAttribute : Attribute
    {
        /// <summary>
        /// 
        /// </summary>
        public const string Empty = "<to do>";

        private string _description;
        private readonly string _id;
        private string _name;
        private string _path;
        private string _strategyGroup;

        /// <summary>
        /// Initializes a new instance of the <see cref="StrategyAttribute"/> class.
        /// </summary>
        public StrategyAttribute()
        {
            _id = Empty;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StrategyAttribute"/> class.
        /// </summary>
        /// <param name="strategyId">The strategy id.</param>
        public StrategyAttribute(string strategyId)
        {
            _id = strategyId;
        }

        /// <summary>
        /// Gets or sets the strategy group.
        /// </summary>
        /// <value>The strategy group.</value>
        public string StrategyGroup
        {
            get { return _strategyGroup; }
            set { _strategyGroup = value; }
        }

        /// <summary>
        /// Gets the strategy id.
        /// </summary>
        /// <value>The strategy id.</value>
        public string StrategyId
        {
            get { return _id; }
        }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>The description.</value>
        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }

        /// <summary>
        /// Gets or sets the path.
        /// </summary>
        /// <value>The path.</value>
        public string Path
        {
            get { return _path; }
            set { _path = value; }
        }

        /// <summary>
        /// Gets or sets the display name.
        /// </summary>
        /// <value>The display name.</value>
        public string DisplayName
        {
            get { return _name; }
            set { _name = value; }
        }
    }
}