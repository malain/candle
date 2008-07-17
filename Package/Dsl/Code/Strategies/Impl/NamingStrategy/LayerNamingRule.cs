using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Xml.Serialization;

namespace DSLFactory.Candle.SystemModel.Strategies
{
    /// <summary>
    /// Regles de nommage spécifique à une couche
    /// </summary>
    [Serializable]
    public class LayerNamingRule
    {
        private string _layerType;
        private string _defaultName;
        private string _formatString;
        private string _assemblyFormatString;
        private string _projectFolderFormatString;
        private string _elementFormatString;

        /// <summary>
        /// Type de la couche
        /// </summary>
        [XmlAttribute(AttributeName = "type")]
        [Description("Layer type name")]
        [Editor(typeof(DSLFactory.Candle.SystemModel.Editor.LayerNameEditor), typeof(System.Drawing.Design.UITypeEditor))]
        public string LayerType
        {
            get { return _layerType; }
            set { _layerType = value; }
        }

        /// <summary>
        /// Nom de la couche
        /// </summary>
        [XmlAttribute(AttributeName = "defaultName")]
        [Description("Default layer name if FormatString is empty")]
        [DisplayName("Default layer package name")]
        public string DefaultName
        {
            get { return _defaultName; }
            set { _defaultName = value; }
        }

        /// <summary>
        /// Format de chaine : 
        ///     {0} = Nom de la couche par défaut, 
        ///     {1} = Nom du composant, 
        ///     {2} = Namespace du composant
        ///     {3} = Nom du package
        ///     {4} = Nom de la couche associée (pour les interfaces)
        /// </summary>
        [XmlAttribute(AttributeName = "formatString")]
        [Description("Layer name pattern\r\n {0}=Default Name\r\n{1}=Component name\r\n{2}=Component namespace\r\n{3}=Layer package name\r\n{4}=Associated layer name (for interface layer)")]
        [DisplayName("Name layer pattern")]
        public string FormatString
        {
            get { return _formatString; }
            set { _formatString = value; }
        }

        /// <summary>
        /// Format de chaine : {0} = Nom du project Visual Studio, {1} = Namespace du composant {2} = Namespace de la couche
        /// </summary>
        [XmlAttribute(AttributeName = "assemblyFormatString")]
        [Description("Assembly name pattern\r\n{0}=Visual Studio project name\r\n{1}=layer namespace\r\n{2}=layer name\r\n{3}=model name\r\n={4}component name")]
        public string AssemblyFormatString
        {
            get { return _assemblyFormatString; }
            set { _assemblyFormatString = value; }
        }

        /// <summary>
        /// Format de chaine : {0} = Nom de la couche, {1} = Namespace du composant, {2} = Nom de la couche par défaut
        /// </summary>
        [XmlAttribute("projectFolderFormatString")]
        [Description("Project folder pattern\r\n{0}=package layer name\r\n{1}=layer name\r\n{2}=Component namespace\r\n{3}=default layer name")]
        public string ProjectFolderFormatString
        {
            get { return _projectFolderFormatString; }
            set { _projectFolderFormatString = value; }
        }

        /// <summary>
        /// Format de chaine de l'élément contenu dans la couche : {0} = initial Name, {1} = Nom de la couche
        /// </summary>
        [XmlAttribute("elementFormatString")]
        [Description("Element name pattern\r\n{0}=Initial name\r\n{1}=Layer name")]
        public string ElementFormatString
        {
            get { return _elementFormatString; }
            set { _elementFormatString = value; }
        }

        /// <summary>
        /// Surcharge pour affichage dans la fenètre des propriétés
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return LayerType;
        }
    }
}
