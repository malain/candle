using System.Collections.Generic;

namespace DSLFactory.Candle.SystemModel.Strategies
{
    /// <summary>
    /// Enregistrement de toutes les propriétés dépendantes
    /// </summary>
    internal class DependencyPropertyRegistry
    {
        private static readonly Dictionary<string, IDependencyProperty> s_dependencies = new Dictionary<string, IDependencyProperty>();
        private static readonly DependencyPropertyRegistry s_instance = new DependencyPropertyRegistry();

        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <value>The instance.</value>
        public static DependencyPropertyRegistry Instance
        {
            get { return s_instance; }
        }

        /// <summary>
        /// Registers the specified strategy id.
        /// </summary>
        /// <param name="strategyId">The strategy id.</param>
        /// <param name="property">The property.</param>
        public void Register(string strategyId, IDependencyProperty property)
        {
            s_dependencies.Add(strategyId + property.Name, property);
        }

        /// <summary>
        /// Finds the dependency property.
        /// </summary>
        /// <param name="strategyId">The strategy id.</param>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public IDependencyProperty FindDependencyProperty(string strategyId, string name)
        {
            IDependencyProperty p;
            if (s_dependencies.TryGetValue(strategyId + name, out p))
                return p;
            return null;
        }
    }
}