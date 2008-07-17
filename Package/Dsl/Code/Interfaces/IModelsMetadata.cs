namespace DSLFactory.Candle.SystemModel.Repository
{
    /// <summary>
    /// Classe de service g�rant les donn�es de description des mod�les
    /// </summary>
    public interface IModelsMetadata
    {
        /// <summary>
        /// Liste des metadatas
        /// </summary>
        MetadataCollection Metadatas { get;}

        /// <summary>
        /// Supprime un mod�le dans le repository local
        /// </summary>
        /// <param name="cmm">Metadata du mod�le</param>
        void RemoveModelInLocalRepository(ComponentModelMetadata cmm );

        /// <summary>
        /// Publication du mod�le local vers son serveur initial (ou le principal si 1�re fois)
        /// </summary>
        /// <param name="model">Mod�le � publier</param>
        void PublishLocalModel(CandleModel model);

        /// <summary>
        /// Affiche une boite de dialogue permettant de s�lectionner un mod�le dans le repository pour 
        /// l'associer � un composant externe
        /// </summary>
        /// <param name="model">Composant externe � mettre � jour</param>
        /// <param name="metaData">Metadata du mod�le s�lectionn�</param>
        /// <returns>false si l'utilisateur � annul�</returns>
        bool SelectModel(ExternalComponent model, out ComponentModelMetadata metaData);

        /// <summary>
        /// R�cup�re un mod�le dans le repository local
        /// </summary>
        /// <param name="metaData"></param>
        bool GetModelInLocalRepository(ComponentModelMetadata metaData);

        /// <summary>
        /// Publication du mod�le pr�sent dans la solution dans le repository local puis sur le serveur distant 
        /// le cas �ch�ant
        /// </summary>
        /// <param name="model">Mod�le � publier</param>
        /// <param name="fileName">Fichier initial contenant le mod�le</param>
        /// <param name="promptWarning">Indique si il faut demander une confirmation � l'utilisateur</param>
        void PublishModel(CandleModel model, string fileName, bool promptWarning);

        /// <summary>
        /// Publication d'un mod�le en tant que template de mod�le
        /// </summary>
        /// <param name="modelFileName"></param>
        /// <param name="remoteName"></param>
        /// <param name="strategiesFileName"></param>
        /// <param name="remoteStrategiesFileName"></param>
        void PublishModelAsTemplate(string modelFileName, string remoteName, string strategiesFileName, string remoteStrategiesFileName);

        /// <summary>
        /// Copie un mod�le dans un r�pertoire temporaire en read only.
        /// </summary>
        /// <param name="metaData"></param>
        /// <returns></returns>
        string CopyModelInTemporaryFolder(ComponentModelMetadata metaData);
    }
}
