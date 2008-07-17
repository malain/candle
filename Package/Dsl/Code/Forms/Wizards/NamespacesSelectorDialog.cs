using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace DSLFactory.Candle.SystemModel
{
    /// <summary>
    /// 
    /// </summary>
    public partial class NamespacesSelectorDialog : Form
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NamespacesSelectorDialog"/> class.
        /// </summary>
        /// <param name="packages">The packages.</param>
        public NamespacesSelectorDialog(IList<Package> packages)
        {
            InitializeComponent();

            foreach (Package package in packages)
            {
                lstPackages.Items.Add(package.Name);
            }
        }

        /// <summary>
        /// Gets the selected namespace.
        /// </summary>
        /// <value>The selected namespace.</value>
        public string SelectedNamespace
        {
            get { return lstPackages.SelectedItem.ToString(); }
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