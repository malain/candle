namespace DSLFactory.Candle.SystemModel.Utilities
{
    /// <summary>
    /// Visitor utilisé avec LayerReferenceWalker
    /// </summary>
    public interface ILayerReferenceVisitor
    {
        /// <summary>
        /// Accepts the specified link.
        /// </summary>
        /// <param name="link">The link.</param>
        /// <param name="externalService">The external service.</param>
        void Accept(ClassUsesOperations link, ExternalServiceContract externalService);
        /// <summary>
        /// Accepts the specified link.
        /// </summary>
        /// <param name="link">The link.</param>
        /// <param name="internalService">The internal service.</param>
        void Accept(ClassUsesOperations link, ServiceContract internalService);
        /// <summary>
        /// Accepts the specified link.
        /// </summary>
        /// <param name="link">The link.</param>
        /// <param name="internalClass">The internal class.</param>
        void Accept(ClassUsesOperations link, ClassImplementation internalClass);
        /// <summary>
        /// Accepts the specified link.
        /// </summary>
        /// <param name="link">The link.</param>
        /// <param name="internalClass">The internal class.</param>
        void Accept(Implementation link, ClassImplementation internalClass);
    }
}