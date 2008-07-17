using System.Windows.Forms;
using DSLFactory.Candle.SystemModel.Rules.Wizards;
using Microsoft.VisualStudio.Modeling;

namespace DSLFactory.Candle.SystemModel.Rules
{
    /// <summary>
    /// Création d'un port dans la DAL associé à cette entité
    /// </summary>
    [RuleOn(typeof (SoftwareComponent), FireTime=TimeToFire.TopLevelCommit, InitiallyDisabled=false)]
    public class SoftwareComponentInsertRule : AddRule
    {
        // Ajout de l'élément
        /// <summary>
        /// Alerts listeners that a rule has been used.
        /// </summary>
        /// <param name="e">An ElementAddedEventArgs that contains the event data.</param>
        public override void ElementAdded(ElementAddedEventArgs e)
        {
            #region Condition

            // Test the element
            SoftwareComponent component = e.ModelElement as SoftwareComponent;
            if (component == null)
                return;

            // Teste si on est en train de charger le modèle
            if (component.Store.TransactionManager.CurrentTransaction.TopLevelTransaction.IsSerializing ||
                component.Store.InUndoRedoOrRollback ||
                component.Store.TransactionManager.CurrentTransaction.Context.ContextInfo.ContainsKey(
                    "InitializeComponentWizard"))
                return;

            #endregion

            #region Traitement

            ShowComponentWizard(component);

            #endregion
        }

        /// <summary>
        /// Shows the component wizard.
        /// </summary>
        /// <param name="component">The component.</param>
        /// <remarks>
        /// Ce traitement peut-être appelé soit lors de l'insertion d'un composant avec le designer soit à l'ouverture d'un modèle dont
        /// le nom du composant est un '?' (cas du template). Dans ce dernier cas, on ne passe pas par une règle car une règle déclenchée
        /// lors de l'ouverture du document ne notifie pas le designer des changements (car le designer ne s'aboone aux notifications qu'après
        /// le chargement du doc.
        /// </remarks>
        public static void ShowComponentWizard(SoftwareComponent component)
        {
            if (component.Name != "?")
                return;

            component.Store.TransactionManager.CurrentTransaction.Context.ContextInfo["InitializeComponentWizard"] =
                true;

            // Wizard pour demander le namespace et la version
            ApplicationNamespaceForm form = new ApplicationNamespaceForm(component);
            if (form.ShowDialog() != DialogResult.OK)
            {
                throw new CanceledByUser();
            }

            // Demande du namespace
            component.Namespace = form.Namespace;
            component.Name = form.ApplicationName;

            // Mise à jour de la définition du système
            CandleModel def = component.Model;
            if (def != null)
            {
                def.IsLibrary = form.IsLibrary;
                def.Version = form.Version;
                def.Comment = form.Description;
                def.Url = form.URL;
                def.Name = form.ApplicationName;
                def.Path = form.DomainPath;
            }
        }
    }
}