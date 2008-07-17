using System;
using System.Windows.Forms;

namespace DSLFactory.Candle.SystemModel.Rules.Wizards
{
    /// <summary>
    /// 
    /// </summary>
    public partial class AssociationPropertiesSelectorForm : Form
    {
        private readonly Association _association;

        /// <summary>
        /// Fenetre de propriétés d'une association. Affichage des mappings de clé étrangère
        /// </summary>
        /// <param name="association">The association.</param>
        public AssociationPropertiesSelectorForm(Association association)
        {
            _association = association;

            InitializeComponent();

            // Vérification des clés primaires
            foreach (Property primaryKey in association.Target.PrimaryKeys)
            {
                ForeignKey fk = null;

                bool skip = false;
                foreach (ForeignKey foreignKey in association.ForeignKeys)
                {
                    if (foreignKey.PrimaryKey == null)
                        skip = true;
                    if (foreignKey.PrimaryKey == primaryKey)
                    {
                        fk = foreignKey;
                        break;
                    }
                }

                // Si la foreignKey n'est pas correctement renseigné et qu'on a pas trouvé de correspondance
                // on ne fait rien sinon on crée une ligne suplémentaire
                if (fk == null && skip == false)
                {
                    fk = new ForeignKey(association.Store);
                    fk.PrimaryKey = primaryKey;
                    association.ForeignKeys.Add(fk);
                }
            }

            // Initialisation des combo box avec la liste des diffèrents champs 
            ColForeignKey.DataSource = association.Source.Properties;
            ColPrimaryKey.DataSource = association.Target.Properties;

            // On ajoute les lignes à la datagrid en plus de celle initiale (pour permettre l'ajout)
            dgForeignKeys.Rows.Add(Math.Max(association.ForeignKeys.Count, 1));

            // Nom des tables
            lblSource.Text = association.Source.Name;
            lblTarget.Text = association.Target.Name;
        }

        /// <summary>
        /// Mise à jour d'une cellule
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Forms.DataGridViewCellValueEventArgs"/> instance containing the event data.</param>
        private void dgForeignKeys_CellValuePushed(object sender, DataGridViewCellValueEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < _association.ForeignKeys.Count)
            {
                ForeignKey fk = _association.ForeignKeys[e.RowIndex];
                DataGridViewComboBoxCell cb =
                    (DataGridViewComboBoxCell) dgForeignKeys.Rows[e.RowIndex].Cells[e.ColumnIndex];
                foreach (Property p in cb.Items)
                {
                    if (p.Name == (string) e.Value)
                    {
                        if (e.ColumnIndex == 0)
                        {
                            fk.Column = p;
                        }
                        else
                        {
                            fk.PrimaryKey = p;
                        }
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Handles the CellValueNeeded event of the dgForeignKeys control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Forms.DataGridViewCellValueEventArgs"/> instance containing the event data.</param>
        private void dgForeignKeys_CellValueNeeded(object sender, DataGridViewCellValueEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < _association.ForeignKeys.Count)
            {
                ForeignKey fk = _association.ForeignKeys[e.RowIndex];
                if (e.ColumnIndex == 0)
                    e.Value = fk.Column;
                else
                    e.Value = fk.PrimaryKey;
            }
        }

        /// <summary>
        /// Handles the DataError event of the dgForeignKeys control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Forms.DataGridViewDataErrorEventArgs"/> instance containing the event data.</param>
        private void dgForeignKeys_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
        }

        /// <summary>
        /// Suppression de la ligne du datagrid par la touche 'Delete'
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Forms.KeyEventArgs"/> instance containing the event data.</param>
        private void dgForeignKeys_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete && dgForeignKeys.SelectedRows.Count > 0)
                dgForeignKeys.Rows.Remove(dgForeignKeys.SelectedRows[0]);
        }

        /// <summary>
        /// Suupression d'une ligne
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Forms.DataGridViewRowsRemovedEventArgs"/> instance containing the event data.</param>
        private void dgForeignKeys_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            _association.ForeignKeys.RemoveAt(e.RowIndex);
        }

        /// <summary>
        /// Ajout d'une nouvelle ligne
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Forms.DataGridViewRowsAddedEventArgs"/> instance containing the event data.</param>
        private void dgForeignKeys_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            _association.ForeignKeys.Add(new ForeignKey(_association.Store));
        }

        /// <summary>
        /// Suppression des lignes non completes avant de sortir
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void buttonOK_Click(object sender, EventArgs e)
        {
            ForeignKey foreignKeyToDelete = null;
            do
            {
                foreignKeyToDelete = null;
                foreach (ForeignKey fk in _association.ForeignKeys)
                {
                    if (fk.PrimaryKey == null || fk.Column == null)
                    {
                        foreignKeyToDelete = fk;
                        break;
                    }
                }
                if (foreignKeyToDelete != null)
                {
                    _association.ForeignKeys.Remove(foreignKeyToDelete);
                }
            } while (foreignKeyToDelete != null);
        }
    }
}