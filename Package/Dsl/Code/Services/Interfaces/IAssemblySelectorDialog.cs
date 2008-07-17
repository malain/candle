
namespace DSLFactory.Candle.SystemModel
{
    /// <summary>
    /// Contrat que doivent implémenter les boites de dialogues permettant de sélectionner des assemblies de références dans visual studio
    /// </summary>
    interface IAssemblySelectorDialog
    {
        /// <summary>
        /// Liste des assemblies sélectionnées
        /// </summary>
        System.Collections.Generic.List<System.Reflection.Assembly> SelectedAssemblies { get; }

        /// <summary>
        /// Affichage de la fenetre de sélection
        /// </summary>
        /// <param name="max">Nbre maximun d'assembly à sélectionner</param>
        /// <returns></returns>
        bool ShowDialog(int max);
    }
}
