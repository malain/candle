using System;
using System.Collections.Generic;
namespace DSLFactory.Candle.SystemModel
{
    /// <summary>
    /// Interface permettant de manipuler le contenu d'une solution visual studio
    /// </summary>
    public interface IShellHelper
    {
        /// <summary>
        /// Ajoute un nouveau modèle pour la solution actuelle
        /// </summary>
        /// <param name="modelTemplate">The model template.</param>
        /// <param name="strategyTemplate">The strategy template.</param>
        /// <param name="solutionName">Name of the solution.</param>
        /// <param name="showDialog">if set to <c>true</c> [show dialog].</param>
        void AddDSLModelToSolution(string modelTemplate, string strategyTemplate, string solutionName, bool showDialog);
        /// <summary>
        /// Effectue un check-out avant écriture dans un fichier.
        /// </summary>
        /// <param name="modelFileName"></param>
        void EnsureCheckout(string modelFileName);
        /// <summary>
        /// Adds the file to folder.
        /// </summary>
        /// <param name="folder">The folder.</param>
        /// <param name="filePath">The file path.</param>
        /// <returns></returns>
        EnvDTE.ProjectItem AddFileToFolder(EnvDTE.ProjectItem folder, string filePath);
        /// <summary>
        /// Adds the file to project.
        /// </summary>
        /// <param name="prj">The PRJ.</param>
        /// <param name="filePath">The file path.</param>
        /// <returns></returns>
        EnvDTE.ProjectItem AddFileToProject(EnvDTE.Project prj, string filePath);
        /// <summary>
        /// Adds the file to project.
        /// </summary>
        /// <param name="projectName">Name of the project.</param>
        /// <param name="filePath">The file path.</param>
        /// <returns></returns>
        EnvDTE.ProjectItem AddFileToProject(string projectName, string filePath);
        /// <summary>
        /// Adds the file to solution.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <returns></returns>
        EnvDTE.ProjectItem AddFileToSolution(string fileName);
        /// <summary>
        /// Adds the folder to project.
        /// </summary>
        /// <param name="projectItems">The project items.</param>
        /// <param name="folderName">Name of the folder.</param>
        /// <returns></returns>
        EnvDTE.ProjectItem AddFolderToProject(EnvDTE.ProjectItems projectItems, string folderName);
        /// <summary>
        /// Adds the folder to solution.
        /// </summary>
        /// <param name="folderName">Name of the folder.</param>
        /// <returns></returns>
        EnvDTE.Project AddFolderToSolution(string folderName);
        /// <summary>
        /// Adds the project references to project.
        /// </summary>
        /// <param name="project">The project.</param>
        /// <param name="projects">The projects.</param>
        void AddProjectReferencesToProject(EnvDTE.Project project, System.Collections.Generic.List<string> projects);
        /// <summary>
        /// Adds the reference to project.
        /// </summary>
        /// <param name="project">The project.</param>
        /// <param name="assemblyName">Name of the assembly.</param>
        void AddReferenceToProject(EnvDTE.Project project, string assemblyName);
        /// <summary>
        /// Associates the source files.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="associateFileName">Name of the associate file.</param>
        void AssociateSourceFiles(string fileName, string associateFileName);
        /// <summary>
        /// Builds the project.
        /// </summary>
        /// <param name="project">The project.</param>
        void BuildProject(EnvDTE.Project project);
        //  void ClearAssociatedSourceFiles(string fileName);
        /// <summary>
        /// Creates the VS project.
        /// </summary>
        /// <param name="folderName">Name of the folder.</param>
        /// <param name="projectName">Name of the project.</param>
        /// <param name="templateName">Name of the template.</param>
        /// <returns></returns>
        EnvDTE.Project CreateVSProject(string folderName, string projectName, string templateName);
        /// <summary>
        /// Suppression d'un fichier dans la solution
        /// </summary>
        /// <param name="projectItems">null pour un fichier dans la solution</param>
        /// <param name="filePath">Chemin complet du fichier à supprimer</param>
        void DeleteFileFromProject(EnvDTE.ProjectItems projectItems, string filePath);
        /// <summary>
        /// Ensures the child project item.
        /// </summary>
        /// <param name="parentHierarchy">The parent hierarchy.</param>
        /// <param name="parentItemId">The parent item id.</param>
        /// <param name="fileName">Name of the file.</param>
        void EnsureChildProjectItem(Microsoft.VisualStudio.Shell.Interop.IVsHierarchy parentHierarchy, uint parentItemId, string fileName);
        /// <summary>
        /// Ensures the document open.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="editorFactoryGuid">The editor factory GUID.</param>
        /// <returns></returns>
        EnvDTE.ProjectItem EnsureDocumentOpen(string path, Guid editorFactoryGuid);
        /// <summary>
        /// Executes the wizard.
        /// </summary>
        /// <param name="folder">The folder.</param>
        /// <param name="projectName">Name of the project.</param>
        /// <param name="projectPath">The project path.</param>
        /// <param name="wizardName">Name of the wizard.</param>
        /// <returns></returns>
        EnvDTE.Project ExecuteWizard(EnvDTE80.SolutionFolder folder, string projectName, string projectPath, string wizardName);
        /// <summary>
        /// Finds the document in RDT.
        /// </summary>
        /// <param name="fullPath">The full path.</param>
        /// <param name="hierarchy">The hierarchy.</param>
        /// <param name="itemid">The itemid.</param>
        /// <param name="docCookie">The doc cookie.</param>
        /// <returns></returns>
        object FindDocumentInRDT(string fullPath, out Microsoft.VisualStudio.Shell.Interop.IVsHierarchy hierarchy, out uint itemid, out uint docCookie);
        /// <summary>
        /// Finds the or create project item with template.
        /// </summary>
        /// <param name="projectItems">The project items.</param>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="languageName">Name of the language.</param>
        /// <param name="templateName">Name of the template.</param>
        /// <returns></returns>
        EnvDTE.ProjectItem FindOrCreateProjectItemWithTemplate(EnvDTE.ProjectItems projectItems, string fileName, string languageName, string templateName);
        /// <summary>
        /// Finds the name of the project by.
        /// </summary>
        /// <param name="projectName">Name of the project.</param>
        /// <returns></returns>
        EnvDTE.Project FindProjectByName(string projectName);
        /// <summary>
        /// Finds the project in folder.
        /// </summary>
        /// <param name="folder">The folder.</param>
        /// <param name="projectItemName">Name of the project item.</param>
        /// <returns></returns>
        EnvDTE.Project FindProjectInFolder(EnvDTE.Project folder, string projectItemName);
        /// <summary>
        /// Finds the name of the project item by file.
        /// </summary>
        /// <param name="projectItems">The project items.</param>
        /// <param name="fileName">Name of the file.</param>
        /// <returns></returns>
        EnvDTE.ProjectItem FindProjectItemByFileName(EnvDTE.ProjectItems projectItems, string fileName);
        /// <summary>
        /// Gets the file code model.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <returns></returns>
        EnvDTE.FileCodeModel GetFileCodeModel(string fileName);
        //string GetFileName(EnvDTE.Project prj, string fullName, string fileNameWithoutExtension);
        /// <summary>
        /// Gets the global property.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        string GetGlobalProperty(string name);
        /// <summary>
        /// Reloads the document.
        /// </summary>
        /// <param name="modelFileName">Name of the model file.</param>
        void ReloadDocument(string modelFileName);
        /// <summary>
        /// Sets the project property.
        /// </summary>
        /// <param name="prj">The PRJ.</param>
        /// <param name="mode">The mode.</param>
        /// <param name="propertyName">Name of the property.</param>
        /// <param name="value">The value.</param>
        void SetProjectProperty(EnvDTE.Project prj, ConfigurationMode mode, string propertyName, string value);
        /// <summary>
        /// Sets the property.
        /// </summary>
        /// <param name="project">The project.</param>
        /// <param name="propertyName">Name of the property.</param>
        /// <param name="propertyValue">The property value.</param>
        void SetProperty(EnvDTE.Project project, string propertyName, string propertyValue);
        /// <summary>
        /// Gets the solution.
        /// </summary>
        /// <value>The solution.</value>
        EnvDTE.Solution Solution { get; }
        /// <summary>
        /// Gets the solution folder.
        /// </summary>
        /// <value>The solution folder.</value>
        string SolutionFolder { get; }
        /// <summary>
        /// Tries to show.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="className">Name of the class.</param>
        /// <param name="operationName">Name of the operation.</param>
        void TryToShow(string fileName, string className, string operationName);
        /// <summary>
        /// Gets all projects.
        /// </summary>
        /// <value>All projects.</value>
        IEnumerable<EnvDTE.Project> AllProjects { get;}
        /// <summary>
        /// Unadvises the document event.
        /// </summary>
        /// <param name="fullPath">The full path.</param>
        void UnadviseDocumentEvent(string fullPath);
        /// <summary>
        /// Suspends the file change.
        /// </summary>
        /// <param name="suspend">if set to <c>true</c> [suspend].</param>
        /// <param name="fileName">Name of the file.</param>
        void SuspendFileChange(bool suspend, string fileName);
        /// <summary>
        /// Saves if dirty.
        /// </summary>
        /// <param name="modelFileName">Name of the model file.</param>
        void SaveIfDirty(string modelFileName);
        /// <summary>
        /// Open a brownser and navigates the specified URL.
        /// </summary>
        /// <param name="url">The URL.</param>
        void Navigate(string url);

        /// <summary>
        /// Gets the modeling doc datas from running document table.
        /// </summary>
        /// <returns></returns>
        List<Microsoft.VisualStudio.Modeling.Shell.ModelingDocData> GetModelingDocDatasFromRunningDocumentTable();

        /// <summary>
        /// Gets the name of the solution associated model.
        /// </summary>
        /// <returns></returns>
        string GetSolutionAssociatedModelName();

        /// <summary>
        /// Ensures the document is not in RDT.
        /// </summary>
        /// <param name="_fileName">Name of the _file.</param>
        void EnsureDocumentIsNotInRDT(string _fileName);

        /// <summary>
        /// Finds the name of the project by assembly.
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        EnvDTE.Project FindProjectByAssemblyName(string p);
    }
}
