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
                // Si c'est le layer qui est sélectionné, on considère que tout ce qu'il contient
                //  sera généré. Donc on met à null, le selectedElement pour forcer la génération des 
                //  autres éléments. (qui sera repositionné dans le finally)
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
                    context.SelectedElement = Id; // On remet comme c'était
            }

            return false;
        }
    }
}