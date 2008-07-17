using System;
using System.Collections.Generic;

namespace DSLFactory.Candle.SystemModel.Dependencies
{
    /// <summary>
    /// 
    /// </summary>
    [CLSCompliant(true)]
    public struct ReferenceContext
    {
        /// <summary>
        /// 
        /// </summary>
        public bool IsExternal;
        /// <summary>
        /// 
        /// </summary>
        public ConfigurationMode Mode;
        /// <summary>
        /// 
        /// </summary>
        public List<Guid> Ports;
        /// <summary>
        /// 
        /// </summary>
        public ReferenceScope Scope;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReferenceContext"/> struct.
        /// </summary>
        /// <param name="mode">The mode.</param>
        /// <param name="scope">The scope.</param>
        /// <param name="ports">The ports.</param>
        /// <param name="isExternal">if set to <c>true</c> [is external].</param>
        public ReferenceContext(ConfigurationMode mode, ReferenceScope scope, List<Guid> ports, bool isExternal)
        {
            Mode = mode;
            Scope = scope;
            Ports = ports;
            IsExternal = isExternal;
        }

        /// <summary>
        /// Checks the scope.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public bool CheckScope(ReferenceScope value)
        {
            if (Scope == ReferenceScope.All)
                return true;
            return ((value & Scope) == Scope);
        }

        /// <summary>
        /// Checks the port.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        public bool CheckPort(Guid id)
        {
            return Ports == null || Ports.Contains(id);
        }
    }
}