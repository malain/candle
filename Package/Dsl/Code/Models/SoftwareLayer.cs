using System;
using System.Diagnostics;
using DSLFactory.Candle.SystemModel.Repository;
using DSLFactory.Candle.SystemModel.Rules.Wizards;
using DSLFactory.Candle.SystemModel.Strategies;
using Microsoft.VisualStudio.Modeling;

namespace DSLFactory.Candle.SystemModel
{
    /// <summary>
    /// 
    /// </summary>
    partial class SoftwareLayer : DSLFactory.Candle.SystemModel.Dependencies.IHasReferences, IHasNamespace
    {
        /// <summary>
        /// Gets the owner.
        /// </summary>
        /// <value>The owner.</value>
        public override ICustomizableElement Owner
        {
            get { return this.Component; }
        }

        /// <summary>
        /// Gets the software component.
        /// </summary>
        /// <value>The software component.</value>
        public SoftwareComponent SoftwareComponent
        {
            get
            {
                return (SoftwareComponent)this.Component;
            }
        }

        /// <summary>
        /// Nom du répertoire (logique) contenant le projet
        /// </summary>
        /// <returns></returns>
        public virtual string GetProjectFolderName()
        {
            return StrategyManager.GetInstance(this.Store).NamingStrategy.CreateProjectFolderName(this, null);
        }

        private string _vsProjectName;
        /// <summary>
        /// Gets the VS project name value.
        /// </summary>
        /// <returns></returns>
        public string GetVSProjectNameValue()
        {
            if( _vsProjectName == null )
                _vsProjectName = this.Name;
            return _vsProjectName;
        }

        /// <summary>
        /// Sets the VS project name value.
        /// </summary>
        /// <param name="value">The value.</param>
        public void SetVSProjectNameValue(string value)
        {
            _vsProjectName = value;
        }

        #region Dependencies
        /// <summary>
        /// Adds the reference to service.
        /// </summary>
        /// <param name="modelId">The model id.</param>
        /// <param name="displayName">The display name.</param>
        /// <param name="modelVersion">The model version.</param>
        /// <param name="serviceName">Name of the service.</param>
        /// <returns></returns>
        public ExternalServiceReference AddReferenceToService(Guid modelId, string displayName, VersionInfo modelVersion, string serviceName)
        {
            return AddReferenceToService(modelId, displayName, modelVersion, serviceName, ReferenceScope.Compilation, "*");
        }

        /// <summary>
        /// Ajout d'une référence sur une couche. Cette action ne peut se faire qu'avec un composant binaire.
        /// </summary>
        /// <param name="modelId">The model id.</param>
        /// <param name="displayName">The display name.</param>
        /// <param name="modelVersion">The model version.</param>
        /// <param name="serviceName">Name of the service.</param>
        /// <param name="scope">The scope.</param>
        /// <param name="mode">The mode.</param>
        /// <returns></returns>
        public ExternalServiceReference AddReferenceToService(Guid modelId, string displayName, VersionInfo modelVersion, string serviceName, ReferenceScope scope, string mode)
        {
            ExternalComponent externalComponent = this.AddExternalModel(modelId, displayName, modelVersion);
          
            if (externalComponent != null && externalComponent.MetaData.ComponentType == ComponentType.Library)
            {
                foreach (ExternalPublicPort port in externalComponent.Ports)
                {
                    if (Utils.StringCompareEquals(serviceName, port.Name))
                    {
                        ExternalServiceReference link = ExternalServiceReference.GetLink(this, port);
                        if (link == null)
                        {
                            using (Transaction transaction = port.Store.TransactionManager.BeginTransaction("Create external service reference"))
                            {
                                link = new ExternalServiceReference(this, port);
                                transaction.Commit();
                            }
                            link.Scope = scope;
                            if (!port.IsInGac)
                                link.Scope |= ReferenceScope.Runtime;
                            link.ConfigurationMode = mode;
                        }
                        return link;
                    }
                }
            }
            ILogger logger = ServiceLocator.Instance.GetService<ILogger>();
            if (logger != null)
                logger.Write("Add reference to service", String.Format("Can not create a link between {0} and {1} because the port {2} doesn't exist", this.Name, displayName, serviceName), LogType.Error);
            return null;
        }

