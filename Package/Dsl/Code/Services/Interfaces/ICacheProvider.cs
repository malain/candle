namespace DSLFactory.Candle.SystemModel
{
    /// <summary>
    /// Service de cache
    /// </summary>
    public interface ICacheProvider
    {
        /// <summary>
        /// Puts the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="obj">The obj.</param>
        void Put(string key, object obj);
        /// <summary>
        /// Gets the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        object Get(string key);
        /// <summary>
        /// Clears the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        void Clear(string key);
        /// <summary>
        /// Clears all the cache
        /// </summary>
        void Clear();
    }
}
