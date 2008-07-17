namespace DSLFactory.Candle.SystemModel
{
    /// <summary>
    /// 
    /// </summary>
    public interface IProvidesNamespaceResolver
    {
        /// <summary>
        /// Gets or sets the namespace resolver.
        /// </summary>
        /// <value>The namespace resolver.</value>
        StandardNamespaceResolver NamespaceResolver { get; set; }
    }
}