using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using DSLFactory.Candle.SystemModel.Repository;
using DSLFactory.Candle.SystemModel.Strategies;
using Microsoft.VisualStudio.TextTemplating;
using Microsoft.VisualStudio.TextTemplating.VSHost;
using Microsoft.Win32;

namespace DSLFactory.Candle.SystemModel.CodeGeneration
{
    //the text template transformation engine is responsible for running 
    //the transformation process
    //the host is responsible for all input and output, locating files, 
    //and and anything else related to the external environment
    //-------------------------------------------------------------------------
    /// <summary>
    /// 
    /// </summary>
    public class CandleTemplateHost : ITextTemplatingEngineHost
    {
        //the path and file name of the text template that is being processed
        //---------------------------------------------------------------------
        private static CandleTemplateHost s_instance;
        private static TemplateProperties s_templateProperties;
        private readonly ICustomizableElement _currentElement;
        private readonly IList<string> standardAssemblyReferences;
        private CompilerErrorCollection _errorsValue;
        private string _fileExtensionValue = ".cs";
        public string _inputFile;

        /// <summary>
        /// Initializes a new instance of the <see cref="CandleTemplateHost"/> class.
        /// </summary>
        /// <param name="currentElement">The current element.</param>
        /// <param name="properties">The properties.</param>
        public CandleTemplateHost(ICustomizableElement currentElement, TemplateProperties properties)
        {
            s_templateProperties = properties;
            if (properties == null)
                s_templateProperties = new TemplateProperties();
            _currentElement = currentElement;
            s_instance = this;

            standardAssemblyReferences = new List<string>();
            standardAssemblyReferences.Add("System.dll");
            standardAssemblyReferences.Add("Microsoft.VisualStudio.Modeling.SDK, Version=" +
                                           ModelConstants.VisualStudioVersion);
            standardAssemblyReferences.Add("Microsoft.VisualStudio.TextTemplating.VSHost, Version=" +
                                           ModelConstants.VisualStudioVersion);

            //if (type != null && type.Assembly.FullName != this.GetType().Assembly.FullName)
            //    standardAssemblyReferences.Add(type.Assembly.FullName);
        }

        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <value>The instance.</value>
        public static CandleTemplateHost Instance
        {
            get { return s_instance; }
        }

        /// <summary>
        /// Gets the current element.
        /// </summary>
        /// <value>The current element.</value>
        public ICustomizableElement CurrentElement
        {
            get { return _currentElement; }
        }

        /// <summary>
        /// Gets the properties.
        /// </summary>
        /// <value>The properties.</value>
        public TemplateProperties Properties
        {
            get { return s_templateProperties; }
        }

        /// <summary>
        /// Gets the file extension.
        /// </summary>
        /// <value>The file extension.</value>
        public string FileExtension
        {
            get { return _fileExtensionValue; }
        }

        /// <summary>
        /// Gets the errors.
        /// </summary>
        /// <value>The errors.</value>
        public CompilerErrorCollection Errors
        {
            get { return _errorsValue; }
        }

        #region ITextTemplatingEngineHost Members

        /// <summary>
        /// Gets the host option.
        /// </summary>
        /// <param name="optionName">Name of the option.</param>
        /// <returns></returns>
        public object GetHostOption(string optionName)
        {
            if (StringComparer.OrdinalIgnoreCase.Compare(optionName, "CacheAssemblies") == 0)
            {
                return true;
            }
            return null;
        }


        //the host can provide standard assembly references
        //the engine will use these references when compiling and
        //executing the generated transformation class
        //--------------------------------------------------------------
        /// <summary>
        /// Gets the standard assembly references.
        /// </summary>
        /// <value>The standard assembly references.</value>
        public IList<string> StandardAssemblyReferences
        {
            get { return standardAssemblyReferences; }
        }


        //the host can provide standard imports or using statements
        //the engine will add these statements to the generated 
        //transformation class
        //--------------------------------------------------------------
        /// <summary>
        /// Gets the standard imports.
        /// </summary>
        /// <value>The standard imports.</value>
        public IList<string> StandardImports
        {
            get
            {
                return
                    new string[]
                        {
                            "System", "Microsoft.VisualStudio.TextTemplating.VSHost", "DSLFactory.Candle.SystemModel",
                            "DSLFactory.Candle.SystemModel.Strategies"
                        };
            }
        }


        /// <summary>
        /// Loads the include text.
        /// </summary>
        /// <param name="requestFileName">Name of the request file.</param>
        /// <param name="content">The content.</param>
        /// <param name="location">The location.</param>
        /// <returns></returns>
        public bool LoadIncludeText(string requestFileName, out string content, out string location)
        {
            //if the argument is the fully qualified path of an existing file,
            //then we are done
            //----------------------------------------------------------------
            RepositoryFile rfi = new RepositoryFile(requestFileName);
            content = rfi.ReadContent();
            location = String.Empty;
            if (content != String.Empty)
            {
                location = rfi.LocalPhysicalPath;
                return true;
            }

            return false;
        }


