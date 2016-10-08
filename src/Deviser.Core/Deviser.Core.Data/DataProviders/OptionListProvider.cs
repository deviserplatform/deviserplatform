using Autofac;
using Deviser.Core.Common.DomainTypes;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;

namespace Deviser.Core.Data.DataProviders
{
    public interface IOptionListProvider
    {
        PropertyOptionList CreateOptionList(PropertyOptionList dbPropertyOptionList);
        List<PropertyOptionList> GetOptionLists();
        PropertyOptionList GetOptionList(Guid optionListId);
        PropertyOptionList GetOptionList(string listName);
        PropertyOptionList UpdateOptionList(PropertyOptionList dbContentType);
    }

    public class OptionListProvider : DataProviderBase, IOptionListProvider
    {
        //Logger
        private readonly ILogger<OptionListProvider> _logger;

        //Constructor
        public OptionListProvider(ILifetimeScope container)
            :base(container)
        {
            _logger = container.Resolve<ILogger<OptionListProvider>>();
        }

        public PropertyOptionList CreateOptionList(PropertyOptionList propertyOptionList)
        {
            try
            {
                using (var context = new DeviserDbContext(DbOptions))
                {
                    var dbPropertyOptionList = Mapper.Map<Entities.PropertyOptionList>(propertyOptionList);
                    dbPropertyOptionList.CreatedDate = dbPropertyOptionList.LastModifiedDate = DateTime.Now;
                    var result = context.PropertyOptionList.Add(dbPropertyOptionList).Entity;
                    context.SaveChanges();
                    return Mapper.Map<PropertyOptionList>(result);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Error occured while creating PropertyOptionList", ex);
            }
            return null;
        }

        public List<PropertyOptionList> GetOptionLists()
        {
            try
            {
                using (var context = new DeviserDbContext(DbOptions))
                {
                    var result = context.PropertyOptionList.ToList();
                    return Mapper.Map<List<PropertyOptionList>>(result);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Error occured while getting PropertyOptionList", ex);
            }
            return null;
        }

        public PropertyOptionList GetOptionList(Guid optionListId)
        {
            try
            {

                using (var context = new DeviserDbContext(DbOptions))
                {
                    var result = context.PropertyOptionList
                               .FirstOrDefault(e => e.Id == optionListId);
                    return Mapper.Map<PropertyOptionList>(result);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Error occured while getting PropertyOptionList by id", ex);
            }
            return null;
        }

        public PropertyOptionList GetOptionList(string listName)
        {
            try
            {

                using (var context = new DeviserDbContext(DbOptions))
                {
                    var result = context.PropertyOptionList
                               .FirstOrDefault(e => e.Name.ToLower() == listName.ToLower());
                    return Mapper.Map<PropertyOptionList>(result);
                }
            }


            catch (Exception ex)
            {
                _logger.LogError("Error occured while getting PropertyOptionList by id", ex);
            }
            return null;
        }
        

        public PropertyOptionList UpdateOptionList(PropertyOptionList propertyOptionList)
        {
            try
            {
                using (var context = new DeviserDbContext(DbOptions))
                {
                    var dbPropertyOptionList = Mapper.Map<Entities.PropertyOptionList>(propertyOptionList);
                    dbPropertyOptionList.LastModifiedDate = DateTime.Now;
                    var result = context.PropertyOptionList.Attach(dbPropertyOptionList).Entity;
                    context.Entry(dbPropertyOptionList).State = EntityState.Modified;
                    context.SaveChanges();
                    return Mapper.Map<PropertyOptionList>(result);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Error occured while updating PropertyOptionList", ex);
            }
            return null;
        }
    }
}
