namespace DSLFactory.Candle.SystemModel.Commands
{
    /// <summary>
    /// Interface à implémenter pour les shapes qui peuvent positionner leurs shapes enfants
    /// </summary>
    public interface ISupportArrangeShapes
    {
        /// <summary>
        /// Arranges the shapes.
        /// </summary>
        void ArrangeShapes();
    }
}
