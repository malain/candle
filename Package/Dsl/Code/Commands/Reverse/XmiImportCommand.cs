using System;
using DSLFactory.Candle.SystemModel.Commands.Reverse;
using Microsoft.VisualStudio.Modeling.Diagrams;

namespace DSLFactory.Candle.SystemModel.Commands
{
    /// <summary>
    /// Importation d'un fichier xmi
    /// </summary>
    public class ImportXmiCommand : ICommand
    {
        private readonly DataLayer _layer;

        /// <summary>
        /// Initializes a new instance of the <see cref="ImportXmiCommand"/> class.
        /// </summary>
        /// <param name="element">The element.</param>
        public ImportXmiCommand(object element)
        {
            NodeShape shape = element as NodeShape;
            if( shape == null )
                return;
            _layer = shape.ModelElement as DataLayer;
        }

        #region ICommand Members
        /// <summary>
        /// Gets a value indicating whether this <see cref="ICommand"/> is enabled.
        /// </summary>
        /// <value><c>true</c> if enabled; otherwise, <c>false</c>.</value>
        public bool Enabled
        {
            get { return Visible(); }
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="ICommand"/> is visible.
        /// </summary>
        /// <returns></returns>
        /// <value><c>true</c> if visible; otherwise, <c>false</c>.</value>
        public bool Visible()
        {
            return _layer != null;
        }

        /// <summary>
        /// Execute the command
        /// </summary>
        public void Exec()
        {
            if( _layer == null )
                return;
            XmiImporter importer = new XmiImporter(_layer);

            PromptBox prompt = new PromptBox("Xmi file name", "xmi files|*.xmi");
            if (prompt.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                try
                {
                    importer.Import(prompt.Value);
                }
                catch (Exception ex)
                {
                    ServiceLocator.Instance.IDEHelper.ShowError("Import error : " + ex.Message);
                }
            }
        }

        #endregion
    }
}
