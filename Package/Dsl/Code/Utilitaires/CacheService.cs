using System;
using System.Collections.Generic;
using DSLFactory.Candle.SystemModel.Configuration;

namespace DSLFactory.Candle.SystemModel.Utilities
{
    internal class BasicCache : ICacheProvider
    {
        private readonly Dictionary<string, CacheItem> _cache = new Dictionary<string, CacheItem>();

        #region ICacheProvider Members

        /// <summary>
        /// Stocke un objet dans le cache
        /// </summary>
        /// <param name="key"></param>
        /// <param name="obj"></param>
        public void Put(string key, object obj)
        {
            // Si 0, on ne gére pas le cache
            if (CandleSettings.RepositoryDelaiCache == 0)
                return;

            CacheItem ci = new CacheItem();
            ci.TimeStamp = DateTime.Now;
            ci.Value = obj;
            _cache[key] = ci;
        }

        /// <summary>
        /// Gets the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public object Get(string key)
        {
            if (CandleSettings.RepositoryDelaiCache == 0)
                return null;

            CacheItem ci = _cache[key];
            if (ci != null)
            {
                if (!CandleSettings.CacheExpired(DateTime.Now))
                {
                    return ci.Value;
                }
            }
            return null;
        }

        /// <summary>
        /// Supprime un objet du cache
        /// </summary>
        /// <param name="key"></param>
        public void Clear(string key)
        {
            if (_cache.ContainsKey(key))
                _cache.Remove(key);
        }

        /// <summary>
        /// Clears all the cache
        /// </summary>
        public void Clear()
        {
            _cache.Clear();
        }

        #endregion

        #region Nested type: CacheItem

        private class CacheItem
        {
            public DateTime TimeStamp;
            public object Value;
        }

        #endregion
    }
}