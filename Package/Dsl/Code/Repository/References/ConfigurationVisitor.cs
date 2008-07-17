using System.Collections.Generic;

namespace DSLFactory.Candle.SystemModel.Dependencies
{
    /// <summary>
    /// Récupération des configurations des composants externes
    /// </summary>
    /// <example>
    ///    ReferenceWalker walker = new ReferenceWalker(ReferenceScope.Runtime, new ConfigurationMode());
    ///    ConfigurationVisitor visitor = new ConfigurationVisitor();
    ///    walker.Traverse(visitor, model);
    ///    List&lt;ConfigurationPart&gt; configurations = visitor.Configurations;
    /// </example>
    public class ConfigurationVisitor : IReferenceVisitor
    {
        /// <summary>
        /// Indique si on doit prendre en compte la configuration du modèle initial ou 
        /// seulement les externes
        /// </summary>
        private readonly bool _includeInitialModelConfigurations;

        /// <summary>
        /// Stockage des modèles dèja traaités
        /// </summary>
        private readonly Dictionary<ComponentSig, object> _models = new Dictionary<ComponentSig, object>();

        /// <summary>
        /// Contiendra les configurations trouvées
        /// </summary>
        private List<ConfigurationPart> _configurations;

        /// <summary>
        /// Constructeur
        /// </summary>
        /// <param name="includeInitialModelConfigurations">Indique si on doit prendre en compte la configuration du modèle initial ou
        /// seulement les externes</param>
        public ConfigurationVisitor(bool includeInitialModelConfigurations)
        {
            _includeInitialModelConfigurations = includeInitialModelConfigurations;
            _configurations = new List<ConfigurationPart>();
        }

        /// <summary>
        /// Configurations trouvées
        /// </summary>
        /// <value>The configurations.</value>
        public List<ConfigurationPart> Configurations
        {
            get { return _configurations; }
            set { _configurations = value; }
        }

        #region IReferenceVisitor Members

        /// <summary>
        /// Récupération des configurations lorsqu'on rencontre un modèle sauf pour le modèle initial.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns></returns>
        bool IReferenceVisitor.Accept(ReferenceItem item)
        {
            if (item.Element is CandleModel)
            {
                CandleModel model = item.Element as CandleModel;
                if (model == null)
                    return false;

                ComponentSig key = new ComponentSig(model.Id, model.Version);

                // Si dèja vu, on arrete
                if (_models.ContainsKey(key))
                    return false;

                // On ne prend jamais la config du modèle initial
                if (_includeInitialModelConfigurations || _models.Count > 0)
                {
                    if (model.IsLibrary && model.SoftwareComponent != null)
                    {
                        foreach (SoftwareLayer asm in model.SoftwareComponent.Layers)
                        {
                            RetrieveConfigurations(asm);
                        }
                    }
                    else if (model.BinaryComponent != null)
                    {
                        foreach (DotNetAssembly asm in model.BinaryComponent.Assemblies)
                        {
                            RetrieveConfigurations(asm);
                        }
                    }
                }
                _models.Add(key, null);
            }
            return true;
        }

        /// <summary>
        /// Exits the element.
        /// </summary>
        /// <param name="item">The item.</param>
        void IReferenceVisitor.ExitElement(ReferenceItem item)
        {
        }

        #endregion

        /// <summary>
        /// Récupère les configurations
        /// </summary>
        /// <param name="layer">The layer.</param>
        private void RetrieveConfigurations(AbstractLayer layer)
        {
            foreach (ConfigurationPart part in layer.Configurations)
            {
                if (part.Visibility == Visibility.Public)
                    _configurations.Add(part);
            }
        }
    }
}