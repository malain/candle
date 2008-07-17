using System;
using System.Windows.Forms;

namespace DSLFactory.Candle.SystemModel.Strategies
{
    /// <summary>
    /// 
    /// </summary>
    public partial class LanguageConfigurationControl : UserControl
    {
        private LanguageConfiguration _config;

        /// <summary>
        /// Initializes a new instance of the <see cref="LanguageConfigurationControl"/> class.
        /// </summary>
        public LanguageConfigurationControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initializes the specified lang CFG.
        /// </summary>
        /// <param name="langCfg">The lang CFG.</param>
        internal void Initialize(LanguageConfiguration langCfg)
        {
            _config = langCfg;
            cbLanguage.Text = _config.Name;
            txtExtension.Text = _config.Extension;
            txtProjectTemplate.Text = _config.DefaultLibraryTemplateName;
        }

        /// <summary>
        /// Handles the TextChanged event of the cbLanguage control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void cbLanguage_TextChanged(object sender, EventArgs e)
        {
            _config.Name = cbLanguage.Text;
            txtProjectTemplate.Text = String.Empty;
            txtExtension.Text = String.Empty;
        }

        /// <summary>
        /// Handles the TextChanged event of the txtExtension control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void txtExtension_TextChanged(object sender, EventArgs e)
        {
            _config.Extension = txtExtension.Text;
        }

        /// <summary>
        /// Handles the TextChanged event of the txtProjectTemplate control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void txtProjectTemplate_TextChanged(object sender, EventArgs e)
        {
            _config.DefaultLibraryTemplateName = txtProjectTemplate.Text;
        }
    }
}