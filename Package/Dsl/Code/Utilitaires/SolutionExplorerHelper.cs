using System;
using System.IO;
using System.Text;
using System.Windows.Forms;
using EnvDTE;
using Microsoft.VisualStudio.Modeling;

namespace DSLFactory.Candle.SystemModel.Code.Utilitaires
{
    /// <summary>
    /// Classe permettant de renvoyer le projet déposé sur le designer de visual studio lors
    /// d'un drag and drop
    /// </summary>
    internal static class SolutionExplorerHelper
    {
        /// <summary>
        /// Retourne le projet référencé par un drag an drop à partir de l'explorateur de solution
        /// </summary>
        /// <param name="dataObject">The data object.</param>
        /// <param name="store">The store.</param>
        /// <returns></returns>
        public static Project GetProjectReferenceFromDragAndDrop(IDataObject dataObject, Store store)
        {
            if (dataObject.GetDataPresent("CF_VSREFPROJECTS"))
                return null;
            object obj = dataObject.GetData("CF_VSREFPROJECTS");
            if (obj == null)
                return null;

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
                DTE dte = (DTE) store.GetService(typeof (DTE));
                return dte.Solution.Item(infos[1]);
            }
            return null;
        }
    }
}