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

namespace Deviser.Modules.ContentManagement.Services
{
    public class PropertyAdminService : IAdminService<Property>
    {
        private readonly ILogger<PropertyAdminService> _logger;
        private readonly IOptionListRepository _optionListRepository;
        private readonly IPropertyRepository _propertyRepository;

        public PropertyAdminService(ILogger<PropertyAdminService> logger,
            IOptionListRepository optionListRepository,
            IPropertyRepository propertyRepository)
        {
            _logger = logger;
            _optionListRepository = optionListRepository;
            _propertyRepository = propertyRepository;
        }

        public async Task<PagedResult<Property>> GetAll(int pageNo, int pageSize, string orderByProperties, FilterNode filter = null)
        {
            var properties = _propertyRepository.GetProperties();
            if (filter != null)
            {
                properties = properties.ApplyFilter(filter).ToList();
            }
            var result = new PagedResult<Property>(properties, pageNo, pageSize, orderByProperties);
            return await Task.FromResult(result);
        }

        public async Task<Property> GetItem(string itemId)
        {
            var result = _propertyRepository.GetProperty(Guid.Parse(itemId));
            ParseResult(result);
            return await Task.FromResult(result);
        }

        public async Task<IFormResult<Property>> CreateItem(Property item)
        {
            ParseProperty(item);
            var resultProperty = _propertyRepository.CreateProperty(item);
            ParseResult(resultProperty);
            if (resultProperty == null)
                return new FormResult<Property>()
                {
                    IsSucceeded = false,
                    ErrorMessage = "Unable to create the Property"
                };

            var result = new FormResult<Property>(resultProperty)
            {
                IsSucceeded = true,
                SuccessMessage = "Property has been created successfully"
            };
            return await Task.FromResult(result);
        }

        public async Task<IFormResult<Property>> UpdateItem(Property item)
        {
            ParseProperty(item);
            var resultProperty = _propertyRepository.UpdateProperty(item);
            ParseResult(resultProperty);
            if (resultProperty == null)
                return new FormResult<Property>()
                {
                    IsSucceeded = false,
                    ErrorMessage = "Unable to update the Property"
                };

            var result = new FormResult<Property>(resultProperty)
            {
                IsSucceeded = true,
                SuccessMessage = "Property has been saved successfully"
            };
            return await Task.FromResult(result);
        }

        public async Task<IAdminResult<Property>> DeleteItem(string itemId)
        {
            var property = _propertyRepository.GetProperty(Guid.Parse(itemId));
            if (property == null)
            {
                return new FormResult<Property>()
                {
                    IsSucceeded = false,
                    ErrorMessage = "Unable to delete the Property"
                };
            }

            property.IsActive = false;
            property = _propertyRepository.UpdateProperty(property);
            var result = new FormResult<Property>(property)
            {
                IsSucceeded = true,
                SuccessMessage = "Property has been deleted successfully"
            };
            return await Task.FromResult(result);
        }

        public IList<OptionList> GetOptionList()
        {
            var result = _optionListRepository.GetOptionLists();
            return result;
        }

        public IList<PropertyOption> GetPropertyOption(OptionList optionList)
        {
            var result = _optionListRepository.GetOptionList(optionList.Id);
            return result?.List;
        }

        public async Task<ValidationResult> ValidatePropertyName(string propertyName)
        {
            var result = _propertyRepository.IsPropertyExist(propertyName) ? ValidationResult.Failed(new ValidationError() { Code = "Property available!", Description = "Property already exist" }) : ValidationResult.Success;
            return await Task.FromResult(result);
        }

        private static void ParseResult(Property result)
        {
            if (!string.IsNullOrEmpty(result.DefaultValue))
            {
                result.DefaultValuePropertyOption =
                    result.OptionList?.List?.FirstOrDefault(li => li.Id == Guid.Parse(result.DefaultValue));
            }
        }

        private static void ParseProperty(Property item)
        {
            item.OptionListId = item.OptionList?.Id;
            item.OptionList = null;
            if (item.DefaultValuePropertyOption != null)
            {
                item.DefaultValue = item.DefaultValuePropertyOption.Id.ToString();
            }
        }
    }
}
