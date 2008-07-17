using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace DSLFactory.Candle.SystemModel
{
    /// <summary>
    /// Classe utilitaire
    /// </summary>
    public sealed class Utils
    {
        private static string s_tempFolder;

        /// <summary>
        /// Gets the mouse position.
        /// </summary>
        /// <value>The mouse position.</value>
        public static Point MousePosition
        {
            get { return Control.MousePosition; }
        }

        /// <summary>
        /// Répertoire de travail de candle
        /// </summary>
        /// <returns></returns>
        public static string GetCandleTempFolder()
        {
            if (s_tempFolder == null)
            {
                s_tempFolder = Path.Combine(Path.GetTempPath(), @"CandleTemp");
                Directory.CreateDirectory(s_tempFolder);
            }
            return s_tempFolder;
        }

        /// <summary>
        /// Retourne un nouveau répertoire temporaire dans le répertoire de travail de candle
        /// </summary>
        /// <returns></returns>
        public static string GetTemporaryFolder()
        {
            string tempFile = Path.GetTempFileName(); // Génere un nouveau nom
            DeleteFile(tempFile);
            return Path.Combine(GetCandleTempFolder(), Path.GetFileName(tempFile));
        }

        /// <summary>
        /// Retourne un nouveau chemin temporaire dans le répertoire de travail de candle
        /// </summary>
        /// <param name="relativeModelFileName">Nom du fichier</param>
        /// <returns></returns>
        public static string GetTemporaryFileName(string relativeModelFileName)
        {
            string targetFileName = Path.Combine(GetTemporaryFolder(), relativeModelFileName);
            return targetFileName;
        }

        /// <summary>
        /// Normalise un nom de variable en respectant les conventions d'écriture. Par défaut, il est en
        /// Pascal Casing
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string NormalizeName(string name)
        {
            if (String.IsNullOrEmpty(name))
                return "_";


            StringBuilder sb = new StringBuilder();
            if (Char.IsDigit(name[0]))
            {
                sb.Append("_");
            }

            // Séparateur de nom :
            //  - Tout autre car que lettre ou chiffre
            //  - Une majuscule précédée d'une minuscule ou d'une autre majuscule (avec un max de 3) si le 
            //    mot initial contient au moins une minuscule.

            bool containsLowerChars = false;
            for (int i = 0; i < name.Length; i++)
            {
                if (char.IsLower(name, i))
                {
                    containsLowerChars = true;
                    break;
                }
            }

            bool newWord = true;
            char prevChar = ' ';
            int upperCount = 0;
            for (int i = 0; i < name.Length; i++)
            {
                char ch = name[i];
                // Est ce que c'est un séparateur
                if (char.IsLetterOrDigit(ch))
                {
                    if (!newWord && containsLowerChars)
                    {
                        if (char.IsUpper(ch) && char.IsLower(prevChar))
                        {
                            upperCount = 0;
                            newWord = true;
                        }
                        else if (char.IsUpper(ch) && char.IsUpper(prevChar) && upperCount < 3)
                        {
                            upperCount++;
                            newWord = true;
                        }
                        else
                            upperCount = 0;
                    }
                    prevChar = ch;

                    if (newWord)
                    {
                        ch = char.ToUpper(ch);
                    }
                    else
                    {
                        ch = char.ToLower(ch);
                    }

                    newWord = false;
                    sb.Append(ch);
                }
                else
                {
                    newWord = true;
                    if (ch == '_')
                        sb.Append(ch);
                }
            }
            name = sb.ToString();
            //    name = this.LegalizeKeywords(name);
            return name;
        }

        /// <summary>
        /// S"assure qu'une chaine ne fera pas plus d'une certaine longueur. La tronque si nécessaire
        /// et rajoute le prefixe '...'.
        /// </summary>
        /// <param name="str">Chaine à tronquer</param>
        /// <param name="maxLength">Longueur maximun souhaité</param>
        /// <returns>Chaine d'une longueur maximun de 'maxLength'</returns>
        public static string StripString(string str, int maxLength)
        {
            if (str.Length <= maxLength)
                return str;

            // On se positionne au maximun et on remonte jusqu'à trouver un séparateur.
            for (int i = str.Length - (maxLength - 3); i < str.Length; i++)
            {
                if (str[i] == Path.DirectorySeparatorChar)
                {
                    return String.Concat("...", str.Substring(i));
                }
            }

            return String.Concat("...", str.Substring(str.Length - (maxLength - 3)));
        }

        /// <summary>
        /// Copie le contenu d'un répertoire (non récursif) dans un autre
        /// </summary>
        /// <param name="sourceFolder"></param>
        /// <param name="targetFolder"></param>
        /// <param name="filePattern"></param>
        public static int CopyFiles(string sourceFolder, string targetFolder, string filePattern)
        {
            if (!Directory.Exists(sourceFolder))
                return -1;

            int cx = 0;
            Directory.CreateDirectory(targetFolder);

            DirectoryInfo di = new DirectoryInfo(sourceFolder);
            foreach (FileInfo fi in di.GetFiles(filePattern))
            {
                string destFile = Path.Combine(targetFolder, fi.Name);
                CopyFile(fi.FullName, destFile);
                cx++;
            }
            return cx;
        }

        /// <summary>
        /// Copie un fichier en s'assurant que le répertoire destination existe et que le
        /// fichier destination peut-être écrasé si il existe.
        /// </summary>
        /// <param name="sourceFileName"></param>
        /// <param name="destFileName"></param>
        public static void CopyFile(string sourceFileName, string destFileName)
        {
            if (String.IsNullOrEmpty(sourceFileName) || !File.Exists(sourceFileName))
                return;
            DeleteFile(destFileName);
            string folder = Path.GetDirectoryName(destFileName);
            Directory.CreateDirectory(folder);

            File.Copy(sourceFileName, destFileName, true);
        }

        /// <summary>
        /// Unsets the read only.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        public static void UnsetReadOnly(string fileName)
        {
            try
            {
                if (File.Exists(fileName))
                    File.SetAttributes(fileName, FileAttributes.Normal);
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// Suppression d'un fichier
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        public static void DeleteFile(string fileName)
        {
            if (fileName != null && File.Exists(fileName))
            {
                try
                {
                    File.SetAttributes(fileName, FileAttributes.Normal);
                    File.Delete(fileName);
                }
                catch
                {
                }
            }
        }

        /// <summary>
        /// Comparaison de 2 chaines sans tenir compte de la casse et en prenant en compte la culture courante
        /// </summary>
        /// <param name="str1"></param>
        /// <param name="str2"></param>
        /// <returns></returns>
        public static bool StringCompareEquals(string str1, string str2)
        {
            return String.Compare(str1, str2, StringComparison.CurrentCultureIgnoreCase) == 0;
        }

        /// <summary>
        /// Strings the starts with.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="startingString">The starting string.</param>
        /// <returns></returns>
        public static bool StringStartsWith(string source, string startingString)
        {
            if (startingString == null)
                return false;
            return source.StartsWith(startingString, StringComparison.CurrentCultureIgnoreCase);
        }

        /// <summary>
        /// Strings the ends with.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="endingString">The ending string.</param>
        /// <returns></returns>
        public static bool StringEndsWith(string source, string endingString)
        {
            if (endingString == null)
                return false;
            return source.EndsWith(endingString, StringComparison.CurrentCultureIgnoreCase);
        }

        /// <summary>
        /// Files the date equals.
        /// </summary>
        /// <param name="targetFileName">Name of the target file.</param>
        /// <param name="fileName">Name of the file.</param>
        /// <returns></returns>
        internal static bool FileDateEquals(string targetFileName, string fileName)
        {
            return File.GetLastWriteTime(targetFileName) == File.GetLastWriteTime(fileName);
        }

        /// <summary>
        /// Copies the directory.
        /// </summary>
        /// <param name="sourcefolder">The sourcefolder.</param>
        /// <param name="targetFolder">The target folder.</param>
        public static void CopyDirectory(string sourcefolder, string targetFolder)
        {
            if (!Directory.Exists(sourcefolder))
                return;

            DirectoryInfo di = new DirectoryInfo(sourcefolder);
            CopyDirectoryInternal(di, targetFolder);
        }

        /// <summary>
        /// Copies the directory internal.
        /// </summary>
        /// <param name="di">The di.</param>
        /// <param name="targetFolder">The target folder.</param>
        private static void CopyDirectoryInternal(DirectoryInfo di, string targetFolder)
        {
            Directory.CreateDirectory(targetFolder);

            foreach (DirectoryInfo subdir in di.GetDirectories())
            {
                CopyDirectoryInternal(subdir, Path.Combine(targetFolder, subdir.Name));
            }
            foreach (FileInfo file in di.GetFiles())
            {
                try
                {
                    file.CopyTo(Path.Combine(targetFolder, file.Name), true);
                }
                catch
                {
                }
            }
        }

        /// <summary>
        /// Supprime un répertoire avec son contenu (y compris ses sous-répertoires)
        /// </summary>
        /// <param name="path"></param>
        public static void RemoveDirectory(string path)
        {
            if (!Directory.Exists(path))
                return;

            foreach (string subdir in Directory.GetDirectories(path, "*"))
            {
                RemoveDirectory(subdir);
            }

            foreach (string file in Directory.GetFiles(path))
            {
                DeleteFile(file);
            }

            try
            {
                File.SetAttributes(path, FileAttributes.Normal);
                Directory.Delete(path);
            }
            catch
            {
            }
        }

        /// <summary>
        /// Determines whether [is key pressed] [the specified key].
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>
        /// 	<c>true</c> if [is key pressed] [the specified key]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsKeyPressed(Keys key)
        {
            return (Control.ModifierKeys & key) > Keys.None;
        }

        /// <summary>
        /// Determines whether [is mouse button pressed] [the specified button].
        /// </summary>
        /// <param name="button">The button.</param>
        /// <returns>
        /// 	<c>true</c> if [is mouse button pressed] [the specified button]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsMouseButtonPressed(MouseButtons button)
        {
            return (Control.MouseButtons & button) == button;
        }

        /// <summary>
        /// Vérifie si un fichier et vérrouillé (read only)
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns>
        /// 	<c>true</c> if [is file locked] [the specified path]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsFileLocked(string path)
        {
            return
                path != null && File.Exists(path) &&
                (File.GetAttributes(path) & FileAttributes.ReadOnly) == FileAttributes.ReadOnly;
        }

        /// <summary>
        /// Moves the directory.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="target">The target.</param>
        internal static void MoveDirectory(string source, string target)
        {
            if (!Directory.Exists(source))
                return;
            if (Directory.Exists(target))
                RemoveDirectory(target);
            File.SetAttributes(source, FileAttributes.Normal);
            Directory.Move(source, target);
        }

        /// <summary>
        /// Swaps two instance
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="instance1">The instance1.</param>
        /// <param name="instance2">The instance2.</param>
        public static void Swap<T>(ref T instance1, ref T instance2)
        {
            T tmp = instance1;
            instance1 = instance2;
            instance2 = tmp;
        }

        /// <summary>
        /// Recherche d'un fichier dans un répertoire et ses sous-répertoires
        /// </summary>
        /// <param name="folderPath">The folder path.</param>
        /// <param name="filterPattern">The filter pattern.</param>
        /// <returns></returns>
        public static List<string> SearchFile(string folderPath, string filterPattern)
        {
            List<string> result = new List<string>();

            DirectoryInfo di = new DirectoryInfo(folderPath);
            if (di.Exists)
            {
                foreach (FileInfo fi in di.GetFiles(filterPattern))
                    result.Add(fi.FullName);

                foreach (DirectoryInfo child in di.GetDirectories())
                {
                    result.AddRange(SearchFile(child.FullName, filterPattern));
                }
            }

            return result;
        }
    }
}