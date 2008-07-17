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
        /// Cette passe est active lors de l'insertion d'un �lement dans le mod�le.
        /// </summary>
        ElementAdded = 4,

        /// <summary>
        /// Dans cette passe, seul le m�ta mod�le est mis � jour. Les projets ne sont pas cr�es 
        /// et le code g�n�r� est ignor�
        /// </summary>
        MetaModelUpdate = 1,

        /// <summary>
        /// G�nration du code
        /// </summary>
        CodeGeneration = 2,

        /// <summary>
        /// Publication du mod�le
        /// </summary>
        Publishing = 0
    }
}