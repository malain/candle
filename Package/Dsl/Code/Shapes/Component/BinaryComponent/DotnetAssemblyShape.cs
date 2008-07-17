using System.IO;
using System.Windows.Forms;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Diagrams;
using DslModeling=Microsoft.VisualStudio.Modeling;

namespace DSLFactory.Candle.SystemModel
{
    partial class DotnetAssemblyShape
    {
        #region Import d'un service par drag'n drop

        /// <summary>
        /// Import d'un service
        /// </summary>
        /// <param name="e">The diagram drag event arguments.</param>
        public override void OnDragDrop(DiagramDragEventArgs e)
        {
            base.OnDragDrop(e);

            if (e.Data.GetDataPresent(DataFormats.Text))
            {
                string txt = (string) e.Data.GetData(DataFormats.Text);
                using (
                    Transaction transaction = ModelElement.Store.TransactionManager.BeginTransaction("Import interface")
                    )
                {
                    IImportInterfaceHelper importer = ServiceLocator.Instance.GetService<IImportInterfaceHelper>();
                    if (importer == null)
                        return;
                    if (importer.ImportOperations(ModelElement as Layer, null, txt))
                    {
                        RebuildShape();
                        transaction.Commit();
                    }
                }
            }
        }

        /// <summary>
        /// Alerts listeners when the shape is dragged over its bounds.
        /// </summary>
        /// <param name="e">The diagram drag event arguments.</param>
        public override void OnDragOver(DiagramDragEventArgs e)
        {
            base.OnDragOver(e);
            if (e.Data.GetDataPresent(DataFormats.Text))
            {
                string txt = (string) e.Data.GetData(DataFormats.Text);
                if (File.Exists(txt))
                    e.Effect = DragDropEffects.Link;
            }
        }

        #endregion
    }
}