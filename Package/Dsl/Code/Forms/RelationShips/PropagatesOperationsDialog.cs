using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows.Forms;

namespace DSLFactory.Candle.SystemModel.Commands
{
    /// <summary>
    /// Sélection des opérations à propoager
    /// </summary>
    public partial class PropagatesOperationsDialog : Form
    {
        /// <summary>
        /// Affichage des types dans les combobox
        /// </summary>
        struct ListItem
        {
            public TypeWithOperations Value;

            /// <summary>
            /// Returns the fully qualified type name of this instance.
            /// </summary>
            /// <returns>
            /// A <see cref="T:System.String"></see> containing a fully qualified type name.
            /// </returns>
            public override string ToString()
            {
                // On prend le nom du type et un niveau de namespace en dessous
                string[] parts = Value.FullName.Split('.');
                if (parts.Length > 2)
                    return string.Join(".", parts, parts.Length - 2, 2);

                return Value.FullName;
            }
        }

        private List<Operation> selectedOperations = new List<Operation>();
        private TypeWithOperations source;
        private TypeWithOperations target;
        private List<TypeWithOperations> sources;
        private List<TypeWithOperations> targets;

        /// <summary>
        /// Initializes a new instance of the <see cref="PropagatesOperationsDialog"/> class.
        /// </summary>
        /// <param name="sources">The sources.</param>
        /// <param name="targets">The targets.</param>
        public PropagatesOperationsDialog(List<TypeWithOperations> sources, List<TypeWithOperations> targets)
        {
            InitializeComponent();
            this.sources = sources;
            this.targets = targets;
            Populate();
        }

        /// <summary>
        /// Initialisation des données à afficher
        /// </summary>
        private void Populate()
        {
            lstOperations.Items.Clear();
            btnOK.Enabled = false;

            PopulateTypeList(cbSource, sources);
            if (cbSource.Items.Count > 0)
                cbSource.SelectedIndex = 0;
            else
                lstOperations.Items.Clear();

            PopulateTypeList(cbTarget, targets);
        }

        /// <summary>
        /// Populates the type list.
        /// </summary>
        /// <param name="combo">The combo.</param>
        /// <param name="list">The list.</param>
        private void PopulateTypeList(ComboBox combo, List<TypeWithOperations> list)
        {
            combo.Items.Clear();
            foreach (TypeWithOperations t in list)
            {
                ListItem item;
                item.Value = t;
                combo.Items.Add(item);
            }
        }        

        /// <summary>
        /// Element destination
        /// </summary>
        public TypeWithOperations Target
        {
            get { return target; }
        }

        /// <summary>
        /// Elément source
        /// </summary>
        public TypeWithOperations Source
        {
            get { return source; }
        }

        /// <summary>
        /// Liste des opérations sélectionnées
        /// </summary>
        public List<Operation> SelectedOperations
        {
            get { return selectedOperations; }
        }

        /// <summary>
        /// Indique si on doit supprimer les opérations existantes sur la cible avant la copie
        /// </summary>
        public bool ClearTargetOperations
        {
            get { return chkClear.Checked; }
        }

        /// <summary>
        /// Handles the Click event of the buttonOK control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void buttonOK_Click(object sender, EventArgs e)
        {
            if (source != null && target != null)
            {
                foreach (Operation op in this.lstOperations.CheckedItems)
                {
                    selectedOperations.Add(op);
                }
                this.Hide();
            }
            else
                MessageBox.Show("You must select a source and a target.");
        }

        /// <summary>
        /// Permutation de la source et de la cible
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void btnSwitch_Click(object sender, EventArgs e)
        {
            source = target = null;

            List<TypeWithOperations> tmp = sources;
            sources = targets;
            targets = tmp;
            Populate();
        }

        /// <summary>
        /// Sélection dans la combobox de la cible
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void cbTarget_SelectedIndexChanged(object sender, EventArgs e)
        {
            target = ((ListItem) cbTarget.SelectedItem).Value;
            btnOK.Enabled = target != null;
        }

        /// <summary>
        /// Sélection dans la combobox de la source
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void cbSource_SelectedIndexChanged(object sender, EventArgs e)
        {
            source = ((ListItem)cbSource.SelectedItem).Value;
            
            lstOperations.Items.Clear();
            foreach (Operation oper in source.Operations)
            {
                lstOperations.Items.Add(oper, true);
            }
        }
    }
}