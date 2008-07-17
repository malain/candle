using System;
using Microsoft.VisualStudio.Modeling;

namespace DSLFactory.Candle.SystemModel.Rules
{
    /// <summary>
    /// 
    /// </summary>
    [RuleOn(typeof (DotnetAssemblyShape), FireTime=TimeToFire.TopLevelCommit, InitiallyDisabled=false)]
    public class ExternalAssemblyShapeInsertRule : AddRule
    {
        // Ajout de l'élément
        /// <summary>
        /// Alerts listeners that a rule has been used.
        /// </summary>
        /// <param name="e">An ElementAddedEventArgs that contains the event data.</param>
        public override void ElementAdded(ElementAddedEventArgs e)
        {
            #region Condition

            // Test the element
            DotnetAssemblyShape shape = e.ModelElement as DotnetAssemblyShape;
            if (shape == null)
                return;

            // Teste qu'elle est la source qui a générée l'événement
            if (shape.Store.TransactionManager.CurrentTransaction.TopLevelTransaction.IsSerializing ||
                shape.Store.InUndoRedoOrRollback)
                return;

            #endregion

            #region Traitement

            DotNetAssembly externalAssembly = shape.ModelElement as DotNetAssembly;
            if (externalAssembly != null && !String.IsNullOrEmpty(externalAssembly.FullName))
                return;

            using (
                Transaction transaction =
                    externalAssembly.Store.TransactionManager.BeginTransaction("Retrieve assembly infos"))
            {
                try
                {
                    IAssemblySelectorDialog selector = ServiceLocator.Instance.GetService<IAssemblySelectorDialog>();
                    if (selector.ShowDialog(1))
                    {
                        externalAssembly.InitFromAssembly(selector.SelectedAssemblies[0], true);
                        transaction.Commit();
                    }
                }
                catch (Exception ex)
                {
                    ILogger logger = ServiceLocator.Instance.GetService<ILogger>();
                    if (logger != null)
                        logger.WriteError("Import assembly", "Reflection error", ex);
                    transaction.Rollback();
                }
            }

            #endregion
        }
    }
}