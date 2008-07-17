using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing.Design;
using System.IO;
using System.Xml.Serialization;
using DSLFactory.Candle.SystemModel.CodeGeneration;
using DSLFactory.Candle.SystemModel.Editor;
using EnvDTE;
using Microsoft.VisualStudio.Modeling;

namespace DSLFactory.Candle.SystemModel.Strategies
{
    /// <summary>
    /// Base class for strategy
    /// </summary>
    [CLSCompliant(true)]
    [Serializable()]
    public abstract class StrategyBase : IPackagedStrategy, IDisposable
    {
        /// <summary>
        /// Contexte d'exécution doit être initialisé à chaque exécution
        /// </summary>
        private GenerationContext _context;

        /// <summary>Element sur lequel s'applique la stratégie</summary>
        private ICustomizableElement _currentElement;

        /// <summary>Répertoire de génération de la strategie</summary>
        private string _defaultGeneratedFilePathPattern;

        /// <summary>Description</summary>
        private string _description = String.Empty;

        /// <summary>Nom affichable</summary>
        private string _displayName = String.Empty;

        /// <summary>Indique si la stratégie est active</summary>
        private bool _enabled = true;

        /// <summary>URL to display help informations</summary>
        private string _helpURL = String.Empty;

        private string _owner;

        /// <summary>Nom du package contenant la stratégy</summary>
        private string _packageName;

        /// <summary>cache pour stocker le repértoire ou se trouve la strategie </summary>
        private string _strategyFolder;

        /// <summary>Groupe utilisé pour l'affichage</summary>
        private string _strategyGroup = String.Empty;

        /// <summary>Unique ID representation</summary>
        private string _strategyId = String.Empty;

        /// <summary>Path for the strategy tree representation (name1/name2/...)</summary>
        private string _strategyPath = String.Empty;

        #region Properties

        /// <summary>
        /// Gets or sets the strategy folder.
        /// </summary>
        /// <value>The strategy folder.</value>
        [Browsable(false)]
        [XmlIgnore()]
        public string StrategyFolder
        {
            get { return _strategyFolder; }
            set { _strategyFolder = value; }
        }

        /// <summary>
        /// Layer propriétaire
        /// </summary>
        /// <value>The owner.</value>
        [XmlAttribute("modelOwner")]
        [Browsable(false)]
        public string Owner
        {
            [DebuggerStepThrough]
            get { return _owner; }
            [DebuggerStepThrough]
            set { _owner = value; }
        }

        /// <summary>
        /// Répertoire de génération
        /// </summary>
        /// <value>The default generated file path pattern.</value>
        [XmlAttribute("defaultGeneratedFilePathPattern")]
        public virtual string DefaultGeneratedFilePathPattern
        {
            [DebuggerStepThrough]
            get { return _defaultGeneratedFilePathPattern; }
            [DebuggerStepThrough]
            set { _defaultGeneratedFilePathPattern = value; }
        }

        /// <summary>
        /// Nom de la stratégie
        /// </summary>
        /// <value>The display name.</value>
        [XmlAttribute("name")]
        public virtual string DisplayName
        {
            get
            {
                if (String.IsNullOrEmpty(_displayName)) _displayName = GetType().Name;
                return _displayName;
            }
            [DebuggerStepThrough]
            set { _displayName = value; }
        }

        /// <summary>
        /// Gets or sets the current element.
        /// </summary>
        /// <value>The current element.</value>
        [XmlIgnore]
        [Browsable(false)]
        public ICustomizableElement CurrentElement
        {
            [DebuggerStepThrough]
            get { return _currentElement; }
            [DebuggerStepThrough]
            set { _currentElement = value; }
        }

        /// <summary>
        /// Gets or sets the strategy id.
        /// </summary>
        /// <value>The strategy id.</value>
        [EditorAttribute(typeof (GuidTypeEditor), typeof (UITypeEditor))]
        [XmlAttribute("id")]
        [Description("Unique ID. Must not be modified after first code generation with this strategy")]
        public string StrategyId
        {
            get
            {
                if (String.IsNullOrEmpty(_strategyId))
                {
                    // Recherche dans les attributs
                    object[] attributes = GetType().GetCustomAttributes(typeof (StrategyAttribute), false);
                    if (attributes.Length > 0)
                    {
                        StrategyAttribute sa = (StrategyAttribute) attributes[0];
                        _strategyId = sa.StrategyId;
                    }
                    else
                        // Sinon on prend le nom du type en cours
                        _strategyId = GetType().FullName;
                }
                return _strategyId;
            }
            [DebuggerStepThrough]
            set
            {
                // On ne peut pas écraser l'id contenu dans l'attribut car il a pu être utilisé dans le code de la
                // stratégie (ex: dependencyproperty)
                object[] attributes = GetType().GetCustomAttributes(typeof (StrategyAttribute), false);
                if (attributes.Length == 1)
                {
                    StrategyAttribute sa = (StrategyAttribute) attributes[0];
                    if (sa.StrategyId != StrategyAttribute.Empty && sa.StrategyId != null)
                        return;
                }
                _strategyId = value;
            }
        }

