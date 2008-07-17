using System;
using DSLFactory.Candle.SystemModel.Utilities;

namespace DSLFactory.Candle.SystemModel.Commands
{
    /// <summary>
    /// Importation d'assemblies externes
    /// </summary>
    public class ShowModelCommand : ICommand
    {
        private readonly IServiceProvider _serviceProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="ShowModelCommand"/> class.
        /// </summary>
        /// <param name="serviceProvider">The service provider.</param>
        public ShowModelCommand( IServiceProvider serviceProvider )
        {
            this._serviceProvider = serviceProvider;
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
        /// Execute the command
        /// </summary>
        public void Exec()
        {
            ModelVisitor visitor = new ModelVisitor();
            VSHierarchyWalker walker =  new VSHierarchyWalker(visitor);
            walker.Traverse(_serviceProvider);

            if( visitor.Models.Count == 0 )
            {
                try
                {
                    string solutionName = (string)ServiceLocator.Instance.ShellHelper.Solution.Properties.Item(9).Value;
                    if (solutionName == null)
                        throw new System.Exception();
                    ServiceLocator.Instance.ShellHelper.AddDSLModelToSolution(null, null, solutionName, true);
                }
                catch
                {
                    ServiceLocator.Instance.IDEHelper.ShowError("Can't create new model. You must save the solution before.");
                }
                return;
            }

            string modelFileName = visitor.Models[0];
            if (visitor.Models.Count > 1)
            {
                // TODO Affichage pour sélection
            }

            ServiceLocator.Instance.ShellHelper.EnsureDocumentOpen(modelFileName, new System.Guid( "a347c751-7722-4fa1-b73e-2e03db41d1c9" )); // SystemModelEditorFactoryID
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="ICommand"/> is visible.
        /// </summary>
        /// <returns></returns>
        /// <value><c>true</c> if visible; otherwise, <c>false</c>.</value>
        public bool Visible()
        {
            return _serviceProvider != null;
        }
    }
}
