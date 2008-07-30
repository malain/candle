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
    /// Generic strategy to use with a T4 template
    /// </summary>
    [CLSCompliant(false)]
    [Strategy("70661F21-A0A7-47b6-AE0E-39BBEB1AA934")]
    public class WebServiceStrategy : StrategyBase, IStrategyCodeGenerator, IStrategyProvidesProjectTemplates
    {
        private string  _template;

        public WebServiceStrategy()
        {
        }

        //[Description( "Name of the template file without extension" )]
        //[EditorAttribute( typeof( DSLFactory.Candle.SystemModel.Editor.TemplateNameEditor ), typeof( System.Drawing.Design.UITypeEditor ) )]
        //public string T4Template
        //{
        //    get { return _template; }
        //    set { _template = value; }
        //}

        #region Members
        public void Execute()
        {
            try
            {
                PresentationLayer layer = CurrentElement as PresentationLayer;
                if (Context.GenerationPass != GenerationPass.CodeGeneration || layer == null)
                    return;

                // Création d'un service web pour tous les ports du layer.
                foreach (ClassImplementation port in layer.Classes)
                {
                    string asmxFile = CallT4Template(Context.Project,
                            "asmx.T4",
                            port,
                            String.Format("~{0}.asmx", port.Name));

                    string codeFile = CallT4Template(Context.Project,
                            "webservice.T4",
                            port);                            
                }                
            }
            catch( Exception ex )
            {
                LogError( ex );
            }
        }

        #endregion

        #region IStrategyProvidesProjectTemplates Members

        public string GetProjectTemplate(SoftwareLayer layer)
        {
            if( layer is Layer && ((Layer)layer).HostingContext == HostingContext.Web )
                return @"web\csharp\EmptyWeb";
            return null;
        }

        public string GetAssemblyExtension(SoftwareLayer layer)
        {
            if (layer is Layer && ((Layer)layer).HostingContext == HostingContext.Web)
                return ".dll";
            return null;
        }

        #endregion
    }
}
