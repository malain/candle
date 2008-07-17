using System.Collections.Generic;

namespace DSLFactory.Candle.SystemModel.Wizard
{
    /// <summary>
    /// 
    /// </summary>
    public enum ButtonType
    {
        /// <summary>
        /// 
        /// </summary>
        Next,
        /// <summary>
        /// 
        /// </summary>
        Previous,
        /// <summary>
        /// 
        /// </summary>
        Finish,
        /// <summary>
        /// 
        /// </summary>
        Cancel
    }

    /// <summary>
    /// 
    /// </summary>
    public class WizardFinishedEventArgs
    {
        /// <summary>
        /// 
        /// </summary>
        public Dictionary<string, object> UserData = new Dictionary<string, object>();

        /// <summary>
        /// Initializes a new instance of the <see cref="WizardFinishedEventArgs"/> class.
        /// </summary>
        /// <param name="userData">The user data.</param>
        internal WizardFinishedEventArgs(Dictionary<string, object> userData)
        {
            UserData = userData;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public delegate void WizardFinishedHandler(object sender, WizardFinishedEventArgs e);
}