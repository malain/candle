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
    /// Strat�gie de cr�ation des projets Visual Studio dans la solution courante
    /// </summary>
    [CLSCompliant(false)]
    [Strategy("96290682-B3D7-4bdd-A3F3-15EA0E08AFE2")]
    public class StrategyProjectGenerator : StrategyBase, IStrategyProjectGenerator
    {
        private SoftwareLayer _layer;
        private string _projectTemplate;

        /// <summary>
        /// Nom du template VS � utiliser
        /// </summary>
        /// <remarks>
        /// Si non renseign�, on prend celui d�clarer dans la configuration du langage cible
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
        /// G�n�ration du projet correspondant � cette couche
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

                // Cr�ation du projet si il n'existe pas
                project = ServiceLocator.Instance.ShellHelper.CreateVSProject(folderName, _layer.VSProjectName, templateName);

                if (project != null)
                {
                    ServiceLocator.Instance.ShellHelper.SetProperty(project, "DefaultNamespace", _layer.Namespace);
                    ServiceLocator.Instance.ShellHelper.SetProperty(project, "AssemblyName", _layer.AssemblyName);

                    // Ajout des r�f�rences
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
                // Si il y avait un �l�ment s�lectionn� pour la g�n�ration, il faut quand m�me 
                // initialis� le projet concern�
                project = GetProject();
            }

            //            ShellHelper.SetProjectProperty( project, ConfigurationMode.All, "OutputPath", ResolveRepositoryPath( DSLFactory.Candle.SystemModel.Repository.RepositoryManager.LATEST ) );
            Context.Project = project;
        }

        /// <summary>
        /// Recherche du template de g�n�ration
        /// </summary>
        /// <param name="layer">The layer.</param>
        /// <returns></returns>
        public string GetProjectTemplate(SoftwareLayer layer)
        {
            // Le template de la couche est prioritaire
            if (!string.IsNullOrEmpty(layer.Template))
                return layer.Template;

            // Puis on cherche dans les autres strat�gies
            foreach (StrategyBase strategy in layer.GetStrategies(true))
            {
                if (strategy is IStrategyProvidesProjectTemplates && strategy != this)
                {
                    string projectTemplate = ((IStrategyProvidesProjectTemplates) strategy).GetProjectTemplate(layer);
                    if (!String.IsNullOrEmpty(projectTemplate))
                        return projectTemplate;
                }
            }

            // Puis celui de la strat�gie courante
            if (!String.IsNullOrEmpty(ProjectTemplate))
                return ProjectTemplate;

            // Valeur par d�faut           
            if (layer is Layer && ((Layer) layer).HostingContext == HostingContext.Web)
            {
                return StrategyManager.GetInstance(layer.Store).TargetLanguage.DefaultWebLibraryTemplateName;
            }

            return StrategyManager.GetInstance(layer.Store).TargetLanguage.DefaultLibraryTemplateName;
        }

        /// <summary>
        /// Extension de l'assembly g�n�r�e
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
        /// R�cup�re le projet
        /// </summary>
        /// <returns></returns>
        private Project GetProject()
        {
            return ServiceLocator.Instance.ShellHelper.FindProjectByName(_layer.VSProjectName);
        }
    }
}