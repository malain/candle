using System;
using System.Collections.Generic;
using DSLFactory.Candle.SystemModel.Strategies;

namespace DSLFactory.Candle.SystemModel.Repository
{
    /// <summary>
    /// Interface d'accés aux infos d'un repository
    /// </summary>
    public interface IRepositoryProvider
    {
        /// <summary>
        /// Récupération d'un modèle (cml)
        /// </summary>
        /// <param name="metadata">Caractèristiques du modèle</param>
        /// <returns></returns>
        RepositoryServerItemStatus GetModel(ComponentModelMetadata metadata);

        /// <summary>
        /// Récupère un fichier dans le repository
        /// </summary>
        /// <param name="category">Category du fichier à récupérer</param>
        /// <param name="path">Chemin d'accés relatif à la catégorie</param>
        /// <param name="localFile">Chemin absolu local de destination</param>
        /// <returns>Status du chargement</returns>
        RepositoryServerItemStatus GetFile( RepositoryCategory category, string path, string localFile );

        /// <summary>
        /// Permet de récupèrer les metadatas d'un modèle
        /// </summary>
        /// <param name="id">ID du modèle</param>
        /// <param name="version">N° de version</param>
        /// <returns>null si le modèle n'existe pas</returns>
        ComponentModelMetadata GetModelMetadata(Guid id, VersionInfo version);

        /// <summary>
        /// Retourne la liste des métadatas de tous les modéles présent dans le repository
        /// </summary>
        /// <returns></returns>
        List<ComponentModelMetadata> GetAllMetadata();

        /// <summary>
        /// Retourne la liste des manifests des stratégies
        /// </summary>
        /// <returns>null pour les stratégies locales</returns>
        StrategyManifest[] GetStrategyManifests( );

        /// <summary>
        /// Publie un modèle
        /// </summary>
        /// <param name="model">Le modèle à publier</param>
        /// <param name="fileName">Chemin absolu du modèle à publier</param>
        /// <param name="remoteName">Nom du fichier modèle dans le repository</param>
        /// <returns></returns>
        string PublishModel( CandleModel model, string fileName, string remoteName );

        /// <summary>
        /// Publie un fichier sur le référentiel
        /// </summary>
        /// <param name="fileName">Chemin relatif du fichier</param>
        /// <param name="category">Categorie d'appartenance</param>
        /// <param name="remoteName">Nom du fichier destination</param>
        /// <returns></returns>
        string PublishFile(string fileName, RepositoryCategory category, string remoteName);

        /// <summary>
        /// Nom du provider
        /// </summary>
        string Name { get;}

        /// <summary>
        /// Enumère le contenu d'une catégorie
        /// </summary>
        /// <param name="category">Type de la catégorie</param>
        /// <param name="filter">Filtre sur les fichiers ou null pour tous (*.xxx)</param>
        /// <param name="recursive">Prend en compte les sous-répertoires</param>
        /// <returns></returns>
        List<RepositoryFileInfo> EnumerateCategory( RepositoryCategory category, string filter, bool recursive );

        /// <summary>
        /// N° de version du repository
        /// </summary>
        /// <returns></returns>
        Version GetVersion();

        /// <summary>
        /// Création d'un path
        /// </summary>
        /// <param name="path">Chemin (xxx/../xxx)</param>
        void CreateDomainPath(string path);

        /// <summary>
        /// Suppression d'un path
        /// </summary>
        /// <param name="path">Chemin (xxx/../xxx)</param>
        void RemoveDomainPath(string path);

    }
   
}
