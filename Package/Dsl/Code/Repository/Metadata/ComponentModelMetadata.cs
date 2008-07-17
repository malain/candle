using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using Microsoft.VisualStudio.Modeling;

namespace DSLFactory.Candle.SystemModel.Repository
{
    /// <summary>
    /// Classe des m�tadonn�es d'un mod�le qui permet de d�crire un mod�le dans le repository
    /// </summary>
    [Serializable]
    [CLSCompliant(true)]
    public class ComponentModelMetadata
    {
        private string _path;
        private ComponentType _componentType;
        private string _description;
        private string _documentationUrl;
        private Guid _id;
        private Int32 _localVersion;
        private RepositoryLocation _location;
        private string _modelFileName;
        private string _name;
        private string _serverUrl;
        private Int32 _serverVersion;
        private string _testBaseAddress;
        private VersionInfo _version;
        private Visibility _visibility;

        /// <summary>
        /// N� de version du mod�le stock� sur le serveur
        /// </summary>
        public Int32 ServerVersion
        {
            [DebuggerStepThrough()]
            get { return _serverVersion; }
            [DebuggerStepThrough()]
            set { _serverVersion = value; }
        }

        /// <summary>
        /// N� de version du mod�le local
        /// </summary>
        public Int32 LocalVersion
        {
            [DebuggerStepThrough()]
            get { return _localVersion; }
            [DebuggerStepThrough()]
            set { _localVersion = value; }
        }

        /// <summary>
        /// Visibilit� du mod�le dans le domaine applicatif
        /// </summary>
        public Visibility Visibility
        {
            [DebuggerStepThrough()]
            get { return _visibility; }
            [DebuggerStepThrough()]
            set { _visibility = value; }
        }

        /// <summary>
        /// Type du composant (software/binaire)
        /// </summary>
        public ComponentType ComponentType
        {
            [DebuggerStepThrough()]
            get { return _componentType; }
            [DebuggerStepThrough()]
            set { _componentType = value; }
        }

        /// <summary>
        /// Est ce qu'il existe une version disponible sur le serveur
        /// </summary>
        public bool ExistsOnServer
        {
            get { return _serverVersion >= 0; }
        }

        /// <summary>
        /// Url du repository d'ou provient le mod�le
        /// </summary>
        public string ServerUrl
        {
            [DebuggerStepThrough()]
            get { return _serverUrl; }
            [DebuggerStepThrough()]
            set { _serverUrl = value; }
        }

        /// <summary>
        /// Adresse de test des services propos�s par ce mod�le
        /// </summary>
        public string TestBaseAddress
        {
            [DebuggerStepThrough()]
            get { return _testBaseAddress; }
            [DebuggerStepThrough()]
            set { _testBaseAddress = value; }
        }

        /// <summary>
        /// Url de la documentation associ�e
        /// </summary>
        public string DocUrl
        {
            [DebuggerStepThrough()]
            get { return _documentationUrl; }
            [DebuggerStepThrough()]
            set { _documentationUrl = value; }
        }

        /// <summary>
        /// Emplacement actuel du mod�le (local ou distant)
        /// </summary>
        public RepositoryLocation Location
        {
            [DebuggerStepThrough()]
            get { return _location; }
            [DebuggerStepThrough()]
            set { _location = value; }
        }

        /// <summary>
        /// Nom du fichier contenant le mod�le (sans le path)
        /// </summary>
        public string ModelFileName
        {
            [DebuggerStepThrough()]
            get { return _modelFileName; }
            [DebuggerStepThrough()]
            set { _modelFileName = value; }
        }

        /// <summary>
        /// Description du mod�le
        /// </summary>
        public string Description
        {
            [DebuggerStepThrough()]
            get { return _description; }
            [DebuggerStepThrough()]
            set { _description = value; }
        }

