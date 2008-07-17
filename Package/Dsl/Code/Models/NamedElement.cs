using System;
using Microsoft.VisualStudio.Modeling;

namespace DSLFactory.Candle.SystemModel
{
    /// <summary>
    /// 
    /// </summary>
    partial class NamedElement
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="partition">Partition where new element is to be created.</param>
        /// <param name="propertyAssignments">List of domain property id/value pairs to set once the element is created.</param>
        protected NamedElement(Partition partition, PropertyAssignment[] propertyAssignments)
            : base(partition, propertyAssignments)
        {
            // InstanceId = Guid.NewGuid(); // Init par défaut
        }

        /// <summary>
        /// Allows the model element to configure itself immediately after the Merge process has related it to the target element.
        /// </summary>
        /// <param name="elementGroup">The group of source elements that have been added back into the target store.</param>
        protected override void MergeConfigure(ElementGroup elementGroup)
        {
            base.MergeConfigure(elementGroup);
            Comment = String.Empty;
        }
    }
}