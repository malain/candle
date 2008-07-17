using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.Modeling;
using DSLFactory.Candle.SystemModel.Strategies;
using DSLFactory.Candle.SystemModel.Utilities;
using DSLFactory.Candle.SystemModel.Dependencies;

namespace DSLFactory.Candle.SystemModel
{
    /// <summary>
    /// 
    /// </summary>
    partial class DataLayer
    {
        /// <summary>
        /// 
        /// </summary>
        class ExternalDataLayerVisitor : IReferenceVisitor
        {
            List<DataType> _types;
            List<Guid> _models;

            /// <summary>
            /// Initializes a new instance of the <see cref="ExternalDataLayerVisitor"/> class.
            /// </summary>
            /// <param name="types">The types.</param>
            /// <param name="models">The models.</param>
            public ExternalDataLayerVisitor(List<DataType> types, List<Guid> models)
            {
                this._types = types;
                this._models = models;
            }

            #region IReferenceVisitor Members

            /// <summary>
            /// Accepts the specified item.
            /// </summary>
            /// <param name="item">The item.</param>
            /// <returns></returns>
            public bool Accept(ReferenceItem item)
            {
                if (item.Element is CandleModel)
                {
                    CandleModel model = item.Element as CandleModel;
                    if (!_models.Contains(model.Id) && model.SoftwareComponent != null && model.SoftwareComponent.IsDataLayerExists)
                    {
                        _types.AddRange(model.DataLayer.GetAllTypes(null, true));
                        _models.Add(model.Id);
                    }
                }
                return true;
            }

            /// <summary>
            /// Exits the element.
            /// </summary>
            /// <param name="item">The item.</param>
            public void ExitElement(ReferenceItem item)
            {
            }

            #endregion
        }

        /// <summary>
        /// Merges the configure.
        /// </summary>
        /// <param name="elementGroup">The element group.</param>
        protected override void MergeConfigure(Microsoft.VisualStudio.Modeling.ElementGroup elementGroup)
        {
            base.MergeConfigure(elementGroup);

            DomainClassInfo.SetUniqueName(this, StrategyManager.GetInstance(this.Store).NamingStrategy.CreateLayerName(null, this, this.Name));
            Namespace = StrategyManager.GetInstance(this.Store).NamingStrategy.CreateNamespace(Component.Namespace, Name, this);
        }

        /// <summary>
        /// Gets the references.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        public override IEnumerable<DSLFactory.Candle.SystemModel.Dependencies.ReferenceItem> GetReferences(DSLFactory.Candle.SystemModel.Dependencies.ReferenceContext context)
        {
            IList<DataLayerReferencesExternalComponent> list = DataLayerReferencesExternalComponent.GetLinksToReferencedExternalComponents(this);
            foreach (DataLayerReferencesExternalComponent link in list)
            {
                if (context.Mode.CheckConfigurationMode(link.ConfigurationMode) && context.CheckScope(link.Scope))
                {
                    CandleModel model = link.ReferencedExternalComponent.ReferencedModel;
                    if (model != null && model.DataLayer != null)
                    {
                        List<Guid> ports = new List<Guid>();
                        ports.Add(model.DataLayer.Id);
                        yield return new DSLFactory.Candle.SystemModel.Dependencies.ReferenceItem(this, model.DataLayer, context.Scope, ports, true);
                    }
                }
            }

            foreach (DSLFactory.Candle.SystemModel.Dependencies.ReferenceItem ri in base.GetReferences(context))
                yield return ri;        
        }

        /// <summary>
        /// Finds the type of the global.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public DataType FindGlobalType(string name)
        {
            DataType type = this.FindType(name);
            if (type == null)
            {
                foreach (ExternalComponent sys in this.ReferencedExternalComponents)
                {
                    CandleModel model = sys.ReferencedModel;
                    if (model == null || model.DataLayer == null)
                        return null;

                    type = model.DataLayer.FindType(name);
                    if (type != null)
                        return type;
                }
            }
            return type;
        }

