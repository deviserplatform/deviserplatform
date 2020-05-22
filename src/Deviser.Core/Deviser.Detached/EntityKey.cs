using System.Collections.Generic;
using System.Linq;

namespace Deviser.Detached
{
    public class EntityKey
    {
        Dictionary<string, object> keyValue;

        private int _hashCode; // computed as needed

        public EntityKey()
        {
            keyValue = new Dictionary<string, object>();
        }

        public void AddKeyValue(string key, object value)
        {
            keyValue.Add(key, value);
        }

        public object GetValue(string key)
        {
            return keyValue[key];
        }

        public bool TryGetValue(string key, out object value)
        {            
            return keyValue.TryGetValue(key, out value);
        }
        
        public List<object> GetAllValues()
        {
            return keyValue.Values.ToList();
        }

        public override bool Equals(object obj)
        {
            return InternalEquals(this, obj as EntityKey);
        }

        public override int GetHashCode()
        {
            var hash = 17;
            foreach (var value in keyValue.Values)
            {
                unchecked
                {
                    hash = hash * 31 + value.GetHashCode();
                }
            }
            return hash;
        }

        public static bool operator ==(EntityKey key1, EntityKey key2)
        {
            return InternalEquals(key1, key2);
        }

        public static bool operator !=(EntityKey key1, EntityKey key2)
        {
            return !InternalEquals(key1, key2);
        }

        internal static bool InternalEquals(EntityKey key1, EntityKey key2)
        {
            if (ReferenceEquals(key1, key2))
            {
                return true;
            }

            var result = (key1.keyValue.Count == key1.keyValue.Count && !key1.keyValue.Except(key2.keyValue).Any());

            return result;
        }
    }
}
