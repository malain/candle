using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using DSLFactory.Candle.SystemModel.Strategies;
using EnvDTE;
using EnvDTE80;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Modeling.Shell;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using VSLangProj;
using VsWebSite;
using Constants=EnvDTE.Constants;
using IServiceProvider=Microsoft.VisualStudio.OLE.Interop.IServiceProvider;

namespace DSLFactory.Candle.SystemModel.CodeGeneration
{
    /// <summary>
    /// Classe permettant de manipuler la solution courante
    /// </summary>
    [CLSCompliant(false)]
    public class ShellHelper : IShellHelper
    {
        private DTE _dte;

        /// <summary>
        /// Initializes a new instance of the <see cref="ShellHelper"/> class.
        /// </summary>
        internal ShellHelper()
        {
            EnsureDte();
        }

        #region IShellHelper Members

        /// <summary>
        /// Sets the project property.
        /// </summary>
        /// <param name="prj">The PRJ.</param>
        /// <param name="mode">The mode.</param>
        /// <param name="propertyName">Name of the property.</param>
        /// <param name="value">The value.</param>
        public void SetProjectProperty(Project prj, ConfigurationMode mode, string propertyName, string value)
        {
            for (int idx = 0; idx < prj.ConfigurationManager.Count; idx++)
            {
                EnvDTE.Configuration conf = prj.ConfigurationManager.Item(idx + 1, null);
                if (mode.CheckConfigurationMode(conf.ConfigurationName))
                {
                    EnvDTE.Property property = conf.Properties.Item(propertyName);
                    property.Value = value;
                }
            }
        }


        /// <summary>
        /// Ensures the document is open.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <param name="editorFactoryGuid">The editor factory GUID.</param>
        /// <returns></returns>
        public ProjectItem EnsureDocumentOpen(string path, Guid editorFactoryGuid)
        {
            IServiceProvider provider;
            IVsWindowFrame frame;
            IVsUIHierarchy hierarchy = null;
            uint itemID = 0;
            Guid logicalViewGuid = Guid.Empty;
            IVsUIShellOpenDocument document =
                (IVsUIShellOpenDocument) ModelingPackage.GetGlobalService(typeof (IVsUIShellOpenDocument));
            if (
                ErrorHandler.Succeeded(
                    document.OpenDocumentViaProjectWithSpecific(path, 3, ref editorFactoryGuid, String.Empty,
                                                                ref logicalViewGuid, out provider, out hierarchy,
                                                                out itemID, out frame)) && frame != null &&
                !ErrorHandler.Failed(frame.ShowNoActivate()))
            {
                ErrorHandler.ThrowOnFailure(frame.Show());
            }

            object obj = null;
            if (hierarchy != null && (hierarchy.GetProperty(itemID, -2027, out obj) == 0) && (obj != null))
            {
                return obj as ProjectItem;
            }
            return null;
        }

        /// <summary>
        /// Finds the document in RDT.
        /// </summary>
        /// <param name="fullPath">The full path.</param>
        /// <param name="hierarchy">The hierarchy.</param>
        /// <param name="itemid">The itemid.</param>
        /// <param name="docCookie">The doc cookie.</param>
        /// <returns></returns>
        public object FindDocumentInRDT(string fullPath, out IVsHierarchy hierarchy, out uint itemid, out uint docCookie)
        {
            docCookie = itemid = 0;
            hierarchy = null;
            object obj = null;
            IVsRunningDocumentTable rdt =
                ModelingPackage.GetGlobalService(typeof (SVsRunningDocumentTable)) as IVsRunningDocumentTable;
            if (rdt != null)
            {
                IntPtr ptr = IntPtr.Zero;
                ErrorHandler.ThrowOnFailure(
                    rdt.FindAndLockDocument(0, fullPath, out hierarchy, out itemid, out ptr, out docCookie));
                if (ptr == IntPtr.Zero)
                {
                    return obj;
                }
                try
                {
                    obj = Marshal.GetObjectForIUnknown(ptr);
                }
                finally
                {
                    Marshal.Release(ptr);
                }
            }
            return obj;
        }

        /// <summary>
        /// Unadvises the document event.
        /// </summary>
        /// <param name="fullPath">The full path.</param>
        public void UnadviseDocumentEvent(string fullPath)
        {
            IVsRunningDocumentTable rdt =
                ModelingPackage.GetGlobalService(typeof (SVsRunningDocumentTable)) as IVsRunningDocumentTable;
            if (rdt != null)
            {
                uint docCookie = 0, itemid = 0;
                IVsHierarchy hierarchy = null;
                IntPtr ptr = IntPtr.Zero;
                if (rdt.FindAndLockDocument(0, fullPath, out hierarchy, out itemid, out ptr, out docCookie) ==
                    (int) VSConstants.S_OK)
                    rdt.UnadviseRunningDocTableEvents(docCookie);
            }
        }

        /// <summary>
        /// Tries to show.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="className">Name of the class.</param>
        /// <param name="operationName">Name of the operation.</param>
        public void TryToShow(string fileName, string className, string operationName)
        {
            try
            {
                ProjectItem item = EnsureDocumentOpen(fileName, Guid.Empty);
                    // FindProjectItemByFileName( project.ProjectItems, fileName );
                if (item != null && !String.IsNullOrEmpty(className))
                {
                    CodeClass2 cc = ClassCodeGenerator.FindClass(item.FileCodeModel, className);
                    if (cc == null)
                        return;

                    TextPoint tp = cc.GetStartPoint(vsCMPart.vsCMPartHeader);
                    if (!String.IsNullOrEmpty(operationName))
                    {
                        CodeFunction2 cf = ClassCodeGenerator.FindFunctionCodeElement(cc, operationName, null);
                        if (cf != null)
                        {
                            tp = cf.GetStartPoint(vsCMPart.vsCMPartHeader);
                        }
                    }
                    if (tp != null)
                    {
                        try
                        {
                            TextSelection selection = tp.Parent.Selection;
                            if (selection != null)
                            {
                                selection.MoveToLineAndOffset(tp.Line, 1, false);
                                selection.ActivePoint.TryToShow(vsPaneShowHow.vsPaneShowCentered, null);
                            }
                        }
                        catch
                        {
                        }
                    }
                }
            }
            catch
            {
            }
        }

