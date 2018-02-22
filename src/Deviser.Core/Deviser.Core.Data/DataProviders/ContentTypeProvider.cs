using Autofac;
using Deviser.Core.Common.DomainTypes;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;

namespace Deviser.Core.Data.DataProviders
{
    public interface IContentTypeProvider
    {
        List<ContentType> GetContentTypes();
        List<ContentDataType> GetContentDataTypes();
        ContentType GetContentType(Guid contentTypeId);
        ContentType CreateContentType(ContentType dbContentType);
        ContentType GetContentType(string contentTypeName);
        ContentType UpdateContentType(ContentType dbContentType);

    }

    /// <summary>
    /// Data provider for both ContentTypes and ContentDataType
    /// </summary>
    public class ContentTypeProvider : DataProviderBase, IContentTypeProvider
    {
        //Logger
        private readonly ILogger<ContentTypeProvider> _logger;

        //Constructor
        public ContentTypeProvider(ILifetimeScope container)
            : base(container)
        {
            _logger = container.Resolve<ILogger<ContentTypeProvider>>();
            
        }

        /// <summary>
        /// It creates new ContentType
        /// </summary>
        /// <param name="contentType"></param>
        /// <returns></returns>
        public ContentType CreateContentType(ContentType contentType)
        {
            try
            {
                //using (var context = new DeviserDbContext(DbOptions))
                //{
                    var dbContentType = Mapper.Map<Entities.ContentType>(contentType);
                    dbContentType.Id = Guid.NewGuid();                    
                    if (dbContentType.ContentTypeProperties!=null && dbContentType.ContentTypeProperties.Count > 0)
                    {
                        foreach(var ctp in dbContentType.ContentTypeProperties)
                        {
                            ctp.ContentTypeId = dbContentType.Id;
                        }
                    }
                    dbContentType.CreatedDate = dbContentType.LastModifiedDate = DateTime.Now;
                    var result = context.ContentType.Add(dbContentType).Entity;
                    context.SaveChanges();
                    return Mapper.Map<ContentType>(result);
                //}
            }
            catch (Exception ex)
            {
                _logger.LogError("Error occured while creating ContentType", ex);
            }
            return null;
        }

        /// <summary>
        /// It returns ContentType for given id
        /// </summary>
        /// <param name="contentTypeId"></param>
        /// <returns></returns>
        public ContentType GetContentType(Guid contentTypeId)
        {
            try
            {
                //using (var context = new DeviserDbContext(DbOptions))
                //{
                    var result = context.ContentType
                               .FirstOrDefault(e => e.Id == contentTypeId);

                    return Mapper.Map<ContentType>(result);
                //}
            }
            catch (Exception ex)
            {
                _logger.LogError("Error occured while getting ContentType by id", ex);
            }
            return null;
        }

        /// <summary>
        /// It returns all ContentDataTypes
        /// </summary>
        /// <returns></returns>
        public List<ContentDataType> GetContentDataTypes()
        {
            try
            {
                //using (var context = new DeviserDbContext(DbOptions))
                //{
                    var result = context.ContentDataType
                        .OrderBy(cd=>cd.Name)
                        .ToList();
                    return Mapper.Map<List<ContentDataType>>(result);
                //}
            }
            catch (Exception ex)
            {
                _logger.LogError("Error occured while getting ContentDataTypes", ex);
            }
            return null;
        }

        /// <summary>
        /// It returns a ContentType by arrribute Name
        /// </summary>
        /// <param name="contentTypeName"></param>
        /// <returns></returns>
        public ContentType GetContentType(string contentTypeName)
        {
            try
            {
                //using (var context = new DeviserDbContext(DbOptions))
                //{
                    var result = context.ContentType
                               .Where(e => String.Equals(e.Name, contentTypeName, StringComparison.CurrentCultureIgnoreCase))
                               .OrderBy(ct => ct.Name)
                               .FirstOrDefault();

                    return Mapper.Map<ContentType>(result);
                //}
            }
            catch (Exception ex)
            {
                _logger.LogError("Error occured while getting ContentType by id", ex);
            }
            return null;
        }

        /// <summary>
        /// It retuns all ContentTypes
        /// </summary>
        /// <returns></returns>
        public List<ContentType> GetContentTypes()
        {
            try
            {
                //using (var context = new DeviserDbContext(DbOptions))
                //{
                    var result = context.ContentType
                        .Include(c => c.ContentTypeProperties).ThenInclude(cp => cp.Property).ThenInclude(p => p.OptionList)
                        .Include(c => c.ContentDataType)
                        .OrderBy(c=>c.Name)
                        .ToList();   
                    return Mapper.Map<List<ContentType>>(result);
                //}
            }
            catch (Exception ex)
            {
                _logger.LogError("Error occured while getting all ContentTypes", ex);
            }
            return null;
        }

        /// <summary>
        /// It updates ContentType and ContentTypeProperties.
        /// ContentTypeProperties are updated based on data from client i.e. sync (creates/update/delete)
        /// </summary>
        /// <param name="contentType"></param>
        /// <returns></returns>
        public ContentType UpdateContentType(ContentType contentType)
        {
            try
            {
                //using (var context = new DeviserDbContext(DbOptions))
                //{
                    var dbContentType = Mapper.Map<Entities.ContentType>(contentType);

                    if (dbContentType.ContentTypeProperties != null && dbContentType.ContentTypeProperties.Count > 0)
                    {

                        var toRemoveFromClient = dbContentType.ContentTypeProperties.Where(clientProp => context.ContentTypeProperty.Any(dbProp =>
                         clientProp.ContentTypeId == dbProp.ContentTypeId && clientProp.PropertyId == dbProp.PropertyId)).ToList();

                        var currentTypeProperties = context.ContentTypeProperty.Where(ctp => ctp.ContentTypeId == dbContentType.Id).ToList();

                        List<Entities.ContentTypeProperty> toRemoveFromDb = null;

                        if (currentTypeProperties != null && currentTypeProperties.Count > 0)
                        {
                            toRemoveFromDb = currentTypeProperties.Where(dbProp => !dbContentType.ContentTypeProperties.Any(clientProp => dbProp.PropertyId == clientProp.PropertyId)).ToList();
                        }

                        if (toRemoveFromClient != null && toRemoveFromClient.Count > 0)
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
                    }

                    
                    dbContentType.LastModifiedDate = DateTime.Now;
                    var result = context.ContentType.Attach(dbContentType).Entity;
                    context.Entry(dbContentType).State = EntityState.Modified;
                    context.SaveChanges();
                    return Mapper.Map<ContentType>(result);
                //}
            }
            catch (Exception ex)
            {
                _logger.LogError("Error occured while updating ContentType", ex);
            }
            return null;
        }
    }
}
