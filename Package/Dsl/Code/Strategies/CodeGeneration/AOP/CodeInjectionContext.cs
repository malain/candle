using DSLFactory.Candle.SystemModel.Strategies;

namespace DSLFactory.Candle.SystemModel
{
    /// <summary>
    /// 
    /// </summary>
    public class CodeInjectionContext
    {
        private ICustomizableElement _element;
        private GenerationContext _generationContext;
        private IStrategyCodeInjector _strategy;

        /// <summary>
        /// Initializes a new instance of the <see cref="CodeInjectionContext"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
        public CodeInjectionContext(GenerationContext context)
        {
            _generationContext = context;
        }

        /// <summary>
        /// Gets or sets the current element.
        /// </summary>
        /// <value>The current element.</value>
        public ICustomizableElement CurrentElement
        {
            get { return _element; }
            set { _element = value; }
        }

        /// <summary>
        /// Gets or sets the strategy.
        /// </summary>
        /// <value>The strategy.</value>
        public IStrategyCodeInjector Strategy
        {
            get { return _strategy; }
            set { _strategy = value; }
        }

        /// <summary>
        /// Gets or sets the generation context.
        /// </summary>
        /// <value>The generation context.</value>
        public GenerationContext GenerationContext
        {
            get { return _generationContext; }
            set { _generationContext = value; }
        }
    }
}