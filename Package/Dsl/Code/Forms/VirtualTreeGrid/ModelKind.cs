using System;

namespace DSLFactory.Candle.SystemModel.Utilities
{
    /// <summary>
    /// Type de membres contenus dans la fenetre de d�tail
    /// </summary>
    [CLSCompliant(true)]
    public enum ModelKind
    {
        /// <summary>
        /// Mod�le racine (doit impl�menter <see cref="ISupportDetailView">ISupportDetailView</see>
        /// et �ventuellement <see cref="IShowCodeProperties">IShowCodeProperties</see>)
        /// </summary>
        Root,
        /// <summary>
        /// Permet de cr�er plusieurs cat�gories
        /// </summary>
        Category,
        /// <summary>
        /// Membre (doit impl�menter <see cref="IHasChildren">IHasChildren</see> et <see cref="ITypeMember">ITypeMember</see>)
        /// </summary>
        Member,
        /// <summary>
        /// Enfant (doit impl�menter <see cref="ITypeMember">ITypeMember</see>)
        /// </summary>
        Child
    }
}