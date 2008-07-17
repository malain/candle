using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using Microsoft.VisualStudio.TextTemplating;

namespace DSLFactory.Candle.SystemModel.CodeGeneration
{
    /// <summary>
    /// 
    /// </summary>
    public class ContextProcessor : DirectiveProcessor
    {
        private CandleTemplateHost currentHost;
        private CodeDomProvider languageProvider;
        private StringWriter writer;

        /// <summary>
        /// Starts the processing run.
        /// </summary>
        /// <param name="languageProvider">The language provider.</param>
        /// <param name="templateContents">The template contents.</param>
        /// <param name="errors">The errors.</param>
        public override void StartProcessingRun(CodeDomProvider languageProvider, string templateContents,
                                                CompilerErrorCollection errors)
        {
            base.StartProcessingRun(languageProvider, templateContents, errors);
            if (languageProvider == null)
            {
                throw new ArgumentNullException("languageProvider");
            }
            writer = new StringWriter(CultureInfo.CurrentCulture);
            this.languageProvider = languageProvider;
        }

        /// <summary>
        /// Initializes the specified host.
        /// </summary>
        /// <param name="host">The host.</param>
        public override void Initialize(ITextTemplatingEngineHost host)
        {
            base.Initialize(host);
            currentHost = (CandleTemplateHost) host;
        }

        /// <summary>
        /// Finishes the processing run.
        /// </summary>
        public override void FinishProcessingRun()
        {
        }

        /// <summary>
        /// Gets the class code for processing run.
        /// </summary>
        /// <returns></returns>
        public override string GetClassCodeForProcessingRun()
        {
            return writer.ToString();
        }

        /// <summary>
        /// Gets the imports for processing run.
        /// </summary>
        /// <returns></returns>
        public override string[] GetImportsForProcessingRun()
        {
            return null;
        }

        /// <summary>
        /// Gets the post initialization code for processing run.
        /// </summary>
        /// <returns></returns>
        public override string GetPostInitializationCodeForProcessingRun()
        {
            return null;
        }

        /// <summary>
        /// Gets the pre initialization code for processing run.
        /// </summary>
        /// <returns></returns>
        public override string GetPreInitializationCodeForProcessingRun()
        {
            return null;
        }

        /// <summary>
        /// Gets the references for processing run.
        /// </summary>
        /// <returns></returns>
        public override string[] GetReferencesForProcessingRun()
        {
            return new string[] {base.GetType().Module.Name};
        }

        /// <summary>
        /// Determines whether [is directive supported] [the specified directive name].
        /// </summary>
        /// <param name="directiveName">Name of the directive.</param>
        /// <returns>
        /// 	<c>true</c> if [is directive supported] [the specified directive name]; otherwise, <c>false</c>.
        /// </returns>
        public override bool IsDirectiveSupported(string directiveName)
        {
            if (directiveName == null)
            {
                throw new ArgumentNullException("directiveName");
            }
            if (Utils.StringCompareEquals(directiveName, "context"))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Processes the directive.
        /// </summary>
        /// <param name="directiveName">Name of the directive.</param>
        /// <param name="arguments">The arguments.</param>
        public override void ProcessDirective(string directiveName, IDictionary<string, string> arguments)
        {
            if (string.Compare(directiveName, "context", true) == 0)
            {
                //CodeMemberProperty property = new CodeMemberProperty();
                //property.Name = "Context";
                //property.Type = new CodeTypeReference( typeof(GenerationContext) );
                //property.Attributes = MemberAttributes.Public | MemberAttributes.Final | MemberAttributes.Override;
                //property.GetStatements.Add( new CodeMethodReturnStatement(
                //    new CodeCastExpression( property.Type,
                //        new CodePropertyReferenceExpression(
                //        new CodeTypeReferenceExpression( typeof( Generator ) ), "Context" )
                //) ) );
                //CodeGeneratorOptions options = new CodeGeneratorOptions();
                //options.BracingStyle = "C";
                //this.languageProvider.GenerateCodeFromMember( property, this.writer, options );

                writer.WriteLine(
                    @"public override DSLFactory.Candle.SystemModel.GenerationContext Context
        {
            get
            {
                return ((DSLFactory.Candle.SystemModel.GenerationContext)(DSLFactory.Candle.SystemModel.CodeGeneration.Generator.Context));
            }
        }");

                if (currentHost.CurrentElement != null)
                    writer.WriteLine(
                        String.Format(
                            "private {0} CurrentElement {{ get {{ return ({0})DSLFactory.Candle.SystemModel.CodeGeneration.CandleTemplateHost.Instance.CurrentElement;}} }}",
                            currentHost.CurrentElement.GetType().FullName));
            }
        }
    }
}