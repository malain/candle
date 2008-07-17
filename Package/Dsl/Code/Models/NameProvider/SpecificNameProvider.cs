using System.Collections.Generic;
using Microsoft.VisualStudio.Modeling;

namespace DSLFactory.Candle.SystemModel.NameProvider
{
    /// <summary>
    /// Générateur de nom unique modifié pour permettre que le premier nom ne contiennent pas de suffixe à
    /// la diffèrence du générateur par défaut qui commence tjs par générer un nom avec le suffixe '1'.
    /// </summary>
    /// <remarks>
    /// Pour l'activer, il faut mettre à jour dans le designer la propriété 'Element Name Provider' sur la propriété
    /// Name de la classe du domaine concerné
    /// </remarks>
    internal class SpecificNameProvider : ElementNameProvider
    {
        /// <summary>
        /// Interception de la génération d'un nom unique si le nom proposé n'est pas encore utilisé,
        /// on le prend sinon on rajoute un suffixe.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <param name="baseName">The name for the element.</param>
        /// <param name="siblingNames">Names that can be added to the base name.</param>
        protected override void SetUniqueNameCore(ModelElement element, string baseName,
                                                  IDictionary<string, ModelElement> siblingNames)
        {
            if (!siblingNames.ContainsKey(baseName))
            {
                DomainProperty.SetValue(element, baseName);
                return;
            }
            base.SetUniqueNameCore(element, baseName, siblingNames);
        }
    }
}