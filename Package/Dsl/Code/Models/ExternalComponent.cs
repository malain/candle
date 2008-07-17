using System;
using System.Collections.Generic;
using DSLFactory.Candle.SystemModel.Dependencies;
using DSLFactory.Candle.SystemModel.Repository;
using DSLFactory.Candle.SystemModel.Strategies;
using Microsoft.VisualStudio.Modeling;

namespace DSLFactory.Candle.SystemModel
{
    /// <summary>
    /// 
    /// </summary>
    partial class ExternalComponent : IHasReferences
    {
        /// <summary>
        /// Gets the referenced model.
        /// </summary>
        /// <value>The referenced model.</value>
        public CandleModel ReferencedModel
        {
            get
            {
                ModelLoader loader = ModelLoader.GetLoader(this);
                if (loader == null || loader.Model == null)
                {
                    IIDEHelper ide = ServiceLocator.Instance.GetService<IIDEHelper>();
                    if (ide != null)
                        ide.LogError(false, String.Format("Unable to load model {0}", Name ?? "???"), 0, 0, "Load model");
                    return null;
                }
                return loader.Model;
            }
        }


        /// <summary>
        /// Recherche du metadata
        /// </summary>
        public ComponentModelMetadata MetaData
        {
            get
            {
                if (ModelMoniker == Guid.Empty || Version == null)
                    return null;
                return
                    RepositoryManager.Instance.ModelsMetadata.Metadatas.FindWithPartialVersion(ModelMoniker, Version, 3);
            }
        }

        /// <summary>
        /// Gets the owner.
        /// </summary>
        /// <value>The owner.</value>
        public override ICustomizableElement Owner
        {
            get { return Model; }
        }

        /// <summary>
        /// Gets the strategies owner.
        /// </summary>
        /// <value>The strategies owner.</value>
        public override CandleElement StrategiesOwner
        {
            get { return Model; }
        }

        #region IHasReferences Members

        /// <summary>
        /// Gets the references.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        public IEnumerable<ReferenceItem> GetReferences(ReferenceContext context)
        {
            CandleModel model = ReferencedModel;
            if (model != null)
                yield return new ReferenceItem(this, model, ReferenceScope.Compilation, context.Ports, true);
        }

        #endregion

        /// <summary>
        /// Gets the is last version value.
        /// </summary>
        /// <returns></returns>
        internal bool GetIsLastVersionValue()
        {
            ComponentModelMetadata cmm = MetaData;
            return cmm != null && cmm.Version.Equals(Version) && cmm.IsLastVersion();
        }

