using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Deviser.Detached
{
    public interface IEntityManager
    {
        bool AreKeysIdentical(object newValue, object dbValue);
        object CreateEmptyEntityWithKey(object entity);
        IKey GetPrimaryKey(Type clrType);
        IKey GetPrimaryKey(object entity);
        EntityKey GetEntityKey(object instance);
        IEnumerable<PropertyInfo> GetPrimaryKeyFieldsFor(object entity);
        IEntityType GetEntityType(Type clrType);
        IEnumerable<INavigation> GetRequiredNavigationPropertiesForType(Type clrType);
        TEntity LoadPersisted<TEntity>(TEntity entity, List<string> includeStringPaths)
            where TEntity : class;

    }
}