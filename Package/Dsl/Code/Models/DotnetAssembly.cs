using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using DSLFactory.Candle.SystemModel.Dependencies;
using DSLFactory.Candle.SystemModel.Repository;
using DSLFactory.Candle.SystemModel.Rules.Wizards;
using DSLFactory.Candle.SystemModel.Strategies;
using Microsoft.VisualStudio.Modeling.Validation;

namespace DSLFactory.Candle.SystemModel
{
    /// <summary>
    /// 
    /// </summary>
    [ValidationState(ValidationState.Enabled)]
    partial class DotNetAssembly : IHasReferences
    {
        /// <summary>
        /// Gets the owner.
        /// </summary>
        /// <value>The owner.</value>
        public override ICustomizableElement Owner
        {
            get { return Component; }
        }

        #region IHasReferences Members

        /// <summary>
        /// Gets the references.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        public override IEnumerable<ReferenceItem> GetReferences(ReferenceContext context)
        {
            foreach (
                AssemblyReferencesAssemblies link in
                    AssemblyReferencesAssemblies.GetLinksToInternalAssemblyReferences(this))
            {
                if (context.CheckScope(link.Scope))
                {
                    if (context.Scope != ReferenceScope.Runtime || link.SourceAssembly.IsInGac == false ||
                        context.Scope == ReferenceScope.All)
                        yield return new ReferenceItem(this, link.SourceAssembly, context.IsExternal);
                }
            }

            foreach (ReferenceItem ri in base.GetReferences(context))
                yield return ri;
        }

        #endregion

        /// <summary>
        /// Called by the model before the element is deleted.
        /// </summary>
        protected override void OnDeleting()
        {
            base.OnDeleting();
            if (Component != null)
                Component.Model.RegisterExternalAssemblyPendingDelete(this);
        }

        /// <summary>
        /// Alerts listeners that an element has been added back to a model.
        /// </summary>
        protected override void OnResurrected()
        {
            base.OnResurrected();

            CandleModel sys = CandleModel.GetInstance(Store);
            sys.UnregisterExternalAssemblyPendingDelete(this);
        }

        /// <summary>
        /// Lors de la sauvegarde du modèle, on synchronise le repository local avec la dll sous-jacente.
        /// </summary>
        /// <param name="context">The context.</param>
        [ValidationMethod(ValidationCategories.Save)]
        private void SynchronizeRepository(ValidationContext context)
        {
            ILogger logger = ServiceLocator.Instance.GetService<ILogger>();
            if (String.IsNullOrEmpty(InitialLocation) || !File.Exists(InitialLocation))
                return;

            try
            {
                // Copie dans le repository local
                string destFile =
                    Component.Model.MetaData.ResolvePath(Path.GetFileName(InitialLocation), PathKind.Absolute);
                if (string.IsNullOrEmpty(destFile))
                    return;
                // Si on édite le modèle directement à partir du repository local, il ne faut pas faire de copie.
                if (!Utils.StringCompareEquals(destFile, InitialLocation))
                {
                    // C'est bon, on copie.
                    Utils.CopyFile(InitialLocation, destFile);
                    if (logger != null)
                        logger.Write("Synchronize repository",
                                     String.Format("Copy file {0} to the local repository ({1})", InitialLocation,
                                                   destFile), LogType.Info);
                }
            }
            catch (Exception  ex)
            {
                // Piti problème
                context.LogError(
                    String.Format("Error when copying the assembly {0} in the local repository - {1}", InitialLocation,
                                  ex.Message),
                    "ExternalAssemblyModel01", // Unique error number
                    this);
            }
        }

        /// <summary>
        /// Initialisation à partir de l'import d'une assembly
        /// </summary>
        /// <param name="assembly">L'assembly à importer</param>
        /// <param name="insertDependencies">Doit on créer les dépendances externes</param>
        internal void InitFromAssembly(Assembly assembly, bool insertDependencies)
        {
            AssemblyName an = assembly.GetName();
            AssemblyName = an.Name + ".dll";
            Name = an.Name;
            FullName = an.FullName;
            Version = new VersionInfo(an.Version);
            IsInGac = assembly.GlobalAssemblyCache; // TODO a revoir

            // Sauvegarde de l'emplacement de la dll pour permettre la copie dans le référentiel
            // (Qui ne s'effectuera que lors de la sauvegarde du modèle)
            InitialLocation = assembly.Location;

            if (insertDependencies)
            {
                InsertDependencies(assembly);
            }
        }

