using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Xml;
using Microsoft.VisualStudio.Modeling;

namespace DSLFactory.Candle.SystemModel.Commands.Reverse
{
    /// <summary>
    /// Import d'un modéle XMI
    /// </summary>
    class XmiImporter
    {
        private XmlDocument _xdoc;
        private XmlNamespaceManager _nsManager;
        private readonly DataLayer _layer;
        private readonly string _initialNamespace;

        private readonly Dictionary<string, string> _classRefCache = new Dictionary<string, string>();
        private readonly Dictionary<string, string> _typeRefCache = new Dictionary<string, string>();

        /// <summary>
        /// Initializes a new instance of the <see cref="XmiImporter"/> class.
        /// </summary>
        /// <param name="layer">The layer.</param>
        public XmiImporter(DataLayer layer)
        {
            this._layer = layer;
            this._initialNamespace = layer.Namespace;
        }

        /// <summary>
        /// Importation du modèle
        /// </summary>
        /// <param name="filePath">The file path.</param>
        public void Import(string filePath)
        {

            _xdoc = new XmlDocument();
            _xdoc.Load(filePath);
            _nsManager = new XmlNamespaceManager(_xdoc.NameTable);
            _nsManager.AddNamespace("UML", "org.omg.xmi.namespace.UML");

            CheckVersion();

            using (Transaction transaction = _layer.Store.TransactionManager.BeginTransaction("Import models"))
            {
                transaction.Context.ContextInfo.Add("ReverseInProgress", true);

                //layer.Component.Namespace = xdoc.SelectSingleNode("/XMI/XMI.content/UML:Model", nsManager).Attributes["name"].Value;
                ReadTypes();
                ReadClasses();
                ReadAssociations();
                ReadGeneralizations();
                transaction.Commit();
            }
        }

        /// <summary>
        /// Reads the types.
        /// </summary>
        private void ReadTypes()
        {
            XmlNodeList nodes = _xdoc.SelectNodes("/XMI/XMI.content/UML:Model/UML:Namespace.ownedElement/UML:Class", _nsManager);
            foreach (XmlNode classNode in nodes)
            {
                if (classNode.Attributes["name"] != null)
                    _classRefCache.Add(classNode.Attributes["xmi.id"].Value, classNode.Attributes["name"].Value);
            }

            nodes = _xdoc.SelectNodes("/XMI/XMI.content/UML:Model/UML:Namespace.ownedElement/UML:DataType", _nsManager);
            foreach (XmlNode classNode in nodes)
            {
                if( classNode.Attributes["name"] !=null)
                    _typeRefCache.Add(classNode.Attributes["xmi.id"].Value, classNode.Attributes["name"].Value);
            }
        }

        /// <summary>
        /// Reads the classes.
        /// </summary>
        private void ReadClasses()
        {
            XmlNodeList classes = _xdoc.SelectNodes("/XMI/XMI.content/UML:Model/UML:Namespace.ownedElement/UML:Class", _nsManager);
            foreach (XmlNode classNode in classes)
            {
                Entity clazz = CreateModel(classNode);
                clazz.IsAbstract = classNode.Attributes["isAbstract"].Value == "true";

                foreach (XmlNode attrNode in classNode.SelectNodes("UML:Classifier.feature/UML:Attribute", _nsManager))
                {
                    Property p = new Property(_layer.Store);
                    string typeName;
                    bool isPrimitive;
                    GetTypeInfo( attrNode.SelectSingleNode("UML:StructuralFeature.type/UML:DataType", _nsManager).Attributes["xmi.idref"].Value,
                        out typeName, out isPrimitive );
                    p.Type = CreateFullName(typeName);
                    p.Name = attrNode.Attributes["name"].Value;
                    clazz.Properties.Add(p);
                }
            }
        }

        /// <summary>
        /// Création d'un full name avec changement de namespace si nécessaire
        /// </summary>
        /// <param name="initialFullName">Initial name of the full.</param>
        /// <returns></returns>
        private string CreateFullName(string initialFullName)
        {
            if (initialFullName.StartsWith("System."))
                return initialFullName;

            int pos = initialFullName.LastIndexOf('.');
            if (pos > 0)
                initialFullName = String.Format("{0}{1}", _layer.Namespace, initialFullName.Substring(pos));
            return initialFullName;
        }

        /// <summary>
        /// Gets the type info.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="name">The name.</param>
        /// <param name="isPrimitive">if set to <c>true</c> [is primitive].</param>
        private void GetTypeInfo(string id, out string name, out bool isPrimitive)
        {
            isPrimitive = false;
            if (_classRefCache.TryGetValue(id, out name))
                return;
            isPrimitive = true;
            name = _typeRefCache[id];
        }

