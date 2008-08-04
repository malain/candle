<%@ WebHandler Language="C#" Class="Candle.Repository.UploadModel" %>

using System;
using System.Web;
using System.IO;
using DSLFactory.Candle.SystemModel;
using DSLFactory.Candle.SystemModel.Repository;

namespace Candle.Repository
{

/// <summary>
/// Copie d'un modele sur le repository
/// </summary>
    public class UploadModel : IHttpHandler
    {

        /// <summary>
        /// La requete est sous la forme : upload.ashx?n=nom 
        /// avec n = nom relatif du fichier zip.
        /// </summary>
        /// <param name="context"></param>
        public void ProcessRequest(HttpContext context)
        {
            ILogger logger = ServiceLocator.Instance.GetService<ILogger>();
            context.Response.Clear();

            // Le nom
            string name = context.Request.QueryString["n"];

            if (String.IsNullOrEmpty(name))
            {
                if( logger != null)
                    logger.Write("UploadModel", "412 invalid name", LogType.Error);
                throw new HttpException(412, "invalid name");
            }

            RepositoryCategory category = RepositoryCategory.Models;
            try
            {
                string cat = context.Request.QueryString["c"];
                if( !String.IsNullOrEmpty(cat))
                    category = (RepositoryCategory)Enum.Parse(typeof(RepositoryCategory), cat);
            }
            catch
            {
                if (logger != null)
                    logger.Write("UploadModel", "412 invalid category", LogType.Error);
                throw new HttpException(412, "invalid category");
            }
            
            try
            {
                // Path du fichier dans le répertoire des modèles
                
                // Verrue pour passer au stockage sur 3n° de version - A virer quand tout le monde est bon
                string[] parts = name.Split(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar);
                string[] versionParts = parts[1].Split('.');
                parts[1] = String.Join(".", versionParts, 0, 3); // On vire le dernier n°
                name = String.Join(Path.DirectorySeparatorChar.ToString(), parts);                
                // Fin verrue
                
                string fileName = RepositoryManager.ResolvePath(category, name);

                // Normalement il y en a qu'un
                foreach (string f in context.Request.Files.AllKeys)
                {
                    HttpPostedFile file = context.Request.Files[f];
                    Directory.CreateDirectory(Path.GetDirectoryName(fileName));
                    file.SaveAs(fileName);

                    // Décompresse le fichier modele
                    RepositoryZipFile zipFile = new RepositoryZipFile(fileName, false);
                    zipFile.ExtractFileWithExtension(Path.GetDirectoryName(fileName), ModelConstants.FileNameExtension);
                }

                DSLFactory.Candle.Repository.CandleRepositoryController.Instance.NotifyUploadModel(context.User, context.Request["id"],  name);
                context.Response.Write(String.Format("Repository update successfull with file {0}", name));
            }
            catch (Exception ex)
            {
                if (logger != null)
                    logger.WriteError("UploadModel", "read model", ex);
                throw new HttpException(500, String.Format("Error upload model {0} - {1}", name, ex.Message));
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}