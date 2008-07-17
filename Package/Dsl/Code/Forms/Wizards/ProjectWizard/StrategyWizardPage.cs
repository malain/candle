using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using DSLFactory.Candle.SystemModel.Repository;

namespace DSLFactory.Candle.SystemModel.Wizard
{
    /// <summary>
    /// 
    /// </summary>
    public partial class StrategyWizardPage : CandleWizardPage
    {
        private string _dataKey;

        /// <summary>
        /// Initializes a new instance of the <see cref="StrategyWizardPage"/> class.
        /// </summary>
        /// <param name="parentForm">The parent form.</param>
        /// <param name="key">The key.</param>
        public StrategyWizardPage(CandleWizardForm parentForm, string key)
            : this(parentForm, "Select a strategies template to initialize your project.",
                   key, RepositoryManager.Instance.GetStrategiesConfigurationList())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StrategyWizardPage"/> class.
        /// </summary>
        /// <param name="parentForm">The parent form.</param>
        /// <param name="headerText">The header text.</param>
        /// <param name="key">The key.</param>
        /// <param name="data">The data.</param>
        protected StrategyWizardPage(CandleWizardForm parentForm, string headerText, string key, List<string> data)
            : base(parentForm)
        {
            Initialize(key, data);
            HeaderText = headerText;
        }

        /// <summary>
        /// Gets the empty name of the item.
        /// </summary>
        /// <value>The empty name of the item.</value>
        protected virtual string EmptyItemName
        {
            get { return "No strategies"; }
        }

        /// <summary>
        /// Gets the index of the item image.
        /// </summary>
        /// <value>The index of the item image.</value>
        protected virtual int ItemImageIndex
        {
            get { return 1; }
        }

        /// <summary>
        /// Gets or sets the selected model.
        /// </summary>
        /// <value>The selected model.</value>
        protected string SelectedModel
        {
            get
            {
                if (lstModels.SelectedItems.Count == 0)
                    return null;
                return lstModels.SelectedItems[0].Tag as string;
            }
            set
            {
                if (String.IsNullOrEmpty(value) && lstModels.Items.Count > 0)
                    lstModels.Items[0].Selected = true;
                else
                {
                    foreach (ListViewItem item in lstModels.Items)
                    {
                        item.Selected = (value != null && Utils.StringCompareEquals(item.Name, value)) ||
                                        (value == null && item.Tag == null);
                    }
                }
            }
        }

        /// <summary>
        /// Initializes the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="data">The data.</param>
        protected void Initialize(string key, List<string> data)
        {
            _dataKey = key;
            InitializeComponent();

            try
            {
                lstModels.Items.Add(new ListViewItem(EmptyItemName, ItemImageIndex));

                foreach (string name in data)
                {
                    ListViewItem item = new ListViewItem();
                    item.Text = name;
                    if (name.IndexOfAny(Path.GetInvalidPathChars()) < 0)
                        item.Text = Path.GetFileNameWithoutExtension(name);
                    item.Tag = name;
                    item.ImageIndex = ItemImageIndex;
                    lstModels.Items.Add(item);
                }
            }
            catch (Exception ex)
            {
                lstModels.Items.Add(new ListViewItem("err:" + ex.Message));
            }

            // Préselection (si il y en a)
            SelectedModel = Wizard.GetUserData<string>(key);
        }

        /// <summary>
        /// Called when [deactivated].
        /// </summary>
        /// <param name="finish">if set to <c>true</c> [finish].</param>
        /// <returns></returns>
        public override bool OnDeactivated(bool finish)
        {
            SetData();
            return true;
        }

        /// <summary>
        /// Sets the data.
        /// </summary>
        private void SetData()
        {
            Wizard.SetUserData(_dataKey, SelectedModel);
        }

        /// <summary>
        /// Enables the next button.
        /// </summary>
        protected virtual void EnableNextButton()
        {
            Wizard.EnableButton(ButtonType.Next, (lstModels.SelectedItems.Count > 0) && !Wizard.IsLastPage);
        }

        /// <summary>
        /// Handles the SelectedIndexChanged event of the lstModels control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void lstModels_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetData();
            EnableNextButton();
        }

        /// <summary>
        /// Handles the MouseDoubleClick event of the lstModels control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Forms.MouseEventArgs"/> instance containing the event data.</param>
        private void lstModels_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Control item = lstModels.GetChildAtPoint(new Point(e.X, e.Y));
            Wizard.DoDefault();
        }
    }
}