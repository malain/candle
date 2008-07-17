using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Xml.Serialization;
using Microsoft.VisualStudio.Modeling;

namespace DSLFactory.Candle.SystemModel.Commands.Reverse
{
    /// <summary>
    /// Importation d'une assembly dans la couche des modèles
    /// </summary>
    public class ReverseModels
    {
        private readonly DataLayer _layer;
        private readonly string _initialNamespace;
        private Dictionary<DataType, Type> _classMapping = new Dictionary<DataType, Type>();

        /// <summary>
        /// Permet d'importer une assembly dans la couche modèle
        /// </summary>
        /// <param name="layer">The layer.</param>
        public ReverseModels(DataLayer layer)
        {
            if( layer == null )
                throw new ArgumentNullException("layer");

            // Namespace initial
            this._initialNamespace = layer.Namespace;
            this._layer = layer;
        }

        /// <summary>
        /// Referenceds the assemblies.
        /// </summary>
        /// <param name="assemblies">The assemblies.</param>
        public void ReferencedAssemblies(List<Assembly> assemblies)
        {
            // TODO (voir SystemDefinition.ImportAssemblies)
            //AddModelReferenceDialog dlg = new AddModelReferenceDialog();

            //using (Transaction transaction = layer.Store.TransactionManager.BeginTransaction("Import models"))
            //{
            //    foreach (Assembly asm in assemblies)
            //    {
            //        if (!asm.FullName.StartsWith("System.") && !asm.FullName.StartsWith("mscorlib"))
            //        {
            //            dlg.InitialPath = Path.GetDirectoryName(asm.GetName().Name);
            //            if( dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            //            {
            //                ExternalComponentModel sys = new ExternalComponentModel(layer.Store);
            //                sys.ModelFileNamex = dlg.FileName;
            //                sys.Version = (Version)asm.GetName().Version.Clone();
            //                layer.Component.SystemDefinition.ExternalComponents.Add( sys );
            //            }
            //        }
            //    }
            //    transaction.Commit();
            //}
        }

        /// <summary>
        /// Récupération des classes d'une assembly
        /// </summary>
        /// <param name="fullPath">The full path.</param>
        /// <param name="selectedClasses">The selected classes.</param>
        public void AddAssembly(string fullPath, IList selectedClasses)
        {
            try
            {
                using( AssemblyResolver resolver = new AssemblyResolver(fullPath) )
                {
                    using( Transaction transaction = _layer.Store.TransactionManager.BeginTransaction("Import models") )
                    {
                        transaction.Context.ContextInfo.Add("ReverseInProgress", true);

                        // 1er passage : Création des modéles 
                        foreach( Type clazz in selectedClasses )
                        {
                            // Enumeration
                            if( clazz.IsEnum )
                            {
                                CreateEnumModel(clazz);
                            }
                            else
                            {
                                _classMapping.Add(CreateModel(clazz, false), clazz);
                            }
                        }

                        // 2ème passage : Création des propriétés et des associations
                        foreach( DataType clazz in _classMapping.Keys )
                        {
                            Entity entity = clazz as Entity;
                            if( entity == null )
                                continue;

                            CreateEntity(entity, _classMapping[clazz]);

                        }
                        transaction.Commit();
                    }
                }
            }
            finally
            {
                // Marche pas 
                //  layer.Store.RuleManager.EnableRule( typeof( DSLFactory.Candle.SystemModel.Rules.TypedElementChangeRule ) );
                _classMapping = null;
            }
        }

        /// <summary>
        /// Recherche d'un modèle par son nom
        /// </summary>
        /// <param name="fullName">The full name.</param>
        /// <returns></returns>
        private DataType FindModelClassByName(string fullName)
        {
            ClassNameInfo nameHelper = new ClassNameInfo(_initialNamespace, fullName);

            // On recherche d'abord dans le modèle courant
            DataType clazz = _layer.FindType(nameHelper.FullName);
            if( clazz == null )
            {
                // Puis dans les les systèmes référencés
                foreach( ExternalComponent sys in _layer.Component.Model.ExternalComponents )
                {
                    CandleModel model = sys.ReferencedModel;
                    if( model != null && model.DataLayer != null )
                    {
                        clazz = model.DataLayer.FindType(nameHelper.FullName);
                        if( clazz != null )
                            break;
                    }
                }
            }
            return clazz;
        }

