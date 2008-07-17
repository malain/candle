namespace DSLFactory.Candle.SystemModel
{
    /// <summary>
    /// 
    /// </summary>
    partial class ScenarioThumbnailShape
    {
    }

    partial class ScenarioShape
    {
        /// <summary>
        /// Gets a shape and checks to see whether its nested child shapes should be automatically positioned on the diagram.
        /// </summary>
        /// <value></value>
        /// <returns>true if the nested child shapes should be automatically positioned; otherwise, false.</returns>
        public override bool ShouldAutoPlaceChildShapes
        {
            get { return true; }
        }

        /// <summary>
        /// Gets the shape and checks to see whether it has a shadow.
        /// </summary>
        /// <value></value>
        /// <returns>true if the shape has a shadow; otherwise, false.</returns>
        public override bool HasShadow
        {
            get { return false; }
        }

        /// <summary>
        /// Gets the shape and checks to see whether it is highlighted.
        /// </summary>
        /// <value></value>
        /// <returns>true if the shape is highlighted; otherwise, false. </returns>
        public override bool HasHighlighting
        {
            get { return false; }
        }

        /// <summary>
        /// Gets the child shape and checks to see whether its parent shape can be resized when the child shape is resized.
        /// </summary>
        /// <value></value>
        /// <returns>true if the parent of a child shape can be resized when the child shape is resized; otherwise, false.</returns>
        public override bool AllowsChildrenToResizeParent
        {
            get { return true; }
        }
    }
}