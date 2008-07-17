using System;
using System.Collections.Generic;
using System.IO;
using DSLFactory.Candle.SystemModel.Repository;
using DSLFactory.Candle.SystemModel.Repository.Providers;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using Microsoft.VisualStudio.Zip;

namespace DSLFactory.Candle.SystemModel.MSBuild
{
    /// <summary>
    /// Tache msbuild permettant de créer le package de stratégie sous forme
    /// de fichier compréssé.
    /// </summary>
    public class CandleStrategyPackager : Task
    {
        private ITaskItem[] _artefacts;
        private string _fileName;
        private ITaskItem[] _url;

        /// <summary>
        /// Initializes a new instance of the <see cref="CandleStrategyPackager"/> class.
        /// </summary>
        public CandleStrategyPackager()
        {
        }

        /// <summary>
        /// Liste des fichiers à intégrer au package. Si null, on supprime
        /// le package
        /// </summary>
        public ITaskItem[] Artefacts
        {
            get { return _artefacts; }
            set { _artefacts = value; }
        }

        /// <summary>
        /// Nom du package
        /// </summary>
        [Required]
        public string Package
        {
            get { return _fileName; }
            set { _fileName = value; }
        }

        /// <summary>
        /// Adresse du serveur distant
        /// </summary>        
        public ITaskItem[] TargetUrl
        {
            get { return _url; }
            set { _url = value; }
        }

        /// <summary>
        /// Execution de la tache
        /// </summary>
        /// <returns></returns>
        public override bool Execute()
        {
            string packageName = _fileName;
            if (!Path.HasExtension(_fileName))
                packageName += ".zip";
            else if (
                String.Compare(Path.GetExtension(_fileName).ToLower(), ".zip",
                               StringComparison.CurrentCultureIgnoreCase) == 0)
            {
                Log.LogMessageFromText("Invalid package extension. It must be '.zip'. It will be replaced.",
                                       MessageImportance.Low);
                packageName = Path.ChangeExtension(_fileName, ".zip");
            }

            if (_artefacts == null)
            {
                Utils.DeleteFile(packageName);
                return true;
            }

            // Création dans un répertoire temporaire du package voulu en tenant compte 
            // des chemins relatifs puis compression de ce dossier.
            // On est obligé de procéder comme ça car ZipFileCompressor ne propose pas de 
            // stocker différemment les fichiers (avec ou sans chemin relatif)

            string tmpPath = Path.GetTempFileName();
            try
            {
                // Création d'un répertoire temporaire
                File.Delete(tmpPath);
                tmpPath = tmpPath.Substring(0, tmpPath.Length - 4);
                Directory.CreateDirectory(tmpPath);

                // Liste des fichiers contenus dans le package sous forme
                //  de chemin relatif à la racine du package
                List<string> files = new List<string>();

                // Ajout de tous les fichiers passés en args
                foreach (ITaskItem item in _artefacts)
                {
                    string targetPath = String.Empty;
                    try
                    {
                        string fn = Path.GetFileName(item.ItemSpec);
                        files.Add(fn);
                        targetPath = Path.Combine(tmpPath, fn);
                        // Copie dans le répertoire temporaire (qui va étre
                        //  compréssé)
                        File.Copy(item.ItemSpec, targetPath);
                    }
                    catch (Exception ex2)
                    {
                        Log.LogMessageFromText(
                            String.Format("Error when adding file {0} ({2})- {1}", item.ItemSpec, ex2.Message,
                                          targetPath), MessageImportance.High);
                        return false;
                    }
                }

                // Création du fichier zip
                new ZipFileCompressor(packageName, tmpPath, files.ToArray(), true);
                Log.LogMessageFromText(String.Format("Candle strategies package {0} created", _fileName),
                                       MessageImportance.Normal);
                if (_url != null)
                {
                    foreach (ITaskItem ti in _url)
                    {
                        Publish(packageName, ti.ItemSpec);
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                Log.LogErrorFromException(ex);
            }
            finally
            {
                Utils.RemoveDirectory(tmpPath);
            }

            return false;
        }

        /// <summary>
        /// Publishes the specified package name.
        /// </summary>
        /// <param name="packageName">Name of the package.</param>
        /// <param name="target">The target.</param>
        /// <returns></returns>
        public bool Publish(string packageName, string target)
        {
            try
            {
                //if( String.Compare(_url, "local",  StringComparison.CurrentCultureIgnoreCase ) == 0 )
                //{
                //    FileRepositoryProvider frp = new FileRepositoryProvider(CandleSettings.GetFolderPath(DSLFactory.Candle.SystemModel.Repository.RepositoryCategory.Strategies));
                //    Log.LogMessageFromText("copie locale", MessageImportance.Normal);
                //    frp.PublishFile(packageName, DSLFactory.Candle.SystemModel.Repository.RepositoryCategory.Strategies, packageName);
                //    Log.LogMessageFromText(String.Format("Candle package {0} published locally", packageName), MessageImportance.Normal);
                //    return true;
                //}

                Uri url = new Uri(target);
                // Copie en local
                if (url.IsFile)
                {
                    string folder = Uri.UnescapeDataString(url.AbsolutePath);
                    string targetPath = Path.Combine(folder, Path.GetFileName(packageName));
                    Directory.CreateDirectory(folder);
                    Utils.CopyFile(packageName, targetPath);
                    Log.LogMessageFromText(
                        String.Format("Candle package {0} published locally in {1}", packageName, folder),
                        MessageImportance.Normal);
                    return true;
                }
                else if (url.Scheme == "http")
                {
                    // Copie distante
                    WebServiceRepositoryProvider wrp = new WebServiceRepositoryProvider(target);
                    wrp.PublishFile(packageName, RepositoryCategory.Strategies, packageName);
                    Log.LogMessageFromText(String.Format("Candle package {0} published to {1}", packageName, target),
                                           MessageImportance.Normal);
                    return true;
                }

                Log.LogError("Invalid url for publishing");
            }
            catch (Exception ex)
            {
                Log.LogErrorFromException(ex);
            }

            return false;
        }
    }
}