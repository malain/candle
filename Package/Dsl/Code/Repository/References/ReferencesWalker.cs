using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.Modeling;

namespace DSLFactory.Candle.SystemModel.Dependencies
{
    /// <summary>
    /// Permet de parcourir l'arbre des références
    /// </summary>
    public class ReferenceWalker
    {
        private readonly ConfigurationMode _configuration;
        private readonly ReferenceScope _scope;
        private IReferenceVisitor _visitor;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReferenceWalker"/> class.
        /// </summary>
        /// <param name="scope">The scope.</param>
        /// <param name="configuration">The configuration.</param>
        public ReferenceWalker(ReferenceScope scope, ConfigurationMode configuration)
        {
            if (configuration == null)
                throw new ArgumentNullException("configuration");

            _scope = scope;
            _configuration = configuration;
        }

        /// <summary>
        /// Traverses the specified visitor.
        /// </summary>
        /// <param name="visitor">The visitor.</param>
        /// <param name="element">The element.</param>
        public void Traverse(IReferenceVisitor visitor, ModelElement element)
        {
            if (visitor == null)
                throw new ArgumentNullException("visitor");
            if (element == null)
                return;

            _visitor = visitor;

            // Il faut toujours commencer par un modèle pour initialiser le contexte
            CandleModel model = null;
            if (!(element is CandleModel))
            {
                model = CandleModel.GetInstance(element.Store);
                visitor.Accept(new ReferenceItem(null, model, false)); // Initialisation du contexte
            }

            // Parcours de l'élément choisi
            Traverse(new ReferenceItem(null, element, false));

            if (model != null)
                visitor.ExitElement(new ReferenceItem(null, model, false));
        }

        /// <summary>
        /// Traverses the specified ref item.
        /// </summary>
        /// <param name="refItem">The ref item.</param>
        private void Traverse(ReferenceItem refItem)
        {
            if (_visitor.Accept(refItem))
            {
                try
                {
                    IHasReferences container = refItem.Element as IHasReferences;
                    if (container == null)
                        return;

                    List<Guid> ports = refItem.Ports;
                    ReferenceContext context = new ReferenceContext(_configuration, _scope, ports, refItem.IsExternal);
                    foreach (ReferenceItem reference in container.GetReferences(context))
                    {
                        Traverse(reference);
                    }
                }
                finally
                {
                    _visitor.ExitElement(refItem);
                }
            }
        }
    }
}