        /// <summary>
        /// Associates the source files.
        /// </summary>
        /// <param name="fileName">Fichier principal</param>
        /// <param name="associateFileName">Name of the associate file.</param>
        public void AssociateSourceFiles(string fileName, string associateFileName)
        {
            try
            {
                if (!File.Exists(associateFileName))
                    return;

                ServiceProvider serviceProvider = new ServiceProvider((IServiceProvider) _dte);
                IVsUIShellOpenDocument documentService =
                    serviceProvider.GetService(typeof (IVsUIShellOpenDocument)) as IVsUIShellOpenDocument;
                uint itemId;
                IVsUIHierarchy hierarchy = null;
                IntPtr ptr1 = IntPtr.Zero;
                int num2;
                IServiceProvider provider2;
                ErrorHandler.ThrowOnFailure(
                    documentService.IsDocumentInAProject(fileName, out hierarchy, out itemId, out provider2, out num2));
                if ((hierarchy != null) && (itemId > 0))
                {
                    object obj1 = null;
                    if (((hierarchy == null) || !ErrorHandler.Succeeded(hierarchy.GetProperty(itemId, -2027, out obj1))) ||
                        (obj1 == null))
                    {
                        return;
                    }
                    ProjectItem projectItem = obj1 as ProjectItem;
                    if ((projectItem == null) || (projectItem.ProjectItems == null))
                    {
                        return;
                    }
                    try
                    {
                        projectItem.ProjectItems.AddFromFile(associateFileName);
                        return;
                    }
                    catch (COMException)
                    {
                    }
                }
            }
            catch
            {
            }
        }

        //public void ClearAssociatedSourceFiles( string fileName)
        //{
        //    try
        //    {
        //        ServiceProvider serviceProvider = new ServiceProvider( (Microsoft.VisualStudio.OLE.Interop.IServiceProvider)Dte );
        //        IVsUIShellOpenDocument documentService = serviceProvider.GetService( typeof( IVsUIShellOpenDocument ) ) as IVsUIShellOpenDocument;
        //        uint itemId;
        //        IVsUIHierarchy hierarchy = null;
        //        IntPtr ptr1 = IntPtr.Zero;
        //        int num2;
        //        Microsoft.VisualStudio.OLE.Interop.IServiceProvider provider2;
        //        Microsoft.VisualStudio.ErrorHandler.ThrowOnFailure( documentService.IsDocumentInAProject( fileName, out hierarchy, out itemId, out provider2, out num2 ) );
        //        if( ( hierarchy != null ) && ( itemId > 0 ) )
        //        {
        //            object obj = null;
        //            if( ( ( hierarchy == null ) || !ErrorHandler.Succeeded( hierarchy.GetProperty( itemId, -2027, out obj ) ) ) || ( obj == null ) )
        //            {
        //                return;
        //            }
        //            ProjectItem projectItem = obj as ProjectItem;
        //            if( ( projectItem == null ) || ( projectItem.ProjectItems == null ) )
        //            {
        //                return;
        //            }
        //            try
        //            {
        //                for( int i=0; i < projectItem.ProjectItems.Count;i++ )
        //                {
        //                    ProjectItem pi = projectItem.ProjectItems.Item( i+1 );
        //                    if( pi.FileCount > 0)
        //                        Utils.DeleteFile( pi.get_FileNames( 1 ) );
        //                    pi.Remove();
        //                }
        //            }
        //            catch
        //            {
        //            }
        //        }
        //    }
        //    catch { }
        //}


        /// <summary>
        /// Ensures the child project item.
        /// </summary>
        /// <param name="parentHierarchy">The parent hierarchy.</param>
        /// <param name="parentItemId">The parent item id.</param>
        /// <param name="fileName">Name of the file.</param>
        [CLSCompliant(false)]
        public void EnsureChildProjectItem(IVsHierarchy parentHierarchy, uint parentItemId, string fileName)
        {
            if (parentHierarchy == null)
            {
                throw new ArgumentNullException("parentHierarchy");
            }
            if ((parentItemId == uint.MaxValue) || (parentItemId == 0xfffffffe))
            {
                throw new ArgumentOutOfRangeException("parentItemId");
            }
            if (string.IsNullOrEmpty(fileName))
            {
                throw new ArgumentNullException("fileName");
            }
            object obj = null;
            if (((parentHierarchy == null) ||
                 !ErrorHandler.Succeeded(parentHierarchy.GetProperty(parentItemId, -2027, out obj))) || (obj == null))
            {
                return;
            }
            ProjectItem item = obj as ProjectItem;
            if ((item == null) || (item.ProjectItems == null))
            {
                return;
            }
            try
            {
                item.ProjectItems.AddFromFile(fileName);
                return;
            }
            catch (COMException)
            {
            }
        }

        /// <summary>
        /// Gets the global property.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public string GetGlobalProperty(string name)
        {
            if (Solution != null && Solution.Globals.get_VariableExists(name))
                return Solution.Globals[name].ToString();
            return null;
        }

