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
    partial class DataLayer
    {
        /// <summary>
        /// Generates the childs code.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        protected override bool GenerateChildsCode(GenerationContext context)
        {
            foreach( Package package in this.Packages )
            {
                if( package.GenerateCode( context ) )
                    return true;
            }

            return false;
        }


    }
}
