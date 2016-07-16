﻿using Autofac;
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
        ContentType GetContentType(Guid contentTypeId);
        ContentType CreateContentType(ContentType contentType);
        ContentType UpdateContentType(ContentType contentType);

    }

    public class ContentTypeProvider : DataProviderBase, IContentTypeProvider
    {
        //Logger
        private readonly ILogger<ContentTypeProvider> logger;

        //Constructor
        public ContentTypeProvider(ILifetimeScope container)
            :base(container)
        {
            logger = container.Resolve<ILogger<ContentTypeProvider>>();
        }

        public ContentType CreateContentType(ContentType contentType)
        {
            try
            {
                using (var context = new DeviserDBContext(dbOptions))
                {
                    ContentType result;
                    result = context.ContentType.Add(contentType).Entity;
                    context.SaveChanges();
                    return result;
                }
            }
            catch (Exception ex)
            {
                logger.LogError("Error occured while creating LayoutType", ex);
            }
            return null;
        }

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

        public List<ContentType> GetContentTypes()
        {
            try
            {
                using (var context = new DeviserDBContext(dbOptions))
                {
                    var returnData = context.ContentType
                        .Include(c=>c.ContentTypeProperties).ThenInclude(cp=>cp.Property)
                        .Include(c=>c.ContentDataType)
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

        public ContentType UpdateContentType(ContentType contentType)
        {
            try
            {
                using (var context = new DeviserDBContext(dbOptions))
                {
                    ContentType result;
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