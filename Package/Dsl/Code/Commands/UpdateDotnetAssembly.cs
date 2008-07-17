using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows.Forms;
using DSLFactory.Candle.SystemModel.Repository;
using Microsoft.VisualStudio.Modeling;

namespace DSLFactory.Candle.SystemModel.Commands
{
    /// <summary>
    /// Mise � jour d'une nouvelle version d'un assembly de composant binaire
    /// </summary>
    public class UpdateDotNetAssembly : ICommand
    {
        private readonly DotNetAssembly _element;

        /// <summary>
        /// Initializes a new instance of the <see cref="GenerateAllCommand"/> class.
        /// </summary>
        /// <param name="shape">The shape.</param>
        public UpdateDotNetAssembly(object shape)
        {
            if (shape != null && shape is DotnetAssemblyShape)
            {
                _element = ((DotnetAssemblyShape) shape).ModelElement as DotNetAssembly;
            }
        }

        #region ICommand Members

        /// <summary>
        /// Gets a value indicating whether this <see cref="ICommand"/> is enabled.
        /// </summary>
        /// <value><c>true</c> if enabled; otherwise, <c>false</c>.</value>
        public bool Enabled
        {
            get { return Visible(); }
        }

        /// <summary>
        /// V�rification si la commande peut s'activer
        /// </summary>
        /// <returns></returns>
        /// <value><c>true</c> if visible; otherwise, <c>false</c>.</value>
        public bool Visible()
        {
            return _element != null;
        }

        /// <summary>
        /// Execute the command
        /// </summary>
        public void Exec()
        {
            // Affiche la fenetre de dialogue permettant de choisir les assemblies
            IAssemblySelectorDialog selector = ServiceLocator.Instance.GetService<IAssemblySelectorDialog>();
            if (selector.ShowDialog(1)) // Une seule s�lection possible
            {
                Assembly asm = selector.SelectedAssemblies[0];
                using (Transaction transaction = _element.Store.TransactionManager.BeginTransaction("Update model version"))
                {
                    // Mise � jour de la version du composant et son emplacement initial
                    // et cr�ation des nouvelles d�pendances (les d�pendances actuelles ne seront pas touch�es
                    // mais il faudra s'assurer qu'elles soient bien � jour car la proc�dure ne v�rifie que le nom 
                    // et ignore la version)
                    _element.InitFromAssembly(asm, true /* creation des nouvelles d�pendances*/);
                    transaction.Commit();
                }

                // V�rification si les assemblies existantes poss�dent la bonne version
                foreach (AssemblyName assemblyName in asm.GetReferencedAssemblies())
                {
                    // On ignore les assemblies syst�mes
                    if (Utils.StringCompareEquals(assemblyName.Name, "mscorlib") ||
                        assemblyName.Name.StartsWith("System", StringComparison.CurrentCultureIgnoreCase))
                        continue;

                    // On regarde si cette assembly existe d�j� dans le mod�le
                    DotNetAssembly eam = _element.Component.FindDotNetAssemblyModelFromAssembly(assemblyName);
                    if (eam != null)
                    {
                        if (!assemblyName.Version.Equals((Version) eam.Version))
                        {
                            using (Transaction transaction = _element.Store.TransactionManager.BeginTransaction("Update assembly version"))
                            {
                                eam.Version = new VersionInfo(assemblyName.Version);
                                transaction.Commit();
                            }
                        }
                    }
                    else
                    {
                        // Idem pour les composants externes
                        ExternalComponent esm = _element.Component.Model.FindExternalComponentByName(assemblyName.Name);
                        if (esm != null && esm.MetaData != null && esm.MetaData.ComponentType == ComponentType.Library)
                        {
                            if (!assemblyName.Version.Equals((Version)esm.Version))
                            {
                                // Recherche si il existe un mod�le avec la bonne version
                                List<ComponentModelMetadata> versions = RepositoryManager.Instance.ModelsMetadata.Metadatas.GetAllVersions(esm.Id);
                                ComponentModelMetadata metadata = versions.Find(delegate(ComponentModelMetadata m) { return assemblyName.Version.Equals(m.Version); });
                                using (Transaction transaction = _element.Store.TransactionManager.BeginTransaction("Update model version"))
                                {
                                    if (metadata != null)
                                    {
                                        // OK on met a jour
                                        esm.Version = metadata.Version;
                                    }
                                    else // erreur On a pas trouv� de mod�le
                                    {
                                        // Sauvegarde de la bonne version
                                        esm.Version = new VersionInfo(assemblyName.Version);
                                        // On force � null pour obliger l'utilisateur � s�lectionner un
                                        // mod�le
                                        esm.ModelMoniker = Guid.Empty; 
                                    }
                                    transaction.Commit();
                                }
                            }
                        }
                    }
                }

                // Demande si il faut aussi mettre � jour le n� de version du composant
                if (_element.Visibility == Visibility.Public)
                {
                    if( MessageBox.Show(String.Format("Do you want to change the current version of the component from {0} to {1}",
                                          _element.Component.Model.Version, _element.Version), "Component version change", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        using (Transaction transaction = _element.Store.TransactionManager.BeginTransaction("Update model version"))
                        {
                            _element.Component.Model.Version = _element.Version;
                            transaction.Commit();
                        }
                    }
                }
            }
        }

        #endregion
    }
}