        /// <summary>
        /// Création d'un modèle de type classe
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="clazz">Clr Type initial</param>
        private void CreateEntity(Entity model, Type clazz)
        {
            model.Properties.Clear();

            // Héritage
            if( !(clazz.BaseType.FullName.StartsWith("System.")) )
            {
                model.SuperClass = _layer.SoftwareComponent.FindGlobalType(clazz.BaseType.FullName) as Entity;
            }

            // Propriétés de la classe
            foreach( PropertyInfo prop in clazz.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly) )
            {
                if( prop.CanRead )
                {
                    string xmlName = "";
                    CustomAttributeReflector car = new CustomAttributeReflector(prop);
                    if( car.MoveTo(typeof(XmlElementAttribute)))
                    {
                        xmlName = car.GetPropertyValue<string>("ElementName");
                    }

                    // Création d'une association
                    if( prop.PropertyType.IsClass && prop.PropertyType.Assembly == clazz.Assembly )
                    {
                        string typeName = GetTypeElement(prop);
                        if( typeName != null )
                        {
                            Entity target = _layer.SoftwareComponent.FindGlobalType(typeName) as Entity;
                            if( target != null )
                            {
                                Association association = model.AddAssociationTo(target);
                                association.SourceRoleName = prop.Name;
                                association.SourceMultiplicity = IsListe(prop.PropertyType) ? Multiplicity.OneMany : Multiplicity.One;
                                association.XmlName = xmlName;
                                continue;
                            }
                        }

                        typeName = "$error:unable to determine the type.";
                    }

                    // Création d'un attribut
                    DSLFactory.Candle.SystemModel.Property p = new DSLFactory.Candle.SystemModel.Property(_layer.Store);
                    p.Type = prop.PropertyType.FullName;
                    p.Name = prop.Name;
                    //p.IsPrimitive = IsTypePrimitive(prop.PropertyType);
                    p.XmlName = xmlName;
                    model.Properties.Add(p);
                }
            }
        }

        /// <summary>
        /// Création d'un modèle de type classe
        /// </summary>
        /// <param name="clazz">Type clr de la classe</param>
        /// <param name="createEnum">if set to <c>true</c> [create enum].</param>
        /// <returns></returns>
        private DataType CreateModel(Type clazz, bool createEnum)
        {
            bool classExists;
            ClassNameInfo nameHelper = new ClassNameInfo(_initialNamespace, clazz.FullName);
            DataType model = _layer.AddTypeIfNotExists(nameHelper.Namespace, clazz.Name, createEnum, out classExists);

            CustomAttributeReflector c = new CustomAttributeReflector(clazz);
            if( c.MoveTo(typeof(XmlRootAttribute)))
            {
                model.XmlName = c.GetPropertyValue<string>("ElementName");
                _layer.XmlNamespace = c.GetPropertyValue<string>("Namespace");
            }

            return model;
        }

        /// <summary>
        /// Création d'un modèle de type enum
        /// </summary>
        /// <param name="clazz">Type clr de l'enum</param>
        private void CreateEnumModel(Type clazz)
        {
            DataType obj = CreateModel(clazz, true);
            if( !(obj is Enumeration) )
            {
                ServiceLocator.Instance.IDEHelper.ShowMessage(String.Format("The enum type {0} already exists in the model like an entity, the import will be ignored. You can delete it and retry the import.", obj.Name));
                return;
            }

            Enumeration model = obj as Enumeration;

            // Les propriétés de l'enum sont ces valeurs
            foreach( FieldInfo fi in clazz.GetFields(BindingFlags.Public | BindingFlags.Static) )
            {
                EnumValue v = new EnumValue(_layer.Store);
                v.Name = fi.Name;
                v.Type = fi.FieldType.Name;
                v.Value = (int)fi.GetRawConstantValue();
                v.HasValue = true;

                CustomAttributeReflector car = new CustomAttributeReflector(fi);
                if( car.MoveTo(typeof(XmlElementAttribute)) )
                {
                    v.XmlName = car.GetPropertyValue<string>("ElementName");
                }

                model.Values.Add(v);
            }
        }

