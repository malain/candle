namespace DSLFactory.Candle.SystemModel
{
    /// <summary>
    /// 
    /// </summary>
    public interface IArgument : ITypeMember
    {
        /// <summary>
        /// Gets or sets the direction of the argument .
        /// </summary>
        /// <value>The direction.</value>
        ArgumentDirection Direction { get; set; }
    }
}