using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text;
using DSLFactory.Candle.SystemModel.Dependencies;
using DSLFactory.Candle.SystemModel.Repository;
using EnvDTE;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Validation;

namespace DSLFactory.Candle.SystemModel.Strategies
{
    /// <summary>
    /// 
    /// </summary>
    [Strategy(WCFStrategy.StrategyID)]
    public class WCFStrategy : StrategyBase, 
        IStrategyCodeGenerator, 
        IStrategyProvidesProjectTemplates, 
        IStrategyCodeInjector, 
        IStrategyPublishEvents,
        IStrategyValidator,
        IStrategyAddElementInterceptor
    {
        #region Fields
        private const string StrategyID = "WCFStrategy"; // or a Guid
        private string _contractTemplate;
        private string _configTemplate;
        private string _hostingTemplate;
        private string _svcTemplate;
        //private bool generateProxy;

        /// <summary>
        /// Liste temporaire servant à la génération des attributs sur les opérations d'un contrat permettant
        /// de gérér les noms identiques.
        /// </summary>
        private List<string> tempOperationNames;

        #endregion

        #region Properties
        //public bool GenerateProxy
        //{
        //    get { return generateProxy; }
        //    set { generateProxy = value; }
        //}


        /// <summary>
        /// Gets or sets the hosting template.
        /// </summary>
        /// <value>The hosting template.</value>
        [Editor(typeof(DSLFactory.Candle.SystemModel.Editor.TemplateNameEditor), typeof(System.Drawing.Design.UITypeEditor))]
        public string HostingTemplate
        {
            [global::System.Diagnostics.DebuggerStepThrough]
            get { return _hostingTemplate; }
            [global::System.Diagnostics.DebuggerStepThrough]
            set { _hostingTemplate = value; }
        }

        /// <summary>
        /// Gets or sets the SVC template.
        /// </summary>
        /// <value>The SVC template.</value>
        [EditorAttribute(typeof(DSLFactory.Candle.SystemModel.Editor.TemplateNameEditor), typeof(System.Drawing.Design.UITypeEditor))]
        public string SvcTemplate
        {
            [global::System.Diagnostics.DebuggerStepThrough]
            get { return _svcTemplate; }
            [global::System.Diagnostics.DebuggerStepThrough]
            set { _svcTemplate = value; }
        }

        /// <summary>
        /// Gets or sets the config template.
        /// </summary>
        /// <value>The config template.</value>
        [EditorAttribute(typeof(DSLFactory.Candle.SystemModel.Editor.TemplateNameEditor), typeof(System.Drawing.Design.UITypeEditor))]
        public string ConfigTemplate
        {
            [global::System.Diagnostics.DebuggerStepThrough]
            get { return _configTemplate; }
            [global::System.Diagnostics.DebuggerStepThrough]
            set { _configTemplate = value; }
        }

        /// <summary>
        /// Gets or sets the contract template.
        /// </summary>
        /// <value>The contract template.</value>
        [EditorAttribute(typeof(DSLFactory.Candle.SystemModel.Editor.TemplateNameEditor), typeof(System.Drawing.Design.UITypeEditor))]
        public string ContractTemplate
        {
            [global::System.Diagnostics.DebuggerStepThrough]
            get { return _contractTemplate; }
            [global::System.Diagnostics.DebuggerStepThrough]
            set { _contractTemplate = value; }
        }
        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="WCFStrategy"/> class.
        /// </summary>
        public WCFStrategy()
        {
            //_entityTemplate = "WCFDataContract";
            _contractTemplate = "WCFServiceContract";
            _configTemplate = "WCFWebConfig";
            _hostingTemplate = "WCFHosting";
            _svcTemplate = "WCFWeb";            
        }

        #region Dependency properties
        public static readonly DependencyProperty<bool> GenerateProxyProperty;
        public static readonly DependencyProperty<bool> WCFServerProperty;

        public static readonly DependencyProperty<bool> GenerateWCFAttributes;

        /// <summary>
        /// ServiceContract's properties
        /// </summary>
        public static readonly DependencyProperty<System.ServiceModel.SessionMode> SessionModeProperty;
        public static readonly DependencyProperty<string> ConfigurationNameProperty;
        public static readonly DependencyProperty<string> CallbackContractProperty;
        public static readonly DependencyProperty<System.Net.Security.ProtectionLevel> ProtectionLevelProperty;

        /// <summary>
        /// OperationContract's properties
        /// </summary>
        public static readonly DependencyProperty<bool> AsyncPatternProperty;
        public static readonly DependencyProperty<bool> IsInitiatingProperty;
        public static readonly DependencyProperty<bool> IsOneWayProperty;
        public static readonly DependencyProperty<bool> IsTerminatingProperty;
        public static readonly DependencyProperty<string> ReplyActionProperty;
        public static readonly DependencyProperty<EditableCollection<FaultContractData>> FaultContractProperty;

        /// <summary>
        /// DataMember's properties
        /// </summary>
        public static readonly DependencyProperty<bool> IsRequiredProperty;
        public static readonly DependencyProperty<bool> EmitDefaultValueProperty;
        public static readonly DependencyProperty<int> OrderProperty;

        [TypeConverter(typeof(ExpandableElementConverter<FaultContractData>))]
        public class FaultContractData : ISerializableProperty
        {
            private string detailType;

            /// <summary>
            /// Gets or sets the type of the detail.
            /// </summary>
            /// <value>The type of the detail.</value>
            public string DetailType
            {
                get { return detailType; }
                set { detailType = value; }
            }

            /// <summary>
            /// Gets the display name.
            /// </summary>
            /// <value>The display name.</value>
            [Browsable(false)]
            public string DisplayName
            {
                get
                {
                    return String.Format("typeof({0})", DetailType);
                }
            }

            /// <summary>
            /// Gets the description.
            /// </summary>
            /// <value>The description.</value>
            [Browsable(false)]
            public string Description
            {
                get { return "Full name of the exception type"; }
            }

            /// <summary>
            /// Converts from string.
            /// </summary>
            /// <param name="value">The value.</param>
            public void ConvertFromString(string value)
            {
                if (String.IsNullOrEmpty(value))
                    value = String.Empty;
                detailType = value;
            }

            /// <summary>
            /// Converts to string.
            /// </summary>
            /// <returns></returns>
            public string ConvertToString()
            {
                return DetailType;
            }

            /// <summary>
            /// Returns a <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
            /// </summary>
            /// <returns>
            /// A <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
            /// </returns>
            public override string ToString()
            {
                return DisplayName;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        static WCFStrategy()
        {
            GenerateProxyProperty = new DependencyProperty<bool>(StrategyID, "GenerateProxy");
            GenerateProxyProperty.Category = "WCF";
            GenerateProxyProperty.DefaultValue = true;

            GenerateWCFAttributes = new DependencyProperty<bool>(StrategyID, "GenerateWCFAttributes");
            GenerateWCFAttributes.Category = "WCF";
            GenerateWCFAttributes.DefaultValue = true;

            WCFServerProperty = new DependencyProperty<bool>(StrategyID, "Host WCF Services");
            WCFServerProperty.Category = "WCF";
            WCFServerProperty.DefaultValue = true;

            ReplyActionProperty = new DependencyProperty<string>(StrategyID, "ReplyAction");
            ReplyActionProperty.Category = "WCF";
            IsInitiatingProperty = new DependencyProperty<bool>(StrategyID, "IsInitiating");
            IsInitiatingProperty.Category = "WCF";
            IsOneWayProperty = new DependencyProperty<bool>(StrategyID, "IsOneWay");
            IsOneWayProperty.Category = "WCF";
            IsTerminatingProperty = new DependencyProperty<bool>(StrategyID, "IsTerminating");
            IsTerminatingProperty.Category = "WCF";

            ProtectionLevelProperty = new DependencyProperty<System.Net.Security.ProtectionLevel>(StrategyID, "ProtectionLevel");
            ProtectionLevelProperty.Category = "WCF";
            ProtectionLevelProperty.DefaultValue = System.Net.Security.ProtectionLevel.None;

            CallbackContractProperty = new DependencyProperty<string>(StrategyID, "CallbackContract");
            CallbackContractProperty.Category = "WCF";

            FaultContractProperty = new DependencyProperty<EditableCollection<FaultContractData>>(StrategyID, "FaultContract");
            FaultContractProperty.Category = "WCF";
            FaultContractProperty.DefaultValueCreatorHandler = delegate { return new EditableCollection<FaultContractData>(); };
            FaultContractProperty.TypeConverter = typeof(ExpandableElementConverter<EditableCollection<FaultContractData>>);

            ConfigurationNameProperty = new DependencyProperty<string>(StrategyID, "ConfigurationName");
            ConfigurationNameProperty.Category = "WCF";

            SessionModeProperty = new DependencyProperty<System.ServiceModel.SessionMode>(StrategyID, "SessionMode");
            SessionModeProperty.Category = "WCF";
            SessionModeProperty.DefaultValue = System.ServiceModel.SessionMode.Allowed;

            AsyncPatternProperty = new DependencyProperty<bool>(StrategyID, "AsyncPattern");
            AsyncPatternProperty.Category = "WCF";

            IsRequiredProperty = new DependencyProperty<bool>(StrategyID, "IsRequired");
            IsRequiredProperty.Category = "WCF";

            EmitDefaultValueProperty = new DependencyProperty<bool>(StrategyID, "EmitDefaultValue");
            EmitDefaultValueProperty.Category = "WCF";

            OrderProperty = new DependencyProperty<int>(StrategyID, "Order");
            OrderProperty.Category = "WCF";
        }

        /// <summary>
        /// Contextual dependency properties filter
        /// </summary>
        /// <param name="modelElement"></param>
        /// <returns></returns>        
        public override PropertyDescriptorCollection GetCustomProperties(ModelElement modelElement)
        {
            PropertyDescriptorCollection collections = base.GetCustomProperties(modelElement);

            if (modelElement is ClassUsesOperations)
            {
                ClassUsesOperations l = modelElement as ClassUsesOperations;
                if (l.ExternalTargetService != null)
                {
                    collections.Add(GenerateProxyProperty.Register(modelElement));
                }
            }

            if (modelElement is Property)
            {
                collections.Add(IsRequiredProperty.Register(modelElement));
                collections.Add(EmitDefaultValueProperty.Register(modelElement));
                collections.Add(OrderProperty.Register(modelElement));
                collections.Add(GenerateWCFAttributes.Register(modelElement));
            }

            // ServiceContract de la couche publique
            // Ou operation d'un servicecontract de la couche publique
            if (CheckPublicContract(modelElement))
            {
                if (modelElement is DSLFactory.Candle.SystemModel.ServiceContract)
                {
                    collections.Add(SessionModeProperty.Register(modelElement));
                    collections.Add(ConfigurationNameProperty.Register(modelElement));
                    collections.Add(CallbackContractProperty.Register(modelElement));
                    collections.Add(ProtectionLevelProperty.Register(modelElement));
                }
            }

            if (modelElement is DSLFactory.Candle.SystemModel.Operation)
            {
                Operation op = (Operation)modelElement;
                if (op.Parent is ServiceContract && CheckPublicContract(op.Parent))
                {
                    collections.Add(GenerateWCFAttributes.Register(modelElement));
                    collections.Add(FaultContractProperty.Register(modelElement));
                    collections.Add(AsyncPatternProperty.Register(modelElement));
                    collections.Add(IsTerminatingProperty.Register(modelElement));
                    collections.Add(IsInitiatingProperty.Register(modelElement));
                    collections.Add(ProtectionLevelProperty.Register(modelElement));
                    collections.Add(ReplyActionProperty.Register(modelElement));
                    collections.Add(IsOneWayProperty.Register(modelElement));
                }                
            }

            if (modelElement is Association)
            {
                collections.Add(GenerateWCFAttributes.Register(modelElement));
            }

            if (modelElement is SoftwareComponent )
            {
                collections.Add(WCFServerProperty.Register(modelElement));
            }
            return collections;
        }

        /// <summary>
        /// Teste si on est sur un contract de la couche publique
        /// </summary>
        /// <param name="element">The element.</param>
        /// <returns></returns>
        private bool CheckPublicContract(ModelElement element)
        {
            if (element is ServiceContract)
            {
                // Uniquement la couche publique
                InterfaceLayer layer = ((ServiceContract)element).Layer;
                if (layer != null && layer.LayerPackage.Layers.Count > 0 && layer.LayerPackage.Layers[0].HostingContext != HostingContext.None)
                    return true;
            }
            if (element is Layer)
            {
                return ((Layer)element).HostingContext != HostingContext.None;
            }
            return false;
        }
        #endregion

        #region Strategy Code Generation
        /// <summary>
        /// Executes this instance.
        /// </summary>
        public void Execute()
        {
            if (Context.GenerationPass == GenerationPass.ElementAdded)
                return;

            try
            {
                if (CurrentElement is ClassImplementation)
                {
                    ClassImplementation clazz = CurrentElement as ClassImplementation;
                    GenerateWebEndPoint(clazz.Layer);
                    if (clazz.Contract != null && CheckPublicContract(clazz.Contract))
                    {
                   //     clazz.Layer.AddReference("DiscoveryLib", ArtifactType.Assembly, ReferenceScope.Compilation, "*");
                    }                 
                }

                // Host
                if (CurrentElement is Layer)
                {
                    Layer layer = CurrentElement as Layer;
                    if (Context.GenerationPass == GenerationPass.MetaModelUpdate)
                    {
                        AddWCFReference(layer);
                    // Fichier de config
                        GenerateConfig(layer);
                    }

                    if (Context.GenerationPass == GenerationPass.CodeGeneration)
                    {
                        // Host console
                        GenerateConsoleServer(layer);
                        GenerateServiceProxy(layer);
                    }
                }
            }
            catch (Exception ex)
            {
                LogError(ex);
            }
        }

        /// <summary>
        /// Génération du fichier de config
        /// </summary>
        /// <param name="layer"></param>
        private void GenerateConfig(Layer layer)
        {
            if (!String.IsNullOrEmpty(ConfigTemplate) && layer.HostingContext != HostingContext.None && layer.LayerPackage.InterfaceLayer!=null)
            {
                string data = CallT4Template(Context.Project,
                    ConfigTemplate,
                    (CandleElement)CurrentElement,
                    null);
                if (data != null)
                    layer.AddXmlConfigurationContent("wcfconfig", data);
            }
        }

        /// <summary>
        /// Generates the console server.
        /// </summary>
        /// <param name="layer">The layer.</param>
        private void GenerateConsoleServer(Layer layer)
        {
            if (!WCFServerProperty.GetValue(layer.Component))
                return;

            if (layer.HostingContext == HostingContext.Standalone && HostingTemplate != null && layer.LayerPackage.InterfaceLayer != null)
            {
                CallT4Template(Context.Project,
                    HostingTemplate,
                    (CandleElement)CurrentElement,
                    "~/Program.cs");
            }
        }

        /// <summary>
        /// Generates the web end point.
        /// </summary>
        /// <param name="layer">The layer.</param>
        private void GenerateWebEndPoint(Layer layer)
        {
            if (!WCFServerProperty.GetValue(layer.Component))
                return;
            // TODO il faut obligatoirement une couche d'interface
            if (layer.HostingContext == HostingContext.Web && SvcTemplate != null && layer.LayerPackage.InterfaceLayer != null)
            {
                CallT4Template(Context.Project,
                    SvcTemplate,
                    (CandleElement)CurrentElement,
                    String.Format("~/{0}.svc", CurrentElement.Name));
            }
        }

        /// <summary>
        /// Generates the service proxy.
        /// </summary>
        /// <param name="layer">The layer.</param>
        private void GenerateServiceProxy(Layer layer)
        {
            string folder = Path.GetTempFileName();
            File.Delete(folder);
            Directory.CreateDirectory(folder);
            string configFile = Path.Combine(folder, "app.config");

            try
            {
                foreach (ClassUsesOperations service in layer.GetServicesUsed(Context.Mode))
                {
                    if (!(service.TargetService is ExternalServiceContract) || service.ExternalTargetService.Parent.MetaData == null)
                        continue;

                    // Ici on est en relation avec un composant externe, on va créer le proxy si nécessaire ou simplement récupèrer les
                    // infos de config.
                    ExternalServiceContract esc = service.TargetService as ExternalServiceContract;

                    string mexAddress = null;
                    string tmp = esc.Parent.MetaData.TestBaseAddress;
                    if (tmp != null)
                    {
                        tmp = tmp.TrimEnd('/', '\\');
                        if (tmp.EndsWith(".asmx"))
                        {
                            // C'est un service web classique
                            //ILogger logger = ServiceLocator.Instance.GetService<ILogger>();
                            //if (logger != null)
                            //    logger.Write("WCFStrategy", "Web service behavior not implemented", LogType.Error);
                            //continue;
                            mexAddress = String.Format(@"""{0}?wsdl""", tmp);
                        }
                        else
                            mexAddress = String.Format(@"""{0}/{1}""", tmp, esc.Name);

                    }

                    // On regarde si le modèle ne contient pas son wsdl et si oui,
                    // on le copie dans le répertoire de génération
                    string repositoryFolder = Path.GetDirectoryName(esc.Parent.MetaData.GetFileName(PathKind.Absolute));
                    if (Utils.CopyFiles(repositoryFolder, folder, "*.wsdl") > 0)
                    {
                        Utils.CopyFiles(repositoryFolder, folder, "*.xsd");
                        mexAddress = "*.wsdl *.xsd";
                    }

                    if (mexAddress == null)
                        continue;

                    string extension = StrategyManager.GetInstance(service.Store).TargetLanguage.Extension;
                    SvcUtilProcess tool = new SvcUtilProcess();
                    string namesp = esc.Parent.Namespace;

                    string outputFileName = CreateOutputFileName(Context.Project, esc, null);
                    try
                    {
                        // Génération du proxy si la date du fichier du proxy est antérieure à la date du
                        // modèle externe.
                        if (!File.Exists(outputFileName) || File.GetLastWriteTimeUtc(outputFileName) < esc.Parent.MetaData.LocalDateTime)
                        {
                            // Nouveau fichier de config
                            File.WriteAllText(configFile, "<configuration/>");

                            if (tool.CreateProxy(folder, namesp, mexAddress) == 0)
                            {
                                // Ajout des infos de config
                                string cfg = null;
                                if (File.Exists(configFile))
                                {
                                    cfg = File.ReadAllText(configFile);
                                    if (String.IsNullOrEmpty(cfg))
                                        cfg = null;
                                }
                                layer.AddXmlConfigurationContent("wcf_config_endpoint_" + service.Name, cfg);

                                string sourceProxyFilePath = Path.Combine(folder, String.Concat("proxy", extension));
                                if (GenerateProxyProperty.GetValue(service))
                                {
                                    // Ajout du proxy
                                    Utils.CopyFile(sourceProxyFilePath, outputFileName);
                                    if (File.Exists(outputFileName))
                                    {
                                        ProjectItem pi = ServiceLocator.Instance.ShellHelper.AddFileToProject(Context.Project, outputFileName);
                                        WCFProxyVisitor wcfVisitor = new WCFProxyVisitor(esc);
                                        DSLFactory.Candle.SystemModel.CodeGeneration.CodeModel.CodeModelWalker walker = new DSLFactory.Candle.SystemModel.CodeGeneration.CodeModel.CodeModelWalker(wcfVisitor);
                                        walker.Traverse(pi.FileCodeModel);
                                    }
                                }
                                else
                                    Utils.DeleteFile(sourceProxyFilePath);
                            }
                            else
                                LogError("WCFStrategy - Create proxy " + tool.ErrorMessage);
                        }
                    }
                    catch (Exception ex)
                    {
                        LogError("WCFStrategy - Create proxy " + ex.Message);
                        if (File.Exists(outputFileName))
                            File.Delete(outputFileName);
                    }
                }
            }
            finally
            {
                Utils.RemoveDirectory(folder);
            }
        }

        /// <summary>
        /// Adds the WCF reference.
        /// </summary>
        /// <param name="layer">The layer.</param>
        private void AddWCFReference(Layer layer)
        {
            foreach (ClassUsesOperations service in layer.GetServicesUsed(Context.Mode))
            {
                if (!(service.TargetService is ExternalServiceContract))
                    continue;
                if (service.ExternalTargetService.Parent.ReferencedModel != null)
                {
                    if (GenerateProxyProperty.GetValue(service))
                    {
                        // Mise à jour du namespace
                        if (String.IsNullOrEmpty(service.ExternalTargetService.Parent.Namespace))
                            service.ExternalTargetService.Parent.Namespace = service.ExternalTargetService.Parent.ReferencedModel.SoftwareComponent.Namespace;

                        service.Scope = ReferenceScope.None;
                    }
                    else
                    {
                        service.ExternalTargetService.Parent.Namespace = null;
                        service.Scope |= ReferenceScope.Compilation;
                        if (!service.ExternalTargetService.IsInGac)
                            service.Scope |= ReferenceScope.Runtime;
                    }
                    layer.AddReference("System.ServiceModel", ArtifactType.DotNetFramework, ReferenceScope.Compilation, "*");
                    layer.AddReference("System.Runtime.Serialization", ArtifactType.DotNetFramework, ReferenceScope.Compilation, "*");
                }
            }
        }        
        #endregion

        #region IStrategyProvidesProjectTemplates Members
        ///// <summary>
        ///// Gets a value indicating whether this instance is web hosting.
        ///// </summary>
        ///// <value>
        ///// 	<c>true</c> if this instance is web hosting; otherwise, <c>false</c>.
        ///// </value>
        //private bool IsWebHosting
        //{
        //    get
        //    {
        //        if (this.Context == null)
        //            return false;
        //        Layer layer = this.Context.Model.SoftwareComponent.GetMainLayer();
        //        return layer != null && layer.HostingContext == HostingContext.Web;
        //    }
        //}

        /// <summary>
        /// Gets the project template.
        /// </summary>
        /// <param name="layer">The layer.</param>
        /// <returns></returns>
        public string GetProjectTemplate(SoftwareLayer layer)
        {
            if (layer != null && layer is PresentationLayer)
            {
                if (((PresentationLayer)layer).HostingContext == HostingContext.Web)
                    return @"web\EmptyWeb.zip";
                else if (((PresentationLayer)layer).HostingContext == HostingContext.Standalone)
                    return @"CSharp/consoleapplication.zip";
            }
            return null;
        }

        /// <summary>
        /// Gets the assembly extension.
        /// </summary>
        /// <param name="layer">The layer.</param>
        /// <returns></returns>
        public string GetAssemblyExtension(SoftwareLayer layer)
        {
            if (layer != null && layer is PresentationLayer && ((PresentationLayer)layer).HostingContext == HostingContext.Standalone)
                return ".exe";

            return null;
        }

        #endregion

        #region IStrategyCodeInjector Members

        /// <summary>
        /// Called when [generate class].
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="clazz">The clazz.</param>
        void IStrategyCodeInjector.OnGenerateClass(CodeInjectionContext context, DSLFactory.Candle.SystemModel.CodeGeneration.CodeModel.CandleCodeClass clazz)
        {
            if (context.CurrentElement is ClassImplementation)
            {
                ClassImplementation impl = context.CurrentElement as ClassImplementation;
                //if (impl.Contract != null && CheckPublicContract(impl.Contract))
                //{
                //    clazz.AddAttribute("Microsoft.ServiceModel.Samples.Discovery.Discoverable");
                //}
            }

            // Ajout des attributs
            if (context.CurrentElement is Entity)
            {
                Entity entity = context.CurrentElement as Entity;
                Dictionary<string, string> args = new Dictionary<string, string>();

                // A remettre
                //if (!String.IsNullOrEmpty(entity.XmlName))
                //{
                //    args.Add("Name", "\"" + entity.XmlName + "\"");
                //}
                if (!String.IsNullOrEmpty(entity.DataLayer.XmlNamespace))
                {
                    args.Add("Namespace", "\"" + entity.DataLayer.XmlNamespace + "\"");
                }

                clazz.AddAttribute("DataContract", context.Strategy.StrategyId, false, args);
            }
        }

        /// <summary>
        /// Called when [generate enum].
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="enumeration">The enumeration.</param>
        void IStrategyCodeInjector.OnGenerateEnum(CodeInjectionContext context, DSLFactory.Candle.SystemModel.CodeGeneration.CodeModel.CandleCodeEnum enumeration)
        {
        }

        /// <summary>
        /// Called when [generate function].
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="function">The function.</param>
        void IStrategyCodeInjector.OnGenerateFunction(CodeInjectionContext context, DSLFactory.Candle.SystemModel.CodeGeneration.CodeModel.CandleCodeFunction function)
        {
            // Service factory
            if (context.CurrentElement is Layer)
            {
                // On part du principe que le template du servicelocator à généré une classe nommée ServiceLocator et 
                // une méthode par création d'instance dont le nom commence par CreateProxyForxxxxxx
                if (function.Parent.Name == "ServiceLocatorBase" && function.Name.StartsWith("CreateProxyFor"))
                {
                    string currentName = function.Name.Substring("CreateProxyFor".Length);
                    foreach (ClassUsesOperations service in ((Layer)context.CurrentElement).GetServicesUsed(context.GenerationContext.Mode))
                    {
                        if (!(service.TargetService is ExternalServiceContract))
                            continue;

                        ExternalServiceContract esc = service.TargetService as ExternalServiceContract;
                        if (esc.Name != function.Name.Substring("CreateProxyFor".Length))
                            continue;

                        // Reference peut se faire de plusieurs façons:
                        //  1 - Statique (liaison avec les assemblies publiques du service)
                        //  2 - Par proxy (Avec création du proxy)
                        // La configuration se fait toujours dans le fichier de config.
                        string baseAddress = esc.Parent.ReferencedModel.BaseAddress;

                        if (GenerateProxyProperty.GetValue(service))
                        {
                            function.ReplaceBody(String.Format("return new {0}(\"DefaultBinding_{1}_{1}\", \"{2}/{3}\");", esc.FullName + "ClientProxy", esc.Name, baseAddress, esc.Name));
                        }
                        else
                        {
                            string fullType = function.CodeElement.Type.AsFullName;
                            StringBuilder sb = new StringBuilder();
                            sb.AppendFormat("System.ServiceModel.EndpointAddress endpoint = new System.ServiceModel.EndpointAddress(\"{0}/{1}\");", baseAddress, esc.Name);
                            sb.AppendLine();
                            sb.AppendFormat("System.ServiceModel.ChannelFactory<{0}> factory = new System.ServiceModel.ChannelFactory<{0}>(\"DefaultBinding_{1}\", endpoint);", fullType, esc.Name);
                            sb.AppendLine();
                            sb.Append("return factory.CreateChannel();");
                            function.ReplaceBody(sb.ToString());
                        }
                    }
                }
            }

            // ServiceContract
            if (context.CurrentElement is ServiceContract && CheckPublicContract(context.CurrentElement as ModelElement))
            {
                ServiceContract contract = context.CurrentElement as ServiceContract;
                Operation op = function.FindOperationFromContract(contract);
                if (op != null)
                {
                    if (!GenerateWCFAttributes.GetValue(op))
                        return;

                    Dictionary<string, string> args = new Dictionary<string, string>();
                    
                    // TODO a remettre (faire une recherche sur 'a remettre')
                    //string name = op.XmlName;
                    //if (String.IsNullOrEmpty(name))
                    string name = op.Name;
                    int nb = tempOperationNames.FindAll(delegate(string s) { return s == name; }).Count;
                    if (nb > 0)
                        args.Add("Name" , "\"" + name + nb.ToString() + "\"");
                    else
                        args.Add("Name", "\"" + name + "\"");

                    tempOperationNames.Add(name); // Contient les noms des opérations (RAZ dans le OnGenerateInterface)

                    if (WCFStrategy.AsyncPatternProperty.GetValue(op))
                        args.Add("AsyncPattern", "true");
                    if (WCFStrategy.IsInitiatingProperty.GetValue(op))
                        args.Add("IsInitiating", "true");
                    if (WCFStrategy.IsOneWayProperty.GetValue(op))
                        args.Add("IsOneWay", "true");
                    if (WCFStrategy.IsTerminatingProperty.GetValue(op))
                        args.Add("IsTerminating", "true");
                    if (WCFStrategy.ProtectionLevelProperty.GetValue(op).ToString() != "None")
                        args.Add("ProtectionLevel", "ProtectionLevel." + WCFStrategy.ProtectionLevelProperty.GetValue(op).ToString());
                    if (!string.IsNullOrEmpty(WCFStrategy.ReplyActionProperty.GetValue(op)))
                        args.Add("ReplyAction", "\"" + WCFStrategy.ReplyActionProperty.GetValue(op) + "\"");

                    function.AddAttribute("OperationContract", context.Strategy.StrategyId, false, args);

                    EditableCollection<FaultContractData> list = FaultContractProperty.GetValue(op);
                    if (list != null)
                    {
                        foreach (FaultContractData data in list)
                            function.AddAttribute("System.Runtime.Serialization.FaultContract", context.Strategy.StrategyId, true, String.Empty, "typeof(" + data.DetailType + ")");
                    }
                }
            }
        }

        /// <summary>
        /// Genere au niveau de la classe, l'attribut knownType
        /// </summary>
        /// <param name="context">Contexte d'injection</param>
        /// <param name="property">Propriété à l'origine de cette génération</param>
        /// <param name="entity">Entité courante</param>
        private static void GenerateKnownTypeAttribute(CodeInjectionContext context, DSLFactory.Candle.SystemModel.CodeGeneration.CodeModel.CandleCodeProperty property, Entity entity)
        {
            // Tableau temporaire utilisé pour passer les attributs
            Dictionary<string, string> args = new Dictionary<string, string>();

            // Création pour les sous-classes de la classe concernée
            foreach (Entity subclass in entity.SubClasses)
            {
                args.Clear();
                args.Add(String.Empty, String.Format("typeof(global::{0})", subclass.FullTypeName));
                property.Parent.AddAttribute("System.Runtime.Serialization.KnownType", context.Strategy.StrategyId, true, args);
            }
        }

        /// <summary>
        /// Called when [generate interface].
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="interf">The interf.</param>
        void IStrategyCodeInjector.OnGenerateInterface(CodeInjectionContext context, DSLFactory.Candle.SystemModel.CodeGeneration.CodeModel.CandleCodeInterface interf)
        {
            if (context.CurrentElement is ServiceContract && CheckPublicContract(context.CurrentElement as ModelElement))
            {
                tempOperationNames = new List<string>();

                Dictionary<string, string> args = new Dictionary<string, string>();

                ServiceContract contract = context.CurrentElement as ServiceContract;

                if (!string.IsNullOrEmpty(WCFStrategy.ConfigurationNameProperty.GetValue(contract)))
                    args.Add("ConfigurationName", "\"" + WCFStrategy.ConfigurationNameProperty.GetValue(contract) + "\"");
                args.Add("Namespace", String.Format("\"{0}\"", contract.FullName));
                if (WCFStrategy.ProtectionLevelProperty.GetValue(contract).ToString() != "None")
                    args.Add("ProtectionLevel", "ProtectionLevel." + WCFStrategy.ProtectionLevelProperty.GetValue(contract).ToString());
                if (WCFStrategy.SessionModeProperty.GetValue(contract).ToString() != "Allowed")
                    args.Add("SessionMode", "SessionMode." + WCFStrategy.SessionModeProperty.GetValue(contract).ToString());
                if (WCFStrategy.CallbackContractProperty.GetValue(contract) != null)
                    args.Add("CallbackContract", "typeof(" + WCFStrategy.CallbackContractProperty.GetValue(contract) + ")");

                interf.AddAttribute("ServiceContract", context.Strategy.StrategyId, false, args);
            }
        }

        /// <summary>
        /// Called when [generate parameter].
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="parm">The parm.</param>
        void IStrategyCodeInjector.OnGenerateParameter(CodeInjectionContext context, DSLFactory.Candle.SystemModel.CodeGeneration.CodeModel.CandleCodeParameter parm)
        {
        }

        /// <summary>
        /// Called when [generate property].
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="property">The property.</param>
        void IStrategyCodeInjector.OnGenerateProperty(CodeInjectionContext context, DSLFactory.Candle.SystemModel.CodeGeneration.CodeModel.CandleCodeProperty property)
        {
            // Ajout des attributs
            if (context.CurrentElement is Entity)
            {
                // Recherche de la property
                string xmlName = String.Empty;
                Entity entity = context.CurrentElement as Entity;
                bool generateWcfAttribute = false;

                ICustomizableElement elem = null;

                // On va rechercher l'élément associé en parcourant les associations issues de l'entité en 
                // cours.
                IList<Association> associations = Association.GetLinksToTargets(entity);
                foreach (Association a in associations)
                {
                    // Recherche sur le nom du role
                    if (a.SourceRoleName == property.Name)
                    {
                        elem = a.Source;
                        generateWcfAttribute = GenerateWCFAttributes.GetValue(a);
                        xmlName = a.SourceRoleName;
                        if (!String.IsNullOrEmpty(a.XmlName))
                            xmlName = a.XmlName;
                        break;
                    }
                }

                // Si pas une association, c'est que c'est une propriété
                if (elem == null)
                {
                    elem = property.FindPropertyFromEntity(entity);
                    if (elem != null)
                    {
                        generateWcfAttribute = GenerateWCFAttributes.GetValue(elem);
                        // A remettre
                        // xmlName = ((Property)elem).XmlName;
                    }
                }

                // Si on a trouvé quelque chose, on génére
                if (elem != null)
                {
                    if (!generateWcfAttribute)
                        return;

                    // Les attributs KnownType pour les classes surchargées ou les associations dont le type de retour n'est pas connu
                    GenerateKnownTypeAttribute(context, property, entity);

                    // Les attributs WCF
                    Dictionary<string, string> args = new Dictionary<string, string>();

                    if (WCFStrategy.IsRequiredProperty.GetValue(elem))
                        args.Add("AsyncPattern", "true");
                    if (WCFStrategy.EmitDefaultValueProperty.GetValue(elem))
                        args.Add("EmitDefaultValue", "true");
                    if (WCFStrategy.OrderProperty.GetValue(elem) > 0)
                    {
                        args.Add("Order=", WCFStrategy.OrderProperty.GetValue(elem).ToString());
                    }
                    if (WCFStrategy.IsRequiredProperty.GetValue(elem))
                        args.Add("IsRequired", "true");
                    if (!String.IsNullOrEmpty(xmlName))
                    {
                        args.Add("Name", "\"" + xmlName + "\"");
                    }
                    property.AddAttribute("DataMember", context.Strategy.StrategyId, false, args);
                }
            }
        }

        /// <summary>
        /// Called when [generate struct].
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="structure">The structure.</param>
        void IStrategyCodeInjector.OnGenerateStruct(CodeInjectionContext context, DSLFactory.Candle.SystemModel.CodeGeneration.CodeModel.CandleCodeStruct structure)
        {
        }

        /// <summary>
        /// Called when [generate variable].
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="variable">The variable.</param>
        void IStrategyCodeInjector.OnGenerateVariable(CodeInjectionContext context, DSLFactory.Candle.SystemModel.CodeGeneration.CodeModel.CandleCodeVariable variable)
        {
        }

        /// <summary>
        /// Called when [meta model update].
        /// </summary>
        /// <param name="context">The context.</param>
        void IStrategyCodeInjector.OnMetaModelUpdate(CodeInjectionContext context)
        {
            if (context.CurrentElement is DataType)
            {
                Entity entity = context.CurrentElement as Entity;
                if( entity != null)
                    entity.Package.Layer.AddReference("System.Runtime.Serialization", ArtifactType.DotNetFramework);
                return;
            }
            if (context.CurrentElement is ServiceContract && CheckPublicContract(context.CurrentElement as ServiceContract))
            {
                ServiceContract contract = context.CurrentElement as ServiceContract;
                contract.Layer.AddReference("System.ServiceModel", ArtifactType.DotNetFramework);
            }
        }

        /// <summary>
        /// Called when [generate using].
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        String[] IStrategyCodeInjector.OnGenerateUsing(CodeInjectionContext context)
        {
            List<string> usings = new List<string>();
            if (context.CurrentElement is DataType)
            {
                usings.Add("System.Runtime.Serialization");
            }
            if( context.CurrentElement is ServiceContract && CheckPublicContract(context.CurrentElement as ServiceContract))
                usings.Add("System.ServiceModel");

            return usings.ToArray();
        }

        #endregion

        #region IStrategyValidator Members

        /// <summary>
        /// Validates the specified element.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <param name="logInfo">The log info.</param>
        public void Validate(ModelElement element, ValidationContext logInfo)
        {
            SoftwareLayer layer = element as SoftwareLayer;
            if (layer != null)
            {
                SoftwareComponent component = layer.SoftwareComponent;
                int major = component.Model.DotNetFrameworkVersion.Major;
                if (major < 3)
                {
                    bool exists = false;
                    foreach (ValidationMessage msg in logInfo.CurrentViolations)
                    {
                        if (msg.Code == "WCF0002")
                        {
                            exists = true;
                            break;
                        }
                    }
                    if (!exists)
                        logInfo.LogError("The .net framework version used must be greater than version 2 in the model properties", "WCF0002", component.Model);
                }
            }

            if( element is PresentationLayer )
            {
                // Si on est sur un serveur WCF, il faut une adresse de test
                PresentationLayer pLayer = element as PresentationLayer;
                if (pLayer.HostingContext != HostingContext.None && WCFServerProperty.GetValue(pLayer.Component))
                {
                    if (String.IsNullOrEmpty(pLayer.Component.Model.BaseAddress))
                        logInfo.LogError("You must provide a base address in the model's properties to publish WCF services", "WCF0001", pLayer.Component.Model);
                }
            }
        }

        #endregion

        #region IStrategyAddElementInterceptor Members

        /// <summary>
        /// Gets the wizard.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public IStrategyWizard GetWizard(ModelElement model)
        {
            return null;
        }

        /// <summary>
        /// Called when [element added].
        /// </summary>
        /// <param name="owner">The owner.</param>
        /// <param name="e">The <see cref="DSLFactory.Candle.SystemModel.Strategies.StrategyElementElementAddedEventArgs"/> instance containing the event data.</param>
        public void OnElementAdded(ICustomizableElement owner, StrategyElementElementAddedEventArgs e)
        {
            if (owner is SoftwareComponent)
            {
                ((SoftwareComponent)owner).Model.BaseAddress = "http://localhost:8000";
            }
        }

        #endregion

        #region IStrategyPublishEvents Members

        /// <summary>
        /// Called when [before local publication].
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="modelFileName">Name of the model file.</param>
        public void OnBeforeLocalPublication(CandleModel model, string modelFileName)
        {
        }

        /// <summary>
        /// Called when [after local publication].
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="modelFileName">Name of the model file.</param>
        public void OnAfterLocalPublication(CandleModel model, string modelFileName)
        {
            ILogger logger = ServiceLocator.Instance.GetService<ILogger>();
            if (model.SoftwareComponent == null)
                return;

            // Génération du wsdl avec svcutil
            Layer layer = model.SoftwareComponent.GetMainLayer();
            if (layer == null || layer.LayerPackage.InterfaceLayer == null)
                return;

            bool flag = false;
            // Recherche si il y a bien une couhe publique
            foreach (Layer l in layer.LayerPackage.Layers)
            {
                if (l.HostingContext != HostingContext.None)
                {
                    flag = true;
                    break;
                }
            }

            if (!flag)
                return;

            // Dossier dans lequel a été publié le projet
            string folder = Path.GetDirectoryName( model.MetaData.GetFileName(PathKind.Absolute));
            // Nom de la dll
            string args = layer.LayerPackage.InterfaceLayer.AssemblyName + ".dll";
            if (File.Exists(Path.Combine(folder, args)))
            {
                try
                {
                    SvcUtilProcess svcutil = new SvcUtilProcess();

                    // Calcul des dépendances
                    ConfigurationMode mode = new ConfigurationMode();
                    ReferenceWalker rw = new ReferenceWalker(ReferenceScope.Compilation, mode);
                    ReferenceVisitor visitor = new ReferenceVisitor(ReferenceScope.Compilation, folder);
                    rw.Traverse(visitor, layer);

                    StringBuilder sb = new StringBuilder();
                    foreach (string filePath in visitor.References)
                    {
                        if (File.Exists(filePath)) // TODO virer les références aux frameworks
                        {
                            sb.Append(@" /r:""");
                            sb.Append(filePath);
                            sb.Append(@"""");
                        }
                    }
                    if (sb.Length > 0)
                        args = String.Concat(args, sb.ToString());
                    svcutil.CreateWsdl(folder, args);
                    if (!String.IsNullOrEmpty(svcutil.ErrorMessage) && logger != null)
                        logger.Write("Publish services description", "svcutil error " + svcutil.ErrorMessage, LogType.Error);
                }
                catch (Exception ex)
                {
                    if (logger != null)
                        logger.WriteError("Publish services description", "Error when generating wsdl", ex);
                }
            }
            else if (logger != null)
                logger.Write("Publish services description", "Unable to generate wsdl. You must generate the dll", LogType.Warning);
        }

        /// <summary>
        /// Called when [before server publication].
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="modelFileName">Name of the model file.</param>
        public void OnBeforeServerPublication(CandleModel model, string modelFileName)
        {
        }

        /// <summary>
        /// Called when [after server publication].
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="modelFileName">Name of the model file.</param>
        public void OnAfterServerPublication(CandleModel model, string modelFileName)
        {
        }

        /// <summary>
        /// Called when [publication ended].
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="modelFileName">Name of the model file.</param>
        public void OnPublicationEnded(CandleModel model, string modelFileName)
        {
        }

        /// <summary>
        /// Called when [publication error].
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="modelFileName">Name of the model file.</param>
        public void OnPublicationError(CandleModel model, string modelFileName)
        {
        }

        #endregion
    }
}
