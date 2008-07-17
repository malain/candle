using System;
using System.Collections.Generic;

namespace DSLFactory.Candle.SystemModel.Strategies
{
    /// <summary>
    /// Stockage des propriétes pouvant être passées à un template T4 et accessible par le contexte
    /// de génération
    /// </summary>
    [CLSCompliant(true)]
    public class TemplateProperties
    {
        private readonly Dictionary<string, object> _properties = new Dictionary<string, object>();

        /// <summary>
        /// Gets the <see cref="System.Object"/> with the specified name.
        /// </summary>
        /// <value></value>
        internal object this[string name]
        {
            get
            {
                if (_properties.ContainsKey(name))
                    return _properties[name];
                return null;
            }
        }

        /// <summary>
        /// Adds the specified name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="value">The value.</param>
        public void Add(string name, object value)
        {
            _properties[name] = value;
        }

        /// <summary>
        /// Gets the value.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public T GetValue<T>(string name)
        {
            return (T) this[name];
        }
    }
}