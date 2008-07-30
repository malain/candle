using System;
using System.Collections.Generic;
using System.Text;
using EnvDTE;
using System.IO;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Validation;
using System.ComponentModel;

namespace DSLFactory.Candle.SystemModel.Strategies
{
    [Strategy(ServiceFactoryStrategy.StrategyID)]
    public class ServiceFactoryStrategy : StrategyBase, IStrategyCodeGenerator
    {
        private const string StrategyID = "18e3e650-1eb4-4df1-8dbe-32daf5283c43"; // or an unique name

        #region IStrategyCodeGenerator Members
        private string _outputFileName = "ServiceFactory";
        private string _template = "StaticServiceFactory";

        [Description("Name of the output file")]
        public string OutputFileName
        {
            [global::System.Diagnostics.DebuggerStepThrough]
            get { return _outputFileName; }
            [global::System.Diagnostics.DebuggerStepThrough]
            set { _outputFileName = value; }
        }

        [Description("Name of the template file without extension")]
        [EditorAttribute(typeof(DSLFactory.Candle.SystemModel.Editor.TemplateNameEditor), typeof(System.Drawing.Design.UITypeEditor))]
        public string T4Template
        {
            [global::System.Diagnostics.DebuggerStepThrough]
            get { return _template; }
            [global::System.Diagnostics.DebuggerStepThrough]
            set { _template = value; }
        }

        public void Execute()
        {
            if (!(CurrentElement is Layer))
                return;

            // Vérification si la génération de la factory est pertinente (cad qu'il existe des références à partir de cette couche)	
            int nb = 0;
            Layer layer = CurrentElement as Layer;
            foreach (ClassImplementation clazz in layer.Classes)
            {
                foreach (ClassUsesOperations service in ClassUsesOperations.GetLinksToServicesUsed(clazz))
                {
                    if (!Context.Mode.CheckConfigurationMode(service.ConfigurationMode))
                        continue;

                    if (service.TargetService is ExternalServiceContract )
                        nb++;
                    else
                    {
                        foreach (Implementation impl in Implementation.GetLinksToImplementations((ServiceContract)service.TargetService))
                        {
                            if (Context.Mode.CheckConfigurationMode(impl.ConfigurationMode))
                                nb++;
                        }
                    }
                }
            }
    
            if (nb > 0)
                CallT4Template(Context.Project, T4Template, (CandleElement)CurrentElement, OutputFileName);
        }

        #endregion
    }
}
