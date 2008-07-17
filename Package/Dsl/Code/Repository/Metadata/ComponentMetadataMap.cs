using System;
using System.Drawing;
using DSLFactory.Candle.SystemModel.Properties;

namespace DSLFactory.Candle.SystemModel.Repository
{
    /// <summary>
    /// Classe contenant les caractèristiques de l'assembly importée
    /// </summary>
    public class ComponentMetadataMap
    {
        private string _assemblyName;
        private ComponentType? _componentType;
        private ComponentModelMetadata _metaData;
        private string _name;
        private VersionInfo _version;
        private bool _alreadyExists;

        /// <summary>
        /// Gets or sets the initial type of the component.
        /// </summary>
        /// <value>The initial type of the component.</value>
        public ComponentType? InitialComponentType
        {
            get { return _componentType; }
            set { _componentType = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [already exists].
        /// </summary>
        /// <value><c>true</c> if [already exists]; otherwise, <c>false</c>.</value>
        public bool AlreadyExists
        {
            get { return _alreadyExists; }
            set { _alreadyExists = value; }
        }

        /// <summary>
        /// null si pas de binding avec un modèle existant
        /// </summary>
        public ComponentModelMetadata MetaData
        {
            get { return _metaData; }
            set { _metaData = value; }
        }

        /// <summary>
        /// Gets the warning image.
        /// </summary>
        /// <value>The warning image.</value>
        public Image WarningImage
        {
            get { return _metaData == null ? Resources.Warning : Resources.Blank; }
        }

        /// <summary>
        /// Gets or sets the version.
        /// </summary>
        /// <value>The version.</value>
        public VersionInfo Version
        {
            get { return _version; }
            set { _version = value; }
        }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        /// <summary>
        /// Gets or sets the name of the assembly.
        /// </summary>
        /// <value>The name of the assembly.</value>
        public string AssemblyName
        {
            get { return _assemblyName; }
            set { _assemblyName = value; }
        }

        /// <summary>
        /// Creates the external component.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public ExternalComponent CreateComponent(CandleModel model)
        {
            ExternalComponent externalComponent;
            if (_metaData != null)
            {
                // Recherche par l'id si possible
                externalComponent = model.FindExternalComponent(_metaData.Id);
            }
            else
            {
                // Sinon par le nom pour éviter des doublons
                externalComponent = model.FindExternalComponentByName(Name);
            }

            if (externalComponent != null)
                return externalComponent;

            if (_metaData != null)
                return _metaData.CreateComponent(model);

            // Component par défaut avec le nom de l'assembly

            externalComponent = new ExternalComponent(model.Store);
            //ExternalPublicPort port = new ExternalPublicPort(model.Store);
            //port.Name = this.AssemblyName;
            //((ExternalBinaryComponent)externalComponent).Ports.Add(port);
            externalComponent.ModelMoniker = Guid.Empty;
            externalComponent.Name = AssemblyName;
            externalComponent.Version = Version;

            // Ajout au modèle
            model.ExternalComponents.Add(externalComponent);
            return externalComponent;
        }
    }
}