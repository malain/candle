using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using DSLFactory.Candle.SystemModel.Dependencies;
using DSLFactory.Candle.SystemModel.Repository;
using DSLFactory.Candle.SystemModel.Strategies;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Diagrams;
using Microsoft.VisualStudio.Modeling.Shell;
using Microsoft.VisualStudio.Modeling.Validation;

namespace DSLFactory.Candle.SystemModel
{
    /// <summary>
    /// 
    /// </summary>
    partial class CandleModel : IHasReferences
    {
        private List<Guid> _pendingsElementsDeleted = new List<Guid>();
        private Dictionary<Guid, string> _pendingsExternalAssemblyDeleted = new Dictionary<Guid, string>();

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="store">Store where new element is to be created.</param>
        /// <param name="propertyAssignments">List of domain property id/value pairs to set once the element is created.</param>
        public CandleModel(Store store, params PropertyAssignment[] propertyAssignments)
            : this(store != null ? store.DefaultPartition : null, propertyAssignments)
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="partition">Partition where new element is to be created.</param>
        /// <param name="propertyAssignments">List of domain property id/value pairs to set once the element is created.</param>
        public CandleModel(Partition partition, params PropertyAssignment[] propertyAssignments)
            : base(partition, propertyAssignments)
        {
        }

        ///// <summary>
        ///// Accés à toutes les stratégies applicables à ce modèle
        ///// </summary>
        //public List<StrategyBase> Strategies
        //{
        //    get { return StrategyManager.GetInstance(clazz.Store).GetStrategies(StrategiesOwner); }
        //}

        /// <summary>
        /// Gets the data layer.
        /// </summary>
        /// <value>The data layer.</value>
        public DataLayer DataLayer
        {
            get
            {
                return
                    SoftwareComponent != null && SoftwareComponent.IsDataLayerExists
                        ? SoftwareComponent.DataLayer
                        : null;
            }
        }

        /// <summary>
        /// Gets the meta data.
        /// </summary>
        /// <value>The meta data.</value>
        public ComponentModelMetadata MetaData
        {
            get
            {
                // Recup dans le repository pour avoir les données serveurs
                ComponentModelMetadata metadata = RepositoryManager.Instance.ModelsMetadata.Metadatas.Find(Id, Version);
                if (metadata == null) // Le modèle n'est pas encore dans le repository
                    metadata = new ComponentModelMetadata();


                // On reforce tjs avec les propriétés courantes
                metadata.Id = Id;
                metadata.Name = Name;
                metadata.Version = Version;
                if (String.IsNullOrEmpty(metadata.ModelFileName))
                {
                    if (FileName != null)
                        metadata.ModelFileName = System.IO.Path.GetFileName(FileName);
                    else
                    {
                        string name = Name;
                        try
                        {
                            name =
                                System.IO.Path.GetFileNameWithoutExtension(
                                    ServiceLocator.Instance.ShellHelper.Solution.FileName);
                        }
                        catch
                        {
                        }
                        metadata.ModelFileName = name + ModelConstants.FileNameExtension;
                    }
                }
                metadata.DocUrl = Url;
                metadata.Description = Comment;
                metadata.Path = Path;
                return metadata;
            }
        }

        /// <summary>
        /// Gets the software component.
        /// </summary>
        /// <value>The software component.</value>
        public SoftwareComponent SoftwareComponent
        {
            get { return Component as SoftwareComponent; }
        }

        /// <summary>
        /// Gets the binary component.
        /// </summary>
        /// <value>The binary component.</value>
        public BinaryComponent BinaryComponent
        {
            get { return Component as BinaryComponent; }
        }

        /// <summary>
        /// Gets the strategies owner.
        /// </summary>
        /// <value>The strategies owner.</value>
        public override CandleElement StrategiesOwner
        {
            get { return null; }
        }

        /// <summary>
        /// Gets the owner.
        /// </summary>
        /// <value>The owner.</value>
        public override ICustomizableElement Owner
        {
            get { return null; }
        }

        /// <summary>
        /// Gets the name of the file.
        /// </summary>
        /// <value>The name of the file.</value>
        public string FileName
        {
            get { return GetFileNameForStore(Store); }
        }

        #region Pendings delete

        /// <summary>
        /// Enregistrement de la dll à supprimer
        /// </summary>
        /// <param name="model">The model.</param>
        internal void RegisterExternalAssemblyPendingDelete(DotNetAssembly model)
        {
            // Non on laisse tout dans le repository local (a voir)
            //pendingsExternalAssemblyDeleted[model.Id] = model.Name;
        }

        /// <summary>
        /// Registers the element pending delete.
        /// </summary>
        /// <param name="elem">The elem.</param>
        internal void RegisterElementPendingDelete(CandleElement elem)
        {
            if (!_pendingsElementsDeleted.Contains(elem.Id))
                _pendingsElementsDeleted.Add(elem.Id);
        }

        /// <summary>
        /// Annulation de la suppression (si l'utilisateur fait un undo)
        /// </summary>
        /// <param name="externalAssemblyModel"></param>
        internal void UnregisterExternalAssemblyPendingDelete(DotNetAssembly externalAssemblyModel)
        {
            //pendingsExternalAssemblyDeleted.Remove(externalAssemblyModel.Id);
        }