        //the engine calls this method to resolve assembly references used in
        //the generated transformation class projectOrSolution, and for the optional 
        //assembly directive if the user has specified it in the text template
        //this method can be called 0, 1, or more times
        //---------------------------------------------------------------------
        /// <summary>
        /// Resolves the assembly reference.
        /// </summary>
        /// <param name="assemblyReference">The assembly reference.</param>
        /// <returns></returns>
        public string ResolveAssemblyReference(string assemblyReference)
        {
            if (!Path.IsPathRooted(assemblyReference))
            {
                if (assemblyReference.EndsWith(".dll"))
                    assemblyReference = assemblyReference.Substring(0, assemblyReference.Length - 4);

                string filePath = FileLocationHelper.DslToolsInstallDir;
                if (!String.IsNullOrEmpty(filePath))
                {
                    filePath = Path.Combine(filePath, assemblyReference);
                    if (File.Exists(filePath))
                    {
                        return filePath;
                    }
                }

                filePath = FileLocationHelper.VSInstallDir;
                if (!String.IsNullOrEmpty(filePath))
                {
                    filePath = Path.Combine(filePath, assemblyReference);
                    if (File.Exists(filePath))
                    {
                        return filePath;
                    }
                }

                Assembly asm = typeof (ModelingTextTransformation).Assembly;
                if (asm != null)
                {
                    Type t = asm.GetType("Microsoft.VisualStudio.TextTemplating.GlobalAssemblyCacheHelper", false);
                    if (t != null)
                    {
                        MethodInfo mi = t.GetMethod("GetLocation", BindingFlags.NonPublic | BindingFlags.Static);
                        filePath = mi.Invoke(null, new string[] {assemblyReference}) as string;
                        if (!string.IsNullOrEmpty(filePath))
                        {
                            return filePath;
                        }
                    }
                }

                if (!Utils.StringStartsWith(assemblyReference, "Microsoft") &&
                    !Utils.StringStartsWith(assemblyReference, "System"))
                {
                    filePath = RepositoryManager.ResolveAssemblyLocation(assemblyReference);
                    // COMMENT-BEGIN : La dll doit être synchronisée avec le package
                    //if (filePath != null)//&& File.Exists( filePath ) )
                    //{
                    //    RepositoryFile rfi = new RepositoryFile(filePath);
                    //    if (rfi.Exists())
                    //        return rfi.LocalPhysicalPath;
                    //}
                    if (filePath != null && File.Exists(filePath))
                    {
                        return filePath;
                    }
                }
            }

            return assemblyReference;
        }

        //the engine calls this method based on the directives the user has 
        //specified it in the text template
        //this method can be called 0, 1, or more times
        //---------------------------------------------------------------------
        /// <summary>
        /// Resolves the directive processor.
        /// </summary>
        /// <param name="processorName">Name of the processor.</param>
        /// <returns></returns>
        public Type ResolveDirectiveProcessor(string processorName)
        {
            if (processorName == "ContextProcessor")
                return typeof (ContextProcessor);

            if (processorName == "PropertyProcessor")
                return typeof (PropertyProcessor);

            //if the directive processor can not be found, throw an error
            throw new Exception("Directive Processor not found");
        }


        //a directive processor can call this method if a file name does not 
        //have a path
        //the host can attempt to provide path information by searching 
        //specific paths for the file and returning the file and path if found
        //this method can be called 0, 1, or more times
        //---------------------------------------------------------------------
        /// <summary>
        /// Resolves the path.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <returns></returns>
        public string ResolvePath(string fileName)
        {
            if (fileName == null)
            {
                throw new ArgumentNullException("the file name cannot be null");
            }

            //if the argument is the fully qualified path of an existing file,
            //then we are done
            //----------------------------------------------------------------
            if (File.Exists(fileName))
            {
                return fileName;
            }

            //maybe the file is in the same folder as the text template that 
            //called the directive
            //----------------------------------------------------------------
            if (_inputFile != null)
            {
                string candidate = Path.Combine(Path.GetDirectoryName(_inputFile), fileName);
                if (File.Exists(candidate))
                {
                    return candidate;
                }
            }

            // On regarde si ce n'est pas une valeur de paramètre
            return ResolveParameterValue("", "", fileName);
        }

        //if a call to a directive in a text template does not provide a value
        //for a required parameter, the directive processor can try to get it
        //from the host by calling this method
        //this method can be called 0, 1, or more times
        //---------------------------------------------------------------------
        /// <summary>
        /// Resolves the parameter value.
        /// </summary>
        /// <param name="directiveId">The directive id.</param>
        /// <param name="processorName">Name of the processor.</param>
        /// <param name="parameterName">Name of the parameter.</param>
        /// <returns></returns>
        public string ResolveParameterValue(string directiveId, string processorName, string parameterName)
        {
            return String.Empty;
        }


        //the engine calls this method to change the extension of the 
        //generated text output file based on the optional output directive 
        //if the user specifies it in the text template
        //---------------------------------------------------------------------
        /// <summary>
        /// Sets the file extension.
        /// </summary>
        /// <param name="extension">The extension.</param>
        public void SetFileExtension(string extension)
        {
            //the parameter extension has a '.' in front of it already
            //--------------------------------------------------------
            _fileExtensionValue = extension;
        }


