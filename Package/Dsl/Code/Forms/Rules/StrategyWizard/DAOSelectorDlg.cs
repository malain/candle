using System;
using System.Windows.Forms;
using DSLFactory.Candle.SystemModel.Strategies;
using Microsoft.VisualStudio.Modeling;

namespace DSLFactory.Candle.SystemModel.Rules.Wizards
{
    /// <summary>
    /// Fenetre permettant de sélectionner la classe DAO associée à une entité
    /// </summary>
    public partial class DAOSelectorDlg : Form, IStrategyWizard
    {
        private readonly DataAccessLayer _dataAccessLayer;
        private readonly Entity _entity;

        /// <summary>
        /// Initializes a new instance of the <see cref="DAOSelectorDlg"/> class.
        /// </summary>
        /// <param name="dal">The dal.</param>
        /// <param name="entity">The entity.</param>
        public DAOSelectorDlg(DataAccessLayer dal, Entity entity)
        {
            InitializeComponent();

            _entity = entity;
            if (String.IsNullOrEmpty(entity.RootName))
                entity.RootName = entity.Name;
            lblHeader.Text = String.Format(lblHeader.Text, entity.FullName);
            _dataAccessLayer = dal;

            foreach (ClassImplementation clazz in dal.Classes)
            {
                if (clazz.AssociatedEntity == null)
                    cbDAO.Items.Add(clazz);
            }

            txtDAOName.Text =
                StrategyManager.GetInstance(dal.Store).NamingStrategy.CreateElementName(dal, entity.RootName);

            if (cbDAO.Items.Count == 0)
            {
                rbSelect.Enabled = false;
                rbNew.Checked = true;
            }
            else
            {
                int pos = cbDAO.FindStringExact(txtDAOName.Text);
                if (pos >= 0)
                    cbDAO.SelectedIndex = pos;
            }
        }

        #region IStrategyWizard Members

        /// <summary>
        /// Runs the wizard.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="DSLFactory.Candle.SystemModel.Strategies.StrategyElementElementAddedEventArgs"/> instance containing the event data.</param>
        public void RunWizard(ModelElement sender, StrategyElementElementAddedEventArgs e)
        {
            Entity entity = sender as Entity;
            if (entity != null)
            {
                // Si il y a une couche DAL, on crée le port correspondant
                DataAccessLayer dal =
                    (DataAccessLayer)
                    entity.DataLayer.Component.Layers.Find(
                        delegate(SoftwareLayer layer) { return layer is DataAccessLayer; });
                if (dal != null)
                {
                    DAOSelectorDlg dlg = new DAOSelectorDlg(dal, entity);
                    e.UserCancel = dlg.ShowDialog() == DialogResult.Cancel;
                    if (e.UserCancel)
                        return;

                    dlg.CreateDAO();
                }
            }
        }

        #endregion

        /// <summary>
        /// Handles the Click event of the buttonOK control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void buttonOK_Click(object sender, EventArgs e)
        {
            if (rbNew.Checked &&
                !StrategyManager.GetInstance(_dataAccessLayer.Store).NamingStrategy.IsClassNameValid(txtDAOName.Text))
            {
                errors.SetError(txtDAOName, "Invalid class name");
                return;
            }
            if (rbNew.Checked &&
                _dataAccessLayer.Classes.Find(
                    delegate(ClassImplementation clazz) { return clazz.Name == txtDAOName.Text; }) != null)
            {
                errors.SetError(txtDAOName, "Class already exists");
                return;
            }
            if (rbSelect.Checked && cbDAO.SelectedItem == null)
            {
                errors.SetError(cbDAO, "Your must select a class");
                return;
            }
            Hide();
        }

        /// <summary>
        /// Creates the DAO.
        /// </summary>
        public void CreateDAO()
        {
            if (rbNone.Checked)
                return;

            if (rbSelect.Checked)
            {
                ClassImplementation port = cbDAO.SelectedItem as ClassImplementation;
                port.AssociatedEntity = _entity;
            }
            else if (rbNew.Checked)
            {
                ClassImplementation port = new ClassImplementation(_dataAccessLayer.Store);
                port.Name = txtDAOName.Text;
                port.RootName = _entity.RootName;
                port.AssociatedEntity = _entity;
                _dataAccessLayer.Classes.Add(port);

                UnplacedModelHelper.RegisterNewModel(_dataAccessLayer.Store, port);
            }
        }

        /// <summary>
        /// Shows the area.
        /// </summary>
        /// <param name="flag">The flag.</param>
        private void ShowArea(byte flag)
        {
            cbDAO.Enabled = (flag & 1) == 1;
            lbExisting.Enabled = (flag & 1) == 1;
            txtDAOName.Enabled = (flag & 2) == 2;
            lbCreate.Enabled = (flag & 2) == 2;
        }

        /// <summary>
        /// Handles the CheckedChanged event of the rbSelect control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void rbSelect_CheckedChanged(object sender, EventArgs e)
        {
            if (rbSelect.Checked)
                ShowArea(1);
            else if (rbNew.Checked)
                ShowArea(2);
            else
                ShowArea(4);
        }
    }
}