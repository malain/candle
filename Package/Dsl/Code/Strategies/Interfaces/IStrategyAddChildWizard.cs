using System;
using System.Collections.Generic;
using System.Text;

namespace DSLFactory.Candle.SystemModel.Strategies
{
    /// <summary>
    /// Interface que doivent implémenter les wizards de stratégies
    /// </summary>
    [CLSCompliant(true)]
    public interface IStrategyWizard
    {
        /// <summary>
        /// Runs the wizard.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="DSLFactory.Candle.SystemModel.Strategies.StrategyElementElementAddedEventArgs"/> instance containing the event data.</param>
        void RunWizard( Microsoft.VisualStudio.Modeling.ModelElement sender, StrategyElementElementAddedEventArgs e );
    }
}
