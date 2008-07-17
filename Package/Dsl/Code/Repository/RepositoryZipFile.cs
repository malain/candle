using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Microsoft.VisualStudio.Zip;

namespace DSLFactory.Candle.SystemModel.Repository
{
    /// <summary>
    /// 
    /// </summary>
    public class RepositoryZipFile
    {
        private readonly string _zipFileName;
        private readonly bool _preservePath;

        /// <summary>
        /// Constructeur
        /// </summary>
        /// <param name="fileName">Nom du fichier compressé</param>
        public RepositoryZipFile(string fileName)
        {
            _zipFileName = fileName;
            _preservePath = false;
        }

        /// <summary>
        /// Constructeur
        /// </summary>
        /// <param name="fileName">Nom du fichier compressé</param>
        /// <param name="preservePath">if set to <c>true</c> [preserve path].</param>
        public RepositoryZipFile(string fileName, bool preservePath)
        {
            _zipFileName = fileName;
            _preservePath = preservePath;
        }

        /// <summary>
        /// Archive une liste de fichiers à partir d'un répertoire
        /// </summary>
        /// <param name="baseDirectory">The base directory.</param>
        public void ArchiveFiles(string baseDirectory)
        {
            baseDirectory = baseDirectory.TrimEnd('/', '\\');
            List<string> tmp = new List<string>();
            foreach (string fileName in Utils.SearchFile(baseDirectory, "*.*"))
            {
                tmp.Add(fileName.Substring(baseDirectory.Length + 1));
            }

            if (tmp.Count > 0)
                new ZipFileCompressor(_zipFileName, baseDirectory, tmp.ToArray(), true);
        }

        /// <summary>
        /// Archive une liste de fichiers
        /// </summary>
        /// <param name="fileNames">The file names.</param>
        public void ArchiveFiles(List<string> fileNames)
        {
            RepositoryCategory category = RepositoryCategory.Models;
            List<string> tmp = new List<string>();
            for (int i = 0; i < fileNames.Count; i++)
            {
                if (File.Exists(fileNames[i]))
                {
                    tmp.Add(RepositoryManager.MakeRelative(fileNames[i], out category));
                }
            }

            if (tmp.Count > 0)
                new ZipFileCompressor(_zipFileName, RepositoryManager.GetFolderPath(category), tmp.ToArray(), true);
        }

        /// <summary>
        /// Extrait un fichier
        /// </summary>
        /// <param name="folder">Répertoire cible</param>
        /// <param name="filterExtension">Filtre d'extension sous la forme (.xxx)</param>
        public void ExtractFileWithExtension(string folder, string filterExtension)
        {
            ZipFileDecompressor decompressor = new ZipFileDecompressor(_zipFileName);
            try
            {
                foreach (ZipEntry zipEntry in decompressor.ZipFileEntries)
                {
                    if (filterExtension == null || zipEntry.IsADirectory ||
                        Utils.StringCompareEquals(filterExtension, Path.GetExtension(zipEntry.FileName)))
                        ExtractEntry(folder, decompressor, zipEntry);
                }
            }
            finally
            {
                decompressor.Close();
            }
        }

        /// <summary>
        /// Extracts the entry.
        /// </summary>
        /// <param name="folder">The folder.</param>
        /// <param name="decompressor">The decompressor.</param>
        /// <param name="zipEntry">The zip entry.</param>
        private void ExtractEntry(string folder, ZipFileDecompressor decompressor, ZipEntry zipEntry)
        {
            if (!_preservePath)
            {
                // Si on ne veut pas conserver le chemin, on ignore les répertoires et on force le chemin
                //  du fichier
                if (zipEntry.IsADirectory)
                    return;
                // Forcage du nom du fichier (la propriété est 'internal')
                FieldInfo fi = typeof (ZipEntry).GetField("_fileName", BindingFlags.Instance | BindingFlags.NonPublic);
                fi.SetValue(zipEntry, Path.GetFileName(zipEntry.FileName));
            }

            decompressor.UncompressZipEntry(zipEntry, folder, true);
        }


        /// <summary>
        /// Extrait tous les fichiers dans le repertoire cible
        /// </summary>
        /// <param name="folder">Chemin du répertoire cible</param>
        /// <remarks>
        /// L'extraction est transactionelle, elle est d'abord effectuée dans un répertoire temporaire et
        /// si tout est ok, ce répertoire est déplacé sur le répetrtoire cible aprés suppression de celui-ci.
        /// De cette manière, le répertoire cible contient l'image exacte du fichier zip.
        /// 
        /// <b>Nota :</b> Tous les fichiers extraits se trouve à la racine.
        /// </remarks>
        public void ExtractAll(string folder)
        {
            string tempFolder = folder;
            try
            {
                // Extraction dans un répertoire temporaire
                tempFolder = Path.GetTempFileName();
                File.Delete(tempFolder);

                // Extraction dans un répertoire temporaire
                Directory.CreateDirectory(tempFolder);

                ZipFileDecompressor decompressor = new ZipFileDecompressor(_zipFileName);
                try
                {
                    foreach (ZipEntry zipEntry in decompressor.ZipFileEntries)
                    {
                        ExtractEntry(tempFolder, decompressor, zipEntry);
                    }
                }
                finally
                {
                    decompressor.Close();
                }

                if (Directory.Exists(tempFolder))
                {
                    // Rename
                    try
                    {
                        Utils.MoveDirectory(folder, folder + ".bak");
                    }
                    catch
                    {
                    }
                    Utils.CopyDirectory(tempFolder, folder);
                }
            }
            catch
            {
                Utils.MoveDirectory(folder + ".bak", folder);
            }
            finally
            {
                Utils.RemoveDirectory(folder + ".bak");
                Utils.RemoveDirectory(tempFolder);
            }
        }
    }
}