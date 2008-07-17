using System.Collections.Generic;
using System.Windows.Forms;

namespace DSLFactory.Candle.SystemModel.Strategies
{
    /// <summary>
    /// 
    /// </summary>
    public partial class RunningStrategiesForm : Form
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RunningStrategiesForm"/> class.
        /// </summary>
        /// <param name="element">The element.</param>
        public RunningStrategiesForm(CandleElement element)
        {
            InitializeComponent();

            btnOK.Enabled = false;

            chkPass.Items.Clear();
            chkPass.Items.Add(GenerationPass.MetaModelUpdate, true);
            chkPass.Items.Add(GenerationPass.CodeGeneration, true);
            chkPass.Items.Add(GenerationPass.ElementAdded, false);

            lstStrategies.Items.Clear();
            if (element != null)
            {
                foreach (
                    StrategyBase strategy in StrategyManager.GetInstance(element.Store).GetStrategies(element, true))
                {
                    ListViewItem item = new ListViewItem(strategy.DisplayName);
                    item.SubItems.Add(strategy.Description);
                    item.Checked = true;
                    item.Tag = strategy;
                    lstStrategies.Items.Add(item);
                    btnOK.Enabled = true;
                }
            }
        }

        /// <summary>
        /// Gets the selected generation passes.
        /// </summary>
        /// <value>The selected generation passes.</value>
        public GenerationPass SelectedGenerationPasses
        {
            get
            {
                GenerationPass passes = 0;
                foreach (GenerationPass pass in chkPass.CheckedItems)
                {
                    passes |= pass;
                }
                return passes;
            }
        }

        /// <summary>
        /// Gets the selected strategies.
        /// </summary>
        /// <value>The selected strategies.</value>
        public List<StrategyBase> SelectedStrategies
        {
            get
            {
                List<StrategyBase> strategies = new List<StrategyBase>();
                foreach (ListViewItem item in lstStrategies.Items)
                {
                    if (item.Checked)
                    {
                        StrategyBase strategy = item.Tag as StrategyBase;
                        strategies.Add(strategy);
                    }
                }
                return strategies;
            }
        }
    }
}