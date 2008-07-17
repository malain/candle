using Microsoft.VisualStudio.Modeling.Shell;
using DslModeling=Microsoft.VisualStudio.Modeling;
using DslDiagrams=Microsoft.VisualStudio.Modeling.Diagrams;
using DslShell=Microsoft.VisualStudio.Modeling.Shell;

namespace DSLFactory.Candle.SystemModel
{
    /// <summary>
    /// Class that hosts the diagram surface in the Visual Studio document area.
    /// </summary>
    internal partial class CandleDocView
    {
        /// <summary>
        /// Lors de la fermeture de la vue principale, on s'assure que toutes les autres seront bien fermèes
        /// </summary>
        protected override void OnClose()
        {
            // Pattern pour supprimer un item dans un iterateur
            for (;;)
            {
                ModelingDocView viewToClose = null;
                foreach (ModelingDocView view in DocData.DocViews)
                {
                    if (view != this)
                    {
                        viewToClose = view;
                        break;
                    }
                }

                // Si il n'y a rien à supprimer, on s'arrete
                if (viewToClose == null)
                    break;

                viewToClose.Frame.CloseFrame(0);
            }

            base.OnClose();
        }
    }
}