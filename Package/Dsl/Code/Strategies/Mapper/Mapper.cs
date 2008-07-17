using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using System.Xml.Serialization;
using EnvDTE;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;

namespace DSLFactory.Candle.SystemModel.Strategies
{
    /// <summary>
    /// Classe permettant de créer une association entre les éléments
    /// du modèle et les fichiers générés. Cette association posséde une
    /// propriété 'CanRegenerate' modifiable par l'utilisateur via un extender
    /// de Visual Studio <see cref="CandleExtender"/>.
    /// </summary>
    public class Mapper : IVsTrackProjectDocumentsEvents2
    {
        /// <summary>
        /// 
        /// </summary>
        public const string FileExtension = ".map";
        private static readonly Mapper s_instance = new Mapper();
        private static TrackDocumentEventHandler s_directoryRemovedDelegate;
        private static TrackDocumentEventHandler s_fileRemovedDelegate;

        private MapperConfiguration _config;
        private string _filePath;
        private bool _initialized;
        private CandleModel _model;
        private IServiceProvider _serviceProvider;
        private string _solutionFolder;
        private uint _trackProjectDocuments2Cookie;
        private List<MapItem> _transactionItemList;

        /// <summary>
        /// Initializes a new instance of the <see cref="Mapper"/> class.
        /// </summary>
        internal Mapper()
        {
            s_fileRemovedDelegate = OnFileRemoved;
            s_directoryRemovedDelegate = OnDirectoryRemoved;

            //DSLFactory.Candle.SystemModel.Utilities.HierarchyVisitor visitor = new DSLFactory.Candle.SystemModel.Utilities.HierarchyVisitor();
            //visitor.Traverse(ProcessHierarchyNode);
        }

        /// <summary>
        /// Singleton
        /// </summary>
        /// <value>The instance.</value>
        public static Mapper Instance
        {
            get { return s_instance; }
        }

        /// <summary>
        /// Teste si le mapper est actif (si il a été mis à jour par le package)
        /// </summary>
        /// <value><c>true</c> if this instance is valid; otherwise, <c>false</c>.</value>
        internal bool IsValid
        {
            get { return _filePath != null; }
        }

        /// <summary>
        /// Liste des associations
        /// </summary>
        /// <value>The items.</value>
        internal List<MapItem> Items
        {
            get { return _config == null ? null : _config.Items; }
        }

        #region IVsTrackProjectDocumentsEvents2 Members

        /// <summary>
        /// Called when [after add directories ex].
        /// </summary>
        /// <param name="cProjects">The c projects.</param>
        /// <param name="cDirectories">The c directories.</param>
        /// <param name="rgpProjects">The RGP projects.</param>
        /// <param name="rgFirstIndices">The rg first indices.</param>
        /// <param name="rgpszMkDocuments">The RGPSZ mk documents.</param>
        /// <param name="rgFlags">The rg flags.</param>
        /// <returns></returns>
        public int OnAfterAddDirectoriesEx(int cProjects, int cDirectories, IVsProject[] rgpProjects,
                                           int[] rgFirstIndices, string[] rgpszMkDocuments,
                                           VSADDDIRECTORYFLAGS[] rgFlags)
        {
            return 0;
        }

        /// <summary>
        /// Called when [after add files ex].
        /// </summary>
        /// <param name="cProjects">The c projects.</param>
        /// <param name="cFiles">The c files.</param>
        /// <param name="rgpProjects">The RGP projects.</param>
        /// <param name="rgFirstIndices">The rg first indices.</param>
        /// <param name="rgpszMkDocuments">The RGPSZ mk documents.</param>
        /// <param name="rgFlags">The rg flags.</param>
        /// <returns></returns>
        public int OnAfterAddFilesEx(int cProjects, int cFiles, IVsProject[] rgpProjects, int[] rgFirstIndices,
                                     string[] rgpszMkDocuments, VSADDFILEFLAGS[] rgFlags)
        {
            return 0;
        }

        /// <summary>
        /// Called when [after remove directories].
        /// </summary>
        /// <param name="cProjects">The c projects.</param>
        /// <param name="cDirectories">The c directories.</param>
        /// <param name="rgpProjects">The RGP projects.</param>
        /// <param name="rgFirstIndices">The rg first indices.</param>
        /// <param name="rgpszMkDocuments">The RGPSZ mk documents.</param>
        /// <param name="rgFlags">The rg flags.</param>
        /// <returns></returns>
        public int OnAfterRemoveDirectories(int cProjects, int cDirectories, IVsProject[] rgpProjects,
                                            int[] rgFirstIndices, string[] rgpszMkDocuments,
                                            VSREMOVEDIRECTORYFLAGS[] rgFlags)
        {
            OnTrackProjectDocuments(cProjects, cDirectories, rgpProjects, rgFirstIndices, rgpszMkDocuments,
                                    s_directoryRemovedDelegate);
            return 0;
        }