        /// <summary>
        /// Rechargement d'un fichier dans l'éditeur (si il était ouvert)
        /// </summary>
        /// <param name="modelFileName">Nom complet du fichier</param>
        public void ReloadDocument(string modelFileName)
        {
            uint itemId;
            uint docCookie;
            IVsHierarchy ivsHierarchy;
            IVsPersistDocData2 docdata =
                FindDocumentInRDT(modelFileName, out ivsHierarchy, out itemId, out docCookie) as IVsPersistDocData2;
            int isReloadable;
            if (docdata != null)
            {
                docdata.IsDocDataReloadable(out isReloadable);
                if (isReloadable != 0)
                    docdata.ReloadDocData((uint) 0);
            }
        }

        /// <summary>
        /// Saves if dirty.
        /// </summary>
        /// <param name="modelFileName">Name of the model file.</param>
        public void SaveIfDirty(string modelFileName)
        {
            uint itemId;
            uint docCookie;
            IVsHierarchy ivsHierarchy;
            IVsPersistDocData2 docdata =
                FindDocumentInRDT(modelFileName, out ivsHierarchy, out itemId, out docCookie) as IVsPersistDocData2;
            if (docdata != null)
            {
                int pfDirty;
                docdata.IsDocDataDirty(out pfDirty);

                if (pfDirty != 0)
                {
                    EnsureCheckout(modelFileName);

                    int pfSaveCanceled;
                    string newDocumentName;
                    docdata.SaveDocData(VSSAVEFLAGS.VSSAVE_SilentSave, out newDocumentName, out pfSaveCanceled);
                }
            }
        }

        /// <summary>
        /// Suspends the file change.
        /// </summary>
        /// <param name="suspend">if set to <c>true</c> [suspend].</param>
        /// <param name="fileName">Name of the file.</param>
        public void SuspendFileChange(bool suspend, string fileName)
        {
            return;
            IVsHierarchy hierarchy;
            uint itemId;
            uint docCookie;
            IntPtr docData = IntPtr.Zero;

            ServiceProvider serviceProvider = new ServiceProvider((IServiceProvider) _dte);

            try
            {
                IVsRunningDocumentTable rdt =
                    serviceProvider.GetService(typeof (SVsRunningDocumentTable)) as IVsRunningDocumentTable;
                if (rdt == null) return;
                ErrorHandler.ThrowOnFailure(
                    rdt.FindAndLockDocument((uint) _VSRDTFLAGS.RDT_NoLock, fileName, out hierarchy, out itemId,
                                            out docData, out docCookie));
                if ((docCookie == (uint) 0) || docData == IntPtr.Zero)
                    return;

                IVsFileChangeEx fileChangeService =
                    serviceProvider.GetService(typeof (IVsFileChangeEx)) as IVsFileChangeEx;
                if (fileChangeService != null)
                {
                    if (docData != IntPtr.Zero)
                    {
                        IVsPersistDocData persistDocData = null;

                        // if interface is not supported, return null
                        object unknown = Marshal.GetObjectForIUnknown(docData);
                        if (unknown is IVsPersistDocData)
                        {
                            persistDocData = (IVsPersistDocData) unknown;
                            if (persistDocData is IVsDocDataFileChangeControl)
                            {
                                IVsDocDataFileChangeControl fileChangeControl =
                                    (IVsDocDataFileChangeControl) persistDocData;

                                if (!suspend)
                                {
                                    ErrorHandler.ThrowOnFailure(fileChangeService.SyncFile(fileName));
                                    ErrorHandler.ThrowOnFailure(fileChangeService.IgnoreFile(0, fileName, 0));
                                    if (fileChangeControl != null)
                                    {
                                        ErrorHandler.ThrowOnFailure(fileChangeControl.IgnoreFileChanges(0));
                                    }
                                }
                                else
                                {
                                    ErrorHandler.ThrowOnFailure(fileChangeService.IgnoreFile(0, fileName, (int) 1));
                                    if (fileChangeControl != null)
                                    {
                                        ErrorHandler.ThrowOnFailure(fileChangeControl.IgnoreFileChanges(1));
                                    }
                                }
                            }
                        }
                    }
                }
            }
            finally
            {
                if (docData != IntPtr.Zero)
                {
                    Marshal.Release(docData);
                }
            }
        }

        /// <summary>
        /// Ensures the file is checkout.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        public void EnsureCheckout(string fileName)
        {
            if (!File.Exists(fileName) || Solution == null || !Solution.IsOpen)
                return;

            // SCC
            IVsQueryEditQuerySave2 scc =
                ServiceLocator.Instance.GetService<IVsQueryEditQuerySave2>(typeof (SVsQueryEditQuerySave));
            if (scc != null)
            {
                uint result;
                uint info;
                string[] files = new string[1];
                files[0] = fileName;
                scc.QueryEditFiles((uint) tagVSQueryEditFlags.QEF_ForceEdit_NoPrompting, 1, files, null, null,
                                   out result, out info);
            }
            File.SetAttributes(fileName, FileAttributes.Normal);
        }


        /// <summary>
        /// Open a brownser and navigates the specified URL.
        /// </summary>
        /// <param name="url">The URL.</param>
        public void Navigate(string url)
        {
            if (Solution != null)
                Solution.DTE.ItemOperations.Navigate(url, vsNavigateOptions.vsNavigateOptionsNewWindow);
        }

