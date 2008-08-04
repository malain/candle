<%@ WebHandler Language="C#" Class="Candle.Repository.DownloadFile" %>

using System;
using System.Web;
using System.IO;
using DSLFactory.Candle.SystemModel;
using DSLFactory.Candle.Repository;

namespace Candle.Repository
{

    /// <summary>
    /// Chargement d'un fichier sous la forme :
    ///   download.ashx?p=relative_path&c=category&id=id_client&d=localFileDate
    /// </summary>
    public class DownloadFile : IHttpHandler
    {
        /// <summary>
        /// Traitement de la requète
        /// </summary>
        /// <param name="context"></param>
        public void ProcessRequest(HttpContext context)
        {
            ILogger logger = ServiceLocator.Instance.GetService<ILogger>();
            try
            {
                context.Response.Clear();
                // Récupération de la catégory
                // 
                DSLFactory.Candle.SystemModel.Repository.RepositoryCategory category;
                try
                {
                    category = (DSLFactory.Candle.SystemModel.Repository.RepositoryCategory)
                        Enum.Parse(typeof(DSLFactory.Candle.SystemModel.Repository.RepositoryCategory), context.Request.QueryString["c"]);
                }
                catch
                {
                    if (logger != null)
                        logger.Write("Downloadfile", "412 invalid category", LogType.Error);
                    throw new HttpException(412, "invalid category");
                }

                // Récupération du chemin
                //
                string relativePath = context.Request.QueryString["p"];
                if (String.IsNullOrEmpty(relativePath))
                {
                    if (logger != null)
                        logger.Write("Downloadfile", "412 invalid path", LogType.Error);

                    throw new HttpException(412, "invalid path");
                }

                // Récupération de la date
                //
                DateTime localDate = DateTime.MinValue;
                try
                {
                    long ticks = long.Parse(context.Request.QueryString["d"]);
                    localDate = new DateTime(ticks);
                }
                catch
                {
                }

                // Vérification si le fichier existe
                //
                string version = context.Request.QueryString["v"];
                if (version == null) version = "0.0";
                
                // Verrue (n1) à virer à partir du client 0.9
                // Les modèles sont stockés avec la version sur 3 chifres dans le path
                string relativePath2 = String.Empty;
                string[] parts = relativePath.Split(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar);                
                if( parts.Length > 1) {
                    string[] versionParts = parts[1].Split('.');
                    if (parts.Length == 4)
                    {
                        parts[1] = String.Join(".", versionParts, 0, 3); // On vire le dernier n°
                        relativePath2 = String.Join(Path.DirectorySeparatorChar.ToString(), parts);
                    }
                }
                // Fin verrue

                // On regarde d'abord si il existe dans une version spécifique dans le rep Vxxx
                //  xxx = la version de candle du client (normalement sous la forme n.n.n.n)
                string tmp = Path.Combine("V" + version, relativePath);
                string path = DSLFactory.Candle.SystemModel.Repository.RepositoryManager.ResolvePath(category, tmp);                
                        
                if (!File.Exists(path))
                {
                    // verrue (n1)
                    tmp = Path.Combine("V" + version, relativePath2);
                    path = DSLFactory.Candle.SystemModel.Repository.RepositoryManager.ResolvePath(category, tmp);                
                    if(!File.Exists(path)) {
                    // fin verrue
                    
                        // Si non, on prend la version courante
                        path = DSLFactory.Candle.SystemModel.Repository.RepositoryManager.ResolvePath(category, relativePath);
                        if (!File.Exists(path))
                        {
                            path = DSLFactory.Candle.SystemModel.Repository.RepositoryManager.ResolvePath(category, relativePath);
                            // Fin                      
                            if (!File.Exists(path))
                            {   
                                // verrue (n1)
                                path = DSLFactory.Candle.SystemModel.Repository.RepositoryManager.ResolvePath(category, relativePath2);                
                                if(!File.Exists(path)) {
                                // fin verrue
                                    context.Response.StatusCode = (int)System.Net.HttpStatusCode.NotFound;
                                    context.Response.End();
                                }
                            }
                        }
                    }
                }

                //
                // Vérification si le fichier est à jour
                if (File.GetLastWriteTimeUtc(path) <= localDate)
                {
                    context.Response.StatusCode = (int)System.Net.HttpStatusCode.NotModified;
                    context.Response.End();
                }

                //
                // Envoi toujours sous forme binaire
                //
                context.Response.ContentType = "application/octet-stream";

                // Envoi du fichier
                //
                using (BinaryReader reader = new BinaryReader(File.Open(path, FileMode.Open, FileAccess.Read, FileShare.Read)))
                {
                    byte[] buffer = new byte[0x400];
                    using (Stream writer = context.Response.OutputStream)
                    {
                        int nb = 1;
                        while (nb > 0)
                        {
                            nb = reader.Read(buffer, 0, 0x400);
                            writer.Write(buffer, 0, nb);
                        }
                        writer.Flush();
                    }
                }
                CandleRepositoryController.Instance.NotifyDownloadFile(context.User, context.Request["id"], category, relativePath);
            }
            catch (Exception ex)
            {
                if (logger != null)
                    logger.WriteError("Downloadfile", context.Request.RawUrl, ex);
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