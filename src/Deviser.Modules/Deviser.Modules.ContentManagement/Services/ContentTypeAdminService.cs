using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Deviser.Admin.Config;
using Deviser.Admin.Data;
using Deviser.Core.Common.DomainTypes;
using Deviser.Core.Data.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Deviser.Modules.ContentManagement.Services
{
    public class ContentTypeAdminService : IAdminService<ContentType>
    {
        private readonly ILogger<ContentTypeAdminService> _logger;
        private readonly IContentTypeRepository _contentTypeRepository;
        private readonly IPropertyRepository _propertyRepository;

        public ContentTypeAdminService(ILogger<ContentTypeAdminService> logger,
            IContentTypeRepository contentTypeRepository,
            IPropertyRepository propertyRepository)
        {
            _logger = logger;
            _contentTypeRepository = contentTypeRepository;
            _propertyRepository = propertyRepository;
        }

        public async Task<PagedResult<ContentType>> GetAll(int pageNo, int pageSize, string orderByProperties)
        {
            var contentTypes = _contentTypeRepository.GetContentTypes();
            var skip = (pageNo - 1) * pageSize;
            var total = contentTypes.Count;
            var paging = contentTypes.Skip(skip).Take(pageSize);
            var result = new PagedResult<ContentType>(paging, pageNo, pageSize, total);
            return await Task.FromResult(result);
        }

        public async Task<ContentType> GetItem(string contentTypeId)
        {
            var result = _contentTypeRepository.GetContentType(Guid.Parse(contentTypeId));
            return await Task.FromResult(result);
        }

        public async Task<ContentType> CreateItem(ContentType contentType)
        {
            var result = _contentTypeRepository.CreateContentType(contentType);
            return await Task.FromResult(result);
        }

        public async Task<ContentType> UpdateItem(ContentType contentType)
        {
            var result = _contentTypeRepository.UpdateContentType(contentType);
            return await Task.FromResult(result);
        }

        public async Task<ContentType> DeleteItem(string contentTypeId)
        {
            var contentType = _contentTypeRepository.GetContentType(Guid.Parse(contentTypeId));
            if (contentType == null)
            {
                return await Task.FromResult<ContentType>(null);
            }

            contentType.IsActive = false;
            var result = _contentTypeRepository.UpdateContentType(contentType);
            return await Task.FromResult(result);
        }

        public async Task<ValidationResult> ValidateContentTypeName(string contentTypeName)
        {
            var result = _contentTypeRepository.GetContentType(contentTypeName) != null ? ValidationResult.Failed(new ValidationError(){Code = "ContentType Exist!", Description = "LayoutType already exist" }) : ValidationResult.Success;
            return await Task.FromResult(result);
        }

        public List<Property> GetProperties()
        {
            var result = _propertyRepository.GetProperties();
            return result;
        }
    }
}
