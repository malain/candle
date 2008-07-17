namespace DSLFactory.Candle.SystemModel.Repository
{
    /// <summary>
    /// Classe de service gérant les données de description des modèles
    /// </summary>
    public interface IModelsMetadata
    {
        /// <summary>
        /// Liste des metadatas
        /// </summary>
        MetadataCollection Metadatas { get;}

        /// <summary>
        /// Supprime un modèle dans le repository local
        /// </summary>
        /// <param name="cmm">Metadata du modèle</param>
        void RemoveModelInLocalRepository(ComponentModelMetadata cmm );

        /// <summary>
        /// Publication du modèle local vers son serveur initial (ou le principal si 1ère fois)
        /// </summary>
        /// <param name="model">Modéle à publier</param>
        void PublishLocalModel(CandleModel model);

        /// <summary>
        /// Affiche une boite de dialogue permettant de sélectionner un modèle dans le repository pour 
        /// l'associer à un composant externe
        /// </summary>
        /// <param name="model">Composant externe à mettre à jour</param>
        /// <param name="metaData">Metadata du modèle sélectionné</param>
        /// <returns>false si l'utilisateur à annulé</returns>
        bool SelectModel(ExternalComponent model, out ComponentModelMetadata metaData);

        /// <summary>
        /// Récupère un modèle dans le repository local
        /// </summary>
        /// <param name="metaData"></param>
        bool GetModelInLocalRepository(ComponentModelMetadata metaData);

        /// <summary>
        /// Publication du modèle présent dans la solution dans le repository local puis sur le serveur distant 
        /// le cas échéant
        /// </summary>
        /// <param name="model">Modèle à publier</param>
        /// <param name="fileName">Fichier initial contenant le modèle</param>
        /// <param name="promptWarning">Indique si il faut demander une confirmation à l'utilisateur</param>
        void PublishModel(CandleModel model, string fileName, bool promptWarning);

        /// <summary>
        /// Publication d'un modèle en tant que template de modèle
        /// </summary>
        /// <param name="modelFileName"></param>
        /// <param name="remoteName"></param>
        /// <param name="strategiesFileName"></param>
        /// <param name="remoteStrategiesFileName"></param>
        void PublishModelAsTemplate(string modelFileName, string remoteName, string strategiesFileName, string remoteStrategiesFileName);

        /// <summary>
        /// Copie un modèle dans un répertoire temporaire en read only.
        /// </summary>
        /// <param name="metaData"></param>
        /// <returns></returns>
        string CopyModelInTemporaryFolder(ComponentModelMetadata metaData);
    }
}
