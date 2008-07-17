using System;

namespace DSLFactory.Candle.SystemModel.Repository
{
    /// <summary>
    /// Argument pour l'événement 
    /// </summary>
    public class MetadataChangedEventArg : EventArgs
    {
        private ComponentModelMetadata metadata;

        /// <summary>
        /// Initializes a new instance of the <see cref="MetadataChangedEventArg"/> class.
        /// </summary>
        /// <param name="metadata">The metadata.</param>
        public MetadataChangedEventArg(ComponentModelMetadata metadata)
        {
            this.metadata = metadata;
        }

        /// <summary>
        /// Gets or sets the metadata.
        /// </summary>
        /// <value>The metadata.</value>
        public ComponentModelMetadata Metadata
        {
            [System.Diagnostics.DebuggerStepThrough()]
            get { return metadata; }
            [System.Diagnostics.DebuggerStepThrough()]
            set { metadata = value; }
        }
    }
}