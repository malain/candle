using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Xml;
using DSLFactory.Candle.SystemModel.Strategies;
using Microsoft.VisualStudio.Modeling;
using DslModeling = Microsoft.VisualStudio.Modeling;

namespace DSLFactory.Candle.SystemModel
{
    /// <summary>
    /// 
    /// </summary>
    public partial class MyDependencyPropertySerializer : DependencyPropertySerializer
    {
        /// <summary>
        /// Public Write() method that serializes one DependencyProperty instance into XML.
        /// </summary>
        /// <param name="serializationContext">Serialization context.</param>
        /// <param name="element">DependencyProperty instance to be serialized.</param>
        /// <param name="writer">XmlWriter to write serialized data to.</param>
        /// <param name="rootElementSettings">The root element settings if the passed in element is serialized as a root element in the XML. The root element contains additional
        /// information like schema target namespace, version, etc.
        /// This should only be passed for root-level elements. Null should be passed for rest elements (and ideally call the Write() method
        /// without this parameter).</param>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods",
            Justification = "Parameter 'rootElementSettings' can be null, so no need to validate.")]
        public override void Write(SerializationContext serializationContext, ModelElement element, XmlWriter writer,
                                   RootElementSettings rootElementSettings)
        {
            #region Check Parameters

            Debug.Assert(element != null);
            if (element == null)
                throw new ArgumentNullException("element");

            #endregion

            // On ne serialise que si ce n'est pas la valeur par défaut
            DependencyProperty instanceOfDependencyProperty = element as DependencyProperty;
            IDependencyProperty dp =
                DependencyPropertyRegistry.Instance.FindDependencyProperty(instanceOfDependencyProperty.StrategyId,
                                                                           instanceOfDependencyProperty.Name);

            object defaultValue = dp != null ? dp.GetDefaultValue() : null;
            string defaultValueAsString = defaultValue != null ? defaultValue.ToString() : String.Empty;

            if (instanceOfDependencyProperty.Value != null && dp != null &&
                !Utils.StringCompareEquals(instanceOfDependencyProperty.Value.ToString(), defaultValueAsString))
            {
                base.Write(serializationContext, element, writer, rootElementSettings);
            }
        }
    }
}