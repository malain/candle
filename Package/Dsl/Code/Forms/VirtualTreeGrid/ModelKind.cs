using System;

namespace DSLFactory.Candle.SystemModel.Utilities
{
    /// <summary>
    /// Type de membres contenus dans la fenetre de détail
    /// </summary>
    [CLSCompliant(true)]
    public enum ModelKind
    {
        /// <summary>
        /// Modèle racine (doit implémenter <see cref="ISupportDetailView">ISupportDetailView</see>
        /// et éventuellement <see cref="IShowCodeProperties">IShowCodeProperties</see>)
        /// </summary>
        Root,
        /// <summary>
        /// Permet de créer plusieurs catégories
        /// </summary>
        Category,
        /// <summary>
        /// Membre (doit implémenter <see cref="IHasChildren">IHasChildren</see> et <see cref="ITypeMember">ITypeMember</see>)
        /// </summary>
        Member,
        /// <summary>
        /// Enfant (doit implémenter <see cref="ITypeMember">ITypeMember</see>)
        /// </summary>
        Child
    }
}