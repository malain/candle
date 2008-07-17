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
    /// Une stratégie qui implémente cette interface sera toujours executée en dernier.
    /// </summary>
    public interface IStrategyConfigGenerator
    {
    }
}
