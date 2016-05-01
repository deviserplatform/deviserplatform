using Autofac;
using Deviser.Core.Data.Entities;
using Microsoft.Data.Entity.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
