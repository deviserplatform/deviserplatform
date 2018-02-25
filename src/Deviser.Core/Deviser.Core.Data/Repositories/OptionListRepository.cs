using Autofac;
using Deviser.Core.Common.DomainTypes;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;

namespace Deviser.Core.Data.Repositories
{
    public interface IOptionListRepository
    {
        OptionList CreateOptionList(OptionList dbOptionList);
        List<OptionList> GetOptionLists();
        OptionList GetOptionList(Guid optionListId);
        OptionList GetOptionList(string listName);
        OptionList UpdateOptionList(OptionList dbContentType);
    }

    public class OptionListRepository : RepositoryBase, IOptionListRepository
    {
        //Logger
        private readonly ILogger<OptionListRepository> _logger;

        //Constructor
        public OptionListRepository(ILifetimeScope container)
            :base(container)
        {
            _logger = container.Resolve<ILogger<OptionListRepository>>();
        }

        public OptionList CreateOptionList(OptionList optionList)
        {
            try
            {
                //using (var context = new DeviserDbContext(DbOptions))
                //{
                    var dbOptionList = Mapper.Map<Entities.OptionList>(optionList);
                    dbOptionList.CreatedDate = dbOptionList.LastModifiedDate = DateTime.Now;
                    var result = context.OptionList.Add(dbOptionList).Entity;
                    context.SaveChanges();
                    return Mapper.Map<OptionList>(result);
                //}
            }
            catch (Exception ex)
            {
                _logger.LogError("Error occured while creating OptionList", ex);
            }
            return null;
        }

        public List<OptionList> GetOptionLists()
        {
            try
            {
                //using (var context = new DeviserDbContext(DbOptions))
                //{
                    var result = context.OptionList.ToList();
                    return Mapper.Map<List<OptionList>>(result);
                //}
            }
            catch (Exception ex)
            {
                _logger.LogError("Error occured while getting OptionList", ex);
            }
            return null;
        }

        public OptionList GetOptionList(Guid optionListId)
        {
            try
            {
                //using (var context = new DeviserDbContext(DbOptions))
                //{
                    var result = context.OptionList
                               .FirstOrDefault(e => e.Id == optionListId);
                    return Mapper.Map<OptionList>(result);
                //}
            }
            catch (Exception ex)
            {
                _logger.LogError("Error occured while getting OptionList by id", ex);
            }
            return null;
        }

        public OptionList GetOptionList(string listName)
        {
            try
            {
                //using (var context = new DeviserDbContext(DbOptions))
                //{
                    var result = context.OptionList
                               .FirstOrDefault(e => e.Name.ToLower() == listName.ToLower());
                    return Mapper.Map<OptionList>(result);
                //}
            }


            catch (Exception ex)
            {
                _logger.LogError("Error occured while getting OptionList by id", ex);
            }
            return null;
        }
        

        public OptionList UpdateOptionList(OptionList optionList)
        {
            try
            {
                //using (var context = new DeviserDbContext(DbOptions))
                //{
                    var dboptionList = Mapper.Map<Entities.OptionList>(optionList);
                    dboptionList.LastModifiedDate = DateTime.Now;
                    var result = context.OptionList.Attach(dboptionList).Entity;
                    context.Entry(dboptionList).State = EntityState.Modified;
                    context.SaveChanges();
                    return Mapper.Map<OptionList>(result);
                //}
            }
            catch (Exception ex)
            {
                _logger.LogError("Error occured while updating OptionList", ex);
            }
            return null;
        }
    }
}
