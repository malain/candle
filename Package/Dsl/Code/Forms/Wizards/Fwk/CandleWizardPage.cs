using System.Windows.Forms;

namespace DSLFactory.Candle.SystemModel.Wizard
{
    /// <summary>
    /// 
    /// </summary>
    public partial class CandleWizardPage : UserControl
    {
        private string _headerText;
        private CandleWizardForm _wizard;

        /// <summary>
        /// Initializes a new instance of the <see cref="CandleWizardPage"/> class.
        /// </summary>
        /// <param name="parent">The parent.</param>
        public CandleWizardPage(CandleWizardForm parent)
        {
            _wizard = parent;
            Initialize();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CandleWizardPage"/> class.
        /// </summary>
        public CandleWizardPage()
        {
            Initialize();
        }

        /// <summary>
        /// Gets or sets the wizard.
        /// </summary>
        /// <value>The wizard.</value>
        protected CandleWizardForm Wizard
        {
            get { return _wizard; }
            set { _wizard = value; }
        }

        /// <summary>
        /// Gets or sets the header text.
        /// </summary>
        /// <value>The header text.</value>
        public string HeaderText
        {
            get { return _headerText; }
            set { _headerText = value; }
        }

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        private void Initialize()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Called when [activated].
        /// </summary>
        public virtual void OnActivated()
        {
        }

        /// <summary>
        /// Called when [deactivated].
        /// </summary>
        /// <param name="finish">if set to <c>true</c> [finish].</param>
        /// <returns></returns>
        public virtual bool OnDeactivated(bool finish)
        {
            return true;
        }
    }
}