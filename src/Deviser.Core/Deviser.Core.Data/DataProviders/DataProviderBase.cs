using Autofac;
using Deviser.Core.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Deviser.Core.Data.DataProviders
{
    public class DataProviderBase
    {

        protected DbContextOptions<DeviserDBContext> dbOptions;
        protected ILifetimeScope container;

        public DataProviderBase(ILifetimeScope container)
        {
            this.container = container;
            dbOptions = container.Resolve<DbContextOptions<DeviserDBContext>>();
            
        }

    }
}
