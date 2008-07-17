using Microsoft.VisualStudio.Modeling;
using DslModeling=Microsoft.VisualStudio.Modeling;
using DslDiagrams=Microsoft.VisualStudio.Modeling.Diagrams;

namespace DSLFactory.Candle.SystemModel
{
    /// <summary>
    /// Bidouille :
    /// Bug dans le dsl qui ne permet pas de sérialiser correctement dans un sous-diagrammes un lien qui ne possédent pas la 
    /// propriété AllowDuplicates à true.
    /// Pour cette raison, on permet des connections multiples dans le modèle mais on va l'empecher à l'exécution.
    /// </summary>
    /// <remarks>
    /// Pour corriger ce bug, le modèle contient deux relations entre les Entity:
    ///   - EntityHasSuperClasses : Qui n'est définie que pour fonctionner graphiquement. Elle accepte la duplication et correspond au
    ///   GeneralizationLink. 
    ///   - Generalization : Qui est utilisée pour manipuler le modèle et qui est un lien sur la SuperClass
    /// </remarks>
    partial class GeneralizationBuilder
    {
        /// <summary>
        /// Determines whether this instance [can accept source] the specified source element.
        /// </summary>
        /// <param name="sourceElement">The source element.</param>
        /// <returns>
        /// 	<c>true</c> if this instance [can accept source] the specified source element; otherwise, <c>false</c>.
        /// </returns>
        internal static bool CanAcceptSource(Microsoft.VisualStudio.Modeling.ModelElement sourceElement)
        {
            return sourceElement is Entity;
        }

        /// <summary>
        /// Determines whether this instance [can accept source and target] the specified source element.
        /// </summary>
        /// <param name="sourceElement">The source element.</param>
        /// <param name="targetElement">The target element.</param>
        /// <returns>
        /// 	<c>true</c> if this instance [can accept source and target] the specified source element; otherwise, <c>false</c>.
        /// </returns>
        internal static bool CanAcceptSourceAndTarget(Microsoft.VisualStudio.Modeling.ModelElement sourceElement, Microsoft.VisualStudio.Modeling.ModelElement targetElement)
        {
            // On ne permet une connection que si il n'y en a pas.
            if (sourceElement is Entity)
            {
                if (targetElement is Entity)
                    return EntityHasSubClasses.GetLinks((Entity)sourceElement, (Entity)targetElement).Count == 0;
            }
            return false;
        }

        /// <summary>
        /// Connects the specified source element.
        /// </summary>
        /// <param name="sourceElement">The source element.</param>
        /// <param name="targetElement">The target element.</param>
        internal static void Connect(Microsoft.VisualStudio.Modeling.ModelElement sourceElement, Microsoft.VisualStudio.Modeling.ModelElement targetElement)
        {
            if (CanAcceptSourceAndTarget(sourceElement, targetElement))
            {
                Entity source = (Entity)sourceElement;
                Entity target = (Entity)targetElement;
                new EntityHasSubClasses(source, target);
                source.SuperClass = target;
            }
        }
    }
}

namespace DSLFactory.Candle.SystemModel.Rules
{
    /// <summary>
    /// On s'assure que les liens restent cohérents entre eux
    /// </summary>
    [RuleOn(typeof(Generalization), FireTime = TimeToFire.Inline)]
    internal class GeneralizationSuperClassChangeRole : Microsoft.VisualStudio.Modeling.RolePlayerChangeRule
    {
        /// <summary>
        /// </summary>
        /// <param name="e">Provides data for the RolePlayerChanged event.</param>
        public override void RolePlayerChanged(RolePlayerChangedEventArgs e)
        {
            // Modification de la superClass
            if (e.DomainRole.Id == Generalization.SuperClassDomainRoleId)
            {
                Generalization link = e.ElementLink as Generalization;
                if (link.SuperClass.SubClasses.Count > 0)
                    link.SuperClass.SubClasses.RemoveAt(0);
                link.SuperClass.SubClasses.Add(link.SubClass);
            }
        }
    }
}
