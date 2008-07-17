using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing.Design;
using DSLFactory.Candle.SystemModel.Editor;

namespace DSLFactory.Candle.SystemModel.Strategies
{
    /// <summary>
    /// Generic strategy to use with a T4 template
    /// </summary>
    [CLSCompliant(false)]
    [Strategy]
    public class GenericStrategy : StrategyBase, IStrategyCodeGenerator
    {
        private string _outputFileName;
        private List<string> _targetTypeNames;
        private string _template;
        //private SoftwareLayer   _layer;

        /// <summary>
        /// Initializes a new instance of the <see cref="GenericStrategy"/> class.
        /// </summary>
        public GenericStrategy()
        {
            _targetTypeNames = new List<string>();
        }

        /// <summary>
        /// Gets or sets the t4 template.
        /// </summary>
        /// <value>The t4 template.</value>
        [Description("Name of the template file without extension")]
        [EditorAttribute(typeof (TemplateNameEditor), typeof (UITypeEditor))]
        public string T4Template
        {
            [DebuggerStepThrough]
            get { return _template; }
            [DebuggerStepThrough]
            set { _template = value; }
        }

        /// <summary>
        /// Gets or sets the name of the output file.
        /// </summary>
        /// <value>The name of the output file.</value>
        [Description("Name of the output file")]
        public string OutputFileName
        {
            [DebuggerStepThrough]
            get { return _outputFileName; }
            [DebuggerStepThrough]
            set { _outputFileName = value; }
        }

        /// <summary>
        /// Target element the strategy operates on
        /// </summary>
        /// <value>The target type names.</value>
        [Description("Select the models the strategy operates on")]
        [EditorAttribute(typeof (TargetModelEditor), typeof (UITypeEditor))]
        public List<String> TargetTypeNames
        {
            [DebuggerStepThrough]
            get { return _targetTypeNames; }
            [DebuggerStepThrough]
            set { _targetTypeNames = value; }
        }

        #region Members

        /// <summary>
        /// Execution de la stratégie
        /// </summary>
        public void Execute()
        {
            // parcours des types sur lequel doit s'exécuter les stratégies
            if (_targetTypeNames != null)
            {
                foreach (string typeName in _targetTypeNames)
                {
                    if (Type.GetType(typeName).IsInstanceOfType(CurrentElement))
                    {
                        // OK on peut executer
                        try
                        {
                            CallT4Template(Context.Project, T4Template,
                                           (CandleElement) CurrentElement, OutputFileName ?? String.Empty);
                            return;
                        }
                        catch (Exception ex)
                        {
                            LogError(ex);
                        }
                    }
                }
            }
        }

        //public override string CommitChanges()
        //{
        //    // TODO check _targetTypeNames
        //    return base.CommitChanges();
        //}

        #endregion
    }
}