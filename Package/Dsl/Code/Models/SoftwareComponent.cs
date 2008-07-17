using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Microsoft.VisualStudio.Modeling;
using System.Text;
using DSLFactory.Candle.SystemModel.Rules.Wizards;
using DSLFactory.Candle.SystemModel.Strategies;
using System.Xml;
using DSLFactory.Candle.SystemModel.Dependencies;

namespace DSLFactory.Candle.SystemModel
{
    /// <summary>
    /// La couche modèle est obligatoire pour pouvoir définir des nouveaux types
    /// </summary>
    public class DataLayerNotDefinedException : Exception
    {
    }

    /// <summary>
    /// 
    /// </summary>
    partial class Component
    {
        /// <summary>
        /// Gets the strategies owner.
        /// </summary>
        /// <value>The strategies owner.</value>
        public override CandleElement StrategiesOwner
        {
            get { return this; }
        }

        /// <summary>
        /// Gets the owner.
        /// </summary>
        /// <value>The owner.</value>
        public override ICustomizableElement Owner
        {
            get { return this.Model; }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    partial class SoftwareComponent : DSLFactory.Candle.SystemModel.Dependencies.IHasReferences, IProvidesNamespaceResolver
    {
        private StandardNamespaceResolver _fullNameResolver;

        /// <summary>
        /// Permet de calculer le namespace par rapport au contexte (Composant courant ou composant externe)
        /// </summary>
        public StandardNamespaceResolver NamespaceResolver
        {
            get {
                if (_fullNameResolver == null)
                    _fullNameResolver = new StandardNamespaceResolver();
                return _fullNameResolver; 
            }
            set { _fullNameResolver = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public class LayerLevelComparer : IComparer<int>
        {
            #region IComparer<int> Members

            /// <summary>
            /// Compares two objects and returns a value indicating whether one is less than, equal to, or greater than the other.
            /// </summary>
            /// <param name="x">The first object to compare.</param>
            /// <param name="y">The second object to compare.</param>
            /// <returns>
            /// Value Condition Less than zerox is less than y.Zerox equals y.Greater than zerox is greater than y.
            /// </returns>
            public int Compare(int x, int y)
            {
                return y - x;
            }

            #endregion
        }

        private static Dictionary<Guid, int> s_layerLevelCache = new Dictionary<Guid, int>();

        /// <summary>
        /// Merges the disconnect layer.
        /// </summary>
        /// <param name="sourceElement">The source element.</param>
        private void MergeDisconnectLayer( Microsoft.VisualStudio.Modeling.ModelElement sourceElement )
        {
            SoftwareLayer layer = sourceElement as SoftwareLayer;
            if( layer != null )
            {
                foreach( ElementLink link in SoftwareComponentHasLayers.GetLinks( this, layer ) )
                {
                    // Delete the link, but without possible delete propagation to the element since it's moving to a new location.
                    link.Delete(SoftwareComponentHasLayers.SoftwareComponentDomainRoleId, SoftwareComponentHasLayers.SoftwareLayerDomainRoleId);
                }
            }
        }

        /// <summary>
        /// Merges the disconnect layer package.
        /// </summary>
        /// <param name="sourceElement">The source element.</param>
        private void MergeDisconnectLayerPackage(Microsoft.VisualStudio.Modeling.ModelElement sourceElement)
        {
            // Delete link for path ComponentHasLayerPackages.LayerPackages
            LayerPackage lp = sourceElement as LayerPackage;
            foreach (ElementLink link in global::DSLFactory.Candle.SystemModel.ComponentHasLayerPackages.GetLinks((global::DSLFactory.Candle.SystemModel.SoftwareComponent)this, (LayerPackage)sourceElement))
            {
                // Delete the link, but without possible delete propagation to the element since it's moving to a new location.
                link.Delete(global::DSLFactory.Candle.SystemModel.ComponentHasLayerPackages.ComponentDomainRoleId, global::DSLFactory.Candle.SystemModel.ComponentHasLayerPackages.LayerPackageDomainRoleId);
            }
        }

        /// <summary>
        /// A la création d'un package, on ajoute sa couche d'interface
        /// </summary>
        /// <param name="sourceElement">The source element.</param>
        /// <param name="elementGroup">The element group.</param>
        private void MergeRelateLayerPackage(Microsoft.VisualStudio.Modeling.ModelElement sourceElement, Microsoft.VisualStudio.Modeling.ElementGroup elementGroup)
        {
            // Ajout du package
            LayerPackage layerPackage = sourceElement as LayerPackage;
            this.LayerPackages.Add(layerPackage);

            if (this.Store.TransactionManager.CurrentTransaction.IsSerializing || this.Store.InUndoRedoOrRollback)
                return;

            // Recherche si il existe une couche d'interface de ce niveau
            short newLevel = (short)(layerPackage.Level + 1);
            // Pas d'interface pour la couche UI
            if (layerPackage.InterfaceLayer == null ) //&& layerPackage.Level != 100) // UIWorkflowLayer.Level
            {
                InterfaceLayer il = new InterfaceLayer(layerPackage.Store);
                il.Level = layerPackage.LayerLevel;
                layerPackage.InterfaceLayer = il;
                this.Layers.Add(il);
            }
        }

        /// <summary>
        /// Peut on merger un layer sur le composant
        /// </summary>
        /// <param name="rootElement">The root element.</param>
        /// <param name="elementGroupPrototype">The element group prototype.</param>
        /// <returns>
        /// 	<c>true</c> if this instance [can merge software layer] the specified root element; otherwise, <c>false</c>.
        /// </returns>
        private bool CanMergeSoftwareLayer( Microsoft.VisualStudio.Modeling.ProtoElementBase rootElement, Microsoft.VisualStudio.Modeling.ElementGroupPrototype elementGroupPrototype )
        {
            if (rootElement.DomainClassId == DataLayer.DomainClassId && this.IsDataLayerExists)
                return false;

            int sourceLayerLevel = GetLayerLevel( rootElement.DomainClassId );

            // On peut merger un layer sur le composant si il n'existe pas de packageLayer de même niveau
            foreach( LayerPackage package in this.LayerPackages )
            {
                if( package.Level == sourceLayerLevel )
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Récupére le niveau d'un layer. Comme cette méthode est appelé en ne fournissant que le proto, il
        /// est nécessaire d'instancier le layer pour récupérer son niveau puis d'annuler sa création. Le niveau
        /// est stocké en cache.
        /// </summary>
        /// <param name="domainClassId">The domain class id.</param>
        /// <returns></returns>
        internal int GetLayerLevel(Guid domainClassId)
        {
            int sourceLayerLevel;
            //if( rootElement.DomainClassId == DataLayer.DomainClassId )
            //    return 1000;

            if( !s_layerLevelCache.TryGetValue( domainClassId, out sourceLayerLevel ) )
            {
                // Pour connaitre le niveau du layer, il faut l'instancier. On utilise un cache 
                DomainClassInfo classInfo = this.Store.DomainDataDirectory.GetDomainClass( domainClassId );
                using( Transaction transaction =  Store.TransactionManager.BeginTransaction( "dummy transaction" ) )
                {
                    Layer sourceLayer = Store.ElementFactory.CreateElement( classInfo ) as Layer;
                    sourceLayerLevel = sourceLayer.Level;
                    s_layerLevelCache.Add( domainClassId, sourceLayerLevel );
                    transaction.Rollback();
                }
            }
            return sourceLayerLevel;
        }

        /// <summary>
        /// Liste des contrats public du composant
        /// </summary>
        public List<TypeWithOperations> PublicContracts
        {
            get
            {
                List<TypeWithOperations> ports = new List<TypeWithOperations>();

                if (this.Model.IsLibrary)
                {
                    // Dans le cas d'une library, on publie tous les contrats de toutes les couches d'interfaces
                    foreach (AbstractLayer layer in this.Layers)
                    {
                        if (layer is InterfaceLayer)
                        {
                            foreach (ServiceContract contract in ((InterfaceLayer)layer).ServiceContracts)
                            {
                                ports.Add(contract);
                            }
                        }
                    }
                }
                else
                {
                    // Recherche de la couche d'interface publique
                    InterfaceLayer iLayer = null;

                    // Par défaut, c'est celle de la couche principale
                    Layer mainLayer = GetMainLayer();
                    if (mainLayer != null)
                    {
                        iLayer = mainLayer.LayerPackage.InterfaceLayer;
                    }
                    else
                    {
                        // Si il n'y a pas de couche publique, on regarde si ce n'est pas juste une description de
                        // service web (1 couche d'interface mais pas de couche d'implémentation
                        if (this.LayerPackages.Count == 1 && this.LayerPackages[0].Layers.Count == 0)
                            iLayer = this.LayerPackages[0].InterfaceLayer;
                    }

                    if (iLayer != null)
                    {
                        foreach (ServiceContract contract in iLayer.ServiceContracts)
                        {
                            ports.Add(contract);
                        }
                    }
                }
                return ports;
            }
        }

        /// <summary>
        /// Allows the model element to configure itself immediately after the Merge process has related it to the target element.
        /// </summary>
        /// <param name="elementGroup">The group of source elements that have been added back into the target store.</param>
        protected override void MergeConfigure( ElementGroup elementGroup )
        {
            base.MergeConfigure( elementGroup );
            this.Name = "?"; // Pour forcer l'affichage du wizard (voir AddRule)
        }

        /// <summary>
        /// Lors de l'ajout d'un layer, on crée automatiquement le packageLayer qui le contient
        /// </summary>
        /// <param name="sourceElement">The source element.</param>
        /// <param name="elementGroup">The element group.</param>
        private void MergeRelateLayer(Microsoft.VisualStudio.Modeling.ModelElement sourceElement, Microsoft.VisualStudio.Modeling.ElementGroup elementGroup)
        {
            Layer sourceLayer = sourceElement as Layer;
            if (sourceLayer != null)
            {
                // Un Layer est toujours dans un LayerPackage
                if (this.Store.TransactionManager.CurrentTransaction.TopLevelTransaction.IsSerializing || this.Store.InUndoRedoOrRollback)
                    return;

                // Recherche du package layer associé et si il n'existe pas on le crée
                LayerPackage parentPackage = null;
                foreach (LayerPackage package in this.LayerPackages)
                {
                    if (package.Level == sourceLayer.Level)
                    {
                        parentPackage = package;
                        break;
                    }
                }

                // Transaction pour forcer la création du package avant l'ajout du layer
                // cf LayerPackageInsertRule
                using (Transaction transaction = this.Store.TransactionManager.BeginTransaction("Add layer package"))
                {
                    if (parentPackage == null)
                    {
                        parentPackage = new LayerPackage(this.Store);
                        parentPackage.Level = (short)sourceLayer.Level;
                        MergeRelateLayerPackage(parentPackage, null);
                    }

                    // Ajout
                    parentPackage.Layers.Add(sourceLayer);
                    transaction.Commit();
                }
            }

            this.Layers.Add(sourceElement as SoftwareLayer);
        }

        /// <summary>
        /// Gets a value indicating whether this instance is data layer exists.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is data layer exists; otherwise, <c>false</c>.
        /// </value>
        public bool IsDataLayerExists
        {
            get
            {
                return this.Store.ElementDirectory.FindElements<DataLayer>().Count > 0;
            }
        }

        /// <summary>
        /// Accés à la couche modéle
        /// </summary>
        public DataLayer DataLayer
        {
            get
            {
                IList<DataLayer> list = this.Store.ElementDirectory.FindElements<DataLayer>();
                if (list.Count == 0)
                {
                    using (Transaction transaction = this.Store.TransactionManager.BeginTransaction("Add models layer"))
                    {
                        DataLayer model = new DataLayer(this.Store);
                        this.Layers.Add(model);
                        DomainClassInfo.SetUniqueName(model, StrategyManager.GetInstance(this.Store).NamingStrategy.CreateLayerName(null, model, model.Name));
                        model.Namespace = StrategyManager.GetInstance(this.Store).NamingStrategy.CreateNamespace(this.Namespace, model.Name, model);
                        transaction.Commit();
                        return model;
                    }
                }
                return list[0];
            }
        }

        /// <summary>
        /// Liste des types pouvant être utilisés pour déclarer une opération ou une propriété
        /// </summary>
        /// <returns></returns>
        public IList<string> GetDefinedTypeNames()
        {
            return GetDefinedTypeNames( null );
        }

        /// <summary>
        /// Liste des types pouvant être utilisés pour déclarer une opération ou une propriété
        /// </summary>
        /// <param name="typeNames">Liste initiale</param>
        /// <returns></returns>
        public IList<string> GetDefinedTypeNames( List<string> typeNames )
        {
            if( typeNames == null )
                typeNames = new List<string>();

            // Récupération des types définis dans la couche modele
            if (this.IsDataLayerExists)
            {
                foreach( string clazzName in this.DataLayer.GetAllTypeNames(false) )
                {
                    typeNames.Add( clazzName );
                }
            }

            typeNames.AddRange( StrategyManager.GetInstance(this.Store).TargetLanguage.StandardTypes );
            return typeNames;
        }

        /// <summary>
        /// Retourne la couche marquée comme principale
        /// </summary>
        /// <returns></returns>
        public virtual Layer GetMainLayer()
        {
            Layer publicLayer = null;
            foreach (AbstractLayer aLayer in Layers)
            {
                Layer layer = aLayer as Layer;
                if( layer==null)
                    continue;

                if (this.Model.IsLibrary)
                {
                    // On prend la plus haute
                    if (publicLayer == null || publicLayer.Level < layer.Level)
                        publicLayer = layer;
                }
                else
                {
                    if (layer is Layer && ((Layer)layer).StartupProject)
                        return layer as Layer;
                }
            }

            return publicLayer;
        }

        /// <summary>
        /// Suggére le couche la plus appropriée pour être la couche principale
        /// </summary>
        /// <param name="excludedLayer">Couche à ne pas tenir compte</param>
        /// <returns></returns>
        internal Layer SuggestMainLayer(Layer excludedLayer)
        {
            // On recherche la couche la plus haute (Si on en trouve plusieurs, on prend la
            // 1ère qui a un contexte d'exècution)
            Layer theLayer=null;
            foreach (AbstractLayer layer in Layers)
            {
                if (layer is Layer && layer != excludedLayer)
                {
                    Int16 level = ((Layer)layer).Level;
                    if (theLayer == null || level > theLayer.Level || (level == theLayer.Level && theLayer.HostingContext == HostingContext.None))
                        theLayer = layer as Layer;
                }
            }
            return theLayer;
        }

        /// <summary>
        /// Recherche d'un modele dans l'application et dans tous les systèmes référencés
        /// </summary>
        /// <param name="name">Nom</param>
        /// <returns></returns>
        public DataType FindGlobalType( string name )
        {
            if( string.IsNullOrEmpty( name ) || name == "void" )
                return null;

            if (IsDataLayerExists)
            {
                return DataLayer.FindGlobalType( name );
            }

            return null;
        }

        /// <summary>
        /// Finds the name of all types from single.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public List<DataType> FindAllTypesFromSingleName( string name )
        {
            List<DataType> result = new List<DataType>();

            if( string.IsNullOrEmpty( name ) || name == "void" || name.IndexOf( '.' ) >= 0 )
                return result;

            if (IsDataLayerExists)
            {
                result.AddRange( DataLayer.FindAllTypesFromSingleName( name ) );
            }

            return result;
        }

        /// <summary>
        /// Determines whether this instance [can define type] the specified new type name.
        /// </summary>
        /// <param name="newTypeName">New name of the type.</param>
        /// <returns>
        /// 	<c>true</c> if this instance [can define type] the specified new type name; otherwise, <c>false</c>.
        /// </returns>
        internal bool CanDefineType( string newTypeName )
        {
            return true;
        }

        /// <summary>
        /// Création d'un type si il n'existe pas.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public string CreateTypeIfNotExists( string type )
        {
            DataType t = null;
            // le test suivant ne peut pas être null car son appel va générer la création
            // de la couche modèles si elle n'existe pas. C'est bien ce que l'on veut
            if( DataLayer != null )
            {
                // On va demander confirmation pour créer le type.
                t = FindGlobalType( type );
                if( t == null )
                {
                    // Si le type ne contient pas de ., on va chercher ce type dans tous
                    // les namespaces.
                    if( type.IndexOf( '.' ) <= 0 )
                    {
                        List<DataType> types = FindAllTypesFromSingleName( type );
                        // Si il n'y a qu'un, on le prend
                        if( types.Count == 1 )
                            return types[0].FullName;

                        // Si il y en a plusieurs, il faut le sélectionner
                        if( types.Count != 0 )
                        {
                            TypeSelectorDialog dlg = new TypeSelectorDialog( types );
                            switch( dlg.ShowDialog() )
                            {
                                // Il veut en créer
                                case DialogResult.Cancel:
                                    t = EnsureTypeExists( type );
                                    break;
                                // On ne fait plus rien, il garde le nom initial
                                case DialogResult.Ignore:
                                    return type;
                                // Il en a sélectionné un
                                case DialogResult.OK:
                                    return dlg.SelectedType;
                            }
                        }
                    }

                    IIDEHelper ide = ServiceLocator.Instance.GetService<IIDEHelper>();
                    if (ide != null)
                    {
                        if (ide.ShowMessageBox(String.Format("Do you want to create the type '{0}' in the models layer ?", type), "Warning", MessageBoxButtons.YesNo) == DialogResult.Yes)
                        {
                            t = EnsureTypeExists(type);
                        }
                    }
                }
            }

            return t == null ? type : t.FullName;
        }

        /// <summary>
        /// Vérification si un type existe et le crée le cas échéant (y compris les packages)
        /// </summary>
        /// <param name="newTypeName">Nom complet du type</param>
        /// <returns>Une instance du type</returns>
        public DataType EnsureTypeExists( string newTypeName )
        {
            if( String.IsNullOrEmpty( newTypeName ) )
                return null;

            // le test suivant ne peut pas être null car son appel va générer la création
            // de la couche modèles si elle n'existe pas. C'est bien ce que l'on veut
            if (DataLayer == null)
            {
                IIDEHelper ide = ServiceLocator.Instance.GetService<IIDEHelper>();
                if (ide != null)
                {
                    ide.ShowMessage(String.Format("Can't create user type '{0}' because the models layer does not exist.", newTypeName));
                }
                throw new DataLayerNotDefinedException();
            }

            if( !IsDataLayerExists)
                return null;

            if (this.Store.TransactionManager.CurrentTransaction.Context.ContextInfo.ContainsKey("ReverseInProgress"))
                return null;

            DataType newDefinedTypeModel = null;
            DataLayer modelsLayer = DataLayer;

            using( Transaction transaction = this.Store.TransactionManager.BeginTransaction( "Add class to dataLayer" ) )
            {
                transaction.Context.ContextInfo.Add( "Generated", true );

                string typeName = newTypeName;
                ClassNameInfo cnh = new ClassNameInfo( typeName );

                // Si il n'y a pas de namespace, on force le namespace par défaut.
                if( String.IsNullOrEmpty( cnh.Namespace ) )
                {
                    // Si il y a plusieurs packages, on va lui demander d'en sélectionner un
                    if (modelsLayer.Packages.Count > 1)
                    {
                        NamespacesSelectorDialog dlg = new NamespacesSelectorDialog(modelsLayer.Packages);
                        if( dlg.ShowDialog() == DialogResult.Cancel )
                            return null;
                        cnh.Namespace = dlg.SelectedNamespace;
                    }
                    else
                        cnh.Namespace = modelsLayer.DefaultPackage.Name;
                }

                // Recherche si le type existe dans un des packages
                newDefinedTypeModel = modelsLayer.FindType(cnh.Namespace, cnh.ClassName);
                if( newDefinedTypeModel == null )
                {
                    bool classExists;
                    newDefinedTypeModel = modelsLayer.AddTypeIfNotExists(cnh, false, out classExists);
                }

                transaction.Commit();
            }
            return newDefinedTypeModel;
        }

        /// <summary>
        /// Gets the references.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        public System.Collections.Generic.IEnumerable<ReferenceItem> GetReferences(ReferenceContext context)
        {
            // Si il y a des ports sélectionnés, on ne prend que ceux la + le modelsLayer
            // sauf si on est en Runtime et que le modèle est une library (si cette library est dans le GAC, le layer n'a pas du proposer cette dépendance en runtime cf AbstractLayer).
            if (context.Ports != null && ((context.Scope != ReferenceScope.Runtime || !this.Model.IsLibrary) || context.Scope == ReferenceScope.All))
            {
                foreach (Guid portId in context.Ports)
                {
                    ServiceContract contract = this.Store.ElementDirectory.FindElement(portId) as ServiceContract;
                    if (contract != null )
                        yield return new ReferenceItem(this, contract.Layer, context.IsExternal);
                }

                if (IsDataLayerExists)
                {
                    yield return new ReferenceItem(this, this.DataLayer, context.IsExternal);
                }
            }
            // Sinon on prend tout
            else {
                foreach (SoftwareLayer layer in this.Layers)
                    yield return new ReferenceItem(this, layer, context.IsExternal);
            }
        }

    }
}
