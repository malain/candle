using System.Collections.Generic;
using Microsoft.VisualStudio.Modeling;

namespace DSLFactory.Candle.SystemModel.NameProvider
{
    /// <summary>
    /// G�n�rateur de nom unique modifi� pour permettre que le premier nom ne contiennent pas de suffixe �
    /// la diff�rence du g�n�rateur par d�faut qui commence tjs par g�n�rer un nom avec le suffixe '1'.
    /// </summary>
    /// <remarks>
    /// Pour l'activer, il faut mettre � jour dans le designer la propri�t� 'Element Name Provider' sur la propri�t�
    /// Name de la classe du domaine concern�
    /// </remarks>
    internal class SpecificNameProvider : ElementNameProvider
    {
        /// <summary>
        /// Interception de la g�n�ration d'un nom unique si le nom propos� n'est pas encore utilis�,
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