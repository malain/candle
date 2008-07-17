using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.Modeling;

namespace DSLFactory.Candle.SystemModel.Dependencies
{
    /// <summary>
    /// 
    /// </summary>
    public struct ReferenceItem
    {
        private List<Guid> _ports;
        private ModelElement _element;
        private bool _isExternal;
        private ReferenceScope _scope;
        private ModelElement _source;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReferenceItem"/> struct.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="element">The element.</param>
        /// <param name="isExternal">if set to <c>true</c> [is external].</param>
        public ReferenceItem(ModelElement source, ModelElement element, bool isExternal)
        {
            _source = source;
            _scope = ReferenceScope.Compilation;
            _isExternal = isExternal;
            _element = element;
            _ports = null;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ReferenceItem"/> struct.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="element">The element.</param>
        /// <param name="scope">The scope.</param>
        /// <param name="isExternal">if set to <c>true</c> [is external].</param>
        public ReferenceItem(ModelElement source, ModelElement element, ReferenceScope scope, bool isExternal)
        {
            _source = source;
            _scope = scope;
            _isExternal = isExternal;
            _element = element;
            _ports = null;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ReferenceItem"/> struct.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="element">The element.</param>
        /// <param name="scope">The scope.</param>
        /// <param name="ports">The ports.</param>
        /// <param name="isExternal">if set to <c>true</c> [is external].</param>
        public ReferenceItem(ModelElement source, ModelElement element, ReferenceScope scope, List<Guid> ports,
                             bool isExternal)
        {
            _source = source;
            _scope = scope;
            _isExternal = isExternal;
            _element = element;
            _ports = ports;
        }

        /// <summary>
        /// Gets or sets the ports.
        /// </summary>
        /// <value>The ports.</value>
        public List<Guid> Ports
        {
            get { return _ports; }
            set { _ports = value; }
        }

        /// <summary>
        /// Gets or sets the element.
        /// </summary>
        /// <value>The element.</value>
        public ModelElement Element
        {
            get { return _element; }
            set { _element = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is external.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is external; otherwise, <c>false</c>.
        /// </value>
        public bool IsExternal
        {
            get { return _isExternal; }
            set { _isExternal = value; }
        }

        /// <summary>
        /// Gets or sets the scope.
        /// </summary>
        /// <value>The scope.</value>
        public ReferenceScope Scope
        {
            get { return _scope; }
            set { _scope = value; }
        }

        /// <summary>
        /// Gets or sets the source.
        /// </summary>
        /// <value>The source.</value>
        public ModelElement Source
        {
            get { return _source; }
            set { _source = value; }
        }
    }
}