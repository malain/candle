using System;

namespace DSLFactory.Candle.SystemModel
{
    /// <summary>
    /// 
    /// </summary>
    class CandleNotifier : ICandleNotifier
    {
        #region ICandleNotifier Members

        /// <summary>
        /// Occurs when [the candle options changed].
        /// </summary>
        public event EventHandler OptionsChanged;
        /// <summary>
        /// Occurs when [solution opened].
        /// </summary>
        public event EventHandler SolutionOpened;
        /// <summary>
        /// Occurs when [solution closed].
        /// </summary>
        public event EventHandler SolutionClosed;

        /// <summary>
        /// Notifies the options changed.
        /// </summary>
        /// <param name="sender">The sender.</param>
        public void NotifyOptionsChanged(object sender)
        {
            if (OptionsChanged != null)
                OptionsChanged(sender, new EventArgs());
        }

        /// <summary>
        /// Notifies the solution opened.
        /// </summary>
        /// <param name="sender">The sender.</param>
        public void NotifySolutionOpened(object sender)
        {
            if (SolutionOpened != null)
                SolutionOpened(sender, new EventArgs());
        }

        /// <summary>
        /// Notifies the solution closed.
        /// </summary>
        /// <param name="sender">The sender.</param>
        public void NotifySolutionClosed(object sender)
        {
            if (SolutionClosed != null)
                SolutionClosed(sender, new EventArgs());
        }

        #endregion
    }
}
