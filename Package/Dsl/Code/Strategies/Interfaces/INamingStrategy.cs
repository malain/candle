namespace DSLFactory.Candle.SystemModel.Strategies
{
    /// <summary>
    /// Stratégie de nommage
    /// </summary>
    public interface INamingStrategy : IPackagedStrategy
    {
        /// <summary>
        /// Pattern de création du nom du fichier générée
        /// </summary>
        /// <remarks>
        /// '~' : indique de mettre le fichier tel quel à la racine<br/>
        /// [codeFolder]|[code] : Répertoire de code<br/>
        /// [namespace]|[ns]  : namespace hierarchie<br/>
        /// <example>
        ///      [codeFolder]/[namespace]/{0}
        /// </example>
        /// </remarks>
        string DefaultGeneratedCodeFilePattern { get; }

        /// <summary>
        /// Namespace par défaut pour l'initialisation du composant
        /// </summary>
        /// <value>The default namespace.</value>
        string DefaultNamespace { get; }

        /// <summary>
        /// Gets a value indicating whether [collection as array].
        /// </summary>
        /// <value><c>true</c> if [collection as array]; otherwise, <c>false</c>.</value>
        bool CollectionAsArray { get; }

        /// <summary>
        /// Retourne le nom de l'assembly
        /// </summary>
        /// <param name="element">Type de la couche</param>
        /// <returns>Nom d'assembly avec son extension</returns>
        string CreateAssemblyName(SoftwareLayer element);

        /// <summary>
        /// Creates the namespace.
        /// </summary>
        /// <param name="appNamespace">The app namespace.</param>
        /// <param name="projectName">Name of the project.</param>
        /// <param name="element">The element.</param>
        /// <returns></returns>
        string CreateNamespace(string appNamespace, string projectName, SoftwareLayer element);

        /// <summary>
        /// Nom par défaut d'une couche
        /// </summary>
        /// <param name="layerPackage">The layer package.</param>
        /// <param name="element">Type de la couche</param>
        /// <param name="associatedName">Name of the associated.</param>
        /// <returns></returns>
        string CreateLayerName(LayerPackage layerPackage, SoftwareLayer element, string associatedName);

        /// <summary>
        /// Creates the name of the element.
        /// </summary>
        /// <param name="layer">The layer.</param>
        /// <param name="initialName">The initial name.</param>
        /// <returns></returns>
        string CreateElementName(SoftwareLayer layer, string initialName);

        /// <summary>
        /// Determines whether [is class name valid] [the specified class name].
        /// </summary>
        /// <param name="className">Name of the class.</param>
        /// <returns>
        /// 	<c>true</c> if [is class name valid] [the specified class name]; otherwise, <c>false</c>.
        /// </returns>
        bool IsClassNameValid(string className);

        /// <summary>
        /// Determines whether [is namespace valid] [the specified @namespace].
        /// </summary>
        /// <param name="namespace">The @namespace.</param>
        /// <returns>
        /// 	<c>true</c> if [is namespace valid] [the specified @namespace]; otherwise, <c>false</c>.
        /// </returns>
        bool IsNamespaceValid(string @namespace);

        /// <summary>
        /// Creates the name of the private variable.
        /// </summary>
        /// <param name="p">The p.</param>
        /// <returns></returns>
        string CreatePrivateVariableName(string p);

        /// <summary>
        /// Creates the name of the SQL column.
        /// </summary>
        /// <param name="componentName">Name of the component.</param>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns></returns>
        string CreateSQLColumnName(string componentName, string propertyName);

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="name"></param>
        ///// <returns></returns>
        //string CreateEntityName( string name );
        /// <summary>
        /// Toes the camel casing.
        /// </summary>
        /// <param name="var">The var.</param>
        /// <returns></returns>
        string ToCamelCasing(string var);

        /// <summary>
        /// Toes the pascal casing.
        /// </summary>
        /// <param name="var">The var.</param>
        /// <returns></returns>
        string ToPascalCasing(string var);

        /// <summary>
        /// Creates the name of the project folder.
        /// </summary>
        /// <param name="layer">The layer.</param>
        /// <param name="packageLayerName">Name of the package layer.</param>
        /// <returns></returns>
        string CreateProjectFolderName(SoftwareLayer layer, string packageLayerName);

        /// <summary>
        /// Nom du layer package
        /// </summary>
        /// <param name="layer">The layer.</param>
        /// <returns></returns>
        string GetLayerName(Layer layer);
    }
}