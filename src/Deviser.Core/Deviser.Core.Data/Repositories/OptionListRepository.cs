using AutoMapper;
using Deviser.Core.Common.DomainTypes;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Deviser.Core.Data.Repositories
{
    public interface IOptionListRepository
    {
        OptionList CreateOptionList(OptionList dbOptionList);
        List<OptionList> GetOptionLists();
        OptionList GetOptionList(Guid optionListId);
        OptionList GetOptionList(string listName);
        OptionList UpdateOptionList(OptionList dbContentType);
        bool IsPropertyExist(string propertyName);
    }

    public class OptionListRepository : IOptionListRepository
    {
        //Logger
        private readonly ILogger<OptionListRepository> _logger;
        private readonly DbContextOptions<DeviserDbContext> _dbOptions;
        private readonly IMapper _mapper;

        //Constructor
        public OptionListRepository(DbContextOptions<DeviserDbContext> dbOptions,
            ILogger<OptionListRepository> logger,
            IMapper mapper)
        {
            _logger = logger;
            _dbOptions = dbOptions;
            _mapper = mapper;
        }

        public OptionList CreateOptionList(OptionList optionList)
        {
            try
            {
                using var context = new DeviserDbContext(_dbOptions);
                var dbOptionList = _mapper.Map<Entities.OptionList>(optionList);
                dbOptionList.CreatedDate = dbOptionList.LastModifiedDate = DateTime.Now;
                var result = context.OptionList.Add(dbOptionList).Entity;
                context.SaveChanges();
                return _mapper.Map<OptionList>(result);
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
                using var context = new DeviserDbContext(_dbOptions);
                var result = context.OptionList.ToList();
                return _mapper.Map<List<OptionList>>(result);
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
                using var context = new DeviserDbContext(_dbOptions);
                var result = context.OptionList
                    .FirstOrDefault(e => e.Id == optionListId);
                return _mapper.Map<OptionList>(result);
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
                using var context = new DeviserDbContext(_dbOptions);
                var result = context.OptionList
                    .FirstOrDefault(e => e.Name.ToLower() == listName.ToLower());
                return _mapper.Map<OptionList>(result);
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
                using var context = new DeviserDbContext(_dbOptions);
                var dbOptionList = _mapper.Map<Entities.OptionList>(optionList);
                dbOptionList.LastModifiedDate = DateTime.Now;
                var result = context.OptionList.Update(dbOptionList).Entity;
                context.SaveChanges();
                return _mapper.Map<OptionList>(result);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error occured while updating OptionList", ex);
            }
            return null;
        }

        public bool IsPropertyExist(string optionListName)
        {
            try
            {
                using var context = new DeviserDbContext(_dbOptions);
                var result = context.OptionList.Count(e => e.Name == optionListName);
                return result > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error occured while checking whether OptionList by optionListName", ex);
            }
            return false;
        }
    }
}
