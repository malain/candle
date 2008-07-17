using System;
using System.Text;
using DSLFactory.Candle.SystemModel.Strategies;

namespace DSLFactory.Candle.SystemModel.CodeGeneration
{
    /// <summary>
    /// 
    /// </summary>
    public static class CodeGenerationUtils
    {
        /// <summary>
        /// Définition des paramètres avec leurs types
        /// </summary>
        /// <param name="op">The op.</param>
        /// <returns></returns>
        public static string GetParametersDefinition(Operation op)
        {
            StringBuilder paramDef = new StringBuilder();
            for (int i = 0; i < op.Arguments.Count; i++)
            {
                Argument arg = op.Arguments[i];
                paramDef.Append(NormalizeType(arg));
                paramDef.Append(' ');
                paramDef.Append(arg.Name);
                if (i < op.Arguments.Count - 1)
                    paramDef.Append(", ");
            }
            return paramDef.ToString();
        }

        /// <summary>
        /// Liste des paramètres
        /// </summary>
        /// <param name="op">The op.</param>
        /// <returns></returns>
        public static string GetParametersList(Operation op)
        {
            StringBuilder parameters = new StringBuilder();
            for (int i = 0; i < op.Arguments.Count; i++)
            {
                Argument arg = op.Arguments[i];
                parameters.Append(arg.Name);
                if (i < op.Arguments.Count - 1)
                    parameters.Append(", ");
            }
            return parameters.ToString();
        }

        /// <summary>
        /// Permet de résoudre un type en retournant son nom complet
        /// </summary>
        /// <param name="component">The component.</param>
        /// <param name="typeName">Name of the type.</param>
        /// <returns></returns>
        public static string ResolveTypeName(SoftwareComponent component, string typeName)
        {
            DataType typeRef = component.FindGlobalType(typeName);
            if (typeRef != null)
                typeName = typeRef.FullName;
            return typeName;
        }

        /// <summary>
        /// Normalizes the type.
        /// </summary>
        /// <param name="elem">The elem.</param>
        /// <returns></returns>
        public static string NormalizeType(TypeMember elem)
        {
            if (elem.IsCollection)
            {
                if (StrategyManager.GetInstance(elem.Store).NamingStrategy.CollectionAsArray)
                    return String.Format("{0}[]", elem.Type);
                else
                    return String.Format("System.Collections.Generic.List<{0}>", elem.Type);
            }
            return elem.Type;
        }

        /// <summary>
        /// Formate les commentaires en mettant les caractères "///" devant chaque ligne
        /// </summary>
        /// <param name="indent">Indentation des lignes</param>
        /// <param name="comment">Commentaire</param>
        /// <returns></returns>
        public static string FormatComment(string indent, string comment)
        {
            StringBuilder sb = new StringBuilder();
            bool addBalise = !comment.Contains("<summary>");
            if (addBalise)
            {
                comment = "<summary>\r\n" + comment + "\r\n</summary>";
            }

            string[] lines = comment.Split(new string[] {"\r\n", "\n"}, StringSplitOptions.None);

            for (int i = 0; i < lines.Length; i++)
            {
                string line = lines[i];
                sb.Append(indent);
                sb.Append("/// ");
                sb.Append(line);
                if (i < lines.Length - 1)
                    sb.AppendLine();
            }

            return sb.ToString();
        }

        /// <summary>
        /// Determines whether [is value type] [the specified type name].
        /// </summary>
        /// <param name="typeName">Name of the type.</param>
        /// <returns>
        /// 	<c>true</c> if [is value type] [the specified type name]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsValueType(string typeName)
        {
            // TODO dépend du langage
            string[] valueTypes = {
                                      "int", "bool", "double", "long", "byte", "float",
                                      "enum", "sbyte", "char", "short", "ushort", "uint", "ulong",
                                      "struct", "decimal"
                                  };

            return Array.IndexOf<string>(valueTypes, typeName) >= 0;
        }
    }
}