        /// <summary>
        /// Called when [after remove files].
        /// </summary>
        /// <param name="cProjects">The c projects.</param>
        /// <param name="cFiles">The c files.</param>
        /// <param name="rgpProjects">The RGP projects.</param>
        /// <param name="rgFirstIndices">The rg first indices.</param>
        /// <param name="rgpszMkDocuments">The RGPSZ mk documents.</param>
        /// <param name="rgFlags">The rg flags.</param>
        /// <returns></returns>
        public int OnAfterRemoveFiles(int cProjects, int cFiles, IVsProject[] rgpProjects, int[] rgFirstIndices,
                                      string[] rgpszMkDocuments, VSREMOVEFILEFLAGS[] rgFlags)
        {
            OnTrackProjectDocuments(cProjects, cFiles, rgpProjects, rgFirstIndices, rgpszMkDocuments,
                                    s_fileRemovedDelegate);
            return 0;
        }

        /// <summary>
        /// Called when [after rename directories].
        /// </summary>
        /// <param name="cProjects">The c projects.</param>
        /// <param name="cDirs">The c dirs.</param>
        /// <param name="rgpProjects">The RGP projects.</param>
        /// <param name="rgFirstIndices">The rg first indices.</param>
        /// <param name="rgszMkOldNames">The RGSZ mk old names.</param>
        /// <param name="rgszMkNewNames">The RGSZ mk new names.</param>
        /// <param name="rgFlags">The rg flags.</param>
        /// <returns></returns>
        public int OnAfterRenameDirectories(int cProjects, int cDirs, IVsProject[] rgpProjects, int[] rgFirstIndices,
                                            string[] rgszMkOldNames, string[] rgszMkNewNames,
                                            VSRENAMEDIRECTORYFLAGS[] rgFlags)
        {
            return 0;
        }

        /// <summary>
        /// Called when [after rename files].
        /// </summary>
        /// <param name="cProjects">The c projects.</param>
        /// <param name="cFiles">The c files.</param>
        /// <param name="rgpProjects">The RGP projects.</param>
        /// <param name="rgFirstIndices">The rg first indices.</param>
        /// <param name="rgszMkOldNames">The RGSZ mk old names.</param>
        /// <param name="rgszMkNewNames">The RGSZ mk new names.</param>
        /// <param name="rgFlags">The rg flags.</param>
        /// <returns></returns>
        public int OnAfterRenameFiles(int cProjects, int cFiles, IVsProject[] rgpProjects, int[] rgFirstIndices,
                                      string[] rgszMkOldNames, string[] rgszMkNewNames, VSRENAMEFILEFLAGS[] rgFlags)
        {
            return 0;
        }

        /// <summary>
        /// Called when [after SCC status changed].
        /// </summary>
        /// <param name="cProjects">The c projects.</param>
        /// <param name="cFiles">The c files.</param>
        /// <param name="rgpProjects">The RGP projects.</param>
        /// <param name="rgFirstIndices">The rg first indices.</param>
        /// <param name="rgpszMkDocuments">The RGPSZ mk documents.</param>
        /// <param name="rgdwSccStatus">The RGDW SCC status.</param>
        /// <returns></returns>
        public int OnAfterSccStatusChanged(int cProjects, int cFiles, IVsProject[] rgpProjects, int[] rgFirstIndices,
                                           string[] rgpszMkDocuments, uint[] rgdwSccStatus)
        {
            return 0;
        }

        /// <summary>
        /// Called when [query add directories].
        /// </summary>
        /// <param name="pProject">The p project.</param>
        /// <param name="cDirectories">The c directories.</param>
        /// <param name="rgpszMkDocuments">The RGPSZ mk documents.</param>
        /// <param name="rgFlags">The rg flags.</param>
        /// <param name="pSummaryResult">The p summary result.</param>
        /// <param name="rgResults">The rg results.</param>
        /// <returns></returns>
        public int OnQueryAddDirectories(IVsProject pProject, int cDirectories, string[] rgpszMkDocuments,
                                         VSQUERYADDDIRECTORYFLAGS[] rgFlags, VSQUERYADDDIRECTORYRESULTS[] pSummaryResult,
                                         VSQUERYADDDIRECTORYRESULTS[] rgResults)
        {
            return 0;
        }

        /// <summary>
        /// Called when [query add files].
        /// </summary>
        /// <param name="pProject">The p project.</param>
        /// <param name="cFiles">The c files.</param>
        /// <param name="rgpszMkDocuments">The RGPSZ mk documents.</param>
        /// <param name="rgFlags">The rg flags.</param>
        /// <param name="pSummaryResult">The p summary result.</param>
        /// <param name="rgResults">The rg results.</param>
        /// <returns></returns>
        public int OnQueryAddFiles(IVsProject pProject, int cFiles, string[] rgpszMkDocuments,
                                   VSQUERYADDFILEFLAGS[] rgFlags, VSQUERYADDFILERESULTS[] pSummaryResult,
                                   VSQUERYADDFILERESULTS[] rgResults)
        {
            return 0;
        }

