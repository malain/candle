using System;
using EnvDTE;

namespace DSLFactory.Candle.SystemModel.CodeGeneration.CodeModel
{
    /// <summary>
    /// Parcours d'un 'codeModel' est g�n�ration d'un �v�nement � chaque �l�ment pour permettre aux strat�gies impl�mentant
    /// IStrategyCodeInjector de rajouter du code sp�cifique.
    /// </summary>
    /// <remarks>
    /// Cette g�n�ration est appel�e � chaque appel de code template dans StrategyBase.
    /// </remarks>
    public class CodeModelWalker
    {
        private readonly ICodeModelVisitorFilter _filter;
        private readonly ICodeModelVisitor _visitor;

        /// <summary>
        /// Initializes a new instance of the <see cref="CodeModelWalker"/> class.
        /// </summary>
        /// <param name="visitor">The visitor.</param>
        public CodeModelWalker(ICodeModelVisitor visitor) : this(visitor, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CodeModelWalker"/> class.
        /// </summary>
        /// <param name="visitor">The visitor.</param>
        /// <param name="filter">The filter.</param>
        public CodeModelWalker(ICodeModelVisitor visitor, ICodeModelVisitorFilter filter)
        {
            _filter = filter;
            _visitor = visitor;
        }

        /// <summary>
        /// Traverses the specified FCM.
        /// </summary>
        /// <param name="fcm">The FCM.</param>
        public void Traverse(FileCodeModel fcm)
        {
            if (fcm == null)
                return;

            try
            {
                _visitor.BeginTraverse(fcm);

                foreach (CodeElement cel in fcm.CodeElements)
                {
                    if (cel.Kind == vsCMElement.vsCMElementNamespace)
                    {
                        CandleCodeNamespace cns =
                            (CandleCodeNamespace) CandleCodeElement.CreateFromCodeElement(null, cel);
                        TraverseInternal(cns);
                    }
                }
                _visitor.EndTraverse(fcm);
            }
            catch (ExitException)
            {
            }
        }

        /// <summary>
        /// Traverses the internal.
        /// </summary>
        /// <param name="cce">The cce.</param>
        private void TraverseInternal(CandleCodeElement cce)
        {
            if (_filter == null || _filter.ShouldVisit(cce))
                cce.Accept(_visitor);

            foreach (CandleCodeElement child in cce.Members)
            {
                TraverseInternal(child);
            }
        }

        #region Nested type: ExitException

        /// <summary>
        /// Permet de suspendre l'it�ration
        /// </summary>
        public class ExitException : Exception
        {
        }

        #endregion
    }
}