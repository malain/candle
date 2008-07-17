using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using DSLFactory.Candle.SystemModel.Repository;
using Microsoft.VisualStudio.Modeling.Diagrams;

namespace DSLFactory.Candle.SystemModel
{
    /// <summary>
    /// 
    /// </summary>
    internal class VersionTypeConverter : TypeConverter
    {
        /// <summary>
        /// Returns whether this converter can convert an object of the given type to the type of this converter, using the specified context.
        /// </summary>
        /// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext"></see> that provides a format context.</param>
        /// <param name="sourceType">A <see cref="T:System.Type"></see> that represents the type you want to convert from.</param>
        /// <returns>
        /// true if this converter can perform the conversion; otherwise, false.
        /// </returns>
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if (sourceType == typeof (string))
                return true;
            return base.CanConvertFrom(context, sourceType);
        }

        /// <summary>
        /// Converts the given object to the type of this converter, using the specified context and culture information.
        /// </summary>
        /// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext"></see> that provides a format context.</param>
        /// <param name="culture">The <see cref="T:System.Globalization.CultureInfo"></see> to use as the current culture.</param>
        /// <param name="value">The <see cref="T:System.Object"></see> to convert.</param>
        /// <returns>
        /// An <see cref="T:System.Object"></see> that represents the converted value.
        /// </returns>
        /// <exception cref="T:System.NotSupportedException">The conversion cannot be performed. </exception>
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (value != null && value is string)
            {
                string str = (string) value;
                if (str.Length == 0)
                    str = "0.0.0.0";
                return new VersionInfo(new Version(str));
            }
            return base.ConvertFrom(context, culture, value);
        }

        /// <summary>
        /// Returns whether this converter can convert the object to the specified type, using the specified context.
        /// </summary>
        /// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext"></see> that provides a format context.</param>
        /// <param name="destinationType">A <see cref="T:System.Type"></see> that represents the type you want to convert to.</param>
        /// <returns>
        /// true if this converter can perform the conversion; otherwise, false.
        /// </returns>
        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            if (destinationType == typeof (string) && destinationType == typeof (Version))
                return true;
            return base.CanConvertTo(context, destinationType);
        }

        /// <summary>
        /// Converts the given value object to the specified type, using the specified context and culture information.
        /// </summary>
        /// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext"></see> that provides a format context.</param>
        /// <param name="culture">A <see cref="T:System.Globalization.CultureInfo"></see>. If null is passed, the current culture is assumed.</param>
        /// <param name="value">The <see cref="T:System.Object"></see> to convert.</param>
        /// <param name="destinationType">The <see cref="T:System.Type"></see> to convert the value parameter to.</param>
        /// <returns>
        /// An <see cref="T:System.Object"></see> that represents the converted value.
        /// </returns>
        /// <exception cref="T:System.NotSupportedException">The conversion cannot be performed. </exception>
        /// <exception cref="T:System.ArgumentNullException">The destinationType parameter is null. </exception>
        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value,
                                         Type destinationType)
        {
            if (value != null && value is VersionInfo)
            {
                if (destinationType == typeof (string))
                {
                    VersionInfo vi = (VersionInfo) value;
                    return new Version(vi.Major, vi.Minor, vi.Build, vi.Revision).ToString();
                }
                if (destinationType == typeof (Version))
                {
                    VersionInfo vi = (VersionInfo) value;
                    return (Version) vi;
                }
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }

        /// <summary>
        /// Returns whether this object supports a standard set of values that can be picked from a list, using the specified context.
        /// </summary>
        /// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext"></see> that provides a format context.</param>
        /// <returns>
        /// true if <see cref="M:System.ComponentModel.TypeConverter.GetStandardValues"></see> should be called to find a common set of values the object supports; otherwise, false.
        /// </returns>
        public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
        {
            NodeShape shape = context.Instance as NodeShape;
            if (shape != null)
            {
                if (shape.ModelElement is ExternalComponent)
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Returns whether the collection of standard values returned from <see cref="M:System.ComponentModel.TypeConverter.GetStandardValues"></see> is an exclusive list of possible values, using the specified context.
        /// </summary>
        /// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext"></see> that provides a format context.</param>
        /// <returns>
        /// true if the <see cref="T:System.ComponentModel.TypeConverter.StandardValuesCollection"></see> returned from <see cref="M:System.ComponentModel.TypeConverter.GetStandardValues"></see> is an exhaustive list of possible values; false if other values are possible.
        /// </returns>
        public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
        {
            NodeShape shape = context.Instance as NodeShape;
            if (shape != null)
            {
                if (shape.ModelElement is ExternalComponent)
                    return true;
                if (shape.ModelElement is DotNetAssembly) // Non modifiable
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Returns a collection of standard values for the data type this type converter is designed for when provided with a format context.
        /// </summary>
        /// <param name="context">An <see cref="T:System.ComponentModel.ITypeDescriptorContext"></see> that provides a format context that can be used to extract additional information about the environment from which this converter is invoked. This parameter or properties of this parameter can be null.</param>
        /// <returns>
        /// A <see cref="T:System.ComponentModel.TypeConverter.StandardValuesCollection"></see> that holds a standard set of valid values, or null if the data type does not support a standard set of values.
        /// </returns>
        public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            NodeShape shape = context.Instance as NodeShape;
            if (shape != null)
            {
                ExternalComponent element = shape.ModelElement as ExternalComponent;
                if (element != null)
                {
                    List<ComponentModelMetadata> versions =
                        RepositoryManager.Instance.ModelsMetadata.Metadatas.GetAllVersions(element.ModelMoniker);
                    ArrayList items = new ArrayList();
                    if (versions.Count == 0)
                    {
                        object value = context.PropertyDescriptor.GetValue(element);
                        if (value != null)
                            items.Add(value.ToString());
                    }
                    else
                    {
                        foreach (ComponentModelMetadata item in versions)
                        {
                            items.Add(item.Version.ToString());
                        }
                    }

                    return new StandardValuesCollection(items.ToArray());
                }
            }
            return base.GetStandardValues(context);
        }
    }
}