using System;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Xml;
using Microsoft.VisualStudio.Modeling;

namespace DSLFactory.Candle.SystemModel.Strategies
{
    /// <summary>
    /// 
    /// </summary>
    [TypeConverter(typeof(DependencyPropertyValueConverter))]
    public partial class DependencyPropertyValue
    {
        private string valueAsString;
        private object internalValue;
        private IDependencyProperty property;

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>The value.</value>
        public string Value
        {
            get { return valueAsString; }
            set { valueAsString = value; }
        }

        /// <summary>
        /// Sets the value.
        /// </summary>
        /// <param name="property">The property.</param>
        /// <param name="value">The value.</param>
        internal void SetValue(IDependencyProperty property, object value)
        {
            this.property = property;
            internalValue = value;
        }

        /// <summary>
        /// Gets the value.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="property">The property.</param>
        /// <returns></returns>
        internal T GetValue<T>(IDependencyProperty property)
        {
            if (this.property == null)
                internalValue = GetValue<T>(property, valueAsString);
            this.property = property;
            return (T)internalValue;
        }

        /// <summary>
        /// Gets the value.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="property">The property.</param>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        public object GetValue<T>(IDependencyProperty property, string input)
        {
            if (input == null)
                return null;

            try
            {
                Type t = property.PropertyType;
                if (t == typeof(int))
                {
                    int result;
                    if (int.TryParse(input, NumberStyles.Integer, CultureInfo.InvariantCulture, out result))
                    {
                        return result;
                    }
                }
                else
                {
                    if (t == typeof(string))
                    {
                        return input;
                    }
                    if (t == typeof(bool))
                    {
                        bool flag;
                        switch (input)
                        {
                            case "true":
                                return true;

                            case "false":
                                return false;
                        }
                        if (bool.TryParse(input, out flag))
                        {
                            return flag;
                        }
                    }
                    else
                    {
                        T output;
                        //if (t == typeof(Color))
                        //{
                        //    return GetColorValue<T>(input, t);
                        //}
                        TypeConverter converter;
                        Type tc = property.TypeConverter;
                        if (tc != null)
                            converter = (TypeConverter)Activator.CreateInstance(tc);
                        else
                            converter = TypeDescriptor.GetConverter(property.PropertyType);
                        if (((converter != null) && converter.CanConvertFrom(typeof(string))) && converter.CanConvertTo(typeof(string)))
                        {
                            return (T)converter.ConvertFromInvariantString(input);
                        }
                        if (SerializationUtilities.TryGetValueFromBinaryForm<T>(input, out output))
                        {
                            return output;
                        }
                    }
                }
            }
            catch { }

            return default(T);
        }

        /// <summary>
        /// Returns a <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
        /// </returns>
        public override string ToString()
        {
            if (property == null)
                return valueAsString;

            Type propertyType = property.PropertyType;
            if (propertyType == typeof(int))
            {
                int num = (int)internalValue;
                return num.ToString(CultureInfo.InvariantCulture);
            }
            if (propertyType == typeof(bool))
            {
                if ((bool)internalValue)
                {
                    return "true";
                }
                return "false";
            }
            if (propertyType == typeof(string))
            {
                if (internalValue == null)
                {
                    return string.Empty;
                }
                return internalValue.ToString();
            }
            if (internalValue == null)
            {
                return null;
            }

            TypeConverter converter;
            Type t = property.TypeConverter;
            if (t != null)
                converter = (TypeConverter)Activator.CreateInstance(t);
            else
                converter = TypeDescriptor.GetConverter(property.PropertyType);
            if (((converter != null) && converter.CanConvertFrom(typeof(string))) && converter.CanConvertTo(typeof(string)))
            {
                return converter.ConvertToInvariantString(internalValue);
            }
            try
            {
                using (MemoryStream stream = new MemoryStream())
                {
                    if (internalValue is XmlDocument)
                    {
                        using (StreamWriter writer = new StreamWriter(stream, Encoding.UTF8))
                        {
                            ((XmlDocument)internalValue).Save(writer);
                        }
                    }
                    else
                        new BinaryFormatter().Serialize(stream, internalValue);

                    return Convert.ToBase64String(stream.ToArray());
                }
            }
            catch (SerializationException)
            {
            }
            return internalValue.ToString();
        }
    }
}