using System;
using System.Collections.Generic;
using System.Diagnostics;
using EnvDTE;
using EnvDTE80;

namespace DSLFactory.Candle.SystemModel.CodeGeneration.CodeModel
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class CandleCodeElement
    {
        /// <summary>
        /// 
        /// </summary>
        public const string EmptyAttributeNameSig = "Empty!";
//        private const string GeneratedAttributeName = "System.CodeDom.Compiler.GeneratedCode";

        private readonly CandleCodeElement _parent;

        #region Gestion des attributs multiples

        // BEGIN-COMMENT : Inutile car les attributs des méthodes virtuelles sont bien répercutés sur les méthodes d'override

        ///// <summary>
        ///// Recherche si il y a des attributs multiples décrits dans un attribut GeneratedCode.
        ///// </summary>
        ///// <param name="strategyId"></param>
        ///// <returns></returns>
        //private List<int> FindGeneratedHashValue(string strategyId)
        //{
        //    CodeElements elements = GetAttributesInternal();

        //    // Recherche de l'attribut GeneratedCodeValue
        //    foreach (CodeElement element in elements)
        //    {
        //        if (element.Name == GeneratedAttributeName)
        //        {
        //            try
        //            {
        //                CodeAttribute2 attr = (CodeAttribute2)element;
        //                Dictionary<string, List<int>> data = CandleCodeElement.DeserializeGeneratedAttributeValue(attr.Value);
        //                if (data == null)
        //                    return null;
        //                List<int> list;
        //                if (data.TryGetValue(strategyId, out list))
        //                {
        //                    data.Remove(strategyId);
        //                    if (data.Count == 0)
        //                        attr.Delete();
        //                    else
        //                        attr.Value = SerializeGeneratedAttributeValue(data);
        //                    return list;
        //                }
        //            }
        //            catch { }
        //            break;
        //        }
        //    }

        //    return null;
        //}

        ///// <summary>
        ///// Calcul de la valeur de hashage d'un atrribut (Nom + valeur)
        ///// </summary>
        ///// <param name="attribute"></param>
        ///// <returns></returns>
        //private int CalculateAttributHashCode(CodeAttribute2 attribute)
        //{
        //    return attribute.Name.GetHashCode() + attribute.Value.GetHashCode();
        //}

        ///// <summary>
        ///// Supprime les attributs multiples générés précédemment par une stratégie
        ///// </summary>
        ///// <param name="strategyId"></param>
        //public void RemoveGeneratedAttributes(string strategyId)
        //{
        //    List<int> hashValues = FindGeneratedHashValue(strategyId);
        //    if (hashValues == null)
        //        return;

        //    foreach (int hv in hashValues)
        //    {
        //        CodeElements elements = GetAttributesInternal();
        //        foreach (CodeElement element in elements)
        //        {
        //            CodeAttribute2 attr = (CodeAttribute2)element;
        //            if (CalculateAttributHashCode(attr) == hv)
        //                attr.Delete();
        //        }
        //    }
        //}

        ///// <summary>
        ///// Pour chaque attribut généré, on va garder la trace de sa génération pour pouvoir le supprimer lors de la prochaine génération.
        ///// Ceci n'est valable que dans le cas d'une génération dans un fichier de code utilisateur.
        ///// </summary>
        ///// <param name="strategyId"></param>
        ///// <param name="attribute"></param>
        //private void AddGeneratedAttribute(string strategyId, CodeAttribute2 attribute)
        //{
        //    if (strategyId != null)
        //    {
        //        CodeAttribute generatedCodeAttribute = null;
        //        foreach (CodeElement element in GetAttributesInternal())
        //        {
        //            if (element.Name == GeneratedAttributeName)
        //            {
        //                generatedCodeAttribute = element as CodeAttribute2;
        //                break;
        //            }
        //        }

        //        Dictionary<string, List<int>> data = new Dictionary<string, List<int>>();
        //        if (generatedCodeAttribute == null)
        //        {
        //            generatedCodeAttribute = AddAttributeInternal(GeneratedAttributeName);
        //        }
        //        else
        //        {
        //            data = DeserializeGeneratedAttributeValue(generatedCodeAttribute.Value);
        //        }

        //        List<int> list;
        //        if (!data.TryGetValue(strategyId, out list))
        //        {
        //            list = new List<int>();
        //            data.Add(strategyId, list);
        //        }

        //        list.Add(CalculateAttributHashCode(attribute));
        //        generatedCodeAttribute.Value = SerializeGeneratedAttributeValue(data);
        //    }
        //}

        ///// <summary>
        ///// Serialisation des valeurs de hashage de chaque attributs multiples
        ///// </summary>
        ///// <param name="value"></param>
        ///// <returns></returns>
        //internal static string SerializeGeneratedAttributeValue(Dictionary<string, List<int>> value)
        //{
        //    // Forme = strategyid:n,n,n|strategyid:n,n,n
        //    StringBuilder sb = new StringBuilder("\"");
        //    foreach (string strategyId in value.Keys)
        //    {
        //        sb.Append(strategyId);
        //        sb.Append(':');
        //        foreach (int hv in value[strategyId])
        //        {
        //            sb.Append(hv);
        //            sb.Append(',');
        //        }
        //        sb.Append('|');
        //    }
        //    sb.Append("\", \"\"");
        //    return sb.ToString();
        //}

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="value"></param>
        ///// <returns></returns>
        //internal static Dictionary<string, List<int>> DeserializeGeneratedAttributeValue(string value)
        //{
        //    Dictionary<string, List<int>> values = new Dictionary<string, List<int>>();

        //    // Préparation des données à déserialiser
        //    value = value.Substring(0, value.LastIndexOf(','));
        //    value = value.Trim('"');

        //    string[] parts = value.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
        //    foreach (string part in parts)
        //    {
        //        int pos = part.IndexOf(':');
        //        string strategyId = part.Substring(0, pos);
        //        List<int> hashValues = new List<int>();
        //        foreach (string v in part.Substring(pos + 1).Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
        //        {
        //            hashValues.Add(Int32.Parse(v));
        //        }
        //        values.Add(strategyId, hashValues);
        //    }

        //    return values;
        //}
        // END-COMMENT

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="CandleCodeElement"/> class.
        /// </summary>
        /// <param name="parent">The parent.</param>
        public CandleCodeElement(CandleCodeElement parent)
        {
            _parent = parent;
        }

        /// <summary>
        /// Gets the parent.
        /// </summary>
        /// <value>The parent.</value>
        public CandleCodeElement Parent
        {
            get { return _parent; }
        }

        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>The name.</value>
        public virtual string Name
        {
            get { return String.Empty; }
        }

        /// <summary>
        /// Gets the members.
        /// </summary>
        /// <value>The members.</value>
        public IEnumerable<CandleCodeElement> Members
        {
            get
            {
                CodeElements elements = GetCodeElements();
                if (elements != null)
                {
                    foreach (CodeElement cel in elements)
                    {
                        CandleCodeElement cce = CreateFromCodeElement(this, cel);
                        if (cce != null)
                        {
                            yield return cce;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Creates from code element.
        /// </summary>
        /// <param name="parent">The parent.</param>
        /// <param name="elem">The elem.</param>
        /// <returns></returns>
        public static CandleCodeElement CreateFromCodeElement(CandleCodeElement parent, CodeElement elem)
        {
            if (elem == null)
                return null;

            switch (elem.Kind)
            {
                case vsCMElement.vsCMElementNamespace:
                    return new CandleCodeNamespace((CodeNamespace) elem);
                case vsCMElement.vsCMElementProperty:
                    return new CandleCodeProperty(parent, (CodeProperty) elem);
                case vsCMElement.vsCMElementClass:
                    return new CandleCodeClass(parent, (CodeClass) elem);
                case vsCMElement.vsCMElementFunction:
                    return new CandleCodeFunction(parent, (CodeFunction) elem);
                case vsCMElement.vsCMElementParameter:
                    return new CandleCodeParameter(parent, (CodeParameter) elem);
                case vsCMElement.vsCMElementStruct:
                    return new CandleCodeStruct(parent, (CodeStruct) elem);
                case vsCMElement.vsCMElementEnum:
                    return new CandleCodeEnum(parent, (CodeEnum) elem);
                case vsCMElement.vsCMElementInterface:
                    return new CandleCodeInterface(parent, (CodeInterface) elem);
                case vsCMElement.vsCMElementVariable:
                    return new CandleCodeVariable(parent, (CodeVariable) elem);
            }
            return null;
        }

        /// <summary>
        /// Accepts the specified visitor.
        /// </summary>
        /// <param name="visitor">The visitor.</param>
        internal abstract void Accept(ICodeModelVisitor visitor);

        /// <summary>
        /// Gets the code elements.
        /// </summary>
        /// <returns></returns>
        protected virtual CodeElements GetCodeElements()
        {
            return null;
        }

        /// <summary>
        /// Adds the attribute internal.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        protected abstract CodeAttribute2 AddAttributeInternal(string name);

        /// <summary>
        /// Gets the attributes internal.
        /// </summary>
        /// <returns></returns>
        protected abstract CodeElements GetAttributesInternal();

        /// <summary>
        /// Adds the attribute.
        /// </summary>
        /// <param name="attributeName">Name of the attribute.</param>
        /// <param name="values">The values.</param>
        public void AddAttribute(string attributeName, params string[] values)
        {
            AddAttribute(attributeName, null, false, values);
        }

        /// <summary>
        /// Adds the attribute.
        /// </summary>
        /// <param name="attributeName">Name of the attribute.</param>
        /// <param name="args">The args.</param>
        public void AddAttribute(string attributeName, Dictionary<string, string> args)
        {
            AddAttribute(attributeName, null, false, args);
        }

        /// <summary>
        /// Ajout d'un attribut
        /// </summary>
        /// <param name="attributeName">Name of the attribute.</param>
        /// <param name="strategyId">The strategy id.</param>
        /// <param name="multiple">if set to <c>true</c> [multiple].</param>
        /// <param name="values">The values.</param>
        public void AddAttribute(string attributeName, string strategyId, bool multiple, params string[] values)
        {
            Dictionary<string, string> args = new Dictionary<string, string>();
            if (values != null)
            {
                for (int i = 0; i < values.Length; i += 2)
                {
                    string key = values[i];
                    if (String.IsNullOrEmpty(key))
                        key = String.Format("{0}{1}", EmptyAttributeNameSig, i/2);
                    args.Add(key, values[i + 1]);
                }
            }

            AddAttribute(attributeName, strategyId, multiple, args);
        }

        /// <summary>
        /// Fusion des paramètres d'attributs entre l'existant et les nouveaux
        /// </summary>
        /// <param name="attributeName">Name of the attribute.</param>
        /// <param name="strategyId">The strategy id.</param>
        /// <param name="multiple">if set to <c>true</c> [multiple].</param>
        /// <param name="args">The args.</param>
        public void AddAttribute(string attributeName, string strategyId, bool multiple, Dictionary<string, string> args)
        {
            Debug.Assert(attributeName != null);

            CodeAttribute2 attribute = null;
            try
            {
                if (!multiple)
                {
                    foreach (CodeElement element in GetAttributesInternal())
                    {
                        if (element.Name == attributeName)
                        {
                            attribute = element as CodeAttribute2;
                            break;
                        }
                    }
                }

                if (attribute == null)
                {
                    attribute = AddAttributeInternal(attributeName);
                }

                //            Dictionary<string, string> args = SplitArguments(value);
                int i = 0;
                foreach (CodeAttributeArgument argument in attribute.Arguments)
                {
                    string argumentName = argument.Name;
                    string key = argumentName;

                    if (String.IsNullOrEmpty(key))
                        key = String.Format("{0}{1}", EmptyAttributeNameSig, i);
                    i++;
                    if (args.ContainsKey(key))
                    {
                        string newValue = args[key];
                        if (argument.Value != newValue)
                            argument.Value = newValue;
                        args.Remove(key);
                    }
                }
                foreach (string remainingArgumentName in args.Keys)
                {
                    attribute.AddArgument(args[remainingArgumentName],
                                          remainingArgumentName.StartsWith(EmptyAttributeNameSig)
                                              ? String.Empty
                                              : remainingArgumentName, -1);
                }

                // On enleve cette possibilité car à priori les attributs définis sur une méthode sont bien répercutés sur les méthodes
                // de surcharge
//                if (multiple)
//                    AddGeneratedAttribute(strategyId, attribute);
            }
            catch (Exception ex)
            {
                ILogger logger = ServiceLocator.Instance.GetService<ILogger>();
                if (logger != null)
                    logger.WriteError("Code injection", String.Format("can add attribute {0}", attributeName), ex);
            }
        }
    }
}