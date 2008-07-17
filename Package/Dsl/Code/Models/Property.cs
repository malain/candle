using System;
using System.Collections.Generic;
using System.Text;
using DSLFactory.Candle.SystemModel.Utilities;
using Microsoft.VisualStudio.Modeling;
using System.ComponentModel;
using DSLFactory.Candle.SystemModel.Strategies;

namespace DSLFactory.Candle.SystemModel
{
//    [TypeDescriptionProvider( typeof( StrategyProviderTypeDescriptorProvider ) )]
    /// <summary>
    /// 
    /// </summary>
    partial class Property : ITypeMember
    {
        /// <summary>
        /// Gets the owner.
        /// </summary>
        /// <value>The owner.</value>
        public override ICustomizableElement Owner
        {
            get { return this.Parent; }
        }

        /// <summary>
        /// Gets the data layer.
        /// </summary>
        /// <value>The data layer.</value>
        public DataLayer DataLayer
        {
            get { return Parent.DataLayer; }
        }

        /// <summary>
        /// Returns a <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
        /// </returns>
        public override string ToString()
        {
            return this.Name;
        }

        /// <summary>
        /// Gets a value indicating whether this instance is foreign key.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is foreign key; otherwise, <c>false</c>.
        /// </value>
        public bool IsForeignKey
        {
            get
            {
                foreach( Association association in Association.GetLinksToTargets( this.Parent ) )
                {
                    foreach (ForeignKey fk in association.ForeignKeys)
                    {
                        if (fk.Column == this)
                            return true;
                    }
                }
                return false;
            }
        }
    }
}
