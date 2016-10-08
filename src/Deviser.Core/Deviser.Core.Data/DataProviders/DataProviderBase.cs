using Autofac;
using Deviser.Core.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Deviser.Core.Data.DataProviders
{
    public class DataProviderBase
    {

        protected DbContextOptions<DeviserDbContext> DbOptions;
        protected ILifetimeScope Container;

        public DataProviderBase(ILifetimeScope container)
        {
            this.Container = container;
            DbOptions = container.Resolve<DbContextOptions<DeviserDbContext>>();
            
        }

    }
}
