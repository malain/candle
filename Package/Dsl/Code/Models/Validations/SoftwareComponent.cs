using System;
using System.Collections.Generic;
using DSLFactory.Candle.SystemModel.Strategies;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Validation;

namespace DSLFactory.Candle.SystemModel
{
    [ValidationState(ValidationState.Enabled)]
    public partial class SoftwareComponent
    {
        /// <summary>
        /// Namespace obligatoire
        /// </summary>
        /// <param name="context"></param>
        [ValidationMethod(ValidationCategories.Save | ValidationCategories.Menu)]
        protected void ValidateNamespace(ValidationContext context)
        {
            string msg = "Invalid namespace";
            bool test = false;
            try
            {
                // Set the test boolean to true, if validation is correct.
                test = !String.IsNullOrEmpty(Namespace) &&
                       StrategyManager.GetInstance(Store).NamingStrategy.IsNamespaceValid(Namespace);
            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }

            if (!test)
            {
                context.LogError(
                    msg,
                    "1", // Unique error number
                    this);
            }
            else if (!String.IsNullOrEmpty(StrategyManager.GetInstance(Store).NamingStrategy.DefaultNamespace) &&
                     !Namespace.StartsWith(StrategyManager.GetInstance(Store).NamingStrategy.DefaultNamespace))
            {
                context.LogWarning(
                    String.Format("Namespace must begin with '{0}'",
                                  StrategyManager.GetInstance(Store).NamingStrategy.DefaultNamespace),
                    "2", // Unique error number
                    this);
            }
        }

        /// <summary>
        /// Une seule couche principale
        /// </summary>
        /// <param name="context"></param>
        [ValidationMethod(ValidationCategories.Save | ValidationCategories.Menu)]
        protected void ValidateUILayer(ValidationContext context)
        {
            int cx = 0;
            int maxLevel = -1;

            foreach (AbstractLayer absLayer in Layers)
            {
                Layer layer = absLayer as Layer;
                if (layer == null)
                    continue;

                // Recherche des couches les + hautes
                if (layer.LayerPackage.Level > maxLevel)
                    maxLevel = layer.LayerPackage.Level;

                if (layer.StartupProject)
                {
                    cx++; // Nbre de couche principale
                }
            }

            if (cx > 1)
            {
                context.LogError("You can have only one main layer in a component.", "VALUI01", this);
            }

            // Seules les couches les + hautes peuvent hostées
            foreach (AbstractLayer absLayer in Layers)
            {
                Layer layer = absLayer as Layer;
                if (layer == null)
                    continue;
                if (layer.LayerPackage.Level != maxLevel)
                {
                    // Hosting 
                    if (layer.HostingContext != HostingContext.None)
                        context.LogError("Invalid hosting context. Change the hosting context to None", "VALUI02", layer);
                }
            }
        }

        /// <summary>
        /// Les noms des assemblies doivent être cohérents avec les noms des projets
        /// </summary>
        /// <param name="context"></param>
        [ValidationMethod(ValidationCategories.Save | ValidationCategories.Menu)]
        protected void ValidateAssemblyNames(ValidationContext context)
        {
            string msg = "All the layers within the same Visual Studio project must have the same assembly name";
            bool isValid = true;
            ModelElement elemInError = this;
            try
            {
                // Tous les projets Visual Studio de même nom doivent avoir le même nom
                // d'assemblie
                Dictionary<string, string> names = new Dictionary<string, string>();
                foreach (AbstractLayer layer in Layers)
                {
                    SoftwareLayer slayer = layer as SoftwareLayer;
                    if (slayer == null)
                        continue;

                    if (names.ContainsKey(slayer.VSProjectName))
                    {
                        if (!Utils.StringCompareEquals(names[slayer.VSProjectName], slayer.AssemblyName))
                        {
                            isValid = false;
                            elemInError = layer;
                            break;
                        }
                    }
                    else
                        names[slayer.VSProjectName] = slayer.AssemblyName;
                }
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                isValid = false;
            }

            if (!isValid)
            {
                context.LogError(
                    msg,
                    "NMS1", // Unique error number
                    elemInError);
            }
        }
    }
}