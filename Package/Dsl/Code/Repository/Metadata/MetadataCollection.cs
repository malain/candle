using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

namespace DSLFactory.Candle.SystemModel.Repository
{
    /// <summary>
    /// Handler pour l'évenement MetadataChanged
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <returns></returns>
    public delegate void MetadataChangedEventHandler(object sender, MetadataChangedEventArg e);


    /// <summary>
    /// Cache des metadata
    /// </summary>
    public class MetadataCollection : IEnumerable
    {
        /// <summary>
        /// Objet de synchronisation
        /// </summary>
        private readonly object _syncObject = new object();

        /// <summary>
        /// Metadonnées en cache
        /// </summary>
        private List<ComponentModelMetadata> _models;

        #region IEnumerable Members

        /// <summary>
        /// Enumération des metadatas de la liste
        /// </summary>
        /// <returns></returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetMetadatas().GetEnumerator();
        }

        #endregion

        /// <summary>
        /// Evénement déclenché quand tous les metadatas ont été chargées
        /// </summary>
        public event EventHandler MetadatasLoaded;


        /// <summary>
        /// Occurs when [metadata changed].
        /// </summary>
        public event MetadataChangedEventHandler MetadataChanged;

        /// <summary>
        /// Recherche d'un métadata 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="version"></param>
        /// <returns></returns>
        public ComponentModelMetadata Find(Guid id, VersionInfo version)
        {
            if (version == null)
                throw new ArgumentNullException("version");
            if (id == Guid.Empty)
                throw new ArgumentOutOfRangeException("guid");
            return
                GetMetadatas().Find(
                    delegate(ComponentModelMetadata m) { return m.Id == id && version.Equals(m.Version); });
        }

        /// <summary>
        /// Recherche d'un metadata sur un numéro de version partiel 
        /// </summary>
        /// <param name="guid">Id du composant</param>
        /// <param name="version">N° de version </param>
        /// <param name="nb">Indique sur combien de parties de la version on effectue le test</param>
        /// <returns></returns>
        internal ComponentModelMetadata FindWithPartialVersion(Guid guid, VersionInfo version, int nb)
        {
            if (version == null)
                throw new ArgumentNullException("version");
            if (guid == Guid.Empty)
                throw new ArgumentOutOfRangeException("guid");
            if (nb < 1 || nb > 4)
                throw new ArgumentOutOfRangeException("nb must be between 1 and 4");

            return
                GetMetadatas().Find(
                    delegate(ComponentModelMetadata m) { return m.Id == guid && version.Equals(m.Version, nb); });
        }

        /// <summary>
        /// Recherche d'un métadata
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public bool NameExists(string name)
        {
            if (String.IsNullOrEmpty(name))
                return false;

            return
                GetMetadatas().Exists(
                    delegate(ComponentModelMetadata m) { return Utils.StringCompareEquals(m.Name, name); });
        }

        /// <summary>
        /// Suppression de la liste
        /// </summary>
        /// <param name="metadata">The metadata.</param>
        public void RemoveLocal(ComponentModelMetadata metadata)
        {
            // Si le modèle existe sur le serveur, on met à jour les metadata sans les supprimer
            if (metadata.ExistsOnServer)
            {
                metadata.Location = RepositoryLocation.Server;
                metadata.Version.Revision = metadata.ServerVersion;
                metadata.LocalVersion = -1;
            }
            else
            {
                // Si il n'existe pas sur le serveur, il n'y a pas de raison de le garder
                GetMetadatas().Remove(metadata);
            }

            MetadataChangedEventArg e = new MetadataChangedEventArg(metadata);
            OnMetadataChanged(e);
        }

        /// <summary>
        /// Récupère toutes les versions disponibles pour un modèle
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        public List<ComponentModelMetadata> GetAllVersions(Guid id)
        {
            if (id == Guid.Empty)
                throw new ArgumentOutOfRangeException("id");
            return GetMetadatas().FindAll(delegate(ComponentModelMetadata m) { return m.Id == id; });
        }

        /// <summary>
        /// Retourne la liste des métadata et assure qu'elle ne sera jamais nulle.
        /// </summary>
        /// <returns></returns>
        private List<ComponentModelMetadata> GetMetadatas()
        {
            // Si la liste est vide, on la recharge
            if (_models == null)
            {
                LoadAll();
            }
            return _models;
        }

        /// <summary>
        /// RAZ de la liste des métadatas
        /// </summary>
        internal void Clear()
        {
            // Force la relecture au prochain accés
            _models = null;
        }

        /// <summary>
        /// Déclenchement de l'événement MetadataLoaded
        /// </summary>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        public virtual void OnMetadataLoaded(EventArgs e)
        {
            if (MetadatasLoaded != null)
                MetadatasLoaded(this, e);
        }

        /// <summary>
        /// Chargement des metadata à partir du répertoire local et des serveurs distants
        /// </summary>
        /// <returns></returns>
        internal MetadataCollection LoadAll()
        {
            if (_models != null)
                return this;

            // Vérouillage
            lock (_syncObject)
            {
                if (_models != null)
                    return this;

                ILogger logger = ServiceLocator.Instance.GetService<ILogger>();

                // Lecture dans les référentiels - Local en premier
                _models = new List<ComponentModelMetadata>();

                // Local en premier
                IRepositoryProvider localProvider = RepositoryManager.Instance.GetLocalProvider();
                if (localProvider != null)
                    Merge(RepositoryLocation.Local, localProvider.GetAllMetadata());

                // Puis les serveurs
                foreach (IRepositoryProvider provider in RepositoryManager.Instance.GetRemoteProviders())
                {
                    try
                    {
                        List<ComponentModelMetadata> list = provider.GetAllMetadata();
                        if (list != null)
                        {
                            // On met à jour l'url du serveur d'origine
                            list.ForEach(delegate(ComponentModelMetadata m) { m.ServerUrl = provider.Name; });
                            Merge(RepositoryLocation.Server, list);
                        }
                    }
                    catch (Exception ex)
                    {
                        if (logger != null)
                            logger.WriteError("GetMetadatas",
                                              String.Format("Error when trying to retrieve the model list"), ex);
                    }
                }

                // Evénement fin de chargement
                OnMetadataLoaded(new EventArgs());
            }

            return this;
        }

