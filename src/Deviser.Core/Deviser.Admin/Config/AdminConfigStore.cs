using System;
using System.Collections.Concurrent;

namespace Deviser.Admin.Config
{
    public class AdminConfigStore : IAdminConfigStore        
    {
        private static readonly ConcurrentDictionary<Type, object> entityConfiguration =
          new ConcurrentDictionary<Type, object>();

        public AdminConfigStore()
        {

        }

        public object GetOrAdd(Type type, object element)
        {
            return entityConfiguration.GetOrAdd(type, element);
        }

        public bool TryGet(Type type, out object element)
        {   
            var result = entityConfiguration.TryGetValue(type, out element);
            return result;
        }

        //public AdminConfig<TEntity> GetOrAdd(Type type, AdminConfig<TEntity> element)
        //    where TEntity : class
        //{
        //    return entityConfiguration.GetOrAdd(type, element);
        //}

        //public bool TryGet(Type type, out AdminConfig<TEntity> element)
        //{
        //    return entityConfiguration.TryGetValue(type, out element);
        //}
    }
}
