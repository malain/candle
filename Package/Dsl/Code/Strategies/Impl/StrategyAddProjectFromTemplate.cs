using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing.Design;
using System.IO;
using DSLFactory.Candle.SystemModel.Editor;
using DSLFactory.Candle.SystemModel.Repository;

namespace DSLFactory.Candle.SystemModel.Strategies
{
    /// <summary>
    /// Stratégie permettant de rajouter un projet à la solution à partir d'un
    /// template. 
    /// </summary>
    [CLSCompliant(false)]
    [Strategy("9FE27A92-4AFF-46ff-BB90-98CC8D8D0A09")]
    public class StrategyAddProjectFromTemplate : StrategyBase, IStrategyProjectGenerator
    {
        private string _folderName;
        private string _projectName;
        private string _projectTemplate;

        /// <summary>
        /// Gets or sets the project template.
        /// </summary>
        /// <value>The project template.</value>
        [Description("Name of the visual studio project template")]
        [Editor(typeof (VSTemplateTypeEditor), typeof (UITypeEditor))]
        public string ProjectTemplate
        {
            [DebuggerStepThrough]
            get { return _projectTemplate; }
            [DebuggerStepThrough]
            set { _projectTemplate = value; }
        }

        /// <summary>
        /// Gets or sets the name of the folder.
        /// </summary>
        /// <value>The name of the folder.</value>
        [Description("Name of the solution folder")]
        public string FolderName
        {
            [DebuggerStepThrough]
            get { return _folderName; }
            [DebuggerStepThrough]
            set { _folderName = value; }
        }

        /// <summary>
        /// Gets or sets the name of the visual studio project.
        /// </summary>
        /// <value>The name of the project.</value>
        [Description("Name of the visual studio project")]
        public string ProjectName
        {
            [DebuggerStepThrough]
            get { return _projectName; }
            [DebuggerStepThrough]
            set { _projectName = value; }
        }

        //[Description("Generated assembly extension")]
        //public virtual string AssemblyExtension
        //{
        //    get { return extension; }
        //    set { extension = value; }
        //}

        #region IStrategyProjectGenerator Members

        /// <summary>
        /// Génération du projet
        /// </summary>
        /// <param name="component">The component.</param>
        public virtual void GenerateVSProject(ICustomizableElement component)
        {
            if (component == null || Context.GenerationPass == GenerationPass.MetaModelUpdate)
                return;

            if (Context.CanGenerate(component.Id))
            {
                // Si le projet existe, on ne fait rien
                if (ServiceLocator.Instance.ShellHelper.FindProjectByName(_projectName) == null)
                {
                    string template = GetProjectTemplate(null);

                    // Si le template n'est pas un fichier .zip, c'est qu'on a utilisé un répertoire temporaire
                    if (Utils.StringCompareEquals(Path.GetExtension(template), ".vstemplate"))
                        Utils.RemoveDirectory(Path.GetDirectoryName(template));
                }
            }
        }

        /// <summary>
        /// Nom du template du projet
        /// </summary>
        /// <param name="layer">The layer.</param>
        /// <returns></returns>
        /// <remarks>
        /// Le template est soit un template standard de VS, soit un template fournit dans la stratégie.
        /// </remarks>
        public string GetProjectTemplate(SoftwareLayer layer)
        {
            string template = MapPath(_projectTemplate);
            if (File.Exists(template))
            {
                // Template fournit avec la stratégie. 
                // On le décompresse dans un dossier temporaire
                string folder = Utils.GetTemporaryFolder();

                // On extrait le ichier de template (.vstemplate)
                RepositoryZipFile zipFile = new RepositoryZipFile(template);
                zipFile.ExtractAll(folder);
                List<string> files = Utils.SearchFile(folder, "*.vstemplate");

                // Si on le trouve, c'est bon
                if (files.Count > 0)
                    return files[0]; // On supprimera le répertoire temporaire aprés

                // On a rien trouvé
                Utils.RemoveDirectory(folder);
            }
            return template;
        }


        /// <summary>
        /// Extension de l'assembly générée
        /// </summary>
        /// <param name="layer"></param>
        /// <returns></returns>
        public string GetAssemblyExtension(SoftwareLayer layer)
        {
            return null; // AssemblyExtension;
        }

        #endregion

        /// <summary>
        /// Permet de valider la saisie des paramètres dans la boite de configuration des stratégies
        /// </summary>
        /// <returns>null si ok ou un message d'erreur</returns>
        public override string CommitChanges()
        {
            if (String.IsNullOrEmpty(_projectTemplate))
                return "ProjectTemplate is required";
            if (String.IsNullOrEmpty(_projectName))
                return "ProjectName is required";
            if (String.IsNullOrEmpty(_folderName))
                return "FolderName is required";

            return base.CommitChanges();
        }
    }
}