        /// <summary>
        /// Création d'une dépendance avec une assembly importée
        /// </summary>
        /// <param name="asm">The asm.</param>
        internal void InsertDependencies(Assembly asm)
        {
            // Gestion des dépendances
            List<AssemblyName> referencedAssemblies = GetAssemblyListToCreate(asm);
            if (referencedAssemblies.Count == 0)
                return;

            // On va demander de faire le mapping entre l'assembly et les modèles
            // Fenetre permettant de trouver le lien entre une assembly et son modèle
            ReferencedAssembliesForm form = new ReferencedAssembliesForm(this, referencedAssemblies.ToArray());
            if (form.ShowDialog() == DialogResult.OK)
            {
                // Création des composants externes
                List<ExternalComponent> externalComponents = new List<ExternalComponent>();
                foreach (ComponentMetadataMap map in form.SelectedAssemblyBindings)
                {
                    if(!map.AlreadyExists)
                        externalComponents.Add(map.CreateComponent(Component.Model));
                }

                // Puis création des relations avec eux
                foreach (ExternalComponent externalComponent in externalComponents)
                {
                    // Pour l'instant, on ne crée des références qu'avec des composants binaires
                    if (externalComponent == null || externalComponent.MetaData == null ||
                        externalComponent.MetaData.ComponentType != ComponentType.Library)
                        continue;

                    if (externalComponent.Ports.Count == 1)
                    {
                        if (ExternalServiceReference.GetLink(this, externalComponent.Ports[0]) == null)
                        {
                            ExternalServiceReference esr =
                                new ExternalServiceReference(this, externalComponent.Ports[0]);
                            esr.Scope = ReferenceScope.Runtime;
                        }
                    }
                    else if( externalComponent.Ports.Count > 1)
                    {
                        string assemblyName = asm.GetName().Name;
                        foreach (ExternalPublicPort port in externalComponent.Ports)
                        {
                            if (Utils.StringCompareEquals(port.Name, assemblyName) ||
                                Utils.StringCompareEquals(port.Parent.Name, assemblyName))
                            {
                                if (ExternalServiceReference.GetLink(this, port) == null)
                                {
                                    ExternalServiceReference esr = new ExternalServiceReference(this, port);
                                    esr.Scope = ReferenceScope.Runtime;
                                }
                                break;
                            }
                        }
                    }
                    else
                    {
                        IIDEHelper ide = ServiceLocator.Instance.GetService<IIDEHelper>();
                        ide.LogError(false, String.Format("Can not create relationship between {0} and {1}", this.Name, externalComponent.Name), 0, 0, "Import");
                    }
                }
            }
        }

        /// <summary>
        /// Permet lors de l'import d'une assembly de récupérer la liste des assemblies
        /// référencées par celle-ci
        /// </summary>
        /// <param name="asm">The asm.</param>
        /// <returns></returns>
        private List<AssemblyName> GetAssemblyListToCreate(Assembly asm)
        {
            List<AssemblyName> referencedAssemblies = new List<AssemblyName>();
            // Parcours de la liste des assemblies CLR
            foreach (AssemblyName an in asm.GetReferencedAssemblies())
            {
                // On ignore les assemblies systèmes
                if (Utils.StringCompareEquals(an.Name, "mscorlib") ||
                    an.Name.StartsWith("System", StringComparison.CurrentCultureIgnoreCase))
                    continue;

                // On regarde si cette assembly existe déjà dans le modèle
                DotNetAssembly eam = Component.FindDotNetAssemblyModelFromAssembly(an);
                if (eam != null)
                {
                    // Recherche si ce lien existe dèjà
                    bool notFound = AssemblyReferencesAssemblies.GetLink(this, eam) == null &&
                                    AssemblyReferencesAssemblies.GetLink(eam, this) == null;

                    if (notFound)
                    {
                        // Création du lien
                        AssemblyReferencesAssemblies ara = new AssemblyReferencesAssemblies(this, eam);
                        ara.Scope = ReferenceScope.Publish;
                        if (eam.IsInGac)
                            ara.Scope |= ReferenceScope.Runtime;
                        if (eam.Visibility == Visibility.Public)
                            ara.Scope |= ReferenceScope.Compilation;
                        //this.InternalAssemblyReferences.Add( eam );
                    }
                    continue;
                }

                // Idem si c'est une référence externe (binaire)
                ExternalComponent esm = Component.Model.FindExternalComponentByName(an.Name);
                if (esm != null && esm.MetaData != null && esm.MetaData.ComponentType == ComponentType.Library)
                {
                    foreach (ExternalPublicPort port in esm.Ports)
                    {
                        if (Utils.StringCompareEquals(an.Name, port.Name))
                        {
                            if (ExternalServiceReference.GetLink(this, port) == null)
                                ExternalServiceReferences.Add(port);
                        }
                        break;
                    }
                }

                // Liste des références externes à créer
                referencedAssemblies.Add(an);
            }
            return referencedAssemblies;
        }
    }
}