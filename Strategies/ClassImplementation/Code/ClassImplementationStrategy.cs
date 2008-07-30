using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using EnvDTE;
using System.ComponentModel;
using Microsoft.VisualStudio.Modeling;
using DSLFactory.Candle.SystemModel.CodeGeneration;

namespace DSLFactory.Candle.SystemModel.Strategies
{
/// <summary>
/// Strategy to generate the code of the service implementation
/// </summary>
    [Strategy(ClassImplementationStrategy.ClassImplementationStrategyID)]
    public class ClassImplementationStrategy : StrategyBase, IStrategyCodeGenerator
    {
        private const string ClassImplementationStrategyID = "6D906E29-D887-4e3f-B297-F1D09B41C908";

        #region Global Properties
        private string _customCodeFilePattern;
        private string _template;
        private string _customCodeTemplate;

        [EditorAttribute( typeof( DSLFactory.Candle.SystemModel.Editor.TemplateNameEditor ), typeof( System.Drawing.Design.UITypeEditor ) )]
        public string Template
        {
            [global::System.Diagnostics.DebuggerStepThrough]
            get { return _template; }
            [global::System.Diagnostics.DebuggerStepThrough]
            set { _template = value; }
        }

        public string CustomCodeFilePattern
        {
            [global::System.Diagnostics.DebuggerStepThrough]
            get { return _customCodeFilePattern; }
            [global::System.Diagnostics.DebuggerStepThrough]
            set { _customCodeFilePattern = value; }
        }

        [EditorAttribute(typeof(DSLFactory.Candle.SystemModel.Editor.TemplateNameEditor), typeof(System.Drawing.Design.UITypeEditor))]
        public string CustomCodeTemplate
        {
            [global::System.Diagnostics.DebuggerStepThrough]
            get { return _customCodeTemplate; }
            [global::System.Diagnostics.DebuggerStepThrough]
            set { _customCodeTemplate = value; }
        }

        public ClassImplementationStrategy()
        {
            Template = "ClassImplementation";
            CustomCodeTemplate = "CustomClassImplementation";
            CustomCodeFilePattern = "[code]/code/{0}";
        }
        #endregion

        #region Dependency properties

        /// <summary>
        /// Generate custom code ?
        /// </summary>
        public static readonly DependencyProperty<bool> ClassGenerateDoubleDerivate;

        static ClassImplementationStrategy()
        {
            ClassGenerateDoubleDerivate = new DependencyProperty<bool>(ClassImplementationStrategyID, "GenerateDoubleDerivate");
            ClassGenerateDoubleDerivate.Category = typeof(ClassImplementationStrategy).Name;
            ClassGenerateDoubleDerivate.DefaultValue = true;
        }

        public override PropertyDescriptorCollection GetCustomProperties(ModelElement modelElement)
        {
            PropertyDescriptorCollection collections = base.GetCustomProperties(modelElement);

            if (modelElement is ClassImplementation)
            {
                collections.Add(ClassGenerateDoubleDerivate.Register(modelElement));
            }

            return collections;
        }
        #endregion

        #region ILayerStrategy Members

        public void Execute( )
        {
            if (Context.GenerationPass == GenerationPass.ElementAdded)
                return;

            try
            {
                ClassImplementation clazz = CurrentElement as ClassImplementation;
                if (clazz == null)
                    return;

                Project prj = Context.Project;
                string fileName=null;

                // Création du code à personnaliser
                if (!String.IsNullOrEmpty(CustomCodeTemplate) && !String.IsNullOrEmpty(CustomCodeFilePattern))
                {
                    fileName = String.Format(CustomCodeFilePattern, clazz.Name);
                    fileName = CallT4Template(Context.Project,
                             CustomCodeTemplate,
                             clazz,
                             fileName);

                    if (!String.IsNullOrEmpty(fileName))
                    {
                        // Par défaut, ce fichier n'est pas regénérable
                        Mapper.Instance.SetCanGeneratePropertyValue(fileName, false);
                    }
                }

                // Création de la classe d'implémentation 
                if (ClassGenerateDoubleDerivate.GetValue(clazz) && !String.IsNullOrEmpty(Template))
                {
                    fileName = CallT4Template(Context.Project,
                             Template,
                             clazz,
                             clazz.Name + "Base");
                }
            }
            catch (Exception ex)
            {
                LogError(ex);
            }
        }

        public override string CommitChanges()
        {
            bool flag1 = String.IsNullOrEmpty(CustomCodeTemplate);
            bool flag2 = String.IsNullOrEmpty(CustomCodeFilePattern);
            if (flag1 && !flag2 || !flag1 && flag2)
            {
                return flag1 ? "You must provide a custom code template" : "You must provide a custom code file pattern";
            }

            if (!flag2)
            {
                try
                {
                    String.Format(CustomCodeFilePattern, "dummy");
                }
                catch
                {
                    return "Incorrect format for CustomCodeFilePattern";
                }
            }
            return base.CommitChanges();
        }
        #endregion
    }
}