        /// <summary>
        /// Liste de tous les modèles contenues dans la solution courante
        /// </summary>
        /// <returns></returns>
        public List<ModelingDocData> GetModelingDocDatasFromRunningDocumentTable()
        {
            List<ModelingDocData> list = new List<ModelingDocData>();
            uint pceltFetched;
            IVsRunningDocumentTable service =
                (IVsRunningDocumentTable) ModelingPackage.GetGlobalService(typeof (IVsRunningDocumentTable));
            if (service == null)
            {
                return list;
            }
            IEnumRunningDocuments ppenum = null;
            ErrorHandler.ThrowOnFailure(service.GetRunningDocumentsEnum(out ppenum));
            if (ppenum == null)
            {
                return list;
            }
            ErrorHandler.ThrowOnFailure(ppenum.Reset());
            uint[] rgelt = new uint[1];
            while (((ErrorHandler.ThrowOnFailure(ppenum.Next(1, rgelt, out pceltFetched)) == 0) && (pceltFetched == 1)) &&
                   (rgelt[0] != 0))
            {
                uint pgrfRDTFlags;
                uint pdwReadLocks;
                uint pdwEditLocks;
                string pbstrMkDocument;
                IVsHierarchy ppHier;
                uint pitemid;
                IntPtr ppunkDocData;
                ErrorHandler.ThrowOnFailure(
                    service.GetDocumentInfo(rgelt[0], out pgrfRDTFlags, out pdwReadLocks, out pdwEditLocks,
                                            out pbstrMkDocument, out ppHier, out pitemid, out ppunkDocData));
                ModelingDocData item = Marshal.GetObjectForIUnknown(ppunkDocData) as ModelingDocData;
                Marshal.Release(ppunkDocData);
                if ((item != null) && (item.Store != null))
                {
                    list.Add(item);
                }
            }
            return list;
        }

        /// <summary>
        /// Ensures the document is not in RDT.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        public void EnsureDocumentIsNotInRDT(string fileName)
        {
            IVsHierarchy ppHier;
            uint docCookie;
            uint itemid;

            IVsRunningDocumentTable rdt =
                ModelingPackage.GetGlobalService(typeof (SVsRunningDocumentTable)) as IVsRunningDocumentTable;
            if (rdt != null)
            {
                IntPtr ptr = IntPtr.Zero;
                ErrorHandler.ThrowOnFailure(
                    rdt.FindAndLockDocument(0, fileName, out ppHier, out itemid, out ptr, out docCookie));
                if (ptr == IntPtr.Zero || docCookie == 0)
                    return;

                IVsRunningDocumentTable2 rdt2 = (IVsRunningDocumentTable2) rdt;
                if (rdt2 != null)
                {
                    ErrorHandler.ThrowOnFailure(
                        rdt2.CloseDocuments((uint) __FRAMECLOSE.FRAMECLOSE_SaveIfDirty, null, docCookie));
                }
            }
        }

        #endregion

        /// <summary>
        /// Ensures the DTE.
        /// </summary>
        private void EnsureDte()
        {
            _dte = (DTE) ModelingPackage.GetGlobalService(typeof (DTE));
        }

        /// <summary>
        /// Sauvegarde du nom du modèle dans les paramètres globaux de la solution
        /// </summary>
        /// <param name="targetFileName">Name of the target file.</param>
        private void RegisterModelNameInSolution(string targetFileName)
        {
            if (Solution != null)
            {
                Solution.Globals["CandleModel"] = Path.GetFileName(targetFileName);
                Solution.Globals.set_VariablePersists("CandleModel", true);
            }
        }

        #region Manipulations de la solution et des projets

        /// <summary>
        /// Gets the solution.
        /// </summary>
        /// <value>The solution.</value>
        public Solution Solution
        {
            get
            {
                if (_dte == null)
                    EnsureDte();
                return _dte == null ? null : _dte.Solution;
            }
        }

        /// <summary>
        /// Gets the solution folder.
        /// </summary>
        /// <value>The solution folder.</value>
        public string SolutionFolder
        {
            get { return Path.GetDirectoryName((string) Solution.Properties.Item("Path").Value); }
        }

        /// <summary>
        /// Gets the file code model.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <returns></returns>
        public FileCodeModel GetFileCodeModel(string fileName)
        {
            ProjectItem pi = Solution.FindProjectItem(fileName);
            if (pi != null)
                return pi.FileCodeModel;
            return null;
        }

        /// <summary>
        /// Création d'un projet à partir d'un template
        /// </summary>
        /// <param name="folderName">Name of the folder.</param>
        /// <param name="projectName">Name of the project.</param>
        /// <param name="templateName">Name of the template.</param>
        /// <returns></returns>
        public Project CreateVSProject(string folderName, string projectName, string templateName)
        {
            string solutionPath = SolutionFolder;
            string projectPath = Path.Combine(solutionPath, projectName);

            Project folder = null;
            Project project;
            if (!String.IsNullOrEmpty(folderName))
            {
                folder = AddFolderToSolution(folderName);
                project = FindProjectInFolder(folder, projectName);
                if (project == null)
                {
                    if (File.Exists(Path.Combine(projectPath, projectName)))
                        throw new Exception(
                            String.Format(
                                "Unable to create project {0} in folder {1} because it already exists in the solution in another location",
                                projectName, folderName));

                    //try
                    //{
                    //    if (Directory.Exists(projectPath))
                    //        Directory.Delete(projectPath, true);
                    //}
                    //catch
                    //{
                    //    throw new Exception(String.Format("Unable to create project {0} in folder {1} because it already exists in the solution in another location", projectName, folderName));
                    //}

                    // TODO ajout au controle de code source
                    project = ExecuteWizard((SolutionFolder) folder.Object, projectName, projectPath, templateName);
                    project = FindProjectInFolder(folder, projectName); // A revoir
                }
            }
            else
            {
                project = FindProjectByName(projectName);
                if (project == null)
                {
                    //if (Directory.Exists(projectPath))
                    //    Directory.Delete(projectPath, true);

                    project = ExecuteWizard(null, projectName, projectPath, templateName);
                    project = FindProjectByName(projectName);
                }
            }
            //Utils.DeleteFile( Path.Combine( projectPath, "class1.cs" ) );
            return project;
        }

