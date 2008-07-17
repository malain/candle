using System;
using System.Collections.Generic;
using System.IO;
using DSLFactory.Candle.SystemModel.Repository;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Diagrams;

namespace DSLFactory.Candle.SystemModel
{
    /// <summary>
    /// Classe permettant de charger la définition d'un modèle
    /// </summary>
    public class ModelLoader : IDisposable
    {
        /// <summary>
        /// Classe permettant de gérer le cache
        /// </summary>
        private class CacheItem
        {
            public ModelLoader loader;
            public DateTime lastUpdate;
        }

        /// <summary>
        /// Stockage du cache
        /// </summary>
        private static readonly Dictionary<string, CacheItem> s_modelsCache = new Dictionary<string, CacheItem>();

        private readonly CandleModel _model;
        private string _folderPath;
        private string _fileName;
        private static SerializationResult _serializationResult;

        /// <summary>
        /// Store du modèle
        /// </summary>
        public Store Store
        {
            get { return _model == null ? null : _model.Store; }
        }

        /// <summary>
        /// Chargement d'un modèle
        /// </summary>
        /// <param name="system"></param>
        /// <returns></returns>
        public static ModelLoader GetLoader(ExternalComponent system)
        {
            ComponentModelMetadata metaData = system.MetaData;
            ModelLoader loader = GetLoader(metaData);
            if (loader != null && loader.Model != null && loader.Model.SoftwareComponent != null)
                loader.Model.SoftwareComponent.NamespaceResolver = new ExternalNamespaceProvider(system);
            return loader;
        }

        /// <summary>
        /// Chargement du modèle (Récupèration sur le serveur si il n'est pas présent)
        /// </summary>
        /// <param name="metaData"></param>
        /// <returns></returns>
        public static ModelLoader GetLoader(ComponentModelMetadata metaData)
        {
            if (metaData == null)
                return null;

            string fileName = metaData.GetFileName(PathKind.Absolute);

            bool enableCache = true;
            string tmpFileName = null;

            // Si le modèle n'est pas présent en local, on le récupère dans un endroit temporaire pour pouvoir le charger en
            // mémoire. Ceci est nécessaire car le chargement de ce modèle sert juste à vérifier si il est à jour sur le serveur
            // pour pouvoir ensuite demander à l'utilisateur si il veut le charger. Si on le charge directement dans le repository
            // local, on court-circuite le choix de l'utilisateur.
            if (!File.Exists(fileName))
            {
                enableCache = false; // Pas de mis en cache pour un modèle temporaire

                // On supprime le fichier temporaire sinon il posséde une date qui fait croire, lors de
                // la récupération du modèle sur le serveur, que le fichier en local est plus récent.
                fileName = tmpFileName = Path.GetTempFileName();
                Utils.DeleteFile(fileName);

                RepositoryManager.Instance.GetFileFromRepository(RepositoryCategory.Models,
                                                                 metaData.GetFileName(PathKind.Relative), tmpFileName);
            }
            try
            {
                return GetLoader(fileName, enableCache);
            }
            finally
            {
                // On nettoie le fichier temporaire
                Utils.DeleteFile(tmpFileName);
            }
        }

        /// <summary>
        /// Permet de supprimer un modèle du cache
        /// </summary>
        /// <param name="fileName"></param>
        public static void ClearCache(string fileName)
        {
            if (s_modelsCache != null)
                s_modelsCache.Remove(fileName);
        }

        /// <summary>
        /// Effacement de tout le cache des modèles
        /// </summary>
        public static void ClearCache()
        {
            s_modelsCache.Clear();
        }

        /// <summary>
        /// Chargement d'un modèle à partir du repository local (Il doit être présent)
        /// </summary>
        /// <param name="fileName">Fichier du modèle</param>
        /// <param name="enableCache">Indique si on doit utiliser le cache</param>
        /// <returns></returns>
        public static ModelLoader GetLoader(string fileName, bool enableCache)
        {
            _serializationResult = null;

            if (String.IsNullOrEmpty(fileName))
                return null;

            if (!File.Exists(fileName))
            {
                //ILogger logger = ServiceLocator.Instance.GetService<ILogger>();
                //if( logger != null )
                //    logger.Write("Model Loader", String.Format("The model {0} doesn't exist", fileName), LogType.Error);
                return null;
            }

            try
            {
                if (!enableCache)
                {
                    return new ModelLoader(fileName);
                }

                CacheItem ci;
                // On cherche dans le cache 
                s_modelsCache.TryGetValue(fileName, out ci);

                // Le cache n'est plus valide si le modèle a été modifié entre temps
                DateTime lastUpdateDateTime = File.GetLastWriteTimeUtc(fileName);
                if (ci == null || ci.lastUpdate < lastUpdateDateTime)
                {
                    // Rien dans le cache, on crée une nouvelle entrée
                    if (ci == null)
                        ci = new CacheItem();
                    ci.loader = new ModelLoader(fileName);
                    ci.lastUpdate = lastUpdateDateTime;
                    s_modelsCache[fileName] = ci;
                }

                return ci.loader;
            }
            catch (Exception ex)
            {
                ILogger logger = ServiceLocator.Instance.GetService<ILogger>();
                if (logger != null)
                    logger.WriteError("Load model",
                                      String.Format("Error when loading external system {0}",
                                                    Path.GetFileNameWithoutExtension(fileName)), ex);
                return null;
            }
        }

