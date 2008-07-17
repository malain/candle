using EnvDTE;

namespace DSLFactory.Candle.SystemModel.Utilities
{
    /// <summary>
    /// 
    /// </summary>
    public interface IVSHierarchyVisitor
    {
        /// <summary>
        /// Accepts the specified project item.
        /// </summary>
        /// <param name="projectItem">The project item.</param>
        void Accept(ProjectItem projectItem);
    }
}