namespace DSLFactory.Candle.SystemModel.VisualStudio
{
    /// <summary>
    /// 
    /// </summary>
    class DialogService : IDialogService
    {
        #region IDialogService Members

        /// <summary>
        /// Creates the publish model form.
        /// </summary>
        /// <param name="serverAvailable">if set to <c>true</c> [server available].</param>
        /// <returns>
        /// null si la fenetre n'a pas de sens dans le contexte d'execution (batch)
        /// </returns>
        public DSLFactory.Candle.SystemModel.Repository.PublishModelForm CreatePublishModelForm(bool serverAvailable)
        {
            return new DSLFactory.Candle.SystemModel.Repository.PublishModelForm(serverAvailable);
        }

        #endregion
    }
}
