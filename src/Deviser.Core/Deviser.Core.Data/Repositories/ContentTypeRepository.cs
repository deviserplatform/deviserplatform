using AutoMapper;
using Deviser.Core.Common.DomainTypes;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Deviser.Core.Data.Repositories
{
    /// <summary>
    /// Data provider for both ContentTypes and ContentDataType
    /// </summary>
    public class ContentTypeRepository : IContentTypeRepository
    {
        //Logger
        private readonly ILogger<ContentTypeRepository> _logger;
        private readonly DbContextOptions<DeviserDbContext> _dbOptions;
        private readonly IMapper _mapper;


        public ContentTypeRepository(DbContextOptions<DeviserDbContext> dbOptions,
            ILogger<ContentTypeRepository> logger,
            IMapper mapper)
        {
            _logger = logger;
            _dbOptions = dbOptions;
            _mapper = mapper;
        }

        /// <summary>
        /// It creates new ContentType
        /// </summary>
        /// <param name="contentType"></param>
        /// <returns></returns>
        public ContentType CreateContentType(ContentType contentType)
        {
            using var context = new DeviserDbContext(_dbOptions);
            var dbContentType = _mapper.Map<Entities.ContentType>(contentType);
            dbContentType.Id = Guid.NewGuid();
            if (dbContentType.ContentTypeProperties != null && dbContentType.ContentTypeProperties.Count > 0)
            {
                foreach (var ctp in dbContentType.ContentTypeProperties)
                {
                    ctp.Property = null;
                    ctp.ContentTypeId = dbContentType.Id;
                }
            }
            dbContentType.CreatedDate = dbContentType.LastModifiedDate = DateTime.Now;
            var result = context.ContentType.Add(dbContentType).Entity;
            context.SaveChanges();
            return _mapper.Map<ContentType>(result);
        }

        public List<ContentFieldType> GetContentFieldTypes()
        {
            using var context = new DeviserDbContext(_dbOptions);
            var result = context.ContentFieldType
                .AsNoTracking()
                .OrderBy(c => c.Name)
                .ToList();
            return _mapper.Map<List<ContentFieldType>>(result);
        }

        /// <summary>
        /// It returns ContentType for given id
        /// </summary>
        /// <param name="contentTypeId"></param>
        /// <returns></returns>
        public ContentType GetContentType(Guid contentTypeId)
        {
            using var context = new DeviserDbContext(_dbOptions);
            var result = context.ContentType
                .Include(c => c.ContentTypeFields).ThenInclude(ctf => ctf.ContentFieldType)
                .Include(c => c.ContentTypeProperties).ThenInclude(cp => cp.Property).ThenInclude(p => p.OptionList)
                .FirstOrDefault(e => e.Id == contentTypeId);

            return _mapper.Map<ContentType>(result);
        }

        /// <summary>
        /// It returns a ContentType by arrribute Name
        /// </summary>
        /// <param name="contentTypeName"></param>
        /// <returns></returns>
        public ContentType GetContentType(string contentTypeName)
        {
            using var context = new DeviserDbContext(_dbOptions);
            var result = context.ContentType
                .Where(e => e.Name.ToLower() == contentTypeName.ToLower())
                .OrderBy(ct => ct.Name)
                .FirstOrDefault();

            return _mapper.Map<ContentType>(result);
        }

        /// <summary>
        /// It retuns all ContentTypes
        /// </summary>
        /// <returns></returns>
        public List<ContentType> GetContentTypes()
        {
            using var context = new DeviserDbContext(_dbOptions);
            var result = context.ContentType
                .Include(c => c.ContentTypeFields).ThenInclude(ctf => ctf.ContentFieldType)
                .Include(c => c.ContentTypeProperties).ThenInclude(cp => cp.Property).ThenInclude(p => p.OptionList)
                .OrderBy(c => c.Name)
                .ToList();
            return _mapper.Map<List<ContentType>>(result);
        }

        /// <summary>
        /// It updates ContentType and ContentTypeProperties.
        /// ContentTypeProperties are updated based on data from client i.e. sync (creates/update/delete)
        /// </summary>
        /// <param name="contentType"></param>
        /// <returns></returns>
        public ContentType UpdateContentType(ContentType contentType)
        {
            using var context = new DeviserDbContext(_dbOptions);
            var dbContentType = _mapper.Map<Entities.ContentType>(contentType);

            if (dbContentType.ContentTypeProperties != null && dbContentType.ContentTypeProperties.Count > 0)
            {

                var toRemoveFromClient = dbContentType.ContentTypeProperties.Where(clientProp => context.ContentTypeProperty.Any(dbProp =>
                    clientProp.ContentTypeId == dbProp.ContentTypeId && clientProp.PropertyId == dbProp.PropertyId)).ToList();

                var currentTypeProperties = context.ContentTypeProperty.Where(ctp => ctp.ContentTypeId == dbContentType.Id).ToList();

                List<Entities.ContentTypeProperty> toRemoveFromDb = null;

                if (currentTypeProperties.Count > 0)
                {
                    toRemoveFromDb = currentTypeProperties.Where(dbProp => !dbContentType.ContentTypeProperties.Any(clientProp => dbProp.PropertyId == clientProp.PropertyId)).ToList();
                }

                if (toRemoveFromClient.Count > 0)
                {
                    foreach (var contentTypeProp in toRemoveFromClient)
                    {
                        //ContentTypeProperty exist in db, therefore remove it from contentType (client source)
                        dbContentType.ContentTypeProperties.Remove(contentTypeProp);
                    }
                }

                if (toRemoveFromDb != null && toRemoveFromDb.Count > 0)
                {
                    //ContentTypeProperty is not exist in contentType (client source), because client has been removed it. Therefor, remove it from db.
                    context.ContentTypeProperty.RemoveRange(toRemoveFromDb);
                }

                if (dbContentType.ContentTypeProperties != null && dbContentType.ContentTypeProperties.Count > 0)
                {
                    foreach (var contentTypeProp in dbContentType.ContentTypeProperties)
                    {
                        contentTypeProp.Property = null;
                        context.ContentTypeProperty.Add(contentTypeProp);
                    }

                }
            }

            var dbContentTypeFields = context.ContentTypeField
                .AsNoTracking()
                .Where(ctf => ctf.ContentTypeId == dbContentType.Id)
                .ToDictionary(keySelector => keySelector.Id, valueSelector => valueSelector);
            var uiContentTypeFieldIds = dbContentType.ContentTypeFields.Select(ctf => ctf.Id).ToHashSet();
            var toDeleteContentTypeFields = dbContentTypeFields
                .Where(ctf => !uiContentTypeFieldIds.Contains(ctf.Key))
                .Select(ctf => ctf.Value)
                .ToList();
            context.ContentTypeField.RemoveRange(toDeleteContentTypeFields);

            dbContentType.LastModifiedDate = DateTime.Now;
            var result = context.ContentType.Update(dbContentType).Entity;
            //context.Entry(dbContentType).State = EntityState.Modified;
            context.SaveChanges();
            return _mapper.Map<ContentType>(result);
        }

        public async Task<IList<ContentTypeField>> SortContentTypeFields(IList<ContentTypeField> contentTypeFields)
        {
            await using var context = new DeviserDbContext(_dbOptions);
            var contentTypeId = contentTypeFields.First().ContentTypeId;
            var uiSorting = contentTypeFields.ToDictionary(keySelector => keySelector.Id,
                valueSelector => valueSelector.SortOrder);
            var dbContentTypeFields = await context.ContentTypeField
                .Where(ctf => ctf.ContentTypeId == contentTypeId)
                .ToListAsync();

            foreach (var contentTypeField in dbContentTypeFields)
            {
                contentTypeField.SortOrder = uiSorting[contentTypeField.Id];
            }

            await context.SaveChangesAsync();

            return _mapper.Map<IList<ContentTypeField>>(dbContentTypeFields);
        }
    }
}
