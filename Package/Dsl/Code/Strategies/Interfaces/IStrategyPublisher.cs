using System;

namespace DSLFactory.Candle.SystemModel.Strategies
{
    /// <summary>
    /// 
    /// </summary>
    [Obsolete()]
    [CLSCompliant(true)]
    public interface IStrategyPublisher
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <param name="folder"></param>
        void Publish(CandleModel model, string folder);
    }

    /// <summary>
    /// 
    /// </summary>
    public class PublishingException : Exception
    {
    }

    /// <summary>
    /// 
    /// </summary>
    [CLSCompliant(true)]
    public interface IStrategyPublishEvents
    {
        /// <summary>
        /// Called when [before local publication].
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="modelFileName">Name of the model file.</param>
        void OnBeforeLocalPublication(CandleModel model, string modelFileName);

        /// <summary>
        /// Called when [after local publication].
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="modelFileName">Name of the model file.</param>
        void OnAfterLocalPublication(CandleModel model, string modelFileName);

        /// <summary>
        /// Called when [before server publication].
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="modelFileName">Name of the model file.</param>
        void OnBeforeServerPublication(CandleModel model, string modelFileName);

        /// <summary>
        /// Called when [after server publication].
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="modelFileName">Name of the model file.</param>
        void OnAfterServerPublication(CandleModel model, string modelFileName);

        /// <summary>
        /// Called when [publication ended].
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="modelFileName">Name of the model file.</param>
        void OnPublicationEnded(CandleModel model, string modelFileName);

        /// <summary>
        /// Called when [publication error].
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="modelFileName">Name of the model file.</param>
        void OnPublicationError(CandleModel model, string modelFileName);
    }
}