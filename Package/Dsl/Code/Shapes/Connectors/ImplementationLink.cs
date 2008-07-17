using System;
using Microsoft.VisualStudio.Modeling.Diagrams.GraphObject;

namespace DSLFactory.Candle.SystemModel
{
    partial class ImplementationLink
    {
        /// <summary>
        /// Gets the default line routing style for this connector.
        /// </summary>
        /// <value></value>
        [CLSCompliant(false)]
        protected override VGRoutingStyle DefaultRoutingStyle
        {
            get { return VGRoutingStyle.VGRouteSimpleHV; }
        }
    }
}