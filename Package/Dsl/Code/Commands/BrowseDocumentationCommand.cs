using System;
using DSLFactory.Candle.SystemModel.Repository;
using Microsoft.VisualStudio.Modeling.Diagrams;

namespace DSLFactory.Candle.SystemModel.Commands
{
    /// <summary>
    /// Importation d'assemblies externes
    /// </summary>
    public class ShowDocumentationCommand : ICommand
    {
        private readonly ComponentModelMetadata _metadata;

        /// <summary>
        /// Initializes a new instance of the <see cref="ShowDocumentationCommand"/> class.
        /// </summary>
        /// <param name="metadata">The metadata.</param>
        public ShowDocumentationCommand(ComponentModelMetadata metadata)
        {
            this._metadata = metadata;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ShowDocumentationCommand"/> class.
        /// </summary>
        /// <param name="obj">The obj.</param>
        public ShowDocumentationCommand(object obj)
        {
            NodeShape shape = obj as NodeShape;
            if (shape != null && shape.ModelElement is ExternalComponent)
            {
                _metadata = ((ExternalComponent)shape.ModelElement).MetaData;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="ICommand"/> is enabled.
        /// </summary>
        /// <value><c>true</c> if enabled; otherwise, <c>false</c>.</value>
        public bool Enabled
        {
            get { return Visible() && !String.IsNullOrEmpty( _metadata.DocUrl); }
        }

        /// <summary>
        /// Execute the command
        /// </summary>
        public void Exec()
        {
            IShellHelper shell = ServiceLocator.Instance.ShellHelper;
            if (shell != null)
                shell.Navigate(_metadata.DocUrl);
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="ICommand"/> is visible.
        /// </summary>
        /// <returns></returns>
        /// <value><c>true</c> if visible; otherwise, <c>false</c>.</value>
        public bool Visible()
        {
            return _metadata != null;
        }
    }
}
