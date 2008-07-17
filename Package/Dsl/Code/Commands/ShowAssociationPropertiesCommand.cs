using DSLFactory.Candle.SystemModel.Rules.Wizards;
using Microsoft.VisualStudio.Modeling;

namespace DSLFactory.Candle.SystemModel.Commands
{
    class ShowAssociationPropertiesCommand : ICommand
    {
        private readonly Association _association;

        /// <summary>
        /// Initializes a new instance of the <see cref="ShowAssociationPropertiesCommand"/> class.
        /// </summary>
        /// <param name="element">The element.</param>
        public ShowAssociationPropertiesCommand( ModelElement element )
        {
            this._association = element as Association;
        }

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
            return _association != null;
        }

        /// <summary>
        /// Execute the command
        /// </summary>
        public void Exec()
        {
            using (Transaction transaction = _association.Store.TransactionManager.BeginTransaction("Change foreign keys"))
            {
                AssociationPropertiesSelectorForm dlg = new AssociationPropertiesSelectorForm(this._association);
                if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    transaction.Commit();
                }
                else
                    transaction.Rollback();
            }
        }
    }
}
