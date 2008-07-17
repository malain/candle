using System;
using DSLFactory.Candle.SystemModel.CodeGeneration;

namespace DSLFactory.Candle.SystemModel
{
    partial class SoftwareLayer
    {

        /// <summary>
        /// G�n�ration du code pour les mod�les enfants
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        protected virtual bool GenerateChildsCode(GenerationContext context)
        {
            //foreach (ExternalServiceReference relationShip in ExternalServiceReference.GetLinksToExternalServiceReferences(this))
            //{
            //    if (relationShip.GenerateCode(context))
            //        return true;
            //}
            return false;
        }

        /// <summary>
        /// Generates the code.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        internal sealed override bool GenerateCode(GenerationContext context)
        {
            bool selected = context.IsModelSelected(this.Id);

            // G�n�ration du layer - Contient les strat�gies qui g�n�rent les projets
            if (context.GenerationPass == GenerationPass.CodeGeneration)
            {
                Generator.ApplyProjectGeneratorStrategies(this, context);
            }

            if (context.CanGenerate(this.Id))
            {
                if (context.Project == null && context.GenerationPass == GenerationPass.CodeGeneration)
                    return selected;

                Generator.ApplyStrategies(this, context);
            }
            try
            {
                // Si c'est le layer qui est s�lectionn�, on consid�re que tout ce qu'il contient
                //  sera g�n�r�. Donc on met � null, le selectedElement pour forcer la g�n�ration des 
                //  autres �l�ments. (qui sera repositionn� dans le finally)
                if (selected)
                    context.SelectedElement = Guid.Empty; // RAZ temporaire

                if (GenerateChildsCode(context))
                    return true;
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