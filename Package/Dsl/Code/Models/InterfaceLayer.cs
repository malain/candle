using System;
using System.Collections.Generic;
using DSLFactory.Candle.SystemModel.Dependencies;
using DSLFactory.Candle.SystemModel.Strategies;

namespace DSLFactory.Candle.SystemModel
{
    /// <summary>
    /// 
    /// </summary>
    partial class InterfaceLayer : ISortedLayer
    {
        /// <summary>
        /// Gets the references.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        public override IEnumerable<ReferenceItem> GetReferences(ReferenceContext context)
        {
            if ((context.Scope == ReferenceScope.Runtime && context.IsExternal == false) ||
                context.Scope == ReferenceScope.All)
            {
                //Layers d'en dessous
                List<Guid> layers = new List<Guid>();
                foreach (ServiceContract contract in ServiceContracts)
                {
                    foreach (Implementation impl in Implementation.GetLinksToImplementations(contract))
                    {
                        if (context.Mode.CheckConfigurationMode(impl.ConfigurationMode) &&
                            !layers.Contains(impl.ClassImplementation.Layer.Id))
                        {
                            Layer layer = impl.ClassImplementation.Layer;
                            layers.Add(layer.Id);
                            yield return new ReferenceItem(this, layer, context.IsExternal);
                        }
                    }
                }
            }

            // La couche modèle
            if (SoftwareComponent.IsDataLayerExists)
                yield return new ReferenceItem(this, SoftwareComponent.DataLayer, context.IsExternal);

            foreach (ReferenceItem ri in base.GetReferences(context))
                yield return ri;
        }

        /// <summary>
        /// Nom du répertoire (logique) contenant le projet
        /// </summary>
        /// <returns></returns>
        public override string GetProjectFolderName()
        {
            return
                StrategyManager.GetInstance(Store).NamingStrategy.CreateProjectFolderName(this,
                                                                                          LayerPackage != null
                                                                                              ? LayerPackage.Name
                                                                                              : null);
        }

        /// <summary>
        /// Generates the childs code.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        protected override bool GenerateChildsCode(GenerationContext context)
        {
            foreach (ServiceContract contract in ServiceContracts)
            {
                if (contract.GenerateCode(context))
                    return true;
            }
            return false;
        }
    }
}