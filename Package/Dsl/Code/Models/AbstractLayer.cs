using System;
using System.Collections.Generic;
using System.Xml;
using DSLFactory.Candle.SystemModel.Dependencies;
using Microsoft.VisualStudio.Modeling;

namespace DSLFactory.Candle.SystemModel
{
    partial class AbstractLayer
    {
        /// <summary>
        /// Gets the strategies owner.
        /// </summary>
        /// <value>The strategies owner.</value>
        public override CandleElement StrategiesOwner
        {
            get { return this; }
        }

        #region References
        /// <summary>
        /// Caractèristiques d'une relation
        /// </summary>
        struct LinkDef
        {
            public List<Guid> Ports;
            public ReferenceScope Scope;
        }

        /// <summary>
        /// Gets the references.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        public virtual System.Collections.Generic.IEnumerable<ReferenceItem> GetReferences(ReferenceContext context)
        {
            // Liste des références externes à partir de cette couche
            if (context.Scope != ReferenceScope.Publish)
            {
                // Sockage temporaire du lien entre le composant externe et ses ports utilisés
                Dictionary<ExternalComponent, LinkDef> candidates = new Dictionary<ExternalComponent, LinkDef>();

                IList<ExternalServiceReference> externalServiceLinks = ExternalServiceReference.GetLinksToExternalServiceReferences(this);
                foreach (ExternalServiceReference link in externalServiceLinks)
                {
                    if (context.Mode.CheckConfigurationMode(link.ConfigurationMode))
                    {
                        if ( context.Scope == ReferenceScope.All || (
                                                                        ((context.Scope == ReferenceScope.Runtime && !link.ExternalPublicPort.IsInGac) || context.Scope == ReferenceScope.Compilation)
                                                                        && context.CheckScope(link.Scope))
                            )
                        {
                            LinkDef def;
                            if (!candidates.TryGetValue(link.ExternalPublicPort.Parent, out def))
                            {
                                def.Ports = new List<Guid>();
                                def.Scope = link.Scope;
                                candidates.Add(link.ExternalPublicPort.Parent, def);
                            }
                            def.Ports.Add(link.ExternalPublicPort.ComponentPortMoniker);
                        }
                    }
                }

                foreach (ExternalComponent externalComponent in candidates.Keys)
                {
                    LinkDef def = candidates[externalComponent];
                    ReferenceItem ri = new ReferenceItem(this, externalComponent, def.Scope, def.Ports, context.IsExternal);
                    yield return ri;
                }
            }

            // Liste des artifacts
            IList<LayerHasArtifacts> artifactsLinks = LayerHasArtifacts.GetLinksToArtifacts(this);
            foreach (LayerHasArtifacts link in artifactsLinks)
            {
                if (context.Mode.CheckConfigurationMode(link.Artifact.ConfigurationMode) && context.CheckScope(link.Artifact.Scope))
                {
                    yield return new ReferenceItem(this, link.Artifact, link.Artifact.Scope, context.IsExternal);
                }
            }
        }     

        #endregion

        /// <summary>
        /// Ajout d'un fragment xml de configuration qui sera fusionné dans le fichier de config
        /// </summary>
        /// <param name="id"></param>
        /// <param name="xmlContent">Le fragment xml valide contenant le noeud racine 'configuration' ou null pour supprimer</param>
        public void AddXmlConfigurationContent(string id, string xmlContent)
        {
            AddXmlConfigurationContent(id, xmlContent, Visibility.Public);
        }

        /// <summary>
        /// Adds the content of the XML configuration.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="xmlContent">Content of the XML.</param>
        /// <param name="scope">The scope.</param>
        public void AddXmlConfigurationContent(string id, string xmlContent, Visibility scope)
        {
            if (xmlContent != null)
            {
                try
                {
                    XmlDocument xdoc = new XmlDocument();
                    xdoc.LoadXml(xmlContent);
                    if (xdoc.DocumentElement == null || xdoc.DocumentElement.LocalName != "configuration")
                        throw new ArgumentException("Invalid xml content. Root section must be 'configuration')");
                }
                catch (Exception ex)
                {
                    throw new ArgumentException(String.Format("Invalid xml content for layer {0} id={1} ({2}) - Xml={3} ", this.Name, id,  ex.Message , xmlContent));
                }
            }

            using (Transaction transaction = this.Store.TransactionManager.BeginTransaction("Add settings"))
            {
                foreach (ConfigurationPart cfg in this.Configurations)
                {
                    if (Utils.StringCompareEquals(cfg.Name, id) )
                    {
                        if (cfg.XmlContent != xmlContent || cfg.Visibility != scope)
                        {
                            cfg.Visibility = scope;
                            if (xmlContent != null)
                                cfg.XmlContent = xmlContent;
                            else
                                this.Configurations.Remove(cfg);
                            transaction.Commit();
                        }
                        return;
                    }
                }

                if (xmlContent != null)
                {
                    ConfigurationPart cfgPart = new ConfigurationPart(this.Store);
                    cfgPart.XmlContent = xmlContent;
                    cfgPart.Name = id;
                    cfgPart.Enabled = true;
                    cfgPart.Visibility = scope;
                    this.Configurations.Add(cfgPart);
                    transaction.Commit();
                }
            }
        }
    }
}