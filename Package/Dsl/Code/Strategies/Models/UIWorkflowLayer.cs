using System;

namespace DSLFactory.Candle.SystemModel
{
    partial class UIWorkflowLayer
    {
        /// <summary>
        /// Generates the childs code.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        protected override bool GenerateChildsCode(GenerationContext context)
        {
            bool selected = context.IsModelSelected(Id);
            try
            {
                // Si c'est le layer qui est s�lectionn�, on consid�re que tout ce qu'il contient
                //  sera g�n�r�. Donc on met � null, le selectedElement pour forcer la g�n�ration des 
                //  autres �l�ments. (qui sera repositionn� dans le finally)
                if (selected)
                    context.SelectedElement = Guid.Empty; // RAZ temporaire

                foreach (Scenario scenario in Scenarios)
                {
                    if (scenario.GenerateCode(context))
                        return true;
                }
            }
            finally
            {
                if (selected)
                    context.SelectedElement = Id; // On remet comme c'�tait
            }

            return false;
        }
    }
}