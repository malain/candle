using System;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace DSLFactory.Candle.SystemModel.Repository
{
    /// <summary>
    /// Classe proxy pour un fichier stock� dans le r�f�rentiel. Elle assure de toujours fournir la derni�re version
    /// du fichier du r�f�rentiel central. Si le fichier n'existe pas en local, elle le r�cup�re sur le serveur.
    /// Le fichier local est toujours stock� dans le r�f�rentiel local. 
    /// </summary>
    public class RepositoryFile
    {
        /// <summary>
        /// Chemin d'acc�s physique
        /// </summary>
        private readonly string _absolutePath;

        /// <summary>
        /// Chemin relatif dans le repository
        /// </summary>
        private readonly string _path;

        /// <summary>
        /// Cat�gorie ou est stock� le fichier dans le r�f�rentiel
        /// </summary>
        private readonly RepositoryCategory _category;

        /// <summary>
        /// Constructeur � partir d'un chemin relatif
        /// </summary>
        /// <param name="category">Categorie dans lequel se trouve le fichier</param>
        /// <param name="path">Fichier relatif</param>
        public RepositoryFile(RepositoryCategory category, string path)
        {
            Debug.Assert(!Path.IsPathRooted(path), "Le chemin doit etre relatif");
            _category = category;
            _path = path;
            _absolutePath = CreateAbsoluteLocalPath(path);
        }

        /// <summary>
        /// Constructeur � partir d'un chemin absolu. La cat�gorie est d�duite
        /// </summary>
        /// <param name="physicalPath">Chemin physique dans le r�f�rentiel</param>
        public RepositoryFile(string physicalPath)
        {
            Debug.Assert(Path.IsPathRooted(physicalPath), "Le chemin ne doit pas etre relatif");
            _absolutePath = physicalPath;
            _path = RepositoryManager.MakeRelative(physicalPath, out _category);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RepositoryFile"/> class.
        /// </summary>
        /// <param name="physicalPath">The physical path.</param>
        /// <param name="category">The category.</param>
        /// <param name="relativePath">The relative path.</param>
        public RepositoryFile(string physicalPath, RepositoryCategory category, string relativePath)
        {
            _absolutePath = physicalPath;
            _path = relativePath;
            _category = category;
        }

        /// <summary>
        /// Existses this instance.
        /// </summary>
        /// <returns></returns>
        public bool Exists()
        {
            if (SynchronizeFromServer())
                return true;
            return File.Exists(_absolutePath);
        }

        /// <summary>
        /// Chemin physique sur le disque local avec syncrhonisation
        /// </summary>
        /// <value>The local physical path.</value>
        public string LocalPhysicalPath
        {
            get
            {
                SynchronizeFromServer();
                return _absolutePath;
            }
        }

        /// <summary>
        /// Chemin relatif avec synchronisation
        /// </summary>
        /// <value>The local relative path.</value>
        public string LocalRelativePath
        {
            get
            {
                SynchronizeFromServer();
                return _path;
            }
        }

        /// <summary>
        /// Recherche du type d'encodage d'un fichier
        /// </summary>
        /// <param name="inputFile">The input file.</param>
        /// <returns></returns>
        public static Encoding FindEncodingFromFile(string inputFile)
        {
            Encoding encoding = Encoding.Default;

            try
            {
                // Ouverture du fichier initial
                using (FileStream initialFile = new FileStream(inputFile, FileMode.Open, FileAccess.Read))
                {
                    // Si il n'est pas vide, on va chercher son encodage
                    if (initialFile.Length > 0)
                    {
                        using (StreamReader reader = new StreamReader(initialFile, true))
                        {
                            // Lecture d'un caract�re pour initialiser le stream
                            char[] ch = new char[1];
                            reader.Read(ch, 0, 1);
                            // Ici on peut le r�cup�rer
                            encoding = reader.CurrentEncoding;
                            reader.BaseStream.Position = 0;
                            // Si on est en UTF-8, on va essayer de lire la signature
                            if (encoding == Encoding.UTF8)
                            {
                                // Recup de la signature
                                byte[] utfSig = encoding.GetPreamble();
                                if (initialFile.Length >= utfSig.Length)
                                {
                                    // On regarde si on la trouve dans le fichier
                                    byte[] buffer = new byte[utfSig.Length];
                                    initialFile.Read(buffer, 0, buffer.Length);
                                    for (int ix = 0; ix < buffer.Length; ix++)
                                    {
                                        if (buffer[ix] != utfSig[ix])
                                        {
                                            encoding = Encoding.Default;
                                            break;
                                        }
                                    }
                                }
                                else
                                {
                                    encoding = Encoding.Default;
                                }
                            }
                        }
                    }
                }
            }
            catch
            {
            }
            return encoding ?? Encoding.Default;
        }

        /// <summary>
        /// Reads the content.
        /// </summary>
        /// <returns></returns>
        public string ReadContent()
        {
            Encoding encoding;
            return ReadContent(out encoding);
        }

        /// <summary>
        /// Lecture du contenu du fichier. Si le fichier n'est pas pr�sent en local, il est r�cup�r� du serveur
        /// </summary>
        /// <param name="encoding">Encodage du fichier</param>
        /// <returns>Contenu complet du fichier</returns>
        public string ReadContent(out Encoding encoding)
        {
            if (SynchronizeFromServer())
            {
                encoding = FindEncodingFromFile(_absolutePath);
                return File.ReadAllText(_absolutePath);
            }

            encoding = Encoding.Default;
            return string.Empty;
        }

        /// <summary>
        /// Lecture du fichier dans le r�f�rentiel si le fichier n'est
        /// pas pr�sent ou si il date trop.
        /// </summary>
        /// <returns></returns>
        public bool SynchronizeFromServer()
        {
            // V�rif du cache
            if (File.Exists(_absolutePath))
            {
                DateTime dt = File.GetLastWriteTime(_absolutePath);
                if (!DSLFactory.Candle.SystemModel.Configuration.CandleSettings.CacheExpired(dt))
                    return true;
            }
            // Si le fichier est en-read only, on n'y touche pas
            if (Utils.IsFileLocked(_absolutePath))
            {
                return true;
            }
            return RepositoryManager.Instance.GetFileFromRepository(_category, _path, _absolutePath);
        }

        /// <summary>
        /// Cr�ation du fichier absolu � partir du chemin relatif
        /// </summary>
        /// <param name="relativePath">The relative path.</param>
        /// <returns></returns>
        private string CreateAbsoluteLocalPath(string relativePath)
        {
            return RepositoryManager.ResolvePath(_category, relativePath);
        }
    }
}