        /// <summary>
        /// Ajout d'un dossier dans un projet
        /// </summary>
        /// <param name="projectItems">The project items.</param>
        /// <param name="folderName">Name of the folder.</param>
        /// <returns></returns>
        public ProjectItem AddFolderToProject(ProjectItems projectItems, string folderName)
        {
            folderName = folderName.Trim();
            foreach (ProjectItem folder in projectItems)
            {
                if (Utils.StringCompareEquals(folder.Name, folderName))
                    return folder;
            }
            try
            {
                return projectItems.AddFromDirectory(folderName);
            }
            catch
            {
                return projectItems.AddFolder(folderName, Constants.vsProjectItemKindPhysicalFolder);
            }
        }

        /// <summary>
        /// Ajout d'un dossier de solution
        /// </summary>
        /// <param name="folderName">Name of the folder.</param>
        /// <returns></returns>
        public Project AddFolderToSolution(string folderName)
        {
            Solution2 sln = (Solution2) Solution;
            folderName = folderName.Trim();

            foreach (Project folder in sln.Projects)
            {
                if (Utils.StringCompareEquals(folder.Name, folderName))
                    return folder;
            }
            Project prj = sln.AddSolutionFolder(folderName);
            return prj;
        }

        /// <summary>
        /// Ajout d'un fichier au niveau de la solution (dans 'Solution Items')
        /// </summary>
        /// <param name="fileName">Chemin physique du fichier (Il ne sera pas copié)</param>
        /// <returns></returns>
        public ProjectItem AddFileToSolution(string fileName)
        {
            // Si le fichier n'est pas dans le répertoire de la solution, on l'ignore
            // Pour gérer le cas de la sauvegarde d'un modèle venant du repository (et se
            // trouvant dans un répertoire temporaire) car on ne veut pas qu'il soit rajouter
            // à la solution (voir CandleDocData.Save)
            // TODO voir peut-être un autre moyen
            try
            {
                if (!Utils.StringStartsWith(Path.GetDirectoryName(fileName), SolutionFolder))
                    return null;

                Solution2 sln = (Solution2) Solution;
                Project folder = FindProjectByName(StrategyManager.ConfigFolder);
                if (folder == null)
                    folder = AddFolderToSolution(StrategyManager.ConfigFolder);
                string fn = Path.GetFileName(fileName);
                foreach (ProjectItem it in folder.ProjectItems)
                {
                    if (Utils.StringCompareEquals(it.Name, fn))
                        return it;
                }

                return folder.ProjectItems.AddFromFile(fileName);
            }
            catch (Exception ex)
            {
                ILogger logger = ServiceLocator.Instance.GetService<ILogger>();
                if (logger != null)
                    logger.WriteError("VSShell", "Add file to solution", ex);
            }
            return null;
        }

        /// <summary>
        /// Liste récursive de tous les projets
        /// </summary>
        /// <value>All projects.</value>
        public IEnumerable<Project> AllProjects
        {
            get
            {
                foreach (Project prj in Solution.Projects)
                {
                    if (prj.ProjectItems != null)
                    {
                        if (prj.Name.IndexOf(".vdproj") < 0)
                            yield return prj;

                        IEnumerable<Project> projets = EnumerateProject(prj);
                        foreach (Project prj2 in projets)
                            yield return prj2;
                    }
                }
            }
        }

        /// <summary>
        /// Recherche d'un projet dans la solution
        /// </summary>
        /// <param name="projectName">Name of the project.</param>
        /// <returns></returns>
        public Project FindProjectByName(string projectName)
        {
            foreach (Project prj in AllProjects)
            {
                string name = GetSimpleNameFromProject(prj);
                if (Utils.StringCompareEquals(name, projectName) ||
                    Utils.StringCompareEquals(prj.UniqueName, projectName))
                    return prj;
            }
            return null;
        }

        /// <summary>
        /// Recherche d'un projet par le nom de l'assembly
        /// </summary>
        /// <param name="assemblyName">Nom de l'assembly</param>
        /// <returns></returns>
        public Project FindProjectByAssemblyName(string assemblyName)
        {
            foreach (Project prj in AllProjects)
            {
                try
                {
                    string name = (string) prj.Properties.Item("OutputFileName").Value;
                    if (Utils.StringCompareEquals(name, assemblyName))
                        return prj;
                }
                catch
                {
                }
            }
            return null;
        }

        /// <summary>
        /// Recherche d'un projet dans la solution récursivement
        /// </summary>
        /// <param name="folder">The folder.</param>
        /// <param name="projectItemName">Name of the project item.</param>
        /// <returns></returns>
        public Project FindProjectInFolder(Project folder, string projectItemName)
        {
            foreach (Project prj in EnumerateProject(folder))
            {
                string name = GetSimpleNameFromProject(prj);
                if (Utils.StringCompareEquals(name, projectItemName))
                    return prj;
            }
            return null;
        }

