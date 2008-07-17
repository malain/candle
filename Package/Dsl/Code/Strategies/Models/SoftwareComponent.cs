using System.Collections.Generic;
using DSLFactory.Candle.SystemModel.CodeGeneration;

namespace DSLFactory.Candle.SystemModel
{
    partial class SoftwareComponent
    {
        /// <summary>
        /// Méthode principale pour la génération du code
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        internal override bool GenerateCode(GenerationContext context)
        {
            if (!context.IsModelSelected(Id))
            {
                if (IsDataLayerExists && DataLayer.GenerateCode(context))
                    return true;

                // Copie pour éviter le cas ou une stratégie utilise la propriété DataLayer alors
                // que la couche n'existe pas. Dans ce cas, la couche va automatiquement etre créée et va
                // ainsi modifier la liste des Layers (ce qui fait planter l'itération)
                List<SoftwareLayer> layers = new List<SoftwareLayer>(Layers);
                foreach (SoftwareLayer layer in layers)
                {
                    if (layer is InterfaceLayer)
                    {
                        if (layer.GenerateCode(context))
                            return true;
                    }
                }

                foreach (SoftwareLayer layer in layers)
                {
                    if (!(layer is InterfaceLayer) && !(layer is DataLayer))
                    {
                        if (layer.GenerateCode(context))
                            return true;
                    }
                }
            }

            if (context.CanGenerate(Id))
            {
                Generator.ApplyProjectGeneratorStrategies(this, context);
                Generator.ApplyStrategies(this, context);
            }

            return false;
        }
    }
}