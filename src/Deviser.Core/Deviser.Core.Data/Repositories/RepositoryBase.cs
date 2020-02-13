using Deviser.Core.Data.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;

namespace Deviser.Core.Data.Repositories
{
    public class RepositoryBase : AbstractRepository, IRepositoryBase
    {

        protected readonly DbContextOptions<DeviserDbContext> _dbOptions;
        private readonly ILogger<RepositoryBase> _logger;
        //protected readonly DeviserDbContext context;

        public RepositoryBase(DbContextOptions<DeviserDbContext> dbOptions,
            ILogger<PropertyRepository> logger)
        {
            _dbOptions = dbOptions;
            _logger = _logger;

            //context = container.Resolve<DeviserDbContext>();
            //context.ChangeTracker.AutoDetectChangesEnabled = false;

        }

        public bool IsDatabaseExist()
        {
            try
            {
                using var context = new DeviserDbContext(_dbOptions);
                return context.Database.Exists();
            }
            catch (Exception ex)
            {
                _logger.LogError("Error occured while creating ContentControl", ex);
            }
            return false;
        }

        //public DomainType CreateContentControl<DomainType, DbType>(DomainType contentControl)
        //    where DomainType:class where DbType:class
        //{
        //    try
        //    {
        //        using (var context = new DeviserDbContext(DbOptions))
        //        {
        //            var dbContentControl = Mapper.Map<DbType>(contentControl);
        //            //dbContentControl.Id = Guid.NewGuid();
        //            //dbContentControl.CreatedDate = dbContentControl.LastModifiedDate = DateTime.Now;
        //            var result = context.Add(dbContentControl);
        //            context.SaveChanges();
        //            return Mapper.Map<DomainType>(result);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError("Error occured while creating ContentControl", ex);
        //    }
        //    return null;
        //}

    }
}