        /// <summary>
        /// Recherche d'un item récursivement par son nom
        /// </summary>
        /// <param name="projectItems">The project items.</param>
        /// <param name="fileName">Name of the file.</param>
        /// <returns></returns>
        public ProjectItem FindProjectItemByFileName(ProjectItems projectItems, string fileName)
        {
            if (projectItems == null)
                return Solution.FindProjectItem(fileName);

            foreach (ProjectItem item in projectItems)
            {
                if (item.Properties != null)
                {
                    string filePath = (string) item.Properties.Item("FullPath").Value;
                    filePath = filePath.TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
                    if (Utils.StringCompareEquals(filePath, fileName))
                        return item;
                }
                else
                {
                    if (item.FileCount > 0 && Utils.StringCompareEquals(item.get_FileNames(1), fileName))
                        return item;
                }

                if (item.ProjectItems != null && item.ProjectItems.Count > 0)
                {
                    ProjectItem item2 = FindProjectItemByFileName(item.ProjectItems, fileName);
                    if (item2 != null)
                        return item2;
                }
            }
            return null;
        }

        /// <summary>
        /// Builds the project.
        /// </summary>
        /// <param name="project">The project.</param>
        public void BuildProject(Project project)
        {
            SolutionBuild builder = Solution.SolutionBuild;
            if (builder != null)
            {
                string name = builder.ActiveConfiguration.Name;
                builder.BuildProject(name, project.UniqueName, true);
            }
        }

        /// <summary>
        /// Finds the or create project item with template.
        /// </summary>
        /// <param name="projectItems">The project items.</param>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="languageName">Name of the language.</param>
        /// <param name="templateName">Name of the template.</param>
        /// <returns></returns>
        public ProjectItem FindOrCreateProjectItemWithTemplate(ProjectItems projectItems, string fileName,
                                                               string languageName, string templateName)
        {
            ProjectItem pi = FindProjectItemByFileName(projectItems, fileName);
            if (pi != null)
                return pi;

            string templateFile =
                ((Solution2) projectItems.DTE.Solution).GetProjectItemTemplate(templateName, languageName);
            try
            {
                projectItems.AddFromTemplate(templateFile, Path.GetFileName(fileName));
            }
            catch
            {
                try
                {
                    projectItems.AddFromFile(fileName);
                }
                catch
                {
                }
            }
            pi = FindProjectItemByFileName(projectItems, fileName);
            if (pi != null && pi.Document != null)
                pi.Document.Close(vsSaveChanges.vsSaveChangesYes);
            return pi;
        }

        /// <summary>
        /// Ajout d'un fichier à un projet
        /// </summary>
        /// <param name="projectName">Nom du projet</param>
        /// <param name="filePath">Chemin du fichier</param>
        /// <returns></returns>
        public ProjectItem AddFileToProject(string projectName, string filePath)
        {
            return AddFileToProject(FindProjectByName(projectName), filePath);
        }

        /// <summary>
        /// Ajout d'un fichier à un projet
        /// </summary>
        /// <param name="prj">Instance du projet</param>
        /// <param name="filePath">Chemin du fichier</param>
        /// <returns></returns>
        public ProjectItem AddFileToProject(Project prj, string filePath)
        {
            return AddFileToProject(Path.GetDirectoryName(prj.FullName), prj.ProjectItems, filePath);
        }

        /// <summary>
        /// Adds the file to folder.
        /// </summary>
        /// <param name="folder">The folder.</param>
        /// <param name="filePath">The file path.</param>
        /// <returns></returns>
        public ProjectItem AddFileToFolder(ProjectItem folder, string filePath)
        {
            string folderPath;
            ProjectItems items = folder.ProjectItems;
            if (items == null)
            {
                items = ((Project) folder.Object).ProjectItems;
                folderPath = Path.GetDirectoryName(((Project) folder.Object).FullName);
            }
            else
            {
                folderPath = folder.get_FileNames(1);
            }
            return AddFileToProject(folderPath, items, filePath);
        }

        /// <summary>
        /// Suppression d'un fichier dans la solution
        /// </summary>
        /// <param name="projectItems">null pour un fichier dans la solution</param>
        /// <param name="filePath">Chemin complet du fichier à supprimer</param>
        public void DeleteFileFromProject(ProjectItems projectItems, string filePath)
        {
            if (projectItems == null)
            {
                Solution2 sln = (Solution2) Solution;
                Project folder = FindProjectByName(StrategyManager.ConfigFolder);
                if (folder == null)
                    return;
                projectItems = folder.ProjectItems;
            }

            ProjectItem item = FindProjectItemByFileName(projectItems, filePath);
            if (item != null)
            {
                item.Remove();
                Utils.DeleteFile(filePath);
                return;
            }
        }