        /// <summary>
        /// Called when [query remove directories].
        /// </summary>
        /// <param name="pProject">The p project.</param>
        /// <param name="cDirectories">The c directories.</param>
        /// <param name="rgpszMkDocuments">The RGPSZ mk documents.</param>
        /// <param name="rgFlags">The rg flags.</param>
        /// <param name="pSummaryResult">The p summary result.</param>
        /// <param name="rgResults">The rg results.</param>
        /// <returns></returns>
        public int OnQueryRemoveDirectories(IVsProject pProject, int cDirectories, string[] rgpszMkDocuments,
                                            VSQUERYREMOVEDIRECTORYFLAGS[] rgFlags,
                                            VSQUERYREMOVEDIRECTORYRESULTS[] pSummaryResult,
                                            VSQUERYREMOVEDIRECTORYRESULTS[] rgResults)
        {
            return 0;
        }

        /// <summary>
        /// Called when [query remove files].
        /// </summary>
        /// <param name="pProject">The p project.</param>
        /// <param name="cFiles">The c files.</param>
        /// <param name="rgpszMkDocuments">The RGPSZ mk documents.</param>
        /// <param name="rgFlags">The rg flags.</param>
        /// <param name="pSummaryResult">The p summary result.</param>
        /// <param name="rgResults">The rg results.</param>
        /// <returns></returns>
        public int OnQueryRemoveFiles(IVsProject pProject, int cFiles, string[] rgpszMkDocuments,
                                      VSQUERYREMOVEFILEFLAGS[] rgFlags, VSQUERYREMOVEFILERESULTS[] pSummaryResult,
                                      VSQUERYREMOVEFILERESULTS[] rgResults)
        {
            return 0;
        }

        /// <summary>
        /// Called when [query rename directories].
        /// </summary>
        /// <param name="pProject">The p project.</param>
        /// <param name="cDirs">The c dirs.</param>
        /// <param name="rgszMkOldNames">The RGSZ mk old names.</param>
        /// <param name="rgszMkNewNames">The RGSZ mk new names.</param>
        /// <param name="rgFlags">The rg flags.</param>
        /// <param name="pSummaryResult">The p summary result.</param>
        /// <param name="rgResults">The rg results.</param>
        /// <returns></returns>
        public int OnQueryRenameDirectories(IVsProject pProject, int cDirs, string[] rgszMkOldNames,
                                            string[] rgszMkNewNames, VSQUERYRENAMEDIRECTORYFLAGS[] rgFlags,
                                            VSQUERYRENAMEDIRECTORYRESULTS[] pSummaryResult,
                                            VSQUERYRENAMEDIRECTORYRESULTS[] rgResults)
        {
            return 0;
        }

        /// <summary>
        /// Called when [query rename files].
        /// </summary>
        /// <param name="pProject">The p project.</param>
        /// <param name="cFiles">The c files.</param>
        /// <param name="rgszMkOldNames">The RGSZ mk old names.</param>
        /// <param name="rgszMkNewNames">The RGSZ mk new names.</param>
        /// <param name="rgFlags">The rg flags.</param>
        /// <param name="pSummaryResult">The p summary result.</param>
        /// <param name="rgResults">The rg results.</param>
        /// <returns></returns>
        public int OnQueryRenameFiles(IVsProject pProject, int cFiles, string[] rgszMkOldNames, string[] rgszMkNewNames,
                                      VSQUERYRENAMEFILEFLAGS[] rgFlags, VSQUERYRENAMEFILERESULTS[] pSummaryResult,
                                      VSQUERYRENAMEFILERESULTS[] rgResults)
        {
            return 0;
        }

        #endregion

        ///// <summary>
        ///// Processes the hierarchy node.
        ///// </summary>
        ///// <param name="hierarchy">The hierarchy.</param>
        ///// <param name="itemid">The itemid.</param>
        ///// <param name="recursionLevel">The recursion level.</param>
        //private void ProcessHierarchyNode(IVsHierarchy hierarchy, uint itemid, int recursionLevel)
        //{
        //    //object obj = null;
        //    //if ((hierarchy.GetProperty(itemid, -2027, out obj) == 0) && (obj != null))
        //    //{
        //    //    return obj as ProjectItem;
        //    //}
        //}

        /// <summary>
        /// Désabonnement
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        internal void Dispose(bool disposing)
        {
            if (disposing)
            {
                if ((_trackProjectDocuments2Cookie != 0) && (_serviceProvider != null))
                {
                    try
                    {
                        IVsTrackProjectDocuments2 service =
                            (IVsTrackProjectDocuments2) _serviceProvider.GetService(typeof (SVsTrackProjectDocuments));
                        if (service != null)
                        {
                            service.UnadviseTrackProjectDocumentsEvents(_trackProjectDocuments2Cookie);
                        }
                    }
                    catch (InvalidCastException)
                    {
                    }
                    _trackProjectDocuments2Cookie = 0;
                }
            }
        }


