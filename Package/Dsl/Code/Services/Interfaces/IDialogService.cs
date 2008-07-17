namespace DSLFactory.Candle.SystemModel
{
    /// <summary>
    /// Service permettant de créer la fenetre de confirmation de la publication.
    /// </summary>
    interface IDialogService
    {
        /// <summary>
        /// Creates the publish model form.
        /// </summary>
        /// <param name="serverAvailable">if set to <c>true</c> [server available].</param>
        /// <returns>null si la fenetre n'a pas de sens dans le contexte d'execution (batch)</returns>
        DSLFactory.Candle.SystemModel.Repository.PublishModelForm CreatePublishModelForm(bool serverAvailable);
    }
}