        /// <summary>
        /// Gets or sets the strategy group.
        /// </summary>
        /// <value>The strategy group.</value>
        [XmlAttribute("group")]
        [Browsable(false)]
        public string StrategyGroup
        {
            [DebuggerStepThrough]
            get { return _strategyGroup; }
            [DebuggerStepThrough]
            set { _strategyGroup = value; }
        }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>The description.</value>
        [XmlIgnore]
        public string Description
        {
            [DebuggerStepThrough]
            get { return _description; }
            [DebuggerStepThrough]
            set { _description = value; }
        }

        //[Browsable(false)]
        //[XmlAttribute("strategyFile")]
        //public string StrategyFile // A virer
        //{
        //    [global::System.Diagnostics.DebuggerStepThrough]
        //    set { _strategyFile = value; }
        //    [global::System.Diagnostics.DebuggerStepThrough]
        //    get { return _strategyFile; }
        //}

        /// <summary>
        /// Gets or sets a value indicating whether this instance is enabled.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is enabled; otherwise, <c>false</c>.
        /// </value>
        [Browsable(false)]
        [XmlAttribute("enabled")]
        [DefaultValue(true)]
        public bool IsEnabled
        {
            [DebuggerStepThrough]
            set { _enabled = value; }
            [DebuggerStepThrough]
            get { return _enabled; }
        }

        /// <summary>
        /// Gets or sets the strategy path.
        /// </summary>
        /// <value>The strategy path.</value>
        [XmlAttribute("path")]
        [Browsable(false)]
        public string StrategyPath
        {
            [DebuggerStepThrough]
            set { _strategyPath = value; }
            [DebuggerStepThrough]
            get { return _strategyPath; }
        }

        /// <summary>
        /// Gets or sets the help URL.
        /// </summary>
        /// <value>The help URL.</value>
        [Browsable(false)]
        [XmlAttribute("url")]
        public string HelpUrl
        {
            [DebuggerStepThrough]
            get { return _helpURL; }
            [DebuggerStepThrough]
            set { _helpURL = value; }
        }

        /// <summary>
        /// Layer propriétaire
        /// </summary>
        /// <value></value>
        [Browsable(false)]
        [XmlAttribute("package")]
        public string PackageName
        {
            [DebuggerStepThrough]
            get { return _packageName; }
            [DebuggerStepThrough]
            set { _packageName = value; }
        }

        #endregion

        #region IDisposable Members

        /// <summary>
        /// Destructeur
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion

        /// <summary>
        /// Affichage d'un message d'erreur dans la liste des taches
        /// </summary>
        /// <param name="ex">Exception à logguer</param>
        protected void LogError(Exception ex)
        {
#if DEBUG
            LogError(String.Format("{0} StackTrace: {1}", ex.Message, ex.StackTrace));
#else
            LogError(ex.Message);
#endif
        }

        /// <summary>
        /// Affichage d'un message d'erreur dans la liste des taches
        /// </summary>
        /// <param name="message">Texte du message</param>
        protected void LogError(string message)
        {
            ILogger logger = ServiceLocator.Instance.GetService<ILogger>();
            if (logger != null)
                logger.Write(DisplayName, message, LogType.Error);
        }

        /// <summary>
        /// Permet à une stratégie de s'approprier la génération d'un élement et d'empêcher ainsi aux autres strétagies
        /// d'écraser le code générée.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="currentElement">The current element.</param>
        /// <param name="generatedFileName">Name of the generated file.</param>
        /// <returns>
        /// 	<c>true</c> if [is model generation exclusive] [the specified context]; otherwise, <c>false</c>.
        /// </returns>
        public virtual bool IsModelGenerationExclusive(GenerationContext context, ICustomizableElement currentElement,
                                                       string generatedFileName)
        {
            return false;
        }

        /// <summary>
        /// Permet de valider la saisie des paramètres dans la boite de configuration des stratégies
        /// </summary>
        /// <returns>null si ok ou un message d'erreur</returns>
        public virtual string CommitChanges()
        {
            return null;
        }

