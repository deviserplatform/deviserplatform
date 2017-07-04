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
    public interface IContentControlProvider
    {
        List<ContentControl> GetContentControls();
        List<FieldType> GetFieldTypes();
        ContentControl GetContentControl(Guid controlId);
        ContentControl CreateContentControl(ContentControl contentControl);
        ContentControl GetContentControl(string controlName);
        ContentControl UpdateContentControl(ContentControl contentControl);

    }
    public class ContentControlProvider : DataProviderBase, IContentControlProvider
    {
        //Logger
        private readonly ILogger<ContentTypeProvider> _logger;

        //Constructor
        public ContentControlProvider(ILifetimeScope container)
            : base(container)
        {
            _logger = container.Resolve<ILogger<ContentTypeProvider>>();
        }

        public ContentControl CreateContentControl(ContentControl contentControl)
        {
            try
            {
                using (var context = new DeviserDbContext(DbOptions))
                {
                    var dbContentControl = Mapper.Map<Entities.ContentControl>(contentControl);
                    dbContentControl.Id = Guid.NewGuid();                                        
                    dbContentControl.CreatedDate = dbContentControl.LastModifiedDate = DateTime.Now;
                    var result = context.ContentControl.Add(dbContentControl).Entity;
                    context.SaveChanges();
                    return Mapper.Map<ContentControl>(result);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Error occured while creating ContentControl", ex);
            }
            return null;
        }

        public ContentControl GetContentControl(Guid controlId)
        {
            try
            {

                using (var context = new DeviserDbContext(DbOptions))
                {
                    var result = context.ContentControl
                               .FirstOrDefault(e => e.Id == controlId);

                    return Mapper.Map<ContentControl>(result);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Error occured while getting ContentControl by id", ex);
            }
            return null;
        }

        public ContentControl GetContentControl(string controlName)
        {
            try
            {

                using (var context = new DeviserDbContext(DbOptions))
                {
                    var result = context.ContentControl
                        .Where(e => String.Equals(e.Name, controlName, StringComparison.CurrentCultureIgnoreCase))
                               .FirstOrDefault();

                    return Mapper.Map<ContentControl>(result);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Error occured while getting ContentControl by name", ex);
            }
            return null;
        }

        public List<ContentControl> GetContentControls()
        {
            try
            {
                using (var context = new DeviserDbContext(DbOptions))
                {
                    var result = context.ContentControl
                        .OrderBy(cd => cd.Name)
                        .ToList();
                    return Mapper.Map<List<ContentControl>>(result);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Error occured while getting FieldTypes", ex);
            }
            return null;
        }

        public List<FieldType> GetFieldTypes()
        {
            try
            {
                using (var context = new DeviserDbContext(DbOptions))
                {
                    var result = context.FieldType
                        .OrderBy(cd => cd.Name)
                        .ToList();
                    return Mapper.Map<List<FieldType>>(result);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Error occured while getting FieldTypes", ex);
            }
            return null;
        }

        public ContentControl UpdateContentControl(ContentControl contentControl)
        {
            try
            {
                using (var context = new DeviserDbContext(DbOptions))
                {
                    var dbContentControl = Mapper.Map<Entities.ContentControl>(contentControl);
                    var result = context.ContentControl.Attach(dbContentControl).Entity;
                    context.Entry(dbContentControl).State = EntityState.Modified;
                    context.SaveChanges();
                    return Mapper.Map<ContentControl>(result);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Error occured while updating ContentControl", ex);
            }
            return null;
        }
    }
}
