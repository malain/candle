using System;
using System.Collections.Generic;
using DSLFactory.Candle.SystemModel.Utilities;

namespace DSLFactory.Candle.SystemModel
{
    /// <summary>
    /// Interface pour les mod�les supportant l'affichage dans la fen�tre de d�tail
    /// </summary>
    [CLSCompliant(true)]
    public interface ISupportDetailView : IHasChildren
    {
        /// <summary>
        /// Type par d�faut pour un membre
        /// </summary>
        /// <param name="modelKind">Type de membre</param>
        /// <returns>Un nom de type</returns>
        string GetDefaultType(ModelKind modelKind);

        /// <summary>
        /// Cr�ation de l'instance d'un membre
        /// </summary>
        /// <param name="modelKind">Type de membre</param>
        /// <returns>Instance de mod�le</returns>
        ITypeMember CreateModel(ModelKind modelKind);

        /// <summary>
        /// Initialise les propri�t�s de la fen�tre d�tail
        /// </summary>
        /// <param name="title">Titre de la fenetre</param>
        /// <param name="categories">Liste des cat�gories de membres � afficher ou null si une seule par d�faut</param>
        /// <param name="memberSeparators">Liste des caract�res s�parateurs des membres</param>
        /// <param name="childSeparators">Liste des caract�res s�parateurs des enfants</param>
        void GetEditProperties(out string title, out List<VirtualTreeGridCategory> categories, out string memberSeparators, out string childSeparators);
    }
}