        /// <summary>
        /// Retourne un service
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T GetService<T>()
        {
            return ServiceLocator.Instance.GetService<T>();
        }

        /// <summary>
        /// Gets the service.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t">The t.</param>
        /// <returns></returns>
        public T GetService<T>(Type t)
        {
            return ServiceLocator.Instance.GetService<T>(t);
        }

        /// <summary>
        /// Appelée lors du chargement de la strategie
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="eventArgs">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        public virtual void OnLoading(object sender, EventArgs eventArgs)
        {
        }

        /// <summary>
        /// Appelée lors du déchargement de la stratégie
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="eventArgs">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        public virtual void OnUnloading(object sender, EventArgs eventArgs)
        {
        }

        /// <summary>
        /// Called when [removing].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        public virtual void OnRemoving(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// Destructeur
        /// </summary>
        /// <param name="disposing">false si cette méthode est appelée par le finaliseur, true sinon</param>
        /// <remarks>
        /// Si disposing est a true, on ne doit supprimer que les ressources non gérées par le finaliseur sinon il
        /// faut tout supprimer.
        /// </remarks>
        protected virtual void Dispose(bool disposing)
        {
        }

        #region IStrategy Member

        /// <summary>
        /// Gets the context.
        /// </summary>
        /// <value>The context.</value>
        protected GenerationContext Context
        {
            [DebuggerStepThrough]
            get
            {
                //Debug.Assert( _context != null, "InitializeContext n'a pas été appelée" );
                return _context;
            }
        }

        /// <summary>
        /// Initializes the context.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="element">The element.</param>
        public void InitializeContext(GenerationContext context, ICustomizableElement element)
        {
            ILogger logger = ServiceLocator.Instance.GetService<ILogger>();
            _context = context;
            _currentElement = element;
            if (context != null)
                _context.CurrentStrategy = this;
            if (logger != null)
            {
                if (element != null)
                    logger.Write("Initializaing strategy context",
                                 String.Concat("Strategy initialized with current element ", element.Name, " (id=",
                                               element.Id, ")"), LogType.Debug);
                else
                    logger.Write("Initializaing strategy context", "Reset strategy context", LogType.Debug);
            }
        }

