using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Xml;
using DSLFactory.Candle.SystemModel.Dependencies;
using DSLFactory.Candle.SystemModel.Repository;
using DSLFactory.Candle.SystemModel.Utilities;
using EnvDTE;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell.Interop;
using Constants=EnvDTE.Constants;

namespace DSLFactory.Candle.SystemModel.Strategies
{
    /// <summary>
    /// Stratégie servant à générer les fichiers de config.
    /// Fonctionnement:
    ///     - Génération du fichier à partir d'un template
    ///     - Récupération du fichier de l'utilisateur (doit s'appeler app.config et web.config)
    ///     - Fusion des deux dans un fichier nommé (app.config ou web.config)
    ///     - Association du fichier généré au fichier de l'utilisateur
    /// </summary>
    [CLSCompliant(false)]
    [Strategy("{AA927CB4-92B6-4a7c-B36B-1D66BF59B67E}")]
    public class ConfigurationFileStrategy : StrategyBase,
                                             IStrategyCodeGenerator,
                                             IStrategyConfigGenerator,
                                             IVsRunningDocTableEvents
    {
        private static readonly char[] pathSeparators;
        private readonly List<string> _configFolders = new List<string>();
        private string _configFolderName;
        private bool _generating;
        private DateTime _lastGenerationTime;
        private uint _rdtEventsCookie;
        private string _templateFormatName;

        /// <summary>
        /// Initializes the <see cref="ConfigurationFileStrategy"/> class.
        /// </summary>
        static ConfigurationFileStrategy()
        {
            pathSeparators = new char[] {Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar};
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigurationFileStrategy"/> class.
        /// </summary>
        public ConfigurationFileStrategy()
        {
            _lastGenerationTime = DateTime.MinValue;
            _templateFormatName = "{0}.config";
            _configFolderName = "Config";
        }

        /// <summary>
        /// Format de constitution du nom du fichier de config pour la recherche sur
        /// le reférentiel
        /// </summary>
        public string TemplateFormatName
        {
            [DebuggerStepThrough]
            get { return _templateFormatName; }
            [DebuggerStepThrough]
            set { _templateFormatName = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string ConfigFolderName
        {
            [DebuggerStepThrough]
            get { return _configFolderName; }
            [DebuggerStepThrough]
            set { _configFolderName = value; }
        }

        #region Members

        /// <summary>
        /// Génération des fichiers de stratégies
        /// </summary>
        /// <remarks>
        /// </remarks>
        public void Execute()
        {
            // Pour éviter de boucler, on ne regénére pas si la dernière génération s'est faite dans les dernières 2 sec
            TimeSpan ts = DateTime.Now - _lastGenerationTime;
            if (ts.TotalSeconds <= 2)
                return;

            IShellHelper shell = ServiceLocator.Instance.ShellHelper;
            // vérification si la stratégie s'applique sur l'élément courant
            if (Context.GenerationPass != GenerationPass.CodeGeneration || shell == null)
                return;

            if (!(CurrentElement is Layer && ((Layer) CurrentElement).LayerPackage.IsHigherLevel()))
                return;

            try
            {
                _generating = true;
                string configFolderPath =
                    GenerateConfigurationFile(CurrentElement as Layer, Context.Project, Context.ProjectFolder, true);
                // Puis on s'abonne aux events de modif
                if (configFolderPath != null)
                    AdviseFileEvents(configFolderPath);
            }
            catch (Exception ex)
            {
                LogError(ex);
            }
            finally
            {
                _lastGenerationTime = DateTime.Now;
                _generating = false;
            }
        }

        /// <summary>
        /// Calculates the name of the application configuration file.
        /// </summary>
        /// <param name="layer">The layer.</param>
        /// <returns></returns>
        private static string CalculateApplicationConfigurationFileName(Layer layer)
        {
            return layer != null && layer.HostingContext == HostingContext.Web ? "web.config" : "app.config";
        }

        /// <summary>
        /// Génération du fichier de config à partir des fichiers de l'utilisateur
        /// </summary>
        /// <param name="layer">The layer.</param>
        /// <param name="project">The project.</param>
        /// <param name="projectFolder">The project folder.</param>
        /// <param name="forceGeneration">if set to <c>true</c> [force generation].</param>
        /// <returns></returns>
        private string GenerateConfigurationFile(Layer layer, Project project, string projectFolder,
                                                 bool forceGeneration)
        {
            if (project == null)
                return null;

            IShellHelper shell = ServiceLocator.Instance.GetService<IShellHelper>();

            // Vérification si le répertoire de config existe
            string configFolderPath = Path.Combine(projectFolder, ConfigFolderName);

            // On s'assure de son intègration dans la solution
            Directory.CreateDirectory(configFolderPath);
            ProjectItem folder = shell.AddFolderToProject(project.ProjectItems, ConfigFolderName);

            // Création des fichiers à personnaliser

            // D'abord le fichier correspondant au mode courant.
            ConfigurationMode mode = new ConfigurationMode(project);
            string currentFileName = CreateCustomConfigurationFile(mode.CurrentMode, configFolderPath, folder);
            // Puis le fichier commun
            string commonFileName = CreateCustomConfigurationFile("Common", configFolderPath, folder);
            if (forceGeneration || File.GetLastWriteTime(commonFileName) > _lastGenerationTime ||
                File.GetLastWriteTime(currentFileName) > _lastGenerationTime)
            {
                // Fichier généré
                string generatedFileName = CalculateApplicationConfigurationFileName(layer);
                if (generatedFileName == null)
                    return configFolderPath;

                if (project != null && shell != null)
                {
                    // Création d'un fichier xml correspondant à la fusion du fichier commun et
                    // du fichier correspondant au contexte courant (debug, release...)

                    // Merge
                    XmlMerger merger = new XmlMerger();
                    XmlDocument customDocument = merger.MergeXml(currentFileName, commonFileName);

                    // Puis merge avec les config du modèle
                    XmlDocument resultDocument = MergeDeclaredConfigurations(customDocument, layer.Component);

                    // Sauvegarde
                    string generatedFilePath = Path.Combine(projectFolder, generatedFileName);
                    Directory.CreateDirectory(Path.GetDirectoryName(generatedFilePath));
                    shell.EnsureCheckout(generatedFilePath);
                    resultDocument.Save(generatedFilePath);
                    shell.AddFileToProject(project, generatedFilePath);
                }
            }
            return configFolderPath;
        }

        /// <summary>
        /// Creates the custom configuration file.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="configFolderPath">The config folder path.</param>
        /// <param name="folder">The folder.</param>
        /// <returns></returns>
        private string CreateCustomConfigurationFile(string name, string configFolderPath, ProjectItem folder)
        {
            IShellHelper shell = ServiceLocator.Instance.GetService<IShellHelper>();
            // Calcule le nom du fichier personnalisable
            string customFileName = String.Concat(name, ".config");
            string customFilePath = Path.Combine(configFolderPath, customFileName);

            XmlDocument xdoc = new XmlDocument();

            // Ajout si n'existe pas
            if (!File.Exists(customFilePath))
            {
                // On regarde si il n'existe pas un template
                bool fileCreated = false;
                string templateFileName = String.Format(_templateFormatName, name);
                RepositoryFile rf =
                    new RepositoryFile(RepositoryCategory.T4Templates, Path.Combine("ConfigFiles", templateFileName));
                try
                {
                    if (rf.Exists())
                    {
                        xdoc.Load(rf.LocalPhysicalPath);
                        fileCreated = true;
                    }
                }
                catch
                {
                }

                // Si on est pas arrivé à en créer un à partir d'un template, on en crée un vide.
                if (!fileCreated)
                    xdoc.LoadXml(@"<?xml version=""1.0"" encoding=""utf-8"" ?><configuration/>");

                xdoc.Save(customFilePath);
                shell.AddFileToFolder(folder, customFilePath);
            }
            else
                xdoc.Load(customFilePath);
            return customFilePath;
        }

        /// <summary>
        /// Merge des fichiers xml
        /// </summary>
        /// <param name="result">Fichier xml initial</param>
        /// <param name="component">The component.</param>
        /// <returns></returns>
        internal static XmlDocument MergeDeclaredConfigurations(XmlDocument result, SoftwareComponent component)
        {
            // Merge des configurations des layers
            foreach (AbstractLayer layer in component.Layers)
            {
                result = MergeConfiguration(result, layer.Configurations);
            }

            // Récupération des configurations des librairies externes
            ReferenceWalker walker = new ReferenceWalker(ReferenceScope.Runtime, new ConfigurationMode());
            ConfigurationVisitor visitor = new ConfigurationVisitor(false);
            walker.Traverse(visitor, component.Model);
            result = MergeConfiguration(result, visitor.Configurations);

            // Il faut toujours placer la configSection en premier
            XmlNode cfgNode = result.SelectSingleNode("/configuration");
            if (cfgNode != null)
            {
                XmlNode cs = cfgNode.SelectSingleNode("configSections");
                if (cs != null)
                {
                    cfgNode.RemoveChild(cs);
                    cfgNode.InsertAfter(cs, null);
                }
            }
            return result;
        }

        /// <summary>
        /// Merges the configuration.
        /// </summary>
        /// <param name="result">The result.</param>
        /// <param name="configurations">The configurations.</param>
        /// <returns></returns>
        private static XmlDocument MergeConfiguration(XmlDocument result, IList<ConfigurationPart> configurations)
        {
            XmlMerger merger = new XmlMerger();
            foreach (ConfigurationPart cfg in configurations)
            {
                if (cfg.Enabled)
                {
                    XmlDocument xdoc = new XmlDocument();
                    xdoc.LoadXml(cfg.XmlContent);
                    result = merger.MergeXml(result, xdoc);
                }
            }
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string CommitChanges()
        {
            return base.CommitChanges();
        }

        #endregion

        #region IVsRunningDocTableEvents Members

        /// <summary>
        /// Called when [after attribute change].
        /// </summary>
        /// <param name="docCookie">The doc cookie.</param>
        /// <param name="grfAttribs">The GRF attribs.</param>
        /// <returns></returns>
        int IVsRunningDocTableEvents.OnAfterAttributeChange(uint docCookie, uint grfAttribs)
        {
            return VSConstants.S_OK;
        }

        /// <summary>
        /// Called when [after document window hide].
        /// </summary>
        /// <param name="docCookie">The doc cookie.</param>
        /// <param name="pFrame">The p frame.</param>
        /// <returns></returns>
        int IVsRunningDocTableEvents.OnAfterDocumentWindowHide(uint docCookie, IVsWindowFrame pFrame)
        {
            return VSConstants.S_OK;
        }

        /// <summary>
        /// Called when [after first document lock].
        /// </summary>
        /// <param name="docCookie">The doc cookie.</param>
        /// <param name="dwRDTLockType">Type of the dw RDT lock.</param>
        /// <param name="dwReadLocksRemaining">The dw read locks remaining.</param>
        /// <param name="dwEditLocksRemaining">The dw edit locks remaining.</param>
        /// <returns></returns>
        int IVsRunningDocTableEvents.OnAfterFirstDocumentLock(uint docCookie, uint dwRDTLockType,
                                                              uint dwReadLocksRemaining, uint dwEditLocksRemaining)
        {
            return VSConstants.S_OK;
        }

        /// <summary>
        /// Called when [after save].
        /// </summary>
        /// <param name="docCookie">The doc cookie.</param>
        /// <returns></returns>
        int IVsRunningDocTableEvents.OnAfterSave(uint docCookie)
        {
            if (_generating)
                return VSConstants.S_OK;

            // Pour éviter de boucler, on ne regénére pas si la dernière génération s'est faite dans les dernières 2 sec
            TimeSpan ts = DateTime.Now - _lastGenerationTime;
            if (ts.TotalSeconds <= 2)
                return VSConstants.S_OK;

            IVsRunningDocumentTable rdtEvents = GetService<IVsRunningDocumentTable>(typeof (SVsRunningDocumentTable));
            uint pgrfRDTFlags;
            uint pdwReadLocks;
            uint pdwEditLocks;
            string filePath;
            IVsHierarchy ppHier;
            IntPtr ppunkDocData;
            uint itemId;
            if (
                rdtEvents.GetDocumentInfo(docCookie, out pgrfRDTFlags, out pdwReadLocks, out pdwEditLocks, out filePath,
                                          out ppHier, out itemId, out ppunkDocData) == VSConstants.S_OK &&
                filePath != null)
            {
                string folderPath = Path.GetDirectoryName(filePath).ToLower();

                if (!_configFolders.Contains(folderPath) || Path.GetExtension(filePath).ToLower() != ".config")
                    return VSConstants.S_OK;

                try
                {
                    _generating = true;

                    // TODO recherche le modele à partir d'autre chose que la RDT car il faut que ça marche
                    // même si le modèle n'a pas été ouvert.
                    IShellHelper shell = GetService<IShellHelper>();
                    if (shell != null)
                    {
                        CandleModel model = CandleModel.GetModelFromCurrentSolution();
                        if (model != null && model.SoftwareComponent != null)
                        {
                            foreach (LayerPackage lp in model.SoftwareComponent.LayerPackages)
                            {
                                if (lp.IsHigherLevel())
                                {
                                    foreach (Layer layer in lp.Layers)
                                    {
                                        Project project = shell.FindProjectByName(layer.VSProjectName);
                                        if (project != null)
                                        {
                                            string projectFolder = Path.GetDirectoryName(project.FileName);
                                            GenerateConfigurationFile(layer, project, projectFolder, false);
                                        }
                                    }
                                    break;
                                }
                            }
                        }
                    }
                }
                catch (Exception)
                {
                    return VSConstants.S_FALSE;
                }
                finally
                {
                    _lastGenerationTime = DateTime.Now;
                    _generating = false;
                }
            }

            return VSConstants.S_OK;
        }

        /// <summary>
        /// Called when [before document window show].
        /// </summary>
        /// <param name="docCookie">The doc cookie.</param>
        /// <param name="fFirstShow">The f first show.</param>
        /// <param name="pFrame">The p frame.</param>
        /// <returns></returns>
        int IVsRunningDocTableEvents.OnBeforeDocumentWindowShow(uint docCookie, int fFirstShow, IVsWindowFrame pFrame)
        {
            return VSConstants.S_OK;
        }

        /// <summary>
        /// Called when [before last document unlock].
        /// </summary>
        /// <param name="docCookie">The doc cookie.</param>
        /// <param name="dwRDTLockType">Type of the dw RDT lock.</param>
        /// <param name="dwReadLocksRemaining">The dw read locks remaining.</param>
        /// <param name="dwEditLocksRemaining">The dw edit locks remaining.</param>
        /// <returns></returns>
        int IVsRunningDocTableEvents.OnBeforeLastDocumentUnlock(uint docCookie, uint dwRDTLockType,
                                                                uint dwReadLocksRemaining, uint dwEditLocksRemaining)
        {
            return VSConstants.S_OK;
        }

        #endregion

        /// <summary>
        /// Chargement de la stratégie. On recherche les répertoires de config
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        public override void OnLoading(object sender, EventArgs e)
        {
            base.OnLoading(sender, e);

            // Recherche si il existe un répertoire de config
            FindConfigFolderVisitor visitor = new FindConfigFolderVisitor(this);
            VSHierarchyWalker walker = new VSHierarchyWalker(visitor);
            walker.Traverse(ServiceLocator.Instance);
        }

        /// <summary>
        /// Advises the file events.
        /// </summary>
        /// <param name="folderPath">The folder path.</param>
        private void AdviseFileEvents(string folderPath)
        {
            // Abonnement aux événements de modif
            if (_rdtEventsCookie == 0)
            {
                IVsRunningDocumentTable rdtEvents =
                    GetService<IVsRunningDocumentTable>(typeof (SVsRunningDocumentTable));
                if (rdtEvents != null)
                    rdtEvents.AdviseRunningDocTableEvents(this, out _rdtEventsCookie);
            }

            folderPath = folderPath.ToLower().TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);

            // Stockage des répertoires de config
            if (!_configFolders.Contains(folderPath))
                _configFolders.Add(folderPath);
        }

        /// <summary>
        /// Appelée lors du déchargement de la stratégie
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="eventArgs"></param>
        public override void OnUnloading(object sender, EventArgs eventArgs)
        {
            base.OnUnloading(sender, eventArgs);

            IVsRunningDocumentTable rdtEvents = GetService<IVsRunningDocumentTable>(typeof (SVsRunningDocumentTable));
            if (rdtEvents != null && _rdtEventsCookie != 0)
            {
                try
                {
                    rdtEvents.UnadviseRunningDocTableEvents(_rdtEventsCookie);
                }
                catch
                {
                }
                _configFolders.Clear();
            }
        }

        /// <summary>
        /// Destructeur
        /// </summary>
        /// <param name="disposing">false si cette méthode est appelée par le finaliseur, true sinon</param>
        /// <remarks>
        /// Si disposing est a true, on ne doit supprimer que les ressources non gérées par le finaliseur sinon il
        /// faut tout supprimer.
        /// </remarks>
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            OnUnloading(this, new EventArgs());
        }

        #region Nested type: FindConfigFolderVisitor

        /// <summary>
        /// 
        /// </summary>
        private class FindConfigFolderVisitor : IVSHierarchyVisitor
        {
            /// <summary>
            /// 
            /// </summary>
            private ConfigurationFileStrategy strategy;

            public FindConfigFolderVisitor(ConfigurationFileStrategy strategy)
            {
                this.strategy = strategy;
            }

            #region IVSHierarchyVisitor Members

            /// <summary>
            /// Accepts the specified project item.
            /// </summary>
            /// <param name="projectItem">The project item.</param>
            void IVSHierarchyVisitor.Accept(ProjectItem projectItem)
            {
                if (projectItem != null && projectItem.FileCount > 0 &&
                    projectItem.Kind == Constants.vsProjectItemKindPhysicalFolder)
                {
                    string folderName = projectItem.get_FileNames(1);
                    string[] parts = folderName.Split(pathSeparators, StringSplitOptions.RemoveEmptyEntries);
                    if (parts.Length > 0 &&
                        Utils.StringCompareEquals(parts[parts.Length - 1], strategy.ConfigFolderName))
                    {
                        strategy.AdviseFileEvents(folderName);
                        return;
                    }
                }
            }

            #endregion
        }

        #endregion
    }

    /// <summary>
    /// Classe utilitaire permettant de fusionner 2 fichiers XML.
    /// La fusion fonctionne en ajoutant les noeuds du fichiers secondaires dans le 
    /// fichier principal si ils n'existent pas. Si les noeuds existent dans les 2 fichiers,
    /// on fusionne les attributs (en cas de conflit c'est la valeur du fichier maitre qui
    /// est conservée)
    /// </summary>
    public sealed class XmlMerger
    {
        private int prefixSequence;

        /// <summary>
        /// Fusion des fichiers XML
        /// </summary>
        /// <param name="masterFile">The master file.</param>
        /// <param name="secondaryFile">The secondary file.</param>
        /// <returns></returns>
        public XmlDocument MergeXml(string masterFile, string secondaryFile)
        {
            XmlDocument xdoc = new XmlDocument();
            if (File.Exists(masterFile))
                xdoc.Load(masterFile);
            return MergeXml(xdoc, secondaryFile);
        }

        /// <summary>
        /// Merges the XML.
        /// </summary>
        /// <param name="masterDocument">The master document.</param>
        /// <param name="secondaryFile">The secondary file.</param>
        /// <returns></returns>
        public XmlDocument MergeXml(XmlDocument masterDocument, string secondaryFile)
        {
            XmlDocument xdoc2 = new XmlDocument();
            if (File.Exists(secondaryFile))
                xdoc2.Load(secondaryFile);

            return MergeXml(masterDocument, xdoc2);
        }

        /// <summary>
        /// Merges the XML.
        /// </summary>
        /// <param name="masterDocument">The master document.</param>
        /// <param name="secondaryDocument">The secondary document.</param>
        /// <returns></returns>
        public XmlDocument MergeXml(XmlDocument masterDocument, XmlDocument secondaryDocument)
        {
            XmlNamespaceManager nmgr = new XmlNamespaceManager(masterDocument.NameTable);
            MergeNode(nmgr, masterDocument, masterDocument, secondaryDocument,
                      GetXPathNameAndPushContext(nmgr, masterDocument));
            return masterDocument;
        }

        /// <summary>
        /// Gets the X path name and push context.
        /// </summary>
        /// <param name="nmgr">The NMGR.</param>
        /// <param name="node">The node.</param>
        /// <returns></returns>
        private string GetXPathNameAndPushContext(XmlNamespaceManager nmgr, XmlNode node)
        {
            switch (node.NodeType)
            {
                case XmlNodeType.Comment:
                    return "/comment()";
                case XmlNodeType.Document:
                    return "/";
                case XmlNodeType.ProcessingInstruction:
                    return "/processing-instruction()";
                case XmlNodeType.CDATA:
                case XmlNodeType.Text:
                    return "/text()";
                default:
                    if (node.NamespaceURI != String.Empty)
                    {
                        string prefix = node.Prefix;
                        if (prefix == String.Empty)
                            prefix = String.Format("prefix{0}", prefixSequence);
                        nmgr.PushScope();
                        prefixSequence++;
                        nmgr.AddNamespace(prefix, node.NamespaceURI);
                        return String.Concat('/', prefix, ':', node.LocalName);
                    }
                    return String.Concat('/', node.LocalName);
            }
        }

        /// <summary>
        /// Pops the context.
        /// </summary>
        /// <param name="nmgr">The NMGR.</param>
        /// <param name="node">The node.</param>
        private void PopContext(XmlNamespaceManager nmgr, XmlNode node)
        {
            if (node.NamespaceURI != String.Empty)
            {
                nmgr.PopScope();
                prefixSequence--;
            }
        }

        /// <summary>
        /// Merges the node.
        /// </summary>
        /// <param name="nmgr">The NMGR.</param>
        /// <param name="masterParentNode">The master parent node.</param>
        /// <param name="masterNode">The master node.</param>
        /// <param name="secondaryNode">The secondary node.</param>
        /// <param name="parentPath">The parent path.</param>
        private void MergeNode(XmlNamespaceManager nmgr, XmlNode masterParentNode, XmlNode masterNode,
                               XmlNode secondaryNode, string parentPath)
        {
            if (secondaryNode == null ||
                secondaryNode.NodeType == XmlNodeType.XmlDeclaration ||
                secondaryNode.NodeType == XmlNodeType.Entity ||
                secondaryNode.NodeType == XmlNodeType.Notation)
                return;

            // Si le master n'a pas de noeud, on copie l'autre
            if (masterNode == null)
            {
                XmlDocument ownerDocument = masterParentNode.OwnerDocument;
                if (ownerDocument == null) ownerDocument = (XmlDocument) masterParentNode;
                masterNode = masterParentNode.AppendChild(ownerDocument.ImportNode(secondaryNode, true));
                return;
            }
            else
            {
                if (secondaryNode.Attributes != null)
                {
                    // Merge attributes 
                    foreach (XmlAttribute attr2 in secondaryNode.Attributes)
                    {
                        if (masterNode.Attributes[attr2.Name] == null)
                        {
                            masterNode.Attributes.Append(
                                (XmlAttribute) masterNode.OwnerDocument.ImportNode(attr2, false));
                        }
                    }
                }
            }

            if (secondaryNode.HasChildNodes)
            {
                foreach (XmlNode child2 in secondaryNode.ChildNodes)
                {
                    string path = String.Concat(parentPath, GetXPathNameAndPushContext(nmgr, child2));

                    // On regarde si il n'y a pas une clé
                    if (child2.Attributes != null)
                    {
                        // La clé est forcée
                        XmlAttribute keyNameAttr = null;
                        if (masterNode.Attributes != null && masterNode.Attributes.Count > 0)
                            keyNameAttr = masterNode.Attributes["__key"];

                        if (keyNameAttr != null)
                        {
                            PrepareXPath(child2, keyNameAttr.Value, ref path);
                            masterNode.Attributes.Remove(keyNameAttr);
                        }
                        else
                        {
                            // Sinon on essaye de la trouver
                            if (!PrepareXPath(child2, "id", ref path))
                                if (!PrepareXPath(child2, "key", ref path))
                                    PrepareXPath(child2, "name", ref path);
                        }
                    }

                    XmlNode child = masterNode.SelectSingleNode(path, nmgr);
                    MergeNode(nmgr, masterNode, child, child2, path);
                    PopContext(nmgr, child2);
                }
            }
        }

        /// <summary>
        /// Prepares the X path.
        /// </summary>
        /// <param name="child2">The child2.</param>
        /// <param name="keyName">Name of the key.</param>
        /// <param name="path">The path.</param>
        /// <returns></returns>
        private bool PrepareXPath(XmlNode child2, string keyName, ref string path)
        {
            XmlAttribute key = child2.Attributes[keyName];
            if (key != null)
            {
                path = String.Format("{0}[@{1}='{2}']", path, keyName, key.Value);
                return true;
            }
            return false;
        }
    }
}