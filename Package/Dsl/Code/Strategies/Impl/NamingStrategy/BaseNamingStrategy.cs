using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Xml.Serialization;
using Microsoft.CSharp;
using System.Diagnostics;
using System.Text;
using System.ComponentModel;

namespace DSLFactory.Candle.SystemModel.Strategies
{
    /// <summary>
    /// Classe de base pour les classes de nommages
    /// </summary>
    [Serializable()]
    [XmlRoot("NamingStrategy")]
    public class BaseNamingStrategy : INamingStrategy
    {
       // private string _interfaceNameFormat = "I{0}";
        private string _privateVariableNameFormat = "_{0}";
       // private string _privateEntityNameFormat = "{0}Info";
        private string _columNameFormat = "{0}_{1}";
        private string _projectFolderFormat = "Layer {0}";
        private string _defaultGeneratedCodeFilePattern = "[code]/GeneratedCode/[sn]/{0}";

        private List<LayerNamingRule> _layersNamingRules = new List<LayerNamingRule>();

        private CodeDomProvider _codeDomProvider;
        private bool _collectionAsArray = false;

        private string _defaultNamespace;

        /// <summary>
        /// Gets the code DOM provider.
        /// </summary>
        /// <value>The code DOM provider.</value>
        [XmlIgnore]
        [Browsable(false)]
        public CodeDomProvider CodeDomProvider
        {
            get 
            {
                if( _codeDomProvider == null )
                    _codeDomProvider = new CSharpCodeProvider();
                return _codeDomProvider; 
            }
        }

        /// <summary>
        /// Nom du package.
        /// </summary>
        /// <value></value>
        [XmlIgnore]
        [Browsable(false)]
        public virtual string PackageName
        {
            get { return null; } // En interne
        }

        // Pattern dans le nom du fichier
        // '~' : indique de mettre le fichier tel quel à la racine
        // [codeFolder]|[code] : Répertoire de code (racine ou app_code)
        // [namespace]|[nspc]  : namespace hierarchie
        // [strategyName]|[sn] : Nom de la strategie
        //      ex : [codeFolder]/[namespace]/xxx.cs
        /// <summary>
        /// Pattern de création du nom du fichier générée
        /// </summary>
        /// <value></value>
        /// <remarks>
        /// '~' : indique de mettre le fichier tel quel à la racine<br/>
        /// [codeFolder]|[code] : Répertoire de code<br/>
        /// [namespace]|[ns]  : namespace hierarchie<br/>
        /// 	<example>
        /// [codeFolder]/[namespace]/{0}
        /// </example>
        /// </remarks>
        [Description("Default generated file name pattern\r\n'~\':project folder root\r\n[codeFolder]|[code] : Code folder (App_Code for web project or main folder)\r\n[namespace]|[nspc]  : namespace hierarchy\r\n[strategyName]|[sn] : Strategy name\r\nex: [code]\\[sn]\\{0}.cs")]
        public string DefaultGeneratedCodeFilePattern
        {
            get { return _defaultGeneratedCodeFilePattern; }
            set { _defaultGeneratedCodeFilePattern = value; }
        }

        /// <summary>
        /// Gets a value indicating whether [collection as array].
        /// </summary>
        /// <value><c>true</c> if [collection as array]; otherwise, <c>false</c>.</value>
        [Description("if true generate collection as a static array else as a generic List<T> in")]
        [XmlAttribute(AttributeName = "collectionAsArray")]
        public bool CollectionAsArray
        {
            get { return _collectionAsArray = false; }
            set { _collectionAsArray = value; }
        }

        /// <summary>
        /// Gets or sets the layer naming rules.
        /// </summary>
        /// <value>The layer naming rules.</value>
        [Editor(typeof(DSLFactory.Candle.SystemModel.Editor.GenericCollectionEditor<LayerNamingRule>), typeof(System.Drawing.Design.UITypeEditor))]
        public List<LayerNamingRule> LayerNamingRules
        {
            get { return _layersNamingRules; }
            set { _layersNamingRules = value; }
        }

        /// <summary>
        /// Gets or sets the default project folder format.
        /// </summary>
        /// <value>The default project folder format.</value>
        [Description("Project folder pattern\r\n{0}=package layer name\r\n{1}=layer name\r\n{2}=Component namespace\r\n{3}=default layer name")]
        public string DefaultProjectFolderFormat
        {
            get { return _projectFolderFormat; }
            set { _projectFolderFormat = value; }
        }

