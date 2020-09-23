using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Deviser.Core.Common.DomainTypes;

namespace Deviser.Core.Data.Repositories
{
    public interface IContentTypeRepository
    {
        List<ContentType> GetContentTypes();
        List<ContentFieldType> GetContentFieldTypes();
        ContentType GetContentType(Guid contentTypeId);
        ContentType CreateContentType(ContentType dbContentType);
        ContentType GetContentType(string contentTypeName);
        ContentType UpdateContentType(ContentType dbContentType);
        Task<IList<ContentTypeField>> SortContentTypeFields(IList<ContentTypeField> contentTypeFields);
    }
}