        /// <summary>
        /// Détermine si le type est une liste
        /// </summary>
        /// <param name="type">Type clr</param>
        /// <returns>vrai si c'est une liste</returns>
        private bool IsListe(Type type)
        {
            return (type.IsArray ||
                type.GetInterface("ICollection") != null ||
                type.GetInterface("IList") != null);
        }

        /// <summary>
        /// Détermine le type de l'élément d'une liste
        /// </summary>
        /// <param name="prop">Propriété</param>
        /// <returns></returns>
        private string GetTypeElement(PropertyInfo prop)
        {
            Type t = prop.PropertyType;
            if( t.IsGenericType )
            {
                // TODO si il y en a plusieurs
                Type[] args = t.GetGenericArguments();
                return args[0].FullName;
            }
            else
            {
                if( IsListe(t) )
                {
                    t = t.GetElementType();
                    if( t != null && t != typeof(object) )
                        return t.FullName;
                }
                else
                {
                    return t.FullName;
                }
            }

            CustomAttributeReflector car = new CustomAttributeReflector(prop);
            if( car.MoveTo(typeof(XmlArrayItemAttribute)) )
            {
                return car.GetPropertyValue<Type>("Type").FullName;
            }

            // TODO erreur impossible de déterminer le type
            return null;
        }

        /// <summary>
        /// Détermine si le type est un type primitif
        /// </summary>
        /// <param name="type">Type clr à tester</param>
        /// <returns>
        /// 	<c>true</c> if [is type primitive] [the specified type]; otherwise, <c>false</c>.
        /// </returns>
        private bool IsTypePrimitive(Type type)
        {
            return type.IsPrimitive ||
                type.IsEnum ||
                type == typeof(String) ||
                type == typeof(DateTime);
        }
    }


    /// <summary>
    /// Classe helper permettant de résoudre les assemblies lors de la réflection
    /// </summary>
    public class AssemblyResolver : IDisposable
    {
        private readonly string _initialAssemblyPath;
        private List<Assembly> _referencedAssemblies;

        /// <summary>
        /// Constructeur
        /// </summary>
        /// <param name="fullPath">Chemin de base de l'assembly initial</param>
        public AssemblyResolver(string fullPath)
        {
            _initialAssemblyPath = fullPath;
            _referencedAssemblies = new List<Assembly>();
            AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(CurrentDomain_AssemblyResolve);
            AppDomain.CurrentDomain.ReflectionOnlyAssemblyResolve += new ResolveEventHandler(CurrentDomain_AssemblyResolve);
        }

        /// <summary>
        /// Gets or sets the referenced assemblies.
        /// </summary>
        /// <value>The referenced assemblies.</value>
        public List<Assembly> ReferencedAssemblies
        {
            get { return _referencedAssemblies; }
            set { _referencedAssemblies = value; }
        }

        #region IDisposable Members

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            AppDomain.CurrentDomain.AssemblyResolve -= new ResolveEventHandler(CurrentDomain_AssemblyResolve);
        }
        #endregion

        /// <summary>
        /// Méthode appelée quand la clr n'arrive pas à résoudre l'assembly
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="args">The <see cref="System.ResolveEventArgs"/> instance containing the event data.</param>
        /// <returns></returns>
        Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            string name = args.Name;
            int pos = name.IndexOf(',');
            if( pos > 0 )
                name = name.Substring(0, pos).Trim();
            name += ".dll";

            Assembly assembly = null;
            try
            {
                assembly = Assembly.ReflectionOnlyLoad(args.Name);
            }
            catch
            {
                try
                {
                    assembly = Assembly.ReflectionOnlyLoadFrom(Path.Combine(_initialAssemblyPath, name));
                }
                catch
                {
                    // On demande à l'utilisateur
                    PromptBox prompt = new PromptBox("Veuillez indiquer l'emplacement de l'assembly : " + name, "Assemblies|*.dll");
                    if( prompt.ShowDialog() == System.Windows.Forms.DialogResult.OK )
                    {
                        assembly = Assembly.ReflectionOnlyLoadFrom(Path.Combine(prompt.Value, name));
                    }
                }
            }
            if( assembly != null )
                _referencedAssemblies.Add(assembly);
            return assembly;
        }

    }
}
