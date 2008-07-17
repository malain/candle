using System;
using System.Collections.Generic;
using System.IO;
using DSLFactory.Candle.SystemModel.Strategies;

namespace DSLFactory.Candle.SystemModel.Dependencies
{
    /// <summary>
    /// Calcul des dépendances à partir d'un modèle
    /// </summary>
    public class ReferenceVisitor : IReferenceVisitor
    {
        /// <summary>
        /// Liste des modèles précédemment traités
        /// </summary>
        private readonly List<Guid> _checkedModels = new List<Guid>();

        /// <summary>
        /// Pile de contexte contenant le modèle en train d'être traité. Le 1er est toujours le modèle initial ensuite on empile les références externes.
        /// </summary>
        private readonly Stack<CandleModel> _modelsStack = new Stack<CandleModel>();

        /// <summary>
        /// 
        /// </summary>
        private readonly Stack<string> _projectPath = new Stack<string>();

        /// <summary>
        /// Folder de la solution initiale. Si null, on prendra le repository
        /// </summary>
        private string _solutionFolder;

        /// <summary>
        /// Scope pris en compte pour résoudre les références
        /// </summary>
        private readonly ReferenceScope _scope;

        /// <summary>
        /// Liste des références trouvées
        /// </summary>
        private readonly ReferencesCollection _references = new ReferencesCollection();

        /// <summary>
        /// Liste des modèles rencontrés
        /// </summary>
        private readonly List<CandleModel> _models = new List<CandleModel>();

        /// <summary>
        /// Indique si le modèle initial est contenu dans une solution ou dans le repository
        /// </summary>
        private readonly bool _initialModelIsInSolution;

        private Guid _initialModelId = Guid.Empty;

        /// <summary>
        /// Le calcul se fera à partir du repository
        /// </summary>
        /// <param name="scope"></param>
        public ReferenceVisitor(ReferenceScope scope)
            : this(scope, null)
        {
        }

        /// <summary>
        /// Calcul se fera à partir d'un modèle contenu dans une solution
        /// </summary>
        /// <param name="scope"></param>
        /// <param name="solutionFolder"></param>
        public ReferenceVisitor(ReferenceScope scope, string solutionFolder)
        {
            this._scope = scope;
            this._solutionFolder = solutionFolder;
            _initialModelIsInSolution = !String.IsNullOrEmpty(solutionFolder);
        }

        /// <summary>
        /// Folder du modèle initial
        /// </summary>
        private string SolutionFolder
        {
            get
            {
                // Si on est pas dans une solution, on pioche directement dans le repository local
                if (!_initialModelIsInSolution)
                {
                    _solutionFolder = CurrentModelPath;
                }
                if (_solutionFolder == null)
                    throw new Exception("Root model  not found in repository");

                return _solutionFolder;
            }
        }

        /// <summary>
        /// Répertoire de base du modele courant
        /// </summary>
        private string CurrentModelPath
        {
            get
            {
                if (IsExternalModel || !_initialModelIsInSolution)
                {
                    CandleModel model = _modelsStack.Peek();
                    return
                        Path.GetDirectoryName(
                            model.MetaData.GetFileName(DSLFactory.Candle.SystemModel.Repository.PathKind.Absolute));
                }
                else
                    return _solutionFolder;
            }
        }

        /// <summary>
        /// Indique si le modèle courant est une référence externe
        /// </summary>
        private bool IsExternalModel
        {
            get { return _modelsStack.Count > 1; }
        }

        /// <summary>
        /// Gets the models.
        /// </summary>
        /// <value>The models.</value>
        public List<CandleModel> Models
        {
            get { return _models; }
        }

        /// <summary>
        /// Liste des références trouvées
        /// </summary>
        public ReferencesCollection References
        {
            get { return _references; }
        }

        /// <summary>
        /// Appelée pour chaque modèle
        /// </summary>
        /// <param name="model"></param>
        /// <param name="isInitialModel"></param>
        protected virtual void AcceptModel(CandleModel model, bool isInitialModel)
        {
            if (!_models.Contains(model))
                _models.Add(model);
        }

        #region IReferenceVisitor Members

        /// <summary>
        /// Fin de traitement d'un elément
        /// </summary>
        /// <param name="item">The item.</param>
        public void ExitElement(ReferenceItem item)
        {
            if (item.Element is CandleModel)
                _modelsStack.Pop();
            else if (item.Element is DataLayer)
            {
                DataLayer modelsLayer = item.Element as DataLayer;
                CandleModel model = modelsLayer.Component.Model;
                if (model.Id != _initialModelId)
                    _modelsStack.Pop();
            }
            else if (item.Element is SoftwareLayer || item.Element is DotNetAssembly)
                _projectPath.Pop();
        }

