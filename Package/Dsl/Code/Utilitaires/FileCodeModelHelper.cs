using System;
using System.Collections.Generic;
using EnvDTE;
using EnvDTE80;

namespace DSLFactory.Candle.SystemModel.CodeGeneration
{
    /// <summary>
    /// Classe permettant de naviguer
    /// </summary>
    [CLSCompliant(false)]
    public class ClassCodeGenerator
    {
        //private List<StrategyBase> _strategies;

        /// <summary>
        /// Initializes a new instance of the <see cref="ClassCodeGenerator"/> class.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="port">The port.</param>
        public ClassCodeGenerator(ProjectItem item, ClassImplementation port)
        {
            //_strategies = new List<StrategyBase>();
            //foreach( StrategyBase strategy in port.GetStrategies(true) )
            //{
            //    IStrategyInjectCode si = strategy as IStrategyInjectCode;
            //    if( si == null || ( strategy is IStrategyInterface ) )
            //        continue;

            //    _strategies.Add( strategy );
            //}
        }

        #region Manipulation du code

        /// <summary>
        /// Finds the or create custom attribute element.
        /// </summary>
        /// <param name="clazz">The clazz.</param>
        /// <param name="name">The name.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static CodeAttribute2 FindOrCreateCustomAttributeElement(CodeClass clazz, string name, string value)
        {
            foreach (CodeElement ce in clazz.Attributes)
            {
                CodeAttribute2 attr = ce as CodeAttribute2;
                if (attr != null && attr.Name == name)
                    return attr;
            }
            return (CodeAttribute2) clazz.AddAttribute(name, value, -1);
        }

        /// <summary>
        /// Finds the or create interface element.
        /// </summary>
        /// <param name="cn">The cn.</param>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public static CodeInterface2 FindOrCreateInterfaceElement(CodeNamespace cn, string name)
        {
            foreach (CodeElement ce in cn.Members)
            {
                if (ce is CodeInterface2 && ce.Name == name)
                    return (CodeInterface2) ce;
            }
            return
                (CodeInterface2)
                cn.AddInterface(name, 0 /* au debut */, null /*classes de bases*/, vsCMAccess.vsCMAccessPublic);
        }

