using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace DSLFactory.Candle.SystemModel.Strategies
{
    /// <summary>
    /// Un manifest permet de décrire une stratégie à télécharger
    /// </summary>
    [Serializable]
    [XmlRoot("manifest")]
    public class StrategyManifest
    {
        private string _fileName;

        /// <summary>
        /// Nom du package contenant le manifest. Ce nom est initialisé lors du chargement 
        /// du manifest par le package
        /// </summary>
        [XmlAttribute("package")] public string PackageName;

        /// <summary>
        /// Elément libre pour stocker des paramètres d'initialisation de la stratégie
        /// </summary>
        [XmlAnyElement] public XmlNode StrategyConfiguration;

        /// <summary>
        /// Nom du type de la strategie
        /// </summary>
        [XmlElement("strategyType")] public string StrategyTypeName;

        /// <summary>
        /// Nom du fichier (sans le chemin)
        /// </summary>
        [XmlIgnore]
        [XmlAttribute("fileName")]
        public string FileName
        {
            get { return _fileName; }
            set { _fileName = Path.GetFileName(value); }
        }

        /// <summary>
        /// Permet de faire des regroupements lors de l'affichage
        /// </summary>
        [XmlIgnore]
        public virtual string StrategyGroup
        {
            get { return GetXmlAttribute("group", String.Empty); }
        }

        /// <summary>
        /// Gets the display name.
        /// </summary>
        /// <value>The display name.</value>
        [XmlIgnore]
        public virtual string DisplayName
        {
            get { return GetXmlAttribute("name", PackageName); }
        }

        /// <summary>
        /// Gets the strategy path.
        /// </summary>
        /// <value>The strategy path.</value>
        [XmlIgnore]
        public virtual string StrategyPath
        {
            get { return GetXmlAttribute("path", String.Empty); }
        }

        /// <summary>
        /// Gets the description.
        /// </summary>
        /// <value>The description.</value>
        [XmlIgnore]
        public virtual string Description
        {
            get { return GetXmlElement("Description", String.Empty); }
        }

        /// <summary>
        /// Instancie le manifest
        /// </summary>
        /// <param name="manifestFileName">Name of the manifest file.</param>
        /// <returns></returns>
        internal static StrategyManifest DeserializeManifest(string manifestFileName)
        {
            using (StreamReader reader = new StreamReader(manifestFileName))
            {
                // Désérialization de la description
                XmlSerializer serializer = new XmlSerializer(typeof (StrategyManifest));
                StrategyManifest manifest = (StrategyManifest) serializer.Deserialize(reader);
                return manifest;
            }
        }


        /// <summary>
        /// Création d'un fichier manifest à partir d'un type
        /// </summary>
        /// <param name="strategyType">Type of the strategy.</param>
        /// <returns></returns>
        public static string CreateManifest(Type strategyType)
        {
            StrategyManifest m = new StrategyManifest();

            StrategyBase strategy = (StrategyBase) Activator.CreateInstance(strategyType ?? typeof (GenericStrategy));

            StringBuilder sb = new StringBuilder();
            using (StringWriter ms = new StringWriter(sb))
            {
                XmlSerializer serializer = new XmlSerializer(typeof (StrategyBase), new Type[] {strategyType});
                serializer.Serialize(ms, strategy);

                // On met le résultat dans le noeud dédié
                XmlDocument xdoc = new XmlDocument();
                xdoc.LoadXml(sb.ToString().Substring(39));
                m.StrategyConfiguration = xdoc.FirstChild;
            }

            if (strategyType != null)
                m.StrategyTypeName =
                    String.Format("{0},{1}", strategyType.FullName, strategyType.Assembly.GetName().Name);

            sb = new StringBuilder();
            using (StringWriter w = new StringWriter(sb))
            {
                // Désérialization de la description
                XmlSerializer serializer = new XmlSerializer(typeof (StrategyManifest));
                serializer.Serialize(w, m);
            }

            return sb.ToString();
        }

        /// <summary>
        /// Gets the XML attribute.
        /// </summary>
        /// <param name="attributeName">Name of the attribute.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns></returns>
        private string GetXmlAttribute(string attributeName, string defaultValue)
        {
            if (StrategyConfiguration != null)
            {
                try
                {
                    XmlAttribute attr = StrategyConfiguration.Attributes[attributeName];
                    if (attr != null)
                        return attr.Value;
                }
                catch
                {
                }
            }
            return defaultValue;
        }

        /// <summary>
        /// Gets the XML element.
        /// </summary>
        /// <param name="elementName">Name of the element.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns></returns>
        private string GetXmlElement(string elementName, string defaultValue)
        {
            if (StrategyConfiguration != null)
            {
                try
                {
                    XmlNode element = StrategyConfiguration.SelectSingleNode(String.Format("//{0}", elementName));
                    if (element != null)
                    {
                        return element.InnerText;
                    }
                }
                catch
                {
                }
            }
            return defaultValue;
        }
    }
}