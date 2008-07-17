using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Diagrams;

namespace DSLFactory.Candle.SystemModel.Rules
{
    /// <summary>
    /// Lors de l'insertion d'un package layer, on veut forcer la création de son shape avant tous les autres
    /// pour pouvoir le mettre comme parent d'un LayerShape 
    /// </summary>
    [RuleOn(typeof (LayerPackage), FireTime=TimeToFire.LocalCommit, InitiallyDisabled=false)]
    public class LayerPackageInsertRule : AddRule
    {
        // Ajout de l'élément
        /// <summary>
        /// Alerts listeners that a rule has been used.
        /// </summary>
        /// <param name="e">An ElementAddedEventArgs that contains the event data.</param>
        public override void ElementAdded(ElementAddedEventArgs e)
        {
            // Test the element
            LayerPackage model = e.ModelElement as LayerPackage;
            if (model == null)
                return;

            Diagram.FixUpDiagram(model.Component, model);
        }
    }
}