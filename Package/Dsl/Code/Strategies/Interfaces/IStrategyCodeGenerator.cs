using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.Modeling;
using System.ComponentModel;
using EnvDTE;
using Microsoft.VisualStudio.Modeling.Validation;

namespace DSLFactory.Candle.SystemModel.Strategies
{
    /// <summary>
    /// Interface décrivant une stratégie générant du code
    /// </summary>
    /// <remarks>
    /// Pour créer une nouvelle stratégie, vous devez implémenter cette interface ou plus simplement hériter
    /// de <see cref="StrategyBase"/>
    /// </remarks>
    [CLSCompliant( false )]
    public interface IStrategyCodeGenerator
    {
        /// <summary>
        /// Execution de la stratégie
        /// </summary>
        void Execute( );
    }
}
