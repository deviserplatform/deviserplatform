using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace Deviser.Admin.Config
{
    public class AdminConfigStore<TEntity> : IAdminConfigStore<TEntity>
        where TEntity : class
    {
        private static readonly ConcurrentDictionary<Type, AdminConfig<TEntity>> entityConfiguration =
          new ConcurrentDictionary<Type, AdminConfig<TEntity>>();

        public AdminConfigStore()
        {

        }

        public AdminConfig<TEntity> GetOrAdd(Type type, AdminConfig<TEntity> element)
        {
            return entityConfiguration.GetOrAdd(type, element);
        }

        public bool TryGet(Type type, out AdminConfig<TEntity> element)
        {
            return entityConfiguration.TryGetValue(type, out element);
        }
    }
}
