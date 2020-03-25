using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Deviser.Admin.Config;
using Deviser.Admin.Data;
using Deviser.Core.Common.DomainTypes;
using Deviser.Core.Data.Repositories;
using Microsoft.Extensions.Logging;

namespace Deviser.Modules.ModuleManagement.Services
{
    public class ModuleAdminService : IAdminService<Module>
    {
        private readonly ILogger<ModuleAdminService> _logger;
        private readonly IModuleRepository _moduleRepository;
        private readonly IPropertyRepository _propertyRepository;

        public ModuleAdminService(ILogger<ModuleAdminService> logger,
            IModuleRepository moduleRepository,
            IPropertyRepository propertyRepository)
        {
            _logger = logger;
            _moduleRepository = moduleRepository;
            _propertyRepository = propertyRepository;
        }

        public async Task<PagedResult<Module>> GetAll(int pageNo, int pageSize, string orderByProperties)
        {
            var contentTypes = _moduleRepository.GetModules();
            var skip = (pageNo - 1) * pageSize;
            var total = contentTypes.Count;
            var paging = contentTypes.Skip(skip).Take(pageSize);
            var result = new PagedResult<Module>(paging, pageNo, pageSize, total);
            return await Task.FromResult(result);
        }

        public async Task<Module> GetItem(string itemId)
        {
            var result = _moduleRepository.GetModule(Guid.Parse(itemId));
            return await Task.FromResult(result);
        }

        public async Task<FormResult<Module>> CreateItem(Module item)
        {
            var resultModule = _moduleRepository.Create(item);
            var result = new FormResult<Module>(resultModule);
            return await Task.FromResult(result);
        }

        public async Task<FormResult<Module>> UpdateItem(Module item)
        {
            var resultModule = _moduleRepository.UpdateModule(item);
            var result = new FormResult<Module>(resultModule);
            return await Task.FromResult(result);
        }

        public async Task<Module> DeleteItem(string itemId)
        {
            var contentType = _moduleRepository.GetModule(Guid.Parse(itemId));
            if (contentType == null)
            {
                return await Task.FromResult<Module>(null);
            }

            contentType.IsActive = false;
            var result = _moduleRepository.UpdateModule(contentType);
            return await Task.FromResult(result);
        }

        public async Task<ValidationResult> ValidateModuleName(string moduleName)
        {
            var result = _moduleRepository.GetModule(moduleName) != null ? ValidationResult.Failed(new ValidationError() { Code = "Module available!", Description = "Module already exist" }) : ValidationResult.Success;
            return await Task.FromResult(result);
        }
        
        public List<Property> GetProperties()
        {
            var result = _propertyRepository.GetProperties();
            return result;
        }

        public List<ModuleActionType> GetActionType()
        {
            var result = _moduleRepository.GetModuleActionType();
            return result;
        }
    }
}
