using System;
using System.Collections.Generic;

namespace DSLFactory.Candle.SystemModel.Utilities
{
    /// <summary>
    /// Parcours les références d'une couche
    /// </summary>
    public class LayerReferenceWalker
    {
        private readonly ILayerReferenceVisitor _visitor;

        /// <summary>
        /// Initializes a new instance of the <see cref="LayerReferenceWalker"/> class.
        /// </summary>
        /// <param name="visitor">The visitor.</param>
        public LayerReferenceWalker(ILayerReferenceVisitor visitor)
        {
            _visitor = visitor;
        }

        /// <summary>
        /// Traverses the specified layer.
        /// </summary>
        /// <param name="layer">The layer.</param>
        /// <param name="Scope">The scope.</param>
        /// <param name="mode">The mode.</param>
        public void Traverse(SoftwareLayer layer, ReferenceScope Scope, ConfigurationMode mode)
        {
            // Stocke les éléments traités pour éviter les doublons
            List<Guid> guids = new List<Guid>();

            if (layer is InterfaceLayer)
            {
                foreach (ServiceContract contract in ((InterfaceLayer) layer).ServiceContracts)
                {
                    foreach (Implementation impl in Implementation.GetLinksToImplementations(contract))
                    {
                        if (mode.CheckConfigurationMode(impl.ConfigurationMode) &&
                            !guids.Contains(impl.ClassImplementation.Id))
                        {
                            _visitor.Accept(impl, impl.ClassImplementation);
                            guids.Add(impl.ClassImplementation.Id);
                        }
                    }
                }
            }
            else if (layer is Layer)
            {
                foreach (ClassImplementation clazz in ((Layer) layer).Classes)
                {
                    foreach (NamedElement service in clazz.ServicesUsed)
                    {
                        IList<ClassUsesOperations> externalServiceLinks =
                            ClassUsesOperations.GetLinksToServicesUsed(clazz);
                        foreach (ClassUsesOperations link in externalServiceLinks)
                        {
                            if (mode.CheckConfigurationMode(link.ConfigurationMode) && ((link.Scope & Scope) == Scope))
                            {
                                if (service is ExternalServiceContract)
                                {
                                    _visitor.Accept(link, (ExternalServiceContract) service);
                                }
                                else if (service is ServiceContract)
                                {
                                    _visitor.Accept(link, (ServiceContract) service);
                                }
                                else if (service is ClassImplementation)
                                {
                                    _visitor.Accept(link, (ClassImplementation) service);
                                }
                                else
                                    throw new Exception("Type not implemented");
                                guids.Add(service.Id);
                            }
                        }
                    }
                }
            }
        }
    }
}