        /// <summary>
        /// Sauvegarde du fichier de mapping
        /// </summary>
        public void Save()
        {
            if (_filePath == null)
                return;

            ServiceLocator.Instance.ShellHelper.EnsureCheckout(_filePath);
            ServiceLocator.Instance.ShellHelper.EnsureDocumentIsNotInRDT(_filePath);

            using (SafeStreamWriter writer = new SafeStreamWriter(_filePath))
            {
                XmlSerializer ser = new XmlSerializer(typeof (MapperConfiguration));
                ser.Serialize(writer, _config);
            }
        }

        /// <summary>
        /// Chargement du fichier
        /// </summary>
        public void Load()
        {
            if (_filePath == null)
                return;

            if (File.Exists(_filePath))
            {
                try
                {
                    using (StreamReader reader = new StreamReader(_filePath))
                    {
                        XmlSerializer ser = new XmlSerializer(typeof (MapperConfiguration));
                        _config = (MapperConfiguration) ser.Deserialize(reader);
                    }
                    if (_config != null)
                        return;
                }
                catch (Exception ex)
                {
                    ILogger logger = ServiceLocator.Instance.GetService<ILogger>();
                    if (logger != null)
                        logger.WriteError("Mapper", String.Format("Unable to load map file {0}", _filePath), ex);
                }
            }

            _config = new MapperConfiguration();

            Save();
        }

        /// <summary>
        /// Registers the events.
        /// </summary>
        internal void RegisterEvents()
        {
            if (_model != null && _model.Store != null && !_initialized)
            {
                _model.Store.EventManagerDirectory.ElementAdded.Add(
                    new EventHandler<ElementAddedEventArgs>(OnElementAdded));
                _model.Store.EventManagerDirectory.ElementPropertyChanged.Add(
                    new EventHandler<ElementPropertyChangedEventArgs>(OnPropertyChanged));
                _model.Store.EventManagerDirectory.ElementDeleted.Add(
                    new EventHandler<ElementDeletedEventArgs>(OnElementDeleted));
                _initialized = true;
            }
        }

        /// <summary>
        /// Initialisation du modèle (lors du chargement du modèle dans le DocData)
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="fileName">Name of the file.</param>
        internal void SetModel(CandleModel model, string fileName)
        {
            // On ne charge que celui de la solution courante (au cas ou on a ouvert d'autre modèle en navigant)
            string folder = Path.GetDirectoryName(fileName);
            try
            {
                if (!Utils.StringCompareEquals(folder, ServiceLocator.Instance.ShellHelper.SolutionFolder))
                    return;
            }
            catch
            {
                // Aucune solution ouverte
                return;
            }

            fileName = Path.ChangeExtension(fileName, FileExtension);
            if (_config != null && fileName == _filePath)
                return;

            // Si il y avait un modèle, on se désabonne.
            if (_model != null && _model.Store != null)
            {
                Dispose(true);

                model.Store.EventManagerDirectory.ElementAdded.Remove(
                    new EventHandler<ElementAddedEventArgs>(OnElementAdded));
                model.Store.EventManagerDirectory.ElementDeleted.Remove(
                    new EventHandler<ElementDeletedEventArgs>(OnElementDeleted));
                model.Store.EventManagerDirectory.ElementPropertyChanged.Remove(
                    new EventHandler<ElementPropertyChangedEventArgs>(OnPropertyChanged));
                _initialized = false;
            }

            _filePath = fileName;
            _model = model;

            if (model != null)
            {
                if (Initialize())
                {
                    Load();
                    try
                    {
                        ServiceLocator.Instance.ShellHelper.AddFileToSolution(_filePath);
                    }
                    catch
                    {
                    }
                }
            }
        }

