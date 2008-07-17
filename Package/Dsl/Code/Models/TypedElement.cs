using System;
using DSLFactory.Candle.SystemModel.Strategies;

namespace DSLFactory.Candle.SystemModel
{
    /// <summary>
    /// 
    /// </summary>
    partial class TypeMember : ITypeMember
    {
        /// <summary>
        /// Gets the strategies owner.
        /// </summary>
        /// <value>The strategies owner.</value>
        public override CandleElement StrategiesOwner
        {
            get { return Owner != null ? Owner.StrategiesOwner : null; }
        }

        #region ITypeMember Members

        /// <summary>
        /// Gets the full name of the type.
        /// </summary>
        /// <value>The full name of the type.</value>
        public virtual string FullTypeName
        {
            get
            {
                string typeName = Type;
                CandleModel model = CandleModel.GetInstance(Store);
                if (model.SoftwareComponent != null && model.SoftwareComponent.IsDataLayerExists)
                {
                    DataType type = model.DataLayer.FindType(Type);
                    if (type != null)
                        typeName = type.FullTypeName;
                }
                return NormalizeTypeName(typeName, IsCollection);
            }
        }

        #endregion

        /// <summary>
        /// Gets the display name value.
        /// </summary>
        /// <returns></returns>
        internal string GetDisplayNameValue()
        {
            return String.Concat(Name, " : ", FullTypeName);
        }

        /// <summary>
        /// Normalizes the name of the type.
        /// </summary>
        /// <param name="fullName">The full name.</param>
        /// <param name="isCollection">if set to <c>true</c> [is collection].</param>
        /// <returns></returns>
        public string NormalizeTypeName(string fullName, bool isCollection)
        {
            if (isCollection)
            {
                if (StrategyManager.GetInstance(Store).NamingStrategy.CollectionAsArray)
                    return String.Concat(fullName, "[]");
                return String.Concat("System.Collections.Generic.List<", fullName, ">");
            }
            return fullName;
        }
    }
}