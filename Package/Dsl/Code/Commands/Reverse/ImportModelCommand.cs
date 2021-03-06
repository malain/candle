using System;
using System.IO;
using DSLFactory.Candle.SystemModel.Commands.Reverse;
using EnvDTE;
using Microsoft.VisualStudio.Modeling.Diagrams;

namespace DSLFactory.Candle.SystemModel.Commands
{
    /// <summary>
    /// 
    /// </summary>
    public class ImportModelCommand : ICommand
    {
        private readonly DataLayer _layer;
        private readonly NodeShape _shape;

        /// <summary>
        /// Initializes a new instance of the <see cref="ImportModelCommand"/> class.
        /// </summary>
        /// <param name="element">The element.</param>
        public ImportModelCommand( object element )
        {
            _shape = element as NodeShape;
            if( _shape == null )
                return;
            _layer = _shape.ModelElement as DataLayer;
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

            ReverseModelsForm form = new ReverseModelsForm(delegate(Type type) { return (type.IsClass || type.IsEnum) && type.IsPublic; });

            Project prj = ServiceLocator.Instance.ShellHelper.FindProjectByName(_layer.Name);
            if( prj != null )
            {
                try
                {
                    // TODO prendre la valeur dans la config du projet
                    string path = String.Format(@"{0}\bin\debug\{1}", Path.GetDirectoryName(prj.FileName), prj.Properties.Item("AssemblyName").Value);
                    form.Init( path );
                }
                catch { }
            }

            if( form.ShowDialog() == System.Windows.Forms.DialogResult.Cancel )
                return;

            ReverseModels action = new ReverseModels( _layer );
            action.ReferencedAssemblies( form.ReferencedAssemblies );
            action.AddAssembly( form.FullPath, form.SelectedClasses );

            ArrangeShapesCommand command = new ArrangeShapesCommand( _shape );
            command.Exec();
        }

        #endregion
    }
}