        /// <summary>
        /// Permet de s'abonner aux événements sur les fichiers de la solution
        /// </summary>
        /// <returns></returns>
        private bool Initialize()
        {
            try
            {
                _serviceProvider =
                    new ServiceProvider(
                        ServiceLocator.Instance.ShellHelper.Solution.DTE as
                        Microsoft.VisualStudio.OLE.Interop.IServiceProvider);
                _solutionFolder = ServiceLocator.Instance.ShellHelper.SolutionFolder;
                if (_serviceProvider != null)
                {
                    IVsTrackProjectDocuments2 service =
                        (IVsTrackProjectDocuments2) _serviceProvider.GetService(typeof (SVsTrackProjectDocuments));
                    ErrorHandler.ThrowOnFailure(
                        service.AdviseTrackProjectDocumentsEvents(this, out _trackProjectDocuments2Cookie));
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Stocke l'association d'un fichier avec le contexte qui l'a
        /// généré
        /// </summary>
        /// <param name="projectName">Nom du projet VS contenant le fichier</param>
        /// <param name="fileName">Fichier généré</param>
        /// <param name="strategyId">ID de la stratégie</param>
        /// <param name="elementId">ID de l'élément concerné</param>
        /// <returns>
        /// Une association ou null si le nom du fichier est invalide
        /// </returns>
        internal MapItem Associate(string projectName, string fileName, string strategyId, Guid elementId)
        {
            if (String.IsNullOrEmpty(fileName) || !File.Exists(fileName))
                return null;

            MapItem item = FindMapItem(fileName);
            if (item == null)
            {
                item = new MapItem();
//                item.StrategyOwner = owner;
                item.ProjectName = projectName;
                item.FileName = MakeRelative(fileName);
                Items.Add(item);
                item.ElementId = elementId;
                item.StrategyId = strategyId;
                //Save();
            }
            return item;
        }

        /// <summary>
        /// Purge des maps d'une stratégie pour un élément et une stratégie
        /// </summary>
        /// <param name="strategyId">ID de la stratégie</param>
        /// <param name="elementId">ID de l'élément</param>
        internal void Purge(string strategyId, Guid elementId)
        {
            Purge(
                delegate(MapItem currentItem) { return currentItem.ElementId == elementId && currentItem.StrategyId == strategyId; });
        }

        /// <summary>
        /// Suppression des fichiers générés par une strategie
        /// </summary>
        /// <param name="elementId">The element id.</param>
        internal void RemoveStrategyGeneratedFiles(Guid elementId)
        {
            foreach (Project prj in ServiceLocator.Instance.ShellHelper.AllProjects)
            {
                // Force la suppression physique des fichiers
                DeleteFiles(prj,
                            Items.FindAll(
                                delegate(MapItem currentItem)
                                    {
                                        return
                                            currentItem.ElementId == elementId &&
                                            (currentItem.ProjectName == prj.Name ||
                                             String.IsNullOrEmpty(currentItem.ProjectName));
                                    })
                            , true);
            }

            // Puis suppression dans le fichier de mapping
            Purge(delegate(MapItem currentItem) { return currentItem.ElementId == elementId; });
        }

        /// <summary>
        /// Suppression des fichiers générés par une strategie
        /// </summary>
        /// <param name="strategyId">The strategy id.</param>
        /// <param name="owner">The owner.</param>
        internal void RemoveStrategyGeneratedFiles(string strategyId, string owner)
        {
            // TODO owner à prendre en compte
            foreach (Project prj in ServiceLocator.Instance.ShellHelper.AllProjects)
            {
                // Force la suppression physique des fichiers
                DeleteFiles(prj,
                            Items.FindAll(
                                delegate(MapItem currentItem)
                                    {
                                        return
                                            currentItem.StrategyId == strategyId &&
                                            (currentItem.ProjectName == prj.Name ||
                                             String.IsNullOrEmpty(currentItem.ProjectName));
                                    })
                            , true);
            }

            // Puis suppression dans le fichier de mapping
            Purge(delegate(MapItem currentItem) { return currentItem.StrategyId == strategyId; });
        }

        /// <summary>
        /// Purge des maps d'un élément
        /// </summary>
        /// <param name="elementId">The element id.</param>
        internal void Purge(Guid elementId)
        {
            Purge(delegate(MapItem currentItem) { return currentItem.ElementId == elementId; });
        }

        /// <summary>
        /// Purge des associations par rapport à une méthode de filtrage
        /// </summary>
        /// <param name="filter">The filter.</param>
        private void Purge(Predicate<MapItem> filter)
        {
            Queue<MapItem> queue = new Queue<MapItem>(Items.FindAll(filter));
            if (queue.Count > 0)
            {
                while (queue.Count > 0)
                {
                    MapItem item = queue.Dequeue();
                    Items.Remove(item);
                }
                Save();
            }
        }

        /// <summary>
        /// Purge d'un dossier
        /// </summary>
        /// <param name="path">Path du dossier</param>
        internal void PurgeFolder(string path)
        {
            Queue<MapItem> queue = new Queue<MapItem>(Items.FindAll(
                                                          delegate(MapItem currentItem)
                                                              {
                                                                  return
                                                                      Utils.StringStartsWith(currentItem.FileName, path);
                                                              }
                                                          ));

            if (queue.Count > 0)
            {
                while (queue.Count > 0)
                {
                    MapItem item = queue.Dequeue();
                    Items.Remove(item);
                }
                Save();
            }
        }

        /// <summary>
        /// Called when [property changed].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="Microsoft.VisualStudio.Modeling.ElementPropertyChangedEventArgs"/> instance containing the event data.</param>
        private static void OnPropertyChanged(object sender, ElementPropertyChangedEventArgs e)
        {
            //if (e.DomainProperty.Id == NamedElement.NameDomainPropertyId)
            //{
            //    foreach (MapItem item in Items.FindAll(delegate(MapItem currentItem) { return currentItem.ElementId == e.ElementId; }))
            //    {
            //        if (Path.GetFileNameWithoutExtension(item.FileName) == (string)e.OldValue)
            //        {
            //            string fileName = ResolvePath(item.FileName);
            //            string newFileName = Path.Combine(Path.GetDirectoryName(fileName), (string)e.NewValue + Path.GetExtension(fileName));
            //            ProjectItem projectItem = FindProjectItem(fileName);
            //            if (projectItem != null)
            //            {
            //                projectItem.SaveAs(newFileName);
            //                Utils.DeleteFile(fileName);
            //                item.FileName = MakeRelative(newFileName);
            //            }
            //        }
            //    }
            //}
        }

        /// <summary>
        /// Called when [element added].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="Microsoft.VisualStudio.Modeling.ElementAddedEventArgs"/> instance containing the event data.</param>
        private static void OnElementAdded(object sender, ElementAddedEventArgs e)
        {
        }

        /// <summary>
        /// Suppression d'un élément dans le modèle
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="Microsoft.VisualStudio.Modeling.ElementDeletedEventArgs"/> instance containing the event data.</param>
        private void OnElementDeleted(object sender, ElementDeletedEventArgs e)
        {
            Purge(e.ElementId);
        }

        /// <summary>
        /// Suppression d'un dossier dans le projet
        /// </summary>
        /// <param name="path">The path.</param>
        private void OnDirectoryRemoved(string path)
        {
            PurgeFolder(MakeRelative(path));
        }

        /// <summary>
        /// Called when [file removed].
        /// </summary>
        /// <param name="path">The path.</param>
        private void OnFileRemoved(string path)
        {
            MapItem item = FindMapItem(path);
            if (item != null)
            {
                Items.Remove(item);
                Save();
            }
        }

        /// <summary>
        /// Gets the files.
        /// </summary>
        /// <param name="elementId">The element id.</param>
        /// <param name="canGenerate">if set to <c>true</c> [can generate].</param>
        /// <returns></returns>
        internal List<string> GetFiles(Guid elementId, bool canGenerate)
        {
            List<string> files = new List<string>();
            foreach (MapItem item in Items)
            {
                if (item.ElementId == elementId && item.CanGenerate == canGenerate)
                {
                    files.Add(ResolvePath(item.FileName));
                }
            }
            return files;
        }

        /// <summary>
        /// Affichage du source correspondant à un élément. Si plusieurs fichiers
        /// correspondent, une fenètre de sélection est affichée
        /// </summary>
        /// <param name="elementId">ID de l'élément</param>
        /// <param name="className">Nom de la classe ou null</param>
        /// <param name="operationName">Nom de la méthode ou null</param>
        public void ShowCode(Guid elementId, string className, string operationName)
        {
            try
            {
                // Recherche des fichiers générés
                List<string> files1 = GetFiles(elementId, false);
                List<string> files2 = GetFiles(elementId, true);

                string fileName = null;
                if (files1.Count == 0 && files2.Count == 0)
                {
                    // On essaye de chercher le fichier dans la solution
                    fileName =
                        String.Concat(className, StrategyManager.GetInstance(_model.Store).TargetLanguage.Extension);
                    ProjectItem item = ServiceLocator.Instance.ShellHelper.FindProjectItemByFileName(null, fileName);
                    if (item == null)
                        return; // Aucun
                    fileName = item.get_FileNames(1);
                }
                else if (files1.Count == 1 && files2.Count == 0)
                    fileName = files1[0];
                else if (files1.Count == 0 && files2.Count == 1)
                    fileName = files2[0];
                else
                {
                    // Il y en a plusieurs, on affiche une fenètre de sélection
                    SelectFile dlg = new SelectFile(files1, files2);
                    if (dlg.ShowDialog() == DialogResult.OK)
                        fileName = dlg.SelectedFile;
                }

                if (fileName != null)
                {
                    ServiceLocator.Instance.ShellHelper.TryToShow(fileName, className, operationName);
                }
            }
            catch (Exception ex)
            {
                ILogger logger = ServiceLocator.Instance.GetService<ILogger>();
                if (logger != null)
                    logger.WriteError("Show code", "Unable to show code for " + className, ex);
            }
        }

        ///// <summary>
        ///// Finds the project item.
        ///// </summary>
        ///// <param name="fileName">Name of the file.</param>
        ///// <returns></returns>
        //private ProjectItem FindProjectItem(string fileName)
        //{
        //    foreach (Project project in ServiceLocator.Instance.ShellHelper.Solution.Projects)
        //    {
        //        ProjectItem projectItem = ServiceLocator.Instance.ShellHelper.FindProjectItemByFileName(project.ProjectItems, fileName);
        //        if (projectItem != null)
        //            return projectItem;
        //    }
        //    return null;
        //}

        /// <summary>
        /// Transforme en chemin relatif à partir de la racine de la solution
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <returns></returns>
        private string MakeRelative(string fileName)
        {
            if (Utils.StringStartsWith(fileName, _solutionFolder))
                return fileName.Substring(_solutionFolder.Length + 1);
            return fileName;
        }

        /// <summary>
        /// Résoud en chemin physique
        /// </summary>
        /// <param name="fileName">Chemin relatif</param>
        /// <returns></returns>
        private string ResolvePath(string fileName)
        {
//            return Path.Combine(ServiceLocator.Instance.ShellHelper.SolutionFolder, fileName);
            return Path.Combine(_solutionFolder, fileName);
        }

        /// <summary>
        /// Recherche d'une association par rapport à un nom de fichier
        /// </summary>
        /// <param name="fileName">Chemin absolu du fichier</param>
        /// <returns></returns>
        internal MapItem FindMapItem(string fileName)
        {
            fileName = MakeRelative(fileName);
            return Items != null ? Items.Find(delegate(MapItem item) { return item.FileName == fileName; }) : null;
        }

        /// <summary>
        /// Affecte une valeur à la propriété 'CanRegenerate' de l'association
        /// </summary>
        /// <param name="fileName">Chemin absolu du fichier</param>
        /// <param name="value">Valeur</param>
        public void SetCanGeneratePropertyValue(string fileName, bool value)
        {
            MapItem item = FindMapItem(fileName);
            if (item != null && item.CanGenerate != value)
            {
                item.CanGenerate = value;
                Save();
            }
        }

        /// <summary>
        /// Récupère la valeur de la propriété 'CanRegenerate' de l'association
        /// </summary>
        /// <param name="fileName">Chemin absolu du fichier</param>
        /// <returns>Valeur</returns>
        public bool GetCanGeneratePropertyValue(string fileName)
        {
            MapItem item = FindMapItem(fileName);
            if (item != null)
            {
                return item.CanGenerate;
            }
            return true;
        }

        /// <summary>
        /// Called when [track project documents].
        /// </summary>
        /// <param name="numProjects">The num projects.</param>
        /// <param name="numItems">The num items.</param>
        /// <param name="projects">The projects.</param>
        /// <param name="firstIndices">The first indices.</param>
        /// <param name="documents">The documents.</param>
        /// <param name="callback">The callback.</param>
        private static void OnTrackProjectDocuments(int numProjects, int numItems, IVsProject[] projects, int[] firstIndices,
                                             string[] documents, TrackDocumentEventHandler callback)
        {
            for (int i = 0; i < numProjects; i++)
            {
                //IVsProject project1 = projects[i];
                //if (this.ProjectMatchesHandler(project1))
                {
                    int num3 = i < (numProjects - 1) ? firstIndices[i + 1] : numItems;
                    for (int j = firstIndices[i]; j < num3; j++)
                    {
                        callback(documents[j]);
                    }
                }
            }
        }

        /// <summary>
        /// Begins the generation transaction.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="strategyId">The strategy id.</param>
        /// <param name="elementId">The element id.</param>
        public void BeginGenerationTransaction(GenerationContext context, string strategyId, Guid elementId)
        {
            _transactionItemList = new List<MapItem>();
            if (context.Project == null || context.GenerationPass != GenerationPass.CodeGeneration)
                return;

            // Stockage des fichiers concernés en vue de la suppression des fichiers obsolètes
            // Suppression des fichiers d'une stratégie
            Queue<MapItem> tmp =
                new Queue<MapItem>(
                    Items.FindAll(
                        delegate(MapItem currentItem)
                            {
                                return
                                    currentItem.StrategyId == strategyId && currentItem.ElementId == elementId &&
                                    currentItem.CanGenerate;
                            }));

            // Suppression des items dans le fichier
            while (tmp.Count > 0)
            {
                MapItem item = tmp.Dequeue();
                _transactionItemList.Add(item);
                Items.Remove(item);
            }
        }

        /// <summary>
        /// On remet les items qui avaient été supprimés dans le bein de la transaction
        /// </summary>
        /// <param name="context">The context.</param>
        public void RollbackGenerationTransaction(GenerationContext context)
        {
            if (context.Project != null && context.GenerationPass == GenerationPass.CodeGeneration)
            {
                // On remet les mapItems précédemment supprimés
                foreach (MapItem item in _transactionItemList)
                {
                    if (!Items.Contains(item))
                        Items.Add(item);
                }
            }
        }

        /// <summary>
        /// Suppression des fichiers de génération obsolétes à partir de la liste initiale
        /// </summary>
        /// <param name="context">The context.</param>
        public void CommitGenerationTransaction(GenerationContext context)
        {
            if (context.Project == null || context.GenerationPass != GenerationPass.CodeGeneration)
                return;
            DeleteFiles(context.Project, _transactionItemList, false);
        }

        /// <summary>
        /// Suppression physique de fichiers dans un projet Visual Studio.
        /// Cette méthode est appelée soit :
        /// - Par le rollback d'une transaction pour supprimer
        /// les fichiers qui n'ont pas été regénérés (au cas ou la stratégie à changer et
        /// génére moins de fichier) (force=false)
        /// - Lors de la suppression d'une stratégie quand on veut supprimer les fichiers
        /// que cette stratégie avait généré. (force=true)
        /// </summary>
        /// <param name="project">The project.</param>
        /// <param name="items">Liste des items à supprimer</param>
        /// <param name="force">if set to <c>true</c> [force].</param>
        internal void DeleteFiles(Project project, List<MapItem> items, bool force)
        {
            ILogger logger = ServiceLocator.Instance.GetService<ILogger>();

            // Liste des répertoires qui ont eu des fichiers supprimés pour pouvoir
            //  les supprimer ensuite
            List<string> folders = new List<string>();

            if (items.Count > 0)
            {
                foreach (MapItem item in items)
                {
                    // Suppression des fichiers
                    try
                    {
                        // Si ce fichier n'existe pas dans le fichier de mapping, c'est
                        // qu'il n'a pas été regénéré donc on supprime physiquement le 
                        // fichier le concernant.
                        if (force || FindMapItem(item.FileName) == null)
                        {
                            FileInfo file = new FileInfo(ResolvePath(item.FileName));
                            // Oui on supprime dans l'arborescence du projet et sur le disque
                            Project prj = project;
                            if (prj == null)
                            {
                                prj = ServiceLocator.Instance.ShellHelper.FindProjectByName(item.ProjectName);
                            }
                            ProjectItem pi = null;
                            if (project.ProjectItems != null)
                                pi =
                                    ServiceLocator.Instance.ShellHelper.FindProjectItemByFileName(prj.ProjectItems,
                                                                                                  file.FullName);

                            if (pi != null)
                            {
                                // Suppression dans le projet
                                pi.Delete();
                            }

                            // Suppression physique
                            Utils.DeleteFile(file.FullName);

                            // Sauvegarde du répertoire
                            string folderName = Path.GetDirectoryName(file.FullName);
                            if (!folders.Contains(folderName))
                                folders.Add(folderName);
                        }
                    }
                    catch (Exception ex)
                    {
                        if (logger != null)
                            logger.WriteError("Mapper", "Purge generated file before generation", ex);
                    }
                }

                // Sauvegarde du fichier de mapping
                //Save();

                // Suppression des répertoires
                // Tri dans l'ordre inverse des répertoires pour pouvoir supprimer
                // en commencant par les dossiers enfants et en redescendant vers le père
                folders.Sort(delegate(string a, string b) { return b.CompareTo(a); });

                foreach (string folder in folders)
                {
                    DirectoryInfo di = new DirectoryInfo(folder);
                    if (!di.Exists || di.GetFiles().Length > 0 || di.GetDirectories().Length > 0)
                        continue;

                    if (project.ProjectItems != null)
                    {
                        ProjectItem pi =
                            ServiceLocator.Instance.ShellHelper.FindProjectItemByFileName(project.ProjectItems, folder);
                        if (pi != null)
                            pi.Delete();
                    }
                    // Suppression physique
                    Utils.RemoveDirectory(folder);
                }
            }
        }

        #region Nested type: MapItem

        /// <summary>
        /// 
        /// </summary>
        [Serializable]
        public class MapItem
        {
            private bool canGenerate = true;
            private Guid elementId;
            private string fileName;
            private string projectName;
            private string strategyId;
            private string strategyOwner;

            /// <summary>
            /// Gets or sets the strategy owner.
            /// </summary>
            /// <value>The strategy owner.</value>
            [XmlElement("strategyOwner")]
            public string StrategyOwner
            {
                get { return strategyOwner; }
                set { strategyOwner = value; }
            }

            /// <summary>
            /// Gets or sets the name of the project.
            /// </summary>
            /// <value>The name of the project.</value>
            [XmlElement("project")]
            public string ProjectName
            {
                get { return projectName; }
                set { projectName = value; }
            }

            /// <summary>
            /// Gets or sets a value indicating whether this instance can generate.
            /// </summary>
            /// <value>
            /// 	<c>true</c> if this instance can generate; otherwise, <c>false</c>.
            /// </value>
            [XmlAttribute("canGenerate")]
            public bool CanGenerate
            {
                get { return canGenerate; }
                set { canGenerate = value; }
            }

            /// <summary>
            /// Gets or sets the strategy id.
            /// </summary>
            /// <value>The strategy id.</value>
            [XmlAttribute("strategyId")]
            public string StrategyId
            {
                get { return strategyId; }
                set { strategyId = value; }
            }

            /// <summary>
            /// Gets or sets the element id.
            /// </summary>
            /// <value>The element id.</value>
            [XmlAttribute("elementId")]
            public Guid ElementId
            {
                get { return elementId; }
                set { elementId = value; }
            }

            /// <summary>
            /// Gets or sets the name of the file.
            /// </summary>
            /// <value>The name of the file.</value>
            public string FileName
            {
                get { return fileName; }
                set { fileName = value; }
            }
        }

        #endregion

        #region Nested type: MapperConfiguration

        /// <summary>
        /// 
        /// </summary>
        [XmlRoot("modelMap")]
        public class MapperConfiguration
        {
            private List<MapItem> _items = new List<MapItem>();

            /// <summary>
            /// Gets or sets the items.
            /// </summary>
            /// <value>The items.</value>
            [XmlArray("maps")]
            [XmlArrayItem("map")]
            public List<MapItem> Items
            {
                get { return _items; }
                set { _items = value; }
            }
        }

        #endregion

        #region Nested type: TrackDocumentEventHandler

        private delegate void TrackDocumentEventHandler(string path);

        #endregion
    }
}