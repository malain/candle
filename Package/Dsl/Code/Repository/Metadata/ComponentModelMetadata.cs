using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using Microsoft.VisualStudio.Modeling;

namespace DSLFactory.Candle.SystemModel.Repository
{
    /// <summary>
    /// Classe des métadonnées d'un modèle qui permet de décrire un modèle dans le repository
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
        /// N° de version du modèle stocké sur le serveur
        /// </summary>
        public Int32 ServerVersion
        {
            [DebuggerStepThrough()]
            get { return _serverVersion; }
            [DebuggerStepThrough()]
            set { _serverVersion = value; }
        }

        /// <summary>
        /// N° de version du modèle local
        /// </summary>
        public Int32 LocalVersion
        {
            [DebuggerStepThrough()]
            get { return _localVersion; }
            [DebuggerStepThrough()]
            set { _localVersion = value; }
        }

        /// <summary>
        /// Visibilité du modèle dans le domaine applicatif
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
        /// Url du repository d'ou provient le modèle
        /// </summary>
        public string ServerUrl
        {
            [DebuggerStepThrough()]
            get { return _serverUrl; }
            [DebuggerStepThrough()]
            set { _serverUrl = value; }
        }

        /// <summary>
        /// Adresse de test des services proposés par ce modèle
        /// </summary>
        public string TestBaseAddress
        {
            [DebuggerStepThrough()]
            get { return _testBaseAddress; }
            [DebuggerStepThrough()]
            set { _testBaseAddress = value; }
        }

        /// <summary>
        /// Url de la documentation associée
        /// </summary>
        public string DocUrl
        {
            [DebuggerStepThrough()]
            get { return _documentationUrl; }
            [DebuggerStepThrough()]
            set { _documentationUrl = value; }
        }

        /// <summary>
        /// Emplacement actuel du modèle (local ou distant)
        /// </summary>
        public RepositoryLocation Location
        {
            [DebuggerStepThrough()]
            get { return _location; }
            [DebuggerStepThrough()]
            set { _location = value; }
        }

        /// <summary>
        /// Nom du fichier contenant le modèle (sans le path)
        /// </summary>
        public string ModelFileName
        {
            [DebuggerStepThrough()]
            get { return _modelFileName; }
            [DebuggerStepThrough()]
            set { _modelFileName = value; }
        }

        /// <summary>
        /// Description du modèle
        /// </summary>
        public string Description
        {
            [DebuggerStepThrough()]
            get { return _description; }
            [DebuggerStepThrough()]
            set { _description = value; }
        }

        /// <summary>
        /// Arborescence de classification dans la fenêtre de repository
        /// </summary>
        public string Path
        {
            [DebuggerStepThrough()]
            get { return _path; }
            [DebuggerStepThrough()]
            set { _path = value; }
        }

        /// <summary>
        /// N° de version du modèle
        /// </summary>
        public VersionInfo Version
        {
            [DebuggerStepThrough]
            get { return _version; }
            [DebuggerStepThrough]
            set { _version = value; }
        }

        /// <summary>
        /// Nom du modèle
        /// </summary>
        public string Name
        {
            [DebuggerStepThrough()]
            get { return _name; }
            [DebuggerStepThrough()]
            set { _name = value; }
        }

        /// <summary>
        /// Identifiant du modèle
        /// </summary>
        public Guid Id
        {
            [DebuggerStepThrough]
            get { return _id; }
            [DebuggerStepThrough]
            set { _id = value; }
        }

        /// <summary>
        /// Date du modèle associé
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
        /// Indique si le modèle est présent dans le repository local
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
        /// Permet de calculer le chemin du fichier modèle
        /// </summary>
        /// <param name="id">Id du modèle</param>
        /// <param name="version">n° de version</param>
        /// <param name="modelFileName">Nom du fichier (sans le path) du modèle (extension facultative)</param>
        /// <param name="kind">Type de chemin souhaité</param>
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
        /// Chemin physique du modèle
        /// </summary>
        public string GetFileName(PathKind kind)
        {
            return GetFileName(_id, _version, _modelFileName, kind);
        }

        /// <summary>
        /// Permet de résoudre un nom de fichier par rapport au répertoire contenant le modèle
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="kind"></param>
        /// <returns></returns>
        internal string ResolvePath(string fileName, PathKind kind)
        {
            return System.IO.Path.Combine(System.IO.Path.GetDirectoryName(GetFileName(kind)), fileName);
        }

        /// <summary>
        /// Création d'un composant externe à partir correspondant à un modèle
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
        /// Est ce que la dernière version est présente en local ?
        /// </summary>
        /// <returns></returns>
        internal bool IsLastVersion()
        {
            return (!ExistsOnServer || _localVersion >= _serverVersion) && File.Exists(GetFileName(PathKind.Absolute));
        }

        /// <summary>
        /// Met à jour les caractéristiques d'un composant externe à partir des metadonnées
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
        /// Lecture des metadonnées d'un modéle
        /// </summary>
        /// <param name="fileName">Chemin absolu du fichier contenant le modèle</param>
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
        /// Mise à jour des métadata à partir du modèle
        /// </summary>
        /// <param name="item">Metadata à mettre à jour</param>
        /// <param name="fileName">Fichier contenant le modèle</param>
        internal static void UpdateMetadata(ComponentModelMetadata item, string fileName)
        {
            item.ModelFileName = System.IO.Path.GetFileName(fileName);
            ModelLoader loader = null;
            try
            {
                // Chargement du modèle pour récupèrer les metadata
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
                    // On essaye de déduire le n° de version à partir du nom de fichier.
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