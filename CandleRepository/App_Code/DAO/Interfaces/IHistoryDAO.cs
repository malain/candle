using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using DSLFactory.Candle.SystemModel.Repository;
using System.Collections.Generic;
using DSLFactory.Candle.SystemModel;

namespace DSLFactory.Candle.Repository
{

    /// <summary>
    /// DAO pour stocker les logs d'utilisation
    /// </summary>
    public interface ICandleRepositoryDAO
    {
        /// <summary>
        /// Enregistre dans le log qu'un modèle vient d'être publié
        /// </summary>
        /// <param name="userName">Utilisateur qui a publié</param>
        /// <param name="id">License id de candle</param>
        /// <param name="fileName">Chemin relatif du fichier</param>
        void WriteUploadModelLog(string userName, string id, string fileName);

        /// <summary>
        /// Incrémente le compteur de téléchargement d'un fichier
        /// </summary>
        /// <param name="userName">Utilisateur qui a publié</param>
        /// <param name="id">License id de candle</param>
        /// <param name="category">Categorie de fichier</param>
        /// <param name="fileName">Chemin relatif du fichier</param>
        void IncrementDownloadFileCounter(string userName, string id, RepositoryCategory category, string fileName);

        /// <summary>
        /// Liste des derniers fichiers publiés
        /// </summary>
        /// <param name="nb"></param>
        /// <returns></returns>
        List<HistoryEntry> GetLastUpload(int nb);

        /// <summary>
        /// Historique de publication d'un fichier
        /// </summary>
        /// <param name="modelId"></param>
        /// <param name="version"></param>
        /// <returns></returns>
        List<HistoryEntry> GetModelHistoric(Guid modelId, VersionInfo version);

    }
}