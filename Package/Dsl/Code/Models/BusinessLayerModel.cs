using System;
using System.Collections.Generic;
using System.Text;

namespace DSLFactory.Candle.SystemModel
{
    /// <summary>
    /// 
    /// </summary>
    partial class BusinessLayer
    {
        /// <summary>
        /// Gets the level.
        /// </summary>
        /// <value>The level.</value>
        public override short Level
        {
            get { return 50; }
        }

        //internal override bool GenerateCode(GenerationContext context)
        //{
        //    bool flag = base.GenerateCode(context);
        //    if (context.CanGenerate(this.Id))
        //    {
        //        foreach (Process process in this.Process)
        //        {
        //            if (process.GenerateCode(context))
        //                return true;
        //        }
        //    }
        //    return flag;
        //}
    }
}