        /// <summary>
        /// Gets or sets the column name format.
        /// </summary>
        /// <value>The column name format.</value>
        [Description("SQL Column name pattern\r\n{0}=Component name\r\n{1}=Property name")]
        public string ColumnNameFormat
        {
            get { return _columNameFormat; }
            set { _columNameFormat = value; }
        }

        /// <summary>
        /// Gets or sets the private variable name format.
        /// </summary>
        /// <value>The private variable name format.</value>
        [Description("Private variable pattern\r\n{0}=name")]
        public string PrivateVariableNameFormat
        {
            get { return _privateVariableNameFormat; }
            set { _privateVariableNameFormat = value; }
        }

        #region INamingStrategy Members
        /// <summary>
        /// Namespace par défaut pour l'initialisation du composant
        /// </summary>
        /// <value>The default namespace.</value>
        public string DefaultNamespace
        {
            get { return _defaultNamespace; }
            set { _defaultNamespace = value; }
        }

        /// <summary>
        /// Creates the name of the element.
        /// </summary>
        /// <param name="layer">The layer.</param>
        /// <param name="initialName">The initial name.</param>
        /// <returns></returns>
        public virtual string CreateElementName(SoftwareLayer layer, string initialName)
        {
            if (layer == null)
                return initialName;
            if (initialName == null)
                initialName = String.Empty;

            LayerNamingRule rule = FindRule(layer);
            return string.Format(rule.ElementFormatString, ToPascalCasing( initialName ), layer.Name);
        }

        /// <summary>
        /// Finds the rule.
        /// </summary>
        /// <param name="layer">The layer.</param>
        /// <returns></returns>
        protected LayerNamingRule FindRule(SoftwareLayer layer)
        {
            string layerTypeName = layer.GetType().Name;
            
            LayerNamingRule rule = _layersNamingRules.Find(delegate(LayerNamingRule r) { return Utils.StringCompareEquals(r.LayerType, layerTypeName); });

            // Si n'existe pas, on en crée une par défaut
            if (rule == null)
            {
                rule = new LayerNamingRule();
                rule.LayerType = layerTypeName;
                rule.AssemblyFormatString = "{2}";
                rule.ProjectFolderFormatString = DefaultProjectFolderFormat;
                rule.FormatString = "{0}";
                rule.ElementFormatString = "{0}";
                rule.DefaultName = layer.GetType().Name;

                // Regles par défaut
                if (layer is DataLayer)
                {
                    rule.DefaultName = "InfoLayer";
                    rule.ElementFormatString = "{0}Info";
                }
                else if (layer is InterfaceLayer)
                {
                    rule.DefaultName = "Interfaces";
                    rule.ElementFormatString = "I{0}";
                    rule.FormatString = "I{4}";
                }
                else if (layer is DataAccessLayer)
                {
                    rule.DefaultName = "DAO";
                    rule.ElementFormatString = "{0}DAO";
                }
                else if (layer is BusinessLayer)
                {
                    rule.DefaultName = "Services";
                    rule.ElementFormatString = "{0}BLL";
                }
                else if (layer is PresentationLayer)
                {
                    rule.DefaultName = "UILayer";
                    rule.ElementFormatString = "{0}";
                }
                else
                {
                    rule.DefaultName = "UILayer";
                    rule.ElementFormatString = "{0}";
                }
                _layersNamingRules.Add(rule);
            }
            return rule;
        }

        /// <summary>
        /// Nom par défaut d'une couche
        /// </summary>
        /// <param name="layerPackage">The layer package.</param>
        /// <param name="element">Type de la couche</param>
        /// <param name="associatedName">Name of the associated.</param>
        /// <returns></returns>
        public virtual string CreateLayerName(LayerPackage layerPackage, SoftwareLayer element, string associatedName)
        {
            string typeName = element.GetType().Name;
            LayerNamingRule rule = FindRule(element);
            if (associatedName == null)
                associatedName = rule.DefaultName;
            return string.Format(rule.FormatString, rule.DefaultName, element.Component.Name, element.Namespace, layerPackage != null ? layerPackage.Name : String.Empty, associatedName);
        }