        /// <summary>
        /// Arborescence de classification dans la fen�tre de repository
        /// </summary>
        public string Path
        {
            [DebuggerStepThrough()]
            get { return _path; }
            [DebuggerStepThrough()]
            set { _path = value; }
        }

        /// <summary>
        /// N� de version du mod�le
        /// </summary>
        public VersionInfo Version
        {
            [DebuggerStepThrough]
            get { return _version; }
            [DebuggerStepThrough]
            set { _version = value; }
        }

        /// <summary>
        /// Nom du mod�le
        /// </summary>
        public string Name
        {
            [DebuggerStepThrough()]
            get { return _name; }
            [DebuggerStepThrough()]
            set { _name = value; }
        }

        /// <summary>
        /// Identifiant du mod�le
        /// </summary>
        public Guid Id
        {
            [DebuggerStepThrough]
            get { return _id; }
            [DebuggerStepThrough]
            set { _id = value; }
        }

        /// <summary>
        /// Date du mod�le associ�
        /// </summary>
        public DateTime LocalDateTime
        {
            get
            {
                string filePath = GetFileName(PathKind.Absolute);
                if (File.Exists(filePath))
                    return File.GetLastWriteTimeUtc(filePath);
                return DateTime.MinValue;
            }
        }

        /// <summary>
        /// Indique si le mod�le est pr�sent dans le repository local
        /// </summary>
        internal bool ExistsLocally
        {
            get
            {
                string path = GetFileName(PathKind.Absolute);
                return !String.IsNullOrEmpty(path) && File.Exists(path);
            }
        }