        //the engine calls this method when it is done processing a text
        //template to pass any errors that occurred to the host
        //the host can decide how to display them
        //---------------------------------------------------------------------
        /// <summary>
        /// Logs the errors.
        /// </summary>
        /// <param name="errors">The errors.</param>
        public void LogErrors(CompilerErrorCollection errors)
        {
            _errorsValue = errors;
        }

        /// <summary>
        /// Sets the output encoding.
        /// </summary>
        /// <param name="encoding">The encoding.</param>
        /// <param name="fromOutputDirective">if set to <c>true</c> [from output directive].</param>
        public void SetOutputEncoding(Encoding encoding, bool fromOutputDirective)
        {
        }

        /// <summary>
        /// Gets the template file.
        /// </summary>
        /// <value>The template file.</value>
        public string TemplateFile
        {
            get { return _inputFile ?? string.Empty; }
        }

        //this is the application domain that is used to compile and execute
        //the generated transformation class to create the generated text output
        //----------------------------------------------------------------------
        /// <summary>
        /// Provides the templating app domain.
        /// </summary>
        /// <param name="content">The content.</param>
        /// <returns></returns>
        public AppDomain ProvideTemplatingAppDomain(string content)
        {
            return AppDomain.CurrentDomain;
        }

        #endregion

        //public string GenerateCode(string inputContent)
        //{
        //    Engine engine = new Engine();
        //    return engine.ProcessTemplate(inputContent, this);
        //}

        //    public void CallT4Template(string templateFileName, string outputFileName)
        //    {
        //        if (templateFileName == null)
        //        {
        //            throw new ArgumentNullException("the file name cannot be null");
        //        }

        //        if (!File.Exists(templateFileName))
        //        {
        //            throw new FileNotFoundException("the file cannot be found");
        //        }

        //        CustomCmdLineHost host = new CustomCmdLineHost();
        //        Engine engine = new Engine();

        //        host.inputFile = templateFileName;

        //        //read the text template
        //        string input = File.ReadAllText(templateFileName);

        //        //transform the text template
        //        string output = engine.ProcessTemplate(input, host);

        //        if (host.Errors.HasErrors)
        //        {
        //            StringBuilder sb = new StringBuilder();
        //            StringWriter writer = new StringWriter(sb);
        //            foreach (CompilerError error in host.Errors)
        //            {
        //                writer.WriteLine(error.ToString());
        //            }
        //            output = sb.ToString();
        //        }

        //        File.WriteAllText(outputFileName, output);

        //    }
    }

    /// <summary>
    /// 
    /// </summary>
    public static class FileLocationHelper
    {
        private static string _dslToolsInstallDir;
        private static string _netInstallDir;
        private static string _vsInstallDir;

        /// <summary>
        /// Gets the net install dir.
        /// </summary>
        /// <value>The net install dir.</value>
        public static string NetInstallDir
        {
            get
            {
                if (_netInstallDir == null)
                {
                    using (RegistryKey key1 = Registry.LocalMachine.OpenSubKey(@"Software\Microsoft\.NETFramework"))
                    {
                        if (key1 != null)
                        {
                            _netInstallDir = (string) key1.GetValue("InstallRoot");
                        }
                    }
                }

                return _netInstallDir;
            }
        }

        /// <summary>
        /// Gets the DSL tools install dir.
        /// </summary>
        /// <value>The DSL tools install dir.</value>
        public static string DslToolsInstallDir
        {
            get
            {
                if (_dslToolsInstallDir == null)
                {
                    using (
                        RegistryKey key1 = Registry.LocalMachine.OpenSubKey(@"Software\Microsoft\VisualStudio\DSLTools")
                        )
                    {
                        if (key1 != null)
                        {
                            using (RegistryKey key2 = key1.OpenSubKey("2.0"))
                            {
                                if (key2 != null)
                                {
                                    _dslToolsInstallDir = (string) key2.GetValue("AssemblyDir");
                                    if (string.IsNullOrEmpty(_dslToolsInstallDir))
                                    {
                                        _dslToolsInstallDir = (string) key2.GetValue("RedistSDKDir");
                                    }
                                }
                            }
                        }
                    }
                }

                return _dslToolsInstallDir;
            }
        }

        /// <summary>
        /// Gets the VS install dir.
        /// </summary>
        /// <value>The VS install dir.</value>
        public static string VSInstallDir
        {
            get
            {
                if (_vsInstallDir == null)
                {
                    using (RegistryKey key1 = Registry.LocalMachine.OpenSubKey(@"Software\Microsoft\VisualStudio"))
                    {
                        if (key1 != null)
                        {
                            using (RegistryKey key2 = key1.OpenSubKey(ModelConstants.VisualStudioRegKey))
                            {
                                if (key2 != null)
                                {
                                    _vsInstallDir = (string) key2.GetValue("InstallDir");
                                }
                            }
                        }
                    }
                }
                return _vsInstallDir;
            }
        }
    }
}