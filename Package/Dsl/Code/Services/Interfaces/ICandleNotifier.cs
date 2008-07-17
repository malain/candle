using System;

namespace DSLFactory.Candle.SystemModel
{
    /// <summary>
    /// Service permettant de générer les événements de modifications du contexte
    /// </summary>
    public interface ICandleNotifier
    {
        /// <summary>
        /// Occurs when [the candle options changed].
        /// </summary>
        event EventHandler OptionsChanged;
        /// <summary>
        /// Occurs when [solution opened].
        /// </summary>
        event EventHandler SolutionOpened;
        /// <summary>
        /// Occurs when [solution closed].
        /// </summary>
        event EventHandler SolutionClosed;

        /// <summary>
        /// Notifies the options changed.
        /// </summary>
        /// <param name="sender">The sender.</param>
        void NotifyOptionsChanged(object sender);
        /// <summary>
        /// Notifies the solution opened.
        /// </summary>
        /// <param name="sender">The sender.</param>
        void NotifySolutionOpened(object sender);
        /// <summary>
        /// Notifies the solution closed.
        /// </summary>
        /// <param name="sender">The sender.</param>
        void NotifySolutionClosed(object sender);
    }
}