        /// <summary>
        /// Accepts the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns></returns>
        public bool Accept(ReferenceItem item)
        {
            if (item.Element is DataLayer)
            {
                DataLayer modelsLayer = item.Element as DataLayer;
                // Verification si ce système n'a pas dèjà été vu (pour éviter les boucles)
                if (_checkedModels.Contains(item.Element.Id))
                    return false;
                _checkedModels.Add(item.Element.Id);

                // On accepte aussi le modèle de la couche (si il n'a pas dèjà été vu par ailleurs)
                if (!_models.Contains(modelsLayer.Component.Model))
                    AcceptModel(modelsLayer.Component.Model, _initialModelId == Guid.Empty);

                CandleModel model = modelsLayer.Component.Model;
                if (model.Id != _initialModelId)
                    _modelsStack.Push(model);
            }

            if (item.Element is CandleModel)
            {
                // Verification si ce système n'a pas dèjà été vu (pour éviter les boucles)
                if (_checkedModels.Contains(item.Element.Id))
                    return false;

                if (!_models.Contains((CandleModel) item.Element))
                    AcceptModel((CandleModel) item.Element, _initialModelId == Guid.Empty);

                if (_initialModelId == Guid.Empty)
                    _initialModelId = item.Element.Id;

                _checkedModels.Add(item.Element.Id);
                _modelsStack.Push(item.Element as CandleModel);
                return true;
            }

            // Toujours devant
            if (item.Element is SoftwareLayer)
            {
                // Calcul du répertoire du projet courant
                string currentProjectPath = CurrentModelPath;
                if (_initialModelIsInSolution && !IsExternalModel)
                    currentProjectPath = Path.Combine(currentProjectPath, ((SoftwareLayer) item.Element).VSProjectName);
                _projectPath.Push(currentProjectPath);
            }

            if (item.Element is InterfaceLayer)
            {
                InterfaceLayer ilayer = item.Element as InterfaceLayer;
                if (_scope == ReferenceScope.Compilation && IsExternalModel == false)
                {
                    _references.Add(ilayer.VSProjectName);
                }
                else
                {
                    AddFullNameAssembly(ilayer, ".dll");
                }
            }
            else if (item.Element is SoftwareLayer)
            {
                AcceptSoftwareLayer(item.Element as SoftwareLayer);
            }
            else if (item.Element is DotNetAssembly)
            {
                AcceptDotNetAssembly(item.Element as DotNetAssembly);
            }
            else if (item.Element is Artifact)
            {
                AcceptArtifact(item.Element as Artifact);
            }
            return true;
        }

        /// <summary>
        /// Prise en compte d'une référence sur un SoftwareLayer
        /// </summary>
        /// <param name="layer">The layer.</param>
        private void AcceptSoftwareLayer(SoftwareLayer layer)
        {
            if (_scope == ReferenceScope.Compilation)
            {
                if (layer is Layer && ((Layer) layer).InterfaceLayer != null)
                    return;

                if (!IsExternalModel && _initialModelIsInSolution)
                {
                    // Pour une compilation, si il n'y a pas d'interface et qu'on est dans le modèle initial, on
                    // se contente de prendre le nom du projet
                    _references.Add(layer.VSProjectName);
                    return;
                }
            }

            string extension = ".dll";
            if (_initialModelIsInSolution)
                extension = StrategyManager.GetInstance(layer.Store).GetAssemblyExtension(layer);

            AddFullNameAssembly(layer, extension);
        }

        /// <summary>
        /// Accepts the data layer.
        /// </summary>
        /// <param name="layer">The layer.</param>
        /// <param name="isExternal">if set to <c>true</c> [is external].</param>
        private void AcceptDataLayer(DataLayer layer, bool isExternal)
        {
            if (_scope == ReferenceScope.Compilation)
            {
                if (!isExternal && _initialModelIsInSolution)
                {
                    _references.Add(layer.VSProjectName);
                    return;
                }
            }

            string extension = ".dll";
            if (_initialModelIsInSolution)
                extension = StrategyManager.GetInstance(layer.Store).GetAssemblyExtension(layer);

            AddFullNameAssembly(layer, extension);
        }

        /// <summary>
        /// Adds the full name assembly.
        /// </summary>
        /// <param name="layer">The layer.</param>
        /// <param name="extension">The extension.</param>
        private void AddFullNameAssembly(SoftwareLayer layer, string extension)
        {
            string assemblyName = String.Concat(layer.AssemblyName, extension);

            // Si on prend les références à partir d'un modèle d'une solution et qu'on est
            // dans ce modèle (count==1) alors on résoud les chemins à partir du répertoire binaire
            if (!IsExternalModel && _initialModelIsInSolution)
            {
                EnvDTE.Project project = ServiceLocator.Instance.ShellHelper.FindProjectByName(layer.VSProjectName);
                if (project != null)
                {
                    string binaryProjectPath = Path.Combine(_projectPath.Peek(), "bin");
                    if (!(project.Object is VsWebSite.VSWebSite))
                    {
                        string outputPath =
                            (string)
                            project.ConfigurationManager.ActiveConfiguration.Properties.Item("OutputPath").Value;
                        binaryProjectPath = Path.Combine(_projectPath.Peek(), outputPath);
                    }
                    _references.Add(Path.Combine(binaryProjectPath, assemblyName));
                }
            }
                // Sinon on prend direct dans le repository
            else
            {
                _references.Add(Path.Combine(CurrentModelPath, assemblyName));
            }
        }

