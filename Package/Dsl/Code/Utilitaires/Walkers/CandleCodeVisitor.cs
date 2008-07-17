using System;
using DSLFactory.Candle.SystemModel.Strategies;
using EnvDTE;
using EnvDTE80;

namespace DSLFactory.Candle.SystemModel.CodeGeneration.CodeModel
{
    /// <summary>
    /// 
    /// </summary>
    internal class CodeInjectorVisitor : ICodeModelVisitor
    {
        private readonly CodeInjectionContext _context;
        private readonly IStrategyCodeInjector _injector;


        /// <summary>
        /// Initializes a new instance of the <see cref="CodeInjectorVisitor"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
        public CodeInjectorVisitor(CodeInjectionContext context)
        {
            _context = context;
            _injector = context.Strategy;
        }

        #region ICodeVisitor

        public void BeginTraverse(FileCodeModel fcm)
        {
            FileCodeModel2 fcm2 = fcm as FileCodeModel2;
            if(fcm2==null)
                return;
            String[] imports = _injector.OnGenerateUsing(_context);
            if (imports != null)
            {
                foreach (string import in imports)
                {
                    try
                    {
                        fcm2.AddImport(import, null, String.Empty);
                    }
                    catch
                    {
                    }
                }
            }
        }

        /// <summary>
        /// Ends the traverse.
        /// </summary>
        /// <param name="fcm">The FCM.</param>
        public void EndTraverse(FileCodeModel fcm)
        {
        }

        /// <summary>
        /// Visits the specified clazz.
        /// </summary>
        /// <param name="clazz">The clazz.</param>
        public void Visit(CandleCodeClass clazz)
        {
            _injector.OnGenerateClass(_context, clazz);
        }

        /// <summary>
        /// Visits the specified function.
        /// </summary>
        /// <param name="function">The function.</param>
        public void Visit(CandleCodeFunction function)
        {
            _injector.OnGenerateFunction(_context, function);
        }


        /// <summary>
        /// Visits the specified parm.
        /// </summary>
        /// <param name="parm">The parm.</param>
        public void Visit(CandleCodeParameter parm)
        {
            // attributs
            _injector.OnGenerateParameter(_context, parm);
        }

        /// <summary>
        /// Visits the specified enumeration.
        /// </summary>
        /// <param name="enumeration">The enumeration.</param>
        public void Visit(CandleCodeEnum enumeration)
        {
            // attributs
            _injector.OnGenerateEnum(_context, enumeration);
        }

        /// <summary>
        /// Visits the specified structure.
        /// </summary>
        /// <param name="structure">The structure.</param>
        public void Visit(CandleCodeStruct structure)
        {
            // attributs
            _injector.OnGenerateStruct(_context, structure);
        }

        /// <summary>
        /// Visits the specified prop.
        /// </summary>
        /// <param name="prop">The prop.</param>
        public void Visit(CandleCodeProperty prop)
        {
            // attributs
            _injector.OnGenerateProperty(_context, prop);
        }

        /// <summary>
        /// Visits the specified variable.
        /// </summary>
        /// <param name="variable">The variable.</param>
        public void Visit(CandleCodeVariable variable)
        {
            // attributs
            _injector.OnGenerateVariable(_context, variable);
        }

        /// <summary>
        /// Visits the specified interf.
        /// </summary>
        /// <param name="interf">The interf.</param>
        public void Visit(CandleCodeInterface interf)
        {
            // attributs
            _injector.OnGenerateInterface(_context, interf);
        }

        /// <summary>
        /// Visits the specified ns.
        /// </summary>
        /// <param name="ns">The ns.</param>
        public void Visit(CandleCodeNamespace ns)
        {
            // using
        }

        #endregion
    }
}