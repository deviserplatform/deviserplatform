using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Deviser.Admin.Config;
using Deviser.Admin.Data;
using Deviser.Core.Common.DomainTypes;
using Deviser.Core.Data.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Deviser.Modules.ContentManagement.Services
{
    public class LayoutTypeAdminService : IAdminService<LayoutType>
    {
        private readonly ILogger<LayoutTypeAdminService> _logger;
        private readonly ILayoutTypeRepository _layoutTypeRepository;
        private readonly IPropertyRepository _propertyRepository;

        public LayoutTypeAdminService(ILogger<LayoutTypeAdminService> logger,
            ILayoutTypeRepository layoutTypeRepository,
            IPropertyRepository propertyRepository)
        {
            _logger = logger;
            _layoutTypeRepository = layoutTypeRepository;
            _propertyRepository = propertyRepository;
        }

        public async Task<PagedResult<LayoutType>> GetAll(int pageNo, int pageSize, string orderByProperties)
        {
            var layoutTypes = GetLayoutTypes();

            var skip = (pageNo - 1) * pageSize;
            var total = layoutTypes.Count;
            var paging = layoutTypes.Skip(skip).Take(pageSize);
            var result = new PagedResult<LayoutType>(paging, pageNo, pageSize, total);
            return await Task.FromResult(result);
        }

        public List<LayoutType> GetLayoutTypes()
        {
            var layoutTypes = _layoutTypeRepository.GetLayoutTypes();
            return layoutTypes;
        }


        public async Task<LayoutType> GetItem(string layoutTypeId)
        {
            var result = _layoutTypeRepository.GetLayoutType(Guid.Parse(layoutTypeId));

            ParseLayoutTypeIds(result);

            return await Task.FromResult(result);
        }

        private void ParseLayoutTypeIds(LayoutType result)
        {
            if (string.IsNullOrEmpty(result.LayoutTypeIds)) return;

            var layoutTypes = _layoutTypeRepository.GetLayoutTypes();
            var layoutTypeIds = result.LayoutTypeIds.Split(",").Select(li => Guid.Parse(li)).ToList();
            result.AllowedLayoutTypes = layoutTypes.Where(lt => layoutTypeIds.Contains(lt.Id)).ToList();
        }

        public async Task<LayoutType> CreateItem(LayoutType layoutType)
        {
            ParseAllowedLayoutTypes(layoutType);
            var result = _layoutTypeRepository.CreateLayoutType(layoutType);
            ParseLayoutTypeIds(result);
            return await Task.FromResult(result);
        }

        private static void ParseAllowedLayoutTypes(LayoutType layoutType)
        {
            if (layoutType.AllowedLayoutTypes != null && layoutType.AllowedLayoutTypes.Count > 0)
            {
                layoutType.LayoutTypeIds = string.Join(",",
                    layoutType.AllowedLayoutTypes.Select(lt => lt.Id.ToString()).ToArray());
            }
        }

        public async Task<LayoutType> UpdateItem(LayoutType layoutType)
        {
            ParseAllowedLayoutTypes(layoutType);
            var result = _layoutTypeRepository.UpdateLayoutType(layoutType);
            ParseLayoutTypeIds(result);
            return await Task.FromResult(result);
        }

        public async Task<LayoutType> DeleteItem(string layoutTypeId)
        {
            var layoutType = _layoutTypeRepository.GetLayoutType(Guid.Parse(layoutTypeId));
            if (layoutType == null)
            {
                return await Task.FromResult<LayoutType>(null);
            }

            layoutType.IsActive = false;
            var result = _layoutTypeRepository.UpdateLayoutType(layoutType);
            return await Task.FromResult(result);
        }

        public async Task<ValidationResult> ValidateLayoutTypeName(string layoutTypeName)
        {
            var result = _layoutTypeRepository.GetLayoutType(layoutTypeName) != null ? ValidationResult.Failed(new ValidationError() { Code = "LayoutType available!", Description = "LayoutType already exist" }) : ValidationResult.Success;
            return await Task.FromResult(result);
        }

        public List<Property> GetProperties()
        {
            var result = _propertyRepository.GetProperties();
            return result;
        }
    }
}
