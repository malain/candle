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
    /// <summary>
    /// Strategy permettant de créer un label dans TFS lors de la publication du modèle sur le serveur
    /// </summary>
    [Strategy(VersionningStrategy.StrategyID)]
    public class VersionningStrategy : StrategyBase, IStrategyPublishEvents
    {
        private const string StrategyID = "11002235-8889-4efe-8EA6-FB49F6B17210"; // or an unique name

        private string _outputFileName = "AssemblyInfo";
        private string _template = "AssemblyInfo";

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

        /// <summary>
        /// 
        /// </summary>
        public VersionningStrategy()
        {
            DefaultGeneratedFilePathPattern = @"[code]\Properties\{0}";
        }

        #region IStrategyPublishEvents Members

        public void OnAfterLocalPublication(CandleModel model, string modelFileName)
        {
        }

        /// <summary>
        /// A la fin de la publication, on met à jour les fichiers des infos de l'assemblie avec le
        /// n° de version du modèle.
        /// </summary>
        /// <param name="model"></param>
        /// <param name="modelFileName"></param>
        void IStrategyPublishEvents.OnPublicationEnded(CandleModel model, string modelFileName)
        {
            if (!String.IsNullOrEmpty(T4Template))
            {
                if (model.SoftwareComponent != null)
                {
                    // Création du fichier AssemblyInfo
                    foreach (AbstractLayer layer in model.SoftwareComponent.Layers)
                    {
                        if (layer is SoftwareLayer)
                        {
                            IShellHelper shell = ServiceLocator.Instance.GetService<IShellHelper>();
                            if (shell != null)
                            {
                                // Recherche du projet associé
                                Project prj = shell.FindProjectByName(((SoftwareLayer)layer).VSProjectName);
                                if (prj != null)
                                {
                                    Context.Project = prj;
                                    CallT4Template(prj, T4Template, layer, _outputFileName);
                                }
                            }
                        }
                    }
                }
            }
        }

        void IStrategyPublishEvents.OnPublicationError(CandleModel model, string modelFileName)
        {
        }

        public void OnAfterServerPublication(CandleModel model, string modelFileName)
        {
        }

        public void OnBeforeLocalPublication(CandleModel model, string modelFileName)
        {
        }

        public void OnBeforeServerPublication(CandleModel model, string modelFileName)
        {
        }

        #endregion
    }
}
