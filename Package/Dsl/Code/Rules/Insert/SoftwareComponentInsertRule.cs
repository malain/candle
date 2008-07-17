using System.Windows.Forms;
using DSLFactory.Candle.SystemModel.Rules.Wizards;
using Microsoft.VisualStudio.Modeling;

namespace DSLFactory.Candle.SystemModel.Rules
{
    /// <summary>
    /// Cr�ation d'un port dans la DAL associ� � cette entit�
    /// </summary>
    [RuleOn(typeof (SoftwareComponent), FireTime=TimeToFire.TopLevelCommit, InitiallyDisabled=false)]
    public class SoftwareComponentInsertRule : AddRule
    {
        // Ajout de l'�l�ment
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

            // Teste si on est en train de charger le mod�le
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
        /// Ce traitement peut-�tre appel� soit lors de l'insertion d'un composant avec le designer soit � l'ouverture d'un mod�le dont
        /// le nom du composant est un '?' (cas du template). Dans ce dernier cas, on ne passe pas par une r�gle car une r�gle d�clench�e
        /// lors de l'ouverture du document ne notifie pas le designer des changements (car le designer ne s'aboone aux notifications qu'apr�s
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

            // Mise � jour de la d�finition du syst�me
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