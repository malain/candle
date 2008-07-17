using System;
using System.Collections.Generic;
using System.Text;

namespace DSLFactory.Candle.SystemModel.Strategies
{
    /// <summary>
    /// Interface implémentée par les stratégies contenues dans un package
    /// </summary>
    public interface IPackagedStrategy 
    {
        /// <summary>
        /// Nom du package.
        /// </summary>
        string PackageName { get;}
    }
}
