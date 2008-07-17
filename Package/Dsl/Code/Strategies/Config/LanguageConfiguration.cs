using System;
using System.CodeDom.Compiler;
using System.Text;
using System.Xml.Serialization;
using Microsoft.CSharp;

namespace DSLFactory.Candle.SystemModel.Strategies
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable()]
    public class LanguageConfiguration
    {
        //private string _defaultClassTemplateName = "CSharp/class.zip";
        private string _defaultLibraryTemplateName = "CSharp/ClassLibrary.zip";
        private string _defaultWebLibraryTemplateName = "Web/EmptyWeb.zip";
        private string _extension = ".cs";
        private string _name = "CSharp";

        private readonly string[] _standardTypes = new string[]
            {
                "int", "byte", "string", "void", "long", "double",
                "float", "bool", "char", "short", "uint", "sbyte", "ulong", "object", "ushort", "decimal", "DateTime"
            };

        /// <summary>
        /// Types standards du langage
        /// </summary>
        [XmlIgnore]
        public string[] StandardTypes
        {
            get { return _standardTypes; }
        }

        //[XmlAttribute("defaultClassTemplate")]
        //public string DefaultClassTemplateName
        //{
        //    get
        //    {
        //        return _defaultClassTemplateName;
        //    }
        //    set
        //    {
        //        if( String.IsNullOrEmpty( value ) )
        //            throw new ArgumentNullException( "DefaultClassTemplateName" );
        //        _defaultClassTemplateName = value;
        //    }
        //}

        /// <summary>
        /// Nom du template par défaut (csClassLibrary)
        /// </summary>
        [XmlAttribute("defaultProjectTemplate")]
        public string DefaultLibraryTemplateName
        {
            get { return _defaultLibraryTemplateName; }
            set
            {
                if (String.IsNullOrEmpty(value))
                    throw new ArgumentNullException("DefaultLibraryTemplateName");
                _defaultLibraryTemplateName = value;
            }
        }

        /// <summary>
        /// Gets or sets the default name of the web library template.
        /// </summary>
        /// <value>The default name of the web library template.</value>
        [XmlAttribute("defaultWebProjectTemplate")]
        public string DefaultWebLibraryTemplateName
        {
            get { return _defaultWebLibraryTemplateName; }
            set
            {
                if (String.IsNullOrEmpty(value))
                    throw new ArgumentNullException("DefaultWebLibraryTemplateName");
                _defaultWebLibraryTemplateName = value;
            }
        }

        // TODO generic private string _codeDomProviderTypeName;
        /// <summary>
        /// Gets the code DOM provider.
        /// </summary>
        /// <value>The code DOM provider.</value>
        [XmlIgnore]
        public CodeDomProvider CodeDomProvider
        {
            get { return new CSharpCodeProvider(); }
        }

        /// <summary>
        /// Gets or sets the extension.
        /// </summary>
        /// <value>The extension.</value>
        [XmlAttribute("defaultExtension")]
        public string Extension
        {
            get { return _extension; }
            set
            {
                if (String.IsNullOrEmpty(value))
                    throw new ArgumentNullException("Extension");
                if (value[0] != '.')
                    value = '.' + value;
                _extension = value;
            }
        }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        [XmlAttribute("defaultName")]
        public string Name
        {
            get { return _name; }
            set
            {
                if (String.IsNullOrEmpty(value))
                    throw new ArgumentNullException("Name");
                _name = value;
            }
        }

        /// <summary>
        /// Creates the parameter list.
        /// </summary>
        /// <param name="operation">The operation.</param>
        /// <returns></returns>
        internal string CreateParameterList(Operation operation)
        {
            StringBuilder parameters = new StringBuilder();
            for (int i = 0; i < operation.Arguments.Count; i++)
            {
                Argument arg = operation.Arguments[i];
                switch (arg.Direction)
                {
                    case ArgumentDirection.InOut:
                        parameters.Append("ref ");
                        break;
                    case ArgumentDirection.Out:
                        parameters.Append("out ");
                        break;
                    case ArgumentDirection.In:
                    default:
                        break;
                }
                parameters.Append(arg.Name);
                if (i < operation.Arguments.Count - 1)
                    parameters.Append(", ");
            }
            return parameters.ToString();
        }

        /// <summary>
        /// Creates the parameters definition.
        /// </summary>
        /// <param name="operation">The operation.</param>
        /// <returns></returns>
        internal string CreateParametersDefinition(Operation operation)
        {
            StringBuilder paramDef = new StringBuilder();
            for (int i = 0; i < operation.Arguments.Count; i++)
            {
                Argument arg = operation.Arguments[i];
                switch (arg.Direction)
                {
                    case ArgumentDirection.InOut:
                        paramDef.Append("ref ");
                        break;
                    case ArgumentDirection.Out:
                        paramDef.Append("out ");
                        break;
                    case ArgumentDirection.In:
                    default:
                        break;
                }
                paramDef.Append(arg.FullTypeName);
                paramDef.Append(' ');
                paramDef.Append(arg.Name);
                if (i < operation.Arguments.Count - 1)
                    paramDef.Append(", ");
            }
            return paramDef.ToString();
        }
    }
}