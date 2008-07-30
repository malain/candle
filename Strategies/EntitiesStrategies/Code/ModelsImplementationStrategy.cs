using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using EnvDTE;
using DSLFactory.Candle.SystemModel.CodeGeneration;
using System.ComponentModel;


namespace DSLFactory.Candle.SystemModel.Strategies
{
    /// <summary>
    /// Strategy to generate the code for the element of the models layer : Entity and Enumeration
    /// </summary>
    [CLSCompliant(false)]
    [Strategy("811A5492-07FB-42da-A6A6-ACCA4DFED1A9")]
    public class ModelsImplementationStrategy : StrategyBase, IStrategyCodeGenerator 
    {
        private string _entityTemplate;
        private string _enumTemplate;

        public ModelsImplementationStrategy()
        {
            _entityTemplate = "modelEntity";
            _enumTemplate = "EnumEntity";
        }

        [EditorAttribute( typeof( DSLFactory.Candle.SystemModel.Editor.TemplateNameEditor ), typeof( System.Drawing.Design.UITypeEditor ) )]
        public string EnumTemplate
        {
            [global::System.Diagnostics.DebuggerStepThrough]
            get { return _enumTemplate; }
            [global::System.Diagnostics.DebuggerStepThrough]
            set { _enumTemplate = value; }
        }
	
        [EditorAttribute( typeof( DSLFactory.Candle.SystemModel.Editor.TemplateNameEditor ), typeof( System.Drawing.Design.UITypeEditor ) )]
        public string EntityTemplate
        {
            [global::System.Diagnostics.DebuggerStepThrough]
            get { return _entityTemplate; }
            [global::System.Diagnostics.DebuggerStepThrough]
            set { _entityTemplate = value; }
        }

        #region ModelsImplementationStrategy Members
        public void Execute()
        {
            if (Context.GenerationPass == GenerationPass.ElementAdded)
                return;

            try
            {
                DataType elem = CurrentElement as DataType;
                if( elem == null )
                    return;

                // Ajout d'une référence au projet vers System.Xml
                if( Context.GenerationPass == GenerationPass.MetaModelUpdate )
                {
                    elem.DataLayer.AddReference("System.Xml", ArtifactType.DotNetFramework);
                }

                if (Context.GenerationPass == GenerationPass.CodeGeneration)
                    CallT4Template(Context.Project, elem is Enumeration ? EnumTemplate : EntityTemplate, elem);
            }
            catch (Exception ex)
            {
                LogError(ex);
            }
        }

        #endregion
    }
}