        /// <summary>
        /// Comparaison de 2 metadata
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is ComponentModelMetadata))
                return false;
            ComponentModelMetadata other = obj as ComponentModelMetadata;
            return _id == other._id && _version.Equals(other._version);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return String.Concat(_id, _version.ToString()).GetHashCode();
        }

        /// <summary>
        /// Permet de calculer le chemin du fichier mod�le
        /// </summary>
        /// <param name="id">Id du mod�le</param>
        /// <param name="version">n� de version</param>
        /// <param name="modelFileName">Nom du fichier (sans le path) du mod�le (extension facultative)</param>
        /// <param name="kind">Type de chemin souhait�</param>
        /// <returns>Path absolu ou relatif au repository</returns>
        public static string GetFileName(Guid id, VersionInfo version, string modelFileName, PathKind kind)
        {
            if (!System.IO.Path.HasExtension(modelFileName))
                modelFileName += ModelConstants.FileNameExtension;

            // Chemin relatif
            string relPath =
                System.IO.Path.Combine(System.IO.Path.Combine(id.ToString(), version.ToString(3)), modelFileName);
            if (kind == PathKind.Relative)
                return relPath;
            // Chemin physique
            return RepositoryManager.ResolvePath(RepositoryCategory.Models, relPath);
        }

        /// <summary>
        /// Chemin physique du mod�le
        /// </summary>
        public string GetFileName(PathKind kind)
        {
            return GetFileName(_id, _version, _modelFileName, kind);
        }

        /// <summary>
        /// Permet de r�soudre un nom de fichier par rapport au r�pertoire contenant le mod�le
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="kind"></param>
        /// <returns></returns>
        internal string ResolvePath(string fileName, PathKind kind)
        {
            return System.IO.Path.Combine(System.IO.Path.GetDirectoryName(GetFileName(kind)), fileName);
        }

        /// <summary>
        /// Cr�ation d'un composant externe � partir correspondant � un mod�le
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        internal ExternalComponent CreateComponent(CandleModel model)
        {
            ExternalComponent externalComponent = model.FindExternalComponent(_id);
            if (externalComponent != null)
                return externalComponent;

            externalComponent = new ExternalComponent(model.Store);
            UpdateComponent(externalComponent);

            model.ExternalComponents.Add(externalComponent);
            externalComponent.UpdateFromModel();
            return externalComponent;
        }

        /// <summary>
        /// Est ce que la derni�re version est pr�sente en local ?
        /// </summary>
        /// <returns></returns>
        internal bool IsLastVersion()
        {
            return (!ExistsOnServer || _localVersion >= _serverVersion) && File.Exists(GetFileName(PathKind.Absolute));
        }

        /// <summary>
        /// Met � jour les caract�ristiques d'un composant externe � partir des metadonn�es
        /// </summary>
        /// <param name="component"></param>
        internal void UpdateComponent(ExternalComponent component)
        {
            component.Name = _name;
            component.Version = _version;
            component.ModelMoniker = _id;
            component.Description = _description;
        }

        /// <summary>
        /// Lecture des metadonn�es d'un mod�le
        /// </summary>
        /// <param name="fileName">Chemin absolu du fichier contenant le mod�le</param>
        /// <returns>Ne renvoit jamais null</returns>
        internal static ComponentModelMetadata RetrieveMetadata(string fileName)
        {
            ComponentModelMetadata item = new ComponentModelMetadata();
            UpdateMetadata(item, fileName);
            item.LocalVersion = -1;
            item.ServerVersion = item._version.Revision;
            return item;
        }

        /// <summary>
        /// Mise � jour des m�tadata � partir du mod�le
        /// </summary>
        /// <param name="item">Metadata � mettre � jour</param>
        /// <param name="fileName">Fichier contenant le mod�le</param>
        internal static void UpdateMetadata(ComponentModelMetadata item, string fileName)
        {
            item.ModelFileName = System.IO.Path.GetFileName(fileName);
            ModelLoader loader = null;
            try
            {
                // Chargement du mod�le pour r�cup�rer les metadata
                ModelLoader.ClearCache(fileName);
                loader = ModelLoader.GetLoader(fileName, true);

                item.Id = loader.Model.Id;
                item.Version = loader.Model.Version ?? new VersionInfo();
                item.Name = loader.Model.Name;
                item.Path = loader.Model.Path;
                item.DocUrl = loader.Model.Url ?? String.Empty;
                item.TestBaseAddress = loader.Model.BaseAddress ?? String.Empty;
                item.Description = loader.Model.Comment ?? String.Empty;
                item.ComponentType = loader.Model.ComponentType;
                item.Location = RepositoryLocation.Local; // Toujours
                //item.ServerVersion = item.version.Revision;
                item.Visibility = loader.Model.Visibility;
                //item.LocalVersion = -1;
            }
            catch (Exception ex)
            {
                try
                {
                    // On essaye de d�duire le n� de version � partir du nom de fichier.
                    // Idem pour l'id
                    string v = System.IO.Path.GetDirectoryName(fileName);
                    string[] parts = v.Split(System.IO.Path.DirectorySeparatorChar);
                    if (parts.Length > 2)
                    {
                        item.Version = VersionInfo.TryParse(parts[parts.Length - 1]);
                        item.Id = new Guid(parts[parts.Length - 2]);
                    }
                }
                catch
                {
                    item.Id = Guid.NewGuid();
                    item.Version = new VersionInfo();
                }

                item.DocUrl = String.Empty;
                if (ModelLoader.LastSerializationResult != null && ModelLoader.LastSerializationResult.Failed)
                {
                    StringBuilder sb = new StringBuilder(ex.Message);
                    foreach (SerializationMessage msg in ModelLoader.LastSerializationResult)
                    {
                        sb.AppendLine(String.Format("{0} at {1},{2}", msg.Message, msg.Line, msg.Column));
                    }
                    item.Description = sb.ToString();
                }
                else
                    item.Description = ex.Message;
                item.ServerVersion = -1;
                item.Location = RepositoryLocation.Local; // Toujours
                item.Name = String.Format("Loading error : {0}", System.IO.Path.GetFileNameWithoutExtension(fileName));
                item.Path = "????";
                item.Visibility = Visibility.Public;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return String.Concat(_name, " V:", _version.ToString());
        }
    }
}