        /// <summary>
        /// Gets or sets the last serialization result.
        /// </summary>
        /// <value>The last serialization result.</value>
        public static SerializationResult LastSerializationResult
        {
            get { return _serializationResult; }
            set { _serializationResult = value; }
        }

        /// <summary>
        /// Constructeur privé. On ne peut instancier un loader qu'avec GetLoader (qui gére le cache)
        /// </summary>
        /// <param name="fileName"></param>
        private ModelLoader(string fileName)
        {
            SetFileName(fileName);
            Store store = CreateStore();

            // Ouverture d'une transaction avec l'attribut IsSerializing à True
            using (Transaction transaction = store.TransactionManager.BeginTransaction("Load model", true))
            {
                DisableAllRules(store);

                transaction.Context.ContextInfo["InModelLoader"] = true;
                // Chargement du modèle (Peut générer des exceptions)
                _serializationResult = new SerializationResult();
                _model = CandleSerializationHelper.Instance.LoadModel(_serializationResult, store, fileName, null, null);
                transaction.Commit();
            }
        }

        /// <summary>
        /// Création d'un modèle vide
        /// </summary>
        /// <param name="name">Nom du modéle</param>
        /// <param name="version">The version.</param>
        /// <returns></returns>
        public static CandleModel CreateModel(string name, VersionInfo version)
        {
            if (name == null) throw new ArgumentNullException("name");
            if (version == null) throw new ArgumentNullException("version");

            Store store = CreateStore();
            using (Transaction transaction = store.TransactionManager.BeginTransaction("Create model", true))
            {
                CandleModel model = new CandleModel(store);
                model.Name = name;
                model.Version = version;
                transaction.Commit();
                return model;
            }
        }

        /// <summary>
        /// Désactive toutes les règles du modèle
        /// </summary>
        /// <param name="store">The store.</param>
        private static void DisableAllRules(Store store)
        {
            CandleDomainModel.DisableDiagramRules(store);

            foreach (Type type in CandleDomainModel.CustomTypes)
            {
                if (typeof (Rule).IsAssignableFrom(type))
                {
                    store.RuleManager.DisableRule(type);
                }
            }
        }

        /// <summary>
        /// Création d'un store
        /// </summary>
        /// <returns></returns>
        private static Store CreateStore()
        {
            Store store = new Store();

            // Initialisation du store avec la définition des modèles          
            store.LoadDomainModels(
                typeof (CoreDesignSurfaceDomainModel), // Obligatoire 
                typeof (CandleDomainModel)); // Correspond à la classe de votre modèle
            return store;
        }

        /// <summary>
        /// Desctructeur
        /// </summary>
        ~ModelLoader()
        {
            Dispose(true);
        }

        /// <summary>
        /// Gets the model.
        /// </summary>
        /// <value>The model.</value>
        public CandleModel Model
        {
            [System.Diagnostics.DebuggerStepThrough]
            get { return _model; }
        }

        /// <summary>
        /// Gets the data layer.
        /// </summary>
        /// <value>The data layer.</value>
        public DataLayer DataLayer
        {
            get
            {
                if (_model != null && _model.SoftwareComponent != null && _model.SoftwareComponent.IsDataLayerExists)
                    return _model.SoftwareComponent.DataLayer;
                return null;
            }
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected void Dispose(bool disposing)
        {
            if (disposing)
            {
            }
        }

        /// <summary>
        /// Gets or sets the name of the file.
        /// </summary>
        /// <value>The name of the file.</value>
        public string FileName
        {
            get { return _fileName; }
            set { _fileName = value; }
        }

        /// <summary>
        /// Sets the name of the file.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        internal void SetFileName(string fileName)
        {
            _fileName = fileName;
            _folderPath = Path.GetDirectoryName(fileName);
        }

        /// <summary>
        /// Gets the folder path.
        /// </summary>
        /// <value>The folder path.</value>
        public string FolderPath
        {
            get { return _folderPath; }
        }

        /// <summary>
        /// Saves this instance.
        /// </summary>
        /// <returns></returns>
        public bool Save()
        {
            return Save(_fileName);
        }

        /// <summary>
        /// Saves the specified temp file.
        /// </summary>
        /// <param name="tempFile">The temp file.</param>
        /// <returns></returns>
        public bool Save(string tempFile)
        {
            return Save(_model, tempFile);
        }

        /// <summary>
        /// Saves the specified model.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="fileName">Name of the file.</param>
        /// <returns></returns>
        public static bool Save(CandleModel model, string fileName)
        {
            Microsoft.VisualStudio.Modeling.SerializationResult result =
                new Microsoft.VisualStudio.Modeling.SerializationResult();
            CandleSerializationHelper.Instance.SaveModel(result, model, fileName);
            return !result.Failed;
        }
    }
}