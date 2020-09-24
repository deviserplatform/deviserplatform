using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Deviser.Admin.Config;
using Deviser.Admin.Config.Filters;
using Deviser.Admin.Data;
using Deviser.Admin.Extensions;
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

        public async Task<PagedResult<Module>> GetAll(int pageNo, int pageSize, string orderByProperties, FilterNode filter = null)
        {
            var contentTypes = _moduleRepository.GetModules();
            if (filter != null)
            {
                contentTypes = contentTypes.ApplyFilter(filter).ToList();
            }
            var result = new PagedResult<Module>(contentTypes, pageNo, pageSize, orderByProperties);
            return await Task.FromResult(result);
        }

        public async Task<Module> GetItem(string itemId)
        {
            var result = _moduleRepository.GetModule(Guid.Parse(itemId));
            return await Task.FromResult(result);
        }

        public async Task<IFormResult<Module>> CreateItem(Module item)
        {
            var resultModule = _moduleRepository.Create(item);
            var result = new FormResult<Module>(resultModule)
            {
                IsSucceeded = true,
                SuccessMessage = "Module has been created"
            };
            return await Task.FromResult(result);
        }

        public async Task<IFormResult<Module>> UpdateItem(Module item)
        {
            var resultModule = _moduleRepository.UpdateModule(item);
            var result = new FormResult<Module>(resultModule)
            {
                IsSucceeded = true,
                SuccessMessage = "Module has been updated"
            };
            return await Task.FromResult(result);
        }

        public async Task<IAdminResult<Module>> DeleteItem(string itemId)
        {
            var contentType = _moduleRepository.GetModule(Guid.Parse(itemId));
            if (contentType == null)
            {
                return await Task.FromResult<AdminResult<Module>>(new AdminResult<Module>()
                {
                    IsSucceeded = false,
                    ErrorMessage = "Unable to delete Module"
                });
            }

            contentType.IsActive = false;
            contentType = _moduleRepository.UpdateModule(contentType);
            var result = new FormResult<Module>(contentType)
            {
                IsSucceeded = true,
                SuccessMessage = "Module has been deleted"
            };
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

        public List<ModuleViewType> GetActionType()
        {
            var result = _moduleRepository.GetModuleViewType();
            return result;
        }
    }
}
