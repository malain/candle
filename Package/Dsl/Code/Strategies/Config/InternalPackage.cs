using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace DSLFactory.Candle.SystemModel.Strategies
{
    /// <summary>
    /// Correspond à un package virtuel correspondant à l'assemby courante
    /// </summary>
    public class InternalPackage
    {
        /// <summary>
        /// Creates the strategy instance.
        /// </summary>
        /// <param name="strategyTypeName">Name of the strategy type.</param>
        /// <param name="node">The node.</param>
        /// <returns></returns>
        internal virtual StrategyBase CreateStrategyInstance(string strategyTypeName, XmlNode node)
        {
            try
            {
                return CreateStrategyInstance(GetStrategyType(strategyTypeName), node);
            }
            catch( Exception ex )
            {
                string txt = strategyTypeName ?? "unknow";
                ILogger logger = ServiceLocator.Instance.GetService<ILogger>();
                if( logger!=null)
                    logger.WriteError("StrategyPackage", String.Format("Error when instanciang strategy '{0}'", txt), ex);
            }
            return null;
        }

        /// <summary>
        /// Gets the type of the strategy.
        /// </summary>
        /// <param name="strategyTypeName">Name of the strategy type.</param>
        /// <returns></returns>
        internal virtual Type GetStrategyType(string strategyTypeName)
        {
            return Type.GetType(strategyTypeName);
        }

        /// <summary>
        /// Repertoire du package
        /// </summary>
        internal virtual string PackageFolder
        {
            get { return Path.GetDirectoryName(this.GetType().Assembly.Location); }
        }

        /// <summary>
        /// Creates the strategy instance.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="node">The node.</param>
        /// <returns></returns>
        protected StrategyBase CreateStrategyInstance(Type type, XmlNode node)
        {
            if( type == null )
                throw new Exception("Unknow type");

            // Si il y a des paramètres d'initialisation
            if( node != null && node.FirstChild != null )
            {
                using( XmlReader reader = new XmlNodeReader(node) )
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(StrategyBase), new Type[] { type });
                    return (StrategyBase)serializer.Deserialize(reader);
                }
            }
            else
            {
                return (StrategyBase)Activator.CreateInstance(type);
            }
        }
    }
}