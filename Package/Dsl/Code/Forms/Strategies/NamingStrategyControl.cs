using System.Windows.Forms;

namespace DSLFactory.Candle.SystemModel.Strategies
{
    /// <summary>
    /// 
    /// </summary>
    public partial class NamingStrategyControl : UserControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NamingStrategyControl"/> class.
        /// </summary>
        public NamingStrategyControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initializes the specified strategy.
        /// </summary>
        /// <param name="strategy">The strategy.</param>
        internal void Initialize(INamingStrategy strategy)
        {
            SetStrategy(strategy);
        }

        /// <summary>
        /// Sets the strategy.
        /// </summary>
        /// <param name="strategy">The strategy.</param>
        private void SetStrategy(INamingStrategy strategy)
        {
            txtNamingStrategyType.Text = strategy.GetType().AssemblyQualifiedName;
            propGrid.SelectedObject = strategy;
        }
    }
}