        /// <summary>
        /// Mise à jour à partir d'un modèle local
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="fileName">Name of the file.</param>
        internal void Update(ComponentModelMetadata data, string fileName)
        {
            if (data != null)
            {
                ComponentModelMetadata.UpdateMetadata(data, fileName);
            }
            else
            {
                data = ComponentModelMetadata.RetrieveMetadata(fileName);
                Merge(RepositoryLocation.Local, data);
                data = Find(data.Id, data.Version);
            }

            MetadataChangedEventArg e = new MetadataChangedEventArg(data);
            OnMetadataChanged(e);
        }

        /// <summary>
        /// Indique qu'un modèle vient d'être publié
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="model">The model.</param>
        internal void SetMetadataPublished(ComponentModelMetadata data, CandleModel model)
        {
            data.ServerVersion = data.LocalVersion = model.Version.Revision;

            MetadataChangedEventArg e = new MetadataChangedEventArg(data);
            OnMetadataChanged(e);
        }

        /// <summary>
        /// Indique qu'un modèle a été chargé en local
        /// </summary>
        /// <param name="metaData">The meta data.</param>
        internal void SetMetadataLoaded(ComponentModelMetadata metaData)
        {
            metaData.Location = RepositoryLocation.Local;
            metaData.LocalVersion = metaData.ServerVersion;

            MetadataChangedEventArg e = new MetadataChangedEventArg(metaData);
            OnMetadataChanged(e);
        }

        /// <summary>
        /// Génération de l'événement MetadataChanged
        /// </summary>
        /// <param name="e">The e.</param>
        private void OnMetadataChanged(MetadataChangedEventArg e)
        {
            if (MetadataChanged != null)
                MetadataChanged(this, e);
        }

        /// <summary>
        /// Fusion avec un les metadatas existantes
        /// </summary>
        /// <param name="origin">Indique l'origine de la metadata à fusionner</param>
        /// <param name="metadata">Metadata à fusionner</param>
        public void Merge(RepositoryLocation origin, ComponentModelMetadata metadata)
        {
            Debug.Assert(_models != null);

            List<ComponentModelMetadata> list = new List<ComponentModelMetadata>();
            list.Add(metadata);
            Merge(origin, list);
        }

        /// <summary>
        /// Fusion entre les metadata locales et distantes
        /// </summary>
        /// <param name="origin">The origin.</param>
        /// <param name="list">Liste des metadata à fusionner avec celles existantes</param>
        public void Merge(RepositoryLocation origin, List<ComponentModelMetadata> list)
        {
            if (list == null)
                return;

            // Parcours des metadatas à fusionner
            foreach (ComponentModelMetadata item in list)
            {
                // Recherche parmi celle existante (on n'utilise pas la méthode FindWithPartialVersion car celle-ci appele
                // GetMetadata qui utilise Merge en interne). On se base sur la liste des modèles actuelles
                ComponentModelMetadata cmm =
                    _models.Find(
                        delegate(ComponentModelMetadata m) { return m.Id == item.Id && item.Version.Equals(m.Version, 3); }
                        );

                // Il n'existe pas dèja
                if (cmm == null)
                {
                    // On le rajoute
                    _models.Add(item);

                    // On met à jour les n° de version en partant du principe que un metadata
                    // à sa version serveur initialisé par défaut.
                    if (origin == RepositoryLocation.Server)
                    {
                        item.Location = RepositoryLocation.Server;
                        // On garde la version serveur et on indique qu'il n'y a rien en local
                        item.LocalVersion = -1; // Rien en local
                    }
                    else
                    {
                        // On est en local. On prend la version serveur initialisé pour mettre à jour la version locale
                        item.LocalVersion = item.ServerVersion;
                        // Et on indique qu'il n'y a rien sur le serveur
                        item.ServerVersion = -1;
                    }
                }
                else // Fusion
                {
                    // On met à jour un metadata qui vient du serveur
                    if (origin == RepositoryLocation.Server)
                    {
                        // Le référentiel ne retourne que des informations considérées comme locales
                        // donc on met à jour l'information serveur maintenant.
                        // Les infos du serveur priment
                        cmm.Path = item.Path;
                        cmm.Name = item.Name;
                        if (!String.IsNullOrEmpty(item.Description))
                            cmm.Description = item.Description;
                        cmm.ModelFileName = item.ModelFileName;
                        cmm.ServerUrl = item.ServerUrl;
                        if (!String.IsNullOrEmpty(item.TestBaseAddress))
                            cmm.TestBaseAddress = item.TestBaseAddress;
                        if (!String.IsNullOrEmpty(item.DocUrl))
                            cmm.DocUrl = item.DocUrl;

                        cmm.ServerVersion = item.ServerVersion;
                    }
                    else // On est en local
                    {
                        cmm.ModelFileName = item.ModelFileName;
                        cmm.LocalVersion = item.ServerVersion; // Initialisation de la version locale
                        cmm.Location = RepositoryLocation.Local;
                    }
                }
            }
        }
    }
}