        /// <summary>
        /// Ajoute une référence sur un modèle externe.
        /// </summary>
        /// <param name="modelId">The model id.</param>
        /// <param name="displayName">The display name.</param>
        /// <param name="version">Version désirée ou null</param>
        /// <returns></returns>
        /// <remarks>
        /// Si la référence existe dans le référentiel avec le bon numèro de version,
        /// on rajoute la référence si le numéro de version n'est pas précisé ou n'est pas
        /// présent dans le référentiel, on affiche la boite de dialogue de sélection
        /// si ce n'est toujours pas bon, on rajoute la référence en tant que référence
        /// classique statique.
        /// </remarks>
        protected ExternalComponent AddExternalModel(Guid modelId, string displayName, VersionInfo version)
        {
            ExternalComponent externalComponent = this.Component.Model.FindExternalComponent(modelId);
            if (externalComponent != null)
            {
                return externalComponent;
            }

            // Recherche le modèle dans le repository
            ComponentModelMetadata metadata = RepositoryManager.Instance.ModelsMetadata.Metadatas.Find(modelId, version);
            if (metadata != null)
            {
                // Ok on l'a trouvé
                using (Transaction transaction = this.Store.TransactionManager.BeginTransaction("Add external component"))
                {
                    externalComponent = metadata.CreateComponent(this.Component.Model);
                    transaction.Commit();
                    return externalComponent;
                }
            }
            else
            {
                // Pas trouvé
                ReferencedAssembliesForm form = new ReferencedAssembliesForm(modelId, displayName, version, null ); 
                if (form.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    if (form.SelectedAssemblyBindings.Count > 0)
                        return form.SelectedAssemblyBindings[0].CreateComponent(this.Component.Model);
                }
            }

            return null;
        }

        /// <summary>
        /// Finds the reference.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="content">The content.</param>
        /// <returns></returns>
        public Artifact FindReference(string fileName, ArtifactType content)
        {
            string simpleName = fileName;
            if (System.IO.Path.IsPathRooted(fileName))
                simpleName = System.IO.Path.GetFileName(fileName);

            foreach (Artifact artifact in this.Artifacts)
            {
                if (Utils.StringCompareEquals(simpleName, artifact.FileName))
                {
                    if (content == artifact.Type)
                        return artifact;
                }
            }
            return null;
        }

        /// <summary>
        /// Adds the reference.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="content">The content.</param>
        /// <returns></returns>
        public Artifact AddReference(string fileName, ArtifactType content)
        {
            return AddReference(fileName, content, content == ArtifactType.Content ? ReferenceScope.Runtime : ReferenceScope.Compilation, "*");
        }

        /// <summary>
        /// Adds the reference.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="content">The content.</param>
        /// <param name="scope">The scope.</param>
        /// <param name="mode">The mode.</param>
        /// <returns></returns>
        public Artifact AddReference(string fileName, ArtifactType content, ReferenceScope scope, string mode)
        {
            Artifact artifact = FindReference(fileName, content);
            if (artifact != null)
                return artifact;

            using (Transaction transaction = this.Store.TransactionManager.BeginTransaction("Add artifact"))
            {
                artifact = new Artifact(this.Store);

                if (System.IO.Path.IsPathRooted(fileName))
                {
                    artifact.Type = content;
                    artifact.FileName = System.IO.Path.GetFileName(fileName);
                    artifact.InitialFileName = fileName;
                }
                else
                {
                    artifact.Type = content;
                    artifact.FileName = fileName;
                }

                artifact.Scope = scope;
                artifact.ConfigurationMode = mode;
                this.Artifacts.Add(artifact);

                transaction.Commit();
                return artifact;
            }
        }
        #endregion


        #region IHasNamespace Members

        /// <summary>
        /// Recherche itérative du parent
        /// </summary>
        /// <value></value>
        public string NamespaceDeclaration
        {
            get
            {
                StandardNamespaceResolver resolver = null;
                ICustomizableElement elem = this.StrategiesOwner;
                while (elem != null)
                {
                    if (elem is IProvidesNamespaceResolver)
                    {
                        resolver = ((IProvidesNamespaceResolver)elem).NamespaceResolver;
                        break;
                    }
                    elem = elem.Owner;
                }

                Debug.Assert(resolver != null);
                return resolver.Resolve(this.Namespace);
            }
        }

        #endregion
    }
}