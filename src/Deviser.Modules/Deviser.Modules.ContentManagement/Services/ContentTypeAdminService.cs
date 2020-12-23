using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Deviser.Admin.Config;
using Deviser.Admin.Config.Filters;
using Deviser.Admin.Data;
using Deviser.Admin.Extensions;
using Deviser.Admin.Services;
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

        public async Task<PagedResult<ContentType>> GetAll(int pageNo, int pageSize, string orderByProperties, FilterNode filter = null)
        {
            var contentTypes = _contentTypeRepository.GetContentTypes();

            if (filter != null)
            {
                contentTypes = contentTypes.ApplyFilter(filter).ToList();
            }
            var result = new PagedResult<ContentType>(contentTypes, pageNo, pageSize, orderByProperties);
            return await Task.FromResult(result);
        }

        public async Task<ContentType> GetItem(string contentTypeId)
        {
            var result = _contentTypeRepository.GetContentType(Guid.Parse(contentTypeId));
            return await Task.FromResult(result);
        }

        public async Task<IFormResult<ContentType>> CreateItem(ContentType contentType)
        {
            ParseContentTypeField(contentType);
            contentType = _contentTypeRepository.CreateContentType(contentType);
            if (contentType == null)
                return new FormResult<ContentType>()
                {
                    IsSucceeded = false,
                    ErrorMessage = "Unable to create the ContentType"
                };

            var result = new FormResult<ContentType>(contentType)
            {
                IsSucceeded = true,
                SuccessMessage = "ContentType has been created"
            };
            return await Task.FromResult(result);
        }

        public async Task<IFormResult<ContentType>> UpdateItem(ContentType contentType)
        {
            ParseContentTypeField(contentType);
            contentType = _contentTypeRepository.UpdateContentType(contentType);
            if (contentType == null)
                return new FormResult<ContentType>()
                {
                    IsSucceeded = false,
                    ErrorMessage = "Unable to update the ContentType"
                };

            var result = new FormResult<ContentType>(contentType)
            {
                IsSucceeded = true,
                SuccessMessage = "ContentType has been updated"
            };
            return await Task.FromResult(result);
        }

        public async Task<IAdminResult<ContentType>> DeleteItem(string contentTypeId)
        {
            var contentType = _contentTypeRepository.GetContentType(Guid.Parse(contentTypeId));
            if (contentType == null)
            {
                return await Task.FromResult<AdminResult<ContentType>>(null);
            }

            contentType.IsActive = false;
            contentType = _contentTypeRepository.UpdateContentType(contentType);
            if (contentType == null)
                return new FormResult<ContentType>()
                {
                    IsSucceeded = false,
                    ErrorMessage = "Unable to delete the ContentType"
                };

            var result = new AdminResult<ContentType>(contentType)
            {
                IsSucceeded = true,
                SuccessMessage = "ContentType has been deleted"
            };
            return await Task.FromResult(result);
        }

        public async Task<PagedResult<ContentTypeField>> SortContentType(int pageNo, int pageSize, IList<ContentTypeField> contentTypeFields)
        {
            var sortedContentTypeFields = await _contentTypeRepository.SortContentTypeFields(contentTypeFields);
            var result = new PagedResult<ContentTypeField>(sortedContentTypeFields, pageNo, pageSize);
            return result;
        }

        public async Task<ValidationResult> ValidateContentTypeName(string contentTypeName)
        {
            var result = _contentTypeRepository.GetContentType(contentTypeName) != null ? ValidationResult.Failed(new ValidationError() { Code = "ContentType available!", Description = "ContentType already exist" }) : ValidationResult.Success;
            return await Task.FromResult(result);
        }

        public List<Property> GetProperties()
        {
            var result = _propertyRepository.GetProperties();
            return result;
        }

        private static void ParseContentTypeField(ContentType contentType)
        {
            if (contentType.ContentTypeFields != null)
            {
                foreach (var ctf in contentType.ContentTypeFields)
                {
                    ctf.ContentFieldTypeId = ctf.ContentFieldType.Id;
                    ctf.ContentFieldType = null;
                }
            }
        }
    }
}
