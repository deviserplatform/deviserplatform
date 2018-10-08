using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Deviser.Admin.Config
{
    public class EntityConfig
    {
        public IKey PrimaryKey { get; set; }        
        public IEnumerable<INavigation> Navigations { get; set; }
        public IEnumerable<IForeignKey> ForeignKeys { get; set; }
        public Type DbContextType { get; set; }
    }
}
