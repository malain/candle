using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace DSLFactory.Candle.SystemModel.Strategies
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class EditableCollection<T> : List<T>, ICustomTypeDescriptor, ISerializableProperty
        where T : ISerializableProperty, new()
    {
        #region ICustomTypeDescriptor impl

        /// <summary>
        /// Returns the class name of this instance of a component.
        /// </summary>
        /// <returns>
        /// The class name of the object, or null if the class does not have a name.
        /// </returns>
        public String GetClassName()
        {
            return TypeDescriptor.GetClassName(this, true);
        }

        /// <summary>
        /// Returns a collection of custom attributes for this instance of a component.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.ComponentModel.AttributeCollection"></see> containing the attributes for this object.
        /// </returns>
        public AttributeCollection GetAttributes()
        {
            return TypeDescriptor.GetAttributes(this, true);
        }

        /// <summary>
        /// Returns the name of this instance of a component.
        /// </summary>
        /// <returns>
        /// The name of the object, or null if the object does not have a name.
        /// </returns>
        public String GetComponentName()
        {
            return TypeDescriptor.GetComponentName(this, true);
        }

        /// <summary>
        /// Returns a type converter for this instance of a component.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.ComponentModel.TypeConverter"></see> that is the converter for this object, or null if there is no <see cref="T:System.ComponentModel.TypeConverter"></see> for this object.
        /// </returns>
        public TypeConverter GetConverter()
        {
            return TypeDescriptor.GetConverter(this, true);
        }

        /// <summary>
        /// Returns the default event for this instance of a component.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.ComponentModel.EventDescriptor"></see> that represents the default event for this object, or null if this object does not have events.
        /// </returns>
        public EventDescriptor GetDefaultEvent()
        {
            return TypeDescriptor.GetDefaultEvent(this, true);
        }

        /// <summary>
        /// Returns the default property for this instance of a component.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.ComponentModel.PropertyDescriptor"></see> that represents the default property for this object, or null if this object does not have properties.
        /// </returns>
        public PropertyDescriptor GetDefaultProperty()
        {
            return TypeDescriptor.GetDefaultProperty(this, true);
        }

        /// <summary>
        /// Returns an editor of the specified type for this instance of a component.
        /// </summary>
        /// <param name="editorBaseType">A <see cref="T:System.Type"></see> that represents the editor for this object.</param>
        /// <returns>
        /// An <see cref="T:System.Object"></see> of the specified type that is the editor for this object, or null if the editor cannot be found.
        /// </returns>
        public object GetEditor(Type editorBaseType)
        {
            return TypeDescriptor.GetEditor(this, editorBaseType, true);
        }

        /// <summary>
        /// Returns the events for this instance of a component using the specified attribute array as a filter.
        /// </summary>
        /// <param name="attributes">An array of type <see cref="T:System.Attribute"></see> that is used as a filter.</param>
        /// <returns>
        /// An <see cref="T:System.ComponentModel.EventDescriptorCollection"></see> that represents the filtered events for this component instance.
        /// </returns>
        public EventDescriptorCollection GetEvents(Attribute[] attributes)
        {
            return TypeDescriptor.GetEvents(this, attributes, true);
        }

        /// <summary>
        /// Returns the events for this instance of a component.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.ComponentModel.EventDescriptorCollection"></see> that represents the events for this component instance.
        /// </returns>
        public EventDescriptorCollection GetEvents()
        {
            return TypeDescriptor.GetEvents(this, true);
        }

        /// <summary>
        /// Returns an object that contains the property described by the specified property descriptor.
        /// </summary>
        /// <param name="pd">A <see cref="T:System.ComponentModel.PropertyDescriptor"></see> that represents the property whose owner is to be found.</param>
        /// <returns>
        /// An <see cref="T:System.Object"></see> that represents the owner of the specified property.
        /// </returns>
        public object GetPropertyOwner(PropertyDescriptor pd)
        {
            return this;
        }


        /// <summary>
        /// Called to get the properties of this type. Returns properties with certain
        /// attributes. this restriction is not implemented here.
        /// </summary>
        /// <param name="attributes">An array of type <see cref="T:System.Attribute"></see> that is used as a filter.</param>
        /// <returns>
        /// A <see cref="T:System.ComponentModel.PropertyDescriptorCollection"></see> that represents the filtered properties for this component instance.
        /// </returns>
        public PropertyDescriptorCollection GetProperties(Attribute[] attributes)
        {
            return GetProperties();
        }

        /// <summary>
        /// Called to get the properties of this type.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.ComponentModel.PropertyDescriptorCollection"></see> that represents the properties for this component instance.
        /// </returns>
        public PropertyDescriptorCollection GetProperties()
        {
            // Create a collection object to hold property descriptors
            PropertyDescriptorCollection pds = new PropertyDescriptorCollection(null);

            // Iterate the list of employees
            for (int i = 0; i < Count; i++)
            {
                // Create a property descriptor for the employee item and add to the property descriptor collection
                CollectionElementPropertyDescriptor<T> pd = new CollectionElementPropertyDescriptor<T>(this, i);
                pds.Add(pd);
            }
            // return the property descriptor collection
            return pds;
        }

        #endregion

        // Implementation of interface ICustomTypeDescriptor 

        /// <summary>
        /// Gets the description.
        /// </summary>
        /// <value>The description.</value>
        public string Description
        {
            get { return String.Empty; }
        }

        /// <summary>
        /// Gets the display name.
        /// </summary>
        /// <value>The display name.</value>
        public string DisplayName
        {
            get { return ToString(); }
        }

        #region ISerializableProperty Members

        /// <summary>
        /// Converts from string.
        /// </summary>
        /// <param name="value">The value.</param>
        public void ConvertFromString(string value)
        {
            if (value == null)
                return;

            // Split avec la virgule comme séparateur
            List<string> args = new List<string>();
            int deb = 0;
            bool inString = false;
            for (int i = 0; i < value.Length; i++)
            {
                if (value[i] == '|' && !inString)
                {
                    T elem = new T();
                    elem.ConvertFromString(value.Substring(deb, i - deb));
                    Add(elem);
                    deb = i + 1;
                }
                else if (value[i] == '"')
                    inString = !inString;
            }

            if (deb < value.Length)
            {
                T el = new T();
                el.ConvertFromString(value.Substring(deb));
                Add(el);
            }
        }

        /// <summary>
        /// Converts to string.
        /// </summary>
        /// <returns></returns>
        public string ConvertToString()
        {
            return ConvertToString("|");
        }

        #endregion

        /// <summary>
        /// Converts to string.
        /// </summary>
        /// <param name="sep">The sep.</param>
        /// <returns></returns>
        public string ConvertToString(string sep)
        {
            StringBuilder sb = new StringBuilder();
            foreach (T elem in this)
            {
                sb.Append(elem.ConvertToString());
                sb.Append(sep);
            }
            return sb.ToString();
        }

        /// <summary>
        /// Returns a <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
        /// </returns>
        public override string ToString()
        {
            return ConvertToString(",");
        }
    }
}