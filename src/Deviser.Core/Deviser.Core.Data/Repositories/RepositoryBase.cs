using Autofac;
using AutoMapper;
using Deviser.Core.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using Deviser.Core.Data.Extensions;

namespace Deviser.Core.Data.Repositories
{
    public class RepositoryBase : IRepositoryBase
    {

        protected readonly DbContextOptions<DeviserDbContext> DbOptions;
        protected readonly ILifetimeScope Container;
        private readonly ILogger<RepositoryBase> _logger;
        //protected readonly DeviserDbContext context;

        public RepositoryBase(ILifetimeScope container)
        {
            this.Container = container;
            DbOptions = container.Resolve<DbContextOptions<DeviserDbContext>>();
            _logger = container.Resolve<ILogger<RepositoryBase>>();

            //context = container.Resolve<DeviserDbContext>();
            //context.ChangeTracker.AutoDetectChangesEnabled = false;

        }

        public bool IsDatabaseExist()
        {
            try
            {
                using (var context = new DeviserDbContext(DbOptions))
                {
                    return context.Database.Exists();
                }
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
