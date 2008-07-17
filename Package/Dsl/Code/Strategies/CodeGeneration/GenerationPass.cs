using System;

namespace DSLFactory.Candle.SystemModel
{
    /// <summary>
    /// 
    /// </summary>
    [Flags]
    public enum GenerationPass
    {
        /// <summary>
        /// Cette passe est active lors de l'insertion d'un élement dans le modèle.
        /// </summary>
        ElementAdded = 4,

        /// <summary>
        /// Dans cette passe, seul le méta modèle est mis à jour. Les projets ne sont pas crées 
        /// et le code généré est ignoré
        /// </summary>
        MetaModelUpdate = 1,

        /// <summary>
        /// Génration du code
        /// </summary>
        CodeGeneration = 2,

        /// <summary>
        /// Publication du modèle
        /// </summary>
        Publishing = 0
    }
}