        /// <summary>
        /// Liste de tous les types disponibles dans le modèle
        /// </summary>
        /// <param name="sourceLayer">Source à partir duquel on veut récupérer les types (null pour récupérer à partir du modelsLayer)</param>
        /// <param name="includeExternalTypes">Indique si on doit inclure les types externes</param>
        /// <returns></returns>
        public IList<DataType> GetAllTypes(SoftwareLayer sourceLayer, bool includeExternalTypes)
        {
            List<DataType> types = new List<DataType>();
            foreach (Package package in this.Packages)
            {
                foreach (DataType clazz in package.Types)
                {
                    types.Add(clazz);
                }
            }

            if (includeExternalTypes)
            {
                // Référence entre les couches modèles
                List<Guid> models = new List<Guid>();
                foreach (ExternalComponent sys in this.ReferencedExternalComponents)
                {
                    CandleModel model = sys.ReferencedModel;
                    if (model != null && model.DataLayer != null)
                    {
                        models.Add(model.Id);
                        types.AddRange(model.DataLayer.AllTypes);
                    }
                }

                if (sourceLayer != null)
                {
                    ExternalDataLayerVisitor visitor = new ExternalDataLayerVisitor(types, models);
                    ReferenceWalker walker = new ReferenceWalker(ReferenceScope.Compilation, new ConfigurationMode());
                    walker.Traverse(visitor, sourceLayer);
                }
            }
            return types;
        }

        /// <summary>
        /// Liste de tous les types référencées dans le modèle y compris les externes
        /// </summary>
        public IList<DataType> AllTypes
        {
            get
            {
                return GetAllTypes(null, true);
            }
        }

         /// <summary>
        /// Liste de tous les noms de types référencées dans le modèle y compris les externes
        /// </summary>         
        public IList<string> GetAllTypeNames(bool isExternal)
        {
            List<string> list = new List<string>();
            foreach (Package package in this.Packages)
            {
                foreach (DataType clazz in package.Types)
                {
                    list.Add(isExternal ? clazz.FullName : clazz.Name);
                }
            }

            // On va trier alphabétiquement par composants
            // Tri intermédiaire sur le composant courant
            list.Sort();

            // Liste temporaire pour trier sur les composants externes
            List<string> tmp = new List<string>();
            foreach (ExternalComponent sys in this.ReferencedExternalComponents)
            {
                CandleModel model = sys.ReferencedModel;
                if (model != null && model.DataLayer != null)
                {
                    list.AddRange(model.DataLayer.GetAllTypeNames(true));
                }
            }

            // Tri puis ajout
            tmp.Sort();
            list.AddRange(tmp);
            return list;
        }

        /// <summary>
        /// Adds the type if not exists.
        /// </summary>
        /// <param name="className">Name of the class.</param>
        /// <param name="createEnum">if set to <c>true</c> [create enum].</param>
        /// <param name="classAlreadyExists">if set to <c>true</c> [class already exists].</param>
        /// <returns></returns>
        public DataType AddTypeIfNotExists( ClassNameInfo className, bool createEnum, out bool classAlreadyExists )
        {
            return AddTypeIfNotExists( className.Namespace, className.ClassName, createEnum, out classAlreadyExists );
        }

        /// <summary>
        /// Ajoute une nouvelle classe si elle n'existe pas encore
        /// </summary>
        /// <param name="packageName">Name of the package.</param>
        /// <param name="className">Nom de la classe</param>
        /// <param name="createEnum">if set to <c>true</c> [create enum].</param>
        /// <param name="classAlreadyExists">True si la classe existait dèjà</param>
        /// <returns>Retourne la nouvelle classe</returns>
        public DataType AddTypeIfNotExists( string packageName, string className, bool createEnum, out bool classAlreadyExists )
        {
            System.Diagnostics.Debug.Assert( !String.IsNullOrEmpty( packageName ) );
            System.Diagnostics.Debug.Assert( !String.IsNullOrEmpty( className ) );

            //int pos = className.IndexOf('.');
            //if( pos > 0 )
            //{
            //    packageName = className.Substring( 0, pos );
            //    className = className.Substring( pos+1 );
            //}
            Package package = AddPackageIfNotExists( packageName );

            classAlreadyExists = true;
            // Recherche si la classe existe dèjà
            foreach( DataType clazz in package.Types )
            {
                if( clazz.Name == className )
                    return clazz;
            }

            // Non on la rajoute
            classAlreadyExists = false;
            DataType modelClass;
            if( createEnum )
                modelClass = new Enumeration( this.Store );
            else
                modelClass = new Entity( this.Store );
            modelClass.Name = className;
            modelClass.RootName = className;
            package.Types.Add( modelClass );
            return modelClass;
        }

