using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using DSLFactory.Candle.SystemModel.Dependencies;

namespace DSLFactory.Candle.SystemModel
{
    /// <summary>
    /// 
    /// </summary>
    partial class BinaryComponent : IHasReferences
    {
        /// <summary>
        /// Gets the public ports.
        /// </summary>
        /// <value>The public ports.</value>
        public List<NamedElement> PublicPorts
        {
            get
            {
                List<NamedElement> ports = new List<NamedElement>();
                foreach (DotNetAssembly assembly in Assemblies)
                {
                    if (assembly.Visibility == Visibility.Public)
                        ports.Add(assembly);
                }
                return ports;
            }
        }

        //public void RetrieveReferences(ReferenceContext ctx) { }

        #region IHasReferences Members

        /// <summary>
        /// Liste des références
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        public IEnumerable<ReferenceItem> GetReferences(ReferenceContext context)
        {
            // Compilation
            if (context.Scope == ReferenceScope.Compilation)
            {
                foreach (DotNetAssembly assembly in Assemblies)
                {
                    if (context.CheckPort(assembly.Id))
                        yield return new ReferenceItem(this, assembly, context.IsExternal);
                }
            }

            // Tout
            if (context.Scope == ReferenceScope.Runtime || context.Scope == ReferenceScope.Publish ||
                context.Scope == ReferenceScope.All)
            {
                foreach (DotNetAssembly layer in Assemblies)
                {
                    yield return new ReferenceItem(this, layer, context.IsExternal);
                }
            }
        }

        #endregion

        /// <summary>
        /// Recherche une externalAssembly par rapport à une assembly
        /// </summary>
        /// <param name="assemblyName">Name of the assembly.</param>
        /// <returns></returns>
        internal DotNetAssembly FindDotNetAssemblyModelFromAssembly(AssemblyName assemblyName)
        {
            Debug.Assert(assemblyName != null, "assembly ne peut pas être null");

            // On recherche juste sur le nom sans tenir compte de la version
            // car il ne peut de toute façon y avoir qu'une seule version de la même assembly
            // dans un modèle
            foreach (DotNetAssembly externalAssembly in Assemblies)
            {
                if (Utils.StringCompareEquals(externalAssembly.Name, assemblyName.Name))
                    return externalAssembly;
            }
            return null;
        }
    }
}