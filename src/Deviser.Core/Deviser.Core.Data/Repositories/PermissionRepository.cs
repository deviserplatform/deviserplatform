using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;
using Deviser.Core.Common.DomainTypes;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Deviser.Core.Data.Repositories
{
    public interface IPermissionRepository
    {
        IList<Permission> GetPagePermissions();
        IList<Permission> GerPermissions();
    }

    public class PermissionRepository : IPermissionRepository
    {
        private readonly ILogger<PermissionRepository> _logger;
        private readonly DbContextOptions<DeviserDbContext> _dbOptions;
        private readonly IMapper _mapper;

        public PermissionRepository(DbContextOptions<DeviserDbContext> dbOptions,
            ILogger<PermissionRepository> logger,
            IMapper mapper)
        {
            _logger = logger;
            _dbOptions = dbOptions;
            _mapper = mapper;
        }
        
        public IList<Permission> GetPagePermissions()
        {
            try
            {
                using var context = new DeviserDbContext(_dbOptions);
                var result = context.Permission
                    .Where(p => p.Entity == "PAGE")
                    .OrderBy(r => r.Name)
                    .ToList();
                return _mapper.Map<List<Permission>>(result);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error occured while PAGE Permissions", ex);
            }
            return null;
        }

        public IList<Permission> GerPermissions()
        {
            try
            {
                using var context = new DeviserDbContext(_dbOptions);
                var result = context.Permission
                    .Include(p=>p.ContentPermissions)
                    .Include(p => p.ModulePermissions)
                    .Include(p => p.PagePermissions)
                    .OrderBy(r => r.Name)
                    .ToList();
                return _mapper.Map<List<Permission>>(result);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error occured while PAGE Permissions", ex);
            }
            return null;
        }
    }
}
