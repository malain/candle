using System;

namespace DSLFactory.Candle.SystemModel
{
    /// <summary>
    /// Contexte d'utilisation d'une r�ference
    /// </summary>
    [Flags]
    [CLSCompliant(true)]
    public enum ReferenceScope
    {
        /// <summary>
        /// Jamais pris en compte
        /// </summary>
        None=0,
        /// <summary>
        /// Pris en compte dans la phase de compilation (=r�f�rences du projet)
        /// </summary>
        Compilation=1,
        /// <summary>
        /// Pris en compte lors de l'ex�cution
        /// </summary>
        Runtime=2,
        /// <summary>
        /// Pris en compte lors de la publication sur le r�f�rentiel
        /// </summary>
        Publish=4,
        /// <summary>
        /// Pris en compte dans tous les cas
        /// </summary>
        All=255
    }


}
