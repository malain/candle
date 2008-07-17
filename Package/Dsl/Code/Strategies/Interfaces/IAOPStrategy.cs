using System;
using System.Collections.Generic;
using System.Text;
using DSLFactory.Candle.SystemModel.CodeGeneration.CodeModel;

namespace DSLFactory.Candle.SystemModel.Strategies
{
    /// <summary>
    /// 
    /// </summary>
    public interface IStrategyCodeInjector 
    {
        
        /// <summary>
        /// Identifiant unique de la stratégie
        /// </summary>
        String StrategyId { get;set;}

        /// <summary>
        /// Called when [generate class].
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="clazz">The clazz.</param>
        void OnGenerateClass(CodeInjectionContext context, CandleCodeClass clazz);

        /// <summary>
        /// Called when [generate function].
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="function">The function.</param>
        void OnGenerateFunction(CodeInjectionContext context, CandleCodeFunction function);

        /// <summary>
        /// Called when [generate parameter].
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="parm">The parm.</param>
        void OnGenerateParameter(CodeInjectionContext context, CandleCodeParameter parm);

        /// <summary>
        /// Called when [generate enum].
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="enumeration">The enumeration.</param>
        void OnGenerateEnum(CodeInjectionContext context, CandleCodeEnum enumeration);

        /// <summary>
        /// Called when [generate struct].
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="structure">The structure.</param>
        void OnGenerateStruct(CodeInjectionContext context, CandleCodeStruct structure);

        /// <summary>
        /// Called when [generate variable].
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="variable">The variable.</param>
        void OnGenerateVariable(CodeInjectionContext context, CandleCodeVariable variable);

        /// <summary>
        /// Called when [generate interface].
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="interf">The interf.</param>
        void OnGenerateInterface(CodeInjectionContext context, CandleCodeInterface interf);

        /// <summary>
        /// Called when [generate property].
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="prop">The prop.</param>
        void OnGenerateProperty(CodeInjectionContext context, CandleCodeProperty prop);

        /// <summary>
        /// Called when [meta model update].
        /// </summary>
        /// <param name="context">The context.</param>
        void OnMetaModelUpdate(CodeInjectionContext context);

        /// <summary>
        /// Retourne la liste des using
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns>null, tableau vide ou liste des usings</returns>
        string[] OnGenerateUsing(CodeInjectionContext context);
    }
}
