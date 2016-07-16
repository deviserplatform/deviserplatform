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
    public interface IOptionListProvider
    {
        PropertyOptionList CreateOptionList(PropertyOptionList optionList);
        List<PropertyOptionList> GetOptionLists();
        PropertyOptionList GetOptionList(Guid optionListId);
        PropertyOptionList GetOptionList(string listName);
        PropertyOptionList UpdateOptionList(PropertyOptionList contentType);
    }

    public class OptionListProvider : DataProviderBase, IOptionListProvider
    {
        //Logger
        private readonly ILogger<OptionListProvider> logger;

        //Constructor
        public OptionListProvider(ILifetimeScope container)
            :base(container)
        {
            logger = container.Resolve<ILogger<OptionListProvider>>();
        }

        public PropertyOptionList CreateOptionList(PropertyOptionList optionList)
        {
            try
            {
                using (var context = new DeviserDBContext(dbOptions))
                {
                    PropertyOptionList result;
                    optionList.CreatedDate = optionList.LastModifiedDate = DateTime.Now;
                    result = context.PropertyOptionList.Add(optionList).Entity;
                    context.SaveChanges();
                    return result;
                }
            }
            catch (Exception ex)
            {
                logger.LogError("Error occured while creating PropertyOptionList", ex);
            }
            return null;
        }

        public PropertyOptionList GetOptionList(Guid optionListId)
        {
            try
            {

                using (var context = new DeviserDBContext(dbOptions))
                {
                    var returnData = context.PropertyOptionList.Where(e => e.Id == optionListId)
                               .FirstOrDefault();
                    return returnData;
                }
            }
            catch (Exception ex)
            {
                logger.LogError("Error occured while getting PropertyOptionList by id", ex);
            }
            return null;
        }

        public PropertyOptionList GetOptionList(string listName)
        {
            try
            {

                using (var context = new DeviserDBContext(dbOptions))
                {
                    var returnData = context.PropertyOptionList.Where(e => e.Name.ToLower() == listName.ToLower())
                               .FirstOrDefault();
                    return returnData;
                }
            }
            catch (Exception ex)
            {
                logger.LogError("Error occured while getting PropertyOptionList by id", ex);
            }
            return null;
        }

        public List<PropertyOptionList> GetOptionLists()
        {
            try
            {
                using (var context = new DeviserDBContext(dbOptions))
                {
                    var returnData = context.PropertyOptionList.ToList();
                    return returnData;
                }
            }
            catch (Exception ex)
            {
                logger.LogError("Error occured while getting PropertyOptionList", ex);
            }
            return null;
        }

        public PropertyOptionList UpdateOptionList(PropertyOptionList contentType)
        {
            try
            {
                using (var context = new DeviserDBContext(dbOptions))
                {
                    PropertyOptionList result;
                    contentType.LastModifiedDate = DateTime.Now;
                    result = context.PropertyOptionList.Attach(contentType).Entity;
                    context.Entry(contentType).State = EntityState.Modified;
                    context.SaveChanges();
                    return result;
                }
            }
            catch (Exception ex)
            {
                logger.LogError("Error occured while updating PropertyOptionList", ex);
            }
            return null;
        }
    }
}
