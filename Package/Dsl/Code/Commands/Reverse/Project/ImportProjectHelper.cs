using System;
using System.IO;
using System.Text;
using System.Windows.Forms;
using DSLFactory.Candle.SystemModel.Commands.Reverse;
using EnvDTE;
using Microsoft.VisualStudio.Modeling;

namespace DSLFactory.Candle.SystemModel.Commands
{
    /// <summary>
    /// Importation d'un projet pour créer un layer
    /// </summary>
    internal class ImportProjectHelper
    {
        /// <summary>
        /// Imports the specified obj.
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <param name="component">The component.</param>
        /// <param name="package">The package.</param>
        public static void Import(object obj, SoftwareComponent component, LayerPackage package)
        {
            MemoryStream ms = obj as MemoryStream;
            if (ms != null)
            {
                string projectInfo = String.Empty;
                String data = Encoding.Unicode.GetString(ms.ToArray());
                StringBuilder sb = new StringBuilder();
                for (int ix = 0; ix < data.Length; ix++)
                {
                    char ch = data[ix];
                    if (ch == 0)
                    {
                        if (sb.Length > 2)
                            projectInfo = sb.ToString();
                        if (sb.Length != 0)
                            sb = new StringBuilder();
                        continue;
                    }
                    sb.Append(ch);
                }

                string[] infos = projectInfo.Split('|');
                //// Comprend rien à la codification des noms de projet.
                //// 
                //// Si contient un ., on prend le nom avant le / sinon c'est un site web, on
                //// prend le nom complet
                //string projectName = infos[1];
                //if (projectName.IndexOf('.') > 0)
                //{
                //    int pos = projectName.LastIndexOf('\\');
                //    projectName = projectName.Substring(0, pos);
                //}

                Project prj = ServiceLocator.Instance.ShellHelper.FindProjectByName(infos[1]);
                if (prj != null)
                {
                    using (Transaction transaction = component.Store.TransactionManager.BeginTransaction("Import layer"))
                    {
                        ImportProjectWizard wizard = new ImportProjectWizard();
                        if (wizard.ShowDialog() == DialogResult.OK)
                        {
                            SoftwareLayer layer = wizard.CreateLayer(component, package);
                            string name = prj.Name;
                            string[] parts = name.Split('\\');
                            if (parts.Length > 1)
                                name = parts[parts.Length - 2];
                            layer.Name = name;
                            layer.VSProjectName = name;

                            // Récupération des propriétés
                            for (int i = 0; i < prj.Properties.Count; i++)
                            {
                                EnvDTE.Property prop = prj.Properties.Item(i + 1);
                                if (prop.Name == "RootNamespace")
                                    layer.Namespace = prop.Value as string;
                                else if (prop.Name == "OutputFileName")
                                    layer.AssemblyName = System.IO.Path.GetFileNameWithoutExtension(prop.Value as string);
                            }
                        }
                        transaction.Commit();
                    }
                }
            }
        }

    }
}