        /// <summary>
        /// Vérifie qu'une propriété est bien valide
        /// </summary>
        /// <param name="element">The element.</param>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns></returns>
        internal bool CheckPropertyValid(ModelElement element, string propertyName)
        {
            foreach (PropertyDescriptor desc in GetCustomProperties(element))
            {
                if (desc.Name == propertyName)
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Gets the custom properties.
        /// </summary>
        /// <param name="modelElement">The model element.</param>
        /// <returns></returns>
        public virtual PropertyDescriptorCollection GetCustomProperties(ModelElement modelElement)
        {
            PropertyDescriptorCollection properties = new PropertyDescriptorCollection(null);
            return properties;
        }

        /// <summary>
        /// Maps the path.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns></returns>
        public string MapPath(string path)
        {
            if (_strategyFolder == null)
                return path;
            return Path.Combine(_strategyFolder, path);
        }

        #endregion

        #region Call template

        /// <summary>
        /// Calls the t4 template.
        /// </summary>
        /// <param name="prj">The PRJ.</param>
        /// <param name="t4TemplateName">Name of the t4 template.</param>
        /// <param name="selectedElement">The selected element.</param>
        /// <returns></returns>
        protected string CallT4Template(Project prj, string t4TemplateName, CandleElement selectedElement)
        {
            return CallT4Template(prj, t4TemplateName, selectedElement, String.Empty, null);
        }

        /// <summary>
        /// Calls the t4 template.
        /// </summary>
        /// <param name="prj">The PRJ.</param>
        /// <param name="t4TemplateName">Name of the t4 template.</param>
        /// <param name="selectedElement">The selected element.</param>
        /// <param name="outputFileName">Name of the output file.</param>
        /// <returns></returns>
        protected string CallT4Template(Project prj, string t4TemplateName, CandleElement selectedElement,
                                        string outputFileName)
        {
            return CallT4Template(prj, t4TemplateName, selectedElement, outputFileName, null);
        }

        /// <summary>
        /// Execution d'un template
        /// </summary>
        /// <param name="project">The project.</param>
        /// <param name="t4TemplateName">Nom du template (avec ou sans extension)</param>
        /// <param name="selectedElement">Modele concerné</param>
        /// <param name="outputFileName">Nom du fichier de sortie (si empty, il sera calculé si à null il sera ignoré)</param>
        /// <param name="properties">The properties.</param>
        /// <returns></returns>
        protected string CallT4Template(Project project, string t4TemplateName, CandleElement selectedElement,
                                        string outputFileName, TemplateProperties properties)
        {
            if (!Utils.StringCompareEquals(Path.GetExtension(t4TemplateName), ".t4"))
                t4TemplateName += ".t4";

            // Est ce que le template existe dans le directory de la stratégie
            string templateFilePath = MapPath(t4TemplateName);
            if (!File.Exists(templateFilePath))
            {
                // On garde le relatif dans le repository
                templateFilePath = t4TemplateName;
            }

            // Nom du fichier de sortie (si <> null)
            // Calcul du nom par défaut si il n'a pas été forcé ou
            // si ce n'est pas un chemin absolu.
            // NOTA : Si outputFileName est null, c'est qu'on ne veut pas 
            //        générer de fichier de sortie (on laisse comme ça)
            if (outputFileName != null &&
                (outputFileName.Length == 0 || Path.GetPathRoot(outputFileName).IndexOf(':') < 0))
                // Si ce n'est pas un chemin physique
            {
                outputFileName = CreateOutputFileName(project, selectedElement, outputFileName);
            }

            // Vérification dans les propriétés de l'extender si ce fichier 
            // peut-être regénéré
            bool canRegenerate = true;
            if (outputFileName != null && File.Exists(outputFileName))
            {
                if (Mapper.Instance.GetCanGeneratePropertyValue(outputFileName) == false)
                    canRegenerate = false;
            }

            if (outputFileName != null && Context.IsGenerationLocked(selectedElement, this, outputFileName))
                return null;

            // Génération
            Context.Template = templateFilePath;
            string data = null;
            try
            {
                if (canRegenerate)
                {
                    if (outputFileName != null && File.Exists(outputFileName))
                        ServiceLocator.Instance.ShellHelper.SuspendFileChange(true, outputFileName);

                    data = Generator.CallT4Template(selectedElement, templateFilePath, outputFileName, properties);
                }

                if (Context.GenerationPass == GenerationPass.MetaModelUpdate)
                {
                    Generator.ApplyCodeInjectionStrategies(null, selectedElement, Context);
                }

                if (data == null && canRegenerate)
                    return null;

                if (outputFileName == null)
                    return data;

                if (File.Exists(outputFileName) && project != null)
                {
                    // Insertion du fichier dans le projet
                    ProjectItem projectItem =
                        ServiceLocator.Instance.ShellHelper.AddFileToProject(project, outputFileName);

                    // L'association se fait sur l'id du modèle sur lequel s'exécute la strategie
                    Mapper.Instance.Associate(project.Name, outputFileName, StrategyId, CurrentElement.Id);

                    // Rechargement si ouvert dans l'éditeur
                    if (canRegenerate)
                        ServiceLocator.Instance.ShellHelper.ReloadDocument(outputFileName);

                    //
                    // Insertion des stratégies style AOP
                    //
                    Generator.ApplyCodeInjectionStrategies(projectItem, selectedElement, Context);

                    ServiceLocator.Instance.ShellHelper.SaveIfDirty(outputFileName);
                }

                return outputFileName;
            }
            finally
            {
                if (outputFileName != null && File.Exists(outputFileName))
                {
                    if (canRegenerate)
                    {
                        ServiceLocator.Instance.ShellHelper.SuspendFileChange(false, outputFileName);
                        ServiceLocator.Instance.ShellHelper.ReloadDocument(outputFileName);
                    }
                }
            }
        }

        /// <summary>
        /// Création du fichier dans le projet
        /// </summary>
        /// <param name="prj">Projet contenant le fichier</param>
        /// <param name="element">Elément concerné ou null</param>
        /// <param name="fileName">Nom du fichier ou null (default: element.Name)</param>
        /// <returns>Chemin complet du fichier</returns>
        protected string CreateOutputFileName(Project prj, ICustomizableElement element, string fileName)
        {
            if (String.IsNullOrEmpty(fileName))
                fileName = element.Name;

            if (!Path.HasExtension(fileName))
                fileName =
                    String.Format("{0}{1}", fileName,
                                  StrategyManager.GetInstance(_currentElement.Store).TargetLanguage.Extension);

            // On le prend tel quel
            string tmpFileName = fileName;
            if (tmpFileName.Length > 0 && tmpFileName[0] == '~')
            {
                tmpFileName = tmpFileName.Substring(1);
            }
            else
            {
                // Si il n'y a que le nom du fichier, on prend le pattern par défaut
                if (Path.GetFileName(tmpFileName) == tmpFileName)
                {
                    string pattern = DefaultGeneratedFilePathPattern;
                    if (String.IsNullOrEmpty(pattern))
                        pattern =
                            StrategyManager.GetInstance(_currentElement.Store).NamingStrategy.
                                DefaultGeneratedCodeFilePattern;
                    tmpFileName = String.Format(pattern, tmpFileName);
                }
            }

            tmpFileName = ResolvePattern(element, tmpFileName);

            // Suppression du '/' de debut
            if (tmpFileName.Length > 1 &&
                (tmpFileName[0] == Path.AltDirectorySeparatorChar || tmpFileName[0] == Path.DirectorySeparatorChar))
                tmpFileName = tmpFileName.Substring(1);

            string pathName = tmpFileName.Replace('/', '\\');
            if (pathName.Length > 0 && pathName[0] == '\\')
                pathName = pathName.Substring(1);
            // Permet de surcharger 
            pathName = CreateRelativeOutputFileName(element, pathName);

            Debug.Assert(pathName.Length > 0 && pathName[0] != '\\', "the file name must be relatif");
            Debug.Assert(Path.HasExtension(pathName), "file name must have an extension");
            Debug.Assert(pathName.IndexOf('{') < 0, "incorrect file name");
            Context.RelativeGeneratedFileName = pathName;
            return Context.ProjectFolder != null ? Path.Combine(Context.ProjectFolder, pathName) : pathName;
        }

        /// <summary>
        /// Résolution du nom du fichier en remplacçant les mots-clés par leurs valeurs courantes.
        /// </summary>
        /// <param name="element">Element concerné par la génération</param>
        /// <param name="filePattern">Pattern du nom de fichier</param>
        /// <returns>Chaine résolue</returns>
        /// <remarks>
        /// Pattern dans le nom du fichier
        /// '~' : indique de mettre le fichier tel quel à la racine
        /// [codeFolder]|[code] : Répertoire de code (racine ou app_code)
        /// [namespace]|[nspc]  : namespace hierarchie
        /// [strategyName]|[sn] : Nom de la strategie
        /// ex : [codeFolder]/[namespace]/xxx.cs
        /// </remarks>
        /// <exception cref="Exception">Erreur de syntaxe dans le pattern</exception>
        private string ResolvePattern(ICustomizableElement element, string filePattern)
        {
            // Pour que le replace marche dans tous les cas, on va d'abord parcourir la chaine
            // pour mettre tous les mot-clés en minuscules sans toucher aux autres caractères.
            char[] buffer = filePattern.ToCharArray();
            bool inKeyword = false;
            for (int i = 0; i < buffer.Length; i++)
            {
                char ch = buffer[i];
                if (ch == ']')
                {
                    if (!inKeyword)
                        throw new Exception(
                            String.Format("Syntax error in the generated file pattern '{1}' for element {0}",
                                          element.Name, filePattern));
                    inKeyword = false;
                }
                else if (ch == '[')
                {
                    if (inKeyword)
                        throw new Exception(
                            String.Format("Syntax error in the generated file pattern '{1}' for element {0}",
                                          element.Name, filePattern));
                    inKeyword = true;
                }
                else if (inKeyword)
                {
                    buffer[i] = Char.ToLower(ch);
                }
            }

            if (inKeyword)
                throw new Exception(
                    String.Format("Syntax error in the generated file pattern '{1}' for element {0}", element.Name,
                                  filePattern));

            // Remplacement des mots-clés
            string tmpFileName = new String(buffer);

            tmpFileName = tmpFileName.Replace("[code]", "[codefolder]");
            tmpFileName = tmpFileName.Replace("[sn]", "[strategyname]");
            tmpFileName = tmpFileName.Replace("[nspc]", "[namespace]");

            tmpFileName = tmpFileName.Replace("[codefolder]", Context.RelativeProjectCodeFolder);
            tmpFileName = tmpFileName.Replace("[strategyname]", DisplayName);

            if (tmpFileName.Contains("[namespace]"))
            {
                // Arborescence correspondant au namespace
                string ns = String.Empty;
                if (element != null)
                {
                    ClassNameInfo cni = new ClassNameInfo(element.FullName);
                    ns = cni.Namespace.Replace('.', Path.DirectorySeparatorChar);
                }
                tmpFileName = tmpFileName.Replace("[namespace]", ns);
            }

            return tmpFileName;
        }

        /// <summary>
        /// Relative generated code file name
        /// </summary>
        /// <param name="element">Current element</param>
        /// <param name="fileName">Pre calculate file name</param>
        /// <returns></returns>
        protected virtual string CreateRelativeOutputFileName(ICustomizableElement element, string fileName)
        {
            return fileName;
        }

        #endregion
    }
}