        /// <summary>
        /// Ajout d'une référence à un projet
        /// </summary>
        /// <param name="project">The project.</param>
        /// <param name="assemblyName">Name of the assembly.</param>
        public void AddReferenceToProject(Project project, string assemblyName)
        {
            string simpleAssemblyName = assemblyName;
            if (Path.IsPathRooted(assemblyName))
            {
                // On ne prend que le nom (GetFileNameWithoutExtension ne marche pas car la dll peut contenir des points)
                int pos =
                    assemblyName.LastIndexOfAny(new char[] {Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar});
                if (pos > 0)
                    simpleAssemblyName = assemblyName.Substring(pos + 1);
                // On enleve l'extension si existe
                if (simpleAssemblyName.EndsWith(".dll", StringComparison.CurrentCultureIgnoreCase))
                    simpleAssemblyName = simpleAssemblyName.Substring(0, simpleAssemblyName.Length - 4);
            }

            VSProject prj = project.Object as VSProject;
            if (prj != null)
            {
                // On supprime la référence si elle existait pour être sur de bien pointer
                // sur la bonne version.
                Reference refToRemove = null;
                foreach (Reference reference in prj.References)
                {
                    // Si les fichiers sont identiques, on ne fait rien
                    if (Utils.StringCompareEquals(reference.Path, assemblyName))
                        return;

                    // Sinon si ils ont les mêmes noms c'est que la version différe. 
                    if (Utils.StringCompareEquals(reference.Name, simpleAssemblyName))
                    {
                        // On ne peut pas supprimer directement l'item d'une itération
                        refToRemove = reference;
                        break;
                    }
                }

                if (refToRemove != null)
                    refToRemove.Remove();

                // On l'ajoute
                ILogger logger = ServiceLocator.Instance.GetService<ILogger>();
                try
                {
                    prj.References.Add(assemblyName);
                }
                catch
                {
                    try
                    {
                        Project prj2 = ServiceLocator.Instance.ShellHelper.FindProjectByName(simpleAssemblyName);
                        if (prj2 != null)
                        {
                            prj.References.AddProject(prj2);
                        }
                        else
                        {
                            prj.References.Add(simpleAssemblyName);
                            if (logger != null)
                                logger.Write("Adding reference",
                                             "Unable to reference assembly : " + assemblyName + " use generic " +
                                             simpleAssemblyName, LogType.Error);
                        }
                    }
                    catch
                    {
                        if (logger != null)
                            logger.Write("Adding reference", "Unable to reference assembly : " + assemblyName,
                                         LogType.Error);
                    }
                }
            }
            else if (project.Object is VSWebSite)
            {
                VSWebSite webSite = project.Object as VSWebSite;

                // On supprime la référence si elle existait pour être sur de bien pointer
                // sur la bonne version.
                AssemblyReference refToRemove = null;
                foreach (AssemblyReference reference in webSite.References)
                {
                    // Si les fichiers sont identiques, on ne fait rien
                    ProjectItem pi =
                        FindProjectItemByFileName(reference.ContainingProject.ProjectItems, reference.FullPath);
                    if (pi != null)
                    {
                        EnvDTE.Property p = pi.Properties.Item("AutoRefreshPath");
                        if (p != null && Utils.StringCompareEquals((string) p.Value, assemblyName))
                            return;
                    }

                    if (Utils.StringCompareEquals(reference.Name, simpleAssemblyName))
                    {
                        // TODO eviter de supprimer si les fichiers ont les mêmes dates
                        // On ne peut pas supprimer directement l'item d'une itération
                        refToRemove = reference;
                        break;
                    }
                }

                if (refToRemove != null)
                    refToRemove.Remove();

                try
                {
                    // On l'ajoute
                    webSite.References.AddFromFile(assemblyName);
                }
                catch
                {
                    try
                    {
                        Project prj2 = ServiceLocator.Instance.ShellHelper.FindProjectByName(simpleAssemblyName);
                        if (prj2 != null)
                            webSite.References.AddFromProject(prj2);
                    }
                    catch
                    {
                    }
                }
            }
        }

        /// <summary>
        /// Ajout des références à un projet
        /// </summary>
        /// <param name="project">The project.</param>
        /// <param name="projects">The projects.</param>
        public void AddProjectReferencesToProject(Project project, List<string> projects)
        {
            VSProject prj = project.Object as VSProject;
            if (prj != null)
            {
                foreach (string projectName in projects)
                {
                    try
                    {
                        Project project2 = FindProjectByName(projectName);
                        if (project != project2)
                        {
                            if (project2 != null)
                            {
                                bool exists = false;
                                foreach (Reference refer in prj.References)
                                {
                                    if (refer.SourceProject != null &&
                                        Utils.StringCompareEquals(refer.SourceProject.UniqueName, project2.UniqueName))
                                    {
                                        exists = true;
                                        break;
                                    }
                                }
                                if (!exists)
                                    prj.References.AddProject(project2);
                            }
                            else
                                AddReferenceToProject(project, projectName);
                        }
                    }
                    catch
                    {
                    }
                }
            }
            else if (project.Object is VSWebSite)
            {
                VSWebSite webSite = project.Object as VSWebSite;
                foreach (string projectName in projects)
                {
                    try
                    {
                        Project project2 = FindProjectByName(projectName);
                        if (project != project2)
                        {
                            if (project2 != null)
                            {
                                bool exists = false;
                                foreach (AssemblyReference aref in webSite.References)
                                {
                                    if (
                                        Utils.StringCompareEquals(aref.ContainingProject.UniqueName, project2.UniqueName))
                                    {
                                        exists = true;
                                        break;
                                    }
                                }
                                if (!exists)
                                    webSite.References.AddFromProject(project2);
                            }
                            else
                            {
                                AddReferenceToProject(project, projectName);
                            }
                        }
                    }
                    catch
                    {
                    }
                }
            }
        }


