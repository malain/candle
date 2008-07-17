using System;
using System.ComponentModel;
using Microsoft.VisualStudio.Modeling;

namespace DSLFactory.Candle.SystemModel.Strategies
{
    /// <summary>
    /// Handler sur une propriété personnalisée d'une stratégie permettant de persister sa
    /// valeur.
    /// </summary>
    /// <typeparam name="T">Type de la propriété</typeparam>
    public class DependencyPropertyDescriptor<T> : PropertyDescriptor
    {
        private readonly ICustomizableElement _ownerModel;
        private readonly DependencyProperty<T> _propertyConfiguration;

        /// <summary>
        /// Initializes a new instance of the <see cref="DependencyPropertyDescriptor&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="propertyConfiguration">The property configuration.</param>
        public DependencyPropertyDescriptor(ICustomizableElement model, DependencyProperty<T> propertyConfiguration)
            : base(propertyConfiguration.Name, propertyConfiguration.Attributes.ToArray())
        {
            _ownerModel = model;
            _propertyConfiguration = propertyConfiguration;
        }

        /// <summary>
        /// When overridden in a derived class, gets the type of the property.
        /// </summary>
        /// <value></value>
        /// <returns>A <see cref="T:System.Type"></see> that represents the type of the property.</returns>
        public override Type PropertyType
        {
            get { return _propertyConfiguration.PropertyType; }
        }

        /// <summary>
        /// When overridden in a derived class, gets the type of the component this property is bound to.
        /// </summary>
        /// <value></value>
        /// <returns>A <see cref="T:System.Type"></see> that represents the type of component this property is bound to. When the <see cref="M:System.ComponentModel.PropertyDescriptor.GetValue(System.Object)"></see> or <see cref="M:System.ComponentModel.PropertyDescriptor.SetValue(System.Object,System.Object)"></see> methods are invoked, the object specified might be an instance of this type.</returns>
        public override Type ComponentType
        {
            get { return typeof (ModelElement); }
        }

        /// <summary>
        /// When overridden in a derived class, gets a value indicating whether this property is read-only.
        /// </summary>
        /// <value></value>
        /// <returns>true if the property is read-only; otherwise, false.</returns>
        public override bool IsReadOnly
        {
            get { return false; }
        }

        /// <summary>
        /// When overridden in a derived class, gets the current value of the property on a component.
        /// </summary>
        /// <param name="component">The component with the property for which to retrieve the value.</param>
        /// <returns>
        /// The value of a property for a given component.
        /// </returns>
        public override object GetValue(object component)
        {
            return _propertyConfiguration.GetValue(_ownerModel);
        }

        /// <summary>
        /// When overridden in a derived class, sets the value of the component to a different value.
        /// </summary>
        /// <param name="component">The component with the property value that is to be set.</param>
        /// <param name="value">The new value.</param>
        public override void SetValue(object component, object value)
        {
            _propertyConfiguration.SetValue(_ownerModel, (T) value);
        }

        /// <summary>
        /// When overridden in a derived class, returns whether resetting an object changes its value.
        /// </summary>
        /// <param name="component">The component to test for reset capability.</param>
        /// <returns>
        /// true if resetting the component changes its value; otherwise, false.
        /// </returns>
        public override bool CanResetValue(object component)
        {
            if (!_propertyConfiguration.HasDefaultValue)
                return false;
            T defaultValue = _propertyConfiguration.DefaultValue;
            if (defaultValue == null && component == null)
                return true;
            return defaultValue.Equals(component);
        }

        /// <summary>
        /// When overridden in a derived class, resets the value for this property of the component to the default value.
        /// </summary>
        /// <param name="component">The component with the property value that is to be reset to the default value.</param>
        public override void ResetValue(object component)
        {
            SetValue(component, _propertyConfiguration.DefaultValue);
        }

        /// <summary>
        /// When overridden in a derived class, determines a value indicating whether the value of this property needs to be persisted.
        /// </summary>
        /// <param name="component">The component with the property to be examined for persistence.</param>
        /// <returns>
        /// true if the property should be persisted; otherwise, false.
        /// </returns>
        public override bool ShouldSerializeValue(object component)
        {
            return true;
        }
    }
}