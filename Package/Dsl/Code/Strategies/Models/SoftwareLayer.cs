using System;
using DSLFactory.Candle.SystemModel.CodeGeneration;

namespace DSLFactory.Candle.SystemModel
{
    partial class SoftwareLayer
    {

        /// <summary>
        /// Génération du code pour les modèles enfants
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

            // Génération du layer - Contient les stratégies qui générent les projets
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
                // Si c'est le layer qui est sélectionné, on considère que tout ce qu'il contient
                //  sera généré. Donc on met à null, le selectedElement pour forcer la génération des 
                //  autres éléments. (qui sera repositionné dans le finally)
                if (selected)
                    context.SelectedElement = Guid.Empty; // RAZ temporaire

                if (GenerateChildsCode(context))
                    return true;
            }
            finally
            {
                if (selected)
                    context.SelectedElement = this.Id; // On remet comme c'était
            }
            return selected;
        }
    }
}