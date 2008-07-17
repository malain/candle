using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace DSLFactory.Candle.SystemModel.Strategies
{
    /// <summary>
    /// Fenetre de sélection des fichiers sources
    /// </summary>
    public partial class SelectFile : Form
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SelectFile"/> class.
        /// </summary>
        /// <param name="files1">The files1.</param>
        /// <param name="files2">The files2.</param>
        public SelectFile(List<string> files1, List<string> files2)
        {
            InitializeComponent();

            lstFiles1.Items.Clear();
            foreach (string file in files1)
            {
                lstFiles1.Items.Add(new FileItem(file));
            }
            lstFiles2.Items.Clear();
            foreach (string file in files2)
            {
                lstFiles2.Items.Add(new FileItem(file));
            }
        }

        /// <summary>
        /// Gets the selected file.
        /// </summary>
        /// <value>The selected file.</value>
        public string SelectedFile
        {
            get
            {
                FileItem item = lstFiles1.SelectedItem as FileItem;
                if (item != null)
                    return item.FileName;

                item = lstFiles2.SelectedItem as FileItem;
                return item != null ? item.FileName : null;
            }
        }

        /// <summary>
        /// Handles the SelectedIndexChanged event of the lstFiles control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void lstFiles_SelectedIndexChanged(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }

        #region Nested type: FileItem

        /// <summary>
        /// 
        /// </summary>
        private class FileItem
        {
            private readonly string _fileName;

            /// <summary>
            /// Initializes a new instance of the <see cref="FileItem"/> class.
            /// </summary>
            /// <param name="fileName">Name of the file.</param>
            public FileItem(string fileName)
            {
                _fileName = fileName;
            }

            /// <summary>
            /// Gets the name of the file.
            /// </summary>
            /// <value>The name of the file.</value>
            public string FileName
            {
                get { return _fileName; }
            }

            /// <summary>
            /// Returns a <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
            /// </summary>
            /// <returns>
            /// A <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
            /// </returns>
            public override string ToString()
            {
                return Utils.StripString(_fileName, 100);
            }
        }

        #endregion
    }
}