        /// <summary>
        /// Permet d'afficher les ports corresponds aux ports définis dans le modèle
        /// </summary>
        internal bool UpdateFromModel()
        {
            ILogger logger = ServiceLocator.Instance.GetService<ILogger>();
            try
            {
                object flag;
                if (Store.TransactionManager.InTransaction &&
                    Store.TransactionManager.CurrentTransaction.TopLevelTransaction.Context.ContextInfo.TryGetValue(
                        "InModelLoader", out flag))
                    return false;

                ComponentModelMetadata metaData = MetaData;
                CandleModel model = ReferencedModel;
                if (model == null || metaData == null) //|| metaData.Version.Equals(this.Version))
                    return false;

                // Création des ports externes
                using (Transaction transaction = Store.TransactionManager.BeginTransaction("Populate external system"))
                {
                    // Mise à jour du numèro de version
                    if (!metaData.Version.Equals(Version))
                        Version = metaData.Version;

                    List<Guid> portsId = new List<Guid>();
                    if (model.BinaryComponent != null)
                    {
                        foreach (DotNetAssembly asm in model.BinaryComponent.Assemblies)
                        {
                            if (asm.Visibility == Visibility.Private)
                                continue;

                            portsId.Add(asm.Id);
                            ExternalPublicPort publicPort =
                                Ports.Find(
                                    delegate(ExternalPublicPort port) { return port.ComponentPortMoniker == asm.Id; });
                            if (publicPort == null)
                            {
                                publicPort = new ExternalPublicPort(Store);
                                Ports.Add(publicPort);
                            }

                            publicPort.Name = asm.Name;
                            publicPort.ComponentPortMoniker = asm.Id;
                            publicPort.IsInGac = asm.IsInGac;
                        }
                    }
                    else if (model.SoftwareComponent != null)
                    {
                        if (model.IsLibrary)
                        {
                            foreach (TypeWithOperations pub in model.SoftwareComponent.PublicContracts)
                            {
                                portsId.Add(pub.Id);
                                ExternalPublicPort publicPort =
                                    Ports.Find(
                                        delegate(ExternalPublicPort port) { return port.ComponentPortMoniker == pub.Id; });
                                if (publicPort == null)
                                {
                                    publicPort = new ExternalPublicPort(Store);
                                    Ports.Add(publicPort);
                                }
                                publicPort.Name = pub.Name;
                                publicPort.ComponentPortMoniker = pub.Id;
                            }
                        }
                        else
                        {
                            foreach (TypeWithOperations pub in model.SoftwareComponent.PublicContracts)
                            {
                                portsId.Add(pub.Id);
                                ExternalPublicPort publicPort =
                                    Ports.Find(
                                        delegate(ExternalPublicPort port) { return port.ComponentPortMoniker == pub.Id; });
                                if (publicPort == null)
                                {
                                    publicPort = new ExternalServiceContract(Store);
                                    Ports.Add(publicPort);
                                }
                                publicPort.Name = pub.Name;
                                publicPort.ComponentPortMoniker = pub.Id;
                            }
                        }
                    }

                    RemoveUnusedPorts(portsId);
                    if (transaction.HasPendingChanges)
                    {
                        transaction.Commit();
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                if (logger != null)
                    logger.WriteError("Update from model", String.Format("Error in UpdateFromModel for {0}", Name), ex);
            }
            return false;
        }

        /// <summary>
        /// Suppression des ports externes qui ne sont plus dans le modèle de référence
        /// </summary>
        /// <param name="modelPorts">The model ports.</param>
        protected void RemoveUnusedPorts(List<Guid> modelPorts)
        {
            // Suppression des inutiles
            IList<ExternalPublicPort> ports = Ports;
            while (true)
            {
                ExternalPublicPort portToDelete = null;
                foreach (ExternalPublicPort publicPort in ports)
                {
                    if (!modelPorts.Contains(publicPort.ComponentPortMoniker))
                    {
                        portToDelete = publicPort;
                        break;
                    }
                }
                if (portToDelete != null)
                {
                    // Recherche du port qui le remplace
                    ExternalPublicPort remplacant = null;
                    foreach (ExternalPublicPort newPort in Ports)
                    {
                        if (newPort.Name == portToDelete.Name && newPort != portToDelete)
                        {
                            remplacant = newPort;
                            break;
                        }
                    }
                    IList<ClassUsesOperations> classReferences = null;
                    IList<ExternalServiceReference> layerReferences = null;

                    if (remplacant != null)
                    {
                        // Si ce port avait un lien, on essaye de le recréer sur le nouveau port du même nom
                        classReferences = ClassUsesOperations.GetLinksToSources(portToDelete);
                        layerReferences = ExternalServiceReference.GetLinksToClients(portToDelete);

                        // Suppression de l'ancien port
                        portToDelete.Delete();

                        // Re création des liens
                        if (layerReferences != null)
                        {
                            foreach (ExternalServiceReference link in layerReferences)
                            {
                                if (ReferencedModel != null)
                                    ((SoftwareLayer) link.Client).AddReferenceToService(ReferencedModel.Id,
                                                                                        ReferencedModel.Name,
                                                                                        ReferencedModel.Version,
                                                                                        portToDelete.Name);
                            }
                        }

                        if (classReferences != null)
                        {
                            foreach (ClassUsesOperations link in classReferences)
                            {
                                new ClassUsesOperations(link.Source, remplacant);
                            }
                        }
                    }
                    else
                        portToDelete.Delete();
                }
                else
                    break;
            }
        }
    }
}