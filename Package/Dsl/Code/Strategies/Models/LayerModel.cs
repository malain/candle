using System;
using System.Collections.Generic;
using System.Text;
using EnvDTE;
using DSLFactory.Candle.SystemModel.Strategies;
using DSLFactory.Candle.SystemModel.CodeGeneration;

namespace DSLFactory.Candle.SystemModel
{
    /// <summary>
    /// 
    /// </summary>
    partial class Layer
    {
        /// <summary>
        /// G�n�ration du code pour les mod�les enfants
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        protected override bool GenerateChildsCode(GenerationContext context)
        {
            foreach (ClassImplementation clazz in this.Classes)
            {
                if (clazz.GenerateCode(context))
                    return true;
            }
            return base.GenerateChildsCode(context);
        }
       
    }
}