        /// <summary>
        /// Gestion de la suppression des assemblies externes. La suppression physique de la dll dans le repository
        /// ne se fait pas directement au moment de la suppression dans le modèle mais lors de la
        /// sauvegarde du modèle
        /// </summary>
        /// <param name="context">The context.</param>
        [ValidationMethod(ValidationCategories.Save)]
        private void CommitPendingsDelete(ValidationContext context)
        {
            // Les assemblys
            foreach (string name in _pendingsExternalAssemblyDeleted.Values)
            {
                CommitExternalAssemblyDelete(name, context);
            }
            _pendingsExternalAssemblyDeleted.Clear();

            // Puis les fichiers de génération des éléments
            foreach (Guid id in _pendingsElementsDeleted)
            {
                // On s'assure qu'il n'a pas été réactivé
                ModelElement elem = Store.ElementDirectory.FindElement(id);
                if (elem == null || elem.IsDeleted)
                {
                    Mapper.Instance.RemoveStrategyGeneratedFiles(id);
                }
            }
            _pendingsElementsDeleted.Clear();
        }

        /// <summary>
        /// Suppression physique de la dll dans le repository local
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="context">The context.</param>
        protected void CommitExternalAssemblyDelete(string name, ValidationContext context)
        {
            ILogger logger = ServiceLocator.Instance.GetService<ILogger>();
            // Suppression de la ddl dans le repository local
            string destFile = MetaData.ResolvePath(name + ".dll", PathKind.Absolute);

            if (File.Exists(destFile))
            {
                try
                {
                    File.Delete(destFile);
                }
                catch (Exception ex)
                {
                    if (logger != null)
                        logger.WriteError("Deleting unused model",
                                          String.Format("Unable to delete assembly {0} in the local repository",
                                                        destFile), ex);
                }
            }
        }

        #endregion

        #region IHasReferences Members

        /// <summary>
        /// Gets the references.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        public IEnumerable<ReferenceItem> GetReferences(ReferenceContext context)
        {
            if (Store != null && Component != null)
                yield return
                    new ReferenceItem(this, Component, ReferenceScope.Compilation, context.Ports, context.IsExternal);
        }

        #endregion

        /// <summary>
        /// Gets the component type value.
        /// </summary>
        /// <returns></returns>
        internal ComponentType GetComponentTypeValue()
        {
            return Component != null && Component is BinaryComponent ? ComponentType.Library : ComponentType.Component;
        }

        /// <summary>
        /// Recherche une système externe
        /// </summary>
        /// <param name="modelId">The model id.</param>
        /// <returns></returns>
        public ExternalComponent FindExternalComponent(Guid modelId)
        {
            foreach (ExternalComponent externalComponent in ExternalComponents)
            {
                if (externalComponent.ModelMoniker == modelId)
                    return externalComponent;
            }
            return null;
        }

        /// <summary>
        /// Finds the name of the external component by.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        internal ExternalComponent FindExternalComponentByName(string name)
        {
            foreach (ExternalComponent externalComponent in ExternalComponents)
            {
                if (externalComponent.Name == name)
                    return externalComponent;
            }
            return null;
        }

        /// <summary>
        /// Get the filename in which a Store is persisted
        /// </summary>
        /// <param name="store">The store.</param>
        /// <returns></returns>
        public static string GetFileNameForStore(Store store)
        {
            foreach (Diagram diagram in store.ElementDirectory.FindElements<Diagram>(true))
            {
                VSDiagramView view = diagram.ActiveDiagramView as VSDiagramView;
                if (view != null)
                {
                    // Get the corresponding file
                    return view.DocData.FileName;
                }
            }

            return null;
        }

        /// <summary>
        /// Recherche le modèle associé à la solution courante
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// Le modèle associé est contenu dans un fichier portant le même nom que la solution
        /// </remarks>
        public static CandleModel GetModelFromCurrentSolution()
        {
            try
            {
                IShellHelper shell = ServiceLocator.Instance.GetService<IShellHelper>();
                if (shell == null)
                    return null;

                // On cherche d'abord si le nom est enregistré dans la solution
                string candleModelPath = shell.GetSolutionAssociatedModelName();
                if (candleModelPath == null)
                {
                    // Si pas trouvé, on le déduit à partir du nom de la solution
                    string solutionName = (string) shell.Solution.Properties.Item("Name").Value;
                    candleModelPath =
                        System.IO.Path.Combine(shell.SolutionFolder,
                                               String.Concat(solutionName, ModelConstants.FileNameExtension));
                }
                // Recherche dans la RDT
                CandleModel model = GetModelFromCurrentSolution(candleModelPath);
                // Si tjs null, on essaye de le charger directement
                if (model == null)
                {
                    ModelLoader loader = ModelLoader.GetLoader(candleModelPath, true);
                    if (loader != null)
                        model = loader.Model;
                }
                return model;
            }
            catch
            {
            }

            return null;
        }

        /// <summary>
        /// Chargement du modèle à partir de la RDT
        /// </summary>
        /// <param name="modelFilePath">The model file path.</param>
        /// <returns></returns>
        public static CandleModel GetModelFromCurrentSolution(string modelFilePath)
        {
            try
            {
                List<ModelingDocData> list =
                    ServiceLocator.Instance.ShellHelper.GetModelingDocDatasFromRunningDocumentTable();
                foreach (ModelingDocData doc in list)
                {
                    if (Utils.StringCompareEquals(doc.FileName, modelFilePath))
                    {
                        return GetInstance(doc.Store);
                    }
                }
            }
            catch
            {
            }
            return null;
        }

        #region Singleton

        /// <summary>
        /// Permet de récupèrer l'instance de la définition du système
        /// </summary>
        /// <param name="store"></param>
        /// <returns></returns>
        public static CandleModel GetInstance(Store store)
        {
            ReadOnlyCollection<CandleModel> lists = store.ElementDirectory.FindElements<CandleModel>();
            return lists.Count > 0 ? lists[0] : null;
        }

        #endregion
    }
}