using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace DSLFactory.Candle.SystemModel.Commands
{
    /// <summary>
    /// 
    /// </summary>
    public partial class SelectModelForm : Form
    {
        private readonly List<CandleModel> _allModels;
        private readonly List<CandleModel> _unUpatedModels;

        /// <summary>
        /// Initializes a new instance of the <see cref="SelectModelForm"/> class.
        /// </summary>
        /// <param name="unUpatedModels">The un upated models.</param>
        /// <param name="allModels">All models.</param>
        public SelectModelForm(List<CandleModel> unUpatedModels, List<CandleModel> allModels)
        {
            _unUpatedModels = unUpatedModels;
            _allModels = allModels;

            InitializeComponent();

            if (allModels == null || allModels.Count == unUpatedModels.Count)
                chkSelectAll.Visible = false;

            PopulateItems(unUpatedModels);
        }

        /// <summary>
        /// Gets the selected models.
        /// </summary>
        /// <value>The selected models.</value>
        public List<CandleModel> SelectedModels
        {
            get
            {
                List<CandleModel> models = new List<CandleModel>(lstModels.CheckedItems.Count);
                foreach (Item item in lstModels.CheckedItems)
                    models.Add(item.Model);
                return models;
            }
        }

        /// <summary>
        /// Populates the items.
        /// </summary>
        /// <param name="models">The models.</param>
        private void PopulateItems(List<CandleModel> models)
        {
            lstModels.Items.Clear();
            foreach (CandleModel model in models)
            {
                lstModels.Items.Add(new Item(model), true);
            }
        }

        /// <summary>
        /// Handles the CheckedChanged event of the chkSelectAll control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void chkSelectAll_CheckedChanged(object sender, EventArgs e)
        {
            if (chkSelectAll.Checked)
                PopulateItems(_allModels);
            else
                PopulateItems(_unUpatedModels);
        }

        #region Nested type: Item

        private class Item
        {
            private readonly CandleModel _model;

            /// <summary>
            /// Initializes a new instance of the <see cref="Item"/> class.
            /// </summary>
            /// <param name="model">The model.</param>
            public Item(CandleModel model)
            {
                _model = model;
            }

            /// <summary>
            /// Gets the model.
            /// </summary>
            /// <value>The model.</value>
            public CandleModel Model
            {
                get { return _model; }
            }

            /// <summary>
            /// Returns a <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
            /// </summary>
            /// <returns>
            /// A <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
            /// </returns>
            public override string ToString()
            {
                return String.Concat(_model.Name, " (", _model.Version.ToString(), ")");
            }
        }

        #endregion
    }
}