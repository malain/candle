using System;

namespace DSLFactory.Candle.SystemModel.Strategies
{
    /// <summary>
    /// 
    /// </summary>
    public interface IDependencyProperty
    {
        /// <summary>
        /// Gets the type converter.
        /// </summary>
        /// <value>The type converter.</value>
        Type TypeConverter { get;}
        /// <summary>
        /// Gets the type of the property.
        /// </summary>
        /// <value>The type of the property.</value>
        Type PropertyType { get;}
        /// <summary>
        /// Gets the default value.
        /// </summary>
        /// <returns></returns>
        object GetDefaultValue();
        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>The name.</value>
        string Name { get;}
    }
}