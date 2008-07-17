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
    /// Interface d�crivant une strat�gie g�n�rant du code
    /// </summary>
    /// <remarks>
    /// Pour cr�er une nouvelle strat�gie, vous devez impl�menter cette interface ou plus simplement h�riter
    /// de <see cref="StrategyBase"/>
    /// </remarks>
    [CLSCompliant( false )]
    public interface IStrategyCodeGenerator
    {
        /// <summary>
        /// Execution de la strat�gie
        /// </summary>
        void Execute( );
    }
}
