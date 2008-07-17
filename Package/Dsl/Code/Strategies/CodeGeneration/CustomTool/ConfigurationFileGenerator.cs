//using System;
//using System.Collections.Generic;
//using System.Text;
//using System.Reflection;
//using Microsoft.VisualStudio.Modeling;
//using Microsoft.VisualStudio.TextTemplating.VSHost;
//using System.IO;
//using EnvDTE;
//using Microsoft.VisualStudio.Shell.Interop;
//using System.Xml;
//using Microsoft.VisualStudio.Shell;
//using Microsoft.VisualStudio.Modeling.Shell;
//using Microsoft.VisualStudio;
//using System.Runtime.InteropServices;

//namespace DSLFactory.Candle.SystemModel.Strategies
//{
//    /// <summary>
//    /// Classe de base pour les templates executés par le <see cref="TTEngine"/>
//    /// </summary>
//    [System.Runtime.InteropServices.Guid("C55A27FD-6389-449f-B92D-E49E84A2191E")]
//    public class ConfigurationFileGenerator : BaseCodeGeneratorWithSite
//    {
//        public const string Name = "CandleConfigurationFileGenerator";

//        protected override byte[] GenerateCode(string inputFileName, string inputFileContent)
//        {
//            ILogger logger = ServiceLocator.Instance.GetService<ILogger>();
//            try
//            {
//                EnvDTE.DTE dte = (EnvDTE.DTE)ModelingPackage.GetGlobalService(typeof(EnvDTE.DTE));
//                Array ary = (Array)dte.ActiveSolutionProjects;
//                if (ary.Length == 0)
//                    return null;

//                EnvDTE.Project project = (EnvDTE.Project)ary.GetValue(0);
//                EnvDTE.ProjectItem item;
//                int iFound = 0;
//                uint itemId = 0;
//                Microsoft.VisualStudio.Shell.Interop.VSDOCUMENTPRIORITY[] pdwPriority = new Microsoft.VisualStudio.Shell.Interop.VSDOCUMENTPRIORITY[1];

//                // obtain a reference to the current project as an IVsProject type
//                Microsoft.VisualStudio.Shell.Interop.IVsProject VsProject = ToVsProject(project);

//                // this locates, and returns a handle to our source file, as a ProjectItem
//                VsProject.IsDocumentInProject(InputFilePath, out iFound, pdwPriority, out itemId);

//                // if our source file was found in the project (which it should have been)
//                if (iFound != 0 && itemId != 0)
//                {
//                    Microsoft.VisualStudio.OLE.Interop.IServiceProvider oleSp = null;
//                    VsProject.GetItemContext(itemId, out oleSp);
//                    if (oleSp != null)
//                    {
//                        ServiceProvider sp = new ServiceProvider(oleSp);
//                        // convert our handle to a ProjectItem
//                        item = sp.GetService(typeof(EnvDTE.ProjectItem)) as EnvDTE.ProjectItem;

//                        if (item.ProjectItems != null && item.ProjectItems.Count > 0)
//                        {
//                            List<ModelingDocData> list = ServiceLocator.Instance.ShellHelper.GetModelingDocDatasFromRunningDocumentTable();
//                            foreach(ModelingDocData doc in list)
//                            {
//                                string name = Path.GetFileNameWithoutExtension(doc.FileName);
//                                if (Utils.StringCompareEquals(name, (string)ServiceLocator.Instance.ShellHelper.Solution.Properties.Item("Name").Value))
//                                {
//                                    CandleModel model = CandleModel.GetInstance(doc.Store);
//                                    string generatedFileName = item.ProjectItems.Item(1).get_FileNames(1);
//                                    ConfigurationFileStrategy.GenerateConfigFile(inputFileName, generatedFileName, model.SoftwareComponent);
//                                    break;
//                                }
//                            }
//                        }
//                    }
//                }
//            }
//            catch(Exception ex)
//            {
//                if (logger != null)
//                    logger.WriteError("Generate config file", "Merge config error", ex);
//            }

//            return null;
//        }

//        public override string GetDefaultExtension()
//        {
//            return ".config";
//        }

//        public static IVsHierarchy ToHierarchy(EnvDTE.Project project)
//        {
//            if (project == null) throw new ArgumentNullException("project"); string projectGuid = null;        // DTE does not expose the project GUID that exists at in the msbuild project file.        // Cannot use MSBuild object model because it uses a static instance of the Engine,         // and using the Project will cause it to be unloaded from the engine when the         // GC collects the variable that we declare.       
//            using (XmlReader projectReader = XmlReader.Create(project.FileName))
//            {
//                projectReader.MoveToContent();
//                object nodeName = projectReader.NameTable.Add("ProjectGuid");
//                while (projectReader.Read())
//                {
//                    if (Object.Equals(projectReader.LocalName, nodeName))
//                    {
//                        projectGuid = (String)projectReader.ReadElementContentAsString(); break;
//                    }
//                }
//            }
//            IServiceProvider serviceProvider = new ServiceProvider(project.DTE as Microsoft.VisualStudio.OLE.Interop.IServiceProvider); 
//            return VsShellUtilities.GetHierarchy(serviceProvider, new Guid(projectGuid));
//        }
//        public static IVsProject ToVsProject(EnvDTE.Project project)
//        {
//            if (project == null) throw new ArgumentNullException("project");
//            IVsProject vsProject = ToHierarchy(project) as IVsProject;
//            if (vsProject == null)
//            {
//                throw new ArgumentException("Project is not a VS project.");
//            }
//            return vsProject;
//        }
//    }

//}
