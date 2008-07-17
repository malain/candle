using Microsoft.VisualStudio.Modeling.Diagrams;

namespace DSLFactory.Candle.SystemModel.Commands
{
    /// <summary>
    /// 
    /// </summary>
    public class ArrangeShapesCommand : ICommand
    {
        private readonly NodeShape _shape;

        /// <summary>
        /// Initializes a new instance of the <see cref="ArrangeShapesCommand"/> class.
        /// </summary>
        /// <param name="shape">The shape.</param>
        public ArrangeShapesCommand(NodeShape shape)
        {
            this._shape = shape;
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
        /// Execute the command
        /// </summary>
        public void Exec()
        {
            if (_shape != null && _shape is ISupportArrangeShapes)
                ((ISupportArrangeShapes)_shape).ArrangeShapes();
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="ICommand"/> is visible.
        /// </summary>
        /// <returns></returns>
        /// <value><c>true</c> if visible; otherwise, <c>false</c>.</value>
        public bool Visible()
        {
            return _shape != null && _shape is ISupportArrangeShapes && ( _shape.NestedChildShapes.Count > 0 || _shape.RelativeChildShapes.Count > 0 );
        }

        #endregion
    }
}