        /// <summary>
        /// Accepts the dot net assembly.
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        private void AcceptDotNetAssembly(DotNetAssembly assembly)
        {
            _projectPath.Push(CurrentModelPath);

            // Si l'assembly est dans le GAC
            if (assembly.IsInGac)
            {
                if (_scope == ReferenceScope.Compilation)
                    _references.Add(assembly.FullName);

                if (_scope != ReferenceScope.Publish) // == Runtime
                    return;
            }

            string assemblyName = assembly.AssemblyName;
            string fileName = Path.Combine(_projectPath.Peek(), assemblyName);

            if (_initialModelIsInSolution && !IsExternalModel)
            {
                if (!File.Exists(fileName))
                {
                    // 1ère fois - Copie dans le répertoire local
                    Utils.CopyFile(assembly.InitialLocation, fileName);
                }

                if (!File.Exists(fileName)) // Toujours pas (même aprés copie éventuelle)
                {
                    fileName = assembly.AssemblyName;
                }
            }
            _references.Add(fileName);
        }

        /// <summary>
        /// Accepts the artifact.
        /// </summary>
        /// <param name="artifact">The artifact.</param>
        private void AcceptArtifact(Artifact artifact)
        {
            if (artifact.Type == ArtifactType.Content && _scope == ReferenceScope.Compilation)
                return;

            if ((artifact.Scope & _scope) != _scope)
                return;

#if DEBUG
            if (_scope == ReferenceScope.Publish)
            {
                if (artifact.Type == ArtifactType.AssemblyInGac && !_initialModelIsInSolution)
                    throw new Exception("cas incorrect");

                if (IsExternalModel && _scope != ReferenceScope.Runtime)
                    throw new Exception("cas incorrect");
            }

#endif
            // Assemblie dans le GAC, on le prend tel quel            
            if (artifact.Type == ArtifactType.AssemblyInGac)
            {
                if (_scope == ReferenceScope.Compilation)
                    _references.Add(artifact.FileName);
                return;
            }

            // Une assemblie du framework sélectionné, on résoud son chemin
            if (artifact.Type == ArtifactType.DotNetFramework)
            {
                if (_scope == ReferenceScope.Compilation)
                {
                    CandleModel model = global::DSLFactory.Candle.SystemModel.CandleModel.GetInstance(artifact.Store);
                    _references.Add(
                        DSLFactory.Candle.SystemModel.Utilities.DotNetFrameworkHelper.ResolvePath(model,
                                                                                                  artifact.FileName));
                }
                return;
            }

            //if (artifact.Type == ArtifactType.Assembly)
            //{
            //    //if( parent != null )
            //    references.Add(Path.Combine(LocalPath, artifact.FileName));
            //    //else
            //    //    References.Add( Path.Combine( LatestPath, artifact.FileName ) );
            //    return;
            //}

            // Référence de type projet
            if (artifact.Type == ArtifactType.Project)
            {
                // Si on est dans le projet courant, on recherche le nom de l'assembly
                if (!IsExternalModel && _initialModelIsInSolution)
                {
                    if (_scope == ReferenceScope.Compilation)
                    {
                        _references.Add(artifact.FileName);
                    }
                    else
                    {
                        // Nom de l'assembly du projet
                        IShellHelper shell = ServiceLocator.Instance.ShellHelper;
                        if (shell != null)
                        {
                            EnvDTE.Project project = shell.FindProjectByName(artifact.FileName);
                            if (project != null)
                            {
                                string outputPath =
                                    (string)
                                    project.ConfigurationManager.ActiveConfiguration.Properties.Item("OutputPath").Value;
                                string assemblyName = (string) project.Properties.Item("OutputFileName").Value;
                                _references.Add(
                                    Path.Combine(Path.GetDirectoryName(project.FullName),
                                                 String.Concat(outputPath, assemblyName)));
                            }
                        }
                    }
                }
                else // On est dans un modèle qui ne correspond pas au projet courant
                {
                    if (_scope == ReferenceScope.Runtime)
                    {
                        string fileName = Path.Combine(CurrentModelPath, artifact.InitialFileName);
                        _references.Add(fileName);
                    }
                }

                return;
            }

            // Si on est dans le modèle courant
            if (_initialModelIsInSolution && !IsExternalModel)
            {
                string fileName = Path.Combine(_projectPath.Peek(), artifact.FileName);
                if (!File.Exists(fileName))
                {
                    Utils.CopyFile(artifact.InitialFileName, fileName);
                }

                if (!File.Exists(fileName))
                {
                    fileName = Path.Combine(SolutionFolder, artifact.FileName);
                }
                _references.Add(fileName);
            }
            else
            {
                _references.Add(Path.Combine(_projectPath.Peek(), artifact.FileName));
            }
        }

        #endregion
    }
}