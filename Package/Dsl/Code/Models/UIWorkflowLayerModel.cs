using System;
using System.Collections.Generic;
using DSLFactory.Candle.SystemModel.Dependencies;

namespace DSLFactory.Candle.SystemModel
{
    /// <summary>
    /// 
    /// </summary>
    partial class UIWorkflowLayer
    {
        /// <summary>
        /// Gets the level.
        /// </summary>
        /// <value>The level.</value>
        public override short Level
        {
            get { return 100; }
        }

        /// <summary>
        /// Retourne la liste des éléments pouvant faire référence à un service d'une autre couche
        /// </summary>
        /// <returns></returns>
        protected override List<TypeWithOperations> GetImplementations()
        {
            List<TypeWithOperations> types = base.GetImplementations();
            foreach (TypeWithOperations t in Scenarios)
                types.Add(t);
            return types;
        }

        /// <summary>
        /// Gets the service references.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="layers">The layers.</param>
        /// <returns></returns>
        protected override IEnumerable<ReferenceItem> GetServiceReferences(ReferenceContext context, List<Guid> layers)
        {
            foreach (Scenario scenario in Scenarios)
            {
                foreach (ReferenceItem ri in GetServiceReferences(scenario, context, layers))
                    yield return ri;
            }

            foreach (ReferenceItem ri in base.GetServiceReferences(context, layers))
                yield return ri;
        }
    }
}