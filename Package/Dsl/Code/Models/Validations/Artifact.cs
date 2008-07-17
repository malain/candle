using System;
using Microsoft.VisualStudio.Modeling.Validation;

namespace DSLFactory.Candle.SystemModel
{
    [ValidationState(ValidationState.Enabled)]
    public partial class Artifact
    {
        // Define when the validation occurs
        /// <summary>
        /// Validates the project.
        /// </summary>
        /// <param name="context">The context.</param>
        [ValidationMethod(ValidationCategories.Save | ValidationCategories.Menu)]
        protected void ValidateProject(ValidationContext context)
        {
            if (Type != ArtifactType.Project)
                return;

            string msg = "Assembly name is not valid for the artifact {0} in the layer {1}.";
            bool isValid = false;
            try
            {
                IShellHelper shell = ServiceLocator.Instance.GetService<IShellHelper>();
                if (shell != null)
                    isValid = shell.FindProjectByAssemblyName(InitialFileName) != null;
            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }

            if (!isValid)
            {
                AbstractLayer layer = LayerHasArtifacts.GetLayer(this);
                string layerName = layer != null ? layer.Name : "???";
                context.LogError(
                    String.Format(msg, FileName, layerName),
                    "ERRART1", // Unique error number
                    this);
            }
        }
    }
}