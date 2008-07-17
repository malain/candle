namespace DSLFactory.Candle.SystemModel.CodeGeneration.CodeModel
{
    /// <summary>
    /// 
    /// </summary>
    public interface ICodeModelVisitorFilter
    {
        /// <summary>
        /// Shoulds the visit.
        /// </summary>
        /// <param name="codeElement">The code element.</param>
        /// <returns></returns>
        bool ShouldVisit(CandleCodeElement codeElement);
    }
}