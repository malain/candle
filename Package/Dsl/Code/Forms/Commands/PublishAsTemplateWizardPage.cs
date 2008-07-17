using DSLFactory.Candle.SystemModel.Wizard;

namespace DSLFactory.Candle.SystemModel.Commands
{
    /// <summary>
    /// 
    /// </summary>
    public partial class PublishAsTemplateWizardPage : CandleWizardPage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PublishAsTemplateWizardPage"/> class.
        /// </summary>
        public PublishAsTemplateWizardPage()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PublishAsTemplateWizardPage"/> class.
        /// </summary>
        /// <param name="parentForm">The parent form.</param>
        public PublishAsTemplateWizardPage(CandleWizardForm parentForm) : base(parentForm)
        {
            InitializeComponent();
        }

        /// <summary>
        /// Called when [activated].
        /// </summary>
        public override void OnActivated()
        {
            base.OnActivated();
            txtModelName.Text = Wizard.GetUserData<string>("ModelName");
        }

        /// <summary>
        /// Called when [deactivated].
        /// </summary>
        /// <param name="finish">if set to <c>true</c> [finish].</param>
        /// <returns></returns>
        public override bool OnDeactivated(bool finish)
        {
            Wizard.SetUserData("ModelName", txtModelName.Text);
            Wizard.SetUserData("StrategiesName", txtStrategiesName.Text);

            return base.OnDeactivated(finish);
        }
    }
}