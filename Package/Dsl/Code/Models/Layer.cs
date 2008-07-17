using System;
using System.Collections.Generic;
using DSLFactory.Candle.SystemModel.Dependencies;
using DSLFactory.Candle.SystemModel.Strategies;
using Microsoft.VisualStudio.Modeling;

namespace DSLFactory.Candle.SystemModel
{
    /// <summary>
    /// 
    /// </summary>
    partial class Layer : ISortedLayer
    {
        /// <summary>
        /// Gets the interface layer.
        /// </summary>
        /// <value>The interface layer.</value>
        public InterfaceLayer InterfaceLayer
        {
            get { return LayerPackage.InterfaceLayer; }
        }

        #region ISortedLayer Members

        /// <summary>
        /// Gets the level.
        /// </summary>
        /// <value>The level.</value>
        public abstract Int16 Level { get; }

        #endregion

        /// <summary>
        /// Récupére la liste des services utilisés par cette couche. Cette méthode assure que les services sont uniques.
        /// </summary>
        /// <param name="mode">Mode à prendre en compte</param>
        /// <returns></returns>
        public List<ClassUsesOperations> GetServicesUsed(ConfigurationMode mode)
        {
            List<ClassUsesOperations> services = new List<ClassUsesOperations>();
            List<TypeWithOperations> types = GetImplementations();
            if (types == null || types.Count == 0)
                return services;

            List<Guid> doublons = new List<Guid>();
            foreach (TypeWithOperations clazz in types)
            {
                foreach (ClassUsesOperations service in ClassUsesOperations.GetLinksToServicesUsed(clazz))
                {
                    if (!mode.CheckConfigurationMode(service.ConfigurationMode) ||
                        doublons.Contains(service.TargetService.Id))
                        continue;
                    doublons.Add(service.TargetService.Id);
                    services.Add(service);
                }
            }
            return services;
        }

        /// <summary>
        /// Retourne la liste des éléments pouvant faire référence à un service d'une autre couche
        /// </summary>
        /// <returns></returns>
        protected virtual List<TypeWithOperations> GetImplementations()
        {
            List<TypeWithOperations> types = new List<TypeWithOperations>();
            foreach (TypeWithOperations t in Classes)
                types.Add(t);
            return types;
        }

        /// <summary>
        /// Nom du répertoire (logique) contenant le projet
        /// </summary>
        /// <returns></returns>
        public override string GetProjectFolderName()
        {
            return StrategyManager.GetInstance(Store).NamingStrategy.CreateProjectFolderName(this, LayerPackage.Name);
        }

        /// <summary>
        /// Gets the references.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        public override IEnumerable<ReferenceItem> GetReferences(ReferenceContext context)
        {
            if (context.Scope == ReferenceScope.Compilation && LayerPackage.InterfaceLayer != null
                /* (loop avec interfacelayer) || context.Scope == ReferenceScope.All*/)
                yield return new ReferenceItem(this, LayerPackage.InterfaceLayer, context.IsExternal);

            // La couche modèle
            if (SoftwareComponent.IsDataLayerExists)
                yield return new ReferenceItem(this, SoftwareComponent.DataLayer, context.IsExternal);

            // Ref couche en dessous
            if (context.Scope != ReferenceScope.Publish)
            {
                List<Guid> layers = new List<Guid>();
                foreach (ReferenceItem ri in GetServiceReferences(context, layers))
                    yield return ri;
            }

            foreach (ReferenceItem ri in base.GetReferences(context))
                yield return ri;
        }

        /// <summary>
        /// Gets the service references.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="layers">The layers.</param>
        /// <returns></returns>
        protected virtual IEnumerable<ReferenceItem> GetServiceReferences(ReferenceContext context, List<Guid> layers)
        {
            foreach (ClassImplementation clazz in Classes)
            {
                foreach (ReferenceItem ri in GetServiceReferences(clazz, context, layers))
                    yield return ri;
            }
        }

        /// <summary>
        /// Gets the service references.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="context">The context.</param>
        /// <param name="layers">The layers.</param>
        /// <returns></returns>
        protected IEnumerable<ReferenceItem> GetServiceReferences(TypeWithOperations item, ReferenceContext context,
                                                                  List<Guid> layers)
        {
            foreach (NamedElement service in item.ServicesUsed)
            {
                IList<ClassUsesOperations> externalServiceLinks = ClassUsesOperations.GetLinksToServicesUsed(item);
                foreach (ClassUsesOperations link in externalServiceLinks)
                {
                    if (context.Mode.CheckConfigurationMode(link.ConfigurationMode) && context.CheckScope(link.Scope))
                    {
                        if (service is ExternalServiceContract)
                        {
                            ExternalServiceContract contract = service as ExternalServiceContract;

                            // Ici on part du principe qu'un composant ne publie qu'une couche donc
                            // on ne prend en considèration que le 1er port rencontré. Si ce n'était pas le
                            // cas, il faudrait d'abord créer un tableau global des ports rencontrés PUIS faire
                            // un RetrieveReferencesForExternalComponent avec tous les ports.
                            List<Guid> ports2 = new List<Guid>();
                            ports2.Add(contract.ComponentPortMoniker);
                            yield return
                                new ReferenceItem(this, contract.Parent, link.Scope, ports2, context.IsExternal);
                        }
                        else if (service is ServiceContract)
                        {
                            ServiceContract contract = service as ServiceContract;
                            if (!layers.Contains(contract.Layer.Id))
                            {
                                layers.Add(contract.Layer.Id);
                                yield return new ReferenceItem(this, contract.Layer, context.IsExternal);
                            }
                        }
                        else if (service is ClassImplementation)
                        {
                            ClassImplementation targetClazz = service as ClassImplementation;
                            if (!layers.Contains(targetClazz.Layer.Id))
                            {
                                layers.Add(targetClazz.Layer.Id);
                                yield return new ReferenceItem(this, targetClazz.Layer, context.IsExternal);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Allows the model element to configure itself immediately after the Merge process has related it to the target element.
        /// </summary>
        /// <param name="elementGroup">The group of source elements that have been added back into the target store.</param>
        protected override void MergeConfigure(ElementGroup elementGroup)
        {
            base.MergeConfigure(elementGroup);
            LayerPackage.Level = Level;

            DomainClassInfo.SetUniqueName(this,
                                          StrategyManager.GetInstance(Store).NamingStrategy.CreateLayerName(
                                              LayerPackage, this, Name));
            Namespace =
                StrategyManager.GetInstance(Store).NamingStrategy.CreateNamespace(Component.Namespace, Name, this);

            Layer mainLayer = SoftwareComponent.GetMainLayer();
            if (mainLayer == null)
            {
                mainLayer = SoftwareComponent.SuggestMainLayer(null);
                if (mainLayer == this)
                {
                    mainLayer.StartupProject = true;
                    mainLayer.HostingContext = HostingContext.Standalone;
                }
            }
            else if (mainLayer.Level < Level)
                HostingContext = mainLayer.HostingContext;
        }
    }
}