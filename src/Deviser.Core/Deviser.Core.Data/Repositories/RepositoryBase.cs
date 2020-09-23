using Deviser.Core.Data.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;

namespace Deviser.Core.Data.Repositories
{
    public class RepositoryBase : AbstractRepository, IRepositoryBase
    {

        protected readonly DbContextOptions<DeviserDbContext> _dbOptions;

        public RepositoryBase(DbContextOptions<DeviserDbContext> dbOptions)
        {
            _dbOptions = dbOptions;
        }

        public bool IsDatabaseExist()
        {
            using var context = new DeviserDbContext(_dbOptions);
            return context.Database.Exists();
        }
    }
}
