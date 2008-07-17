using System;
using System.ComponentModel;

namespace DSLFactory.Candle.SystemModel.Strategies
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class CollectionElementPropertyDescriptor<T> : PropertyDescriptor where T : ISerializableProperty, new()
    {
        private readonly EditableCollection<T> _collection = null;
        private readonly int _index = -1;

        /// <summary>
        /// Initializes a new instance of the <see cref="CollectionElementPropertyDescriptor&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="coll">The coll.</param>
        /// <param name="idx">The idx.</param>
        public CollectionElementPropertyDescriptor(EditableCollection<T> coll, int idx)
            :
                base("#" + idx.ToString(), null)
        {
            _collection = coll;
            _index = idx;
        }

        /// <summary>
        /// Gets the collection of attributes for this member.
        /// </summary>
        /// <value></value>
        /// <returns>An <see cref="T:System.ComponentModel.AttributeCollection"></see> that provides the attributes for this member, or an empty collection if there are no attributes in the <see cref="P:System.ComponentModel.MemberDescriptor.AttributeArray"></see>.</returns>
        public override AttributeCollection Attributes
        {
            get { return new AttributeCollection(null); }
        }

        /// <summary>
        /// When overridden in a derived class, gets the type of the component this property is bound to.
        /// </summary>
        /// <value></value>
        /// <returns>A <see cref="T:System.Type"></see> that represents the type of component this property is bound to. When the <see cref="M:System.ComponentModel.PropertyDescriptor.GetValue(System.Object)"></see> or <see cref="M:System.ComponentModel.PropertyDescriptor.SetValue(System.Object,System.Object)"></see> methods are invoked, the object specified might be an instance of this type.</returns>
        public override Type ComponentType
        {
            get { return _collection.GetType(); }
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
        /// Gets the name of the member.
        /// </summary>
        /// <value></value>
        /// <returns>The name of the member.</returns>
        public override string Name
        {
            get { return "#" + _index.ToString(); }
        }

        /// <summary>
        /// When overridden in a derived class, gets the type of the property.
        /// </summary>
        /// <value></value>
        /// <returns>A <see cref="T:System.Type"></see> that represents the type of the property.</returns>
        public override Type PropertyType
        {
            get { return _collection[_index].GetType(); }
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
            return true;
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
            return _collection[_index];
        }

        /// <summary>
        /// When overridden in a derived class, resets the value for this property of the component to the default value.
        /// </summary>
        /// <param name="component">The component with the property value that is to be reset to the default value.</param>
        public override void ResetValue(object component)
        {
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

        /// <summary>
        /// When overridden in a derived class, sets the value of the component to a different value.
        /// </summary>
        /// <param name="component">The component with the property value that is to be set.</param>
        /// <param name="value">The new value.</param>
        public override void SetValue(object component, object value)
        {
            _collection[_index] = (T) value;
        }
    }
}