        /// <summary>
        /// Creates the name of the assembly.
        /// </summary>
        /// <param name="layer">The layer.</param>
        /// <returns></returns>
        public virtual string CreateAssemblyName(SoftwareLayer layer)
        {
            LayerNamingRule rule = FindRule(layer);
            string fmt = rule.AssemblyFormatString;
            if ((layer is PresentationLayer || layer is UIWorkflowLayer) && ((Layer)layer).HostingContext == HostingContext.Standalone)
                fmt = "{4}";
            return string.Format(fmt, layer.VSProjectName, layer.Namespace, layer.Name, layer.Component.Model.Name, layer.Component.Name);
        }

        /// <summary>
        /// Creates the name of the project folder.
        /// </summary>
        /// <param name="layer">The layer.</param>
        /// <param name="packageLayerName">Name of the package layer.</param>
        /// <returns></returns>
        public virtual string CreateProjectFolderName(SoftwareLayer layer, string packageLayerName)
        {
            if (packageLayerName == null)
                packageLayerName = layer.Name;
            LayerNamingRule rule = FindRule(layer);
            return string.Format(rule.ProjectFolderFormatString, packageLayerName, layer.VSProjectName, layer.Component.Namespace, rule.DefaultName);
        }

        /// <summary>
        /// Nom du layer package
        /// </summary>
        /// <param name="layer">The layer.</param>
        /// <returns></returns>
        public virtual string GetLayerName(Layer layer)
        {
            LayerNamingRule rule = FindRule(layer);
            return rule.DefaultName;
        }

        /// <summary>
        /// Creates the namespace.
        /// </summary>
        /// <param name="appNamespace">The app namespace.</param>
        /// <param name="projectName">Name of the project.</param>
        /// <param name="element">The element.</param>
        /// <returns></returns>
        public virtual string CreateNamespace(string appNamespace, string projectName, SoftwareLayer element)
        {
            return string.Format( "{0}.{1}", appNamespace, projectName );
        }

        /// <summary>
        /// Determines whether [is class name valid] [the specified class name].
        /// </summary>
        /// <param name="className">Name of the class.</param>
        /// <returns>
        /// 	<c>true</c> if [is class name valid] [the specified class name]; otherwise, <c>false</c>.
        /// </returns>
        public bool IsClassNameValid( string className )
        {
            return CodeDomProvider.IsValidIdentifier( className );
        }

        /// <summary>
        /// Determines whether [is namespace valid] [the specified @namespace].
        /// </summary>
        /// <param name="namespace">The @namespace.</param>
        /// <returns>
        /// 	<c>true</c> if [is namespace valid] [the specified @namespace]; otherwise, <c>false</c>.
        /// </returns>
        public bool IsNamespaceValid( string @namespace )
        {
            string[] parts = @namespace.Split( '.' );
            foreach( String part in parts )
            {
                if( !IsClassNameValid( part ) )
                    return false;
            }
            return true;
        }

        /// <summary>
        /// Toes the pascal casing.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public string ToPascalCasing( string name )
        {
            if( String.IsNullOrEmpty( name ) )
                return String.Empty;

            name = Utils.NormalizeName(name);
            return Char.ToUpper( name[0] ) + name.Substring( 1 );
        }

        /// <summary>
        /// Toes the camel casing.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public string ToCamelCasing( string name )
        {
            if( String.IsNullOrEmpty( name ) )
                return String.Empty;
            name = Utils.NormalizeName( name );
            return Char.ToLower( name[0] ) + name.Substring( 1 );
        }

        /// <summary>
        /// Creates the name of the private variable.
        /// </summary>
        /// <param name="varName">Name of the var.</param>
        /// <returns></returns>
        public virtual string CreatePrivateVariableName(string varName)
        {
            return String.Format( PrivateVariableNameFormat, ToCamelCasing(varName) );
        }

        /// <summary>
        /// Creates the name of the SQL column.
        /// </summary>
        /// <param name="componentName">Name of the component.</param>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns></returns>
        public virtual string CreateSQLColumnName(string componentName, string propertyName)
        {
            if( String.IsNullOrEmpty( componentName ) )
            {
                if( String.IsNullOrEmpty( propertyName ) )
                    return String.Empty;
                return propertyName.ToUpper();
            }
            return String.Format( ColumnNameFormat, componentName, propertyName ).ToUpper();
        }
        #endregion

    }
}
