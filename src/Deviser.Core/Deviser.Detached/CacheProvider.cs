using System;
using System.Collections.Concurrent;
using System.Linq;


namespace Deviser.Detached
{
    internal interface ICacheProvider<T>
    {
        void Insert(string key, T value);
        void Clear();
        bool TryGet(string key, out T element);
        T GetOrAdd(string key, T element);
    }

    internal class CacheProvider<T> : ICacheProvider<T>
        where T : class
    {
        private static readonly ConcurrentDictionary<string, T> _openHandlerCache =
           new ConcurrentDictionary<string, T>();

        public void Insert(string key, T value)
        {
            _openHandlerCache.AddOrUpdate(key, value, (k, oldvalue) => oldvalue);
        }

        public void Clear()
        {
            _openHandlerCache.Clear();
        }

        public bool TryGet(string key, out T element)
        {
            return _openHandlerCache.TryGetValue(key, out element);
        }

        public T GetOrAdd(string key, T element)
        {
            return _openHandlerCache.GetOrAdd(key, element);
        }
    }
}
