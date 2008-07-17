using System;
using System.Collections.Generic;
using DSLFactory.Candle.SystemModel.Utilities;

namespace DSLFactory.Candle.SystemModel
{
    /// <summary>
    /// Interface pour les modèles supportant l'affichage dans la fenètre de détail
    /// </summary>
    [CLSCompliant(true)]
    public interface ISupportDetailView : IHasChildren
    {
        /// <summary>
        /// Type par défaut pour un membre
        /// </summary>
        /// <param name="modelKind">Type de membre</param>
        /// <returns>Un nom de type</returns>
        string GetDefaultType(ModelKind modelKind);

        /// <summary>
        /// Création de l'instance d'un membre
        /// </summary>
        /// <param name="modelKind">Type de membre</param>
        /// <returns>Instance de modèle</returns>
        ITypeMember CreateModel(ModelKind modelKind);

        /// <summary>
        /// Initialise les propriétés de la fenêtre détail
        /// </summary>
        /// <param name="title">Titre de la fenetre</param>
        /// <param name="categories">Liste des catégories de membres à afficher ou null si une seule par défaut</param>
        /// <param name="memberSeparators">Liste des caractères séparateurs des membres</param>
        /// <param name="childSeparators">Liste des caractères séparateurs des enfants</param>
        void GetEditProperties(out string title, out List<VirtualTreeGridCategory> categories, out string memberSeparators, out string childSeparators);
    }
}
