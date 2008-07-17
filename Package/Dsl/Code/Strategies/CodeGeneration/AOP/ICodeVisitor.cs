using EnvDTE;

namespace DSLFactory.Candle.SystemModel.CodeGeneration.CodeModel
{
    /// <summary>
    /// 
    /// </summary>
    public interface ICodeModelVisitor
    {
        /// <summary>
        /// Begins the traverse.
        /// </summary>
        /// <param name="fcm">The FCM.</param>
        void BeginTraverse(FileCodeModel fcm);
        /// <summary>
        /// Ends the traverse.
        /// </summary>
        /// <param name="fcm">The FCM.</param>
        void EndTraverse(FileCodeModel fcm);
        /// <summary>
        /// Visits the specified variable.
        /// </summary>
        /// <param name="variable">The variable.</param>
        void Visit(CandleCodeVariable variable);
        /// <summary>
        /// Visits the specified prop.
        /// </summary>
        /// <param name="prop">The prop.</param>
        void Visit(CandleCodeProperty prop);
        /// <summary>
        /// Visits the specified ns.
        /// </summary>
        /// <param name="ns">The ns.</param>
        void Visit(CandleCodeNamespace ns);
        /// <summary>
        /// Visits the specified interf.
        /// </summary>
        /// <param name="interf">The interf.</param>
        void Visit(CandleCodeInterface interf);
        /// <summary>
        /// Visits the specified structure.
        /// </summary>
        /// <param name="structure">The structure.</param>
        void Visit(CandleCodeStruct structure);
        /// <summary>
        /// Visits the specified function.
        /// </summary>
        /// <param name="function">The function.</param>
        void Visit(CandleCodeFunction function);
        /// <summary>
        /// Visits the specified clazz.
        /// </summary>
        /// <param name="clazz">The clazz.</param>
        void Visit(CandleCodeClass clazz);
        /// <summary>
        /// Visits the specified enumeration.
        /// </summary>
        /// <param name="enumeration">The enumeration.</param>
        void Visit(CandleCodeEnum enumeration);
        /// <summary>
        /// Visits the specified parm.
        /// </summary>
        /// <param name="parm">The parm.</param>
        void Visit(CandleCodeParameter parm);
    }
}