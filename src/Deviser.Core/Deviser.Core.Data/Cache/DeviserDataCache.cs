using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace Deviser.Core.Data.Cache
{
    public interface IDeviserDataCache
    {
        void AddOrUpdate<T>(string key, T value);
        T GetItem<T>(string key);
        bool ContainsKey(string key);
    }

    public class DeviserDataCache : IDeviserDataCache
    {
        private readonly ConcurrentDictionary<string, object> _cacheDictionary;

        public DeviserDataCache()
        {
            _cacheDictionary = new ConcurrentDictionary<string, object>();
        }

        public void AddOrUpdate<T>(string key, T value)
        {
            _cacheDictionary.AddOrUpdate(key, value, (s, o) => value);
        }

        public T GetItem<T>(string key)
        {
            if (_cacheDictionary.TryGetValue(key, out var value))
            {
                return (T)value;
            }

            return default(T);
        }

        public bool ContainsKey(string key)
        {
            return _cacheDictionary.ContainsKey(key);
        }

    }
}
