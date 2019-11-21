using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Deviser.Admin.Config
{
    public class EntityConfig
    {
        public IKey PrimaryKey { get; }
        public IEnumerable<INavigation> Navigations { get; }
        public IEnumerable<IForeignKey> ForeignKeys { get; }
        public Type DbContextType { get; }

        public IEntityType EntityType { get; }

        public EntityConfig(Type dbContextType, IEntityType entityType)
        {
            EntityType = entityType;
            DbContextType = dbContextType;
            PrimaryKey = entityType.FindPrimaryKey();
            ForeignKeys = entityType.GetForeignKeys();
            Navigations = entityType.GetNavigations();
        }
    }
}