        /// <summary>
        /// Ajoute un projet en executant un wizard
        /// </summary>
        /// <param name="folder">The folder.</param>
        /// <param name="projectName">Nom du projet</param>
        /// <param name="projectPath">Path ou sera créé le nouveau projet</param>
        /// <param name="wizardName">Nom du wizard</param>
        /// <returns></returns>
        public Project ExecuteWizard(SolutionFolder folder, string projectName, string projectPath, string wizardName)
        {
            ILogger logger = ServiceLocator.Instance.GetService<ILogger>();
            if (String.IsNullOrEmpty(wizardName))
                throw new ArgumentNullException("wizardName");

            Solution2 sln = (Solution2) Solution;
            string templatePath = wizardName;

            // Si le chemin n'est pas absolu
            if (!templatePath.Contains(":"))
            {
                string lang = null; // StrategyManager.GetInstance(clazz.Store).TargetLanguage.Name;
                int pos = wizardName.IndexOfAny(new char[] {'/', '\\'});
                if (pos >= 0)
                {
                    lang = wizardName.Substring(0, pos);
                    wizardName = wizardName.Substring(pos + 1);
                }

                // Recherche du wizard
                try
                {
                    templatePath = sln.GetProjectTemplate(wizardName, lang);
                }
                catch (Exception ex)
                {
                    if (logger != null)
                        logger.WriteError("ExecuteWizard",
                                          "Cannot find template : " + wizardName + " for project " + projectName, ex);
                    return null;
                }
            }

            try
            {
                // Création du projet
                if (folder != null)
                    return folder.AddFromTemplate(templatePath, projectPath, projectName);
                else
                    return sln.AddFromTemplate(templatePath, projectPath, projectName, false);
            }
            catch (Exception ex)
            {
                if (logger != null)
                    logger.WriteError("ExecuteWizard", projectName, ex);
                return FindProjectByName(projectName);
            }
        }

        /// <summary>
        /// Retourne le nom complet du fichier modèle associé à la solution ou null si pas trouvé
        /// </summary>
        /// <returns></returns>
        public string GetSolutionAssociatedModelName()
        {
            string m = GetGlobalProperty("CandleModel");
            if (m != null)
                return Path.Combine(SolutionFolder, m);

            string solutionName = Path.GetFileNameWithoutExtension(Solution.FileName);
            return Path.Combine(SolutionFolder, String.Concat(solutionName, ModelConstants.FileNameExtension));
        }

        /// <summary>
        /// Adds the DSL model to solution.
        /// </summary>
        /// <param name="template">The template.</param>
        /// <param name="strategyTemplate">The strategy template.</param>
        /// <param name="solutionName">Name of the solution.</param>
        /// <param name="showDialog">if set to <c>true</c> [show dialog].</param>
        public void AddDSLModelToSolution(string template, string strategyTemplate, string solutionName, bool showDialog)
        {
            try
            {
                string modelFileName =
                    CreateNewModelHelper.CreateModel(SolutionFolder, solutionName, template, strategyTemplate,
                                                     showDialog);
                ProjectItem fileItem = AddFileToSolution(modelFileName);
                if (fileItem != null)
                {
                    RegisterModelNameInSolution(Path.GetFileName(modelFileName));
                    fileItem.Open("{00000000-0000-0000-0000-000000000000}");
                }
            }
            catch
            {
                throw new CanceledByUser();
            }
        }

        /// <summary>
        /// Mise à jour d'une propriété d'un projet
        /// </summary>
        /// <param name="project">The project.</param>
        /// <param name="propertyName">Name of the property.</param>
        /// <param name="propertyValue">The property value.</param>
        public void SetProperty(Project project, string propertyName, string propertyValue)
        {
            string oldValue = null;
            try
            {
                oldValue = project.Properties.Item(propertyName).Value as string;
            }
            catch
            {
            }

            if (oldValue != propertyValue)
            {
                try
                {
                    project.Properties.Item(propertyName).Value = propertyValue;
                }
                catch
                {
                }
            }
        }

        /// <summary>
        /// Enumerates the project.
        /// </summary>
        /// <param name="project">The project.</param>
        /// <returns></returns>
        public IEnumerable<Project> EnumerateProject(Project project)
        {
            foreach (ProjectItem item in project.ProjectItems)
            {
                Project prj = item.Object as Project;
                if (item.Object is SolutionFolder)
                {
                    foreach (Project prj2 in EnumerateProject(prj))
                        yield return prj2;
                }
                else if (prj != null && prj.Name.IndexOf(".vdproj") < 0)
                {
                    yield return prj;
                }
            }
        }

        /// <summary>
        /// Gets the simple name from project.
        /// </summary>
        /// <param name="prj">The PRJ.</param>
        /// <returns></returns>
        public static string GetSimpleNameFromProject(Project prj)
        {
            string name = prj.Name;
            if (prj.Object is VSWebSite)
            {
                name = ((VSWebSite) prj.Object).Project.Name;
                name = GetSimpleNameForProject(name);
            }
            return name;
        }

        /// <summary>
        /// Gets the simple name for project.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public static string GetSimpleNameForProject(string name)
        {
            string[] parts = name.Split(Path.DirectorySeparatorChar);
            if (parts.Length > 1)
                name = parts[parts.Length - 2];
            return name;
        }

        /// <summary>
        /// Adds the file to project.
        /// </summary>
        /// <param name="folderPath">The folder path.</param>
        /// <param name="projectItems">The project items.</param>
        /// <param name="filePath">The file path.</param>
        /// <returns></returns>
        private ProjectItem AddFileToProject(string folderPath, ProjectItems projectItems, string filePath)
        {
            if (projectItems == null)
                throw new ArgumentNullException("projectItems");

            if (!File.Exists(filePath))
                throw new ArgumentException(
                    String.Format("Unable to add file {0} because it doesn't exist.", Path.GetFileName(filePath)));

            ProjectItem projectItem = FindProjectItemByFileName(projectItems, filePath);
            if (projectItem != null)
                return projectItem;

            // Ajout du fichier
            projectItem = projectItems.AddFromFile(filePath);

            // Force la fermeture (BUG lors de l'ajout l'IDE l'ouvre systèmatiquement)
            if (projectItem != null && projectItem.Document != null)
            {
                projectItem.Document.Close(vsSaveChanges.vsSaveChangesYes);
            }

            return projectItem;
        }

        #endregion
    }
}