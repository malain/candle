using System;
using DSLFactory.Candle.SystemModel.Strategies;
using Microsoft.VisualStudio.Modeling;

namespace DSLFactory.Candle.SystemModel
{
    /// <summary>
    /// 
    /// </summary>
    partial class ServiceContract : ISupportDetailView
    {
        /// <summary>
        /// Gets the strategies owner.
        /// </summary>
        /// <value>The strategies owner.</value>
        public override CandleElement StrategiesOwner
        {
            get { return Layer.StrategiesOwner; }
        }

        /// <summary>
        /// Gets the owner.
        /// </summary>
        /// <value>The owner.</value>
        public override ICustomizableElement Owner
        {
            get { return Layer; }
        }

        /// <summary>
        /// Gets the name of the assembly qualified.
        /// </summary>
        /// <value>The name of the assembly qualified.</value>
        public string AssemblyQualifiedName
        {
            get { return String.Format("{0}, {1}", FullName, Layer.AssemblyName); }
        }

        /// <summary>
        /// Merges the configure.
        /// </summary>
        /// <param name="elementGroup">The element group.</param>
        protected override void MergeConfigure(ElementGroup elementGroup)
        {
            base.MergeConfigure(elementGroup);
            Name = "?"; // On veut forcer le nommage via la rule
        }
    }
}