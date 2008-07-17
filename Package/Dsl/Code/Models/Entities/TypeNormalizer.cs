using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.EnterpriseTools.ArtifactModel.Clr;

namespace DSLFactory.Candle.SystemModel
{
    /// <summary>
    /// Délégué pour la création d'un type non existant dans le modèle
    /// </summary>
    /// <param name="type">Nom du type</param>
    /// <returns>Nom complet du type</returns>
    public delegate string EnsureTypeExistsHandler(string type);

    /// <summary>
    /// Classe helper permettant de vérifier si un type saisie est valide et le normalise en ajoutant
    /// les namespaces sur les types y compris les arguments de génériques.
    /// </summary>
    public class ClrTypeParser
    {
        #region ClrTypeHelper
        /// <summary>
        /// 
        /// </summary>
        class ClrTypesHelper
        {
            /// <summary>
            /// Recherche parmi un type saisi dans la fenetre de détail d'un port la liste des types
            /// qui doivent être insérés dans la couche modèle
            /// </summary>
            /// <param name="language">The language.</param>
            /// <param name="name">The name.</param>
            /// <returns></returns>
            public static IList<string> GetModelNamesFromClrType(Language language, string name)
            {
                if (ParseType.CanParse(language, name))
                {
                    ParseType parseType = new ParseType(language, name);
                    TypeInstance typeInstance = parseType.TypeInstance;
                    if (typeInstance.TypeKind == TypeKind.Type)
                        return GetModelsNameRecursive(language, typeInstance);
                }
                return null;
            }

            /// <summary>
            /// Gets the models name recursive.
            /// </summary>
            /// <param name="language">The language.</param>
            /// <param name="typeInstance">The type instance.</param>
            /// <returns></returns>
            private static IList<string> GetModelsNameRecursive(Language language, TypeInstance typeInstance)
            {
                List<string> list = new List<string>();

                // Si c'est un type de base, on ne le prend pas
                if (typeInstance.TypeKind != TypeKind.Type)
                    return list;

                // Si c'est un générique, on cherche dans ses arguments les types potentiels
                if (typeInstance.IsGenericType)
                {
                    foreach (TypeInstance genericType in typeInstance.GenericArguments)
                    {
                        list.AddRange(GetModelsNameRecursive(language, genericType));
                    }
                }
                else
                {
                    string name = typeInstance.GetLookupName(language);
                    if (!name.StartsWith("System."))
                    {
                        // On s'assure de ne pas prendre un type système
                        if (Type.GetType(String.Format("System.{0}", name), false) == null)
                            list.Add(name);
                    }
                }

                return list;
            }
        }
        #endregion    

        /// <summary>
        /// Vérifie si un type est valide. Si le namespace n'est pas indiqué, on considère qu'il se trouve dans
        /// le namespace par défaut.
        /// </summary>
        /// <param name="initialTypeName">type saisie</param>
        /// <param name="handler">handler appelé pour créer un type non existant</param>
        /// <returns></returns>
        public static string Parse(string initialTypeName, EnsureTypeExistsHandler handler)
        {
            try
            {
                return GetParser(initialTypeName, handler).Analyze();
            }
            catch
            {
                Language language = LanguageCSharp.Instance();
                ClrTypesHelper.GetModelNamesFromClrType(CurrentLanguage, initialTypeName);
                return null;
            }
        }

        /// <summary>
        /// Gets the parser.
        /// </summary>
        /// <param name="initialTypeName">Initial name of the type.</param>
        /// <param name="handler">The handler.</param>
        /// <returns></returns>
        static ITypeParser GetParser(string initialTypeName, EnsureTypeExistsHandler handler)
        {
            return new CSharpTypeParser(initialTypeName, handler);
        }

        /// <summary>
        /// Gets the current language.
        /// </summary>
        /// <value>The current language.</value>
        static Language CurrentLanguage
        {
            get { return LanguageCSharp.Instance(); }
        }

