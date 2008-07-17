using System;
using System.ComponentModel;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Design;

namespace DSLFactory.Candle.SystemModel.Strategies
{
    /// <summary>
    /// 
    /// </summary>
    public class StrategyProviderTypeDescriptorProvider : TypeDescriptionProvider
    {
        /// <summary>
        /// Gets a custom type descriptor for the given type and object.
        /// </summary>
        /// <param name="objectType">The type of object for which to retrieve the type descriptor.</param>
        /// <param name="instance">An instance of the type. Can be null if no instance was passed to the <see cref="T:System.ComponentModel.TypeDescriptor"></see>.</param>
        /// <returns>
        /// An <see cref="T:System.ComponentModel.ICustomTypeDescriptor"></see> that can provide metadata for the type.
        /// </returns>
        public override ICustomTypeDescriptor GetTypeDescriptor(Type objectType, object instance)
        {
            if (instance == null)
                return base.GetTypeDescriptor(objectType, instance);

            return new StrategyProviderTypeDescriptor((ModelElement) instance);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class StrategyProviderTypeDescriptor : ElementTypeDescriptor
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StrategyProviderTypeDescriptor"/> class.
        /// </summary>
        /// <param name="obj">The obj.</param>
        public StrategyProviderTypeDescriptor(ModelElement obj)
            : base(obj)
        {
        }

        /// <summary>
        /// Gets the properties.
        /// </summary>
        /// <param name="attributes">The attributes.</param>
        /// <returns></returns>
        public override PropertyDescriptorCollection GetProperties(Attribute[] attributes)
        {
            PropertyDescriptorCollection properties = base.GetProperties(attributes);

            // Recherche dans les stratégies associés à ce modèle
            if (ModelElement is ICustomizableElement && !ModelElement.IsDeleted)
            {
                foreach (
                    StrategyBase strategy in
                        StrategyManager.GetStrategies(((ICustomizableElement) ModelElement).StrategiesOwner, null))
                    // On prend toutes les stratégies
                {
                    try
                    {
                        foreach (PropertyDescriptor prop in strategy.GetCustomProperties(ModelElement))
                        {
                            properties.Add(prop);
                        }
                    }
                    catch
                    {
                    }
                }
            }
            return properties;
        }
    }
}