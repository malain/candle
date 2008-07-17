using System;
using System.IO;
using System.Windows.Forms;
using DSLFactory.Candle.SystemModel.Configuration.VisualStudio;
using DSLFactory.Candle.SystemModel.Forms;
using DSLFactory.Candle.SystemModel.Repository;

namespace DSLFactory.Candle.SystemModel.Configuration
{
    /// <summary>
    /// Wizard affiché lors de la 1ère exécution pour saisir
    /// les paramètres de configuration.
    /// </summary>
    public partial class OptionsInitForm : Form, IDisposable
    {
        private readonly OptionsPageControl _optionsPageControl;

        /// <summary>
        /// Initializes a new instance of the <see cref="OptionsInitForm"/> class.
        /// </summary>
        /// <param name="optionsPage">The options page.</param>
        public OptionsInitForm(OptionsPage optionsPage)
        {
            InitializeComponent();

            _optionsPageControl = optionsPage.PageControl;
            body.Controls.Add(_optionsPageControl);

            _optionsPageControl.DataValid += optionsPageControl_DataValid;
        }

        #region IDisposable Members

        /// <summary>
        /// Releases all resources used by the <see cref="T:System.ComponentModel.Component"></see>.
        /// </summary>
        void IDisposable.Dispose()
        {
            body.Controls.Remove(_optionsPageControl);
        }

        #endregion

        /// <summary>
        /// Optionses the page control_ data valid.
        /// </summary>
        /// <param name="isValid">if set to <c>true</c> [is valid].</param>
        private void optionsPageControl_DataValid(bool isValid)
        {
            btnOK.Enabled = isValid;
        }

        /// <summary>
        /// Handles the Click event of the btnOK control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void btnOK_Click(object sender, EventArgs e)
        {
            _optionsPageControl.CommitChanges();

            try
            {
                Directory.CreateDirectory(_optionsPageControl.OptionsPage.BaseDirectory);
                Directory.CreateDirectory(
                    Path.Combine(_optionsPageControl.OptionsPage.BaseDirectory, RepositoryCategory.Strategies.ToString()));
                Directory.CreateDirectory(
                    Path.Combine(_optionsPageControl.OptionsPage.BaseDirectory,
                                 RepositoryCategory.Configuration.ToString()));
            }
            catch
            {
                ServiceLocator.Instance.IDEHelper.ShowError(
                    String.Format(GuiResources.UnableCreateDirectory, _optionsPageControl.OptionsPage.BaseDirectory));
                return;
            }

            try
            {
                if (_optionsPageControl.OptionsPage.RepositoryEnabled)
                    RepositoryManager.CheckRepository(_optionsPageControl.OptionsPage.RepositoryUrl);
            }
            catch
            {
                ServiceLocator.Instance.IDEHelper.ShowError(
                    String.Format(GuiResources.InvalidUrlRepository, _optionsPageControl.OptionsPage.RepositoryUrl));
                return;
            }

            _optionsPageControl.OptionsPage.SaveSettingsToStorage();
            DialogResult = DialogResult.OK;
        }
    }
}