using DSLFactory.Candle.SystemModel.Strategies;

namespace DSLFactory.Candle.SystemModel
{
    /// <summary>
    /// 
    /// </summary>
    partial class EnumValue : ITypeMember
    {
        /// <summary>
        /// Gets the owner.
        /// </summary>
        /// <value>The owner.</value>
        public override ICustomizableElement Owner
        {
            get { return Parent; }
        }

        /// <summary>
        /// Gets the data layer.
        /// </summary>
        /// <value>The data layer.</value>
        public DataLayer DataLayer
        {
            get { return Parent.Package.Layer; }
        }
    }
}