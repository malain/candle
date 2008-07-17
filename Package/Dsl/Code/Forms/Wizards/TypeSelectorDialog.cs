using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace DSLFactory.Candle.SystemModel
{
    /// <summary>
    /// 
    /// </summary>
    public partial class TypeSelectorDialog : Form
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TypeSelectorDialog"/> class.
        /// </summary>
        /// <param name="types">The types.</param>
        public TypeSelectorDialog(List<DataType> types)
        {
            InitializeComponent();
            lstTypes.ValueMember = "Name";
            lstTypes.DisplayMember = "FullName";

            foreach (DataType type in types)
            {
                lstTypes.Items.Add(type);
            }
        }

        /// <summary>
        /// Gets the type of the selected.
        /// </summary>
        /// <value>The type of the selected.</value>
        public string SelectedType
        {
            get { return lstTypes.SelectedValue.ToString(); }
        }

        /// <summary>
        /// Handles the SelectedIndexChanged event of the lstTypes control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void lstTypes_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnSelect.Enabled = true;
        }
    }
}