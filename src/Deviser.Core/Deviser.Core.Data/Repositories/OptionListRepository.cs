using AutoMapper;
using Deviser.Core.Common.DomainTypes;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Deviser.Core.Data.Repositories
{
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
            using var context = new DeviserDbContext(_dbOptions);
            var dbOptionList = _mapper.Map<Entities.OptionList>(optionList);
            dbOptionList.CreatedDate = dbOptionList.LastModifiedDate = DateTime.Now;
            var result = context.OptionList.Add(dbOptionList).Entity;
            context.SaveChanges();
            return _mapper.Map<OptionList>(result);
        }

        public List<OptionList> GetOptionLists()
        {
            using var context = new DeviserDbContext(_dbOptions);
            var result = context.OptionList.ToList();
            return _mapper.Map<List<OptionList>>(result);
        }

        public OptionList GetOptionList(Guid optionListId)
        {
            using var context = new DeviserDbContext(_dbOptions);
            var result = context.OptionList
                .FirstOrDefault(e => e.Id == optionListId);
            return _mapper.Map<OptionList>(result);
        }

        public OptionList GetOptionList(string listName)
        {
            using var context = new DeviserDbContext(_dbOptions);
            var result = context.OptionList
                .FirstOrDefault(e => e.Name.ToLower() == listName.ToLower());
            return _mapper.Map<OptionList>(result);
        }


        public OptionList UpdateOptionList(OptionList optionList)
        {
            using var context = new DeviserDbContext(_dbOptions);
            var dbOptionList = _mapper.Map<Entities.OptionList>(optionList);
            dbOptionList.LastModifiedDate = DateTime.Now;
            var result = context.OptionList.Update(dbOptionList).Entity;
            context.SaveChanges();
            return _mapper.Map<OptionList>(result);
        }

        public bool IsPropertyExist(string optionListName)
        {
            using var context = new DeviserDbContext(_dbOptions);
            var result = context.OptionList.Count(e => e.Name == optionListName);
            return result > 0;
        }
    }
}
