using Autofac;
using Deviser.Core.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Deviser.Core.Data.DataProviders
{
    public interface IContentTypeProvider
    {
        List<ContentType> GetContentTypes();
        List<ContentDataType> GetContentDataTypes();
        ContentType GetContentType(Guid contentTypeId);
        ContentType CreateContentType(ContentType contentType);
        ContentType GetContentType(string contentTypeName);
        ContentType UpdateContentType(ContentType contentType);

    }

    /// <summary>
    /// Data provider for both ContentTypes and ContentDataType
    /// </summary>
    public class ContentTypeProvider : DataProviderBase, IContentTypeProvider
    {
        //Logger
        private readonly ILogger<ContentTypeProvider> logger;

        //Constructor
        public ContentTypeProvider(ILifetimeScope container)
            : base(container)
        {
            logger = container.Resolve<ILogger<ContentTypeProvider>>();
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
                using (var context = new DeviserDBContext(dbOptions))
                {
                    ContentType result;
                    contentType.Id = Guid.NewGuid();
                    contentType.ContentDataType = null;
                    if (contentType.ContentTypeProperties!=null && contentType.ContentTypeProperties.Count > 0)
                    {
                        foreach(var ctp in contentType.ContentTypeProperties)
                        {
                            ctp.ConentTypeId = contentType.Id;
                        }
                    }
                    contentType.CreatedDate = contentType.LastModifiedDate = DateTime.Now;
                    result = context.ContentType.Add(contentType).Entity;
                    context.SaveChanges();
                    return result;
                }
            }
            catch (Exception ex)
            {
                logger.LogError("Error occured while creating ContentType", ex);
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

                using (var context = new DeviserDBContext(dbOptions))
                {
                    var returnData = context.ContentType
                               .Where(e => e.Id == contentTypeId)
                               .FirstOrDefault();

                    return returnData;
                }
            }
            catch (Exception ex)
            {
                logger.LogError("Error occured while getting ContentType by id", ex);
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
                using (var context = new DeviserDBContext(dbOptions))
                {
                    var returnData = context.ContentDataType
                        .OrderBy(cd=>cd.Name)
                        .ToList();

                    return returnData;
                }
            }
            catch (Exception ex)
            {
                logger.LogError("Error occured while getting ContentDataTypes", ex);
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

                using (var context = new DeviserDBContext(dbOptions))
                {
                    var returnData = context.ContentType
                               .Where(e => e.Name.ToLower() == contentTypeName.ToLower())
                               .OrderBy(ct => ct.Name)
                               .FirstOrDefault();

                    return returnData;
                }
            }
            catch (Exception ex)
            {
                logger.LogError("Error occured while getting ContentType by id", ex);
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
                using (var context = new DeviserDBContext(dbOptions))
                {
                    var returnData = context.ContentType
                        .Include(c => c.ContentTypeProperties).ThenInclude(cp => cp.Property).ThenInclude(p => p.PropertyOptionList)
                        .Include(c => c.ContentDataType)
                        .OrderBy(c=>c.Name)
                        .ToList();
                    return new List<ContentType>(returnData);
                }
            }
            catch (Exception ex)
            {
                logger.LogError("Error occured while getting all ContentTypes", ex);
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
                using (var context = new DeviserDBContext(dbOptions))
                {
                    if (contentType.ContentTypeProperties != null && contentType.ContentTypeProperties.Count > 0)
                    {

                        var toRemoveFromClient = contentType.ContentTypeProperties.Where(clientProp => context.ContentTypeProperty.Any(dbProp =>
                         clientProp.ConentTypeId == dbProp.ConentTypeId && clientProp.PropertyId == dbProp.PropertyId)).ToList();

                        var currentTypeProperties = context.ContentTypeProperty.Where(ctp => ctp.ConentTypeId == contentType.Id).ToList();

                        List<ContentTypeProperty> toRemoveFromDb = null;

                        if (currentTypeProperties != null && currentTypeProperties.Count > 0)
                        {
                            toRemoveFromDb = currentTypeProperties.Where(dbProp => !contentType.ContentTypeProperties.Any(clientProp => dbProp.PropertyId == clientProp.PropertyId)).ToList();
                        }

                        if (toRemoveFromClient != null && toRemoveFromClient.Count > 0)
                        {
                            foreach (var contentTypeProp in toRemoveFromClient)
                            {
                                //ContentTypeProperty exist in db, therefore remove it from contentType (client source)
                                contentType.ContentTypeProperties.Remove(contentTypeProp);
                            }
                        }

                        if (toRemoveFromDb != null && toRemoveFromDb.Count > 0)
                        {
                            //ContentTypeProperty is not exist in contentType (client source), because client has been removed it. Therefor, remove it from db.
                            context.ContentTypeProperty.RemoveRange(toRemoveFromDb);
                        }
                    }

                    ContentType result;
                    contentType.LastModifiedDate = DateTime.Now;
                    result = context.ContentType.Attach(contentType).Entity;
                    context.Entry(contentType).State = EntityState.Modified;
                    context.SaveChanges();
                    return result;
                }
            }
            catch (Exception ex)
            {
                logger.LogError("Error occured while updating ContentType", ex);
            }
            return null;
        }
    }
}
