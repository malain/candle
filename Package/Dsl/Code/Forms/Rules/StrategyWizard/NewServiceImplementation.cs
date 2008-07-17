using System;
using System.Collections.Generic;
using System.Windows.Forms;
using DSLFactory.Candle.SystemModel.Strategies;
using Microsoft.VisualStudio.Modeling;

namespace DSLFactory.Candle.SystemModel.Rules.Wizards
{
    /// <summary>
    /// Wizard appelé lors de l'insertion d'une classe ou d'un contrat
    /// </summary>
    public partial class NewServiceImplementation : Form, IStrategyWizard
    {
        /// <summary>
        /// Permet de ne plus afficher cette fenetre. (Ne marche que pour la session courante)
        /// </summary>
        private static bool s_dontShow = false;

        private InterfaceLayer _iLayer;
        private SoftwareLayer _layer;

        /// <summary>
        /// Initializes a new instance of the <see cref="NewServiceImplementation"/> class.
        /// </summary>
        public NewServiceImplementation()
        {
            InitializeComponent();
        }

        #region IStrategyWizard Members

        /// <summary>
        /// Execution du wizard
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="DSLFactory.Candle.SystemModel.Strategies.StrategyElementElementAddedEventArgs"/> instance containing the event data.</param>
        public void RunWizard(ModelElement sender, StrategyElementElementAddedEventArgs e)
        {
            CandleElement elem = e.ModelElement as CandleElement;

            txtRootName.Text = elem.RootName;

            if (elem is ServiceContract) // Ce cas est désactivé (voir selection du wizard)
            {
                _layer = ((ServiceContract) e.ModelElement).Layer;
                txtName.Text =
                    StrategyManager.GetInstance(_layer.Store).NamingStrategy.CreateElementName(_layer, elem.RootName);
            }
            else
            {
                _layer = ((ClassImplementation) e.ModelElement).Layer;
                _iLayer = ((ClassImplementation) e.ModelElement).Layer.LayerPackage.InterfaceLayer;
                txtName.Text =
                    StrategyManager.GetInstance(_layer.Store).NamingStrategy.CreateElementName(_layer, elem.RootName);
                if (_iLayer == null)
                    txtContractName.Visible = false;
                else
                    txtContractName.Text =
                        StrategyManager.GetInstance(_layer.Store).NamingStrategy.CreateElementName(_iLayer, txtName.Text);
            }

            if (elem is ServiceContract || ((ClassImplementation) elem).Layer.LayerPackage.InterfaceLayer == null)
            {
                lblContractName.Visible = false;
                txtContractName.Visible = false;
                txtContractName.Text = null;
            }

            lblHeader.Text = String.Format(lblHeader.Text, _layer.Name);
            groupBox1.Text = _layer.Namespace;

            if (!s_dontShow)
            {
                e.UserCancel = (ShowDialog() == DialogResult.Cancel);
                if (e.UserCancel)
                    return;
                s_dontShow = ckDontShow.Checked;
            }

            // Ici on force les noms des classes donc on ne veut pas que la régle basée sur la modification
            // du RootName s'execute. On l'indique dans le contexte de la transaction
            if (
                !elem.Store.TransactionManager.CurrentTransaction.TopLevelTransaction.Context.ContextInfo.ContainsKey(
                     "CustomizableElementChangeRule_Enabled"))
                elem.Store.TransactionManager.CurrentTransaction.TopLevelTransaction.Context.ContextInfo.Add(
                    "CustomizableElementChangeRule_Enabled", false);
            elem.Name = txtName.Text;

            using (Transaction transaction = elem.Store.TransactionManager.BeginTransaction("Set root name"))
            {
                // Force la transaction pour que la règle s'execute tout de suite et qu'on puisse
                // forcer le nom ensuite
                elem.RootName = txtRootName.Text;
                transaction.Commit();
            }

            // Si c'est une classe, on essaye de créer son interface
            ClassImplementation clazz = elem as ClassImplementation;
            if (clazz != null && _iLayer != null && !String.IsNullOrEmpty(txtContractName.Text))
            {
                if (clazz.Contract == null)
                {
                    // On regarde si l'interface n'existe pas
                    clazz.Contract =
                        _iLayer.ServiceContracts.Find(
                            delegate(ServiceContract c) { return c.Name == txtContractName.Text; });

                    if (clazz.Contract == null)
                    {
                        clazz.Contract = new ServiceContract(clazz.Store);
                        clazz.Contract.RootName = txtRootName.Text;
                        clazz.Layer.LayerPackage.InterfaceLayer.ServiceContracts.Add(clazz.Contract);
                        UnplacedModelHelper.RegisterNewModel(clazz.Store, clazz.Contract);

                        // Si la classe courante utilise un seul contract, on le recopie
                        IList<ClassUsesOperations> links = ClassUsesOperations.GetLinksToServicesUsed(clazz);
                        if (links.Count == 1)
                        {
                            ServiceContract contract = links[0].TargetService as ServiceContract;
                            if (contract != null)
                                TypeWithOperations.CopyOperations(contract, clazz.Contract);
                            else
                            {
                                ExternalServiceContract externalContract =
                                    links[0].TargetService as ExternalServiceContract;
                                if (externalContract != null)
                                    TypeWithOperations.CopyOperations(externalContract.ReferencedServiceContract,
                                                                      clazz.Contract);
                            }
                        }
                    }
                }

                using (Transaction transaction = elem.Store.TransactionManager.BeginTransaction("Set root name"))
                {
                    // Force la transaction pour que la règle s'execute tout de suite et qu'on puisse
                    // forcer le nom ensuite
                    clazz.Contract.RootName = elem.RootName;
                    transaction.Commit();
                }

                if (clazz.Contract.Name != txtContractName.Text)
                    clazz.Contract.Name = txtContractName.Text;
            }

            e.CancelBubble = true;
        }

        #endregion

        /// <summary>
        /// Handles the Click event of the buttonOK control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void buttonOK_Click(object sender, EventArgs e)
        {
            if (!StrategyManager.GetInstance(_layer.Store).NamingStrategy.IsClassNameValid(txtRootName.Text))
            {
                errors.SetError(txtRootName, "Nom de classe invalide");
                return;
            }
            Hide();
        }

        /// <summary>
        /// Handles the KeyUp event of the txtRootName control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Forms.KeyEventArgs"/> instance containing the event data.</param>
        private void txtRootName_KeyUp(object sender, KeyEventArgs e)
        {
            txtName.Text =
                StrategyManager.GetInstance(_layer.Store).NamingStrategy.CreateElementName(_layer, txtRootName.Text);
            if (_iLayer != null)
                txtContractName.Text =
                    StrategyManager.GetInstance(_layer.Store).NamingStrategy.CreateElementName(_iLayer, txtName.Text);
        }

        /// <summary>
        /// Handles the KeyUp event of the txtName control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Windows.Forms.KeyEventArgs"/> instance containing the event data.</param>
        private void txtName_KeyUp(object sender, KeyEventArgs e)
        {
            if (_iLayer != null)
                txtContractName.Text =
                    StrategyManager.GetInstance(_layer.Store).NamingStrategy.CreateElementName(_iLayer, txtName.Text);
        }
    }
}