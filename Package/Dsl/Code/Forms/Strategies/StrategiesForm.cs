using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.VisualStudio.Modeling;

namespace DSLFactory.Candle.SystemModel.Strategies
{
    /// <summary>
    /// Fenetre listant les stratégies d'un modèle
    /// </summary>
    public partial class StrategiesForm : Form
    {
        private StrategiesListControl globalsStrategies;
        private LanguageConfigurationControl languageConfig;
        private NamingStrategyControl namingStrategy;
        private List<StrategyRemovedEventArgs> pendingRemovedStrategies = new List<StrategyRemovedEventArgs>();
        private List<StrategiesListControl> specificStrategies;

        private readonly Store _store;

        /// <summary>
        /// Initializes a new instance of the <see cref="StrategiesForm"/> class.
        /// </summary>
        /// <param name="component">The component.</param>
        /// <param name="model">The model.</param>
        public StrategiesForm(SoftwareComponent component, CandleElement model)
        {
            InitializeComponent();

            if (component != null)
                _store = component.Store;
            else
                _store = model.Store;

            specificStrategies = new List<StrategiesListControl>();
            globalsStrategies = new StrategiesListControl();
            namingStrategy = new NamingStrategyControl();
            languageConfig = new LanguageConfigurationControl();

            globalsStrategies.Dock = DockStyle.Fill;
            globalsStrategies.Name = "globalsStrategies";
            globalsStrategies.TabIndex = 1;

            namingStrategy.Dock = DockStyle.Fill;
            namingStrategy.Name = "namingStrategy";
            namingStrategy.TabIndex = 1;

            languageConfig.Dock = DockStyle.Fill;
            languageConfig.Name = "languageConfig";
            languageConfig.TabIndex = 1;

            tabStrategies.TabPages.Clear();

            // Création des onglets (une par couche + une globale + une pour la stratégie de nommage)
            // L'onglet courant est sélectionné

            // D'abord le global
            globalsStrategies.Initialize(_store, null);
            tabGlobals.Controls.Add(globalsStrategies);
            tabStrategies.TabPages.Add(tabGlobals);

            // Puis un par couche
            int index = 1;
            foreach (SoftwareLayer layer in component.Layers)
            {
                StrategiesListControl specificStrategy = new StrategiesListControl();
                specificStrategy.Dock = DockStyle.Fill;
                specificStrategy.Name = String.Format("specificStrategies{0}", index);
                specificStrategy.TabIndex = 0;
                specificStrategy.Initialize(_store, layer);
                specificStrategy.StrategyRemoved += Strategies_StrategyRemoved;

                TabPage tabSpecific = new TabPage();
                tabStrategies.TabPages.Add(tabSpecific);

                tabSpecific.Location = new Point(4, 22);
                tabSpecific.Name = String.Format("tabSpecific{0}", index);
                tabSpecific.Padding = new Padding(3);
                tabSpecific.Size = new Size(741, 348);
                tabSpecific.TabIndex = index;
                tabSpecific.UseVisualStyleBackColor = true;

                tabSpecific.Text = layer.Name;
                if (layer == model.StrategiesOwner)
                {
                    tabSpecific.Text += "*";
                    tabStrategies.SelectedTab = tabSpecific;
                }
                tabSpecific.Controls.Add(specificStrategy);
            }


            // Stratégie de nommage
            namingStrategy.Initialize(StrategyManager.GetInstance(_store).NamingStrategy);
            tabNaming.Controls.Add(namingStrategy);
            tabStrategies.TabPages.Add(tabNaming);

            // Et le language
            languageConfig.Initialize(StrategyManager.GetInstance(_store).TargetLanguage);
            tabLanguage.Controls.Add(languageConfig);
            tabStrategies.TabPages.Add(tabLanguage);

            globalsStrategies.StrategyRemoved += new EventHandler<StrategyRemovedEventArgs>(Strategies_StrategyRemoved);
        }

        /// <summary>
        /// Mise en attente de la fermeture de la fenetre des stratégies
        /// à supprimer
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="DSLFactory.Candle.SystemModel.Strategies.StrategyRemovedEventArgs"/> instance containing the event data.</param>
        private void Strategies_StrategyRemoved(object sender, StrategyRemovedEventArgs e)
        {
            if (
                !pendingRemovedStrategies.Exists(
                     delegate(StrategyRemovedEventArgs s) { return s.Strategy.StrategyId == e.Strategy.StrategyId && e.Owner == s.Owner; }))
            {
                pendingRemovedStrategies.Add(e);
            }
        }

        /// <summary>
        /// Bouton OK. Vérification qu'il n'y ait pas de doublons dans les ID et que ceux ci soient valides.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void btnOK_Click(object sender, EventArgs e)
        {
            foreach (StrategiesListControl ctrl in specificStrategies)
            {
                if (!ctrl.CommitChanges())
                    return;
            }

            if (globalsStrategies.CommitChanges())
            {
                // TODO verif doublons entre les deux tabpages.
                RemoveStrategiesCode();
                StrategyManager.GetInstance(_store).Save(_store);
                DialogResult = DialogResult.OK;
            }
        }

        /// <summary>
        /// Suppression du code généré par les strategies supprimées
        /// </summary>
        private void RemoveStrategiesCode()
        {
            foreach (StrategyRemovedEventArgs e in pendingRemovedStrategies)
            {
                if (
                    MessageBox.Show(
                        String.Format("Do you want to suppress all generated code for strategy {0}",
                                      e.Strategy.DisplayName), "Remove strategy generated code ?",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    Mapper.Instance.RemoveStrategyGeneratedFiles(e.Strategy.StrategyId, e.Owner);
                }
            }
        }
    }
}