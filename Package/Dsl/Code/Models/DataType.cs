using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.Modeling;
using DSLFactory.Candle.SystemModel.Utilities;
using System.Collections;
using DSLFactory.Candle.SystemModel.Strategies;

namespace DSLFactory.Candle.SystemModel
{
    /// <summary>
    /// Base type for the models layer
    /// </summary>
    partial class DataType : IShowCodeProperties, IHasNamespace
    {
        /// <summary>
        /// Gets the strategies owner.
        /// </summary>
        /// <value>The strategies owner.</value>
        public override CandleElement StrategiesOwner
        {
            get { return this.Package.StrategiesOwner; }
        }

        /// <summary>
        /// Gets the owner.
        /// </summary>
        /// <value>The owner.</value>
        public override DSLFactory.Candle.SystemModel.Strategies.ICustomizableElement Owner
        {
            get { return this.Package; }
        }

        /// <summary>
        /// Gets the name of the assembly qualified.
        /// </summary>
        /// <value>The name of the assembly qualified.</value>
        public string AssemblyQualifiedName
        {
            get { return String.Format( "{0}, {1}", FullName, this.DataLayer.AssemblyName ); }
        }

        /// <summary>
        /// Gets the data layer.
        /// </summary>
        /// <value>The data layer.</value>
        public DataLayer DataLayer
        {
            get
            {
                Package package = this.Package;
                return package != null ? package.Layer : null;
            }
        }

        /// <summary>
        /// Gets the name of the layer.
        /// </summary>
        /// <value>The name of the layer.</value>
        string IShowCodeProperties.LayerName
        {
            get { return DataLayer.Name; }
        }

        /// <summary>
        /// Gets the name of the member.
        /// </summary>
        /// <param name="initialName">The initial name.</param>
        /// <returns></returns>
        string IShowCodeProperties.GetMemberName( string initialName )
        {
            return initialName;
        }

        /// <summary>
        /// Returns a <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
        /// </returns>
        public override string ToString()
        {
            return this.FullName;
        }

        /// <summary>
        /// Recherche itérative du parent
        /// </summary>
        /// <value></value>
        public string NamespaceDeclaration
        {
            get
            {
                ICustomizableElement parent = this.Owner;
                IHasNamespace typeContainer = null;
                while (parent != null)
                {
                    if (parent is IHasNamespace)
                    {
                        typeContainer = parent as IHasNamespace;
                        break;
                    }
                    parent = parent.Owner;
                }

                return typeContainer.NamespaceDeclaration;
            }
        }

        /// <summary>
        /// Gets the full name of the type.
        /// </summary>
        /// <value>The full name of the type.</value>
        public override string FullTypeName
        {
            get
            {
                return FullName;
            }
        }

        /// <summary>
        /// Gets the full name.
        /// </summary>
        /// <value>The full name.</value>
        public override string FullName
        {
            get { return String.Concat(NamespaceDeclaration, '.', Name); }
        }
    }
}
