namespace DSLFactory.Candle.SystemModel.Commands
{
    /// <summary>
    /// Interface pour les commandes
    /// </summary>
    interface ICommand
    {
        /// <summary>
        /// Gets a value indicating whether this <see cref="ICommand"/> is enabled.
        /// </summary>
        /// <value><c>true</c> if enabled; otherwise, <c>false</c>.</value>
        bool Enabled { get;}
        /// <summary>
        /// Gets a value indicating whether this <see cref="ICommand"/> is visible.
        /// </summary>
        /// <value><c>true</c> if visible; otherwise, <c>false</c>.</value>
        bool Visible();
        /// <summary>
        /// Execute the command
        /// </summary>
        void Exec();
    }
}
