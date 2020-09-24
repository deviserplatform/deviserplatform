using System;
using System.Collections.Concurrent;

namespace Deviser.Core.Data.Repositories
{
    public abstract class AbstractRepository
    {
        protected static ConcurrentDictionary<string, Object> repoCache = new ConcurrentDictionary<string, object>();

        protected T GetResultFromCache<T>(string cacheName)
            where T : class
        {
            if (repoCache.ContainsKey(cacheName) && repoCache.TryGetValue(cacheName, out var objResult) && objResult != null)
            {
                return (T)objResult;
            }
            return null;
        }

        protected void AddResultToCache(string cacheName, object value)
        {
            repoCache.TryAdd(cacheName, value);
        }
    }
}
