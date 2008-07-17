using System;

namespace DSLFactory.Candle.SystemModel
{
    /// <summary>
    /// Pour les composants externes, il faut tenir compte que le namespace a pu être forcé
    /// </summary>
    [CLSCompliant(true)]
    public class ExternalNamespaceProvider : StandardNamespaceResolver
    {
        private readonly ExternalComponent _externalComponent;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExternalNamespaceProvider"/> class.
        /// </summary>
        /// <param name="externalComponent">The external component.</param>
        public ExternalNamespaceProvider(ExternalComponent externalComponent)
        {
            _externalComponent = externalComponent;
        }

        /// <summary>
        /// Resolves the specified @namespace.
        /// </summary>
        /// <param name="namespace">The @namespace.</param>
        /// <returns></returns>
        public override string Resolve(string @namespace)
        {
            if (String.IsNullOrEmpty(_externalComponent.Namespace))
                return base.Resolve(@namespace); // Normal

            return _externalComponent.Namespace; // forcé
        }
    }
}