        /// <summary>
        /// Finds the class.
        /// </summary>
        /// <param name="fileCodeModel">The file code model.</param>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public static CodeClass2 FindClass(FileCodeModel fileCodeModel, string name)
        {
            foreach (CodeElement ce in fileCodeModel.CodeElements)
            {
                CodeNamespace cn = ce as CodeNamespace;
                if (cn != null)
                {
                    foreach (CodeElement ce2 in cn.Members)
                    {
                        if (ce2 is CodeClass2 && ce2.Name == name)
                            return (CodeClass2) ce2;
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// Finds the or create class element.
        /// </summary>
        /// <param name="cn">The cn.</param>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public static CodeClass2 FindOrCreateClassElement(CodeNamespace cn, string name)
        {
            foreach (CodeElement ce in cn.Members)
            {
                if (ce is CodeClass2 && ce.Name == name)
                    return (CodeClass2) ce;
            }
            return
                (CodeClass2)
                cn.AddClass(name, 0 /* au debut */, null /*classes de bases*/, null /*interfaces*/,
                            vsCMAccess.vsCMAccessPublic);
        }

        /// <summary>
        /// Finds the or create import.
        /// </summary>
        /// <param name="fileCodeModel">The file code model.</param>
        /// <param name="importName">Name of the import.</param>
        /// <returns></returns>
        public static CodeImport FindOrCreateImport(FileCodeModel2 fileCodeModel, string importName)
        {
            foreach (CodeElement ce in (fileCodeModel).CodeElements)
            {
                if (ce is CodeImport && ((CodeImport) ce).Namespace == importName)
                {
                    return (CodeImport) ce;
                }
            }

            return (fileCodeModel).AddImport(importName, 0, null);
        }

        /// <summary>
        /// Finds the or create namespace.
        /// </summary>
        /// <param name="fileCodeModel">The file code model.</param>
        /// <param name="namespace">The @namespace.</param>
        /// <returns></returns>
        internal static CodeNamespace FindOrCreateNamespace(FileCodeModel fileCodeModel, string @namespace)
        {
            // On considère qu'il n'y a qu'un namespace par fichier
            foreach (CodeElement ce in fileCodeModel.CodeElements)
            {
                if (ce is CodeNamespace)
                {
                    ce.Name = @namespace;
                    return (CodeNamespace) ce;
                }
            }

            return fileCodeModel.AddNamespace(@namespace, 0);
        }

        /// <summary>
        /// Finds the function interface element.
        /// </summary>
        /// <param name="cc">The cc.</param>
        /// <param name="functionName">Name of the function.</param>
        /// <param name="arguments">The arguments.</param>
        /// <returns></returns>
        internal static CodeFunction2 FindFunctionInterfaceElement(CodeInterface cc, string functionName,
                                                                   List<Argument> arguments)
        {
            foreach (CodeElement ce in cc.Members)
            {
                if (ce is CodeFunction2 && functionName == ce.Name)
                {
                    CodeFunction2 cf = ce as CodeFunction2;
                    if (cf.Parameters.Count == arguments.Count)
                    {
                        // On regarde si les paramètres correspondent
                        for (int i = 0; i < arguments.Count; i++)
                        {
                            CodeParameter param = (CodeParameter) cf.Parameters.Item(i + 1);
                            if (param.Name != arguments[i].Name)
                                return null;
                        }
                        return cf;
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// Finds the function code element.
        /// </summary>
        /// <param name="cc">The cc.</param>
        /// <param name="functionName">Name of the function.</param>
        /// <param name="arguments">The arguments.</param>
        /// <returns></returns>
        internal static CodeFunction2 FindFunctionCodeElement(CodeClass cc, string functionName,
                                                              List<Argument> arguments)
        {
            foreach (CodeElement ce in cc.Members)
            {
                if (ce is CodeFunction2 && functionName == ce.Name)
                {
                    CodeFunction2 cf = ce as CodeFunction2;

                    if (arguments == null)
                        return cf;

                    if (cf.Parameters.Count == arguments.Count)
                    {
                        // On regarde si les paramètres correspondent
                        for (int i = 0; i < arguments.Count; i++)
                        {
                            CodeParameter param = (CodeParameter) cf.Parameters.Item(i + 1);
                            if (param.Name != arguments[i].Name)
                                return null;
                        }
                        return cf;
                    }
                }
            }
            return null;
        }


        //public void ShowInClassDesigner( string projectName, string typeName )
        //{
        //    //Project prj = FindProject( projectName );

        //    //IVsSolution sln = ApplicationModelPackage.Singleton.GetServiceInternal( typeof( IVsSolution ) ) as IVsSolution;
        //    //IVsHierarchy hierarchy;
        //    //ErrorHandler.ThrowOnFailure( sln.GetProjectOfUniqueName( prj.UniqueName, out hierarchy ) );
        //    //IVsProject3 project2 = hierarchy as IVsProject3;

        //    //ClassDesignerHelper.ShowClassInDiagram( ApplicationModelPackage.Singleton,
        //    //    project2,
        //    //    typeName );
        //}


        /// <summary>
        /// Adds the interface if not exists.
        /// </summary>
        /// <param name="cc">The cc.</param>
        /// <param name="interfaceName">Name of the interface.</param>
        public static void AddInterfaceIfNotExists(CodeClass2 cc, string interfaceName)
        {
            foreach (CodeElement elem in cc.ImplementedInterfaces)
            {
                if (elem.Name == interfaceName)
                    return;
            }

            try
            {
                cc.AddImplementedInterface(interfaceName, 0);
            }
            catch
            {
            }
        }

        #endregion
    }
}