using System;

namespace DSLFactory.Candle.SystemModel
{
    /// <summary>
    /// Contexte d'utilisation d'une réference
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
        /// Pris en compte dans la phase de compilation (=références du projet)
        /// </summary>
        Compilation=1,
        /// <summary>
        /// Pris en compte lors de l'exécution
        /// </summary>
        Runtime=2,
        /// <summary>
        /// Pris en compte lors de la publication sur le référentiel
        /// </summary>
        Publish=4,
        /// <summary>
        /// Pris en compte dans tous les cas
        /// </summary>
        All=255
    }


}