        /// <summary>
        /// Gets the type of the model names from CLR.
        /// </summary>
        /// <param name="clrTypeName">Name of the CLR type.</param>
        /// <returns></returns>
        public static IList<string> GetModelNamesFromClrType(string clrTypeName)
        {
            return ClrTypesHelper.GetModelNamesFromClrType(CurrentLanguage, clrTypeName);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public interface ITypeParser
    {
        /// <summary>
        /// Analyzes this instance.
        /// </summary>
        /// <returns></returns>
        string Analyze();
    }

    /// <summary>
    /// Parser de type pour CSharp
    /// </summary>
    class CSharpTypeParser : ITypeParser
    {
        private readonly string _initialName;
        private readonly char[] _buffer;
        private int _pos;
        private char _currentChar;
        private const char EOF = '\0';
        private static readonly List<string> s_primitiveTypes;
        private readonly EnsureTypeExistsHandler _ensureTypeExists;

        /// <summary>
        /// Initializes the <see cref="CSharpTypeParser"/> class.
        /// </summary>
        static CSharpTypeParser()
        {
            s_primitiveTypes = new List<string>();
            s_primitiveTypes.Add("void");
            s_primitiveTypes.Add("int");
            s_primitiveTypes.Add("byte");
            s_primitiveTypes.Add("string");
            s_primitiveTypes.Add("void");
            s_primitiveTypes.Add("long");
            s_primitiveTypes.Add("double");
            s_primitiveTypes.Add("float");
            s_primitiveTypes.Add("bool");
            s_primitiveTypes.Add("char");
            s_primitiveTypes.Add("short");
            s_primitiveTypes.Add("uint");
            s_primitiveTypes.Add("sbyte");
            s_primitiveTypes.Add("ulong");
            s_primitiveTypes.Add("object");
            s_primitiveTypes.Add("ushort");
            s_primitiveTypes.Add("decimal");
            s_primitiveTypes.Add("list");
            s_primitiveTypes.Add("dictionnary");
            s_primitiveTypes.Add("arraylist");
            s_primitiveTypes.Add("bitarray");
            s_primitiveTypes.Add("hashtable");
            s_primitiveTypes.Add("queue");
            s_primitiveTypes.Add("sortedlist");
            s_primitiveTypes.Add("stack");
            s_primitiveTypes.Add("linkedlist");
            s_primitiveTypes.Add("sorteddictionnary");
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CSharpTypeParser"/> class.
        /// </summary>
        /// <param name="initialTypeName">Initial name of the type.</param>
        /// <param name="handler">The handler.</param>
        public CSharpTypeParser(string initialTypeName, EnsureTypeExistsHandler handler)
        {
            this._initialName = initialTypeName;
            _buffer = initialTypeName.ToCharArray();
            _currentChar = NextChar();
            _ensureTypeExists = handler;
        }

        /// <summary>
        /// Analyzes this instance.
        /// </summary>
        /// <returns></returns>
        public string Analyze()
        {
            StringBuilder s = new StringBuilder(AnalyzeSubType());
            if (_currentChar == '[')
            {
                Accept('[');
                Accept(']');
                s.Append("[]");
            }

            Accept(EOF);
            //NON on garde le non initial
            //return s.ToString();
            return _initialName;
        }

        /// <summary>
        /// Accepts the specified ch.
        /// </summary>
        /// <param name="ch">The ch.</param>
        /// <returns></returns>
        private char Accept(char ch)
        {
            if (_currentChar != ch)
                throw new Exception("syntax error");
            return NextChar();
        }

        /// <summary>
        /// Nexts the char.
        /// </summary>
        /// <returns></returns>
        private char NextChar()
        {
            while (_pos < _buffer.Length)
            {
                _currentChar = _buffer[_pos++];
                if (!Char.IsSeparator(_currentChar))
                    return _currentChar;
            }

            return _currentChar = EOF;
        }

        /// <summary>
        /// Analyzes the sub type.
        /// </summary>
        /// <returns></returns>
        public string AnalyzeSubType()
        {
            StringBuilder s = new StringBuilder(AnalyzeType());
            while (_currentChar != EOF)
            {
                // IL généric
                if (_currentChar == '`')
                {
                    NextChar();
                    StringBuilder strNumber = new StringBuilder();
                    while (Char.IsDigit(_currentChar))
                    {
                        strNumber.Append(_currentChar);
                        NextChar();
                    }

                    // Nbre de paramètres
                    int number = Int16.Parse(strNumber.ToString());

                    Accept('[');
                    s.Append('<');
                    while (number-- > 0)
                    {
                        Accept('[');

                        StringBuilder fullType = new StringBuilder();
                        while (_currentChar != ']')
                        {
                            fullType.Append(_currentChar);
                            NextChar();
                        }

                        Accept(']');
                        string simpleType = fullType.ToString();
                        int pos = simpleType.IndexOf(',');
                        if (pos > 0)
                        {
                            simpleType = simpleType.Substring(0, pos).Trim();
                        }
                        s.Append(FinalType(simpleType));
                        if (number > 0)
                        {
                            Accept(',');
                            s.Append(',');
                        }
                    }
                    Accept(']');
                    s.Append('>');
                }
                else if (_currentChar == '<')
                {
                    Accept('<');
                    s.Append('<');
                    s.Append(AnalyzeSubType());
                    while (_currentChar == ',')
                    {
                        Accept(',');
                        s.Append(',');
                        s.Append(AnalyzeSubType());
                    }
                    Accept('>');
                    s.Append('>');
                }
                else
                    break;
            }

            return s.ToString();
        }

        /// <summary>
        /// Analyzes the type.
        /// </summary>
        /// <returns></returns>
        string AnalyzeType()
        {
            StringBuilder sb = new StringBuilder();
            while (true)
            {
                if (_currentChar == EOF || "`<>[], ".IndexOf(_currentChar) >= 0)
                {
                    return FinalType(sb.ToString());
                }

                sb.Append(_currentChar);
                NextChar();
            }
        }

        /// <summary>
        /// Finals the type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        private string FinalType(string type)
        {
            // On va ajouter ce type au modèle si ce n'est pas un type du framework
            if (!s_primitiveTypes.Contains(type.ToLower()))
            {
                if (Type.GetType(type, false, true) == null)
                {
                    if (type.IndexOf('.') <= 0)
                    {
                        string t = CheckDotNetType(type, _currentChar == '<');
                        if (t != null)
                            return t;
                        if (_ensureTypeExists != null)
                            return _ensureTypeExists(type);
                    }
                }
            }
            return type;
        }

        // TODO on ne peut pas charger un type générique par GetType donc pour l'instant on ignore
        // et on se contente de renvoyer le type passé en paramètre si c'est OK
        /// <summary>
        /// Checks the type of the dot net.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="generic">if set to <c>true</c> [generic].</param>
        /// <returns></returns>
        private string CheckDotNetType(string type, bool generic)
        {
            Type t = Type.GetType("System." + type, false, true);
            if (t != null)
                return t.FullName;

            string n = String.Empty;
            //if( generic )
            //{
            //    n = "Generic.";
            //}
            t = Type.GetType("System.Collections." + n + type, false, true);
            if (t != null)
                return type; //  t.FullName;

            return null;
        }
    }
}
