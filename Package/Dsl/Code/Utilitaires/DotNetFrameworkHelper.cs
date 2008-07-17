using System;
using System.IO;
using DSLFactory.Candle.SystemModel.CodeGeneration;

namespace DSLFactory.Candle.SystemModel.Utilities
{
    /// <summary>
    /// Classe permettant de résoudre la localistion d'une dll du framework par rapport à la version du framework définie dans
    /// le modèle.
    /// </summary>
    internal sealed class DotNetFrameworkHelper
    {
        private static readonly string[] s_frameworkVersions = new string[] {"1.0.0.0", "1.1.0.0", "2.0.0.0"};
        private static readonly string[] s_versionsMapping = new string[] {"1.0.3705", "1.1.4322", "2.0.50727"};

        /// <summary>
        /// Resolves the path.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="assemblyName">Name of the assembly.</param>
        /// <returns></returns>
        public static string ResolvePath(CandleModel model, string assemblyName)
        {
            if (assemblyName.EndsWith(".dll", StringComparison.CurrentCultureIgnoreCase) == false)
                assemblyName += ".dll";

            string version = model.DotNetFrameworkVersion.ToString();

            string folder = FindInFirstFramework(version);
            if (folder != null)
                return Path.Combine(folder, assemblyName);

            // Si pas trouvé, on recherche dans le répertoire apparu depuis la version 3
            // "%ProgramFiles%\Reference Assemblies\Microsoft\Framework\v3.0
            string[] parts = version.Split('.');
            version = String.Join(".", parts, 0, 2);
            folder =
                Path.Combine(Environment.ExpandEnvironmentVariables("%ProgramFiles%"),
                             @"Reference Assemblies\Microsoft\Framework\v" + version);
            // Bidouille pour le 3.0 car l'assembly peut exister dans le 2.0
            string fileName = Path.Combine(folder, assemblyName);
            if (version == "3.0" && File.Exists(fileName))
                return fileName;
            return Path.Combine(FindInFirstFramework("2.0.0.0"), assemblyName);
        }

        /// <summary>
        /// Recherche dans les n° de versions des premiers frameworks
        /// </summary>
        /// <param name="version">The version.</param>
        /// <returns></returns>
        private static string FindInFirstFramework(string version)
        {
            for (int i = 0; i < s_frameworkVersions.Length; i++)
            {
                if (s_frameworkVersions[i].StartsWith(version))
                {
                    return Path.Combine(FileLocationHelper.NetInstallDir, String.Format("v{0}", s_versionsMapping[i]));
                }
            }
            return null;
        }
    }
}