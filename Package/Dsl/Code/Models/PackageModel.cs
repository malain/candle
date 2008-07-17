using System;
using System.Diagnostics;
using DSLFactory.Candle.SystemModel.Strategies;
using Microsoft.VisualStudio.Modeling;

namespace DSLFactory.Candle.SystemModel
{
    /// <summary>
    /// 
    /// </summary>
    partial class Package : IHasNamespace
    {
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
                ICustomizableElement elem = StrategiesOwner;
                while (elem != null)
                {
                    if (elem is IProvidesNamespaceResolver)
                    {
                        resolver = ((IProvidesNamespaceResolver) elem).NamespaceResolver;
                        break;
                    }
                    elem = elem.Owner;
                }

                Debug.Assert(resolver != null);
                return resolver.Resolve(Name);
            }
        }

        #endregion

        /// <summary>
        /// Allows the model element to configure itself immediately after the Merge process has related it to the target element.
        /// </summary>
        /// <param name="elementGroup">The group of source elements that have been added back into the target store.</param>
        protected override void MergeConfigure(ElementGroup elementGroup)
        {
            base.MergeConfigure(elementGroup);
            if (Layer.Packages.Count == 1)
                Name = Layer.Namespace;
            else
                Name = String.Format("{0}.{1}", Layer.Namespace, Name);
        }
    }
}