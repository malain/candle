using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing.Design;
using DSLFactory.Candle.SystemModel.Dependencies;
using DSLFactory.Candle.SystemModel.Editor;
using EnvDTE;

namespace DSLFactory.Candle.SystemModel.Strategies
{
    /// <summary>
    /// Stratégie de création des projets Visual Studio dans la solution courante
    /// </summary>
    [CLSCompliant(false)]
    [Strategy("96290682-B3D7-4bdd-A3F3-15EA0E08AFE2")]
    public class StrategyProjectGenerator : StrategyBase, IStrategyProjectGenerator
    {
        private SoftwareLayer _layer;
        private string _projectTemplate;

        /// <summary>
        /// Nom du template VS à utiliser
        /// </summary>
        /// <remarks>
        /// Si non renseigné, on prend celui déclarer dans la configuration du langage cible
        /// </remarks>
        [Editor(typeof (VSTemplateTypeEditor), typeof (UITypeEditor))]
        [Description("Name of the visual studio project template")]
        public string ProjectTemplate
        {
            [DebuggerStepThrough]
            get { return _projectTemplate; }
            [DebuggerStepThrough]
            set { _projectTemplate = value; }
        }

        #region IStrategyProjectGenerator Members

        /// <summary>
        /// Génération du projet correspondant à cette couche
        /// </summary>
        /// <param name="elem">The elem.</param>
        public virtual void GenerateVSProject(ICustomizableElement elem)
        {
            _layer = elem as SoftwareLayer;
            if (_layer == null)
                return;

            Project project = null;
            if (Context.CanGenerate(_layer.Id))
            {
                // Nom du dossier qui contient le projet
                string folderName = _layer.GetProjectFolderName();

                // Template du projet
                string templateName = GetProjectTemplate(_layer);

                // Création du projet si il n'existe pas
                project = ServiceLocator.Instance.ShellHelper.CreateVSProject(folderName, _layer.VSProjectName, templateName);

                if (project != null)
                {
                    ServiceLocator.Instance.ShellHelper.SetProperty(project, "DefaultNamespace", _layer.Namespace);
                    ServiceLocator.Instance.ShellHelper.SetProperty(project, "AssemblyName", _layer.AssemblyName);

                    // Ajout des références
                    ServiceLocator.Instance.ShellHelper.AddProjectReferencesToProject(project,
                                                                                      ReferencesHelper.GetReferences(
                                                                                          Context.Mode,
                                                                                          ReferenceScope.Compilation,
                                                                                          ServiceLocator.Instance.
                                                                                              ShellHelper.SolutionFolder,
                                                                                          _layer));

                    if (_layer is Layer && ((Layer) _layer).StartupProject)
                    {
                        ServiceLocator.Instance.ShellHelper.Solution.SolutionBuild.StartupProjects = project.UniqueName;
                    }
                }
            }
            else
            {
                // Si il y avait un élément sélectionné pour la génération, il faut quand même 
                // initialisé le projet concerné
                project = GetProject();
            }

            //            ShellHelper.SetProjectProperty( project, ConfigurationMode.All, "OutputPath", ResolveRepositoryPath( DSLFactory.Candle.SystemModel.Repository.RepositoryManager.LATEST ) );
            Context.Project = project;
        }

        /// <summary>
        /// Recherche du template de génération
        /// </summary>
        /// <param name="layer">The layer.</param>
        /// <returns></returns>
        public string GetProjectTemplate(SoftwareLayer layer)
        {
            // Le template de la couche est prioritaire
            if (!string.IsNullOrEmpty(layer.Template))
                return layer.Template;

            // Puis on cherche dans les autres stratégies
            foreach (StrategyBase strategy in layer.GetStrategies(true))
            {
                if (strategy is IStrategyProvidesProjectTemplates && strategy != this)
                {
                    string projectTemplate = ((IStrategyProvidesProjectTemplates) strategy).GetProjectTemplate(layer);
                    if (!String.IsNullOrEmpty(projectTemplate))
                        return projectTemplate;
                }
            }

            // Puis celui de la stratégie courante
            if (!String.IsNullOrEmpty(ProjectTemplate))
                return ProjectTemplate;

            // Valeur par défaut           
            if (layer is Layer && ((Layer) layer).HostingContext == HostingContext.Web)
            {
                return StrategyManager.GetInstance(layer.Store).TargetLanguage.DefaultWebLibraryTemplateName;
            }

            return StrategyManager.GetInstance(layer.Store).TargetLanguage.DefaultLibraryTemplateName;
        }

        /// <summary>
        /// Extension de l'assembly générée
        /// </summary>
        /// <param name="layer"></param>
        /// <returns></returns>
        public virtual string GetAssemblyExtension(SoftwareLayer layer)
        {
            return
                layer != null && layer is PresentationLayer &&
                ((PresentationLayer) layer).HostingContext == HostingContext.Standalone
                    ? ".exe"
                    : ".dll";
        }

        #endregion

        /// <summary>
        /// Récupère le projet
        /// </summary>
        /// <returns></returns>
        private Project GetProject()
        {
            return ServiceLocator.Instance.ShellHelper.FindProjectByName(_layer.VSProjectName);
        }
    }
}