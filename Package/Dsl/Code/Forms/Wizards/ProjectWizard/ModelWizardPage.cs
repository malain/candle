using System.IO;
using DSLFactory.Candle.SystemModel.Repository;

namespace DSLFactory.Candle.SystemModel.Wizard
{
    /// <summary>
    /// 
    /// </summary>
    public partial class ModelWizardPage : StrategyWizardPage
    {
        private readonly string _initialStrategy;
        private readonly string _strategiesKey;

        /// <summary>
        /// Initializes a new instance of the <see cref="ModelWizardPage"/> class.
        /// </summary>
        /// <param name="parentForm">The parent form.</param>
        /// <param name="key">The key.</param>
        /// <param name="strategiesKey">The strategies key.</param>
        public ModelWizardPage(CandleWizardForm parentForm, string key, string strategiesKey)
            : base(parentForm, "Select a model template to initialize your project.",
                   key, RepositoryManager.Instance.GetTemplateModelList())
        {
            _initialStrategy = Wizard.GetUserData<string>(strategiesKey);
            _strategiesKey = strategiesKey;
        }

        /// <summary>
        /// Gets the index of the item image.
        /// </summary>
        /// <value>The index of the item image.</value>
        protected override int ItemImageIndex
        {
            get { return 0; }
        }

        /// <summary>
        /// Gets the empty name of the item.
        /// </summary>
        /// <value>The empty name of the item.</value>
        protected override string EmptyItemName
        {
            get { return "Empty component"; }
        }

        /// <summary>
        /// Called when [deactivated].
        /// </summary>
        /// <param name="finish">if set to <c>true</c> [finish].</param>
        /// <returns></returns>
        public override bool OnDeactivated(bool finish)
        {
            Wizard.SetUserData(_strategiesKey, _initialStrategy);

            string template = SelectedModel;
            if (template != null)
            {
                string temp = Path.GetTempFileName();
                try
                {
                    if (
                        RepositoryManager.Instance.GetFileFromRepository(RepositoryCategory.Configuration, template,
                                                                         temp))
                    {
                        ModelLoader loader = ModelLoader.GetLoader(temp, false);
                        if (loader != null && loader.Model != null)
                        {
                            Wizard.SetUserData(_strategiesKey, loader.Model.StrategyTemplate);
                        }
                    }
                }
                finally
                {
                    Utils.DeleteFile(temp);
                }
            }

            return base.OnDeactivated(finish);
        }
    }
}