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
    partial class Package
    {
        /// <summary>
        /// Gets the strategies owner.
        /// </summary>
        /// <value>The strategies owner.</value>
        public override CandleElement StrategiesOwner
        {
            get { return this.Layer.StrategiesOwner; }
        }

        /// <summary>
        /// Gets the owner.
        /// </summary>
        /// <value>The owner.</value>
        public override ICustomizableElement Owner
        {
            get { return this.Layer; }
        }

        /// <summary>
        /// Generates the code.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        internal override bool GenerateCode( GenerationContext context )
        {
            bool selected = base.GenerateCode(context);
            
            try
            {
                // Si c'est le layer qui est s�lectionn�, on consid�re que tout ce qu'il contient
                //  sera g�n�r�. Donc on met � null, le selectedElement pour forcer la g�n�ration des 
                //  autres �l�ments. (qui sera repositionn� dans le finally)
                if (selected)
                    context.SelectedElement = Guid.Empty; // RAZ temporaire

                foreach (DataType type in this.Types)
                {
                    if (type.GenerateCode(context))
                        return true;
                }
            }
            finally
            {
                if (selected)
                    context.SelectedElement = this.Id; // On remet comme c'�tait
            }

            return selected;
        }


    }
}
