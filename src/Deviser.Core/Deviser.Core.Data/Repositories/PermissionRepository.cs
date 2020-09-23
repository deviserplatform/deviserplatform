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
            using var context = new DeviserDbContext(_dbOptions);
            var result = context.Permission
                .Where(p => p.Entity == "PAGE")
                .OrderBy(r => r.Name)
                .ToList();
            return _mapper.Map<List<Permission>>(result);
        }

        public IList<Permission> GerPermissions()
        {
            using var context = new DeviserDbContext(_dbOptions);
            var result = context.Permission
                .Include(p => p.ContentPermissions)
                .Include(p => p.ModulePermissions)
                .Include(p => p.PagePermissions)
                .OrderBy(r => r.Name)
                .ToList();
            return _mapper.Map<List<Permission>>(result);
        }
    }
}
