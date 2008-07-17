using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace DSLFactory.Candle.SystemModel
{
    /// <summary>
    /// Fenetre permettant de faire saisir une information par l'utilisateur
    /// </summary>
    public partial class PromptBox : Form
    {
        private string filter;

        /// <summary>
        /// Fenetre d'interrogation
        /// </summary>
        /// <param name="texte">Texte de la question</param>
        public PromptBox(string texte) : this(texte, null)
        {
        }

        /// <summary>
        /// Fenetre d'interrogation permettant de choisir un fichier
        /// </summary>
        /// <param name="texte">Texte de la question</param>
        /// <param name="filter">filtre des fichiers sous la forme texte|*.dll[|texte2|filtre2]</param>
        public PromptBox(string texte, string filter)
        {
            this.filter = filter;
            InitializeComponent();
            lblText.Text = texte;
            btnBrowse.Visible = filter != null;
        }

        /// <summary>
        /// Récupération de la valeur choisie
        /// </summary>
        public string Value
        {
            get { return txtValue.Text; }
        }

        /// <summary>
        /// Clic sur le bouton de sélection de fichier
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnBrowse_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Multiselect = false;
            ofd.CheckFileExists = true;
            ofd.Filter = filter;
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                txtValue.Text = ofd.FileName;
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if( txtValue.Text.Length == 0 )
            {
                errorProvider.SetError(txtValue, "Required");
                return;
            }
            this.DialogResult = DialogResult.OK;
        }
    }
}