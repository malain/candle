using System;
using System.Reflection;

namespace DSLFactory.Candle.SystemModel.Commands.Reverse
{
    /// <summary>
    /// 
    /// </summary>
    internal class CustomAttributeReflector
    {
        private readonly MemberInfo _member;
        private CustomAttributeData _attribute;

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomAttributeReflector"/> class.
        /// </summary>
        /// <param name="member">The member.</param>
        public CustomAttributeReflector(MemberInfo member)
        {
            _member = member;
        }

        /// <summary>
        /// Moves to.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public bool MoveTo(Type type)
        {
            _attribute = null;
            foreach( CustomAttributeData att in CustomAttributeData.GetCustomAttributes(_member) )
            {
                if( att.Constructor.DeclaringType.FullName == type.FullName )
                {
                    _attribute = att;
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Gets the property value.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public T GetPropertyValue<T>(string name)
        {
            if( _attribute != null )
            {
                foreach( CustomAttributeNamedArgument arg in _attribute.NamedArguments )
                {
                    if( arg.MemberInfo.Name == name )
                    {
                        return (T)arg.TypedValue.Value;
                    }
                }
            }
            return default(T);
        }
    }
}