        /// <summary>
        /// Reads the generalizations.
        /// </summary>
        private void ReadGeneralizations()
        {
            foreach (XmlNode node in _xdoc.SelectNodes("/XMI/XMI.content/UML:Model/UML:Namespace.ownedElement/UML:Generalization", _nsManager))
            {
                string typeName;
                bool isPrimitive;
                bool classExists;
                XmlNode node2 = node.SelectSingleNode("UML:Generalization.parent/UML:Class", _nsManager);
                string id = node2.Attributes["xmi.idref"].Value;
                GetTypeInfo(id, out typeName, out isPrimitive);

                ClassNameInfo nameHelper = new ClassNameInfo( _initialNamespace, typeName );
                Entity parent = (Entity)_layer.AddTypeIfNotExists(nameHelper, false, out classExists);

                Debug.Assert(isPrimitive == false && classExists == true);
                node2 = node.SelectSingleNode("UML:Generalization.child/UML:Class", _nsManager);
                id = node2.Attributes["xmi.idref"].Value;
                GetTypeInfo(id, out typeName, out isPrimitive);
                nameHelper = new ClassNameInfo(_initialNamespace, typeName);
                Entity child = (Entity)_layer.AddTypeIfNotExists( nameHelper, false, out classExists );
                Debug.Assert(isPrimitive == false && classExists == true);
                
                child.BaseType = parent.Name;
            }
        }

        /// <summary>
        /// Reads the associations.
        /// </summary>
        private void ReadAssociations()
        {
            foreach (XmlNode assocNode in _xdoc.SelectNodes("/XMI/XMI.content/UML:Model/UML:Namespace.ownedElement/UML:Association/UML:Association.connection", _nsManager))
            {
                XmlNodeList assocEndNodes = assocNode.SelectNodes("UML:AssociationEnd", _nsManager);

                string typeName;
                bool isPrimitive;
                bool classExists;
         
                XmlNode node2 = assocEndNodes[0].SelectSingleNode("UML:AssociationEnd.participant/UML:Class", _nsManager);
                string id = node2.Attributes["xmi.idref"].Value;
                GetTypeInfo(id, out typeName, out isPrimitive);

                ClassNameInfo nameHelper = new ClassNameInfo( _initialNamespace, typeName );
                Entity source = (Entity)_layer.AddTypeIfNotExists( nameHelper, false, out classExists );
                Debug.Assert(isPrimitive == false && classExists == true);
                XmlNode multiplicityNode = assocEndNodes[0].SelectSingleNode("UML:AssociationEnd.multiplicity/UML:Multiplicity/UML:Multiplicity.range/UML:MultiplicityRange", _nsManager);
                Multiplicity sourceMultiplicity = GetMultiplicityFromValue(multiplicityNode.Attributes["lower"].Value, multiplicityNode.Attributes["upper"].Value);
              
                node2 = assocEndNodes[1].SelectSingleNode("UML:AssociationEnd.participant/UML:Class", _nsManager);
                id = node2.Attributes["xmi.idref"].Value;
                GetTypeInfo(id, out typeName, out isPrimitive);
                Entity target = (Entity)_layer.AddTypeIfNotExists( nameHelper, false, out classExists );
                Debug.Assert(isPrimitive == false && classExists == true);
                multiplicityNode = assocEndNodes[1].SelectSingleNode("UML:AssociationEnd.multiplicity/UML:Multiplicity/UML:Multiplicity.range/UML:MultiplicityRange", _nsManager);
                Multiplicity targetMultiplicity = GetMultiplicityFromValue(multiplicityNode.Attributes["lower"].Value, multiplicityNode.Attributes["upper"].Value);

                Association assoc = source.AddAssociationTo(target);
                assoc.SourceMultiplicity = sourceMultiplicity;
                assoc.TargetMultiplicity = targetMultiplicity;
            }
        }

        /// <summary>
        /// Gets the multiplicity from value.
        /// </summary>
        /// <param name="lower">The lower.</param>
        /// <param name="upper">The upper.</param>
        /// <returns></returns>
        private Multiplicity GetMultiplicityFromValue( string lower, string upper )
        {
            int nLower = Int16.Parse(lower);
            int nUpper = Int16.Parse(upper);
            if (nLower == 0 || nLower == 1)
            {
                if (nUpper < 0)
                    return Multiplicity.OneMany;
                else
                    return Multiplicity.One;
            }
            else
            {
                if (nUpper < 0)
                    return Multiplicity.ZeroMany;
                else
                    return Multiplicity.OneMany;
            }
        }

        /// <summary>
        /// Checks the version.
        /// </summary>
        private void CheckVersion()
        {
            if (_xdoc.DocumentElement.Attributes["xmi.version"].Value != "1.2")
                throw new Exception("Bad version. Only version 1.2 is supported.");
        }


        /// <summary>
        /// Creates the model.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <returns></returns>
        private Entity CreateModel(XmlNode node)
        {
            string typeName = node.Attributes["name"].Value;
            bool classExists;
            ClassNameInfo nameHelper = new ClassNameInfo( _initialNamespace, typeName );
            Entity model = (Entity)_layer.AddTypeIfNotExists( nameHelper, false, out classExists );
            model.Properties.Clear();
            return model;
        }
    }
}