        /// <summary>
        /// Recherche d'un modèle
        /// </summary>
        /// <param name="className">Nom complet ou simple</param>
        /// <returns></returns>
        public DataType FindType( string className )
        {
            if (string.IsNullOrEmpty(className))
                return null;

            bool isFullName = className.IndexOf( '.' ) > 0;

            // recherche dans tous les packages
            foreach( Package pack in Packages )
            {
                foreach( DataType mb in pack.Types )
                {
                    if( isFullName && mb.FullName == className || !isFullName && mb.Name == className )
                        return mb;
                }
            }
            return null;
        }

        /// <summary>
        /// Finds the name of all types from single.
        /// </summary>
        /// <param name="className">Name of the class.</param>
        /// <returns></returns>
        public List<DataType> FindAllTypesFromSingleName( string className )
        {
            List<DataType> result = new List<DataType>();
            System.Diagnostics.Debug.Assert( className.IndexOf( '.' ) < 0 );

            // recherche dans tous les packages
            foreach( Package pack in Packages )
            {
                foreach( DataType mb in pack.Types )
                {
                    if( mb.Name == className )
                        result.Add( mb );
                }
            }
            return result;
        }

        /// <summary>
        /// Recherche d'un modèle
        /// </summary>
        /// <param name="packageName">Name of the package.</param>
        /// <param name="className">Name of the class.</param>
        /// <returns></returns>
        public DataType FindType( string packageName, string className )
        {
            if( packageName == null )
                throw new ArgumentNullException( "packageName" );

            Package package = FindPackage( packageName );
            if( package == null )
                return null;

            foreach( DataType clazz in package.Types )
            {
                if( clazz.Name == className )
                    return clazz;
            }

            return null;
        }

        /// <summary>
        /// Ajoute une nouvelle classe si elle n'existe pas encore
        /// </summary>
        /// <param name="packageNames">The package names.</param>
        /// <returns>Retourne le nouveau package</returns>
        public Package AddPackageIfNotExists( string packageNames )
        {
            return FindPackage( packageNames, true );
        }

        /// <summary>
        /// Finds the package.
        /// </summary>
        /// <param name="packagesNames">The packages names.</param>
        /// <returns></returns>
        public Package FindPackage( string packagesNames )
        {
            return FindPackage( packagesNames, false );
        }

        /// <summary>
        /// Finds the package.
        /// </summary>
        /// <param name="packageName">Name of the package.</param>
        /// <param name="createIfNotExists">if set to <c>true</c> [create if not exists].</param>
        /// <returns></returns>
        private Package FindPackage( string packageName, bool createIfNotExists )
        {
            if( packageName == null )
                packageName = String.Empty;

            // Recherche si le package existe dèjà
            foreach( Package package in Packages )
            {
                if( package.Name == packageName )
                {
                    return package;
                }
            }

            if( !createIfNotExists )
                return null;

            // Non on la rajoute

            Package newPackage = new Package( this.Store );
            newPackage.Name = packageName;
            Packages.Add( newPackage );

            return newPackage;
        }

        /// <summary>
        /// Retourne le package par défaut (qui est celui ayant le même nom que le modèle)
        /// </summary>
        /// <value>The default package.</value>
        public Package DefaultPackage
        {
            get
            {
                foreach( Package package in Packages )
                {
                    if( Utils.StringCompareEquals( package.Name, this.Namespace ) )
                    {
                        return package;
                    }
                }

                // Si il n'existe pas, on le crée
                using( Transaction transaction = this.Store.TransactionManager.BeginTransaction( "Create default package" ) )
                {
                    Package package = new Package( this.Store );
                    package.Name = this.Namespace;
                    this.Packages.Add( package );
                    transaction.Commit();
                    return package;
                }
            }
        }
    }
}